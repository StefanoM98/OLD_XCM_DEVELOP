using DevExpress.Spreadsheet;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Timers;
using XCM_DOCUMENT_SERVICE.APS;
using XCM_DOCUMENT_SERVICE.EntityModels;
using XCM_DOCUMENT_SERVICE.CodeReport;
using System.Data.Entity.Validation;
using System.Threading;
using CommonAPITypes.ESPRITEC;

//Claudio using GestFile Aggiunto il 08-11-2023
using GestFile;

namespace XCM_DOCUMENT_SERVICE
{
    public class Automazione
    {
        GestFile.LogFile LOG = new LogFile();

        #region Credenziali Vivisol test
        //readonly string VivisolUsrAPI = "2SsHrmgcaTUmJWqAG63K";
        //readonly string VivisolPswAPI = "VivisolPortals!20";
        //readonly string VivisolEndPoint = "https://sandbox.solgroup.com/test";
        //List<string> BEMVivisolInviate = new List<string>();
        #endregion

        #region Attributi Generali

        public static string endpointAPI_Espritec = "https://192.168.2.254:9500";
        public static string userAPIAmministrativaEspritex = "Administrator";
        public static string passwordAPIAmministrativaEspritec = "admin";
        public static string endpointAPI_UNITEX = "https://010761.espritec.cloud:9500";
        public static string userAPIUNITEX = "xcm";
        public static string passwordAPIUNITEX = "Xcm@2022";
        //public static string userAPIUNITEX = "dvalitutti";
        //public static string passwordAPIUNITEX = "Dv$2022!";


        static string quote = "\"";
        Exception LastException = new Exception("AVVIO");
        DateTime DateLastException = DateTime.MinValue;
        DateTime LastCheckChangesWMS = DateTime.MinValue;
        DateTime LastCheckChangesTMS = DateTime.MinValue;
        string PathLastCheckChangesFileWMS = "LastCheckChangesWMS.txt";
        string PathLastCheckChangesFileTMS = "LastCheckChangesTMS.txt";
        string PathTemplateMovMag = "Template_Mov_mag.xlsx";
        string PathTemplateMovMagRaggruppataPerQuantita = "Template_Mov_magRaggruppataPerQuantita.xlsx";
        string PathTemplateGiacenzeMag = "Template_Report_Excel.xlsx";
        public static string WorkPath = @"C:\XCM\documenti_prodotti";
        public static string docSospesi = "IDDocDaAnalizzare.txt";
        public static string config = "config.ini";
        double cicloTimer = 600000;
        #endregion

        #region CRMSyncro
        bool daSincronizzareCRM = true;
        List<Customer> UtentiCRM = new List<Customer>();
        #endregion

        #region APS_DAFNE
        bool daNotificareAPS_Dafne = true;
        string PathFileSingoliDafne = @"C:\FTP\APS\OUT\DAFNE\SINGOLI";
        bool daNotificareAPS_DDT = true;
        string PathFileSingoliDDT = @"C:\FTP\APS\OUT\DDT\SINGOLI";
        #endregion

        #region Token
        DateTime DataScadenzaToken_XCM = DateTime.Now;
        string token_XCM = "";
        DateTime DataScadenzaToken_UNITEX = DateTime.Now;
        string token_UNITEX = "";
        DateTime DataScadenzaToken_APIXCM = DateTime.Now;
        string token_APIXCM = "";
        #endregion

        #region Logger
        internal static Logger _loggerCode = LogManager.GetLogger("loggerCode");
        internal static Logger _loggerAPI = LogManager.GetLogger("LogAPI");
        internal static Logger _loggerTracking = LogManager.GetLogger("LogTracking");
        internal static Logger _loggerErroriVivisolAPI = LogManager.GetLogger("LogErroriVivisolAPI");
        #endregion

        #region LoginXCMApi
        private void XcmLogin()
        {
            try
            {
                _loggerCode.Info($"Autenticazione in corso su endpoint {endpointAPI_Espritec}");
                var client = new RestClient(endpointAPI_Espritec + "/api/token");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json-patch+json");
                request.AddHeader("Cache-Control", "no-cache");
                var body = @"{" + "\n" +
                $@"  ""username"": ""{userAPIAmministrativaEspritex}""," + "\n" +
                $@"  ""password"": ""{passwordAPIAmministrativaEspritec}""," + "\n" +
                @"  ""tenant"": """"" + "\n" +
                @"}";
                request.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse response = client.Execute(request);
                var resp = JsonConvert.DeserializeObject<RootobjectLoginXCM>(response.Content);

                DataScadenzaToken_XCM = resp.user.expire;
                token_XCM = resp.user.token;

                _loggerAPI.Info($"Nuovo token XCM: {token_XCM}");

            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("XcmLogin", ee);
                }
                LastException = ee;
            }
        }
        private void UNITEXLogin()
        {
            try
            {
                _loggerCode.Info($"Autenticazione in corso su endpoint {endpointAPI_UNITEX}");
                var client = new RestClient(endpointAPI_UNITEX + "/api/token");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json-patch+json");
                request.AddHeader("Cache-Control", "no-cache");
                var body = @"{" + "\n" +
                $@"  ""username"": ""{userAPIUNITEX}""," + "\n" +
                $@"  ""password"": ""{passwordAPIUNITEX}""," + "\n" +
                @"  ""tenant"": ""UNITEX""" + "\n" +
                @"}";
                request.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse response = client.Execute(request);
                var resp = JsonConvert.DeserializeObject<RootobjectLoginXCM>(response.Content);

                DataScadenzaToken_UNITEX = resp.user.expire;
                token_UNITEX = resp.user.token;

                _loggerAPI.Info($"Nuovo token UNITEX: {token_UNITEX}");

            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("XcmLogin", ee);
                }
                LastException = ee;
            }
        }
        private void RecuperaConnessione()
        {
            if ((DateTime.Now + TimeSpan.FromHours(1)) > DataScadenzaToken_XCM)
            {
                XcmLogin();
            }
            if ((DateTime.Now + TimeSpan.FromHours(1)) > DataScadenzaToken_UNITEX)
            {
                UNITEXLogin();
            }

            //if ((DateTime.Now + TimeSpan.FromHours(1)) > DataScadenzaToken_APIXCM)
            //{
            //    APIXCMLogin();
            //}

        }
        #endregion

        #region TopShelf
        public void Start()
        {
            LOG.ScriviLogManuale("START");

            _loggerCode.Info($"Avvio servizio - connesso all'endpoint {endpointAPI_Espritec}");
            _loggerCode.Info($"Servizio Mail verso l'esterno abilitato = {GestoreMail.InvioEsternoAbilitato}");

            CaricaConfigurazioni();
            
            SetTimer(); 

            OnTimedEvent(null, null);
            OnTimedEventDigiPharm(null, null);

            //OnTimedEventCleanDriveTrashBin(null, null); //Era cos'ì
        }
        private void CaricaConfigurazioni()
        {
            var conf = File.ReadAllLines(config);
            cicloTimer = double.Parse(conf[5]);
        }
        public void Stop()
        {
            timerAggiornamentoCiclo.Stop();
        }
        #endregion

        #region Timers
        System.Timers.Timer timerAggiornamentoCiclo = new System.Timers.Timer();
        System.Timers.Timer timerAggiornamentoGiacenzeDigiPharm = new System.Timers.Timer();
        System.Timers.Timer timerAggiornamentoCleanDriveTrashBin = new System.Timers.Timer();
        //System.Timers.Timer timerAggiornamentoSyncroDamor = new System.Timers.Timer();
        private void SetTimer()
        {
            timerAggiornamentoCiclo = new System.Timers.Timer(cicloTimer);
            timerAggiornamentoCiclo.Elapsed += OnTimedEvent;
            timerAggiornamentoCiclo.AutoReset = true;
            timerAggiornamentoCiclo.Enabled = true;
            _loggerCode.Info($"Timer ciclo settato {timerAggiornamentoCiclo.Interval} ms");

            timerAggiornamentoGiacenzeDigiPharm = new System.Timers.Timer(3600000);
            timerAggiornamentoGiacenzeDigiPharm.Elapsed += OnTimedEventDigiPharm;
            timerAggiornamentoGiacenzeDigiPharm.AutoReset = true;
            timerAggiornamentoGiacenzeDigiPharm.Enabled = true;
            _loggerCode.Info($"Timer digipharm settato {timerAggiornamentoGiacenzeDigiPharm.Interval} ms");

            timerAggiornamentoCleanDriveTrashBin = new System.Timers.Timer(86400000);
            timerAggiornamentoCleanDriveTrashBin.Elapsed += OnTimedEventCleanDriveTrashBin;
            timerAggiornamentoCleanDriveTrashBin.AutoReset = true;
            timerAggiornamentoCleanDriveTrashBin.Enabled = true;
            _loggerCode.Info($"Timer CleanDriveTrashBin settato {timerAggiornamentoCleanDriveTrashBin.Interval} ms");
        }

