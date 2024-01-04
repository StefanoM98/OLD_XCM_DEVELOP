using System;
using System.Text.RegularExpressions;

namespace MassiveMailSender.Model
{
    public class DocumentRowModel
    {
        public string RagioneSociale { get; set; }
        public string Citta { get; set; }
        public string Nazione { get; set; }
        public string Indirizzo { get; set; }
        public string Provincia { get; set; }
        public string Recapiti { get; set; }
        public string Mail { get; set; }
        public string Settore { get; set; }
        public string Note { get; set; }

        public int Id { get; set; }
        public bool MailIsValid
        {
            get
            {
                var rx = "^\\S+@\\S+\\.\\S+$";
                if (!string.IsNullOrEmpty(this.Mail))
                {
                    return Regex.IsMatch(this.Mail, rx);

                }
                else
                {
                    return false;
                }

            }
        }
        public DateTime DataInvioEmail { get; set; }

    }
}
