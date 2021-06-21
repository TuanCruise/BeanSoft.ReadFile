using DataAccess.Controllers;
using Microsoft.Extensions.Configuration;
using Models.Common;
using Models.Controllers;
using System;
using System.Data;
using System.IO;
using System.Text;
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

            //TestOutNapasBBB();
            //TestOutNapas();
            //var filename = @"E:\Working\Core\Services\BeanSoft.ReadFile\Data\Billing\102119_VAS_ISS_INC_VRB970421.DAT";
            //var test = ReadFileBilling.ReadFile(filename);

            //  Get All File in Directory
            var listFile = CommonUtils.GetListFile(App.Configs.DirectoryDataPath);

            //var options = new ParallelOptions()
            //{
            //    MaxDegreeOfParallelism = 4
            //};

            foreach (var listATM in listFile)
            {
                if (listATM.FileName.Count > 0)
                {
                    if (listATM.Extension == Constants.EXTENSION_JRN)
                    {
                        Console.WriteLine("Start read ATM :" + listATM.DirName);
                        var files = listATM.FileName;  
                        foreach (var file in files)
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
                    }
                    else if (listATM.Extension == Constants.EXTENSION_LOG)
                    {
                        Console.WriteLine("Start read ATM :" + listATM.DirName);
                        var files = listATM.FileName;
                        foreach (var file in files)
                        {
                            ExecuteReadFileJDATA(file, listATM.DirName);
                        }
                    }
                    else if (listATM.Extension == Constants.EXTENSION_Excel)
                    {
                        Console.WriteLine("Start read Excels :" + listATM.DirName);
                        var files = listATM.FileName;
                        foreach (var file in files)
                        {
                            ExecuteReadFileExcel(file);
                        }
                    }
                    else if (listATM.Extension == Constants.EXTENSION_DAT)
                    {
                        if (listATM.DirName.ToUpper() == Constants.FOLDER_BILLING)
                        {
                            Console.WriteLine("Start read Billing :" + listATM.DirName);
                            var files = listATM.FileName;
                            foreach (var file in files)
                            {
                                ExecuteReadFileBilling(file);
                            }
                        }
                        else if (listATM.DirName.ToUpper() == Constants.FOLDER_ECOM)
                        {
                            Console.WriteLine("Start read Ecom :" + listATM.DirName);
                            var files = listATM.FileName;
                            foreach (var file in files)
                            {
                                ExecuteReadFileEcom(file);
                            }
                        }
                    }
                }
            }

            Console.WriteLine("End Read File.");
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }

        private static void TestOutNapas()
        {
            string fileName = @"D:\" + DateTime.Now.ToString("MMddyy") + "_VAS_ISS_OUT_VRB970421.dat";



            WriteVASISS_OUT writeVASISS_OUT = new WriteVASISS_OUT();
            string test;
            test=writeVASISS_OUT.GetCheckSum("0002101910000000061    060000000600000000093141093140120712077399  970421      1234567870400000000VICREBILLING0000000000002090865902100116");
            //test=writeVASISS_OUT.GetCheckSum("0002104610000003559    060000000010000000000000081153111011107399  970421      11111111704000000000000VTLTOPUP0000000000038838556702100117");
            //test=writeVASISS_OUT.GetCheckSum("0002104810000006198    060000000001000000000000101142111011107399  970421      11111111704000000000000VTLTOPUP0000000000038458414702100117");
            test=writeVASISS_OUT.GetCheckSum("0009000000001           V1000047909314007122020");


            writeVASISS_OUT.ReadFileJson(@"D:\structure_napas.json");
            string strHeader = writeVASISS_OUT.BuildHeader();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                file.WriteLine(strHeader);
            }

            DataTable dataTable = new DataTable();
            dataTable = writeVASISS_OUT.GetData();
            var strBody = new StringBuilder();

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName,true))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    file.WriteLine(writeVASISS_OUT.BuildBody(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7].ToString(), row[8].ToString()));
                }
            }

            string strFooter = writeVASISS_OUT.BuildFooter(dataTable.Rows.Count.ToString());

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
            {
                file.WriteLine(strFooter);
            }
        }
        private static void TestOutNapasBBB()
        {
            //string fileName = @"D:\" + DateTime.Now.ToString("MMddyy") + "_ISS_VRB_970421_1_KTC.dat";
            string fileName = @"D:\120720_ISS_VRB_970421_1_SL_BNB.dat";


            WriteBBB_OUT writeBBB_OUT = new WriteBBB_OUT();
            string test;
            //test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]   9704211084279705[F3]000000[SVC]          [TCC]  [F4]000040000000[RTA]000040000000[F49]704[F5]000040000000[F50]704[F9]00000001[F6]000040000000[RCA]000040000000[F51]704[F10]00000001[F11]119188[F12]142550[F13]1220[F15]1220[F18]5100[F22]000[F25]22[F41]00000005[ACQ]981957  [ISS]  970421[MID]000000100080731[BNB]        [F102]              00000000000000[F103]                            [F37]000004644756[F38]      [TRN]AAcDQQCQX+LwcwAA[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            //test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]   9704211084279705[F3]000000[SVC]          [TCC]  [F4]000040000000[RTA]000040000000[F49]704[F5]000040000000[F50]704[F9]00000001[F6]000040000000[RCA]000040000000[F51]704[F10]00000001[F11]119190[F12]154005[F13]1220[F15]1220[F18]5100[F22]000[F25]22[F41]00000005[ACQ]981957  [ISS]  970421[MID]000000100080731[BNB]        [F102]              00000000000000[F103]                            [F37]000004644758[F38]      [TRN]AAcDQQCQX+MB2gAA[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            //test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]   9704211084279705[F3]000000[SVC]          [TCC]  [F4]000040000000[RTA]000040000000[F49]704[F5]000040000000[F50]704[F9]00000001[F6]000040000000[RCA]000040000000[F51]704[F10]00000001[F11]119191[F12]154008[F13]1220[F15]1220[F18]5100[F22]000[F25]22[F41]00000005[ACQ]981957  [ISS]  970421[MID]000000100080731[BNB]        [F102]              00000000000000[F103]                            [F37]000004644759[F38]      [TRN]AAcDQQCQX+MB3QAA[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            //test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]   9704211084279705[F3]000000[SVC]          [TCC]  [F4]000040000000[RTA]000040000000[F49]704[F5]000040000000[F50]704[F9]00000001[F6]000040000000[RCA]000040000000[F51]704[F10]00000001[F11]119192[F12]154010[F13]1220[F15]1220[F18]5100[F22]000[F25]22[F41]00000005[ACQ]981957  [ISS]  970421[MID]000000100080731[BNB]        [F102]              00000000000000[F103]                            [F37]000004644760[F38]      [TRN]AAcDQQCQX+MB3wAA[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            //test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]   9704211084279705[F3]000000[SVC]          [TCC]  [F4]000040000000[RTA]000040000000[F49]704[F5]000040000000[F50]704[F9]00000001[F6]000040000000[RCA]000040000000[F51]704[F10]00000001[F11]119193[F12]154013[F13]1220[F15]1220[F18]5100[F22]000[F25]22[F41]00000005[ACQ]981957  [ISS]  970421[MID]000000100080731[BNB]        [F102]              00000000000000[F103]                            [F37]000004644761[F38]      [TRN]AAcDQQCQX+MB4QAA[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");

            //test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]   9704211084279705[F3]010000[SVC]          [TCC]  [F4]000010000000[RTA]000010000000[F49]704[F5]000010000000[F50]704[F9]00000001[F6]000010000000[RCA]000010000000[F51]704[F10]00000001[F11]119199[F12]154026[F13]1220[F15]1220[F18]6011[F22]021[F25]22[F41]00000001[ACQ]981957  [ISS]  970421[MID]AB             [BNB]        [F102]              00000000000000[F103]                            [F37]000004644767[F38]      [TRN]AAcDQQCQX+MB7gAA[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            //test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]   9704211084279705[F3]010000[SVC]          [TCC]  [F4]000010000000[RTA]000010000000[F49]704[F5]000010000000[F50]704[F9]00000001[F6]000010000000[RCA]000010000000[F51]704[F10]00000001[F11]119201[F12]154034[F13]1220[F15]1220[F18]6011[F22]021[F25]22[F41]00000001[ACQ]981957  [ISS]  970421[MID]AB             [BNB]        [F102]              00000000000000[F103]                            [F37]000004644769[F38]      [TRN]AAcDQQCQX+MB9gAA[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            //test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]   9704211084279705[F3]010000[SVC]          [TCC]  [F4]000010000000[RTA]000010000000[F49]704[F5]000010000000[F50]704[F9]00000001[F6]000010000000[RCA]000010000000[F51]704[F10]00000001[F11]119202[F12]154040[F13]1220[F15]1220[F18]6011[F22]021[F25]22[F41]00000001[ACQ]981957  [ISS]  970421[MID]AB             [BNB]        [F102]              00000000000000[F103]                            [F37]000004644770[F38]      [TRN]AAcDQQCQX+MB/QAA[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            //test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]   9704211084279705[F3]010000[SVC]          [TCC]  [F4]000010000000[RTA]000010000000[F49]704[F5]000010000000[F50]704[F9]00000001[F6]000010000000[RCA]000010000000[F51]704[F10]00000001[F11]119203[F12]155140[F13]1220[F15]1220[F18]6011[F22]021[F25]22[F41]00000001[ACQ]981957  [ISS]  970421[MID]AB             [BNB]        [F102]              00000000000000[F103]                            [F37]000004644771[F38]      [TRN]AAcDQQCQX+MEkAAA[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            //test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]   9704211084279705[F3]010000[SVC]          [TCC]  [F4]000010000000[RTA]000010000000[F49]704[F5]000010000000[F50]704[F9]00000001[F6]000010000000[RCA]000010000000[F51]704[F10]00000001[F11]119204[F12]155140[F13]1220[F15]1220[F18]6011[F22]021[F25]22[F41]00000001[ACQ]981957  [ISS]  970421[MID]AB             [BNB]        [F102]              00000000000000[F103]                            [F37]000004644772[F38]      [TRN]AAcDQQCQX+MEkQAA[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            //test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]    104010000001004[F3]912000[SVC]    IF_DEP[TCC]04[F4]005000000000[RTA]005000000000[F49]704[F5]005000000000[F50]704[F9]61000000[F6]000000000000[RCA]000000000000[F51]704[F10]00000000[F11]005975[F12]164947[F13]1220[F15]1220[F18]7399[F22]000[F25]00[F41]97042100[ACQ]970421  [ISS]  970421[MID]               [BNB]  970406[F102]             104010000001004[F103]            9704060214599002[F37]035809005975[F38]      [TRN]kXz5hXEtlLJaGGOv[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            //test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]    104010000001004[F3]912000[SVC]    IF_DEP[TCC]04[F4]001000000000[RTA]001000000000[F49]704[F5]001000000000[F50]704[F9]61000000[F6]000000000000[RCA]000000000000[F51]704[F10]00000000[F11]005976[F12]165009[F13]1220[F15]1220[F18]7399[F22]000[F25]00[F41]97042100[ACQ]970421  [ISS]  970421[MID]               [BNB]  970406[F102]             104010000001004[F103]            9704060214599002[F37]035809005976[F38]      [TRN]POba9UsMqTBaXvJK[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            //test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]    104010000001004[F3]912000[SVC]    IF_DEP[TCC]04[F4]000900000000[RTA]000900000000[F49]704[F5]000900000000[F50]704[F9]61000000[F6]000000000000[RCA]000000000000[F51]704[F10]00000000[F11]005977[F12]165204[F13]1220[F15]1220[F18]7399[F22]000[F25]00[F41]97042100[ACQ]970421  [ISS]  970421[MID]               [BNB]  970406[F102]             104010000001004[F103]            9704060214599002[F37]035809005977[F38]      [TRN]dcvSuffNQzt2ZGZt[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");


            test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]   9704211084279705[F3]000000[SVC]          [TCC]  [F4]000040000000[RTA]000040000000[F49]704[F5]000040000000[F50]704[F9]00000001[F6]000040000000[RCA]000040000000[F51]704[F10]00000001[F11]119191[F12]154008[F13]1223[F15]1223[F18]5100[F22]000[F25]22[F41]00000005[ACQ]981957  [ISS]  970421[MID]000000100080731[BNB]        [F102]              00000000000000[F103]                            [F37]000004644759[F38]      [TRN]AAcDQQCQX+MB3QAA[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]   9704211084279705[F3]000000[SVC]          [TCC]  [F4]000040000000[RTA]000040000000[F49]704[F5]000040000000[F50]704[F9]00000001[F6]000040000000[RCA]000040000000[F51]704[F10]00000001[F11]119192[F12]154010[F13]1223[F15]1223[F18]5100[F22]000[F25]22[F41]00000005[ACQ]981957  [ISS]  970421[MID]000000100080731[BNB]        [F102]              00000000000000[F103]                            [F37]000004644760[F38]      [TRN]AAcDQQCQX+MB3wAA[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]   9704211084279705[F3]010000[SVC]          [TCC]  [F4]000010000000[RTA]000010000000[F49]704[F5]000010000000[F50]704[F9]00000001[F6]000010000000[RCA]000010000000[F51]704[F10]00000001[F11]119200[F12]154027[F13]1223[F15]1223[F18]6011[F22]021[F25]22[F41]00000001[ACQ]981957  [ISS]  970421[MID]AB             [BNB]        [F102]              00000000000000[F103]                            [F37]000004644768[F38]      [TRN]AAcDQQCQX+MB7wAA[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]   9704211084279705[F3]010000[SVC]          [TCC]  [F4]000010000000[RTA]000010000000[F49]704[F5]000010000000[F50]704[F9]00000001[F6]000010000000[RCA]000010000000[F51]704[F10]00000001[F11]119197[F12]154022[F13]1223[F15]1223[F18]6011[F22]021[F25]22[F41]00000001[ACQ]981957  [ISS]  970421[MID]AB             [BNB]        [F102]              00000000000000[F103]                            [F37]000004644765[F38]      [TRN]AAcDQQCQX+MB6wAA[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]    104010000001004[F3]912000[SVC]    IF_DEP[TCC]04[F4]005199428000[RTA]005199428000[F49]704[F5]005199428000[F50]704[F9]61000000[F6]000000000000[RCA]000000000000[F51]704[F10]00000000[F11]005973[F12]164850[F13]1223[F15]1223[F18]7399[F22]000[F25]00[F41]97042100[ACQ]970421  [ISS]  970421[MID]               [BNB]  970406[F102]             104010000001004[F103]            9704060214599002[F37]035809005973[F38]      [TRN]ZIC3Mq6QPeBfMC4P[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]    104010000001004[F3]912000[SVC]    IF_DEP[TCC]04[F4]005200000000[RTA]005200000000[F49]704[F5]005200000000[F50]704[F9]61000000[F6]000000000000[RCA]000000000000[F51]704[F10]00000000[F11]005974[F12]164915[F13]1223[F15]1223[F18]7399[F22]000[F25]00[F41]97042100[ACQ]970421  [ISS]  970421[MID]               [BNB]  970406[F102]             104010000001004[F103]            9704060214599002[F37]035809005974[F38]      [TRN]vFgmeQiJ5WjT7YoW[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            //test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]    104010000001004[F3]912000[SVC]    IF_DEP[TCC]04[F4]001000000000[RTA]001000000000[F49]704[F5]001000000000[F50]704[F9]61000000[F6]000000000000[RCA]000000000000[F51]704[F10]00000000[F11]005976[F12]165009[F13]1220[F15]1220[F18]7399[F22]000[F25]00[F41]97042100[ACQ]970421  [ISS]  970421[MID]               [BNB]  970406[F102]             104010000001004[F103]            9704060214599002[F37]035809005976[F38]      [TRN]POba9UsMqTBaXvJK[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");
            //test = writeBBB_OUT.GetCheckSum("DR[MTI]0210[F2]    104010000001004[F3]912000[SVC]    IF_DEP[TCC]04[F4]000900000000[RTA]000900000000[F49]704[F5]000900000000[F50]704[F9]61000000[F6]000000000000[RCA]000000000000[F51]704[F10]00000000[F11]005977[F12]165204[F13]1220[F15]1220[F18]7399[F22]000[F25]00[F41]97042100[ACQ]970421  [ISS]  970421[MID]               [BNB]  970406[F102]             104010000001004[F103]            9704060214599002[F37]035809005977[F38]      [TRN]dcvSuffNQzt2ZGZt[RRC]0116[RSV1]                                                                                                    [RSV2]                                                                                                    [RSV3]                                                                                                    [CSR]");



            test = writeBBB_OUT.GetCheckSum("TR[NOT]000000006[CRE]               admin[TIME]143918[DATE]24122020[CSF]");
            //test = writeBBB_OUT.GetCheckSum("TR000000002               admin06060002122020");


            writeBBB_OUT.ReadFileJson(@"D:\structure_napas_bbb.json");
            string strHeader = writeBBB_OUT.BuildHeader();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                file.WriteLine(strHeader);
            }

            DataTable dataTable = new DataTable();
            dataTable = writeBBB_OUT.GetDataBBB();
            var strBody = new StringBuilder();

            if (dataTable != null)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
                {

                    foreach (DataRow row in dataTable.Rows)
                    {
                        file.WriteLine(writeBBB_OUT.BuildBody(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7].ToString(), row[8].ToString(),
                            row[9].ToString(), row[10].ToString(), row[11].ToString(), row[12].ToString(), row[13].ToString(), row[14].ToString(), row[15].ToString()
                            ));
                    }
                }
            }


            //string strFooter = writeBBB_OUT.BuildFooter(dataTable.Rows.Count.ToString());
            string strFooter = writeBBB_OUT.BuildFooter(11.ToString());
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
            {
                file.WriteLine(strFooter);
            }
        }

        private static void ExecuteReadFileEcom(string filename)
        {
            try
            {
                Console.WriteLine("Start read file :" + filename);
                using (TransactionScope scope = new TransactionScope())
                {
                    var listEcomInfo = ReadFileEcom.ReadFile(filename);
                    string DataLogID = string.Empty;
                    // Ten file phai dinh dang DDMMYYYY
                    DataLogController.FinalReadFile(Constants.FileTopupBilling, Path.GetFileNameWithoutExtension(filename).Substring(0, 6), Path.GetFileName(filename), out DataLogID);
                    if (!string.IsNullOrEmpty(DataLogID))
                    {
                        ReadFileEcomController.ReadFileEcom(listEcomInfo, DataLogID, Path.GetFileNameWithoutExtension(filename).Substring(0, 6), Path.GetFileName(filename));
                        scope.Complete();

                        //BackUpFile(filename, "Ecom", Constants.READFILE_SUCCESS);
                        Console.WriteLine("End read file Success:" + filename);
                    }
                }

            }
            catch (Exception ex)
            {
                BackUpFile(filename, "Ecom", Constants.READFILE_FAIL);
                Console.WriteLine(filename + "-" + ex.Message);
                ErrorUtils.WriteLog(filename + "-" + ex.Message);
            }
            finally
            {
                BackUpFile(filename, "Ecom", Constants.READFILE_SUCCESS);
            }

        }

        private static void ExecuteJRN(string filename, string machineCode)
        {
            try
            {
                Console.WriteLine("Start read file :" + filename);
                using (TransactionScope scope = new TransactionScope())
                {
                    string DataLogID = string.Empty;
                    var listTransInfo = ReadFileATM.ReadFileJRN(filename);

                    DataLogController.FinalReadFile(Constants.FileData_JRN, Path.GetFileNameWithoutExtension(filename).Substring(Path.GetFileNameWithoutExtension(filename).Length - 8, 8), machineCode + Path.GetFileName(filename), out DataLogID);
                    if (!string.IsNullOrEmpty(DataLogID))
                    {
                        ReadFileATMController.ReadFileJRN(listTransInfo, machineCode, DataLogID);
                        scope.Complete();
                        Console.WriteLine("End read file Success:" + filename);
                        BackUpFile(filename, machineCode, Constants.READFILE_SUCCESS);
                    }
                }
            }
            catch (TransactionAbortedException ex)
            {
                BackUpFile(filename, machineCode, Constants.READFILE_FAIL);
                ErrorUtils.WriteLog("ReadFileJDATA : TransactionAbortedException File: " + filename + " MsgErr:" + ex.Message);
                //throw ex;
            }
            catch (Exception ex)
            {
                BackUpFile(filename, machineCode, Constants.READFILE_FAIL);
                Console.WriteLine(filename + "-" + ex.Message);
                ErrorUtils.WriteLog(filename + "-" + ex.Message);
            }

        }
        private static void ExecuteReadFileJDATA(string filename, string machineCode)
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
                        DataLogController.FinalReadFile(Constants.FileData_LOG, DateTime.Parse(trandate).ToString("ddMMyyyy"), machineCode + Path.GetFileName(filename), out DataLogID);
                        if (!string.IsNullOrEmpty(DataLogID))
                        {
                            ReadFileATMController.ReadFileEJDATA(listTransInfo, machineCode, DataLogID);
                            scope.Complete();
                            Console.WriteLine("End read file Success:" + filename);
                            BackUpFile(filename, machineCode, Constants.READFILE_SUCCESS);
                        }
                    }
                }
                else
                {
                    BackUpFile(filename, machineCode, Constants.READFILE_FAIL);
                }

            }
            catch (TransactionAbortedException ex)
            {
                BackUpFile(filename, machineCode, Constants.READFILE_FAIL);
                ErrorUtils.WriteLog("ReadFileJDATA : TransactionAbortedException File: " + filename + " MsgErr:" + ex.Message);
                //throw ex;
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
                    string DataLogID = string.Empty;
                    if (filename.Contains(Constants.FILE_NAME_VISA_DEBIT))
                    {
                        var listVisaDebitFee = ReadFileExcel.ReadExcelVisaDebit(filename);
                        // Ten file phai dinh dang DDMMYYYY
                        DataLogController.FinalReadFile(Constants.FileData_VisaDebitFee, "0101" + Path.GetFileNameWithoutExtension(filename).Substring(Path.GetFileNameWithoutExtension(filename).Length - 4, 4), Path.GetFileName(filename), out DataLogID);
                        if (!string.IsNullOrEmpty(DataLogID))
                        {
                            ReadFileExcelController.ReadFileExcelVisa(listVisaDebitFee, DataLogID, Path.GetFileNameWithoutExtension(filename).Substring(Path.GetFileNameWithoutExtension(filename).Length - 4, 4));
                        }
                    }
                    else
                    {
                        var listTransInfo = ReadFileExcel.ReadExcel(filename);
                        // Ten file phai dinh dang DDMMYYYY
                        DataLogController.FinalReadFile(Constants.FileData_HT, Path.GetFileNameWithoutExtension(filename).Substring(Path.GetFileNameWithoutExtension(filename).Length - 8, 8), Path.GetFileName(filename), out DataLogID);
                        if (!string.IsNullOrEmpty(DataLogID))
                        {
                            ReadFileExcelController.ReadFileExcel(listTransInfo, DataLogID, Path.GetFileNameWithoutExtension(filename).Substring(Path.GetFileNameWithoutExtension(filename).Length - 8, 8));
                        }
                    }

                    scope.Complete();
                    BackUpFile(filename, "Excels", Constants.READFILE_SUCCESS);
                    Console.WriteLine("End read file Success:" + filename);

                }
            }
            catch (TransactionAbortedException ex)
            {
                BackUpFile(filename, "Excels", Constants.READFILE_FAIL);
                ErrorUtils.WriteLog("ReadFileExcel : TransactionAbortedException File: " + filename + " MsgErr:" + ex.Message);
                //throw ex;
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
                Console.WriteLine("Start read file :" + filename);
                using (TransactionScope scope = new TransactionScope())
                {
                    var listBillingInfo = ReadFileBilling.ReadFile(filename);
                    string DataLogID = string.Empty;
                    // Ten file phai dinh dang DDMMYYYY
                    DataLogController.FinalReadFile(Constants.FileTopupBilling, Path.GetFileNameWithoutExtension(filename).Substring(0, 6), Path.GetFileName(filename), out DataLogID);
                    if (!string.IsNullOrEmpty(DataLogID))
                    {
                        ReadFileBillingController.ReadFileBilling(listBillingInfo, DataLogID, Path.GetFileNameWithoutExtension(filename).Substring(0, 6), Path.GetFileName(filename));
                        scope.Complete();

                        //BackUpFile(filename, "Billing", Constants.READFILE_SUCCESS);
                        Console.WriteLine("End read file Success:" + filename);
                    }
                }

            }
            catch (Exception ex)
            {
                BackUpFile(filename, "Billing", Constants.READFILE_FAIL);
                Console.WriteLine(filename + "-" + ex.Message);
                ErrorUtils.WriteLog(filename + "-" + ex.Message);
            }
            finally
            {
                BackUpFile(filename, "Billing", Constants.READFILE_SUCCESS);
            }
        }

        private static Boolean CheckBeforeIns(string fileName, string extension)
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
        private static void BackUpFile(string fileName, string Dir, string typeFile)
        {
            try
            {
                string fileNameresult = Path.GetFileName(fileName);
                string sourcePath = App.Configs.DirectoryDataPath + Dir;
                string targetPath = string.Empty;
                if (typeFile == Constants.READFILE_SUCCESS)
                {
                    targetPath = App.Configs.DirectoryDataPath.ToString() + @"Backup\" + Dir;

                }
                else
                {
                    targetPath = App.Configs.DirectoryDataPath.ToString() + @"Backup\Error\" + Dir;

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
                        //Console.WriteLine("Backup Sucessful file " + fileName);
                        try
                        {
                            File.Delete(sourceFile);
                        }
                        catch (Exception ex)
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
            catch(Exception ex)
            {
                ErrorUtils.WriteLog("Error Backup file  " + fileName + ":" + ex.Message);
            }
        }
    }
}
