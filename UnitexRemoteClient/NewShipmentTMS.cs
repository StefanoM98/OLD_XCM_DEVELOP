using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UnitexRemoteClient
{

    public class RootobjectNewShipmentTMS
    {
        public HeaderNewShipmentTMS header { get; set; }
        public StopNewShipmentTMS[] stops { get; set; }
        public GoodNewShipmentTMS[] goods { get; set; }
        public ParcelNewShipmentTMS[] parcels { get; set; }
        public RestrictionNewShipmentTMS[] restrictions { get; set; }
        public ReferenceNewShipmentTMS[] references { get; set; }


    }

    public class HeaderNewShipmentTMS 
    {
        public string type { get; set; }
        public string externRef { get; set; }
        public string insideRef { get; set; }
        public string customerID { get; set; }
        public string docDate { get; set; }
        public string serviceType { get; set; }
        public string transportType { get; set; }
        public string carrierType { get; set; }
        public string incoterm { get; set; }
        public string internalNote { get; set; }
        public string publicNote { get; set; }
        public decimal cashValue { get; set; }
        public string cashCurrency { get; set; }
        public string cashPayment { get; set; }
        public string cashNote { get; set; }
        //public string ownerAgency { get; set; }
        //public string recAgency { get; set; }
        //public string startAgency { get; set; }
        //public string endAgency { get; set; }
    }

    public class StopNewShipmentTMS
    {
        public int? stopID { get; set; }
        public string type { get; set; }
        public int? locationID { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public string zipCode { get; set; }
        public string location { get; set; }
        public string district { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string note { get; set; }
        public string ets { get; set; }
        public string eta { get; set; }
        public string contactName { get; set; }
        public string contactMail { get; set; }
        public string contactMail1 { get; set; }
        public string contactPhone { get; set; }
        public string contactPhone1 { get; set; }
        public string obligatoryType { get; set; }
    }

    public class GoodNewShipmentTMS
    {
        public int? goodsID { get; set; }
        public int? loadStopID { get; set; }
        public int? unLoadStopID { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public string holderID { get; set; }
        public string packsTypeID { get; set; }
        public string packsTypeDes { get; set; }
        public int packs { get; set; }
        public int floorPallet { get; set; }
        public int totalPallet { get; set; }
        public decimal? netWeight { get; set; }
        public decimal? grossWeight { get; set; }
        public decimal? cube { get; set; }
        public decimal? meters { get; set; }
        public int? seat { get; set; }
        public decimal? height { get; set; }
        public decimal? width { get; set; }
        public decimal? depth { get; set; }
        public string containerNo { get; set; }
    }

    public class ParcelNewShipmentTMS
    {
        public int stopID { get; set; }
        public int goodsID { get; set; }
        public string barcodeExt { get; set; }
        public string barcodeMaster { get; set; }
    }

    public class RestrictionNewShipmentTMS
    {
        public string restrictionID { get; set; }
        public string note { get; set; }
    }

    public class ReferenceNewShipmentTMS
    {
        public int goodsID { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public string date { get; set; }
        public string note { get; set; }
    }

    public static class RestAPI
    {
        public static IRestResponse RestEspritecInsertNewShipment(RootobjectNewShipmentTMS ship, string token)
        {
            var client = new RestClient("https://010761.espritec.cloud:9500/api/tms/shipment/new");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var body = JsonConvert.SerializeObject(ship, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            return client.Execute(request);

        }
    }



    public class RootobjectResponseNewShipmentTMS
    {
        public ResultNewShipmentTMS result { get; set; }
        public int id { get; set; }
        public string itemId { get; set; }
    }

    public class ResultNewShipmentTMS
    {
        public string[] messages { get; set; }
        public string info { get; set; }
        public int maxPages { get; set; }
        public bool status { get; set; }
    }


}
