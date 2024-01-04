
// NOTA: con il codice generato potrebbe essere richiesto almeno .NET Framework 4.5 o .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class SoggettiDestinazione
{

    private SoggettiDestinazioneSoggettoDestinazione[] soggettoDestinazioneField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("SoggettoDestinazione")]
    public SoggettiDestinazioneSoggettoDestinazione[] SoggettoDestinazione
    {
        get
        {
            return this.soggettoDestinazioneField;
        }
        set
        {
            this.soggettoDestinazioneField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiDestinazioneSoggettoDestinazione
{

    private SoggettiDestinazioneSoggettoDestinazioneIdDestinazione idDestinazioneField;

    private SoggettiDestinazioneSoggettoDestinazioneRagioneSociale ragioneSocialeField;

    private SoggettiDestinazioneSoggettoDestinazioneIndirizzoDestinazione indirizzoDestinazioneField;

    private SoggettiDestinazioneSoggettoDestinazioneCapDestinazione capDestinazioneField;

    private SoggettiDestinazioneSoggettoDestinazioneCittaDestinazione cittaDestinazioneField;

    private SoggettiDestinazioneSoggettoDestinazioneProvDestinazione provDestinazioneField;

    /// <remarks/>
    public SoggettiDestinazioneSoggettoDestinazioneIdDestinazione idDestinazione
    {
        get
        {
            return this.idDestinazioneField;
        }
        set
        {
            this.idDestinazioneField = value;
        }
    }

    /// <remarks/>
    public SoggettiDestinazioneSoggettoDestinazioneRagioneSociale ragioneSociale
    {
        get
        {
            return this.ragioneSocialeField;
        }
        set
        {
            this.ragioneSocialeField = value;
        }
    }

    /// <remarks/>
    public SoggettiDestinazioneSoggettoDestinazioneIndirizzoDestinazione indirizzoDestinazione
    {
        get
        {
            return this.indirizzoDestinazioneField;
        }
        set
        {
            this.indirizzoDestinazioneField = value;
        }
    }

    /// <remarks/>
    public SoggettiDestinazioneSoggettoDestinazioneCapDestinazione capDestinazione
    {
        get
        {
            return this.capDestinazioneField;
        }
        set
        {
            this.capDestinazioneField = value;
        }
    }

    /// <remarks/>
    public SoggettiDestinazioneSoggettoDestinazioneCittaDestinazione cittaDestinazione
    {
        get
        {
            return this.cittaDestinazioneField;
        }
        set
        {
            this.cittaDestinazioneField = value;
        }
    }

    /// <remarks/>
    public SoggettiDestinazioneSoggettoDestinazioneProvDestinazione provDestinazione
    {
        get
        {
            return this.provDestinazioneField;
        }
        set
        {
            this.provDestinazioneField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiDestinazioneSoggettoDestinazioneIdDestinazione
{

    private string idDestinazioneField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string IdDestinazione
    {
        get
        {
            return this.idDestinazioneField;
        }
        set
        {
            this.idDestinazioneField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiDestinazioneSoggettoDestinazioneRagioneSociale
{

    private string nomeDestinazioneField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NomeDestinazione
    {
        get
        {
            return this.nomeDestinazioneField;
        }
        set
        {
            this.nomeDestinazioneField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiDestinazioneSoggettoDestinazioneIndirizzoDestinazione
{

    private string indDestinazioneField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string IndDestinazione
    {
        get
        {
            return this.indDestinazioneField;
        }
        set
        {
            this.indDestinazioneField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiDestinazioneSoggettoDestinazioneCapDestinazione
{

    private string capDestinazioneField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CapDestinazione
    {
        get
        {
            return this.capDestinazioneField;
        }
        set
        {
            this.capDestinazioneField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiDestinazioneSoggettoDestinazioneCittaDestinazione
{

    private string cittaDestinazioneField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CittaDestinazione
    {
        get
        {
            return this.cittaDestinazioneField;
        }
        set
        {
            this.cittaDestinazioneField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiDestinazioneSoggettoDestinazioneProvDestinazione
{

    private string provDestinazioneField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ProvDestinazione
    {
        get
        {
            return this.provDestinazioneField;
        }
        set
        {
            this.provDestinazioneField = value;
        }
    }
}

