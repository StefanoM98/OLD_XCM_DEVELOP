using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GreenPassValidator
{
    public static class GestoreMail
    {
        //private static string DelegatiControlloGreenPass = "a.panico@xcmhealthcare.com, magazzino@xcmhealthcare.com, p.disa@xcmhealthcare.com, gaetano.colella@xcmhealthcare.com";
        private static string ReferentiSegnalazioneAssenze = "gaetano.colella@xcmhealthcare.com,p.disa@xcmhealthcare.com,d.iorio@xcmhealthcare.com";
        private static string MailsDev = "p.disa@xcmhealthcare.com";
        //public static void SendMailInvalidGreenPass(string nome, string cognome, string dataNascita)
        //{
        //    //var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "Supporto IT XCM");
        //    //var toAddress = new MailAddress(DelegatiControlloGreenPass);

        //    const string fromPassword = "AdminIT.Manager";
        //    const string subject = " Green Pass esito negativo";
        //    string body = $"Si notifica che {cognome} {nome} - {dataNascita} ha effettuato accesso in azienda senza un green pass valido";

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential("itsupport@xcmhealthcare.com", fromPassword)
        //    };
        //    using (var message = new MailMessage("itsupport@xcmhealthcare.com", DelegatiControlloGreenPass)
        //    {
        //        Subject = subject,
        //        Body = body
        //    })
        //    {
        //        smtp.Send(message);
        //    }
        //}

        internal static void SendReportAssenzeDigitali(List<Anagrafica> assenti)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"Assenze digitali del {DateTime.Now.ToString("dd/MM/yyyy")}";
            string assentis = "";

            foreach(var ass in assenti)
            {
                assentis += $"{ass.NOME} {ass.COGNOME} - {ass.DATA_NASCITA.ToString("dd/MM/yyyy")}" + "\r\n";
            }

            string body =
$@"Si notifica che i seguenti dipendenti non hanno utilizzato il terminale per l'accesso in azienda:
{"\r\n"+assentis}
";
            var toAddress = ReferentiSegnalazioneAssenze.Split(',').ToList();
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

#if DEBUG
                    toa = new MailAddress("p.disa@xcmhealthcare.com");
#endif

                    var message = new MailMessage(fromAddress, toa);

                    message.Subject = subject;
                    message.Body = body;
                    smtp.Send(message);
#if DEBUG
                    break;
#endif
                }
            }
        }
        internal static void SegnalaErroreDev(Exception ee)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"Assenze digitali del {DateTime.Now.ToString("dd/MM/yyyy")}";


            string body = $"Errore in Controllo accessi:\r\n{ee}";
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
        internal static void SegnalaErroreDev(string subject, string body)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
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
