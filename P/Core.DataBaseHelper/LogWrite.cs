using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LogWriteHelp
{
    public static class LogWrite
    {
        public static String SetupBasePath { get; set; }
        public static int AccountId { get; set; }
        public static Boolean Enabled { get; set; }
        public static String IP { get; set; }

        public static void Write(String tag, String message)
        {
            if (Enabled)
            {
                if (SetupBasePath == null) SetupBasePath = "C:\\";
                String FileName = "MessageLog" + DateTime.Now.ToString("yyyyMMddHH") + ".txt";
                FileStream fs = File.Open(SetupBasePath + FileName, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine("[" +DateTime.Now.ToString() + "][" + AccountId + "][" + IP + "][Tag:" + tag + "][" + message + "]");
                sw.Close();
                sw.Dispose();
                fs.Close();
                fs.Dispose();
            }
        }

        public static void ErrWrite(String tag, Exception ex)
        {
            if (Enabled)
            {
                if (SetupBasePath == null) SetupBasePath = "C:\\";
                String FileName = "ErrorLog" + DateTime.Now.ToString("yyyyMMddHH") + ".txt";
                FileStream fs = File.Open(SetupBasePath + FileName, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine("[" + DateTime.Now.ToString() + "][" + AccountId + "][" + IP + "][Tag:" + tag + "][" + ex.Message + "][" + ex.StackTrace + "][" + ex.Source + "]");
                sw.Close();
                sw.Dispose();
                fs.Close();
                fs.Dispose();
            }
        }
    }
}
