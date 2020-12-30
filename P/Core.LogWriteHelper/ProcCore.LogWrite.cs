using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using ProcCore.NetExtension;

namespace ProcCore
{
    public static class Log
    {
        public static Boolean Enabled { get; set; }
        public static String SetupBasePath { get; set; }
        private static Queue<String> QueueMessage = new Queue<String>();

        #region Write Method Function

        public static void WriteToFile()
        {
            //if (SetupBasePath == null) SetupBasePath = "D:\\";
            //String FileName = "Log." + DateTime.Now.ToString("yyyyMMddHH") + ".txt";
            //FileStream fs = File.Open(SetupBasePath + FileName, FileMode.Append);
            //StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            //while (QueueMessage.Count > 0)
            //{
            //    String dq = QueueMessage.Dequeue();
            //    sw.WriteLine(dq);
            //}
            //sw.Close();
            //sw.Dispose();
            //fs.Close();
            //fs.Dispose();
        }
        public static void Write(String message)
        {
            if (Enabled)
            {
                QueueMessage.Enqueue("<" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ">:" + message);
            }
        }

        public static void Write(LogPlamInfo pi, String className, String messages)
        {
            String strTpl = "[{0}][I:{1},{2},{3}][M:{4}]";
            Write(String.Format(strTpl,
                pi.AccountId,
                pi.IP,
                pi.BroswerInfo,
                className, messages));
        }

        public static void Write(LogPlamInfo pi, String className, params String[] message)
        {

            String strTpl = "[{0}][I:{1},{2},{3}][M:{4}]";

            String messages = string.Join(",", message);

            Write(String.Format(strTpl,
                pi.AccountId,
                pi.IP,
                pi.BroswerInfo,
                className, messages));
        }

        public static void Write(LogPlamInfo pi, String className, Exception ex)
        {
            String message = String.Format("<[{0}][{1}]>", ex.Message, ex.StackTrace);
            Write(pi, className, message);
        }
        public static void Write(LogPlamInfo pi, String className, LogicError ex)
        {
            String message = String.Format("<[{0}][{1}]>", ex.Message, ex.StackTrace);
            Write(pi, className, message);
        }
        public static void Write(LogPlamInfo pi, String className, ExceptionRoll ex)
        {
            String message = String.Format("<[{0}][{1}]>", ex.Message, ex.StackTrace);
            Write(pi, className, message);
        }
        #endregion

        public class LogPlamInfo
        {
            public Boolean AllowWrite { get; set; }
            public Int32 AccountId { get; set; }
            public String IP { get; set; }
            public String BroswerInfo { get; set; }
        }
        public enum LogMessageType
        {
            Info, Success, LogicError, SystemError
        }
    }
}
