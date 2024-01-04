using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using NLog;

namespace UnitexFSC.Code.APIs
{
    public class EspritecAPI_UNITEX
    {
        #region Logger
        internal static Logger _loggerCode = LogManager.GetLogger("loggerCode");
        internal static Logger _loggerAPI = LogManager.GetLogger("LogAPI");
        #endregion

        #region Login
        private static readonly string Endpoint = "https://010761.espritec.cloud:9500";
        //private static readonly string userAPIAmministrativa = "glsApi";
        //private static readonly string passwordAPIAmministrativa = "#kS#@^0";
        //private static readonly string userAPIAmministrativa = "dvalitutti";
        //private static readonly string passwordAPIAmministrativa = "Dv$2022!";

        private static string Username { get; set; }
        private static string Password { get; set; }

        private static DateTime DataScadenzaToken = DateTime.Now;
        private static string Token = "";

        public static void Init(string username, string password, string tenant)
        {
            try
            {
                IRestResponse response = RestSharpHelper.Login(Endpoint, "/api/token", username, password, tenant);
                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonResponse = JsonConvert.DeserializeObject<Token>(response.Content);
                        if (jsonResponse != null && jsonResponse.result != null)
                        {
                            if (jsonResponse.result.status)
                            {
                                if (jsonResponse.user != null)
                                {
                                    Username = username;
                                    Password = password;
                                    DataScadenzaToken = jsonResponse.user.expire;
                                    Token = jsonResponse.user.token;
                                }
                                else
                                {
                                    _loggerCode.Error($"Errore in EspritecAPI_UNITEX.RecuperaConnessione\n {jsonResponse.result}");
                                }
                            }
                            else
                            {
                                _loggerCode.Error($"Errore in EspritecAPI_UNITEX.RecuperaConnessione\n {jsonResponse.result}");

                            }
                        }
                        else
                        {
                            _loggerCode.Error($"Errore in EspritecAPI_UNITEX.RecuperaConnessione\n {jsonResponse}");

                        }
                    }
                    else
                    {
                        _loggerCode.Error($"Errore in EspritecAPI_UNITEX.RecuperaConnessione\n {response.StatusCode}\n {response.Content}");

                    }
                }
                else
                {
                    _loggerCode.Error($"Errore in EspritecAPI_UNITEX.RecuperaConnessione\n Il response del login è null");
                }

            }
            catch (Exception InitException)
            {
                _loggerAPI.Error(InitException);
            }
        }

        private static bool RecuperaConnessione(string username, string password)
        {
            var result = false;
            try
            {
                if (!string.IsNullOrEmpty(DataScadenzaToken.ToString()) && (DateTime.Now + TimeSpan.FromHours(1)) > DataScadenzaToken)
                {
                    IRestResponse response = RestSharpHelper.Login(Endpoint, "/api/token", Username, Password, "UNITEX");
                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var jsonResponse = JsonConvert.DeserializeObject<Token>(response.Content);
                            if (jsonResponse != null && jsonResponse.result != null)
                            {
                                if (jsonResponse.result.status)
                                {
                                    if (jsonResponse.user != null)
                                    {
                                        DataScadenzaToken = jsonResponse.user.expire;
                                        Token = jsonResponse.user.token;
                                        result = true;
                                    }
                                    else
                                    {
                                        _loggerCode.Error($"Errore in EspritecAPI_UNITEX.RecuperaConnessione\n {jsonResponse.result}");
                                    }
                                }
                                else
                                {
                                    _loggerCode.Error($"Errore in EspritecAPI_UNITEX.RecuperaConnessione\n {jsonResponse.result}");
                                }
                            }
                            else
                            {
                                _loggerCode.Error($"Errore in EspritecAPI_UNITEX.RecuperaConnessione\n {jsonResponse}");
                            }
                        }
                        else
                        {
                            _loggerCode.Error($"Errore in EspritecAPI_UNITEX.RecuperaConnessione\n {response.StatusCode}\n {response.Content}");
                        }
                    }
                    else
                    {
                        _loggerCode.Error($"Errore in EspritecAPI_UNITEX.RecuperaConnessione\n Il response del login è null");
                    }
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee, "EspritecAPI_UNITEX.RecuperaConnessione");
            }
            return result;
        }
        #endregion

        public static List<Shipment> TmsShipmentList(string startDate, string endDate)
        {
            var result = new List<Shipment>();
            try
            {
                var toDate = !string.IsNullOrEmpty(endDate) ? endDate : DateTime.Now.ToString("MM-dd-yyyy");
                RecuperaConnessione(null, null);
                var pageNumber = 1;
                var pageRows = 50;
                var resource = $"/api/tms/shipment/list/{pageRows}/{pageNumber}?StartDate={startDate}&EndDate={toDate}";
                var client = new RestClient(Endpoint);
                var request = new RestRequest(resource, Method.GET);

                client.Timeout = -1;
                request.AddHeader("Authorization", $"Bearer {Token}");
                request.AlwaysMultipartFormData = true;
                IRestResponse response = client.Execute(request);

                var resp = JsonConvert.DeserializeObject<TmsShipmentList>(response.Content);

                if (resp != null && resp.shipments != null)
                {
                    result = resp.shipments.ToList();
                    var maxPages = resp.result.maxPages;

                    while (maxPages > 1)
                    {
                        pageNumber++;
                        maxPages--;
                        resource = $"/api/tms/shipment/list/{pageRows}/{pageNumber}?StartDate={startDate}&EndDate={toDate}";
                        request = new RestRequest(resource, Method.GET);
                        request.AddHeader("Authorization", $"Bearer {Token}");
                        request.AlwaysMultipartFormData = true;
                        response = client.Execute(request);
                        resp = JsonConvert.DeserializeObject<TmsShipmentList>(response.Content);

                        if (resp != null && resp.shipments != null)
                        {
                            result.AddRange(resp.shipments.ToList());
                        }

                    }
                }
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee, "EspritecAPI_UNITEX.GetShipments");
            }
            return result;
        }

        public static List<TmsTripListTrip> TmsTripList()
        {
            var result = new List<TmsTripListTrip>();

            try
            {
                RecuperaConnessione(null, null);
                var pageNumber = 1;
                var pageRows = 50;
                var resource = $"/api/tms/trip/list/{pageRows}/{pageNumber}?StartDate=10-01-2022";
                var client = new RestClient(Endpoint);
                var request = new RestRequest(resource, Method.GET);

                client.Timeout = -1;
                request.AddHeader("Authorization", $"Bearer {Token}");
                request.AlwaysMultipartFormData = true;
                IRestResponse response = client.Execute(request);

                var resp = JsonConvert.DeserializeObject<TmsTripList>(response.Content);

                if (resp != null && resp.trips != null)
                {
                    result = resp.trips.ToList();
                    var maxPages = resp.result.maxPages;

                    while (maxPages > 1)
                    {
                        pageNumber++;
                        maxPages--;
                        resource = $"/api/tms/trip/list/{pageRows}/{pageNumber}?StartDate=10-01-2022";
                        request = new RestRequest(resource, Method.GET);
                        request.AddHeader("Authorization", $"Bearer {Token}");
                        request.AlwaysMultipartFormData = true;
                        response = client.Execute(request);
                        resp = JsonConvert.DeserializeObject<TmsTripList>(response.Content);

                        if (resp != null && resp.trips != null)
                        {
                            result.AddRange(resp.trips.ToList());
                        }

                    }
                }
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee, "EspritecAPI_UNITEX.GetTrips");
            }
            return result;
        }

        public static List<Stop> TmsTripStopList(int tripId)
        {
            List<Stop> result = new List<Stop>();

            RecuperaConnessione(null, null);

            var client = new RestClient(Endpoint);
            var request = new RestRequest($"/api/tms/trip/stop/list/{tripId}", Method.GET);

            client.Timeout = -1;
            request.AddHeader("Authorization", $"Bearer {Token}");
            request.AlwaysMultipartFormData = true;
            IRestResponse response = client.Execute(request);

            var resp = JsonConvert.DeserializeObject<TmsTripStopList>(response.Content);

            if (resp != null && resp.stops != null)
            {
                result = resp.stops.ToList();
            }
            return result;
        }

        public static List<Parcel> TmsShipmentParcelList(int shipId)
        {
            List<Parcel> result = new List<Parcel>();

            RecuperaConnessione(null, null);

            var client = new RestClient(Endpoint);
            var request = new RestRequest($"/api/tms/shipment/parcel/list/{shipId}", Method.GET);

            client.Timeout = -1;
            request.AddHeader("Authorization", $"Bearer {Token}");
            request.AlwaysMultipartFormData = true;
            IRestResponse response = client.Execute(request);

            var resp = JsonConvert.DeserializeObject<TmsShipmentParcelList>(response.Content);

            if (resp != null && resp.parcel != null)
            {
                result = resp.parcel.ToList();
            }
            return result;
        }

        public static TmsShipmentGetShipment TmsShipmentGet(int shipId)
        {
            TmsShipmentGetShipment result = new TmsShipmentGetShipment();

            RecuperaConnessione(null, null);

            var client = new RestClient(Endpoint);
            var request = new RestRequest($"/api/tms/shipment/get/{shipId}", Method.GET);

            client.Timeout = -1;
            request.AddHeader("Authorization", $"Bearer {Token}");
            request.AlwaysMultipartFormData = true;
            IRestResponse response = client.Execute(request);

            var resp = JsonConvert.DeserializeObject<TmsShipmentGet>(response.Content);

            result = resp.shipment;

            return result;
        }

        public static List<TmsShipmentTrackingListEvent> TmsShipmentTrackingList(int shipId)
        {
            List<TmsShipmentTrackingListEvent> result = new List<TmsShipmentTrackingListEvent>();

            RecuperaConnessione(null, null);

            var client = new RestClient(Endpoint);
            var request = new RestRequest($"/api/tms/shipment/tracking/list/{shipId}", Method.GET);

            client.Timeout = -1;
            request.AddHeader("Authorization", $"Bearer {Token}");
            request.AlwaysMultipartFormData = true;
            IRestResponse response = client.Execute(request);

            var resp = JsonConvert.DeserializeObject<TmsShipmentTrackingList>(response.Content);

            if (resp != null && resp.events != null)
            {
                result = resp.events.ToList();
            }
            return result;
        }

        public static IRestResponse TmsShipmentTrackingNew(TmsShipmentTrackingNew bodyModel)
        {
            IRestResponse response = null;
            var client = new RestClient(Endpoint + "/api/tms/shipment/tracking/new");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", $"Bearer {Token}");
            request.AddHeader("Content-Type", "application/json");
            var body = JsonConvert.SerializeObject(bodyModel);

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            response = client.Execute(request);

            return response;
        }

        public static List<TmsShipmentTrackingChangesEvent> TmsShipmentTrackingChanges(string fromDate)
        {
            List<TmsShipmentTrackingChangesEvent> result = new List<TmsShipmentTrackingChangesEvent>();

            try
            {
                var toDate = !string.IsNullOrEmpty(fromDate) ? fromDate : DateTime.Now.ToString("MM-dd-yyyy");
                var pageNumber = 1;
                var pageRows = 50;
                var resource = $"/api/tms/shipment/tracking/changes/{pageRows}/{pageNumber}?FromTimeStamp={fromDate}";
                var client = new RestClient(Endpoint);
                var request = new RestRequest(resource, Method.GET);

                request.AddHeader("Authorization", $"Bearer {Token}");
                request.AlwaysMultipartFormData = true;
                IRestResponse response = client.Execute(request);

                var resp = JsonConvert.DeserializeObject<TmsShipmentTrackingChanges>(response.Content);

                if (resp != null && resp.events != null)
                {
                    result = resp.events.ToList();
                    var maxPages = resp.result.maxPages;

                    while (maxPages > 1)
                    {
                        pageNumber++;
                        maxPages--;
                        resource = $"/api/tms/shipment/tracking/changes/{pageRows}/{pageNumber}?FromTimeStamp={fromDate}";
                        request = new RestRequest(resource, Method.GET);
                        request.AddHeader("Authorization", $"Bearer {Token}");
                        request.AlwaysMultipartFormData = true;
                        response = client.Execute(request);
                        resp = JsonConvert.DeserializeObject<TmsShipmentTrackingChanges>(response.Content);

                        if (resp != null && resp.events != null)
                        {
                            result.AddRange(resp.events.ToList());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerAPI.Error(ex);
            }
            return result;
        }
    }


}