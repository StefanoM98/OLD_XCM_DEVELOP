using MassiveMailSender.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace MassiveMailSender.Code
{
    public static class GestoreMail
    {
        public static bool InvioEsternoAbilitato = true;
        //private static string MailsDev = "p.disa@xcmhealthcare.com,d.valitutti@xcmhealthcare.com";
        private static string MailsDev = "c.mazzone@xcmhealthcare.com";

        private static string DevMail = "itsupport@unitexpress.it";
        private static string DevMailPassword = "!UnitEx-IT.Password@";
        private static string DevMailName = "Unitex";
        private static string DevSmtpHost = "smtp.gmail.com";
        private static int DevSmtpPort = 587;

        private static Exception LastException = new Exception();
        private static MailSettings _mailSettings = null;

        public static void ConfigureMail(MailSettings mailSettings)
        {
            _mailSettings = mailSettings;
        }

        public static void SendMail(List<string> daInviare, string mailTo, string subject, string body)
        {
#if DEBUGi
            mailTo = "d.valitutti@xcmhealthcare.com";
#endif
            var fromAddress = new MailAddress(_mailSettings.SenderMail, _mailSettings.SenderMailName);

            try
            {
                if (!string.IsNullOrWhiteSpace(mailTo))
                {
                    var toa = new MailAddress(mailTo);
                    var smtp = new SmtpClient
                    {
                        Host = _mailSettings.SenderSmtpHost,
                        Port = _mailSettings.SenderSmtpPort,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(_mailSettings.SenderMail, _mailSettings.SenderMailPassword)
                    };

                    var message = new MailMessage(fromAddress, toa);

                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    foreach (var da in daInviare)
                    {
                        message.Attachments.Add(new Attachment(da));
                    }

                    smtp.Send(message);
                    Form1.progressBarValue++;

                }
            }
            catch (Exception ee)
            {
                if (ee.Message != LastException.Message)
                {
                    SegnalaErroreDev(ee);
                    LastException = ee;
                }

            }

        }
        public static void SegnalaErroreDev(Exception ee)
        {
            var fromAddress = new MailAddress(DevMail, DevMailName);
            string subject = $"Errore Massive Mail Sender - {ee.Message}";
            string body = $"{ee}";
            var toAddress = MailsDev.Split(',').ToList();

            foreach (var ta in toAddress)
            {
                if (!string.IsNullOrWhiteSpace(ta))
                {
                    var toa = new MailAddress(ta);
                    var smtp = new SmtpClient
                    {
                        Host = DevSmtpHost,
                        Port = DevSmtpPort,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(DevMail, DevMailPassword)
                    };

                    var message = new MailMessage(fromAddress, toa);

                    message.Subject = subject;
                    message.Body = body;
                    smtp.Send(message);
                }
            }
        }
        public static void SegnalaErroreDev(string msg, Exception ee)
        {
            var fromAddress = new MailAddress(DevMail, DevMailName);
            string subject = $"Errore Massive Mail Sender {msg}";
            string body = $"Errore in {msg}:\r\n{ee}";
            var toAddress = MailsDev.Split(',').ToList();

            foreach (var ta in toAddress)
            {
                if (!string.IsNullOrWhiteSpace(ta))
                {
                    var toa = new MailAddress(ta);
                    var smtp = new SmtpClient
                    {
                        Host = DevSmtpHost,
                        Port = DevSmtpPort,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(DevMail, DevMailPassword)
                    };

                    var message = new MailMessage(fromAddress, toa);

                    message.Subject = subject;
                    message.Body = body;
                    smtp.Send(message);
                }
            }
        }
        public static void SegnalaErroreDev(string msg, string body)
        {
            var fromAddress = new MailAddress(DevMail, DevMailName);
            string subject = $"Errore Massive Mail Sender {msg}";
            var toAddress = MailsDev.Split(',').ToList();

            foreach (var ta in toAddress)
            {
                if (!string.IsNullOrWhiteSpace(ta))
                {
                    var toa = new MailAddress(ta);
                    var smtp = new SmtpClient
                    {
                        Host = DevSmtpHost,
                        Port = DevSmtpPort,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(DevMail, DevMailPassword)
                    };

                    var message = new MailMessage(fromAddress, toa);

                    message.Subject = subject;
                    message.Body = body;
                    smtp.Send(message);
                }
            }
        }
        public static bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static string GetSrcImage()
        {
            var response = "";
            if (_mailSettings.SenderMail.Split('@')[1] == "unitexpress.it")
            {
                response = @"https://unitexpress.it/wp-content/uploads/2022/03/unitex_logo_200-54x47.png";

            }
            else if (_mailSettings.SenderMail.Split('@')[1] == "xcmhealthcare.com")
            {
                response = @"https://xcmhealthcare.com/wp-content/uploads/2020/10/cropped-4b-per-mirko-sito-09.png";

            }
            return response;
        }

        #region HtmlParserOLD
        public static string ReplaceSrcImage(string htmlText)
        {
            //TODO: non va bene per vari casi
            var response = "";
            if (htmlText.Contains("img"))
            {
                int imgPosition = htmlText.LastIndexOf("img");
                int startPosition = htmlText.IndexOf("src", imgPosition);
                int endPosition = htmlText.IndexOf("\"", startPosition + 5);

                var countRemove = endPosition - startPosition - 5;

                //srcPosition = srcPosition + 5;
                htmlText = htmlText.Remove(startPosition + 5, countRemove);

                if (_mailSettings.SenderMail.Split('@')[1] == "unitexpress.it")
                {
                    response = htmlText.Insert(startPosition + 5, @"https://unitexpress.it/wp-content/uploads/2022/03/unitex_logo_200-54x47.png");

                }
                else if (_mailSettings.SenderMail.Split('@')[1] == "xcmhealthcare.com")
                {
                    response = htmlText.Insert(startPosition + 5, @"https://xcmhealthcare.com/wp-content/uploads/2020/10/cropped-4b-per-mirko-sito-09.png");

                }

            }
            return response;
        }

        public static string ExtractBodyOfHtmlDocument(string body)
        {
            var response = "";
            if (body.Contains("body"))
            {
                //body = GestoreMail.ReplaceSrcImage(body);

                int bodyPosition = body.IndexOf("body");
                int closeBody = body.LastIndexOf("body");

                response = body.Substring(bodyPosition + 5, closeBody - bodyPosition - 7);
            }
            return response;
        }

        public static string InsertSignature(string body, string sign)
        {
            var response = "";
            int lastPosition = body.LastIndexOf("body");

            body = body.Remove(lastPosition - 2, body.Count() - lastPosition);
            response = body.Insert(lastPosition, $"{sign}</body></html>");
            return response;

        }

        public static string StyleTagExtract(string htmlText)
        {
            var response = "";
            if (htmlText.Contains("style"))
            {

                int startPosition = htmlText.IndexOf("style");
                startPosition = htmlText.IndexOf(">", startPosition)+2;
                int endPosition = htmlText.IndexOf("/style", startPosition);
                response = htmlText.Substring(startPosition, endPosition - startPosition - 1);
            }
            return response.Trim();
        }

        public static string StyleTagInsert(string htmlText, string styleContent)
        {
            var response = "";
            if (htmlText.Contains("style"))
            {

                int startPosition = htmlText.IndexOf("style");
                startPosition = htmlText.IndexOf(">", startPosition) + 2;


                response = htmlText.Insert(startPosition+1, styleContent);
            }
            return response;
        }
        #endregion
    }
}
