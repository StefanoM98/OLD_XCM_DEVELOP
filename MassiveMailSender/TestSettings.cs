using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using MassiveMailSender.Model;
using MassiveMailSender.Code;

namespace MassiveMailSender
{
    public partial class TestSettings : XtraForm
    {
        
        MailSettings _mailSettings = null;
        public TestSettings(MailSettings mailSettings)
        {
            InitializeComponent();
            _mailSettings = mailSettings;
        }

        private void TestSettings_Shown(object sender, EventArgs e)
        {
            if (_mailSettings == null)
            {
                MessageBox.Show(this, "Attezione impostazioni non valide\r\nImpossibile proseguire", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void simpleButtonInviaMailTest_Click(object sender, EventArgs e)
        {
            GestoreMail.ConfigureMail(_mailSettings);

            Task.Factory.StartNew(() =>
            {
                GestoreMail.SendMail(new List<string>(), textEditMailTestSettings.Text, "Invio Test", "Invio Test");
            });

            DialogResult result = MessageBox.Show(this, "La mail è stata ricevuta correttamente?", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                DialogResult res2 = MessageBox.Show(this, "Vuoi confermare il salvataggio delle impostazioni?", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (res2 == DialogResult.Yes)
                {
                    SaveMailSettings();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void SaveMailSettings()
        {
            File.WriteAllText(MailSettings.fileName, Crypt.Encrypt(JsonConvert.SerializeObject(_mailSettings)));
        }

        private void textEditMailTestSettings_Validated(object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;
            if (string.IsNullOrEmpty(textEditMailTestSettings.Text))
            {
                dxErrorProvider1.SetError(ctrl, "Il campo non può essere vuoto");
            }
        }

        private void simpleButtonChiudi_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
