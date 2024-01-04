
// NOTA: con il codice generato potrebbe essere richiesto almeno .NET Framework 4.5 o .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class SoggettiFatturazione
{

    private SoggettiFatturazioneSoggettoFatturazione[] soggettoFatturazioneField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("SoggettoFatturazione")]
    public SoggettiFatturazioneSoggettoFatturazione[] SoggettoFatturazione
    {
        get
        {
            return this.soggettoFatturazioneField;
        }
        set
        {
            this.soggettoFatturazioneField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiFatturazioneSoggettoFatturazione
{

    private SoggettiFatturazioneSoggettoFatturazioneIdFatturazione idFatturazioneField;

    private SoggettiFatturazioneSoggettoFatturazioneNumeroOrdine numeroOrdineField;

    private SoggettiFatturazioneSoggettoFatturazioneRagioneSociale ragioneSocialeField;

    private SoggettiFatturazioneSoggettoFatturazioneIndirizzoFatturazione indirizzoFatturazioneField;

    private SoggettiFatturazioneSoggettoFatturazioneCapFatturazione capFatturazioneField;

    private SoggettiFatturazioneSoggettoFatturazioneCittaFatturazione cittaFatturazioneField;

    private SoggettiFatturazioneSoggettoFatturazionePIva pIvaField;

    private SoggettiFatturazioneSoggettoFatturazioneProvFatturazione provFatturazioneField;

    /// <remarks/>
    public SoggettiFatturazioneSoggettoFatturazioneIdFatturazione idFatturazione
    {
        get
        {
            return this.idFatturazioneField;
        }
        set
        {
            this.idFatturazioneField = value;
        }
    }

    /// <remarks/>
    public SoggettiFatturazioneSoggettoFatturazioneNumeroOrdine numeroOrdine
    {
        get
        {
            return this.numeroOrdineField;
        }
        set
        {
            this.numeroOrdineField = value;
        }
    }

    /// <remarks/>
    public SoggettiFatturazioneSoggettoFatturazioneRagioneSociale ragioneSociale
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
    public SoggettiFatturazioneSoggettoFatturazioneIndirizzoFatturazione indirizzoFatturazione
    {
        get
        {
            return this.indirizzoFatturazioneField;
        }
        set
        {
            this.indirizzoFatturazioneField = value;
        }
    }

    /// <remarks/>
    public SoggettiFatturazioneSoggettoFatturazioneCapFatturazione capFatturazione
    {
        get
        {
            return this.capFatturazioneField;
        }
        set
        {
            this.capFatturazioneField = value;
        }
    }

    /// <remarks/>
    public SoggettiFatturazioneSoggettoFatturazioneCittaFatturazione cittaFatturazione
    {
        get
        {
            return this.cittaFatturazioneField;
        }
        set
        {
            this.cittaFatturazioneField = value;
        }
    }

    /// <remarks/>
    public SoggettiFatturazioneSoggettoFatturazionePIva pIva
    {
        get
        {
            return this.pIvaField;
        }
        set
        {
            this.pIvaField = value;
        }
    }

    /// <remarks/>
    public SoggettiFatturazioneSoggettoFatturazioneProvFatturazione provFatturazione
    {
        get
        {
            return this.provFatturazioneField;
        }
        set
        {
            this.provFatturazioneField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiFatturazioneSoggettoFatturazioneIdFatturazione
{

    private string idOrdineField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string IdOrdine
    {
        get
        {
            return this.idOrdineField;
        }
        set
        {
            this.idOrdineField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiFatturazioneSoggettoFatturazioneNumeroOrdine
{

    private string numeroOrdineField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NumeroOrdine
    {
        get
        {
            return this.numeroOrdineField;
        }
        set
        {
            this.numeroOrdineField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiFatturazioneSoggettoFatturazioneRagioneSociale
{

    private string ragSocFatturazioneField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string RagSocFatturazione
    {
        get
        {
            return this.ragSocFatturazioneField;
        }
        set
        {
            this.ragSocFatturazioneField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiFatturazioneSoggettoFatturazioneIndirizzoFatturazione
{

    private string indFatturazioneField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string IndFatturazione
    {
        get
        {
            return this.indFatturazioneField;
        }
        set
        {
            this.indFatturazioneField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiFatturazioneSoggettoFatturazioneCapFatturazione
{

    private string capFatturazioneField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CapFatturazione
    {
        get
        {
            return this.capFatturazioneField;
        }
        set
        {
            this.capFatturazioneField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiFatturazioneSoggettoFatturazioneCittaFatturazione
{

    private string cittaFatturazioneField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CittaFatturazione
    {
        get
        {
            return this.cittaFatturazioneField;
        }
        set
        {
            this.cittaFatturazioneField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiFatturazioneSoggettoFatturazionePIva
{

    private string pIvaFatturazioneField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string PIvaFatturazione
    {
        get
        {
            return this.pIvaFatturazioneField;
        }
        set
        {
            this.pIvaFatturazioneField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class SoggettiFatturazioneSoggettoFatturazioneProvFatturazione
{

    private string provFatturazioneField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ProvFatturazione
    {
        get
        {
            return this.provFatturazioneField;
        }
        set
        {
            this.provFatturazioneField = value;
        }
    }
}

