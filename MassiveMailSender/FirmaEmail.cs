
using System;
using System.Windows.Forms;
using DevExpress.XtraRichEdit.API.Native;
using MassiveMailSender.Code;

namespace MassiveMailSender
{
    public partial class FirmaEmail : Form
    {
        Helper helper = new Helper();
        

        public FirmaEmail()
        {
            InitializeComponent();
            

        }

        private void FirmaEmail_Load(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(Signature._signature))
            {
                richEditControl1.Document.InsertHtmlText(richEditControl1.Document.CaretPosition, Signature._signature);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                Document doc = richEditControl1.Document;
                var htmlText = doc.HtmlText;
                htmlText = helper.ReplaceSrcImage(htmlText);
                Signature.Save(htmlText);
                MessageBox.Show(this, "Salvataggio della firma avvenuto con successo", "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ee)
            {
                MessageBox.Show(this, $"Errore nel salvataggio della firma.\r\n{ee.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }
    }
}