        private void OnTimedEventCleanDriveTrashBin(object sender, ElapsedEventArgs e)
        {
            //autenticati con google
            //Google.Apis.Auth.OAuth2
            //svuota il cestino
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            timerAggiornamentoCiclo.Stop();

            try
            {
                RecuperaConnessione();
                RecuperaClientiCRM();

                RecuperaLavoroSuDocumentiDaFile();

                RecuperaLastCheckChangesWMS();
                RecuperaLastCheckChangesTMS();
                VerificaMessaggiEDI();
                RecuperaCambiamenti();
                

                var dtn = DateTime.Now;
                

                _loggerCode.Info("START AnalizzaInvioAPS");
                AnalizzaInvioAPS(dtn);
                _loggerCode.Info("STOP AnalizzaInvioAPS");
               
                #region Movimenti e giacenze
                if (dtn.Day == 1)
                {
                    _loggerCode.Info("G1 - InviaMovimentiMagazzinoEGiacenze(true)");
                    InviaMovimentiMagazzinoEGiacenze(true);
                }
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    _loggerCode.Info("DOMENICA - InviaMovimentiMagazzinoEGiacenze(true)");
                    InviaMovimentiMagazzinoEGiacenze(false);
                }
                #endregion

                #region SyncroCRM
                //if (daSincronizzareCRM && dtn.Hour > 1 && dtn.Hour < 2)
                //{
                //    SincronizzaDBCRM();
                //    daSincronizzareCRM = false;
                //}
                //else if (dtn.Hour > 2 && !daSincronizzareCRM)
                //{
                //    daSincronizzareCRM = true;
                //}
                #endregion
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("OnTimedEvent", ee);
                }
                LastException = ee;
            }
            finally
            {
                timerAggiornamentoCiclo.Start();
            }
        }

        #region Digi-Pharm
        private void OnTimedEventDigiPharm(object sender, ElapsedEventArgs e)
        {
            //recupera giacenze digipharm
            timerAggiornamentoGiacenzeDigiPharm.Stop();

            try
            {
                RecuperaConnessione();
                var db = new GnXcmEntities();

                var stockTotal = db.uvwWmsWarehouse.Where(x => x.CustomerID == "00025" && x.ItemStatus == 10).ToList();
                if (stockTotal != null && stockTotal.Count() > 0)
                {
                    //raggruppa referenze ed elimina campioni
                    var giacenzaRaggruppata = RaggruppaPerQuantita(stockTotal);

                    //scrivi file giacenze
                    List<string> GiacenzeDigiPharm = ConvertiInFormatoDigiPharm(giacenzaRaggruppata);

                    File.WriteAllLines(@"C:\FTP\DIGIPHARM\STOCK\stock.csv", GiacenzeDigiPharm);
                }
                else
                {
                    throw new Exception("OnTimedEventDigiPharm lo stock non esiste!");
                }
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("OnTimedEventDigiPharm", ee);
                }

                LastException = ee;
            }
            finally
            {
                timerAggiornamentoGiacenzeDigiPharm.Start();
            }

        }
        private List<string> ConvertiInFormatoDigiPharm(List<uvwWmsWarehouse> giacenzePulite)
        {
            List<string> resp = new List<string>();

            foreach (var gp in giacenzePulite)
            {
                resp.Add($"{gp.PrdCod},{gp.TotalQty.Value.ToString("0")}");
            }

            return resp;
        }
        private List<uvwWmsWarehouse> RaggruppaPerQuantita(List<uvwWmsWarehouse> stockDes)
        {
            var resp = new List<uvwWmsWarehouse>();

            foreach (var stock in stockDes)
            {
                if (stock.PrdCod.EndsWith("/C")) continue;
                else if (stock.PrdCod.EndsWith("/C10")) continue;
                else if (stock.PrdCod.EndsWith("/CP")) continue;

                var esiste = resp.FirstOrDefault(x => x.PrdCod == stock.PrdCod);
                if (esiste != null)
                {
                    esiste.TotalQty += stock.TotalQty;
                }
                else
                {
                    resp.Add(stock);
                }
            }
            return resp;
        }
        #endregion

        private void test()
        {
            var db = new GnXcmEntities();
            var OrdineApi = CommonAPITypes.ESPRITEC.EspritecDocuments.RestEspritecGetDocument(136153, token_XCM);
            var ordineDes = JsonConvert.DeserializeObject<CommonAPITypes.ESPRITEC.EspritecDocuments.RootobjectOrder>(OrdineApi.Content);
            string numDDT = "";
            if (ordineDes.links.Count() == 1)
            {
                var ddtAPI = CommonAPITypes.ESPRITEC.EspritecDocuments.RestEspritecGetDocument(ordineDes.links[0].id, token_XCM);
                var ddtAPIdes = JsonConvert.DeserializeObject<CommonAPITypes.ESPRITEC.EspritecDocuments.RootobjectOrder>(ddtAPI.Content);

                numDDT = db.uvwWmsDocument.FirstOrDefault(x => x.uniq == ddtAPIdes.header.id).DocNum2;

            }
            else
            {
                throw new Exception("DDT dell'ordine non trovato");
            }
        }

        private void RecuperaClientiCRM()
        {
            UtentiCRM = new XCM_CRMEntities().Customer.Where(x => x.Customer_IsEnableCRM).ToList();
        }
        private void SincronizzaDBCRM()
        {
            SincronizzaOrdini();
            SincronizzaAnagraficheProdotti();
            SincronizzaSpedizioni();

            //LOCALITA DI CARICO E SCARICO - info dati db espritec
            //SincronizzaLocalitaCaricoScarico();
        }
        private void SincronizzaOrdini()
        {
            var dtn = DateTime.Now;
            //RECUPERA TUTTI GLI ORDINI CREATI E AGGIORNATI FINO A 3 GIORNI
            var dbGespe = new GnXcmEntities();
            var dbCRM = new XCM_CRMEntities();
            var dataDaOrd = dtn - TimeSpan.FromDays(5);
            var allNewDocs = dbGespe.uvwWmsDocument.Where(x => x.DocTip == 203 && (x.RecCreate > dataDaOrd || x.RecChange > dataDaOrd)).ToList();
            try
            {
                foreach (var nD in allNewDocs)
                {
                    if (UtentiCRM.Any(x => x.Customer_id == nD.CustomerID))
                    {
                        var esiste = dbCRM.Orders.FirstOrDefault(x => x.Orders_GespeUniq == nD.uniq);
                        if (esiste == null)
                        {
                            var dbCRMSave = new XCM_CRMEntities();
                            dbCRMSave.Orders.Add(ConvertiOrdineEspritecInOrdineCRM(nD));
                            dbCRMSave.SaveChanges();
                        }
                        else //if (esiste.Orders_statusId != nD.ItemStatus)
                        {
                            var dbCRMSave = new XCM_CRMEntities();
                            var daCambiare = dbCRMSave.Orders.FirstOrDefault(x => x.Orders_GespeUniq == nD.uniq);
                            aggiornaOrdineCRMdaDocumentoEspritec(daCambiare, nD);
                            dbCRMSave.SaveChanges();
                        }
                    }
                }
            }
            catch (DbEntityValidationException rr)
            {

            }
            catch (Exception ee)
            {

            }
        }
        private void SincronizzaAnagraficheProdotti()
        {
            var dbEspritec = new GnXcmEntities();
            var dbCRM = new XCM_CRMEntities();
            var dtDa = DateTime.Now - TimeSpan.FromDays(360);

            var AnaPrdMod = dbEspritec.ViewPrdCRM.Where(x => (x.RecChange >= dtDa || x.RecCreate >= dtDa) && x.CustomerID == "00012").ToList();
            decimal? przNull = null;

            try
            {
                foreach (var prd in AnaPrdMod)
                {
                    if (string.IsNullOrEmpty(prd.PrdDes))
                    {
                        continue;
                    }

                    var esiste = dbCRM.Anagrafica_Prodotti.FirstOrDefault(x => x.CODICE_PRODOTTO == prd.PrdCod && x.GESPE_CUSTOMERID == prd.CustomerID);
                    if (esiste != null)
                    {
                        var db2 = new XCM_CRMEntities();
                        var daAggiornare = db2.Anagrafica_Prodotti.First(x => x.CODICE_PRODOTTO == esiste.CODICE_PRODOTTO && x.GESPE_CUSTOMERID == prd.CustomerID);
                        //daAggiornare.PREZZO_UNITARIO = (prd.PrzMin != null) ? prd.PrzMin.Value : przNull;
                        //daAggiornare.DESCRIZIONE_PRODOTTO = prd.PrdDes;
                        //daAggiornare.DATA_ULTIMA_MODIFICA = DateTime.Now;
                        //db2.Anagrafica_Prodotti.Add(daAggiornare);
                        //db2.SaveChanges();
                    }
                    else
                    {
                        if (prd.PrdDes.Contains("OBSOLETO"))
                        {
                            continue;
                        }
                        var db2 = new XCM_CRMEntities();
                        var daAggiungere = new Anagrafica_Prodotti()
                        {
                            CODICE_PRODOTTO = prd.PrdCod,
                            DATA_CREAZIONE = DateTime.Now,
                            DATA_ULTIMA_MODIFICA = DateTime.Now,
                            GESPE_CUSTOMERID = prd.CustomerID,
                            DESCRIZIONE_PRODOTTO = prd.PrdDes,
                            PREZZO_UNITARIO = (prd.PrzMin != null) ? prd.PrzMin.Value : przNull,
                        };
                        db2.Anagrafica_Prodotti.Add(daAggiungere);
                        db2.SaveChanges();
                    }
                }
            }
            catch (Exception ee)
            {

            }
        }
        private void SincronizzaLocalitaCaricoScarico()
        {
            var dbEspritec = new GnXcmEntities();
            var dbCrm = new XCM_CRMEntities();

            var locationDaVerificare = dbCrm.Client_Location.Where(x => x.Location_GespeLocationId == 0).ToList();
            try
            {
                foreach (var loc in locationDaVerificare)
                {
                    var corrEsatto = dbEspritec.TmsLocationDetail.FirstOrDefault(x => x.Address.ToLower() == loc.Location_Address.ToLower());

                    if (corrEsatto != null)
                    {
                        var dbCrm2 = new XCM_CRMEntities();
                        dbCrm2.Client_Location.First(x => x.Location_Id == loc.Location_Id).Location_GespeLocationId = corrEsatto.LocationID;
                        dbCrm2.SaveChanges();
                    }
                }
            }
            catch (Exception ee)
            {


            }
        }
        private List<RootobjectShipment> RecuperaShipmentsByAPI_UNITEX(DateTime dataDaShip)
        {
            throw new NotImplementedException();
        }
        private Orders AggiornaDocumentoCRM(uvwWmsDocument nD, Orders esiste)
        {
            throw new NotImplementedException();
        }
        private Orders ConvertiOrdineEspritecInOrdineCRM(uvwWmsDocument nD)
        {
            int docTip = (nD.DocTip == 203) ? 203 : 204;
            return new Orders
            {
                Orders_consigneeAddress = nD.ConsigneeAddress,
                Orders_consigneeCountry = nD.ConsigneeCountry,
                Orders_consigneeDes = nD.ConsigneeName,
                Orders_consigneeDistrict = nD.ConsigneeDistrict,
                Orders_consigneeID = nD.ConsigneeID,
                Orders_consigneeLocation = nD.ConsigneeLocation,
                Orders_consigneeRegion = nD.ConsigneeRegion,
                Orders_consigneeZipCode = nD.ConsigneeZipCode,
                Orders_coverage = nD.Coverage,
                Orders_customerDes = nD.CustomerDes,
                Orders_customerID = nD.CustomerID,
                Orders_docDate = nD.DocDta.Value,
                Orders_docNumber = nD.DocNum,
                Orders_executed = nD.Executed,
                Orders_externalID = nD.ExternalID,
                Orders_GespeUniq = nD.uniq,
                Orders_info1 = nD.Info1,
                Orders_deliveryNote = null,
                Orders_info2 = nD.Info2,
                Orders_info3 = nD.Info3,
                Orders_info4 = nD.Info4,
                Orders_info5 = nD.Info5,
                Orders_info6 = nD.Info6,
                Orders_info7 = nD.Info7,
                Orders_info8 = nD.Info8,
                Orders_info9 = nD.Info9,
                Orders_internalNote = nD.ItemInfo,
                Orders_ownerDes = nD.OwnerDes,
                Orders_ownerID = nD.OwnerID,
                Orders_planned = nD.Planned,
                Orders_publicNote = "",
                Orders_reference = nD.Reference,
                Orders_reference2 = nD.Reference2,
                Orders_reference2Date = nD.RefDta2,
                Orders_referenceDate = nD.RefDta.Value,
                Orders_reTypeID = nD.RegTypeID,
                Orders_senderAddress = nD.SenderAddress,
                Orders_senderCountry = nD.SenderCountry,
                Orders_senderDes = nD.SenderName,
                Orders_senderDistrict = nD.SenderDistrict,
                Orders_senderID = nD.SenderID,
                Orders_senderLocation = nD.SenderLocation,
                Orders_senderRegion = nD.SenderRegion,
                Orders_senderZipCode = nD.SenderZipCode,
                Orders_shipDocNumber = nD.ShipDocNum,
                Orders_shipID = nD.ShipUniq,
                Orders_siteID = nD.SiteID,
                Orders_statusDes = nD.StatusDes,
                Orders_statusId = nD.ItemStatus.Value,
                Orders_totalBoxes = nD.TotalBoxes,
                Orders_totalCube = nD.TotalCube,
                Orders_totalGrossWeight = nD.TotalGrossWeight,
                Orders_totalNetWeight = nD.TotalNetWeight,
                Orders_totalPacks = nD.TotalPacks,
                Orders_totalQty = nD.TotalQty,
                Orders_tripDocNumber = nD.TripDocNum,
                Orders_tripID = nD.TripUniq,
                Orders_unloadAddress = nD.UnloadAddress,
                Orders_unloadCountry = nD.UnloadCountry,
                Orders_unloadDes = nD.UnloadName,
                Orders_unloadDistrict = nD.UnloadDistrict,
                Orders_unloadID = nD.UnloadID,
                Orders_unloadLocation = nD.UnloadLocation,
                Orders_unloadRegion = nD.UnloadRegion,
                Orders_unloadZipCode = nD.UnloadZipCode,
                Orders_XcmID = nD.uniq,
                Orders_docTip = docTip
            };
        }
        private void aggiornaOrdineCRMdaDocumentoEspritec(Orders docDaAggiornare, uvwWmsDocument nD)
        {
            docDaAggiornare.Orders_consigneeAddress = nD.ConsigneeAddress;
            docDaAggiornare.Orders_consigneeCountry = nD.ConsigneeCountry;
            docDaAggiornare.Orders_consigneeDes = nD.ConsigneeName;
            docDaAggiornare.Orders_consigneeDistrict = nD.ConsigneeDistrict;
            docDaAggiornare.Orders_consigneeID = nD.ConsigneeID;
            docDaAggiornare.Orders_consigneeLocation = nD.ConsigneeLocation;
            docDaAggiornare.Orders_consigneeRegion = nD.ConsigneeRegion;
            docDaAggiornare.Orders_consigneeZipCode = nD.ConsigneeZipCode;
            docDaAggiornare.Orders_coverage = nD.Coverage;
            docDaAggiornare.Orders_customerDes = nD.CustomerDes;
            docDaAggiornare.Orders_customerID = nD.CustomerID;
            docDaAggiornare.Orders_docDate = nD.DocDta.Value;
            docDaAggiornare.Orders_docNumber = nD.DocNum;
            docDaAggiornare.Orders_executed = nD.Executed;
            docDaAggiornare.Orders_externalID = nD.ExternalID;
            docDaAggiornare.Orders_GespeUniq = nD.uniq;
            docDaAggiornare.Orders_info1 = nD.Info1;
            docDaAggiornare.Orders_deliveryNote = null;
            docDaAggiornare.Orders_info2 = nD.Info2;
            docDaAggiornare.Orders_info3 = nD.Info3;
            docDaAggiornare.Orders_info4 = nD.Info4;
            docDaAggiornare.Orders_info5 = nD.Info5;
            docDaAggiornare.Orders_info6 = nD.Info6;
            docDaAggiornare.Orders_info7 = nD.Info7;
            docDaAggiornare.Orders_info8 = nD.Info8;
            docDaAggiornare.Orders_info9 = nD.Info9;
            docDaAggiornare.Orders_internalNote = nD.ItemInfo;
            docDaAggiornare.Orders_ownerDes = nD.OwnerDes;
            docDaAggiornare.Orders_ownerID = nD.OwnerID;
            docDaAggiornare.Orders_planned = nD.Planned;
            docDaAggiornare.Orders_publicNote = "";
            docDaAggiornare.Orders_reference = nD.Reference;
            docDaAggiornare.Orders_reference2 = nD.Reference2;
            docDaAggiornare.Orders_reference2Date = nD.RefDta2;
            docDaAggiornare.Orders_referenceDate = nD.RefDta.Value;
            docDaAggiornare.Orders_reTypeID = nD.RegTypeID;
            docDaAggiornare.Orders_senderAddress = nD.SenderAddress;
            docDaAggiornare.Orders_senderCountry = nD.SenderCountry;
            docDaAggiornare.Orders_senderDes = nD.SenderName;
            docDaAggiornare.Orders_senderDistrict = nD.SenderDistrict;
            docDaAggiornare.Orders_senderID = nD.SenderID;
            docDaAggiornare.Orders_senderLocation = nD.SenderLocation;
            docDaAggiornare.Orders_senderRegion = nD.SenderRegion;
            docDaAggiornare.Orders_senderZipCode = nD.SenderZipCode;
            docDaAggiornare.Orders_shipDocNumber = nD.ShipDocNum;
            docDaAggiornare.Orders_shipID = nD.ShipUniq;
            docDaAggiornare.Orders_siteID = nD.SiteID;
            docDaAggiornare.Orders_statusDes = nD.StatusDes;
            docDaAggiornare.Orders_statusId = nD.ItemStatus.Value;
            docDaAggiornare.Orders_totalBoxes = nD.TotalBoxes;
            docDaAggiornare.Orders_totalCube = nD.TotalCube;
            docDaAggiornare.Orders_totalGrossWeight = nD.TotalGrossWeight;
            docDaAggiornare.Orders_totalNetWeight = nD.TotalNetWeight;
            docDaAggiornare.Orders_totalPacks = nD.TotalPacks;
            docDaAggiornare.Orders_totalQty = nD.TotalQty;
            docDaAggiornare.Orders_tripDocNumber = nD.TripDocNum;
            docDaAggiornare.Orders_tripID = nD.TripUniq;
            docDaAggiornare.Orders_unloadAddress = nD.UnloadAddress;
            docDaAggiornare.Orders_unloadCountry = nD.UnloadCountry;
            docDaAggiornare.Orders_unloadDes = nD.UnloadName;
            docDaAggiornare.Orders_unloadDistrict = nD.UnloadDistrict;
            docDaAggiornare.Orders_unloadID = nD.UnloadID;
            docDaAggiornare.Orders_unloadLocation = nD.UnloadLocation;
            docDaAggiornare.Orders_unloadRegion = nD.UnloadRegion;
            docDaAggiornare.Orders_unloadZipCode = nD.UnloadZipCode;
            docDaAggiornare.Orders_docTip = (nD.DocTip == 203) ? 203 : 204;
        }
        private void VerificaMessaggiEDI()
        {
            try
            {
                _loggerCode.Info($"Controllo messaggi EDI");
                var client = new RestClient(endpointAPI_Espritec + $"/api/edi/message/list/500/1");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", $"Bearer {token_XCM}");
                IRestResponse response = client.Execute(request);
                var Messages = JsonConvert.DeserializeObject<RootobjectMessageEDI>(response.Content);

                //_loggerCode.Info($"Recuperati {Messages.messages.Count()} messaggi EDI");

                var messaggiRecenti = Messages.messages.Where(x => x.receivedTimeStamp != null && DateTime.Parse(x.receivedTimeStamp) >= LastCheckChangesWMS && x.statusId == 3).ToList();
                if (messaggiRecenti.Count > 0)
                {
                    foreach (var mr in messaggiRecenti)
                    {
                        _loggerCode.Error($"E' stato rilevato un errore di import flussi {mr.msgTypeName}");
                        GestoreMail.SendMailErrorEDI(mr);
                    }
                }
            }
            catch (Exception ee)
            {
                _loggerCode.Error($"E' stato rilevato un errore di lettura messaggi EDI\r\n{ee}");
                GestoreMail.SendMailErrorEDICustom(ee, ee.Message);
            }

        }
        #endregion

        #region Metodi di controllo
        private void RecuperaLavoroSuDocumentiDaFile()
        {
            if (File.Exists(docSospesi))
            {
                _loggerCode.Info("RecuperaLavoroSuDocumentiDaFile Esiste: " + docSospesi);
                var idDaVerificare = File.ReadAllLines(docSospesi).ToList();
                foreach (var id in idDaVerificare)
                {
                    var client = new RestClient(endpointAPI_Espritec + $"/api/wms/document/get/{id}");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Authorization", $"Bearer {token_XCM}");
                    IRestResponse response = client.Execute(request);
                    var docOsservato = JsonConvert.DeserializeObject<RootobjectXCMOrderNEW>(response.Content);
                    if (docOsservato.header != null)
                    {
                        VerificaCambiamentoDocWMS(docOsservato);
                    }
                    else
                    {
                        GestoreMail.SegnalaErroreDev($"Errore documento non trovato {id}", $"documento con id {id} non trovato in gespe all'endpoint {endpointAPI_Espritec}");
                        _loggerCode.Error($"documento con id {id} non trovato in gespe all'endpoint {endpointAPI_Espritec}");
                    }
                }
                File.WriteAllText(docSospesi, "");
            }
            else
            {
                _loggerCode.Info("RecuperaLavoroSuDocumentiDaFile Non Esiste: " + docSospesi);
            }
        }
        private RootobjectXCMRowsNEW RecuperaRigheDocumentoXCMLink(RootobjectXCMOrderNEW docOsservato)
        {
            RootobjectXCMRowsNEW xcmDocRows = null;
            var client = new RestClient(endpointAPI_Espritec + $"/api/wms/document/row/list/{docOsservato.links[0].id}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token_XCM}");
            var response = client.Execute(request);
            return xcmDocRows = JsonConvert.DeserializeObject<RootobjectXCMRowsNEW>(response.Content);
        }
        #endregion

        #region Report
        /// <summary>
        /// Crea report movimenti sommando le quantità ed ignorando il lotto sempre dal primo del mese
        /// </summary>
        private void CreaReportMovimentiSommatiPerQuantitaDaInizioMeseEReportGiacenze(ANAGRAFICA_MANDANTI m)
        {
            DateTime dtn = DateTime.Now;
            DateTime dataA = dtn;
            DateTime dataDa = new DateTime();

            if (dataA.Month == 1)
            {
                dataDa = new DateTime(dataA.Year - 1, 12, 01);
            }
            else
            {
                dataDa = new DateTime(dataA.Year, dataA.Month - 1, 01);
            }

            List<string> ReportDaInviare = new List<string>();
            string periodo = $"dal {dataDa.ToShortDateString().Replace("/", "-")} al {dataA.ToShortDateString().Replace("/", "-")}";

            var dd = dtn.Day;
            var MM = dtn.Month;
            var yyyy = dtn.Year;

            ReportDaInviare.Add(CreaReportMovMagSommatiPerQuantita(m, dataDa, dataA, periodo));
            ReportDaInviare.Add(ProduciFileGiacenze(m));


            GestoreMail.SendMailMovimentiMagazzinoMeseeGiacSett(ReportDaInviare, m.MAIL_NOTIFICA_MOV_MAG, periodo);
        }
        /// <summary>
        /// provvede alla creazione e all'invio dei movimenti di magazzino e le giacenze alla data di esecuzione
        /// </summary>
        /// <param name="m">anagrafica mandante ado</param>
        /// <param name="interoMese"></param>
        private void CreaEdInviaReportMovimentiMagazzinoEGiacenzeAlMandante(ANAGRAFICA_MANDANTI m, bool interoMese)
        {
            DateTime dtn = DateTime.Now;
            DateTime dataDa = dtn;
            DateTime dataA = dtn;
            List<string> ReportDaInviare = new List<string>();
            string periodo = "";

            var dd = dtn.Day;
            var MM = dtn.Month;
            var yyyy = dtn.Year;

            #region Calcolo Periodo
            if (interoMese)
            {
                if (dtn.Month == 1)
                {
                    MM = 12;
                    yyyy = dtn.Year - 1;
                }
                else
                {
                    MM = dtn.Month - 1;
                }
                dd = DateTime.DaysInMonth(yyyy, MM);
                dataDa = new DateTime(yyyy, MM, 01);
                dataA = new DateTime(yyyy, MM, dd, 23, 59, 59);
                periodo = recuperaStringaMese(MM) + $" {yyyy}";
            }
            else
            {

                dataDa = DateTime.Now.Date - TimeSpan.FromDays(7);
                dataA = DateTime.Now;
                periodo = $"dal {dataDa.ToShortDateString().Replace("/", "-")} al {dataA.ToShortDateString().Replace("/", "-")}";
            }
            #endregion

            ReportDaInviare.Add(CreaReportMovMag(m, dataDa, dataA, periodo));
            if (interoMese)
            {
                ReportDaInviare.Add(CreaReportMovMagSommatiPerQuantita(m, dataDa, dataA, periodo));
            }
            if (m.ID_MANDANTE_GESPE == "00007")
            {
                ReportDaInviare.Add(ProduciFileGiacenzeVivisol(m, "ASLNANORD"));
                ReportDaInviare.Add(ProduciFileGiacenzeVivisol(m, "VIVISOLNA"));
            }
            else
            {
                ReportDaInviare.Add(ProduciFileGiacenze(m));
            }

            GestoreMail.SendMailGiacenzeMagazzinoEMovimentazioni(ReportDaInviare.ToArray(), m.MAIL_NOTIFICA_MOV_MAG, periodo);
        }
        /// <summary>
        /// Crea il report dei movimenti di magazzino per il mandante passato
        /// </summary>
        /// <param name="m">anagrafica mandante ado</param>
        /// <param name="dataDa">data inizio 00:00:00</param>
        /// <param name="dataA">data fine 23:59:59</param>
        /// <param name="periodo">stringa intestazione documento</param>
        /// <returns></returns>
        private string CreaReportMovMag(ANAGRAFICA_MANDANTI m, DateTime dataDa, DateTime dataA, string periodo)
        {
            var dbG = new GnXcmEntities();
            var dtn = DateTime.Now.Date;
            Workbook workbook = new Workbook();
            workbook.LoadDocument(PathTemplateMovMag);
            string finalDest = "";
            int i = 0;

            try
            {
                _loggerCode.Info($"Produzione movimenti magazzino per il cliente {m.NOME_MANDANTE}");
                var MovMag = dbG.uvwWmsRegistrations.Where(x => x.DateReg >= dataDa && x.DateReg <= dataA && x.CustomerID == m.ID_MANDANTE_GESPE &&
                                        (x.RegTypeID == "MAN" || x.RegTypeID == "RESOCLI" || x.RegTypeID == "DEL" || x.RegTypeID == "IN")).OrderBy(x => x.DateReg).ToList();

                var rettInv = dbG.uvwWmsRegistrations.Where(x => x.DateReg >= dataDa && x.DateReg <= dataA && x.CustomerID == m.ID_MANDANTE_GESPE && x.RegTypeID == "INV").ToList();

                if (rettInv != null && rettInv.Count > 0)
                {
                    List<uvwWmsRegistrations> RettInvNormalizzate = NormalizzaRigheInventario(rettInv);
                    MovMag.AddRange(RettInvNormalizzate);
                }

                var movandRett = MovMag.OrderBy(x => x.DateReg).ToList();


                if (movandRett.Count > 0)
                {
                    bool thereAreMagazzinoLogico = false;
                    var wksheet = workbook.Worksheets[0];
                    var totRighe = movandRett.Count();
                    workbook.BeginUpdate();

                    wksheet.Cells[$"A{2}"].Value = $"Movimenti di Magazzino {m.NOME_MANDANTE} - {periodo}";
                    for (i = 0; i < totRighe; i++)
                    {
                        var record = movandRett[i];

                        thereAreMagazzinoLogico = !string.IsNullOrWhiteSpace(record.LogicWareID);
                        string causale = "";

                        if (record.RegTypeID == "MAN")
                        {
                            causale = "MANUALE";
                        }
                        else if (record.RegTypeID == "INV")
                        {
                            causale = "RETT. INVENTARIO";
                        }
                        else if (record.RegTypeID == "RESOCLI")
                        {
                            causale = "RESO CLIENTE";
                        }
                        else if (record.RegTypeID == "DEL")
                        {
                            causale = "USCITA";
                        }
                        else if (record.RegTypeID == "IN")
                        {
                            causale = "INGRESSO";
                        }

                        string motivazione = "";
                        if (causale != "USCITA" && causale != "INGRESSO")
                        {
                            motivazione = record.ItemNote;

                        }
                        if (!string.IsNullOrEmpty(motivazione))
                        {
                            var interna = motivazione.IndexOf('#');
                            if (interna > 0)
                            {
                                motivazione = motivazione.Substring(0, interna);
                            }
                        }
                        #region Scrittura dati
                        if (record.Qty < 0)
                        {
                            //TotUscite -= record.Qty.Value;//gestione errore?? come cazzo fanno questi a ammettere registrazioni senza dato certo di quantità?!?!?!?!?!?!?!
                            wksheet.Cells[$"H{i + 4}"].Value = record.Qty;
                            wksheet.Cells[$"H{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                            wksheet.Cells[$"H{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                        }
                        else
                        {
                            if (causale == "USCITA")
                            {
                                causale = "INGRESSO";
                            }
                            //TotEntrate += record.Qty.Value;
                            wksheet.Cells[$"I{i + 4}"].Value = record.Qty;
                            wksheet.Cells[$"I{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                            wksheet.Cells[$"I{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                        }

                        wksheet.Cells[$"A{i + 4}"].Value = record.DateReg.Value.ToString("dd/MM/yyyy");
                        wksheet.Cells[$"B{i + 4}"].Value = causale;
                        wksheet.Cells[$"B{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                        wksheet.Cells[$"B{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                        wksheet.Cells[$"C{i + 4}"].Value = motivazione;
                        wksheet.Cells[$"D{i + 4}"].Value = record.DocReference;
                        wksheet.Cells[$"E{i + 4}"].Value = record.PrdCod;
                        wksheet.Cells[$"F{i + 4}"].Value = record.PrdDes;
                        wksheet.Cells[$"G{i + 4}"].Value = record.LogicWareID;
                        wksheet.Cells[$"J{i + 4}"].Value = record.BatchNo;
                        wksheet.Cells[$"J{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                        wksheet.Cells[$"J{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                        wksheet.Cells[$"K{i + 4}"].Value = record.DateExpire;
                        wksheet.Cells[$"K{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                        wksheet.Cells[$"K{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                        wksheet.Cells[$"L{i + 4}"].Value = record.SenderName;
                        //wksheet.Cells[$"M{i + 4}"].Value = record.SenderLocation;
                        wksheet.Cells[$"M{i + 4}"].Value = record.ConsigneeName;
                        //wksheet.Cells[$"O{i + 4}"].Value = record.ConsigneeLocation;
                        #endregion
                    }
                    //wksheet.Cells[$"H2"].Value = $"Tot Uscite: {TotUscite.ToString("#")}";
                    wksheet.Cells[$"H2"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"H2"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    //wksheet.Cells[$"I2"].Value = $"Tot Entrate: {TotEntrate.ToString("#")}";
                    wksheet.Cells[$"I2"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"I2"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                    #region Colora righe
                    var docRange = wksheet.GetUsedRange();

                    for (int z = 2; z < docRange.RowCount; z++)
                    {
                        bool odds = (z % 2 == 0);
                        if (odds)
                        {
                            wksheet.Rows[z].FillColor = Color.LightGray;
                        }
                    }
                    if (!thereAreMagazzinoLogico)
                    {
                        wksheet.Columns["G"].Delete();
                    }
                    docRange.AutoFitColumns();
                    #endregion

                    bool interoMese = (dataA - dataDa > TimeSpan.FromDays(20));

                    if (interoMese)
                    {
                        finalDest = Path.Combine(WorkPath, m.NOME_MANDANTE, "MovMag", $"Movimenti magazzino {periodo}.xlsx");
                    }
                    else
                    {
                        finalDest = Path.Combine(WorkPath, m.NOME_MANDANTE, "MovMag", $"Movimenti magazzino settimanali dal {dataDa.Day} al {dataA.Day}.xlsx");
                    }

                    if (File.Exists(finalDest))
                    {
                        File.Delete(finalDest);
                    }
                    if (!Directory.Exists(Path.GetDirectoryName(finalDest)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(finalDest));
                    }
                }
                else
                {
                    var wksheet = workbook.Worksheets[0];
                    workbook.BeginUpdate();
                    wksheet.Cells[$"A{2}"].Value = $"Movimenti di Magazzino {m.NOME_MANDANTE} - {periodo}";

                    finalDest = Path.Combine(WorkPath, m.NOME_MANDANTE, "MovMag", $"Movimenti magazzino settimanali dal {dataDa.Day} al {dataA.Day}.xlsx");
                    if (File.Exists(finalDest))
                    {
                        File.Delete(finalDest);
                    }
                    if (!Directory.Exists(Path.GetDirectoryName(finalDest)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(finalDest));
                    }
                }
            }
            catch (Exception ee)
            {
                if (ee.Message != "MOVIMENTI NON PRESENTI")
                {
                    throw;
                }
            }
            finally
            {
                workbook.EndUpdate();
                workbook.SaveDocument(finalDest, DocumentFormat.OpenXml);
            }

            return finalDest;
        }
        private string CreaReportMovMagSommatiPerQuantita(ANAGRAFICA_MANDANTI m, DateTime dataDa, DateTime dataA, string periodo)
        {
            var dbG = new GnXcmEntities();
            var dtn = DateTime.Now.Date;
            Workbook workbook = new Workbook();
            workbook.LoadDocument(PathTemplateMovMagRaggruppataPerQuantita);
            string finalDest = "";
            int i = 0;

            try
            {
                _loggerCode.Info($"Produzione movimenti magazzino per il cliente {m.NOME_MANDANTE}");
                var MovMag = dbG.uvwWmsRegistrations.Where(x => x.DateReg >= dataDa && x.DateReg <= dataA && x.CustomerID == m.ID_MANDANTE_GESPE &&
                                        (x.RegTypeID == "MAN" || x.RegTypeID == "RESOCLI" || x.RegTypeID == "DEL" || x.RegTypeID == "IN")).OrderBy(x => x.PrdDes).ToList();

                if (MovMag.Count > 0)
                {
                    bool thereAreMagazzinoLogico = MovMag.Where(x => x.LogicWareID != null).Count() > 0;
                    var wksheet = workbook.Worksheets[0];

                    workbook.BeginUpdate();

                    var listaRaggruppataPerSommaQuantita = new List<uvwWmsRegistrations>();

                    foreach (var gc in MovMag)
                    {
                        gc.ItemNote = "";
                        var esiste = listaRaggruppataPerSommaQuantita.FirstOrDefault(x => x.PrdCod == gc.PrdCod && x.RegTypeID == gc.RegTypeID);
                        if (esiste != null)
                        {
                            esiste.Qty += gc.Qty;
                            esiste.DateReg = gc.DateReg;
                        }
                        else
                        {
                            listaRaggruppataPerSommaQuantita.Add(gc);
                        }
                    }
                    var totRighe = listaRaggruppataPerSommaQuantita.Count();
                    wksheet.Cells[$"A{2}"].Value = $"Movimenti di Magazzino {m.NOME_MANDANTE} - {periodo}";

                    for (i = 0; i < totRighe; i++)
                    {
                        var record = listaRaggruppataPerSommaQuantita[i];

                        string causale = "";

                        if (record.RegTypeID == "MAN")
                        {
                            causale = "MANUALE";
                        }
                        else if (record.RegTypeID == "INV")
                        {
                            causale = "RETT. INVENTARIO";
                        }
                        else if (record.RegTypeID == "RESOCLI")
                        {
                            causale = "RESO CLIENTE";
                        }
                        else if (record.RegTypeID == "DEL")
                        {
                            causale = "USCITA";
                        }
                        else if (record.RegTypeID == "IN")
                        {
                            causale = "INGRESSO";
                        }

                        #region Scrittura dati                           
                        wksheet.Cells[$"A{i + 4}"].Value = record.DateReg.Value.ToString("dd/MM/yyyy");
                        wksheet.Cells[$"B{i + 4}"].Value = causale;
                        wksheet.Cells[$"B{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                        wksheet.Cells[$"B{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                        wksheet.Cells[$"C{i + 4}"].Value = record.PrdCod;
                        wksheet.Cells[$"D{i + 4}"].Value = record.PrdDes;
                        wksheet.Cells[$"E{i + 4}"].Value = record.Qty;
                        wksheet.Cells[$"E{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                        wksheet.Cells[$"E{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                        #endregion
                    }

                    #region Colora righe
                    var docRange = wksheet.GetUsedRange();

                    for (int z = 2; z < docRange.RowCount; z++)
                    {
                        bool odds = (z % 2 == 0);
                        if (odds)
                        {
                            wksheet.Rows[z].FillColor = Color.LightGray;
                        }
                    }
                    if (!thereAreMagazzinoLogico)
                    {
                        wksheet.Columns["G"].Delete();
                    }
                    docRange.AutoFitColumns();
                    #endregion

                    finalDest = Path.Combine(WorkPath, m.NOME_MANDANTE, "MovMag", $"Movimenti magazzino {periodo} raggruppati.xlsx");

                    if (File.Exists(finalDest))
                    {
                        File.Delete(finalDest);
                    }
                    if (!Directory.Exists(Path.GetDirectoryName(finalDest)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(finalDest));
                    }
                }
                else
                {
                    var wksheet = workbook.Worksheets[0];
                    workbook.BeginUpdate();
                    wksheet.Cells[$"A{2}"].Value = $"Movimenti di Magazzino {m.NOME_MANDANTE} - {periodo}";

                    finalDest = Path.Combine(WorkPath, m.NOME_MANDANTE, "MovMag", $"Movimenti magazzino settimanali dal {dataDa.Day} al {dataA.Day}.xlsx");
                    if (File.Exists(finalDest))
                    {
                        File.Delete(finalDest);
                    }
                    if (!Directory.Exists(Path.GetDirectoryName(finalDest)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(finalDest));
                    }
                }
            }
            catch (Exception ee)
            {
                if (ee.Message != "MOVIMENTI NON PRESENTI")
                {
                    throw;
                }
            }
            finally
            {
                workbook.EndUpdate();
                workbook.SaveDocument(finalDest, DocumentFormat.OpenXml);
            }

            return finalDest;

        }
        /// <summary>
        /// valorizza solo le voci che non fanno somma 0 eliminando le superflue
        /// </summary>
        /// <param name="rettInv">lista registrazioni filtrate per tipo movimento inventario</param>
        /// <returns></returns>
        private List<uvwWmsRegistrations> NormalizzaRigheInventario(List<uvwWmsRegistrations> rettInv)
        {
            var resp = new List<uvwWmsRegistrations>();
            //TODO accorpare le voci per lotto e prodotto e segnalare solo le voci che a somma non fanno 0
            var listaRaggruppataPerSommaQuantita = new List<uvwWmsRegistrations>();

            foreach (var gc in rettInv)
            {
                gc.ItemNote = "";
                var esiste = listaRaggruppataPerSommaQuantita.FirstOrDefault(x => x.PrdCod == gc.PrdCod && x.BatchNo == gc.BatchNo && x.LogicWareID == gc.LogicWareID);
                if (esiste != null)
                {
                    esiste.Qty += gc.Qty;
                    esiste.DateReg = gc.DateReg;
                }
                else
                {
                    listaRaggruppataPerSommaQuantita.Add(gc);
                }
            }

            foreach (var azero in listaRaggruppataPerSommaQuantita)
            {
                if (azero.Qty != 0)
                {
                    resp.Add(azero);
                }
            }

            return resp;
        }
        /// <summary>
        /// produce il file xslx per le giacenze di mangazzino
        /// </summary>
        /// <param name="m">anagrafica mandante ado</param>
        /// <returns></returns>
        private string ProduciFileGiacenze(ANAGRAFICA_MANDANTI m)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var savepath = Path.Combine(WorkPath, "Giacenze", m.NOME_MANDANTE);
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }

            var finalDest = Path.Combine(savepath, $"Giacenze_{m.NOME_MANDANTE}_{DateTime.Now.ToString("ddMMyyyy")}.xlsx");

            CreaExportPersonalizzato(m, finalDest);

            return finalDest;
        }
        private string ProduciFileGiacenzeVivisol(ANAGRAFICA_MANDANTI m, string magazzinoLogico)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var savepath = Path.Combine(WorkPath, "Giacenze", m.NOME_MANDANTE);
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }

            var finalDest = Path.Combine(savepath, $"Giacenze_{m.NOME_MANDANTE}_{magazzinoLogico}_{DateTime.Now.ToString("ddMMyyyy")}.xlsx");

            CreaExportPersonalizzatoVivisol(m, finalDest, magazzinoLogico);

            return finalDest;
        }
        /// <summary>
        /// funzione di export giacenze con template XCM
        /// </summary>
        /// <param name="m">anagrafica mandante ado</param>
        /// <param name="finalDest">path di creazione finale del documento</param>
        private void CreaExportPersonalizzato(ANAGRAFICA_MANDANTI m, string finalDest)
        {
            var db = new GnXcmEntities();
            var giacCli = db.uvwWmsWarehouse.Where(x => x.CustomerID == m.ID_MANDANTE_GESPE && x.ItemStatus == 10)
                .Select(x => new GiacenzaProdotto()
                {
                    CodiceProdotto = x.PrdCod,
                    DataRiferimento = (x.DateRef != null) ? x.DateRef.Value : DateTime.MinValue,
                    DataScadenza = (x.DateExpire != null) ? x.DateExpire.Value : DateTime.MinValue,
                    DescrizioneProdotto = x.PrdDes,
                    Lotto = x.BatchNo,
                    MagazzinoLogico = x.LogWareID,
                    MapID = x.MapID,
                    QuantitaTotale = (x.TotalQty != null) ? x.TotalQty.Value : -1,
                    Riferimento = x.Reference,
                    ShelflifePrd = (x.OutShelfLife != null) ? x.OutShelfLife.Value : 0
                }).OrderBy(x => x.DescrizioneProdotto).ToList();

            var listaRaggruppataPerSommaQuantita = new List<GiacenzaProdotto>();

            foreach (var gc in giacCli)
            {
                if (listaRaggruppataPerSommaQuantita.Any(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MapID == gc.MapID && x.MagazzinoLogico == gc.MagazzinoLogico))
                {
                    var esiste = listaRaggruppataPerSommaQuantita.FirstOrDefault(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MapID == gc.MapID && x.MagazzinoLogico == gc.MagazzinoLogico);
                    if (esiste != null)
                    {
                        esiste.QuantitaTotale += gc.QuantitaTotale;
                    }
                    else
                    {
                        esiste = listaRaggruppataPerSommaQuantita.FirstOrDefault(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MapID == gc.MapID);
                        if (esiste != null)
                        {
                            esiste.QuantitaTotale += gc.QuantitaTotale;
                        }
                        else
                        {
                            esiste = listaRaggruppataPerSommaQuantita.FirstOrDefault(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MagazzinoLogico == gc.MagazzinoLogico);
                            if (esiste != null)
                            {
                                if (esiste.MapID == gc.MapID)
                                {
                                    esiste.QuantitaTotale += gc.QuantitaTotale;
                                }
                                else
                                {
                                    listaRaggruppataPerSommaQuantita.Add(gc);
                                }
                            }
                            else
                            {
                                listaRaggruppataPerSommaQuantita.Add(gc);
                            }
                        }
                    }
                }
                else
                {
                    listaRaggruppataPerSommaQuantita.Add(gc);
                }
            }

            Workbook workbook = new Workbook();
            workbook.LoadDocument(PathTemplateGiacenzeMag);

            try
            {
                var wksheet = workbook.Worksheets[0];

                var totRighe = listaRaggruppataPerSommaQuantita.Count();
                workbook.BeginUpdate();
                bool thereIsMagazzinoLogico = false;
                wksheet.Cells[$"A{2}"].Value = $"Giacenze al {DateTime.Now.ToString("dd/MM/yyyy")}";
                for (int i = 0; i < totRighe; i++)
                {
                    var rigaDoc = listaRaggruppataPerSommaQuantita[i];

                    bool scadenzaValida = false;
                    int scadAllaVendita = 0;
                    int scadEffettiva = 0;
                    if (rigaDoc.DataScadenza.Year > 2000)
                    {
                        scadAllaVendita = (int)(DateTime.Now - (rigaDoc.DataScadenza - TimeSpan.FromDays(rigaDoc.ShelflifePrd))).TotalDays * -1;
                        scadEffettiva = (int)(DateTime.Now.Date - rigaDoc.DataScadenza).TotalDays * -1;
                        scadenzaValida = true;
                    }

                    #region Reparto
                    var Rep = "";
                    if (rigaDoc.MapID == "VN")
                    {
                        Rep = "VENDIBILI";
                    }
                    else if (rigaDoc.MapID == "IN")
                    {
                        Rep = "INVENDIBILI";
                    }
                    else if (rigaDoc.MapID == "QR")
                    {
                        Rep = "QUARANTENA";
                    }
                    else if (rigaDoc.MapID == "PR")
                    {
                        Rep = "PROMOZIONALE";
                    }
                    else if (rigaDoc.MapID == "RS")
                    {
                        Rep = "RESI";
                    }
                    else if (rigaDoc.MapID == "SM")
                    {
                        Rep = "DA SMALTIRE";
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(rigaDoc.MagazzinoLogico))
                    {
                        thereIsMagazzinoLogico = true;
                    }

                    #region Scrittura dati
                    wksheet.Cells[$"A{i + 4}"].Value = Rep;
                    wksheet.Cells[$"A{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"A{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"B{i + 4}"].Value = rigaDoc.CodiceProdotto;
                    wksheet.Cells[$"C{i + 4}"].Value = rigaDoc.DescrizioneProdotto;
                    wksheet.Cells[$"D{i + 4}"].Value = (int)rigaDoc.QuantitaTotale;
                    wksheet.Cells[$"D{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"D{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"E{i + 4}"].Value = rigaDoc.Lotto;
                    wksheet.Cells[$"E{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"E{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"F{i + 4}"].Value = (scadenzaValida) ? rigaDoc.DataScadenza.ToString("dd/MM/yyyy") : "";
                    wksheet.Cells[$"F{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"F{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"G{i + 4}"].Value = rigaDoc.ShelflifePrd;
                    wksheet.Cells[$"G{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"G{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"H{i + 4}"].Value = scadAllaVendita;
                    wksheet.Cells[$"H{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"H{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"I{i + 4}"].Value = scadEffettiva;
                    wksheet.Cells[$"I{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"I{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"J{i + 4}"].Value = rigaDoc.MagazzinoLogico;
                    wksheet.Cells[$"J{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"J{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    #endregion
                }

                #region Colora righe
                var docRange = wksheet.GetUsedRange();

                for (int i = 2; i < docRange.RowCount; i++)
                {
                    bool odds = (i % 2 == 0);
                    if (odds)
                    {
                        wksheet.Rows[i].FillColor = Color.LightGray;
                    }
                }
                #endregion

                #region Pulizia
                if (!thereIsMagazzinoLogico)
                {
                    wksheet.Columns["J"].Delete();
                }

                if (File.Exists(finalDest))
                {
                    File.Delete(finalDest);
                }
                #endregion
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);


                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("InviaMovimentiMagazzino", ee);
                }

                LastException = ee;
            }
            finally
            {
                workbook.EndUpdate();
            }
            workbook.SaveDocument(finalDest, DocumentFormat.OpenXml);
        }
        private void CreaExportPersonalizzatoVivisol(ANAGRAFICA_MANDANTI m, string finalDest, string magazzinoLogico)
        {
            var db = new GnXcmEntities();
            var giacCli = db.uvwWmsWarehouse.Where(x => x.CustomerID == m.ID_MANDANTE_GESPE && x.ItemStatus == 10 && x.LogWareID == magazzinoLogico)
                .Select(x => new GiacenzaProdotto()
                {
                    CodiceProdotto = x.PrdCod,
                    DataRiferimento = (x.DateRef != null) ? x.DateRef.Value : DateTime.MinValue,
                    DataScadenza = (x.DateExpire != null) ? x.DateExpire.Value : DateTime.MinValue,
                    DescrizioneProdotto = x.PrdDes,
                    Lotto = x.BatchNo,
                    MagazzinoLogico = x.LogWareID,
                    MapID = x.MapID,
                    QuantitaTotale = (x.TotalQty != null) ? x.TotalQty.Value : -1,
                    Riferimento = x.Reference,
                    ShelflifePrd = (x.OutShelfLife != null) ? x.OutShelfLife.Value : 0
                }).OrderBy(x => x.DescrizioneProdotto).ToList();

            var listaRaggruppataPerSommaQuantita = new List<GiacenzaProdotto>();

            foreach (var gc in giacCli)
            {
                if (listaRaggruppataPerSommaQuantita.Any(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MapID == gc.MapID && x.MagazzinoLogico == gc.MagazzinoLogico))
                {
                    var esiste = listaRaggruppataPerSommaQuantita.FirstOrDefault(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MapID == gc.MapID && x.MagazzinoLogico == gc.MagazzinoLogico);
                    if (esiste != null)
                    {
                        esiste.QuantitaTotale += gc.QuantitaTotale;
                    }
                    else
                    {
                        esiste = listaRaggruppataPerSommaQuantita.FirstOrDefault(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MapID == gc.MapID);
                        if (esiste != null)
                        {
                            esiste.QuantitaTotale += gc.QuantitaTotale;
                        }
                        else
                        {
                            esiste = listaRaggruppataPerSommaQuantita.FirstOrDefault(x => x.CodiceProdotto == gc.CodiceProdotto && x.Lotto == gc.Lotto && x.MagazzinoLogico == gc.MagazzinoLogico);
                            if (esiste != null)
                            {
                                if (esiste.MapID == gc.MapID)
                                {
                                    esiste.QuantitaTotale += gc.QuantitaTotale;
                                }
                                else
                                {
                                    listaRaggruppataPerSommaQuantita.Add(gc);
                                }
                            }
                            else
                            {
                                listaRaggruppataPerSommaQuantita.Add(gc);
                            }
                        }
                    }
                }
                else
                {
                    listaRaggruppataPerSommaQuantita.Add(gc);
                }
            }

            Workbook workbook = new Workbook();
            workbook.LoadDocument(PathTemplateGiacenzeMag);

            try
            {
                var wksheet = workbook.Worksheets[0];

                var totRighe = listaRaggruppataPerSommaQuantita.Count();
                workbook.BeginUpdate();
                bool thereIsMagazzinoLogico = false;
                wksheet.Cells[$"A{2}"].Value = $"Giacenze al {DateTime.Now.ToString("dd/MM/yyyy")}";
                for (int i = 0; i < totRighe; i++)
                {
                    var rigaDoc = listaRaggruppataPerSommaQuantita[i];

                    bool scadenzaValida = false;
                    int scadAllaVendita = 0;
                    int scadEffettiva = 0;
                    if (rigaDoc.DataScadenza.Year > 2000)
                    {
                        scadAllaVendita = (int)(DateTime.Now - (rigaDoc.DataScadenza - TimeSpan.FromDays(rigaDoc.ShelflifePrd))).TotalDays * -1;
                        scadEffettiva = (int)(DateTime.Now.Date - rigaDoc.DataScadenza).TotalDays * -1;
                        scadenzaValida = true;
                    }

                    #region Reparto
                    var Rep = "";
                    if (rigaDoc.MapID == "VN")
                    {
                        Rep = "VENDIBILI";
                    }
                    else if (rigaDoc.MapID == "IN")
                    {
                        Rep = "INVENDIBILI";
                    }
                    else if (rigaDoc.MapID == "QR")
                    {
                        Rep = "QUARANTENA";
                    }
                    else if (rigaDoc.MapID == "PR")
                    {
                        Rep = "PROMOZIONALE";
                    }
                    else if (rigaDoc.MapID == "SM")
                    {
                        Rep = "DA SMALTIRE";
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(rigaDoc.MagazzinoLogico))
                    {
                        thereIsMagazzinoLogico = true;
                    }

                    #region Scrittura dati
                    wksheet.Cells[$"A{i + 4}"].Value = Rep;
                    wksheet.Cells[$"A{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"A{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"B{i + 4}"].Value = rigaDoc.CodiceProdotto;
                    wksheet.Cells[$"C{i + 4}"].Value = rigaDoc.DescrizioneProdotto;
                    wksheet.Cells[$"D{i + 4}"].Value = (int)rigaDoc.QuantitaTotale;
                    wksheet.Cells[$"D{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"D{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"E{i + 4}"].Value = rigaDoc.Lotto;
                    wksheet.Cells[$"E{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"E{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"F{i + 4}"].Value = (scadenzaValida) ? rigaDoc.DataScadenza.ToString("dd/MM/yyyy") : "";
                    wksheet.Cells[$"F{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"F{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"G{i + 4}"].Value = rigaDoc.ShelflifePrd;
                    wksheet.Cells[$"G{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"G{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"H{i + 4}"].Value = scadAllaVendita;
                    wksheet.Cells[$"H{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"H{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"I{i + 4}"].Value = scadEffettiva;
                    wksheet.Cells[$"I{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"I{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    wksheet.Cells[$"J{i + 4}"].Value = rigaDoc.MagazzinoLogico;
                    wksheet.Cells[$"J{i + 4}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    wksheet.Cells[$"J{i + 4}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    #endregion
                }

                #region Colora righe
                var docRange = wksheet.GetUsedRange();

                for (int i = 2; i < docRange.RowCount; i++)
                {
                    bool odds = (i % 2 == 0);
                    if (odds)
                    {
                        wksheet.Rows[i].FillColor = Color.LightGray;
                    }
                }
                #endregion

                #region Pulizia
                if (!thereIsMagazzinoLogico)
                {
                    wksheet.Columns["J"].Delete();
                }

                if (File.Exists(finalDest))
                {
                    File.Delete(finalDest);
                }
                #endregion
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);


                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("InviaMovimentiMagazzino", ee);
                }

                LastException = ee;
            }
            finally
            {
                workbook.EndUpdate();
            }
            workbook.SaveDocument(finalDest, DocumentFormat.OpenXml);
        }
        /// <summary>
        /// provvede al recupero del mese passato in formato stringa
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        private string recuperaStringaMese(int month)
        {
            switch (month)
            {
                case 1:
                    return "Gennaio";
                case 2:
                    return "Febbraio";
                case 3:
                    return "Marzo";
                case 4:
                    return "Aprile";
                case 5:
                    return "Maggio";
                case 6:
                    return "Giugno";
                case 7:
                    return "Luglio";
                case 8:
                    return "Agosto";
                case 9:
                    return "Settembre";
                case 10:
                    return "Ottobre";
                case 11:
                    return "Novembre";
                case 12:
                    return "Dicembre";
                default:
                    return "ND";
            }
        }
        private void InviaMovimentiMagazzinoEGiacenze(bool interoMese)
        {
            try
            {
                var dbM = new InterscambioAPIEntities();
                var mandanti = dbM.ANAGRAFICA_MANDANTI.ToList();




                foreach (var m in mandanti)
                {
                    //Claudio Recupero Giacenze POLARIS-----------
                    //if (m.NOME_MANDANTE.ToUpper() != "POLARIS") 
                    //    continue;
                    //--------------------------------------------


                    var dtn = DateTime.Now;
                    if (!m.NOTIFICA_GIACENZE) continue;
                    if (!interoMese && dtn - m.DATA_ULTIMA_NOTIFICA_GIACENZE < TimeSpan.FromDays(1)) continue;
                    if (!interoMese && dtn - m.DATA_ULTIMA_NOTIFICA_MOVI_MAG < TimeSpan.FromDays(1)) continue;
                    if (interoMese && m.DATA_ULTIMA_NOTIFICA_MOVI_MAG.Month == DateTime.Now.Month && m.DATA_ULTIMA_NOTIFICA_MOVI_MAG.Year == DateTime.Now.Year) continue;

                    var dbM2 = new InterscambioAPIEntities();
                    var mds = dbM2.ANAGRAFICA_MANDANTI.First(x => x.ID_ANAGRAFICA_MANDANTE == m.ID_ANAGRAFICA_MANDANTE);
                    if (m.ID_MANDANTE_GESPE == "00013")
                    {
                        CreaReportMovimentiSommatiPerQuantitaDaInizioMeseEReportGiacenze(m);
                    }
                    else
                    {
                        CreaEdInviaReportMovimentiMagazzinoEGiacenzeAlMandante(m, interoMese);
                    }

                    if (interoMese)
                    {
                        mds.DATA_ULTIMA_NOTIFICA_MOVI_MAG = dtn;
                        if (dtn.DayOfWeek == DayOfWeek.Sunday)
                        {
                            mds.DATA_ULTIMA_NOTIFICA_GIACENZE = dtn;
                        }
                    }
                    else
                    {
                        mds.DATA_ULTIMA_NOTIFICA_GIACENZE = dtn;
                    }
                    dbM2.SaveChanges();
                }
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("InviaMovimentiMagazzino", ee);
                }

                LastException = ee;
            }
        }
        #endregion

        #region Cambi stato documento
        #region Timer Cambi
        /// <summary>
        /// Provvede a recuperare in caso di primo avvio l'ultimo datetime valido che ha fatto la chiamata per recuperare i cambiamenti
        /// </summary>
        private void RecuperaLastCheckChangesWMS()
        {
            LastCheckChangesWMS = DateTime.Parse(File.ReadAllLines(PathLastCheckChangesFileWMS)[0]);
        }
        private void RecuperaLastCheckChangesTMS()
        {
            LastCheckChangesTMS = DateTime.Parse(File.ReadAllLines(PathLastCheckChangesFileTMS)[0]);
        }
        /// <summary>
        /// scrive il datetime valido dell'ultima chiamata di recupero cambiamenti
        /// </summary>
        /// <param name="append"></param>
        private void ScriviLastCheckChangesWMS(bool append)
        {
            if (!append)
            {
                File.WriteAllText(PathLastCheckChangesFileWMS, LastCheckChangesWMS.ToString());
            }
            else
            {
                File.AppendAllText(PathLastCheckChangesFileWMS, "\r\n" + LastCheckChangesWMS.ToString());
            }
        }
        private void ScriviLastCheckChangesTMS(bool append)
        {
            if (!append)
            {
                File.WriteAllText(PathLastCheckChangesFileTMS, LastCheckChangesTMS.ToString());
            }
            else
            {
                File.AppendAllText(PathLastCheckChangesFileTMS, "\r\n" + LastCheckChangesTMS.ToString());
            }
        }
        #endregion
        /// <summary>
        /// recupera i cambi stato da gespe tramite endpoint api/wms/document/tracking/changes
        /// </summary>
        private void RecuperaCambiamenti()
        {
            RecuperaConnessione();

            CambiWMS();
            CambiTMS();

        }
        List<long> CambiTackingGiaNotificati = new List<long>();
        private void CambiTMS()
        {
            try
            {
                _loggerCode.Info("Recupero Cambi TMS da API");

                RestClient client = null;
                RecuperaLastCheckChangesTMS();
                string ts = (LastCheckChangesTMS).ToString("s", CultureInfo.InvariantCulture);
                client = new RestClient(endpointAPI_UNITEX + $"/api/tms/shipment/tracking/changes/500/1?FromTimeStamp={ts}");
                //client = new RestClient(endpointAPI_UNITEX + $"/api/tms/shipment/tracking/changes/500/1");
                LastCheckChangesTMS = DateTime.Now;

                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", $"Bearer {token_UNITEX}");
                IRestResponse response = client.Execute(request);
                var TrackingUnitex = JsonConvert.DeserializeObject<RootobjectShipmentTracking>(response.Content);
                if (TrackingUnitex.events != null)
                {
                    _loggerCode.Info($"Trovati {TrackingUnitex.events.Count()} cambi spedizioni");
                    foreach (var ShipTrackingUnitex in TrackingUnitex.events)
                    {

                        var shipUnitex = RecuperaShipUnitexByShipmentID(ShipTrackingUnitex.shipID);
                        if (shipUnitex == null || CambiTackingGiaNotificati.Contains(shipUnitex.id))
                        {
                            continue;
                        }
                        var db = new GnXcmEntities();
                        var dateTo = DateTime.Now - TimeSpan.FromDays(90);
                        var OrdineXCMDB = db.uvwWmsDocument.FirstOrDefault(x => x.DocNum == shipUnitex.externRef && x.DocDta >= dateTo);

                        if (OrdineXCMDB == null) continue;
                        VerificaCambiamentoDocTMSUNITEX(shipUnitex, OrdineXCMDB);
                        VerificaSincronizzazioneCRMShipment(shipUnitex, OrdineXCMDB, ShipTrackingUnitex);
                        CambiTackingGiaNotificati.Add(shipUnitex.id);
                    }
                }
                int Cambiamenti = 0;
                if (TrackingUnitex.events != null)
                {
                    Cambiamenti = TrackingUnitex.events.Count();
                }

                _loggerCode.Info($"LastTimeCheckTMS: {LastCheckChangesTMS.ToString("dd/MM/yyyy HH:mm:ss")} - Cambi recuperati {Cambiamenti}");
                ScriviLastCheckChangesTMS(false);
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("CambiTMS", ee);
                }

                LastException = ee;
            }
        }
        private void VerificaSincronizzazioneCRMShipment(Shipment shipConnesso, uvwWmsDocument ordineXCM, EventShipmentTracking TrackingUnitex)
        {
            try
            {
                var cli = UtentiCRM.FirstOrDefault(x => x.Customer_id == ordineXCM.CustomerID);
                if (cli != null && cli.Customer_IsEnableCRM && cli.Customer_Authorization != null && cli.Customer_Authorization.ExpireDateTracking > DateTime.Now)
                {
                    var dtn = DateTime.Now;
                    var db = new XCM_CRMEntities();
                    var dbGespe = new GnXcmEntities();

                    var shipCRM = ConvertiShipDBEspritecToCRMDB(shipConnesso, ordineXCM, TrackingUnitex);
                    var esiste = db.ShipmentList.FirstOrDefault(x => x.Shipment_GespeID_UNITEX == shipConnesso.id);

                    if (esiste != null)
                    {
                        if (esiste.Shipment_StatusID != shipConnesso.statusId)
                        {
                            esiste = shipCRM;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var dbSave = new XCM_CRMEntities();
                        dbSave.ShipmentList.Add(shipCRM);
                        dbSave.SaveChanges();
                    }
                }
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("VerificaSincronizzazioneCRMShipment", ee);
                }

                LastException = ee;
            }

        }
        private EntityModels.ShipmentList ConvertiShipDBEspritecToCRMDB(Shipment shipConnesso, uvwWmsDocument ordineXCM, EventShipmentTracking TrackingUnitex)
        {
            DateTime? dtnull = null;
            return new EntityModels.ShipmentList()
            {
                Shipment_AttachCount = 0,
                Shipment_CashCurrency = shipConnesso.cashCurrency,
                Shipment_CashNote = shipConnesso.cashNote,
                Shipment_CashPayment = shipConnesso.cashPayment,
                Shipment_CashValue = shipConnesso.cashValue,
                Shipment_ConsigneeDes = shipConnesso.consigneeDes,
                Shipment_ConsigneeID = shipConnesso.consigneeID,
                Shipment_Cube = shipConnesso.cube,
                Shipment_CustomerDes = shipConnesso.customerDes,
                Shipment_CustomerID = shipConnesso.customerID,
                Shipment_DeliveryDateTime = shipConnesso.deliveryDateTime.ToString(),
                Shipment_DocDate = (shipConnesso.docDate != null) ? shipConnesso.docDate.Value : dtnull,
                Shipment_DocNumber = shipConnesso.docNumber,
                Shipment_ExternalRef = shipConnesso.externRef,
                Shipment_FirstStopDes = "",
                Shipment_FirstStopID = 0,
                Shipment_FloorPallets = shipConnesso.floorPallets,
                Shipment_GespeID_UNITEX = shipConnesso.id,
                Shipment_GrossWeight = shipConnesso.grossWeight,
                Shipment_InsideRef = shipConnesso.insideRef,
                Shipment_LastStopDes = "",
                Shipment_LastStopID = 0,
                Shipment_Meters = shipConnesso.meters,
                Shipment_NetWeight = shipConnesso.netWeight,
                Shipment_OwnerAgency = shipConnesso.ownerAgency,
                Shipment_Packs = shipConnesso.packs,
                Shipment_PickupDateTime = shipConnesso.pickupDateTime,
                Shipment_PickupSupplierDes = shipConnesso.pickupSupplierDes,
                Shipment_PickupSupplierID = shipConnesso.pickupSupplierID,
                Shipment_SenderDes = shipConnesso.senderDes,
                Shipment_SenderID = shipConnesso.senderID,
                Shipment_ServiceType = shipConnesso.serviceType,
                Shipment_StatusDes = shipConnesso.statusDes,
                Shipment_StatusID = shipConnesso.statusId,
                Shipment_StatusType = shipConnesso.statusType,
                Shipment_TotalPallets = shipConnesso.totalPallets,
                Shipment_TransportType = shipConnesso.transportType,
                Shipment_WebOrderID = shipConnesso.webOrderID,
                Shipment_WebOrderNumber = shipConnesso.webOrderNumber,
                Shipment_WebStatusID = shipConnesso.webStatusId,
                Shipment_WebStatusType = "",
                Shipment_DocGespeID_XCM = 0,//
                Shipment_CustomerID_XCM = ordineXCM.CustomerID,
                Tracking = { new Tracking() { Tracking_Data = TrackingUnitex.timeStamp, Tracking_StatusID = TrackingUnitex.statusID, Tracking_ShipmentID = TrackingUnitex.shipID, Tracking_StatusDes = TrackingUnitex.statusDes } },
            };
        }
        private void VerificaCambiamentoDocTMSUNITEX(Shipment ShipConnessoUnitex, uvwWmsDocument spedizioneDB)
        {
            if (spedizioneDB != null)
            {
                var anagCorr = new InterscambioAPIEntities().ANAGRAFICA_MANDANTI.FirstOrDefault(x => x.ID_MANDANTE_GESPE == spedizioneDB.CustomerID);
                if (anagCorr == null) return;
                else if (!anagCorr.NOTIFICA_TRACKING)
                {
                    return;
                }

                var statoBreve = "";
                var bodyMail = "";

                if (ShipConnessoUnitex.statusId == 1)//INGRESSATA DA UNITEX
                {
                    bodyMail = "La spedizione in oggetto è partita dalll'hub del nostro corriere";
                    statoBreve = "PARTITA";
                }
                else if (ShipConnessoUnitex.statusId == 10)
                {
                    bodyMail = "La spedizione in oggetto è in cosegna";
                    statoBreve = "IN CONSEGNA";
                }
                else if (ShipConnessoUnitex.statusId == 30)
                {
                    bodyMail = "La spedizione in oggetto è stata consegnata al destinatario";
                    statoBreve = "CONSEGNATA";
                }
                else if (ShipConnessoUnitex.statusId == 40)
                {
                    bodyMail = "La spedizione in oggetto non è stata ritirada dal destinatario";
                    statoBreve = "MANCATA CONSEGNA";
                }
                else if (ShipConnessoUnitex.statusId == 50)
                {
                    bodyMail = "La spedizione in oggetto risulta in giacenza";
                    statoBreve = "IN GIACENZA";
                }
                else if (ShipConnessoUnitex.statusId == 55)
                {
                    bodyMail = "La spedizione in oggetto risulta in Giacenza in attesa di svincolo";
                    statoBreve = "IN GIACENZA ATTESA DI SVINCOLO";
                }
                else if (ShipConnessoUnitex.statusId == 60)
                {
                    bodyMail = "La spedizione in oggetto risulta da riconsegnare";
                    statoBreve = "DA RICONSEGNARE";
                }
                else if (ShipConnessoUnitex.statusId == 70)
                {
                    bodyMail = "La spedizione in oggetto risulta restituito al mittente";
                    statoBreve = "RESTITUITO AL MITTENTE";
                }
                else
                {
                    return;
                }

                var objMail = $"SPEDIZIONE VS. RIF. {spedizioneDB.Reference.Trim()} - STATO {statoBreve}";

                GestoreMail.InviaMailTrackingShipment(objMail, bodyMail, anagCorr.MAIL_TRACKING);
            }

        }
        private Shipment RecuperaShipUnitexByShipmentID(int shipID)
        {
            var resp = new Shipment();
            var clientLink = new RestClient(endpointAPI_UNITEX + $"/api/tms/shipment/get/{shipID}");
            clientLink.Timeout = -1;
            var requestLink = new RestRequest(Method.GET);
            requestLink.AddHeader("Authorization", $"Bearer {token_UNITEX}");
            IRestResponse responseLink = clientLink.Execute(requestLink);
            if (responseLink.IsSuccessful)
            {
                var shipR = JsonConvert.DeserializeObject<RootobjectShipment>(responseLink.Content);
                resp = shipR.shipment;
            }
            return resp;
        }
        private void CambiWMS()
        {
            try
            {
                _loggerCode.Info("Recupero Cambi WMS da API");

                RestClient client = null;
                //if (LastCheckChangesWMS == DateTime.MinValue)

                //RecuperaLastCheckChangesWMS();
                string ts = LastCheckChangesWMS.ToString("s", CultureInfo.InvariantCulture);
                client = new RestClient(endpointAPI_Espritec + $"/api/wms/document/tracking/changes/500/1?FromTimeStamp={ts}");

                //else
                //{
                //    client = new RestClient(endpointAPI_XCM + $"/api/wms/document/tracking/changes/500/1");
                //}

                LastCheckChangesWMS = DateTime.Now;
                ScriviLastCheckChangesWMS(true);

                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", $"Bearer {token_XCM}");
                IRestResponse response = client.Execute(request);
                var DocTrack = JsonConvert.DeserializeObject<RootobjectTrackingDocument>(response.Content);
                if (DocTrack.result.status == true)
                {
                    var dttt = DocTrack.trackings;
                    _loggerCode.Info($"Trovati {dttt.Count()} cambi");
                    foreach (var dt in dttt)
                    {
                        //DEBUGITERATOR
                        //if (dt.docNumber != "01221/BEM") 
                        //{
                        //    continue;
                        //}

                        //if (BEMVivisolInviate.Contains(dt.docNumber))
                        //{
                        //    GestoreMail.SegnalaErroreDev($"Documento duplicato {dt.docNumber}","il documento in oggetto risulta già inviato");                            
                        //    continue;
                        //}

                        try
                        {
                            _loggerCode.Debug($"Analisi del documento {dt.docNumber}");
                            if (dt.docNumber.EndsWith("/INV"))
                            {
                                continue;
                            }
                            var client2 = new RestClient(endpointAPI_Espritec + $"/api/wms/document/get/{dt.docID}");
                            client2.Timeout = -1;
                            var request2 = new RestRequest(Method.GET);
                            request2.AddHeader("Authorization", $"Bearer {token_XCM}");
                            IRestResponse response2 = client2.Execute(request2);
                            var docOsservato = JsonConvert.DeserializeObject<RootobjectXCMOrderNEW>(response2.Content);

                            if (docOsservato != null && docOsservato.header == null) continue;

                            //if (string.IsNullOrEmpty(docOsservato.header.info1) || docOsservato.header.info1 == "XC")
                            //{
                            //    continue;
                            //}


                            if ((docOsservato.header.customerID != "00024" || docOsservato.header.customerID != "00007") &&
                                ((dt.doctype == "DeliveryOUT" && dt.statusID == 10) || dt.doctype == "DeliveryIN" && (dt.statusID == 30 || dt.statusID == 20)))
                            {
                                continue;
                            }
                            VerificaCambiamentoDocWMS(docOsservato);

                            //VerificaSincronizzazioneCRMOrdini(docOsservato); //se ne occupa la sincronizzazione notturna
                        }
                        catch (Exception ee)
                        {
                            string msg = $"Errore durante l'analisi del documento {dt.docNumber}";
                            _loggerCode.Error(msg);
                            _loggerCode.Error(ee);
                            GestoreMail.SegnalaErroreDev(msg, ee);
                        }
                    }
                }
                else
                {
                    //non ci sono documenti aggiornati
                }
                int Cambiamenti = 0;
                if (DocTrack.trackings != null)
                {
                    Cambiamenti = DocTrack.trackings.Count();
                }

                _loggerCode.Info($"LastTimeCheckWMS: {LastCheckChangesWMS.ToString("dd/MM/yyyy HH:mm:ss")} - Cambi recuperati {Cambiamenti}");

                ScriviLastCheckChangesWMS(false);
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("CambiWMS", ee);
                }

                LastException = ee;
            }
        }
        private void VerificaSincronizzazioneCRMOrdini(RootobjectXCMOrderNEW docOsservato)
        {
            var cli = UtentiCRM.FirstOrDefault(x => x.Customer_id == docOsservato.header.customerID);
            if (cli != null && cli.Customer_IsEnableCRM && cli.Customer_Authorization.ExpireDateOrders > DateTime.Now)
            {
                var db = new XCM_CRMEntities();
                var ordCRM = ConvertiJsonEspritecToCRMDB(docOsservato);
                var esiste = db.Orders.FirstOrDefault(x => x.Orders_GespeUniq == docOsservato.header.id);

                if (esiste != null)
                {
                    if (esiste.Orders_statusId != docOsservato.header.statusId)
                    {
                        esiste = ordCRM;
                        db.SaveChanges();
                    }
                }
                else
                {
                    db.Orders.Add(ordCRM);
                    db.SaveChanges();
                }
            }
        }
        private static Orders ConvertiJsonEspritecToCRMDB(RootobjectXCMOrderNEW docOsservato)
        {
            DateTime? dtnull = null;
            return new Orders()
            {
                Orders_consigneeAddress = docOsservato.header.consigneeAddress,
                Orders_consigneeCountry = docOsservato.header.consigneeCountry,
                Orders_consigneeDes = docOsservato.header.consigneeDes,
                Orders_consigneeDistrict = docOsservato.header.consigneeDistrict,
                Orders_consigneeID = docOsservato.header.consigneeID,
                Orders_consigneeLocation = docOsservato.header.consigneeLocation,
                Orders_consigneeRegion = docOsservato.header.consigneeRegion,
                Orders_consigneeZipCode = docOsservato.header.consigneeZipCode,
                Orders_coverage = docOsservato.header.coverage,
                Orders_customerDes = docOsservato.header.customerDes,
                Orders_customerID = docOsservato.header.customerID,
                Orders_deliveryNote = docOsservato.header.deliveryNote,
                Orders_docDate = docOsservato.header.docDate,
                Orders_docNumber = docOsservato.header.docNumber,
                Orders_executed = docOsservato.header.executed,
                Orders_externalID = docOsservato.header.externalID,
                Orders_GespeUniq = docOsservato.header.id,
                Orders_info1 = docOsservato.header.info1,
                Orders_info2 = docOsservato.header.info2,
                Orders_info3 = docOsservato.header.info3,
                Orders_info4 = docOsservato.header.info4,
                Orders_info5 = docOsservato.header.info5,
                Orders_info6 = docOsservato.header.info6,
                Orders_info7 = docOsservato.header.info7,
                Orders_info8 = docOsservato.header.info8,
                Orders_info9 = docOsservato.header.info9,
                Orders_internalNote = docOsservato.header.internalNote,
                Orders_ownerDes = docOsservato.header.ownerDes,
                Orders_ownerID = docOsservato.header.ownerID,
                Orders_planned = docOsservato.header.planned,
                Orders_publicNote = docOsservato.header.publicNode,
                Orders_reference = docOsservato.header.reference,
                Orders_reference2 = docOsservato.header.reference2,
                Orders_reference2Date = (DateTime.TryParse(docOsservato.header.reference2Date, out DateTime DT)) ? DT : dtnull,
                Orders_referenceDate = docOsservato.header.referenceDate,
                Orders_reTypeID = docOsservato.header.regTypeID,
                Orders_rowsNo = docOsservato.header.rowsNo,
                Orders_senderAddress = docOsservato.header.senderAddress,
                Orders_senderCountry = docOsservato.header.senderCountry,
                Orders_senderDes = docOsservato.header.senderDes,
                Orders_senderDistrict = docOsservato.header.senderDistrict,
                Orders_senderID = docOsservato.header.senderID,
                Orders_senderLocation = docOsservato.header.senderLocation,
                Orders_senderRegion = docOsservato.header.senderRegion,
                Orders_senderZipCode = docOsservato.header.senderZipCode,
                Orders_shipDocNumber = docOsservato.header.shipDocNumber,
                Orders_shipID = docOsservato.header.shipID,
                Orders_siteID = docOsservato.header.siteID,
                Orders_statusDes = docOsservato.header.statusDes,
                Orders_statusId = docOsservato.header.statusId,
                Orders_totalBoxes = docOsservato.header.totalBoxes,
                Orders_totalCube = docOsservato.header.totalCube,
                Orders_totalGrossWeight = docOsservato.header.totalGrossWeight,
                Orders_totalNetWeight = docOsservato.header.totalNetWeight,
                Orders_totalPacks = docOsservato.header.totalPacks,
                Orders_totalQty = docOsservato.header.totalQty,
                Orders_tripDocNumber = docOsservato.header.tripDocNumber,
                Orders_tripID = docOsservato.header.tripID,
                Orders_unloadAddress = docOsservato.header.unloadAddress,
                Orders_unloadCountry = docOsservato.header.unloadCountry,
                Orders_unloadDes = docOsservato.header.unLoadDes,
                Orders_unloadDistrict = docOsservato.header.unloadDistrict,
                Orders_unloadID = docOsservato.header.unloadID,
                Orders_unloadLocation = docOsservato.header.unloadLocation,
                Orders_unloadRegion = docOsservato.header.unloadRegion,
                Orders_unloadZipCode = docOsservato.header.unloadZipCode

            };
        }
        private void VerificaCambiamentoDocWMS(RootobjectXCMOrderNEW docOsservato)
        {
            int StatusID = docOsservato.header.statusId;
            switch (docOsservato.header.docType)
            {
                case "DeliveryIN":
                    if (StatusID == 10 || StatusID == 20)
                    {
                        if (docOsservato.header.customerID == "00015")
                        {
                            ControllaSeInseritoManualmente_PH_PH(docOsservato);
                        }
                    }
                    else if (StatusID == 31) //BEM IN INGRESSO TERMINATA
                    {
                        _loggerCode.Info($"{docOsservato.header.docNumber}: BEM IN INGRESSO TERMINATA");
                        if (docOsservato.header.customerID == "00007")
                        {
                            ComunicazioneAPIVivisol(true, docOsservato);
                        }
                        else
                        if (docOsservato.header.customerID == "00015")
                        {
                            CreaFileInterscambioPHPH(true, /*doc,*/ docOsservato);
                        }

                        ProduciEdInviaBEMalCliente(docOsservato);

                    }
                    else if (StatusID == 40)
                    {
                        _loggerCode.Debug($"{docOsservato.header.docNumber}: BEM IN INGRESSO ANNULLATA");
                    }
                    break;
                case "DeliveryOUT":


                    //if ((StatusID == 20) || (StatusID == 21)) Claudio Modifica per recupero PH PH

                    if (StatusID == 20) //DDT CREATO
                    {
                        _loggerCode.Debug($"{docOsservato.header.docNumber}: DOCUMENTO DDT CREATO");
                        /*if (docOsservato.header.customerID == "00007")
                        {
                            //ComunicazioneAPIVivisol(false, docOsservato);
                        }
                        else*/
                        if (docOsservato.header.customerID == "00015")
                        {
                            CreaFileInterscambioPHPH(false, /*doc,*/ docOsservato);
                        }
                        else if (docOsservato.header.customerID == "00024")
                        {
                            CreaCSVRispostaEvasioneOrdineAPS(docOsservato);
                        }
                        else
                        {
                            // Attualmente inviato da Gespe documento DDT di evasione al cliente
                        }
                    }
                    else if (StatusID == 21)
                    {
                        _loggerCode.Debug($"{docOsservato.header.docNumber}: TRASMESSA AL MINISTERO PER PRESENZA FARMACI");
                    }
                    else if (StatusID == 40)
                    {
                        _loggerCode.Debug($"{docOsservato.header.docNumber}: DOCUMENTO ANNULLATO");
                    }
                    break;
                case "OrderIN":
                    _loggerCode.Debug($"{docOsservato.header.docNumber}: CAMBIO RILEVATO SU ORDINE A FORNITORE");
                    break;
                case "OrderOUT":
                    if (StatusID == 10)
                    {
                        if (docOsservato.header.customerID == "00024")//
                        {
                            if (!ControllaSeOrdineDafneEdInviaDocumentoPerConferma(docOsservato))
                            {
                                ControllaOrdineNuovoEVerificaCheCiSianoTutteLeInfo(docOsservato);
                            }
                        }
                        /*else if (docOsservato.header.customerID == "00007")
                        {
                            //PopolaIDatiMancantiPianificaVivisol(docOsservato);
                        }*/
                        else if (docOsservato.header.customerID == "00015")
                        {
                            //PopolaDocNum2PerPrebolla(docOsservato);
                            ControllaSeInseritoManualmente_PH_PH(docOsservato);
                        }
                        else if (docOsservato.header.customerID == "00018")// || docOsservato.header.customerID == "00029")
                        {
                            ControllaSeOrdineDafneEdInviaDocumentoPerConferma(docOsservato);
                        }
                    }
                    else if (StatusID == 20)
                    {
                        _loggerCode.Debug($"{docOsservato.header.docNumber}: ORDINE IN PREPARAZIONE");

                        if (docOsservato.header.customerID == "00002" ||
                                docOsservato.header.customerID == "00003" ||
                                docOsservato.header.customerID == "00006" ||
                                docOsservato.header.customerID == "00008" ||
                                docOsservato.header.customerID == "00011")
                        {
                            ControllaSeHannoInseritoGliScontiCorrettamente(docOsservato);
                        }
                        else if (docOsservato.header.customerID == "00024")
                        {
                            if (!string.IsNullOrEmpty(docOsservato.header.info1))
                            {
                                CreaFileOrdiniAPS_DAFNE(docOsservato);
                            }
                        }

                    }
                    else if (StatusID == 30)
                    {
                        /* if (docOsservato.header.customerID == "00007")
                         {
                             //InviaAVivisolPreparazioneInCorso(docOsservato);
                         }*/

                        _loggerCode.Debug($"{docOsservato.header.docNumber}: ORDINE EVASO DDT NON ANCORA CREATO");
                    }
                    else if (StatusID == 40)
                    {
                        _loggerCode.Debug($"{docOsservato.header.docNumber}: ORDINE SPEDITO");
                    }
                    else if (StatusID == 50)
                    {
                        _loggerCode.Debug($"{docOsservato.header.docNumber}: ORDINE ANNULLATO");
                    }
                    break;
            }
        }
        private void PopolaDocNum2PerPrebolla(RootobjectXCMOrderNEW docOsservato)
        {
            var db = new GnXcmEntities();
            var qu = $"UPDATE WmsDocument set DocNum2={docOsservato.header.reference} WHERE uniq={docOsservato.header.id}";
            db.Database.ExecuteSqlCommand(qu);
        }
        private void ControllaOrdineNuovoEVerificaCheCiSianoTutteLeInfo(RootobjectXCMOrderNEW docOsservato)
        {
            try
            {
                var db = new GnXcmEntities();
                var righeOrdineDB = db.uvwWmsDocumentRows_XCM.Where(x => x.uniq == docOsservato.header.id);

                bool noteDocumento = false;
                bool noteDiPreparazione = false;
                if (righeOrdineDB.Count() > 0)
                {
                    if (righeOrdineDB.Any(x => x.RecUserID != 999)) return;

                    noteDocumento = righeOrdineDB.Any(x => x.PrdCod == "00001007" || x.PrdCod == "00001017");
                    noteDiPreparazione = righeOrdineDB.Any(x => x.PrdCod == "000020125" || x.PrdCod == "00003225" || x.PrdCod == "00008420" || x.PrdCod == "00001009" || x.PrdCod == "00020300");

                    if (noteDocumento || noteDiPreparazione)
                    {
                        InviaTramiteAPINote(docOsservato, noteDocumento, noteDiPreparazione);
                    }
                }
            }
            catch (Exception ee)
            {

                //throw;
            }

        }
        private void InviaTramiteAPINote(RootobjectXCMOrderNEW docOsservato, bool noteDocumento, bool noteDiPreparazione)
        {
            var client = new RestClient(endpointAPI_Espritec + $"/api/wms/document/update");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            var raw = new RootobjectUpdateDocument()
            {
                header = new HeaderUpdateDocument()
                {
                    id = docOsservato.header.id,
                    publicNote = noteDocumento ? "**Dispositivo di nostra proprietà esclusiva consegnato per essere utilizzato a supporto dell'attività di monitoraggio dei valori glicemici ed essere riconsegnato al termine del periodo di utilizzo in conformità agli accordi commerciali intercorsi." : "",
                    internalNote = noteDiPreparazione ? "ALLEGARE CERTIFICATO" : "",
                }

            };

            var jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            jsetting.DefaultValueHandling = DefaultValueHandling.Ignore;

            var body = JsonConvert.SerializeObject(raw, Newtonsoft.Json.Formatting.Indented, jsetting);
            request.AddHeader("Authorization", $"Bearer {token_XCM}");
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            IRestResponse response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                GestoreMail.SegnalaAlCustomerCustom("Errore in inserimento annotazioni APS", $"per il documento {docOsservato.header.docNumber} non è stato possibile inserire le note per i prodotti al suo interno");
            }

        }

        #region APS
        private void CreaFileOrdiniAPS_DAFNE(RootobjectXCMOrderNEW docOsservato)
        {
            try
            {
                var db = new GnXcmEntities();
                var dbAPS = new InterscambioAPIEntities();

                List<string> righe = new List<string>();
                var d = db.uvwWmsDocument.Where(x => x.uniq == docOsservato.header.id).FirstOrDefault();
                var righeDocumento = db.uvwWmsDocumentRows_XCM.Where(x => x.uniq == d.uniq).ToList();// RecuperaRigheDocumentoXCM(docOsservato).rows;

                string numRif = d.DocNum2;
                string refer = d.Reference;

                List<string> FileRisposta = new List<string>();
                //FileRisposta.Add($"OR_STATO,ORSERIAL,ORTIPDOC,ORNUMDOC,ORALFDOC,ORDATDOC,ORNUMEST,ORALFEST,ORDATEST,ORTIPCON,ORCODCON,ORCODAGE,ORCODDES,DDNOMDES,DDINDIRI,DD__CAP,DDLOCALI,DDPROVIN,DDCODNAZ,ORCODPAG,MVDESDOC,ORCODVAL,ORSCOCL1,ORSCOCL2,ORSCOPAG,ORFLSCOR,ORSCONTI,ORTDTEVA,ORSPEINC,ORSPETRA,ORSPEIMB,ORCODSPE,ORCODVET,ORCODPOR,ORCONCON,UTCC,UTDC,UTCV,UTDV,OR__NOTE,ORMERISP,ORTOTORD,OR_EMAIL,ORFLMAIL,ORNETMER,ORLOGSTM,ORRIFEST,ORVALNAZ,ORCAOVAL,ORACCONT,ORVALACC,ORSPEBOL,CPROWNUM,ORCODICE,CPROWORD,ORCODART,ORCODVAR,ORDESART,ORTIPRIG,ORDESSUP,ORUNIMIS,ORQTAMOV,ORQTAUM1,ORPREZZO,ORSCONT1,ORSCONT2,ORSCONT3,ORSCONT4,ORSCOIN1,ORSCOIN2,ORSCOIN3,ORSCOIN4,ORCODIVA,ORFLOMAG,ORDATEVA,ORCODLIS,ORCONTRA,ORRIFKIT,ORLOGSTD,ORUMNODI,ORVALSCO");
                int i = 1;
                foreach (var riga in righeDocumento)
                {

                    string serial = d.uniq.ToString();
                    var artAPS = dbAPS.APS_DAFNE.FirstOrDefault(x => x.CODICE_PRODOTTO == riga.PrdCod);

                    var des1 = riga.PrdDes;
                    var des2 = "";

                    if (des1.Length > 40)
                    {
                        des1 = riga.PrdDes.Substring(0, 40);
                        des2 = riga.PrdDes.Substring(40, riga.PrdDes.Length - des1.Length);
                    }
                    while (serial.Length < 9)
                    {
                        serial = serial.Insert(0, "0");
                    }

                    string piva = RecuperaPivaAPSDaCodiceMittenteDAFNE(d.Info1);

                    var nr = new OrdiniDafneToAPS()
                    {
                        ORSERIAL = $"DAF-{serial}",//$"CRMADM-WEB-{docOsservato.header.docDate.ToString("yy")}{d.DocNum2}/01",
                        ORTIPDOC = "OR",
                        ORDATDOC = docOsservato.header.docDate.ToString("dd/MM/yyyy"),
                        ORTIPCON = "C",
                        ORCODCON = piva,
                        ORCODAGE = "1",
                        ORCODPAG = "DAFNE",
                        MVDESDOC = "",
                        ORCODVAL = "EUR",
                        ORVALNAZ = "EUR",
                        CPROWNUM = $"{i}",
                        ORCODART = $"{riga.PrdCod}",
                        ORDESART = $"{des1}",
                        ORDESSUP = $"{des2}",
                        ORUNIMIS = $"Pz",
                        ORQTAMOV = (riga.Qty != null) ? $"{riga.Qty.Value.ToString("0.000").Replace(",", ".")}" : "0",
                        ORQTAUM1 = (riga.Qty != null) ? $"{riga.Qty.Value.ToString("0.000").Replace(",", ".")}" : "0",
                        ORSCONTI = (riga.Discount != null) ? $"{riga.Discount.Value.ToString("0.0000").Replace(",", ".")}" : "0.0000",
                        ORCODIVA = (artAPS != null) ? $"{artAPS.IVA_PRODOTTO}" : "10",
                        ORFLOMAG = "X",
                        ORDATEVA = docOsservato.header.docDate.ToString("dd/MM/yyyy"),
                        //ORPREZZO = (riga.SellPrice != null) ? $"{riga.SellPrice.Value.ToString("0.000").Replace(",", ".")}" : "0.000",
                        ORPREZZO = (artAPS != null) ? artAPS.PREZZO_PRODOTTO.ToString().Replace(",", ".") : "0.00000",
                        ORSCONT1 = (artAPS != null) ? artAPS.SCONTO_PRODOTTO.ToString().Replace(",", ".") : "0.00000",
                        DDCODNAZ = docOsservato.header.unloadCountry,
                        DDINDIRI = docOsservato.header.unloadAddress,
                        DDLOCALI = docOsservato.header.unloadLocation,
                        DDNOMDES = docOsservato.header.unLoadDes,
                        DDPROVIN = docOsservato.header.unloadDistrict,
                        DD__CAP = docOsservato.header.unloadZipCode,
                        ORCODDES = docOsservato.header.unloadID.ToString(),

                    };

                    //if ((docOsservato.header.consigneeAddress != docOsservato.header.unloadAddress) ||
                    //    (docOsservato.header.consigneeZipCode != docOsservato.header.unloadZipCode) ||
                    //     (docOsservato.header.consigneeDistrict != docOsservato.header.unloadDistrict))
                    //{
                    //    nr.DDCODNAZ = docOsservato.header.unloadCountry;
                    //    nr.DDINDIRI = docOsservato.header.unloadAddress;
                    //    nr.DDLOCALI = docOsservato.header.unloadLocation;
                    //    nr.DDNOMDES = docOsservato.header.unLoadDes;
                    //    nr.DDPROVIN = docOsservato.header.unloadDistrict;
                    //    nr.DD__CAP = docOsservato.header.unloadZipCode;
                    //    nr.ORCODDES = docOsservato.header.unloadID.ToString();
                    //}


                    FileRisposta.Add(nr.ToString());
                    i++;
                }

                File.WriteAllLines($@"C:\FTP\APS\OUT\DAFNE\SINGOLI\DAFNE_{docOsservato.header.id}.csv", FileRisposta);
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        private string RecuperaPivaAPSDaCodiceMittenteDAFNE(string codDafne)
        {
            if (codDafne == "20049R001W48") //COROFAR DISTRIBUZIONE 
            {
                return "04508200401";
            }
            else if (codDafne == "00147050926")
            {
                return "03432530925";
            }
            else if(codDafne == "03531670820")//RE ROBERTO
            {
                return "03531670820";
            }
            else if (codDafne == "01913600902")//DIFARMA 
            {
                return "01913600902";
            }
            else if (codDafne == "00153470737")//NEW COTAFARTI
            {
                return "10151500963";
            }
            else
            {
                return codDafne;
            }
        }
        private bool ControllaSeOrdineDafneEdInviaDocumentoPerConferma(RootobjectXCMOrderNEW docOsservato)
        {
            if (!string.IsNullOrEmpty(docOsservato.header.info1))
            {
                _loggerCode.Info($"Trovato ordine dafne da comunicare ad {docOsservato.header.customerDes} - {docOsservato.header.id}");
                InviaOrdineDiConferma(docOsservato);

                return true;
            }
            else
            {
                return false;
            }
        }
        private void InviaOrdineDiConferma(RootobjectXCMOrderNEW docOsservato)
        {
            string csID = docOsservato.header.customerID;
            var db = new InterscambioAPIEntities();
            var mandante = db.ANAGRAFICA_MANDANTI.First(x => x.ID_MANDANTE_GESPE == csID);

            var client = new RestClient(endpointAPI_Espritec + $"/api/wms/document/row/list/{docOsservato.header.id}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token_XCM}");
            var response = client.Execute(request);
            var xcmRec = JsonConvert.DeserializeObject<RootobjectXCMRowsNEW>(response.Content);
            if (xcmRec.rows == null)
            {
                _loggerCode.Error($"Errore nella notifica dell'ordine DAFNE {docOsservato.header.docNumber} Per l'ordine in oggetto non risultano righe di prodotti, non procedo all'invio della notifica d'ordine");
                GestoreMail.SegnalaAlCustomerCustom($"Errore nella notifica dell'ordine DAFNE {docOsservato.header.docNumber}", "Per l'ordine in oggetto non risultano righe di prodotti, non procedo all'invio della notifica d'ordine");
                return;
            }
            var pathOrd = DocORDER.produciDocumentoORDER_DAFNE(xcmRec, docOsservato);
            GestoreMail.SendMailORDINE_DAFNE(pathOrd, docOsservato.header.customerID);
            _loggerCode.Info("Mail ordine dafne inviata");


        }

        private List<string> SviluppaGruppoRigheSeriali(uvwWmsDocument d, uvwWmsDocumentRows_XCM riga, ViewSerialSpec[] righeConSeriali, string serialeDafne)
        {
            var db = new GnXcmEntities();
            var resp = new List<string>();
            var des1 = riga.PrdDes;
            var des2 = "";

            if (des1.Length > 40)
            {
                des1 = riga.PrdDes.Substring(0, 40);
                des2 = riga.PrdDes.Substring(40, riga.PrdDes.Length - des1.Length);
            }
            string CodCliAPS = "";
            if (!string.IsNullOrEmpty(serialeDafne))
            {
                CodCliAPS = RecuperaCodiceClienteAPSDaCodiceMittenteDAFNE(d.Info1);
            }

            for (int i = 0; i < righeConSeriali.Count(); i++)
            {
                if (!string.IsNullOrEmpty(serialeDafne))
                {
                    var specs = righeConSeriali[i];
                    var nr = new RispostaEvasioneDDTAPS()
                    {
                        MVDATDOC = $"{d.DocDta.Value.ToString("dd/MM/yyyy")}",
                        MVNUMDOC = $"{d.DocNum2}",
                        MVALFDOC = $"XCM",
                        MVCODCLI = $"{CodCliAPS}",
                        MVSERIAL = $"DAF-{serialeDafne}",
                        CPROWNUM = $"{riga.RowInfo1}",
                        MVCODART = $"{riga.PrdCod}",
                        MVDESART = $"{des1}",
                        MVDESSUP = $"{des2}",
                        MVQTAMOV = $"1.00",
                        MVCODLOT = $"{riga.Batchno}",
                        MVCODMAT = $"{specs.SerialNumber}",
                    };
                    resp.Add(nr.ToString());
                }
                else
                {
                    var specs = righeConSeriali[i];
                    var nr = new RispostaEvasioneDDTAPS()
                    {
                        MVDATDOC = $"{d.DocDta.Value.ToString("dd/MM/yyyy")}",
                        MVNUMDOC = $"{d.DocNum2}",
                        MVALFDOC = $"{d.Info4}",
                        MVCODCLI = $"{d.Info5}",
                        MVSERIAL = $"{d.Info2}",
                        CPROWNUM = $"{riga.RowInfo1}",
                        MVCODART = $"{riga.PrdCod}",
                        MVDESART = $"{des1}",
                        MVDESSUP = $"{des2}",
                        MVQTAMOV = $"1.00",
                        MVCODLOT = $"{riga.Batchno}",
                        MVCODMAT = $"{specs.SerialNumber}",
                    };
                    resp.Add(nr.ToString());
                }
            }

            return resp;

        }

        private void CreaCSVRispostaEvasioneOrdineAPS(RootobjectXCMOrderNEW docOsservato)
        {
            var db = new GnXcmEntities();
            List<string> righe = new List<string>();
            var doc = db.uvwWmsDocument.Where(x => x.uniq == docOsservato.header.id).FirstOrDefault();
            if (doc.RegTypeID != "OUT") return;
            //var docFromID = docOsservato.links[0].id;
            var righeDocumento = db.uvwWmsDocumentRows_XCM.Where(x => x.uniq == docOsservato.header.id).ToList();// RecuperaRigheDocumentoXCM(docOsservato).rows;
                                                                                                                 //var righeRegistrazione = db.uvwWmsRegistrations.Where(x => x.UniqDoc == docFromID && x.RegTypeID == "DEL").ToList();
            string numRif = doc.DocNum2;
            string refer = doc.Reference;

            List<uvwWmsDocumentRows_XCM> raggruppatePerRigheCliente = new List<uvwWmsDocumentRows_XCM>();

            foreach (var r in righeDocumento)
            {
                var esiste = raggruppatePerRigheCliente.FirstOrDefault(x => x.RowInfo1 == r.RowInfo1 && x.Batchno == r.Batchno);
                if (esiste != null)
                {
                    esiste.Qty += r.Qty;
                }
                else
                {
                    raggruppatePerRigheCliente.Add(r);
                }
            }


            List<string> FileRisposta = new List<string>();
            List<uvwWmsDocumentRows_XCM> aggiunte = new List<uvwWmsDocumentRows_XCM>();
            var dbReg = new GnXcmEntities();
            bool isDafneDDT = false;
            int i = 1;

            string serial = "";
            var ordine = db.uvwWmsDocument.FirstOrDefault(x => x.DocTip == 203 && x.ShipUniq == doc.ShipUniq);
            if (ordine != null)
            {
                serial = ordine.uniq.ToString();
            }
            while (serial.Length < 9)
            {
                serial = serial.Insert(0, "0");
            }

            foreach (var riga in raggruppatePerRigheCliente)
            {

                var righeConSeriali = db.ViewSerialSpec.Where(x => x.PrdCod == riga.PrdCod && x.UniqFrom == riga.uniq).ToList();
                if (righeConSeriali != null && righeConSeriali.Count() > 0)
                {
                    //controlla se ci sono più lotti per prodotto con seriali
                    var grupBybC = righeConSeriali.GroupBy(x => x.WareBarcode).ToList();

                    if (grupBybC.Count > 1)
                    {
                        var esiste = FileRisposta.FirstOrDefault(x => x.Contains(riga.PrdCod));

                        //se già inserito deve skippare
                        if (esiste != null)
                        {
                            continue;
                        }

                        foreach (var gbc in grupBybC)
                        {
                            //confronta l'udc per prendere il lotto di appartenenza
                            var ff = dbReg.uvwWmsRegistrations.FirstOrDefault(x => x.Barcode == gbc.Key);
                            var dc = righeConSeriali.Where(x => x.WareBarcode == ff.Barcode).ToList();
                            riga.Batchno = ff.BatchNo;
                            FileRisposta.AddRange(SviluppaGruppoRigheSeriali(doc, riga, dc.ToArray(), serial));
                            aggiunte.Add(riga);
                        }
                        continue;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(doc.Info1))
                        {

                            FileRisposta.AddRange(SviluppaGruppoRigheSeriali(doc, riga, righeConSeriali.ToArray(), ""));
                        }
                        else
                        {
                            var ff = dbReg.uvwWmsRegistrations.FirstOrDefault(x => x.Barcode == grupBybC.First().Key);
                            var dc = righeConSeriali.Where(x => x.WareBarcode == ff.Barcode).ToList();
                            riga.Batchno = ff.BatchNo;
                            FileRisposta.AddRange(SviluppaGruppoRigheSeriali(doc, riga, righeConSeriali.ToArray(), serial));

                        }

                        aggiunte.Add(riga);
                        continue;
                    }
                }

                //var rigaRegistrazione = righeRegistrazione.FirstOrDefault(x => x.UniqDocRow == riga.RowID);
                var des1 = riga.PrdDes;
                var des2 = "";

                if (des1.Length > 40)
                {
                    des1 = riga.PrdDes.Substring(0, 40);
                    des2 = riga.PrdDes.Substring(40, riga.PrdDes.Length - des1.Length);
                }


                if (!string.IsNullOrEmpty(doc.Info1))
                {
                    isDafneDDT = true;
                    //TODO MANCA AGGANCIO CON SERIALE SU AHR

                    string CodCliAPS = RecuperaCodiceClienteAPSDaCodiceMittenteDAFNE(doc.Info1);

                    var nr = new RispostaEvasioneDDTAPS()
                    {
                        MVDATDOC = $"{doc.DocDta.Value.ToString("dd/MM/yyyy")}",
                        MVNUMDOC = $"{doc.DocNum2}",
                        MVALFDOC = $"XCM",
                        MVCODCLI = $"{CodCliAPS}",
                        MVSERIAL = $"DAF-{serial}",
                        CPROWNUM = $"{i}",
                        MVCODART = $"{riga.PrdCod}",
                        MVDESART = $"{des1}",
                        MVDESSUP = $"{des2}",
                        MVQTAMOV = $"{riga.Qty.Value.ToString("0.00").Replace(",", ".")}",
                        MVCODLOT = $"{riga.Batchno}",
                        MVCODMAT = $""
                    };
                    FileRisposta.Add(nr.ToString());
                    i++;

                }
                else
                {
                    var nr = new RispostaEvasioneDDTAPS()
                    {
                        MVDATDOC = $"{doc.DocDta.Value.ToString("dd/MM/yyyy")}",
                        MVNUMDOC = $"{doc.DocNum2}",
                        MVALFDOC = (string.IsNullOrEmpty(riga.PrdGrp)) ? "XCM" : doc.Info4,
                        MVCODCLI = $"{doc.Info5}",
                        MVSERIAL = $"{doc.Info2}",
                        CPROWNUM = $"{riga.RowInfo1}",
                        MVCODART = $"{riga.PrdCod}",
                        MVDESART = $"{des1}",
                        MVDESSUP = $"{des2}",
                        MVQTAMOV = $"{riga.Qty.Value.ToString("0.00").Replace(",", ".")}",
                        MVCODLOT = (riga.PrdGrp == "MTPR") ? "" : $"{riga.Batchno}",
                        MVCODMAT = $""

                    };
                    FileRisposta.Add(nr.ToString());
                }

            }
            if (!isDafneDDT)
            {
                var filename = $"DDT_{DateTime.Now.ToString("ddMMyyyy_HHmmssfff")}.csv";
                File.WriteAllLines($@"{Path.Combine(PathFileSingoliDDT, filename)}", FileRisposta);
            }
            else
            {
                //PathFileSingoliDafne?
                var filename = $"DDTDAFNE_{DateTime.Now.ToString("ddMMyyyy_HHmmssfff")}.csv";
                File.WriteAllLines($@"{Path.Combine(PathFileSingoliDDT, filename)}", FileRisposta);
            }

        }
        private string RecuperaCodiceClienteAPSDaCodiceMittenteDAFNE(string codDafne)
        {
            if (codDafne == "01318041215")
            {
                return "00734";
            }
            else if (codDafne == "00272680174")
            {
                return "00036";
            }
            else if (codDafne == "00218770881")
            {
                return "00045";
            }
            else if (codDafne == "03048300549")
            {
                return "00052";
            }
            else if (codDafne == "03279221208")
            {
                return "CRM000000001879";
            }
            else if (codDafne == "00269990636")
            {
                return "00465";
            }
            else if (codDafne == "00210470126")
            {
                return "CRM000000001909";
            }
            else if (codDafne == "01323720399")
            {
                return "CRM000000001958";
            }
            else if (codDafne == "00536030828")
            {
                return "CRM000000001962";
            }
            else if (codDafne == "02658190307")
            {
                return "CRM000000001873";
            }
            else if (codDafne == "11985010153")
            {
                return "CRM000000001979";
            }
            else if (codDafne == "02217430343")
            {
                return "00360";
            }
            else if (codDafne == "00123510224")
            {
                return "CRM000000001992";
            }
            else if (codDafne == "03432530925")
            {
                return "CRM000000001991";
            }
            else if (codDafne == "02290110044")
            {
                return "CRM000000001993";
            }
            else if (codDafne == "00055560775")
            {
                return "00362";
            }
            else if (codDafne == "00761840354")
            {
                return "CRM000000000105";
            }
            else if (codDafne == "10151500963")
            {
                return "00597";
            }
            else if (codDafne == "03795140106")
            {
                return "CRM000000001994";
            }
            else if (codDafne == "06398720968")
            {
                return "01292";
            }
            else if (codDafne == "10406510155")
            {
                return "00031";
            }
            else if (codDafne == "01913600902")
            {
                return "CRM000000000099";
            }
            else if(codDafne == "03531670820")
            {
                return "00347";
            }
            else if (codDafne == "01913600902")
            {
                return "CRM000000000099";
            }
            else if (codDafne == "00153470737")
            {
                return "00597";
            }
            else if (codDafne == "00536030828")
            {
                return "00348";
            }
            else if(codDafne == "20049R001W48") 
            {
                return "1612";
            }
            else
            {
                return codDafne;
            }
        }
        private void AnalizzaInvioAPS(DateTime dtn)
        {

            _loggerCode.Info("AnalizzaInvioAPS daNotificareAPS_Dafne:" + daNotificareAPS_Dafne.ToString());
            _loggerCode.Info("AnalizzaInvioAPS dtn.Hour:" + dtn.Hour.ToString());            
            if (daNotificareAPS_Dafne && dtn.Hour >= 16 && dtn.Hour < 17)
            {
                try
                {

                    _loggerCode.Info("Invio dafne.csv ad APS");
                    daNotificareAPS_Dafne = false;
                    _loggerCode.Info("RaggruppaOrdiniDafne");
                    RaggruppaOrdiniDafne();
                }
                catch (Exception rr)
                {
                    _loggerCode.Info("ERRORE RaggruppaOrdiniDafne");
                    _loggerCode.Error(rr);
                }

            }
            else if (!daNotificareAPS_Dafne && dtn.Hour > 17)
            {
                daNotificareAPS_Dafne = true;
            }

            if (daNotificareAPS_DDT && dtn.Hour >= 16 && dtn.Hour < 17)
            {
                try
                {
                    _loggerCode.Info("Invio ddt.csv ad APS");
                    daNotificareAPS_DDT = false;
                    _loggerCode.Info("ERRORE RaggruppaOrdiniDDT");
                    RaggruppaOrdiniDDT();
                }
                catch (Exception rr)
                {
                    _loggerCode.Info("ERRORE RaggruppaOrdiniDDT");
                    _loggerCode.Error(rr);
                }

            }
            else if (!daNotificareAPS_DDT && dtn.Hour > 17)
            {
                daNotificareAPS_DDT = true;
            }
        }
        private void RaggruppaOrdiniDDT()
        {
            if (File.Exists(@"C:\FTP\APS\OUT\DDT\DDT.csv"))
            {
                GestoreMail.SegnalaAllaMandanteCustom("ERRORE: DDT non recuperati", "Ci risulta che il file DDT.csv non è stato ancora recuperato" +
                    "dalla cartella FTP, finchè il file non viene rimosso non è possibile generare un ulteriore file", "magazzino@a-ps.it", new List<string>());
                //daNotificareAPS_DDT = true;
                return;
            }
            if (File.Exists(@"C:\FTP\APS\OUT\DDT\DDTDAFNE.csv"))
            {
                GestoreMail.SegnalaAllaMandanteCustom("ERRORE: DDT non recuperati", "Ci risulta che il file DDTDAFNE.csv non è stato ancora recuperato" +
                    "dalla cartella FTP, finchè il file non viene rimosso non è possibile generare un ulteriore file", "magazzino@a-ps.it", new List<string>());
                //daNotificareAPS_DDT = true;
                return;
            }
            var filesDaRaggruppareDDT = Directory.GetFiles(PathFileSingoliDDT).Where(x => x.Contains("DDT_")).ToList();
            if (filesDaRaggruppareDDT.Count() > 0)
            {
                List<string> righeRaggruppate = new List<string>();
                righeRaggruppate.Add($"MVDATDOC,MVNUMDOC,MVALFDOC,MVCODCLI,MVSERIAL,CPROWNUM,MVCODART,MVDESART,MVDESSUP,MVQTAMOV,MVCODLOT,MVCODMAT");
                foreach (var fls in filesDaRaggruppareDDT)
                {
                    righeRaggruppate.AddRange(File.ReadAllLines(fls));
                    File.Move(fls, $@"C:\FTP\APS\OUT\DDT\SINGOLI\Elaborati\{Path.GetFileName(fls)}");
                }

                var pathCsvRaggruppato = $@"C:\FTP\APS\OUT\DDT\DDT.csv";
                File.AppendAllLines(pathCsvRaggruppato, righeRaggruppate);


            }
            var filesDaRaggruppareDDTDAFNE = Directory.GetFiles(PathFileSingoliDDT).Where(x => x.Contains("DDTDAFNE_")).ToList();
            if (filesDaRaggruppareDDTDAFNE.Count() > 0)
            {
                List<string> righeRaggruppate = new List<string>();
                righeRaggruppate.Add($"MVDATDOC,MVNUMDOC,MVALFDOC,MVCODCLI,MVSERIAL,CPROWNUM,MVCODART,MVDESART,MVDESSUP,MVQTAMOV,MVCODLOT,MVCODMAT");
                foreach (var fls in filesDaRaggruppareDDTDAFNE)
                {
                    righeRaggruppate.AddRange(File.ReadAllLines(fls));
                    File.Move(fls, $@"C:\FTP\APS\OUT\DDT\SINGOLI\Elaborati\{Path.GetFileName(fls)}");
                }

                var pathCsvRaggruppato = $@"C:\FTP\APS\OUT\DDT\DDTDAFNE.csv";
                File.AppendAllLines(pathCsvRaggruppato, righeRaggruppate);

            }
        }
        private void RaggruppaOrdiniDafne()
        {
            var pathCsvRaggruppato = $@"C:\FTP\APS\OUT\DAFNE\ordini.csv";
            if (File.Exists(pathCsvRaggruppato))
            {
                GestoreMail.SegnalaAllaMandanteCustom("ERRORE: Ordini dafne non recuperati", "Ci risulta che il file ordini.csv non è stato ancora recuperato" +
                    "dalla cartella FTP, finchè il file non viene rimosso non è possibile generare un ulteriore file", "ordini@a-ps.it", new List<string>());
                //daNotificareAPS_Dafne = true;
                return;
            }
            var filesDaRaggruppare = Directory.GetFiles(PathFileSingoliDafne);
            if (filesDaRaggruppare.Count() > 0)
            {
                List<string> righeRaggruppate = new List<string>();
                righeRaggruppate.Add($"OR_STATO,ORSERIAL,ORTIPDOC,ORNUMDOC,ORALFDOC,ORDATDOC,ORNUMEST,ORALFEST,ORDATEST,ORTIPCON,ORCODCON,ORCODAGE,ORCODDES,DDNOMDES,DDINDIRI,DD__CAP,DDLOCALI,DDPROVIN,DDCODNAZ,ORCODPAG,MVDESDOC,ORCODVAL,ORSCOCL1,ORSCOCL2,ORSCOPAG,ORFLSCOR,ORSCONTI,ORTDTEVA,ORSPEINC,ORSPETRA,ORSPEIMB,ORCODSPE,ORCODVET,ORCODPOR,ORCONCON,UTCC,UTDC,UTCV,UTDV,OR__NOTE,ORMERISP,ORTOTORD,OR_EMAIL,ORFLMAIL,ORNETMER,ORLOGSTM,ORRIFEST,ORVALNAZ,ORCAOVAL,ORACCONT,ORVALACC,ORSPEBOL,CPROWNUM,ORCODICE,CPROWORD,ORCODART,ORCODVAR,ORDESART,ORTIPRIG,ORDESSUP,ORUNIMIS,ORQTAMOV,ORQTAUM1,ORPREZZO,ORSCONT1,ORSCONT2,ORSCONT3,ORSCONT4,ORSCOIN1,ORSCOIN2,ORSCOIN3,ORSCOIN4,ORCODIVA,ORFLOMAG,ORDATEVA,ORCODLIS,ORCONTRA,ORRIFKIT,ORLOGSTD,ORUMNODI,ORVALSCO");

                foreach (var fls in filesDaRaggruppare)
                {
                    righeRaggruppate.AddRange(File.ReadAllLines(fls));
                    File.Move(fls, $@"C:\FTP\APS\OUT\DAFNE\SINGOLI\Elaborati\{Path.GetFileName(fls)}");
                }

                File.AppendAllLines(pathCsvRaggruppato, righeRaggruppate);
                //Invia mail con allegato
                //if (true)
                //{
                //    List<string> allegati = new List<string>();
                //    allegati.Add(pathCsvRaggruppato);
                //    GestoreMail.SegnalaAllaMandanteCustom("Ordini DAFNE", "In allegato il file con gli ordini di DAFNE\r\n\r\n" +
                //        "Mail automatica, si prega di non rispondere ma rivogesi al customercare@xcmhealthcare.com", "ordini@a-ps.it", allegati);
                //    allegati.Clear();
                //    Thread.Sleep(1000);
                //    File.Delete(pathCsvRaggruppato);
                //}
            }

        }
        #endregion

        private void ControllaSeInseritoManualmente_PH_PH(RootobjectXCMOrderNEW docOsservato)
        {
            if (string.IsNullOrEmpty(docOsservato.header.info1) || string.IsNullOrEmpty(docOsservato.header.info2) ||
                string.IsNullOrEmpty(docOsservato.header.info3) || string.IsNullOrEmpty(docOsservato.header.info4))
            {
                var db = new GnXcmEntities();

                var DocDB = db.uvwWmsDocument.FirstOrDefault(x => x.uniq == docOsservato.header.id);
                int daCazziare = 0;

                if (DocDB != null && DocDB.RecUserID != null)
                {
                    daCazziare = DocDB.RecUserID.Value;
                }

                GestoreMail.InviaMailCustomerErroreInserimentoManuale(docOsservato, daCazziare);
            }

        }
        private void PopolaIDatiMancantiPianificaVivisol(RootobjectXCMOrderNEW docOsservato)
        {
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

            var client = new RestClient(endpointAPI_Espritec + $"/api/wms/document/update");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            var raw = new RootobjectUpdateDocument()
            {
                header = new HeaderUpdateDocument()
                {
                    id = docOsservato.header.id,
                    logWareID = mag,
                    consignee = new ConsigneeUpdateDocument()
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

            var jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            jsetting.DefaultValueHandling = DefaultValueHandling.Ignore;

            var body = JsonConvert.SerializeObject(raw, Newtonsoft.Json.Formatting.Indented, jsetting);
            //_loggerAPI.Info(body);
            request.AddHeader("Authorization", $"Bearer {token_XCM}");
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            IRestResponse responseVivisol = client.Execute(request);
        }
        private void ControllaSeHannoInseritoGliScontiCorrettamente(RootobjectXCMOrderNEW docOsservato)
        {
            string csID = docOsservato.header.customerID;

            var client = new RestClient(endpointAPI_Espritec + $"/api/wms/document/row/list/{docOsservato.header.id}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token_XCM}");
            var response = client.Execute(request);
            var xcmRec = JsonConvert.DeserializeObject<RootobjectXCMRowsNEW>(response.Content);
            var db = new GnXcmEntities();

            var DocDB = db.uvwWmsDocument.FirstOrDefault(x => x.uniq == docOsservato.header.id);
            int daCazziare = 0;

            if (DocDB != null && DocDB.RecChangeUserID != null)
            {
                daCazziare = DocDB.RecChangeUserID.Value;
            }

            if (csID == "00002" || csID == "00003" || csID == "00006" || csID == "00008" || csID == "00011")
            {
                if (xcmRec.rows.Any(x => x.discount < 5))
                {
                    GestoreMail.InviaMailCustomerDiscount(docOsservato, daCazziare);
                }
            }
        }
        #endregion

        /// <summary>
        /// Recupera righe di registrazione del documento con lotto e scadenza aggiornati da posizionamento
        /// </summary>
        /// <param name="docOsservato">documento DB d'appoggio</param>
        /// <returns>ritorna tutte le righe di registrazione appartenente al documento passato in ingresso</returns>
        private RootobjectXCMRowsNEW RecuperaRigheRegistrazioneDocumentoXCM(RootobjectXCMOrderNEW docOsservato)
        {

            var client = new RestClient(endpointAPI_Espritec + $"/api/wms/registration/list/500/1/?RegTypeGroup=Move&DocID={docOsservato.header.id}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token_XCM}");
            var response = client.Execute(request);
            var xcmRec = JsonConvert.DeserializeObject<RootobjectXCMRegistrations>(response.Content);

            if (xcmRec.result.status)
            {
                var nuoveRighe = new List<RowXCMRowsNew>();
                foreach (var r in xcmRec.registrations)
                {
                    if (r.totalQty < 1) continue;
                    var nrr = new RowXCMRowsNew()
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
                var xcmDoc = new RootobjectXCMRowsNEW();
                xcmDoc.rows = nuoveRighe.ToArray();
                xcmDoc.result = new ResultXCMRowsNEW()
                {
                    status = true,
                };
                return xcmDoc;
            }
            else
            {
                var resp = new RootobjectXCMRowsNEW()
                {
                    result = new ResultXCMRowsNEW()
                    {
                        status = false
                    }
                };
                return resp;
            }
        }
        private RootobjectXCMRowsNEW RecuperaRigheRegistrazioneDocumentoXCMPHPH(RootobjectXCMOrderNEW docOsservato)
        {
            var client = new RestClient(endpointAPI_Espritec + $"/api/wms/registration/list/500/1/?RegTypeGroup=Move&DocID={docOsservato.header.id}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token_XCM}");
            var response = client.Execute(request);
            var xcmRec = JsonConvert.DeserializeObject<RootobjectXCMRegistrations>(response.Content);

            if (xcmRec.result.status)
            {
                var nuoveRighe = new List<RowXCMRowsNew>();
                foreach (var r in xcmRec.registrations)
                {
                    if (r.totalQty < 1) continue;
                    var nrr = new RowXCMRowsNew()
                    {
                        partNumber = r.partNumber,
                        batchNo = r.batchNo,
                        expireDate = r.dateExpire,
                        id = r.id,
                        qty = r.totalQty,
                        partNumberDes = r.partNumberDescription,

                    };
                    nuoveRighe.Add(nrr);


                }
                var xcmDoc = new RootobjectXCMRowsNEW();
                xcmDoc.rows = nuoveRighe.ToArray();
                xcmDoc.result = new ResultXCMRowsNEW()
                {
                    status = true,
                };
                return xcmDoc;
            }
            else
            {
                var resp = new RootobjectXCMRowsNEW()
                {
                    result = new ResultXCMRowsNEW()
                    {
                        status = false
                    }
                };
                return resp;
            }
        }
        /// <summary>
        /// Crea oggetto righe da API GESPE
        /// </summary>
        /// <param name="docOsservato">si passa il documento presente sul DB d'interscambio</param>
        /// <returns>ritorna oggetto righe documento GESPE</returns>
        private RootobjectXCMRowsNEW RecuperaRigheDocumentoXCM(RootobjectXCMOrderNEW docOsservato)
        {
            RootobjectXCMRowsNEW xcmDocRows = null;
            var client = new RestClient(endpointAPI_Espritec + $"/api/wms/document/row/list/{docOsservato.header.id}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token_XCM}");
            var response = client.Execute(request);
            return xcmDocRows = JsonConvert.DeserializeObject<RootobjectXCMRowsNEW>(response.Content);

        }

        /// <summary>
        /// Provvede alla creazione e all'invio tramite mail (dalla classe GestoreMail) della BEM d'ingresso 
        /// dopo essere stata controllata dall'ufficio logistica
        /// </summary>
        /// <param name="doc">documento DB d'appoggio</param>
        /// <param name="docOsservato">documento gespe</param>
        private void ProduciEdInviaBEMalCliente(RootobjectXCMOrderNEW docOsservato)
        {
            try
            {
                var db = new InterscambioAPIEntities();
                var mandante = db.ANAGRAFICA_MANDANTI.First(x => x.ID_MANDANTE_GESPE == docOsservato.header.customerID);

                if (!mandante.INVIA_DOCUMENTO_BEM)
                {
                    return;
                }
                List<string> pathDocumentiProdotti = new List<string>();
                RootobjectXCMRowsNEW RigheRegistrazione = RecuperaRigheRegistrazioneDocumentoXCM(/*doc*/docOsservato);
                RootobjectXCMRowsNEW RigheAPI = RecuperaRigheDaAPI(docOsservato);
                if (!RigheRegistrazione.result.status)
                {
                    throw new Exception($"Errore produzione BEM per il docNumber righe registrazioni non presenti {docOsservato.header.docNumber}");
                }
                if (docOsservato.header.customerID == "00018" || docOsservato.header.customerID == "00007" || docOsservato.header.customerID == "00002" || docOsservato.header.customerID == "00016")
                {
                    pathDocumentiProdotti.Add(DocBEM.ProduciTracciatoXLSBEM(RigheRegistrazione, docOsservato));
                }

                pathDocumentiProdotti.Add(DocBEM.produciDocumentoBEM(RigheRegistrazione, docOsservato, RigheAPI));

                if (pathDocumentiProdotti.Count > 0)
                {
                    GestoreMail.SendMailBEM(pathDocumentiProdotti, mandante.MAIL_NOTIFICA_BEM);
                    _loggerCode.Info($"INVIATA BEM AL CLIENTE {mandante.NOME_MANDANTE} - {docOsservato.header.docNumber}");
                }
                else
                {
                    throw new Exception($"Errore produzione BEM 0 documenti prodotti per il docNumber {docOsservato.header.docNumber}");
                }
            }
            catch (Exception ee)
            {

                _loggerCode.Error($"Analisi del documento {docOsservato.header.id} {ee}");

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("ProduciEdInviaBEMalCliente", ee);
                }

                LastException = ee;
            }

        }
        private RootobjectXCMRowsNEW RecuperaRigheDaAPI(RootobjectXCMOrderNEW docOsservato)
        {
            var client = new RestClient(endpointAPI_Espritec + $"/api/wms/document/row/list/{docOsservato.header.id}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token_XCM}");
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<RootobjectXCMRowsNEW>(response.Content);
        }
        private Shipment RecuperaShipConnessoByAPI(int shipID)
        {
            //string ts = (DateTime.Now - TimeSpan.FromDays(2)).ToString("s", CultureInfo.InvariantCulture);
            var resp = new Shipment();
            var clientLink = new RestClient(endpointAPI_Espritec + $"/api/tms/shipment/get/{shipID}");
            clientLink.Timeout = -1;
            var requestLink = new RestRequest(Method.GET);
            requestLink.AddHeader("Authorization", $"Bearer {token_XCM}");
            IRestResponse responseLink = clientLink.Execute(requestLink);
            if (responseLink.IsSuccessful)
            {
                var shipR = JsonConvert.DeserializeObject<RootobjectShipment>(responseLink.Content);
                resp = shipR.shipment;
            }
            return resp;
        }
        private uvwTmsShipment RecuperaShipConnessoByDB(int shipID)
        {
            var DB = new GnXcmEntities();
            return DB.uvwTmsShipment.FirstOrDefault(x => x.Uniq == shipID);
        }

        #region PH_PH
        /// <summary>
        /// Connettore per produzione tracciati Navision per il cliente Pharmaday
        /// </summary>
        /// <param name="isIngresso">indicare se il documento passato è per procedura d'ingresso o di uscita</param>
        /// <param name="doc">documento DB d'appoggio</param>
        /// <param name="docOsservato">documento gespe</param>
        private void CreaFileInterscambioPHPH(bool isIngresso, RootobjectXCMOrderNEW docOsservato)
        {
            //Occorre il doc in quanto come ultima cosa deve eliminare la voce dal db mio
            try
            {
                _loggerCode.Debug($"ingresso {isIngresso} docid {docOsservato.header.id}");
                if (isIngresso)
                {
                    //Produci txt BEM
                    var db = new GnXcmEntities();

                    #region Testata Documento
                    string testata = "";
                    List<string> righe = new List<string>();
                    var d = db.uvwWmsDocument.FirstOrDefault(x => x.uniq == docOsservato.header.id);

                    string numRif = d.DocNum2;
                    //string refer = d.Reference.PadRight(10).Substring(0, 10);

                    #region Data Documento
                    string data = "";
                    if (d.DocDta != null)
                    {
                        data = 1 + d.DocDta.Value.ToString("yyMMdd");
                    }
                    else
                    {
                        data = 0 + DateTime.MinValue.ToString("yyMMdd");
                    }
                    #endregion

                    if (d.Info1 == null)
                    {
                        var ex = new Exception($"Errore nella produzione del file di scambio con PH_PH estremi del documento: ID {docOsservato.header.id} Number {docOsservato.header.docNumber} mancano i dati stabiliti nel tracciato");
                        GestoreMail.SegnalaErroreDev(ex);
                        return;
                    }

                    //<Tipo Record>	1	6 ok
                    //<Divisione>	7	2 ok
                    //<Tipo Ordine>	9	3 ok
                    //<Nr Ordine>	12	10 ok
                    //<Nr Ricevimento>	22	10 ko 0000001
                    //<Cod. For/Cli>	29	10 ko
                    //<Edificio>	39	1 ko sempre 1 
                    //<Nr DDT Fornitore>	40	10 ok
                    //<Data DDT Fornitore>	50	 7 ok
                    //<Ultimo Nr Carico>		20 ko
                    //<User ID>		20 ko
                    string codF = "2589"; // statico
                    string nDDT = d.Reference;

                    while (nDDT.Length < 10)
                    {
                        nDDT = nDDT.Insert(0, "0");
                    }
                    //if(d.Reference2== null)//TODO:
                    //{
                    //    d.Reference2 = d.Reference;
                    //}
                    //Divisione ok        Tipo Ordine ok         Nr Ordine ok         Nr Ricevimento ko       Cod. For/Cli sessoCAR      Edificio ko  Nr DDT Fornitore ok   Data DDT Fornitore ok
                    testata = $"HSTAHD{d.Info1.PadRight(2)}{d.Info2.PadRight(3)}{d.Reference.PadRight(10)}{"0000001"}{codF.PadRight(10)}{"1"}{nDDT}{data}";
                    righe.Add(testata);
                    #endregion

                    #region Righe Documento
                    //var r = db.uvwWmsDocumentRows.Where(x => x.DocNum == d.DocNum && x.CustomerID == d.CustomerID).ToList();
                    //var r = db.uvwWmsDocumentRows.Where(x => x.uniq == doc.docID).ToList();
                    RootobjectXCMRowsNEW RigheRegistrazione = RecuperaRigheRegistrazioneDocumentoXCMPHPH(docOsservato);
                    var tt = RigheRegistrazione.rows.OrderBy(x => x.partNumber).ToList();
                    var righeBEM = RecuperaRigheDocumentoXCM(docOsservato);

                    var RigheRegistrateNormalizzate = new RootobjectXCMRowsNEW();
                    var ListaRigheRaggruppatePerQuantita = new List<RowXCMRowsNew>();

                    foreach (var rr in RigheRegistrazione.rows)
                    {
                        var esiste = ListaRigheRaggruppatePerQuantita.FirstOrDefault(x => x.partNumber == rr.partNumber && x.batchNo == rr.batchNo && x.info2 != rr.info2);
                        if (esiste != null)
                        {
                            esiste.qty += rr.qty;
                        }
                        else
                        {
                            ListaRigheRaggruppatePerQuantita.Add(rr);
                        }
                    }
                    RigheRegistrateNormalizzate.rows = ListaRigheRaggruppatePerQuantita.OrderBy(x => x.partNumber).ToArray();
                    //int cr = 1;
                    foreach (var rec in righeBEM.rows)
                    {
                        string riga = "";
                        var righeMovCorr = RigheRegistrateNormalizzate.rows.Where(x => x.partNumber == rec.partNumber).ToList();
                        if (righeMovCorr.Count() > 1)//controlla se quantità corrisponde a riga originale
                        {

                            RowXCMRowsNew rigaCerta = righeMovCorr.FirstOrDefault(x => x.qty == rec.qty);
                            if (rigaCerta != null)
                            {
                                rec.batchNo = rigaCerta.batchNo;
                                rec.expireDate = rigaCerta.expireDate;
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(rec.info1))
                                {
                                    if (righeMovCorr[0].qty > 0)
                                    {
                                        //righeMovCorr[0].qty = righeMovCorr[0].qty - rec.qty;
                                        if (righeBEM.rows.Count() == 1)
                                        {
                                            rec.info1 = righeBEM.rows[0].info1;
                                        }
                                        else
                                        {
                                            rec.info1 = righeMovCorr[0].info1;
                                        }

                                    }
                                    else
                                    {
                                        //non dovrebbe mai verificarsi altrimenti sono uccelli diabetici
                                    }
                                }

                            }
                        }
                        else if (righeMovCorr.Count() == 1)
                        {
                            rec.batchNo = righeMovCorr[0].batchNo;
                            rec.expireDate = righeMovCorr[0].expireDate;
                        }
                        else
                        {
                            //CHE FACCIO SE NON TROVO RIGHE MOVIMENTATE????? LE INSERISCO A QUANTITA' 0 O LE ESCLUDO?
                            continue;
                        }

                        #region Calcolo Quantità
                        string qta = ((int)rec.qty).ToString();

                        while (qta.Length < 8)
                        {
                            qta = qta.Insert(0, "0");
                        }
                        qta = qta + "000";
                        #endregion

                        #region CalcoloScadenza
                        string dtaExp = "";
                        if (rec.expireDate != null)
                        {
                            dtaExp = 1 + rec.expireDate.Value.ToString("yyMMdd");
                        }
                        else
                        {
                            dtaExp = 0 + DateTime.MinValue.ToString("yyMMdd");
                        }
                        #endregion

                        //string crs = cr.ToString();
                        //while (crs.Length < 5)
                        //{
                        //    crs = crs.Insert(0, "0");
                        //}
                        //cr++;

                        riga = $"HSTADT{d.Info1.PadRight(2)}{d.Info2.PadRight(3)}{d.Info7.PadRight(10)}{"0000001"}{rec.partNumber.PadRight(25)}{"PZ".PadRight(3)}{rec.info1.Substring(0, 5)}" +
                            $"{qta}DISP{rec.batchNo.PadRight(20)}{"001"}{dtaExp}";

                        righe.Add(riga);
                    }
                    #endregion
                    string nomeFile = $"COF_{DateTime.Now.ToString("yyyyMMdd_hhmmssffff")}.txt";//controlla nome file!
                    string Dir = @"C:\FTP\PH_PH\OUT";

                    if (!Directory.Exists(Dir)) Directory.CreateDirectory(Dir);

                    string finalDest = Path.Combine(Dir, nomeFile);
                    File.AppendAllLines(finalDest, righe);
                }
                else
                {
                    //Produci txt DDT
                    var db = new GnXcmEntities();
                    string yy = DateTime.Now.ToString("yy");
                    #region Testata Documento
                    string testata = "";
                    List<string> righe = new List<string>();
                    var d = db.uvwWmsDocument.Where(x => x.uniq == docOsservato.header.id).FirstOrDefault();

                    string numRif = d.DocNum2;
                    string refer = d.Reference;

                    while (numRif.Length < 5)
                    {
                        numRif = numRif.Insert(0, "0");
                    }
                    if (!numRif.StartsWith(yy)) { numRif = yy + numRif; }

                    #region Data Documento
                    string data = "";
                    if (d.DocDta != null)
                    {
                        data = 1 + d.DocDta.Value.ToString("yyMMdd");
                    }
                    else
                    {
                        data = 0 + DateTime.MinValue.ToString("yyMMdd");
                    }
                    #endregion

                    testata = $"HSTPKH{d.Info1.PadRight(2)}{d.Info2.PadRight(3)}{refer.PadRight(20)}{numRif.PadRight(10)}{data}{d.Info3.PadRight(10)}{d.Info4.PadRight(10)}    ";
                    righe.Add(testata);
                    #endregion
                    //YY00003 doc
                    #region Righe Documento
                    var dtn = DateTime.Now;
                    var tt = new List<uvwWmsDocumentRows_XCM>();
                    var r = db.uvwWmsDocumentRows_XCM.Where(x => x.DocNum == d.DocNum && x.CustomerID == d.CustomerID && x.DocDta.Value.Year == dtn.Year).ToList();

                    foreach (var rrr in r)
                    {
                        var esiste = tt.FirstOrDefault(x => x.PrdCod == rrr.PrdCod && x.Batchno == rrr.Batchno);
                        if (esiste != null)
                        {
                            esiste.Qty += rrr.Qty;
                        }
                        else
                        {
                            tt.Add(rrr);
                        }
                    }

                    foreach (var rec in tt)
                    {
                        string riga = "";

                        #region Calcolo Quantità
                        string qta = "0";
                        if (rec.Qty != null)
                        {
                            qta = ((int)rec.Qty.Value).ToString();
                        }
                        while (qta.Length < 8)
                        {
                            qta = qta.Insert(0, "0");
                        }
                        qta = qta + "000";
                        #endregion

                        #region CalcoloScadenza
                        string dtaExp = "";
                        if (rec.DateExpire != null)
                        {
                            dtaExp = 1 + rec.DateExpire.Value.ToString("yyMMdd");
                        }
                        else
                        {
                            dtaExp = 0 + DateTime.MinValue.ToString("yyMMdd");
                        }
                        #endregion

                        riga = $"HSTPKD{rec.HeaderInfo1.PadRight(2)}{refer.PadRight(20)}{numRif.PadRight(10)}{data}{rec.RowInfo2.PadRight(7)}{rec.PrdCod.PadRight(25)}{qta}{rec.Batchno.PadRight(20)}{dtaExp}";

                        righe.Add(riga);
                    }
                    #endregion
                    var dtf = DateTime.Now.ToString("yyyyMMdd_HHmmssffff");
                    string nomeFile = $"CON_{dtf}.txt";
                    string Dir = @"C:\FTP\PH_PH\OUT";

                    if (!Directory.Exists(Dir)) Directory.CreateDirectory(Dir);

                    string finalDest = Path.Combine(Dir, nomeFile);
                    File.AppendAllLines(finalDest, righe);
                }
            }
            catch (Exception ee)
            {
                string msg = $"Errore durante l'analisi del documento {docOsservato.header.docNumber}";
                _loggerCode.Error(msg);
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("CreaFileInterscambioPHPH", msg + "\r\n\r\n" + ee);
                }

                LastException = ee;
            }
        }
        #endregion

        #region VIVISOL
        List<long> DocumentiGiaTrasmessi = new List<long>();

        /// <summary>
        /// invia a vivisol il progresso di preparazione per il documento interessato
        /// </summary>
        /// <param name="docOsservato">documento gespe</param>
        private void InviaAVivisolPreparazioneInCorso(RootobjectXCMOrderNEW docOsservato)
        {
            var db = new GnXcmEntities();
            var testataDocumentoLinked = db.uvwWmsDocument.First(x => x.uniq == docOsservato.header.id);
            var client = new RestClient("https://api-production.solgroup.com/xcm-experience/api/invioPreparazioneCarico");
            client.Timeout = -1;
            var request = new RestRequest(Method.PATCH);
            request.AddHeader("Authorization", "Basic MlNzSHJtZ2NhVFVtSldxQUc2M0s6Vml2aXNvbFBvcnRhbHMhMjA=");
            request.AddHeader("Content-Type", "application/json");
            var tt = testataDocumentoLinked.Info6;
            if (string.IsNullOrEmpty(tt))
            {
                return;
            }
            var tt2 = testataDocumentoLinked.ExternalID;
            var raw = new RootobjectInvioPreparazioneCarico()
            {
                idRouting = tt,
                system = "Sintesi_Italia"
            };
            var body = JsonConvert.SerializeObject(raw, Newtonsoft.Json.Formatting.Indented);
            _loggerAPI.Info($"PLAN {docOsservato.header.reference} preparata");
            _loggerAPI.Info(body);

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            IRestResponse response = client.Execute(request);
            _loggerAPI.Info(response.Content);
        }
        /// <summary>
        /// Connettore API con Vivisol
        /// </summary>
        /// <param name="isIngresso">indica se la procedura va effettuata per un documento in ingresso (BEM) o in uscita (DDT)</param>
        /// <param name="doc">documento DB d'appoggio</param>
        /// <param name="docOsservato">documento gespe</param>
        private void ComunicazioneAPIVivisol(bool isIngresso, RootobjectXCMOrderNEW docOsservato)
        {
            string hh = "";
            try
            {
                LOG.ScriviLogManuale("START ComunicazioneAPIVivisol");
                if (DocumentiGiaTrasmessi.Contains(docOsservato.header.id))
                {
                    GestoreMail.SegnalaErroreDev($"Documento già trattato", $"non faccio nulla per la trasmissione del documento {docOsservato.header.docNumber}");
                    _loggerCode.Warn($"Documento già trattato non faccio nulla per la trasmissione del documento {docOsservato.header.docNumber}");
                }
                if (isIngresso)
                {
                    LOG.ScriviLogManuale("STEP1 ComunicazioneAPIVivisol");
                    string ordID = "";
                    if (docOsservato.header.reference.StartsWith("300") && string.IsNullOrEmpty(docOsservato.header.internalNote))
                    {
                        //sono i decessi non occorre dare comunicazione a vivisol in quanto sol ha già effettuato i movimenti
                        LOG.ScriviLogManuale("EXIT 1 ComunicazioneAPIVivisol");
                        return;

                        //Hanno cambiato idea, ora si fanno
                        //ordID = docOsservato.header.externalID;
                    }

                    LOG.ScriviLogManuale("STEP2 ComunicazioneAPIVivisol");
                    RootobjectXCMRowsNEW RigheDocRegistrazione = RecuperaRigheRegistrazioneDocumentoXCM(docOsservato);
                    if (RigheDocRegistrazione.rows.Any(x => string.IsNullOrEmpty(x.batchNo)))
                    {
                        RigheDocRegistrazione = RecuperaRigheDocumentoXCM(docOsservato);
                    }

                    RootobjectXCMOrderNEW docOsservatoLink = null;

                    LOG.ScriviLogManuale("STEP3 ComunicazioneAPIVivisol");
                    if (docOsservato.links != null && docOsservato.links.Count() == 1)
                    {
                        var clientLink = new RestClient(endpointAPI_Espritec + $"/api/wms/document/get/{docOsservato.links[0].id}");
                        clientLink.Timeout = -1;
                        var requestLink = new RestRequest(Method.GET);
                        requestLink.AddHeader("Authorization", $"Bearer {token_XCM}");
                        IRestResponse responseLink = clientLink.Execute(requestLink);
                        docOsservatoLink = JsonConvert.DeserializeObject<RootobjectXCMOrderNEW>(responseLink.Content);
                    }
                    else if (docOsservato.links != null && docOsservato.links.Count() > 1)
                    {
                        LOG.ScriviLogManuale("ERROR 1 ComunicazioneAPIVivisol");
                        throw new Exception("PRESENTI PIU' DOCUMENTI LINKATI PER LA BEM");
                    }
                    //else if (docOsservato.links == null && RigheDocRegistrazione.rows != null && !RigheDocRegistrazione.rows.Any(x => x.logWareID == "ASLNANORD"))//se magazzino logico asl deve proseguire
                    //{
                    //    _loggerAPI.Info($"BEM ASLNANORD IN EVASIONE {docOsservato.header.id}");
                    //    //return;//Inserito per errore mulesoft dopo valutazione di ingresso senza external id
                    //}

                    LOG.ScriviLogManuale("STEP4 ComunicazioneAPIVivisol");
                    if (RigheDocRegistrazione.result != null && RigheDocRegistrazione.result.status)
                    {
                        var db = new InterscambioAPIEntities();
                        var RigheDoc = RecuperaRigheDocumentoXCM(docOsservato);

                        LOG.ScriviLogManuale("STEP5 ComunicazioneAPIVivisol");
                        foreach (var r in RigheDocRegistrazione.rows)//NON DEVE MAI ACCADERE CHE AGGIUNGANO VOCI MANUALMENTE IN BEM
                        {
                            try
                            {
                                RowXCMRowsNew pres = RigheDoc.rows.FirstOrDefault(x => x.partNumber == r.partNumber);
                                string LogMag = "";
                                if (docOsservato.links != null)
                                {
                                    if (pres != null)
                                    {
                                        LogMag = pres.info1;
                                    }
                                }
                                if (string.IsNullOrEmpty(LogMag))
                                {
                                    var rigaCorr = RigheDoc.rows.Where(x => x.partNumber == r.partNumber).ToList();

                                    if (rigaCorr != null && rigaCorr.Count == 1)
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
                                            LOG.ScriviLogManuale("ERRORE 2: ComunicazioneAPIVivisol");
                                            GestoreMail.SegnalaErroreDev("Vivisol Magazzino non rilevato", $"id documento in errore {docOsservato.header.id}");
                                            return;
                                        }
                                    }

                                    //var corr = righeLink.rows.FirstOrDefault(x => x.partNumber == r.partNumber);
                                    if (string.IsNullOrEmpty(r.logWareID))
                                    {
                                        LOG.ScriviLogManuale("ERRORE 3 ComunicazioneAPIVivisol");
                                        GestoreMail.SegnalaErroreDev("Vivisol Magazzino non rilevato", $"id documento in errore {docOsservato.header.id}");
                                        return;
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

                                LOG.ScriviLogManuale("STEP6 ComunicazioneAPIVivisol");
                                var raw = new RootobjectEntrataMerce()
                                {
                                    batch = r.batchNo,
                                    expiryDate = r.expireDate.Value.ToUniversalTime().ToString("o"),
                                    logWareId = LogMag,
                                    movementDate = DateTime.Now.ToUniversalTime().ToString("o"),
                                    partNumber = r.partNumber,
                                    quantity = r.qty,

                                    note = ""
                                };


                                if (string.IsNullOrEmpty(ordID))
                                {
                                    raw.orderId = (docOsservatoLink != null) ? docOsservatoLink.header.externalID : "";
                                    raw.orderDetailId = (pres != null) ? pres.linkedExternalID : "";
                                }
                                else
                                {
                                    raw.orderId = ordID;
                                    raw.orderDetailId = pres.externalID;
                                }

                                //TODO:Claudio verifica API Vivisol
                                LOG.ScriviLogManuale("STEP7 ComunicazioneAPIVivisol");

                                var clientVivisol = new RestClient("https://api-production.solgroup.com/xcm-experience/api/entrataMerce");
                                clientVivisol.Timeout = -1;
                                var requestVivisol = new RestRequest(Method.POST);
                                var body = JsonConvert.SerializeObject(raw, Newtonsoft.Json.Formatting.Indented);
                                _loggerAPI.Info(body);
                                requestVivisol.AddHeader("Authorization", "Basic MlNzSHJtZ2NhVFVtSldxQUc2M0s6Vml2aXNvbFBvcnRhbHMhMjA=");
                                requestVivisol.AddParameter("application/json", body, ParameterType.RequestBody);

                                clientVivisol.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
                                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                                IRestResponse responseVivisol = clientVivisol.Execute(requestVivisol);

                                _loggerAPI.Info(responseVivisol.Content);
                                hh = responseVivisol.Content;
                            }

                            catch (Exception ee)
                            {
                                LOG.ScriviLogManuale("ERRORE 4 ComunicazioneAPIVivisol");
                                LOG.ScriviLog(ref ee);
                                _loggerAPI.Error("content: " + hh);
                            }
                        }
                    }
                    else
                    {
                        LOG.ScriviLogManuale("ERRORE 5 ComunicazioneAPIVivisol");
                        string msg = $"Non sono state trovate righe di registrazione per il documento {docOsservato.header}";
                        _loggerAPI.Warn(msg);
                        throw new Exception(msg);
                    }
                }
                else
                {
                    LOG.ScriviLogManuale("STEP7 ComunicazioneAPIVivisol");
                    RootobjectXCMOrderNEW docOsservatoLink = null;
                    //if(doc == null)
                    //{
                    //    doc = new XCM_TrackingDocument();
                    //    doc.id = docOsservato.header.id;
                    //}
                    RootobjectXCMRowsNEW RigheDoc = RecuperaRigheDocumentoXCM(docOsservato);
                    var DB = new GnXcmEntities();
                    //var shipConnesso = DB.uvwTmsShipment.FirstOrDefault(x => x.DocNum ==  docOsservato.header.shipDocNumber);

                    Shipment shipConnesso = RecuperaShipConnessoByAPI(docOsservato.header.shipID);
                    LOG.ScriviLogManuale("STEP8 ComunicazioneAPIVivisol");
                    if (docOsservato.links != null && docOsservato.links.Count() == 1)
                    {
                        var clientLink = new RestClient(endpointAPI_Espritec + $"/api/wms/document/get/{docOsservato.links[0].id}");
                        clientLink.Timeout = -1;
                        var requestLink = new RestRequest(Method.GET);
                        requestLink.AddHeader("Authorization", $"Bearer {token_XCM}");
                        IRestResponse responseLink = clientLink.Execute(requestLink);
                        docOsservatoLink = JsonConvert.DeserializeObject<RootobjectXCMOrderNEW>(responseLink.Content);
                    }
                    else if (docOsservato.links != null && docOsservato.links.Count() > 1)
                    {
                        var collegatoDiretto = docOsservato.links.Where(x => x.docType == "OrderOUT");
                        if (collegatoDiretto.Count() > 1)
                        {
                            LOG.ScriviLogManuale("ERRORE 6 ComunicazioneAPIVivisol");
                            throw new Exception("PRESENTI PIU' DOCUMENTI LINKATI PER LA BEM");
                        }
                    }

                    var db = new GnXcmEntities();
                    var testataDocumentoLinked = db.uvwWmsDocument.First(x => x.uniq == docOsservatoLink.header.id);

                    if (string.IsNullOrEmpty(testataDocumentoLinked.ExternalID))
                    {
                        return;
                    }
                    if (RigheDoc.result.status)
                    {
                        foreach (var record in RigheDoc.rows)
                        {
                            string LogMag = "";
                            if (record.logWareID == "VIVISOLNA")
                            {
                                LogMag = "243270";
                            }
                            else
                            {
                                LogMag = "243310";
                            }
                            var client = new RestClient("https://api-production.solgroup.com/xcm-experience/api/invioMovimenti");
                            client.Timeout = -1;
                            var request = new RestRequest(Method.POST);
                            var dataDocO = docOsservato.header.docDate.ToUniversalTime().ToString("o");//docOsservato.header.docDate;

                            var numC = 0M;

                            if (shipConnesso != null && shipConnesso.packs != 0)
                            {
                                numC = shipConnesso.packs;
                            }
                            else
                            {
                                LOG.ScriviLogManuale("ERRORE 7 ComunicazioneAPIVivisol");
                                throw new Exception($"Non sono riuscito a recuperare lo ship connesso al DDT ");
                            }

                            var raw = new RootobjectInvioMovimenti()
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
                                pallet = DammiBarCodePallet(testataDocumentoLinked.uniq),//(docOsservato.header.id),
                                system = "Sintesi_Italia"
                            };

                            LOG.ScriviLogManuale("STEP9 ComunicazioneAPIVivisol");
                            var body = JsonConvert.SerializeObject(raw, Newtonsoft.Json.Formatting.Indented);
                            _loggerAPI.Info(body);
                            request.AddHeader("Authorization", "Basic MlNzSHJtZ2NhVFVtSldxQUc2M0s6Vml2aXNvbFBvcnRhbHMhMjA=");
                            request.AddHeader("Content-Type", "application/json");
                            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
                            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                            request.AddParameter("application/json", body, ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);
                            _loggerAPI.Info(response.Content);

                        }
                    }
                    else
                    {
                        LOG.ScriviLogManuale("STEP10 ComunicazioneAPIVivisol");
                    }
                }
                DocumentiGiaTrasmessi.Add(docOsservato.header.id);
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    LOG.ScriviLogManuale("ERRORE 10 ComunicazioneAPIVivisol");
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("ComunicazioneAPIVivisol", ee);
                }
                LastException = ee;
            }
        }
        /// <summary>
        /// recupera il barcode pallet per vivisol
        /// </summary>
        /// <param name="ordID">id ordine gespe</param>
        /// <returns><strong>ritorna stringa barcone128 XCMxxxxxxxxx</strong></returns>
        private string DammiBarCodePallet(int ordID)
        {

            var doc = new GnXcmEntities().uvwWmsDocument.FirstOrDefault(x => x.uniq == ordID);
            var orm = doc.DocNum;
            var wmsDocumentOrm = orm.Split('/')[0];

            var suffisso = $"XCM{DateTime.Now.ToString("yy")}";
            while (wmsDocumentOrm.Length < 7)
            {
                wmsDocumentOrm = wmsDocumentOrm.Insert(0, "0");
            }
            return $"{suffisso}{wmsDocumentOrm}";

            #region OLD
            //var resp = ordID.ToString();
            //while (resp.Length < 9)
            //{
            //    resp = resp.Insert(0, "0");
            //}
            //return $"XCM{resp}"; 
            #endregion
        }
        #endregion

        #region test
        private void SincronizzaSpedizioni()
        {
            var db = new GnCommonXcmEntities();
            RecuperaConnessione();
            string startDate = (DateTime.Now - TimeSpan.FromDays(30)).ToString("o");
            var result = new List<EspritecShipment.RootobjectShipmentList>();
            var pageNumber = 1;
            var pageRows = 50;
            var resource = $"/api/tms/shipment/list/{pageRows}/{pageNumber}?StartDate={startDate}";
            var client = new RestClient("https://010761.espritec.cloud:9500");
            var request = new RestRequest(resource, Method.GET);

            client.Timeout = -1;
            request.AddHeader("Authorization", $"Bearer {token_UNITEX}");
            request.AlwaysMultipartFormData = true;
            IRestResponse response = client.Execute(request);

            var resp = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(response.Content);

            if (resp != null && resp.shipments != null)
            {
                StoricizzaDBCRMShipment(resp);
                var maxPages = resp.result.maxPages;

                while (maxPages > 1)
                {
                    pageNumber++;
                    maxPages--;
                    resource = $"/api/tms/shipment/list/{pageRows}/{pageNumber}?StartDate={startDate}";
                    request = new RestRequest(resource, Method.GET);
                    request.AddHeader("Authorization", $"Bearer {token_UNITEX}");
                    request.AlwaysMultipartFormData = true;
                    response = client.Execute(request);
                    resp = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(response.Content);

                    if (resp != null && resp.shipments != null)
                    {
                        StoricizzaDBCRMShipment(resp);
                    }
                }
            }
        }
        private void StoricizzaDBCRMShipment(EspritecShipment.RootobjectShipmentList ships)
        {
            try
            {
                var db = new GnXcmEntities();
                var dbCRM = new XCM_CRMEntities();
                DateTime? dtNull = null;
                foreach (var shipment in ships.shipments)
                {
                    uvwWmsDocument documentoInEsame = db.uvwWmsDocument.FirstOrDefault(x => x.DocNum.StartsWith(shipment.externRef));
                    var esiste = dbCRM.ShipmentList.FirstOrDefault(x => x.Shipment_GespeID_UNITEX == shipment.id);
                    if (esiste != null && esiste.Shipment_StatusID != shipment.statusId)
                    {
                        esiste.Shipment_StatusID = shipment.statusId;
                        esiste.Shipment_StatusDes = shipment.statusDes;
                        db.SaveChanges();
                        //aggiorna tracking non necessario, si effettua chiamata diretta su API Unitex
                    }
                    else
                    {
                        if (documentoInEsame == null) continue;
                        var cid = UtentiCRM.FirstOrDefault(x => x.Customer_id == documentoInEsame.CustomerID);
                        if (cid == null || !cid.Customer_IsEnableCRM) continue;
                    }

                    //List<Tracking> TrackingsOrder = new List<Tracking>();
                    //var trs = EspritecShipment.RestEspritecGetTracking(shipment.id, token_UNITEX);
                    //var trsDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentTracking>(trs.Content);
                    //if (trsDes.events == null) continue;
                    //foreach (var t in trsDes.events)
                    //{
                    //    Tracking tr = new Tracking()
                    //    {
                    //        Tracking_Data = t.timeStamp,
                    //        Tracking_ShipmentID = shipment.id,
                    //        Tracking_StatusDes = t.statusDes,
                    //        Tracking_StatusID = t.statusID
                    //    };
                    //    TrackingsOrder.Add(tr);
                    //}

                    var ns = new EntityModels.ShipmentList()
                    {
                        Shipment_AttachCount = shipment.attachCount,
                        Shipment_CashCurrency = shipment.cashCurrency,
                        Shipment_CashNote = shipment.cashNote,
                        Shipment_CashPayment = shipment.cashPayment,
                        Shipment_CashValue = shipment.cashValue,
                        Shipment_ConsigneeDes = shipment.consigneeDes,
                        Shipment_ConsigneeID = shipment.consigneeID,
                        Shipment_Cube = shipment.cube,
                        Shipment_CustomerDes = shipment.customerDes,
                        Shipment_CustomerID = shipment.customerID,
                        Shipment_CustomerID_XCM = documentoInEsame.CustomerID,
                        Shipment_DeliveryDateTime = shipment.deliveryDateTime,
                        Shipment_DocDate = shipment.docDate,
                        Shipment_DocGespeID_XCM = documentoInEsame.uniq,
                        Shipment_DocNumber = shipment.docNumber,
                        Shipment_ExternalRef = shipment.externRef,
                        Shipment_FirstStopDes = shipment.firstStopDes,
                        Shipment_FirstStopID = shipment.firstStopID,
                        Shipment_FloorPallets = shipment.floorPallets,
                        Shipment_GespeID_UNITEX = shipment.id,
                        Shipment_GrossWeight = shipment.grossWeight,
                        Shipment_InsideRef = shipment.insideRef,
                        Shipment_LastStopDes = shipment.lastStopDes,
                        Shipment_LastStopID = shipment.lastStopID,
                        Shipment_Meters = shipment.meters,
                        Shipment_NetWeight = shipment.netWeight,
                        Shipment_OwnerAgency = shipment.ownerAgency,
                        Shipment_Packs = shipment.packs,
                        Shipment_PickupDateTime = shipment.pickupDateTime,
                        Shipment_PickupSupplierDes = shipment.pickupSupplierDes,
                        Shipment_PickupSupplierID = shipment.pickupSupplierID,
                        Shipment_SenderDes = shipment.senderDes,
                        Shipment_SenderID = shipment.senderID,
                        Shipment_ServiceType = shipment.serviceType,
                        Shipment_StatusDes = shipment.statusDes,
                        Shipment_StatusID = shipment.statusId,
                        Shipment_StatusType = shipment.statusType,
                        Shipment_TotalPallets = shipment.totalPallets,
                        Shipment_TransportType = shipment.transportType,
                        Shipment_WebOrderID = shipment.webOrderID,
                        Shipment_WebOrderNumber = shipment.webOrderNumber,
                        Shipment_WebStatusID = shipment.webStatusId,
                        Shipment_WebStatusType = shipment.webStatusType,
                        //Tracking = TrackingsOrder
                    };
                    dbCRM.ShipmentList.Add(ns);
                    dbCRM.SaveChanges();
                }
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("StoricizzaDBCRMShipment", ee);
                }

                LastException = ee;

            }

        }
        #endregion

        #region DEPRECATE
        //private void APIXCMLogin()
        //{
        //    try
        //    {
        //        _loggerCode.Info($"Autenticazione in corso su endpoint {endpointAPI_APIXCM}");
        //        var client = new RestClient(endpointAPI_APIXCM + "/token");
        //        client.Timeout = -1;
        //        var request = new RestRequest(Method.POST);
        //        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        //        request.AddParameter("username", $"{userAPIXCM}");
        //        request.AddParameter("password", $"{passwordAPIXCM}");
        //        request.AddParameter("grant_type", "password");

        //        client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
        //        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        //        IRestResponse response = client.Execute(request);

        //        _loggerAPI.Info($"Nuovo token APIXCM: {token_APIXCM}");

        //        var resp = JsonConvert.DeserializeObject<LoginResponse>(response.Content);
        //        if (resp != null)
        //        {
        //            this.token_XCM = resp.access_token;
        //            DataScadenzaToken_XCM = DateTime.Now + TimeSpan.FromSeconds(resp.expires_in);
        //        }

        //    }
        //    catch (Exception ee)
        //    {
        //        _loggerCode.Error(ee);

        //        if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
        //        {
        //            DateLastException = DateTime.Now;
        //            GestoreMail.SegnalaErroreDev("APIXCMLogin", ee);
        //        }
        //        LastException = ee;
        //    }
        //}
        /// <summary>
        /// se passato un nuovo documento derivante dal tracking gespe aggiorna tutti gli altri campi e lo salva nel DB
        /// </summary>
        /// <param name="xdt">documento db xcm</param>
        /// <returns>ritorna documento XCM aggiornato di tutti i campi</returns>
        /*private XCM_TrackingDocument AggiornaCambioDocumento(XCM_TrackingDocument xdt)
        {
            var client2 = new RestClient(endpointAPI_XCM + $"/api/wms/document/get/{xdt.docID}");
            client2.Timeout = -1;
            var request2 = new RestRequest(Method.GET);
            request2.AddHeader("Authorization", $"Bearer {token_XCM}");
            IRestResponse response2 = client2.Execute(request2);
            var documentoAPI = JsonConvert.DeserializeObject<RootobjectXCMOrder>(response2.Content);
            if (documentoAPI.header == null)
            {
                return xdt;
            }
            string sdt2 = documentoAPI.header.reference2Date;
            DateTime rdt2 = (sdt2 != "") ? DateTime.Parse(sdt2) : DateTime.MinValue;
            DateTime? rdt2n = null;

            xdt.consigneeDes = documentoAPI.header.consigneeDes;
            xdt.customerDes = documentoAPI.header.customerDes;
            xdt.customerID = documentoAPI.header.customerID;
            xdt.reference = documentoAPI.header.reference;
            xdt.reference2 = documentoAPI.header.reference2;
            xdt.reference2Date = (rdt2 > DateTime.Now) ? rdt2 : rdt2n;
            xdt.referenceDate = documentoAPI.header.referenceDate;
            xdt.senderDes = documentoAPI.header.senderDes;
            xdt.totalBoxes = documentoAPI.header.totalBoxes;
            xdt.totalGrossWeight = Convert.ToDecimal(documentoAPI.header.totalGrossWeight);
            xdt.totalNetWeight = Convert.ToDecimal(documentoAPI.header.totalNetWeight);
            xdt.totalPacks = documentoAPI.header.totalPacks;
            xdt.totalQty = Convert.ToDecimal(documentoAPI.header.totalQty);
            xdt.unLoadDes = documentoAPI.header.unLoadDes;
            xdt.statusDes = documentoAPI.header.statusDes;
            return xdt;
        }*/
        /// <summary>
        /// Provvede al salvataggio nel DB d'appoggio nuovi documenti rilevati
        /// </summary>
        /// <param name="daInserire">lista di documenti DB d'appoggio</param>
        /*private void SalvaNuoviDocumenti(XCM_TrackingDocument daInserire)
        {
            var db = new InterscambioAPIEntities();
            db.XCM_TrackingDocument.Add(daInserire);
            db.SaveChanges();
        }*/
        /// <summary>
        /// Verifica i documenti nel DB d'appoggio se hanno cambiato stato, perchè noi di gespe non ci fidiamo...
        /// </summary>
        //private void VerificaDocumentiInCarico()
        //{
        //    RecuperaConnessione();
        //    try
        //    {
        //        _loggerCode.Info("Verifico documenti in carico");

        //        var db = new InterscambioAPIEntities();
        //        var dbTD = db.XCM_TrackingDocument.ToList();
        //        var listaAggiornati = new List<RootobjectXCMOrder>();
        //        //List<XCM_TrackingDocument> daEliminare = new List<XCM_TrackingDocument>();
        //        foreach (var doc in dbTD)
        //        {
        //            var ii = doc.docNumber.Split('/')[1];
        //            if (ii == "INV")
        //            {
        //                db.XCM_TrackingDocument.Remove(doc);
        //                db.SaveChanges();
        //                continue;
        //            }
        //            var client = new RestClient(endpointAPI_XCM + $"/api/wms/document/get/{doc.docID}");
        //            client.Timeout = -1;
        //            var request = new RestRequest(Method.GET);
        //            request.AddHeader("Authorization", $"Bearer {token_XCM}");
        //            IRestResponse response = client.Execute(request);
        //            var docOsservato = JsonConvert.DeserializeObject<RootobjectXCMOrder>(response.Content);

        //            if (docOsservato.result.status == true)
        //            {
        //                VerificaCambiamento(/*isPresente,*/ docOsservato/*, out bool DocdaEliminare*/);

        //                //if (DocdaEliminare)
        //                //{
        //                //    db.XCM_TrackingDocument.Remove(doc);
        //                //    db.SaveChanges();
        //                //}
        //                //else
        //                //{
        //                //    //AggiornaDocumentoInCarico(doc, docOsservato);
        //                //}
        //            }
        //            else
        //            {
        //                _loggerCode.Warn($"Documento {doc.docNumber} non trovato in database da API!!!! al momento lo elimino");
        //                db.XCM_TrackingDocument.Remove(doc);
        //                db.SaveChanges();
        //            }

        //        }
        //    }
        //    catch (Exception ee)
        //    {

        //        _loggerCode.Error(ee);

        //        if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
        //        {
        //            DateLastException = DateTime.Now;
        //            GestoreMail.SegnalaErroreDev("VerificaDocumentiInCarico", ee);
        //        }

        //        LastException = ee;
        //    }
        //    finally
        //    {

        //    }
        //}
        /// <summary>
        /// Effettua operazioni in base al tipo di cambiamento e del mandante
        /// </summary>
        /// <param name="doc">documento DB d'appoggio</param>
        /// <param name="docOsservato">documento DB gespe</param>
        /// <param name="daEliminare">passa alla funzione chiamante se il documento va eliminato dal DB d'appoggio</param> 
        ///  /// <summary>
        /// provvede all'invio dei movimenti del magazzino ai clienti
        /// </summary>
        /// <param name="interoMese">se true recupera tutti i movimenti del mese precedente a questa chiamata</param>
        /// <summary>
        /// Crea documento XCM d'interscambio partendo da documento API GESPE
        /// </summary>
        /// <param name="docOsservatoLink">documento API GESPE</param>
        /// <returns>Ritorna documento d'interscambio XCM</returns>
        /*private XCM_TrackingDocument CreaDocumentoXCM(RootobjectXCMOrder docOsservatoLink)
        {
            var xdt = new XCM_TrackingDocument
            {
                docID = docOsservatoLink.header.id,
                docNumber = docOsservatoLink.header.docNumber,
                doctype = docOsservatoLink.header.docType,
                id = docOsservatoLink.header.id,
                statusDes = docOsservatoLink.header.statusDes,
                statusID = docOsservatoLink.header.statusId,

            };

            var xdtAgg = AggiornaCambioDocumento(xdt);
            return xdtAgg;
        }*/
        #endregion
    }
}
