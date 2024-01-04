using DevExpress.DataAccess.Excel;
using DevExpress.XtraEditors;
using DevExpress.XtraExport.Csv;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Windows.Forms;
using PoolingFileDaElaborare;
using DevExpress.Spreadsheet;

namespace EzOrdiniRemoti
{
    public partial class FormOrdini : XtraForm
    {
        FTPClass ftp = new FTPClass();
        public FormOrdini()
        {
            InitializeComponent();

        }

        string tracciatoDaUtilizzare = Properties.Settings.Default.TracciatoDefault;
        List<VociOrdine> ProdottiDaInserire = new List<VociOrdine>();
        string FileDaImportareGlobal = "";
        bool reteInterna = Properties.Settings.Default.reteInterna;
        string mandanteAttivo = "";
        FluentFTP.FtpClient _clientFTP = null;


        private void AzzeraTutto()
        {
            //textEditCapDestinazione.Text = textEditCapFatturazione.Text = textEditCittaDestinazione.Text = textEditCittaFatturazione.Text = textEditIndirizzoDestinazione.Text =
            //    textEditIndirizzoFatturazione.Text = textEditNote.Text = textEditOrdine.Text = textEditPIva.Text = textEditProvDestinazione.Text = textEditProvFatturazione.Text = "";
            //comboBoxEditDestinazione.SelectedIndex = comboBoxEditFatturazione.SelectedIndex = -1;
            FileDaImportareGlobal = "";
            gridViewProdotti.BeginUpdate();
            ProdottiDaInserire.Clear();
            gridViewProdotti.EndUpdate();
            pdfViewer1.Visible = false;
            memoEdit1.Visible = false;
            gridControlExcelIngresso.Visible = false;
            labelControlTrascina.Visible = true;
            //comboBoxEdit1.Text = "0";
            //mandanteAttivo = "";
            try
            {
                if (_clientFTP != null)
                {
                    _clientFTP.Disconnect();
                    _clientFTP.Dispose();
                    _clientFTP = null;
                }
            }
            catch (Exception)
            {
                _clientFTP = null;
            }

        }
        private void storicoOrdiniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Apri nuova form con griglia che elenca lo storico degli ordini
        }

