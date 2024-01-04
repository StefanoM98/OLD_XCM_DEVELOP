
// NOTA: con il codice generato potrebbe essere richiesto almeno .NET Framework 4.5 o .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class ordini
{

    private ordiniOrdine[] ordineField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ordine")]
    public ordiniOrdine[] ordine
    {
        get
        {
            return this.ordineField;
        }
        set
        {
            this.ordineField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ordiniOrdine
{

    private object[] itemsField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("barcode", typeof(ordiniOrdineBarcode))]
    [System.Xml.Serialization.XmlElementAttribute("capDestinazione", typeof(ordiniOrdineCapDestinazione))]
    [System.Xml.Serialization.XmlElementAttribute("capFatturazione", typeof(ordiniOrdineCapFatturazione))]
    [System.Xml.Serialization.XmlElementAttribute("cittaDestinazione", typeof(ordiniOrdineCittaDestinazione))]
    [System.Xml.Serialization.XmlElementAttribute("cittaFatturazione", typeof(ordiniOrdineCittaFatturazione))]
    [System.Xml.Serialization.XmlElementAttribute("codiceProdotto", typeof(ordiniOrdineCodiceProdotto))]
    [System.Xml.Serialization.XmlElementAttribute("descrizione", typeof(ordiniOrdineDescrizione))]
    [System.Xml.Serialization.XmlElementAttribute("idOrdine", typeof(ordiniOrdineIdOrdine))]
    [System.Xml.Serialization.XmlElementAttribute("impTotale", typeof(ordiniOrdineImpTotale))]
    [System.Xml.Serialization.XmlElementAttribute("impUnitario", typeof(ordiniOrdineImpUnitario))]
    [System.Xml.Serialization.XmlElementAttribute("indirizzoDestinazione", typeof(ordiniOrdineIndirizzoDestinazione))]
    [System.Xml.Serialization.XmlElementAttribute("indirizzoFatturazione", typeof(ordiniOrdineIndirizzoFatturazione))]
    [System.Xml.Serialization.XmlElementAttribute("iva", typeof(ordiniOrdineIva))]
    [System.Xml.Serialization.XmlElementAttribute("lotto", typeof(ordiniOrdineLotto))]
    [System.Xml.Serialization.XmlElementAttribute("nomeDestinazione", typeof(ordiniOrdineNomeDestinazione))]
    [System.Xml.Serialization.XmlElementAttribute("note", typeof(ordiniOrdineNote))]
    [System.Xml.Serialization.XmlElementAttribute("numeroOrdine", typeof(ordiniOrdineNumeroOrdine))]
    [System.Xml.Serialization.XmlElementAttribute("pIva", typeof(ordiniOrdinePIva))]
    [System.Xml.Serialization.XmlElementAttribute("provDestinazione", typeof(ordiniOrdineProvDestinazione))]
    [System.Xml.Serialization.XmlElementAttribute("provFatturazione", typeof(ordiniOrdineProvFatturazione))]
    [System.Xml.Serialization.XmlElementAttribute("quantita", typeof(ordiniOrdineQuantita))]
    [System.Xml.Serialization.XmlElementAttribute("ragioneSociale", typeof(ordiniOrdineRagioneSociale))]
    [System.Xml.Serialization.XmlElementAttribute("sconto", typeof(ordiniOrdineSconto))]
    public object[] Items
    {
        get
        {
            return this.itemsField;
        }
        set
        {
            this.itemsField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ordiniOrdineBarcode
{

    private string barcodeField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Barcode
    {
        get
        {
            return this.barcodeField;
        }
        set
        {
            this.barcodeField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ordiniOrdineCapDestinazione
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
public partial class ordiniOrdineCapFatturazione
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
public partial class ordiniOrdineCittaDestinazione
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
public partial class ordiniOrdineCittaFatturazione
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
public partial class ordiniOrdineCodiceProdotto
{

    private string codiceProdottoField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CodiceProdotto
    {
        get
        {
            return this.codiceProdottoField;
        }
        set
        {
            this.codiceProdottoField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ordiniOrdineDescrizione
{

    private string descrizioneField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Descrizione
    {
        get
        {
            return this.descrizioneField;
        }
        set
        {
            this.descrizioneField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ordiniOrdineIdOrdine
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
public partial class ordiniOrdineImpTotale
{

    private string impTotaleField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ImpTotale
    {
        get
        {
            return this.impTotaleField;
        }
        set
        {
            this.impTotaleField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ordiniOrdineImpUnitario
{

    private string impUnitarioField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ImpUnitario
    {
        get
        {
            return this.impUnitarioField;
        }
        set
        {
            this.impUnitarioField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ordiniOrdineIndirizzoDestinazione
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
public partial class ordiniOrdineIndirizzoFatturazione
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
public partial class ordiniOrdineIva
{

    private string ivaField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Iva
    {
        get
        {
            return this.ivaField;
        }
        set
        {
            this.ivaField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ordiniOrdineLotto
{

    private string lottoField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Lotto
    {
        get
        {
            return this.lottoField;
        }
        set
        {
            this.lottoField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ordiniOrdineNomeDestinazione
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
public partial class ordiniOrdineNote
{

    private string noteField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Note
    {
        get
        {
            return this.noteField;
        }
        set
        {
            this.noteField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ordiniOrdineNumeroOrdine
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
public partial class ordiniOrdinePIva
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
public partial class ordiniOrdineProvDestinazione
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

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ordiniOrdineProvFatturazione
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

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ordiniOrdineQuantita
{

    private string quantitaField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Quantita
    {
        get
        {
            return this.quantitaField;
        }
        set
        {
            this.quantitaField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ordiniOrdineRagioneSociale
{

    private string ragSocFatturazioneField;

    private string nomeDestinazioneField;

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
public partial class ordiniOrdineSconto
{

    private string scontoField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Sconto
    {
        get
        {
            return this.scontoField;
        }
        set
        {
            this.scontoField = value;
        }
    }
}

