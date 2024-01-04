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

namespace ZebraUtils
{
    public partial class Form1 : Form
    {
        ZPLLibUtilsClass zpl = new ZPLLibUtilsClass();
        public Form1()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var bcs = File.ReadAllLines(@"E:\fsc_bc.txt");

            foreach (var b in bcs)
            {

                zpl.StampaFSC(b);
            }

            zpl.stampaBC128VivisolNew("104033", "18243352FD", "5");
          
        }
    }
}
