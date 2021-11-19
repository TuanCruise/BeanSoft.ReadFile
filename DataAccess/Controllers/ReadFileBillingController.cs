using Models.Common;
using Models.Entities;
using System;
using System.Collections.Generic;

namespace DataAccess.Controllers
{
    public class ReadFileBillingController
    {
        public static void ReadFileBilling(List<BillingInfo> billingInfos, string DataLogID, string reportDate, string fileName)
        {
            try
            {
                foreach (var billingInfo in billingInfos)
                {
                    var values = new List<string>();
                    values.Add(billingInfo.DETAILRECORD);
                    values.Add(billingInfo.PANNO);
                    values.Add(billingInfo.PROCESSING_CODE);
                    values.Add(billingInfo.AMOUNT);
                    values.Add(billingInfo.TRACE_NUMBER);
                    values.Add(billingInfo.TRANS_TIME);
                    values.Add(billingInfo.TRANS_DATE);
                    values.Add(billingInfo.PAYMENT_DATE);
                    values.Add(billingInfo.DEVICE_TYPE);
                    values.Add(billingInfo.BANK_CODE);
                    values.Add(billingInfo.AUTH_NUMBER);
                    values.Add(billingInfo.DEVICE_CODE);
                    values.Add(billingInfo.CCY);
                    values.Add(billingInfo.FROM_ACC);
                    values.Add(billingInfo.TO_ACC);
                    values.Add(billingInfo.MTI);
                    values.Add(billingInfo.STATUS);
                    values.Add(billingInfo.CHECKSUM);
                    values.Add(fileName);

                    OracleHelper.ExecuteStoreProcedure(App.Configs.ConnectionString, Constants.PROC_BILLINGINFO_ADD, values.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
