using Models.Base;
using Models.Common;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess.Controllers
{
    public class DataLogController
    {
        public static void FinalReadFile(string FileID,string reportDate, string fileName,out string logID )
        {
            try
            {
                logID = string.Empty;
                var values = new List<string>();
                values.Add(FileID);
                values.Add(reportDate);
                values.Add(fileName);

                DataTable dt;

                OracleHelper.FillDataTable(App.Configs.ConnectionString, Constants.PROC_LOG_ADD,out dt, values.ToArray());
                if (dt.Rows.Count > 0)
                {
                    logID = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorUtils.WriteLog("FinalReadFile Error :" + ex.Message);
                throw ex;
            }
        }
    }
}
