
using System;

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
