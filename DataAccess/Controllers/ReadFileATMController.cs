using Models.Base;
using Models.Common;
using Models.Entities;
using System;
using System.Collections.Generic;

namespace DataAccess.Controllers
{
    public class ReadFileATMController : ControllerBase
    {
        public static void ReadFileEJDATA(List<TransInfo> transInfo,string machineCode, string DataLogID)
        {
            try
            {
                foreach (var traninfo in transInfo)
                {
                    var values = new List<string>();
                    values.Add(traninfo.CARD_INSERTED);
                    values.Add(traninfo.CARD_NUMBER);
                    values.Add(traninfo.CUSTOMER_NAME);
                    values.Add(traninfo.PIN_ENTERED);
                    values.Add(traninfo.TRANSACTION_TYPE);
                    values.Add(traninfo.AMOUNT);
                    values.Add(traninfo.CARD_TAKEN);
                    values.Add(traninfo.TRANSACTION_END);
                    values.Add(traninfo.NOTES_PRESENTED);
                    values.Add(traninfo.NOTES_RETRACTED);
                    values.Add(traninfo.NOTES_TAKEN);
                    values.Add(machineCode);
                    values.Add(DataLogID);

                    OracleHelper.ExecuteStoreProcedure(App.Configs.ConnectionString, Constants.PROC_ATMINFOS_ADD, values.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ReadFileJRN(List<TransInfo> transInfo,string machineCode, string DataLogID)
        {
            try
            {
                foreach (var traninfo in transInfo)
                {
                    var values = new List<string>();
                    values.Add(traninfo.TRANSACTION_START);
                    values.Add(traninfo.CARD_NUMBER);
                    values.Add(traninfo.CASH_REQUEST);
                    values.Add(traninfo.CASH);
                    values.Add(traninfo.CASH_PRESENTED);
                    values.Add(traninfo.CASH_TAKEN);
                    values.Add(traninfo.AMOUNT);
                    values.Add(traninfo.TRANSACTION_END);
                    values.Add(traninfo.TRANSACTION_DATE);
                    values.Add(machineCode);
                    values.Add(DataLogID);

                    OracleHelper.ExecuteStoreProcedure(App.Configs.ConnectionString, Constants.PROC_ATMINFOSJRN_ADD, values.ToArray());
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
