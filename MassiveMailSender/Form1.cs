using DevExpress.Web;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraRichEdit.API.Native;
using MassiveMailSender.Model;
using MassiveMailSender.Code;

namespace MassiveMailSender
{
    public partial class Form1 : Form
    {
        volatile bool arrestoDiEmergenza = false;
        public static int progressBarValue = 0;
        public static int emailSendedCount = 0;

        MailSettings ms = null;
        MailingList ml = new MailingList();
        //List<Allegato> listaAllegati = new List<Allegato>();
        Helper helper = new Helper();

        public Form1()
        {
            InitializeComponent();
            if (!Directory.Exists(Signature.appDataDir))
            {
                Directory.CreateDirectory(Signature.appDataDir);
            }
        }

        private void mailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settingsMail = new ConfiguratoreMail(ms);
            settingsMail.ShowDialog(this);
            ReloadMailSettings();
        }

        private void ReloadMailSettings()
        {
            if (File.Exists(MailSettings.fileName))
            {
                ms = JsonConvert.DeserializeObject<MailSettings>(Crypt.Decrypt(File.ReadAllText(MailSettings.fileName)));
                GestoreMail.ConfigureMail(ms);
                labelControlStatusSettings.Visible = true;
                labelControlStatusSettings.Text = "Stato impostazioni: OK";
                labelControlStatusSettings.BackColor = Color.PaleGreen;
            }
            else
            {

                MessageBox.Show(this, "Non risulta configurata la mail d'invio", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                var settingsMail = new ConfiguratoreMail(ms);
                settingsMail.ShowDialog(this);
                if (ms != null)
                {
                    labelControlStatusSettings.Text = "Stato impostazioni: OK";
                    labelControlStatusSettings.BackColor = Color.PaleGreen;
                }
                else
                {

                    labelControlStatusSettings.Text = "Stato impostazioni: KO";
                    labelControlStatusSettings.BackColor = Color.Tomato;
                }
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            textEditSubject.Text = "Presentazione UNITEX Srl";

            userControl11.richEditControlBody.Document.InsertText(userControl11.richEditControlBody.Document.CaretPosition, MailSettings.defaultBody);

            if (Signature.Exist())
            {
                Signature.ReadFromFile();
                var paragraphs = userControl11.richEditControlBody.Document.Paragraphs.Count();
                if (paragraphs > 0)
                {
                    var lastCharPos = userControl11.richEditControlBody.Document.Paragraphs.Last().Range.End;
                    userControl11.richEditControlBody.Document.InsertHtmlText(lastCharPos, Signature._signature);

                }

            }

            ReloadMailSettings();
        }

        private void selezionaMailingListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var xlsFileFilter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.RestoreDirectory = true;
            ofd.Filter = xlsFileFilter;
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            gridView1.BeginDataUpdate();
            try
            {
                ml.Import(ofd.FileName);
                ml.SalvaMailingListTemp();
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, $"Mailing list non valida\r\nScegliere un file valido.\r\nErrore: {ee.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                gridView1.EndDataUpdate();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gridControl2.AllowDrop = true;
            //gridControl3.AllowDrop = true;
            //gridControl2.DataSource = listaAllegati;
            allegatoBindingSource.DataSource = Allegato.listaAllegati;
            //allegatoBindingSource1.DataSource = Allegato.listaAllegati;
            
            gridView2.OptionsView.ShowIndicator = false;

            try
            {
                gridView1.BeginDataUpdate();
                if (File.Exists(MailingList.dbNameTemp))
                {
                    var resp = MessageBox.Show(this, "Risultano dati non salvati, li vuoi caricare?", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resp == DialogResult.Yes)
                    {
                        ml.CaricaMailingList(true);
                    }
                    else
                    {
                        ml.CaricaMailingList(false);
                        File.Delete(MailingList.dbNameTemp);
                    }
                }
                else if (File.Exists(MailingList.dbName))
                {
                    try
                    {
                        ml.CaricaMailingList(false);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(this, $"Import mailing list fallito\r\b{ee.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

                documentRowModelBindingSource.DataSource = MailingList.mailingList;
            }
            finally
            {
                gridView1.EndDataUpdate();

            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (File.Exists(MailingList.dbNameTemp))
            {
                var resp = MessageBox.Show(this, "Sono presenti dati non salvati\r\nvuoi salvarli?", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resp == DialogResult.Yes)
                {
                    File.Delete(MailingList.dbName);
                    File.Move(MailingList.dbNameTemp, MailingList.dbName);
                }
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            var r = gridView1.GetRow(e.RowHandle) as DocumentRowModel;
            if (r != null)
            {
                ml.AggiornaRecordMailingList(r);
            }
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.CellValue == null) return;
            if (e.Column.FieldName == colMail.FieldName)
            {
                if (string.IsNullOrEmpty(e.CellValue.ToString()) || !helper.MailIsValid(e.CellValue.ToString()))
                {
                    e.Appearance.BackColor = Color.Tomato;
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            arrestoDiEmergenza = false;

            #region ProgressBarConfiguration
            progressBarValue = 0;
            progressBar1.EditValue = 0;
            progressBar1.Properties.Maximum = gridView1.DataRowCount;
            progressBar1.Properties.Minimum = 0;
            progressBar1.Properties.Step = 1;
            progressBar1.Properties.PercentView = true;
            progressBar1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            progressBar1.LookAndFeel.UseDefaultLookAndFeel = false;
            #endregion

            var subject = textEditSubject.Text;
            Document doc = userControl11.richEditControlBody.Document;
            var body = doc.HtmlText;

            if (string.IsNullOrEmpty(Signature._signature))
            {
                var resultMsg = MessageBox.Show(this, "Attenzione firma mail non configurata, continuare?", "Errore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultMsg != DialogResult.Yes)
                    return;
            }
            else
            {
                //body = helper.ValidateBody(body);
                body = helper.ReplaceSrcImage(body);
            }

            if (string.IsNullOrEmpty(subject))
            {
                MessageBox.Show(this, "Attenzione il campo oggetto è vuoto", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(body))
            {
                MessageBox.Show(this, "Attenzione il campo messaggio è vuoto", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Allegato.listaAllegati.Count == 0)
            {
                var res2 = MessageBox.Show(this, "Allegati non presenti, sicuro di voler continuare?", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res2 != DialogResult.Yes)
                {
                    return;
                }
            }

            var res = MessageBox.Show(this, $"Stai per inviare {gridView1.DataRowCount} mail\r\nSi vuole procedere?", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res != DialogResult.Yes)
            {
                return;
            }
            List<int> righeDaAggiornare = new List<int>();
            Task.Factory.StartNew(() =>
            {

                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    if (arrestoDiEmergenza)
                    {
                        MostraMessaggio("Arresto effettuato correttamente", "Attenzione", true);
                        progressBarValue = 0;
                        AggiornaProgressBarValue();
                        break;
                    }

                    var row = gridView1.GetRow(i) as DocumentRowModel;

                    if (!string.IsNullOrEmpty(row.Mail))
                    {
                        GestoreMail.SendMail(Allegato.listaAllegati.Select(x => x.fullPath).ToList(), row.Mail, subject, body);
                        righeDaAggiornare.Add(row.Id);
                        //gridView1.SetRowCellValue(i, colDataInvioEmail, DateTime.Now);
                    }
                    AggiornaProgressBarValue();
                }
                MostraMessaggio($"Invio completato correttamente", "Informazione", false);

            }).ContinueWith(t => { 

                if(t.Exception != null)
                {
                    MessageBox.Show(this, $"{t.Exception.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        gridView1.BeginDataUpdate();
                        foreach(var i in righeDaAggiornare)
                        {
                            MailingList.mailingList.Where(x => x.Id == i).FirstOrDefault().DataInvioEmail = DateTime.Now.Date;
                        }
                    }
                    finally
                    {
                        gridView1.EndDataUpdate();
                    }
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }
        private void ClearEmailSendedCount()
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                emailSendedCount = 0;
            });
        }

        private void IncrementEmailSendedCount()
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                emailSendedCount++;
            });
        }

        private void MostraMessaggio(string msg, string title, bool msgTypeWarning)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                if (msgTypeWarning)
                    MessageBox.Show(this, msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show(this, msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }

        private void AggiornaProgressBarValue()
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                progressBar1.EditValue = progressBarValue;
                progressBar1.Update();

            });
        }

        private void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.ListSourceRowIndex < 0) return;
            if (e.Column.FieldName == colDataInvioEmail.FieldName)
            {
                if ((DateTime)e.Value == DateTime.MinValue)
                {
                    e.DisplayText = "Non inviata";

                }
            }

        }

        private void simpleButtonArresto_Click(object sender, EventArgs e)
        {
            arrestoDiEmergenza = true;
        }

        private void gridControl2_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                gridView2.BeginDataUpdate();
                //gridView3.BeginDataUpdate();
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                int c = 0;
                foreach (var f in files)
                {
                    Allegato.listaAllegati.Add(new Allegato
                    {
                        id = ++c,
                        fullPath = f
                    });
                }
            }
            finally
            {
                gridView2.EndDataUpdate();
                //gridView3.EndDataUpdate();


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

        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var r = gridView2.GetFocusedRow() as Allegato;

            if (r == null) return;

            var res = MessageBox.Show(this, $"Sicuro di voler eliminare l'allegato\r\n{r.fileName}", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res != DialogResult.Yes) return;

            try
            {
                gridView2.BeginDataUpdate();

                if (r != null)
                {
                    Allegato.listaAllegati.Remove(r);
                }
            }
            finally
            {
                gridView2.EndDataUpdate();

            }
        }

        private void gridView2_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = gridView2.FocusedColumn != gridColumnDelete;
        }

        private void firmaMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FirmaEmail fm = new FirmaEmail();
            fm.ShowDialog(this);

            if (Signature.changed)
            {
                userControl11.richEditControlBody.Document.Delete(userControl11.richEditControlBody.Document.Range);

                userControl11.richEditControlBody.Document.InsertText(userControl11.richEditControlBody.Document.CaretPosition, MailSettings.defaultBody);

                var paragraphs = userControl11.richEditControlBody.Document.Paragraphs.Count();
                if (paragraphs > 0)
                {
                    var lastCharPos = userControl11.richEditControlBody.Document.Paragraphs.Last().Range.End;
                    userControl11.richEditControlBody.Document.InsertHtmlText(lastCharPos, Signature._signature);

                }
                Signature.changed = false;
            }

        }
    }
}
