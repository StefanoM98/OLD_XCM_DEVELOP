using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;



namespace XCM_WMS_GESPE
{
    public partial class Form1 : Form
    {
        #region Attributi
        List<GiacenzaProdotto> inBinding = new List<GiacenzaProdotto>();
        List<Tuple<string, string, string>> Customer = new List<Tuple<string, string, string>>();
        string tmpReport = "Template_Report_Excel.xlsx";
        #endregion

        #region ctor
        public Form1()
        {
            InitializeComponent();
            PopolaClienti();
        } 
        #endregion

        #region Eventi Form
        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var db = new GnXcmEntities();
            var idx = comboBoxEdit1.SelectedIndex;
            var cust = comboBoxEdit1.Properties.Items[idx].ToString();
            var cc = cust.Substring(1, 5);
            var giacCli = db.uvwWmsWarehouse.Where(x => x.CustomerID == cc && (x.ItemStatus == 10 || x.ItemStatus == 20))//.Distinct()
                .Select(x => new GiacenzaProdotto()
                {
                    CodiceProdotto = x.PrdCod,
                    DataRiferimento = (x.DateRef != null) ? x.DateRef.Value : DateTime.MinValue,
                    DataScadenza = (x.DateExpire != null) ? x.DateExpire.Value : DateTime.MinValue,
                    DescrizioneProdotto = x.PrdDes,
                    Lotto = x.BatchNo,
                    MagazzinoLogico = x.LogWareID,
                    MapID = x.MapID,
                    QuantitaTotale = (x.TotalQty != null) ? x.TotalQty.Value : -1,
                    Riferimento = x.Reference,
                    ShelflifePrd = (x.OutShelfLife != null) ? x.OutShelfLife.Value : 0
                }).OrderBy(x => x.DescrizioneProdotto).ToList();

            var listaRaggruppataPerSommaQuantita = new List<GiacenzaProdotto>();

