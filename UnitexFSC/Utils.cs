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
    public partial class Utils : XtraForm
    {
        public Utils()
        {
            InitializeComponent();
          
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            var tripNumber = textEdit2.Text;

            if (string.IsNullOrEmpty(tripNumber))
            {
                XtraMessageBox.Show(this, $"Inserisci un numero di viaggio per continuare", "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            EspritecAPI_UNITEX.Init("dvalitutti", "Dv$2022!", "UNITEX");
            var trip = EspritecAPI_UNITEX.TmsTripList().FirstOrDefault(x => x.docNumber == $"{tripNumber}/TR");

            if(trip != null)
            {
                var stops = EspritecAPI_UNITEX.TmsTripStopList(trip.id);


                var shipments = stops.OrderBy(x => x.location).ThenBy(z => z.district);
                Workbook workbook = new Workbook();
                var wksheet = workbook.Worksheets[0];

                int i = 2; 
                foreach (var ship in shipments)
                {
                    wksheet.Cells[$"A{i}"].Value = ship.shipExternRef;
                    wksheet.Cells[$"B{i}"].Value = ship.description;
                    wksheet.Cells[$"C{i}"].Value = ship.location;
                    wksheet.Cells[$"D{i}"].Value = ship.district;
                    wksheet.Cells[$"E{i}"].Value = ship.packs;
                    wksheet.Cells[$"F{i}"].Value = ship.grossWeight;

                    i++;
                }

                //TODO: controllo se ha prodotto righe
                //Non mi ritorna i stop del trip l'api
                var xlsxFileFilter = "Excel Files|*.xls;*.xlsx;";


                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                sfd.Title = "Salva file";
                sfd.DefaultExt = "xlsx";
                sfd.Filter = xlsxFileFilter;
                sfd.RestoreDirectory = true;

                DialogResult sfdResult = sfd.ShowDialog();


                if (sfdResult == DialogResult.OK)
                {
                    workbook.SaveDocument(sfd.FileName, DocumentFormat.Xlsx);

                    XtraMessageBox.Show(this, "File Salvato con successo", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
