using CommonAPITypes.ESPRITEC;
using CommonAPITypes.VIVISOL;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using VIVISOL_DOCUMENT_SERVICE.Entity;

namespace VIVISOL_DOCUMENT_SERVICE
{
    public class Automazione
    {
        #region Endpoint & Token
        string userAPIVivisol = "Administrator";//"vivisolSync";
        string passwordAPIVivisol = "admin";//"@Vivi2sol!";
        string token_Espritec = "";
        string EndpointEspritec = EspritecLogin.EspritecLoginEndPointXCM;
        DateTime DataScadenzaToken_XCM = DateTime.Now;
        #endregion

        #region Exceptions
        Exception LastException = new Exception("AVVIO");
        DateTime DateLastException = DateTime.MinValue;
        #endregion

        #region Logger
        internal static Logger _loggerCode = LogManager.GetLogger("loggerCode");
        internal static Logger _loggerAPI = LogManager.GetLogger("LogAPI");
        #endregion

        #region INIT Attributi
        double cicloTimer = 3600000;
        Timer timerAggiornamentoCiclo = new Timer();
        DateTime LastCheckChangesDocuments = DateTime.MinValue;
        string PathLastCheckChangesFileDocuments = "LastCheckChangesDocuments.txt";
        string docSospesi = "IDDocVivisolDaAnalizzare.txt";
        string docTrasmessi = "DocumentiTrasmessi.txt";
        string config = "config.ini";
        List<string> DocumentiGiaTrasmessi = new List<string>();
        List<string> emailNotificaErrori = new List<string>() { "r.ninno@xcmhealthcare.com", "a.gentile@vivisol.it", "a.cerqua@vivisol.it" };
        #endregion

        #region INIT Metodi
        private void CaricaConfigurazioni()
        {
            var conf = File.ReadAllLines(config);
            cicloTimer = double.Parse(conf[5]);
            //DocumentiGiaTrasmessi.Add(File.ReadAllText(docTrasmessi));
        }
        private void SetTimer()
        {
            timerAggiornamentoCiclo = new Timer(cicloTimer);
            timerAggiornamentoCiclo.Elapsed += OnTimedEvent;
            timerAggiornamentoCiclo.AutoReset = true;
            timerAggiornamentoCiclo.Enabled = true;
            _loggerCode.Info($"Timer ciclo settato {timerAggiornamentoCiclo.Interval} ms");
        }
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            timerAggiornamentoCiclo.Stop();

