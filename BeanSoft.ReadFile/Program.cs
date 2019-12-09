using Models.Common;
using Models.Entities;
using Models.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using DataAccess.Controllers;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Transactions;

namespace BeanSoft.ReadFile
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            App.Configs.ConnectionString = configuration.GetConnectionString("Connection");
            App.Configs.DirectoryDataPath = configuration.GetConnectionString("StorageData");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Welcome to ReadFile Tool");

            //var filename = @"E:\Working\Core\Services\BeanSoft.ReadFile\Data\Billing\102119_VAS_ISS_INC_VRB970421.DAT";
            //var test = ReadFileBilling.ReadFile(filename);

           //  Get All File in Directory
            var listFile = CommonUtils.GetListFile(App.Configs.DirectoryDataPath);

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 4
            };
            
            foreach(var listATM in listFile)
            {
                if (listATM.Extension == Constants.EXTENSION_JRN)
                {
                    Console.WriteLine("Start read ATM :" + listATM.DirName);
                    var files = listATM.FileName;
                    foreach(var file in files)
                    {
                        ExecuteJRN(file, listATM.DirName);
                    }
                    // Chua can dung parallel vi it file qua
                    //Parallel.ForEach(files, options, file =>
                    //{
                    //    if (CheckBeforeIns(file,listATM.Extension))
                    //    {
                    //        Console.WriteLine("Start read file :" + file);
                    //        ExecuteJRN(file,listATM.DirName);
                    //        Console.WriteLine("End read file :" + file);
                    //    }
                        
                    //});
                } else if (listATM.Extension == Constants.EXTENSION_Excel)
                {
                    Console.WriteLine("Start read Excels :" + listATM.DirName);
                    var files = listATM.FileName;
                    foreach(var file in files)
                    {
                        ExecuteReadFileExcel(file);
                    }
                }
                else
                {
                    Console.WriteLine("Start read ATM :" + listATM.DirName);
                    var files = listATM.FileName;
                    foreach (var file in files)
                    {
                        ExecuteReadFileJDATA(file,listATM.DirName);
                    }
                }
            }

            Console.WriteLine("End Read File Successful.");
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }
        private static void ExecuteJRN (string filename,string machineCode)
        {
            try
            {
                Console.WriteLine("Start read file :" + filename);
                using (TransactionScope scope = new TransactionScope())
                {
                    string DataLogID = string.Empty;
                    var listTransInfo = ReadFileATM.ReadFileJRN(filename);

                    DataLogController.FinalReadFile(Path.GetFileNameWithoutExtension(filename).Substring(Path.GetFileNameWithoutExtension(filename).Length - 8, 8), Path.GetFileName(filename), out DataLogID);
                    if (!string.IsNullOrEmpty(DataLogID))
                    {
                        ReadFileATMController.ReadFileJRN(listTransInfo, machineCode,DataLogID);
                        scope.Complete();
                    }
                }

                Console.WriteLine("End read file :" + filename);
                BackUpFile(filename, machineCode, Constants.READFILE_SUCCESS);
            }
            catch (TransactionAbortedException ex)
            {
                BackUpFile(filename, machineCode, Constants.READFILE_FAIL);
                ErrorUtils.WriteLog("ReadFileJDATA : TransactionAbortedException File: " + filename + " MsgErr:" + ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                BackUpFile(filename, machineCode, Constants.READFILE_FAIL);
                Console.WriteLine(filename + "-" + ex.Message);
                ErrorUtils.WriteLog( filename + "-" + ex.Message);
            }
              
        }
        private static void ExecuteReadFileJDATA(string filename,string machineCode)
        {
            try
            {
                Console.WriteLine("Start read file :" + filename);
                var listTransInfo = ReadFileATM.ReadFileEJDATA(filename);
                if (listTransInfo.Count > 0)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {

                        string trandate = listTransInfo[0].CARD_INSERTED.Substring(0, 8);
                        string DataLogID = string.Empty;
                        DataLogController.FinalReadFile(DateTime.Parse(trandate).ToString("ddMMyyyy"), Path.GetFileName(filename), out DataLogID);
                        if (!string.IsNullOrEmpty(DataLogID))
                        {
                            ReadFileATMController.ReadFileEJDATA(listTransInfo, machineCode, DataLogID);
                            scope.Complete();
                        }
                    }
                }
                    
                
                Console.WriteLine("End read file :" + filename);
                BackUpFile(filename, machineCode, Constants.READFILE_SUCCESS);

            }
            catch (TransactionAbortedException ex)
            {
                BackUpFile(filename, machineCode, Constants.READFILE_FAIL);
                ErrorUtils.WriteLog("ReadFileJDATA : TransactionAbortedException File: " + filename + " MsgErr:" + ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                BackUpFile(filename, machineCode, Constants.READFILE_FAIL);
                Console.WriteLine(filename + "-" + ex.Message);
                ErrorUtils.WriteLog(filename + "-" + ex.Message);
            }
        }

        private static void ExecuteReadFileExcel(string filename)
        {
            try
            {
                Console.WriteLine("Start read file :" + filename);
                using (TransactionScope scope = new TransactionScope())
                {
                    var listTransInfo = ReadFileExcel.ReadExcel (filename);
                    string DataLogID = string.Empty;
                    // Ten file phai dinh dang DDMMYYYY
                    DataLogController.FinalReadFile(Path.GetFileNameWithoutExtension(filename).Substring(Path.GetFileNameWithoutExtension(filename).Length - 8, 8), Path.GetFileName(filename), out DataLogID);
                    if(!string.IsNullOrEmpty(DataLogID))
                    {
                        ReadFileExcelController.ReadFileExcel(listTransInfo, DataLogID);
                        BackUpFile(filename, "Excels", Constants.READFILE_SUCCESS);
                        scope.Complete();
                        Console.WriteLine("End read file :" + filename);
                    }
                }
            }
            catch (TransactionAbortedException ex)
            {
                BackUpFile(filename, "Excels", Constants.READFILE_FAIL);
                ErrorUtils.WriteLog("ReadFileExcel : TransactionAbortedException File: " + filename + " MsgErr:" + ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                BackUpFile(filename, "Excels", Constants.READFILE_FAIL);
                Console.WriteLine(filename + "-" + ex.Message);
                ErrorUtils.WriteLog(filename + "-" + ex.Message);
            }
        }

        private static void ExecuteReadFileBilling(string filename)
        {
            try
            {
                var listBillingInfo = ReadFileBilling.ReadFile(filename);
                ReadFileBillingController.ReadFileBilling(listBillingInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(filename + "-" + ex.Message);
                ErrorUtils.WriteLog(filename + "-" + ex.Message);
            }
        }

        private static Boolean CheckBeforeIns(string fileName,string extension)
        {
            Boolean bCheck = true;
           
            //if (extension == Constants.EXTENSION_JRN)
            //{
            //    var txDate = Path.GetFileNameWithoutExtension(fileName);
            //    TxCheckController.CheckFileJRN(txDate,out bCheck);
            //}
            //if (extension == Constants.EXTENSION_Excel)
            //{
            //    var txDate = Path.GetFileNameWithoutExtension(fileName);
            //    TxCheckController.CheckFileExcel(fileName, out bCheck);
            //}
            //else
            //{
            //   TxCheckController.CheckFileEJDATA(fileName, out bCheck);
            //}

            return bCheck;
        }
        private static void BackUpFile(string fileName,string Dir,string typeFile)
        {
            string fileNameresult = Path.GetFileName(fileName);
            string sourcePath = App.Configs.DirectoryDataPath +  Dir;
            string targetPath = string.Empty;
            if (typeFile == Constants.READFILE_SUCCESS)
            {
                targetPath = App.Configs.DirectoryDataPath.ToString() + @"Backup\" + Dir;

            }
            else
            {
                targetPath = App.Configs.DirectoryDataPath.ToString() + @"Error\" + Dir;

            }

            // Use Path class to manipulate file and directory paths.
            string sourceFile = System.IO.Path.Combine(sourcePath, fileNameresult);
            string destFile = System.IO.Path.Combine(targetPath, fileNameresult);

            // To copy a folder's contents to a new location:
            // Create a new target folder. 
            // If the directory already exists, this method does not create a new directory.
            System.IO.Directory.CreateDirectory(targetPath);

            // To copy a file to another location and 
            // overwrite the destination file if it already exists.
            System.IO.File.Copy(sourceFile, destFile, true);

            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                    Console.WriteLine("Backup Sucessful file " + fileName);
                    try
                    {
                        File.Delete(sourceFile);
                    }
                    catch(Exception ex)
                    {
                        ErrorUtils.WriteLog("Error Delete File " + sourcePath + "-" + ex.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Source path does not exist!");
            }
        }
    }
}
