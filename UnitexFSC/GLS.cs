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
using UnitexFSC.Code.APIs;

namespace UnitexFSC
{
    public partial class GLS : XtraForm
    {
        public GLS()
        {
            InitializeComponent();

            
            var trips = TEMI.GetTrips();

            if (trips.Count() == 0)
            {
                if (XtraMessageBox.Show("Non sono stati trovati viaggi", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    return;
                }

            }
            else
            {
                unitexTripBindingSource.DataSource = trips.OrderByDescending(x => x.docDate);

            }
        }

        private void simpleButtonGls_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var record = glsGridView.GetFocusedRow() as TmsTripListTrip;
                if (record == null)
                {
                    if (XtraMessageBox.Show("Impossibile leggere il bordero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    {
                        return;
                    }
                }

                var fileContent = TEMI.GetGLSFileContent(record.docNumber);
                //var fileContent = TEMI.GetExcelFileContent(record.docNumber);
                //var wkSheet = fileContent.Worksheets[0];

                //var docRange = wkSheet.GetUsedRange();
                //var totRighe = docRange.RowCount;


                if (fileContent.Count > 0)
                {
                    var csvFileFilter = "CSV Files|*.csv;*.CSV;";

                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    sfd.Title = "Salva file";
                    sfd.DefaultExt = "csv";
                    sfd.Filter = csvFileFilter;
                    sfd.RestoreDirectory = true;

                    DialogResult sfdResult = sfd.ShowDialog();


                    if (sfdResult == DialogResult.OK)
                    {
                        File.WriteAllLines(sfd.FileName, fileContent);
                        XtraMessageBox.Show(this, "File Salvato con successo", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }


                    //var excelFileFilter = "Excel Files|*.xls;*.xlsx;";

                    //SaveFileDialog sfd = new SaveFileDialog();
                    //sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    //sfd.Title = "Salva file";
                    //sfd.DefaultExt = "xlsx";
                    //sfd.Filter = excelFileFilter;
                    //sfd.RestoreDirectory = true;

                    //DialogResult sfdResult = sfd.ShowDialog();


                    //if (sfdResult == DialogResult.OK)
                    //{
                    //    //File.WriteAllLines(sfd.FileName, fileContent);
                    //    fileContent.SaveDocument(sfd.FileName, DocumentFormat.Xlsx);
                    //    XtraMessageBox.Show(this, "File Salvato con successo", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}

                }
                else
                {
                    XtraMessageBox.Show(this, "Generazione file fallita\r\ncontatta il reparto IT", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                //this.Dispose();
                //this.Close();
            }
            catch (Exception ee)
            {
                XtraMessageBox.Show(this, $"Import fallito, contattare il reparto IT\r\n{ee.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }
    }
}
