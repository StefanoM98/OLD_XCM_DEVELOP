using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CommonAPITypes.VIVISOL
{
    public class VivisolTypes
    {
        #region Entrata Merce
        public class RootobjectVivisolEntrataMerce
        {
            public string partNumber { get; set; }
            public string logWareId { get; set; }
            public string batch { get; set; }
            public string expiryDate { get; set; }
            public string movementDate { get; set; }
            public double quantity { get; set; } // modificato, comunicare a bianchini da decimal a double
            public string orderDetailId { get; set; } //ExternalID riga ricevuto dall'ordine fornitore
            public string note { get; set; } //
            public string orderId { get; set; } // ExternalID testata ricevuto dall'ordine fornitore
            public string system { get { return "Sintesi_Italia"; } }
        }
        public class RootobjectVivisolEntrataMerceResponse
        {
            public string recordId { get; set; }
            public bool success { get; set; }
            public string errorMessage { get; set; }
        }
        public static IRestResponse RestSegnalaEntrataMerce(RootobjectVivisolEntrataMerce raw)
        {
            var clientVivisol = new RestClient("https://api-production.solgroup.com/xcm-experience/api/entrataMerce");
            clientVivisol.Timeout = -1;
            var requestVivisol = new RestRequest(Method.POST);
            var body = JsonConvert.SerializeObject(raw, Formatting.Indented);

            requestVivisol.AddHeader("Authorization", "Basic MlNzSHJtZ2NhVFVtSldxQUc2M0s6Vml2aXNvbFBvcnRhbHMhMjA=");
            requestVivisol.AddParameter("application/json", body, ParameterType.RequestBody);

            clientVivisol.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return clientVivisol.Execute(requestVivisol);
        }
        #endregion

        #region Preparazione Carico
        public class RootobjectInvioPreparazioneCarico
        {
            public string idRouting { get; set; } //dock (routeID) = passa in preparazione su SOL il viaggio
            public string system { get; set; }//{ get { return "Sintesi_Italia"; } }

        }
        public class RootobjectInvioPreparazioneCaricoResponse
        {
            public string idRouting { get; }
            public bool success { get; }
            public string errorMessage { get; }
        }
        public static IRestResponse RestPreparazioneInCorso(RootobjectInvioPreparazioneCarico raw)
        {
            var client = new RestClient("https://api-production.solgroup.com/xcm-experience/api/invioPreparazioneCarico");
            client.Timeout = -1;
            var request = new RestRequest(Method.PATCH);
            request.AddHeader("Authorization", "Basic MlNzSHJtZ2NhVFVtSldxQUc2M0s6Vml2aXNvbFBvcnRhbHMhMjA=");
            request.AddHeader("Content-Type", "application/json");
            var body = JsonConvert.SerializeObject(raw, Formatting.Indented);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return client.Execute(request);
        }
        #endregion

        #region Invio Movimenti
        public class RootobjectVivisolInvioMovimenti
        {
            public string idRouting { get; set; }
            public string pallet { get; set; }
            public string batch { get; set; }
            public string movementDate { get; set; }
            public double quantity { get; set; }
            public string logWareId { get; set; }
            public string note { get; set; }
            public string idUtente { get; set; }
            public string idPianifica { get; set; }
            public double numColli { get; set; }//lato sol, note pianifica
            public string partNumber { get; set; }
            public string system { get; set; }
        }
        public class RootobjectVivisolInvioMovimentiResponse
        {
            public string idRouting { get; set; }
            public bool success { get; set; }
            public string errorMessage { get; set; }
        }
        public static IRestResponse RestSegnalaInvioMovimenti(RootobjectVivisolInvioMovimenti raw)
        {
            var clientVivisol = new RestClient("https://api-production.solgroup.com/xcm-experience/api/invioMovimenti");
            clientVivisol.Timeout = -1;
            var requestVivisol = new RestRequest(Method.POST);
            var body = JsonConvert.SerializeObject(raw, Formatting.Indented);
            requestVivisol.AddHeader("Authorization", "Basic MlNzSHJtZ2NhVFVtSldxQUc2M0s6Vml2aXNvbFBvcnRhbHMhMjA=");
            requestVivisol.AddParameter("application/json", body, ParameterType.RequestBody);
            clientVivisol.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return clientVivisol.Execute(requestVivisol);
        }
        #endregion
    }
}
