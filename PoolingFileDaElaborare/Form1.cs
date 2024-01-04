using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoolingFileDaElaborare
{
    public partial class Form1 : XtraForm
    {
        List<FileDaImportare> files = new List<FileDaImportare>();
        public Form1()
        {
            InitializeComponent();
            gridControl1.DataSource = files;

            LoadWordToDo();
        }

        private void LoadWordToDo()
        {
            try
            {
                bool nuoviFiles = false;
                gridView1.BeginUpdate();
                gridView1.BeginDataUpdate();
                var FilesDaElaborare = Directory.GetFiles(@"\\192.168.1.231\Inserimenti automatici Gespe", "*.*",
                    SearchOption.AllDirectories).Where(x => !x.Contains("@") && (!x.ToLower().Contains("elaborati"))).ToList();

                foreach (var ff in FilesDaElaborare)
                {
                    if (!files.Any(x => x.PathCompleto.Contains(ff)))
                    {
                        var nf = new FileDaImportare
                        {
                            DataInserimento = DateTime.Now,
                            MsgStato = "Da processare",
                            PathCompleto = ff,
                            StatoEsecuzione = 0
                        };
                        files.Add(nf);
                        nuoviFiles = true;
                    }
                }

                if (FilesDaElaborare.Count < files.Count)
                {
                    var elaborati = files.Where(x => !FilesDaElaborare.Contains(x.PathCompleto)).ToList();
                    foreach (var vv in elaborati)
                    {
                        files.Remove(vv);
                    }
                }

                if (nuoviFiles)
                {
                    notifyIcon1.BalloonTipTitle = "Test XCM";
                    notifyIcon1.BalloonTipText = "Ci sono file da iportare";
                    notifyIcon1.ShowBalloonTip(3000);
                }
            }
            finally
            {
                gridView1.EndUpdate();
                gridView1.EndDataUpdate();
            }
        }

        private void simpleButtonTestaAutomazione_Click(object sender, EventArgs e)
        {

            var selezionato = gridView1.GetFocusedRow() as FileDaImportare;

            if (selezionato != null)
            {
                if (selezionato.PathFile == "PMS")
                {
                    if (Path.GetExtension(selezionato.PathCompleto).ToLower() == ".txt")
                    {

                        var testo = File.ReadAllText(selezionato.PathCompleto);
                        var fDest = Path.Combine(Path.GetDirectoryName(selezionato.PathCompleto), "ELABORATI", selezionato.NomeFileEXT);
                        var NuovoOrdine = TextToCsv.ConvertiTestoOrdinePMS(testo, out Exception ex);
                        if (NuovoOrdine != null && NuovoOrdine.Count > 1)
                        {

                            var excelTemp = TextToCsv.CopiaModelloExcel(selezionato.PathCompleto);

                            var pathCSV = TextToCsv.PopolaIlFileExcelConIlNuovoOrdineESalvaInCSV(NuovoOrdine, excelTemp,"");

                            if (!string.IsNullOrEmpty(pathCSV))
                            {
                                var D = Path.GetDirectoryName(selezionato.PathCompleto);
                                var inD = Path.Combine(D, "ELABORATI\\IMPORT_AUTOMATICO");
                                if (!Directory.Exists(inD)) Directory.CreateDirectory(inD);
                                var fDest2 = Path.Combine(inD, Path.GetFileName(pathCSV));
                                if (File.Exists(fDest2)) File.Delete(fDest2);
                                File.Copy(pathCSV, fDest2);
                                File.Delete(pathCSV);


                                //InvioAvvisoInQualcheModo
                            }



                            gridView1.BeginUpdate();
                            selezionato.MsgStato = "Terminato";
                            gridView1.EndUpdate();
                            File.Copy(selezionato.PathCompleto, fDest);
                            File.Delete(selezionato.PathCompleto);
                        }

                        if (ex != null && !File.Exists(fDest))
                        {
                            var eeeer = Path.Combine(Path.GetDirectoryName(selezionato.PathCompleto), "ELABORATI\\IN_ERRORE");
                            if (!Directory.Exists(eeeer)) Directory.CreateDirectory(eeeer);
                            var nonf = Path.Combine(eeeer, selezionato.NomeFileEXT);
                            File.Copy(selezionato.PathCompleto, nonf);
                            File.Delete(selezionato.PathCompleto);
                            GestoreMail.InviaMail($"Non è stato possibile importare in automatico il file {selezionato.NomeFileEXT} a causa del seguente errore:\r\n{ex.Message}",
                                $"Errore importazione per il documento {selezionato.NomeFileEXT}", null, null);
                            //Invia mail con eccezione a qualcuno per inserimento manuale
                        }
                    }
                }
                else if (selezionato.PathFile == "FarmaImpresa")
                {
                    var testoPDF = "";
                    try
                    {
                        testoPDF = PdfClass.ReadPDF(selezionato.PathCompleto);
                    }
                    catch (Exception ee)
                    {
                        gridView1.BeginUpdate();
                        selezionato.MsgStato = "Errore nell'elaborazione del testo pdf";
                        gridView1.EndUpdate();
                    }


                    Exception ex = null;
                    var fDest = Path.Combine(Path.GetDirectoryName(selezionato.PathCompleto), "ELABORATI", selezionato.NomeFileEXT);
                    var NuovoOrdine = TextToCsv.ConvertiTestoOrdineFarmaImpresa(testoPDF, out ex);
                    if (NuovoOrdine != null)
                    {
                        var excelTemp = TextToCsv.CopiaModelloExcel(selezionato.PathCompleto);

                        var pathCSV = TextToCsv.PopolaIlFileExcelConIlNuovoOrdineESalvaInCSV(NuovoOrdine, excelTemp,"");

                        if (!string.IsNullOrEmpty(pathCSV))
                        {
                            var D = Path.GetDirectoryName(selezionato.PathCompleto);
                            var inD = Path.Combine(D, "ELABORATI\\IMPORT_AUTOMATICO");
                            if (!Directory.Exists(inD)) Directory.CreateDirectory(inD);
                            var fDest2 = Path.Combine(inD, Path.GetFileName(pathCSV));
                            if (File.Exists(fDest2)) File.Delete(fDest2);
                            File.Copy(pathCSV, fDest2);
                            File.Delete(pathCSV);


                            //InvioAvvisoInQualcheModo
                        }

                        gridView1.BeginUpdate();
                        selezionato.MsgStato = "Terminato";
                        gridView1.EndUpdate();
                        File.Copy(selezionato.PathCompleto, fDest);
                        File.Delete(selezionato.PathCompleto);
                    }
                    if (ex != null && !File.Exists(fDest))
                    {
                        var eeeer = Path.Combine(Path.GetDirectoryName(selezionato.PathCompleto), "ELABORATI\\IN_ERRORE");
                        if (!Directory.Exists(eeeer)) Directory.CreateDirectory(eeeer);
                        var nonf = Path.Combine(eeeer, selezionato.NomeFileEXT);
                        File.Copy(selezionato.PathCompleto, nonf);
                        File.Delete(selezionato.PathCompleto);
                        GestoreMail.InviaMail($"Non è stato possibile importare in automatico il file {selezionato.NomeFileEXT} a causa del seguente errore:\r\n{ex.Message}",
                            $"Errore importazione per il documento {selezionato.NomeFileEXT}", null, null);
                        //Invia mail con eccezione a qualcuno per inserimento manuale
                    }
                }
                else if(selezionato.PathFile == "")
                {
                    
                }
            }
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadWordToDo();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.Visible = true;

                notifyIcon1.BalloonTipTitle = "Test XCM";
                notifyIcon1.BalloonTipText = "Applicazione ridotta ad icona";

                notifyIcon1.ShowBalloonTip(3000);
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            //notifyIcon1.Visible = false;
        }
    }
}
