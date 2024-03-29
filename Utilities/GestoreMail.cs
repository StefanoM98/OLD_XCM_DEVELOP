﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Utilities
{
    public static class GestoreMailMandanti
    {        
        public static void SendMailBEM(List<string> daInviare, string mailTo)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"Notifica Ingresso Merci";
            string body =
@"Gentile Cliente,
in allegato troverà il documento d'ingresso merci a magazzino per suo conto. 
La merce è stata già allocata ed è quindi immediatamente disponibile per un ordine di vendita.

si richiede di non rispondere alla presente e-mail in quanto generata automaticamente da un indirizzo di posta elettronica di solo invio,
nel caso contattare il servizio clienti alla mail customercare@xcmhealthcare.com

Si porgono distinti saluti
";

            List<string> toAddress = mailTo.Split(',').ToList();
            

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

                    foreach (var da in daInviare)
                    {
                        message.Attachments.Add(new Attachment(da));
                    }
                    smtp.Send(message);
                }
            }
        }
        public static void SendMailORDINE_DAFNE(string daInviare)
        {
            var fromAddress = "itsupport@xcmhealthcare.com";

            string fromPassword = "AdminIT.Manager";
            string subject = $"Notifica Ordine DAFNE";
            string body =
@"Gentile Cliente,
in allegato troverà il dettaglio dell'ordine ricevuto tramite piattaforma dafne. 
Attendiamo conferma per poterlo evadere.

Si porgono distinti saluti
";


            var toa = "plaraia@a-ps.it,rvariale@a-ps.it,diuso@a-ps.it,mmassa@a-ps.it,fkonica@a-ps.it";
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
            message.CC.Add(GestoreMailCustomer.MailsCust);
            message.Attachments.Add(new Attachment(daInviare));
            smtp.Send(message);
        }
        public static void SendMailGiacenzeMagazzino(string pathFile, string mailTo, string periodo)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"Giacenze di magazzino XCM Healthcare al {periodo}";
            string body =
@"Gentile Cliente,
in allegato quanto in oggetto.
Nel file sono elencati in ordine alfabetico i prodotti in giacenza nel magazzino.

si richiede di non rispondere alla presente e-mail in quanto generata automaticamente da un indirizzo di posta elettronica di solo invio,
nel caso contattare il servizio clienti alla mail customercare@xcmhealthcare.com

Si porgono distinti saluti
";

            List<string> toAddress = mailTo.Split(',').ToList();
            

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
                    message.Attachments.Add(new Attachment(pathFile));

                    smtp.Send(message);
                }
            }
        } 
        public static void SendMailGiacenzeMagazzinoEMovimentazioni(string[] pathFile, string mailTo, string periodo)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"Giacenze attuali e movimenti magazzino XCM Healthcare {periodo}";
            string body =
@"Gentile Cliente,
in allegato quanto in oggetto.
Nei file sono elencati in ordine di data gli articoli movimentati nel periodo in oggetto e le giacenze attuali di magazzino.

si richiede di non rispondere alla presente e-mail in quanto generata automaticamente da un indirizzo di posta elettronica di solo invio,
nel caso contattare il servizio clienti alla mail customercare@xcmhealthcare.com

Si porgono distinti saluti
";
            List<string> toAddress = mailTo.Split(',').ToList();
            
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
                    foreach (var p in pathFile)
                    {
                        message.Attachments.Add(new Attachment(p));
                    }
                    smtp.Send(message);
                }
            }
        }
        internal static void SendMailMovimentiMagazzino(string finalDest, string mailTo, string periodo)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"Movimenti magazzino XCM Healthcare di {periodo}";
            string body =
$@"Gentile Cliente,
in allegato quanto in oggetto.
Nel file sono elencati in ordine di data gli articoli movimentati nel periodo {periodo}.

si richiede di non rispondere alla presente e-mail in quanto generata automaticamente da un indirizzo di posta elettronica di solo invio,
nel caso contattare il servizio clienti alla mail customercare@xcmhealthcare.com

