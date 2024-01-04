using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace UnitexRemoteClient
{
    public class GestoreMail
    {
        List<string> MailsCust = new List<string>() { "ritirimelzo@unitexpress.it", "customercare@unitexpress.it" };
        public void SegnalaRichiestaRitiro(string tpMail, RootobjectNewShipmentTMS shipmentTMS)
        {
            var fromAddress = new MailAddress("itsupport@unitexpress.it", "UNITEX");
            DateTime dataRitiro = DateTime.Parse(shipmentTMS.stops[0].date);
            var dtTs = dataRitiro.ToString("dd/MM/yyyy");
            string obj = $"Ritiro {dtTs}";
            string body =
$@"Si richiede un ritiro in data {dtTs} presso il seguente indirizzo:

{shipmentTMS.stops[0].description}
{shipmentTMS.stops[0].address}
{shipmentTMS.stops[0].zipCode} - {shipmentTMS.stops[0].location} - {shipmentTMS.stops[0].district}

Persona di riferimento:
{shipmentTMS.stops[0].contactName} - Tel: {shipmentTMS.stops[0].contactPhone}

Mail generata automaticamente, si prega di non rispondere a questa mail, o escludendo itsupport@unitexpress.it dalla risposta. 
Per qualsiasi informazione potete contattare ritirimelzo@unitexpress.it e/o customercare@unitexpress.it

";

            string fromPassword = "!UnitEx-IT.Password@";
            string subject = obj;


            if (!string.IsNullOrWhiteSpace(tpMail))
            {
                var toa = new MailAddress(tpMail);

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("itsupport@unitexpress.it", fromPassword)
                };

                var message = new MailMessage(fromAddress, toa);
                MailAddress copy1 = new MailAddress("ritirimelzo@unitexpress.it");
                MailAddress copy2 = new MailAddress("customercare@unitexpress.it");
                message.CC.Add(copy1);
                message.CC.Add(copy2);
                message.Subject = subject;
                message.Body = body;
                smtp.Send(message);

            }

        }
    }
}
