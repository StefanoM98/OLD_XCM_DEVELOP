using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XCM_Crypt
{
    public partial class FormXCM_REMOTE_CLIENT : Form
    {
        public FormXCM_REMOTE_CLIENT()
        {
            InitializeComponent();
        }

        private void buttonGenera_Click(object sender, EventArgs e)
        {
            textBox2.Text = Crypt.Encrypt(textBox1.Text);
            //textBox2.Text = Crypt.Decrypt(textBox1.Text);
        }
    }
}