Si porgono distinti saluti
";

            List<string> toAddress = mailTo.Split(',').ToList();
            
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
                    message.Attachments.Add(new Attachment(finalDest));

                    smtp.Send(message);
                }
            }
        }
        internal static void SendMailMovimentiMagazzinoMeseSett(List<string> daInviare, string mailTo, string periodo)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"Giacenze attuali e movimenti magazzino XCM Healthcare di {periodo}";
            string body =
@"Gentile Cliente,
in allegato troverà i movimenti settimanali del magazzino,
i movimenti del mese scorso e l'attuale giacenza di magazzino.

si richiede di non rispondere alla presente e-mail in quanto generata automaticamente da un indirizzo di posta elettronica di solo invio,
nel caso contattare il servizio clienti alla mail customercare@xcmhealthcare.com

Si porgono distinti saluti
";

            List<string> toAddress = mailTo.Split(',').ToList();
            
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
                    foreach (var da in daInviare)
                    {
                        message.Attachments.Add(new Attachment(da));
                    }
                    smtp.Send(message);
                }
            }
        }
        internal static void SendMailMovimentiMagazzinoMeseeGiacSett(List<string> daInviare, string mailTo, string periodo)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"Giacenze attuali e movimenti magazzino XCM Healthcare di {periodo}";
            string body =
@"Gentile Cliente,
in allegato troverà i movimenti del mese scorso e l'attuale giacenza di magazzino.

si richiede di non rispondere alla presente e-mail in quanto generata automaticamente da un indirizzo di posta elettronica di solo invio,
nel caso contattare il servizio clienti alla mail customercare@xcmhealthcare.com

Si porgono distinti saluti
";

            List<string> toAddress = mailTo.Split(',').ToList();
            
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
                    foreach (var da in daInviare)
                    {
                        message.Attachments.Add(new Attachment(da));
                    }
                    smtp.Send(message);
                }
            }
        }
        internal static void InviaMailTrackingShipment(string objMail, string bodyMail, string mAIL_NOTIFICA_BEM)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            bodyMail +=
@"  
si richiede di non rispondere alla presente e-mail in quanto generata automaticamente da un indirizzo di posta elettronica di solo invio,
nel caso contattare il servizio clienti alla mail customercare@xcmhealthcare.com

