using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace GestionePersonale
{
    public partial class Form1 : Form
    {
        BarcodeReader barcodeReader = new BarcodeReader();
       
        

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

      
        private void From1_CLosing(object sender, EventArgs e)
        {
           
        }

    
    }
}
