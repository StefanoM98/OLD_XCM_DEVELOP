
// NOTA: con il codice generato potrebbe essere richiesto almeno .NET Framework 4.5 o .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class dataroot
{

    private datarootMitt mittField;

    /// <remarks/>
    public datarootMitt mitt
    {
        get
        {
            return this.mittField;
        }
        set
        {
            this.mittField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class datarootMitt
{

    private string id_mittField;

    private datarootMittDest[] destField;

    private string tipo_mField;

    /// <remarks/>
    public string id_mitt
    {
        get
        {
            return this.id_mittField;
        }
        set
        {
            this.id_mittField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("dest")]
    public datarootMittDest[] dest
    {
        get
        {
            return this.destField;
        }
        set
        {
            this.destField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string tipo_m
    {
        get
        {
            return this.tipo_mField;
        }
        set
        {
            this.tipo_mField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class datarootMittDest
{

    private string id_destField;

    private datarootMittDestFAT fATField;

    private string tipo_dField;

    /// <remarks/>
    public string id_dest
    {
        get
        {
            return this.id_destField;
        }
        set
        {
            this.id_destField = value;
        }
    }

    /// <remarks/>
    public datarootMittDestFAT FAT
    {
        get
        {
            return this.fATField;
        }
        set
        {
            this.fATField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string tipo_d
    {
        get
        {
            return this.tipo_dField;
        }
        set
        {
            this.tipo_dField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class datarootMittDestFAT
{

    private datarootMittDestFATAIC[] aICField;

    private string tipo_trField;

    private string meseField;

    private string annoField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AIC")]
    public datarootMittDestFATAIC[] AIC
    {
        get
        {
            return this.aICField;
        }
        set
        {
            this.aICField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string tipo_tr
    {
        get
        {
            return this.tipo_trField;
        }
        set
        {
            this.tipo_trField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string mese
    {
        get
        {
            return this.meseField;
        }
        set
        {
            this.meseField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string anno
    {
        get
        {
            return this.annoField;
        }
        set
        {
            this.annoField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class datarootMittDestFATAIC
{

    private string codField;

    private string valField;

    private string qtaField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string cod
    {
        get
        {
            return this.codField;
        }
        set
        {
            this.codField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string val
    {
        get
        {
            return this.valField;
        }
        set
        {
            this.valField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string qta
    {
        get
        {
            return this.qtaField;
        }
        set
        {
            this.qtaField = value;
        }
    }
}

