using Models.Common;
using Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccess.Controllers
{
    public class WriteVASISS_OUT
    {
        List<NapasInfo> napasInfos = new List<NapasInfo>();
        public string BuildHeader()
        {
            string result = string.Empty;
            var sb = new StringBuilder();
            sb.Append(GetValue("HeaderRecord", null));
            sb.Append(GetValue("BIN", "970421"));
            //sb.Append(GetValue("TXDATE", DateTime.Now.ToString("MMddyy")));
            sb.Append(GetValue("TXDATE","111020" ));
            return result = sb.ToString();
        }
        public string BuildBody(string CardNo,string Amount,string TraceNo,string TranHour,string TranDate,string PayDate,string CCYCD,string FromAcc,string ToAcc)
        {
            string result = string.Empty;
            var sb = new StringBuilder();
            //sb.Append(GetValue("DetailRecord", null));
            //sb.Append(GetValue("F2", "250710000022197")); // so the
            //sb.Append(GetValue("F3", "060000")); // ma xu ly
            //sb.Append(GetValue("F4", amount.ToString().Replace(".", null))); // so tien giao dich
            //sb.Append(GetValue("F11", "174614")); //trace
            //sb.Append(GetValue("F12", DateTime.Now.ToString("HHmmss"))); // gio giao dich
            //sb.Append(GetValue("F13", DateTime.Now.ToString("MMdd"))); // ngay giao dich
            //sb.Append(GetValue("F15", DateTime.Now.ToString("MMdd"))); // ngay thanh toan
            //sb.Append(GetValue("F18", "6012")); // loai thiet bi
            //sb.Append(GetValue("F32", "970421")); // ma to chuc chap nhan the
            //sb.Append(GetValue("F38", null)); // so tham tra
            //sb.Append(GetValue("F41", "11111111")); // ma so thiet bi
            //sb.Append(GetValue("F49", "704")); //ccycd
            //sb.Append(GetValue("F102", "000000000000VTLTOPUP")); // tu so tk
            //sb.Append(GetValue("F013", "00000000000343315305")); // den so tk
            //sb.Append(GetValue("MTI", null)); // ma dinh dang thong diep
            //sb.Append(GetValue("RC", "0000")); // trang thai giao dich

            sb.Append(GetValue("DetailRecord", null));
            sb.Append(GetValue("F2", CardNo)); // so the
            sb.Append(GetValue("F3", "060000")); // ma xu ly
            sb.Append(GetValue("F4", Amount)); // so tien giao dich
            sb.Append(GetValue("F11", TraceNo)); //trace
            sb.Append(GetValue("F12", TranHour)); // gio giao dich
            sb.Append(GetValue("F13", TranDate)); // ngay giao dich
            sb.Append(GetValue("F15", PayDate)); // ngay thanh toan
            sb.Append(GetValue("F18", "7399")); // loai thiet bi
            sb.Append(GetValue("F32", "970421")); // ma to chuc chap nhan the
            sb.Append(GetValue("F38", null)); // so tham tra
            sb.Append(GetValue("F41", "11111111")); // ma so thiet bi
            sb.Append(GetValue("F49", CCYCD)); //ccycd
            sb.Append(GetValue("F102", FromAcc)); // tu so tk
            sb.Append(GetValue("F103", ToAcc)); // den so tk
            sb.Append(GetValue("MTI", null)); // ma dinh dang thong diep
            sb.Append(GetValue("RC", "0017")); // trang thai giao dich


            sb.Append(GetCheckSum(sb.ToString()));
            return result = sb.ToString();
        }
        public  string BuildFooter(string totalTrans)
        {
            string result = string.Empty;
            var sb = new StringBuilder();
            sb.Append(GetValue("TrailerRecord", null));
            sb.Append(GetValue("TOTAL_TRANS", totalTrans));
            sb.Append(GetValue("CREATED_BY", "admin"));
            sb.Append(GetValue("CREATED_HOUR", DateTime.Now.ToString("HHmmss")));
            sb.Append(GetValue("CREADTED_DATE", DateTime.Now.ToString("MMddyyyy")));
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
                            result = (value.Length == napasInfo.MaxLength) ? value : value.PadRight(napasInfo.MaxLength, Char.Parse(napasInfo.DefaultValue));

                        }
                        else
                        {
                            result = (value.Length == napasInfo.MaxLength) ? value : value.PadLeft(napasInfo.MaxLength, Char.Parse(napasInfo.DefaultValue));
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
            return objMD5.getMd5Hash(value);
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
    }
}
