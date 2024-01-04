using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrasferimentiVivisol
{
    public partial class FormDDT : Form
    {
        //public static string endpointAPI_Espritec = "https://api.xcmhealthcare.com:9500";
        //public static string userAPIEspritecVivisol = "apisol";
        //public static string passwordAPIEspritecVivisol = "Mt03r9h_";
        public static string endpointAPI_Espritec = "https://api.xcmhealthcare.com:9500";
        public static string userAPIEspritecVivisol = "administrator";
        public static string passwordAPIEspritecVivisol = "admin";

        public static string endpointAPI_XCM = /*"https://localhost:44327";*/"http://185.30.181.192:8092";
        public static string userAPIXCMVivisol = "info@vivisol.it";
        public static string passwordAPIXCMVivisol = "!Vivisol@2022";

        #region Token
        DateTime DataScadenzaToken_XCM = DateTime.Now;
        string token_XCM = "";
        DateTime DataScadenzaToken_Espritec = DateTime.Now;
        string token_Espritec = "";
        #endregion

        public FormDDT()
        {
            InitializeComponent();
            RecuperaConnessione();
            dateEdit1.DateTime = DateTime.Now - TimeSpan.FromDays(4);
            //RecuperaDDT();
            RecuperaDDTByAPI();//DA XCM
        }

        private void RecuperaDDTByAPI()
        {
            var t = endpointAPI_XCM + $"/api/sol/getdocuments?datada={dateEdit1.DateTime.ToString("dd/MM/yyyy")}";
            var client = new RestClient(t);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token_XCM}");
            IRestResponse response = client.Execute(request);
            var docOsservati = JsonConvert.DeserializeObject<DocumentList[]>(response.Content);

            documentListBindingSource.DataSource = docOsservati.OrderBy(x => x.docNumber).ToList();
        }

        private void RecuperaConnessione()
        {
            if (DataScadenzaToken_Espritec < DateTime.Now + TimeSpan.FromHours(1))
            {
                try
                {
                    var client = new RestClient(endpointAPI_Espritec + "/api/token");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json-patch+json");
                    request.AddHeader("Cache-Control", "no-cache");
                    var body = @"{" + "\n" +
                    $@"  ""username"": ""{userAPIEspritecVivisol}""," + "\n" +
                    $@"  ""password"": ""{passwordAPIEspritecVivisol}""," + "\n" +
                    @"  ""tenant"": """"" + "\n" +
                    @"}";
                    request.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                    client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                    IRestResponse response = client.Execute(request);
                    var resp = JsonConvert.DeserializeObject<RootobjectLoginXCM>(response.Content);

                    DataScadenzaToken_Espritec = resp.user.expire;
                    token_Espritec = resp.user.token;
                }
                catch (Exception ee)
                {

                }
            }
            if (DataScadenzaToken_XCM < DateTime.Now + TimeSpan.FromHours(1))
            {
                try
                {
                    var client = new RestClient(endpointAPI_XCM + "/token");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);

                    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                    request.AddParameter("username", $"{userAPIXCMVivisol}");
                    request.AddParameter("password", $"{passwordAPIXCMVivisol}");
                    request.AddParameter("grant_type", "password");

                    IRestResponse response = client.Execute(request);
                    var resp = JsonConvert.DeserializeObject<LoginResponse>(response.Content);
                    if (resp != null)
                    {
                        this.token_XCM = resp.access_token;
                        DataScadenzaToken_XCM = DateTime.Now + TimeSpan.FromSeconds(resp.expires_in);
                    }
                }
                catch (Exception ee)
                {

                }
            }
        }
        private void RecuperaDDT()
        {
            var db = new GnXcmEntities1();
            var ddtVivisol = db.uvwWmsDocument.Where(x => x.DocDta >= dateEdit1.DateTime && x.DocTip == 204).OrderByDescending(x => x.DocNum2).ToList();
            uvwWmsDocumentBindingSource.DataSource = ddtVivisol;
        }
        private void simpleButtonAggiornaGriglia_Click(object sender, EventArgs e)
        {
            RecuperaDDT();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var r = gridView1.GetFocusedRow() as uvwWmsDocument;

            if (r != null)
            {
                var resp = MessageBox.Show(this, $"Sicuri di voler creare la BEM dal DDT {r.DocNum2}?", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resp != DialogResult.Yes) return;

                CreaBEMDaDDT(r);

            }
        }

        private void CreaBEMDaDDT(uvwWmsDocument r)
        {

            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show(this, "Cliente non selezionato", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ANAGRAFICA_MANDANTI mandanteSelezionato = comboBox1.SelectedItem as ANAGRAFICA_MANDANTI;

            if(mandanteSelezionato == null)
            {
                MessageBox.Show(this, "Errore lettura mandante", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            

            var db = new GnXcmEntities1();
            var rr = db.uvwWmsDocumentRows_XCM.Where(x => x.DocNum == r.DocNum).ToList();

            if (rr.Count() > 0)
            {

                #region Header
                var paziente = new SenderBEM()
                {
                    address = r.UnloadAddress,
                    country = r.UnloadCountry,
                    description = r.UnloadName,
                    district = r.UnloadDistrict,
                    location = r.UnloadLocation,
                    region = r.UnloadRegion,
                    zipCode = r.UnloadZipCode
                };
                var ritornaA = new UnloadBEM()
                {
                    locationID = 6
                };
                var header = new HeaderBEM()
                {
                    anaID = mandanteSelezionato.ID_MANDANTE_GESPE,
                    anaType = 1,
                    customerID = mandanteSelezionato.ID_MANDANTE_GESPE,
                    docType = "DeliveryIN",
                    logWareID = r.LogWareID,
                    ownerID = mandanteSelezionato.ID_MANDANTE_GESPE,
                    reference = "rientro " + r.Reference,
                    siteID = "01",
                    referenceDate = (r.RefDta != null) ? r.RefDta.Value.ToString("o") : "",
                    regTypeID = "IN",
                    reference2 = r.Reference2,
                    reference2Date = (r.RefDta2 != null) ? r.RefDta2.Value.ToString("o") : "",
                    //sender = paziente,
                    //unload = ritornaA,
                    procID = 2,
                    publicNote = $"creato ingresso da DDT {r.DocNum2}",

                };
                #endregion

                #region Rows
                List<RowBEM> righeBem = new List<RowBEM>();

                foreach (var rb in rr)
                {
                    var rws = new RowBEM()
                    {
                        batchNo = rb.Batchno,
                        discount = rb.Discount,
                        expireDate = rb.DateExpire.Value.ToString("o"),
                        logWareId = rb.LogWareID,
                        note = rb.ItemNote,
                        packageID = rb.PackageID,
                        partNumber = rb.PrdCod,
                        procID = 2,
                        qty = rb.Qty,
                        um = rb.Prdum,
                    };
                    var esiste = righeBem.FirstOrDefault(x => x.partNumber == rws.partNumber && x.batchNo == rws.batchNo);
                    if (esiste != null)
                    {
                        esiste.qty += rws.qty;
                    }
                    else
                    {
                        righeBem.Add(rws);
                    }

                }

                #endregion

                var nBEM = new RootobjectBEM()
                {
                    header = header,
                    rows = righeBem.ToArray()
                };
                InviaBEMToAPI(nBEM);
            }
            else
            {
                MessageBox.Show(this, "Non sono state trovate righe per il documento selezionato\r\nimpossibile continuare", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void InviaBEMToAPI(RootobjectBEM nBEM)
        {
            var ch = JsonConvert.SerializeObject(nBEM, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var client = new RestClient(endpointAPI_Espritec + "/api/wms/document/new");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json-patch+json");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {token_Espritec}");
            request.AddJsonBody(ch);

            request.AddParameter("application/json-patch+json", ParameterType.RequestBody);
            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageBox.Show(this, "Bolla ingresso creata correttamente", "Creazione BEM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "Non è stato possibile creare la BEM nel sistema", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormDDT_Load(object sender, EventArgs e)
        {
            var db = new InterscambioAPIEntities();
            comboBox1.DataSource = db.ANAGRAFICA_MANDANTI.ToList();
            comboBox1.Text = "";
            comboBox1.SelectedIndex = -1;
        }
    }
}

