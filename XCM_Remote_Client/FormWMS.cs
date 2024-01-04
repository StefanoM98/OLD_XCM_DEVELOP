using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using Microsoft.Win32;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FluentFTP;
using System.IO;
using Newtonsoft.Json.Linq;


namespace XCM_Remote_Client
{

    public partial class FormWMS : XtraForm
    {
        #region Token
        static DateTime DataScadenzaToken_XCM = DateTime.Now;
        static string token_XCM = "";
        #endregion

        #region FTP
        FtpClient client = null;
        #endregion

        public static string usrAPI = Crypt.Decrypt(Properties.Settings.Default.UserAPICry);
        bool isConnected = false;
        List<Anagrafiche> AnagraficheGenerali = new List<Anagrafiche>();
        List<AnagraficheLookUp> ana = new List<AnagraficheLookUp>();
        List<Document> OrdiniMonitorati = new List<Document>();
        List<Document> DDTMonitorati = new List<Document>();

        public static string pswAPI = Crypt.Decrypt(Properties.Settings.Default.PswAPICry);
        string SenderID = Crypt.Decrypt(Properties.Settings.Default.CodClienteCry);
        string UID = Crypt.Decrypt(Properties.Settings.Default.UIDPC);
        string localDBAna = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XCM Healthcare\\XCM Remote Client", "Anagrafiche.db");

#if eDEBUG
        static string endpointAPI_XCM = "https://api.xcmhealthcare.com:9501";
#else
        static string endpointAPI_XCM = "https://api.xcmhealthcare.com:9500";
#endif
        List<Stock> ListStockBinding = new List<Stock>();
        List<DatiNuovoOrdine> NuovoOrdine = new List<DatiNuovoOrdine>();

