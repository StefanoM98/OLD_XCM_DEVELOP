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
    public partial class FormRappresentaDocumento : Form
    {
        List<DocumentRow> RigheDocumento = new List<DocumentRow>();
        public FormRappresentaDocumento(Document doc, DocumentRow[] rows)
        {
            InitializeComponent();
            this.Text = $"Documento: {doc.reference}";
            PopolaTestata(doc);
            RigheDocumento.AddRange(rows);

            documentRowBindingSource.DataSource = RigheDocumento;
        }

       

        private void PopolaTestata(Document doc)
        {
            textEditCapDestinatario.Text = doc.consigneeZipCode;
            textEditCittaDestinatario.Text = doc.consigneeLocation;
            textEditIndirizzoDestinatario.Text = doc.consigneeAddress;
            textEditNazioneDestinatario.Text = doc.consigneeCountry;
            textEditProvDestinatario.Text = doc.consigneeDistrict;
            textEditRagSocDestinatario.Text = doc.consigneeDes;
            textEditRegioneDestinatario.Text = doc.consigneeRegion;

            textEditCAPDestinazione.Text = doc.unloadZipCode;
            textEditCittaDestinazione.Text = doc.unloadLocation;
            textEditIndirizzoDestinazione.Text = doc.unloadAddress;
            textEditNazioneDestinazione.Text = doc.unloadCountry;
            textEditProvDestinazione.Text = doc.unloadDistrict;
            textEditRagSocialeDestinazione.Text = doc.unLoadDes;
            textEditRegioneDestinazione.Text = doc.unloadRegion;


            textEditRifOrdine.Text = doc.reference;
            dateEditRifOrdine.DateTime = doc.referenceDate;
            textEditNote.Text = doc.internalNote;
        }
    }
}
