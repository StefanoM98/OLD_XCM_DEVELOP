using CommonAPITypes.ESPRITEC;
using DevExpress.Charts.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace XCM_DOCUMENT_SERVICE
{
    internal class CDLife
    {
        public object CreaCSVBollaCDL(int idDocumento, string token)
        {
            var OrdineApi = CommonAPITypes.ESPRITEC.EspritecDocuments.RestEspritecGetDocument(idDocumento, token);
            var ordineDes = JsonConvert.DeserializeObject<CommonAPITypes.ESPRITEC.EspritecDocuments.RootobjectOrder>(OrdineApi.Content);

            EspritecDocuments.RootobjectOrder DDTosservato = null;

            if(ordineDes != null && ordineDes.links.Count() == 1)
            {
                var ddtAPI = EspritecDocuments.RestEspritecGetDocument(ordineDes.links[0].id, token);
                DDTosservato = JsonConvert.DeserializeObject<EspritecDocuments.RootobjectOrder>(ddtAPI.Content);
                
            }
            else
            {
                return $"DDT relativo all'ordine {idDocumento} non trovato in database";
            }
            

            var resp = new List<string>();
            var db = new EntityModels.GnXcmEntities();
            //var docOsservato = db.uvwWmsDocument.FirstOrDefault(x => x.uniq == idDocumento);
            if (DDTosservato != null)
            {
                var righeOrdineAPI = EspritecDocuments.RestEspritecRowsListFromDocumentID(DDTosservato.header.id, token);//db.uvwWmsDocumentRows_XCM.Where(x => x.RowIdLink == idDocumento);
                var righeOrdine = JsonConvert.DeserializeObject<EspritecDocuments.RootobjectEspritecRows>(righeOrdineAPI.Content);
                if (righeOrdine != null)
                {
                    foreach (var row in righeOrdine.rows)
                    {
                        var nr = new ModelloCSVCdlife
                        {
                            CodiceFPRDestinatario = DDTosservato.header.info7,
                            CodiceIPADestinatario = DDTosservato.header.info8,
                            TipoDittaDestinatario = DDTosservato.header.info5,
                            EntePublicoDestinatario = "",
                            CodiceValuta = "EUR",
                            NumeroDocumento = DDTosservato.header.docNumber,
                            CodiceFiscaleDestinatario = DDTosservato.header.info9,
                            PartitaIVADestinatario = "",
                            IBAN = "",
                            CodiceAgente = "",
                            RagioneSocialeDestinatario = DDTosservato.header.consigneeDes,
                            IndirizzoDestinatario = DDTosservato.header.consigneeAddress,
                            LocalitaDestinatario = DDTosservato.header.consigneeLocation,
                            ProvDestinatario = DDTosservato.header.consigneeDistrict,
                            CAPDestinatario = DDTosservato.header.consigneeZipCode,
                            NazioneDestinatario = DDTosservato.header.consigneeCountry,
                            RagioneSocialeDestinazione = DDTosservato.header.unLoadDes,
                            IndirizzoDestinazione = DDTosservato.header.unloadAddress,
                            LocalitaDestinazione = DDTosservato.header.unloadLocation,
                            CAPDestinazione = DDTosservato.header.unloadZipCode,
                            ProvDestinazione = DDTosservato.header.unloadDistrict,
                            NazioneDestinazione = DDTosservato.header.unloadCountry,
                            CodiceDocumento = "DDT",
                            DataDocumento = DDTosservato.header.docDate.ToString("yyyy/MM/dd"),
                            AliquotaIVA = "",
                            Cambio = "",
                            CodicePagamento = DDTosservato.header.info4,
                            CodiceScontoContabileContropartita = "",
                            CodiceSedeAmministrativa = "",
                            CodiceSedeOperativa = "",
                            CodiceArticolo = row.partNumber,
                            DescrizioneArticolo = row.partNumberDes,
                            PrezzoUnitario = row.sellPrice.ToString("0:00000000"),
                            Quantita = row.qty.ToString("0:00000000"),
                            ScontoRiga = row.discount.ToString("0:00000000"),
                            UnitaDiMisura = "PZ",
                            

                        };
                        resp.Add(nr.ToString());
                    }
                    return resp;
                }
                else
                {
                    return $"Righe non trovate in DB del documento {DDTosservato.header.id}";
                }
            }
            else
            {
                return $"Documento non trovato in DB con id {DDTosservato.header.id}";
            }
        }
    }

    class ModelloCSVCdlife
    {
        #region Testata
        public string RagioneSocialeDestinatario { get; set; }//1
        public string IndirizzoDestinatario { get; set; }//2
        public string LocalitaDestinatario { get; set; }//3
        public string CAPDestinatario { get; set; }//4
        public string ProvDestinatario { get; set; }//5
        public string NazioneDestinatario { get; set; }//6
        public string PartitaIVADestinatario { get; set; }//7
        public string CodiceFiscaleDestinatario { get; set; }//8
        public string CodiceIPADestinatario { get; set; }//9
        public string CodiceFPRDestinatario { get; set; }//10
        public string EntePublicoDestinatario { get; set; }//11
        public string TipoDittaDestinatario { get; set; }//12
        public string RagioneSocialeDestinazione { get; set; }//13
        public string IndirizzoDestinazione { get; set; }//14
        public string LocalitaDestinazione { get; set; }//15
        public string CAPDestinazione { get; set; }//16
        public string ProvDestinazione { get; set; }//17
        public string NazioneDestinazione { get; set; }//18
        public string CodiceDocumento { get; set; }//19
        public string CodiceSedeOperativa { get; set; }//20
        public string CodiceSedeAmministrativa { get; set; }//21
        public string NumeroDocumento { get; set; }//22
        public string DataDocumento { get; set; }//23-YYYYMMDD
        public string IBAN { get; set; }//24
        public string CodiceValuta { get; set; } //25
        public string Cambio { get; set; }//26
        public string CodicePagamento { get; set; }//27
        public string CodiceAgente { get; set; }//28
        #endregion

        #region Righe
        public string CodiceArticolo { get; set; }//29
        public string DescrizioneArticolo { get; set; }//30
        public string UnitaDiMisura { get; set; } //31
        public string Quantita { get; set; }//32
        public string PrezzoUnitario { get; set; }//33
        public string ScontoRiga { get; set; }//34
        public string CodiceScontoContabileContropartita { get; set; }//35
        public string AliquotaIVA { get; set; }//36
        #endregion

        public override string ToString()
        {
            return $"{RagioneSocialeDestinatario};{IndirizzoDestinatario};{LocalitaDestinatario};{CAPDestinatario};{ProvDestinatario};{NazioneDestinatario};{PartitaIVADestinatario};" +
                $"{CodiceFiscaleDestinatario};{CodiceIPADestinatario};{CodiceFPRDestinatario};{EntePublicoDestinatario};{TipoDittaDestinatario};{RagioneSocialeDestinazione};" +
                $"{IndirizzoDestinazione};{LocalitaDestinazione};{CAPDestinazione};{ProvDestinazione};{NazioneDestinazione};{CodiceDocumento};{CodiceSedeOperativa};{CodiceSedeAmministrativa};" +
                $"{NumeroDocumento};{DataDocumento};{IBAN};{CodiceValuta};{Cambio};{CodicePagamento};{CodiceAgente};{CodiceArticolo};{DescrizioneArticolo};{UnitaDiMisura};{Quantita};" +
                $"{PrezzoUnitario};{ScontoRiga};{CodiceScontoContabileContropartita};{AliquotaIVA}";
        }
    }
}