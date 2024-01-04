using DevExpress.XtraEditors;
using Newtonsoft.Json;
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
    public partial class FormRitiri : Form
    {
        public FormRitiri()
        {
            InitializeComponent();
        }

        private void FormRitiri_Load(object sender, EventArgs e)
        {
            dateEditRitiro.DateTime = DateTime.Now + TimeSpan.FromDays(1);

#if DEBUG
            textEditExtRif.Text = "RITIRO TEST";

            textEditCAPRitiro.Text = "20019";
            textEditIndirizzoRitiro.Text = "VIA DELLE VIE";
            textEditLocalitaRitiro.Text = "CASERTA";
            textEditPersonaRifRitiro.Text = "MARIO";
            textEditProvRitiro.Text = "CE";
            textEditRagSocialeRitiro.Text = "MARIO ROSSI";
            textEditTelRifRitiro.Text = "3333333333";
            memoEditNoteScarico.Text = "scaricare solo il martedì dalle 10 alle 15";
            spinEditColli.Value = 10;
            spinEditPallet.Value = 1;

            textEditCapConsegna.Text = "20019";
            textEditIndirizzoConsegna.Text = "PIAZZA DELLE PIAZZE";
            textEditLocalitaConsegna.Text = "SETTIMO MILANESE";
            textEditPersRifConsegna.Text = "GIUSEPPE";
            textEditProvConsegna.Text = "MI";
            textEditRagSocialeConsegna.Text = "GIUSEPPE VERDI";
            textEditTelRifConsegna.Text = "3334444444";
            memoEditNoteCarico.Text = "orario di chiusura 13.30-15.30";
#endif
        }

        private void simpleButtonInvia_Click(object sender, EventArgs e)
        {
            try
            {
                ControllaDatiInseriti();
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, ee.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string TipoSegnacolli = "";

            if (spinEditPallet.Value > 0)
            {
                TipoSegnacolli = "PLT";
            }
            else if (spinEditColli.Value > 0)
            {
                TipoSegnacolli = "COLLO";
            }
            else
            {
                TipoSegnacolli = "EDI";
            }

            //costruisci oggetto API
            RootobjectNewShipmentTMS shipmentTMS = new RootobjectNewShipmentTMS();
            var stops = new List<StopNewShipmentTMS>();
            var goods = new List<GoodNewShipmentTMS>();
            var header = new HeaderNewShipmentTMS()
            {
                carrierType = TipoSegnacolli,
                serviceType = "S",
                incoterm = "PF",
                transportType = "8-25",
                docDate = DateTime.Now.ToString("o"),
                externRef = textEditExtRif.Text.Trim(),
                insideRef = "RITIRO-URC",
                internalNote = "RITIRO EFFETTUATO TRAMITE UNITEX REMOTE CLIENT",
                publicNote = memoEditNoteUnitex.Text.Trim(),
                type = "Groupage",
                customerID = ConnectionManager.CustomerGespeID,
                cashCurrency = null,
                cashNote = null,
                cashPayment = null,
                cashValue = 0
            };
            var ng = new GoodNewShipmentTMS()
            {
                packs = (int)spinEditColli.Value,
                floorPallet = (int)spinEditPallet.Value,
                totalPallet = (int)spinEditPallet.Value,
                containerNo = null,
                cube = null,
                depth = null,
                description = null,
                goodsID = null,
                grossWeight = null,
                height = null,
                holderID = null,
                loadStopID = null,
                meters = null,
                netWeight = null,
                packsTypeDes = null,
                packsTypeID = null,
                seat = null,
                type = null,
                unLoadStopID = null,
                width = null
            };
            goods.Add(ng);
            //presa
            var presa = new StopNewShipmentTMS()
            {
                address = textEditIndirizzoRitiro.Text.Trim(),
                country = textEditNazioneRitiro.Text.Trim(),
                contactPhone = textEditTelRifRitiro.Text.Trim(),
                contactName = textEditPersonaRifRitiro.Text.Trim(),
                date = dateEditRitiro.DateTime.ToString("o"),
                description = textEditRagSocialeRitiro.Text.Trim(),
                district = textEditProvRitiro.Text.Trim(),
                note = memoEditNoteCarico.Text.Trim(),
                zipCode = textEditCAPRitiro.Text.Trim(),
                location = textEditLocalitaRitiro.Text.Trim(),
                locationID = null,
                stopID = null,
                type = "P"

            };
            //scarico
            var scarico = new StopNewShipmentTMS()
            {
                address = textEditIndirizzoConsegna.Text.Trim(),
                country = textEditNazConsegna.Text.Trim(),
                contactPhone = textEditTelRifConsegna.Text.Trim(),
                contactName = textEditPersRifConsegna.Text.Trim(),
                date = dateEditRitiro.DateTime.ToString("o"),
                description = textEditRagSocialeConsegna.Text.Trim(),
                district = textEditProvConsegna.Text.Trim(),
                note = memoEditNoteScarico.Text.Trim(),
                zipCode = textEditCapConsegna.Text.Trim(),
                location = textEditLocalitaConsegna.Text.Trim(),
                locationID = null,
                stopID = null,
                type = "D"
            };
            stops.Add(presa);
            stops.Add(scarico);
            shipmentTMS.header = header;
            shipmentTMS.goods = goods.ToArray();
            shipmentTMS.stops = stops.ToArray();

            //riconosci provincia
            string provRitiro = textEditProvRitiro.Text.Trim();

            //rileva TP
            var tp = AnagraficaTP.tPs.FirstOrDefault(x => x.ProvCompetenza.Contains(provRitiro.ToUpper()));
            if (tp == null)
            {
                MessageBox.Show(this, $"Provincia di ritiro {provRitiro} non supportata, contattare il customer care", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //invia oggetto API
            var resp = RestAPI.RestEspritecInsertNewShipment(shipmentTMS, ConnectionManager.tokenAPI);
            var res = JsonConvert.DeserializeObject<RootobjectResponseNewShipmentTMS>(resp.Content);
            if (res.result.status)
            {
                if (res.id > 0)
                {
                    //invia mail notifica
                    var gs = new GestoreMail();
                    gs.SegnalaRichiestaRitiro(tp.email, shipmentTMS);
                    MessageBox.Show(this, "Richiesta di ritiro inserita", "Notifica", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, $"Errore durante l'inserimento dell'ordine di ritiro.\r\nErrore:{res.result.messages}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show(this, $"Errore durante l'inserimento dell'ordine di ritiro.\r\nErrore:{res.result.messages}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void ControllaDatiInseriti()
        {
            if (string.IsNullOrEmpty(textEditProvRitiro.Text.Trim()) || textEditProvRitiro.Text.Trim().Length > 2)
            {
                throw new Exception("Provincia di ritiro non valida");
            }
            if (string.IsNullOrEmpty(textEditRagSocialeRitiro.Text.Trim()))
            {
                throw new Exception("Ragione sociale ritiro non valida");
            }
            if (string.IsNullOrEmpty(textEditIndirizzoRitiro.Text.Trim()))
            {
                throw new Exception("Indirizzo ritiro non valida");
            }
            if (string.IsNullOrEmpty(textEditCAPRitiro.Text.Trim()))
            {
                throw new Exception("CAP ritiro non valida");
            }
            if (string.IsNullOrEmpty(textEditLocalitaRitiro.Text.Trim()))
            {
                throw new Exception("Località ritiro non valida");
            }
            if (string.IsNullOrEmpty(textEditNazioneRitiro.Text.Trim()))
            {
                throw new Exception("Nazione ritiro non valida");
            }
            if (string.IsNullOrEmpty(textEditProvConsegna.Text.Trim()) || textEditProvConsegna.Text.Trim().Length > 2)
            {
                throw new Exception("Provincia di ritiro non valida");
            }
            if (string.IsNullOrEmpty(textEditRagSocialeConsegna.Text.Trim()))
            {
                throw new Exception("Ragione sociale ritiro non valida");
            }
            if (string.IsNullOrEmpty(textEditIndirizzoConsegna.Text.Trim()))
            {
                throw new Exception("Indirizzo ritiro non valida");
            }
            if (string.IsNullOrEmpty(textEditCapConsegna.Text.Trim()))
            {
                throw new Exception("CAP ritiro non valida");
            }
            if (string.IsNullOrEmpty(textEditLocalitaConsegna.Text.Trim()))
            {
                throw new Exception("Località ritiro non valida");
            }
            if (string.IsNullOrEmpty(textEditNazConsegna.Text.Trim()))
            {
                throw new Exception("Nazione ritiro non valida");
            }

            if (dateEditRitiro.DateTime < DateTime.Now + TimeSpan.FromHours(12))
            {
                throw new Exception("Data ritiro non valida");
            }
            if (spinEditColli.Value == 0 && spinEditPallet.Value == 0)
            {
                throw new Exception("Inserire il numero colli o pallet");
            }
        }

        private void textEdit_Leave(object sender, EventArgs e)
        {
            var s = sender as TextEdit;
            if (s != null)
            {
                s.Text = s.Text.ToUpper();
            }
        }
    }
}
