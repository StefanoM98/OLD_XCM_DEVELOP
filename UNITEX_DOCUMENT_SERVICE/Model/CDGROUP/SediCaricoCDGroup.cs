using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model.CDGROUP
{
    public static class SediCaricoCDGroup
    {
        internal static MagazzinoCDGroup Liscate = new MagazzinoCDGroup()
        {
            address = "Via Guido Rossa 1/3",
            country = "IT",
            district = "MI",
            location = "Liscate",
            zipCode = "20050",
            MandantiAbbinati = new List<string>() {
                "C49", "C40", "T50", "C18", "CH5", "C20", "C52", "C55", "C63", "C70",
                "C72", "C75", "T40", "C82", "T10", "D01", "T20", "T04", "C67", "T01",
                "D04", "D05", "C39", "D08", "D07", "C87", "D10" }
        };
        internal static MagazzinoCDGroup Agnadello = new MagazzinoCDGroup()
        {
            address = "Strada statale Bergamina Km 11,300",
            country = "IT",
            district = "CR",
            location = "Agnadello",
            zipCode = "26020",
            MandantiAbbinati = new List<string>() { "A50", "C71", "D50", "D51", "DLV" }
        };
        internal static MagazzinoCDGroup Arzago = new MagazzinoCDGroup()
        {
            address = "Strada provinciale rivoltana",
            country = "IT",
            district = "BG",
            location = "Arzago D'Adda",
            zipCode = "24040",
            MandantiAbbinati = new List<string>() { "F40", "F23", "C51", "F54", "U51", "F80", "F85","F70" }
        };
        internal static MagazzinoCDGroup Vailate = new MagazzinoCDGroup()
        {
            address = "Via Ferri",
            country = "IT",
            district = "CR",
            location = "Vailate",
            zipCode = "26019",
            MandantiAbbinati = new List<string>() { "F15" }
        };

        internal static MagazzinoCDGroup Calvenzano = new MagazzinoCDGroup()
        {
            address = "Via Milano",
            country = "IT",
            district = "BG",
            location = "Calvenzano",
            zipCode = "24040",
            MandantiAbbinati = new List<string>() { "F52", "C31", "F01", "F21", "F60", "F04", "F63", "F66", "F70", "F73", "F86", "F94", "T02", "F64", "OF2", "D02", "F90", "D03", "F03", "C92", "F49", "F59", "F07", "F99", "F98", "F08" }
        };
        public static MagazzinoCDGroup SedeLegale = new MagazzinoCDGroup()
        {
            address = "VIA BICE CREMAGNANI 15/7",
            country = "IT",
            district = "MB",
            zipCode = "20871",
            location = "VIMERCATE"
        };

        internal static List<MagazzinoCDGroup> Magazzini = new List<MagazzinoCDGroup>() { Liscate, Agnadello, Arzago, Calvenzano, Vailate };

        public static MagazzinoCDGroup RecuperaLaSedeCDGroup(string codiceMandante)
        {
            return Magazzini.FirstOrDefault(x => x.MandantiAbbinati.Contains(codiceMandante));
        }
    }

    public class MagazzinoCDGroup
    {
        public string address { get; set; }
        public string country { get; set; }
        public string district { get; set; }
        public string zipCode { get; set; }
        public string location { get; set; }
        public List<string> MandantiAbbinati { get; set; }
    }

}
