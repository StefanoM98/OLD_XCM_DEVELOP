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
    public partial class FormLogin : DevExpress.XtraEditors.XtraForm
    {
        public FormLogin(string usr, string psw)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(usr))
            {
                textBoxUser.Text = usr;
            }
            if (!string.IsNullOrEmpty(psw))
            {
                textBoxPassword.Text = psw;
                checkEditRicordami.Checked = true;
            }
            else
            {
                checkEditRicordami.Checked = false;
            }
            
        }


        private void simpleButtonVerifica_Click(object sender, EventArgs e)
        {
            FormWMS.usrAPI = textBoxUser.Text.Trim();
            FormWMS.pswAPI = textBoxPassword.Text;

            if (checkEditRicordami.Checked)
            {
                FormWMS.SalvaInfoCryptate(textBoxUser.Text.Trim(), textBoxPassword.Text);
            }
            else
            {
                FormWMS.SalvaInfoCryptate(FormWMS.usrAPI, "");
            }
            this.DialogResult = DialogResult.OK;
        }

        private void simpleButtonAnnulla_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
