using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovimentiMagazzinoFromGespe
{
    public partial class FormFatturazione : Form
    {
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MinValue;
        List<Mandante> MandantiXCM = new List<Mandante>();

        List<uvwWmsDocument> dbDocuments = new List<uvwWmsDocument>();
        ContenitoriXCMEntities dbCont = new ContenitoriXCMEntities();
        DateTime dtnY = DateTime.Now;
        public FormFatturazione()
        {
            InitializeComponent();

            var now = DateTime.Now;

            bool IsGennaio = now.Month == 1;
            if (!IsGennaio)
            {
                var dinM = DateTime.DaysInMonth(now.Year, now.Month - 1);
                startDate = new DateTime(now.Year, now.Month - 1, 1);
                endDate = new DateTime(now.Year, now.Month - 1, dinM) + TimeSpan.FromDays(1) - TimeSpan.FromMinutes(1);
            }
            else
            {
                int m = 12;
                var dinM = DateTime.DaysInMonth(now.Year, 12);
                startDate = new DateTime(now.Year, now.Month, 1);
                endDate = new DateTime(now.Year, now.Month, dinM);
            }

            dateEditAccessiDal.DateTime = startDate;
            dateEditAccessiAl.DateTime = endDate;
            PopolaAnagrafica();
        }
        private void PopolaAnagrafica()
        {
            var dbAna = new XCM_WMSEntities().ANAGRAFICA_CLIENTI;

            foreach (var ana in dbAna)
            {
                var nA = new Mandante()
                {
                    ID_MANDANTE = ana.ID_ANAGRAFICA_CLIENTE,
                    CodMandante = ana.ID_GESPE,
                    Descrizione = ana.RAGIONE_SOCIALE,
                };
                MandantiXCM.Add(nA);
            }
            mandanteBindingSource.DataSource = MandantiXCM;
        }

        public static int sqtVivisol = 0;
        private void RecuperaColliEPallet(uvwWmsDocument document)
        {
            throw new NotImplementedException();
        }
        private List<RigheDocumento> CreaRigheDocumento(uvwWmsDocument document, List<uvwWmsDocumentRows_XCM> righeDelMese, string CodMandante, string TipoMovimentazione)
        {
            var resp = new List<RigheDocumento>();

            var righeDelDocumento = righeDelMese.Where(x => x.DocNum == document.DocNum).ToList();

            foreach (var rdd in righeDelDocumento)
            {

                var nR = new RigheDocumento()
                {
                    AliquotaIva = rdd.VatID,
                    CodiceProdotto = rdd.PrdCod,
                    ConfezioniPerCollo = (rdd.Packs != null) ? rdd.Packs.Value : 1,
                    DescrizioneProdotto = rdd.PrdDes,
                    GruppoProdotto = rdd.PrdGrp,
                    TemperaturaTrasporto = rdd.PrdTree,
                    ImportoUnitario = (rdd.NetSellPrice != null) ? rdd.NetSellPrice.Value : 0,
                    Lotto = rdd.Batchno,
                    PesoUnitario = (rdd.NetWeight != null) ? rdd.NetWeight.Value : 0,
                    Quantita = (rdd.Qty != null) ? rdd.Qty.Value : 0,
                    rowIdLink = (rdd.RowIdLink != null) ? rdd.RowIdLink.Value : 0,
                    Scadenza = (rdd.DateExpire != null) ? rdd.DateExpire.Value : DateTime.MinValue,
                    Sconto = (rdd.Discount != null) ? rdd.Discount.Value : 0,
                    uniq = rdd.uniq,
                    CodMandante = CodMandante,
                    MagazzinoLogicoRiga = rdd.LogWareID,
                    TipoMovimentazione = TipoMovimentazione,
                    ColliRiga = rdd.Boxes,
                    QtaXconf = rdd.GrdPackQty
                };
                resp.Add(nR);
            }

            return resp;
        }
        private TestataDocumento CreaNuovaTestata(uvwWmsDocument document)
        {
            var resp = new TestataDocumento()
            {
                Committente = document.ConsigneeName,
                Corriere = document.ShipUnLoadCarrierDes,
                DataDDT = document.DocDta,
                Destinatario = document.SenderName,
                DocNumGespe = document.DocNum,
                NumDDT = document.DocNum2,
                NomeDestinatazione = document.UnloadName,
                IndirizzoDestinazione = document.UnloadAddress,
                RegioneDestinazione = document.UnloadRegion,
                NazioneDestinazione = document.UnloadCountry,
                NoteDDT = document.ItemInfo,
                ProvDestinazione = document.UnloadDistrict,
                RifOrdine = document.Reference,
                ShipGespe = document.ShipDocNum,
                TipoMovimentazione = document.RegTypeID,
                TripGespe = document.TripDocNum,
                uniq = document.uniq,
                DataUltimaModifica = document.DtaTracking,
                NumeroColli = (document.TotalPacks != null) ? document.TotalPacks.Value : 0,
                NumeroPallet = (document.TotalPallets != null) ? document.TotalPallets.Value : 0,
                MagazzinoLogico = document.LogWareID
            };

            var ID_ORM = RilevaOrmGespe(document);
            ////var pltIN = RilevaPalletINBOUND(ID_ORM);
            PopolaCartoniUtilizzatiXCM(ID_ORM, resp);

            return resp;
        }
        private void PopolaCartoniUtilizzatiXCM(long iD_ORM, TestataDocumento testata)
        {
            string oo = "";
            if(dtnY.Month == 1)
            {
                var AA = (dtnY.Year-1).ToString().Substring(2);
                oo = AA.ToString()+iD_ORM.ToString();
            }
            else
            {
                var AA = dtnY.Year.ToString().Substring(2);
                oo = AA.ToString() + iD_ORM.ToString();
            }
            int ooi = int.Parse(oo);
            var cts = dbCont.REGISTRAZIONE_CONTENITORE.AsNoTracking().Where(x => x.ID_DOCUMENTO == ooi).ToList();

            foreach (var c in cts)
            {
                var tp = c.ANAGRAFICA_CONTENITORE;
                switch (tp)
                {
                    case 1:
                        testata.Cartone163148100 += c.QUANTITA_CONTENITORE;
                        break;
                    case 2:
                        testata.Cartone253211225 += c.QUANTITA_CONTENITORE;
                        break;
                    case 3:
                        testata.Cartone311311240 += c.QUANTITA_CONTENITORE;
                        break;
                    case 4:
                        testata.Cartone343148100 += c.QUANTITA_CONTENITORE;
                        break;
                    case 5:
                        testata.Cartone553378195 += c.QUANTITA_CONTENITORE;
                        break;
                    case 6:
                        testata.Cartone600400400 += c.QUANTITA_CONTENITORE;
                        break;
                    default:
                        break;
                }
            }


        }
        private long RilevaOrmGespe(uvwWmsDocument document)
        {
            int annoRif = 0;
            if (document.DocTip == 202)
            {
                return 0;
            }
            if(dtnY.Month == 1)
            {
                annoRif = dtnY.Year - 1;
            }
            else
            {
                annoRif = dtnY.Year;
            }
            var corr = dbDocuments.FirstOrDefault(x => x.ShipDocNum == document.ShipDocNum && x.DocTip == 203 && x.DocDta.Value.Year == annoRif);
            if (corr == null) return 0;
            return corr.uniq;
        }
        private void simpleButtonEsportaXslx_Click(object sender, EventArgs e)
        {

            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var savepath = Path.Combine(desktop, "Export Documenti Magazzino");
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }

            string finalDest = "";
            var r = gridViewFatturazione.GetFocusedRow() as Mandante;
            if (r != null)
            {
                finalDest = Path.Combine(savepath, $"Export_{r.Descrizione}_{DateTime.Now.ToString("ddMMyyyy")}.xlsx");
            }
            else
            {
                finalDest = Path.Combine(savepath, $"Export_Documenti_Magazzino_dal{startDate.ToString("ddMMyyyy")}_al_{endDate.ToString("ddMMyyyy")}.xlsx");
            }

            if (File.Exists(finalDest)) File.Delete(finalDest);
            gridViewFatturazione.ExportToXlsx(finalDest);

            Process.Start(savepath);
        }
        private void gridViewFatturazione_DoubleClick(object sender, EventArgs e)
        {
            var gv = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            if (gv != null && gv.FocusedColumn.FieldName == colDescrizione.FieldName)
            {
                var r = gridViewFatturazione.GetFocusedRow() as Mandante;

                if (r != null)
                {
                    if (r.ValoreFattura > 0)
                    {
                        var resp = MessageBox.Show(this, "Sono presenti già calcoli effettuati.\r\nVuoi sovrascriverli?", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (resp != DialogResult.Yes) return;
                    }

                    //if(dateEditAccessiDal.DateTime < DateTime.Now - TimeSpan.FromDays(60))
                    //{
                    //    MessageBox.Show(this, "Dati su GESPE non disponibili\r\nImpossibile proseguire per le date indicate", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    return;
                    //}
                    CalcolaRigaMandante(r);
                }
            }
        }
        object semaphoro = new object();
        private void CalcolaRigaMandante(Mandante r)
        {
            lock(semaphoro)
            {
                try
                {
                    if (r.CodMandante == "00007" && r.Descrizione != "VIVISOL")
                    {
                        SviluppaDatiPerMagazzinoLogico(r);
                        return;
                    }
                    r.DocumentiDelMandante = new List<TestataDocumento>();
                    var db = new GnXcmEntities();
                    var dbPIN = new XCM_WMSEntities();

                    #region PalletPuntaMax
                    //var giacCli = db.uvwWmsWarehouse.Where(x => x.CustomerID == r.CodMandante).ToList();

                    //int PuntaMAX = 0;
                    //int StockNuovoMese = 0;
                    //int StockMesePrecedente = dbPIN.PALLET_PUNTA_MAX.FirstOrDefault(x => x.ANAGRAFICA_CLIENTI.ID_GESPE == r.CodMandante && x.DATA_RILEVAZIONE.Month == startDate.Month).PALLET_STOCK;

                    //var movDelMese = db.uvwWmsRegistrations.Where(x => x.DateReg >= startDate && x.DateReg <= endDate && x.CustomerID == r.CodMandante && (x.RegTypeID == "OUT" | x.RegTypeID == "IN" || x.RegTypeID == "MAN")).OrderBy(y=>y.DateReg).GroupBy(x=>x.DateReg.Value.Day).ToList();

                    //foreach(var mdm in movDelMese)
                    //{
                    //    var mord = mdm.OrderBy(x => x.RegTypeID).ToList();
                    //    foreach(var m in mord)
                    //    {
                    //        if (m.RegTypeID == "IN")
                    //        {
                    //            PuntaMAX += mord.Count;
                    //            continue;
                    //        }
                    //        else if (m.RegTypeID == "OUT")
                    //        {

                    //        }
                    //        else
                    //        {

                    //        }
                    //    }

                    //} 
                    #endregion

                    Cursor = Cursors.WaitCursor;
                    decimal pp = progressBar1.Maximum / 5;
                    progressBar1.Value = (int)Math.Ceiling(pp);
                    gridViewFatturazione.BeginUpdate();
                    AggiornaDocumentiDB(r.CodMandante);
                    //List<TestataDocumento> documents = new List<TestataDocumento>();

                    List<uvwWmsDocument> DocDelMese = db.uvwWmsDocument.Where(x => x.CustomerID == r.CodMandante && (x.DocTip == 203 || x.DocTip == 202) && x.DocDta >= startDate && x.DocDta <= endDate).OrderBy(x => x.DocDta).ToList();
                    List<uvwWmsDocumentRows_XCM> RigheDelMese = db.uvwWmsDocumentRows_XCM.Where(x => x.CustomerID == r.CodMandante && (x.DocTip == 203 || x.DocTip == 202) && x.DocDta >= startDate && x.DocDta <= endDate).ToList();
                    List<uvwWmsRegistrations> RigheRegistrazione = db.uvwWmsRegistrations.Where(x => x.CustomerID == r.CodMandante && x.DateReg >= startDate && x.DateReg <= endDate).ToList();
                    List<PALLET_IN> pIN = dbPIN.PALLET_IN.Where(x => x.DATA_INSERIMENTO >= startDate && x.DATA_INSERIMENTO <= endDate && x.ANAGRAFICA_CLIENTI.ID_GESPE == r.CodMandante).ToList();
                    r.PalletINBOUND = pIN.Sum(x => x.PALLET_IN1);
                    int cc = DocDelMese.Count();
                    int cs = 0;
                    progressBar1.Maximum = cc;

                    foreach (var document in DocDelMese)
                    {
                        if (document.DocTip == 202 && document.ItemStatus == 40)//bem annullata
                        {
                            continue;
                        }
                        else if (document.DocTip == 203 && document.ItemStatus == 50)// ddt annullato
                        {
                            continue;
                        }

                        Console.WriteLine(cs);
                        var rigeRegDoc = RigheRegistrazione.Where(x => x.UniqDoc == document.uniq).ToList();
                        bool DocIN = document.DocTip == 202;
                        TestataDocumento nr = new TestataDocumento();
                        List<RigheDocumento> nRighe = new List<RigheDocumento>();
                        try
                        {
                            nr = CreaNuovaTestata(document);
                        }
                        catch (Exception ee)
                        {


                        }
                        try
                        {
                            nRighe = CreaRigheDocumento(document, RigheDelMese, r.CodMandante, nr.TipoMovimentazione);
                        }
                        catch (Exception ee)
                        {


                        }
                        nr.CodMandanteTestata = r.CodMandante;

                        if (DocIN)
                        {
                            var rr = rigeRegDoc.GroupBy(x => x.BarcodeExt).ToList();
                            nr.NumeroPallet = rr.Count();
                        }
                        else
                        {
                            var rr = rigeRegDoc.GroupBy(x => x.BarcodeExt).ToList();
                            nr.NumeroPallet = rr.Count();
                        }
                        nr.RigheD = nRighe;
                        r.DocumentiDelMandante.Add(nr);
                        progressBar1.Value = cs;

                        cs++;

                    }
                    //r.StockPuntaMax825 = CalcolaPuntaMAX825(r, RigheRegistrazione);
                    //r.StockPuntaMax28 = CalcolaPuntaMAX28(r, RigheRegistrazione);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
                finally
                {
                    progressBar1.Value = progressBar1.Maximum;
                    gridViewFatturazione.EndUpdate();
                    gridViewFatturazione.RefreshData();

                    //progressBar1.Visible = false;
                    Cursor = Cursors.Default;
                }
            }
          
        }

        private int CalcolaPuntaMAX28(Mandante mandante, List<uvwWmsRegistrations> righeRegistrazione)
        {
            return 0;
        }

        private int CalcolaPuntaMAX825(Mandante mandante, List<uvwWmsRegistrations> righeRegistrazione)
        {
            var DocOrderDTA = mandante.DocumentiDelMandante.OrderBy(x => x.DataDDT).ToList();
            var db = new XCM_WMSEntities();
            var dbPM = db.PALLET_MAX.Where(x => x.FK_CLIENTE == mandante.ID_MANDANTE).ToList();
            DateTime fromDTA = endDate;
            int start = dbPM.FirstOrDefault(x => x.DATA_PALLET_START.Month == endDate.Month - 1).PALLET_START;
            int puntaMax = 0;
            int end = 0;
            DateTime dataPTMAX = DateTime.MinValue;
            foreach (var dt in DocOrderDTA)
            {
                if (dt.TipoMovimentazione == "IN" || dt.TipoMovimentazione == "RESOCLI" || dt.TipoMovimentazione == "RESORFA")
                {
                    start += (int)dt.NumeroPallet;
                    if (puntaMax < start)
                    {
                        puntaMax = start;
                        dataPTMAX = dt.DataDDT.Value;
                    }
                }
                else
                {
                    //start -= (int)dt.NumeroPallet;
                }
                end = start;
            }

            var nuovoPmax = new PALLET_MAX()
            {
                DATA_PALLET_PUNTA_MAX = dataPTMAX,
                FK_CLIENTE = mandante.ID_MANDANTE,
                DATA_PALLET_START = DocOrderDTA.Last().DataDDT.Value,
                PALLET_PUNTA_MAX = puntaMax,
                PALLET_START = start,

            };
            db.PALLET_MAX.Add(nuovoPmax);
            db.SaveChanges();

            return puntaMax;

        }

        private void SviluppaDatiPerMagazzinoLogico(Mandante r)
        {
            r.DocumentiDelMandante = new List<TestataDocumento>();
            var db = new GnXcmEntities();
            var dbPIN = new XCM_WMSEntities();

            var maglog = (r.Descrizione.ToUpper() == "ASLNANORD") ? "ASLNANORD" : "VIVISOLNA";

            Cursor = Cursors.WaitCursor;
            decimal pp = progressBar1.Maximum / 5;
            progressBar1.Value = (int)Math.Ceiling(pp);
            gridViewFatturazione.BeginUpdate();
            AggiornaDocumentiDB(r.CodMandante);
            //List<TestataDocumento> documents = new List<TestataDocumento>();
            List<uvwWmsDocument> DocDelMese = db.uvwWmsDocument.Where(x => x.CustomerID == r.CodMandante && (x.DocTip == 204 || x.DocTip == 202) && x.DocDta >= startDate && x.DocDta <= endDate).OrderBy(x => x.DocDta).ToList();
            List<uvwWmsDocumentRows_XCM> RigheDelMese = db.uvwWmsDocumentRows_XCM.Where(x => x.CustomerID == r.CodMandante && (x.DocTip == 204 || x.DocTip == 202) && x.DocDta >= startDate && x.DocDta <= endDate && x.LogWareID == maglog).ToList();
            List<uvwWmsRegistrations> RigheRegistrazione = db.uvwWmsRegistrations.Where(x => x.CustomerID == r.CodMandante && x.DateReg >= startDate && x.DateReg <= endDate).ToList();
            List<PALLET_IN> pIN = dbPIN.PALLET_IN.Where(x => x.DATA_INSERIMENTO >= startDate && x.DATA_INSERIMENTO <= endDate && x.ANAGRAFICA_CLIENTI.ID_GESPE == r.CodMandante && x.ANAGRAFICA_CLIENTI.RAGIONE_SOCIALE == maglog).ToList();
            r.PalletINBOUND = pIN.Sum(x => x.PALLET_IN1);
            int cc = DocDelMese.Count();
            int cs = 0;
            progressBar1.Maximum = cc;
            foreach (var document in DocDelMese)
            {
                Console.WriteLine(cs);
                var rigeRegDoc = RigheRegistrazione.Where(x => x.UniqDoc == document.uniq).ToList();
                bool DocIN = document.DocTip == 202;
                TestataDocumento nr = CreaNuovaTestata(document);
                List<RigheDocumento> nRighe = CreaRigheDocumento(document, RigheDelMese, r.CodMandante, nr.TipoMovimentazione);

                if (nRighe.Count > 0)
                {
                    nr.CodMandanteTestata = r.CodMandante;
                    if (DocIN)
                    {
                        var rr = rigeRegDoc.GroupBy(x => x.BarcodeExt).ToList();
                        nr.NumeroPallet = rr.Count();
                    }
                    else
                    {
                        var rr = rigeRegDoc.GroupBy(x => x.BarcodeExt).ToList();
                        nr.NumeroPallet = rr.Count();
                    }
                    nr.RigheD = nRighe;
                    r.DocumentiDelMandante.Add(nr);
                }
                progressBar1.Value = cs;
                cs++;

            }
        }
        private void dateEditAccessiDal_EditValueChanged(object sender, EventArgs e)
        {
            startDate = dateEditAccessiDal.DateTime;
        }
        private void dateEditAccessiAl_EditValueChanged(object sender, EventArgs e)
        {
            endDate = dateEditAccessiAl.DateTime;
        }
        private void gridViewTestateDocumenti_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            //if (e.Column.FieldName == colTipoMovimentazione.FieldName || e.Column.FieldName == colTipoMovimentazione1.FieldName)
            //{
            //    if (e.Value.ToString() == "OUT" || e.Value.ToString() == "TRASF")
            //    {
            //        e.DisplayText = "USCITA";
            //    }
            //    else
            //    {
            //        e.DisplayText = "INGRESSO";
            //    }
            //}
        }
        private void buttonMeseCorrente_Click(object sender, EventArgs e)
        {
            var dtn = DateTime.Now;
            var giorniDelMese = DateTime.DaysInMonth(dtn.Year, dtn.Month);

            dateEditAccessiDal.DateTime = new DateTime(dtn.Year, dtn.Month, 01);
            dateEditAccessiAl.DateTime = new DateTime(dtn.Year, dtn.Month, giorniDelMese);
        }
        private void buttonOggi_Click(object sender, EventArgs e)
        {
            var dtn = DateTime.Now;
            dateEditAccessiDal.DateTime = new DateTime(dtn.Year, dtn.Month, dtn.Day, 00, 00, 01);
            dateEditAccessiAl.DateTime = new DateTime(dtn.Year, dtn.Month, dtn.Day, 23, 59, 59);
        }
        private void buttonMeseScorso_Click(object sender, EventArgs e)
        {
            var dtn = DateTime.Now;
            if (dtn.Month == 1)
            {
                dateEditAccessiDal.DateTime = new DateTime(dtn.Year-1, 12, 01);
                dateEditAccessiAl.DateTime = new DateTime(dtn.Year-1, 12, 31);
            }
            else
            {
                var giorniDelMese = DateTime.DaysInMonth(dtn.Year, dtn.Month - 1);

                dateEditAccessiDal.DateTime = new DateTime(dtn.Year, dtn.Month - 1, 01);
                dateEditAccessiAl.DateTime = new DateTime(dtn.Year, dtn.Month - 1, giorniDelMese);
            }
        }
        private void gridViewTestateDocumenti_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.CellValue == null) return;
            if (e.Column.FieldName == gridColumnCT1.FieldName)
            {
                if ((decimal)e.CellValue > 0)
                {
                    e.Appearance.BackColor = Color.PaleGreen;
                }
            }
            else if (e.Column.FieldName == gridColumnCT2.FieldName)
            {
                if ((decimal)e.CellValue > 0)
                {
                    e.Appearance.BackColor = Color.PaleGreen;
                }
            }
            else if (e.Column.FieldName == gridColumnCT3.FieldName)
            {
                if ((decimal)e.CellValue > 0)
                {
                    e.Appearance.BackColor = Color.PaleGreen;
                }
            }
            else if (e.Column.FieldName == gridColumnCT4.FieldName)
            {
                if ((decimal)e.CellValue > 0)
                {
                    e.Appearance.BackColor = Color.PaleGreen;
                }
            }
            else if (e.Column.FieldName == gridColumnCT5.FieldName)
            {
                if ((decimal)e.CellValue > 0)
                {
                    e.Appearance.BackColor = Color.PaleGreen;
                }
            }

        }
        private void FormFatturazione_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    Cursor = Cursors.WaitCursor;
            //    AggiornaDocumentiDB();
            //}
            //finally
            //{
            //    Cursor = Cursors.Default;
            //}
        }
        private void AggiornaDocumentiDB(string idGespe)
        {
            var from = startDate - TimeSpan.FromDays(10);
            var to = endDate + TimeSpan.FromDays(10);
            dbDocuments = new GnXcmEntities().uvwWmsDocument.Where(x => x.DocDta >= from && x.DocDta <= to && x.CustomerID == idGespe).ToList();
        }
        private void gridViewFatturazione_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                //DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                //if (e.Column.FieldName == gridColumnCartoniXCM.FieldName)
                //{
                //    var r = view.GetRow(e.ListSourceRowIndex) as Mandante;

                //    if (r != null)
                //    {
                //        if(r.DocumentiDelMandante != null)
                //        {
                //            e.Value = r.DocumentiDelMandante.Sum(x => x.Cartone163148100 + x.Cartone253211225 + x.Cartone311311240 + x.Cartone343148100 + x.Cartone553378195);
                //        }
                //    }
                //}
            }
        }
        private void buttonTuttiTraLeDate_Click(object sender, EventArgs e)
        {
            var mgrid = mandanteBindingSource.DataSource as List<Mandante>;
            if (mgrid != null)
            {
                foreach (var m in mgrid)
                {

                    CalcolaRigaMandante(m);
                    Application.DoEvents();

                }
            }
        }

        private void gridViewRigheDocumento_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.CellValue == null) return;
            if (e.Column.FieldName == colImportoUnitario.FieldName)
            {
                if ((decimal)e.CellValue == 0)
                {
                    e.Appearance.BackColor = Color.Tomato;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormDettaglioRigheDocumenti righeDocumenti = new FormDettaglioRigheDocumenti();
            righeDocumenti.Show();
        }

		private void buttonPalletIN_Click(object sender, EventArgs e)
		{
            FormPalletIN palletIn = new FormPalletIN();
            palletIn.Show();
		}
	}
}
