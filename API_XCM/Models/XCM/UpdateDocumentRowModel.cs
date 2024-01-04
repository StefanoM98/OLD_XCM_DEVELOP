
using System.Collections.Generic;

public class UpdateDocumentRowRootobject
{
    public Row row { get; set; }  
}

public class Row
{
    public int id { get; set; }
    public string partNumber { get; set; }
    public int qty { get; set; }
}
