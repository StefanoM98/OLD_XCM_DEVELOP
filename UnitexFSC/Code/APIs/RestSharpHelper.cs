using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace UnitexFSC.Code.APIs
{
    public class RestSharpHelper
    {
        #region Logger
        internal static Logger _loggerCode = LogManager.GetLogger("loggerCode");
        internal static Logger _loggerAPI = LogManager.GetLogger("LogAPI");
        #endregion

        public static IRestResponse Login(string endpoint, string resource, string username, string password, string tenant)
        {
            IRestResponse response = null;
            try
            {
                var client = new RestClient(endpoint)
                {
                    Timeout = -1
                };
                var request = new RestRequest(resource, Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };
                request.AddHeader("Content-Type", "application/json-patch+json");
                request.AddHeader("Cache-Control", "no-cache");
                if (!string.IsNullOrEmpty(tenant))
                {
                    request.AddJsonBody(new { username, password, tenant });

                }
                else
                {
                    request.AddJsonBody(new { username, password });

                }


                client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                response = client.Execute(request);

            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee, "RestSharpHelper.Login");
            }
            return response;
        }

        public static IRestResponse Post(string endpoint, string resource, string token, Object body)
        {
            IRestResponse response = null;
            try
            {
                var client = new RestClient(endpoint)
                {
                    Timeout = -1
                };
                var request = new RestRequest(resource, Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };
                request.AddHeader("Authorization", $"Bearer {token}");
                request.AddJsonBody(body);

                response = client.Execute(request);
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee, "RestSharpHelper.Login");
            }
            return response;
        }

        public static IRestResponse Get(string endpoint, string resource, string token, Dictionary<string, object> parameters)
        {
            IRestResponse response = null;
            try
            {
                var client = new RestClient(endpoint)
                {
                    Timeout = -1
                };

                var request = new RestRequest(resource, Method.GET);

                request.AddHeader("Authorization", $"Bearer {token}");
                foreach (var parms in parameters)
                {
                    request.AddParameter(parms.Key, parms.Value);
                }
                response = client.Execute(request);

            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee, "RestSharpHelper.Get");

            }
            return response;

        }


    }
}