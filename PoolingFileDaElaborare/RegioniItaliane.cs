using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolingFileDaElaborare
{
    public static class RegioniItaliane
    {
        public static string EstraiRegione(string provincia)
        {
            string reg = "";

            if (provincia.ToLower() == "aquila"
                || provincia.ToLower() == "aq"
                || provincia.ToLower() == "chieti"
                || provincia.ToLower() == "ch"
                || provincia.ToLower() == "pescara"
                || provincia.ToLower() == "pe"
                || provincia.ToLower() == "teramo"
                || provincia.ToLower() == "te")
            {
                reg = "Abruzzo";
            }
            else if (provincia.ToLower() == "matera"
                || provincia.ToLower() == "mt"
                || provincia.ToLower() == "potenza"
                || provincia.ToLower() == "pz")
            {
                reg = "Basilicata";
            }
            else if (provincia.ToLower() == "catanzaro"
                || provincia.ToLower() == "cz"
                || provincia.ToLower() == "cosenza"
                || provincia.ToLower() == "cs"
                || provincia.ToLower() == "crotone"
                || provincia.ToLower() == "kr"
                || provincia.ToLower() == "reggio calabria"
                || provincia.ToLower() == "rc"
                || provincia.ToLower() == "vibo valentia"
                || provincia.ToLower() == "vv")
            {
                reg = "Calabria";
            }
            else if (provincia.ToLower() == "avellino"
               || provincia.ToLower() == "av"
               || provincia.ToLower() == "benevento"
               || provincia.ToLower() == "bn"
               || provincia.ToLower() == "caserta"
               || provincia.ToLower() == "ce"
               || provincia.ToLower() == "napoli"
               || provincia.ToLower() == "na"
               || provincia.ToLower() == "salerno"
               || provincia.ToLower() == "sa")
            {
                reg = "Campania";
            }
            else if (provincia.ToLower() == "bologna"
               || provincia.ToLower() == "bo"
               || provincia.ToLower() == "ferrara"
               || provincia.ToLower() == "fe"
               || provincia.ToLower() == "forlì-cesena"
               || provincia.ToLower() == "fc"
               || provincia.ToLower() == "modena"
               || provincia.ToLower() == "mo"
               || provincia.ToLower() == "parma"
               || provincia.ToLower() == "pr"
               || provincia.ToLower() == "piacenza"
               || provincia.ToLower() == "pc"
               || provincia.ToLower() == "ravenna"
               || provincia.ToLower() == "ra"
               || provincia.ToLower() == "reggio emilia"
               || provincia.ToLower() == "re"
               || provincia.ToLower() == "rimini"
               || provincia.ToLower() == "rn")
            {
                reg = "Emilia Romagna";
            }
            else if (provincia.ToLower() == "gorizia"
               || provincia.ToLower() == "go"
               || provincia.ToLower() == "pordenone"
               || provincia.ToLower() == "pn"
               || provincia.ToLower() == "trieste"
               || provincia.ToLower() == "ts"
               || provincia.ToLower() == "udine"
               || provincia.ToLower() == "ud")
            {
                reg = "Friuli Venezia Giulia";
            }
            else if (provincia.ToLower() == "frosinone"
               || provincia.ToLower() == "fr"
               || provincia.ToLower() == "latina"
               || provincia.ToLower() == "lt"
               || provincia.ToLower() == "rieti"
               || provincia.ToLower() == "ri"
               || provincia.ToLower() == "roma"
               || provincia.ToLower() == "rm"
               || provincia.ToLower() == "viterbo"
               || provincia.ToLower() == "vt")
            {
                reg = "Lazio";
            }
            else if (provincia.ToLower() == "genova"
               || provincia.ToLower() == "ge"
               || provincia.ToLower() == "imperia"
               || provincia.ToLower() == "im"
               || provincia.ToLower() == "spezia"
               || provincia.ToLower() == "sp"
               || provincia.ToLower() == "savona"
               || provincia.ToLower() == "sv")
            {
                reg = "Liguria";
            }
            else if (provincia.ToLower() == "bergamo"
               || provincia.ToLower() == "bg"
               || provincia.ToLower() == "brescia"
               || provincia.ToLower() == "bs"
               || provincia.ToLower() == "como"
               || provincia.ToLower() == "co"
               || provincia.ToLower() == "cremona"
               || provincia.ToLower() == "cr"
               || provincia.ToLower() == "lecco"
               || provincia.ToLower() == "lc"
               || provincia.ToLower() == "lodi"
               || provincia.ToLower() == "lo"
               || provincia.ToLower() == "mantova"
               || provincia.ToLower() == "mn"
               || provincia.ToLower() == "milano"
               || provincia.ToLower() == "mi"
               || provincia.ToLower() == "monza e brianza"
               || provincia.ToLower() == "monza"
               || provincia.ToLower() == "brianza"
               || provincia.ToLower() == "mb"
               || provincia.ToLower() == "pavia"
               || provincia.ToLower() == "pv"
               || provincia.ToLower() == "sondrio"
               || provincia.ToLower() == "so"
               || provincia.ToLower() == "varese"
               || provincia.ToLower() == "va")
            {
                reg = "Lombardia";
            }
            else if (provincia.ToLower() == "ancona"
                || provincia.ToLower() == "an"
                || provincia.ToLower() == "ascoli piceno"
                || provincia.ToLower() == "ap"
                || provincia.ToLower() == "fermo"
                || provincia.ToLower() == "fm"
                || provincia.ToLower() == "macerata"
                || provincia.ToLower() == "mc"
                || provincia.ToLower() == "pesaro e urbino"
                || provincia.ToLower() == "urbino"
                || provincia.ToLower() == "pesaro"
                || provincia.ToLower() == "pu")
            {
                reg = "Marche";
            }
            else if (provincia.ToLower() == "campobasso"
                || provincia.ToLower() == "cb"
                || provincia.ToLower() == "isernia"
                || provincia.ToLower() == "is")
            {
                reg = "Molise";
            }
            else if (provincia.ToLower() == "alessandria"
               || provincia.ToLower() == "al"
               || provincia.ToLower() == "asti"
               || provincia.ToLower() == "at"
               || provincia.ToLower() == "biella"
               || provincia.ToLower() == "bi"
               || provincia.ToLower() == "cuneo"
               || provincia.ToLower() == "cn"
               || provincia.ToLower() == "novara"
               || provincia.ToLower() == "no"
               || provincia.ToLower() == "torino"
               || provincia.ToLower() == "to"
               || provincia.ToLower() == "verbano cusio ossola"
               || provincia.ToLower() == "vb"
               || provincia.ToLower() == "vercelli"
               || provincia.ToLower() == "vc")
            {
                reg = "Piemonte";
            }
            else if (provincia.ToLower() == "bari"
               || provincia.ToLower() == "ba"
               || provincia.ToLower() == "barletta"
               || provincia.ToLower() == "andia"
               || provincia.ToLower() == "trani"
               || provincia.ToLower() == "bt"
               || provincia.ToLower() == "brindisi"
               || provincia.ToLower() == "br"
               || provincia.ToLower() == "foggia"
               || provincia.ToLower() == "fg"
               || provincia.ToLower() == "lecce"
               || provincia.ToLower() == "le"
               || provincia.ToLower() == "taranto"
               || provincia.ToLower() == "te")
            {
                reg = "Puglia";
            }
            else if (provincia.ToLower() == "cagliari"
              || provincia.ToLower() == "ca"
              || provincia.ToLower() == "carbonia-iglesias"
              || provincia.ToLower() == "carbonia"
              || provincia.ToLower() == "iglesias"
              || provincia.ToLower() == "ci"
              || provincia.ToLower() == "medio campidano"
              || provincia.ToLower() == "vs"
              || provincia.ToLower() == "nuoro"
              || provincia.ToLower() == "nu"
              || provincia.ToLower() == "ogliastra"
              || provincia.ToLower() == "og"
              || provincia.ToLower() == "olbia-tempio"
              || provincia.ToLower() == "olbia"
              || provincia.ToLower() == "tempio"
              || provincia.ToLower() == "ot"
              || provincia.ToLower() == "oristano"
              || provincia.ToLower() == "or"
              || provincia.ToLower() == "sassari"
              || provincia.ToLower() == "ss")
            {
                reg = "Sardegna";
            }
            else if (provincia.ToLower() == "agrigento"
              || provincia.ToLower() == "ag"
              || provincia.ToLower() == "caltanissetta"
              || provincia.ToLower() == "cl"
              || provincia.ToLower() == "catania"
              || provincia.ToLower() == "ct"
              || provincia.ToLower() == "enna"
              || provincia.ToLower() == "en"
              || provincia.ToLower() == "messina"
              || provincia.ToLower() == "me"
              || provincia.ToLower() == "palermo"
              || provincia.ToLower() == "pa"
              || provincia.ToLower() == "ragusa"
              || provincia.ToLower() == "rg"
              || provincia.ToLower() == "siracusa"
              || provincia.ToLower() == "sr"
              || provincia.ToLower() == "trapani"
              || provincia.ToLower() == "tp")
            {
                reg = "Sicilia";
            }
            else if (provincia.ToLower() == "arezzo"
              || provincia.ToLower() == "ar"
              || provincia.ToLower() == "firenze"
              || provincia.ToLower() == "fi"
              || provincia.ToLower() == "grosseto"
              || provincia.ToLower() == "gr"
              || provincia.ToLower() == "livorno"
              || provincia.ToLower() == "li"
              || provincia.ToLower() == "lucca"
              || provincia.ToLower() == "lu"
              || provincia.ToLower() == "massa-carrara"
              || provincia.ToLower() == "massa carrara"
              || provincia.ToLower() == "ms"
              || provincia.ToLower() == "pisa"
              || provincia.ToLower() == "pi"
              || provincia.ToLower() == "pistoia"
              || provincia.ToLower() == "pt"
              || provincia.ToLower() == "prato"
              || provincia.ToLower() == "po"
              || provincia.ToLower() == "siena"
              || provincia.ToLower() == "si")
            {
                reg = "Toscana";
            }
            else if (provincia.ToLower() == "bolzano"
               || provincia.ToLower() == "bz"
               || provincia.ToLower() == "trento"
               || provincia.ToLower() == "tr")
            {
                reg = "Trentino Alto Adige";
            }
            else if (provincia.ToLower() == "perugia"
               || provincia.ToLower() == "pg"
               || provincia.ToLower() == "terni"
               || provincia.ToLower() == "tr")
            {
                reg = "Umbria";
            }
            else if (provincia.ToLower() == "valle d'aosta"
               || provincia.ToLower() == "ao")
            {
                reg = "Valle d'Aosta";
            }
            else if (provincia.ToLower() == "belluno"
              || provincia.ToLower() == "bl"
              || provincia.ToLower() == "padova"
              || provincia.ToLower() == "pd"
              || provincia.ToLower() == "rovigo"
              || provincia.ToLower() == "ro"
              || provincia.ToLower() == "treviso"
              || provincia.ToLower() == "tv"
              || provincia.ToLower() == "venezia"
              || provincia.ToLower() == "ve"
              || provincia.ToLower() == "verona"
              || provincia.ToLower() == "vr"
              || provincia.ToLower() == "vicenza"
              || provincia.ToLower() == "vi")
            {
                reg = "Veneto";
            }
            //vettoreMigliore = RilevaVettoreMigliore(reg);
            return reg;
        }

        private static string RilevaVettoreMigliore(string reg)
        {
            if (string.IsNullOrEmpty(reg)) return "";
            //var vet = popolaVettori();

            return "";

        }

        private static List<Vettore> popolaVettori()
        {
            throw new NotImplementedException();
        }

        private class Vettore
        {
            public string nome { get; set; }
            public string prov { get; set; }
            public decimal costo { get; set; }
            public TimeSpan tempoConsegna { get; set; }
            public bool temperaturaControllata { get; set; }
        }
    }
}
