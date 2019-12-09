using Models.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccess.Controllers
{
    public class TxCheckController
    {
        public static void CheckFileJRN(string txDate,out Boolean bCheck)
        {
            bCheck = true;
            var values = new List<string>();
            DataTable dt = new DataTable();
            values.Add(txDate);

            OracleHelper.FillDataTable (App.Configs.ConnectionString, Constants.PROC_GET_ATMPOSJRN, out dt, values.ToArray());
            if (dt.Rows.Count > 0) bCheck = false;
        }
        public static void CheckFileEJDATA(string txDate, out Boolean bCheck)
        {
            bCheck = true;
            var values = new List<string>();
            DataTable dt = new DataTable();
            values.Add(txDate);

            OracleHelper.FillDataTable(App.Configs.ConnectionString, Constants.PROC_GET_ATMPOS, out dt, values.ToArray());
            if (dt.Rows.Count > 0) bCheck = false;
        }
        public static void CheckFileExcel(string txDate, out Boolean bCheck)
        {
            bCheck = true;
            var values = new List<string>();
            DataTable dt = new DataTable();
            values.Add(txDate);

            OracleHelper.FillDataTable(App.Configs.ConnectionString, Constants.PROC_GET_ACCOUNTING, out dt, values.ToArray());
            if (dt.Rows.Count > 0) bCheck = false;
        }

    }
}
