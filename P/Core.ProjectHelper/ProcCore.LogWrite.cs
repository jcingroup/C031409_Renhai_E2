using System;
using System.Text;
using System.IO;

namespace ProcCore //Log記錄除錯 放在名命空間第一層 以利除錯方便使用
{
    public class LogWriteHelp
    {
        public String SetupBasePath { get; set; }
        public int AccountId { get; set; }
        public Boolean Enabled { get; set; }
        public String IP { get; set; }
        public String BroswerInfo { get; set; }

        private String FileName;

        public LogWriteHelp() {
            SetupBasePath = "D:\\";
            FileName = "MessageLog" + DateTime.Now.ToString("yyyyMMddHH") + ".txt";
        }

        public void Write(String tag, String message)
        {
            if (Enabled)
            {

                FileStream fs = File.Open(SetupBasePath + FileName, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine("[PASS][" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "][" + AccountId + "][" + IP + "][" + BroswerInfo + "][Tag:" + tag + "][" + message + "]");
                sw.Close();
                sw.Dispose();
                fs.Close();
                fs.Dispose();
            }
        }
        public void ErrWrite(String tag, Exception ex)
        {
            if (Enabled)
            {
                FileStream fs = File.Open(SetupBasePath + FileName, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine("[ERROR][" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "][" + AccountId + "][" + IP + "][" + BroswerInfo + "][Tag:" + tag + "][" + ex.Message + "][" + ex.StackTrace + "][" + ex.Source + "]");
                sw.Close();
                sw.Dispose();
                fs.Close();
                fs.Dispose();
            }
        }
    }
}
