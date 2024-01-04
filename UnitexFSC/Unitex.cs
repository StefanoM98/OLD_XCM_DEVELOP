using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;



namespace UnitexFSC
{
    public partial class Unitex : XtraForm
    {
        string LocalDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        API api = new API();

        public Unitex()
        {
            InitializeComponent();
            simpleButton5.Visible = false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var xlsFileFilter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            //var pdfFileFilter = "PDF Files|*.pdf";
            //var fn = $"BORDERO_FSC_{DateTime.Now.ToString("yyyyMMdd_HHmmssffff")}.pdf";
            //var outDir = @"C:\UnitexStorico\Clienti\FSC\OUT\";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.RestoreDirectory = true;
            ofd.Filter = xlsFileFilter;

            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.InitialDirectory = LocalDesktop;
            //sfd.Title = "Salva bordero finale";
            //sfd.DefaultExt = "pdf";
            //sfd.Filter = pdfFileFilter;
            //sfd.RestoreDirectory = true;

            //PrintDialog pd = new PrintDialog();
            //pd.ShowHelp = true;

            DialogResult ofdResult = ofd.ShowDialog();

            if (ofdResult == DialogResult.OK)
            {

                Workbook workbookOut = new Workbook();
                var wksheet = workbookOut.Worksheets[0];

                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    List<InterpreteFSC> righeNuovoBorderau = CreaOggettoFSCDaXlsTestataGespe(ofd.FileName);
                    if (righeNuovoBorderau.Count > 0)
                    {
                        List<InterpreteFSC> righeFinali = api.InviaFileSpedizioniFSC(righeNuovoBorderau).ToList();
                        if (righeFinali.Count == 0)
                        {
                            //TODO: dettagliare
                            throw new Exception("Attenzione! Nessuna spedizione nuova da aggiungere");
                        }
                        else
                        {
                            //Bordero newBordero = new Bordero();
                            //RichEditDocumentServer docs = newBordero.produciBorderoFSC(righeFinali);

                            //DialogResult saveDialogResult = sfd.ShowDialog();
                           
                            //if(saveDialogResult == DialogResult.OK)
                            //{
                            //    if (!Directory.Exists(outDir))
                            //    {
                            //        Directory.CreateDirectory(outDir);
                            //    }
                            //    var saveAs = Path.Combine(outDir, sfd.FileName);
                            //    docs.ExportToPdf(saveAs);
                            XtraMessageBox.Show(this, "Import terminato\r\nsaranno necessari fino a 5 minuti per vedere l'import su GESPE", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //}
                            //else
                            //{
                            //    DialogResult dialogResult = XtraMessageBox.Show(this, "Sicuro di voler terminare senza salvare il bordero finale?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            //    if(dialogResult == DialogResult.Yes)
                            //    {
                            //        XtraMessageBox.Show(this, "Import terminato\r\nsaranno necessari fino a 5 minuti per vedere l'import su GESPE", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //    }
                            //    else
                            //    {
                            //        if(sfd.ShowDialog() == DialogResult.OK)
                            //        {
                            //            if (!Directory.Exists(outDir))
                            //            {
                            //                Directory.CreateDirectory(outDir);
                            //            }
                            //            var saveAs = Path.Combine(outDir, sfd.FileName);
                            //            docs.ExportToPdf(saveAs);
                            //            XtraMessageBox.Show(this, "Bordero finale salvato con successo.\r\nImport effettuato, saranno necessari fino a 5 minuti per vedere l'import su GESPE", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //        }
                            //    }
                            //}
                        }
                    }

                }
                catch (Exception rr)
                {
                    if (XtraMessageBox.Show(rr.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    {
                        return;
                    }
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }

        }

        public string ripulisciStringa(string str, string replaceWith)
        {
            var rx = @"[^0-9a-zA-Z.]+";
            return Regex.Replace(str, rx, replaceWith);
        }

        public List<InterpreteFSC> CreaOggettoFSCDaXlsTestataGespe(string sourceFileName)
        {

            Workbook workbook = new Workbook();
            workbook.LoadDocument(sourceFileName);
            var wksheet = workbook.Worksheets[0];

            var docRange = wksheet.GetUsedRange();
            var totRighe = docRange.RowCount;

            List<string> testataFlussoEDI = new List<string>();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            var righeSenzaData = new List<string>();


            testataFlussoEDI.Add("Data Stampa Bordereau");
            testataFlussoEDI.Add("Numero Documento");
            testataFlussoEDI.Add("Nome Cliente / Nome Fornitore");
            testataFlussoEDI.Add("Colli");
            testataFlussoEDI.Add("Pallet");
            testataFlussoEDI.Add("Peso");
            testataFlussoEDI.Add("Citta");
            testataFlussoEDI.Add("CAP");
            testataFlussoEDI.Add("Indirizzo");
            testataFlussoEDI.Add("Provincia");
            testataFlussoEDI.Add("Riferimento Ordine");
            testataFlussoEDI.Add("Note");
            //testataFlussoEDI.Add("Num Cell");
            testataFlussoEDI.Add("Presenza Frigo");
            testataFlussoEDI.Add("Stato Ordine");
            testataFlussoEDI.Add("Tipo Documento");

            var columnCount = docRange.ColumnCount;
            for (int i = 0; i <= columnCount; i++)
            {
                var testoCella = wksheet.Cells[0, i].DisplayText;

                if (!testataFlussoEDI.Contains(testoCella))
                {
                    wksheet.Columns[i].Delete();
                    i--;
                    columnCount--;
                }
                else
                {
                    testataFlussoEDI.Remove(testoCella);
                    dict.Add(testoCella.Replace(" ", "").Trim().ToLower(), GetLetterOfIndex(i));
                }
            }

            //TODO: controllo se il dict è popolato correttamente
            var checkTestata = CheckTestata(testataFlussoEDI);
            if(checkTestata == "OK")
            {
                List<InterpreteFSC> DATI = new List<InterpreteFSC>();

                List<string> LastShipsNoDate = GetLastNoDateShips();
                
                for (int i = 2; i <= totRighe; i++)
                {
                    
                    var dt = DateTime.Now;
                    var dtf = wksheet.Cells[$"{dict["datastampabordereau"]}{i}"].Value.ToString().Trim();
                    var statoOrdine = wksheet.Cells[$"{dict["statoordine"]}{i}"].Value.ToString().Trim();

                    if (statoOrdine == "Pronto per la spedizione") continue;

                    if (!string.IsNullOrEmpty(dtf))
                    {
                        dt = DateTime.Parse(dtf);
                    }
                    else
                    {
                        var numeroDocumento = ripulisciStringa(wksheet.Cells[$"{dict["numerodocumento"]}{i}"].Value.ToString().Trim(), " ");

                        if (LastShipsNoDate.Contains(numeroDocumento)) continue;

                        DateTime dtMax = DateTime.MinValue;

                        for (int t = 1; t < totRighe; t++)
                        {
                            dtf = wksheet.Cells[$"{dict["datastampabordereau"]}{i + t}"].Value.ToString().Trim();
                            
                            if (!string.IsNullOrEmpty(dtf))
                            {
                                var dtTemp = DateTime.Parse(dtf);
                                if(dtTemp > dtMax)
                                {
                                    dtMax = dtTemp;
                                }
                            }
                        }
                        if(dtMax == DateTime.MinValue)
                        {
                            dt = DateTime.Now;
                        }
                        else
                        {
                            dt = dtMax;
                        }
                        righeSenzaData.Add(numeroDocumento);
                    }

                    var nfscTest = new InterpreteFSC()
                    {
                        DataStampaBorderau = dt.ToString("o"),
                        NumeroDocumento = ripulisciStringa(wksheet.Cells[$"{dict["numerodocumento"]}{i}"].Value.ToString(), " "),
                        NomeClienteFornitore = ripulisciStringa(wksheet.Cells[$"{dict["nomecliente/nomefornitore"]}{i}"].Value.ToString(), " "),
                        Colli = int.Parse(ripulisciStringa(wksheet.Cells[$"{dict["colli"]}{i}"].Value.ToString(), " ")),
                        Pallet = int.Parse(ripulisciStringa(wksheet.Cells[$"{dict["pallet"]}{i}"].Value.ToString(), " ")),
                        Peso = double.Parse(ripulisciStringa(wksheet.Cells[$"{dict["peso"]}{i}"].Value.ToString(), " ")),
                        Citta = ripulisciStringa(wksheet.Cells[$"{dict["citta"]}{i}"].Value.ToString(), " "),
                        CAP = ripulisciStringa(wksheet.Cells[$"{dict["cap"]}{i}"].Value.ToString(), " "),
                        Indirizzo = ripulisciStringa(wksheet.Cells[$"{dict["indirizzo"]}{i}"].Value.ToString(), " "),
                        Provincia = ripulisciStringa(wksheet.Cells[$"{dict["provincia"]}{i}"].Value.ToString(), " "),
                        RiferimentoOrdine = ripulisciStringa(wksheet.Cells[$"{dict["riferimentoordine"]}{i}"].Value.ToString(), " "),
                        Note = ripulisciStringa(wksheet.Cells[$"{dict["note"]}{i}"].Value.ToString(), " "),
                        //NumCell = ripulisciStringa(wksheet.Cells[$"{dict["numcell"]}{i}"].Value.ToString(), " "),
                        PresenzaFrigo = ripulisciStringa(wksheet.Cells[$"{dict["presenzafrigo"]}{i}"].Value.ToString(), " ")

                    };
                    DATI.Add(nfscTest);
                }
                if(righeSenzaData.Count > 0)
                {
                    SaveLastNoDateShips(righeSenzaData);
                }
                return DATI;

            }
            else
            {
                XtraMessageBox.Show(checkTestata, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //MessageBox.Show(this, checkTestata, "Errore", MessageBoxButtons.OK);
                return new List<InterpreteFSC>();
            }
        }

        public string CheckTestata(List<string> testataFlussoEDI)
        {
            if(testataFlussoEDI.Count == 0)
            {
                return "OK";
            }
            else
            {
                var header = "Attenzione! Nel file mancano le seguenti colonne:\r\n\r\n";
                var columnsName = $"";
                foreach(var str in testataFlussoEDI)
                {
                    columnsName += $"{str}\r\n";
                }
                return header + columnsName;
            }

        }

        public string GetLetterOfIndex(int indice)
        {
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            return alpha[indice].ToString();
        }

        public List<string> GetLastNoDateShips()
        {
            string appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Unitex");
            if (!Directory.Exists(appDataDir))
            {
                Directory.CreateDirectory(appDataDir);
            }
            string fileName = "FSCNoDate.txt";
            string workPath = Path.Combine(appDataDir, fileName);
            List<string> response = new List<string>();
            if (File.Exists(workPath))
            {
                response = File.ReadAllLines(workPath).ToList();
            }
            return response;
        }

        public void SaveLastNoDateShips(List<string> values)
        {

            string appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Unitex");
            if (!Directory.Exists(appDataDir))
            {
                Directory.CreateDirectory(appDataDir);
            }
            string fileName = "FSCNoDate.txt";
            string workPath = Path.Combine(appDataDir, fileName);
            if (File.Exists(workPath))
            {
                File.Delete(workPath);
            }
            File.WriteAllLines(workPath, values);

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                
                Cursor.Current = Cursors.WaitCursor;
                XcmForm xcmForm = new XcmForm();
                xcmForm.ShowDialog(this);
                
            }
            catch (Exception ee)
            {
                
                XtraMessageBox.Show($"Errore contattare il supporto IT\n\r{ee.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                Esiti esitForm = new Esiti();
                esitForm.ShowDialog(this);

            }
            catch (Exception ee)
            {

                XtraMessageBox.Show($"Errore contattare il supporto IT\n\r{ee.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                GLS glsForm = new GLS();
                glsForm.ShowDialog(this);

            }
            catch (Exception ee)
            {

                XtraMessageBox.Show($"Errore contattare il supporto IT\n\r{ee.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                Utils utilsForm = new Utils();
                utilsForm.ShowDialog(this);

            }
            catch (Exception ee)
            {

                XtraMessageBox.Show($"Errore contattare il supporto IT\n\r{ee.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
    }
}