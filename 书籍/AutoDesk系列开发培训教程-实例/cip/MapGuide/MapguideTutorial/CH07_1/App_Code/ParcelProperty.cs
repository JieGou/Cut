public class ParcelProperty
{
    public ParcelProperty()
    {
        acreage = "";
        billingAddr = "";
        description1 = "";
        description2 = "";
        description3 = "";
        description4 = "";
        id = "";
        LotDimension = "";
        LotSize = 0;
        owner = "";
        zoning = "";
    }

    private string id;
    public string ID
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }


    private string acreage;
    public string Acreage
    {
        get {
            return acreage; 
        }
        set {
            acreage = value; 
        }
    }

    private string billingAddr;
    public string BillingAddr
    {
        get
        {
            return billingAddr;
        }
        set
        {
            billingAddr = value;
        }
    }

    private string description1;
    public string Description1
    {
        get
        {
            return description1;
        }
        set
        {
            description1 = value;
        }
    }

    private string description2;
    public string Description2
    {
        get
        {
            return description2;
        }
        set
        {
            description2 = value;
        }
    }

    private string description3;
    public string Description3
    {
        get
        {
            return description3;
        }
        set
        {
            description3 = value;
        }
    }

    private string description4;
    public string Description4
    {
        get
        {
            return description4;
        }
        set
        {
            description4 = value;
        }
    }

    private string lotDimension;
    public string LotDimension
    {
        get
        {
            return lotDimension;
        }
        set
        {
            lotDimension = value;
        }
    }

    private int lotSize;
    public int LotSize
    {
        get
        {
            return lotSize;
        }
        set
        {
            lotSize = value;
        }
    }

    private string owner;
    public string Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;
        }
    }

    private string zoning;
    public string Zoning
    {
        get
        {
            return zoning;
        }
        set
        {
            zoning = value;
        }
    }

}
