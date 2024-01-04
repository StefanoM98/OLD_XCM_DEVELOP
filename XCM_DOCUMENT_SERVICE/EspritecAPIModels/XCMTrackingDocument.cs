using System;

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
