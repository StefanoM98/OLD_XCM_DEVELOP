
using System;

public class RootobjectShip
{
    public ResultShip result { get; set; }
    public Shipment[] shipments { get; set; }
}

public class ResultShip
{
    public object[] messages { get; set; }
    public string info { get; set; }
    public int maxPages { get; set; }
    public bool status { get; set; }
}

public class Shipment
{
    public int id { get; set; }
    public string docNumber { get; set; }
    public DateTime docDate { get; set; }
    public int webStatusId { get; set; }
    public string insideRef { get; set; }
    public string externRef { get; set; }
    public string serviceType { get; set; }
    public string transportType { get; set; }
    public string customerDes { get; set; }
    public string senderDes { get; set; }
    public string senderAddress { get; set; }
    public string senderCountry { get; set; }
    public string senderCity { get; set; }
    public string senderRegion { get; set; }
    public string senderZipCode { get; set; }
    public string unloadDes { get; set; }
    public string unloadAddress { get; set; }
    public string unloadCountry { get; set; }
    public string unloadCity { get; set; }
    public string unloadRegion { get; set; }
    public string unloadZipCode { get; set; }
    public int packs { get; set; }
    public int totalPallets { get; set; }
    public float netWeight { get; set; }
    public float grossWeight { get; set; }
    public float cube { get; set; }

}
