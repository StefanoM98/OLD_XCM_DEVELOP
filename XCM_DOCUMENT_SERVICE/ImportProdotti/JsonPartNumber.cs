
public class RootobjectPartNuberNew
{
    public HeaderPartNuberNew header { get; set; }
    public AuthorizationPartNuberNew[] authorizations { get; set; }
    public PackagePartNuberNew[] packages { get; set; }
    public BarcodePartNuberNew[] barcodes { get; set; }
}

public class HeaderPartNuberNew
{
    public string id { get; set; }
    public string description { get; set; }
    public string descriptionShort { get; set; }
    public string kind { get; set; }
    public string inboundUM { get; set; }
    public string outBoundUM { get; set; }
    public int umRatio { get; set; }
    public string groupCode { get; set; }
    public string categoryCode { get; set; }
    public string classCode { get; set; }
    public string goodsType { get; set; }
    public string brandCode { get; set; }
    public string producerCode { get; set; }
    public string customsCode { get; set; }
    public string adrCode { get; set; }
    public string aicCode { get; set; }
    public int inBoundShelfLife { get; set; }
    public int outBoundShelfLife { get; set; }
    public int height { get; set; }
    public int width { get; set; }
    public int depth { get; set; }
}

public class AuthorizationPartNuberNew
{
    public string siteID { get; set; }
    public int procedureID { get; set; }
    public string customerID { get; set; }
    public bool isEnable { get; set; }
}

public class PackagePartNuberNew
{
    public string packageID { get; set; }
    public bool isDefault { get; set; }
    public int unitNetWeight { get; set; }
    public int unitGrossWeight { get; set; }
    public int unitCube { get; set; }
    public int packQty { get; set; }
    public int packsxBox { get; set; }
    public int boxesxPlt { get; set; }
    public int layerQty { get; set; }
    public int layerNo { get; set; }
    public int maxFloor { get; set; }
}

public class BarcodePartNuberNew
{
    public string type { get; set; }
    public string barcode { get; set; }
}


public class RootobjectResponsePartNumber
{
    public object[] messages { get; set; }
    public string info { get; set; }
    public int maxPages { get; set; }
    public bool status { get; set; }
}

