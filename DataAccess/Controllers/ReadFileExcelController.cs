using Models.Base;
using Models.Common;
using Models.Entities;
using System;
using System.Collections.Generic;

namespace DataAccess.Controllers
{
    public class ReadFileExcelController: ControllerBase
    {
        public static void ReadFileExcel(List<Accounting> traninfos, string DataLogID)
        {
            try
            {
                foreach (var traninfo in traninfos)
                {
                    var values = new List<string>();
                    values.Add(traninfo.Mid);
                    values.Add(traninfo.OrgName);
                    values.Add(traninfo.TxDate);
                    values.Add(traninfo.Acctno);
                    values.Add(traninfo.Qtty);
                    values.Add(traninfo.Amt);
                    values.Add(traninfo.Fee);
                    values.Add(traninfo.Vat);
                    values.Add(traninfo.OrgAmt);
                    values.Add(traninfo.FeePercent);
                    values.Add(traninfo.BranchID);
                    values.Add(traninfo.FeeMaster);
                    values.Add(traninfo.FeeJCB);
                    values.Add(traninfo.TranType);
                    values.Add(traninfo.CifNo);
                    values.Add(traninfo.TranDate);
                    values.Add(traninfo.Description);
                    values.Add(traninfo.Parameters);
                    values.Add(traninfo.CardNo);
                    values.Add(traninfo.FeeAndVat);
                    values.Add(traninfo.Currency);
                    values.Add(traninfo.PosEximNo);
                    values.Add(traninfo.PosVrbNo);
                    values.Add(DataLogID);


                    OracleHelper.ExecuteStoreProcedure(App.Configs.ConnectionString, Constants.PROC_ACCOUNTING_ADD, values.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
