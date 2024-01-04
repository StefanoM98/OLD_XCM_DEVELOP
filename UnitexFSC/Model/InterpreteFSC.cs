using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitexFSC
{
    public class RootInterpreteFSC
    {
        private string Sel { get; set; }

        private string TipoDocumento { get; set; }
        private string DataDocumento { get; set; }
        private string DataTrasporto { get; set; }
        private string TotaleDocumento { get; set; }
        private string StatoOrdine { get; set; }
        private string ClienteFornitore { get; set; }
        private string NomeVettore { get; set; }
        private string NoteBorderau { get; set; }
        private string IDDeposito { get; set; }
        private string ZonaCliente { get; set; }
        private string Contrassegno { get; set; }
        private string CodiceNazione { get; set; }
        private string IDCliente { get; set; }
        private string IDVendita { get; set; }
        private string IDVettore { get; set; }
        private string Email { get; set; }

        public string DataStampaBorderau { get; set; }
        public string NumeroDocumento { get; set; }
        public string NomeClienteFornitore { get; set; }
        public int Colli { get; set; }
        public int Pallet { get; set; }
        public string PresenzaFrigo { get; set; }
        public double Peso { get; set; }
        public string Citta { get; set; }
        public string CAP { get; set; }
        public string Indirizzo { get; set; }
        public string Provincia { get; set; }
        public string RiferimentoOrdine { get; set; }
        public string Note { get; set; }
        public string NumCell { get; set; }

        public override string ToString()
        {
            return $"{DataStampaBorderau} {NumeroDocumento} {Colli} {Pallet}";
        }
    }

    public class InterpreteFSC : RootInterpreteFSC
    {
        public string Carrier { get; set; }
        public string TrasportType { get; set; }
    }
}
