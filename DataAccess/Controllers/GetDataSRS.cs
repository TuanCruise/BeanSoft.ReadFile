using Models.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccess.Controllers
{
    public class GetDataSRS
    {
        public static DataTable GetData()
        {           
            DataTable dt;
            var values = new List<string>();
            OracleHelper.FillDataTable(App.Configs.ConnectionString, Constants.PROC_GET_DATA_SRS, out dt, values.ToArray());
            return dt;           
        }
    }
}
