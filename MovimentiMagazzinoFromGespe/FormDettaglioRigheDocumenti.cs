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
    public partial class FormDettaglioRigheDocumenti : Form
    {
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MinValue;
        List<RigheDocumento> RigheTraLeDate = new List<RigheDocumento>();
        public FormDettaglioRigheDocumenti()
        {
            InitializeComponent();
        }

        private void FormDettaglioRigheDocumenti_Load(object sender, EventArgs e)
        {
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
                //int m = 12;
                var dinM = DateTime.DaysInMonth(now.Year, 12);
                startDate = new DateTime(now.Year, now.Month, 1);
                endDate = new DateTime(now.Year, now.Month, dinM);
            }

            dateEditDataDa.DateTime = startDate;
            dateEditDataA.DateTime = endDate;

            PopolaClienti();

        }

        private void PopolaClienti()
        {
            var db = new XCM_WMSEntities();
            comboBoxEdit1.Properties.Items.AddRange(db.ANAGRAFICA_CLIENTI.ToList());
        }

        private void simpleButtonAggiornaGriglia_Click(object sender, EventArgs e)
        {
            RigheTraLeDate.Clear();
            AggiornaDati();
        }

        private void AggiornaDati()
        {
            //recuperare il mandante
            Cursor = Cursors.WaitCursor;
            gridView1.BeginDataUpdate();
            try
            {
                var db = new GnXcmEntities();
                var idx = comboBoxEdit1.SelectedIndex;
                ANAGRAFICA_CLIENTI cc = comboBoxEdit1.Properties.Items[idx] as ANAGRAFICA_CLIENTI;
                List<uvwWmsDocumentRows_XCM> RigheDelMese = db.uvwWmsDocumentRows_XCM.Where(x => x.CustomerID == cc.ID_GESPE && x.DocTip == 203 && x.DocDta >= startDate && x.DocDta <= endDate && x.StatusDes != "ANNULLATO").ToList();
                gridControl1.DataSource = RigheDelMese;
            }
            finally
            {

                gridView1.EndUpdate();
            }


        }
        private void buttonMeseScorso_Click(object sender, EventArgs e)
        {
            var dtn = DateTime.Now;
            if (dtn.Month == 1)
            {
                dateEditDataDa.DateTime = new DateTime(dtn.Year - 1, 12, 01);
                dateEditDataA.DateTime = new DateTime(dtn.Year - 1, 12, 31);
            }
            else
            {
                var giorniDelMese = DateTime.DaysInMonth(dtn.Year, dtn.Month - 1);

                dateEditDataDa.DateTime = new DateTime(dtn.Year, dtn.Month - 1, 01);
                dateEditDataA.DateTime = new DateTime(dtn.Year, dtn.Month - 1, giorniDelMese);
            }
        }

        private void dateEditDataDa_EditValueChanged(object sender, EventArgs e)
        {
            startDate = dateEditDataDa.DateTime;
        }

        private void dateEditDataA_EditValueChanged(object sender, EventArgs e)
        {
            endDate = dateEditDataA.DateTime;
        }

        private void simpleButtonEsportaXslx_Click(object sender, EventArgs e)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var savepath = Path.Combine(desktop, "Export Documenti Magazzino");
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }

            string finalDest = Path.Combine(savepath, $"Export_Righe_Magazzino_dal{startDate.ToString("ddMMyyyy")}_al_{endDate.ToString("ddMMyyyy")}.xlsx");


            if (File.Exists(finalDest)) File.Delete(finalDest);
            gridView1.ExportToXlsx(finalDest);

            Process.Start(savepath);
        }

        private void buttonMeseCorrente_Click(object sender, EventArgs e)
        {
            var dtn = DateTime.Now;
            var giorniDelMese = DateTime.DaysInMonth(dtn.Year, dtn.Month);

            dateEditDataDa.DateTime = new DateTime(dtn.Year, dtn.Month, 01);
            dateEditDataA.DateTime = new DateTime(dtn.Year, dtn.Month, giorniDelMese);
        }

        private void buttonAnnoCorrente_Click(object sender, EventArgs e)
        {
            var dtn = DateTime.Now;
            dateEditDataDa.DateTime = new DateTime(dtn.Year, 01, 01);
            dateEditDataA.DateTime = dtn;
        }

        private void buttonAnnoPrecedente_Click(object sender, EventArgs e)
        {
            var dtn = DateTime.Now;
            dateEditDataDa.DateTime = new DateTime(dtn.Year - 1, 01, 01);
            dateEditDataA.DateTime = new DateTime(dtn.Year - 1, 12, 31);
        }
    }
}
