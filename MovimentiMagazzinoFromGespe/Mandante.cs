using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovimentiMagazzinoFromGespe
{
    public class Mandante
    {

        #region DatiGenerali
        public long ID_MANDANTE { get; set; }
        public string CodMandante { get; set; }
        public string Descrizione { get; set; }
        #endregion

        #region PalletINBOUD
        public int PalletINBOUND { get; set; }
        public decimal ValorePalletINNBOUND
        {
            get
            {
                return PalletINBOUND * RecuperaIlPrezzoDiListinoPerIlMandantePalletINBOUND();
            }
        }
        private decimal RecuperaIlPrezzoDiListinoPerIlMandantePalletINBOUND()
        {
            if (CodMandante == "00007")
            {
                return 8M;
            }
            else if (CodMandante == "00024")
            {
                return 8.0M;
            }
            else if (CodMandante == "00010")
            {
                return 6.0M;
            }
            else if (CodMandante == "00019")
            {
                return 5M;
            }
            else if (CodMandante == "00010")
            {
                return 2.5M;
            }
            else if (CodMandante == "00012")
            {
                return 8M;
            }
            else if (CodMandante == "00014")
            {
                return 2M;
            }
            else if (CodMandante == "00020")
            {
                return 8M;
            }
            else if (CodMandante == "00018")
            {
                return 8M;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region CartoniXCM
        public int CartoniXCMUtilizzati
        {
            get
            {
                if (DocumentiDelMandante == null) { return 0; }
                if (CodMandante == "00007")
                {
                    return 0;
                }
                if (CodMandante != "00013")
                {
                    return this.DocumentiDelMandante.Sum(x => x.Cartone163148100 + x.Cartone253211225 + x.Cartone311311240 + x.Cartone343148100 + x.Cartone553378195);
                }

                else
                {
                    return 1;
                }
            }
        }
        public decimal ValoreCartoniXCM
        {
            get
            {
                if (DocumentiDelMandante == null) { return 0; }
                if (CodMandante == "00007")
                {
                    return 0;
                }
                if (CodMandante != "00013")
                {
                    return this.DocumentiDelMandante.Sum(x => x.PrzCartone163148100 + x.PrzCartone253211225 + x.PrzCartone311311240 + x.PrzCartone343148100 + x.PrzCartone553378195);
                }
                else
                {
                    return 50M;
                }
            }
        }
        #endregion

        #region Recuperati su GESPE
        public int StockPuntaMax825 { get; set; }
        public decimal ValoreStockPuntaMax825
        {
            get
            {
                if (CodMandante == "00019")
                {
                    return StockPuntaMax825 * 15M;
                }
                else if (CodMandante == "00018")
                {
                    return StockPuntaMax825 * 4.5M;
                }
                else if (CodMandante == "00015")
                {
                    return StockPuntaMax825 * 5M;
                }
                else if (CodMandante == "00020")
                {
                    return StockPuntaMax825 * 5M;
                }
                else if (CodMandante == "00007")
                {
                    StockPuntaMax825 = 0;
                    return 0;
                }
                return 0;
            }
        }
        public int StockPuntaMax28 { get; set; }
        public decimal ValoreStockPuntaMax28 { get; set; }
        #endregion

        public int PalletIN
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                else if (CodMandante == "00007") { return 0; }
                //return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "IN" || x.TipoMovimentazione == "RESOCLI").Sum(y => y.RigheD.Count);//TODO: prendere il dato da registrazioni 
                return (int)DocumentiDelMandante.Where(x => x.TipoMovimentazione == "IN" || x.TipoMovimentazione == "RESOCLI").Sum(y => y.NumeroPallet);
            }

        }
        public decimal ValorePalletIN
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00024")
                {
                    return PalletIN * 1.50M;
                }
                else if (CodMandante == "00007")
                {
                    return 0;
                    //return PalletIN * 2M;
                }
                else if (CodMandante == "00019")
                {
                    return PalletIN * 5M;
                }
                else if (CodMandante == "00013")
                {
                    return PalletIN * 2.5M;
                }
                else if (CodMandante == "00006")
                {
                    return PalletIN * 2M;
                }
                else if (CodMandante == "00014")
                {
                    return PalletIN * 0.5M;
                }
                else if (CodMandante == "00015")
                {
                    return PalletIN * 1.5M;
                }
                else if (CodMandante == "00011")
                {
                    return PalletIN * 1M;
                }

                return 0;
            }
        }
        public int PalletOUT
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00007")
                {
                    //var tt = DocumentiDelMandante.Where(x => x.TipoMovimentazione == "TRASF").ToList();
                    //var plt = 0;

                    //foreach (var t in tt)//TODO recupero pallet effettivi da tmslegs
                    //{
                    //    if (t.NumeroPallet < 1)
                    //    {
                    //        plt = 1;
                    //    }
                    //    else
                    //    {
                    //        plt = (int)Math.Ceiling(t.NumeroPallet.Value);
                    //    }
                    //}

                    //var tt2 = tt.Sum(y => y.RigheD.Count);
                    //return tt2;
                    return 0;
                }

                //return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "OUT" || x.TipoMovimentazione == "TRASF").Sum(y => y.RigheD.Count);//TODO: prendere il dato da registrazioni 
                return (int)DocumentiDelMandante.Where(x => x.TipoMovimentazione == "OUT" || x.TipoMovimentazione == "TRASF").Sum(y => y.NumeroPallet);
            }

        }
        public decimal ValorePalletOut
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00007")
                {
                    return PalletOUT * 2M;
                }
                else if (CodMandante == "00019")
                {
                    return PalletOUT * 5M;
                }
                else if (CodMandante == "00014" || CodMandante == "00016")
                {
                    return PalletOUT * 2M;
                }
                return 0;
            }
        }
        //public int Picking
        //{
        //    get
        //    {
        //        return _Picking;
        //    }
        //    set
        //    {
        //        _Picking = value;
        //    }
        //}
        public int Picking
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00024")
                {
                    return DocumentiDelMandante.Count();
                }
                if (CodMandante == "00007")
                {
                    return (int)(ValorePicking / 0.2M);
                    //return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "OUT" || x.TipoMovimentazione == "TRASF").Count();
                }
                else if (CodMandante == "00015")
                {
                    return DocumentiDelMandante.Count();
                }
                else if (CodMandante == "00010")
                {
                    return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "OMAG").Count();
                }
                else if (CodMandante == "00013")
                {
                    return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "OUT" || x.TipoMovimentazione == "TRASF" || x.TipoMovimentazione == "OMAG").Count();
                }
                else if (CodMandante == "00014")
                {
                    return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "OUT" || x.TipoMovimentazione == "TRASF").Count();
                }
                else if (CodMandante == "00018")
                {
                    return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "OUT" || x.TipoMovimentazione == "TRASF" || x.TipoMovimentazione == "OMAG"
                    || x.TipoMovimentazione == "CAMBIO_MERCE" || x.TipoMovimentazione == "OUTINV" || x.TipoMovimentazione == "OUTSM" || x.TipoMovimentazione == "OUTTRASF"
                    || x.TipoMovimentazione == "RESORFA").Sum(x => x.RigheD.Count());

                }
                else if (CodMandante == "00020")
                {
                    var resp = 0;

                    foreach (var r in DocumentiDelMandante)
                    {
                        resp += r.RigheD.Count;
                    }
                }
                return 0;
            }
            set
            {
                Picking = value;
            }
        }
        public decimal ValorePicking
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00007")
                {
                    return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "OUT" || x.TipoMovimentazione == "TRASF").Sum(x => x.FatturazioneDocumento);
                }
                else if (CodMandante == "00024")
                {
                    return DocumentiDelMandante.Sum(x => x.FatturazioneDocumento);
                }
                else if (CodMandante == "00010")
                {
                    return this.Picking * 25M;
                }
                else if (CodMandante == "00014")
                {
                    return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "OUT" || x.TipoMovimentazione == "TRASF").Sum(x => x.FatturazioneDocumento);
                }
                else if (CodMandante == "00020")
                {
                    return this.Picking * 2M;
                }
                if (CodMandante == "00015")
                {
                    return this.Picking * 10M;
                }
                if (CodMandante == "00018")
                {
                    return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "OUT" || x.TipoMovimentazione == "TRASF" || x.TipoMovimentazione == "OMAG"
                          || x.TipoMovimentazione == "CAMBIO_MERCE" || x.TipoMovimentazione == "OUTINV" || x.TipoMovimentazione == "OUTSM" || x.TipoMovimentazione == "OUTTRASF"
                          || x.TipoMovimentazione == "RESORFA").Sum(x => x.FatturazioneDocumento);
                    //return this.Picking;
                }
                else if (CodMandante == "00013")
                {
                    var ddt = DocumentiDelMandante.Where(x => x.TipoMovimentazione == "OUT" || x.TipoMovimentazione == "TRASF" || x.TipoMovimentazione == "OMAG").ToList();

                    int clt = 0;
                    var pcs = 0;
                    foreach (var d in ddt)
                    {
                        clt += (int)Math.Ceiling((d.NumeroColli != null) ? d.NumeroColli.Value : 0);
                        pcs += (int)Math.Ceiling(d.RigheD.Sum(x => x.Quantita.Value));
                    }

                    return 0;// (decimal)(clt * 0.45);// + (decimal)(pcs * 0.25);
                }
                return 0;
            }
        }
        public decimal TracciatoMinisteriale
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00024")
                {
                    return 1;
                }
                else if (CodMandante == "00015")
                {
                    return DocumentiDelMandante.Where(x => x.TramissioneMinistero && (x.TipoMovimentazione == "OUT" || x.TipoMovimentazione == "OMAG" || x.TipoMovimentazione == "TRASF")).Count();
                }
                else if (CodMandante == "00010" || CodMandante == "00018")
                {
                    return DocumentiDelMandante.Where(x => x.TramissioneMinistero).Count();
                }

                return 0;
            }
        }
        public decimal ValoreTracciatoMinisteriale
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00024")
                {
                    return 250M;
                }
                else if (CodMandante == "00015")
                {
                    return TracciatoMinisteriale * 150M;
                }
                else if (CodMandante == "00010" || CodMandante == "00018")
                {
                    return TracciatoMinisteriale * 25M;
                }

                return 0;
            }
        }
        public int ResoClienti
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00007")
                {
                    return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "RESOCLI").Sum(x => x.RigheD.Count());
                }
                else if (CodMandante == "00013")
                {
                    return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "RESOCLI").Sum(x => x.RigheD.Count());
                }
                return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "RESOCLI").Count();
            }
        }
        public decimal ValoreResoClienti
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00007")
                {
                    return this.ResoClienti * 0.15M;
                }
                else if (CodMandante == "00007")
                {
                    return this.ResoClienti * 25M;
                }
                else if (CodMandante == "00013")
                {
                    return this.ResoClienti * 0.45M;
                }
                else if (CodMandante == "00012")
                {
                    return this.ResoClienti * 17M;
                }
                else if (CodMandante == "00020")
                {
                    return this.ResoClienti * 17M;
                }
                else if (CodMandante == "00015")
                {
                    return this.ResoClienti * 10M;
                }
                else if (CodMandante == "00018")
                {
                    return this.ResoClienti * 17M;
                }
                return 0;
            }
        }
        public decimal TariffaPerc
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00002" || //DOMUS
                    CodMandante == "00010" || //FALQUI
                    CodMandante == "00008" || //PRAEVENIO
                    CodMandante == "00003" || //DALTON
                    CodMandante == "00016" || //NGF
                    CodMandante == "00011" || //PMS
                    CodMandante == "00025" || //DIGIPHARM
                    CodMandante == "00006")   //FARMAIMPRESA
                {
                    return DocumentiDelMandante.Sum(x => x.FatturazioneDocumento);
                }
                return 0;
            }
        }
        public int CampionaturaItalia
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00002")
                {
                    return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "OMAG" && (!string.IsNullOrEmpty(x.RegioneDestinazione)) && x.RegioneDestinazione.ToLower() != "campania").Count();
                }
                return 0;
            }
        }
        public decimal ValoreCampionaturaItalia
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00002")
                {
                    return this.CampionaturaItalia * 20M;
                }
                else if (CodMandante == "00016")
                {
                    return this.CampionaturaItalia * 20M;
                }
                return 0;
            }
        }
        public int CampionaturaCampania
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00002")
                {
                    return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "OMAG" && (!string.IsNullOrEmpty(x.RegioneDestinazione)) && x.RegioneDestinazione.ToLower() == "campania").Count();
                }
                else if (CodMandante == "00016")
                {
                    return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "OMAG" && (!string.IsNullOrEmpty(x.RegioneDestinazione)) && x.RegioneDestinazione.ToLower() == "campania").Count();
                }
                return 0;
            }
        }
        public decimal ValoreCampionaturaCampania
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00002")
                {
                    return this.CampionaturaCampania * 10M;
                }
                else if (CodMandante == "00016")
                {
                    return this.CampionaturaCampania * 15M;
                }
                return 0;
            }
        }
        public int InserimentoOrdini
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00020")
                {
                    return (int)Math.Ceiling((DirittiSegreteriaDDT * 10M) / 100M);
                }
                else if (CodMandante == "00012")
                {
                    return (int)Math.Ceiling((DirittiSegreteriaDDT * 10M) / 100M);
                }
                else if (CodMandante == "00018")
                {
                    var tt = DocumentiDelMandante.Count();//Where(x=>x.TipoMovimentazione == "OUT" || x.TipoMovimentazione == "OMAG").ToList();
                    return tt;
                }
                return 0;
            }
        }
        public decimal ValoreInserimentoOrdini
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;

                if (CodMandante == "00020")
                {
                    return this.InserimentoOrdini * 2.5M;
                }
                else if (CodMandante == "00012")
                {
                    return this.InserimentoOrdini * 2.5M;
                }
                else if (CodMandante == "00018")
                {
                    decimal qt5 = this.InserimentoOrdini * 20;
                    decimal qt6 = qt5 / 60;
                    var tt = Math.Ceiling(qt6) * 2.5M;
                    return tt;
                }
                return 0;
            }
        }
        public decimal ValoreDirittiDiSegreteria
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00007")
                {
                    return 300M;
                }
                else if (CodMandante == "00013")
                {
                    return 300M;
                }
                else if (CodMandante == "00012")
                {
                    return 800M;
                }
                else if (CodMandante == "00020")
                {
                    return 800M;
                }
                else if (CodMandante == "00018")
                {
                    return 800M;
                }
                return 0;
            }
        }
        public int DirittiSegreteriaDDT
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00002")
                {
                    return DocumentiDelMandante.Where(x => !string.IsNullOrEmpty(x.NazioneDestinazione) && x.NazioneDestinazione.ToLower() != "it").Count();

                }
                return DocumentiDelMandante.Count;
            }
        }
        public decimal ValoreDirittiSegreteriaDDT
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00002")
                {
                    return this.DirittiSegreteriaDDT * 25M;
                }
                else if (CodMandante == "00023")
                {
                    return this.DirittiSegreteriaDDT * 0.75M;
                }
                return 0;
            }
        }
        public decimal ValoreLavoroEconomia
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00024")
                {
                    //TODARIO: da applicare solo su lavorazioni urgenti che vengono richiesti direttamente dal cliente
                    var docStraordinari = DocumentiDelMandante.Where(x => x.NoteDDT != null && x.NoteDDT.ToLower().StartsWith("urgente")).ToList();
                    return docStraordinari.Count() * 21M;
                }
                else if (CodMandante == "00007")
                {
                    return 0;
                }
                return 0;
            }
        }
        public int Bollinatura { get; set; }
        public decimal ValoreBollinatura { get; set; }
        public decimal ValoreImballiEConfezionaturaForfait
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                return 30M;
            }
        } //se uguale a 0 inserire 30
        public decimal ValoreServiziInformatici { get; set; }
        public decimal ValoreSpeseVarie { get; set; }
        public int Ritiri
        {
            get
            {
                if (DocumentiDelMandante == null) return 0;
                if (CodMandante == "00019")
                {
                    return DocumentiDelMandante.Where(x => x.TipoMovimentazione == "IN").Count();
                }
                return 0;
            }
        }
        public decimal ValoreRitiri
        {
            get
            {
                if (CodMandante == "00019")
                {
                    return Ritiri * 1100;
                }
                return 0;
            }
        }
        public decimal ValoreFattura
        {
            get
            {
                return ValoreStockPuntaMax825 + ValoreStockPuntaMax28 + ValorePalletIN + ValorePalletOut + ValorePicking +
                    ValoreTracciatoMinisteriale + ValoreResoClienti + TariffaPerc + ValoreCampionaturaItalia + ValoreCampionaturaCampania +
                    ValoreInserimentoOrdini + ValoreDirittiDiSegreteria + ValoreDirittiSegreteriaDDT + ValoreLavoroEconomia + ValoreBollinatura + ValoreImballiEConfezionaturaForfait +
                    ValorePalletINNBOUND + ValoreCartoniXCM + ValoreRitiri + ValoreSpeseVarie + ValoreServiziInformatici;
            }
        }
        public List<TestataDocumento> DocumentiDelMandante { get; set; }
    }
}
