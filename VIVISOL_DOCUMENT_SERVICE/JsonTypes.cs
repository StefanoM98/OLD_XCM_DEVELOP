
public class RootobjectGetMaterialBalance
{
    public GetMaterialBalance[] Property1 { get; set; }
}

public class GetMaterialBalance
{
    public string productId { get; set; }
    public string productDescription { get; set; }
    public string expiryDate { get; set; }
    public string batch { get; set; }
}


public class RootobjectPutMaterialBalance
{
    public string centerCode { get; set; }
    public string movementDate { get; set; }
    public PutMaterialBalance[] details { get; set; }
}

public class PutMaterialBalance
{
    public string warehouseCode { get; set; }
    public string masterCode { get; set; }
    public int quantity { get; set; }
    public string batchItemCodeOld { get; set; }
    public string batchExpiryDateOld { get; set; }
    public string batchItemCodeNew { get; set; }
    public string batchExpiryDateNew { get; set; }
}

public class RootobjectPostMaterialBalance
{
    public string centerCode { get; set; }
    public string movementDate { get; set; }
    public PostMaterialBalance[] details { get; set; }
}

public class PostMaterialBalance
{
    public string warehouseCode { get; set; }
    public string masterCode { get; set; }
    public int quantity { get; set; }
    public string batchItemCode { get; set; }
    public string batchExpiryDate { get; set; }
    public string adxCause { get; set; }
}