        #region LoginXCMApi
        public bool XcmLogin()
        {
            if (usrAPI.ToLower() == "administrator")
            {
                MessageBox.Show(this, "Accesso non autorizzato", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                splashScreenManager1.ShowWaitForm();
                var client = new RestClient(endpointAPI_XCM + "/api/token");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json-patch+json");
                request.AddHeader("Cache-Control", "no-cache");
                var body = @"{" + "\n" +
                $@"  ""username"": ""{usrAPI}""," + "\n" +
                $@"  ""password"": ""{pswAPI}""," + "\n" +
                @"  ""tenant"": """"" + "\n" +
                @"}";
                request.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse response = client.Execute(request);
                //MessageBox.Show(this, $"{response.StatusCode}", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var resp = JsonConvert.DeserializeObject<RootobjectLoginXCM>(response.Content);

                if (resp.result.status == true)
                {
                    DataScadenzaToken_XCM = resp.user.expire;
                    token_XCM = resp.user.token;
                    labelStatoConnessione.BackColor = Color.PaleGreen;
                    return true;
                }
                else
                {
                    labelStatoConnessione.BackColor = Color.Tomato;
                    return false;
                }
            }
            finally
            {
                LoginXCM_API();
                Thread.Sleep(1000);
                splashScreenManager1.CloseWaitForm();
            }

        }
        private void RecuperaConnessione()
        {
            isConnected = XcmLogin();
            if (!isConnected)
            {
                throw new Exception("Server al momento non raggiungibile");
            }
        }
        #endregion

        public FormWMS()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
#if DEBUG
            simpleButtonDebug.Visible = true;
#endif
            try
            {
                OrganizzaTabPages();

                #region Licenza
                string idE = VerificaUID();

                if (string.IsNullOrWhiteSpace(Properties.Settings.Default.UIDPC))
                {
                    var fLic = new FormLicenza(idE);
                    fLic.ShowDialog(this);
                }
                else
                {
                    var uidC = Crypt.Decrypt(Properties.Settings.Default.UIDPC);
                    if (uidC != UID)
                    {
                        MessageBox.Show(this, "Questo pc non è abilitato all'uso del software\r\nIl software verrà chiuso", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }
                }
                #endregion

                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    MessageBox.Show("Connessione ad internet assente\r\nImpossibile continuare");
                    xtraTabControl1.Enabled = false;
                    return;
                }

                #region Login
                int ci = 0;
                while (!isConnected)
                {
                    if (ci > 0)
                    {
                        MessageBox.Show(this, "User e/o password errati", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    var loginOK = new FormLogin(usrAPI, pswAPI);
                    loginOK.ShowDialog();
                    if (loginOK.DialogResult != DialogResult.OK)
                    {
                        MessageBox.Show(this, "Impossibile continuare", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        xtraTabControl1.Enabled = false;
                        break;
                    }
                    isConnected = XcmLogin();
                    ci++;
                }
                #endregion
                if (!isConnected) return;
                #region ControlloAggiornamenti
#if !DEBUG
                CercaAggiornamenti();
#endif

                #endregion

                #region Inibisci modifica campi griglia
                foreach (var col in gridViewNuovoOrdine.Columns)
                {
                    var c = col as DevExpress.XtraGrid.Columns.GridColumn;
                    if (c != colQuantita && c != colImportoUnitario && c != colIVA && c != colSconto)
                    {
                        c.OptionsColumn.AllowEdit = false;
                    }
                }
                #endregion

                splashScreenManager1.ShowWaitForm();
                splashScreenManager1.SetWaitFormCaption("Recupero informazioni in corso");

                RecuperaGiacenzeMagazzino();
                dateEditRifOrdine.DateTime = DateTime.Now;

                SettaDateERecuperaDocumenti(true);
                PopolaAnagraficaLocale();
                PopolaLookUpEditAnagrafica();

                gridLookUpEditDestinatario.Properties.PopupFormMinSize = new Size(550, 150);
                gridLookUpEditDestinazione.Properties.PopupFormMinSize = new Size(550, 150);

                #region BSSetting            
                wharehouseStockBindingSource.DataSource = ListStockBinding;
                datiNuovoOrdineBindingSource.DataSource = NuovoOrdine;
                anagraficheBindingSource.DataSource = AnagraficheGenerali;
                documentBindingSource.DataSource = OrdiniMonitorati;
                documentBindingSource1.DataSource = DDTMonitorati;
                anagraficheLookUpBindingSource.DataSource = anagraficheLookUpBindingSource.DataSource = ana;
                #endregion

            }
            finally
            {
                if (splashScreenManager1.IsSplashFormVisible)
                {
                    splashScreenManager1.CloseWaitForm();
                }
            }
        }
        private void PopolaLookUpEditAnagrafica()
        {

            ana.Clear();
            foreach (var a in AnagraficheGenerali)
            {
                var aa = new AnagraficheLookUp()
                {
                    ID = a.ID_ANAGRAFICA,
                    RagioneSociale = a.RagioneSociale,
                    Citta = a.Citta,
                    PIva = a.PIva,
                    Indirizzo = a.Via
                };
                ana.Add(aa);
            }
            anagraficheLookUpBindingSource.DataSource = anagraficheLookUpBindingSource.DataSource = ana;

        }
        private void SettaDateERecuperaDocumenti(bool settaDate)
        {
            try
            {
                codeChange = true;
                gridViewStoricoOrdini.BeginDataUpdate();
                gridViewDDTBEM.BeginDataUpdate();
                OrdiniMonitorati.Clear();
                DDTMonitorati.Clear();
                var dtn = DateTime.Now.Date;
                var dd1s = "";
                var dd2s = "";

                if (settaDate)
                {
                    //TODO CONTROLLARE ORA DELLA DATA
                    var dtDal = dateEditOrdiniDal.DateTime = dateEditDDTdal.DateTime = (dtn - TimeSpan.FromDays(30)).Date;
                    var dtAl = dateEditOrdiniAl.DateTime = dateEditDDTal.DateTime = dtn + TimeSpan.FromDays(7);

                    dd1s = dtDal.ToUniversalTime().ToString("u");
                    dd2s = dtAl.ToUniversalTime().ToString("u");
                }
                else
                {
                    dd1s = dateEditOrdiniDal.DateTime.Date.ToUniversalTime().ToString("u");
                    dd2s = dateEditOrdiniAl.DateTime.ToUniversalTime().ToString("u");
                }

                var client = new RestClient(endpointAPI_XCM + $"/api/wms/document/list/5000/1?FromDate={dd1s}&ToDate={dd2s}");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", $"Bearer {token_XCM}");
                IRestResponse response = client.Execute(request);
                var resp = JsonConvert.DeserializeObject<RootobjectDocument>(response.Content);

                if (resp.documents == null)
                {
                    return;
                }
                var Ordini = resp.documents.Where(x => x.docType == "OrderOUT").ToList();
                var DDTeBEM = resp.documents.Where(x => x.docType == "DeliveryOUT" || x.docType == "DeliveryIN").ToList();
                OrdiniMonitorati.AddRange(Ordini);
                DDTMonitorati.AddRange(DDTeBEM);




            }
            finally
            {
                gridViewStoricoOrdini.EndDataUpdate();
                gridViewDDTBEM.EndDataUpdate();
                codeChange = false;
            }

        }
        private void PopolaAnagraficaLocale()
        {
            //leggi json locale con le anagrafiche e popola la lista anagrafichegenerali
            //throw new NotImplementedException();
            try
            {
                if (!File.Exists(localDBAna)) return;
                var ff = File.ReadAllText(localDBAna);
                if (!string.IsNullOrEmpty(ff))
                {
                    //var anaLoc = JsonConvert.SerializeObject(AnagraficheGenerali, Formatting.Indented);
                    AnagraficheGenerali.AddRange(JsonConvert.DeserializeObject<List<Anagrafiche>>(ff).OrderBy(x => x.ID_ANAGRAFICA).ToList());
                    //AnagraficheGenerali = AnagraficheGenerali;
                }
            }
            catch (Exception ee)
            {

            }
        }
        private void OrganizzaTabPages()
        {
            if (usrAPI == "apisol")
            {
                xtraTabControl1.TabPages.Move(1, xtraTabPageNuovoOrdine);
                xtraTabPageNuovoOrdine.PageVisible = false;
                simpleButtonAggiungiAllOrdine.Visible = false;
                xtraTabControl1.TabPages.Move(2, xtraTabPageGiacenzaMagazzino);
                xtraTabControl1.TabPages.Move(3, xtraTabPageAnagrafiche);
                xtraTabPageAnagrafiche.PageVisible = false;
                xtraTabControl1.TabPages.Move(4, xtraTabPageStoricoOrdini);
                xtraTabControl1.TabPages.Move(5, xtraTabPageDDT);
#if !DEBUG
                xtraTabPageDDT.PageVisible = false;
#endif
                xtraTabControl1.TabPages.Move(6, xtraTabPageAssistenza);
            }
            else
            {
                xtraTabControl1.TabPages.Move(1, xtraTabPageNuovoOrdine);
                xtraTabControl1.TabPages.Move(2, xtraTabPageGiacenzaMagazzino);
                xtraTabControl1.TabPages.Move(3, xtraTabPageAnagrafiche);
                xtraTabControl1.TabPages.Move(4, xtraTabPageStoricoOrdini);
                xtraTabControl1.TabPages.Move(5, xtraTabPageDDT);
#if !DEBUG
                xtraTabPageDDT.PageVisible = false;
#endif
                xtraTabControl1.TabPages.Move(6, xtraTabPageAssistenza);
            }
        }
        private void CercaAggiornamenti()
        {
            try
            {
                splashScreenManager1.ShowWaitForm();
                splashScreenManager1.SetWaitFormCaption("Controllo aggiornamenti");

                var tmp = Path.GetTempFileName();
                client = new FtpClient();
                client.Host = Crypt.Decrypt(Properties.Settings.Default.FDS);
                client.Port = 221;
                client.Credentials = new NetworkCredential(Crypt.Decrypt(Properties.Settings.Default.FUR), Crypt.Decrypt(Properties.Settings.Default.FPW));
                client.Connect();

                client.DownloadFile(tmp, "/XCM_REMOTE_CLIENT/vers.info");

                var vv = File.ReadAllLines(tmp);
                var localVersion = Application.ProductVersion;
                string nuovoDaScaricare = "";
                if (vv[0] != localVersion)
                {
                    Properties.Settings.Default.callUpdate = true;
                    Properties.Settings.Default.Save();

                    foreach (FtpListItem item in client.GetListing("/XCM_REMOTE_CLIENT"))
                    {
                        if (item.Type == FtpFileSystemObjectType.File)
                        {
                            if (item.Name.EndsWith(".msi"))
                            {
                                nuovoDaScaricare = item.FullName;
                                break;
                            }
                        }
                    }

                    var tmp2 = Path.ChangeExtension(Path.GetTempFileName(), ".msi");
                    splashScreenManager1.SetWaitFormCaption("Nuova versione disponibile\r\nDownload in corso...");

                    client.DownloadFile(tmp2, nuovoDaScaricare);

                    Process.Start(tmp2);
                    Environment.Exit(0);
                }
            }
            finally
            {
                Thread.Sleep(1000);
                splashScreenManager1.CloseWaitForm();
            }

        }
        private string VerificaUID()
        {
            var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
            ManagementObjectCollection mbsList = mbs.Get();
            string id = "";
            foreach (ManagementObject mo in mbsList)
            {
                id = mo["ProcessorId"].ToString();
                break;
            }

            ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
            dsk.Get();
            return Crypt.Encrypt(id += "-" + dsk["VolumeSerialNumber"].ToString());
        }
        internal static void SalvaInfoCryptate(string userAPI, string pswdAPI)
        {
            Properties.Settings.Default.UserAPICry = Crypt.Encrypt(userAPI);
            Properties.Settings.Default.PswAPICry = Crypt.Encrypt(pswdAPI);
            Properties.Settings.Default.Save();
        }
        private void RecuperaGiacenzeMagazzino()
        {
            var client = new RestClient(endpointAPI_XCM + $"/api/wms/warehouse/stock/5000/1?ExplodeByBatchno=true");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token_XCM}");
            IRestResponse response = client.Execute(request);

            var allPrd = JsonConvert.DeserializeObject<RootobjectStock>(response.Content);

            //if (allPrd.stock.Any(x => !string.IsNullOrEmpty(x.logWareID))) 
            //{
            //    collogWareID.Visible = true;            
            //}

            var listaRaggruppataPerSommaQuantita = new List<Stock>();

            foreach (var gc in allPrd.stock)
            {
                var esiste = listaRaggruppataPerSommaQuantita.FirstOrDefault(x => x.partNumber == gc.partNumber && x.batchno == gc.batchno && x.logWareID == gc.logWareID);
                if (esiste != null)
                {
                    esiste.availableQty += gc.availableQty;
                    esiste.inUseQty += gc.inUseQty;
                    esiste.totalQty += gc.totalQty;
                }
                else
                {
                    listaRaggruppataPerSommaQuantita.Add(gc);
                }
            }

            ListStockBinding = listaRaggruppataPerSommaQuantita;
            if (usrAPI == "apisol")
            {
                gridViewGiacenzeMagazzino.Columns.First(x => x.FieldName == collogWareID.FieldName).GroupIndex = 1;
            }
            //foreach (var prd in allPrd.stock)
            //{
            //    ListStockBinding.Add(prd);
            //}
        }
        private void simpleButtonInviaOrdine_Click(object sender, EventArgs e)
        {
            try
            {
                if (NuovoOrdine.Count > 0)
                {
                    RecuperaConnessione();
                    CollezionaDatiEdInviaOrdine();
                }
                else
                {
                    MessageBox.Show(this, "Prodotti non presenti in griglia\r\nimpossibile continuare", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Errore irreversibile", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CollezionaDatiEdInviaOrdine()
        {
            //VERIFICA CORRETTEZZA INSERIMENTO DATI
            //INVIA ORDINE TRAMITE API

            ControllaCampiTestata(out bool err);
            if (err)
            {
                return;
            }
            CreaEdInviaOrdine();
        }
        private void ControllaCampiTestata(out bool err)
        {
            err = false;

            if (string.IsNullOrEmpty(textEditFattRifOrdine.Text))
            {
                textEditFattRifOrdine.BackColor = Color.Tomato;
                err = true;
            }
            if (string.IsNullOrEmpty(textEditRagSocialeDestinazione.Text))
            {
                textEditRagSocialeDestinazione.BackColor = Color.Tomato;
                err = true;
            }
            if (string.IsNullOrEmpty(textEditIndirizzoDestinazione.Text))
            {
                textEditIndirizzoDestinazione.BackColor = Color.Tomato;
                err = true;
            }
            if (string.IsNullOrEmpty(textEditFattRifOrdine.Text))
            {
                textEditFattRifOrdine.BackColor = Color.Tomato;
                err = true;
            }
            if (string.IsNullOrEmpty(textEditCAPDestinazione.Text))
            {
                textEditCAPDestinazione.BackColor = Color.Tomato;
                err = true;
            }
            if (string.IsNullOrEmpty(textEditProvDestinazione.Text))
            {
                textEditProvDestinazione.BackColor = Color.Tomato;
                err = true;
            }
            if (string.IsNullOrEmpty(textEditRegioneDestinazione.Text))
            {
                textEditRegioneDestinazione.BackColor = Color.Tomato;
                err = true;
            }
            //----------------------------------------------------------------------//

            if (string.IsNullOrEmpty(textEditRagSocDestinatario.Text))
            {
                textEditRagSocDestinatario.BackColor = Color.Tomato;
                err = true;
            }
            if (string.IsNullOrEmpty(textEditIndirizzoDestinatario.Text))
            {
                textEditIndirizzoDestinatario.BackColor = Color.Tomato;
                err = true;
            }
            if (string.IsNullOrEmpty(textEditCapDestinatario.Text))
            {
                textEditCapDestinatario.BackColor = Color.Tomato;
                err = true;
            }
            if (string.IsNullOrEmpty(textEditProvDestinatario.Text))
            {
                textEditProvDestinatario.BackColor = Color.Tomato;
                err = true;
            }
            if (string.IsNullOrEmpty(textEditRegioneDestinatario.Text))
            {
                textEditRegioneDestinatario.BackColor = Color.Tomato;
                err = true;
            }
        }
        private void CreaEdInviaOrdine()
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("Invio ordine in corso...");

            try
            {
                string regtypeid = "";
                if (comboBoxEditTipoOrdine.Text == "VENDITA")
                {
                    regtypeid = "OUT";
                }
                else if (comboBoxEditTipoOrdine.Text == "OMAGGIO")
                {
                    regtypeid = "OMAG";
                }
                else if (comboBoxEditTipoOrdine.Text == "TRASFERIMENTO")
                {
                    regtypeid = "TRASF";
                }
                else if (comboBoxEditTipoOrdine.Text == "CAMPIONI")
                {
                    regtypeid = "CAMP";
                }
                if (string.IsNullOrEmpty(regtypeid))
                {
                    MessageBox.Show(this, "Errore nel tipo di documento\r\nImpossibile continuare", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DateTime dd = DateTime.Parse(dateEditRifOrdine.EditValue.ToString());
                var NO = new RootobjectOrdine();
                var hr = new HeaderOrdine();
                hr.customerID = SenderID;
                hr.ownerID = SenderID;
                hr.docType = "OrderOUT";
                hr.logWareID = "";//TODO GESTIRE MAGAZZINO LOGICO
                hr.ownerID = SenderID;
                hr.procID = 2;
                hr.reference = textEditFattRifOrdine.Text.Trim();
                hr.referenceDate = dd.ToUniversalTime().ToString("u");
                hr.siteID = "01";
                hr.anaType = 1;
                hr.anaID = SenderID;
                hr.ownerID = SenderID;
                hr.regTypeID = regtypeid;
                hr.publicNote = textEditNote.Text.Trim();
                hr.deliveryNote = memoEditNoteCorriere.Text.Trim();

                var unload = new UnloadOrdine();
                unload.address = PulisciStringOrdine(textEditIndirizzoDestinazione.Text);
                unload.country = PulisciStringOrdine(textEditNazioneDestinazione.Text);
                unload.description = PulisciStringOrdine(textEditRagSocialeDestinazione.Text);
                unload.district = PulisciStringOrdine(textEditProvDestinazione.Text);
                unload.location = PulisciStringOrdine(textEditCittaDestinazione.Text);
                unload.region = PulisciStringOrdine(textEditRegioneDestinazione.Text);
                unload.zipCode = PulisciStringOrdine(textEditCAPDestinazione.Text);

                hr.unload = unload;

                var comm = new ConsigneeOrdine();
                comm.address = PulisciStringOrdine(textEditIndirizzoDestinatario.Text);
                comm.country = PulisciStringOrdine(textEditNazioneDestinatario.Text);
                comm.description = PulisciStringOrdine(textEditRagSocDestinatario.Text);
                comm.district = PulisciStringOrdine(textEditProvDestinatario.Text);
                comm.location = PulisciStringOrdine(textEditCittaDestinatario.Text);
                comm.region = PulisciStringOrdine(textEditRegioneDestinatario.Text);
                comm.zipCode = PulisciStringOrdine(textEditCapDestinatario.Text);

                hr.consignee = comm;

                List<RowOrdine> righe = new List<RowOrdine>();
                foreach (var ro in NuovoOrdine)
                {
                    var rr = new RowOrdine()
                    {
                        batchNo = ro.LottoProdotto.Trim(),
                        discount = ro.Sconto,
                        //logWareId = ro.MagazzinoLogico,
                        partNumber = ro.CodiceProdotto,
                        procID = 2,
                        qty = ro.Quantita,
                        um = ro.UnitaDiMisuta,
                        unitSellPrice = ro.ImportoUnitario//(ro.ImportoUnitario.ToString() != "") ? ro.ImportoUnitario.ToString().Replace(".",",") : "",
                    };
                    righe.Add(rr);
                }
                NO.rows = righe.ToArray();
                NO.header = hr;
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;
                var raw = JsonConvert.SerializeObject(NO, Formatting.Indented, settings);

                var client = new RestClient($"{endpointAPI_XCM}/api/wms/document/new");
                client.Timeout = -1;
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Content-Type", "application/json-patch+json");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", $"Bearer {token_XCM}");
                request.AddParameter("application/json-patch+json", raw, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    //var respNO = JsonConvert.DeserializeObject<RootobjectResponeNewOrder>(response.Content);

                    //AggiornaPrezziByXCM(respNO.id, righe);
                    ResettaTuttiIParametri();
                    RecuperaGiacenzeMagazzino();
                    SettaDateERecuperaDocumenti(true);
                }
                else
                {
                    MessageBox.Show(this, "Errore nell'invio dell'ordine", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                splashScreenManager1.CloseWaitForm();
            }
        }
        private void AggiornaPrezziByXCM(int id, List<RowOrdine> righe)
        {
            RootobjectDocumentRows xcmDoc = new RootobjectDocumentRows();

            var client = new RestClient(endpointAPI_XCM + $"/api/wms/document/row/list/{id}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token_XCM}");
            var response = client.Execute(request);
            var docRows = JsonConvert.DeserializeObject<RootobjectDocumentRows>(response.Content);

            xcmDoc.rows = docRows.rows;
            bool trasmissionePrezziErrore = false;
            foreach (var r in xcmDoc.rows)
            {
                var prezzoCorretto = righe.FirstOrDefault(x => x.partNumber == r.partNumber && x.qty == r.qty && x.batchNo == r.batchNo);
                if (prezzoCorretto != null)
                {
                    var req = new RestClient($"http://185.30.181.192:8092/api/xcm/GetSettaValoriRighaDocumentoOrdine?rowid={r.id}8&price={prezzoCorretto.unitSellPrice}");
                    client.Timeout = -1;
                    var request2 = new RestRequest(Method.GET);
                    var response2 = client.Execute(request);
                }
                else
                {
                    trasmissionePrezziErrore = true;
                }
            }

            if (trasmissionePrezziErrore)
            {
                MessageBox.Show(this,"E' stato rilevato un problema durante la trasmissione del prezzo, si prega di contattare il customer per verifiche", 
                                            "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private string PulisciStringOrdine(string text)
        {
            return text.Replace("\"", "").Trim();
        }
        private void ResettaTuttiIParametri()
        {
            //svuota tutti i campi
            textEditIndirizzoDestinazione.Text = "";
            textEditNazioneDestinazione.Text = "";
            textEditRagSocialeDestinazione.Text = "";
            textEditProvDestinazione.Text = "";
            textEditCittaDestinazione.Text = "";
            textEditRegioneDestinazione.Text = "";
            textEditCAPDestinazione.Text = "";
            textEditIndirizzoDestinatario.Text = "";
            textEditNazioneDestinatario.Text = "";
            textEditRagSocDestinatario.Text = "";
            textEditProvDestinatario.Text = "";
            textEditCittaDestinatario.Text = "";
            textEditRegioneDestinatario.Text = "";
            textEditCapDestinatario.Text = "";
            textEditFattRifOrdine.Text = "";
            textEditNote.Text = "";
            memoEditNoteCorriere.Text = "";
            textEditPivaDestinatario.Text = "";
            //imposta data documento al datetime now
            dateEditRifOrdine.EditValue = DateTime.Now;
            //svuota lista prodotti ordine
            datiNuovoOrdineBindingSource.Clear();
        }
        private void simpleButtonAvviaTeamViewerQS_Click(object sender, EventArgs e)
        {
            Process.Start("TeamViewerQS.exe");
        }
        private void textEdit4_EditValueChanged(object sender, EventArgs e)
        {
            if (textEditProvDestinazione.Text.Length > 2 || textEditProvDestinazione.Text.Length < 1)
            {
                textEditProvDestinazione.Properties.Appearance.BackColor = Color.Tomato;
            }
            else
            {
                textEditProvDestinazione.Properties.Appearance.BackColor = Color.PaleGreen;
            }
        }
        private void gridViewNuovoOrdine_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.Name == colCodiceProdotto.Name)
            {
                var artMag = ListStockBinding.Where(x => x.partNumber == e.Value.ToString().Trim()).ToList();

                if (artMag == null && artMag.Count == 0)
                {
                    return;
                }
                var rh = e.RowHandle;
                var des = artMag[0].partNumberDes;
                if (artMag.Count == 1)
                {
                    var lt = artMag[0].batchno;
                    gridViewNuovoOrdine.SetRowCellValue(rh, colDescrizioneProdotto, des);
                    gridViewNuovoOrdine.SetRowCellValue(rh, colLottoProdotto, lt);
                }
                else
                {
                    gridViewNuovoOrdine.SetRowCellValue(rh, colDescrizioneProdotto, des);
                }
            }
            else if (e.Column == colQuantita)
            {
                var ro = gridViewNuovoOrdine.GetFocusedRow() as DatiNuovoOrdine;
                if (ro != null)
                {
                    if (ro.LottoProdotto == null) ro.LottoProdotto = "";
                    var corrispondente = ListStockBinding.First(x => x.partNumber == ro.CodiceProdotto && x.batchno == ro.LottoProdotto);
                    if (ro.Quantita > corrispondente.availableQty)
                    {
                        ro.Quantita = corrispondente.availableQty;
                        MessageBox.Show(this, "Quantità non disponibile a magazzino\r\ninserita quantità massima", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        int blink = 0;
        private void timerBlinkAggiuntaProdotto_Tick(object sender, EventArgs e)
        {
            if (blink < 1)
            {
                labelInserimentoProdotto.Visible = true;
                blink++;
            }
            else
            {
                timerBlinkAggiuntaProdotto.Stop();
                labelInserimentoProdotto.Visible = false;
                blink = 0;
            }
        }
        private void simpleButtonCopiaDaDestinatario_Click(object sender, EventArgs e)
        {
            textEditCAPDestinazione.Text = textEditCapDestinatario.Text;
            textEditIndirizzoDestinazione.Text = textEditIndirizzoDestinatario.Text;
            textEditNazioneDestinazione.Text = textEditNazioneDestinatario.Text;
            textEditProvDestinazione.Text = textEditProvDestinatario.Text;
            textEditRagSocialeDestinazione.Text = textEditRagSocDestinatario.Text;
            textEditCittaDestinazione.Text = textEditCittaDestinatario.Text;
            textEditRegioneDestinazione.Text = textEditRegioneDestinatario.Text;
        }
        private void simpleButtonDebug_Click(object sender, EventArgs e)
        {
            textEditCAPDestinazione.Text = "81056";
            textEditIndirizzoDestinazione.Text = "via delle vie";
            textEditNazioneDestinazione.Text = "IT";
            textEditProvDestinazione.Text = "CE";
            textEditRagSocialeDestinazione.Text = "de pippis";
            textEditCittaDestinazione.Text = "capocchia";
            textEditRegioneDestinazione.Text = "campania";
            simpleButtonCopiaDaDestinatario_Click(null, null);
            textEditFattRifOrdine.Text = "test1234";
            textEditNote.Text = "ordine test";
        }
        private void textEditProvDestinatario_EditValueChanged(object sender, EventArgs e)
        {
            if (textEditProvDestinatario.Text.Length > 2 || textEditProvDestinatario.Text.Length < 1)
            {
                textEditProvDestinatario.Properties.Appearance.BackColor = Color.Tomato;
            }
            else
            {
                textEditProvDestinatario.Properties.Appearance.BackColor = Color.PaleGreen;
            }
        }
        private void simpleButtonAggiungiAllOrdine_Click(object sender, EventArgs e)
        {
            var rr = gridViewGiacenzeMagazzino.GetRow(gridViewGiacenzeMagazzino.FocusedRowHandle) as Stock;

            var rO = new DatiNuovoOrdine()
            {
                CodiceProdotto = rr.partNumber,
                DescrizioneProdotto = rr.partNumberDes,
                UnitaDiMisuta = rr.um,
                LottoProdotto = rr.batchno,
            };

            datiNuovoOrdineBindingSource.Add(rO);
            labelInserimentoProdotto.Visible = true;
            timerBlinkAggiuntaProdotto.Start();
        }
        private void simpleButtonAggiungiAnagrafica_Click(object sender, EventArgs e)
        {
            try
            {
                ControllaCampiAnagrafica();
                InserisciAnagrafica();
                PopolaLookUpEditAnagrafica();
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, ee.Message, "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ControllaCampiAnagrafica()
        {
            if (string.IsNullOrEmpty(textEditRagSocAnagrafica.Text.Trim()))
            {
                textEditRagSocAnagrafica.BackColor = Color.Tomato;
                throw new Exception("Campo Ragione sociale vuoto");
            }
            else
            {
                textEditRagSocAnagrafica.BackColor = Color.PaleGreen;
            }
            if (string.IsNullOrEmpty(textEditViaAnagrafica.Text.Trim()))
            {
                textEditViaAnagrafica.BackColor = Color.Tomato;
                throw new Exception("Campo via vuoto");
            }
            else
            {
                textEditViaAnagrafica.BackColor = Color.PaleGreen;
            }
            if (string.IsNullOrEmpty(textEditCittaAnagrafica.Text.Trim()))
            {
                textEditCittaAnagrafica.BackColor = Color.Tomato;
                throw new Exception("Campo civico vuoto");
            }
            else
            {
                textEditCittaAnagrafica.BackColor = Color.PaleGreen;
            }
            if (string.IsNullOrEmpty(textEditCapAnagrafica.Text.Trim()))
            {
                textEditCapAnagrafica.BackColor = Color.Tomato;
                throw new Exception("Campo CAP vuoto");
            }
            else
            {
                textEditCapAnagrafica.BackColor = Color.PaleGreen;
            }
            if (string.IsNullOrEmpty(textEditNazioneAnagrafica.Text.Trim()))
            {
                textEditNazioneAnagrafica.BackColor = Color.Tomato;
                throw new Exception("Campo Nazione vuoto");
            }
            else if (textEditNazioneAnagrafica.Text.Length > 3)
            {
                textEditNazioneAnagrafica.BackColor = Color.Tomato;
                throw new Exception("Campo Nazione non conforme, consentito massimo 3 caratteri");
            }
            else
            {
                textEditNazioneAnagrafica.BackColor = Color.PaleGreen;
            }
            if (string.IsNullOrEmpty(textEditProvAnagrafica.Text.Trim()))
            {
                textEditProvAnagrafica.BackColor = Color.Tomato;
                throw new Exception("Campo Provincia vuoto");
            }
            else if (textEditProvAnagrafica.Text.Length != 2)
            {
                textEditProvAnagrafica.BackColor = Color.Tomato;
                throw new Exception("Campo Provincia non conforme, consentito massimo 2 caratteri");
            }
            else
            {
                textEditProvAnagrafica.BackColor = Color.PaleGreen;
            }
            //if (string.IsNullOrEmpty(textEditPivaAnagrafica.Text.Trim()))
            //{
            //    textEditPivaAnagrafica.BackColor = Color.Tomato;
            //    throw new Exception("Campo P.Iva vuoto");
            //}
        }
        private void InserisciAnagrafica()
        {
            gridViewAnagrafiche.BeginUpdate();
            try
            {
                long idLast = 0;

                if (AnagraficheGenerali != null && AnagraficheGenerali.Count > 0)
                {
                    idLast = AnagraficheGenerali.Last().ID_ANAGRAFICA;
                }

                var na = new Anagrafiche()
                {
                    ID_ANAGRAFICA = idLast + 1,
                    CAP = textEditCapAnagrafica.Text.Trim(),
                    Citta = textEditCittaAnagrafica.Text.Trim(),
                    Nazione = textEditNazioneAnagrafica.Text.Trim(),
                    PIva = textEditPivaAnagrafica.Text.Trim(),
                    Provincia = textEditProvAnagrafica.Text.Trim(),
                    RagioneSociale = textEditRagSocAnagrafica.Text.Trim(),
                    Regione = textEditRegioneAnagrafica.Text.Trim(),
                    Via = textEditViaAnagrafica.Text.Trim()
                };

                if (!string.IsNullOrEmpty(na.PIva) && AnagraficheGenerali.Any(x => x.PIva == na.PIva))
                {
                    var resp = MessageBox.Show(this, "Partita iva già presente in anagrafica, sicuro di voler inserire un possibile dato duplicato?", "Attenzione",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resp == DialogResult.Yes)
                    {
                        AnagraficheGenerali.Add(na);
                        salvaAnagrafica();
                    }
                }
                else
                {
                    AnagraficheGenerali.Add(na);
                    salvaAnagrafica();
                }
            }
            finally
            {
                gridViewAnagrafiche.EndUpdate();
            }
        }
        private void salvaAnagrafica()
        {
            if (!File.Exists(localDBAna))
            {
                var tt = File.Create(localDBAna);
                tt.Flush();
                tt.Close();
            }

            var anaLoc = JsonConvert.SerializeObject(AnagraficheGenerali, Formatting.Indented);

            File.WriteAllText(localDBAna, anaLoc);
        }
        private void simpleButtonEliminaSelezionato_Click(object sender, EventArgs e)
        {
            var daEliminare = gridViewAnagrafiche.GetFocusedRow() as Anagrafiche;
            if (daEliminare == null)
            {
                MessageBox.Show(this, "Nessuna riga selezionata", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var resp = MessageBox.Show(this, "Sicuro di voler eliminare la riga selezionata in griglia?", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp == DialogResult.Yes)
            {
                try
                {
                    gridViewAnagrafiche.BeginUpdate();

                    var dE = AnagraficheGenerali.First(x => x.ID_ANAGRAFICA == daEliminare.ID_ANAGRAFICA);
                    AnagraficheGenerali.Remove(dE);
                    salvaAnagrafica();
                    PopolaLookUpEditAnagrafica();

                }
                finally
                {
                    gridViewAnagrafiche.EndUpdate();
                }

            }
        }
        private void gridViewStoricoOrdini_DoubleClick(object sender, EventArgs e)
        {
            var doc = gridViewStoricoOrdini.GetFocusedRow() as Document;
            if (doc != null)
            {
                RootobjectDocumentRows xcmDoc = new RootobjectDocumentRows();
                try
                {
                    splashScreenManager1.ShowWaitForm();
                    splashScreenManager1.SetWaitFormCaption("Recupero informazioni in corso...");
                    var client = new RestClient(endpointAPI_XCM + $"/api/wms/document/row/list/{doc.id}");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Authorization", $"Bearer {token_XCM}");
                    var response = client.Execute(request);
                    var docRows = JsonConvert.DeserializeObject<RootobjectDocumentRows>(response.Content);
                    xcmDoc.rows = docRows.rows;

                }
                finally
                {
                    splashScreenManager1.CloseWaitForm();
                    RappresentaDocumentoXCM(doc, xcmDoc.rows);
                }
            }
        }
        private void RappresentaDocumentoXCM(Document doc, DocumentRow[] rows)
        {
            var UIDocument = new FormRappresentaDocumento(doc, rows);
            UIDocument.ShowDialog();
        }
        volatile bool codeChange = false;
        private void simpleButtonAggiornaStoricoOrdini_Click(object sender, EventArgs e)
        {
            SettaDateERecuperaDocumenti(false);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool bHandled = false;
            if (xtraTabControl1.SelectedTabPage == xtraTabPageAssistenza)
            {
                // switch case is the easy way, a hash or map would be better, 
                // but more work to get set up.
                switch (keyData)
                {
                    case Keys.F12:
                        // do whatever
                        bHandled = true;
                        var resp = MessageBox.Show(this, "Azzerare i parametri di licenza?\r\nsarà necessario riavviare il software", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (resp == DialogResult.Yes)
                        {
                            Properties.Settings.Default.UIDPC = "";
                            Properties.Settings.Default.DEXP = "";
                            Properties.Settings.Default.Save();
                            Environment.Exit(0);
                        }
                        break;
                }
            }
            return bHandled;
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Process.Start("Manuale.docx");
        }
        private void gridViewStoricoOrdini_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }
            if (e.Column == colregTypeID)
            {
                if (e.Value.ToString() == "OUT")
                {
                    e.DisplayText = "USCITA";
                }
                else if (e.Value.ToString() == "IN")
                {
                    e.DisplayText = "INGRESSO";
                }

            }
            else if (e.Column == colsenderDes)
            {
                if (e.Value.ToString() == "")
                {
                    e.DisplayText = "Deposito XCM";
                }
            }
            else if (e.Column == colunLoadDes)
            {
                if (e.Value.ToString() == "")
                {
                    e.DisplayText = "Deposito XCM";
                }
            }
        }
        private void gridViewDDTBEM_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Value == null) return;
            if (e.Column == colregTypeID1)
            {
                if (e.Value.ToString() == "OUT")
                {
                    e.DisplayText = "DDT";
                }
                else if (e.Value.ToString() == "IN")
                {
                    e.DisplayText = "BEM";
                }
            }
            else if (e.Column == colsenderDes1)
            {
                if (string.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.DisplayText = "XCM Healthcare";
                }
            }
        }
        private void lookUpEdit1_Properties_EditValueChanged(object sender, EventArgs e)
        {
            //var idx = lookUpEditDestinazione.ItemIndex;
            //var ana = lookUpEditDestinazione.GetSelectedDataRow() as AnagraficheLookUp;

            //if (ana != null)
            //{
            //    var corr = AnagraficheGenerali.First(x => x.ID_ANAGRAFICA == ana.ID);

            //    textEditRagSocialeDestinazione.Text = corr.RagioneSociale;
            //    textEditIndirizzoDestinazione.Text = corr.Via;
            //    textEditCAPDestinazione.Text = corr.CAP;
            //    textEditNazioneDestinazione.Text = corr.Nazione;
            //    textEditCittaDestinazione.Text = corr.Citta;
            //    textEditProvDestinazione.Text = corr.Provincia;
            //    textEditRegioneDestinazione.Text = corr.Provincia;                
            //}
            //else
            //{
            //    MessageBox.Show("Errore anagrafica\r\ncontattare assistenza", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }
        private void lookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {

            //var ana = lookUpEditDestinatario.GetSelectedDataRow() as AnagraficheLookUp;

            //if (ana != null)
            //{
            //    var corr = AnagraficheGenerali.First(x => x.ID_ANAGRAFICA == ana.ID);

            //    textEditRagSocDestinatario.Text = corr.RagioneSociale;
            //    textEditIndirizzoDestinatario.Text = corr.Via;
            //    textEditCapDestinatario.Text = corr.CAP;
            //    textEditNazioneDestinatario.Text = corr.Nazione;
            //    textEditCittaDestinatario.Text = corr.Citta;
            //    textEditProvDestinatario.Text = corr.Provincia;
            //    textEditRegioneDestinatario.Text = corr.Provincia;
            //}
            //else
            //{
            //    MessageBox.Show("Errore anagrafica\r\ncontattare assistenza", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }
        private void gridLookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {
            var ana = gridLookUpEditDestinazione.GetSelectedDataRow() as AnagraficheLookUp;

            if (ana != null)
            {
                var corr = AnagraficheGenerali.First(x => x.ID_ANAGRAFICA == ana.ID);

                textEditRagSocialeDestinazione.Text = corr.RagioneSociale;
                textEditIndirizzoDestinazione.Text = corr.Via;
                textEditCAPDestinazione.Text = corr.CAP;
                textEditNazioneDestinazione.Text = corr.Nazione;
                textEditCittaDestinazione.Text = corr.Citta;
                textEditProvDestinazione.Text = corr.Provincia;
                textEditRegioneDestinazione.Text = corr.Provincia;
            }
            else
            {
                MessageBox.Show("Errore anagrafica\r\ncontattare assistenza", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void simpleButtonAnagraficaVeloceDestinatario_Click(object sender, EventArgs e)
        {
            try
            {
                textEditCapAnagrafica.Text = textEditCapDestinatario.Text;
                textEditCittaAnagrafica.Text = textEditCittaDestinatario.Text;
                textEditNazioneAnagrafica.Text = textEditNazioneDestinatario.Text;
                textEditPivaAnagrafica.Text = textEditPivaDestinatario.Text;
                textEditProvAnagrafica.Text = textEditProvDestinatario.Text;
                textEditRagSocAnagrafica.Text = textEditRagSocDestinatario.Text;
                textEditRegioneAnagrafica.Text = textEditRegioneDestinatario.Text;
                textEditViaAnagrafica.Text = textEditIndirizzoDestinatario.Text;
                ControllaCampiAnagrafica();
                InserisciAnagrafica();
                PopolaLookUpEditAnagrafica();

                MessageBox.Show(this, "Anagrafica aggiunta correttamente", "Nuova Anagrafica", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, ee.Message, "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            SettaDateERecuperaDocumenti(false);
        }
        private void dateEditOrdiniDal_EditValueChanged(object sender, EventArgs e)
        {
            if (!codeChange)
            {
                codeChange = true;
                dateEditDDTdal.DateTime = dateEditOrdiniDal.DateTime;
                codeChange = false;
            }
        }
        private void dateEditOrdiniAl_EditValueChanged(object sender, EventArgs e)
        {
            if (!codeChange)
            {
                codeChange = true;
                dateEditDDTal.DateTime = dateEditOrdiniAl.DateTime;
                codeChange = false;
            }


        }
        private void dateEditDDTdal_EditValueChanged(object sender, EventArgs e)
        {
            if (!codeChange)
            {
                codeChange = true;
                dateEditOrdiniDal.DateTime = dateEditDDTdal.DateTime;
                codeChange = false;
            }
        }
        private void dateEditDDTal_EditValueChanged(object sender, EventArgs e)
        {
            if (!codeChange)
            {
                codeChange = true;
                dateEditOrdiniAl.DateTime = dateEditOrdiniDal.DateTime;
                codeChange = false;
            }
        }

        private void anagraficheLookUpBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void simpleButtonAnnullaOrdine_Click(object sender, EventArgs e)
        {
            ResettaTuttiIParametri();
            RecuperaGiacenzeMagazzino();
            SettaDateERecuperaDocumenti(true);
        }

        private void simpleButtonEsportaGiacenze_Click(object sender, EventArgs e)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var savepath = Path.Combine(desktop, "ExportXCM");
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }

            string finalDest = "";

            finalDest = Path.Combine(savepath, $"Export_giacenze_magazzino_{DateTime.Now.ToString("dd_MM_yyyy")}.xlsx");
            if (File.Exists(finalDest)) File.Delete(finalDest);
            gridViewGiacenzeMagazzino.ExportToXlsx(finalDest);

            Process.Start(savepath);
        }

        private void simpleButtonAnagraficaVeloceDestinazione_Click(object sender, EventArgs e)
        {
            try
            {
                textEditCapAnagrafica.Text = textEditCAPDestinazione.Text;
                textEditCittaAnagrafica.Text = textEditCittaDestinazione.Text;
                textEditNazioneAnagrafica.Text = textEditNazioneDestinazione.Text;
                textEditProvAnagrafica.Text = textEditProvDestinazione.Text;
                textEditRagSocAnagrafica.Text = textEditRagSocialeDestinazione.Text;
                textEditViaAnagrafica.Text = textEditIndirizzoDestinazione.Text;
                textEditRegioneAnagrafica.Text = textEditRegioneDestinazione.Text;
                textEditPivaAnagrafica.Text = "";
                ControllaCampiAnagrafica();
                InserisciAnagrafica();
                PopolaLookUpEditAnagrafica();
                MessageBox.Show(this, "Anagrafica aggiunta correttamente", "Nuova Anagrafica", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, ee.Message, "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridLookUpEditDestinatario_EditValueChanged(object sender, EventArgs e)
        {
            var ana = gridLookUpEditDestinatario.GetSelectedDataRow() as AnagraficheLookUp;

            if (ana != null)
            {
                var corr = AnagraficheGenerali.First(x => x.ID_ANAGRAFICA == ana.ID);

                textEditRagSocDestinatario.Text = corr.RagioneSociale;
                textEditIndirizzoDestinatario.Text = corr.Via;
                textEditCapDestinatario.Text = corr.CAP;
                textEditNazioneDestinatario.Text = corr.Nazione;
                textEditCittaDestinatario.Text = corr.Citta;
                textEditProvDestinatario.Text = corr.Provincia;
                textEditRegioneDestinatario.Text = corr.Provincia;
                textEditPivaDestinatario.Text = corr.PIva;
            }
            else
            {
                MessageBox.Show("Errore anagrafica\r\ncontattare assistenza", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        string TokenXCM = "";
        public string GetAccessToken()
        {
            if (this.TokenXCM != null)
            {
                return this.TokenXCM;

            }
            else
            {
                return LoginXCM_API();
            }
        }

        private string LoginXCM_API()
        {
            try
            {
                var client = new RestClient("http://185.30.181.192:8092/token");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);

                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("username", "g.fusco@unitexpress.it");
                request.AddParameter("password", "!Unitex.IT@2022");
                request.AddParameter("grant_type", "password");

                IRestResponse response = client.Execute(request);
                var resp = JsonConvert.DeserializeObject<LoginResponse>(response.Content);
                if (resp != null)
                {
                    this.TokenXCM = resp.access_token;
                    return TokenXCM;

                }
                else
                {
                    return "";
                }


            }
            catch (Exception ee)
            {
                //TODO: Logger
                return "";
            }
        }
    }
    public class AnagraficheLookUp
    {
        public long ID { get; set; }
        public string RagioneSociale { get; set; }
        public string PIva { get; set; }
        public string Indirizzo { get; set; }
        public string Citta { get; set; }
    }
}
