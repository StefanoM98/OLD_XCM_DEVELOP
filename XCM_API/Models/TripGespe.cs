
using System;


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
    public DateTime docDate { get; set; }
    public string description { get; set; }
    public int statusId { get; set; }
    public string statusType { get; set; }
    public string statusDes { get; set; }
    public int webStatusId { get; set; }
    public string serviceType { get; set; }
    public string transportType { get; set; }
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
    public DateTime startDate { get; set; }
    public int endLocationID { get; set; }
    public string endDes { get; set; }
    public string endAddress { get; set; }
    public string endZipCode { get; set; }
    public string endLocation { get; set; }
    public string endDistrict { get; set; }
    public string endCountry { get; set; }
    public DateTime endDate { get; set; }
    public string supplierID { get; set; }
    public string supplierDes { get; set; }
    public string carrierID { get; set; }
    public string carrierDes { get; set; }
}