            try
            {

                var dtn = DateTime.Now;
                RecuperaConnessione();
                RecuperaLavoroSuDocumentiDaFile();
                RecuperaLastCheckChangesVivisol();
                RecuperaErrori();
                RecuperaCambiamentiVivisol();
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    Utilities.GestoreMailDev.SegnalaErroreDev("OnTimedEvent", ee);
                }
                LastException = ee;
            }
            finally
            {
                timerAggiornamentoCiclo.Start();
            }
        }
        #endregion

        #region Auth
        private void RecuperaConnessione()
        {
            if ((DateTime.Now + TimeSpan.FromHours(1)) > DataScadenzaToken_XCM)
            {
                _loggerCode.Debug($"Connessione endpoint {EndpointEspritec} in corso");
                EspritecXCMLogin();
            }
        }
        private void EspritecXCMLogin()
        {
            try
            {
                var bb = new Utilities.loginXcmCredentials
                {
                    password = passwordAPIVivisol,
                    username = userAPIVivisol
                };

                var resp = EspritecLogin.RestEspritecLogin(bb, true);
                if (resp != null && resp.Content != null)
                {
                    var rr = JsonConvert.DeserializeObject<EspritecLogin.RootobjectResponseLogin>(resp.Content);
                    DataScadenzaToken_XCM = rr.user.expire;
                    token_Espritec = rr.user.token;

                    _loggerAPI.Debug($"Nuovo token Espritec: {token_Espritec}");
                }
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    Utilities.GestoreMailDev.SegnalaErroreDev("EspritecXCMLogin", ee);
                }
                LastException = ee;
            }
        }
        #endregion

        #region Metodi TopShelf



        public void Start()
        {
            //test();
            CaricaConfigurazioni();
            SetTimer();
            OnTimedEvent(null, null);
        }
        public void Stop()
        {
            timerAggiornamentoCiclo.Stop();
            _loggerCode.Fatal("Servizio fermo!!");
            Utilities.GestoreMailDev.SegnalaErroreDev("Servizio vivisol fermo", "Ricevuto comando di stop!!");
        }
        #endregion     

        #region Gestione Errori
        private void RecuperaErrori()
        {
            _loggerCode.Debug("Recupero elementi in errore in corso...");
            var db = new VisivolSyncEntities();
            var elementiInErrore = db.TRASMISSIONE.Where(x => x.STATO_EVENTO == 1).ToList();

            _loggerCode.Info($"Trovati {elementiInErrore.Count()} elementi in errore");

            foreach (var ele in elementiInErrore)
            {
                bool trasmissioneOK = false;
                string errorMessage = "";
                if (ele.FLUSSO == 1)
                {
                    var raw = JsonConvert.DeserializeObject<VivisolTypes.RootobjectVivisolEntrataMerce>(ele.PAYLOAD);
                    var responseVivisol = VivisolTypes.RestSegnalaEntrataMerce(raw);
                    var desResponse = JsonConvert.DeserializeObject<VivisolTypes.RootobjectVivisolEntrataMerceResponse>(responseVivisol.Content);
                    trasmissioneOK = desResponse.success;
                    errorMessage = desResponse.errorMessage;
                }
                else if (ele.FLUSSO == 4)
                {
                    var raw = JsonConvert.DeserializeObject<VivisolTypes.RootobjectInvioPreparazioneCarico>(ele.PAYLOAD);
                    var responseVivisol = VivisolTypes.RestPreparazioneInCorso(raw);
                    var desResponse = JsonConvert.DeserializeObject<VivisolTypes.RootobjectInvioPreparazioneCaricoResponse>(responseVivisol.Content);
                    trasmissioneOK = desResponse.success;
                    errorMessage = desResponse.errorMessage;
                }
                else if (ele.FLUSSO == 5)
                {
                    var raw = JsonConvert.DeserializeObject<VivisolTypes.RootobjectVivisolInvioMovimenti>(ele.PAYLOAD);
                    var responseVivisol = VivisolTypes.RestSegnalaInvioMovimenti(raw);
                    var desResponse = JsonConvert.DeserializeObject<VivisolTypes.RootobjectVivisolInvioMovimentiResponse>(responseVivisol.Content);
                    trasmissioneOK = desResponse.success;
                    errorMessage = desResponse.errorMessage;
                }

                if (trasmissioneOK)
                {
                    eliminaRecordInErroreDaDB(ele.ID_TRASMISSIONE);
                    //SegnalaErroreTerminatoVivisol(ele.GESPE_DOCNUM, ele.PAYLOAD);
                }
                else
                {
                    aggiornaUltimoTry(ele.ID_TRASMISSIONE, errorMessage);
                }
            }
        }
        private void aggiornaUltimoTry(long idTrasmissione, string errorMessage)
        {
            var db = new VisivolSyncEntities();

            var daAggiornare = db.TRASMISSIONE.First(x => x.ID_DOCUMENTO_GESPE == idTrasmissione);
            //TODO: SE L'ERRORE PERSISTE PER UN TEMPO X (DA RICHIEDERE A VIVISOL) COSA BISOGNA FARE?
            daAggiornare.DATA_ULTIMO_TENTATIVO = DateTime.Now;
            if (daAggiornare.MESSAGGIO_ERRORE != errorMessage)
            {
                daAggiornare.MESSAGGIO_ERRORE = errorMessage;
                //var daAggiornare = db.TRASMISSIONE.FirstOrDefault(x => x.ID_DOCUMENTO_GESPE == idTrasmissione);
                //SegnalaErroreAVivisol(daAggiornare.GESPE_DOCNUM, errorMessage, daAggiornare.PAYLOAD);

                //if (daAggiornare != null)
                //{
                //    daAggiornare.DATA_ULTIMO_TENTATIVO = DateTime.Now;
                //    if (daAggiornare.MESSAGGIO_ERRORE != errorMessage)
                //    {
                //        daAggiornare.MESSAGGIO_ERRORE = errorMessage;
                //        SegnalaErroreAVivisol(daAggiornare.GESPE_DOCNUM, errorMessage, daAggiornare.PAYLOAD);
                //    }
                //    db.SaveChanges();
                //}
            }
        }

        private void eliminaRecordInErroreDaDB(long idTrasmissione)
        {
            var db = new VisivolSyncEntities();
            var daEliminare = db.TRASMISSIONE.FirstOrDefault(x => x.ID_DOCUMENTO_GESPE == idTrasmissione);
            if (daEliminare != null)
            {
                db.TRASMISSIONE.Remove(daEliminare);
                db.SaveChanges();
            }
        }
        private void SegnalaErroreAVivisol(string docNumber, string errorMessage, string payload)
        {
            Utilities.GestoreMailMandanti.SegnalazioneInformaticaVivisol("Errore di comunicazione", $"il documento {docNumber} ha dato errore, non è stato possibile" +
             $"completare la trasmissione al sistema SOL causa seguente errore:\r\n{errorMessage}\r\n\r\nl'elemento in errore ha il seguente payload:\r\n{payload}\r\n\r\nverrà tentanto un nuovo invio tra {TimeSpan.FromMilliseconds(cicloTimer).TotalHours}h", emailNotificaErrori);
        }
        private void SegnalaErroreTerminatoVivisol(string docNumber, string payload)
        {
            Utilities.GestoreMailMandanti.SegnalazioneInformaticaVivisol("RISOLTO: Errore di comunicazione", $"il documento {docNumber} era in errore, ma è stato risolto\r\n" +
             $"l'elemento aveva il seguente payload:\r\n\r\n{payload}", emailNotificaErrori);
        }
        private void SalvaRecordErroreIrreversibileSuDBSync(Exception ee, EspritecDocuments.RootobjectOrder docOsservato)
        {
            var db = new VisivolSyncEntities();
            var esiste = db.TRASMISSIONE.FirstOrDefault(x => x.ID_DOCUMENTO_GESPE == docOsservato.header.id);
            if (esiste == null)
            {
                Utilities.GestoreMailDev.SegnalaErroreDev($"SalvaRecordErroreIrreversibileSuDBSync ID_DOC_GESPE: {docOsservato.header.id}", ee);

                var nr = new TRASMISSIONE()
                {
                    DATA_EVENTO = DateTime.Now,
                    DATA_ULTIMO_TENTATIVO = DateTime.Now,
                    ID_DOCUMENTO_GESPE = docOsservato.header.id,
                    MESSAGGIO_ERRORE = ee.Message,
                    STATO_EVENTO = 99,
                    PAYLOAD = "NON RECUPERABILE",
                    STATO_DOCUMENTO_GESPE = docOsservato.header.statusId,
                    GESPE_DOCNUM = docOsservato.header.docNumber,
                    FLUSSO = 1
                };
                db.TRASMISSIONE.Add(nr);
                db.SaveChanges();
            }
        }
        private void SalvaRecordSuDBSyncPayload(string payload, string errorMessage, EspritecDocuments.RootobjectOrder docOsservato, int flusso)
        {
            var db = new VisivolSyncEntities();
            var esiste = db.TRASMISSIONE.FirstOrDefault(x => x.ID_DOCUMENTO_GESPE == docOsservato.header.id);
            if (esiste == null)
            {
                var nr = new TRASMISSIONE()
                {
                    DATA_EVENTO = DateTime.Now,
                    DATA_ULTIMO_TENTATIVO = DateTime.Now,
                    ID_DOCUMENTO_GESPE = docOsservato.header.id,
                    MESSAGGIO_ERRORE = errorMessage,
                    PAYLOAD = payload,
                    STATO_EVENTO = 1,
                    STATO_DOCUMENTO_GESPE = docOsservato.header.statusId,
                    FLUSSO = flusso,
                    GESPE_DOCNUM = docOsservato.header.docNumber
                };
                db.TRASMISSIONE.Add(nr);
                db.SaveChanges();
            }
            else
            {
                if (errorMessage != esiste.MESSAGGIO_ERRORE)
                {
                    esiste.MESSAGGIO_ERRORE = errorMessage;
                    esiste.DATA_ULTIMO_TENTATIVO = DateTime.Now;
                    SegnalaErroreAVivisol(docOsservato.header.docNumber, errorMessage, payload);
                    db.SaveChanges();
                }

            }

        }
        #endregion

        #region Metodi principali
        private bool InviaAVivisolPreparazioneInCorso(EspritecDocuments.RootobjectOrder docOsservato)
        {
            var raw = new VivisolTypes.RootobjectInvioPreparazioneCarico()
            {
                idRouting = docOsservato.header.info6,
                system = "Sintesi_Italia"
            };
            _loggerCode.Debug($"Preparazione in corso per il documento {docOsservato.header.docNumber} con riferimento {docOsservato.header.reference}");
            var resp = VivisolTypes.RestPreparazioneInCorso(raw);
            var respDes = JsonConvert.DeserializeObject<VivisolTypes.RootobjectInvioPreparazioneCaricoResponse>(resp.Content);


            if (!respDes.success)
            {
                var body = JsonConvert.SerializeObject(raw, Formatting.Indented);
                //segnalare errore e recuperare dopo 1h
                _loggerCode.Error($"Preparazione in errore per il documento {docOsservato.header.docNumber} con riferimento {docOsservato.header.reference}");
                SalvaRecordSuDBSyncPayload(body, respDes.errorMessage, docOsservato, 4);
                SegnalaErroreAVivisol(docOsservato.header.docNumber, respDes.errorMessage, body);
                return false;
            }
            else
            {
                _loggerCode.Debug($"Preparazione in corso comunicata correttamente per il documento {docOsservato.header.docNumber} con riferimento {docOsservato.header.reference}");
                return true;
            }

        }
        private bool ComunicaEntrataMerceVivisol(EspritecDocuments.RootobjectOrder docOsservato)
        {
            //TODO: VERIFICARE CHE NON INVII BEM CHE NON HANNO UN ORF
            if(docOsservato.links is null || !docOsservato.links.First().docNumber.EndsWith("/ORF")) 
            {
                //Non deve inviare
            }
            else if (docOsservato.links.Count() > 1) 
            {
                //TODO: Non so cosa può succedere in questo caso
            }

            try
            {
                if (DocumentiGiaTrasmessi.Contains(docOsservato.header.id.ToString()))
                {
                    _loggerCode.Warn($"Documento già trattato non faccio nulla per la trasmissione del documento {docOsservato.header.docNumber}");
                    Utilities.GestoreMailDev.SegnalaErroreDev($"Documento già trattato", $"non faccio nulla per la trasmissione del documento {docOsservato.header.docNumber}");
                    return true;
                }


                EspritecDocuments.RootobjectEspritecRows RigheDocEspritec = CreaRigheDocumentoAnalizzandoRigheDiRegistrazione(docOsservato);
                if (RigheDocEspritec.rows.Any(x => string.IsNullOrEmpty(x.batchNo)))
                {
                    var resp = EspritecDocuments.RestEspritecRowsListFromDocumentID(docOsservato.header.id, token_Espritec);
                    RigheDocEspritec = JsonConvert.DeserializeObject<EspritecDocuments.RootobjectEspritecRows>(resp.Content);
                }

                EspritecDocuments.RootobjectOrder docOsservatoLink = null;

                if (docOsservato.links != null && docOsservato.links.Count() == 1)
                {
                    var dcLinked = EspritecDocuments.RestEspritecGetDocument(docOsservato.links[0].id, token_Espritec);
                    docOsservatoLink = JsonConvert.DeserializeObject<EspritecDocuments.RootobjectOrder>(dcLinked.Content);
                }
                else if (docOsservato.links != null && docOsservato.links.Count() > 1)
                {
                    throw new Exception("PRESENTI PIU' DOCUMENTI LINKATI PER LA BEM");
                }


                if (RigheDocEspritec != null && RigheDocEspritec.result.status)
                {
                    var RigheDocResponse = EspritecDocuments.RestEspritecRowsListFromDocumentID(docOsservato.header.id, token_Espritec);
                    var RigheDoc = JsonConvert.DeserializeObject<EspritecDocuments.RootobjectEspritecRows>(RigheDocResponse.Content);

                    foreach (var r in RigheDocEspritec.rows)//NON DEVE MAI ACCADERE CHE AGGIUNGANO VOCI MANUALMENTE IN BEM
                    {
                        EspritecDocuments.RowEspritecRows pres = null;
                        string LogMag = "";
                        if (docOsservato.links != null)
                        {
                            pres = RigheDoc.rows.FirstOrDefault(x => x.partNumber == r.partNumber);
                            if (pres != null)
                            {
                                LogMag = pres.info1;
                            }
                        }

                        if (string.IsNullOrEmpty(LogMag))
                        {
                            var rigaCorr = RigheDoc.rows.Where(x => x.partNumber == r.partNumber).ToList();

                            if (rigaCorr != null && rigaCorr.Count() == 1)
                            {
                                r.logWareID = rigaCorr.First().logWareID;
                            }
                            else if (rigaCorr != null)
                            {
                                bool isSameLogwareID = rigaCorr.GroupBy(x => x.logWareID).Count() == 1;
                                if (isSameLogwareID)
                                {
                                    r.logWareID = rigaCorr.First().logWareID;
                                }
                                else
                                {
                                    Utilities.GestoreMailDev.SegnalaErroreDev("Vivisol Magazzino non rilevato", $"id documento in errore {docOsservato.header.id}");
                                }
                            }

                            //var corr = righeLink.rows.FirstOrDefault(x => x.partNumber == r.partNumber);
                            if (string.IsNullOrEmpty(r.logWareID))
                            {
                                Utilities.GestoreMailDev.SegnalaErroreDev("Vivisol Magazzino non rilevato", $"id documento in errore {docOsservato.header.id}");
                                return false;
                            }
                            else if (r.logWareID == "VIVISOLNA")
                            {
                                LogMag = "243270";
                            }
                            else
                            {
                                LogMag = "243310";
                            }
                        }

                        var raw = new VivisolTypes.RootobjectVivisolEntrataMerce()
                        {
                            batch = r.batchNo,
                            expiryDate = r.expireDate.Value.ToUniversalTime().ToString("o"),
                            logWareId = LogMag,
                            movementDate = DateTime.Now.ToUniversalTime().ToString("o"),
                            partNumber = r.partNumber,
                            quantity = r.qty,
                            orderId = (docOsservatoLink != null) ? docOsservatoLink.header.externalID : "",
                            orderDetailId = (pres != null) ? pres.linkedExternalID : "",
                            note = ""
                        };

#if DEBUG
                        var desResponse = new VivisolTypes.RootobjectVivisolEntrataMerceResponse()
                        {
                            errorMessage = "test",
                            success = false
                        };
#else
                        var responseVivisol = VivisolTypes.RestSegnalaEntrataMerce(raw);
                        var desResponse = JsonConvert.DeserializeObject<VivisolTypes.RootobjectVivisolEntrataMerceResponse>(responseVivisol.Content);
#endif
                        if (!desResponse.success)
                        {
                            //TODO: LEGGERE LA QUANTITA' MASSIMA DELL'ORDINE DALLA RESPONSE
                            //REINVIARE IL PAYLOAD CON LA QUANTITA' SEGNALATA DALLA RESPOSNSE
                            //COMUNICARE IL DELTA AI CS XCM E CS VIVISOL PER RICHIESTA INTEGRAZIONE ORF DELLA QUANTITA' AGGIUNTIVA
                            var body = JsonConvert.SerializeObject(raw, Formatting.Indented);
                            //segnalare motivo e riprovare dopo un tot tempo inserendo l'informazione in una tabella temporanea db
                            SalvaRecordSuDBSyncPayload(body, desResponse.errorMessage, docOsservato, 1);
                            return false;
                        }
                        else
                        {
                            if (!DocumentiGiaTrasmessi.Contains(docOsservato.header.id.ToString()))
                            {
                                DocumentiGiaTrasmessi.Add(docOsservato.header.id.ToString());
                                File.AppendAllText(docTrasmessi, docOsservato.header.id.ToString() + "\r\n");
                            }
                            return true;
                        }
                    }
                }
                else
                {
                    string msg = $"Non sono state trovate righe di registrazione per il documento {docOsservato.header}";
                    _loggerAPI.Warn(msg);
                    throw new Exception(msg);
                }


                return true;
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                SalvaRecordErroreIrreversibileSuDBSync(ee, docOsservato);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    Utilities.GestoreMailDev.SegnalaErroreDev("ComunicazioneAPIVivisol", ee);
                }
                LastException = ee;
                return false;
            }
        }
        private bool ComunicaEvasionePianificaVivisol(EspritecDocuments.RootobjectOrder docOsservato)
        {
            try
            {
                EspritecDocuments.RootobjectOrder docOsservatoLink = null;

                var rd = EspritecDocuments.RestEspritecRowsListFromDocumentID(docOsservato.header.id, token_Espritec);
                EspritecDocuments.RootobjectEspritecRows RigheDoc = JsonConvert.DeserializeObject<EspritecDocuments.RootobjectEspritecRows>(rd.Content);

                var shipConn = EspritecShipment.RestEspritecGetShipment(docOsservato.header.shipID, token_Espritec);
                EspritecShipment.RootobjectEspritecShipment shipConnesso = JsonConvert.DeserializeObject<EspritecShipment.RootobjectEspritecShipment>(shipConn.Content);

                if (docOsservato.links != null && docOsservato.links.Count() == 1)
                {
                    var dcLinked = EspritecDocuments.RestEspritecGetDocument(docOsservato.links[0].id, token_Espritec);
                    docOsservatoLink = JsonConvert.DeserializeObject<EspritecDocuments.RootobjectOrder>(dcLinked.Content);
                }
                else if (docOsservato.links != null && docOsservato.links.Count() > 1)
                {
                    var collegatoDiretto = docOsservato.links.Where(x => x.docType == "OrderOUT");
                    if (collegatoDiretto.Count() > 1)
                    {
                        throw new Exception("PRESENTI PIU' DOCUMENTI LINKATI AL DDT");
                    }
                }
                else
                {
                    throw new Exception("NON E' PRESENTE L'ORM LEGATO AL DDT!!!");
                }
                var db = new GnXcmEntities();
                var testataDocumentoLinked = db.uvwWmsDocument.FirstOrDefault(x => x.uniq == docOsservatoLink.header.id);

                if (testataDocumentoLinked == null)
                {
                    throw new Exception($"Ordine di preparazione {docOsservato.header.docNumber} con id {docOsservato.header.id} non trovato nel DB!!");
                }

                if (string.IsNullOrEmpty(testataDocumentoLinked.ExternalID))
                {
                    throw new Exception($"ExternalID nel documento {testataDocumentoLinked.DocNum} con id {testataDocumentoLinked.uniq} non trovato");
                }
                if (RigheDoc.result.status)
                {
                    foreach (var record in RigheDoc.rows)
                    {
                        string LogMag = "243270";
                        //if (record.logWareID == "VIVISOLNA")
                        //{
                        //    LogMag = "243270";
                        //}
                        //else
                        //{
                        //    LogMag = "243310";
                        //}

                        var dataDocO = docOsservato.header.docDate.ToUniversalTime().ToString("o");//docOsservato.header.docDate;

                        var numC = 0M;

                        if (shipConnesso != null && shipConnesso.shipment.packs != 0)
                        {
                            numC = shipConnesso.shipment.packs;
                        }
                        else
                        {
                            throw new Exception($"Non sono riuscito a recuperare lo ship connesso al DDT ");
                        }

                        var raw = new VivisolTypes.RootobjectVivisolInvioMovimenti()
                        {
                            batch = record.batchNo,
                            idPianifica = testataDocumentoLinked.Info6,
                            idRouting = testataDocumentoLinked.ExternalID,
                            idUtente = testataDocumentoLinked.Info3,
                            logWareId = LogMag,
                            movementDate = dataDocO,
                            note = "",//record.info1,
                            numColli = (double)numC,
                            partNumber = record.partNumber,
                            quantity = record.qty,
                            pallet = DammiBarCodePallet(testataDocumentoLinked.uniq.Value),//(docOsservato.header.id),
                            system = "Sintesi_Italia"
                        };

#if yDEBUG
                        var desResp = new VivisolTypes.RootobjectVivisolInvioMovimentiResponse()
                        {
                            errorMessage = "test",
                            success = false
                        };
#else
                        var respEntrata = VivisolTypes.RestSegnalaInvioMovimenti(raw);
                        _loggerAPI.Info(respEntrata.Content);
                        var desResp = JsonConvert.DeserializeObject<VivisolTypes.RootobjectVivisolEntrataMerceResponse>(respEntrata.Content);
#endif
                        if (!desResp.success)
                        {
                            //TODO: VERIFICA SE ERRORE DI QUANTITA DISPONIBILE NEL MAGAZZINO VIVISOL
                            //INSERIRE NEL MAGAZZINO VIVISOL LE QTY PER EVADERE LA RIGA D'ORDINE RIMANDA L'EVASIONE DELLA RIGA
                            //PER CORTESIA SI SEGNALA IL DELTA ALLE MAIL DEL CUSTOMER XCM E CUSTOMER VIVISOL
                            var body = JsonConvert.SerializeObject(raw, Formatting.Indented);
                            //avvisa dell'errore e riprova dopo 1h
                            SalvaRecordSuDBSyncPayload(body, desResp.errorMessage, docOsservato, 5);
                            return false;
                        }
                    }
                }
                else
                {
                    throw new Exception($"Righe documento non trovate su Espritec!!! id documento {docOsservato.header.id}");
                }

                return true;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        #endregion

        #region Utility
        private void RecuperaLavoroSuDocumentiDaFile()
        {
            if (File.Exists(docSospesi))
            {
                var idDaVerificare = File.ReadAllLines(docSospesi).ToList();
                _loggerCode.Debug($"RecuperaLavoroSuDocumentiDaFile in corso...");
                foreach (var id in idDaVerificare)
                {
                    _loggerCode.Info($"Recupero documento con id Gespe {id}");
                    var doc = EspritecDocuments.RestEspritecGetDocument(int.Parse(id), token_Espritec);
                    if (doc != null && !string.IsNullOrEmpty(doc.Content))
                    {
                        var docOsservato = JsonConvert.DeserializeObject<EspritecDocuments.RootobjectOrder>(doc.Content);
                        if (docOsservato != null && docOsservato.header == null) continue;
                        VerificaCambiamentoDocumento(docOsservato);
                    }
                    else
                    {
                        Utilities.GestoreMailDev.SegnalaErroreDev($"Errore documento non trovato {id}", $"documento con id {id} non trovato in gespe");
                        _loggerCode.Error($"documento con id {id} non trovato in gespe");
                    }
                }
                File.WriteAllText(docSospesi, "");
            }
        }
        private string DammiBarCodePallet(int ordID)
        {
            string idString = ordID.ToString();

            var suffisso = $"XCM{DateTime.Now.ToString("yy")}";
            while (idString.Length < 7)
            {
                idString = idString.Insert(0, "0");
            }
            return $"{suffisso}{idString}";
        }
        private bool PopolaIDatiMancantiPianificaVivisol(EspritecDocuments.RootobjectOrder docOsservato)
        {
            _loggerCode.Debug($"Nuova pianifica arrivata {docOsservato.header.docNumber}");
            //RECUPERARE LE RIGHE, SE E' PRESENTE UNA RIGA ASL ASSEGNARE IL DOCUMENTO AL MAGAZZINO ASL
            var db = new GnXcmEntities();
            bool DocAsl = db.uvwWmsDocumentRows_XCM.Where(x => x.uniq == docOsservato.header.id).Any(x => x.LogWareID == "ASLNANORD");
            string mag = "";
            if (DocAsl)
            {
                mag = "ASLNANORD";
            }
            else
            {
                mag = "VIVISOLNA";
            }

            var raw = new EspritecDocuments.RootobjectEspritecUpdateDocument()
            {
                header = new EspritecDocuments.EspritecHeaderUpdateDocument()
                {
                    id = docOsservato.header.id,
                    logWareID = mag,
                    consignee = new EspritecDocuments.EspritecConsigneeUpdateDocument()
                    {
                        address = docOsservato.header.unloadAddress,
                        country = docOsservato.header.unloadCountry,
                        description = docOsservato.header.unLoadDes,
                        district = docOsservato.header.unloadDistrict,
                        location = docOsservato.header.unloadLocation,
                        region = docOsservato.header.unloadRegion,
                        zipCode = docOsservato.header.unloadZipCode,
                    }
                }
            };

            var resp = EspritecDocuments.RestEspritecUpdateDocument(raw, token_Espritec);
            if (!resp.IsSuccessful)
            {
                throw new Exception($"Errore nell'aggiornamento del documento con i paramentri mancanti {docOsservato.header.id} {docOsservato.header.id}");
            }
            return true;
        }
        private void ScriviLastCheckChangesDocuments(bool append)
        {
            if (!append)
            {
                File.WriteAllText(PathLastCheckChangesFileDocuments, LastCheckChangesDocuments.ToString());
            }
            else
            {
                File.AppendAllText(PathLastCheckChangesFileDocuments, "\r\n" + LastCheckChangesDocuments.ToString());
            }
        }
        private void RecuperaLastCheckChangesVivisol()
        {
            LastCheckChangesDocuments = DateTime.Parse(File.ReadAllLines(PathLastCheckChangesFileDocuments)[0]);
        }
        private EspritecDocuments.RootobjectEspritecRows CreaRigheDocumentoAnalizzandoRigheDiRegistrazione(EspritecDocuments.RootobjectOrder docOsservato)
        {

            var RegistrazioniDocumentoEspritec = EspritecDocuments.RestEspritecGetRegistrations(docOsservato.header.id, token_Espritec);
            var xcmRec = JsonConvert.DeserializeObject<EspritecDocuments.RootobjectEspritecRegistrations>(RegistrazioniDocumentoEspritec.Content);

            if (xcmRec.result.status)
            {
                var nuoveRighe = new List<EspritecDocuments.RowEspritecRows>();
                foreach (var r in xcmRec.registrations)
                {
                    if (r.totalQty < 1) continue;
                    var nrr = new EspritecDocuments.RowEspritecRows()
                    {
                        partNumber = r.partNumber,
                        batchNo = r.batchNo,
                        expireDate = r.dateExpire,
                        id = r.id,
                        qty = r.totalQty,
                        partNumberDes = r.partNumberDescription,

                    };

                    var giaPresente = nuoveRighe.FirstOrDefault(x => x.partNumber == nrr.partNumber && x.batchNo == nrr.batchNo);
                    if (giaPresente != null)
                    {
                        giaPresente.qty += nrr.qty;
                    }
                    else
                    {
                        nuoveRighe.Add(nrr);
                    }

                }
                var xcmDoc = new EspritecDocuments.RootobjectEspritecRows();
                xcmDoc.rows = nuoveRighe.ToArray();
                xcmDoc.result = new EspritecDocuments.ResultEspritecRows()
                {
                    status = true,
                };
                return xcmDoc;
            }
            else
            {
                var resp = new EspritecDocuments.RootobjectEspritecRows()
                {
                    result = new EspritecDocuments.ResultEspritecRows()
                    {
                        status = false
                    }
                };
                return resp;
            }
        }
        #endregion

        #region Analisi
        private void RecuperaCambiamentiVivisol()
        {
            try
            {
                _loggerCode.Info("Recupero Cambi WMS da API");

                string ts = LastCheckChangesDocuments.ToString("s", CultureInfo.InvariantCulture);
                var resp = EspritecDocuments.RestEspritecDocumentsChanged(ts, token_Espritec);
                if (resp == null || !resp.IsSuccessful)
                {
                    //Chiamata endpoint fallita per qualche motivo, ci riprova al prossimo passaggio
                    throw new Exception("VIVISOL_DOCUMENT_SERVICE - Cambio documenti vivisol non riuscito");
                }
                LastCheckChangesDocuments = DateTime.Now;
                ScriviLastCheckChangesDocuments(true);

                var DocTrack = JsonConvert.DeserializeObject<EspritecDocuments.RootobjectTrackingDocument>(resp.Content);
                if (DocTrack.result.status == true)
                {
                    var dttt = DocTrack.trackings;
                    _loggerCode.Info($"Trovati {dttt.Count()} cambi");
                    foreach (var dt in dttt)
                    {
                        try
                        {
                            _loggerCode.Debug($"Analisi del documento {dt.docNumber}");
                            if (dt.docNumber.EndsWith("/INV"))
                            {
                                continue;
                            }

                            var doc = EspritecDocuments.RestEspritecGetDocument(dt.docID, token_Espritec);
                            if (doc != null && !string.IsNullOrEmpty(doc.Content))
                            {
                                var docOsservato = JsonConvert.DeserializeObject<EspritecDocuments.RootobjectOrder>(doc.Content);
                                if (docOsservato != null && docOsservato.header == null) continue;
                                VerificaCambiamentoDocumento(docOsservato);
                            }
                            else
                            {
                                throw new Exception($"Errore recupero documento in GET dalle API Espritec {dt.docNumber}");
                            }
                        }
                        catch (Exception ee)
                        {
                            string msg = $"Errore durante l'analisi del documento {dt.docNumber}";
                            _loggerCode.Error(msg);
                            _loggerCode.Error(ee);
                            Utilities.GestoreMailDev.SegnalaErroreDev(msg, ee);
                        }
                    }
                }

                int Cambiamenti = 0;
                if (DocTrack.trackings != null)
                {
                    Cambiamenti = DocTrack.trackings.Count();
                }

                _loggerCode.Info($"LastTimeCheckWMS: {LastCheckChangesDocuments.ToString("dd/MM/yyyy HH:mm:ss")} - Cambi recuperati {Cambiamenti}");

                ScriviLastCheckChangesDocuments(false);
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    Utilities.GestoreMailDev.SegnalaErroreDev("CambiWMS", ee);
                }

                LastException = ee;
            }
        }
        private void VerificaCambiamentoDocumento(EspritecDocuments.RootobjectOrder docOsservato)
        {
            int StatusID = docOsservato.header.statusId;
            switch (docOsservato.header.docType)
            {
                case "DeliveryIN":
                    if (StatusID == 31) //BEM IN INGRESSO TERMINATA
                    {
                        _loggerCode.Debug($"BEM IN INGRESSO TERMINATA {docOsservato.header.docNumber}");
                        ComunicaEntrataMerceVivisol(docOsservato);

                    }
                    break;
                case "DeliveryOUT":
                    if (StatusID == 20) //DDT CREATO
                    {
                        _loggerCode.Debug($"DDT CREATO {docOsservato.header.docNumber}");
                        ComunicaEvasionePianificaVivisol(docOsservato);

                    }
                    break;
                case "OrderIN":
                    break;
                case "OrderOUT":
                    if (StatusID == 10)
                    {
                        _loggerCode.Debug($"Trovata nuova plan {docOsservato.header.docNumber}");
                        PopolaIDatiMancantiPianificaVivisol(docOsservato);

                    }
                    else if (StatusID == 30)
                    {
                        _loggerCode.Debug($"{docOsservato.header.docNumber} in stato 30");
                        InviaAVivisolPreparazioneInCorso(docOsservato);

                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
