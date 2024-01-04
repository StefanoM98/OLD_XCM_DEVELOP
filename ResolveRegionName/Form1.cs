using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResolveRegionName
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
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

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            foreach(var file in files)
            {
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);
                var wksheet = workbook.Worksheets[0];

                var docRange = wksheet.GetUsedRange();
                var totRighe = docRange.RowCount;

                
                List<Model> data = new List<Model>();
                for (int i = 3; i <= totRighe; i++)
                {
                    var header = ripulisciStringa(wksheet.Cells[$"B{i}"].Value.ToString(), " ");

                    if (!string.IsNullOrEmpty(header) && i == 3)
                    {
                        wksheet.Cells[$"BB{i}"].Value = "Regione";
                    }
                    var prov = ripulisciStringa(wksheet.Cells[$"AA{i}"].Value.ToString(), " ");
                    var regionName = "";
                    if (!string.IsNullOrEmpty(prov))
                    {
                        regionName = Api.GetRegionName(prov);
                    }
                    if (!string.IsNullOrEmpty(regionName))
                    {
                        wksheet.Cells[$"BB{i}"].Value = regionName.ToUpper();
                    }


                    //var item = new Model()
                    //{
                    //    ProgSped = ripulisciStringa(wksheet.Cells[$"C{i}"].Value.ToString(), " "),
                    //    Data = ripulisciStringa(wksheet.Cells[$"E{i}"].Value.ToString(), " "),
                    //    NumeroDTT = ripulisciStringa(wksheet.Cells[$"H{i}"].Value.ToString(), " "),
                    //    Destinatario = ripulisciStringa(wksheet.Cells[$"J{i}"].Value.ToString(), " "),
                    //    Destinazione = ripulisciStringa(wksheet.Cells[$"T{i}"].Value.ToString(), " "),
                    //    Provincia = ripulisciStringa(wksheet.Cells[$"AA{i}"].Value.ToString(), " "),
                    //    Colli = ripulisciStringa(wksheet.Cells[$"AD{i}"].Value.ToString(), " "),
                    //    Peso = ripulisciStringa(wksheet.Cells[$"AI{i}"].Value.ToString(), " "),
                    //    Vol = ripulisciStringa(wksheet.Cells[$"AK{i}"].Value.ToString(), " "),
                    //    Tassato = ripulisciStringa(wksheet.Cells[$"AP{i}"].Value.ToString(), " "),
                    //    CASS = ripulisciStringa(wksheet.Cells[$"AS{i}"].Value.ToString(), " "),
                    //    TotImponibile = ripulisciStringa(wksheet.Cells[$"AU{i}"].Value.ToString(), " "),
                    //    CostoCorr = ripulisciStringa(wksheet.Cells[$"AW{i}"].Value.ToString(), " "),
                    //    Differenza = ripulisciStringa(wksheet.Cells[$"AZ{i}"].Value.ToString(), " "),

                    //};

                    //data.Add(item);
                }

                workbook.SaveDocument(file, DocumentFormat.Xls);
                MessageBox.Show("Regioni inserite correttamente", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(file);
                //var test = data;
            }



        }


        public string ripulisciStringa(string str, string replaceWith)
        {
            var rx = @"[^0-9a-zA-Z.]+";
            return Regex.Replace(str, rx, replaceWith);
        }




    }
}
