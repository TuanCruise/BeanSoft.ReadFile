using Models.Base;
using Models.Common;
using Models.Entities;
using System;
using System.Collections.Generic;

namespace Models.Controllers
{
    public class ReadFileBilling : ControllerBase
    {
        public static List<BillingInfo> ReadFile(string fileName)
        {
            try
            {
                BillingInfo billingInfo = new BillingInfo();
                BillingInfoHeader billingInfoHeader = new BillingInfoHeader();
                BillingInfoFooter billingInfoFooter = new BillingInfoFooter();
                List<BillingInfo> listBillingInfo = new List<BillingInfo>();

                string[] lines = System.IO.File.ReadAllLines(fileName);
                foreach (var line in lines)
                {
                    billingInfo = new BillingInfo();
                    //string result = Regex.Replace(line, @"\s", "");
                    string result = string.Empty;
                    if (line.Trim().Length == 18)
                    {
                        ReadFileHeader(line);
                    }
                    else if (line.Trim().Length > 80)
                    {
                        billingInfo.DETAILRECORD = line.Substring(Constants.POS_BASE, 4);
                        billingInfo.PANNO = line.Substring(Constants.POS_1, 19);
                        billingInfo.PROCESSING_CODE = line.Substring(Constants.POS_2, 6);
                        billingInfo.AMOUNT = line.Substring(Constants.POS_3, 12);
                        billingInfo.TRACE_NUMBER = line.Substring(Constants.POS_4, 6);
                        billingInfo.TRANS_TIME = line.Substring(Constants.POS_5, 6);
                        billingInfo.TRANS_DATE = line.Substring(Constants.POS_6, 4);
                        billingInfo.PAYMENT_DATE = line.Substring(Constants.POS_7, 4);
                        billingInfo.DEVICE_TYPE = line.Substring(Constants.POS_8, 4);
                        billingInfo.BANK_CODE = line.Substring(Constants.POS_9, 8);
                        billingInfo.AUTH_NUMBER = line.Substring(Constants.POS_10, 6);
                        billingInfo.DEVICE_CODE = line.Substring(Constants.POS_11, 8);
                        billingInfo.CCY = line.Substring(Constants.POS_12, 3);
                        billingInfo.FROM_ACC = line.Substring(Constants.POS_13, 20);
                        billingInfo.TO_ACC = line.Substring(Constants.POS_14, 20);
                        billingInfo.MTI = line.Substring(Constants.POS_15, 4);
                        billingInfo.STATUS = line.Substring(Constants.POS_16, 4);
                        billingInfo.CHECKSUM = line.Substring(Constants.POS_17, 32);

                        listBillingInfo.Add(billingInfo);
                    }
                    else
                    {
                        // Read Footer
                        ReadFileFooter(line);
                    }


                }
                return listBillingInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static BillingInfoHeader ReadFileHeader(string line)
        {
            try
            {
                BillingInfoHeader billingInfoHeader = new BillingInfoHeader();
                //if (line.Length == 18)
                //{
                // Read Header
                billingInfoHeader.HeaderTypeRecord = line.Substring(Constants.POS_BASE, 4);
                billingInfoHeader.BIN = line.Substring(4, 8);
                billingInfoHeader.TxDate = line.Substring(12, 6);
                //}

                return billingInfoHeader;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static BillingInfoFooter ReadFileFooter(string line)
        {
            try
            {
                BillingInfoFooter billingInfoFooter = new BillingInfoFooter();
                //if (line.Length == 80)
                //{ 
                // Read Footer
                billingInfoFooter.FooterTypeRecord = line.Substring(Constants.POS_BASE, 4);
                billingInfoFooter.NoRecord = line.Substring(5, 9);
                billingInfoFooter.CreatedBy = line.Substring(14, 20);
                billingInfoFooter.TimeCreated = line.Substring(34, 6);
                billingInfoFooter.DateCreated = line.Substring(40, 8);
                billingInfoFooter.CheckFileValue = line.Substring(48, 32);
                // }

                return billingInfoFooter;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
