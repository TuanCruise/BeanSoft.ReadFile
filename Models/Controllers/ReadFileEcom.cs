using Models.Base;
using Models.Common;
using Models.Entities;
using System;
using System.Collections.Generic;

namespace Models.Controllers
{
    public class ReadFileEcom : ControllerBase
    {
        public static List<EcomInfo> ReadFile(string fileName)
        {
            try
            {
                EcomInfo ecomInfo = new EcomInfo();
                EcomInfoHeader ecomInfoHeader = new EcomInfoHeader();
                EcomInfoFooter ecomInfoFooter = new EcomInfoFooter();
                List<EcomInfo> listecomInfo = new List<EcomInfo>();

                string[] lines = System.IO.File.ReadAllLines(fileName);
                foreach (var line in lines)
                {
                    ecomInfo = new EcomInfo();
                    //string result = Regex.Replace(line, @"\s", "");
                    string result = string.Empty;
                    if (line.Trim().Length == 18)
                    {
                        ReadFileHeader(line);
                    }
                    else if (line.Trim().Length > 80)
                    {
                        ecomInfo.DETAILRECORD = line.Substring(Constants.POS_BASE, 4);
                        ecomInfo.PANNO = line.Substring(Constants.POS_1, 19);
                        ecomInfo.PROCESSING_CODE = line.Substring(Constants.POS_2, 6);
                        ecomInfo.AMOUNT = line.Substring(Constants.POS_3, 12);
                        ecomInfo.TRACE_NUMBER = line.Substring(Constants.POS_4, 6);
                        ecomInfo.TRANS_TIME = line.Substring(Constants.POS_5, 6);
                        ecomInfo.TRANS_DATE = line.Substring(Constants.POS_6, 4);
                        ecomInfo.PAYMENT_DATE = line.Substring(Constants.POS_7, 4);
                        ecomInfo.DEVICE_TYPE = line.Substring(Constants.POS_8, 4);
                        ecomInfo.BANK_CODE = line.Substring(Constants.POS_9, 8);
                        ecomInfo.AUTH_NUMBER = line.Substring(Constants.POS_10, 6);
                        ecomInfo.DEVICE_CODE = line.Substring(Constants.POS_11, 8);
                        ecomInfo.CCY = line.Substring(Constants.POS_12, 3);
                        ecomInfo.TRANSACTION_CODE = line.Substring(Constants.POS_13, 20);
                        ecomInfo.AUTHORISATION_CODE = line.Substring(Constants.POS_14, 20);
                        ecomInfo.MTI = line.Substring(Constants.POS_15, 4);
                        ecomInfo.STATUS = line.Substring(Constants.POS_16, 4);
                        ecomInfo.CHECKSUM = line.Substring(Constants.POS_17, 32);

                        listecomInfo.Add(ecomInfo);
                    }
                    else
                    {
                        // Read Footer
                        ReadFileFooter(line);
                    }


                }
                return listecomInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static EcomInfoHeader ReadFileHeader(string line)
        {
            try
            {
                EcomInfoHeader ecomInfoHeader = new EcomInfoHeader();
                //if (line.Length == 18)
                //{
                // Read Header
                ecomInfoHeader.HeaderTypeRecord = line.Substring(Constants.POS_BASE, 4);
                ecomInfoHeader.BIN = line.Substring(4, 8);
                ecomInfoHeader.TxDate = line.Substring(12, 6);
                //}

                return ecomInfoHeader;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static EcomInfoFooter ReadFileFooter(string line)
        {
            try
            {
                EcomInfoFooter ecomInfoFooter = new EcomInfoFooter();
                //if (line.Length == 80)
                //{ 
                // Read Footer
                ecomInfoFooter.FooterTypeRecord = line.Substring(Constants.POS_BASE, 4);
                ecomInfoFooter.NoRecord = line.Substring(5, 9);
                ecomInfoFooter.CreatedBy = line.Substring(14, 20);
                ecomInfoFooter.TimeCreated = line.Substring(34, 6);
                ecomInfoFooter.DateCreated = line.Substring(40, 8);
                ecomInfoFooter.CheckFileValue = line.Substring(48, 32);
                // }

                return ecomInfoFooter;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
