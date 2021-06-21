using Models.Common;
using Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccess.Controllers
{
    public class WriteBBB_OUT
    {
        List<NapasInfo> napasInfos = new List<NapasInfo>();
        public string BuildHeader()
        {
            string result = string.Empty;
            var sb = new StringBuilder();
            sb.Append(GetValue("HeaderRecord", "HR"));
            sb.Append(GetValue("REV", "[REV]"));
            sb.Append(GetValue("BIN", "970421"));
            sb.Append(GetValue("TXDATE", "[DATE]"));
            sb.Append(GetValue("LASTTXDATE", DateTime.Now.ToString("MMddyyyy")));
            
            return result = sb.ToString();
        }
        public string BuildBody(string CardNo,string Amount,string processingcode,string TraceNo,string TranHour,string TranDate,string PayDate,string CCYCD,string FromAcc,string ToAcc,string termsic,
            string posentrymode,string termname,string terminstid,string termretailername,string rc)
        {
            string result = string.Empty;
            var sb = new StringBuilder();
            
            sb.Append(GetValue("DetailRecord", "DR"));
            sb.Append(GetValue("MTI", "[MTI]")); // 
            sb.Append(GetValue("MTINO", "0210")); // 
            sb.Append(GetValue("F2", "[F2]")); // so the
            sb.Append(GetValue("F2NO", CardNo)); // so the
            sb.Append(GetValue("F3", "[F3]")); // ma xu ly
            sb.Append(GetValue("F3NO", processingcode)); // ma xu ly

            sb.Append(GetValue("SVC", "[SVC]")); // ma xu ly
            sb.Append(GetValue("SVCNO", null)); // ma xu ly

            sb.Append(GetValue("TCC", "[TCC]")); // ma xu ly
            sb.Append(GetValue("TCCNO", null)); // ma xu ly

            sb.Append(GetValue("F4", "[F4]")); // so tien giao dich
            sb.Append(GetValue("F4NO", Amount)); // so tien giao dich

            sb.Append(GetValue("RTA", "[RTA]")); // so tien giao dich
            sb.Append(GetValue("RTANO", Amount)); // so tien giao dich

            sb.Append(GetValue("F49", "[F49]")); // so tien giao dich
            sb.Append(GetValue("F49NO", CCYCD)); // so tien giao dich

            sb.Append(GetValue("F5", "[F5]")); // so tien giao dich
            sb.Append(GetValue("F5NO", Amount)); // so tien giao dich

            sb.Append(GetValue("F50", "[F50]")); // 
            sb.Append(GetValue("F50NO", CCYCD)); // 

            sb.Append(GetValue("F9", "[F9]")); // ty gia
            sb.Append(GetValue("F9NO", "00000001")); //

            sb.Append(GetValue("F6", "[F6]")); // so tien giao dich
            sb.Append(GetValue("F6NO", Amount)); // so tien giao dich

            sb.Append(GetValue("RCA", "[RCA]")); // so tien giao dich
            sb.Append(GetValue("RCANO", Amount)); // so tien giao 

            sb.Append(GetValue("F51", "[F51]")); // so tien giao dich
            sb.Append(GetValue("F51NO", CCYCD)); // so tien giao dich

            sb.Append(GetValue("F10", "[F10]")); 
            sb.Append(GetValue("F10NO", "00000001"));

            sb.Append(GetValue("F11", "[F11]"));
            sb.Append(GetValue("F11NO", TraceNo));

            sb.Append(GetValue("F12", "[F12]"));
            sb.Append(GetValue("F12NO", TranHour));

            sb.Append(GetValue("F13", "[F13]"));
            sb.Append(GetValue("F13NO", TranDate));

            sb.Append(GetValue("F15", "[F15]"));
            sb.Append(GetValue("F15NO", PayDate));

            sb.Append(GetValue("F18", "[F18]"));
            sb.Append(GetValue("F18NO", termsic));

            sb.Append(GetValue("F22", "[F22]"));
            sb.Append(GetValue("F22NO", posentrymode));

            sb.Append(GetValue("F25", "[F25]"));
            sb.Append(GetValue("F25NO", "22"));

            sb.Append(GetValue("F41", "[F41]"));
            sb.Append(GetValue("F41NO", termname));

            sb.Append(GetValue("ACQ", "[ACQ]"));
            sb.Append(GetValue("ACQNO", terminstid));

            sb.Append(GetValue("ISS", "[ISS]"));
            sb.Append(GetValue("ISSNO", "970421"));

            sb.Append(GetValue("MID", "[MID]"));
            sb.Append(GetValue("MIDNO", termretailername));

            sb.Append(GetValue("BNB", "[BNB]"));
            sb.Append(GetValue("BNBNO", null));

            sb.Append(GetValue("F102", "[F102]"));
            sb.Append(GetValue("F102NO", FromAcc));

            sb.Append(GetValue("F103", "[F103]"));
            sb.Append(GetValue("F103NO", ToAcc));

            sb.Append(GetValue("F37", "[F37]"));
            sb.Append(GetValue("F37NO", null));

            sb.Append(GetValue("F38", "[F38]"));
            sb.Append(GetValue("F38NO", null));

            sb.Append(GetValue("TRN", "[TRN]"));
            sb.Append(GetValue("TRNNO", null));

            sb.Append(GetValue("RRC", "[RCC]"));
            sb.Append(GetValue("RRCNO", rc));

            sb.Append(GetValue("RRC", "[RSV1]"));
            sb.Append(GetValue("RRCNO", null));

            sb.Append(GetValue("RRC", "[RSV2]"));
            sb.Append(GetValue("RRCNO", null));

            sb.Append(GetValue("RRC", "[RSV3]"));
            sb.Append(GetValue("RRCNO", null));


            sb.Append(GetValue("CSR", "[CSR]"));
            sb.Append(GetCheckSum(sb.ToString()));
            return result = sb.ToString();
        }
        public  string BuildFooter(string totalTrans)
        {
            string result = string.Empty;
            var sb = new StringBuilder();
            sb.Append(GetValue("TrailerRecord", "TR"));
            sb.Append(GetValue("NOT", "[NOT]"));
            sb.Append(GetValue("TOTAL_ROWS", totalTrans));
            sb.Append(GetValue("CREATED_BY", "[CRE]"));
            sb.Append(GetValue("CREATED_BY_VAL", "admin"));
            sb.Append(GetValue("CREATED_HOUR", "[TIME]"));
            sb.Append(GetValue("CREATED_HOUR_VAL", DateTime.Now.ToString("HHmmss")));
            sb.Append(GetValue("CREADTED_DATE", "[DATE]"));
            sb.Append(GetValue("CREADTED_DATE_VAL", DateTime.Now.ToString("MMddyyyy")));
            sb.Append(GetValue("CSF", "[CSF]"));
            sb.Append(GetCheckSum(sb.ToString()));
            return result = sb.ToString();
        }

        private string GetValue(string fldname, string value)
        {
            string result = string.Empty;
            var sb = new StringBuilder();
            foreach (var napasInfo in napasInfos)
            {
                if (napasInfo.FldName.ToUpper() == fldname.ToUpper())
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (napasInfo.Position == "R")
                        {
                            result = (value.Length == napasInfo.MaxLength) ? value : value.PadRight(napasInfo.MaxLength, Char.Parse(napasInfo.DefaultValue.ToString()));

                        }
                        else
                        {
                            result = (value.Length == napasInfo.MaxLength) ? value : value.PadLeft(napasInfo.MaxLength, Char.Parse(napasInfo.DefaultValue.ToString()));
                        }
                    }
                    else
                    {
                        if (napasInfo.DefaultValue.Length == napasInfo.MaxLength)
                        {
                            result = napasInfo.DefaultValue;
                        }
                        else
                        {
                            for (var i = 0; i < napasInfo.MaxLength; i++)
                            {
                                sb.Append(napasInfo.DefaultValue);
                            }
                            result = sb.ToString();
                        }
                    }
                }
            }
            return result;
        }
        public  string GetCheckSum(string value)
        {
            CheckMD5 objMD5 = new CheckMD5();
            //return objMD5.getMd5Hash(value);
            return objMD5.GetCS(value, "970421");
        }

        public void ReadFileJson(string fileName)
        {
            string jsonData = System.IO.File.ReadAllText(fileName);
            napasInfos = JsonConvert.DeserializeObject<List<NapasInfo>>(jsonData);
        }
        public DataTable GetData()
        {
            try
            {
                var values = new List<string>();
                DataTable dt;

                OracleHelper.FillDataTable(App.Configs.ConnectionString, "get_data_napas", out dt, values.ToArray());
                return dt;
            }
            catch
            {
                return null;
            }
        }
        public DataTable GetDataBBB()
        {
            try
            {
                var values = new List<string>();
                DataTable dt;

                OracleHelper.FillDataTable(App.Configs.ConnectionString, "get_data_napas_bbb", out dt, values.ToArray());
                return dt;
            }
            catch
            {
                return null;
            }
        }
    }
}
