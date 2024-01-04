using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using UNITEX_DOCUMENT_SERVICE.Model.UNITEX;

namespace UNITEX_DOCUMENT_SERVICE.Code
{
    public static class GestoreMail
    {
        public static bool InvioEsternoAbilitato = true;
        //private static string MailsDev = "p.disa@xcmhealthcare.com,r.ninno@xcmhealthcare.com,c.mazzone@xcmhealthcare.com";
        private static string MailsDev = "c.mazzone@xcmhealthcare.com";
        
        //!UnitEx-IT.Password@@ Claudio

        private static string DevMail = "itsupport@unitexpress.it";
        private static string DevMailPassword = "!UnitEx-IT.Password@@";
        private static string DevMailName = "Unitex";
        private static string DevSmtpHost = "smtp.gmail.com";
        private static int DevSmtpPort = 587;

        private static string SenderMail = "itsupport@unitexpress.it";
        private static string SenderMailPassword = "!UnitEx-IT.Password@@";
        private static string SenderMailName = "Unitex";
        private static string SenderSmtpHost = "smtp.gmail.com";
        private static int SenderSmtpPort = 587;

        private static Exception LastException = new Exception();

        public static void SendMail(List<string> daInviare, string mailTo, string subject, string body)
        {

            //mailTo = "d.valitutti@xcmhealthcare.com";

            var fromAddress = new MailAddress(SenderMail, SenderMailName);

            try
            {
                var mails = mailTo.Split(',').ToList();

                foreach (var mail in mails)
                {
                    if (!string.IsNullOrWhiteSpace(mail))
                    {
                        var toa = new MailAddress(mail);
                        var smtp = new SmtpClient
                        {
                            Host = SenderSmtpHost,
                            Port = SenderSmtpPort,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(SenderMail, SenderMailPassword)
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
                    }

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

            //foreach (var ta in toAddress)
            //{
            //if (!string.IsNullOrWhiteSpace(ta))
            {
                var toa = new MailAddress(toAddress[1]);
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
                message.CC.Add(toAddress[0]);
                smtp.Send(message);
            }
            //}
        }
        public static void SegnalaErroreDev(string msg, Exception ee)
        {
            var fromAddress = new MailAddress(DevMail, DevMailName);
            string subject = $"{msg}";
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

        internal static void SegnalaInserimentoSpondaIdraulica(UnitexDefaultShipmentHeader standardCSV)
        {
            var fromAddress = new MailAddress(DevMail, DevMailName);
            string subject = $"Inserimento automatico sponda idraulica {standardCSV.externalRef}";
            string body = $"si notifica che è stata inserita in modo automatico la sponda idraulica per la seguente spedizione:\r\nRif. Cliente - {standardCSV.externalRef}\r\nMandante - {standardCSV.InsideRef} {standardCSV.LoadName}\r\nDestinatario - {standardCSV.UnloadName}";

            var toa = new MailAddress("contabilitamelzo@unitexpress.it");
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
