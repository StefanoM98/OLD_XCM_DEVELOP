using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model.Logistica93
{
    public class Logistica93_ShipmentIN
    {
        #region Testata
        /// <summary>
        /// 1.4.2	Note       
        /// (1)	A	= alfanumerico	 ; N 	= numerico        
        /// (2) blank	‘TASSATIVA’	‘ENTRO IL’	'DOPO IL'
        /// (3)	F = porto franco C 	= conto servizioA 	= porto assegnato
        /// (4) ZCOR = Trasporto Corriere(trasporto normale)
        ///     ZDIR = Trasporto Diretto    
        ///     ZESP = Trasporto Espresso 
        ///     ZAGE = Trasporto dotazioni agenti
        ///     ZCOM = Trasporto commerciale 
        ///     ZINF = Trasporto dotazioni  
        ///     ZMKT = Trasporto marketing
        ///     ZTRD = Trasporto trade 
        /// </summary>

        //FSE_zzzz_nnnnn_yyyyy.TXT -> Testata Spedizione

        //Valore fisso "1" (DDT e Consegne)
        public string TipoRecord { get; set; }
        public int[] idxTipoRecord = new int[] { 0, 1 };

        //N° trasporto / Nro Vague
        public string NumeroBorderau { get; set; }
        public int[] idxNumeroBorderau = new int[] { 1, 10 };

        //aaaammgg - Data inizio trasporto effettivo 
        public string DataSpedizione { get; set; }
        public int[] idxDataSpedizione = new int[] { 11, 8 };

        //N° progressivo DDT 
        public string NumeroDDT { get; set; }
        public int[] idxNumeroDDT = new int[] { 19, 10 };

        //aaaammgg - Data uscita merci effettiva 
        public string DataDDT { get; set; }
        public int[] idxDataDDT = new int[] { 29, 8 };

        //Numero delivery SAP(per divisione CAI & INNEOV valorizzato solo se 1 DDT= 1 consegna)
        public string RifNConsegna { get; set; }
        public int[] idxRifNConsegna = new int[] { 37, 10 };

        //Shipping point
        public string LuogoSpedizione { get; set; }
        public int[] idxLuogoSpedizione = new int[] { 47, 4 };

        //Espresso in kg con 2 decimali
        public string PesoDelivery { get; set; }
        public int[] idxPesoDelivery = new int[] { 51, 7 };

        //Intestazione destinatario merci della consegna 
        public string TipoCliente { get; set; }
        public int[] idxTipoCliente = new int[] { 58, 15 };

        //Ragione sociale destinatario merci della consegna 
        public string Destinatario { get; set; }
        public int[] idxDestinatario = new int[] { 73, 35 };

        //Indirizzo destinatario merci della consegna 
        public string Indirizzo { get; set; }
        public int[] idxIndirizzo = new int[] { 108, 40 };

        //Località destinatario merci della consegna 
        public string Localita { get; set; }
        public int[] idxLocalita = new int[] { 148, 40 };

        //CAP (previsto anche per l'estero) 
        public string CAP { get; set; }
        public int[] idxCAP = new int[] { 188, 10 };

        //Sigla provincia destinazione
        public string SiglaProvDestinazione { get; set; }
        public int[] idxSiglaProvDestinazione = new int[] { 198, 2 };

        //Partita IVA o Codice Fiscale
        public string PIVA_CODF { get; set; }
        public int[] idxPIVA_CODF = new int[] { 200, 16 };

        //aaaammgg - valorizzata solo per alcune condizioni di spedizione
        public string DataConsegna { get; set; }
        public int[] idxDataConsegna = new int[] { 216, 8 };

        //Condizione spedizione della consegna (testo descrittivo) (2)
        public string TipoDataConsegna { get; set; }
        public int[] idxTipoDataConsegna = new int[] { 224, 10 };

        //Da input videata stampa bordereau (default F)
        public string TipoSpedizione { get; set; }
        public int[] idxTipoSpedizione = new int[] { 234, 1 };

        //2 decimali - Valore netto fattura se il modo di pagamento è contrassegno
        public string ImportoContrassegno { get; set; }
        public int[] idxImportoContrassegno = new int[] { 235, 11 };

        //Nota modalità di consegna presente sul DDT
        public string NotaModalitaDiConsegna { get; set; }
        public int[] idxNotaModalitaDiConsegna = new int[] { 246, 75 };

        //Nota Commenti tempi consegna presente sul DDT
        public string NotaCommentiTempiConsegna { get; set; }
        public int[] idxNotaCommentiTempiConsegna = new int[] { 321, 75 };

        //Nota EPAL presente sul DDT
        public string NotaEPAL { get; set; }
        public int[] idxNotaEPAL = new int[] { 396, 75 };

        //Nota Bolla presente sul DDT
        public string NotaBolla { get; set; }
        public int[] idxNotaBolla = new int[] { 471, 75 };

        //Numero colli dettaglio (sovraimballi)
        public string NumeroColliDettaglio { get; set; }
        public int[] idxNumeroColliDettaglio = new int[] { 546, 6 };

        //Numero colli standard
        public string NumeroColliStandard { get; set; }
        public int[] idxNumeroColliStandard = new int[] { 552, 6 };

        //Numero espositori / PLV / H.G.
        public string NumeroEspositoriPLV { get; set; }
        public int[] idxNumeroEspositoriPLV = new int[] { 558, 6 };

        //Numero pedane
        public string NumeroPedane { get; set; }
        public int[] idxNumeroPedane = new int[] { 564, 6 };

        //Codice corriere presente sul trasporto (è quello di fatturazione)
        public string CodiceCorriere { get; set; }
        public int[] idxCodiceCorriere = new int[] { 570, 10 };

        //Itinerario corriere
        public string ItinerarioCorriere { get; set; }
        public int[] idxItinerarioCorriere = new int[] { 580, 6 };

        //CAI & INNEOV = sottozona / linea cappario DPL & HR = linea cappario SAIPO = NON VALORIZZATO
        public string SottoZonaCorriere { get; set; }
        public int[] idxSottoZonaCorriere = new int[] { 586, 5 };

        //valore del campo Numero pedane
        public string NumeroPedaneEPAL { get; set; }
        public int[] idxNumeroPedaneEPAL = new int[] { 591, 6 };

        //Tipo di trasporto (Escluso DPL & HR)
        public string TipoTrasporto { get; set; }
        public int[] idxTipoTrasporto = new int[] { 597, 4 };

        //Solo se utilizzato il cappario 
        public string ZonaCorriere { get; set; }
        public int[] idxZonaCorriere = new int[] { 601, 3 };

        //Valorizzato solo per Divisione Prodotti Lusso
        public string PedanaDirezionale { get; set; }
        public int[] idxPedanaDirezionale = new int[] { 604, 1 };

        //Codice abbinamento consegne (solo Saipo)
        public string CodiceAbbinamento { get; set; }
        public int[] idxCodiceAbbinamento = new int[] { 605, 10 };

        //Ordini maquillage (solo Saipo)
        public string NumeroOrdineCliente { get; set; }
        public int[] idxNumeroOrdineCliente = new int[] { 615, 8 };

        //Contratto corriere
        public string ContrattoCorriere { get; set; }
        public int[] idxContrattoCorriere = new int[] { 623, 10 };

        //Via 3
        public string Via3 { get; set; }
        public int[] idxVia3 = new int[] { 633, 40 };

        //N° fattura (asterischi se presenti più fatture)
        public string NumeroFattura { get; set; }
        public int[] idxNumeroFattura = new int[] { 673, 10 };

        //Peso polveri  (Kg 3 dec)
        public string PesoPolveri { get; set; }
        public int[] idxPesoPolveri = new int[] { 683, 9 };

        //Codice filiale
        public string NumeroFiliale { get; set; }
        public int[] idxNumeroFiliale = new int[] { 692, 17 };

        //Intestazione filiale
        public string TipoClienteIntestazione { get; set; }
        public int[] idxTipoClienteIntestazione = new int[] { 709, 15 };

        //Ragione filiale
        public string DestinatarioFiliale { get; set; }
        public int[] idxDestinatarioFiliale = new int[] { 724, 35 };

        //Indirizzo filiale
        public string IndirizzoFiliale { get; set; }
        public int[] idxIndirizzoFiliale = new int[] { 759, 40 };

        //Località filiale
        public string LocalitaFiliale { get; set; }
        public int[] idxLocalitaFiliale = new int[] { 799, 40 };

        //CAP filiale
        public string CAPFiliale { get; set; }
        public int[] idxCAPFiliale = new int[] { 839, 10 };

        //Provincia filiale
        public string SiglaProvDestinazioneFiliale { get; set; }
        public int[] idxSiglaProvDestinazioneFiliale = new int[] { 849, 2 };

        //
        public string Filler { get; set; }
        public int[] idxFiller = new int[] { 851, 10 };

        //Delivery volume (3 dec.)
        public string DeliveryVolume { get; set; }
        public int[] idxDeliveryVolume = new int[] { 861, 15 };

        //Unità di volume (M3)
        public string VolumeUnit { get; set; }
        public int[] idxVolumeUnit = new int[] { 876, 3 };

        //Priorità consegna
        public string PrioritàConsegna { get; set; }
        public int[] idxPrioritàConsegna = new int[] { 861, 2 };

        //N° telefono completo (+39)
        public string SMSPreavviso { get; set; }
        public int[] idxSMSPreavviso = new int[] { 881, 12 };//era 30 ma va fuori range
        #endregion

        public override string ToString()
        {
            return $"Data Spedizione:{DataSpedizione} Numero DDT:{NumeroDDT} Numero Borderau:{NumeroBorderau} Numero Colli Dettaglio:{NumeroColliDettaglio} Numero Colli Standard:{NumeroColliStandard} Numero Pedane:{NumeroPedane}"; 
        }
    }
    public class LorealSegnacolli
    {
        #region Segnacollo
        //FSE_zzzz_nnnnn_yyyyy.TXT

        //Codice cliente destinazione merce
        public string S_CodiceCliente { get; set; }
        public int[] idxS_CodiceCliente = new int[] { 0, 10 };

        //N° progressivo DDT o Consegna SAP
        public string S_NumeroDDT { get; set; }
        public int[] idxS_NumeroDDT = new int[] { 10, 10 };

        //Peso del collo  espresso in Kg (3 decimali)
        public string S_Peso { get; set; }
        public int[] idxS_Peso = new int[] { 20, 6 };

        //N° identificativo del collo 
        public string S_NumeroCollo { get; set; }
        public int[] idxS_NumeroCollo = new int[] { 29, 18 };

        //Zona spedizione (dati interni)  
        public string S_ZonaSpedizione { get; set; }
        public int[] idxS_ZonaSpedizione = new int[] { 47, 2 };

        //Settore merceologico 
        public string S_Marca { get; set; }
        public int[] idxS_Marca = new int[] { 49, 2 };

        //Tipo imballo
        public string S_TipoImballo { get; set; }
        public int[] idxS_TipoImballo = new int[] { 51, 1 };

        //Tipo Elaborazione
        public string S_TipoElaborazione { get; set; }
        public int[] idxS_TipoElaborazione = new int[] { 52, 1 };

        //Progressivo annuale giorno elaborazione 
        public string S_CheckDigit { get; set; }
        public int[] idxS_CheckDigit = new int[] { 56, 1 };

        //Tipo unità magazzino
        public string S_TipoImballoMagazzino { get; set; }
        public int[] idxS_TipoImballoMagazzino = new int[] { 57, 3 };

        //Codice prodotto
        public string S_CodiceProdotto { get; set; }
        public int[] idxS_CodiceProdotto = new int[] { 60, 18 };

        //Descrizione prodotto
        public string S_DescrizioneProdotto { get; set; }
        public int[] idxS_DescrizioneProdotto = new int[] { 78, 40 };

        //Lunghezza collo (3 dec.)
        public string S_BoxLAENG { get; set; }
        public int[] idxS_BoxLAENG = new int[] { 118, 12 };

        //
        public string S_Filler1 { get; set; }
        public int[] idxS_Filler1 = new int[] { 130, 1 };

        //Larghezza collo (3dec.)
        public string S_BoxBREIT { get; set; }
        public int[] idxS_BoxBREIT = new int[] { 131, 12 };

        //
        public string S_Filler2 { get; set; }
        public int[] idxS_Filler2 = new int[] { 143, 1 };

        //Altezza (3 dec.)
        public string S_BoxHOEHE { get; set; }
        public int[] idxS_BoxHOEHE = new int[] { 145, 12 };

        //
        public string S_Filler3 { get; set; }
        public int[] idxS_Filler3 = new int[] { 156, 1 };

        //Unità di misura
        public string S_DimensionUnit { get; set; }
        public int[] idxS_DimensionUnit = new int[] { 157, 2 };

        #endregion
    }
    public class LorealEsiti
    {
        #region Esiti
        /// <summary>
        /// 3.3.1	Note
        ///(1)	A	= alfanumerico
        ///N = numerico
        ///
        ///(2) Lista delle causali di consegna gestibili dall'interfaccia esiti; ad oggi è comunque caricata in automatico la sola causale ‘00’:
        ///00	Consegna a buon fine
        ///03	Apertura giacenza
        ///07	Rientro
        ///
        ///
        ///(5) Lista delle sottocausali di giacenza gestibili dall'interfaccia esiti; 
        ///601	--	NON CONF.A ORDINE
        ///602	--	MOTIVO NN SPECIFIC.
        ///603	--	CONSEGNA DIFFERITA
        ///604	--	NON PRENOT/PREAVVISO
        ///605	--	NN ORDINATA O ANNULL
        ///606	--	RICHIEDE SERVIZI EXT
        ///607	--	RICH/BLOCC DA SAIPO
        ///608	--	IMPOSSIB.DI SCARICO
        ///609	--	CAUSA CONTRASSEGNO
        ///610	--	VUOLE APRIRE IMBALLI
        ///611	--	DEST/INDIRIZ.ERRATO
        ///650	--	DISSERVIZIO CORRIERE
        ///651	--	PROBLEMA PALLETTIZZ.
        ///652	--	MERCE DANNEGGIATA
        ///653	--	COLLO MANCANTE
        ///654	--	MERCE NON ORDINATA
        ///655	--	RITARDO DI CONSEGNA
        ///656	--	EAN ERRATO
        ///657	--	REF.NON IN ASSORTIM
        ///658	--	UNITÀ PER IMBALLO
        ///659	--	ALTRO DA SPECIFICARE
        ///662	--	CLIENTE VUOLE CONTRO
        ///663	--	DESTINATARIO CHIEDE
        ///664	--	DESTINATARIO CHIUSO
        ///665	--	MERCE RESPINTA PER R
        ///666	--	MERCE SPEDITA IN ANT
        ///667	--	NON PRENOT/PREAVVISO
        ///668	--	RIMANDA LO SVINCOLO
        ///669	--	SCONOSCIUTO ALL'INDI
        ///670	--	TROVATO CHIUSO
        ///671	--	VUOLE APRIRE IMBALLI
        ///672	--	ERRATA RAGIONE SOCIA
        /// </summary>

        //V. campo 4 del file bordereau
        public string E_NumeroDDT { get; set; }
        public int[] idxE_NumeroDDT = new int[] { 0, 10 };

        //V. campo 4 del file bordereau
        public string E_RiferimentoNumeroConsegnaSAP { get; set; }
        public int[] idxE_RiferimentoNumeroConsegnaSAP = new int[] { 10, 10 };

        //AAAAMMGG
        public string E_DataConsegnaADestino { get; set; }
        public int[] idxE_DataConsegnaADestino = new int[] { 20, 8 };

        //(2)
        public string E_Causale { get; set; }
        public int[] idxE_Causale = new int[] { 28, 2 };

        //(2)
        public string E_SottoCausale { get; set; }
        public int[] idxE_SottoCausale = new int[] { 30, 3 };

        //(2)
        public string E_RiferimentoCorriere { get; set; }
        public int[] idxE_RiferimentoCorriere = new int[] { 33, 20 };

        //
        public string E_Filler1 { get; set; }
        public int[] idxE_Filler1 = new int[] { 53, 18 };

        //
        public string E_Note { get; set; }
        public int[] idxE_Note = new int[] { 71, 200 };

        //
        public string E_Filler2 { get; set; }
        public int[] idxE_Filler2 = new int[] { 271, 5 };
        #endregion

        public int statoUNITEX { get; set; }

        internal static LorealEsiti FromCsv(string csvLine)
        {
            var values = csvLine.Split(';');
            LorealEsiti esitiOUT = new LorealEsiti()
            {
                E_NumeroDDT = values[0]
            };
            return esitiOUT;
        }
    }

    public class Logistica93_StatiDocumento
    {
        public int IdUnitex { get; set; }
        public string CodiceStato { get; set; }
        public static Logistica93_StatiDocumento FromCsv(string csvLine)
        {
            var values = csvLine.Split(';');
            Logistica93_StatiDocumento stato = new Logistica93_StatiDocumento();
            stato.IdUnitex = Convert.ToInt32(values[0]);
            stato.CodiceStato = Convert.ToString(values[1]);
            return stato;

        }
    }
}
