
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

    private ushort id_mittField;

    private datarootMittDest destField;

    private string tipo_mField;

    /// <remarks/>
    public ushort id_mitt
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
    public datarootMittDest dest
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

    private ushort id_destField;

    private datarootMittDestMOV mOVField;

    private string tipo_dField;

    /// <remarks/>
    public ushort id_dest
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
    public datarootMittDestMOV MOV
    {
        get
        {
            return this.mOVField;
        }
        set
        {
            this.mOVField = value;
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
public partial class datarootMittDestMOV
{

    private string t_docField;

    private uint dDTField;

    private System.DateTime d_trField;

    private System.DateTime h_trField;

    private datarootMittDestMOVAIC aICField;

    private string tipo_trField;

    private string tipo_movField;

    /// <remarks/>
    public string t_doc
    {
        get
        {
            return this.t_docField;
        }
        set
        {
            this.t_docField = value;
        }
    }

    /// <remarks/>
    public uint DDT
    {
        get
        {
            return this.dDTField;
        }
        set
        {
            this.dDTField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
    public System.DateTime d_tr
    {
        get
        {
            return this.d_trField;
        }
        set
        {
            this.d_trField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "time")]
    public System.DateTime h_tr
    {
        get
        {
            return this.h_trField;
        }
        set
        {
            this.h_trField = value;
        }
    }

    /// <remarks/>
    public datarootMittDestMOVAIC AIC
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
    public string tipo_mov
    {
        get
        {
            return this.tipo_movField;
        }
        set
        {
            this.tipo_movField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class datarootMittDestMOVAIC
{

    private uint codField;

    private string lotField;

    private System.DateTime d_scadField;

    private byte qtaField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public uint cod
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
    public string lot
    {
        get
        {
            return this.lotField;
        }
        set
        {
            this.lotField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
    public System.DateTime d_scad
    {
        get
        {
            return this.d_scadField;
        }
        set
        {
            this.d_scadField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte qta
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

