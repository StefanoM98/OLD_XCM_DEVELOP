using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolingFileDaElaborare
{
    public class StatisticheMagazzino
    {
        public decimal Sede { get; set; }
        public DateTime Data { get; set; }
        public decimal IdUtente { get; set; }
        public string NomeUtente { get; set; }
        public decimal NrMissioni { get; set; }
        public decimal TotColli { get; set; }
        public decimal VolTotale { get; set; }
        public decimal Accettazione { get; set; }
        public decimal ColliAccettati { get; set; }
        public decimal VolAccettato { get; set; }
        public decimal Stoccaggio { get; set; }
        public decimal ColliStoccati { get; set; }
        public decimal VolStoccato { get; set; }
        public decimal Spostamenti { get; set; }
        public decimal ColliSpostati { get; set; }
        public decimal VolSpostato { get; set; }
        public decimal Abbassamenti { get; set; }
        public decimal ColliAbbassati { get; set; }
        public decimal VolAbbassamenti { get; set; }
        public decimal PrelieviPallet { get; set; }
        public decimal ColliPrelieviPallet { get; set; }
        public decimal VolPrelieviPallet { get; set; }
        public decimal PrelieviPicking { get; set; }
        public decimal ColliPrelieviPicking { get; set; }
        public decimal VolPrelieviPicking { get; set; }



    }
}
