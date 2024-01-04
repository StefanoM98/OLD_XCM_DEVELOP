using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovimentiMagazzinoFromGespe
{
    public class RigheDocumento
    {
        public int uniq { get; set; }
        public string CodMandante { get; set; }
        public string TipoMovimentazione { get; set; }
        public int? rowIdLink { get; set; }
        public string CodiceProdotto { get; set; }
        public string DescrizioneProdotto { get; set; }
        public string GruppoProdotto { get; set; }
        public decimal? Quantita { get; set; }
        public string Lotto { get; set; }
        public DateTime? Scadenza { get; set; }
        public decimal? Sconto { get; set; }
        public decimal? ImportoUnitario { get; set; }
        public decimal? PesoUnitario { get; set; }
        public decimal? ConfezioniPerCollo { get; set; }
        public decimal? ColliRiga { get; set; }
        public decimal? QtaXconf { get; set; }
        public string AliquotaIva { get; set; }
        public string TemperaturaTrasporto { get; set; }
        public string MagazzinoLogicoRiga { get; set; }
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
                    CodMandante == "00025" || //DIGIPHARM
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
                if (TipoMovimentazione == "OUT")
                {
                    return (importoNetto * 6.11M) / 100;
                    //6,11
                }
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
            else if (CodMandante == "00025")
            {
                return (importoNetto * 3M) / 100;
                //3
            }
            return 0;
        }
        private decimal dammiIlCalcoloPersonalizzato()
        {
            if (TipoMovimentazione == "OUT" || TipoMovimentazione == "TRASF" || TipoMovimentazione == "OMAG")
            {
                if (CodMandante == "00007")
                {
                    if (ConfezioniPerCollo == null || ConfezioniPerCollo == 0)
                    {
                        return 0;
                    }

                    bool isPezzosingolo = Quantita / ConfezioniPerCollo == 1;
                    if (isPezzosingolo)
                    {

                        if (DescrizioneProdotto.ToLower().Contains("singolo"))// || DescrizioneProdotto.ToLower().Contains("x1"))
                        {
                            if (Quantita.Value <= 5)
                            {
                                return 0.20M;
                            }
                            else
                            {
                                return Math.Ceiling(Quantita.Value / 5) * 0.20M;
                            }

                        }
                        else
                        {
                            return (ConfezioniPerCollo.Value) * 0.2M;
                        }
                        //var qtaTemp = 0M;
                        //if (Quantita > 5)
                        //{
                        //    qtaTemp = 5M;
                        //}
                        //else
                        //{
                        //    qtaTemp = Quantita.Value;
                        //}
                        //return qtaTemp * 0.20M;
                    }
                    else
                    {
                        if (ConfezioniPerCollo == null)
                        {
                            return 0;
                        }

                        return (ConfezioniPerCollo.Value) * 0.2M;
                        //return Math.Ceiling(Quantita.Value / ConfezioniPerCollo.Value) * 0.2M;

                    }
                }
                else if (CodMandante == "00024")
                {
                    if (CodiceProdotto == "8055320230004" ||
                       CodiceProdotto == "8055320230011" ||
                       CodiceProdotto == "8055320230028" ||
                       CodiceProdotto == "APAU00710" ||
                       CodiceProdotto == "APAU00810" ||
                       CodiceProdotto == "APAU00910")
                    {
                        return 1.0M;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else if (CodMandante == "00014")
                {
                    var resp = 0M;
                    var deltaPCS = ConfezioniPerCollo.Value / Quantita;


                    if (deltaPCS % 1 > 0)
                    {
                        resp = 0.8M;
                    }
                    else
                    {
                        resp = 2M;
                    }
                    return resp;
                }
                else if (CodMandante == "00018")
                {
                    if (TipoMovimentazione == "OUT" || TipoMovimentazione == "TRASF" || TipoMovimentazione == "OMAG"
                          || TipoMovimentazione == "CAMBIO_MERCE" || TipoMovimentazione == "OUTINV" || TipoMovimentazione == "OUTSM" || TipoMovimentazione == "OUTTRASF"
                          || TipoMovimentazione == "RESORFA")
                    {
                        return 2.0M;
                    }
                    else
                    {
                        return 0M;
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
            else if (TipoMovimentazione == "RESOCLI")
            {
                return 0;
            }
            else
            {
                return 0;
            }
        }
    }
}
