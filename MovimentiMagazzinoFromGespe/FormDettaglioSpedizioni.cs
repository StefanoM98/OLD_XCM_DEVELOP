using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using RestSharp;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MovimentiMagazzinoFromGespe
{
    public partial class FormDettaglioSpedizioni : Form
    {
        public string BaseDati = "";
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MinValue;
        List<Tuple<string, string, string>> Customer = new List<Tuple<string, string, string>>();
        List<DettaglioSpedizioniXCM> ListaSpedizioni = new List<DettaglioSpedizioniXCM>();

        public FormDettaglioSpedizioni()
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

            dateEditDataDa.DateTime = startDate;
            dateEditDataA.DateTime = endDate;

            PopolaClienti();
            gridControlDettaglioSpedizioni.DataSource = ListaSpedizioni;
        }

        private void PopolaClienti()
        {
            var db = new XCM_WMSEntities();
            comboBoxEdit1.Properties.Items.AddRange(db.ANAGRAFICA_CLIENTI.ToList());
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AggiornaDati();
        }

        private void AggiornaDati(bool tutti = false, Tuple<string, string, string> mandante = null)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                gridViewDettaglioSpedizioni.BeginDataUpdate();
                ListaSpedizioni.Clear();
                var DB = new GnXcmEntities();
                var DBX = new XCM_WMSEntities();
                var idx = comboBoxEdit1.SelectedIndex;
                if (idx < 0 && !tutti) return;
                ANAGRAFICA_CLIENTI cc = new ANAGRAFICA_CLIENTI();
                if (!tutti)
                {
                    cc = comboBoxEdit1.Properties.Items[idx] as ANAGRAFICA_CLIENTI;
                    if (!DBX.TMSC_SCAGLIONI.Any(x => x.ID_GESPE == cc.ID_GESPE))
                    {
                        MessageBox.Show(this, "Non esistono listini abbinati al cliente\r\nimpossibile continuare", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }


                var ormGespe = new List<uvwWmsDocument>();
                if (!tutti)
                {
                    ormGespe = DB.uvwWmsDocument.Where(x => x.CustomerID == cc.ID_GESPE && x.DocTip == 204 && x.DocDta >= startDate && x.DocDta <= endDate).ToList();
                }
                else
                {
                    ormGespe = DB.uvwWmsDocument.Where(x => x.DocTip == 204 && x.DocDta >= startDate && x.DocDta <= endDate).ToList();

                }


                var listaDocNUM = new List<string>();
                foreach (var o in ormGespe)
                {
                    listaDocNUM.Add(o.ShipDocNum);
                }

                var shi2 = DB.uvwTmsShipment.Where(x => listaDocNUM.Any(y => y == x.DocNum)).ToList();

                if (shi2 == null || shi2.Count <= 0)
                {
                    MessageBox.Show(this, "Spedizioni non trovate\r\nImpossibile continuare", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (var ord in ormGespe)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(ord.TripUnloadCarrierDes))
                        {
                            continue;
                        }
                        if (ord.DocNum == "08075/DDT")
                        {

                        }
                        var shipConnesso = shi2.First(x => x.Uniq == ord.ShipUniq);
                        var detailShipConnessi = DB.uvwGrdTmsShipmentMovGoods_Detail.Where(x => x.DocNum == shipConnesso.DocNum).ToList();
                        var detailShipConnesso = detailShipConnessi.LastOrDefault();
                        decimal TotAtt = DammiIlPrezzoListinoScaglionato(DBX, ord, shipConnesso, false);
                        decimal TotPass = DammiIlPrezzoListinoScaglionato(DBX, ord, shipConnesso, true);
                        var nds = new DettaglioSpedizioniXCM()
                        {
                            DataDoc = ord.DocDta.Value,
                            NumDoc = ord.DocNum.Trim(),
                            UnloadAddress = ord.UnloadAddress,
                            UnloadCountry = ord.UnloadCountry,
                            UnloadLocation = ord.UnloadLocation,
                            UnloadName = ord.UnloadName,
                            UnloadRegione = ord.UnloadRegion,
                            UnloadZIPCode = ord.UnloadZipCode,
                            PesoReale = shipConnesso.Weight.Value,
                            PesoVolumetrico = shipConnesso.Cube.Value * 300,
                            Volume = shipConnesso.Cube.Value,
                            Vettore = ord.TripUnloadCarrierDes,
                            Colli = shipConnesso.Packs.Value,
                            RifXCM = ord.Reference,
                            Pallet = (detailShipConnesso != null && detailShipConnesso.Pallets == null) ? 0 : detailShipConnesso.Pallets.Value,
                            TotaleAttivo = TotAtt,
                            ShipNum = ord.ShipDocNum,
                            TotalePassivo = TotPass
                        };
                        ListaSpedizioni.Add(nds);
                    }
                    catch (Exception ee)
                    {
                        var nds = new DettaglioSpedizioniXCM()
                        {
                            DataDoc = ord.DocDta.Value,
                            NumDoc = ord.DocNum.Trim(),
                            RifXCM = "ERRORE DATI"
                        };
                        ListaSpedizioni.Add(nds);
                    }
                }
            }
            finally
            {
                gridViewDettaglioSpedizioni.EndDataUpdate();
                Cursor = Cursors.Default;
            }
        }



        private long RecuperaIDFornitore(string vettore)
        {
            var resp = 0;

            if (vettore.ToLower().StartsWith("improta"))
            {
                resp = 1;
            }
            else if (vettore.ToLower().EndsWith("tli"))
            {
                resp = 2;
            }
            else if (vettore.ToLower().StartsWith("gls"))
            {
                resp = 3;
            }
            else
            {
                resp = -1;
            }

            return resp;
        }

        /// <summary>
        /// ritorna il prezzo per scaglione
        /// </summary>
        /// <param name="DB">db entities</param>
        /// <param name="ID_GESPE">id gespe</param>
        /// <param name="unloadRegion">regione di scarico</param>
        /// <param name="pesoListino">prezzo di riferimento dello scaglione</param>
        /// <returns></returns>
        private decimal DammiIlPrezzoListinoScaglionato(XCM_WMSEntities DB, uvwWmsDocument ord, uvwTmsShipment shipConnesso, bool isPassivo)
        {
            string regP = DammiIlNomeRegione(DB, ord.UnloadRegion, ord.UnloadZipCode);
            if (string.IsNullOrWhiteSpace(regP))
            {
                return -1;
            }
            decimal scaglione = 0;
            decimal prezzoListino = 0;
            decimal pesoDal = 0;
            decimal pesoListino = 0;
            decimal pesoVolume = shipConnesso.Cube.Value * 300;
            decimal pesoReale = shipConnesso.Weight.Value;
            string ID_GESPE = ord.CustomerID;

            if (isPassivo)
            {
                if (pesoVolume > pesoReale)
                {
                    pesoListino = pesoReale;
                }
                else
                {
                    if (pesoVolume == 0)
                    {
                        pesoListino = pesoReale;
                    }
                    else
                    {
                        pesoListino = pesoVolume;
                    }

                }
                long FK_FORNITORE = RecuperaIDFornitore(ord.TripUnloadCarrierDes);
                var tt = DB.TMSF_G_LISTINI.Where(x => x.FK_TMSF_FORNITORE == FK_FORNITORE).ToList();
                var ll = DB.TMSF_LISTINI.Where(x => x.FK_FORNITORE == FK_FORNITORE && x.REGIONE.ToLower() == regP &&
                                                                           x.FASCIA_DI_PESO_DA < pesoListino && x.FASCIA_DI_PESO_A >= pesoListino).ToList();

                var ll2 = new TMSF_LISTINI();
                if (ll.Count == 0)
                {
                    ll = DB.TMSF_LISTINI.Where(x => x.FK_FORNITORE == FK_FORNITORE && x.REGIONE.ToLower() == regP).ToList();
                    if (ll.Count == 0)
                    {
                        ll = DB.TMSF_LISTINI.Where(x => x.FK_FORNITORE == FK_FORNITORE && x.REGIONE.ToLower().StartsWith("italia") &&
                                                            x.FASCIA_DI_PESO_DA < pesoListino && x.FASCIA_DI_PESO_A >= pesoListino).ToList();
                        if (ll.Count == 0)
                        {
                            ll = DB.TMSF_LISTINI.Where(x => x.FK_FORNITORE == FK_FORNITORE && x.REGIONE.ToLower().StartsWith("italia")).ToList();

                            if (ll.Count == 0)
                            {

                                return -1;
                            }
                            else
                            {
                                ll2 = ll.Last();
                            }
                        }
                        else
                        {
                            return ll.First().COSTO;
                        }
                    }
                    else
                    {
                        ll2 = ll.Last();
                    }
                }
                else
                {
                    ll2 = ll.Last();
                }

                scaglione = ll2.FASCIA_DI_PESO_A - ll2.FASCIA_DI_PESO_DA;
                prezzoListino = ll2.COSTO;
                pesoDal = ll2.FASCIA_DI_PESO_DA;
            }
            else
            {
                if (pesoVolume > pesoReale)
                {
                    pesoListino = pesoVolume;
                }
                else
                {
                    pesoListino = pesoReale;
                }

                var ll = DB.TMSC_LISTINI.Where(x => x.ID_GESPE == ID_GESPE && x.REGIONE.ToLower() == regP &&
                                                            x.FASCIA_PESO_DA < pesoListino && x.FASCIA_PESO_A >= pesoListino).ToList();

                var ll2 = new TMSC_LISTINI();
                if (ll.Count == 0)
                {
                    ll = DB.TMSC_LISTINI.Where(x => x.ID_GESPE == ID_GESPE && x.REGIONE.ToLower() == regP).ToList();
                    if (ll.Count == 0)
                    {
                        ll = DB.TMSC_LISTINI.Where(x => x.ID_GESPE == ID_GESPE && x.REGIONE.ToLower().StartsWith("italia") &&
                                                           x.FASCIA_PESO_DA < pesoListino && x.FASCIA_PESO_A >= pesoListino).ToList();
                        if (ll.Count == 0)
                        {
                            ll = DB.TMSC_LISTINI.Where(x => x.ID_GESPE == ID_GESPE && x.REGIONE.ToLower().StartsWith("italia")).ToList();
                            if (ll.Count == 0)
                            {
                                return -1;
                            }
                            else
                            {
                                ll2 = ll.Last();
                            }
                        }
                        else
                        {
                            ll2 = ll.Last();
                        }
                    }
                    else
                    {
                        ll2 = ll.Last();
                    }

                }
                else
                {
                    ll2 = ll.Last();
                }

                scaglione = ll2.FASCIA_PESO_A - ll2.FASCIA_PESO_DA;
                prezzoListino = ll2.PREZZO;
                pesoDal = ll2.FASCIA_PESO_DA;
            }

            while (pesoDal + scaglione < pesoListino)
            {
                pesoDal = pesoDal + scaglione;
                prezzoListino += scaglione;
            }
            return prezzoListino;
        }



        private string DammiIlNomeRegione(XCM_WMSEntities DB, string unloadRegion, string cap)
        {
            string resp = "";
            if (string.IsNullOrEmpty(unloadRegion))
            {
                resp = DammiLaRegioneDalCAP(DB, cap).ToLower();
                if (resp == "veneto" || resp.StartsWith("friuli") || resp == "trentino")
                {
                    resp = "triveneto";
                }
            }
            else
            {
                resp = unloadRegion.ToLower();
                if (resp == "veneto" || resp.StartsWith("friuli") || resp == "trentino")
                {
                    resp = "triveneto";
                }

            }
            return resp;
        }

        private string DammiLaRegioneDalCAP(XCM_WMSEntities DB, string cap)
        {
            string resp = "";
            var reg = DB.GEO_IT_CAPOLUOGO.FirstOrDefault(x => x.CAP.Substring(0, 3) == cap.Substring(0, 3));
            if (reg != null)
            {
                resp = reg.REGIONE;
            }
            else
            {
                var reg2 = DB.GEO_IT.FirstOrDefault(x => x.CAP == cap);
                if (reg2 != null)
                {
                    resp = reg2.REGIONE;
                }
                else
                {
                    resp = "";
                }
            }
            return resp;
        }

        //private decimal CalcolaPesoVolumetrico(decimal? cube)
        //{

        //    if()
        //}

        private void dateEditDataDa_EditValueChanged(object sender, EventArgs e)
        {
            startDate = dateEditDataDa.DateTime;
        }

        private void dateEditDataA_EditValueChanged(object sender, EventArgs e)
        {
            endDate = dateEditDataA.DateTime;
        }

        private void simpleButtonDati_Click(object sender, EventArgs e)
        {
            AggiornaDati();
        }

        private void gridViewDettaglioSpedizioni_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {

        }

        private void simpleButtonTuttiTraLeDate_Click(object sender, EventArgs e)
        {
            AggiornaDati(true);
        }

        private void simpleButtonEsportaGriglia_Click(object sender, EventArgs e)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var savepath = Path.Combine(desktop, "Export Documenti Magazzino");
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }
            var idx = comboBoxEdit1.SelectedIndex;
            var mandante = comboBoxEdit1.Properties.Items[idx] as ANAGRAFICA_CLIENTI;
            string finalDest = "";

            finalDest = Path.Combine(savepath, $"Export_spedizioni_{mandante}_{startDate.ToString("ddMMyyyy")}_al_{endDate.ToString("ddMMyyyy")}.xlsx");


            if (File.Exists(finalDest)) File.Delete(finalDest);
            gridViewDettaglioSpedizioni.ExportToXlsx(finalDest);

            Process.Start(savepath);
        }
    }
}
