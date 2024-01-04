using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnitexFSC.Code;

namespace UnitexFSC
{
    public partial class Esiti : DevExpress.XtraEditors.XtraForm
    {
        public Esiti()
        {
            InitializeComponent();

            labelControl2.Visible = false;
            textEdit1.Visible = false;
            labelControl3.Visible = false;
            textEdit2.Visible = false;
            checkEdit1.Visible = false;
            simpleButton2.Visible = false;
            simpleButton3.Visible = false;

            List<string> suppliers = new List<string> { "IMPROTA", "ALLWAYS", "EMMEA", "TLI", "COTRAF", "CDL", "GLS" };
            comboBox1.DataSource = suppliers;
            Data.Init();

        }

        private void simpleButtonEsiti_OpenFile(object sender, EventArgs e)
        {
            var xlsFileFilter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.RestoreDirectory = true;
            ofd.Filter = xlsFileFilter;

            DialogResult ofdResult = ofd.ShowDialog();

            if (ofdResult == DialogResult.OK)
            {
                var name = comboBox1.SelectedItem.ToString();

                FileHeader supplier = Data.db.FirstOrDefault(x => x.Name == name);

                if (supplier == null)
                {
                    XtraMessageBox.Show(this, $"Il Fornitore {name}\r\n non è abilitato contatta il reparto IT", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (name == "CDL" || name == "GLS")
                {
                    var response = Tracking.TrackingNewUNITEX(File.ReadAllLines(ofd.FileName).ToList(), supplier.Name, $"{DateTime.Now.AddMonths(-1):MM-dd-yyyy}");

                    var csvFileFilter = "EXCEL Files|*.xlsx;*.xls";

                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    sfd.Title = "Salva file eisti mancanti";
                    sfd.DefaultExt = "xlsx";
                    sfd.Filter = csvFileFilter;
                    sfd.RestoreDirectory = true;

                    DialogResult sfdResult = sfd.ShowDialog();


                    if (sfdResult == DialogResult.OK)
                    {
                        response.workbook.SaveDocument(sfd.FileName, DocumentFormat.Xlsx);
                        XtraMessageBox.Show(this, $"Esiti mancanti salvati con successo", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                }
                else
                {

                    //Load file into workbook
                    Workbook workbook = new Workbook();
                    workbook.LoadDocument(ofd.FileName);
                    var wksheet = workbook.Worksheets[0];
                    var docRange = wksheet.GetUsedRange();
                    var totRighe = docRange.RowCount;
                    var columnCount = docRange.ColumnCount;
                    List<string> resultColumns = new List<string>();

                    //Elimino le colonne che non ci interessano
                    for (int i = 0; i <= columnCount; i++)
                    {
                        var headerText = wksheet.Cells[0, i].DisplayText;

                        if (name == "COTRAF")
                        {
                            headerText = wksheet.Cells[1, i].DisplayText;
                        }
                        //var tt = headerText.Trim();
                        //var ttt = supplier.DocumentNumber;

                        //var t = tt == ttt;
                        if (headerText.Trim() != supplier.DocumentNumber
                            && headerText.Trim() != supplier.TrackingDate)
                        {

                            wksheet.Columns[i].Delete();
                            i--;
                            columnCount--;
                        }
                        else
                        {
                            resultColumns.Add(headerText.Trim());
                        }
                    }

                    //Check delle colonne
                    if (resultColumns.Count == 0)
                    {
                        var header = "Attenzione! Nel file mancano le seguenti colonne:\r\n\r\n";
                        var footer = "Correggi il nome della colonna dalle impostazioni o aggiungi la colonna mancante";
                        var columnsName = $"{supplier.DocumentNumber}\r\n{supplier.TrackingDate}";

                        XtraMessageBox.Show(this, $"{header}{columnsName}\r\n\r\n{footer}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        var header = "Attenzione! Nel file mancano le seguenti colonne:\r\n\r\n";
                        var footer = "Correggi il nome della colonna dalle impostazioni o aggiungi la colonna mancante";
                        string columnsName = "";
                        if (!resultColumns.Contains(supplier.DocumentNumber))
                        {
                            columnsName = $"{supplier.DocumentNumber}";
                        }
                        if (!resultColumns.Contains(supplier.TrackingDate))
                        {
                            columnsName += $"\r\n{supplier.TrackingDate}";
                        }
                        if (!string.IsNullOrEmpty(columnsName))
                        {
                            XtraMessageBox.Show(this, $"{header}{columnsName}\r\n\r\n{footer}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    }

                    List<string> csvOut = new List<string>();

                    for (int i = 2; i < totRighe; i++)
                    {
                        if (supplier.MancaSH)
                        {
                            var docNumb = wksheet.Cells[$"A{i}"].Value.ToString();
                            var normalizedDocNum = docNumb;
                            if (supplier.MancaZero)
                            {
                                var zeri = "";
                                for (int t = docNumb.Length; t < 5; t++)
                                {
                                    zeri += "0";
                                }

                                normalizedDocNum = $"{zeri}{docNumb}";
                            }
                            wksheet.Cells[$"A{i}"].Value = $"{normalizedDocNum}/SH";
                        }


                        var docNumber = wksheet.Cells[$"A{i}"].Value.ToString();
                        var trackingDate = wksheet.Cells[$"B{i}"].Value.ToString();

                        if (string.IsNullOrEmpty(docNumber) || string.IsNullOrEmpty(trackingDate)) continue;

                        //if (supplier.MancaStatusCode)
                        //{
                        csvOut.Add($"{docNumber};{trackingDate};30");
                        //}

                        if (csvOut.Count > 0)
                        {

                            var response = Tracking.TrackingNewUNITEX(csvOut, supplier.Name, $"{DateTime.Now.AddMonths(-1):MM-dd-yyyy}");

                            var csvFileFilter = "CSV Files|*.csv;*.CSV";

                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            sfd.Title = "Salva file ripulito";
                            sfd.DefaultExt = "csv";
                            sfd.Filter = csvFileFilter;
                            sfd.RestoreDirectory = true;

                            DialogResult sfdResult = sfd.ShowDialog();


                            if (sfdResult == DialogResult.OK)
                            {
                                File.WriteAllLines(sfd.FileName, csvOut);

                                XtraMessageBox.Show(this, $"Pulizia file eseguita con successo", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show(this, $"Nessuna spedizione da savlare", "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                }


            }
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (Data.db == null) Data.Init();
            var settings = Data.db.FirstOrDefault(x => x.Name == comboBox1.SelectedItem.ToString());

            if (settings.Name == "TLI")
            {
                simpleButton3.Visible = false;
            }

            if (settings != null)
            {
                textEdit1.Text = settings.DocumentNumber;
                textEdit2.Text = settings.TrackingDate;
                checkEdit1.EditValue = settings.MancaSH;
            }
        }

        private static bool settingsVisible = false;

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (settingsVisible)
            {
                settingsVisible = false;
            }
            else
            {
                settingsVisible = true;
            }

            labelControl2.Visible = settingsVisible;
            textEdit1.Visible = settingsVisible;
            labelControl3.Visible = settingsVisible;
            textEdit2.Visible = settingsVisible;
            checkEdit1.Visible = settingsVisible;
            simpleButton2.Visible = settingsVisible;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            var name = comboBox1.SelectedItem.ToString();

            var record = Data.db.FirstOrDefault(x => x.Name == name);

            if (record == null)
            {
                XtraMessageBox.Show(this, $"Nessun fornitore trovato", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var docNumber = textEdit1.Text;
            var trackingDate = textEdit2.Text;
            var isSH = checkEdit1.EditValue;

            if (record.DocumentNumber != docNumber)
            {
                record.DocumentNumber = docNumber;
            }
            if (record.TrackingDate != trackingDate)
            {
                record.TrackingDate = trackingDate;
            }
            if (record.MancaSH != (bool)isSH)
            {
                record.MancaSH = (bool)isSH;
            }

            Data.RefreshDb();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {

            var test = TLI.ProduciShipmentOUT("01866/TR");


            var txtFileFilter = "Text Files|*.TXT;*.txt;";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.RestoreDirectory = true;
            ofd.Filter = txtFileFilter;

            DialogResult ofdResult = ofd.ShowDialog();

            if (ofdResult == DialogResult.OK)
            {
                var csvOut = TLI.ProduciCSVEsiti(ofd.FileName);

                if (csvOut.Count > 0)
                {
                    var csvFileFilter = "CSV Files|*.csv;*.CSV";

                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    sfd.Title = "Salva file ripulito";
                    sfd.DefaultExt = "csv";
                    sfd.Filter = csvFileFilter;
                    sfd.RestoreDirectory = true;

                    DialogResult sfdResult = sfd.ShowDialog();


                    if (sfdResult == DialogResult.OK)
                    {
                        File.WriteAllLines(sfd.FileName, csvOut);

                        XtraMessageBox.Show(this, $"Pulizia file eseguita con successo", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    XtraMessageBox.Show(this, $"Nessuna spedizione da savlare", "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }
    }
}
