using Models.Base;
using Models.Common;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Models.Controllers
{
    public class ReadFileBilling : ControllerBase
    {
        public static List<BillingInfo> ReadFile(string fileName)
        {
            try
            {
                BillingInfo billingInfo = new BillingInfo();
                List<BillingInfo> listBillingInfo = new List<BillingInfo>();

                string[] lines = System.IO.File.ReadAllLines(fileName);
                foreach (var line in lines)
                {
                 
                    billingInfo = new BillingInfo();
                    string result = Regex.Replace(line, @"\s", "");
                    if (result.Length > 100)
                    {
                        billingInfo.MTI = result.Substring(Constants.POS_BASE, 4);
                        billingInfo.PAN_ACC = result.Substring(Constants.POS_1, 20);
                        billingInfo.PROCESS_CODE = result.Substring(Constants.POS_2, 6);
                        billingInfo.AMOUNT = result.Substring(Constants.POS_3, 12);
                        billingInfo.TRACE_NUMBER = result.Substring(Constants.POS_4, 6);
                        billingInfo.TRANS_TIME = result.Substring(Constants.POS_5, 6);
                        billingInfo.TRANS_DATE = result.Substring(Constants.POS_6, 4);
                        billingInfo.PAYMENT_DATE = result.Substring(Constants.POS_7, 4);
                        billingInfo.DEVICE_TYPE = result.Substring(Constants.POS_8, 4);
                        billingInfo.BANK_CODE = result.Substring(Constants.POS_9, 6);
                        //billingInfo.AUTH_NUMBER = result.Substring(0, Constants.POS_11);
                        billingInfo.DEVICE_CODE = result.Substring(Constants.POS_10, 8);
                        billingInfo.CCY = result.Substring(Constants.POS_11, 3);
                        billingInfo.ACC_FROM = result.Substring(Constants.POS_12, 20);
                        billingInfo.ACC_TO = result.Substring(Constants.POS_13, 20);
                        billingInfo.STATUS = result.Substring(Constants.POS_14, 4);

                        listBillingInfo.Add(billingInfo);
                    }

                }
                return listBillingInfo;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
