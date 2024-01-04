using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace PackageMonitoringXCM.Code
{
    public class LoggerCustom
    {
        private string projectPath = Directory.GetCurrentDirectory();

        private string path = @"C:\LOGS\MONITOR_CARTONI\error.txt";
        
        public void Info(string message)
        {
            if (File.Exists(path))
            {
                File.Create(path);
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INFO: " + DateTime.Now.ToLocalTime().ToString("G"));
            sb.AppendLine("Source File: " + System.Web.HttpContext.Current.Request.RawUrl);
            sb.AppendLine("Message: " + message);
            sb.AppendLine("------------------------------------------------------------" + Environment.NewLine);
            File.AppendAllText(this.path, sb.ToString());
            
        }

        public void Error(Exception exception)
        {
            if (File.Exists(path))
            {
                File.Create(path);
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ERROR: " + DateTime.Now.ToLocalTime().ToString("G"));
            sb.AppendLine("Source File: " + System.Web.HttpContext.Current.Request.RawUrl);
            GetExceptionInfo(exception, sb);
            sb.AppendLine("------------------------------------------------------------" + Environment.NewLine);
            File.AppendAllText(this.path, sb.ToString());
        }

        private static void GetExceptionInfo(Exception exception, StringBuilder sb)
        {

            sb.AppendLine(exception.GetType().ToString());
            sb.AppendLine(exception.Message);
            sb.AppendLine("Stack Trace: ");
            sb.AppendLine(exception.StackTrace);
            if (exception.InnerException != null)
            {
                sb.AppendLine("InnerException: ");
                GetExceptionInfo(exception.InnerException, sb);
            }
        }
    }
}