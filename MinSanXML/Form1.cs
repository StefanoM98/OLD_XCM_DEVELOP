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
using System.Xml;
using System.Xml.Serialization;
using DevExpress.DataAccess.Excel;
using DevExpress.XtraPrinting;
using Newtonsoft.Json;

namespace MinSanXML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string startPath = "";

        private void simpleButtonProduciXML_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBoxEditTipoDestinatario.Text))
            {
                MessageBox.Show($"Tipo destinatario non selezionato, impossibile continuare",
                   "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(comboBoxEditTipoMittente.Text))
            {
                MessageBox.Show($"Tipo mittente non selezionato, impossibile continuare",
                   "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(comboBoxEditTipoTrasmissione.Text))
            {
                MessageBox.Show($"Tipo trasmissione non selezionato, impossibile continuare",
                   "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            List<DatiBody> bodies = new List<DatiBody>();
            var tmp = Path.GetTempFileName();
            CsvExportOptions options = new CsvExportOptions();
            options.Separator = ";";
            gridView1.ExportToCsv(tmp, options);
            var cc = File.ReadAllLines(tmp).Skip(1).ToArray();
            foreach (var c in cc)
            {
                var pz = c.Split(';');
                var nc = new DatiBody
                {
                    ID_DEST = pz[0],
                    CODICE_AIC = pz[1],
                    QUANTITA = pz[2],
                    VALORE = pz[3]
                };
                bodies.Add(nc);
            }
            File.Delete(tmp);
            //015026
            var minSanXML = new dataroot();

            if (bodies != null)
            {
                var regRaggruppate = bodies.GroupBy(x => x.ID_DEST).ToList();
                var daInserire = new datarootMitt();
                daInserire.tipo_m = comboBoxEditTipoMittente.Text;
                daInserire.id_mitt = "015026";
                daInserire.dest = new datarootMittDest[regRaggruppate.Count];
                for (int i = 0; i<regRaggruppate.Count;i++)
                {
                    var rr = regRaggruppate.ToArray()[i];
                    List<datarootMittDestFATAIC> AICL = new List<datarootMittDestFATAIC>();
                    var fattV = new datarootMittDestFAT();
                    for (int y = 0; y < rr.Count(); y++)
                    {
                        var r = rr.ToArray()[y];
                        if(!r.VALORE.Contains(".") && !r.VALORE.Contains(","))
                        {
                            r.VALORE = r.VALORE + ".00";
                        }
                        var AICV = new datarootMittDestFATAIC();

                        AICV.cod = r.CODICE_AIC;
                        AICV.qta = r.QUANTITA;
                        AICV.val = r.VALORE.Replace(",", ".");
                        AICL.Add(AICV);
                    }
                    fattV.AIC = AICL.ToArray();
                    fattV.anno = numericUpDownAnno.Value.ToString();
                    fattV.mese = numericUpDownMese.Value.ToString();
                    fattV.tipo_tr = comboBoxEditTipoTrasmissione.Text;
                    datarootMittDest mitt_dest1 = new datarootMittDest()
                    {
                        id_dest = rr.First().ID_DEST,
                        FAT = fattV,
                        tipo_d = comboBoxEditTipoDestinatario.Text
                    };

                    daInserire.dest[i] = mitt_dest1;

                }
                minSanXML.mitt = daInserire;

                string xmlMessage = MySerializer<dataroot>.Serialize(minSanXML);
                string mese = numericUpDownMese.Value.ToString();
                while (mese.Length < 2)
                {
                    mese = mese.Insert(0, "0");
                }
                var fn = $"{mese}_{numericUpDownAnno.Value}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xml";
                var dest = Path.Combine(startPath, fn);
                File.WriteAllText(dest, xmlMessage);
            }

        }

        public class MySerializer<T> where T : class
        {
            public static string Serialize(T obj)
            {
                XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
                using (var sww = new StringWriter())
                {
                    using (XmlTextWriter writer = new XmlTextWriter(sww) { Formatting = System.Xml.Formatting.Indented })
                    {
                        xsSubmit.Serialize(writer, obj);
                        return sww.ToString();
                    }
                }
            }
        }

        private void pictureEdit1_DragDrop(object sender, DragEventArgs e)
        {


            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (files.Count() > 1)
            {
                MessageBox.Show("Trascinare un file per volta", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            startPath = Path.GetDirectoryName(files[0]);
            gridView1.BeginUpdate();
            try
            {

                ExcelDataSource excelDS = new ExcelDataSource();
                excelDS.FileName = files[0];

                ExcelWorksheetSettings worksheetSettings = new ExcelWorksheetSettings();
                worksheetSettings.WorksheetName = "Foglio1";


                ExcelSourceOptions sourceOptions = new ExcelSourceOptions();
                sourceOptions.ImportSettings = worksheetSettings;
                sourceOptions.SkipHiddenRows = false;
                sourceOptions.SkipHiddenColumns = false;

                excelDS.SourceOptions = sourceOptions;
                excelDS.Fill();
                gridControl1.DataSource = excelDS;




            }
            finally
            {
                gridView1.EndUpdate();
            }

        }

        private void pictureEdit1_DragEnter(object sender, DragEventArgs e)
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

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(pictureEdit1_DragEnter);
            this.DragDrop += new DragEventHandler(pictureEdit1_DragDrop);

            var Pdtn = DateTime.Now - TimeSpan.FromDays(20);

            numericUpDownMese.Value = Pdtn.Month;
            numericUpDownAnno.Value = Pdtn.Year;

            comboBoxEditTipoDestinatario.SelectedIndex = 0;
            comboBoxEditTipoMittente.SelectedIndex = 0;
            comboBoxEditTipoTrasmissione.SelectedIndex = 0;

        }
    }
}
