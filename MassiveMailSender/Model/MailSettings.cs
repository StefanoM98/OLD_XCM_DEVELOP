using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveMailSender.Model
{
    public class MailSettings
    {
        public static string defaultBody = @"Gentile Cliente,

sono Mario Mastromatteo, Key Account Manager della UNITEX Srl, società di trasporti del gruppo Marzano, di cui fanno parte - tra le altre - la XCM settore logistica e Farmacie Santa Caterina.

Mi permetto di inviarLe, in allegato, una presentazione delle già menzionate società, nello specifico in merito ai settori logistica e trasporti, al fine di programmare un incontro conoscitivo per illustrarle i servizi che offriamo.

Nell’attesa di un Suo cortese cenno di riscontro e ringraziandoLa sin d’ora per la Sua disponibilità, Le invio i miei migliori saluti.";

        public static string fileName = "dhvbsasjhbf";
        public string SenderSmtpHost = "";
        public int SenderSmtpPort = 587;
        public string SenderMail = "";
        public string SenderMailPassword = "";
        public string SenderMailName = "";


    }
}
