using QRCoder;
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


namespace GreenPassValidator
{
    public partial class GeneratoreQRCode : Form
    {
        internal class anagraficaLocale
        {
            internal string nome { get; set; }
            internal string cognome { get; set; }
            internal long id { get; set; }
            internal string cf { get; set; }

            public override string ToString()
            {
                return $"{id}|{cognome}|{nome}|{cf}";
            }
        }

        List<anagraficaLocale> anaLoc = new List<anagraficaLocale>();
        public GeneratoreQRCode()
        {
            InitializeComponent();
            CaricaDipendentiAnagrafica();
            //textBoxQRText.Text = "XCM|1|D'ISA|PIERO|20-09-1985|DSIPRI85P20B963K";
        }

        private void CaricaDipendentiAnagrafica()
        {
            var db = new ControlloAccessiXCMEntities();

            var tutteLeAna = db.Anagrafica.Where(x => x.COGNOME != "OSPITE").ToList();

            foreach(var a in tutteLeAna)
            {
                var na = new anagraficaLocale()
                {
                    id = a.ID_ANAGRAFICA,
                    cf = a.CODICE_FISCALE,
                    cognome = a.COGNOME,
                    nome = a.NOME
                };
                anaLoc.Add(na);
            }
            comboBoxEdit1.Properties.Items.AddRange(anaLoc);


        }


        private void buttonGeneraQR_Click(object sender, EventArgs e)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(textBoxQRText.Text, QRCodeGenerator.ECCLevel.Q);
            //QRCodeData qrCodeData = qrGenerator.CreateQrCode(textBoxMultiline.Text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(150, Color.Black, Color.White, (Bitmap)pictureBoxLogoQR.Image,30,10);
            //Bitmap qrCodeImage = qrCode.GetGraphic(150);
            pictureBox1.Image = qrCodeImage;
        }

       

        private void buttonSalvaQR_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "jpeg";
            saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFile.FileName = $"QR_{textBoxQRText.Text.Replace("|","_")}.jpeg";
            saveFile.ShowDialog();

            if (!string.IsNullOrEmpty(saveFile.FileName))
            {
                pictureBox1.Image.Save(saveFile.FileName);
            }
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxQRText.Text = $"XCM|{comboBoxEdit1.Text}|";
        }
    }
}
