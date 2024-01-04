using FluentFTP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnitexRemoteClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.Text = $"Unitex Remote Client {Assembly.GetEntryAssembly().GetName().Version}";
        }

        private void simpleButtonSpedizioni_Click(object sender, EventArgs e)
        {
            var sp = new Spedizioni();
            this.Hide();
            sp.ShowDialog();
            this.Show();
        }

        private void simpleButtonPrenotaRitiro_Click(object sender, EventArgs e)
        {
            var ritiri = new FormRitiri();
            this.Hide();
            ritiri.ShowDialog();
            this.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("Connessione ad internet assente\r\nImpossibile continuare");
                AggiornaLabelStatus(false);
                return;
            }
            
        }
        private void CercaAggiornamenti()
        {
            try
            {
                splashScreenManager1.ShowWaitForm();
                splashScreenManager1.SetWaitFormCaption("Controllo aggiornamenti");

                var tmp = Path.GetTempFileName();
                var client = new FtpClient();
                client.Host = "185.30.181.192";
                client.Port = 221;
                
                client.Credentials = new NetworkCredential("unitexUpdate", "!UnitEx-IT.Password@");
                client.Connect();

                var tt = client.DirectoryExists("/UNITEX_REMOTE_CLIENT");
                var fff = client.FileExists("/UNITEX_REMOTE_CLIENT/vers.info");

                var ttt = client.GetListing();
                client.DownloadFile(tmp, "/UNITEX_REMOTE_CLIENT/vers.info");

                var vv = File.ReadAllLines(tmp);
                var localVersion = Application.ProductVersion;
                string nuovoDaScaricare = "";
                if (vv[0] != localVersion)
                {
                    Properties.Settings.Default.callUpdate = true;
                    Properties.Settings.Default.Save();

                    foreach (FtpListItem item in client.GetListing("/UNITEX_REMOTE_CLIENT"))
                    {
                        //if (item.Type == FtpFileSystemObjectType.File)
                        //{
                        //    if (item.Name.EndsWith(".msi"))
                        //    {
                        //        nuovoDaScaricare = item.FullName;
                        //        break;
                        //    }
                        //}
                    }

                    var tmp2 = Path.ChangeExtension(Path.GetTempFileName(), ".msi");
                    splashScreenManager1.SetWaitFormCaption("Nuova versione disponibile\r\nDownload in corso...");

                    client.DownloadFile(tmp2, nuovoDaScaricare);

                    Process.Start(tmp2);
                    Environment.Exit(0);
                }
            }
            finally
            {
                Thread.Sleep(1000);
                splashScreenManager1.CloseWaitForm();
            }

        }

        private void AggiornaLabelStatus(bool connessioneOK)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                if (connessioneOK)
                {
                    labelControlStatoConnessione.Text = "Connessione stabilita";
                    labelControlStatoConnessione.BackColor = Color.PaleGreen;
                    labelControlStatoConnessione.ForeColor = Color.Black;
                    simpleButtonSpedizioni.Enabled = true;
                    simpleButtonPrenotaRitiro.Enabled = true;
                    simpleButtonLogin.Enabled = false;
                }
                else
                {
                    labelControlStatoConnessione.Text = "Connessione rifiutata";
                    labelControlStatoConnessione.BackColor = Color.Tomato;
                    labelControlStatoConnessione.ForeColor = Color.White;
                    simpleButtonSpedizioni.Enabled = false;
                    simpleButtonPrenotaRitiro.Enabled = false;
                    simpleButtonLogin.Enabled = true;
                }
            });
        }

        private void simpleButtonLogin_Click(object sender, EventArgs e)
        {
            if (ConnectionManager.RecuperaConnessione())
            {
                AggiornaLabelStatus(true);
            }
            else
            {
                AggiornaLabelStatus(false);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
#if !DEBUG
            CercaAggiornamenti();
#endif
            simpleButtonLogin_Click(null, null);
        }
    }
}
