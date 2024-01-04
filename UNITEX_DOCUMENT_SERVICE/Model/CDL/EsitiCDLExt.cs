using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model
{
    public class EsitiCDLExt
    {
        public string UnitexId { get; set; }
        public string ExternalRef { get; set; }
        public DateTime DataTracking { get; set; }
        public int Stato { get; set; }

        public string CustomerID { get; set; }
        public int ShipID { get; set; }
        public int StatudId { get; set; }

        public static EsitiCDLExt FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(';');
            EsitiCDLExt esiti = new EsitiCDLExt();
            var firstElement = Convert.ToString(values[0]);
            if (firstElement.Contains("/SH"))
            {
                esiti.UnitexId = firstElement;
            }
            else
            {

                esiti.ExternalRef = firstElement;
            }
            esiti.DataTracking = Convert.ToDateTime(values[1]);
            esiti.Stato = Convert.ToInt32(values[2]);
            return esiti;
        }

    }
}
