using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnitexRemoteClient
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
            ConnectionManager.userAPI = textBoxUser.Text.Trim();
            ConnectionManager.pswAPI = textBoxPassword.Text.Trim();

            if (checkEditRicordami.Checked)
            {
                ConnectionManager.SalvaInfoCryptate(textBoxUser.Text.Trim(), textBoxPassword.Text);
            }
            else
            {
                ConnectionManager.SalvaInfoCryptate("", "");
            }
            this.DialogResult = DialogResult.OK;
        }

        private void simpleButtonAnnulla_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
