using Models.Base;
using Models.Common;
using Models.Entities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace Models.Controllers
{
    public class ReadFileExcel: ControllerBase
    {
        public static List<Accounting> ReadExcel(string fileName)
        {
            try
            {
                List<Accounting> lstAccounting = new List<Accounting>();
                Boolean bCheck = true;
                FileInfo existingFile = new FileInfo(fileName);
                using (ExcelPackage package = new ExcelPackage(existingFile))
                {
                    //get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int colCount = worksheet.Dimension.End.Column;  //get Column Count
                    int rowCount = worksheet.Dimension.End.Row;     //get row count

                    for (int row = Constants.StartRow+1; row <= rowCount; row++)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(worksheet.Cells[row, 2].Value.ToString()))
                            {
                                bCheck = false;
                            };
                        }
                        catch(Exception ex)
                        {
                            return lstAccounting;
                        }
                       
                        if (bCheck)
                        {
                            var objAccounting = new Accounting();
                            objAccounting.Mid = worksheet.Cells[row, 2].Value?.ToString().Trim();
                            objAccounting.OrgName = worksheet.Cells[row, 3].Value?.ToString().Trim();
                            objAccounting.TxDate = worksheet.Cells[row, 4].Value?.ToString().Trim();
                            objAccounting.Acctno = worksheet.Cells[row, 5].Value?.ToString().Trim();
                            objAccounting.Qtty = worksheet.Cells[row, 6].Value?.ToString().Trim();
                            objAccounting.Amt = worksheet.Cells[row, 7].Value?.ToString().Trim();
                            objAccounting.Fee = worksheet.Cells[row, 8].Value?.ToString().Trim();
                            objAccounting.Vat = worksheet.Cells[row, 9].Value?.ToString().Trim();
                            objAccounting.OrgAmt = worksheet.Cells[row, 10].Value?.ToString().Trim();
                            objAccounting.FeePercent = worksheet.Cells[row, 11].Value?.ToString().Trim();
                            objAccounting.BranchID = worksheet.Cells[row, 12].Value?.ToString().Trim();
                            objAccounting.FeeMaster = worksheet.Cells[row, 13].Value?.ToString().Trim();
                            objAccounting.FeeJCB = worksheet.Cells[row, 14].Value?.ToString().Trim();
                            objAccounting.TranType = worksheet.Cells[row, 15].Value?.ToString().Trim();
                            objAccounting.CifNo = worksheet.Cells[row, 16].Value?.ToString().Trim();
                            objAccounting.TranDate = worksheet.Cells[row, 17].Value?.ToString().Trim();
                            objAccounting.Description = worksheet.Cells[row, 18].Value?.ToString().Trim();
                            objAccounting.Parameters = worksheet.Cells[row, 19].Value?.ToString().Trim();
                            objAccounting.CardNo = worksheet.Cells[row, 20].Value?.ToString().Trim();
                            objAccounting.FeeAndVat = worksheet.Cells[row, 21].Value?.ToString().Trim();
                            objAccounting.Currency = worksheet.Cells[row, 22].Value?.ToString().Trim();
                            objAccounting.PosEximNo = worksheet.Cells[row, 23].Value?.ToString().Trim();
                            objAccounting.PosVrbNo = worksheet.Cells[row, 24].Value?.ToString().Trim();

                            lstAccounting.Add(objAccounting);
                        }
                    }
                }

                return lstAccounting;
            }
            catch(Exception ex)
            {
                ErrorUtils.WriteLog(ex.Message);
                return null;
            }
          
        }
    }
}
