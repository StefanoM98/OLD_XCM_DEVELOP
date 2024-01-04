using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovimentiMagazzinoFromGespe
{
    class DettaglioSpedizioniXCM
    {
        public DateTime DataDoc { get; set; }
        public string NumDoc { get; set; }

        public string ShipNum { get; set; }
        public string Vettore { get; set; }
        public string UnloadName { get; set; }
        public string UnloadAddress { get; set; }
        public string UnloadZIPCode { get; set; }
        public string UnloadLocation { get; set; }
        public string UnloadCountry { get; set; }
        public string UnloadRegione { get; set; }
        public string RifXCM { get; set; }
        public int Pallet { get; set; }
        public int Colli { get; set; }
        public decimal PesoReale { get; set; }//Peso Lordo
        public decimal PesoVolumetrico { get; set; }
        public decimal Volume { get; set; }
        public decimal TotaleAttivo { get; set; }
        public decimal TotalePassivo { get; set; }//da calcolare sui listini in base a regione e corriere
        public decimal Margine
        {
            get
            {
                return TotaleAttivo - TotalePassivo;
            }
        }
        public bool SpondaIdraulica { get; set; }
        public bool InformatoreScentifico { get; set; }
        public bool AppuntamentoTelefonico { get; set; }
    }
}
