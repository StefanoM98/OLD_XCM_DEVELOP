using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class RootobjectShip
{
    public Shipment[] shipments { get; set; }
}


public class Shipment
{
    [JsonProperty]
    public int Id { get; set; }
    [JsonProperty]
    public string DocNumber { get; set; }
    [JsonProperty]
    public int Colli { get; set; }
    [JsonProperty]
    public decimal Peso { get; set; }
    [JsonProperty]
    public decimal Volume { get; set; }
    [JsonProperty]
    public int Pallet { get; set; }
    [JsonProperty]
    public string IdCorriere { get; set; }
    [JsonProperty]
    public DateTime DataDocumento { get; set; }
    [JsonProperty]
    public string UnloadDes { get; set; }
    [JsonProperty]
    public string UnloadAddress { get; set; }
    [JsonProperty]
    public string UnloadLocation { get; set; }
    [JsonProperty]
    public string UnloadZipCode { get; set; }
    [JsonProperty]
    public string UnloadCountry { get; set; }
    [JsonProperty]
    public string UnloadRegion { get; set; }
    [JsonProperty]
    public string UnloadDistrict { get; set; }
    [JsonProperty]
    public string TipoTrasporto { get; set; }
    [JsonProperty]
    public string RifCliente { get; set; }
    [JsonProperty]
    public string Note { get; set; }
    [JsonProperty]
    public string NumDTT { get; set; }
    [JsonProperty]
    public DateTime DataDTT { get; set; }
    [JsonProperty]
    public string LoadName { get; set; }

    public string SegmentName { get; set; }
    public List<string> ParcelBarcode = new List<string>();

    public string Carrier { get; set; }
    
}


public class RootobjectTrip
{
    public Trip[] trips { get; set; }
}
public class Trip
{
    public long id { get; set; }
    public string docNumber { get; set; }
    public DateTime docDate { get; set; }
    public string transportType { get; set; }   
    public string carrierID { get; set; }
    public string carrierDes { get; set; }

}
public class RootTripXCM
{
    [JsonProperty("tripXCM")]
    public TripXCM[] tripXCM { get; set; }
}
    public class TripXCM
{
    [JsonProperty("id")]
    public long id { get; set; }
    [JsonProperty("docNumber")]
    public string docNumber { get; set; }
    [JsonProperty("docDate")]
    public string docDate { get; set; }
    [JsonProperty("transportType")]
    public string transportType { get; set; }
    [JsonProperty("carrierID")]
    public string carrierID { get; set; }
    [JsonProperty("carrierDes")]
    public string carrierDes { get; set; }

}