        private void anagraficaDestinatariToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Apre nuova form con griglia per anagrafica destinatari
        }

        private void anagraficaFatturazioneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Apre nuova form con griglia per anagrafica fatturazione
        }

        private void anagraficaArticoliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Apre nuova form con griglia per anagrafica articoli
        }

        private void personalizzazioniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Apri form impostazioni
        }

        private void manualeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Manuale d'uso
        }

        private void assistenzaeRemotaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Apri Teamviewer
        }

        private void informazioniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Questo programma è stato sviluppato da:\r\nD'Isa Piero\r\nmailto:p.disa@xcmhealthcare.com\r\nPer uso esclusivo di clienti XCM");
        }

        private void eMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Apri form gestione mail mittente
        }

        private void FormOrdini_Load(object sender, EventArgs e)
        {
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(splitContainerControl1_Panel2_DragEnter);
            this.DragDrop += new DragEventHandler(splitContainerControl1_Panel2_DragDrop);
            //this.WindowState = FormWindowState.Maximized;
            var utilizzaDBLocali = Properties.Settings.Default.AttivaDBLocale;
            if (utilizzaDBLocali)
            {
                checkEditSalvaNuoviDatiDestinazione.Checked = checkEditSalvaNuoviDatiFatturazione.Checked = checkEditSalvaNuoviProdotti.Checked = utilizzaDBLocali;
                checkEditSalvaNuoviDatiDestinazione.Visible = checkEditSalvaNuoviDatiFatturazione.Visible = checkEditSalvaNuoviProdotti.Visible = utilizzaDBLocali;
            }
            else
            {
                checkEditSalvaNuoviDatiDestinazione.Checked = checkEditSalvaNuoviDatiFatturazione.Checked = checkEditSalvaNuoviProdotti.Checked = !utilizzaDBLocali;
                checkEditSalvaNuoviDatiDestinazione.Visible = checkEditSalvaNuoviDatiFatturazione.Visible = checkEditSalvaNuoviProdotti.Visible = !utilizzaDBLocali;
            }
            vociOrdineBindingSource.DataSource = ProdottiDaInserire;
        }

        private void simpleButtonInviaOrdine_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FileDaImportareGlobal))
            {
                MessageBox.Show($"Non è stato rilevato alcun file da processare, impossibile continuare", "Errore lettura file", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            #region ControlloDatiFTP
            //if (!reteInterna)
            //{
            //    DammiLeCredenzialiFTP(mandanteAttivo, out string usr, out string psw);

            //    _clientFTP = new FTPClass().CreaClientFTP(usr, psw);

            //    if (_clientFTP == null)
            //    {
            //        MessageBox.Show($"Errore nella creazione del client ftp, impossibile continuare", "Errore FTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //}
            #endregion
            try
            {
                var fdi = new PoolingFileDaElaborare.FileDaImportare
                {
                    DataInserimento = DateTime.Now,
                    PathCompleto = FileDaImportareGlobal,
                };
                string vett = comboBoxEditVettore.Text;
                string pathCSV = "";
                if (mandanteAttivo.ToLower() == "falqui" || mandanteAttivo.ToLower() == "kps" || mandanteAttivo.ToLower() == "aps")
                {

                    pathCSV = TextToCsv.ConvertiXSLtoCSV(FileDaImportareGlobal, mandanteAttivo, vett);
                    if (!string.IsNullOrWhiteSpace(pathCSV))
                    {
                        DammiLeCredenzialiFTP(mandanteAttivo.ToLower(), out string usr, out string psw);
                        _clientFTP = new FTPClass().CreaClientFTP(usr, psw);

                        if (_clientFTP == null)
                        {
                            MessageBox.Show($"Errore nella creazione del client ftp, impossibile continuare", "Errore FTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var D = Path.GetDirectoryName(fdi.PathCompleto);
                        var test = _clientFTP.GetListing();
                        var justName = Path.GetFileName(pathCSV);
                        _clientFTP.UploadFile(pathCSV, Path.GetFileName(pathCSV), FluentFTP.FtpRemoteExists.Overwrite, false, FluentFTP.FtpVerify.None);
                        File.Delete(pathCSV);
                        File.Delete(fdi.PathCompleto);
                        AzzeraTutto();
                    }
                    else
                    {
                        MessageBox.Show($"Errore nella conversione del csv\r\nimpossibile continuare", "Errore conversione file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }
                var Verificati = vociOrdineBindingSource.DataSource as List<PoolingFileDaElaborare.VociOrdine>;

                if (Verificati == null)
                {
                    MessageBox.Show($"Errore in lettura griglia, impossibile continuare", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                var excelTemp = PoolingFileDaElaborare.TextToCsv.CopiaModelloExcel(fdi.PathCompleto);


                pathCSV = PoolingFileDaElaborare.TextToCsv.PopolaIlFileExcelConIlNuovoOrdineESalvaInCSV(Verificati, excelTemp, vett);

                if (reteInterna)
                {
                    if (!string.IsNullOrEmpty(pathCSV))
                    {
                        DammiLeCredenzialiFTP(mandanteAttivo, out string usr, out string psw);

                        _clientFTP = new FTPClass().CreaClientFTP(usr, psw);

                        if (_clientFTP == null)
                        {
                            MessageBox.Show($"Errore nella creazione del client ftp, impossibile continuare", "Errore FTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var D = Path.GetDirectoryName(fdi.PathCompleto);
                        var test = _clientFTP.GetListing();
                        var justName = Path.GetFileNameWithoutExtension(pathCSV);
                        justName = justName + DateTime.Now.ToString("ddMMYYYY_mmssffff") + ".csv";
                        _clientFTP.UploadFile(pathCSV, Path.GetFileName(pathCSV), FluentFTP.FtpRemoteExists.Overwrite, false, FluentFTP.FtpVerify.None);

                        try
                        {
                            pdfViewer1.CloseDocument();

                            File.Delete(pathCSV);
                            File.Delete(fdi.PathCompleto);
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show($"Ordine inviato correttamente ma non sono riuscito a cancellare il file originale, probabilmente è aperto!!", "Errore Cancellazione", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        MessageBox.Show($"Errore nella produzione del csv da inviare, impossibile continuare", "Errore CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }
                else
                {
                    //Eseguire invio tramite FTP e controllo integrità del file
                }

                AzzeraTutto();
                mandanteAttivo = "";
                comboBoxEditMandante.EditValue = 0;
                comboBoxEditVettore.EditValue = 0;
                comboBoxEditMandante.Text = "";
                comboBoxEditVettore.Text = "";
            }
            catch (Exception ee)
            {
                MessageBox.Show($"Errore nell'elaborazione dell'ordine\r\nVerificare i dati inseriti\r\n\r\nMsg di errore: {ee.Message}",
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void DammiLeCredenzialiFTP(string mandanteAttivo, out string usr, out string psw)
        {
            usr = "";
            psw = "";

            if (mandanteAttivo.ToLower() == "pms")
            {
                usr = "pms";
                psw = "pms";
            }
            else if (mandanteAttivo.ToLower() == "farmaimpresa")
            {
                usr = "farmaimpresa";
                psw = "farmaimpresa";
            }
            else if (mandanteAttivo.ToLower() == "polaris")
            {
                usr = "polaris";
                psw = "polaris";
            }
            else if (mandanteAttivo.ToLower() == "falqui")
            {
                usr = "falqui";
                psw = "falqui";
            }
            else if (mandanteAttivo.ToLower() == "vivisol")
            {
                usr = "vivisol";
                psw = "vivisol";
            }
            else if (mandanteAttivo.ToLower() == "vivisolasl")
            {
                usr = "vivisolasl";
                psw = "vivisolasl";
            }
            else if (mandanteAttivo.ToLower() == "kps")
            {
                usr = "kps";
                psw = "kps";
            }
            else if (mandanteAttivo.ToLower() == "aps")
            {
                usr = "apsCSV";
                psw = "!aps@";
            }

        }


        #region Drag&Drop
        private void splitContainerControl1_Panel2_DragDrop(object sender, DragEventArgs e)
        {
            AzzeraTutto();
            if (string.IsNullOrEmpty(comboBoxEditMandante.Text) || comboBoxEditMandante.Text == "0")
            {
                MessageBox.Show($"Mandante non selezionato, impossibile continuare",
                   "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (files.Count() > 1)
            {
                MessageBox.Show("Trascinare un file per volta", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            FileDaImportareGlobal = files[0];

            if (Path.GetExtension(FileDaImportareGlobal) == ".txt")
            {
                pdfViewer1.Visible = false;
                gridControlExcelIngresso.Visible = false;
                memoEdit1.Text = File.ReadAllText(FileDaImportareGlobal);
                memoEdit1.Visible = true;
                labelControlTrascina.Visible = false;

            }
            else if (Path.GetExtension(FileDaImportareGlobal) == ".pdf")
            {
                memoEdit1.Visible = false;
                gridControlExcelIngresso.Visible = false;
                pdfViewer1.LoadDocument(FileDaImportareGlobal);
                pdfViewer1.Visible = true;
                labelControlTrascina.Visible = false;

            }
            else if (Path.GetExtension(FileDaImportareGlobal).ToLower() == ".xls" || (Path.GetExtension(FileDaImportareGlobal).ToLower() == ".xlsx"))
            {
                //if (mandanteAttivo.ToLower() == "aps")
                //{
                //    simpleButtonInviaOrdine_Click(null, null);
                //    MessageBox.Show(this,"Import aps terminato", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                memoEdit1.Visible = false;
                pdfViewer1.Visible = false;
                labelControlTrascina.Visible = false;
                gridControlExcelIngresso.Visible = true;

                //Leggere il documento excel come file (loaddocument)

                Workbook wb = new Workbook();

                wb.LoadDocument(FileDaImportareGlobal);

                var wk = wb.Worksheets[0].Name;

                //Recuperare i worksheet indipendentemente da nome

                ExcelDataSource source = new ExcelDataSource();
                source.FileName = FileDaImportareGlobal;

                ExcelWorksheetSettings worksheetSettings = new ExcelWorksheetSettings();
                if (mandanteAttivo.ToLower() == "falqui")
                {
                    worksheetSettings.WorksheetName = "Sheet 1";
                }
                else if (mandanteAttivo.ToLower() == "aps")
                {

                    worksheetSettings.WorksheetName = wk;
                }
                else
                {
                    worksheetSettings.WorksheetName = "Foglio1";
                }

                ExcelSourceOptions sourceOptions = new ExcelSourceOptions();
                sourceOptions.ImportSettings = worksheetSettings;
                sourceOptions.SkipHiddenRows = false;
                sourceOptions.SkipHiddenColumns = false;

                source.SourceOptions = sourceOptions;
                source.Fill();

                gridControlExcelIngresso.DataSource = source;
            }
            AnalizzaEdElaboraFile(FileDaImportareGlobal);
        }

        private void splitContainerControl1_Panel2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        #endregion

        int OrdiniNellExcel = 1;
        private void AnalizzaEdElaboraFile(string daImportare)
        {
            if (!string.IsNullOrEmpty(tracciatoDaUtilizzare))
            {
                //TODO: richiedere mandante
            }
            //mandanteAttivo = comboBoxEdit1.Text;
            var selezionato = new PoolingFileDaElaborare.FileDaImportare()
            {
                DataInserimento = DateTime.Now,
                MsgStato = "Da Elaborare",
                PathCompleto = daImportare,

            };

            if (mandanteAttivo.ToLower() == "pms")
            {
                if (selezionato.Tipo.ToLower() == ".txt")
                {

                    var testo = File.ReadAllText(selezionato.PathCompleto);

                    var NuovoOrdine = PoolingFileDaElaborare.TextToCsv.ConvertiTestoOrdinePMS(testo, out Exception ex);
                    if (NuovoOrdine != null && NuovoOrdine.Count > 1)
                    {
                        //PopolaAnagraficheENote(NuovoOrdine[0]);
                        PopolaGrigliaConIProdotti(NuovoOrdine);

                    }

                    //if (ex != null)
                    //{
                    //    var eeeer = Path.Combine(Path.GetDirectoryName(selezionato.PathCompleto), "ELABORATI\\IN_ERRORE");
                    //    if (!Directory.Exists(eeeer)) Directory.CreateDirectory(eeeer);
                    //    var nonf = Path.Combine(eeeer, selezionato.NomeFileEXT);
                    //    File.Copy(selezionato.PathCompleto, nonf);
                    //    File.Delete(selezionato.PathCompleto);
                    //    FileDaImportareGlobal = "";
                    //    PoolingFileDaElaborare.GestoreMail.InviaMail($"Non è stato possibile importare in automatico il file {selezionato.NomeFileEXT} a causa del seguente errore:\r\n{ex.Message}",
                    //        $"Errore importazione per il documento {selezionato.NomeFileEXT}", null, null);
                    //    //Invia mail con eccezione a qualcuno per inserimento manuale
                    //}
                }
                else
                {
                    MessageBox.Show($"Non è ancora disponibile il tracciato per il file richiesto\r\nImpossibile continuare", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    FileDaImportareGlobal = "";
                    return;
                }
            }
            else if (mandanteAttivo.ToLower() == "farmaimpresa")
            {
                if (selezionato.Tipo.ToLower() == ".pdf")
                {
                    var testoPDF = "";
                    try
                    {
                        testoPDF = PoolingFileDaElaborare.PdfClass.ReadPDF(selezionato.PathCompleto);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show($"Errore nell'elaborazione del documento pdf\r\n\r\nMsg di errore: {ee.Message}\r\n\r\nImpossibile continuare",
                                "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FileDaImportareGlobal = "";
                        return;
                    }


                    Exception ex = null;

                    var NuovoOrdine = PoolingFileDaElaborare.TextToCsv.ConvertiTestoOrdineFarmaImpresa(testoPDF, out ex);
                    if (NuovoOrdine != null && NuovoOrdine.Count >= 1)
                    {
                        //PopolaAnagraficheENote(NuovoOrdine[0]);
                        PopolaGrigliaConIProdotti(NuovoOrdine);
                    }

                    if (ex != null)
                    {
                        //var eeeer = Path.Combine(Path.GetDirectoryName(selezionato.PathCompleto), "ELABORATI\\IN_ERRORE");
                        //if (!Directory.Exists(eeeer)) Directory.CreateDirectory(eeeer);
                        //var nonf = Path.Combine(eeeer, selezionato.NomeFileEXT);
                        //File.Copy(selezionato.PathCompleto, nonf);
                        //File.Delete(selezionato.PathCompleto);
                        FileDaImportareGlobal = "";
                        AzzeraTutto();
                        //PoolingFileDaElaborare.GestoreMail.InviaMail($"Non è stato possibile importare in automatico il file {selezionato.NomeFileEXT} a causa del seguente errore:\r\n{ex.Message}",
                        //    $"Errore importazione per il documento {selezionato.NomeFileEXT}", null, null);
                        //Invia mail con eccezione a qualcuno per inserimento manuale
                    }
                }
                else
                {
                    MessageBox.Show($"Non è ancora disponibile il tracciato per il file richiesto\r\nImpossibile continuare", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            else if (mandanteAttivo.ToLower() == "polaris")
            {
                var NuovoOrdine = PoolingFileDaElaborare.TextToCsv.ConvertiInOrdinePolaris(selezionato, out Exception ex, out OrdiniNellExcel);
                if (NuovoOrdine != null && NuovoOrdine.Count >= 1)
                {
                    //PopolaAnagraficheENote(NuovoOrdine[0]);
                    PopolaGrigliaConIProdotti(NuovoOrdine);
                }

                if (ex != null)
                {
                    var eeeer = Path.Combine(Path.GetDirectoryName(selezionato.PathCompleto), "ELABORATI\\IN_ERRORE");
                    if (!Directory.Exists(eeeer)) Directory.CreateDirectory(eeeer);
                    var nonf = Path.Combine(eeeer, selezionato.NomeFileEXT);
                    File.Copy(selezionato.PathCompleto, nonf);
                    File.Delete(selezionato.PathCompleto);
                    FileDaImportareGlobal = "";
                    PoolingFileDaElaborare.GestoreMail.InviaMail($"Non è stato possibile importare in automatico il file {selezionato.NomeFileEXT} a causa del seguente errore:\r\n{ex.Message}",
                        $"Errore importazione per il documento {selezionato.NomeFileEXT}", null, null);
                    //Invia mail con eccezione a qualcuno per inserimento manuale
                }

            }
            else if (mandanteAttivo.ToLower() == "falqui")
            {
                //MessageBox.Show($"Fare riferimento solo alla griglia sottostante del file importato\r\nSe tutti i valori sono corretti si può procedere all'invio ordine", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
            else if (mandanteAttivo.ToLower() == "vivisol asl")
            {
                //MessageBox.Show($"Fare riferimento solo alla griglia sottostante del file importato\r\nSe tutti i valori sono corretti si può procedere all'invio ordine", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
            else if (mandanteAttivo.ToLower() == "vivisol")
            {
                //MessageBox.Show($"Fare riferimento solo alla griglia sottostante del file importato\r\nSe tutti i valori sono corretti si può procedere all'invio ordine", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
            else if (mandanteAttivo.ToLower() == "kps")
            {

            }
            else if (mandanteAttivo.ToLower() == "aps")
            {
                //TextToCsv.ConvertiXSLtoCSV(daImportare, mandanteAttivo.ToLower(), "UNITEX");
            }
            #region Domus TODO
            /*else if (selezionato.PathFile.ToLower() == "domus")
                {

                    if (selezionato.Tipo.ToLower() == ".pdf")
                    {
                        var testoPDF = "";
                        try
                        {
                            testoPDF = PoolingFileDaElaborare.PdfClass.ReadPDF(selezionato.PathCompleto);
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show($"Errore nell'elaborazione del documento pdf\r\n\r\nMsg di errore: {ee.Message}\r\n\r\nImpossibile continuare",
                                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            FileDaImportareGlobal = "";
                            return;
                        }


                        Exception ex = null;
                        var fDest = Path.Combine(Path.GetDirectoryName(selezionato.PathCompleto), "ELABORATI", selezionato.NomeFileEXT);
                        var NuovoOrdine = PoolingFileDaElaborare.TextToCsv.ConvertiTestoOrdineDomus(testoPDF, out ex);
                        if (NuovoOrdine != null && NuovoOrdine.Count > 1)
                        {
                            PopolaAnagraficheENote(NuovoOrdine[0]);
                            PopolaGrigliaConIProdotti(NuovoOrdine);
                        }

                        if (ex != null && !File.Exists(fDest))
                        {
                            var eeeer = Path.Combine(Path.GetDirectoryName(selezionato.PathCompleto), "ELABORATI\\IN_ERRORE");
                            if (!Directory.Exists(eeeer)) Directory.CreateDirectory(eeeer);
                            var nonf = Path.Combine(eeeer, selezionato.NomeFileEXT);
                            File.Copy(selezionato.PathCompleto, nonf);
                            File.Delete(selezionato.PathCompleto);
                            FileDaImportareGlobal = "";
                            PoolingFileDaElaborare.GestoreMail.InviaMail($"Non è stato possibile importare in automatico il file {selezionato.NomeFileEXT} a causa del seguente errore:\r\n{ex.Message}",
                                $"Errore importazione per il documento {selezionato.NomeFileEXT}", null, null);
                            //Invia mail con eccezione a qualcuno per inserimento manuale
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Non è ancora disponibile il tracciato per il file richiesto\r\nImpossibile continuare", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }*/
            #endregion
            else
            {
                MessageBox.Show($"Non è ancora disponibile il tracciato per il file richiesto\r\nImpossibile continuare", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        private void PopolaGrigliaConIProdotti(List<VociOrdine> nuovoOrdine)
        {
            gridViewProdotti.BeginUpdate();
            foreach (var vo in nuovoOrdine)
            {
                ProdottiDaInserire.Add(vo);
            }
            gridViewProdotti.EndUpdate();
        }

        private void simpleButtonAnnullaOrdine_Click(object sender, EventArgs e)
        {
            AzzeraTutto();
        }

        private void comboBoxEdit1_Properties_EditValueChanged(object sender, EventArgs e)
        {
            AzzeraTutto();
            mandanteAttivo = comboBoxEditMandante.Text;

        }

        private void simpleButtonConvertiInCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ofd.Filter = "Excel files (*.xls;*.xlsx)|*.xls;*.xlsx";
            ofd.Multiselect = false;
            ofd.ShowDialog();

            var selezionato = ofd.FileName;

            if (File.Exists(selezionato) && mandanteAttivo.ToLower() != "aps")
            {
                bool ok = PoolingFileDaElaborare.TextToCsv.ConvertiXSLtoCSV(selezionato);
            }
            else
            {
                TextToCsv.ConvertiXSLtoCSV(selezionato, mandanteAttivo.ToLower(), "UNITEX");
            }
        }
    }
}
