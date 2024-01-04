using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovimentiMagazzinoFromGespe
{
    public partial class Form1 : Form
    {

        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MinValue;
        List<Tuple<string, string, string>> Customer = new List<Tuple<string, string, string>>();
        List<DDT> magazzinoPerDDTs = new List<DDT>();
        //List<uvwWmsRegistrations> DBR = null;
        public Form1()
        {
            InitializeComponent();
            var now = DateTime.Now;

            bool IsGennaio = now.Month == 1;
            if (!IsGennaio)
            {
                var dinM = DateTime.DaysInMonth(now.Year, now.Month - 1);
                startDate = new DateTime(now.Year, now.Month - 1, 1);
                endDate = new DateTime(now.Year, now.Month - 1, dinM) + TimeSpan.FromDays(1) - TimeSpan.FromMinutes(1);
            }
            else
            {
                //int m = 12;
                var dinM = DateTime.DaysInMonth(now.Year, 12);
                startDate = new DateTime(now.Year, now.Month, 1);
                endDate = new DateTime(now.Year, now.Month, dinM);
            }

            dateEditAccessiDal.DateTime = startDate;
            dateEditAccessiAl.DateTime = endDate;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopolaClienti();
        }

        private void PopolaClienti()
        {
            Customer.Add(new Tuple<string, string, string>("00003", "DALTON", "info@daltonfarmaceutici.it"));
            Customer.Add(new Tuple<string, string, string>("00002", "DOMUS", "info@domuspetri.it"));
            Customer.Add(new Tuple<string, string, string>("00006", "FARMAIMPRESA", "isf@pjpharma.it"));
            Customer.Add(new Tuple<string, string, string>("00008", "PRAEVENIO", "info@praeveniopharma.it"));
            Customer.Add(new Tuple<string, string, string>("00010", "FALQUI", "giusi.merendino@falqui.it"));
            Customer.Add(new Tuple<string, string, string>("00018", "NGD", "info@ngdpharma.it,c.tripodi@ngdpharma.it,g.voltan@cdlifepharma.com"));
            Customer.Add(new Tuple<string, string, string>("00016", "NGF", "amministrazione@ngfsciencesrl.com"));
            Customer.Add(new Tuple<string, string, string>("00020", "KPS", "francesco.palumbo@kps-cso.com"));
            Customer.Add(new Tuple<string, string, string>("00012", "CD LIFE", "giovanni.liore@cdlifepharma.com"));
            Customer.Add(new Tuple<string, string, string>("00015", "PH PH", "ordini@pharmadaypharmaceutical.it"));
            Customer.Add(new Tuple<string, string, string>("00013", "POLARIS", "ordini@polarisfarmaceutici.com"));
            Customer.Add(new Tuple<string, string, string>("00011", "PMS", "amedeo.pizzoferrato@libero.it"));
            Customer.Add(new Tuple<string, string, string>("00007", "VIVISOL", "a.gentile@vivisol.it"));
            Customer.Add(new Tuple<string, string, string>("00021", "KI GROUP SRL", ""));
            Customer.Add(new Tuple<string, string, string>("00022", "TECHDOW PHARMA ITALY SRL", ""));
            Customer.Add(new Tuple<string, string, string>("00023", "T.CENTER S.R.L.", ""));
            Customer.Add(new Tuple<string, string, string>("00024", "APS", ""));
            comboBoxEdit1.Properties.Items.AddRange(Customer);
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AggiornaDati();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //gridViewMovMag.BeginUpdate();
            //gridViewMovMag.Columns.First(x => x.Name == colNumDDT.Name).GroupIndex = 1;
            //gridViewMovMag.EndUpdate();

        }

        private void dateEditAccessiDal_EditValueChanged(object sender, EventArgs e)
        {
            startDate = dateEditAccessiDal.DateTime;
        }

        private void dateEditAccessiAl_EditValueChanged(object sender, EventArgs e)
        {
            endDate = dateEditAccessiAl.DateTime;
        }

        private void buttonAggiorna_Click(object sender, EventArgs e)
        {
            AggiornaDati();
        }

        private void AggiornaDati(bool tutti = false, Tuple<string, string, string> mandante = null)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                gridViewMovMag.BeginUpdate();
                magazzinoPerDDTs.Clear();
                var db = new GnXcmEntities();
                var idx = comboBoxEdit1.SelectedIndex;
                if (idx < 0 && !tutti) return;

                string cc = "";
                if (mandante == null && !tutti)
                {
                    var cust = comboBoxEdit1.Properties.Items[idx].ToString();
                    cc = cust.Substring(1, 5);
                }
                else if (mandante != null)
                {
                    cc = mandante.Item1.Substring(1, 5);
                }
                //DBR = new GnXcmEntities().uvwWmsRegistrations.Where(x => x.DateReg >= dateEditAccessiDal.DateTime && x.DateReg <= dateEditAccessiAl.DateTime && x.CustomerID == cc).ToList();
                List<DDT> dDTs = new List<DDT>();
                if (!tutti)
                {
                    List<uvwWmsDocumentRows_XCM> DDTDelMese = db.uvwWmsDocumentRows_XCM.Where(x => x.CustomerID == cc && (x.DocTip == 204 || x.DocTip == 202) && x.DocDta >= startDate && x.DocDta <= endDate).OrderBy(x => x.uniq).ToList();
                    foreach (var ddt in DDTDelMese)
                    {
                        DDT nr = CreaNuovoRecord(ddt);
                        dDTs.Add(nr);
                    }
                }
                else
                {
                    var DDTDelMese = db.uvwWmsDocumentRows_XCM.Where(x => (x.DocTip == 204 || x.DocTip == 202) && x.DocDta >= startDate && x.DocDta <= endDate);

                    foreach (var ddt in DDTDelMese)
                    {
                        DDT nr = CreaNuovoRecord(ddt);
                        dDTs.Add(nr);
                    }
                }

                magazzinoPerDDTs = dDTs;
                dDTBindingSource.DataSource = magazzinoPerDDTs;
            }
            finally
            {
                gridViewMovMag.EndUpdate();
                Cursor = Cursors.Default;
            }
        }

        private DDT CreaNuovoRecord(uvwWmsDocumentRows_XCM ddt)
        {
            return new DDT
            {
                rowIdLink = ddt.RowIdLink,
                Pallet = ddt.Pallets,
                uniq = ddt.uniq,
                TipoMovimentazione = ddt.RegTypeID,
                DataDDT = ddt.DocDta,
                Committente = ddt.ConsigneeName,
                Destinatario = ddt.SenderName,
                NomeDestinatazione = ddt.UnloadName,
                NumDDT = ddt.DocNum2,
                ProvDestinazione = ddt.UnloadDistrict,
                RifOrdine = ddt.Reference,
                NoteDDT = ddt.Info,
                Corriere = ddt.CarrierDes,
                Causale = ddt.Project,
                CodiceProdotto = ddt.PrdCod,
                ImportoUnitario = ddt.SellPrice,
                Lotto = ddt.Batchno,
                PesoUnitario = ddt.NetWeight,
                NumeroColli = ddt.Boxes,
                ConfezioniPerCollo = ddt.Packs,
                Quantita = ddt.Qty,
                Scadenza = ddt.DateExpire,
                Sconto = ddt.Discount,
                OrdineGespe = ddt.ORderDocNum,
                IndirizzoDestinazione = ddt.UnloadAddress,
                Citta = ddt.CarrierLocation,
                Regione = ddt.UnloadRegion,
                ShipGespe = ddt.ShipDocNum,
                TripGespe = ddt.TripDocNum,
                GruppoProdotto = ddt.PrdGrp,
                TemperaturaTrasporto = ddt.PrdTree,
                AliquotaIva = ddt.VatID,
                DocNumGespe = ddt.DocNum,
                CodMandante = ddt.CustomerID,
                Mandante = ddt.CustomerDes,
                DescrizioneProdotto = ddt.PrdDes
            };
        }

        private void simpleButtonEsportaXslx_Click(object sender, EventArgs e)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var savepath = Path.Combine(desktop, "Export Documenti Magazzino");
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }
            var mandante = comboBoxEdit1.SelectedItem as Tuple<string, string, string>;
            string finalDest;
            if (mandante != null)
            {
                finalDest = Path.Combine(savepath, $"Export_{mandante.Item2}_{DateTime.Now.ToString("ddMMyyyy")}.xlsx");
            }
            else
            {
                finalDest = Path.Combine(savepath, $"Export_Documenti_Magazzino_dal{startDate.ToString("ddMMyyyy")}_al_{endDate.ToString("ddMMyyyy")}.xlsx");
            }

            if (File.Exists(finalDest)) File.Delete(finalDest);
            gridViewMovMag.ExportToXlsx(finalDest);

            Process.Start(savepath);
        }

        private void simpleButtonEsportaTutti_Click(object sender, EventArgs e)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var savepath = Path.Combine(desktop, "Export Movimentazioni Magazzino");
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }
            foreach (var m in comboBoxEdit1.Properties.Items)
            {

                var mandante = m as Tuple<string, string, string>;
                var db = new GnXcmEntities();
                AggiornaDati(false, mandante);

                var finalDest = Path.Combine(savepath, $"Export_{mandante.Item2}_{DateTime.Now.ToString("ddMMyyyy")}.xlsx");

                gridViewMovMag.ExportToXlsx(finalDest);
            }
        }

        private void buttonOggi_Click(object sender, EventArgs e)
        {
            startDate = dateEditAccessiDal.DateTime = DateTime.Now.Date;
            endDate = dateEditAccessiAl.DateTime = DateTime.Now.Date + new TimeSpan(23, 59, 59);
            AggiornaDati();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            startDate = dateEditAccessiDal.DateTime = DateTime.Now.Date;
            endDate = dateEditAccessiAl.DateTime = DateTime.Now.Date + new TimeSpan(23, 59, 59);
            AggiornaDati(true);
            gridViewMovMag.BeginUpdate();
            gridViewMovMag.Columns.First(x => x.Name == colMandante.Name).GroupIndex = 1;
            gridViewMovMag.EndUpdate();

        }

        private void buttonTuttiTraLeDate_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show(this, "Eseguire la richiesta solo al di fuori dell'orario di servizio del magazzino\r\nVuole continuare?", "Attenzione",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (resp == DialogResult.Yes)
            {
                startDate = dateEditAccessiDal.DateTime;
                endDate = dateEditAccessiAl.DateTime;
                AggiornaDati(true);
            }
        }

        private void gridViewMovMag_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
          
        }

        private decimal CalcoloFatturazione(GridView view, int index)
        {
            decimal qta = Convert.ToDecimal(view.GetListSourceRowCellValue(index, colQuantita));
            string causale = view.GetListSourceRowCellValue(index, colTipoMovimentazione).ToString();
            string descrizione = view.GetListSourceRowCellValue(index, colDescrizioneProdotto).ToString();
            decimal colli = Convert.ToDecimal(view.GetListSourceRowCellValue(index, colNumeroColli));
            decimal confezioni = Convert.ToDecimal(view.GetListSourceRowCellValue(index, colConfezioniPerCollo));
            string mandante = view.GetListSourceRowCellValue(index, colCodMandante).ToString();
            //long idDoc = view.GetListSourceRowCellValue(index,col)
            if (causale == "OUT" || causale == "TRASF")
            {
                if (mandante == "00007")
                {
                    bool isPezzosingolo = qta / confezioni == 1;
                    if (isPezzosingolo)
                    {
                        decimal tt = qta / 5;
                        var ttr = Math.Ceiling(tt);
                        return ttr * 0.20M;
                    }
                    else
                    {
                        return Math.Ceiling(confezioni) * 0.20M;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else if (causale == "IN")
            {
                if (mandante == "00007")
                {
                    //var righePos = DBR.Where(x=>x.UniqDocRow == )
                }
                return 0;
            }
            else
            {
                return 0;
            }
        }

        private void buttonResetLayout_Click(object sender, EventArgs e)
        {

        }
    }
}
