using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text;

namespace Models.Common
{
    public class ErrorUtils
    {
        public static void WriteLog(string errorDescription)
        {
            StreamWriter streamWriter = null;
            string fileName = string.Empty;
            try
            {
                fileName = string.Format("{0:ddMMyyyy}", DateTime.Now) + "Error.txt";
                fileName = Directory.GetCurrentDirectory().ToString() + "\\" + fileName;
                if(!File.Exists(fileName))
                {
                    streamWriter = File.CreateText(fileName);
                }
                else
                {
                    streamWriter = File.AppendText(fileName);
                }
                streamWriter.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                streamWriter.WriteLine(errorDescription + "\n");
                streamWriter.Write(streamWriter.NewLine);
            }
            finally
            {
                streamWriter.Flush();

                streamWriter.Close();
            }
        }
        
    }
}
