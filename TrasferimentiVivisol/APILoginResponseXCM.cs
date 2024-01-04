
using System;

public class RootobjectLoginXCM
{
    public ResultLoginXCM result { get; set; }
    public UserLoginXCM user { get; set; }
}

public class ResultLoginXCM
{
    public object[] messages { get; set; }
    public bool status { get; set; }
    public object info { get; set; }
    public int maxPages { get; set; }
}

public class UserLoginXCM
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

