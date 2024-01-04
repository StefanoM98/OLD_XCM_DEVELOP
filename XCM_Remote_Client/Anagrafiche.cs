
using Newtonsoft.Json;

public class Anagrafiche
{
    [JsonProperty("ID_ANAGRAFICA")]
    public long ID_ANAGRAFICA { get; set; }

    [JsonProperty("RagioneSociale")]
    public string RagioneSociale { get; set; }

    [JsonProperty("PIva")]
    public string PIva { get; set; }

    [JsonProperty("Via")]
    public string Via { get; set; }

    [JsonProperty("Citta")]
    public string Citta { get; set; }

    [JsonProperty("CAP")]
    public string CAP { get; set; }

    [JsonProperty("Nazione")]
    public string Nazione { get; set; }

    [JsonProperty("Provincia")]
    public string Provincia { get; set; }

    [JsonProperty("Regione")]
    public string Regione { get; set; }

    public override string ToString()
    {
        return $"{RagioneSociale} - {CAP} - {Provincia}";
    }

}




