using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XCM_Remote_Client
{
    public partial class FormLicenza : XtraForm
    {
        string UID = "";
        public FormLicenza(string UID)
        {
            InitializeComponent();
            this.UID = UID;

        }

        private void simpleButtonVerifica_Click(object sender, EventArgs e)
        {
            var codCli = textBoxCodiceCliente.Text;
            if (string.IsNullOrEmpty(textBoxCodiceLicenzaCry.Text))
            {
                MessageBox.Show("Codice licenza vuoto", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (string.IsNullOrEmpty(codCli) || codCli.Length != 5 || codCli.Substring(0, 2) != "00")
            {
                MessageBox.Show("Codice cliente non conforme", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                try
                {
                    var cUID = textBoxCodiceLicenzaCry.Text;
                    if (UID != cUID)
                    {
                        MessageBox.Show("Licenza non valida\r\nil software verrà chiuso", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Environment.Exit(0);
                    }
                    else
                    {
                        Properties.Settings.Default.CodClienteCry = Crypt.Encrypt(textBoxCodiceCliente.Text.Trim());
                        Properties.Settings.Default.UIDPC = textBoxCodiceLicenzaCry.Text.Trim();
                        Properties.Settings.Default.DEXP = Crypt.Encrypt((DateTime.Now + TimeSpan.FromDays(365)).ToString("yyyy/MM/dd"));
                        Properties.Settings.Default.Save();
                    }
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception)
                {
                    MessageBox.Show("Licenza non valida\r\nil software verrà chiuso", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Environment.Exit(0);
                }
            }
        }

        private void FormLicenza_Load(object sender, EventArgs e)
        {
            textBoxQuestoPc.Text = Crypt.Decrypt(UID);
        }
    }
}
