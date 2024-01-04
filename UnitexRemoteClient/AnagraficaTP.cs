using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitexRemoteClient
{
    public static class AnagraficaTP
    {
        //PUGLIA
        internal static TP AllWays = new TP()
        {
            email = "i.scarimbolo@allwayslogistic.com",
            ProvCompetenza = new List<string>() { "BA", "BT", "BR", "FO", "LE", "TA" }
        };
        //LAZIO
        internal static TP GIMA = new TP()
        {
            email = "info@gimatrasporti.it,ritir@gimatrasporti.it",
            ProvCompetenza = new List<string>() { "FR", "RM", "LT", "RI", "VT" }
        };
        //CAMPANIA
        internal static TP UNITEX_SUD = new TP()
        {
            email = "operativo@unitexpress.it,customercare@unitexpress.it",
            ProvCompetenza = new List<string>() { "CE", "BN", "AV", "SA", "NA" }
        };
        //LOMBARDIA
        internal static TP UNITEX_NORD = new TP()
        {
            email = "customerservicemilano@unitexpress.it",
            ProvCompetenza = new List<string>() { "BG", "BS", "CO", "CR", "LC", "LO", "MB", "PV", }
        };
        //MARCHE-ABRUZZO-MOLISE
        internal static TP EMMEA = new TP()
        {
            email = "fiorella@emmeasrl.it,andrea@emmeasrl.it,esiti.cepagatti@emmeasrl.it",
            ProvCompetenza = new List<string>() { "CH", "AQ", "PE", "TE", "AN", "AP", "FM", "MC", "PU", "CB", "IS" }
        };
        //SARDEGNA
        internal static TP MURA = new TP()
        {
            email = "operativo@muratrasporti.it",
            ProvCompetenza = new List<string>() { "CA", "NU", "OR", "SS", "SU" }
        };
        //TOSCANA
        internal static TP COTRAF = new TP()
        {
            email = "logistica.firenze@consorzio-cotraf.it",
            ProvCompetenza = new List<string>() { "AR", "FI", "GR", "LI", "LU", "MS", "PI", "PT", "PO" }
        };
        //CALABRIA-SICILIA
        internal static TP TLI = new TP()
        {
            email = "logistica@trasportilogisticaitalia.com",
            ProvCompetenza = new List<string>() { "CZ", "CS", "KR", "RC", "VV", "AG", "CL", "CT", "EN", "ME", "PA", "RG", "SR", "TP" }
        };
        //TRIVENETO
        internal static TP FUTURA = new TP()
        {
            email = "customer@futuratrasporti.com,service@futuratrasporti.com",
            ProvCompetenza = new List<string>() { "GO", "PN", "TS", "UD", "BZ", "TN", "BL", "PD", "RO", "TV", "VE", "VR", "VI" }
        };
        //EMILIA-ROMAGNA
        internal static TP DAMORA = new TP()
        {
            email = "emiliaromagna@damoralogistica.it,assistenza@damoralogistica.it",
            ProvCompetenza = new List<string>() { "BO", "FE", "FC", "MO", "PR", "PC", "RA", "RE", "RN" }
        };
        //PIEMONTE-AOSTA
        internal static TP STURLA = new TP()
        {
            email = "andrea@sturlasrl.it,rosi@sturlasrl.it",
            ProvCompetenza = new List<string>() { "AL", "AT", "BI", "CN", "NO", "TO", "VB", "VC", "AO" }
        };
        //LIGURIA
        internal static TP CD_AUTOTRASPORTI = new TP()
        {
            email = "marjka.pastorino@cdautotrasporti.it,canepa.andrea@cdautotrasporti.it",
            ProvCompetenza = new List<string>() { "GE", "IM", "SP", "SV" }
        };
        //VARESE
        internal static TP MNZ = new TP()
        {
            email = "ritiri@mnztrasporti.it",
            ProvCompetenza = new List<string>() { "VA" }
        };
        //MANTOVA
        internal static TP MB = new TP()
        {
            email = "mbtrasportisnc@alice.it",
            ProvCompetenza = new List<string>() { "MN" }
        };
        //SONDRIO
        internal static TP MARIANA = new TP()
        {
            email = "lucia@marianatrasporti.it",
            ProvCompetenza = new List<string>() { "SO" }
        };
        public class TP
        {
            public string email { get; set; }
            public List<string> ProvCompetenza;
        }
        internal static List<TP> tPs = new List<TP>() { AllWays, GIMA, UNITEX_SUD, UNITEX_NORD };
    }
}
