using Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Models.Common
{
    public class CommonUtils
    {
        public static string ProcessString(string txtInput)
        {
            string result = string.Empty;

            result = txtInput.Replace(Constants.Filter1, string.Empty);
            if (result.Contains(Constants.AMOUNT)) result = result.Replace(Constants.AMOUNT,string.Empty);
            if (result.Contains(Constants.CARD_INSERTED)) result =result.Replace(Constants.CARD_INSERTED, string.Empty);
            if (result.Contains(Constants.CARD_NUMBER)) result = result.Replace(Constants.CARD_NUMBER, string.Empty);
            if (result.Contains(Constants.CARD_TAKEN)) result = result.Replace(Constants.CARD_TAKEN, string.Empty);
            if (result.Contains(Constants.CUSTOMER_NAME)) result = result.Replace(Constants.CUSTOMER_NAME, string.Empty);
            if (result.Contains(Constants.PIN_ENTERED)) result = result.Replace(Constants.PIN_ENTERED, string.Empty);
            if (result.Contains(Constants.TRANSACTION_TYPE)) result = result.Replace(Constants.TRANSACTION_TYPE, string.Empty);
            if (result.Contains(Constants.TRANSACTION_END)) result = result.Replace(Constants.TRANSACTION_END, string.Empty);

            if (result.Contains(Constants.NOTES_PRESENTED)) result = result.Replace(Constants.NOTES_PRESENTED, string.Empty);
            if (result.Contains(Constants.NOTES_TAKEN)) result = result.Replace(Constants.NOTES_TAKEN, string.Empty);

            if (result.Contains(Constants.CASH) && !result.Contains(Constants.CASH_PRESENTED) && !result.Contains(Constants.CASH_TAKEN) && !result.Contains(Constants.CASH_REQUEST)) result = result.Replace(Constants.CASH, string.Empty);
            if (result.Contains(Constants.CASH_PRESENTED)) result = result.Replace(Constants.CASH_PRESENTED, string.Empty);
            if (result.Contains(Constants.CASH_REQUEST)) result = result.Replace(Constants.CASH_REQUEST, string.Empty);
            if (result.Contains(Constants.CASH_TAKEN)) result = result.Replace(Constants.CASH_TAKEN, string.Empty);
            if (result.Contains(Constants.TRANSACTION_START_JRN )) result = result.Replace(Constants.TRANSACTION_START_JRN, string.Empty);
            if (result.Contains(Constants.TRANSACTION_STOP_JRN)) result = result.Replace(Constants.TRANSACTION_STOP_JRN, string.Empty);

            return result.Trim();
        }
        public static List<FilesInfo> GetListFile(string dataPath)
        {
            var filesInfo = new List<FilesInfo>();
            string extension = string.Empty;
            DirectoryInfo directoryInfo = new DirectoryInfo(dataPath);
            // Get Sub Folder
            var subFolders = directoryInfo.GetDirectories();
            foreach(var folder in subFolders )
            {
                var fileInfo = new FilesInfo();
                var folderinfo = new DirectoryInfo(folder.FullName);
                {
                    // Get All File
                    var listFile = new List<string>();

                    foreach (FileInfo fi in folderinfo.GetFiles())
                    {
                        //listFile.Add(fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length));
                        listFile.Add(fi.FullName);
                        extension = fi.Extension.Substring(1, fi.Extension.Length-1);
                    }
                    fileInfo.DirName = folder.Name;
                    fileInfo.FileName = listFile;
                    fileInfo.Extension = extension.ToUpper();
                }
                filesInfo.Add(fileInfo);
            }
            return filesInfo;
        }
    }
}