            foreach (var gc in giacCli)
            {
                if (listaRaggruppataPerSommaQuantita.Any(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MapID == gc.MapID && x.MagazzinoLogico == gc.MagazzinoLogico))
                {
                    var esiste = listaRaggruppataPerSommaQuantita.FirstOrDefault(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MapID == gc.MapID && x.MagazzinoLogico == gc.MagazzinoLogico);
                    if (esiste != null)
                    {
                        esiste.QuantitaTotale += gc.QuantitaTotale;
                    }
                    else
                    {
                        esiste = listaRaggruppataPerSommaQuantita.FirstOrDefault(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MapID == gc.MapID);
                        if (esiste != null)
                        {
                            esiste.QuantitaTotale += gc.QuantitaTotale;
                        }
                        else
                        {
                            esiste = listaRaggruppataPerSommaQuantita.FirstOrDefault(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MagazzinoLogico == gc.MagazzinoLogico);
                            if (esiste != null)
                            {
                                if (esiste.MapID == gc.MapID)
                                {
                                    esiste.QuantitaTotale += gc.QuantitaTotale;
                                }
                                else
                                {
                                    listaRaggruppataPerSommaQuantita.Add(gc);
                                }
                            }
                            else
                            {
                                listaRaggruppataPerSommaQuantita.Add(gc);
                            }
                        }
                    }
                }
                else
                {
                    listaRaggruppataPerSommaQuantita.Add(gc);
                }
            }
            inBinding = listaRaggruppataPerSommaQuantita;
            giacenzaProdottoBindingSource.DataSource = listaRaggruppataPerSommaQuantita;
        }
        private void simpleButtonEsportaXslx_Click(object sender, EventArgs e)
        {
            var invioMail = MessageBox.Show("Inviare anche le email?", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var savepath = Path.Combine(desktop, "Export Giacenze Magazzino");
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }
            var mandante = comboBoxEdit1.SelectedItem as Tuple<string, string, string>;
            var finalDest = Path.Combine(savepath, $"Export_{mandante.Item2}_{DateTime.Now.ToString("ddMMyyyy")}.xlsx");

            //gridView1.ExportToXlsx(finalDest);
            var listaRaggruppataPerSommaQuantita = giacenzaProdottoBindingSource.DataSource as List<GiacenzaProdotto>;
            CreaExportPersonalizzato(listaRaggruppataPerSommaQuantita, finalDest);
            if (invioMail == DialogResult.Yes)
            {
                GestoreMail.SendMailGiacenzeMagazzino(finalDest, mandante.Item3);
            }
            Process.Start(savepath);
        }
        private void simpleButtonInviaReportATutti_Click(object sender, EventArgs e)
        {
            var invioMail = MessageBox.Show("Inviare anche le email?", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var savepath = Path.Combine(desktop, "Export Giacenze Magazzino");
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }

            foreach (var m in comboBoxEdit1.Properties.Items)
            {
                var mandante = m as Tuple<string, string, string>;
                var db = new GnXcmEntities();

                var giacCli = db.uvwWmsWarehouse.Where(x => x.CustomerID == mandante.Item1 && x.ItemStatus == 10)//.Distinct()
                    .Select(x => new GiacenzaProdotto()
                    {
                        CodiceProdotto = x.PrdCod,
                        DataRiferimento = (x.DateRef != null) ? x.DateRef.Value : DateTime.MinValue,
                        DataScadenza = (x.DateExpire != null) ? x.DateExpire.Value : DateTime.MinValue,
                        DescrizioneProdotto = x.PrdDes,
                        Lotto = x.BatchNo,
                        MagazzinoLogico = x.LogWareID,
                        MapID = x.MapID,
                        QuantitaTotale = (x.TotalQty != null) ? x.TotalQty.Value : -1,
                        Riferimento = x.Reference,
                        ShelflifePrd = (x.OutShelfLife != null) ? x.OutShelfLife.Value : 0
                    }).OrderBy(x => x.DescrizioneProdotto).ToList();

                var listaRaggruppataPerSommaQuantita = new List<GiacenzaProdotto>();

                foreach (var gc in giacCli)
                {
                    if (listaRaggruppataPerSommaQuantita.Any(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MapID == gc.MapID && x.MagazzinoLogico == gc.MagazzinoLogico))
                    {
                        var esiste = listaRaggruppataPerSommaQuantita.FirstOrDefault(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MapID == gc.MapID && x.MagazzinoLogico == gc.MagazzinoLogico);
                        if (esiste != null)
                        {
                            esiste.QuantitaTotale += gc.QuantitaTotale;
                        }
                        else
                        {
                            esiste = listaRaggruppataPerSommaQuantita.FirstOrDefault(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MapID == gc.MapID);
                            if (esiste != null)
                            {
                                esiste.QuantitaTotale += gc.QuantitaTotale;
                            }
                            else
                            {
                                esiste = listaRaggruppataPerSommaQuantita.FirstOrDefault(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MagazzinoLogico == gc.MagazzinoLogico);
                                if (esiste != null)
                                {
                                    if (esiste.MapID == gc.MapID)
                                    {
                                        esiste.QuantitaTotale += gc.QuantitaTotale;
                                    }
                                    else
                                    {
                                        listaRaggruppataPerSommaQuantita.Add(gc);
                                    }
                                }
                                else
                                {
                                    listaRaggruppataPerSommaQuantita.Add(gc);
                                }
                            }
                        }
                    }
                    else
                    {
                        listaRaggruppataPerSommaQuantita.Add(gc);
                    }
                }

                var finalDest = Path.Combine(savepath, $"Export_{mandante.Item2}_{DateTime.Now.ToString("ddMMyyyy")}.xlsx");
                inBinding = listaRaggruppataPerSommaQuantita;

                giacenzaProdottoBindingSource.DataSource = listaRaggruppataPerSommaQuantita;

                CreaExportPersonalizzato(listaRaggruppataPerSommaQuantita, finalDest);
                //gridView1.ExportToXlsx(finalDest);
                if (invioMail == DialogResult.Yes)
                {
                    GestoreMail.SendMailGiacenzeMagazzino(finalDest, mandante.Item3);
                }
            }
        }
        #endregion

