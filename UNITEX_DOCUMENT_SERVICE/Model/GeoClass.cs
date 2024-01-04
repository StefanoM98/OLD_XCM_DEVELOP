using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model
{
    public class GeoClass
    {
        public string id { get; set; }
        public string citta { get; set; }
        public string cap { get; set; }
        public string provincia { get; set; }
        public string regione { get; set; }
        public string istatCode { get; set; }

        public bool isCapoluogo 
        {
            get
            {
                return CapoluoghiItaliani.Any(x => x.Contains(citta.ToLower()));
            }
        }

        List<string> CapoluoghiItaliani = new List<string>()
        {
            "aosta",
            "torino",
            "genova",
            "milano",
            "trento",
            "venezia",
            "trieste",
            "bologna",
            "firenze",
            "ancona",
            "perugia",
            "roma",
            "l'aquila",
            "campobasso",
            "bari",
            "napoli",
            "potenza",
            "catanzaro",
            "palermo",
            "cagliari",
        };

        public static GeoClass FromCsv(string csvLine)
        {
            var values = csvLine.Split(';');
            GeoClass geo = new GeoClass();
            geo.id = Convert.ToString(values[0]);
            geo.citta = Convert.ToString(values[1]);
            geo.cap = Convert.ToString(values[2]);
            geo.provincia = Convert.ToString(values[3]);
            geo.regione = Convert.ToString(values[4]);
            geo.istatCode = Convert.ToString(values[5]);
            return geo;

        }
    }
}
