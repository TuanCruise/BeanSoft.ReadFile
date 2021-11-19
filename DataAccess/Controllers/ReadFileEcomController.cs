using Models.Common;
using Models.Entities;
using System;
using System.Collections.Generic;

namespace DataAccess.Controllers
{
    public class ReadFileEcomController
    {
        public static void ReadFileEcom(List<EcomInfo> ecomInfos, string DataLogID, string reportDate, string filename)
        {
            try
            {
                foreach (var ecomInfo in ecomInfos)
                {
                    var values = new List<string>();
                    values.Add(ecomInfo.DETAILRECORD);
                    values.Add(ecomInfo.PANNO);
                    values.Add(ecomInfo.PROCESSING_CODE.Trim());
                    values.Add(ecomInfo.AMOUNT);
                    values.Add(ecomInfo.TRACE_NUMBER);
                    values.Add(ecomInfo.TRANS_TIME);
                    values.Add(ecomInfo.TRANS_DATE);
                    values.Add(ecomInfo.PAYMENT_DATE);
                    values.Add(ecomInfo.DEVICE_TYPE);
                    values.Add(ecomInfo.BANK_CODE);
                    values.Add(ecomInfo.AUTH_NUMBER.Trim());
                    values.Add(ecomInfo.DEVICE_CODE.Trim());
                    values.Add(ecomInfo.CCY);
                    values.Add(ecomInfo.TRANSACTION_CODE);
                    values.Add(ecomInfo.AUTHORISATION_CODE);
                    values.Add(ecomInfo.MTI);
                    values.Add(ecomInfo.STATUS);
                    values.Add(ecomInfo.CHECKSUM);
                    values.Add(filename);

                    OracleHelper.ExecuteStoreProcedure(App.Configs.ConnectionString, Constants.PROC_ECOMINFO_ADD, values.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
