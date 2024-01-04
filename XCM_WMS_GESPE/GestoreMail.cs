using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace XCM_WMS_GESPE
{
    public static class GestoreMail
    {
        public static void SendMailGiacenzeMagazzino(string pathFile, string mailTo)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"Giacenze di magazzino XCM Healthcare al {DateTime.Now.ToString("dd/MM/yyyy")}";
            string body =
@"Gentile Cliente,
in allegato quanto in oggetto.
Nel file sono elencati in ordine alfabetico gli articoli presenti  nel magazzino XCM Healthcare alla data di oggi.
Si evidenzia che la colonna 'scadenza alla vendita' indica i giorni entro i quali il prodotto può essere evaso tenuto conto dei giorni di shelflife da Lei indicati;
la colonna 'scadenza effettiva' indica invece i giorni residui del prodotto alla data di scadenza indicata sulla confezione.

Il file rappresenta una fotografia istantanea del magazzino con gli articoli in linea;
eventuali articoli presenti negli ordini in evasione non sono elencati nel file: alle giacenze in allegato sono già sottratti gli articoli in uscita oggi di cui Le arriveranno i ddt in giornata.

La presente e-mail è stata generata automaticamente da un indirizzo di posta elettronica di solo invio; si chiede pertanto di non rispondere al messaggio.

Si porgono distinti saluti
";


            var toAddress = mailTo.Split(',').ToList();
            foreach(var ta in toAddress)
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
                    message.Attachments.Add(new Attachment(pathFile));

                    smtp.Send(message);
#if DEBUG
                    break;
#endif
                }
            }

            

        }
    }
}
