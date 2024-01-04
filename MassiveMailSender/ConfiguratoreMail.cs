using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using MassiveMailSender.Model;
using Newtonsoft.Json;
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

namespace MassiveMailSender
{
    public partial class ConfiguratoreMail : Form
    {
        public MailSettings _ms = new MailSettings();

        public ConfiguratoreMail(MailSettings ms)
        {
            InitializeComponent();
            

            if(ms != null)
            {
                _ms = ms;
                textEditSMTPHost.Text = _ms.SenderSmtpHost;
                textEditSMTPport.Text = _ms.SenderSmtpPort.ToString();
                textEditSenderMail.Text = _ms.SenderMail;
                textEditSenderMailPassword.Text = _ms.SenderMailPassword;
                textEditSenderMailName.Text = _ms.SenderMailName;
            }
  
        }

        private void simpleButtonTestSettings_Click(object sender, EventArgs e)
        {
            
            if (dxErrorProviderConfiguratoreEmail.HasErrors)
            {
 
                var inErrore = dxErrorProviderConfiguratoreEmail.GetControlsWithError();

                foreach (var c in inErrore)
                {
                    c.Focus();
                    break;
                   
                }
 
                return;
            }
            MailSettings msTemp = new MailSettings();

            msTemp.SenderSmtpHost = textEditSMTPHost.Text;
            msTemp.SenderSmtpPort = int.Parse(textEditSMTPport.Text);

            msTemp.SenderMail = textEditSenderMail.Text;
            msTemp.SenderMailPassword = textEditSenderMailPassword.Text;
            msTemp.SenderMailName = textEditSenderMailName.Text;


            TestSettings ts = new TestSettings(msTemp);

            ts.ShowDialog(this);

        }

        private void textEdit_Validating(object sender, CancelEventArgs e)
        {
            Control ctrl = (Control)sender;
            TextEdit te = (TextEdit)sender;

            if (string.IsNullOrEmpty(te.Text))
            {
               dxErrorProviderConfiguratoreEmail.SetError(ctrl, "Il campo non può essere vuoto");
                te.BackColor = Color.Tomato;
            }
            else if(te.Name == textEditSenderMail.Name)
            {
                var domain = te.Text.Split('@')[1];
                if(domain != "unitexpress.it" && domain != "xcmhealthcare.com")
                {
                    dxErrorProviderConfiguratoreEmail.SetError(ctrl, "Attezione programma non abilitato per domini esterni all'azienda.");
                    te.BackColor = Color.Tomato;
                }

            }
            else
            {
                dxErrorProviderConfiguratoreEmail.SetError(ctrl, "");
                te.BackColor = Color.Empty;
            }
           
        }

        private void textEdit_Enter(object sender, EventArgs e)
        {
            var textEdit = (TextEdit)sender;
            if(textEdit != null)
            {
                textEdit.BackColor = Color.PaleGreen;
            } 

        }

        private void textEdit_Leave(object sender, EventArgs e)
        {
            var textEdit = (TextEdit)sender;
            if (textEdit != null)
            {
                textEdit.BackColor = Color.Empty;
            }

        }

        private void checkButtonMostraPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkButtonMostraPassword.Checked)
            {
                textEditSenderMailPassword.Properties.PasswordChar = '\0';
            }
            else
            {
                textEditSenderMailPassword.Properties.PasswordChar = '*';
            }
        }

        private void ConfiguratoreMail_Load(object sender, EventArgs e)
        {
#if DEBUGi
            simpleButton1.Visible = true;
#endif
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            

            textEditSMTPHost.Text = "smtp.gmail.com";
            textEditSMTPport.Text = "587";
            textEditSenderMail.Text = "itsupport@unitexpress.it";
            textEditSenderMailPassword.Text = "!UnitEx-IT.Password@";
            textEditSenderMailName.Text = "IT Support";
        }
    }
}
