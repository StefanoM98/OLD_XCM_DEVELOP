
using System;

public class ARootobjectShipChgGespe
{
    public AResultShipChgGespe result { get; set; }
    public AEventShipChgGespe[] events { get; set; }
}

public class AResultShipChgGespe
{
    public object[] messages { get; set; }
    public string info { get; set; }
    public int maxPages { get; set; }
    public bool status { get; set; }
}

public class AEventShipChgGespe
{
    public int id { get; set; }
    public int shipID { get; set; }
    public int stopID { get; set; }
    public int tripID { get; set; }
    public string agencycode { get; set; }
    public int statusID { get; set; }
    public string statusType { get; set; }
    public string statusDes { get; set; }
    public DateTime timeStamp { get; set; }
    public string info { get; set; }
    public double longitude { get; set; }
    public double latitude { get; set; }
    public string locationInfo { get; set; }
    public string signature { get; set; }
}
