using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MassiveMailSender.Code
{
    public class Helper
    {
        HtmlAgility ha = new HtmlAgility();
        public string firmaMailFileName = "FirmaMail.html";
        public string appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MassiveMailSender");

        public bool MailIsValid(string mail)
        {
            var rx = "^\\S+@\\S+\\.\\S+$";
            return Regex.IsMatch(mail, rx);
        }


        public string ReplaceSrcImage(string htmlText)
        {
            var response = ha.ReplaceContentOfImgSrc(htmlText, GestoreMail.GetSrcImage());
            return response;
        }

        public string ValidateBody(string body)
        {
            var signature = File.ReadAllText(Path.Combine(appDataDir, firmaMailFileName));

            var signatureStyle = ha.GetContentOfTag(signature, "style");

            body = ha.AppendTextInTag(body, "style", signatureStyle);

            signature = ha.GetContentOfTag(signature, "body");

            var response = ha.AppendTextInTag(body, "body", signature);

            return response;

        }

        public string ReadOutlookSignature()
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
            string signature = string.Empty;
            DirectoryInfo diInfo = new DirectoryInfo(appDataDir);

            if (diInfo.Exists)
            {
                FileInfo[] fiSignature = diInfo.GetFiles("*.htm");

                if (fiSignature.Length > 0)
                {
                    StreamReader sr = new StreamReader(fiSignature[0].FullName, Encoding.Default);
                    signature = sr.ReadToEnd();

                    if (!string.IsNullOrEmpty(signature))
                    {
                        string fileName = fiSignature[0].Name.Replace(fiSignature[0].Extension, string.Empty);
                        signature = signature.Replace(fileName + "_file/", appDataDir + "/" + fileName + "_file/");
                    }
                }
            }
            return signature;
        }
    }
}