        #region Metodi 
        private void PopolaClienti()
        {

            var mandanti = File.ReadAllLines(Properties.Settings.Default.FileMandanti);

            foreach (var m in mandanti)
            {
                var split = m.Split('|');
                Customer.Add(new Tuple<string, string, string>(split[0], split[1], split[2]));

            }
            comboBoxEdit1.Properties.Items.AddRange(Customer);
        }
        private void CreaExportPersonalizzato(List<GiacenzaProdotto> listaRaggruppataPerSommaQuantita, string finalDest)
        {
            Workbook workbook = new Workbook();
            workbook.LoadDocument(tmpReport);

            try
            {
                var wksheet = workbook.Worksheets[0];

                var totRighe = listaRaggruppataPerSommaQuantita.Count();
                workbook.BeginUpdate();
                bool thereIsMagazzinoLogico = false;
                wksheet.Cells[$"A{2}"].Value = $"Giacenze al {DateTime.Now.ToString("dd/MM/yyyy")}";
                for (int i = 0; i < totRighe; i++)
                {
                    var rigaDoc = listaRaggruppataPerSommaQuantita[i];

                    bool scadenzaValida = false;
                    int scadAllaVendita = 0;
                    int scadEffettiva = 0;
                    if (rigaDoc.DataScadenza.Year > 2000)
                    {
                        scadAllaVendita = (int)(DateTime.Now - (rigaDoc.DataScadenza - TimeSpan.FromDays(rigaDoc.ShelflifePrd))).TotalDays * -1;
                        scadEffettiva = (int)(DateTime.Now.Date - rigaDoc.DataScadenza).TotalDays * -1;
                        scadenzaValida = true;
                    }

                    #region Reparto
                    var Rep = "";
                    if (rigaDoc.MapID == "VN")
                    {
                        Rep = "VENDIBILI";
                    }
                    else if (rigaDoc.MapID == "IN")
                    {
                        Rep = "INVENDIBILI";
                    }
                    else if (rigaDoc.MapID == "QR")
                    {
                        Rep = "QUARANTENA";
                    }
                    else if (rigaDoc.MapID == "PR")
                    {
                        Rep = "PROMOZIONALE";
                    }
                    else if (rigaDoc.MapID == "SM")
                    {
                        Rep = "DA SMALTIRE";
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(rigaDoc.MagazzinoLogico))
                    {
                        thereIsMagazzinoLogico = true;
                    }

                    #region Scrittura dati
                    wksheet.Cells[$"A{i + 4}"].Value = Rep;
                    wksheet.Cells[$"A{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"A{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"B{i + 4}"].Value = rigaDoc.CodiceProdotto;
                    wksheet.Cells[$"C{i + 4}"].Value = rigaDoc.DescrizioneProdotto;
                    wksheet.Cells[$"D{i + 4}"].Value = (int)rigaDoc.QuantitaTotale;
                    wksheet.Cells[$"D{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"D{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"E{i + 4}"].Value = rigaDoc.Lotto;
                    wksheet.Cells[$"E{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"E{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"F{i + 4}"].Value = (scadenzaValida) ? rigaDoc.DataScadenza.ToString("dd/MM/yyyy") : "";
                    wksheet.Cells[$"F{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"F{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"G{i + 4}"].Value = rigaDoc.ShelflifePrd;
                    wksheet.Cells[$"G{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"G{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"H{i + 4}"].Value = scadAllaVendita;
                    wksheet.Cells[$"H{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"H{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"I{i + 4}"].Value = scadEffettiva;
                    wksheet.Cells[$"I{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"I{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"J{i + 4}"].Value = rigaDoc.MagazzinoLogico;
                    wksheet.Cells[$"J{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"J{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    #endregion
                }

                #region Colora righe
                var docRange = wksheet.GetUsedRange();

                for (int i = 2; i < docRange.RowCount; i++)
                {
                    bool odds = (i % 2 == 0);
                    if (odds)
                    {
                        wksheet.Rows[i].FillColor = Color.LightGray;
                    }
                }
                #endregion

                #region Pulizia
                if (!thereIsMagazzinoLogico)
                {
                    wksheet.Columns["J"].Delete();
                }

                if (File.Exists(finalDest))
                {
                    File.Delete(finalDest);
                }
                #endregion
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, "ERRORE", $"Non è stato possibile produrre il file {Path.GetFileName(finalDest)} per il seguente errore:{ee.Message}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                workbook.EndUpdate();
            }
            workbook.SaveDocument(finalDest, DocumentFormat.OpenXml);
        }
        #endregion

        #region Gestione griglia
        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                var r = e.Row as GiacenzaProdotto;
                if (r == null) return;
                if (e.Column.Name == gridColumnDataAllaVendita.Name)
                {
                    try
                    {
                        if (r.DataScadenza == DateTime.MinValue)
                        {
                            e.Value = 0;
                        }
                        else
                        {
                            e.Value = (int)(r.DataScadenza - TimeSpan.FromDays(r.ShelflifePrd) - DateTime.Now).Days;
                        }
                    }
                    catch
                    {
                        e.Value = 0;
                    }
                }
                else if (e.Column.Name == gridColumnAllaScadenzaEffettiva.Name)
                {
                    if (r.DataScadenza == DateTime.MinValue)
                    {
                        e.Value = 0;
                    }
                    else
                    {
                        e.Value = (int)(r.DataScadenza - DateTime.Now).Days;
                    }

                }
                else if (e.Column.Name == gridColumnReparto.Name)
                {
                    if (r.MapID == "IN")
                    {
                        e.Value = "INVENDIBILI";
                    }
                    else if (r.MapID == "VN")
                    {
                        e.Value = "VENDIBILI";
                    }
                    else if (r.MapID == "PR")
                    {
                        e.Value = "PROMOZIONALE";
                    }
                    else if (r.MapID == "QR")
                    {
                        e.Value = "QUARANTENA";
                    }
                    else if (r.MapID == "SM")
                    {
                        e.Value = "DA SMALTIRE";
                    }
                }
            }
        }       
        #endregion
    }
}
