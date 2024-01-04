using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using API_XCM.Models.XCM.CRM;

namespace API_XCM.Code.CRM
{
    public class EspritecAPI_XCM
    {
        #region Login
        private static string endpointAPI_XCM = "https://api.xcmhealthcare.com:9500";
        private static string userAPIAmministrativa = "Administrator";
        private static string passwordAPIAmministrativa = "admin";
        private static DateTime DataScadenzaToken_XCM = DateTime.Now;

        private static string token_XCM = "";

        //private Dictionary<string, string> clienti = new Dictionary<string, string>();
        private static void XcmLogin(string username, string password)
        {
            try
            {
                var client = new RestClient(endpointAPI_XCM + "/api/token");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json-patch+json");
                request.AddHeader("Cache-Control", "no-cache");

                var body = "";
                if (username == null && password == null)
                {
                    body = @"{" + "\n" +
                            $@"  ""username"": ""{userAPIAmministrativa}""," + "\n" +
                            $@"  ""password"": ""{passwordAPIAmministrativa}""," + "\n" +
                            @"  ""tenant"": """"" + "\n" +
                            @"}";

                }
                else
                {
                    body = @"{" + "\n" +
                            $@"  ""username"": ""{username}""," + "\n" +
                            $@"  ""password"": ""{password}""," + "\n" +
                            @"  ""tenant"": """"" + "\n" +
                            @"}";
                }

                request.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse response = client.Execute(request);
                var resp = JsonConvert.DeserializeObject<Token>(response.Content);

                //TODO: aggiungere controllo sul response
                DataScadenzaToken_XCM = resp.user.expire;
                token_XCM = resp.user.token;

            }
            catch (Exception ee)
            {

            }
        }
        private static void RecuperaConnessione(string username, string password)
        {

            if ((DateTime.Now + TimeSpan.FromHours(1)) > DataScadenzaToken_XCM)
            {
                XcmLogin(username, password);

            }
        }
        #endregion

        public static List<CustomerEspritecAPI> CustomerList = null;

        public static List<CustomerEspritecAPI> CommonCustomerList()
        {
            if(CustomerList != null)
            {
                return CustomerList;
            }
            RecuperaConnessione(null, null);
            var pageNumber = 1;
            var pageRows = 50;

            var client = new RestClient(endpointAPI_XCM);
            var request = new RestRequest($"/api/common/customer/list?{pageNumber}&{pageRows}", Method.GET);


            client.Timeout = -1;
            request.AddHeader("Authorization", $"Bearer {token_XCM}");
            request.AlwaysMultipartFormData = true;
            IRestResponse response = client.Execute(request);

            var resp = JsonConvert.DeserializeObject<CommonCustomerList>(response.Content);

            if (resp != null && resp.customers != null)
            {
                var maxPages = resp.result.maxPages;
                CustomerList = resp.customers.ToList();
                while (maxPages > 1)
                {
                    pageNumber++;
                    maxPages--;
                    request = new RestRequest($"/api/common/customer/list?{pageNumber}&{pageRows}", Method.GET);
                    request.AddHeader("Authorization", $"Bearer {token_XCM}");
                    request.AlwaysMultipartFormData = true;
                    response = client.Execute(request);
                    resp = JsonConvert.DeserializeObject<CommonCustomerList>(response.Content);

                    if (resp.customers != null)
                    {
                        CustomerList.AddRange(resp.customers.ToList());
                    }

                }
            }
            return CustomerList;
        }
    }
}