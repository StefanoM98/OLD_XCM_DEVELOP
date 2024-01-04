using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static CommonAPITypes.ESPRITEC.EspritecEDIMessage;

namespace IRENA_ERIS_DOCUMENT_SERVICE
{
    public static class GestoreMail
    {
        public static bool InvioEsternoAbilitato = true;
        private static string MailsTo = "customercare@xcmhealthcare.com";
        //private static string MailsCC = "a.ianniello@xcmhealthcare.com,r.ninno@xcmhealthcare.com";
        private static string MailsCC = "a.ianniello@xcmhealthcare.com,c.mazzone@xcmhealthcare.com";
        //private static string MailsDev = "r.ninno@xcmhealthcare.com,p.disa@xcmhealthcare.com";
        private static string MailsDev = "c.mazzone@xcmhealthcare.com";

        public static void SegnalaAlCustomerCustom(string obj, string body)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = obj;

            var CC = MailsCC.Split(',').ToList();

            MailAddressCollection CarbonCopy = new MailAddressCollection();

            foreach (var item in CC)
            {
                CarbonCopy.Add(item);
            }

            var toa = new MailAddress(MailsTo);
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("itsupport@xcmhealthcare.com", fromPassword)
            };

            var message = new MailMessage(fromAddress, toa);

            message.Subject = subject;
            message.Body = body;

            //Per ogni Indirizzo Email presente in carbon copy aggiungi alla proprieta CC di MailMessage
            foreach (var item in CarbonCopy)
            {
                message.CC.Add(item);
            }

            smtp.Send(message);

        }
        public static void SegnalaErroreDevRob(string msg, Exception ee)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"Errore service manager {msg}";

            string body = $"Errore in {msg}:\r\n{ee}";
            var toAddress = MailsDev.Split(',').ToList();
            foreach (var ta in toAddress)
            {
                if (!string.IsNullOrWhiteSpace(ta))
                {
                    var toa = new MailAddress(ta);
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("itsupport@xcmhealthcare.com", fromPassword)
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
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager!";
            string subject = $"Errore service manager {msg}";

            var toAddress = MailsDev.Split(',').ToList();
            foreach (var ta in toAddress)
            {
                if (!string.IsNullOrWhiteSpace(ta))
                {
                    var toa = new MailAddress(ta);
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("itsupport@xcmhealthcare.com", fromPassword)
                    };

                    var message = new MailMessage(fromAddress, toa);

                    message.Subject = subject;
                    message.Body = body;
                    smtp.Send(message);

                }
            }
        }
    }
}
