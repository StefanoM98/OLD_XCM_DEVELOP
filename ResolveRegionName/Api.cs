using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResolveRegionName
{
    public class Api
    {

        #region Login

        public static string Endpoint = $"http://185.30.181.192:8092";
        private static string Username = "g.fusco@unitexpress.it";
        private static string Password = "!Unitex.IT@2022";
        private static string GrantType = "password";
        private static DateTime DataScadenzaToken = DateTime.Now;

        private static string Token = "";

        public Api(string username, string password)
        {
            Username = username;
            Password = password;
        }

        private static void Login()
        {
            try
            {
                var client = new RestClient(Endpoint + "/token");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);

                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("username", Username);
                request.AddParameter("password", Password);
                request.AddParameter("grant_type", $"{GrantType}");

                IRestResponse response = client.Execute(request);

                var resp = JsonConvert.DeserializeObject<LoginResponse>(response.Content);
                if (resp != null)
                {
                    Token = resp.access_token;
                    DataScadenzaToken = DateTime.Now + TimeSpan.FromSeconds(resp.expires_in);
                }
            }
            catch (Exception ee)
            {

            }
        }
        private static void RecuperaConnessione()
        {

            if ((DateTime.Now + TimeSpan.FromHours(1)) > DataScadenzaToken)
            {
                Login();
            }
        }
        #endregion


        public static string GetRegionName(string provincia)
        {
            string result = "";
            try
            {
                RecuperaConnessione();
                var client = new RestClient(Endpoint + $"/api/geo/GetRegionName?provincia={provincia}");
                client.Timeout = -1;

                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", $"Bearer {Token}");


                IRestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    result = JsonConvert.DeserializeObject<string>(response.Content);
                }
            }
            catch (Exception GetUsersException)
            {
                
            }
            return result;
        }
    }
}
