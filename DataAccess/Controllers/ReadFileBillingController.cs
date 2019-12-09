using Models.Common;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Controllers
{
    public class ReadFileBillingController
    {
        public static void ReadFileBilling(List<BillingInfo> billingInfos)
        {
            try
            {
                foreach (var billingInfo in billingInfos)
                {
                    var values = new List<string>();
                    values.Add(billingInfo.MTI);
                    values.Add(billingInfo.PAN_ACC);
                    values.Add(billingInfo.PROCESS_CODE);
                    values.Add(billingInfo.AMOUNT);
                    values.Add(billingInfo.TRACE_NUMBER);
                    values.Add(billingInfo.TRANS_TIME);
                    values.Add(billingInfo.TRANS_DATE);
                    values.Add(billingInfo.PAYMENT_DATE);
                    values.Add(billingInfo.DEVICE_TYPE);
                    values.Add(billingInfo.BANK_CODE);
                    values.Add(billingInfo.DEVICE_CODE);
                    values.Add(billingInfo.CCY);
                    values.Add(billingInfo.ACC_FROM);
                    values.Add(billingInfo.ACC_TO);
                    values.Add(billingInfo.STATUS);

                    OracleHelper.ExecuteStoreProcedure(App.Configs.ConnectionString, Constants.PROC_ATMINFOS_ADD, values.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
