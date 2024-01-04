using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovimentiMagazzinoFromGespe
{
    public class TestataDocumento
    {
        public string CodMandanteTestata { get; set; }
        public int uniq { get; set; }
        public string TipoMovimentazione { get; set; }
        public string NumDDT { get; set; }
        public DateTime? DataDDT { get; set; }
        public DateTime? DataUltimaModifica { get; set; }
        public string Committente { get; set; }
        public string Destinatario { get; set; }
        public string NomeDestinatazione { get; set; }
        public string IndirizzoDestinazione { get; set; }
        public string NazioneDestinazione { get; set; }
        public string RegioneDestinazione { get; set; }
        public string ProvDestinazione { get; set; }
        public string Corriere { get; set; }
        public string RifOrdine { get; set; }
        public string NoteDDT { get; set; }
        public string TripGespe { get; set; }
        public string ShipGespe { get; set; }
        public string DocNumGespe { get; set; }
        public bool TramissioneMinistero
        {
            get
            {
                var presente = RigheD.FirstOrDefault(x => x.CodiceProdotto.StartsWith("0") && x.CodiceProdotto.Length == 9);

                if (presente != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        int Picking { get; set; }
        public int Cartone163148100 { get; set; }
        public int Cartone253211225 { get; set; }
        public int Cartone311311240 { get; set; }
        public int Cartone343148100 { get; set; }
        public int Cartone553378195 { get; set; }
        public int Cartone600400400 { get; set; }

        public decimal PrzCartone163148100 { get { return Cartone163148100 * 0.45M; } }
        public decimal PrzCartone253211225 { get { return Cartone253211225 * 0.60M; } }
        public decimal PrzCartone311311240 { get { return Cartone311311240 * 0.80M; } }
        public decimal PrzCartone343148100 { get { return Cartone343148100 * 0.95M; } }
        public decimal PrzCartone553378195 { get { return Cartone553378195 * 1.60M; } }
        public decimal PrzCartone600400400 { get { return Cartone553378195 * 2M; } }
        //public int PalletINBOUND { get; set; }
        public decimal FatturazioneDocumento
        {
            get
            {
                if (RigheD != null)
                {
                    var SommaRighe = RigheD.Sum(x => x.CalcoloFatturazione);
                    if (TipoMovimentazione == "OUT" || TipoMovimentazione == "TRASF")
                    {
                        if (CodMandanteTestata == "00024")
                        {
                            if (NoteDDT != null && NoteDDT.ToLower().StartsWith("urgente"))
                            {
                                SommaRighe += 21M;
                            }
                            else
                            {
                                SommaRighe += 15M;
                            }
                        }

                    }
                    else if (TipoMovimentazione == "OMAG")
                    {
                        if (CodMandanteTestata == "00024")
                        {
                            if (NoteDDT != null && NoteDDT.ToLower().StartsWith("urgente"))
                            {
                                SommaRighe += 21M;
                            }
                            else
                            {
                                SommaRighe += 15M;
                            }
                        }
                        else if (CodMandanteTestata == "00025")
                        {
                            SommaRighe += 10M;
                        }
                    }
                    else if (TipoMovimentazione == "IN" || TipoMovimentazione == "RESOCLI")
                    {
                        if (CodMandanteTestata == "00024" && TipoMovimentazione == "RESOCLI")
                        {
                            SommaRighe += 24M;
                        }
                    }
                    return SommaRighe;

                }
                else
                {
                    return 0;
                }
            }
        }
        public decimal? NumeroColli { get; set; }
        public decimal? NumeroPallet { get; set; }
        private string _MagazzinoLogico { get; set; }
        public string MagazzinoLogico
        {
            get
            {
                return _MagazzinoLogico;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    if (CodMandanteTestata == "00007")
                    {
                        if (RigheD != null)
                        {
                            var ml = RigheD.FirstOrDefault(x => !string.IsNullOrEmpty(x.MagazzinoLogicoRiga));
                            if (ml != null)
                            {
                                _MagazzinoLogico = ml.MagazzinoLogicoRiga;
                            }
                        }
                        else
                        {
                            _MagazzinoLogico = "";
                        }
                    }
                }
                else
                {
                    _MagazzinoLogico = value;
                }
            }
        }
        public List<RigheDocumento> RigheD { get; set; }
    }
}
