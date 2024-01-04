using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CommonAPITypes.ESPRITEC
{
    public class EspritecEDIMessage
    {
        public class RootobjectMessageEDI
        {
            public ResultMessageEDI result { get; set; }
            public MessageMessageEDI[] messages { get; set; }
        }
        public class ResultMessageEDI
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }
        public class MessageMessageEDI
        {
            public int id { get; set; }
            public int msgTypeID { get; set; }
            public string msgTypeName { get; set; }
            public string name { get; set; }
            public int statusId { get; set; }
            public string statusDes { get; set; }
            public string receivedTimeStamp { get; set; }
            public object processedTimeStamp { get; set; }
            public object abortedTimeStamp { get; set; }
        }
    }
    public class EspritecLogin
    {
        public static string EspritecLoginEndPointXCM = "https://192.168.2.254:9500" + "/api/token";
        public static string EspritecLoginEndPointUNITEX = "https://010761.espritec.cloud:9500" + "/api/token";
        public class RootobjectResponseLogin
        {
            public ResulResponseLogin result { get; set; }
            public UserResponseLogin user { get; set; }
        }
        public class ResulResponseLogin
        {
            public object[] messages { get; set; }
            public bool status { get; set; }
            public object info { get; set; }
            public int maxPages { get; set; }
        }
        public class UserResponseLogin
        {
            public int id { get; set; }
            public string name { get; set; }
            public string lang { get; set; }
            public int type { get; set; }
            public string filter { get; set; }
            public DateTime expire { get; set; }
            public string token { get; set; }
            public object settings { get; set; }
            public string agency { get; set; }
        }

        public class UserLogin
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Tenant { get; set; }
        }

        public static IRestResponse RestEspritecLogin(object body, bool isXCM)
        {
            RestClient client = null;
            if (isXCM)
            {
                client = new RestClient(EspritecLoginEndPointXCM);
            }
            else
            {
                client = new RestClient(EspritecLoginEndPointUNITEX);
            }
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddJsonBody(body);
            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return client.Execute(request);
        }

    }
    public class EspritecDocuments
    {
        public class RootobjectTrackingDocument
        {
            public ResultTrackingDocument result { get; set; }
            public TrackingDocument[] trackings { get; set; }
        }
        public class ResultTrackingDocument
        {
            public object[] messages { get; set; }
            public bool status { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
        }
        public class TrackingDocument
        {
            public int id { get; set; }
            public int docID { get; set; }
            public string docNumber { get; set; }
            public string doctype { get; set; }
            public int statusID { get; set; }
            public string statusDes { get; set; }
            public DateTime timeStamp { get; set; }
            public object info { get; set; }
        }

        public class RootobjectOrder
        {
            public ResultOrder result { get; set; }
            public HeaderOrder header { get; set; }
            public LinkOrder[] links { get; set; }
        }
        public class ResultOrder
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }
        public class HeaderOrder
        {
            public int id { get; set; }
            public string docNumber { get; set; }
            public string docType { get; set; }
            public DateTime docDate { get; set; }
            public string siteID { get; set; }
            public int shipID { get; set; }
            public string shipDocNumber { get; set; }
            public int tripID { get; set; }
            public string tripDocNumber { get; set; }
            public int statusId { get; set; }
            public string statusDes { get; set; }
            public string reference { get; set; }
            public DateTime referenceDate { get; set; }
            public string reference2 { get; set; }
            public string reference2Date { get; set; }
            public string externalID { get; set; }
            public string regTypeID { get; set; }
            public string customerID { get; set; }
            public string customerDes { get; set; }
            public string ownerID { get; set; }
            public string ownerDes { get; set; }
            public int senderID { get; set; }
            public string senderDes { get; set; }
            public string senderAddress { get; set; }
            public string senderZipCode { get; set; }
            public string senderLocation { get; set; }
            public string senderDistrict { get; set; }
            public string senderRegion { get; set; }
            public string senderCountry { get; set; }
            public int consigneeID { get; set; }
            public string consigneeDes { get; set; }
            public string consigneeAddress { get; set; }
            public string consigneeZipCode { get; set; }
            public string consigneeLocation { get; set; }
            public string consigneeDistrict { get; set; }
            public string consigneeRegion { get; set; }
            public string consigneeCountry { get; set; }
            public int unloadID { get; set; }
            public string unLoadDes { get; set; }
            public string unloadAddress { get; set; }
            public string unloadZipCode { get; set; }
            public string unloadLocation { get; set; }
            public string unloadDistrict { get; set; }
            public string unloadRegion { get; set; }
            public string unloadCountry { get; set; }
            public int rowsNo { get; set; }
            public decimal totalQty { get; set; }
            public decimal totalPacks { get; set; }
            public decimal totalBoxes { get; set; }
            public decimal totalNetWeight { get; set; }
            public decimal totalGrossWeight { get; set; }
            public decimal totalCube { get; set; }
            public decimal coverage { get; set; }
            public decimal planned { get; set; }
            public decimal executed { get; set; }
            public string internalNote { get; set; }
            public string publicNode { get; set; }
            public string deliveryNote { get; set; }
            public string info1 { get; set; }
            public string info2 { get; set; }
            public string info3 { get; set; }
            public string info4 { get; set; }
            public string info5 { get; set; }
            public string info6 { get; set; }
            public string info7 { get; set; }
            public string info8 { get; set; }
            public string info9 { get; set; }
        }
        public class LinkOrder
        {
            public int id { get; set; }
            public string docNumber { get; set; }
            public string docType { get; set; }
            public DateTime docDate { get; set; }
            public string siteID { get; set; }
        }

        public class RootobjectEspritecUpdateDocument
        {
            public EspritecHeaderUpdateDocument header { get; set; }
        }
        public class EspritecHeaderUpdateDocument
        {
            public int id { get; set; }
            public int anaType { get; set; }
            public string anaID { get; set; }
            public string reference { get; set; }
            public string referenceDate { get; set; }
            public string reference2 { get; set; }
            public string reference2Date { get; set; }
            public string externalID { get; set; }
            public string dock { get; set; }
            public string logWareID { get; set; }
            public int procID { get; set; }
            public string shipdate { get; set; }
            public string internalNote { get; set; }
            public string publicNote { get; set; }
            public string deliveryNote { get; set; }
            public string info1 { get; set; }
            public string info2 { get; set; }
            public string info3 { get; set; }
            public string info4 { get; set; }
            public string info5 { get; set; }
            public string info6 { get; set; }
            public string info7 { get; set; }
            public string info8 { get; set; }
            public string info9 { get; set; }
            public EspritecSenderUpdateDocument sender { get; set; }
            public EspritecConsigneeUpdateDocument consignee { get; set; }
            public EspritecUnloadUpdateDocument unload { get; set; }
        }
        public class EspritecSenderUpdateDocument
        {
            public int locationID { get; set; }
            public string description { get; set; }
            public string address { get; set; }
            public string zipCode { get; set; }
            public string location { get; set; }
            public string district { get; set; }
            public string region { get; set; }
            public string country { get; set; }
        }
        public class EspritecConsigneeUpdateDocument
        {
            public int locationID { get; set; }
            public string description { get; set; }
            public string address { get; set; }
            public string zipCode { get; set; }
            public string location { get; set; }
            public string district { get; set; }
            public string region { get; set; }
            public string country { get; set; }
        }
        public class EspritecUnloadUpdateDocument
        {
            public int locationID { get; set; }
            public string description { get; set; }
            public string address { get; set; }
            public string zipCode { get; set; }
            public string location { get; set; }
            public string district { get; set; }
            public string region { get; set; }
            public string country { get; set; }
        }
        public class RootobjectEspritecRows
        {
            public ResultEspritecRows result { get; set; }
            public RowEspritecRows[] rows { get; set; }
        }
        public class ResultEspritecRows
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }
        public class RowEspritecRows
        {
            public int id { get; set; }
            public string externalID { get; set; }
            public string partNumber { get; set; }
            public string partNumberDes { get; set; }
            public string logWareID { get; set; }
            public string batchNo { get; set; }
            public DateTime? expireDate { get; set; }
            public string um { get; set; }
            public string um2 { get; set; }
            public decimal um2Ratio { get; set; }
            public double qty { get; set; }
            public decimal qtyUm2 { get; set; }
            public decimal boxes { get; set; }
            public decimal packs { get; set; }
            public decimal qtyCovered { get; set; }
            public decimal qtyPlanned { get; set; }
            public decimal qtyExecuted { get; set; }
            public decimal qtyReceived { get; set; }
            public decimal qtyChecked { get; set; }
            public decimal unitCostPrice { get; set; }
            public decimal costPrice { get; set; }
            public decimal unitSellPrice { get; set; }
            public decimal sellPrice { get; set; }
            public decimal discount { get; set; }
            public decimal netSellPrice { get; set; }
            public string barcode { get; set; }
            public string barcodeExt { get; set; }
            public string barcodeMaster { get; set; }
            public string linkedID { get; set; }
            public string linkedExternalID { get; set; }
            public string info1 { get; set; }
            public string info2 { get; set; }
            public string info3 { get; set; }
            public string info4 { get; set; }
            public string info5 { get; set; }
            public string info6 { get; set; }
            public string info7 { get; set; }
            public string info8 { get; set; }
            public string info9 { get; set; }

            public override string ToString()
            {
                return $"{partNumber} - {batchNo} - {qty} - {logWareID}";
            }
        }
        public class RootobjectEspritecRegistrations
        {
            public ResultEspritecRegistrations result { get; set; }
            public EspritecRegistration[] registrations { get; set; }
        }
        public class ResultEspritecRegistrations
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }
        public class EspritecRegistration
        {
            public int id { get; set; }
            public int groupId { get; set; }
            public string siteID { get; set; }
            public string regTypeID { get; set; }
            public string regTypeGroup { get; set; }
            public string dateReg { get; set; }
            public string reference { get; set; }
            public string partNumber { get; set; }
            public string partNumberDescription { get; set; }
            public double totalQty { get; set; }
            public string batchNo { get; set; }
            public DateTime? dateExpire { get; set; }
            public string dateProd { get; set; }
            public double totalNetWeight { get; set; }
            public double totalGrossWeight { get; set; }
            public double totalCube { get; set; }
            public string customerID { get; set; }
            public string customerName { get; set; }
            public string ownerID { get; set; }
            public string ownerName { get; set; }
            public string anaID { get; set; }
            public string anaName { get; set; }
            public string note { get; set; }
        }

        #region RestSharp
        static string EspritecDocumentChangesEndPoint = "https://192.168.2.254:9500" + $"/api/wms/document/tracking/changes/500/1?FromTimeStamp=";
        public static IRestResponse RestEspritecDocumentsChanged(string ts, string token)
        {
            var client = new RestClient(EspritecDocumentChangesEndPoint + ts);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Cache-Control", "no-cache");
            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return client.Execute(request);
        }

        static string EspritecDocumentGetEndpoint = "https://192.168.2.254:9500" + $"/api/wms/document/get/";
        public static IRestResponse RestEspritecGetDocument(int docID, string token)
        {
            var client = new RestClient(EspritecDocumentGetEndpoint + docID);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            IRestResponse response = client.Execute(request);
            return response;
        }

        static string EspritecDocumentUpdateEndpoint = "https://192.168.2.254:9500" + $"/api/wms/document/update";
        public static IRestResponse RestEspritecUpdateDocument(RootobjectEspritecUpdateDocument raw, string token)
        {
            var client = new RestClient(EspritecDocumentUpdateEndpoint);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            var jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            jsetting.DefaultValueHandling = DefaultValueHandling.Ignore;
            var body = JsonConvert.SerializeObject(raw, Formatting.Indented, jsetting);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return client.Execute(request);
        }

        static string EspritecDocumentRowsList = "https://192.168.2.254:9500" + $"/api/wms/document/row/list/";
        public static IRestResponse RestEspritecRowsListFromDocumentID(long idOrdine, string token)
        {
            var client = new RestClient(EspritecDocumentRowsList + idOrdine);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }

        static string EspritecRegistrationEndpoint = "https://192.168.2.254:9500" + $"/api/wms/registration/list/500/1/?RegTypeGroup=Move&DocID=";
        public static IRestResponse RestEspritecGetRegistrations(long id, string token)
        {
            var client = new RestClient(EspritecRegistrationEndpoint + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }
        #endregion

    }
    public class EspritecShipment
    {
        #region ShipmentGet
        public class RootobjectEspritecShipment
        {
            public ResultShipment result { get; set; }
            public Shipment shipment { get; set; }
        }
        public class ResultShipment
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }
        public class Shipment
        {
            public int id { get; set; }
            public string ownerAgency { get; set; }
            public string recAgency { get; set; }
            public string startAgency { get; set; }
            public string endAgency { get; set; }
            public string docNumber { get; set; }
            public DateTime? docDate { get; set; }
            public bool isLocked { get; set; }
            public int statusId { get; set; }
            public string statusDes { get; set; }
            public string statusType { get; set; }
            public int webStatusId { get; set; }
            public int webOrderID { get; set; }
            public string webOrderNumber { get; set; }
            public string insideRef { get; set; }
            public string externRef { get; set; }
            public string serviceType { get; set; }
            public string transportType { get; set; }
            public string customerID { get; set; }
            public string customerDes { get; set; }
            public string customerAddress { get; set; }
            public string customerLocation { get; set; }
            public string customerZipCode { get; set; }
            public string customerCountry { get; set; }
            public string pickupSupplierID { get; set; }
            public string pickupSupplierDes { get; set; }
            public string deliverySupplierID { get; set; }
            public string deliverySupplierDes { get; set; }
            public DateTime? pickupDateTime { get; set; }
            public DateTime? deliveryDateTime { get; set; }
            public string senderID { get; set; }
            public string senderDes { get; set; }
            public string senderAddress { get; set; }
            public string senderLocation { get; set; }
            public string senderZipcode { get; set; }
            public string senderCountry { get; set; }
            public string consigneeID { get; set; }
            public string consigneeDes { get; set; }
            public string consigneeAddress { get; set; }
            public string consigneeLocation { get; set; }
            public string consigneeZipcode { get; set; }
            public string consigneeCountry { get; set; }
            public int firstStopID { get; set; }
            public string firstStopDes { get; set; }
            public string firstStopAddress { get; set; }
            public string firstStopLocation { get; set; }
            public string firstStopZipCode { get; set; }
            public string firstStopDistrict { get; set; }
            public string firstStopCountry { get; set; }
            public int lastStopID { get; set; }
            public string lastStopDes { get; set; }
            public string lastStopAddress { get; set; }
            public string lastStopLocation { get; set; }
            public string lastStopZipCode { get; set; }
            public string lastStopDistrict { get; set; }
            public string lastStopCountry { get; set; }
            public int packs { get; set; }
            public decimal floorPallets { get; set; }
            public decimal totalPallets { get; set; }
            public decimal netWeight { get; set; }
            public decimal grossWeight { get; set; }
            public decimal cube { get; set; }
            public decimal meters { get; set; }
            public string temperature { get; set; }
            public string incoterm { get; set; }
            public decimal cashValue { get; set; }
            public string cashCurrency { get; set; }
            public string cashPayment { get; set; }
            public string cashNote { get; set; }
            public string cashStatus { get; set; }
            public string internalNote { get; set; }
            public string publicNote { get; set; }
        }
        #endregion

        #region ShipmentUpdate

        public class RootobjectShipmentUpdate
        {
            public ShipmentUpdate shipment { get; set; }
        }
        public class ShipmentUpdate
        {
            public int id { get; set; }
            public string ownerAgency { get; set; }
            public string recAgency { get; set; }
            public string startAgency { get; set; }
            public string endAgency { get; set; }
            public string insideRef { get; set; }
            public string externRef { get; set; }
            public string docDate { get; set; }
            public string serviceType { get; set; }
            public string transportType { get; set; }
            public string carrierType { get; set; }
            public string incoterm { get; set; }
            public double cashValue { get; set; }
            public string cashCurrency { get; set; }
            public string cashPayment { get; set; }
            public string cashNote { get; set; }
            public string note { get; set; }
            public string printNote { get; set; }
            public string temperature { get; set; }
            public int senderID { get; set; }
            public string senderName { get; set; }
            public int consigneeID { get; set; }
            public string consigneeName { get; set; }
        }
        public class RootobjectShipmentUpdateResponse
        {
            public string[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }

        #endregion

        #region ShipmentList
        public class RootobjectShipmentList
        {
            public ResultShipmentList result { get; set; }
            public ShipmentList[] shipments { get; set; }
        }
        public class ResultShipmentList
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }
        public class ShipmentList
        {
            public int id { get; set; }
            public string ownerAgency { get; set; }
            public string docNumber { get; set; }
            public DateTime docDate { get; set; }
            public bool isLocked { get; set; }
            public int statusId { get; set; }
            public string statusType { get; set; }
            public string statusDes { get; set; }
            public string statusColor { get; set; }
            public int webStatusId { get; set; }
            public string webStatusType { get; set; }
            public string webStatusDes { get; set; }
            public string webStatusColor { get; set; }
            public int webOrderID { get; set; }
            public string webOrderNumber { get; set; }
            public string insideRef { get; set; }
            public string externRef { get; set; }
            public string serviceType { get; set; }
            public string transportType { get; set; }
            public string customerID { get; set; }
            public string customerDes { get; set; }
            public string customerAddress { get; set; }
            public string customerLocation { get; set; }
            public string customerZipCode { get; set; }
            public string customerDistrict { get; set; }
            public string customerCountry { get; set; }
            public string pickupSupplierID { get; set; }
            public string pickupSupplierDes { get; set; }
            public string deliverySupplierID { get; set; }
            public string deliverySupplierDes { get; set; }
            public DateTime? pickupDateTime { get; set; }
            public string deliveryDateTime { get; set; }
            public string senderID { get; set; }
            public string senderDes { get; set; }
            public string senderAddress { get; set; }
            public string senderLocation { get; set; }
            public string senderZipCode { get; set; }
            public string senderDistrict { get; set; }
            public string senderCountry { get; set; }
            public string consigneeID { get; set; }
            public string consigneeDes { get; set; }
            public string consigneeAddress { get; set; }
            public string consigneeLocation { get; set; }
            public string consigneeZipCode { get; set; }
            public string consigneeDistrict { get; set; }
            public string consigneeCountry { get; set; }
            public int firstStopID { get; set; }
            public string firstStopDes { get; set; }
            public string firstStopAddress { get; set; }
            public string firstStopLocation { get; set; }
            public string firstStopZipCode { get; set; }
            public string firstStopDistrict { get; set; }
            public string firstStopCountry { get; set; }
            public int lastStopID { get; set; }
            public string lastStopDes { get; set; }
            public string lastStopAddress { get; set; }
            public string lastStopLocation { get; set; }
            public string lastStopZipCode { get; set; }
            public string lastStopDistrict { get; set; }
            public string lastStopCountry { get; set; }
            public int packs { get; set; }
            public decimal floorPallets { get; set; }
            public decimal totalPallets { get; set; }
            public decimal netWeight { get; set; }
            public decimal grossWeight { get; set; }
            public decimal cube { get; set; }
            public decimal meters { get; set; }
            public decimal cashValue { get; set; }
            public string cashCurrency { get; set; }
            public string cashPayment { get; set; }
            public string cashNote { get; set; }
            public int attachCount { get; set; }
            public int recUserID { get; set; }
            public DateTime? recTimeStamp { get; set; }
        }
        #endregion

        #region TrackingShipment
        public class RootobjectShipmentTracking
        {
            public ResultShipmentTracking result { get; set; }
            public EventShipmentTracking[] events { get; set; }
        }
        public class ResultShipmentTracking
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }
        public class EventShipmentTracking
        {
            public int id { get; set; }
            public int shipID { get; set; }
            public int stopID { get; set; }
            public string stopType { get; set; }
            public string stopDescription { get; set; }
            public string stopAddress { get; set; }
            public string stopZipCode { get; set; }
            public string stopLocation { get; set; }
            public string stopDistrict { get; set; }
            public string stopRegion { get; set; }
            public string stopCountry { get; set; }
            public int tripID { get; set; }
            public string agencycode { get; set; }
            public int statusID { get; set; }
            public string statusType { get; set; }
            public string statusDes { get; set; }
            public string statusColor { get; set; }
            public int webStatusID { get; set; }
            public string webStatusDes { get; set; }
            public string webStatusColor { get; set; }
            public string timeStamp { get; set; }
            public string info { get; set; }
            public double longitude { get; set; }
            public double latitude { get; set; }
            public string locationInfo { get; set; }
            public string signature { get; set; }
        }
        public class RootobjectTrackingNew
        {
            public int shipID { get; set; }
            public int stopID { get; set; }
            public int statusID { get; set; }
            public DateTime timeStamp { get; set; }
            public string info { get; set; }
            public string signature { get; set; }
            public int longitude { get; set; }
            public int latitude { get; set; }
            public string locationInfo { get; set; }
            public string agencyCode { get; set; }
        }


        public class RootobjectTrackingUpdate
        {
            public TrackingUpdate tracking { get; set; }
        }

        public class TrackingUpdate
        {
            public int id { get; set; }
            public string timeStamp { get; set; }
            public string info { get; set; }
            public string signature { get; set; }
        }

        public class RootobjectTrackingUpdateResponse
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }


        #endregion

        #region ShipmentRestriction

        public class RootobjectGetRestrictionResponse
        {
            public ResultShipmentRestriction result { get; set; }
            public ShipmentRestriction[] restrictions { get; set; }
        }

        public class ResultShipmentRestriction
        {
            public string[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }

        public class ShipmentRestriction
        {
            public int id { get; set; }
            public int shipID { get; set; }
            public string restrictionID { get; set; }
            public string description { get; set; }
            public bool isActive { get; set; }
            public string note { get; set; }
        }

        #endregion

        static string EspritecGetShipmentEndpoint = "https://010761.espritec.cloud:9500" + $"/api/tms/shipment/get/";
        static string EspritecGetShipmentUpdate = "https://010761.espritec.cloud:9500" + $"/api/tms/shipment/update/";
        static string EspritecGetShipmentListFromDateEndpoint = "https://010761.espritec.cloud:9500" + $"/api/tms/shipment/list/500/1?StartDate=";
        static string EspritecGetTrackingEndpoint = "https://010761.espritec.cloud:9500" + $"/api/tms/shipment/tracking/list/";
        static string EspritecSetTrackingEndpoint = "https://010761.espritec.cloud:9500" + $"/api/tms/shipment/tracking/new";
        static string EspritecUpdateTrackingEndpoint = "https://010761.espritec.cloud:9500" + $"/api/tms/shipment/tracking/update";
        static string EspritecGetShipmentByExternalRefEndpoint = "https://010761.espritec.cloud:9500" + $"/api/tms/shipment/list/50/1?ExternRef=";
        static string EspritecGetShipmentByExternalDocNum = "https://010761.espritec.cloud:9500" + $"/api/tms/shipment/list/50/1?DocNumber=";
        static string EspritecGetShipmentRestriction = "https://010761.espritec.cloud:9500" + $"/api/tms/shipment/restriction/list/";

        public static IRestResponse RestEspritecShipmentUpdate(RootobjectShipmentUpdate raw, string token)
        {
            var client = new RestClient(EspritecGetShipmentUpdate);
            var request = new RestRequest(Method.POST);
            var jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            //jsetting.DefaultValueHandling = DefaultValueHandling.Ignore;
            var body = JsonConvert.SerializeObject(raw, Formatting.Indented, jsetting);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return client.Execute(request);
        }
        public static IRestResponse RestEspritecGetShipmentListByExternalRef(string extRef, int Row, int PagNum, string token)
        {
            var client = new RestClient(EspritecGetShipmentByExternalRefEndpoint + extRef);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }
        public static IRestResponse RestEspritecGetShipmentListByDocNum(string DocNum, int Row, int PagNum, string token)
        {
            var client = new RestClient(EspritecGetShipmentByExternalDocNum + DocNum);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }
        public static IRestResponse RestEspritecGetShipment(long idShipment, string token)
        {
            var client = new RestClient(EspritecGetShipmentEndpoint + idShipment);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }
        public static IRestResponse RestEspritecGetTracking(long idShipment, string token)
        {
            var client = new RestClient(EspritecGetTrackingEndpoint + idShipment);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }
        public static IRestResponse RestEspritecGetShipmentList(DateTime FromDate, int Row, int PagNum, string token)
        {
            var client = new RestClient("https://010761.espritec.cloud:9500" + $"/api/tms/shipment/list/{Row}/{PagNum}?StartDate={FromDate.ToString("o")}");// + $"{}/{}?{}={FromDate.ToString("o")}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }
        public static IRestResponse RestEspritecGetShipmentListTraDate(DateTime FromDate, DateTime ToDate, int Row, int PagNum, string token)
        {
            var client = new RestClient("https://010761.espritec.cloud:9500" + $"/api/tms/shipment/list/{Row}/{PagNum}?StartDate={FromDate.ToString("yyyy-MM-dd")}&EndDate={ToDate.ToString("yyyy-MM-dd")}");// + $"{}/{}?{}={FromDate.ToString("o")}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }
        public static IRestResponse RestEspritecGetShipmentFromBarcode(string barcode, string token)
        {
            var client = new RestClient("https://010761.espritec.cloud:9500" + $"/api/tms/shipment/get/parcel/{barcode}");// + $"{barcode}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }
        public static IRestResponse RestEspritecSetTracking(RootobjectTrackingNew tracking, string token)
        {
            IRestResponse response = null;
            var client = new RestClient(EspritecSetTrackingEndpoint);
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var body = JsonConvert.SerializeObject(tracking);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            response = client.Execute(request);
            return response;
        }
        public static IRestResponse RestEspritecUpdateTracking(RootobjectTrackingUpdate daAggiornare, string token)
        {
            var client = new RestClient(EspritecUpdateTrackingEndpoint);
            var request = new RestRequest(Method.POST);
            var jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            jsetting.DefaultValueHandling = DefaultValueHandling.Ignore;
            var body = JsonConvert.SerializeObject(daAggiornare, Formatting.Indented, jsetting);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return client.Execute(request);
        }
        public static IRestResponse RestEspritecGetShipmentRestriction(long idShipment, string token)
        {
            var client = new RestClient(EspritecGetShipmentRestriction + idShipment);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }
    }
    public class EspritecGoods
    {
        public class RootobjectGoodsList
        {
            public ResultGoodsList result { get; set; }
            public GoodList[] goods { get; set; }
        }
        public class ResultGoodsList
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }
        public class GoodList
        {
            public int id { get; set; }
            public int shipID { get; set; }
            public int loadStopID { get; set; }
            public int unLoadStopID { get; set; }
            public string type { get; set; }
            public string description { get; set; }
            public string holderID { get; set; }
            public string packsTypeID { get; set; }
            public string packsTypeDes { get; set; }
            public int packs { get; set; }
            public int floorPallet { get; set; }
            public int totalPallet { get; set; }
            public decimal netWeight { get; set; }
            public decimal grossWeight { get; set; }
            public decimal cube { get; set; }
            public decimal meters { get; set; }
            public decimal height { get; set; }
            public decimal width { get; set; }
            public decimal deep { get; set; }
            public decimal seat { get; set; }
            public string containerNo { get; set; }
        }

        #region GoodsNew
        public class RootobjectGoodsNew
        {
            public GoodsNew goods { get; set; }
        }
        public class GoodsNew
        {
            public int loadStopID { get; set; }
            public int unLoadStopID { get; set; }
            public string type { get; set; }
            public string description { get; set; }
            public string holderID { get; set; }
            public string packsTypeID { get; set; }
            public string packsTypeDes { get; set; }
            public int packs { get; set; }
            public int floorPallet { get; set; }
            public int totalPallet { get; set; }
            public decimal netWeight { get; set; }
            public decimal grossWeight { get; set; }
            public decimal cube { get; set; }
            public decimal meters { get; set; }
            public int seat { get; set; }
            public decimal height { get; set; }
            public decimal width { get; set; }
            public decimal deep { get; set; }
            public string containerNo { get; set; }
        }
        public class RootobjectResponseGoodsNew
        {
            public ResultResponseGoodsNew result { get; set; }
            public int id { get; set; }
            public string itemId { get; set; }
        }
        public class ResultResponseGoodsNew
        {
            public string[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }
        public static IRestResponse RestEspritecNewGoods(RootobjectGoodsNew raw, long shipID, string token)
        {
            IRestResponse response = null;
            var client = new RestClient(EspritecInsertGoodsEndpoint + shipID);
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            jsetting.DefaultValueHandling = DefaultValueHandling.Ignore;
            var body = JsonConvert.SerializeObject(raw, jsetting);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            response = client.Execute(request);
            return response;
        }
        #endregion

        public class RootobjectGoodsUpdate
        {
            public GoodsUpdate goods { get; set; }
        }
        public class GoodsUpdate
        {
            public int id { get; set; }
            public int loadStopID { get; set; }
            public int unLoadStopID { get; set; }
            public string type { get; set; }
            public string description { get; set; }
            public string holderID { get; set; }
            public string packsTypeID { get; set; }
            public string packsTypeDes { get; set; }
            public int packs { get; set; }
            public int floorPallet { get; set; }
            public int totalPallet { get; set; }
            public decimal netWeight { get; set; }
            public decimal grossWeight { get; set; }
            public decimal cube { get; set; }
            public decimal meters { get; set; }
            public int seat { get; set; }
            public decimal height { get; set; }
            public decimal width { get; set; }
            public decimal depth { get; set; }
            public string containerNo { get; set; }
        }

        static string EspritecListGoodsEndpoint = "https://010761.espritec.cloud:9500" + $"/api/tms/shipment/goods/list/";
        static string EspritecUpdateGoodsEndpoint = "https://010761.espritec.cloud:9500" + $"/api/tms/shipment/goods/update";
        static string EspritecInsertGoodsEndpoint = "https://010761.espritec.cloud:9500" + $"/api/tms/shipment/goods/new/";
        public static IRestResponse RestEspritecGetGoodListOfShipment(int idShip, string token)
        {
            var client = new RestClient(EspritecListGoodsEndpoint + idShip);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);

        }
        public static IRestResponse RestEspritecUpdateGoods(RootobjectGoodsUpdate raw, string token)
        {
            var client = new RestClient(EspritecUpdateGoodsEndpoint);
            var request = new RestRequest(Method.POST);
            var jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            jsetting.DefaultValueHandling = DefaultValueHandling.Ignore;
            var body = JsonConvert.SerializeObject(raw, Formatting.Indented, jsetting);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return client.Execute(request);
        }
        public static IRestResponse RestEspritecUpdateGoodsWithDefaultValue(RootobjectGoodsUpdate raw, string token)
        {
            var client = new RestClient(EspritecUpdateGoodsEndpoint);
            var request = new RestRequest(Method.POST);
            var jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            //jsetting.DefaultValueHandling = DefaultValueHandling.Ignore;
            var body = JsonConvert.SerializeObject(raw, Formatting.Indented, jsetting);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return client.Execute(request);
        }


        public static GoodList ClonaGood(GoodList OriginalGood)
        {
            return new GoodList()
            {
                containerNo = OriginalGood.containerNo,
                cube = OriginalGood.cube,
                deep = OriginalGood.deep,
                description = OriginalGood.description,
                floorPallet = OriginalGood.floorPallet,
                grossWeight = OriginalGood.grossWeight,
                height = OriginalGood.height,
                holderID = OriginalGood.holderID,
                id = OriginalGood.id,
                loadStopID = OriginalGood.loadStopID,
                meters = OriginalGood.meters,
                netWeight = OriginalGood.netWeight,
                packs = OriginalGood.packs,
                packsTypeDes = OriginalGood.packsTypeDes,
                packsTypeID = OriginalGood.packsTypeID,
                seat = OriginalGood.seat,
                shipID = OriginalGood.shipID,
                totalPallet = OriginalGood.totalPallet,
                type = OriginalGood.type,
                unLoadStopID = OriginalGood.unLoadStopID,
                width = OriginalGood.width
            };
        }

        public class RootobjectGoodsUpdateResponse
        {
            public string[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }
    }
    public class EspritecTrip
    {

        public class RootobjectSingleTrip
        {
            public ResultSingleTrip result { get; set; }
            public SingleTrip trip { get; set; }
        }

        public class ResultSingleTrip
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }

        public class SingleTrip
        {
            public int id { get; set; }
            public string docNumber { get; set; }
            public DateTime? docDate { get; set; }
            public string description { get; set; }
            public bool isLocked { get; set; }
            public int statusId { get; set; }
            public string statusType { get; set; }
            public string statusDes { get; set; }
            public string statusColor { get; set; }
            public int webStatusId { get; set; }
            public string webStatusType { get; set; }
            public string webStatusColor { get; set; }
            public string serviceType { get; set; }
            public string transportType { get; set; }
            public string companyCode { get; set; }
            public string agencyCode { get; set; }
            public int shipcount { get; set; }
            public string vehicleID { get; set; }
            public string towID { get; set; }
            public int startLocationID { get; set; }
            public string startDes { get; set; }
            public string startAddress { get; set; }
            public string startZipCode { get; set; }
            public string startLocation { get; set; }
            public string startDistrict { get; set; }
            public string startCountry { get; set; }
            public DateTime? startDate { get; set; }
            public int endLocationID { get; set; }
            public string endDes { get; set; }
            public string endAddress { get; set; }
            public string endZipCode { get; set; }
            public string endLocation { get; set; }
            public string endDistrict { get; set; }
            public string endCountry { get; set; }
            public DateTime? endDate { get; set; }
            public string supplierID { get; set; }
            public string supplierDes { get; set; }
            public string carrierID { get; set; }
            public string carrierDes { get; set; }
        }

        public static IRestResponse RestEspritecGetTrip(long idTrip, string token)
        {
            var client = new RestClient($"https://010761.espritec.cloud:9500/api/tms/trip/get/{idTrip}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }
        #region Trip
        public class RootobjectTrip
        {
            public ResultTrip result { get; set; }
            public Trip[] trips { get; set; }
        }
        public class ResultTrip
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }
        public class Trip
        {
            public int id { get; set; }
            public string docNumber { get; set; }
            public DateTime? docDate { get; set; }
            public string description { get; set; }
            public bool isLocked { get; set; }
            public int statusId { get; set; }
            public string statusType { get; set; }
            public string statusDes { get; set; }
            public string statusColor { get; set; }
            public int webStatusId { get; set; }
            public string webStatusType { get; set; }
            public string webStatusColor { get; set; }
            public string serviceType { get; set; }
            public string transportType { get; set; }
            public string companyCode { get; set; }
            public string agencyCode { get; set; }
            public int shipcount { get; set; }
            public string vehicleID { get; set; }
            public string towID { get; set; }
            public int startLocationID { get; set; }
            public string startDes { get; set; }
            public string startAddress { get; set; }
            public string startZipCode { get; set; }
            public string startLocation { get; set; }
            public string startDistrict { get; set; }
            public string startCountry { get; set; }
            public DateTime? startDate { get; set; }
            public int endLocationID { get; set; }
            public string endDes { get; set; }
            public string endAddress { get; set; }
            public string endZipCode { get; set; }
            public string endLocation { get; set; }
            public string endDistrict { get; set; }
            public string endCountry { get; set; }
            public DateTime? endDate { get; set; }
            public string supplierID { get; set; }
            public string supplierDes { get; set; }
            public string carrierID { get; set; }
            public string carrierDes { get; set; }
            public class RootobjectTripChanges
            {
                public ResultTripChanges result { get; set; }
                public EventTripChanges[] events { get; set; }
            }
            public class ResultTripChanges
            {
                public object[] messages { get; set; }
                public string info { get; set; }
                public int maxPages { get; set; }
                public bool status { get; set; }
            }
            public class EventTripChanges
            {
                public int id { get; set; }
                public int tripID { get; set; }
                public string agencycode { get; set; }
                public int statusID { get; set; }
                public string statusDes { get; set; }
                public DateTime timeStamp { get; set; }
                public string info { get; set; }
                public double longitude { get; set; }
                public double latitude { get; set; }
                public string locationInfo { get; set; }
                public string signature { get; set; }
            }

            public static List<EventTripChanges> RestEspritecGetTripChanges(string dataDa, string token)
            {
                var resp = new List<EventTripChanges>();
                var pageNumber = 1;
                var pageRows = 50;
                var resource = $"/api/tms/trip/tracking/changes/{pageRows}/{pageNumber}?FromTimeStamp={dataDa}";
                var client = new RestClient("https://010761.espritec.cloud:9500");
                var request = new RestRequest(resource, Method.GET);
                client.Timeout = -1;
                request.AddHeader("Authorization", $"Bearer {token}");
                var tripChangesTotal = client.Execute(request);
                var tripChangesDes = JsonConvert.DeserializeObject<RootobjectTripChanges>(tripChangesTotal.Content);
                if (tripChangesDes.events == null) return resp;
                foreach (var sd in tripChangesDes.events)
                {
                    resp.Add(sd);
                }

#if !DEBUG
                var maxPages = tripChangesDes.result.maxPages;

                while (maxPages > 1)
                {
                    pageNumber++;
                    maxPages--;
                    resource = $"/api/tms/trip/tracking/changes/{pageRows}/{pageNumber}?FromTimeStamp={dataDa}";
                    request = new RestRequest(resource, Method.GET);
                    request.AddHeader("Authorization", $"Bearer {token}");
                    request.AlwaysMultipartFormData = true;
                    var response = client.Execute(request);
                    var nextP = JsonConvert.DeserializeObject<RootobjectTripChanges>(response.Content);

                    foreach (var n in nextP.events)
                    {
                        resp.Add(n);
                    }

                }
#endif
                return resp;

            }

        }
        #endregion

        #region Stop
        public class RootobjectTripStop
        {
            public ResultTripStop result { get; set; }
            public TripStop[] stops { get; set; }
        }
        public class ResultTripStop
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }
        public class TripStop
        {
            public int id { get; set; }
            public int shipID { get; set; }
            public int stopID { get; set; }
            public string type { get; set; }
            public DateTime? date { get; set; }
            public string shipDocNumber { get; set; }
            public string shipInsideRef { get; set; }
            public string shipExternRef { get; set; }
            public int locationID { get; set; }
            public string description { get; set; }
            public string address { get; set; }
            public string zipCode { get; set; }
            public string location { get; set; }
            public string district { get; set; }
            public string region { get; set; }
            public string country { get; set; }
            public float longitude { get; set; }
            public float latitude { get; set; }
            public string contactName { get; set; }
            public string contactPhone { get; set; }
            public string contactPhone1 { get; set; }
            public string contactMail { get; set; }
            public string contactMail1 { get; set; }
            public int packs { get; set; }
            public double floorPallet { get; set; }
            public double totalPallet { get; set; }
            public double netWeight { get; set; }
            public double grossWeight { get; set; }
            public double cube { get; set; }
            public double meters { get; set; }
            public string driver1ID { get; set; }
            public string driver1Des { get; set; }
            public string driver2ID { get; set; }
            public string driver2Des { get; set; }
            public string vehicleID { get; set; }
            public string towID { get; set; }
        }

        static string EspritecTripStopEndpoint = "https://010761.espritec.cloud:9500" + $"/api/tms/trip/stop/list/";
        public static IRestResponse RestEspritecGetTripStop(long idViaggio, string token)
        {
            var client = new RestClient(EspritecTripStopEndpoint + idViaggio);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }
        #endregion


    }
    public class EspritecTripList
    {

        public class RootobjectTripList
        {
            public ResultTripList result { get; set; }
            public TripList[] trips { get; set; }
        }

        public class ResultTripList
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }

        public class TripList
        {
            public int id { get; set; }
            public string docNumber { get; set; }
            public DateTime docDate { get; set; }
            public string description { get; set; }
            public bool isLocked { get; set; }
            public int statusId { get; set; }
            public string statusType { get; set; }
            public string statusDes { get; set; }
            public string statusColor { get; set; }
            public int webStatusId { get; set; }
            public string webStatusType { get; set; }
            public string webStatusColor { get; set; }
            public string serviceType { get; set; }
            public string transportType { get; set; }
            public string companyCode { get; set; }
            public string agencyCode { get; set; }
            public int shipcount { get; set; }
            public string vehicleID { get; set; }
            public string towID { get; set; }
            public int startLocationID { get; set; }
            public string startDes { get; set; }
            public string startAddress { get; set; }
            public string startZipCode { get; set; }
            public string startLocation { get; set; }
            public string startDistrict { get; set; }
            public string startCountry { get; set; }
            public DateTime? startDate { get; set; }
            public int endLocationID { get; set; }
            public string endDes { get; set; }
            public string endAddress { get; set; }
            public string endZipCode { get; set; }
            public string endLocation { get; set; }
            public string endDistrict { get; set; }
            public string endCountry { get; set; }
            public DateTime? endDate { get; set; }
            public string supplierID { get; set; }
            public string supplierDes { get; set; }
            public string carrierID { get; set; }
            public string carrierDes { get; set; }
        }

        public static IRestResponse RestEspritecGetTripByDocNum(string docnum, string token)
        {
            var client = new RestClient($"https://010761.espritec.cloud:9500/api/tms/trip/list/50/1?DocNumber={docnum}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }

    }
    public class EspritecParcel
    {
        public class RootobjectParcel
        {
            public ResultParcel result { get; set; }
            public Parcel[] parcel { get; set; }
        }
        public class ResultParcel
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }
        public class Parcel
        {
            public int id { get; set; }
            public int shipID { get; set; }
            public int stopID { get; set; }
            public int goodsID { get; set; }
            public string siteID { get; set; }
            public string barcode { get; set; }
            public string barcodeExt { get; set; }
            public string barcodeMaster { get; set; }
            public int statusID { get; set; }
            public string statusDes { get; set; }
            public double qty { get; set; }
            public double netWeight { get; set; }
            public double grossWeight { get; set; }
            public double cube { get; set; }
            public double width { get; set; }
            public double height { get; set; }
            public double deep { get; set; }
        }

        public class RootobjectParcelUpdate
        {
            public ParcelUpdate Parcel { get; set; }
        }

        public class ParcelUpdate
        {
            public int id { get; set; }
            public string barcodeExt { get; set; }
            public string barcodeMaster { get; set; }
        }

        static string EspritecGetParcelEndpoint = "https://010761.espritec.cloud:9500" + $"/api/tms/shipment/parcel/list/";
        static string EspritecUpdateParcelEndpoint = "https://010761.espritec.cloud:9500" + $"/api/tms/shipment/parcel/update/";

        public static IRestResponse RestEspritecGetParcel(long idShipment, string token)
        {
            var client = new RestClient(EspritecGetParcelEndpoint + idShipment);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }
        public static IRestResponse RestEspritecUpdateParcel(Parcel raw, string token)
        {
            var client = new RestClient(EspritecUpdateParcelEndpoint);
            var request = new RestRequest(Method.POST);
            var jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            jsetting.DefaultValueHandling = DefaultValueHandling.Ignore;
            var body = JsonConvert.SerializeObject(raw, Formatting.Indented, jsetting);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return client.Execute(request);
        }

    }
    public class EspritecWarehouse
    {
        static string EspritecGetStock = "https://192.168.2.254:9500" + "/api/wms/warehouse/stock/50/1";
        public class RootobjectStockWarehouse
        {
            public ResultStockWarehouse result { get; set; }
            public StockWarehouse[] stock { get; set; }
        }
        public class ResultStockWarehouse
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }
        public class StockWarehouse
        {
            public string siteID { get; set; }
            public string customerID { get; set; }
            public string customerDes { get; set; }
            public string ownerID { get; set; }
            public string ownerDes { get; set; }
            public string logWareID { get; set; }
            public string partNumber { get; set; }
            public string partNumberDes { get; set; }
            public string um { get; set; }
            public string batchno { get; set; }
            public string dateExpire { get; set; }
            public string project { get; set; }
            public string subProject { get; set; }
            public decimal totalQty { get; set; }
            public decimal usableQty { get; set; }
            public decimal inUseQty { get; set; }
            public decimal availableQty { get; set; }
        }

        public static List<StockWarehouse> RestEspritecGetWarehouseStokAllPages(string token)
        {
            var resp = new List<StockWarehouse>();
            var pageNumber = 1;
            var pageRows = 500;
            var resource = $"/api/wms/warehouse/stock/{pageRows}/{pageNumber}";
            var client = new RestClient("https://192.168.2.254:9500");
            var request = new RestRequest(resource, Method.GET);
            client.Timeout = -1;
            request.AddHeader("Authorization", $"Bearer {token}");
            var stockTotal = client.Execute(request);
            var stockDes = JsonConvert.DeserializeObject<RootobjectStockWarehouse>(stockTotal.Content);

            foreach (var sd in stockDes.stock)
            {
                resp.Add(sd);
            }

            var maxPages = stockDes.result.maxPages;

            while (maxPages > 1)
            {
                pageNumber++;
                maxPages--;
                resource = $"/api/wms/warehouse/stock/{pageRows}/{pageNumber}";
                request = new RestRequest(resource, Method.GET);
                request.AddHeader("Authorization", $"Bearer {token}");
                request.AlwaysMultipartFormData = true;
                var response = client.Execute(request);
                var nextP = JsonConvert.DeserializeObject<RootobjectStockWarehouse>(response.Content);

                foreach (var n in nextP.stock)
                {
                    resp.Add(n);
                }

            }
            return resp;

        }
    }
    public class EspritecCommon
    {
        public class RootobjectCustomer
        {
            public ResultCustomerList result { get; set; }
            public CustomerList[] Customers { get; set; }
        }

        public class CustomerList
        {
            public string id { get; set; }
            public string description { get; set; }
            public string companyCode { get; set; }
            public bool isEnable { get; set; }
            public string address { get; set; }
            public string zipCode { get; set; }
            public string location { get; set; }
            public string district { get; set; }
            public string country { get; set; }
            public string defaultPriceListId { get; set; }
            public string vatCode { get; set; }
            public string fiscalCode { get; set; }
            public bool sendMail { get; set; }
            public DateTime timeStampLastUpdate { get; set; }
        }

        public class ResultCustomerList
        {
            public object[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }

        static readonly string EspritecCustomerListEndpoint = "https://010761.espritec.cloud:9500" + "/api/common/customer/list/";

        public static IRestResponse RestCustomerListGet(int pageRows, int pageNumber, string token)
        {
            var client = new RestClient(EspritecCustomerListEndpoint + $"{pageRows}/{pageNumber}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }
    }
    public class EspritecShipmentStops
    {
        static string EspritecGetShipmentStopsEndpoint = "https://010761.espritec.cloud:9500" + $"/api/tms/shipment/stop/list/";
        public class RootobjectShipmentStops
        {
            public ResultEspritecStop result { get; set; }
            public EspritecStop[] stops { get; set; }
        }

        public class ResultEspritecStop
        {
            public string[] messages { get; set; }
            public string info { get; set; }
            public int maxPages { get; set; }
            public bool status { get; set; }
        }

        public class EspritecStop
        {
            public int id { get; set; }
            public int shipID { get; set; }
            public int stopID { get; set; }
            public string type { get; set; }
            public string date { get; set; }
            public int locationID { get; set; }
            public string description { get; set; }
            public string address { get; set; }
            public string zipCode { get; set; }
            public string location { get; set; }
            public string district { get; set; }
            public string region { get; set; }
            public string country { get; set; }
            public double longitude { get; set; }
            public double latitude { get; set; }
            public string contactName { get; set; }
            public string contactPhone { get; set; }
            public string contactPhone1 { get; set; }
            public string contactMail { get; set; }
            public string contactMail1 { get; set; }
            public int packs { get; set; }
            public int floorPallet { get; set; }
            public int totalPallet { get; set; }
            public double netWeight { get; set; }
            public double grossWeight { get; set; }
            public double cube { get; set; }
            public double meters { get; set; }
            public string driver1ID { get; set; }
            public string driver1Des { get; set; }
            public string driver2ID { get; set; }
            public string driver2Des { get; set; }
            public string vehicleID { get; set; }
            public string towID { get; set; }
            public string note { get; set; }
            public string ets { get; set; }
            public string eta { get; set; }
            public string obligatoryType { get; set; }
        }

        public static IRestResponse RestEspritecGetShipStop(long idShipment, string token)
        {
            var client = new RestClient(EspritecGetShipmentStopsEndpoint + idShipment);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            return client.Execute(request);
        }

    }


}