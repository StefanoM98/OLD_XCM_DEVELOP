using CommonAPITypes.ESPRITEC;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class RestSharpUtils
    {
        public static IRestResponse RestPostCall(string endpoint, string token, object body)
        {
            var client = new RestClient();
            client.Timeout = -1;
            var request = new RestRequest(endpoint, Method.POST);
            if (!string.IsNullOrEmpty(token))
            {
                request.AddHeader("Authorization", $"Bearer {token}");
            }
            request.AddHeader("Cache-Control", "no-cache");
            request.AddJsonBody(body);
            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return client.Execute(request);

        }

    }
    public class loginXcmCredentials
    {
        public string username { get; set; }
        public string password { get; set; }
        public string tenant { get; set; }
    }
}
