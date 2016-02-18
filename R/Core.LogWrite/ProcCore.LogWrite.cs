using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using ProcCore.NetExtension;

namespace ProcCore //Log記錄除錯 放在名命空間第一層 以利除錯方便使用
{
    public static class Log
    {
        public static string SetupBasePath { get; set; }
        public static bool Enabled { get; set; }
        private static Queue<string> QueueMessage = new Queue<string>();

        #region Write Method Function
        public static void WriteToFile()
        {
            if (QueueMessage.Count > 0)
            {
                FileStream fs = null;
                StreamWriter sw = null;
                try
                {
                    if (SetupBasePath == null) SetupBasePath = "D:\\";
                    string FileName = "Log." + DateTime.Now.ToString("yyyyMMddHH") + ".txt";

                    fs = File.Open(SetupBasePath + FileName, FileMode.Append);
                    sw = new StreamWriter(fs, Encoding.UTF8);
                    while (QueueMessage.Count > 0)
                    {
                        string dq = QueueMessage.Dequeue();
                        sw.WriteLine(dq);
                    }
                    sw.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (fs != null)
                        fs.Dispose();

                    if (sw != null)
                        sw.Dispose();
                }
            }
        }
        public static void Write(string message)
        {
            if (Enabled)
                QueueMessage.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "=>" + message);
        }
        public static void Write(params string[] message)
        {
            Write(string.Join(",", message));
        }
        #endregion

    }
}
