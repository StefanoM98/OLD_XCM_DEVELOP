using DevExpress.DataAccess.Excel;
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

namespace UnitexFSC
{
    public partial class Document : DevExpress.XtraEditors.XtraForm
    {

        public Document()
        {
            InitializeComponent();
            this.AllowDrop = true;
        }

        private void Document_DragDrop(object sender, DragEventArgs e)
        {
            ExcelDataSource myExcelSource = new ExcelDataSource();
            ExcelWorksheetSettings worksheetSettings = new ExcelWorksheetSettings("", "A1:L100");

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            var file = files[0];

            var exstension = Path.GetExtension(file);

            myExcelSource.FileName = files[0];


            if (exstension.ToLower() == ".csv")
            {
                myExcelSource.SourceOptions = new CsvSourceOptions() { CellRange = "A1:L100" };
            }
            else
            {
                myExcelSource.SourceOptions = new ExcelSourceOptions(worksheetSettings);
            }

            myExcelSource.SourceOptions.SkipEmptyRows = false;
            myExcelSource.SourceOptions.UseFirstRowAsHeader = true;

            myExcelSource.Fill();
            gridControl2.DataSource = myExcelSource;



        }

        private void Document_DragEnter(object sender, DragEventArgs e)
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

        private void gridControl2_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                gridView2.BeginDataUpdate();

                ExcelDataSource myExcelSource = new ExcelDataSource();
                ExcelWorksheetSettings worksheetSettings = new ExcelWorksheetSettings("Foglio1", "A1:L100");

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                var file = files[0];

                var exstension = Path.GetExtension(file);

                myExcelSource.FileName = files[0];


                if (exstension.ToLower() == ".csv")
                {
                    myExcelSource.SourceOptions = new CsvSourceOptions() { CellRange = "A1:L100" };
                }
                else
                {
                    myExcelSource.SourceOptions = new ExcelSourceOptions(worksheetSettings);
                }

                myExcelSource.SourceOptions.SkipEmptyRows = false;
                myExcelSource.SourceOptions.UseFirstRowAsHeader = true;

                myExcelSource.Fill();
                gridControl2.DataSource = myExcelSource;

            }
            finally
            {
                gridView2.EndDataUpdate();
            }
        }

        private void gridControl2_DragEnter(object sender, DragEventArgs e)
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
    }
}
