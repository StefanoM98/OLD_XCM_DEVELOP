
using System;

#region Entrata Merce
public class RootobjectEntrataMerce
{
    public string partNumber { get; set; }
    public string logWareId { get; set; }
    public string batch { get; set; }
    public string expiryDate { get; set; }
    public string movementDate { get; set; }
    public double quantity { get; set; } // modificato, comunicare a bianchini da decimal a double
    public string orderDetailId { get; set; } //ExternalID riga ricevuto dall'ordine fornitore
    public string note { get; set; } //
    public string orderId { get; set; } // ExternalID testata ricevuto dall'ordine fornitore
    public string system { get { return "Sintesi_Italia"; } }
}
public class RootobjectEntrataMerceResponse
{
    public string recordId { get; set; }
    public bool success { get; set; }
    public string errorMessage { get; set; }
}
#endregion

#region Preparazione Carico
public class RootobjectInvioPreparazioneCarico
{
    public string idRouting { get; set; } //dock (routeID) = passa in preparazione su SOL il viaggio
    public string system { get; set; }//{ get { return "Sintesi_Italia"; } }

}
public class RootobjecttInvioPreparazioneCaricoResponse
{
    public string idRouting { get; }
    public bool success { get; }
    public string errorMessage { get; }
}
#endregion

#region Invio Movimenti
public class RootobjectInvioMovimenti
{
    //public string Id { get; set; }
    //public int CenterId { get; set; }
    //public int DayOrder { get; set; }
    //public string termineFasePreparazione { get; set; }
    //public string idViaggio { get; set; }
    //public DateTime dataScadenzaLotto { get; set; }
    //public string umArticolo { get; set; }
    public string idRouting { get; set; }
    public string pallet { get; set; }
    public string batch { get; set; }
    public string movementDate { get; set; }
    public double quantity { get; set; }
    public string logWareId { get; set; }
    public string note { get; set; }
    public string idUtente { get; set; }
    public string idPianifica { get; set; }
    public double numColli { get; set; }//lato sol, note pianifica
    public string partNumber { get; set; }
    public string system { get; set; }
}
    public class RootobjectInvioMovimentiResponse
    {
        public string idRouting { get; }
        public bool success { get; }
        public string errorMessage { get; }
    }
    #endregion