Si porgono distinti saluti
";
            List<string> toAddress =  mAIL_NOTIFICA_BEM.Split(',').ToList();

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

                    message.Subject = objMail;
                    message.Body = bodyMail;
                    smtp.Send(message);
                }
            }
        }

        #region Vivisol
        public static void SegnalazioneInformaticaVivisol(string objMail, string bodyMail, List<string> MailsDiNotifica)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            foreach (var ta in MailsDiNotifica)
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

                    message.Subject = objMail;
                    message.Body = bodyMail;
                    smtp.Send(message);

                }
            }
        }     
        #endregion
    }

    public static class GestoreMailDev
    {
        //private static string MailsDev = "r.ninno@xcmhealthcare.com";
        private static string MailsDev = "c.maazzone@xcmhealthcare.com";
        public static void SegnalaErroreDev(Exception ee)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"Errore service manager {ee.Message}";


            string body = $"{ee}";
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
        public static void SegnalaErroreDev(string msg, Exception ee)
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

            string fromPassword = "AdminIT.Manager";
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

    public static class GestoreMailCustomer
    {
        internal static string MailsCust = "a.ianniello@xcmhealthcare.com,s.minaudo@xcmhealthcare.com,g.ambrosino@xcmhealthcare.com";

        internal static void InviaMailCustomerDiscount(CommonAPITypes.ESPRITEC.EspritecDocuments.RootobjectOrder docOsservato, int daCazziare)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"{docOsservato.header.docNumber} - Parametri di sconto non appilicati";
            string body = $"Non risultano applicate le scontistiche standard per il documento {docOsservato.header.docNumber}Si prega d'intervenire immediatamente";

            string toAddress = "";
            if (daCazziare > 0)
            {
                string daCazziareString = "";

                if (daCazziare == 30) { daCazziareString = "Simonetta"; toAddress = "s.minaudo@xcmhealthcare.com"; }
                else if (daCazziare == 43) { daCazziareString = "Anna"; toAddress = "a.ianniello@xcmhealthcare.com"; }
                else if (daCazziare == 44) { daCazziareString = "Giovanna"; toAddress = "g.ambrosino@xcmhealthcare.com"; }
                else if (daCazziare == 13) { daCazziareString = "Alida"; toAddress = "a.panico@xcmhealthcare.com"; }
                if (string.IsNullOrEmpty(daCazziareString))
                {
                    body += $" id utente non trovato per messaggio personalizzato";
                }

            }
            else
            {
                toAddress = "customercare@xcmhealthcare.com";
            }


            //MailsCust.Split(',').ToList();
            //foreach (var ta in toAddress)
            {
                if (!string.IsNullOrWhiteSpace(toAddress))
                {
                    var toa = new MailAddress(toAddress);
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
        
        internal static void InviaMailCustomerErroreInserimentoManuale(CommonAPITypes.ESPRITEC.EspritecDocuments.RootobjectOrder docOsservato, int daCazziare)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"{docOsservato.header.docNumber} - Per il cliente non sono ammessi ingressi manuali";
            string body = $"Il documento {docOsservato.header.docNumber} risulta creato manualmente, si ricorda che per PH_PH non vanno gestiti ordini manuali\r\nEliminare il documento se non diversamente richiesto dal cliente";

            var toAddress = "";
            if (daCazziare > 0)
            {
                string daCazziareString = "";

                if (daCazziare == 30) { daCazziareString = "Simonetta"; toAddress = "s.minaudo@xcmhealthcare.com"; }
                else if (daCazziare == 43) { daCazziareString = "Anna"; toAddress = "a.ianniello@xcmhealthcare.com"; }
                else if (daCazziare == 44) { daCazziareString = "Giovanna"; toAddress = "g.ambrosino@xcmhealthcare.com"; }
                else if (daCazziare == 13) { daCazziareString = "Alida"; toAddress = "a.panico@xcmhealthcare.com"; }
                if (string.IsNullOrEmpty(daCazziareString))
                {
                    body += $" id utente non trovato per email personalizzata\r\nUtente modifica/creazione del documento {daCazziare}";
                }
            }
            else
            {
                toAddress = "customercare@xcmhealthcare.com";
            }


            //MailsCust.Split(',').ToList();
            foreach (var ta in toAddress)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(toAddress))
                    {
                        var toa = new MailAddress(toAddress);
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
                catch (SmtpException rr)
                {
                    File.AppendAllText("debugLogEmail.txt", $"{DateTime.Now}---------------{subject}----------------\r\n" + rr + "\r\n_________\r\n" + rr.InnerException.Message);

                }

            }
        }
        internal static void SendMailErrorEDI(CommonAPITypes.ESPRITEC.EspritecEDIMessage.MessageMessageEDI mr)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"EDI in Errore";
            string body = $"Si segnala che il flusso EDI {mr.name}\r\nper la procedura {mr.msgTypeName} è andata in errore, si prega di verificare";

            List<string> toAddress  = MailsCust.Split(',').ToList();
            
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
        internal static void SendMailErrorEDICustom(Exception ee, string msg)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"EDI in Errore {msg}";
            string body = $"{ee}";

            List<string> toAddress = new List<string>();

            toAddress = GestoreMailCustomer.MailsCust.Split(',').ToList();

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
        public static void SegnalaAlCustomerCustom(string obj, string body)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = obj;

            var toAddress = MailsCust.Split(',').ToList();
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
