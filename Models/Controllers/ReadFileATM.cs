using Models.Base;
using Models.Common;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Models.Controllers
{
    public class ReadFileATM : ControllerBase
    {
        public static List<TransInfo> ReadFileJRN(string fileName)
        {
            Boolean bCheck = false;
            string[] lines = System.IO.File.ReadAllLines(fileName);

            string fileNameresult = Path.GetFileNameWithoutExtension(fileName);

            TransInfo transInfo = new TransInfo();
            List<TransInfo> listTransInfo = new List<TransInfo>();
            foreach (var line in lines)
            {
                if (line.Contains(Constants.TRANSACTION_START_JRN))
                {
                    transInfo = new TransInfo();
                    transInfo.TRANSACTION_START = CommonUtils.ProcessString(line).Substring(0, CommonUtils.ProcessString(line).Length - 3);
                    transInfo.TRANSACTION_DATE = fileNameresult;
                    bCheck = true;
                }
                if (bCheck)
                {
                    if (line.Contains(Constants.TRACK_2_DATA)) transInfo.CARD_NUMBER = CommonUtils.ProcessString(line).Substring(CommonUtils.ProcessString(line).Length - 16, 16);
                    //if (line.Contains(Constants.CARD_TAKEN)) transInfo.CARD_TAKEN = CommonUtils.ProcessString(line);
                    if (line.Contains(Constants.CASH_REQUEST)) transInfo.CASH_REQUEST = CommonUtils.ProcessString(line);
                    if (line.Contains(Constants.CASH) && !line.Contains(Constants.CASH_PRESENTED) && !line.Contains(Constants.CASH_TAKEN) && !line.Contains(Constants.CASH_REQUEST))
                    {
                        transInfo.CASH = CommonUtils.ProcessString(line).Substring(8, CommonUtils.ProcessString(line).Length - 8).Trim();
                    };
                    if (line.Contains(Constants.CASH_PRESENTED)) transInfo.CASH_PRESENTED = CommonUtils.ProcessString(line);
                    if (line.Contains(Constants.CASH_TAKEN)) transInfo.CASH_TAKEN = CommonUtils.ProcessString(line);
                    if (line.Contains(Constants.AMOUNT)) transInfo.AMOUNT = CommonUtils.ProcessString(line);

                    if (line.Contains(Constants.TRANSACTION_STOP_JRN))
                    {
                        transInfo.TRANSACTION_END = CommonUtils.ProcessString(line).Substring(0, CommonUtils.ProcessString(line).Length - 3);
                        listTransInfo.Add(transInfo);
                        bCheck = false;
                    }
                    if (line.Contains(Constants.CASH_RETRACTED))
                    {
                        transInfo.CASH_RETRACTED = Constants.YES;
                    }
                    else
                    {
                        transInfo.CASH_RETRACTED = Constants.NO;
                    }
                    transInfo.TRANSACTION_DATE = fileNameresult;
                }

            }

            List<string> values = new List<string>();
            return listTransInfo;
        }


        public static List<TransInfo> ReadFileEJDATA(string fileName)
        {
            Boolean bCheck = false;
            string[] lines = System.IO.File.ReadAllLines(fileName);
            TransInfo transInfo = new TransInfo();
            List<TransInfo> listTransInfo = new List<TransInfo>();
            foreach (var line in lines)
            {
                if (line.Trim() == Constants.TRANSACTION_START)
                {
                    transInfo = new TransInfo();
                    bCheck = true;
                }
                if (bCheck)
                {
                    if (line.Contains(Constants.CARD_INSERTED)) transInfo.CARD_INSERTED = CommonUtils.ProcessString(line);
                    if (line.Contains(Constants.CARD_NUMBER)) transInfo.CARD_NUMBER = CommonUtils.ProcessString(line);
                    if (line.Contains(Constants.CUSTOMER_NAME)) transInfo.CUSTOMER_NAME = CommonUtils.ProcessString(line);
                    if (line.Contains(Constants.PIN_ENTERED)) transInfo.PIN_ENTERED = CommonUtils.ProcessString(line);
                    if (line.Contains(Constants.TRANSACTION_TYPE)) transInfo.TRANSACTION_TYPE = CommonUtils.ProcessString(line);
                    if (line.Contains(Constants.AMOUNT)) transInfo.AMOUNT = CommonUtils.ProcessString(line);
                    if (line.Contains(Constants.CARD_TAKEN)) transInfo.CARD_TAKEN = CommonUtils.ProcessString(line);
                    if (line.Contains(Constants.NOTES_PRESENTED)) transInfo.NOTES_PRESENTED = CommonUtils.ProcessString(line);
                    if (line.Contains(Constants.NOTES_RETRACTED))
                    {
                        transInfo.NOTES_RETRACTED = Constants.YES;
                    }
                    else
                    {
                        transInfo.NOTES_RETRACTED = Constants.NO;
                    }

                    if (line.Contains(Constants.NOTES_TAKEN)) transInfo.NOTES_TAKEN = CommonUtils.ProcessString(line);

                    if (line.Contains(Constants.TRANSACTION_END))
                    {
                        transInfo.TRANSACTION_END = CommonUtils.ProcessString(line);
                        listTransInfo.Add(transInfo);
                        bCheck = false;
                    }
                }

            }

            List<string> values = new List<string>();
            return listTransInfo;
        }
    }
}
