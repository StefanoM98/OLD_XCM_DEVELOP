using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitexFSC.Model;

namespace UnitexFSC
{
    class API
    {
#if DEBUG
        //public string endpointAPI_XCM = $"https://localhost:44327";//$"https://localhost:44327";
        public string endpointAPI_XCM = $"http://185.30.181.192:8092";
#else
        public string endpointAPI_XCM = $"{ Properties.Settings.Default.IndirizzoAPI }:{Properties.Settings.Default.PortaAPI}";
#endif
        private string accessToken { get; set; }
        public string GetAccessToken()
        {
            if (this.accessToken != null)
            {
                return this.accessToken;

            }
            else
            {
                return Login();
            }
        }

        private string Login()
        {
            try
            {
                var client = new RestClient(this.endpointAPI_XCM + "/token");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);

                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("username", "gaetano.colella@xcmhealthcare.com");
                request.AddParameter("password", "Deni1112!");
                request.AddParameter("grant_type", "password");

                IRestResponse response = client.Execute(request);
                var resp = JsonConvert.DeserializeObject<LoginResponse>(response.Content);
                if (resp != null)
                {
                    this.accessToken = resp.access_token;
                    return accessToken;

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

        public InterpreteFSC[] InviaFileSpedizioniFSC(List<InterpreteFSC> list)
        {
            var client = new RestClient(endpointAPI_XCM + $"/api/fsc/postships");
            client.Timeout = -1;

            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {this.GetAccessToken()}");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(list);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resp = JsonConvert.DeserializeObject<InterpreteFSC[]>(response.Content);

                return resp;
            }
            else
            {
                return new InterpreteFSC[0];
            }
        }

        public string GetRegionName(string provincia)
        {
            var client = new RestClient(endpointAPI_XCM + $"/api/geo/getregionname?provincia={provincia}");
            client.Timeout = -1;

            var request = new RestRequest(Method.GET);
            //request.AddHeader("Authorization", $"Bearer {xcm.GetAccessToken()}");
            

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resp = response.Content;

                return resp;
            }
            else
            {
                return "";
            }
        }

        public TripXCM[] GetTrips()
        {
            var dts = (DateTime.Now - TimeSpan.FromDays(7)).ToUniversalTime().ToString("u");
            //var dts = (DateTime.Now + TimeSpan.FromDays(7)).ToUniversalTime().ToString("u");
            //var client = new RestClient(endpointAPI_XCM + $"/api/values/getships?GespeDocNum=01044/TR");
            var client = new RestClient(endpointAPI_XCM + $"/api/xcm/gettrips?dts={dts}");
            client.Timeout = -1;

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {this.GetAccessToken()}");

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var viaggi = JsonConvert.DeserializeObject<TripXCM[]>(response.Content);
                return viaggi;

            }
            else
            {
                return new TripXCM[0];

            }

        }

        public bool GetShips(string docNumber)
        {
            var boolResponse = false;

            var dts = (DateTime.Now - TimeSpan.FromDays(7)).ToUniversalTime().ToString("u");
            var client = new RestClient(endpointAPI_XCM + $"/api/xcm/getships?GespeDocNum={docNumber}");

            client.Timeout = -1;

            var request = new RestRequest(Method.GET);
            request.AddParameter("text/plain", ParameterType.RequestBody);
            request.AddHeader("Authorization", $"Bearer {this.GetAccessToken()}");

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                boolResponse = bool.Parse(response.Content);

            }
            return boolResponse;
        }

        public bool GetGLS(string tripNumber)
        {
            var boolResponse = false;

            var client = new RestClient(endpointAPI_XCM + $"/api/xcm/getgls?tripNumber={tripNumber}");

            client.Timeout = -1;

            var request = new RestRequest(Method.GET);
            request.AddParameter("text/plain", ParameterType.RequestBody);
            request.AddHeader("Authorization", $"Bearer {this.GetAccessToken()}");

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                boolResponse = bool.Parse(response.Content);

            }
            return boolResponse;
        }
    }
}
