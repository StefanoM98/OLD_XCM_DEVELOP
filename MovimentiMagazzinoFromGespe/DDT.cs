using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovimentiMagazzinoFromGespe
{

    public class DDT
    {
        public int? rowIdLink { get; set; }
        public decimal? Pallet { get; set; }
        public string CodMandante { get; set; }
        public string Mandante { get; set; }
        public string TipoMovimentazione { get; set; }
        public int uniq { get; set; }
        public string NumDDT { get; set; }
        public DateTime? DataDDT { get; set; }
        public string Committente { get; set; }
        public string Destinatario { get; set; }
        public string NomeDestinatazione { get; set; }
        public string IndirizzoDestinazione { get; set; }
        public string ProvDestinazione { get; set; }
        public string Corriere { get; set; }
        public string RifOrdine { get; set; }
        public string NoteDDT { get; set; }
        public string CodiceProdotto { get; set; }
        public string DescrizioneProdotto { get; set; }
        public string GruppoProdotto { get; set; }
        public decimal? Quantita { get; set; }
        public string Lotto { get; set; }
        public DateTime? Scadenza { get; set; }
        public decimal? Sconto { get; set; }
        public decimal? ImportoUnitario { get; set; }
        //decimal ImportoTotale
        //{
        //    get
        //    {
        //        if (ImportoUnitario != null && Quantita != null)
        //        {
        //            return ImportoUnitario.Value * Quantita.Value;
        //        }
        //        else
        //        {
        //            return -1;
        //        }
        //    }
        //}
        public decimal? PesoUnitario { get; set; }
        //decimal PesoTotale
        //{
        //    get
        //    {
        //        if (PesoUnitario != null && Quantita != null)
        //        {
        //            return PesoUnitario.Value * Quantita.Value;
        //        }
        //        else
        //        {
        //            return -1;
        //        }
        //    }
        //}
        public decimal? ConfezioniPerCollo { get; set; }
        public decimal? NumeroColli { get; set; }
        public string Causale { get; set; }
        public string OrdineGespe { get; set; }
        public string Citta { get; set; }
        public string Regione { get; set; }
        public string TripGespe { get; set; }
        public string ShipGespe { get; set; }
        public string TemperaturaTrasporto { get; set; }
        public string AliquotaIva { get; set; }
        public string DocNumGespe { get; set; }

        //public decimal CalcoloPrezzoUnitarioNetto
        //{
        //    get
        //    {
        //        return CalcolaPrezzoNetto();
        //    }
        //}


        #region Listini Fatturazione
        public decimal CalcoloFatturazione
        {
            get
            {
                if (CodMandante == "00002" || //DOMUS
                    CodMandante == "00010" || //FALQUI
                    CodMandante == "00008" || //PRAEVENIO
                    CodMandante == "00003" || //DALTON
                    CodMandante == "00016" || //NGF
                    CodMandante == "00011" || //PMS
                    CodMandante == "00006")   //FARMAIMPRESA
                {
                    return CalcolaPrezzoNetto();

                }
                else
                {
                    return dammiIlCalcoloPersonalizzato();
                }
            }
        }
        private decimal CalcolaPrezzoNetto()
        {
            if (ImportoUnitario == null)
            {
                return 0;
            }

            if (Sconto == null)
            {
                Sconto = 0;
            }
            else if (Sconto > 40 && CodMandante == "00016"/*NGF*/)
            {
                Sconto = 40;
            }

            var importoNetto = ImportoUnitario.Value - ((ImportoUnitario.Value * Sconto.Value) / 100);

            if (CodMandante == "00002")
            {
                return (importoNetto * 2.5M) / 100;
                //2,50
            }
            else if (CodMandante == "00010")
            {
                return (importoNetto * 6.11M) / 100;
                //6,11
            }
            else if (CodMandante == "00008")
            {
                return (importoNetto * 3.5M) / 100;
                //3,5
            }
            else if (CodMandante == "00011")
            {
                return (importoNetto * 2.2M) / 100;
                //2,2
            }
            else if (CodMandante == "00003")
            {
                return (importoNetto * 2.5M) / 100;
                //2,5
            }
            else if (CodMandante == "00016")
            {
                return (importoNetto * 6M) / 100;
                //6
            }
            else if (CodMandante == "00006")
            {
                return (importoNetto * 3.5M) / 100;
                //3,5
            }



            return 0;
        }
        private decimal dammiIlCalcoloPersonalizzato()
        {
            if (TipoMovimentazione == "OUT" || TipoMovimentazione == "TRASF")
            {
                if (CodMandante == "00007")
                {
                    bool isPezzosingolo = Quantita / ConfezioniPerCollo == 1;
                    if (isPezzosingolo)
                    {
                        decimal tt = Quantita.Value / 5;
                        var ttr = Math.Ceiling(tt);
                        return ttr * 0.20M;
                    }
                    else
                    {
                        return Math.Ceiling(ConfezioniPerCollo.Value) * 0.20M;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else if (TipoMovimentazione == "IN")
            {
                if (CodMandante == "00007")
                {
                    return 1 * 2.0M;
                }
                else if (CodMandante == "00024")
                {
                    return 1 * 1.5M;
                }
                return 0;
            }
            else
            {
                return 0;
            }
        }
        #endregion
    }
}
