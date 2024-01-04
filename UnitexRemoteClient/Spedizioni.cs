using CommonAPITypes.ESPRITEC;
using Newtonsoft.Json;
using RestSharp;
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

namespace UnitexRemoteClient
{
    public partial class Spedizioni : Form
    {
        List<EspritecShipment.ShipmentList> DatiGriglia = new List<EspritecShipment.ShipmentList>();
        List<EspritecShipment.RootobjectShipmentList> DatiGrezzi = new List<EspritecShipment.RootobjectShipmentList>();
       
        public Spedizioni()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            dateEditSpedizioniDa.DateTime = DateTime.Now - TimeSpan.FromDays(15);
            dateEditSpedizioniA.DateTime = DateTime.Now + TimeSpan.FromDays(1);           
        }

        private void CaricaSpedizioniInCarico()
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                try
                {
                    splashScreenManager1.ShowWaitForm();
                    gridView1.BeginDataUpdate();
                    DatiGriglia.Clear();
                    DatiGrezzi.Clear();
                    RecuperaSpedizioni();
                    foreach (var dg in DatiGrezzi)
                    {
                        DatiGriglia.AddRange(dg.shipments);
                    }
                    shipmentListBindingSource.DataSource = DatiGriglia;
                }
                finally
                {
                    gridView1.EndDataUpdate();
                    splashScreenManager1.CloseWaitForm();
                }
            });
        }

       
       
        private void RecuperaSpedizioni()
        {
            if (!ConnectionManager.RecuperaConnessione())
            {
                MessageBox.Show(this, "Errore di comunicazione\r\nimpossibile proseguire", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string startDate = dateEditSpedizioniDa.DateTime.ToString("MM-dd-yyyy");
            string endDate = dateEditSpedizioniA.DateTime.ToString("MM-dd-yyyy");
            var result = new List<EspritecShipment.RootobjectShipmentList>();
            var pageNumber = 1;
            var pageRows = 100;
            var resource = $"/api/tms/shipment/list/{pageRows}/{pageNumber}?StartDate={startDate}&EndDate={endDate}";
            var client = new RestClient("https://010761.espritec.cloud:9500");
            var request = new RestRequest(resource, Method.GET);

            client.Timeout = -1;
            request.AddHeader("Authorization", $"Bearer {ConnectionManager.tokenAPI}");
            request.AlwaysMultipartFormData = true;
            IRestResponse response = client.Execute(request);

            var resp = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(response.Content);

            if (resp != null && resp.shipments != null)
            {
                var maxPages = resp.result.maxPages;
                DatiGrezzi.Add(resp);

                while (maxPages > 1)
                {
                    pageNumber++;
                    maxPages--;
                    resource = $"/api/tms/shipment/list/{pageRows}/{pageNumber}?StartDate={startDate}";
                    request = new RestRequest(resource, Method.GET);
                    request.AddHeader("Authorization", $"Bearer {ConnectionManager.tokenAPI}");
                    request.AlwaysMultipartFormData = true;
                    response = client.Execute(request);
                    resp = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(response.Content);

                    if (resp != null && resp.shipments != null)
                    {
                        DatiGrezzi.Add(resp);
                    }

#if DEBUG
                    break;
#endif
                }
            }

        }

        private void Home_Shown(object sender, EventArgs e)
        {           
                Task.Factory.StartNew(() =>
                {
                    CaricaSpedizioniInCarico();
                });            
        }


        private void simpleButtonAggiornaDati_Click(object sender, EventArgs e)
        {
            CaricaSpedizioniInCarico();
        }
    }
}
