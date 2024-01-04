
using System;

public class RootobjectShipmentTracking
{
    public ResultShipmentTracking result { get; set; }
    public EventTracking[] events { get; set; }
}

public class ResultShipmentTracking
{
    public object[] messages { get; set; }
    public string info { get; set; }
    public int maxPages { get; set; }
    public bool status { get; set; }
}

public class EventTracking
{
    public int id { get; set; }
    public int shipID { get; set; }
    public int stopID { get; set; }
    public object stopType { get; set; }
    public object stopDescription { get; set; }
    public object stopAddress { get; set; }
    public object stopZipCode { get; set; }
    public object stopLocation { get; set; }
    public object stopDistrict { get; set; }
    public object stopRegion { get; set; }
    public object stopCountry { get; set; }
    public int tripID { get; set; }
    public string agencycode { get; set; }
    public int statusID { get; set; }
    public string statusType { get; set; }
    public string statusDes { get; set; }
    public object statusColor { get; set; }
    public int webStatusID { get; set; }
    public object webStatusDes { get; set; }
    public object webStatusColor { get; set; }
    public string timeStamp { get; set; }
    public string info { get; set; }
    public double longitude { get; set; }
    public double latitude { get; set; }
    public string locationInfo { get; set; }
    public string signature { get; set; }
    public int recUserId { get; set; }
    public DateTime? creation { get; set; }
}
