using CommonAPITypes.ESPRITEC;
using FluentFTP;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Timers;
using UNITEX_DOCUMENT_SERVICE.Code;
using UNITEX_DOCUMENT_SERVICE.Model.CDL;
using UNITEX_DOCUMENT_SERVICE.Model.DAMORA;
using UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX;
using UNITEX_DOCUMENT_SERVICE.Model.GUNA;
using UNITEX_DOCUMENT_SERVICE.Model.Logistica93;
using UNITEX_DOCUMENT_SERVICE.Model.PoolPharmaDLF;
using UNITEX_DOCUMENT_SERVICE.Model.STMGroup;
using UNITEX_DOCUMENT_SERVICE.Model.StockHouse;
using Renci.SshNet;
using UNITEX_DOCUMENT_SERVICE.Model.Chiapparoli;
using UNITEX_DOCUMENT_SERVICE.Model._3C;
using CommonAPITypes.UNITEX;
using UNITEX_DOCUMENT_SERVICE.Model;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System.Threading.Tasks;
using System.Net.Http;

using GestFile;
using GestObjectToFile;

namespace UNITEX_DOCUMENT_SERVICE
{
    public static class RandomExtensions
    {
        public static double NextDouble(
            this Random random,
            double minValue,
            double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
    public class Automazione
    {
        private static string endpointAPI_UNITEX = "https://010761.espritec.cloud:9500";

        private static string userAPIADMIN = "UNITEX_API_ADMIN";
        private static string passwordAPIADMIN = "wEmU0ks_";

        Exception LastException = new Exception("AVVIO");
        DateTime DateLastException = DateTime.MinValue;
        DateTime LastCheckChangesTMS = DateTime.MinValue;

        //#region CdGroup
        //private static string userAPIDiFarco = "difarcoapi";
        //private static string userAPIPhardis = "Phardisapi";
        //private static string userAPIStockhouse = "stockhouseAPI";
        //private static string passwordAPICDGroup = "[YFnvDL8";
        //private static string token_difarco = "";
        //private static string token_phardis = "";
        //private static string token_stockhouse = "";
        //private static DateTime scadenza_tokenDifarco = DateTime.MinValue;
        //private static DateTime scadenza_tokenPhardis = DateTime.MinValue;
        //private static DateTime scadenza_tokenStockhouse = DateTime.MinValue;
        //#endregion

        #region Token
        DateTime DataScadenzaToken_UNITEX = DateTime.MinValue;
        string token_UNITEX = "";
        #endregion

        #region FTP
        FtpClient _ftp = null;
        #endregion

        #region Logger
        internal static Logger _loggerCode = LogManager.GetLogger("loggerCode");
        internal static Logger _loggerAPI = LogManager.GetLogger("LogAPI");
        #endregion

        #region Esiti
        public static string FileEsitiDaRecuperareCDGroup = "RecuperaEsitiCDGroup.txt";
        public static string FileEsitiDaRecuperareGIMA = "RecuperaEsitiGima.txt";
        public static string FileEsitiDaRecuperareLoreal = "RecuperaEsitiLoreal.txt";
        public static string FileEsitiDaRecuperareSTM = "RecuperaEsitiSTM.txt";
        public static string FileRaddrizzatiDaComunicare = "RaddrizzateDaComunicare.txt";
        List<CDGROUP_EsitiOUT> EsitiDaCoumicareCDGroup = new List<CDGROUP_EsitiOUT>();
        List<CDGROUP_EsitiOUT> EsitiDaRecuperareParzialiCDGroup = File.ReadAllLines(FileEsitiDaRecuperareCDGroup).Select(x => CDGROUP_EsitiOUT.FromCsv(x)).ToList();
        List<STM_EsitiOut> EsitiDaRecuperareParzialiSTM = File.ReadAllLines(FileEsitiDaRecuperareSTM).Select(x => STM_EsitiOut.FromCsv(x)).ToList();
        List<LorealEsiti> EsitiDaRecuperareParzialiLoreal = File.ReadAllLines(FileEsitiDaRecuperareLoreal).Select(x => LorealEsiti.FromCsv(x)).ToList();
        List<STM_EsitiOut> EsitiDaCoumicareSTM = new List<STM_EsitiOut>();
        List<LorealEsiti> EsitiDaComunicareLoreal = new List<LorealEsiti>();
        List<DAMORA_EsitiOUT> EsitiDaComunicareDamora = new List<DAMORA_EsitiOUT>();
        List<_3C_EsitiOUT> EsitiDaComunicare_3C = new List<_3C_EsitiOUT>();
        List<Chiapparoli_EsitiOUT> EsitiDaComunicareChiapparoli = new List<Chiapparoli_EsitiOUT>();
        //public static string MandantiFileNameCDGroup = @"MandantiCodifica.csv";
        public static string CodiciStatoFileNameCDGroup = @"CodiciStatoCDGROUP.csv";
        public static string CodiciStatoFileNameSTM = @"CodiciStatoSTM.csv";
        public static string CodiciStatoFileNameLoreal = @"CodiciStatoLoreal.csv";
        public static string CodiciStatoFilenameChiapparoli = @"CodiciStatoChiapparoli.csv";
        public static string GEO_TAB = @"GEO.csv";
        public static List<Model.GeoClass> GeoTab = File.ReadAllLines(GEO_TAB).Select(x => Model.GeoClass.FromCsv(x)).ToList();
        //List<CDGROUP_Mandante> mandanti = File.ReadAllLines(MandantiFileNameCDGroup).Select(x => CDGROUP_Mandante.FromCsv(x)).ToList();
        List<CDGROUP_StatiDocumento> statiDocumemtoCDGroup = File.ReadAllLines(CodiciStatoFileNameCDGroup).Select(x => CDGROUP_StatiDocumento.FromCsv(x)).ToList();
        List<STM_StatiDocumento> statiDocumemtoSTM = File.ReadAllLines(CodiciStatoFileNameSTM).Select(x => STM_StatiDocumento.FromCsv(x)).ToList();
        List<Logistica93_StatiDocumento> statiDocumemtoLoreal = File.ReadAllLines(CodiciStatoFileNameLoreal).Select(x => Logistica93_StatiDocumento.FromCsv(x)).ToList();
        List<Chiapparoli_StatiDocumento> statiDocumemtoChiapparoli = File.ReadAllLines(CodiciStatoFilenameChiapparoli).Select(x => Chiapparoli_StatiDocumento.FromCsv(x)).ToList();
        #endregion

        Timer timerAggiornamentoCiclo = new Timer();
        double cicloTimer = 60000;

        Timer timerEsiti = new Timer();
        double cicloTimerEsitiCDGroup = 600000;

        public static string config = "config.ini";

        //CDL cdl = new CDL();


        GestFile.LogFile LOGCNTGXO ;
        GestFile.LogFile LOGCNTCEVA;
        public void Start()
        {
            _loggerCode.Debug("TEST LOG DEBUG");
            _loggerCode.Info("TEST LOG DEBUG");
            _loggerCode.Error("TEST LOG DEBUG");
            LOGCNTGXO = new LogFile();
            LOGCNTGXO.NomeFileDiLog = "RDX_Dati_GXO";

            LOGCNTCEVA = new LogFile();
            LOGCNTCEVA.NomeFileDiLog = "RDX_Dati_CEVA";
            CaricaConfigurazioni();
            CheckCustomerPath();

            //RecuperaConnessione(); Era cos'ì
            UnitexGespeAPILogin(userAPIADMIN, passwordAPIADMIN, out token_UNITEX, out DataScadenzaToken_UNITEX);

            //Test(); // Solo per .....

            SetTimer();
            OnTimedEvent(null, null);//controllo nuove spedizioni
            OnTimedEventCambiTMS(null, null);//controllo cambitracking e raddrizza
        }

        private void InizializzaCalendarioGoogle()
        {
            CalendarService service = new CalendarService(new BaseClientService.Initializer()
            {
                ApiKey = LocalGoogleCalendar.API_KEY,
                ApplicationName = "Test"
            });

            var request = service.Events.List(LocalGoogleCalendar.CalendarID);
            LocalGoogleCalendar.CalendarioGoogle = request.Execute();
        }

        private void Test()
        {

            #region Recupero Esiti
            //RecuperaEsitiCdGroup();
            //RecuperaEsitiGima();
            //RecuperaEsitiLoreal();
            //RecuperaEsitiSTM();
            //RecuperaEsitiChiapparoli();
            #endregion

            #region CD Group
            RecuperaVolumeCDGroup();
            //RecuperaPesoDaRifCliente();
            //RecuperaPesoVolumePalletCDGroup();
            //VerificaTrasmissioneEsitiCDGroup();
            //InserisciVolumeVetrinetteLancioCDGroup();
            //CheckExpoFromShipsCDGroup();
            //RecuperaRitiriCDGroup();
            //RecuperaSpondeCDGroup(); 
            //SQTCDGroup();
            #endregion

            #region Loreal
            //RiallineaLDVLoreal(); 
            #endregion

            #region Utility
            //RecuperSpedizioniDaGespe(new DateTime(2023, 03, 01)); 
            //AggiornaVolumeLDV();
            //ControllaSePresenteEXPO();
            //RecuperaTempiResaDaRifCliente();

            //InizializzaCalendarioGoogle();
            //foreach (var cust in CustomerConnections.CDGroup)
            //{
            //VerificaTempiDiResaUNITEX(CustomerConnections.PHARDIS);
            //}
            //AnalizzaTempiDiResaDettagliatiDaRifExt();
            //VerificaCapDisagiati(CustomerConnections.CDGroup); 
            //AnalizzaSpedizionixRifCliente();
            #endregion

            #region Dario
            //PopolaDettagliDario(); 
            #endregion

        }

        private void SQTCDGroup()
        {
            return;
            var listino = Model.CDGROUP.ListinoCDGroup.SpecListinoCDGroup;

            List<string> ModificheEffettuate = new List<string>();
            List<double> importoAggiunto = new List<double>();
            double limiteSQT = 10000;

            var sped = new List<EspritecShipment.ShipmentList>();
            var fromDate = new DateTime(2023, 06, 1);
            var toDate = new DateTime(2023, 06, 30, 23, 59, 59);
            int row = 1000;
            int maxP = 2;
            int page = 1;
            while (page <= maxP)
            {
                var rr = EspritecShipment.RestEspritecGetShipmentListTraDate(fromDate, toDate, row, page, token_UNITEX);
                var rrResp = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(rr.Content);
                maxP = rrResp.result.maxPages;
                Debug.WriteLine($"{page}-{maxP}");
                page++;
                if (rrResp.shipments == null) continue;
                sped.AddRange(rrResp.shipments);
            }
            var tot = sped.Count();

            foreach (var sh in sped)
            {
                if (SpedizioneHaIRequisitiGiusti(sh, out GeoSpec.GeoClass geo))
                {
                    var voceListino = listino.FirstOrDefault(x => (x.DescrizioneInizio.Trim().ToLower() == geo.regione.Trim().ToLower()) && x.ScaglioneInizio > sh.grossWeight && x.ScaglioneFine <= sh.grossWeight);
                    if (voceListino == null)
                    {
                        continue;
                    }

                    var prezzoNolo = (voceListino.PrezzoUnitario);
                }
            }


        }

        private bool SpedizioneHaIRequisitiGiusti(EspritecShipment.ShipmentList sh, out GeoSpec.GeoClass geo)
        {
            geo = null;
            if (sh.grossWeight < 3)
            {
                return false;
            }
            else if (sh.grossWeight > 200)
            {
                return false;
            }
            else if (sh.packs < 3 && sh.totalPallets == 0)
            {
                return false;
            }
            else if (sh.totalPallets > 1)
            {
                return false;
            }

            geo = GeoSpec.GeoList.FirstOrDefault(x => x.citta.Trim().ToLower() == sh.lastStopLocation.Trim());
            if (geo == null)
            {
                geo = GeoSpec.GeoList.FirstOrDefault(x => x.cap.Trim() == sh.lastStopZipCode.Trim());
            }
            if (geo == null)
            {
                return false;
            }

            return true;
        }

        private void RecuperaPesoDaRifCliente()
        {
            var daControllare = File.ReadAllLines("cdGroupSHID.txt");
            List<string> resp = new List<string>();
            int idbg = 0;
            int tot = daControllare.Count();
            foreach (var dc in daControllare)
            {
                idbg++;
                var sh = EspritecShipment.RestEspritecGetShipmentListByDocNum(dc, 5, 1, token_UNITEX);
                var shDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(sh.Content);
                if (shDes.shipments == null)
                {

                }
                resp.Add($"{dc};{shDes.shipments.Last().grossWeight}");
                Debug.WriteLine($"{idbg}-{tot}");
            }
            File.WriteAllLines("resultGW.txt", resp);
        }

        //private void RecuperaTempiResaDaRifCliente(CustomerSpec cust)
        //{
        //    int idbg = 0;

        //    List<ModelTempiResa> TempiModelList = new List<ModelTempiResa>();
        //    var rifCliente = File.ReadAllLines("RecuperoByEXT.txt");
        //    UnitexGespeAPILogin(cust.userAPI, cust.pswAPI, out string token, out DateTime scad);
        //    cust.tokenAPI = token;
        //    cust.scadenzaTokenAPI = scad;
        //    int tot = rifCliente.Count();
        //    Stopwatch sw = new Stopwatch();
        //    foreach (var rif in rifCliente)
        //    {
        //        sw.Restart();
        //        idbg++;
        //        var sh = EspritecShipment.RestEspritecGetShipmentListByExternalRef(rif, 10, 1, token);
        //        var shDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(sh.Content);

        //        var geo = GeoSpec.GeoList.FirstOrDefault(x => x.citta.ToLower() == shDes.shipments.Last().lastStopLocation.Trim().ToLower());
        //        if (geo == null)
        //        {
        //            geo = GeoSpec.GeoList.FirstOrDefault(x => x.cap == shDes.shipments.Last().lastStopZipCode.Trim());
        //        }
        //        if (geo == null)
        //        {
        //            //segnala il cap non presente mezzo mail
        //            _loggerCode.Error($"CAP:{shDes.shipments.Last().lastStopZipCode.Trim()} per la città:{shDes.shipments.Last().lastStopLocation} per la prov:{shDes.shipments.Last().lastStopDistrict} non presente in Gespe, impossibile valutare KPI");
        //            if (geo == null)
        //            {
        //                geo = GeoSpec.GeoList.FirstOrDefault(x => x.provincia == shDes.shipments.Last().lastStopDistrict.Trim());
        //                if (geo == null)
        //                {
        //                    geo = GeoSpec.GeoList.FirstOrDefault(x => x.citta == "Roma");
        //                }
        //            }

        //        }
        //        if (shDes.shipments != null)
        //        {
        //            var shipmentList = shDes.shipments.Last();

        //            EspritecShipment.RootobjectShipmentTracking shipmentTracking1 = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentTracking>(EspritecShipment.RestEspritecGetTracking((long)shipmentList.id, this.token_UNITEX).Content);

        //            EspritecShipment.EventShipmentTracking UltimoStato = null;
        //            bool SlaKPI = false;
        //            if (shipmentTracking1.events == null)
        //            {
        //                var nn = new ModelTempiResa()
        //                {
        //                    RifEsterno = rif,
        //                    Cliente = cust.NOME,
        //                    Destinatario = shipmentList.lastStopDes,
        //                    VettoreConsegna = shipmentList.deliverySupplierDes,
        //                    CodiceHubUnitex = shipmentList.ownerAgency,
        //                    Colli = shipmentList.packs,
        //                    DataCarico = shipmentList.docDate,
        //                    LocalitaConsegna = shipmentList.lastStopLocation,
        //                    NDocumento = shipmentList.docNumber,
        //                    Peso = shipmentList.grossWeight,
        //                    ProvinciaConsegna = shipmentList.lastStopDistrict,
        //                    Mandante = shipmentList.senderDes,
        //                    Pallet = (int)shipmentList.totalPallets,
        //                    StatoConsegna = "IN TRANSITO",
        //                    LocalitaDisagiata = geo.isDisagiata,
        //                    TempoResaReale = 0,
        //                    ResaMax = 0,
        //                };

        //                TempiModelList.Add(nn);
        //                continue;
        //            }


        //            bool SLApreno = shipmentTracking1.events.FirstOrDefault(x => x.statusID == 61) != null;
        //            bool SLA24 = shipmentTracking1.events.FirstOrDefault(x => x.statusID == 50 || x.statusID == 69 || x.statusID > 98 || x.statusID == 42 || x.statusID == 40 || x.statusID >= 100) != null;



        //            var consegnati = shipmentTracking1.events.OrderBy(x => x.id).Where(x => x.statusID == 30).ToList();
        //            if (consegnati.Count > 1)
        //            {
        //                UltimoStato = consegnati.First();
        //            }
        //            else if (consegnati.Count == 1)
        //            {
        //                UltimoStato = consegnati.First();
        //            }
        //            else
        //            {
        //                //continue;
        //                UltimoStato = shipmentTracking1.events.OrderBy(x => x.id).LastOrDefault();
        //            }

        //            EspritecShipmentStops.RootobjectShipmentStops rootobjectShipmentStops = JsonConvert.DeserializeObject<EspritecShipmentStops.RootobjectShipmentStops>(EspritecShipmentStops.RestEspritecGetShipStop((long)shipmentList.id, this.token_UNITEX).Content);
        //            DateTime dataConsegna = DateTime.MinValue;
        //            DateTime.TryParseExact(UltimoStato.timeStamp, "dd/MM/yyyy HH:mm:ss", null, DateTimeStyles.None, out dataConsegna);

        //            DateTime date = DateTime.MinValue;
        //            if (!string.IsNullOrEmpty(rootobjectShipmentStops.stops[0].date))
        //            {
        //                DateTime.TryParseExact(rootobjectShipmentStops.stops[0].date, "yyyy-MM-ddTHH:mm:ss", null, DateTimeStyles.None, out date);
        //            }
        //            else
        //            {
        //                date = shipmentList.docDate;
        //            }
        //            DateTime? DaConfrontare = null;
        //            if (date.Date >= shipmentList.docDate.Date)
        //            {
        //                DaConfrontare = date;
        //            }
        //            else
        //            {
        //                DaConfrontare = shipmentList.docDate;
        //            }

        //            int OreResa = LocalGoogleCalendar.GiorniDiResaEffettivi(DaConfrontare.Value.Date, dataConsegna.Date) * 24;
        //            int rMax = TempiResa.TempiResaUtils.RecuperaOreResaOttimali(geo, shipmentList.ownerAgency, SLA24);

        //            //DA VALUTARE CON CAPPARIO


        //            if (SLApreno || SLA24)
        //            {
        //                if (SLApreno)
        //                {
        //                    SlaKPI = true;
        //                }
        //                else
        //                {
        //                    if (OreResa + 24 <= rMax)
        //                    {
        //                        SlaKPI = true;

        //                    }
        //                }

        //            }

        //            ModelTempiResa tempoResa = new ModelTempiResa();
        //            tempoResa.Cliente = cust.NOME;
        //            tempoResa.CodiceHubUnitex = shipmentList.ownerAgency;
        //            tempoResa.DataCarico = date;
        //            tempoResa.DataConsegna = dataConsegna;
        //            tempoResa.RifEsterno = shipmentList.externRef.Trim();
        //            tempoResa.TempoResaReale = (disagiata != null) ? OreResa + 24 : OreResa;
        //            tempoResa.LocalitaConsegna = shipmentList.lastStopLocation.Trim();
        //            tempoResa.ProvinciaConsegna = shipmentList.lastStopDistrict.Trim();
        //            tempoResa.VettoreConsegna = (!string.IsNullOrEmpty(shipmentList.deliverySupplierDes.Trim())) ? shipmentList.deliverySupplierDes.Trim() : "UNITEX";
        //            tempoResa.Colli = shipmentList.packs;
        //            tempoResa.Pallet = (int)shipmentList.totalPallets;
        //            tempoResa.Destinatario = shipmentList.consigneeDes.Trim();
        //            tempoResa.NDocumento = shipmentList.docNumber;
        //            tempoResa.StatoConsegna = UltimoStato.statusDes;
        //            tempoResa.Peso = shipmentList.grossWeight;
        //            tempoResa.LocalitaDisagiata = (disagiata != null) ? true : false;
        //            tempoResa.SLAKPI = SlaKPI;
        //            tempoResa.TipoSLA = RecuperaTipoSLA(SLA24, SLApreno, disagiata != null);
        //            tempoResa.Mandante = shipmentList.senderDes;
        //            TempiModelList.Add(tempoResa);
        //            Debug.WriteLine(tempoResa.ToString());
        //            Debug.WriteLine($"{idbg}-{tot}-{tot - idbg} --------- {sw.Elapsed.TotalMilliseconds}");

        //        }





        //        //var listResp = new List<string>();
        //        //listResp.Add($"Cliente;HUB;Rif. Cliente;Rif. Unitex;Data Affido;Stato Tracking;Data Tracking;Ore Resa;Ore Resa Max;KPI OK;KPI KO;Loc. Dis.;Regione;Prov.;Localita;Destinatario;Colli;Pallet;Peso;Vettore;SLAKPI;Mandante");
        //        //foreach (var rn in EsitiRaggruppatiHubNord)
        //        //{
        //        //    listResp.Add(rn.ToString());
        //        //}
        //        //foreach (var rn in EsitiRaggruppatiHubSud)
        //        //{
        //        //    listResp.Add(rn.ToString());
        //        //}


        //        //System.IO.File.WriteAllLines("ConsuntivoTempiDiResa_" + cust.NOME + "_" + tot + "_" + ".csv", listResp);
        //    }
        //}

        private string RecuperaTipoSLA(bool sLA24, bool sLApreno, bool disagiata)
        {
            if (sLApreno)
            {
                return "PRENO";
            }
            else if (sLA24 && !disagiata)
            {
                return "24";
            }
            else if (disagiata)
            {
                return "DISAGIATA";
            }
            return "";
        }

        private void ControllaSePresenteEXPO()
        {
            var dati = File.ReadAllLines("cdGroupSHID.txt");
            List<string> Resp = new List<string>();
            int tot = dati.Count();
            int idbg = 0;
            foreach (var d in dati)
            {
                idbg++;
                var shs = EspritecShipment.RestEspritecGetShipmentListByExternalRef(d, 50, 1, token_UNITEX);
                var shsDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(shs.Content);

                if (shsDes.shipments != null && shsDes.shipments.Count() == 1)
                {
                    var idSH = shsDes.shipments.First().id;
                    var shGet = EspritecShipment.RestEspritecGetShipment(idSH, token_UNITEX);
                    var shGetDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectEspritecShipment>(shGet.Content);
                    bool isExpo = shGetDes.shipment.internalNote.StartsWith("ESPOSITORE");
                    string rr = isExpo ? "OK" : "EXPO NON PRESENTE";
                    Resp.Add($"{d};{rr};S");
                }
                else if (shsDes.shipments != null)
                {
                    var idSH = shsDes.shipments.Last().id;
                    var shGet = EspritecShipment.RestEspritecGetShipment(idSH, token_UNITEX);
                    var shGetDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectEspritecShipment>(shGet.Content);
                    bool isExpo = shGetDes.shipment.internalNote.StartsWith("ESPOSITORE");
                    string rr = isExpo ? "OK" : "EXPO NON PRESENTE";
                    Resp.Add($"{d};{rr};M");
                }
                else
                {
                    Resp.Add($"{d};SPEDIZIONE NON TROVATA;");
                }
                Debug.WriteLine($"{idbg}-{tot}");
            }
            File.WriteAllLines("ConsuntivoExpoExt.csv", Resp);
        }

        private void AnalizzaTempiDiResaDettagliatiDaRifExt()
        {
            var daAnalizzare = File.ReadAllLines("RecuperaEsitiLoreal.txt");
            var cust = CustomerConnections.Logistica93;
            string token = cust.tokenAPI;
            var resp = new List<string>();

            foreach (var dA in daAnalizzare)
            {
                //recupera la spedizione
                var sh = EspritecShipment.RestEspritecGetShipmentListByExternalRef(dA, 50, 1, token);
                var shDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(sh.Content);

                if (shDes.shipments != null)
                {
                    var ult = shDes.shipments.OrderBy(x => x.id).LastOrDefault();
                    var trk = EspritecShipment.RestEspritecGetTracking(ult.id, token);
                    var trkDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentTracking>(trk.Content);
                    EspritecShipment.EventShipmentTracking esito = null;
                    if (trkDes.events != null)
                    {
                        var interessante = trkDes.events.Where(x => x.statusID == 30).ToList();
                        if (interessante.Count > 1)
                        {
                            esito = interessante.OrderBy(x => x.id).First();
                        }
                        else if (interessante.Count == 1)
                        {
                            esito = interessante.First();
                        }
                        else
                        {
                            esito = trkDes.events.OrderBy(x => x.id).Last();
                        }
                        DateTime dataConsegna = DateTime.MinValue;
                        DateTime.TryParse(esito.timeStamp, out dataConsegna);
                        int GiorniResa = LocalGoogleCalendar.GiorniDiResaEffettivi(ult.docDate, dataConsegna.Date);

                        resp.Add($"{dA};{ult.docNumber};{ult.docDate.Date};{esito.statusDes};{dataConsegna.Date};{GiorniResa};{ult.consigneeDes};{ult.consigneeDistrict};{ult.consigneeLocation};{ult.consigneeZipCode}");

                    }
                    else
                    {
                        resp.Add($"{dA};{ult.docNumber};{ult.docDate.Date};{esito.statusDes};ND;ND;{ult.consigneeDes};{ult.consigneeDistrict};{ult.consigneeLocation};{ult.consigneeZipCode}");
                    }
                }
                else
                {
                    resp.Add($"{dA};ND;ND;ND;ND;ND;ND;ND;ND;ND");
                }
                //recupera i tracking
                //popola
                File.WriteAllLines($@"VerificaSpedizioniETempi_{cust.NOME}_{DateTime.Now.Date}.csv", resp);
            }


        }

        private void AnalizzaSpedizionixRifCliente()
        {
            var riferimentiCliente = File.ReadAllLines("AnalisiSpedizioniPerRifCliente.txt");
            List<string> resp = new List<string>();
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            resp.Add($"Rif.Unitex;Rif.Cliente;Data Affido;Stato Tracking;Data Tracking;Giorni Resa;Destinatario;Località;Prov.;CAP;Vettore;");
            foreach (var rif in riferimentiCliente)
            {
                var sh = EspritecShipment.RestEspritecGetShipmentListByExternalRef(rif, 500, 1, CustomerConnections.Logistica93.tokenAPI);

                var shs = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(sh.Content);
                if (!shs.result.status || shs.shipments == null)
                {
                    string rr = $"NON TROVATA;{rif};NON TROVATA;NON TROVATA;NON TROVATA;NON CALCOLABILE;NON TROVATA;NON TROVATA;NON TROVATA;NON TROVATA;NON TROVATA;";
                    resp.Add(rr);
                    Debug.WriteLine(rr);

                    continue;
                }
                var shDes = shs.shipments.OrderBy(x => x.docDate).LastOrDefault();

                var trks = EspritecShipment.RestEspritecGetTracking(shDes.id, token_UNITEX);
                var trksDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentTracking>(trks.Content);
                //Debug.WriteLine(sw.ElapsedMilliseconds);
                if (trksDes.events == null)
                {
                    string g = $"{shDes.docNumber};{rif};{shDes.docDate.ToString("dd/MM/yyyy")};NON PRESENTE;NON PRESENTE;" +
                    $"NON CALCOLABILE;{shDes.lastStopDes};{shDes.lastStopLocation};{shDes.lastStopDistrict};{shDes.lastStopZipCode};{shDes.deliverySupplierDes}";
                    resp.Add(g);
                    Debug.WriteLine(g);
                    continue;
                }
                var statoInteressante = trksDes.events.OrderBy(x => x.id).FirstOrDefault(x => x.statusID == 30);
                if (statoInteressante == null)
                {
                    statoInteressante = trksDes.events.OrderBy(x => x.id).LastOrDefault();
                }
                DateTime.TryParse(statoInteressante.timeStamp, out DateTime ts);
                string r = $"{shDes.docNumber};{rif};{shDes.docDate.ToString("dd/MM/yyyy")};{statoInteressante.statusDes};{ts.ToString("dd/MM/yyyy")};" +
                    $"{LocalGoogleCalendar.GiorniDiResaEffettivi(shDes.docDate, ts)};{shDes.lastStopDes};{shDes.lastStopLocation};{shDes.lastStopDistrict};{shDes.lastStopZipCode};{shDes.deliverySupplierDes}";
                resp.Add(r);
                Debug.WriteLine(r);
                Debug.WriteLine(sw.ElapsedMilliseconds);
                sw.Restart();
            }
            File.WriteAllLines("AnalisiSpedizioniPerRifCliente.csv", resp);
        }

        private void RecuperaEsitiChiapparoli()
        {

            UnitexGespeAPILogin(CustomerConnections.CHIAPPAROLI.userAPI, CustomerConnections.CHIAPPAROLI.pswAPI, out string token, out DateTime scad);
            CustomerConnections.CHIAPPAROLI.tokenAPI = token;
            CustomerConnections.CHIAPPAROLI.scadenzaTokenAPI = scad;
            EsitiDaComunicareChiapparoli.Clear();


            var daRecuperare = File.ReadAllLines("RecuperaEsitiChiapparoli.txt");
            int tot = daRecuperare.Count();
            int idBG = 0;
            List<EspritecShipment.EventShipmentTracking> Interessanti = new List<EspritecShipment.EventShipmentTracking>();
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            foreach (var dR in daRecuperare)
            {
                idBG++;
                var shUndes = EspritecShipment.RestEspritecGetShipmentListByExternalRef(dR, 5, 1, CustomerConnections.CHIAPPAROLI.tokenAPI);
                var sht = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(shUndes.Content);//.shipments.FirstOrDefault();
                if (sht.result.status)
                {
                    var sh = sht.shipments.FirstOrDefault(x => x.docDate.Year == DateTime.Now.Year);
                    if (sh != null)
                    {
                        //recupera tracking
                        var tt = EspritecShipment.RestEspritecGetTracking(sh.id, token_UNITEX);
                        var trks = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentTracking>(tt.Content);

                        if (trks.events != null)
                        {
                            var interessante = trks.events.FirstOrDefault(x => x.statusID == 30);
                            if (interessante != null)
                            {
                                Interessanti.Add(interessante);
                            }
                            else
                            {
                                interessante = trks.events.OrderBy(x => x.id).Last();
                                Interessanti.Add(interessante);
                            }
                            AggiungiAllaListaLesitoDaRecupero(CustomerConnections.CHIAPPAROLI, interessante, sh);
                            Debug.WriteLine(interessante.statusDes);
                        }
                        else
                        {
                            Interessanti.Add(new EspritecShipment.EventShipmentTracking());
                        }
                    }
                }
                else
                {

                }
                Debug.WriteLine($"{idBG}-{tot}");

                Debug.WriteLine(sw.Elapsed.TotalSeconds);
                sw.Restart();

            }


            ProduciEsitiChiapparoli();
        }

        private void AggiungiAllaListaLesitoDaRecupero(CustomerSpec cust, EspritecShipment.EventShipmentTracking shipTrackingUnitexNR, EspritecShipment.ShipmentList shipUnitex)
        {
            //Chiapparoli

            //var ts = GeoSpec.GeoList.FirstOrDefault(x => x.citta.ToLower() == shipUnitex.lastStopLocation.ToLower());
            //if (ts == null)
            //{
            //    ts = GeoSpec.GeoList.FirstOrDefault(x => x.cap == shipUnitex.lastStopZipCode);

            //}
            //if (ts == null)
            //{

            //}

            var shipTrackingUnitex = shipTrackingUnitexNR;//RaddrizzaTracking(shipTrackingUnitexNR, cust, shipUnitex, ts);
            DateTime dataEsito = DateTime.MinValue;
            DateTime.TryParseExact(shipTrackingUnitex.timeStamp.ToString(), "dd/MM/yyyy HH:mm:ss", null, DateTimeStyles.None, out dataEsito);


            if (dataEsito == DateTime.MinValue)
            {
                //che faccio?
            }

            if (cust == CustomerConnections.PHARDIS || cust == CustomerConnections.DIFARCO || cust == CustomerConnections.StockHouse)//cdgroup
            {
                if (string.IsNullOrEmpty(shipUnitex.insideRef)) return;
                var checkCodiceStato = statiDocumemtoCDGroup.FirstOrDefault(x => x.IdUnitex == shipTrackingUnitex.statusID);
                if (checkCodiceStato != null)
                {
                    var codiceStato = checkCodiceStato.CodiceStato;

                    var elem = new CDGROUP_EsitiOUT()
                    {
                        MANDANTE = shipUnitex.insideRef,
                        NUMERO_BOLLA = shipUnitex.externRef,
                        DATA_BOLLA = (shipUnitex.docDate != null) ? shipUnitex.docDate.ToString("yyyyMMdd") : "        ",
                        RAGIONE_SOCIALE_VETTORE = "UNITEXPRESS",
                        DATA_PRESA_CONS = (shipUnitex.docDate != null) ? shipUnitex.docDate.ToString("yyyyMMdd") : "        ",
                        STATO_CONSEGNA = codiceStato,
                        DESCRIZIONE_STATO_CONSEGNA = shipTrackingUnitex.statusDes,
                        DATA = dataEsito.ToString("yyyyMMdd"),
                        RIFVETTORE = shipUnitex.docNumber,
                        statoUNITEX = shipTrackingUnitex.statusID
                    };
                    EsitiDaCoumicareCDGroup.Add(elem);
                }

            }
            else if (cust == CustomerConnections.STMGroup)
            {
                var checkCodiceStato = statiDocumemtoSTM.FirstOrDefault(x => x.IdUnitex == shipTrackingUnitex.statusID);
                if (checkCodiceStato != null)
                {
                    var codiceStato = checkCodiceStato.CodiceStato;

                    if (string.IsNullOrEmpty(shipUnitex.insideRef)) return;
                    var geoSpec = GeoTab.FirstOrDefault(x => x.cap == shipUnitex.lastStopZipCode);

                    var elem = new STM_EsitiOut()
                    {
                        CittaDestinatario = shipUnitex.firstStopLocation,
                        DataConsegnaEffettiva = (shipTrackingUnitex.statusID == 30) ? dataEsito.ToString("yyyyMMdd") : "        ",
                        DataConsegnaTassativa = "        ",//TODO: verificare data tassativa da API
                        DataSpedizione = shipUnitex.docDate.ToString("ddMMyyyy"),
                        DataTracking = dataEsito.ToString("ddMMyyyy"),
                        Descrizione_Tracking = shipTrackingUnitex.statusDes,
                        ID_Tracking = codiceStato,
                        NumDDT = shipUnitex.externRef,
                        ProgressivoSpedizione = shipUnitex.docNumber.Split('/')[0],
                        regione = (geoSpec != null) ? geoSpec.regione : "ND"
                    };
                    EsitiDaCoumicareSTM.Add(elem);
                }
            }
            else if (cust == CustomerConnections.Logistica93)
            {
                var checkCodiceStato = statiDocumemtoLoreal.FirstOrDefault(x => x.IdUnitex == shipTrackingUnitex.statusID);
                if (checkCodiceStato != null)
                {
                    string sottoCausale = "000";
                    if (checkCodiceStato.CodiceStato == "03")
                    {
                        sottoCausale = "602";
                    }
                    if (checkCodiceStato.CodiceStato == "03" && shipTrackingUnitex.statusID == 61)
                    {
                        sottoCausale = "604";
                    }
                    var elem = new LorealEsiti()
                    {
                        E_NumeroDDT = shipUnitex.externRef,
                        E_RiferimentoNumeroConsegnaSAP = shipUnitex.insideRef,
                        E_DataConsegnaADestino = dataEsito.ToString("yyyyMMdd"),
                        E_Causale = checkCodiceStato.CodiceStato,
                        E_SottoCausale = sottoCausale,
                        E_RiferimentoCorriere = shipUnitex.docNumber,
                        E_Filler1 = "",
                        E_Note = shipTrackingUnitex.info,
                        E_Filler2 = "",
                        statoUNITEX = shipTrackingUnitex.statusID
                    };
                    EsitiDaComunicareLoreal.Add(elem);
                }
            }
            else if (cust == CustomerConnections.DAMORA)
            {
                var elem = new DAMORA_EsitiOUT()
                {
                    dataEsito = dataEsito.ToString("ddMMyy"),
                    DescrizioneEsito = shipTrackingUnitex.statusDes,
                    rifExt = shipUnitex.externRef,
                };
                EsitiDaComunicareDamora.Add(elem);
            }
            else if (cust == CustomerConnections.CHIAPPAROLI)//chiapparoli
            {
                //recupera i costi di spedizione

                if (!shipUnitex.insideRef.Contains("|"))
                {
                    return;
                }
                var specCHC = shipUnitex.insideRef.Split('|');
                string CodiceDitta = "";
                string SedeCHC = "";

                if (specCHC.Count() > 1)
                {
                    CodiceDitta = specCHC[1];
                    SedeCHC = specCHC[2];
                }
                var statoCHC = statiDocumemtoChiapparoli.FirstOrDefault(x => x.IdUnitex == shipTrackingUnitex.statusID);
                if (statoCHC != null)
                {
                    var elem = new Chiapparoli_EsitiOUT()
                    {
                        CodiceDitta = CodiceDitta,
                        CodiceResa = statoCHC.CodiceStato,//tabella codici stato esiti
                        Colli = shipUnitex.packs.ToString(),
                        DataResaAAMMGG = dataEsito.ToString("yyMMdd"),
                        DataRiferimentoVettoreAAMMGG = DateTime.Now.ToString("yyMMdd"),
                        DataRiferimentoSubVettoreAAMMGG = "",
                        Filler = "",
                        ImportoTotaleSpedizione2d = "",//inserisci la somma dei costi
                        NumeroProgressivo = shipUnitex.externRef,
                        OraResa = dataEsito.ToString("HHmm"),
                        Volume3d = Helper.GetVolumeChiapparoli((double)shipUnitex.cube, 3),
                        Peso2d = Helper.GetPesoChiapparoli((double)shipUnitex.grossWeight, 2),
                        PesoTassato = "",//formula calcolo
                        PosizioneRiga = "",//cos'è??
                        RiferimentoSubVettore = "",
                        RiferimentoVettore = shipUnitex.docNumber,
                        RigaNote = shipTrackingUnitex.statusDes,
                        SedeChiapparoli = SedeCHC,
                        statoUNITEX = shipTrackingUnitex.statusID
                    };
                    EsitiDaComunicareChiapparoli.Add(elem);
                }
            }
        }

        private void PopolaDettagliDario()
        {
            var daAggiornare = File.ReadAllLines(@"C:\Users\Piero\Desktop\Dario\unico.csv");
            int tot = daAggiornare.Count();
            int idBG = 0;
            List<string> ListaAggiornata = new List<string>();
            foreach (var dA in daAggiornare)
            {
                idBG++;
                string resp = dA;
                var pz = dA.Split(';');
                var docNum = pz[2];

                var ship = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(EspritecShipment.RestEspritecGetShipmentListByDocNum(docNum, 5, 1, token_UNITEX).Content).shipments.Where(x => x.docDate.Year == DateTime.Now.Year).FirstOrDefault();

                resp += $"{ship.grossWeight};{ship.cube};{ship.totalPallets}";
                ListaAggiornata.Add(resp);
                Debug.WriteLine($"{idBG}-{tot}");
            }

            File.WriteAllLines(@"C:\Users\Piero\Desktop\Dario\unicoAggiornato.csv", ListaAggiornata);
        }

        private void VerificaCapDisagiati(List<CustomerSpec> listaClientiDaVerificare)
        {
            #region RecuperaShipmentAPI
            var sped = new List<EspritecShipment.ShipmentList>();
            var fromDate = new DateTime(2023, 02, 1);
            var toDate = new DateTime(2023, 03, 31, 23, 59, 59);
            int row = 2000;
            int maxP = 20;
            foreach (var cust in listaClientiDaVerificare)
            {
                int page = 1;
                while (page <= maxP)
                {
                    var rr = EspritecShipment.RestEspritecGetShipmentListTraDate(fromDate, toDate, row, page, cust.tokenAPI);
                    var rrResp = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(rr.Content);
                    maxP = rrResp.result.maxPages;
                    Debug.WriteLine($"{page}-{maxP}-{cust.NOME}");
                    page++;
                    if (rrResp.shipments == null) continue;
                    //var listaPulita = rrResp.shipments.Where(x => x.grossWeight == 0).ToList();
                    sped.AddRange(rrResp.shipments);
                }
                maxP = 20;
            }
            var tot = sped.Count();
            #endregion

            List<string> JustCap = new List<string>();
            List<string> Resp = new List<string>();
            var CapDisagiati = File.ReadAllLines("TempiResaCAPDisagiati.txt");

            foreach (var c in CapDisagiati)
            {
                JustCap.Add(c.Split(';')[0]);
            }

            var spedizioniDisagiate = sped.Where(x => JustCap.Contains(x.lastStopZipCode)).ToList();

            foreach (var sd in spedizioniDisagiate)
            {
                Resp.Add($"{sd.docDate.ToString("dd/MM/yyyy")};{sd.docNumber};{sd.externRef};{sd.insideRef};{sd.senderDes};{sd.consigneeDes};{sd.lastStopLocation};{sd.lastStopDistrict};{sd.lastStopZipCode};");
            }

            File.WriteAllLines("CDGroupSpedizioniDisagiate.csv", Resp);

        }

        private void AggiornaVolumeLDV()
        {
            var daAggiornare = File.ReadAllLines("aggiornaVolume.txt");
            int tot = daAggiornare.Count();
            int i = 0;

            foreach (var r in daAggiornare)
            {
                i++;
                var pz = r.Split(';');
                var id = int.Parse(pz[0]);
                var vol = decimal.Parse(pz[1]);
                var gDaAgg = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsList>(EspritecGoods.RestEspritecGetGoodListOfShipment(id, token_UNITEX).Content);

                var Agg = new EspritecGoods.RootobjectGoodsUpdate()
                {
                    goods = new EspritecGoods.GoodsUpdate()
                    {
                        id = gDaAgg.goods[0].id,
                        cube = vol
                    }
                };

                var aggio = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsUpdateResponse>(EspritecGoods.RestEspritecUpdateGoods(Agg, token_UNITEX).Content);

                if (!aggio.status)
                {

                }
                Debug.WriteLine($"{i}-{tot}");
            }
        }

        private void RecuperaSpondeCDGroup()
        {
            //recupera tutte le spedizioni ddgroup

            #region RecuperaShipmentAPI
            var sped = new List<EspritecShipment.ShipmentList>();
            var fromDate = new DateTime(2023, 03, 31);
            var toDate = new DateTime(2023, 04, 30, 23, 59, 59);
            int row = 2000;
            int maxP = 20;
            foreach (var cust in CustomerConnections.CDGroup)
            {
                int page = 1;
                while (page <= maxP)
                {
                    var rr = EspritecShipment.RestEspritecGetShipmentListTraDate(fromDate, toDate, row, page, cust.tokenAPI);
                    var rrResp = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(rr.Content);
                    maxP = rrResp.result.maxPages;
                    Debug.WriteLine($"{page}-{maxP}-{cust.NOME}");
                    page++;
                    if (rrResp.shipments == null) continue;
                    //var listaPulita = rrResp.shipments.Where(x => x.grossWeight == 0).ToList();
                    sped.AddRange(rrResp.shipments);
                }
                maxP = 20;
            }
            var tot = sped.Count();
            #endregion

            //per ogni spedizione valuta se occorre la sponda
            foreach (var sh in sped)
            {
                //recupera gli stop

                var dest = sh.consigneeDes;
                if (dest.ToLower().Contains("f.cia") || dest.ToLower().Contains("f.cie") ||
                    dest.ToLower().Contains("farmacia") || dest.ToLower().Contains("farmacie") ||
                    dest.ToLower().Contains("osp.") || dest.ToLower().Contains("ospedale"))
                {

                }
                //se occore la sponda inseriscila in gespe
            }
        }

        class CheckGoodsModel
        {
            internal string shipID { get; set; }
            internal string DocNum { get; set; }
            internal string DataDoc { get; set; }
            internal string NumeroExpo { get; set; }
            internal string Volume { get; set; }
            internal string QuantitaRigheSpedizione { get; set; }
            internal string PresenteRigaRilevataDaAPP { get; set; }
            internal string ColliSpedizione { get; set; }
            internal string PesoSpedizione { get; set; }
            internal string Mandante { get; set; }
            internal string RifCliente { get; set; }
            internal double PVolComunicato
            {
                get
                {
                    return double.Parse(Volume) * 200;
                }
            }


            public override string ToString()
            {
                return $"{shipID}|{DocNum}|{RifCliente}|{DataDoc}|{NumeroExpo}|{Volume}|{PVolComunicato}|||{PesoSpedizione}|{PresenteRigaRilevataDaAPP}|{ColliSpedizione}|{Mandante}|{QuantitaRigheSpedizione}";
            }

        }
        private void CheckExpoFromShipsCDGroup()
        {
            List<CheckGoodsModel> Resoconto = new List<CheckGoodsModel>();

            var RifSpedizioni = File.ReadAllLines("CheckExpo.txt");
            var tot = RifSpedizioni.Count();
            int idbg = 0;
            foreach (var rif in RifSpedizioni)
            {
                idbg++;
                var pz = rif.Split(';');
                //recupera la ship da gespe
                var ships = EspritecShipment.RestEspritecGetShipmentListByDocNum(pz[0], 10, 1, token_UNITEX);
                //var ships = EspritecShipment.RestEspritecGetShipmentListByExternalRef(pz[0], 10, 1, token_UNITEX);
                var shipCorretta1 = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(ships.Content);
                if (!shipCorretta1.result.status)
                {
                    var nC = new CheckGoodsModel()
                    {
                        shipID = "NON TROVATO",
                        DocNum = "NON TROVATO",
                        DataDoc = "NON TROVATO",
                        PresenteRigaRilevataDaAPP = "NON TROVATO",
                        NumeroExpo = pz[1],
                        ColliSpedizione = "0",
                        PesoSpedizione = "0",
                        QuantitaRigheSpedizione = "0",
                        Volume = "0",
                        Mandante = "NON TROVATO",
                        RifCliente = pz[0]
                    };
                    Debug.WriteLine($"{idbg}-{tot}");
                    Debug.WriteLine(nC.ToString());
                    Resoconto.Add(nC);
                    continue;
                }
                else
                {
                    var shipCorretta = shipCorretta1.shipments.FirstOrDefault(x => x.docDate.Year == DateTime.Now.Year);
                    //recupera le goods della ship
                    var goods = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsList>(EspritecGoods.RestEspritecGetGoodListOfShipment(shipCorretta.id, token_UNITEX).Content);

                    var righeSped = goods.goods.Count();
                    var expoInseritoDallOperatore = goods.goods.Any(x => x.description.Contains("|"));
                    //analizza
                    var nC = new CheckGoodsModel()
                    {
                        shipID = shipCorretta.id.ToString(),
                        DocNum = shipCorretta.docNumber,
                        DataDoc = shipCorretta.docDate.ToString("dd/MM/yyyy"),
                        PresenteRigaRilevataDaAPP = (expoInseritoDallOperatore) ? "SI" : "NO",
                        NumeroExpo = pz[1],
                        ColliSpedizione = shipCorretta.packs.ToString("0"),
                        PesoSpedizione = shipCorretta.grossWeight.ToString("0.00"),
                        QuantitaRigheSpedizione = righeSped.ToString(),
                        Volume = shipCorretta.cube.ToString("0.00"),
                        Mandante = shipCorretta.senderDes,
                        RifCliente = shipCorretta.externRef
                    };
                    Debug.WriteLine($"{idbg}-{tot}");
                    Debug.WriteLine(nC.ToString());
                    Resoconto.Add(nC);
                }
            }

            List<string> resp = new List<string>();
            foreach (var r in Resoconto)
            {
                resp.Add(r.ToString());
            }
            File.WriteAllLines("CheckExpoResult.txt", resp);
        }

        private void InserisciVolumeVetrinetteLancioCDGroup()
        {
            var sped = File.ReadAllLines("cdGroupSHID.txt");

            #region AlgoritmoVolume
            List<string> ModificheEffettuate = new List<string>();
            ModificheEffettuate.Add("ID;DOCNUM;PESO;COLLI;PALLET;VOL.COMUNICATO;VOL.ALGORITMO");
            int idbg = 0;
            var tot = sped.Count();
            foreach (var ss in sped)
            {
                idbg++;
                var id = int.Parse(ss);
                var shipG = EspritecShipment.RestEspritecGetShipment(id, token_UNITEX);
                var shipDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectEspritecShipment>(shipG.Content);
                var sh = shipDes.shipment;

                var goodsSH = EspritecGoods.RestEspritecGetGoodListOfShipment(id, token_UNITEX);
                var goodsSHDes = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsList>(goodsSH.Content);
                if (goodsSHDes.goods != null && goodsSHDes.goods.Count() > 0)
                {
                    var VetrinettaGiaRilevata = goodsSHDes.goods.Any(x => x.description.Contains('|'));
                    if (VetrinettaGiaRilevata)
                    {
                        continue;
                    }

                    foreach (var g in goodsSHDes.goods)
                    {
                        Debug.WriteLine($"{idbg}-{tot}-{sh.docDate}-COLLI:{g.packs}-PLT:{g.floorPallet}-PESO:{g.grossWeight}-VOL:{g.cube}-{g.description}");
                        if (!string.IsNullOrEmpty(g.description))
                        {
                            continue;
                        }


                        //if (g.cube > 0.01M)
                        //{
                        //    continue;
                        //}

                        var Rng = new EspritecGoods.RootobjectGoodsUpdate();
                        var goods = new EspritecGoods.GoodsUpdate();
                        Random random = new Random();
                        double volume = (double)g.cube;

                        if (sh.senderDes.Contains("BIONIKE"))
                        {
                            //volume+= 
                        }
                        else if (sh.senderDes.Contains("PIERRE"))
                        {
                            //volume+= 
                        }
                        else if (sh.senderDes.Contains("COTY"))
                        {
                            //volume+= 
                        }
                        else if (sh.senderDes.Contains("GIULIANI"))
                        {
                            //volume+= 
                        }
                        else if (sh.senderDes.Contains("MONTEFARMACO"))
                        {
                            //volume+= 
                        }
                        else
                        {
                            //volume defalut?
                            continue;
                        }

                        goods.id = g.id;
                        //goods.description = $"v~{g.cube}";
                        goods.packs = g.packs;
                        goods.totalPallet = g.totalPallet;
                        goods.floorPallet = g.floorPallet;

                        Debug.WriteLine($"{idbg}-{tot}-{goods.cube}-{g.grossWeight}-{goods.packs}-{goods.totalPallet}");

                        Rng.goods = goods;
                        var gUpdate = EspritecGoods.RestEspritecUpdateGoods(Rng, token_UNITEX);
                        var gUpdateDes = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsUpdateResponse>(gUpdate.Content);

                        if (gUpdateDes != null && !gUpdateDes.status)
                        {

                        }
                        ModificheEffettuate.Add($"{sh.id};{sh.docNumber};{g.grossWeight};{goods.packs};{goods.totalPallet};{g.cube};{volume}");
                    }
                }

            }
            #endregion
        }

        private void CheckFileTemporaneiCSVAndClean()
        {
            foreach (string file in Directory.GetFiles("TempCSV", "*.*"))
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch
                {
                }
            }
        }
        private void VerificaTrasmissioneEsitiCDGroup()
        {
            List<string> TutteLeRighe = new List<string>();
            List<string> Verificati = new List<string>();
            var inviati = Directory.GetFiles(@"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT\Save");
            var esitiRichiesti = File.ReadAllLines(@"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT\TRACK_20230608170522.txt");
            var head = $"Data DDT|Rif. Cliente|Stato|Nome File trasmesso|Data Esito|Data Trasmissione|Diff giorni Esito/Trasm|Vettore";
            Verificati.Add(head);
            int idbgf = 0;
            int totFiles = inviati.Count();
            foreach (var inv in inviati)
            {
                idbgf++;
                var fn = Path.GetFileName(inv);
                var righeF = File.ReadAllLines(inv);
                foreach (var r in righeF)
                {
                    var dataTrasm = DateTime.ParseExact(fn.Split('_')[1].Split('.')[0], "yyyyMMddHHmmss", null);
                    var stato = r.Substring(79, 30).Trim();
                    DateTime dataDDT = DateTime.MinValue;
                    DateTime.TryParseExact(r.Substring(30, 8), "yyyyMMdd", null, DateTimeStyles.None, out dataDDT);
                    DateTime DataEsito = DateTime.MinValue;
                    DateTime.TryParseExact(r.Substring(109, 8), "yyyyMMdd", null, DateTimeStyles.None, out DataEsito);
                    if (stato.ToUpper() != "CONSEGNATA" || DataEsito == DateTime.MinValue)
                    {
                        continue;
                    }
                    var giorniPassatiDallEsitoAllaTrasmissione = (dataTrasm - DataEsito).Days;
                    var str = $"{dataDDT}|{r.Substring(15, 10)}|{stato}|{fn}|{DataEsito.ToString("dd/MM/yyyy")}|{dataTrasm.ToString("dd/MM/yyyy")}|{giorniPassatiDallEsitoAllaTrasmissione}";
                    TutteLeRighe.Add(str);
                }
                Debug.WriteLine($"{idbgf}-{totFiles}");
            }
            int idbg = 0;
            int tot = esitiRichiesti.Count();
            foreach (var esito in esitiRichiesti)
            {
                idbg++;
                Debug.WriteLine($"{idbg}-{tot}");
                var str = esito.Substring(15, 10);
                var giaComunicato = TutteLeRighe.FirstOrDefault(x => x.Contains($"|{str}|"));
                var dettSped = EspritecShipment.RestEspritecGetShipmentListByExternalRef(str, 1, 1, token_UNITEX);
                var dettDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(dettSped.Content);
                string vettore = "";
                if (dettDes.shipments != null)
                {
                    vettore = dettDes.shipments[0].deliverySupplierDes;
                }

                if (giaComunicato != null)
                {
                    Verificati.Add(giaComunicato + $"|{vettore}");
                    Debug.WriteLine(giaComunicato + $"|{vettore}");
                }
                //else
                //{
                //    Verificati.Add($"{str}|NON COMUNICATO");
                //    Debug.WriteLine(giaComunicato + $"|{vettore}");

                //}
            }

            File.WriteAllLines("EsitiComunicati.txt", Verificati);

        }

        private void VerificaTempiDiResaUNITEX(CustomerSpec cust)
        {
            if (!string.IsNullOrEmpty(cust.userAPI) && (DateTime.Now + TimeSpan.FromHours(1)) > cust.scadenzaTokenAPI)
            {
                UnitexGespeAPILogin(cust.userAPI, cust.pswAPI, out string token, out DateTime scad);
                cust.tokenAPI = token;
                cust.scadenzaTokenAPI = scad;
            }

            Stopwatch sw = new Stopwatch();
            DateTime dataDa = new DateTime(2023, 06, 01);
            DateTime dataA = new DateTime(2023, 06, 30, 23, 59, 59);

            List<EspritecShipment.ShipmentList> source = new List<EspritecShipment.ShipmentList>();
            int righe = 1000;
            int pag = 1;
            sw.Restart();

            List<double> totS = new List<double>();
            for (int i = 2; pag <= i; ++pag)
            {
                EspritecShipment.RootobjectShipmentList rootobjectShipmentList = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(EspritecShipment.RestEspritecGetShipmentListTraDate(dataDa, dataA, righe, pag, cust.tokenAPI).Content);
                source.AddRange((IEnumerable<EspritecShipment.ShipmentList>)rootobjectShipmentList.shipments);
                if (i == 2) { i = rootobjectShipmentList.result.maxPages; }
                Debug.WriteLine(string.Format($"{pag}-{i}--------------------{sw.Elapsed.TotalSeconds}"));
                totS.Add(sw.Elapsed.TotalSeconds);
                sw.Restart();

            }
            Debug.WriteLine(totS.Sum());
            int tot = source.Count();
            int idbg = 0;

            List<ModelTempiResa> TempiModelList = new List<ModelTempiResa>();

            foreach (EspritecShipment.ShipmentList shipmentList in source)
            {
                ++idbg;
                if (!shipmentList.senderDes.Contains("GSK"))
                {
                    Debug.WriteLine(idbg);
                    continue;
                }
                EspritecShipment.RootobjectShipmentTracking shipmentTracking1 = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentTracking>(EspritecShipment.RestEspritecGetTracking((long)shipmentList.id, this.token_UNITEX).Content);
                var geo = GeoSpec.GeoList.FirstOrDefault(x => x.citta.ToLower() == shipmentList.lastStopLocation.Trim().ToLower());
                if (geo == null)
                {
                    geo = GeoSpec.GeoList.FirstOrDefault(x => x.cap == shipmentList.lastStopZipCode.Trim());
                }
                if (geo == null)
                {
                    //segnala il cap non presente mezzo mail
                    //_loggerCode.Error($"CAP:{shipmentList.lastStopZipCode.Trim()} per la città:{shipmentList.lastStopLocation} per la prov:{shipmentList.lastStopDistrict} non presente in Gespe, impossibile valutare KPI");
                    if (geo == null)
                    {
                        geo = GeoSpec.GeoList.FirstOrDefault(x => x.provincia == shipmentList.lastStopDistrict.Trim());
                        if (geo == null)
                        {
                            continue;
                        }
                    }

                }
                if (geo.regione.ToLower() != "campania")
                {
                    continue;
                }
                EspritecShipment.EventShipmentTracking UltimoStato = null;
                bool SlaKPI = false;
                if (shipmentTracking1.events == null)
                {
                    var nn = new ModelTempiResa()
                    {
                        RifEsterno = shipmentList.externRef,
                        Cliente = cust.NOME,
                        Destinatario = shipmentList.lastStopDes,
                        VettoreConsegna = shipmentList.deliverySupplierDes,
                        CodiceHubUnitex = shipmentList.ownerAgency,
                        Colli = shipmentList.packs,
                        DataCarico = shipmentList.docDate,
                        LocalitaConsegna = shipmentList.lastStopLocation,
                        NDocumento = shipmentList.docNumber,
                        Peso = shipmentList.grossWeight,
                        ProvinciaConsegna = shipmentList.lastStopDistrict,
                        Mandante = shipmentList.senderDes,
                        Pallet = (int)shipmentList.totalPallets,
                        StatoConsegna = "IN TRANSITO",
                        LocalitaDisagiata = geo.isDisagiata,
                        TempoResaReale = 0,
                        ResaMax = 0,
                    };

                    TempiModelList.Add(nn);
                    continue;
                }


                bool SLApreno = shipmentTracking1.events.FirstOrDefault(x => x.statusID == 61) != null;
                bool SLA24 = shipmentTracking1.events.FirstOrDefault(x => x.statusID == 50 || x.statusID == 69 || x.statusID > 98 || x.statusID == 42 || x.statusID == 40 || x.statusID > 100) != null;

                var consegnati = shipmentTracking1.events.OrderBy(x => x.id).Where(x => x.statusID == 30).ToList();
                if (consegnati.Count > 1)
                {
                    UltimoStato = consegnati.First();
                }
                else if (consegnati.Count == 1)
                {
                    UltimoStato = consegnati.First();
                }
                else
                {
                    UltimoStato = shipmentTracking1.events.OrderBy(x => x.id).LastOrDefault();
                }

                EspritecShipmentStops.RootobjectShipmentStops rootobjectShipmentStops = JsonConvert.DeserializeObject<EspritecShipmentStops.RootobjectShipmentStops>(EspritecShipmentStops.RestEspritecGetShipStop((long)shipmentList.id, this.token_UNITEX).Content);
                DateTime dataConsegna = DateTime.MinValue;
                DateTime.TryParseExact(UltimoStato.timeStamp, "dd/MM/yyyy HH:mm:ss", null, DateTimeStyles.None, out dataConsegna);

                DateTime dataCarico = DateTime.MinValue;
                if (!string.IsNullOrEmpty(rootobjectShipmentStops.stops[0].date))
                {
                    DateTime.TryParseExact(rootobjectShipmentStops.stops[0].date, "yyyy-MM-ddTHH:mm:ss", null, DateTimeStyles.None, out dataCarico);
                }
                else
                {
                    dataCarico = shipmentList.docDate;
                }
                DateTime? DaConfrontare = null;
                if (dataCarico.Date >= shipmentList.docDate.Date)
                {
                    DaConfrontare = dataCarico;
                }
                else
                {
                    DaConfrontare = shipmentList.docDate;
                }
                int OreResa = LocalGoogleCalendar.GiorniDiResaEffettivi(DaConfrontare.Value.Date, dataConsegna.Date) * 24;
                int rMax = TempiResa.TempiResaUtils.RecuperaOreResaOttimali(geo, shipmentList.ownerAgency, SLA24);


                if (SLApreno || SLA24)
                {
                    if (SLApreno)
                    {
                        SlaKPI = true;
                    }
                    else
                    {
                        if (OreResa + 24 <= rMax)
                        {
                            SlaKPI = true;
                        }
                    }
                }

                ModelTempiResa tempoResa = new ModelTempiResa();
                tempoResa.Cliente = cust.NOME;
                tempoResa.CodiceHubUnitex = shipmentList.ownerAgency;
                tempoResa.DataCarico = dataCarico;
                tempoResa.DataConsegna = dataConsegna;
                tempoResa.RifEsterno = shipmentList.externRef.Trim();
                tempoResa.TempoResaReale = OreResa;
                tempoResa.LocalitaConsegna = shipmentList.lastStopLocation.Trim();
                tempoResa.ProvinciaConsegna = shipmentList.lastStopDistrict.Trim();
                tempoResa.VettoreConsegna = (!string.IsNullOrEmpty(shipmentList.deliverySupplierDes.Trim())) ? shipmentList.deliverySupplierDes.Trim() : "UNITEX SRL";
                tempoResa.Colli = shipmentList.packs;
                tempoResa.Pallet = (int)shipmentList.totalPallets;
                tempoResa.Destinatario = shipmentList.consigneeDes.Trim();
                tempoResa.NDocumento = shipmentList.docNumber;
                tempoResa.StatoConsegna = UltimoStato.statusDes;
                tempoResa.Peso = shipmentList.grossWeight;
                tempoResa.LocalitaDisagiata = geo.isDisagiata;
                tempoResa.SLAKPI = SlaKPI;
                tempoResa.TipoSLA = RecuperaTipoSLA(SLA24, SLApreno, geo.isDisagiata);
                tempoResa.Mandante = shipmentList.senderDes;
                //contents.Add(modelTempiResa2.ToString());
                TempiModelList.Add(tempoResa);
                Debug.WriteLine(tempoResa.ToString());
                Debug.WriteLine($"{idbg}-{tot}-{tot - idbg} --------- {sw.Elapsed.TotalMilliseconds}");
                sw.Restart();

            }


            var tempiResaItalia = ObjectTempiResa.TempiResaHUB;

            var EsitiRaggruppatiHubNord = new List<ModelTempiResa>();
            var EsitiRaggruppatiHubSud = new List<ModelTempiResa>();
            foreach (var tr in TempiModelList)
            {
                if (tr.CodiceHubUnitex == "01")
                {
                    EsitiRaggruppatiHubSud.Add(tr);
                }
                else if (tr.CodiceHubUnitex == "02")
                {
                    EsitiRaggruppatiHubNord.Add(tr);
                }
                else
                {
                    tr.CodiceHubUnitex = "02";
                    EsitiRaggruppatiHubNord.Add(tr);
                }
            }

            foreach (var hubNord in EsitiRaggruppatiHubNord)
            {
                int rMax = 0;
                if (hubNord.ProvinciaConsegna == "SM")
                {
                    rMax = 72;
                }
                else if (string.IsNullOrEmpty(hubNord.ProvinciaConsegna))
                {
                    rMax = 72;
                }
                else
                {
                    rMax = ObjectTempiResa.TempiResaHUB.FirstOrDefault(x => x.Provincia == hubNord.ProvinciaConsegna).ResaHUBNordMax;
                }
                hubNord.ResaMax = rMax;
                hubNord.DataConsegnaPrevistaMax = LocalGoogleCalendar.CalcolaDataConsegnaPrevista(hubNord.DataCarico.Date, rMax);
            }

            foreach (var hubSud in EsitiRaggruppatiHubSud)
            {
                int rMax = ObjectTempiResa.TempiResaHUB.FirstOrDefault(x => x.Provincia == hubSud.ProvinciaConsegna).ResaHUBSudMax;
                hubSud.ResaMax = rMax;
                hubSud.DataConsegnaPrevistaMax = LocalGoogleCalendar.CalcolaDataConsegnaPrevista(hubSud.DataCarico.Date, rMax);
            }

            var listResp = new List<string>();
            listResp.Add($"Cliente;HUB;Rif. Cliente;Rif. Unitex;Data Affido;Stato Tracking;Data Tracking;Ore Resa;Ore Resa Max;KPI OK;KPI KO;Loc. Dis.;Regione;Prov.;Localita;Destinatario;Colli;Pallet;Peso;Vettore;SLAKPI;Mandante");
            foreach (var rn in EsitiRaggruppatiHubNord)
            {
                listResp.Add(rn.ToString());
            }
            foreach (var rn in EsitiRaggruppatiHubSud)
            {
                listResp.Add(rn.ToString());
            }


            System.IO.File.WriteAllLines("ConsuntivoTempiDiResa_" + cust.NOME + "_" + dataDa.ToString("dd_MM_yyyy") + "_" + dataA.ToString("dd_MM_yyyy") + "_" + tot + "_" + ".csv", listResp);
        }

        class RitiriCDGroupBK
        {
            public string data { get; set; }
            public string extref { get; set; }
            public string colli { get; set; }
            public string peso { get; set; }
            public string volume { get; set; }
            public string plt { get; set; }
        }
        private void RecuperaRitiriCDGroup()
        {
            var ritiriBK = File.ReadAllLines(@"C:\Users\Piero\Desktop\RecuperoCDGroup\rit\rit.txt");
            List<RitiriCDGroupBK> ritiri = new List<RitiriCDGroupBK>();

            foreach (var rit in ritiriBK)
            {
                var pz = rit.Split(';');
                var nB = new RitiriCDGroupBK()
                {
                    data = pz[0],
                    extref = pz[1],
                    colli = pz[2],
                    peso = pz[3],
                    volume = pz[4],
                    plt = pz[5]
                };
                ritiri.Add(nB);
            }

            var idSH = File.ReadAllLines("cdGroupSHID.txt");
            var tot = idSH.Count();
            int idbg = 0;

            foreach (var sh in idSH)
            {
                idbg++;
                //recupera goods
                var id = int.Parse(sh);
                var shipG = EspritecShipment.RestEspritecGetShipment(id, token_UNITEX);
                var shipDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectEspritecShipment>(shipG.Content);

                if (!shipDes.result.status)
                {
                    continue;
                }

                var goodsSH = EspritecGoods.RestEspritecGetGoodListOfShipment(id, token_UNITEX);
                var goodsSHDes = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsList>(goodsSH.Content);
                if (goodsSHDes.goods != null && goodsSHDes.goods.Count() > 0)
                {
                    var corr = ritiri.FirstOrDefault(x => x.extref == shipDes.shipment.externRef);
                    Debug.WriteLine($"{idbg}-{tot}");
                    if (corr == null)
                    {
                        _loggerAPI.Info($"ID NON TROVATO|{shipDes.shipment.id}|{shipDes.shipment.externRef}");
                        Debug.WriteLine($"ID NON TROVATO|{shipDes.shipment.id}|{shipDes.shipment.externRef}");
                        continue;
                    }
                    if (goodsSHDes.goods.Sum(x => x.grossWeight) > 0)
                    {
                        Debug.WriteLine($"PESO PRESENTE{shipDes.shipment.id}|{shipDes.shipment.externRef}");
                        continue;
                    }
                    foreach (var g in goodsSHDes.goods)
                    {
                        Debug.WriteLine($"{idbg}-{tot}-{g.cube}");

                        var Rng = new EspritecGoods.RootobjectGoodsUpdate();
                        var goods = new EspritecGoods.GoodsUpdate();
                        Random random = new Random();
                        double value = random.NextDouble(0.045, 0.069);
                        g.packs = goods.packs = int.Parse(corr.colli);
                        var vol = (decimal)value * g.packs;

                        var peso = corr.peso;
                        var plts = corr.plt;


                        goods.id = g.id;
                        goods.cube = vol;
                        goods.grossWeight = decimal.Parse(peso);
                        goods.floorPallet = goods.totalPallet = int.Parse(plts);
                        Debug.WriteLine($"{idbg}-{tot}-{goods.cube}-{goods.grossWeight}-{goods.packs}-{goods.totalPallet}");

                        Rng.goods = goods;
                        var gUpdate = EspritecGoods.RestEspritecUpdateGoods(Rng, token_UNITEX);
                        var gUpdateDes = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsUpdateResponse>(gUpdate.Content);

                        if (gUpdateDes != null && !gUpdateDes.status)
                        {

                        }
                    }
                }
            }
        }

        private void RecuperSpedizioniDaGespe(DateTime DaData)
        {
            int custMaxPages = 1;

            List<EspritecShipment.ShipmentList> SpedizioniCDGroup = new List<EspritecShipment.ShipmentList>();
            foreach (var cust in CustomerConnections.customers)
            {
                if (!CustomerConnections.CDGroup.Any(x => x.ID_GESPE == cust.ID_GESPE)) continue;


                int currPages = 1;
                var shs = EspritecShipment.RestEspritecGetShipmentList(DaData, 1000, currPages, cust.tokenAPI);
                var shsDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(shs.Content);
                custMaxPages = shsDes.result.maxPages;
                SpedizioniCDGroup.AddRange(shsDes.shipments);
                while (currPages < custMaxPages)
                {
                    currPages++;
                    Debug.WriteLine(currPages + "-" + custMaxPages);
                    var shsp = EspritecShipment.RestEspritecGetShipmentList(DaData, 100, currPages, cust.tokenAPI);
                    var shsDesp = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(shs.Content);
                    SpedizioniCDGroup.AddRange(shsDes.shipments);
                }
            }

            //FILTRO QUERY
            var daSegnalare = SpedizioniCDGroup.Where(x => x.firstStopLocation.ToLower() == "vimercate" && x.firstStopDistrict.ToLower() == "mi").ToList();

            List<string> idShipDaModificare = new List<string>();
            foreach (var id in daSegnalare)
            {
                idShipDaModificare.Add(id.id.ToString());
            }
            File.WriteAllLines("idRilevati.txt", idShipDaModificare);
        }

        //non è possibile aggiornare gli stop dalle API
        #region UpdateStop
        //private void RecuperaMagazziniCDGroup()
        //{

        //    int idbg = 0;

        //    var DT = new DateTime(2023, 03, 01);

        //    int custMaxPages = 1;

        //    List<EspritecShipment.ShipmentList> SpedizioniCDGroup = new List<EspritecShipment.ShipmentList>();
        //    foreach (var cust in CommonAPITypes.UNITEX.CustomerConnections.CDGroup)
        //    {
        //        int currPages = 1;
        //        var shs = EspritecShipment.RestEspritecGetShipmentList(DT, 100, currPages, cust.tokenAPI);
        //        var shsDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(shs.Content);
        //        custMaxPages = shsDes.result.maxPages;
        //        SpedizioniCDGroup.AddRange(shsDes.shipments);
        //        while (currPages < custMaxPages)
        //        {
        //            currPages++;
        //            var shsp = EspritecShipment.RestEspritecGetShipmentList(DT, 100, currPages, cust.tokenAPI);
        //            var shsDesp = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(shs.Content);
        //            SpedizioniCDGroup.AddRange(shsDes.shipments);
        //        }
        //    }

        //    foreach(var sl in SpedizioniCDGroup)
        //    {
        //        //aggiorna il dato
        //        if (sl.firstStopDistrict.ToLower() == "mi" && sl.firstStopLocation.ToLower() == "vimercate")
        //        {
        //            var nSUpdate = new EspritecShipment.ShipmentUpdate()
        //            {
        //                id = sl.id,

        //            };
        //        }
        //        //carica il dato

        //    }




        //} 
        #endregion

        private void RecuperaPesoVolumePalletCDGroup()
        {
            List<string> ModificheEffettuate = new List<string>();
            var idSs = File.ReadAllLines("cdGroupSHID.txt").Select(x => int.Parse(x)).ToList();
            Random random = new Random();
            //recupera le goods dalla shipID
            foreach (var sID in idSs)
            {
                var goods = EspritecGoods.RestEspritecGetGoodListOfShipment(sID, token_UNITEX);
                var goodsDes = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsList>(goods.Content);
                var shG = EspritecShipment.RestEspritecGetShipment(sID, token_UNITEX);
                var shGDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectEspritecShipment>(shG.Content);
                if (goodsDes != null)
                {
                    var plt = goodsDes.goods.Sum(x => x.floorPallet);
                    var pesoOrig = goodsDes.goods.Sum(x => x.grossWeight);
                    var volOrig = goodsDes.goods.Sum(x => x.cube);
                    bool withVolume = volOrig > 0;
                    var pesoComplessivo = new List<double>();
                    var volComplessivo = new List<decimal>();
                    for (int i = 0; i < plt; i++)
                    {
                        double volRDM = random.NextDouble(1.30, 1.50);
                        var volSG = (withVolume) ? volOrig : (decimal)volRDM;
                        if (!withVolume)
                        {
                            volComplessivo.Add(volSG);
                        }
                        else
                        {
                            if (i == 0)
                            {
                                volComplessivo.Add(volSG);
                            }
                        }
                        pesoComplessivo.Add(random.NextDouble(197.0, 199.98));

                    }
                    var GU = new EspritecGoods.RootobjectGoodsUpdate()
                    {
                        goods = new EspritecGoods.GoodsUpdate
                        {
                            id = goodsDes.goods[0].id,
                            cube = volComplessivo.Sum(),
                            grossWeight = (decimal)pesoComplessivo.Sum(),
                            totalPallet = 0,
                            floorPallet = 0,
                            packs = plt
                        }
                    };
                    var rGU = EspritecGoods.RestEspritecUpdateGoods(GU, token_UNITEX);
                    var rGUDes = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsUpdateResponse>(rGU.Content);



                    if (rGUDes.status)
                    {
                        var s = $"{sID}|{shGDes.shipment.docNumber}|{shGDes.shipment.externRef}|{pesoOrig}|{pesoComplessivo.Sum()}|{volOrig}|{volComplessivo.Sum()}|{plt}";
                        Debug.WriteLine(s);
                        ModificheEffettuate.Add(s);
                    }
                    else
                    {

                    }
                }

            }
            File.WriteAllLines("modificheVolCDGroup.txt", ModificheEffettuate);
            //se presente volume calcola volume medio per pallet
            //se non presente il volume applica un volume tra un range tra 1.25 e 1.44
            //sposta il numero pallet nei colli
            //correggi peso totale riga con pesi compresi tra 195 a 199.99

        }
        private void RecuperaVolumeCDGroup()
        {

            #region AzzeraVolumePerLeSpedizioniInEssere
            //var daAzzerare = File.ReadAllLines("LDVModificateVol_02052023154203.txt").Skip(1).ToList();
            //int totDA = daAzzerare.Count();
            //int idbGDA = 0;
            //foreach (var da in daAzzerare)
            //{
            //    idbGDA++;
            //    Debug.WriteLine($"{idbGDA}-{totDA}");
            //    var pcs = da.Split(';');

            //    //recupera le goods della spedizione

            //    var ship = EspritecShipment.RestEspritecGetShipment(long.Parse(pcs[0]), token_UNITEX);
            //    var shipDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectEspritecShipment>(ship.Content);

            //    if (shipDes != null)
            //    {
            //        //recupera le goods
            //        var goodsSH = EspritecGoods.RestEspritecGetGoodListOfShipment(shipDes.shipment.id, token_UNITEX);
            //        var goodsSHDes = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsList>(goodsSH.Content);

            //        if (goodsSHDes != null && goodsSHDes.goods.Count() > 0)
            //        {
            //            foreach (var g in goodsSHDes.goods)
            //            {
            //                if (string.IsNullOrEmpty(g.description))
            //                {
            //                    var Rng = new EspritecGoods.RootobjectGoodsUpdate();
            //                    var goods = new EspritecGoods.GoodsUpdate();

            //                    goods.id = g.id;
            //                    goods.cube = 0.001M;
            //                    Rng.goods = goods;
            //                    var gUpdate = EspritecGoods.RestEspritecUpdateGoods(Rng, token_UNITEX);
            //                    var gUpdateDes = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsUpdateResponse>(gUpdate.Content);

            //                    if (!gUpdateDes.status)
            //                    {

            //                    }
            //                }
            //                else
            //                {

            //                }
            //            }
            //        }
            //    }

            //    //azzera il volume della goods
            //} 
            #endregion

            //per ogni id sh presente nel file di recupero recupera le goods ed aggiorna il volume

            //var idSH = File.ReadAllLines("cdGroupSHID.txt");

            #region Riallineamento in base ai file del cliente
            //List<RootobjectNewShipmentTMS> ListShip = new List<RootobjectNewShipmentTMS>();
            //var fileDaConfrontare = Directory.GetFiles(@"C:\Users\Piero\Desktop\RecuperoCDGroup");
            //int ttf = fileDaConfrontare.Count();
            //int L = 0;
            //foreach (var fr in fileDaConfrontare)
            //{
            //    L++;
            //    Debug.WriteLine($"{L}-{ttf}");
            //    var tutteLeRighe = File.ReadAllLines(fr);
            //    if (tutteLeRighe.Count() > 0)
            //    {
            //        StockHouse_Shipment_IN CdGroup = new StockHouse_Shipment_IN();
            //        for (int i = 0; i < tutteLeRighe.Count(); i++)
            //        {
            //            try
            //            {
            //                Debug.WriteLine(i);
            //                RootobjectNewShipmentTMS shipmentTMS = new RootobjectNewShipmentTMS();
            //                List<ParcelNewShipmentTMS> parcelNewShipment = new List<ParcelNewShipmentTMS>();
            //                List<StopNewShipmentTMS> destinazione = new List<StopNewShipmentTMS>();
            //                List<GoodNewShipmentTMS> merce = new List<GoodNewShipmentTMS>();

            //                var r = tutteLeRighe[i];

            //                #region decodifica
            //                CdGroup.MANDANTE = r.Substring(CdGroup.idxMANDANTE[0], CdGroup.idxMANDANTE[1]).Trim();
            //                CdGroup.NR_BOLLA = r.Substring(CdGroup.idxNRBOLLA[0], CdGroup.idxNRBOLLA[1]).Trim();
            //                CdGroup.DATA_BOLLA = r.Substring(CdGroup.idxDATA_BOLLA[0], CdGroup.idxDATA_BOLLA[1]).Trim();
            //                CdGroup.NR_SHIPMENT = r.Substring(CdGroup.idxNR_SHIPMENT[0], CdGroup.idxNR_SHIPMENT[1]).Trim();
            //                CdGroup.RAG_SOC_MITTENTE = r.Substring(CdGroup.idxRAG_SOC_MITTENTE[0], CdGroup.idxRAG_SOC_MITTENTE[1]).Trim();
            //                CdGroup.INDIRIZZO_MITTENTE = r.Substring(CdGroup.idxINDIRIZZO_MITTENTE[0], CdGroup.idxINDIRIZZO_MITTENTE[1]).Trim();
            //                CdGroup.CAP_MITTENTE = r.Substring(CdGroup.idxCAP_MITTENTE[0], CdGroup.idxCAP_MITTENTE[1]).Trim();
            //                CdGroup.LOC_MITTENTE = r.Substring(CdGroup.idxLOC_MITTENTE[0], CdGroup.idxLOC_MITTENTE[1]).Trim();
            //                CdGroup.PROV_MITTENTE = r.Substring(CdGroup.idxPROV_MITTENTE[0], CdGroup.idxPROV_MITTENTE[1]).Trim();
            //                CdGroup.NAZIONE_MITTENTE = r.Substring(CdGroup.idxNAZIONE_MITTENTE[0], CdGroup.idxNAZIONE_MITTENTE[1]).Substring(0, 2);
            //                CdGroup.RAG_SOC_DESTINATARIO = r.Substring(CdGroup.idxRAG_SOC_DESTINATARIO[0], CdGroup.idxRAG_SOC_DESTINATARIO[1]).Trim().Replace("\"", "");
            //                CdGroup.INDIRIZZO_DESTINATARIO = r.Substring(CdGroup.idxINDIRIZZO_DESTINATARIO[0], CdGroup.idxINDIRIZZO_DESTINATARIO[1]).Trim().Replace("\"", "");
            //                CdGroup.CAP_DESTINATARIO = r.Substring(CdGroup.idxCAP_DESTINATARIO[0], CdGroup.idxCAP_DESTINATARIO[1]).Trim();
            //                CdGroup.LOC_DESTINATARIO = r.Substring(CdGroup.idxLOC_DESTINATARIO[0], CdGroup.idxLOC_DESTINATARIO[1]).Trim();
            //                CdGroup.PROV_DESTINATARIO = r.Substring(CdGroup.idxPROV_DESTINATARIO[0], CdGroup.idxPROV_DESTINATARIO[1]).Trim();
            //                CdGroup.NAZIONE_DESTINATARIO = r.Substring(CdGroup.idxNAZIONE_DESTINATARIO[0], CdGroup.idxNAZIONE_DESTINATARIO[1]).Trim();
            //                CdGroup.PESO_SPEDIZIONE = r.Substring(CdGroup.idxPESO_SPEDIZIONE[0], CdGroup.idxPESO_SPEDIZIONE[1]).Trim();
            //                CdGroup.VOLUME_SPEDIZIONE = r.Substring(CdGroup.idxVOLUME_SPEDIZIONE[0], CdGroup.idxVOLUME_SPEDIZIONE[1]).Trim();
            //                CdGroup.N_CARTONI_CT = r.Substring(CdGroup.idxN_CARTONI_CT[0], CdGroup.idxN_CARTONI_CT[1]).Trim();
            //                CdGroup.N_BANCALI_BA = Helper.StringIntString(r.Substring(CdGroup.idxN_BANCALI_BA[0], CdGroup.idxN_BANCALI_BA[1]).Trim());
            //                CdGroup.N_BANCALI_COLLETTAME_BB = Helper.StringIntString(r.Substring(CdGroup.idxN_BANCALI_COLLETTAME_BB[0], CdGroup.idxN_BANCALI_COLLETTAME_BB[1]).Trim());
            //                CdGroup.N_BA_BB = Helper.StringIntString(r.Substring(CdGroup.idxN_BA_BB[0], CdGroup.idxN_BA_BB[1]).Trim());
            //                CdGroup.PESO_CARTONI_CT = r.Substring(CdGroup.idxPESO_CARTONI_CT[0], CdGroup.idxPESO_CARTONI_CT[1]).Trim();
            //                CdGroup.VALUTA_CONTRASS = r.Substring(CdGroup.idxVALUTA_CONTRASS[0], CdGroup.idxVALUTA_CONTRASS[1]).Trim();
            //                CdGroup.IMPORTO_CONTRASS = r.Substring(CdGroup.idxIMPORTO_CONTRASS[0], CdGroup.idxIMPORTO_CONTRASS[1]).Trim();
            //                CdGroup.NUMERO_COLLI_SPED = Helper.StringIntString(r.Substring(CdGroup.idxNUMERO_COLLI_SPED[0], CdGroup.idxNUMERO_COLLI_SPED[1]).Trim());
            //                CdGroup.DA_SEGNACOLLO = r.Substring(CdGroup.idxDA_SEGNACOLLO[0], CdGroup.idxDA_SEGNACOLLO[1]).Trim();
            //                CdGroup.A_SEGNACOLLO = r.Substring(CdGroup.idxA_SEGNACOLLO[0], CdGroup.idxA_SEGNACOLLO[1]).Trim();
            //                CdGroup.NOTE = r.Substring(CdGroup.idxNOTE[0], CdGroup.idxNOTE[1]).Trim();
            //                CdGroup.VETTORE = r.Substring(CdGroup.idxVETTORE[0], CdGroup.idxVETTORE[1]).Trim();
            //                CdGroup.NR_DISTINTA = r.Substring(CdGroup.idxNR_DISTINTA[0], CdGroup.idxNR_DISTINTA[1]).Trim();
            //                CdGroup.DT_DISTINTA = r.Substring(CdGroup.idxDT_DISTINTA[0], CdGroup.idxDT_DISTINTA[1]).Trim();
            //                CdGroup.COND_PAG = r.Substring(CdGroup.idxCOND_PAG[0], CdGroup.idxCOND_PAG[1]).Trim();
            //                CdGroup.CONS_PIANI = r.Substring(CdGroup.idxCONS_PIANI[0], CdGroup.idxCONS_PIANI[1]).Trim();
            //                CdGroup.TEL_PRIMA_CONS = r.Substring(CdGroup.idxTEL_PRIMA_CONS[0], CdGroup.idxTEL_PRIMA_CONS[1]).Trim();
            //                CdGroup.DT_CONS_TASSAT_1 = r.Substring(CdGroup.idxDT_CONS_TASSAT_1[0], CdGroup.idxDT_CONS_TASSAT_1[1]).Trim();
            //                CdGroup.DT_CONS_TASSAT_2 = r.Substring(CdGroup.idxDT_CONS_TASSAT_2[0], CdGroup.idxDT_CONS_TASSAT_2[1]).Trim();
            //                CdGroup.NOTE_1 = r.Substring(CdGroup.idxNOTE_1[0], CdGroup.idxNOTE_1[1]).Trim();
            //                CdGroup.NOTE_2 = r.Substring(CdGroup.idxNOTE_2[0], CdGroup.idxNOTE_2[1]).Trim();
            //                CdGroup.NOTE_3 = r.Substring(CdGroup.idxNOTE_3[0], CdGroup.idxNOTE_3[1]).Trim();
            //                CdGroup.NOTE_4 = r.Substring(CdGroup.idxNOTE_4[0], CdGroup.idxNOTE_4[1]).Trim();
            //                CdGroup.NOTE_5 = r.Substring(CdGroup.idxNOTE_5[0], CdGroup.idxNOTE_5[1]).Trim();
            //                CdGroup.Libero = r.Substring(CdGroup.idxLibero[0], CdGroup.idxLibero[1]).Trim();
            //                //ShipStockHouse.Libero_1 = r.Substring(ShipStockHouse.idxLibero_1[0], ShipStockHouse.idxLibero_1[1]).Trim();
            //                CdGroup.N_PALLETTS = Helper.StringIntString(r.Substring(CdGroup.idxN_PALLETTS[0], CdGroup.idxN_PALLETTS[1]).Trim());
            //                CdGroup.N_CHEP = Helper.StringIntString(r.Substring(CdGroup.idxN_CHEP[0], CdGroup.idxN_CHEP[1]).Trim());
            //                CdGroup.N_EPAL = Helper.StringIntString(r.Substring(CdGroup.idxN_EPAL[0], CdGroup.idxN_EPAL[1]).Trim());
            //                CdGroup.AANN = r.Substring(CdGroup.idxAANN[0], CdGroup.idxAANN[1]).Trim();
            //                CdGroup.TTRAS = r.Substring(CdGroup.idxTTRAS[0], CdGroup.idxTTRAS[1]).Trim();
            //                CdGroup.M_A = r.Substring(CdGroup.idxM_A[0], CdGroup.idxM_A[1]).Trim();
            //                CdGroup.NR_PREBOLLA = r.Substring(CdGroup.idxNR_PREBOLLA[0], CdGroup.idxNR_PREBOLLA[1]).Trim();
            //                //ShipStockHouse.LEAD_TIME = r.Substring(ShipStockHouse.idxLEAD_TIME[0], ShipStockHouse.idxLEAD_TIME[1]).Trim();
            //                //ShipStockHouse.Libero_2 = r.Substring(ShipStockHouse.idxLibero_2[0], ShipStockHouse.idxLibero_2[1]).Trim();
            //                #endregion

            //                #region apimodel
            //                var headerNewShipment = new HeaderNewShipmentTMS();
            //                {
            //                    headerNewShipment.docDate = DateTime.ParseExact(CdGroup.DATA_BOLLA, "yyyyMMdd", null).ToString("MM-dd-yyyy");
            //                    headerNewShipment.publicNote = $"{CdGroup.NOTE.Trim()} {CdGroup.NOTE_1.Trim()} {CdGroup.NOTE_2.Trim()} {CdGroup.NOTE_3.Trim()} {CdGroup.NOTE_4.Trim()} {CdGroup.NOTE_5.Trim()}".Trim();
            //                    //headerNewShipment.customerID = cust.ID_GESPE;
            //                    headerNewShipment.cashCurrency = "EUR";
            //                    headerNewShipment.cashValue = Helper.GetDecimalFromString(CdGroup.IMPORTO_CONTRASS, 2);
            //                    headerNewShipment.externRef = CdGroup.NR_BOLLA;
            //                    headerNewShipment.carrierType = "EDI";//int.Parse(CdGroup.N_BANCALI_BA) > 0 ? "PLT" : "COLLO"; //TODO: chiedere conferma sulla priorità su pallet e colli
            //                    headerNewShipment.serviceType = "S";
            //                    headerNewShipment.incoterm = "PF";
            //                    headerNewShipment.transportType = "8-25";
            //                    headerNewShipment.type = "Groupage";
            //                    headerNewShipment.cashNote = "";
            //                    headerNewShipment.insideRef = CdGroup.MANDANTE; //Dove lo mettiamo? Serve per il tracciato di output
            //                    headerNewShipment.internalNote = $"{CdGroup.NOTE} {CdGroup.NOTE_1} {CdGroup.NOTE_2} {CdGroup.NOTE_3} {CdGroup.NOTE_4} {CdGroup.NOTE_5}";
            //                    headerNewShipment.cashPayment = "";

            //                }

            //                DateTime dataTassativa = DateTime.MinValue;

            //                DateTime.TryParseExact(CdGroup.DT_CONS_TASSAT_1, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dataTassativa);

            //                if (dataTassativa != DateTime.MinValue)
            //                {
            //                    if (dataTassativa < DateTime.Now)
            //                    {
            //                        dataTassativa = DateTime.MinValue;
            //                    }
            //                }

            //                Model.CDGROUP.MagazzinoCDGroup MagazzinoCarico = Model.CDGROUP.SediCaricoCDGroup.RecuperaLaSedeCDGroup(CdGroup.MANDANTE);
            //                if (MagazzinoCarico == null)
            //                {
            //                    MagazzinoCarico = Model.CDGROUP.SediCaricoCDGroup.SedeLegale;
            //                }


            //                var goods = new GoodNewShipmentTMS();
            //                {
            //                    goods.grossWeight = Helper.GetDecimalFromString(CdGroup.PESO_SPEDIZIONE, 3);
            //                    goods.cube = Helper.GetDecimalFromString(CdGroup.VOLUME_SPEDIZIONE, 3);
            //                    goods.packs = int.Parse(CdGroup.NUMERO_COLLI_SPED);
            //                    goods.totalPallet = int.Parse(CdGroup.N_BANCALI_BA);
            //                    goods.floorPallet = int.Parse(CdGroup.N_BANCALI_BA);
            //                }

            //                merce.Add(goods);

            //                #endregion

            //                var suffissoBarCode = CdGroup.NR_PREBOLLA.Substring(3);

            //                var daSegnacollo = int.Parse(CdGroup.DA_SEGNACOLLO);
            //                var aSegnacollo = int.Parse(CdGroup.A_SEGNACOLLO);

            //                //sh = lunghezza 10
            //                //df ph = lunghezza 9
            //                for (int z = daSegnacollo; z <= aSegnacollo; z++)
            //                {
            //                    if (Path.GetFileName(fr).StartsWith("TVE"))
            //                    {
            //                        string BC = z.ToString();
            //                        while (BC.Length < 3)
            //                        {
            //                            BC = "0" + BC;
            //                        }
            //                        var barCode = new ParcelNewShipmentTMS()
            //                        {
            //                            barcodeExt = $"{suffissoBarCode}{BC}",
            //                        };
            //                        parcelNewShipment.Add(barCode);
            //                    }
            //                    else
            //                    {
            //                        string BC = z.ToString();
            //                        while (BC.Length < 9)
            //                        {
            //                            BC = "0" + BC;
            //                        }
            //                        var barCode = new ParcelNewShipmentTMS()
            //                        {
            //                            barcodeExt = $"{BC}",
            //                        };
            //                        parcelNewShipment.Add(barCode);
            //                    }
            //                }

            //                var totSegnacolli = parcelNewShipment.Count;

            //                if (goods.packs > totSegnacolli)
            //                {
            //                    if (goods.grossWeight <= 200)
            //                    {
            //                        goods.totalPallet = 0;
            //                        goods.floorPallet = 0;
            //                        goods.packs = 1;
            //                        goods.width = 80;
            //                        goods.height = 140;
            //                        goods.depth = 90;
            //                        goods.cube = RecuperaIlVolumeInBaseAlPeso(goods.grossWeight);
            //                    }
            //                    else if (goods.floorPallet == 0)
            //                    {
            //                        //goods.packs = 0;
            //                        goods.totalPallet = totSegnacolli;
            //                        goods.floorPallet = totSegnacolli;
            //                    }
            //                }

            //                shipmentTMS.header = headerNewShipment;
            //                shipmentTMS.parcels = parcelNewShipment.ToArray();
            //                shipmentTMS.goods = merce.ToArray();
            //                shipmentTMS.stops = destinazione.ToArray();
            //                ListShip.Add(shipmentTMS);
            //                //InviaNuovaShipmentAPI_UNITEX(shipmentTMS);
            //            }
            //            catch (Exception ee)
            //            {
            //                _loggerCode.Error(ee);
            //            }

            //        }
            //    }
            //} 
            #endregion

            #region RecuperaShipmentAPI
            //var sped = new List<EspritecShipment.ShipmentList>();
            //var fromDate = new DateTime(2023, 03, 31);
            //var toDate = new DateTime(2023, 04, 30, 23, 59, 59);
            //int row = 2000;
            //int maxP = 20;
            //foreach (var cust in CustomerConnections.CDGroup)
            //{
            //    int page = 1;
            //    while (page <= maxP)
            //    {
            //        var rr = EspritecShipment.RestEspritecGetShipmentListTraDate(fromDate, toDate, row, page, cust.tokenAPI);
            //        var rrResp = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(rr.Content);
            //        maxP = rrResp.result.maxPages;
            //        Debug.WriteLine($"{page}-{maxP}-{cust.NOME}");
            //        page++;
            //        if (rrResp.shipments == null) continue;
            //        //var listaPulita = rrResp.shipments.Where(x => x.grossWeight == 0).ToList();
            //        sped.AddRange(rrResp.shipments);
            //    }
            //    maxP = 20;
            //}
            //var tot = sped.Count();
            #endregion

            #region RecuperaShipmentIDTXT
            var sped = File.ReadAllLines("cdGroupSHID.txt").ToList(); 
            var tot = sped.Count();
            #endregion

            int idbg = 0;

            #region CheckVolumeComunicato
            //List<string> VerificaVolumeComunicato = new List<string>();
            //VerificaVolumeComunicato.Add($"ID;DATA DOC;DOC NUM;RIF.CLI;COLLI;PALLET;VOLUME;PESO;PESOVOLUME;VINCENTE");
            //foreach (var sh in sped)
            //{
            //    idbg++;
            //    var pesoVolume = sh.cube * 200;
            //    string vince = "";
            //    if (pesoVolume > sh.grossWeight)
            //    {
            //        vince = "V";
            //    }
            //    else
            //    {
            //        vince = "P";
            //    }
            //    string resp = $"{sh.id};{sh.docDate};{sh.docNumber};{sh.externRef};{sh.packs};{sh.floorPallets};{sh.cube};{sh.grossWeight};{pesoVolume};{vince}";

            //    Debug.WriteLine($"{idbg}-{tot}");
            //    Debug.WriteLine(resp);
            //    if(sh.insideRef.Length!=3)
            //    {
            //        continue;
            //    }
            //    VerificaVolumeComunicato.Add(resp);

            //}
            //File.WriteAllLines("VerificaVolumeComunicato.txt", VerificaVolumeComunicato);
            #endregion

            #region AlgoritmoVolume
            List<string> ModificheEffettuate = new List<string>();
            ModificheEffettuate.Add("ID;DOCNUM;PESO;COLLI;PALLET;VOL.COMUNICATO;VOL.ALGORITMO");
            idbg = 0;
            int iterations = sped.Count();

            foreach (var ss in sped)
            {
                idbg++;
                Debug.WriteLine($"{idbg} --- {iterations}");
                //recupera goods

                var id = int.Parse(ss);
                //var id = sh.id;
                var shipG = EspritecShipment.RestEspritecGetShipment(id, token_UNITEX); 
                var shipDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectEspritecShipment>(shipG.Content);
                var sh = shipDes.shipment;

                //var shipG = EspritecShipment.RestEspritecGetShipmentListByDocNum(ss, 15, 1, token_UNITEX);

                //var shipDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(shipG.Content);

                //if (shipDes.shipments.Count() > 1)
                //{

                //}

                //var sh = shipDes.shipments.OrderBy(x => x.id).Last();

                //if (!shipDes.result.status)
                //{
                //    continue;
                //}
                if (sh.grossWeight >= 200 && sh.floorPallets > 0)
                {
                    continue;
                }
                if (string.IsNullOrEmpty(sh.insideRef) || sh.insideRef.Length != 3)
                {
                    continue;
                }

                var goodsSH = EspritecGoods.RestEspritecGetGoodListOfShipment(id, token_UNITEX);
                var goodsSHDes = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsList>(goodsSH.Content);
                if (goodsSHDes.goods != null && goodsSHDes.goods.Count() > 0)
                {
                    //var corr = ListShip.FirstOrDefault(x => x.header.externRef == shipDes.shipment.externRef);
                    //Debug.WriteLine($"{idbg}-{tot}");
                    //if (corr == null)
                    //{
                    //    if (shipDes.shipment.externRef.StartsWith("000"))
                    //    {
                    //        corr = ListShip.FirstOrDefault(x => x.header.externRef == shipDes.shipment.externRef.Substring(3));
                    //    }
                    //    if (corr == null)
                    //    {
                    //        _loggerAPI.Info($"ID NON TROVATO|{shipDes.shipment.id}|{shipDes.shipment.externRef}");
                    //        continue;

                    //    }
                    //}
                    foreach (var g in goodsSHDes.goods)
                    {
                        Debug.WriteLine($"{idbg}-{tot}-{sh.docDate}-COLLI:{g.packs}-PLT:{g.floorPallet}-PESO:{g.grossWeight}-VOL:{g.cube}-{g.description}");
                        if (!string.IsNullOrEmpty(g.description))
                        {
                            continue;
                        }
                        //if (g.cube > 0.01M)
                        //{
                        //    continue;
                        //}

                        var Rng = new EspritecGoods.RootobjectGoodsUpdate();
                        var goods = new EspritecGoods.GoodsUpdate();
                        Random random = new Random();
                        double volume = (double)g.cube;

                        if (sh.senderDes.ToLower().Contains("montefarmaco") || sh.senderDes.ToLower().Contains("giuliani"))
                        {

                            var pesoV = (decimal)(volume * 200);
                            bool VincePesoReale = (g.packs <= 35 && sh.grossWeight < 300);
                            if (VincePesoReale)
                            {
                                continue;
                            }
                            bool VincePesoVolume = (g.packs > 49);
                            if (VincePesoVolume)
                            {

                                var resto = pesoV % 50;
                                if (sh.grossWeight > pesoV)
                                {
                                    while (g.grossWeight > pesoV)
                                    {
                                        volume = volume + random.NextDouble(0.015, 0.03);
                                        pesoV = (decimal)(volume * 200);
                                        resto = pesoV % 50;
                                    }
                                }
                            }
                            if (g.grossWeight > pesoV)
                            {
                                //che faccio?
                            }
                            for (int i = 0; i < g.packs; i++)
                            {
                                volume += random.NextDouble(0.015, 0.03);
                            }
                        }
                        else
                        {
                            #region obsoleto

                            //if (g.grossWeight < 5 && g.packs == 1)
                            //{
                            //    value = random.NextDouble(0.029, 0.035) * g.packs;
                            //}
                            //else if (g.grossWeight >= 5 && g.grossWeight < 10 && g.packs == 1)
                            //{
                            //    value = random.NextDouble(0.05, 0.055) * g.packs;
                            //}
                            //else if (g.grossWeight >= 10 && g.grossWeight < 20 && g.packs == 1)
                            //{
                            //    value = random.NextDouble(0.12, 0.14);
                            //}
                            //else if (g.grossWeight >= 15 && g.grossWeight < 20)
                            //{
                            //    value = random.NextDouble(0.12, 0.14);
                            //}
                            //else if (g.grossWeight >= 25 && g.grossWeight < 30)
                            //{
                            //    value = random.NextDouble(0.16, 0.18);
                            //}
                            //else if (g.grossWeight >= 45 && g.grossWeight < 50)
                            //{
                            //    value = random.NextDouble(0.26, 0.28);
                            //}
                            //else if (g.grossWeight >= 70 && g.grossWeight < 75)
                            //{
                            //    value = random.NextDouble(0.37, 0.40);
                            //}
                            //else if (g.grossWeight >= 95 && g.grossWeight < 100)
                            //{
                            //    value = random.NextDouble(0.52, 0.56);
                            //}
                            //else if (g.grossWeight >= 145 && g.grossWeight < 150)
                            //{
                            //    value = random.NextDouble(0.76, 0.79);
                            //}
                            //else
                            //{
                            //    value = random.NextDouble(0.029, 0.075) * g.packs;
                            //}

                            #endregion

                            if (g.grossWeight > 3 && g.grossWeight < 7)
                            {
                                volume = random.NextDouble(0.029, 0.035) * g.packs;
                            }
                            else if (g.grossWeight > 7 && g.grossWeight < 15)
                            {
                                volume = random.NextDouble(0.051, 0.055) * g.packs;
                            }
                            else if (g.grossWeight > 15 && g.grossWeight < 25)
                            {
                                volume = random.NextDouble(0.056, 0.076) * g.packs;
                            }
                            else if (g.grossWeight > 25 && g.grossWeight < 50)
                            {
                                volume = random.NextDouble(0.255, 0.28);
                            }
                            else if (g.grossWeight > 50 && g.grossWeight < 75)
                            {
                                volume = random.NextDouble(0.376, 0.389);
                            }
                            else if (g.grossWeight > 75 && g.grossWeight < 100)
                            {
                                volume = random.NextDouble(0.505, 0.529);
                            }
                            //else if (g.grossWeight > 100 && g.grossWeight < 150)
                            //{
                            //    var goodList = EspritecGoods.RestEspritecGetGoodListOfShipment(id, token_UNITEX);
                            //    var desGoodList = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsList>(goodList.Content);

                            //    if (desGoodList.goods is null || !(desGoodList.result.status is true))
                            //    {
                            //        continue;
                            //    }

                            //    if (desGoodList.goods.Length > 1)
                            //    {
                            //        Console.WriteLine($"\r\nSpedizione già volumetrizzata o troppo complessa");
                            //        continue;
                            //    }

                            //    var VolOriginale = g.cube;
                            //    decimal pesoTarget = g.grossWeight + 50M;
                            //    decimal valoreReale = g.cube * 200;
                            //    var volumesh = g.cube;

                            //    if (valoreReale > pesoTarget) continue;

                            //    decimal costanteScavalla = 0.01M;
                            //    while (valoreReale < pesoTarget)
                            //    {
                            //        volumesh = volumesh + costanteScavalla;

                            //        valoreReale = volumesh * 200;

                            //        Console.WriteLine($"{valoreReale} / {pesoTarget}");
                            //    }

                            //    var vecchiaGoods = desGoodList.goods.First();

                            //    var goodPayload = new EspritecGoods.GoodsUpdate()
                            //    {
                            //        id = vecchiaGoods.id,
                            //        cube = volumesh,
                            //        description = desGoodList.goods[0].description,
                            //        containerNo = desGoodList.goods[0].containerNo,
                            //        floorPallet = desGoodList.goods[0].floorPallet,
                            //        totalPallet = desGoodList.goods[0].totalPallet,
                            //        unLoadStopID = desGoodList.goods[0].unLoadStopID,
                            //        type = desGoodList.goods[0].type,
                            //        width = desGoodList.goods[0].width,
                            //        depth = desGoodList.goods[0].deep,
                            //        height = desGoodList.goods[0].height,
                            //        grossWeight = desGoodList.goods[0].grossWeight,
                            //        holderID = desGoodList.goods[0].holderID,
                            //        loadStopID = desGoodList.goods[0].loadStopID,
                            //        meters = desGoodList.goods[0].meters,
                            //        netWeight = desGoodList.goods[0].netWeight,
                            //        packs = desGoodList.goods[0].packs,
                            //        packsTypeDes = desGoodList.goods[0].packsTypeDes,
                            //        packsTypeID = desGoodList.goods[0].packsTypeID,
                            //    };

                            //    var goodsDaAggiornare = new EspritecGoods.RootobjectGoodsUpdate() 
                            //    {
                            //        goods = goodPayload,
                            //    };

                            //    var goodsUpdate = EspritecGoods.RestEspritecUpdateGoodsWithDefaultValue(goodsDaAggiornare, token_UNITEX);

                            //}
                            //else if (g.grossWeight > 150 && g.grossWeight < 200)
                            //{
                            //    var goodList = EspritecGoods.RestEspritecGetGoodListOfShipment(id, token_UNITEX);
                            //    var desGoodList = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsList>(goodList.Content);

                            //    if (desGoodList.goods is null || !(desGoodList.result.status is true))
                            //    {
                            //        continue;
                            //    }

                            //    if (desGoodList.goods.Length > 1)
                            //    {
                            //        Console.WriteLine($"\r\nSpedizione già volumetrizzata o troppo complessa");
                            //        continue;
                            //    }

                            //    var VolOriginale = g.cube;
                            //    decimal pesoTarget = g.grossWeight + 50M;
                            //    decimal valoreReale = g.cube * 200;
                            //    var volumesh = g.cube;

                            //    if (valoreReale > pesoTarget) continue;

                            //    const decimal costanteScavalla = 0.01M;
                            //    while (valoreReale < pesoTarget)
                            //    {
                            //        volumesh = volumesh + costanteScavalla;

                            //        valoreReale = volumesh * 200;

                            //        Console.WriteLine($"{valoreReale} / {pesoTarget}");
                            //    }

                            //    var vecchiaGoods = desGoodList.goods.First();

                            //    var goodPayload = new EspritecGoods.GoodsUpdate()
                            //    {
                            //        id = vecchiaGoods.id,
                            //        cube = volumesh,
                            //        description = desGoodList.goods[0].description,
                            //        containerNo = desGoodList.goods[0].containerNo,
                            //        floorPallet = desGoodList.goods[0].floorPallet,
                            //        totalPallet = desGoodList.goods[0].totalPallet,
                            //        unLoadStopID = desGoodList.goods[0].unLoadStopID,
                            //        type = desGoodList.goods[0].type,
                            //        width = desGoodList.goods[0].width,
                            //        depth = desGoodList.goods[0].deep,
                            //        height = desGoodList.goods[0].height,
                            //        grossWeight = desGoodList.goods[0].grossWeight,
                            //        holderID = desGoodList.goods[0].holderID,
                            //        loadStopID = desGoodList.goods[0].loadStopID,
                            //        meters = desGoodList.goods[0].meters,
                            //        netWeight = desGoodList.goods[0].netWeight,
                            //        packs = desGoodList.goods[0].packs,
                            //        packsTypeDes = desGoodList.goods[0].packsTypeDes,
                            //        packsTypeID = desGoodList.goods[0].packsTypeID,
                            //    };

                            //    var goodsDaAggiornare = new EspritecGoods.RootobjectGoodsUpdate()
                            //    {
                            //        goods = goodPayload,
                            //    };

                            //    var goodsUpdate = EspritecGoods.RestEspritecUpdateGoodsWithDefaultValue(goodsDaAggiornare, token_UNITEX);
                            //}
                            else if (g.grossWeight >= 100)
                            {
                                if (g.packs > 20)
                                {
                                    volume = random.NextDouble(0.530, 0.550);
                                }

                                //bool VincePesoReale = (g.packs < 35 && sh.grossWeight < 300);
                                //if (VincePesoReale)
                                //{
                                //    continue;
                                //}
                                bool VincePesoVolume = (g.packs > 49);
                                if (VincePesoVolume)
                                {

                                    var pesoV = (decimal)(volume * 200);
                                    var resto = pesoV % 50;
                                    if (sh.grossWeight > pesoV)
                                    {
                                        while (g.grossWeight > pesoV)
                                        {
                                            volume = volume + random.NextDouble(0.069, 0.083);
                                            pesoV = (decimal)(volume * 200);
                                            resto = pesoV % 50;
                                        }
                                    }

                                    if (resto > 40)
                                    {
                                        while (resto > 40)
                                        {
                                            volume = volume + 0.020;
                                            pesoV = (decimal)(volume * 200);
                                            resto = pesoV % 50;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //volume = 0;
                                //for (int i = 0; i < g.packs; i++)
                                //{
                                //    volume += random.NextDouble(0.025, 0.055);
                                //}
                                continue;
                            }
                            var pesoVol = (decimal)(volume * 200);
                            if (g.grossWeight > pesoVol)
                            {
                                continue;
                                //che faccio?
                            }

                        }

                        if (g.totalPallet > g.packs)
                        {
                            g.packs = g.totalPallet;
                            g.totalPallet = g.floorPallet = 0;
                        }
                        //var vol = (decimal)value;

                        if (g.cube < (decimal)volume)
                        {
                            goods.cube = (decimal)volume;

                        }
                        else
                        {
                            continue;
                        }

                        goods.id = g.id;
                        //goods.description = $"v~{g.cube}";
                        goods.packs = g.packs;
                        goods.totalPallet = g.totalPallet;
                        goods.floorPallet = g.floorPallet;

                        Debug.WriteLine($"{idbg} - {tot} - {goods.cube} - {g.grossWeight} - {goods.packs} - {goods.totalPallet}");

                        Rng.goods = goods;
                        var gUpdate = EspritecGoods.RestEspritecUpdateGoods(Rng, token_UNITEX);
                        var gUpdateDes = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsUpdateResponse>(gUpdate.Content);

                        if (gUpdateDes != null && !gUpdateDes.status)
                        {

                        }
                        ModificheEffettuate.Add($"{sh.id};{sh.docNumber};{g.grossWeight};{goods.packs};{goods.totalPallet};{g.cube};{volume}");
                    }
                }

            }
            #endregion

            File.WriteAllLines($"LDVModificateVol_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.txt", ModificheEffettuate);

            //List<string> idModificati = new List<string>();
            //foreach(var s in sped)
            //{
            //    idModificati.Add(s.id.ToString());
            //}
            //File.WriteAllLines("idModificatiVolCDG.txt", idModificati);
        }

        private decimal RecuperaIlVolumeInBaseAlPeso(decimal grossWeight)
        {
            //150->199 -> 1.30 a 1.40
            //100->150 -> 0.9 -> 1.2
            //70->100 -> 0.8 -> 1.0
            //50->70 -> 0.7 -> 0.8
            //0->50 -> 0.6 -> 0.7
            Random random = new Random();
            if (grossWeight > 150 && grossWeight < 200)
            {
                return (decimal)random.NextDouble(1.30, 1.49);
            }
            else if (grossWeight > 100 && grossWeight <= 150)
            {
                return (decimal)random.NextDouble(0.9, 1.29);
            }
            else if (grossWeight > 70 && grossWeight <= 100)
            {
                return (decimal)random.NextDouble(0.8, 1.09);
            }
            else if (grossWeight > 50 && grossWeight <= 70)
            {
                return (decimal)random.NextDouble(0.07, 0.08);
            }
            else if (grossWeight <= 50)
            {
                return (decimal)random.NextDouble(0.6, 0.7);
            }
            else
            {
                return 0;
            }
        }

        private void RecuperaEsitiSTM()
        {
            int idbg = 0;
            int daRecuperare = EsitiDaRecuperareParzialiSTM.Count();
            List<string> ConsuntivoRecupero = new List<string>();
            foreach (var esitoParziale in EsitiDaRecuperareParzialiSTM)
            {
                idbg++;
                Debug.WriteLine($"{idbg}-{daRecuperare}");
                FinalizzaEsitoSTM(esitoParziale);
                string s = $"{esitoParziale.NumDDT};{esitoParziale.DataTracking};{esitoParziale.Descrizione_Tracking};{esitoParziale.ProgressivoSpedizione}";
                Debug.WriteLine(s);
                ConsuntivoRecupero.Add(s);
            }
            //foreach(var ep in EsitiDaRecuperareParzialiSTM)
            {
                EsitiDaCoumicareSTM.AddRange(EsitiDaRecuperareParzialiSTM);

            }
            var c = EsitiDaCoumicareSTM.Where(x => x.Descrizione_Tracking == "CONSEGNATA").ToList();
            ProduciEsitiSTM();
        }
        private void FinalizzaEsitoSTM(STM_EsitiOut esitoParziale)
        {

            var shipGespe = EspritecShipment.RestEspritecGetShipmentListByExternalRef(esitoParziale.NumDDT, 50, 1, CustomerConnections.STMGroup.tokenAPI);
            var shipDes = JsonConvert.DeserializeObject<TmsShipmentList>(shipGespe.Content);
            if (shipDes != null && shipDes.shipments == null) { esitoParziale.Descrizione_Tracking = "ND"; return; }
            var tracks = CommonAPITypes.ESPRITEC.EspritecShipment.RestEspritecGetTracking(shipDes.shipments[0].id, CustomerConnections.STMGroup.tokenAPI);
            var tracksDes = JsonConvert.DeserializeObject<RootobjectTestShip>(tracks.Content);
            if (tracksDes != null && tracksDes.events == null) { esitoParziale.Descrizione_Tracking = "ND"; return; }
            var consegnato = tracksDes.events.FirstOrDefault(x => x.statusID == 30);
            if (consegnato == null)
            {
                var geoSpec = GeoTab.FirstOrDefault(x => x.cap == shipDes.shipments[0].lastStopZipCode);
                esitoParziale.regione = (geoSpec != null) ? geoSpec.regione : "ND";
                esitoParziale.Descrizione_Tracking = tracksDes.events.Last().statusDes;
                esitoParziale.DataTracking = tracksDes.events.Last().timeStamp.ToString().Split(' ')[0];
                esitoParziale.ProgressivoSpedizione = shipDes.shipments[0].docNumber;
                return;
            }
            DateTime ts = DateTime.MinValue;
            DateTime.TryParse(consegnato.timeStamp.ToString(), out ts);
            var checkCodiceStato = statiDocumemtoSTM.FirstOrDefault(x => x.IdUnitex == consegnato.statusID);
            if (checkCodiceStato != null)
            {
                var codiceStato = checkCodiceStato.CodiceStato;
                var geoSpec = GeoTab.FirstOrDefault(x => x.cap == shipDes.shipments[0].lastStopZipCode);
                esitoParziale.DataConsegnaEffettiva = (consegnato.statusID == 30) ? ts.ToString("yyyyMMdd") : "        ";
                esitoParziale.DataConsegnaTassativa = "        ";//TODO: verificare data tassativa da API
                esitoParziale.DataSpedizione = shipDes.shipments[0].docDate.ToString("ddMMyyyy");
                esitoParziale.DataTracking = ts.ToString("ddMMyyyy");
                esitoParziale.Descrizione_Tracking = consegnato.statusDes;
                esitoParziale.ID_Tracking = codiceStato;
                esitoParziale.regione = (geoSpec != null) ? geoSpec.regione : "ND";
                esitoParziale.ProgressivoSpedizione = shipDes.shipments[0].docNumber;
            }
        }

        #region RecuperoEsiti
        private void RecuperaEsitiLoreal()
        {
            var daEsitare = File.ReadAllLines(FileEsitiDaRecuperareLoreal).ToList();
            int tot = daEsitare.Count();
            int daRecuperare = EsitiDaRecuperareParzialiLoreal.Count();
            int idbg = 0;
            List<string> ConsuntivoRecupero = new List<string>();
            foreach (var esitoParziale in EsitiDaRecuperareParzialiLoreal)
            {
                idbg++;
                Debug.WriteLine($"{idbg}-{daRecuperare}");
                FinalizzaEsitoLoreal(esitoParziale, out string vettore);
                if (!string.IsNullOrEmpty(esitoParziale.E_RiferimentoCorriere))
                {
                    EsitiDaComunicareLoreal.Add(esitoParziale);
                }
                else
                {

                }
                string s = $"{esitoParziale.E_NumeroDDT};{esitoParziale.E_Causale};{esitoParziale.E_DataConsegnaADestino};{esitoParziale.E_DataConsegnaADestino};UNITEX";//{vettore}";
                Debug.WriteLine(s);
                ConsuntivoRecupero.Add(s);
            }
            ProduciEsitiLoreal();
            File.WriteAllLines("EsitiLoreal.txt", ConsuntivoRecupero);

        }
        private void FinalizzaEsitoLoreal(LorealEsiti esitoParziale, out string vettore)
        {

            var shipGespe = EspritecShipment.RestEspritecGetShipmentListByExternalRef(esitoParziale.E_NumeroDDT, 50, 1, token_UNITEX);
            var shipDes = JsonConvert.DeserializeObject<TmsShipmentList>(shipGespe.Content);
            vettore = shipDes.shipments.First().deliverySupplierDes;
            if (shipDes != null && shipDes.result.status)
            {
                var tracks = CommonAPITypes.ESPRITEC.EspritecShipment.RestEspritecGetTracking(shipDes.shipments[0].id, token_UNITEX);
                var tracksDes = JsonConvert.DeserializeObject<RootobjectTestShip>(tracks.Content);
                if (tracksDes != null && tracksDes.events != null)
                {
                    var consegnato = tracksDes.events.FirstOrDefault(x => x.statusID == 30);
                    if (consegnato != null)
                    {
                        var ts = DateTime.Parse(consegnato.timeStamp);
                        esitoParziale.E_NumeroDDT = shipDes.shipments.First().externRef;
                        esitoParziale.E_RiferimentoNumeroConsegnaSAP = shipDes.shipments.First().insideRef;
                        esitoParziale.E_DataConsegnaADestino = ts.ToString("yyyyMMdd");
                        esitoParziale.E_Causale = "00";
                        esitoParziale.E_SottoCausale = "00";
                        esitoParziale.E_RiferimentoCorriere = shipDes.shipments.First().docNumber;
                        esitoParziale.E_Filler1 = "";
                        esitoParziale.E_Note = consegnato.statusDes;
                        esitoParziale.E_Filler2 = "";
                    }
                    //SOLO PER RECUPERO DA ESITARE!!
                    else
                    {
                        var interessante = tracksDes.events.OrderBy(y => y.id).LastOrDefault();
                        var checkCodiceStato = statiDocumemtoLoreal.FirstOrDefault(x => x.IdUnitex == interessante.statusID);
                        if (checkCodiceStato != null)
                        {
                            var lastTrk = tracksDes.events.OrderBy(x => x.id).Last();
                            var ts = DateTime.Parse(lastTrk.timeStamp);
                            esitoParziale.E_NumeroDDT = shipDes.shipments.First().externRef;
                            esitoParziale.E_RiferimentoNumeroConsegnaSAP = shipDes.shipments.First().insideRef;
                            esitoParziale.E_DataConsegnaADestino = ts.ToString("yyyyMMdd");
                            esitoParziale.E_Causale = checkCodiceStato.CodiceStato;
                            esitoParziale.E_SottoCausale = "";
                            esitoParziale.E_RiferimentoCorriere = shipDes.shipments.First().docNumber;
                            esitoParziale.E_Filler1 = "";
                            esitoParziale.E_Note = lastTrk.statusDes;
                            esitoParziale.E_Filler2 = "";
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
        private void RecuperaEsitiGima()
        {
            var daEsitare = File.ReadAllLines(FileEsitiDaRecuperareGIMA).ToList();
            List<string> Esitati = new List<string>();
            Esitati.Add("Riferimento;Data Tracking;Descrizione Tracking");
            int idbg = 0;
            int tot = daEsitare.Count();
            foreach (var esito in daEsitare)
            {
                string resp = esito;
                try
                {
                    idbg++;
                    Debug.WriteLine($"{idbg}-{tot}");

                    var shipGespe = EspritecShipment.RestEspritecGetShipmentListByExternalRef(esito, 50, 1, token_UNITEX);
                    var shipDes = JsonConvert.DeserializeObject<TmsShipmentList>(shipGespe.Content);
                    if (shipDes != null && shipDes.result.status)
                    {
                        var tracks = CommonAPITypes.ESPRITEC.EspritecShipment.RestEspritecGetTracking(shipDes.shipments[0].id, token_UNITEX);
                        var tracksDes = JsonConvert.DeserializeObject<RootobjectTestShip>(tracks.Content);
                        if (tracksDes != null)
                        {
                            var consegnato = tracksDes.events.FirstOrDefault(x => x.statusID == 30);
                            if (consegnato != null)
                            {
                                resp = resp + $";{consegnato.timeStamp};{consegnato.statusDes}";
                            }
                            else
                            {
                                var ultimoEvento = tracksDes.events.OrderBy(x => x.id).Last();
                                resp = resp + $";{ultimoEvento.timeStamp};{ultimoEvento.statusDes}";
                            }
                        }
                        else
                        {
                            resp = resp + $";ND;";
                        }
                    }
                    else
                    {
                        resp = resp + $";ND;";
                    }
                }
                catch (Exception ee)
                {
                    resp = resp + $";ND;";
                }
                Debug.WriteLine(resp);
                Esitati.Add(resp);

            }
            File.WriteAllLines("EsitiGima.txt", Esitati);
        }
        private void RecuperaEsitiCdGroup()
        {
            List<CDGROUP_EsitiOUT> esitiRecuperati = new List<CDGROUP_EsitiOUT>();
            int daRecuperare = EsitiDaRecuperareParzialiCDGroup.Count();
            int idbg = 0;
            Stopwatch sw = new Stopwatch();
            List<string> ConsuntivoRecupero = new List<string>();
            foreach (var esitoParziale in EsitiDaRecuperareParzialiCDGroup)
            {
                sw.Restart();
                idbg++;
                Debug.WriteLine($"{idbg}-{daRecuperare}");
                FinalizzaEsitoCDGroup(esitoParziale, out string vettore);
                esitiRecuperati.Add(esitoParziale);
                string s = $"{esitoParziale.RIFVETTORE};{esitoParziale.DESCRIZIONE_STATO_CONSEGNA};{esitoParziale.DATA};{vettore}";
                Debug.WriteLine(s + "--------------------------" + sw.Elapsed.TotalSeconds);

                ConsuntivoRecupero.Add(s);
            }
            File.WriteAllLines("ConsuntivoRecuperoCDGroup.txt", ConsuntivoRecupero);
            var testt = esitiRecuperati.ToList();
            var nd = esitiRecuperati.Where(x => x.DESCRIZIONE_STATO_CONSEGNA == "ND").GroupBy(x => x.NUMERO_BOLLA).Select(x => x.FirstOrDefault()).ToList();
            //foreach (var esito in EsitiDaCoumicareCDGroup)
            if (testt.Count > 0)
            {
                //var outPath = $@"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT\TRACK_{DateTime.Now.ToString("yyyMMddHHmmss")}_totale.txt";
                //var outPathND = $@"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT\TRACK_{DateTime.Now.ToString("yyyMMddHHmmss")}_ND.txt";
                //var outPathNG = $@"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT\TRACK_{DateTime.Now.ToString("yyyMMddHHmmss")}_daEsitare.txt";
                var outPathConsegnate = $@"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT\TRACK_{DateTime.Now.ToString("yyyMMddHHmmss")}.txt";
                if (!Directory.Exists($@"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT"))
                {
                    Directory.CreateDirectory($@"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT");
                }
                //if (File.Exists(outPath))
                //{
                //    File.Delete(outPath);
                //}

                //File.WriteAllLines(outPath, ProduciFileTrackingCDGroup(testt));

                if (testt.Count > 0)
                {
                    var testttt = testt.Where(x => x.statoUNITEX == 30 || x.statoUNITEX == 55).ToList();

                    File.WriteAllLines(outPathConsegnate, ProduciFileTrackingCDGroup(testttt));
                    File.WriteAllLines(outPathConsegnate + "2.txt", ConsuntivoRecupero);
                }
                //if (nd.Count > 0)
                //{
                //    File.WriteAllLines(outPathND, ProduciFileTrackingCDGroup(nd));
                //}
            }
        }
        private void RiallineaLDVLoreal()
        {
            List<Logistica93_ShipmentIN> LorealNonTrovate = new List<Logistica93_ShipmentIN>();
            var tutteleLDV = Directory.GetFiles("DaRecuperareLoreal");
            var cust = CustomerConnections.Logistica93;
            foreach (var ldv in tutteleLDV)
            {
                try
                {
                    var righeLDV = File.ReadAllLines(ldv);
                    var pzFr = ldv.Split('_');
                    bool isVichi = pzFr[1] == "ZCAI";

                    var files = Directory.GetFiles(Path.GetDirectoryName(ldv));

                    var corretto = files.Where(x => x.Contains("FSE") && x.Contains(pzFr[2]) && x.Contains(pzFr[3].Split('.')[0])).FirstOrDefault();
                    var tutteLeRighe = File.ReadAllLines(ldv);//Encoding.Default.GetString(File.ReadAllBytes(fr)).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    var FileSegnacolli = File.ReadAllLines(corretto);//Encoding.Unicode.GetString(File.ReadAllBytes(corretto)).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    var ListShipLoreal = new List<Logistica93_ShipmentIN>();
                    int iiDebug = 0;
                    for (int i = 0; i < tutteLeRighe.Count(); i++)
                    {
                        var rigaFile = tutteLeRighe[i];
                        iiDebug++;
                        Debug.WriteLine(iiDebug);

                        var nl = new Logistica93_ShipmentIN();
                        #region InterpretaTestata
                        nl.TipoRecord = rigaFile.Substring(nl.idxTipoRecord[0], nl.idxTipoRecord[1]).Trim();
                        nl.NumeroBorderau = rigaFile.Substring(nl.idxNumeroBorderau[0], nl.idxNumeroBorderau[1]).Trim();
                        nl.DataSpedizione = rigaFile.Substring(nl.idxDataSpedizione[0], nl.idxDataSpedizione[1]).Trim();
                        nl.NumeroDDT = rigaFile.Substring(nl.idxNumeroDDT[0], nl.idxNumeroDDT[1]).Trim();
                        nl.DataDDT = rigaFile.Substring(nl.idxDataDDT[0], nl.idxDataDDT[1]).Trim();
                        nl.RifNConsegna = rigaFile.Substring(nl.idxRifNConsegna[0], nl.idxRifNConsegna[1]).Trim();
                        nl.LuogoSpedizione = rigaFile.Substring(nl.idxLuogoSpedizione[0], nl.idxLuogoSpedizione[1]).Trim();
                        nl.PesoDelivery = rigaFile.Substring(nl.idxPesoDelivery[0], nl.idxPesoDelivery[1]).Trim();
                        nl.TipoCliente = rigaFile.Substring(nl.idxTipoCliente[0], nl.idxTipoCliente[1]).Trim();
                        nl.Destinatario = rigaFile.Substring(nl.idxDestinatario[0], nl.idxDestinatario[1]).Trim();
                        nl.Indirizzo = rigaFile.Substring(nl.idxIndirizzo[0], nl.idxIndirizzo[1]).Trim();
                        nl.Localita = rigaFile.Substring(nl.idxLocalita[0], nl.idxLocalita[1]).Trim();
                        nl.CAP = rigaFile.Substring(nl.idxCAP[0], nl.idxCAP[1]).Trim();
                        nl.SiglaProvDestinazione = rigaFile.Substring(nl.idxSiglaProvDestinazione[0], nl.idxSiglaProvDestinazione[1]).Trim();
                        nl.PIVA_CODF = rigaFile.Substring(nl.idxPIVA_CODF[0], nl.idxPIVA_CODF[1]).Trim();
                        nl.DataConsegna = rigaFile.Substring(nl.idxDataConsegna[0], nl.idxDataConsegna[1]).Trim();
                        nl.TipoDataConsegna = rigaFile.Substring(nl.idxTipoDataConsegna[0], nl.idxTipoDataConsegna[1]).Trim();
                        nl.TipoSpedizione = rigaFile.Substring(nl.idxTipoSpedizione[0], nl.idxTipoSpedizione[1]).Trim();
                        nl.ImportoContrassegno = rigaFile.Substring(nl.idxImportoContrassegno[0], nl.idxImportoContrassegno[1]).Trim();
                        nl.NotaModalitaDiConsegna = rigaFile.Substring(nl.idxNotaModalitaDiConsegna[0], nl.idxNotaModalitaDiConsegna[1]).Trim();
                        nl.NotaCommentiTempiConsegna = rigaFile.Substring(nl.idxNotaCommentiTempiConsegna[0], nl.idxNotaCommentiTempiConsegna[1]).Trim();
                        nl.NotaEPAL = rigaFile.Substring(nl.idxNotaEPAL[0], nl.idxNotaEPAL[1]).Trim();
                        nl.NotaBolla = rigaFile.Substring(nl.idxNotaBolla[0], nl.idxNotaBolla[1]).Trim();
                        nl.NumeroColliDettaglio = rigaFile.Substring(nl.idxNumeroColliDettaglio[0], nl.idxNumeroColliDettaglio[1]).Trim();
                        nl.NumeroColliStandard = rigaFile.Substring(nl.idxNumeroColliStandard[0], nl.idxNumeroColliStandard[1]).Trim();
                        nl.NumeroEspositoriPLV = rigaFile.Substring(nl.idxNumeroEspositoriPLV[0], nl.idxNumeroEspositoriPLV[1]).Trim();
                        nl.NumeroPedane = rigaFile.Substring(nl.idxNumeroPedane[0], nl.idxNumeroPedane[1]).Trim();
                        nl.CodiceCorriere = rigaFile.Substring(nl.idxCodiceCorriere[0], nl.idxCodiceCorriere[1]).Trim();
                        nl.ItinerarioCorriere = rigaFile.Substring(nl.idxItinerarioCorriere[0], nl.idxItinerarioCorriere[1]).Trim();
                        nl.SottoZonaCorriere = rigaFile.Substring(nl.idxSottoZonaCorriere[0], nl.idxSottoZonaCorriere[1]).Trim();
                        nl.NumeroPedaneEPAL = rigaFile.Substring(nl.idxNumeroPedaneEPAL[0], nl.idxNumeroPedaneEPAL[1]).Trim();
                        nl.TipoTrasporto = rigaFile.Substring(nl.idxTipoTrasporto[0], nl.idxTipoTrasporto[1]).Trim();
                        nl.ZonaCorriere = rigaFile.Substring(nl.idxZonaCorriere[0], nl.idxZonaCorriere[1]).Trim();
                        nl.PedanaDirezionale = rigaFile.Substring(nl.idxPedanaDirezionale[0], nl.idxPedanaDirezionale[1]).Trim();
                        nl.CodiceAbbinamento = rigaFile.Substring(nl.idxCodiceAbbinamento[0], nl.idxCodiceAbbinamento[1]).Trim();
                        nl.NumeroOrdineCliente = rigaFile.Substring(nl.idxNumeroOrdineCliente[0], nl.idxNumeroOrdineCliente[1]).Trim();
                        nl.ContrattoCorriere = rigaFile.Substring(nl.idxContrattoCorriere[0], nl.idxContrattoCorriere[1]).Trim();
                        nl.Via3 = rigaFile.Substring(nl.idxVia3[0], nl.idxVia3[1]).Trim();
                        nl.NumeroFattura = rigaFile.Substring(nl.idxNumeroFattura[0], nl.idxNumeroFattura[1]).Trim();
                        nl.PesoPolveri = rigaFile.Substring(nl.idxPesoPolveri[0], nl.idxPesoPolveri[1]).Trim();
                        nl.NumeroFiliale = rigaFile.Substring(nl.idxNumeroFiliale[0], nl.idxNumeroFiliale[1]).Trim();
                        nl.TipoClienteIntestazione = rigaFile.Substring(nl.idxTipoClienteIntestazione[0], nl.idxTipoClienteIntestazione[1]).Trim();
                        nl.DestinatarioFiliale = rigaFile.Substring(nl.idxDestinatarioFiliale[0], nl.idxDestinatarioFiliale[1]).Trim();
                        nl.IndirizzoFiliale = rigaFile.Substring(nl.idxIndirizzoFiliale[0], nl.idxIndirizzoFiliale[1]).Trim();
                        nl.LocalitaFiliale = rigaFile.Substring(nl.idxLocalitaFiliale[0], nl.idxLocalitaFiliale[1]).Trim();
                        nl.CAPFiliale = rigaFile.Substring(nl.idxCAPFiliale[0], nl.idxCAPFiliale[1]).Trim();
                        nl.SiglaProvDestinazioneFiliale = rigaFile.Substring(nl.idxSiglaProvDestinazioneFiliale[0], nl.idxSiglaProvDestinazioneFiliale[1]).Trim();
                        nl.Filler = rigaFile.Substring(nl.idxFiller[0], nl.idxFiller[1]).Trim();
                        nl.DeliveryVolume = rigaFile.Substring(nl.idxDeliveryVolume[0], nl.idxDeliveryVolume[1]).Trim();
                        nl.VolumeUnit = rigaFile.Substring(nl.idxVolumeUnit[0], nl.idxVolumeUnit[1]).Trim();
                        nl.PrioritàConsegna = rigaFile.Substring(nl.idxPrioritàConsegna[0], nl.idxPrioritàConsegna[1]).Trim();
                        //ShipLoreal93.SMSPreavviso = r.Substring(ShipLoreal93.idxSMSPreavviso[0], ShipLoreal93.idxSMSPreavviso[1]).Trim();
                        ListShipLoreal.Add(nl);
                        #endregion
                    }
                    List<Logistica93_ShipmentIN> ListaRaggruppataLoreal = RaggruppaTestateLoreal(ListShipLoreal);

                    foreach (var ShipLoreal93 in ListaRaggruppataLoreal)
                    {
                        var pesoFile = Helper.GetDecimalFromString(ShipLoreal93.PesoDelivery, 2);//merce.Sum(x => x.grossWeight); 
                        var colliFile = int.Parse(ShipLoreal93.NumeroColliDettaglio) + int.Parse(ShipLoreal93.NumeroColliStandard);
                        var pedaneFile = int.Parse(ShipLoreal93.NumeroPedane);

                        //recupera LDV da Gespe
                        var ldvSuGespe = CommonAPITypes.ESPRITEC.EspritecShipment.RestEspritecGetShipmentListByExternalRef(ShipLoreal93.NumeroDDT, 1, 50, cust.tokenAPI);

                        var ldvDes = JsonConvert.DeserializeObject<CommonAPITypes.ESPRITEC.EspritecShipment.RootobjectShipmentList>(ldvSuGespe.Content);
                        //valuta differenze
                        //fai update delle differenze
                        if (!ldvDes.result.status)
                        {
                            ldvSuGespe = CommonAPITypes.ESPRITEC.EspritecShipment.RestEspritecGetShipmentListByExternalRef(ShipLoreal93.NumeroDDT.Substring(3), 1, 50, cust.tokenAPI);
                            ldvDes = JsonConvert.DeserializeObject<CommonAPITypes.ESPRITEC.EspritecShipment.RootobjectShipmentList>(ldvSuGespe.Content);
                            if (!ldvDes.result.status)
                            {
                                var fromOv = "222" + ShipLoreal93.NumeroBorderau.Substring(3);
                                ldvSuGespe = CommonAPITypes.ESPRITEC.EspritecShipment.RestEspritecGetShipmentListByExternalRef(fromOv, 1, 50, cust.tokenAPI);
                                ldvDes = JsonConvert.DeserializeObject<CommonAPITypes.ESPRITEC.EspritecShipment.RootobjectShipmentList>(ldvSuGespe.Content);
                                if (!ldvDes.result.status)
                                {
                                    LorealNonTrovate.Add(ShipLoreal93);
                                }

                            }
                        }
                        bool daAggiornare = false;
                        if (ldvDes != null && ldvDes.result.status)
                        {
                            if (ldvDes.shipments != null && ldvDes.shipments.Count() == 1)
                            {
                                var ship = ldvDes.shipments.First();
                                if (pesoFile != ship.grossWeight)
                                {
                                    daAggiornare = true;
                                }
                                if (ship.packs != colliFile)
                                {
                                    daAggiornare = true;
                                }
                                if (ship.floorPallets != pedaneFile)
                                {
                                    daAggiornare = true;
                                }

                                if (!daAggiornare)
                                {
                                    continue;
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            Debug.WriteLine(ShipLoreal93.NumeroDDT + " non trovato in gespe");
                            //non trovata ldv con quel riferimento
                        }

                        if (daAggiornare)
                        {

                            var GoodDaAggiornare = CommonAPITypes.ESPRITEC.EspritecGoods.RestEspritecGetGoodListOfShipment(ldvDes.shipments.First().id, token_UNITEX);
                            var goodsListDes = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsList>(GoodDaAggiornare.Content);

                            var rGU = new CommonAPITypes.ESPRITEC.EspritecGoods.RootobjectGoodsUpdate();
                            var goodsUpdate = new CommonAPITypes.ESPRITEC.EspritecGoods.GoodsUpdate()
                            {
                                grossWeight = pesoFile,
                                packs = colliFile,
                                floorPallet = pedaneFile,
                                totalPallet = pedaneFile,
                                id = goodsListDes.goods.First().id
                            };
                            rGU.goods = goodsUpdate;
                            var respC = CommonAPITypes.ESPRITEC.EspritecGoods.RestEspritecUpdateGoods(rGU, token_UNITEX);
                            Debug.WriteLine(ShipLoreal93.NumeroDDT + " aggiornato");
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ldv + "errore lettura file");

                }


                #region unused
                //    RootobjectNewShipmentTMS shipmentTMS = new RootobjectNewShipmentTMS();
                //    List<ParcelNewShipmentTMS> parcelNewShipment = new List<ParcelNewShipmentTMS>();
                //    List<StopNewShipmentTMS> destinazione = new List<StopNewShipmentTMS>();
                //    List<GoodNewShipmentTMS> merce = new List<GoodNewShipmentTMS>();

                //    #region TipoSpedizione
                //    string incoterm = "";
                //    if (ShipLoreal93.TipoSpedizione == "F")//Porto franco
                //    {
                //        incoterm = "PF";
                //    }
                //    else if (ShipLoreal93.TipoSpedizione == "C")//Conto Servizio
                //    {
                //        incoterm = "CS";
                //    }
                //    else// == A //Porto assegnato
                //    {
                //        incoterm = "PA";
                //    }
                //    #endregion

                //    #region Tassativa
                //    string unloadDateType = "";
                //    string unloadDate = "";
                //    //TODO: non so come mandare il tipo data di consegna tramite api
                //    bool isTassativa = false;
                //    if (!string.IsNullOrEmpty(ShipLoreal93.DataConsegna))
                //    {
                //        unloadDate = DateTime.ParseExact(ShipLoreal93.DataConsegna, "yyyyMMdd", null).ToString("MM-dd-yyyy");

                //        if (ShipLoreal93.TipoDataConsegna.ToUpper() == "TASSATIVA")
                //        {
                //            isTassativa = true;
                //        }
                //        else if (ShipLoreal93.TipoDataConsegna.ToUpper() == "ENTRO IL")
                //        {
                //            isTassativa = true;
                //        }
                //        else if (ShipLoreal93.TipoDataConsegna.ToUpper() == "DOPO IL")
                //        {
                //            isTassativa = true;
                //        }

                //    }
                //    #endregion

                //    #region TipoTrasporto
                //    string serviceType = "";
                //    if (ShipLoreal93.TipoTrasporto == "ZCOR")
                //    {
                //        serviceType = "S";
                //    }
                //    else if (ShipLoreal93.TipoTrasporto == "ZESP")
                //    {
                //        serviceType = "P";
                //    }
                //    else if (ShipLoreal93.TipoTrasporto == "ZDIR")
                //    {
                //        serviceType = "D";
                //    }
                //    else if (ShipLoreal93.TipoTrasporto == "ZAGE")
                //    {
                //        serviceType = "AGE";
                //    }
                //    else if (ShipLoreal93.TipoTrasporto == "ZCOM")
                //    {
                //        serviceType = "COM";
                //    }
                //    else if (ShipLoreal93.TipoTrasporto == "ZINF")
                //    {
                //        serviceType = "INF";
                //    }
                //    else if (ShipLoreal93.TipoTrasporto == "ZMKT")
                //    {
                //        serviceType = "MKT";
                //    }
                //    else if (ShipLoreal93.TipoTrasporto == "ZTRD")
                //    {
                //        serviceType = "TRD";
                //    }
                //    else
                //    {
                //        serviceType = "S";
                //    }
                //    #endregion

                //    #region DatiTestata
                //    var headerNewShipment = new HeaderNewShipmentTMS()
                //    {
                //        carrierType = "STD",//"COLLO", // non spediscono mai pallet?
                //        serviceType = serviceType,
                //        incoterm = incoterm,
                //        transportType = "8-25",
                //        type = "Groupage",
                //        insideRef = ShipLoreal93.NumeroDDT, //Server per il tracciato di Esiti
                //        internalNote = ShipLoreal93.NotaCommentiTempiConsegna,
                //        externRef = ShipLoreal93.RifNConsegna,  //Server per il tracciato di Esiti
                //        publicNote = ShipLoreal93.NotaModalitaDiConsegna,
                //        docDate = ShipLoreal93.DataDDT,
                //        customerID = cust.ID_GESPE,
                //        cashCurrency = "EUR",
                //        cashValue = Helper.GetDecimalFromString(ShipLoreal93.ImportoContrassegno, 2),
                //        cashPayment = "",
                //        cashNote = "",
                //    };
                //    #endregion

                //    #region IdentificaLorealVichi
                //    //RifNConsegna sarebbe uguale al NumeroDDT in testata
                //    List<string> tuttiISegnacolliDellaSpedizione = new List<string>();
                //    if (isVichi)
                //    {
                //        tuttiISegnacolliDellaSpedizione = FileSegnacolli.Where(x => x.Substring(10, 10) == ShipLoreal93.NumeroDDT).ToList();
                //    }
                //    else
                //    {
                //        tuttiISegnacolliDellaSpedizione = FileSegnacolli.Where(x => x.Substring(10, 10) == ShipLoreal93.RifNConsegna).ToList();
                //    }
                //    #endregion

                //    #region SegnacolliEDettaglioMerce
                //    List<LorealSegnacolli> dettaglioMerce = new List<LorealSegnacolli>();
                //    foreach (var s in tuttiISegnacolliDellaSpedizione)
                //    {
                //        var segnaCollo = new LorealSegnacolli();
                //        segnaCollo.S_BoxBREIT = s.Substring(segnaCollo.idxS_BoxBREIT[0], segnaCollo.idxS_BoxBREIT[1]).Trim();
                //        segnaCollo.S_BoxHOEHE = s.Substring(segnaCollo.idxS_BoxHOEHE[0], segnaCollo.idxS_BoxHOEHE[1]).Trim();
                //        segnaCollo.S_BoxLAENG = s.Substring(segnaCollo.idxS_BoxLAENG[0], segnaCollo.idxS_BoxLAENG[1]).Trim();
                //        segnaCollo.S_CheckDigit = s.Substring(segnaCollo.idxS_CheckDigit[0], segnaCollo.idxS_CheckDigit[1]).Trim();
                //        segnaCollo.S_CodiceCliente = s.Substring(segnaCollo.idxS_CodiceCliente[0], segnaCollo.idxS_CodiceCliente[1]).Trim();
                //        segnaCollo.S_CodiceProdotto = s.Substring(segnaCollo.idxS_CodiceProdotto[0], segnaCollo.idxS_CodiceProdotto[1]).Trim();
                //        segnaCollo.S_DescrizioneProdotto = s.Substring(segnaCollo.idxS_DescrizioneProdotto[0], segnaCollo.idxS_DescrizioneProdotto[1]).Trim();
                //        segnaCollo.S_Filler1 = s.Substring(segnaCollo.idxS_Filler1[0], segnaCollo.idxS_Filler1[1]).Trim();
                //        segnaCollo.S_Filler2 = s.Substring(segnaCollo.idxS_Filler2[0], segnaCollo.idxS_Filler2[1]).Trim();
                //        segnaCollo.S_Filler3 = s.Substring(segnaCollo.idxS_Filler3[0], segnaCollo.idxS_Filler3[1]).Trim();
                //        segnaCollo.S_Marca = s.Substring(segnaCollo.idxS_Marca[0], segnaCollo.idxS_Marca[1]).Trim();
                //        segnaCollo.S_NumeroCollo = s.Substring(segnaCollo.idxS_NumeroCollo[0], segnaCollo.idxS_NumeroCollo[1]).Trim();
                //        segnaCollo.S_NumeroDDT = s.Substring(segnaCollo.idxS_NumeroDDT[0], segnaCollo.idxS_NumeroDDT[1]).Trim();
                //        segnaCollo.S_Peso = s.Substring(segnaCollo.idxS_Peso[0], segnaCollo.idxS_Peso[1]).Trim();
                //        segnaCollo.S_TipoElaborazione = s.Substring(segnaCollo.idxS_TipoElaborazione[0], segnaCollo.idxS_TipoElaborazione[1]).Trim();
                //        segnaCollo.S_TipoImballo = s.Substring(segnaCollo.idxS_TipoImballo[0], segnaCollo.idxS_TipoImballo[1]).Trim();
                //        segnaCollo.S_TipoImballoMagazzino = s.Substring(segnaCollo.idxS_TipoImballoMagazzino[0], segnaCollo.idxS_TipoImballoMagazzino[1]).Trim();
                //        segnaCollo.S_ZonaSpedizione = s.Substring(segnaCollo.idxS_ZonaSpedizione[0], segnaCollo.idxS_ZonaSpedizione[1]).Trim();
                //        segnaCollo.S_DimensionUnit = s.Substring(segnaCollo.idxS_DimensionUnit[0], segnaCollo.idxS_DimensionUnit[1]).Trim();
                //        dettaglioMerce.Add(segnaCollo);

                //    }
                //    int iiDBG = 0;
                //    foreach (var sc in dettaglioMerce)
                //    {
                //        iiDBG++;
                //        Debug.WriteLine(iiDBG);

                //        var goods = new GoodNewShipmentTMS();
                //        {
                //            //inserire specifiche dei colli
                //            goods.description = sc.S_DescrizioneProdotto;
                //            goods.depth = Helper.GetDecimalFromString(sc.S_BoxBREIT, 3);
                //            goods.height = Helper.GetDecimalFromString(sc.S_BoxHOEHE, 3);
                //            goods.width = Helper.GetDecimalFromString(sc.S_BoxLAENG, 3);
                //            goods.grossWeight = Helper.GetDecimalFromString(sc.S_Peso, 3);
                //            goods.packs = 1;
                //        }

                //        var parcel = new ParcelNewShipmentTMS()
                //        {
                //            barcodeExt = $"00{sc.S_NumeroCollo}"
                //        };

                //        parcelNewShipment.Add(parcel);
                //        merce.Add(goods);
                //    }
                //    var pesoDelivery = Helper.GetDecimalFromString(ShipLoreal93.PesoDelivery, 2);//merce.Sum(x => x.grossWeight); 
                //    var colliDelivery = int.Parse(ShipLoreal93.NumeroColliDettaglio) + int.Parse(ShipLoreal93.NumeroColliStandard);
                //    var pedaneDelivery = int.Parse(ShipLoreal93.NumeroPedane);
                //    #endregion

                //    #region CaricoEScarico
                //    var stop = new StopNewShipmentTMS()
                //    {

                //        address = "VIA PRIMATICCIO 155",//ShipLoreal93.IndirizzoFiliale,
                //        country = "IT",
                //        description = "L'OREAL ITALIA SPA",//ShipLoreal93.DestinatarioFiliale.Trim(),
                //        district = "MI",//ShipLoreal93.SiglaProvDestinazioneFiliale,
                //        zipCode = "20147",//ShipLoreal93.CAPFiliale,
                //        location = "MILANO",//ShipLoreal93.LocalitaFiliale,
                //        date = DateTime.Now.ToString("yyyy-MM-dd"),
                //        type = "P",
                //        region = "Lombardia",
                //        time = "",
                //    };
                //    destinazione.Add(stop);

                //    var regione = GeoTab.FirstOrDefault(x => x.cap == ShipLoreal93.CAP);
                //    var stop2 = new StopNewShipmentTMS()
                //    {

                //        address = ShipLoreal93.Indirizzo.Replace("\"", ""),
                //        country = "IT",
                //        description = ShipLoreal93.Destinatario.Trim().Replace("\"", ""),
                //        district = ShipLoreal93.SiglaProvDestinazione,
                //        zipCode = ShipLoreal93.CAP,
                //        location = ShipLoreal93.Localita,
                //        date = !string.IsNullOrEmpty(unloadDate) ? unloadDate : "",
                //        type = "D",
                //        region = regione != null ? regione.regione : "",
                //        time = "",

                //    };

                //    destinazione.Add(stop2);
                //    #endregion

                //    merce[0].cube = Helper.GetDecimalFromString(ShipLoreal93.DeliveryVolume, 3);
                //    shipmentTMS.header = headerNewShipment;
                //    shipmentTMS.parcels = parcelNewShipment.ToArray();
                //    shipmentTMS.goods = merce.ToArray();
                //    shipmentTMS.stops = destinazione.ToArray();
                //    shipmentTMS.isTassativa = isTassativa;
                #endregion

            }

            ScrivimiLeLDVNonTrovate(LorealNonTrovate);
        }
        private void ScrivimiLeLDVNonTrovate(List<Logistica93_ShipmentIN> lorealNonTrovate)
        {
            string outpath = "lorealND.txt";
            foreach (var lnt in lorealNonTrovate)
            {
                File.AppendAllText(outpath, lnt.ToString() + "\r\n");
            }
        }
        private void FinalizzaEsitoCDGroup(CDGROUP_EsitiOUT esitoParziale, out string vettore)
        {
            //recupera la spedizione tramite riferimento bolla
            vettore = "";
            try
            {
                var shipGespe = EspritecShipment.RestEspritecGetShipmentListByExternalRef(esitoParziale.NUMERO_BOLLA, 50, 1, token_UNITEX);
                var shipDes = JsonConvert.DeserializeObject<TmsShipmentList>(shipGespe.Content);
                if (shipDes == null || !shipDes.result.status)
                {
                    shipGespe = EspritecShipment.RestEspritecGetShipmentListByExternalRef(esitoParziale.NUMERO_BOLLA.Substring(3), 50, 1, token_UNITEX);
                    shipDes = JsonConvert.DeserializeObject<TmsShipmentList>(shipGespe.Content);
                }
                if (shipDes != null && shipDes.result.status)
                {
                    if (shipDes.shipments.Count() == 1)
                    {
                        vettore = shipDes.shipments.First().deliverySupplierDes;
                        if (shipDes.shipments[0].customerID == "00551" || shipDes.shipments[0].customerID == "00035" || shipDes.shipments[0].customerID == "00032")
                        {
                            var tracks = EspritecShipment.RestEspritecGetTracking(shipDes.shipments[0].id, token_UNITEX);
                            var tracksDes = JsonConvert.DeserializeObject<RootobjectShipmentTracking>(tracks.Content);
                            if (tracksDes != null && tracksDes.events != null)
                            {
                                var consegnato = tracksDes.events.FirstOrDefault(x => x.statusID == 30);
                                if (consegnato != null)
                                {
                                    var checkCodiceStato = statiDocumemtoCDGroup.FirstOrDefault(x => x.IdUnitex == consegnato.statusID);
                                    if (checkCodiceStato != null)
                                    {
                                        var cc = DateTime.Parse(consegnato.timeStamp.ToString());
                                        esitoParziale.DESCRIZIONE_STATO_CONSEGNA = consegnato.statusDes;
                                        if (string.IsNullOrEmpty(esitoParziale.DATA_BOLLA)) { esitoParziale.DATA_BOLLA = shipDes.shipments[0].docDate.ToString("yyyyMMdd"); }
                                        esitoParziale.STATO_CONSEGNA = checkCodiceStato.CodiceStato;
                                        esitoParziale.LOCALITA = consegnato.stopLocation.ToString();
                                        esitoParziale.DATA_PRESA_CONS = (string.IsNullOrEmpty(esitoParziale.DATA_PRESA_CONS)) ? shipDes.shipments[0].docDate.ToString("yyyyMMdd") : esitoParziale.DATA_PRESA_CONS;
                                        esitoParziale.RIFVETTORE = shipDes.shipments[0].docNumber;
                                        esitoParziale.DATA = (string.IsNullOrEmpty(esitoParziale.DATA)) ? cc.ToString("yyyyMMdd") : esitoParziale.DATA;
                                        esitoParziale.MANDANTE = (string.IsNullOrEmpty(esitoParziale.MANDANTE)) ? shipDes.shipments[0].insideRef : esitoParziale.MANDANTE;
                                        esitoParziale.LOCALITA = shipDes.shipments[0].lastStopLocation;
                                        esitoParziale.statoUNITEX = consegnato.statusID;
                                        esitoParziale.RAGIONE_SOCIALE_VETTORE = "UNITEXPRESS";
                                    }
                                    else
                                    {
                                        esitoParziale.DESCRIZIONE_STATO_CONSEGNA = "ND";
                                        esitoParziale.RIFVETTORE = shipDes.shipments[0].docNumber;
                                        esitoParziale.statoUNITEX = 9999;
                                    }
                                }
                                else
                                {
                                    var ultimoEvento = tracksDes.events.OrderBy(x => x.id).Last();
                                    var checkCodiceStato = statiDocumemtoCDGroup.FirstOrDefault(x => x.IdUnitex == ultimoEvento.statusID);
                                    if (checkCodiceStato != null)
                                    {
                                        var cc = DateTime.Parse(ultimoEvento.timeStamp.ToString());
                                        esitoParziale.DESCRIZIONE_STATO_CONSEGNA = ultimoEvento.statusDes;
                                        if (string.IsNullOrEmpty(esitoParziale.DATA_BOLLA)) { esitoParziale.DATA_BOLLA = shipDes.shipments[0].docDate.ToString("yyyyMMdd"); }
                                        esitoParziale.STATO_CONSEGNA = checkCodiceStato.CodiceStato;
                                        esitoParziale.LOCALITA = ultimoEvento.stopLocation.ToString();
                                        esitoParziale.DATA_PRESA_CONS = (string.IsNullOrEmpty(esitoParziale.DATA_PRESA_CONS)) ? cc.ToString("yyyyMMdd") : esitoParziale.DATA_PRESA_CONS;
                                        esitoParziale.RIFVETTORE = shipDes.shipments[0].docNumber;
                                        esitoParziale.DATA = (string.IsNullOrEmpty(esitoParziale.DATA)) ? shipDes.shipments[0].docDate.ToString("yyyyMMdd") : esitoParziale.DATA;
                                        esitoParziale.MANDANTE = (string.IsNullOrEmpty(esitoParziale.MANDANTE)) ? shipDes.shipments[0].insideRef : esitoParziale.MANDANTE;
                                        esitoParziale.LOCALITA = shipDes.shipments[0].lastStopLocation;
                                        esitoParziale.RAGIONE_SOCIALE_VETTORE = "UNITEXPRESS";
                                        esitoParziale.statoUNITEX = ultimoEvento.statusID;
                                    }
                                    else
                                    {
                                        var cc = DateTime.Parse(ultimoEvento.timeStamp.ToString());
                                        esitoParziale.DESCRIZIONE_STATO_CONSEGNA = ultimoEvento.statusDes;
                                        if (string.IsNullOrEmpty(esitoParziale.DATA_BOLLA)) { esitoParziale.DATA_BOLLA = shipDes.shipments[0].docDate.ToString("yyyyMMdd"); }
                                        esitoParziale.DATA_PRESA_CONS = (string.IsNullOrEmpty(esitoParziale.DATA_PRESA_CONS)) ? cc.ToString("yyyyMMdd") : esitoParziale.DATA_PRESA_CONS;
                                        esitoParziale.RIFVETTORE = shipDes.shipments[0].docNumber;
                                        esitoParziale.RAGIONE_SOCIALE_VETTORE = "UNITEXPRESS";
                                        esitoParziale.statoUNITEX = ultimoEvento.statusID;
                                    }
                                }
                            }
                            else
                            {
                                esitoParziale.DESCRIZIONE_STATO_CONSEGNA = "ND";
                                esitoParziale.RIFVETTORE = shipDes.shipments[0].docNumber;
                                esitoParziale.statoUNITEX = 9999;
                            }

                        }
                        else
                        {
                            //non ci interessa il mandante, internal ref duplicato con un altro cliente
                            Debug.WriteLine("cliente non interessato");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < shipDes.shipments.Count(); i++)
                        {
                            vettore = shipDes.shipments[i].deliverySupplierDes;
                            if (shipDes.shipments[i].customerID == "00551" || shipDes.shipments[i].customerID == "00035" || shipDes.shipments[i].customerID == "00032")
                            {
                                var tracks = CommonAPITypes.ESPRITEC.EspritecShipment.RestEspritecGetTracking(shipDes.shipments[i].id, token_UNITEX);
                                var tracksDes = JsonConvert.DeserializeObject<RootobjectShipmentTracking>(tracks.Content);
                                if (tracksDes != null && tracksDes.events != null)
                                {
                                    var consegnato = tracksDes.events.FirstOrDefault(x => x.statusID == 30);
                                    if (consegnato != null)
                                    {
                                        var checkCodiceStato = statiDocumemtoCDGroup.FirstOrDefault(x => x.IdUnitex == consegnato.statusID);
                                        if (checkCodiceStato != null)
                                        {
                                            var cc = DateTime.Parse(consegnato.timeStamp.ToString());
                                            esitoParziale.DESCRIZIONE_STATO_CONSEGNA = consegnato.statusDes;
                                            esitoParziale.STATO_CONSEGNA = checkCodiceStato.CodiceStato;
                                            if (string.IsNullOrEmpty(esitoParziale.DATA_BOLLA)) { esitoParziale.DATA_BOLLA = shipDes.shipments[i].docDate.ToString("yyyyMMdd"); }
                                            esitoParziale.LOCALITA = consegnato.stopLocation.ToString();
                                            esitoParziale.DATA_PRESA_CONS = (string.IsNullOrEmpty(esitoParziale.DATA_PRESA_CONS)) ? cc.ToString("yyyyMMdd") : esitoParziale.DATA_PRESA_CONS;
                                            esitoParziale.RIFVETTORE = shipDes.shipments[i].docNumber;
                                            esitoParziale.DATA = (string.IsNullOrEmpty(esitoParziale.DATA)) ? shipDes.shipments[i].docDate.ToString("yyyyMMdd") : esitoParziale.DATA;
                                            esitoParziale.MANDANTE = (string.IsNullOrEmpty(esitoParziale.MANDANTE)) ? shipDes.shipments[i].insideRef : esitoParziale.MANDANTE;
                                            esitoParziale.LOCALITA = shipDes.shipments[i].lastStopLocation;
                                            esitoParziale.RAGIONE_SOCIALE_VETTORE = "UNITEXPRESS";
                                            esitoParziale.statoUNITEX = consegnato.statusID;
                                            break;
                                        }
                                        else
                                        {
                                            esitoParziale.DESCRIZIONE_STATO_CONSEGNA = "ND";
                                        }
                                    }
                                    else
                                    {
                                        var ultimoEvento = tracksDes.events.OrderBy(x => x.id).Last();
                                        var checkCodiceStato = statiDocumemtoCDGroup.FirstOrDefault(x => x.IdUnitex == ultimoEvento.statusID);
                                        if (checkCodiceStato != null)
                                        {
                                            var cc = DateTime.Parse(ultimoEvento.timeStamp.ToString());
                                            esitoParziale.DESCRIZIONE_STATO_CONSEGNA = ultimoEvento.statusDes;
                                            if (string.IsNullOrEmpty(esitoParziale.DATA_BOLLA)) { esitoParziale.DATA_BOLLA = shipDes.shipments[i].docDate.ToString("yyyyMMdd"); }
                                            esitoParziale.STATO_CONSEGNA = checkCodiceStato.CodiceStato;
                                            esitoParziale.LOCALITA = ultimoEvento.stopLocation.ToString();
                                            esitoParziale.DATA_PRESA_CONS = (string.IsNullOrEmpty(esitoParziale.DATA_PRESA_CONS)) ? cc.ToString("yyyyMMdd") : esitoParziale.DATA_PRESA_CONS;
                                            esitoParziale.RIFVETTORE = shipDes.shipments[i].docNumber;
                                            esitoParziale.DATA = (string.IsNullOrEmpty(esitoParziale.DATA)) ? shipDes.shipments[i].docDate.ToString("yyyyMMdd") : esitoParziale.DATA;
                                            esitoParziale.MANDANTE = (string.IsNullOrEmpty(esitoParziale.MANDANTE)) ? shipDes.shipments[i].insideRef : esitoParziale.MANDANTE;
                                            esitoParziale.LOCALITA = shipDes.shipments[i].lastStopLocation;
                                            esitoParziale.RAGIONE_SOCIALE_VETTORE = "UNITEXPRESS";
                                            esitoParziale.statoUNITEX = ultimoEvento.statusID;
                                        }
                                        else
                                        {
                                            esitoParziale.DESCRIZIONE_STATO_CONSEGNA = ultimoEvento.statusDes;
                                            esitoParziale.RIFVETTORE = shipDes.shipments[i].docNumber;
                                            esitoParziale.statoUNITEX = ultimoEvento.statusID;
                                        }
                                    }
                                }
                                else
                                {
                                    esitoParziale.DESCRIZIONE_STATO_CONSEGNA = "ND";
                                    esitoParziale.RIFVETTORE = shipDes.shipments[i].docNumber;
                                    esitoParziale.statoUNITEX = 9999;
                                }

                            }
                            else
                            {
                                //non ci interessa il mandante, internal ref duplicato con un altro cliente
                                Debug.WriteLine("cliente non interessato");
                            }
                        }
                    }

                }
                else
                {
                    //ldv non trovata 

                    esitoParziale.DESCRIZIONE_STATO_CONSEGNA = "ND";
                    esitoParziale.statoUNITEX = 99999;
                }
            }
            catch (Exception ee)
            {
                esitoParziale.DESCRIZIONE_STATO_CONSEGNA = "ND";

            }

        }
        #endregion


        private void CheckCustomerPath()
        {
            foreach (var cust in CustomerConnections.customers)
            {
                if (string.IsNullOrEmpty(cust.LocalInFilePath))
                {
                    continue;
                }
                if (!Directory.Exists(cust.LocalInFilePath))
                {
                    Directory.CreateDirectory(cust.LocalInFilePath);
                }
                if (!Directory.Exists(cust.LocalWorkPath))
                {
                    Directory.CreateDirectory(cust.LocalWorkPath);
                }
                if (!Directory.Exists(cust.LocalErrorFilePath))
                {
                    Directory.CreateDirectory(cust.LocalErrorFilePath);
                }
                if (!Directory.Exists(cust.PathEsiti))
                {
                    Directory.CreateDirectory(cust.PathEsiti);
                }
            }
        }
        private void CaricaConfigurazioni()
        {
            var conf = File.ReadAllLines(config);
            cicloTimer = double.Parse(conf[5]);
        }
        private void SetTimer()
        {

            timerAggiornamentoCiclo = new Timer(cicloTimer);
            timerAggiornamentoCiclo.Elapsed += OnTimedEvent;
            timerAggiornamentoCiclo.AutoReset = true;
            timerAggiornamentoCiclo.Enabled = true;

            timerEsiti = new Timer(cicloTimerEsitiCDGroup);
            timerEsiti.Elapsed += OnTimedEventCambiTMS;
            timerEsiti.AutoReset = true;
            timerEsiti.Enabled = true;

            //SW.Start();
            //controlloDocumentiInCarico.AutoReset = true;
            //controlloDocumentiInCarico.Enabled = true;
            _loggerCode.Info($"Timer ciclo settato {timerAggiornamentoCiclo.Interval} ms");
            //_loggerCode.Info($"Timer esiti settato {timerEsiti.Interval} ms");
        }

        #region Gestito con servizio adHoc 
        string PathLastCheckChangesFileTMS = "LastCheckChangesTMS.txt";
        private void RecuperaLastCheckChangesTMS()
        {
            LastCheckChangesTMS = DateTime.Parse(File.ReadAllLines(PathLastCheckChangesFileTMS)[0]);
        }
        
        private void OnTimedEventCambiTMS(object sender, ElapsedEventArgs e)
        {
            try
            {
                timerEsiti.Stop();
                var dtn = DateTime.Now;
                RecuperaConnessione();
                _loggerCode.Info("recupero cambi tms in corso...");
                RecuperaLastCheckChangesTMS();
                string ts = (LastCheckChangesTMS).ToString("s", CultureInfo.InvariantCulture);
                int totCambi = 0;
                LastCheckChangesTMS = new DateTime(dtn.Year, dtn.Month, dtn.Day, dtn.Hour, dtn.Minute, 00);
                _loggerCode.Info("Recupero Cambi TMS da API");
                foreach (var cust in CustomerConnections.customers.Where(x => x.ClienteDaEsitare))
                {
                    //DEBUGITERATOR
                    //if (cust.ID_GESPE == "00327")
                    //{
                    //}
                    //else { continue; }

                    var tuttiItracking = CambiTMSDelCliente(cust, ts); //Prende tutti i tracking di tutti i clienti e li accumula
                    totCambi += tuttiItracking.Count();
                    if (tuttiItracking.Count() > 0)
                    {
                        _loggerCode.Info($"Trovati {tuttiItracking.Count()} cambi spedizioni per il cliente {cust.NOME}");
                        var payload = JsonConvert.SerializeObject(tuttiItracking, Formatting.Indented);
                        if (!Directory.Exists("../PayloadTracking"))
                        {
                            Directory.CreateDirectory("../PayloadTracking");
                        }
                        File.WriteAllText($@"../PayloadTracking/{cust.NOME}_{LastCheckChangesTMS.ToString("ddMMyyyyHHmmss")}.txt", payload);
                    }
                    int idbg = 0;
                    foreach (var ShipTrackingUnitex in tuttiItracking)
                    {

                        idbg++;
                        Debug.WriteLine(idbg + " - " + tuttiItracking.Count());

                        try
                        {
                            var shipUnitex = RecuperaShipUnitexByShipmentID(ShipTrackingUnitex.shipID);
                            _loggerCode.Debug($"Tracking:ID:{ShipTrackingUnitex.id}|Stato:{ShipTrackingUnitex.statusDes}|Rif.Cli:{shipUnitex.externRef}|DocNum:{shipUnitex.docNumber}");
                            
                            //TODO: Claudio RDZ Valuto se registrare o no
                            AggiungiAllaListaLesito(cust, ShipTrackingUnitex, shipUnitex); //Raddrizza x tutti qualli abilitati e valuta i tracking
                        }
                        catch (Exception ee)
                        {
                            _loggerCode.Error(ee);
                        }
                    }
                }

                //----------------TODO: Claudio RDZ 2 Step Raddrizzo a fine girnata le spedizioni Registrate in funzione delle percentuali ----------------------
                // Se fine giornata valuto le spedizioni registrate 
                // Carico le spedizioni ne valuto la quantita di quelle registrate e in funzione delle percentuali assegnate procedo allo stesso RDZ
                
                //if (dtn.Hour > 21)
                //{
                //    int Percentuale = 0, Totale = 0, NumeroFileRDZ =0;                 
                //    GestObjectToFile.Json JF = new GestObjectToFile.Json();
                //    foreach (var cust in CustomerConnections.customers.Where(x => x.ClienteDaEsitare))
                //    {
                //        var Percorso = Path.Combine(".", cust.NOME);
                //        string[] allfiles = Directory.GetFiles("Percorso", "*.*", SearchOption.AllDirectories);
                //        Array.Sort(allfiles);

                //        Percentuale = 0;
                //        Totale = allfiles.Length / 2;

                //        if (cust.NOME == "GXO") { Percentuale = 95;}
                //        else if (cust.NOME == "CEVA") { Percentuale = 95; }
                //        else if (cust.NOME == "PHARDIS") { Percentuale = 0; }
                //        else if (cust.NOME == "STM") { Percentuale = 93; }
                //        else { Percentuale = 80; }

                //        //NumeroFileRDZ di file da raddrizzaree in funzione della percentuale di Raddrizzamento
                //        NumeroFileRDZ = Convert.ToInt32 ((Totale * Percentuale) / 100);

                //        for (int cnt = 0; cnt < NumeroFileRDZ; cnt = cnt + 2)
                //        {
                //            var FileNameTracking = allfiles[cnt];
                //            var FileNameShipment = allfiles[cnt + 1];
                //            List<EventTracking> ListaTracking = JF.ReadFromFile<EventTracking>(FileNameTracking);
                //            List<Shipment> ListaShipment = JF.ReadFromFile<Shipment>(FileNameShipment);
                //            RaddrizzaSpedizioneRegistrata(ListaTracking[0], ListaShipment[0], cust);
                //        }
                //    }
                //}
                
                //-----------------------------------------------------------------------------------------------------------------------------------------------

                //Da qui in poi vengono prodotti i file (Esiti) risposta da mandare agli FTP / API dei vari clienti
                ProduciEsitiCDGroup();
                ProduciEsitiSTM();
                ProduciEsitiLoreal();
                ProduciEsitiDamora();
                ProduciEsitiChiapparoli();
                //ProduciEsiti3C();


                var TotDaRadGXO = NumeroEsitiDaRaddrizzareGXO(90);
                RaddrizzaEsitiGXO();
                
                var TotdaRadCEVA = NumeroEsitiDaRaddrizzareCEVA(90);
                RaddrizzaEsitiCEVA();

                _loggerCode.Info("invio esiti completato");

                _loggerCode.Info($"LastTimeCheckTMS: {LastCheckChangesTMS.ToString("dd/MM/yyyy HH:mm:ss")} - Cambi recuperati {totCambi}");
                ScriviLastCheckChangesTMS(false);
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("OnTimedEventEsiti", ee);
                }

                LastException = ee;
            }
            finally
            {
                timerEsiti.Start();
            }
        }


        //TODO: Claudio RDZ da sistemare(RaddrizzaEsitiCEVA() RaddrizzaEsitiGXO())

        private int NumeroEsitiDaRaddrizzareCEVA(double PercentualeRDZ)
        {
            try
            {
                var cust = CustomerConnections.CEVA;
                var FilesDaRaddrizzare = Directory.GetFiles(cust.PathEsiti);
                double NumeroTotaleraDdrizzabili = 0d, NumeroDaRaddrizzare = 0d;

                foreach (var fdr in FilesDaRaddrizzare)
                {
                    var esiti = File.ReadAllLines(fdr);
                    int idRiga = 0;

                    foreach (var esito in esiti)
                    {
                        idRiga++;
                        if (idRiga <= 2) continue;

                        var dataEsito = esito.Substring(527, 12);
                        var dataEsitoDT = DateTime.MinValue;
                        DateTime.TryParseExact(dataEsito, "yyyyMMddHHmm", null, DateTimeStyles.None, out dataEsitoDT);

                        if (dataEsitoDT != DateTime.MinValue)
                        {
                            var sh = EspritecShipment.RestEspritecGetShipmentListByDocNum(esito.Substring(457, 10), 100, 1, token_UNITEX);
                            var shDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(sh.Content).shipments.LastOrDefault();
                            var geo = GeoSpec.GeoList.FirstOrDefault(x => x.citta.ToLower().Trim() == shDes.lastStopLocation.ToLower().Trim());
                            if (geo == null) geo = GeoSpec.GeoList.FirstOrDefault(x => x.cap.ToLower().Trim() == shDes.lastStopZipCode.ToLower().Trim());
                            if (geo == null) continue;

                            var rMax = TempiResa.TempiResaUtils.RecuperaOreResaOttimali(geo, shDes.ownerAgency, false);
                            if ((geo.regione.ToLower() == "lombardia" && geo.provincia.ToLower() == "mn") || (geo.regione.ToLower() == "lombardia" && geo.provincia.ToLower() == "so")) rMax = rMax - 24;
                            int OreResa = LocalGoogleCalendar.GiorniDiResaEffettivi(shDes.docDate.Date, dataEsitoDT.Date) * 24;

                            var dataRaddrizzata = ValutaERaddrizzaIlDato(rMax, OreResa, dataEsitoDT);
                            var Ritardo = OreResa - rMax;

                            if (dataEsitoDT > dataRaddrizzata)
                            {
                                var trks = EspritecShipment.RestEspritecGetTracking(shDes.id, token_UNITEX);
                                var trksDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentTracking>(trks.Content);
                                var consegnato = trksDes.events.FirstOrDefault(x => x.statusID == 30);
                                var HasGiacenza = trksDes.events.Any(x => x.statusID == 50);
                                var IsPrenotata = trksDes.events.Any(x => x.statusID == 61);

                                if (!(HasGiacenza || IsPrenotata || consegnato == null))
                                {
                                    if (dataEsitoDT > dataRaddrizzata)
                                    {
                                        NumeroTotaleraDdrizzabili++;
                                        LOGCNTCEVA.ScriviLogManuale("CEVA - FILE: " + fdr + " - RDZ :" + shDes.docNumber);
                                    }
                                    else LOGCNTCEVA.ScriviLogManuale("GXO - FILE: " + fdr + " - NN + (Giacenza = " + HasGiacenza.ToString() + " Prenotata = " + IsPrenotata.ToString() + " Consegnata = " + consegnato.ToString() + ") :" + shDes.docNumber);
                                }
                                else LOGCNTCEVA.ScriviLogManuale("GXO - FILE: " + fdr + " - NN(Data Esito <= Data Raddrizzata) :" + shDes.docNumber);

                            }
                        }
                    }
                }
                NumeroDaRaddrizzare = (NumeroTotaleraDdrizzabili / 100) * PercentualeRDZ;
                LOGCNTCEVA.ScriviLogManuale("NumeroTotaleDaRDZ: " + Math.Round(NumeroDaRaddrizzare).ToString());
                return int.Parse(Math.Round(NumeroDaRaddrizzare).ToString());                
            }
            catch (Exception ex)
            {
                LOGCNTCEVA.ScriviLog(ref ex);
                return 0;
            }
        }
        private void RaddrizzaEsitiCEVA()
        {
            var cust = CustomerConnections.CEVA;
            var FilesDaRaddrizzare = Directory.GetFiles(cust.PathEsiti);

            foreach (var fdr in FilesDaRaddrizzare)
            {
                try
                {
                    List<string> raddrizzati = new List<string>();
                    var esiti = File.ReadAllLines(fdr);
                    var dir = Path.GetDirectoryName(fdr);
                    string fileBK = fdr + ".bk";
                    File.Copy(fdr, fileBK);
                    int idRiga = 0;
                    List<string> righeRaddrizzate = new List<string>();
                    foreach (var esito in esiti)
                    {
                        idRiga++;

                        if (idRiga <= 2)
                        {
                            raddrizzati.Add(esito);
                            continue;
                        }

                        #region DaMettereInProd

                        var dataEsito = esito.Substring(527, 12);
                        var dataEsitoDT = DateTime.MinValue;
                        DateTime.TryParseExact(dataEsito, "yyyyMMddHHmm", null, DateTimeStyles.None, out dataEsitoDT);

                        if (dataEsitoDT != DateTime.MinValue)
                        {
                            var sh = EspritecShipment.RestEspritecGetShipmentListByDocNum(esito.Substring(457, 10), 100, 1, token_UNITEX);
                            var shDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(sh.Content).shipments.LastOrDefault();
                            var geo = GeoSpec.GeoList.FirstOrDefault(x => x.citta.ToLower().Trim() == shDes.lastStopLocation.ToLower().Trim());
                            if (geo == null)
                            {
                                geo = GeoSpec.GeoList.FirstOrDefault(x => x.cap.ToLower().Trim() == shDes.lastStopZipCode.ToLower().Trim());
                            }
                            if (geo == null)
                            {
                                raddrizzati.Add(esito);
                                continue;
                                //TODO: decidere se inviare una mail per aggiungere i dettagli mancanti in GEO
                                //throw new Exception($"impossibile determinare la geo: città: {shDes.lastStopLocation.ToLower().Trim()} cap: {shDes.lastStopZipCode.ToLower().Trim()}");
                            }

                            var rMax = TempiResa.TempiResaUtils.RecuperaOreResaOttimali(geo, shDes.ownerAgency, false);

                            if ((geo.regione.ToLower() == "lombardia" && geo.provincia.ToLower() == "mn") || (geo.regione.ToLower() == "lombardia" && geo.provincia.ToLower() == "so"))
                            {
                                rMax = rMax - 24;
                            }

                            int OreResa = LocalGoogleCalendar.GiorniDiResaEffettivi(shDes.docDate.Date, dataEsitoDT.Date) * 24;

                            Random rand = new Random();
                            bool RNGCheck = false;

                            // Claudio Cambio raddrizzamento CEVA (rand.Next(1, 101) <= 90)
                            if (rand.Next(1, 101) <= 95)
                            {
                                RNGCheck = true;
                            }






                            //Il punto focale è qui, se non viene raggiunto non viene raddrizzato
                            var dataRaddrizzata = ValutaERaddrizzaIlDato(rMax, OreResa, dataEsitoDT);
                            var Ritardo = OreResa - rMax;

                            if (dataEsitoDT <= dataRaddrizzata)
                            {
                                raddrizzati.Add(esito);
                            }
                            else
                            {
                                var trks = EspritecShipment.RestEspritecGetTracking(shDes.id, token_UNITEX);
                                var trksDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentTracking>(trks.Content);
                                var consegnato = trksDes.events.FirstOrDefault(x => x.statusID == 30);
                                var HasGiacenza = trksDes.events.Any(x => x.statusID == 50);
                                var IsPrenotata = trksDes.events.Any(x => x.statusID == 61);

                                if (HasGiacenza || IsPrenotata || consegnato == null)
                                {
                                    raddrizzati.Add(esito);
                                    continue;
                                }

                                var dataTSOrig = DateTime.MinValue;
                                DateTime.TryParseExact(consegnato.timeStamp, "dd/MM/yyyy HH:mm:ss", null, DateTimeStyles.None, out dataTSOrig);
                                string info = $"RDZ{dataRaddrizzata.ToString("yyMMdd")}";// dataRaddrizzata.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                                var rdm = new Random().Next(100, 999);
                                var Trupd = new EspritecShipment.RootobjectTrackingUpdate()
                                {
                                    tracking = new EspritecShipment.TrackingUpdate()
                                    {
                                        id = consegnato.id,
                                        info = info,
                                        signature = "",
                                        timeStamp = dataTSOrig.ToString($"yyyy-MM-ddTHH:mm:ss.{rdm}Z")
                                    }
                                };

                                if (string.IsNullOrEmpty(geo.provincia) || string.IsNullOrEmpty(geo.regione))
                                {
                                    try
                                    {
                                        geo.provincia = shDes.lastStopDistrict;
                                        geo.regione = GeoSpec.RecuperaRegioneDaProvincia(geo.provincia);
                                    }
                                    catch (NullReferenceException ee)
                                    {
                                        GestoreMail.SegnalaErroreDev("Errore nel determinare la geo nella funzione di recovery", ee);
                                    }
                                    finally
                                    {
                                        geo.provincia = "ND";
                                        geo.regione = "ND";
                                    }
                                }

                                //if (number < 10) //Se è un numero tra 90 e 100 non la toccare, 10% di chance che non venga toccata.
                                //{
                                //    //Brutto, brutto, brutto, brutto.
                                //    RNGCheck = false;
                                //    string NonRaddrizzataPerCaso = $"{shDes.id};{shDes.docNumber};{cust.NOME};{RNGCheck};{shDes.pickupDateTime.Value.ToShortDateString()};{dataTSOrig.ToShortDateString()};{dataRaddrizzata.ToShortDateString()};{Ritardo};{DateTime.Now};{geo.provincia};{geo.isCapoluogo};{geo.regione}"; List<string> daComunicare = new List<string>();
                                //    daComunicare.Add(NonRaddrizzataPerCaso);
                                //    File.AppendAllLines(FileRaddrizzatiDaComunicare, daComunicare);
                                //    daComunicare.Clear();
                                //    continue;
                                //}

                                var ok = EspritecShipment.RestEspritecUpdateTracking(Trupd, token_UNITEX);
                                var okDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectTrackingUpdateResponse>(ok.Content);

                                string Raddrizzata = $"{shDes.id};{shDes.docNumber};{cust.NOME};{RNGCheck};{shDes.pickupDateTime.Value.ToShortDateString()};{dataTSOrig.ToShortDateString()};{dataRaddrizzata.ToShortDateString()};{Ritardo};{DateTime.Now};{geo.provincia};{geo.isCapoluogo};{geo.regione}";
                                List<string> daComunicare2 = new List<string>();
                                daComunicare2.Add(Raddrizzata);
                                File.AppendAllLines(FileRaddrizzatiDaComunicare, daComunicare2);
                                daComunicare2.Clear();

                                if (okDes.status)
                                {
                                    string initEsito = esito.Substring(0, 527);
                                    string raddEsito = dataRaddrizzata.ToString("yyyyMMddHHmm");
                                    string endEsito = esito.Substring(539);
                                    string raddrizzato = initEsito + raddEsito + endEsito;
                                    raddrizzati.Add(raddrizzato);
                                    righeRaddrizzate.Add(idRiga.ToString());
                                }
                                else
                                {
                                    throw new Exception("Errore comunicazione con gespe");

                                }
                            }

                        }
                        else
                        {
                            string msg = $"Errore nella calcolo della data esito: {dataEsito} per la riga da correggere {idRiga} del file {Path.GetFileName(fdr)}";
                            _loggerCode.Error(msg);
                            throw new Exception(msg);
                        }
                        #endregion
                    }
                    File.WriteAllLines(fdr, raddrizzati);

                    var justName = Path.GetFileName(fdr);
                    var dest = Path.Combine(cust.PathEsitiDelCliente, justName);
                    var storeAT = Path.Combine(dir, "Inviati");
                    if (!Directory.Exists(storeAT))
                    {
                        Directory.CreateDirectory(storeAT);
                    }
                    string unicaRiga = "";
                    foreach (var r in righeRaddrizzate)
                    {
                        unicaRiga += r + " - ";
                    }

                    string fileFDRF = Path.Combine(storeAT, justName);
                    string fileBKM = Path.Combine(storeAT, justName + ".bk");
                    var nl = new List<string>() { fileFDRF, fileBKM };
                    File.Move(fdr, fileFDRF);
                    File.Move(fileBK, fileBKM);

                    //GestoreMail.SendMail(nl, "r.ninno@xcmhealthcare.com", "RaddrizzatoCEVA", $"In allegato il file da controllare raddrizzato (credenziali in customerspec)\r\n\r\nrighe modificate: {unicaRiga} ");

                    _ftp = CreaClientFTPperIlCliente(cust);
                    _ftp.UploadFile(fileFDRF, dest);
                    _ftp.Dispose();

                }
                catch (Exception ee)
                {
                    if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                    {
                        DateLastException = DateTime.Now;
                        GestoreMail.SegnalaErroreDev("RaddrizzaEsitiCEVA", ee);
                    }
                    LastException = ee;
                }
            }
        }

        private int NumeroEsitiDaRaddrizzareGXO(double PercentualeRDZ)
        {
            try
            {
                var cust = CustomerConnections.GXO;
                var FilesDaRaddrizzare = Directory.GetFiles(cust.PathEsiti);

                double NumeroTotaleraDdrizzabili = 0d, NumeroDaRaddrizzare = 0d;

                foreach (var fdr in FilesDaRaddrizzare)
                {
                    var esiti = File.ReadAllLines(fdr);
                    foreach (var esito in esiti)
                    {
                        var dataEsito = esito.Substring(26, 12);
                        var dataEsitoDT = DateTime.MinValue;
                        DateTime.TryParseExact(dataEsito, "yyyyMMddHHmm", null, DateTimeStyles.None, out dataEsitoDT);

                        if (dataEsitoDT != DateTime.MinValue)
                        {
                            var sh = EspritecShipment.RestEspritecGetShipmentListByExternalRef(esito.Substring(0, 10), 100, 1, token_UNITEX);
                            var shDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(sh.Content).shipments.LastOrDefault();
                            var geo = GeoSpec.GeoList.FirstOrDefault(x => x.citta.ToLower().Trim() == shDes.lastStopLocation.ToLower().Trim());
                            if (geo == null) geo = GeoSpec.GeoList.FirstOrDefault(x => x.cap.ToLower().Trim() == shDes.lastStopZipCode.ToLower().Trim());
                            if (geo == null) continue;

                            var rMax = TempiResa.TempiResaUtils.RecuperaOreResaOttimali(geo, shDes.ownerAgency, false);
                            int OreResa = LocalGoogleCalendar.GiorniDiResaEffettivi(shDes.docDate.Date, dataEsitoDT.Date) * 24;

                            var dataRaddrizzata = ValutaERaddrizzaIlDato(rMax, OreResa, dataEsitoDT);
                            var Ritardo = OreResa - rMax;

                            if (dataEsitoDT > dataRaddrizzata)
                            {
                                var trks = EspritecShipment.RestEspritecGetTracking(shDes.id, token_UNITEX);
                                var trksDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentTracking>(trks.Content);
                                var consegnato = trksDes.events.FirstOrDefault(x => x.statusID == 30);
                                var HasGiacenza = trksDes.events.Any(x => x.statusID == 50);
                                var IsPrenotata = trksDes.events.Any(x => x.statusID == 61);
                                if (!(HasGiacenza || IsPrenotata))
                                {
                                    NumeroTotaleraDdrizzabili++;
                                    LOGCNTGXO.ScriviLogManuale("GXO - FILE: " + fdr + " - RDZ :" + shDes.docNumber);
                                }
                                else LOGCNTGXO.ScriviLogManuale("GXO - FILE: " + fdr + " - NN + (Giacenza = " + HasGiacenza.ToString() + " Prenotata = " + IsPrenotata.ToString() + " Consegnata = " + consegnato.ToString() + ") :"  + shDes.docNumber );
                            }
                            else LOGCNTGXO.ScriviLogManuale("GXO - FILE: " + fdr + " - NN(Data Esito <= Data Raddrizzata) :" + shDes.docNumber );
                        }

                    }
                }
                NumeroDaRaddrizzare = (NumeroTotaleraDdrizzabili / 100) * PercentualeRDZ;
                LOGCNTGXO.ScriviLogManuale("NumeroTotaleDaRDZ: " + Math.Round(NumeroDaRaddrizzare).ToString());
                return int.Parse(Math.Round(NumeroDaRaddrizzare).ToString());
            }
            catch(Exception ex)
            {
                LOGCNTGXO.ScriviLog(ref ex);
                return 0;
            }
        }
        private void RaddrizzaEsitiGXO()
        {
            var cust = CustomerConnections.GXO;
            var FilesDaRaddrizzare = Directory.GetFiles(cust.PathEsiti);

            foreach (var fdr in FilesDaRaddrizzare)
            {
                try
                {
                    var dir = Path.GetDirectoryName(fdr);
                    List<string> raddrizzati = new List<string>();
                    List<string> righeRaddrizzate = new List<string>();

                    string fileBK = fdr + ".bk";
                    File.Copy(fdr, fileBK);

                    var esiti = File.ReadAllLines(fdr);
                    int idRiga = 0;
                    bool raddizzato = false;
                    int tot = esiti.Count();

                    foreach (var esito in esiti)
                    {
                        idRiga++;

                        Debug.WriteLine($"{idRiga} - {tot}");

                        #region daMettereInProd
                        var dataEsito = esito.Substring(26, 12);
                        var dataEsitoDT = DateTime.MinValue;
                        DateTime.TryParseExact(dataEsito, "yyyyMMddHHmm", null, DateTimeStyles.None, out dataEsitoDT);

                        if (dataEsitoDT != DateTime.MinValue)
                        {
                            var sh = EspritecShipment.RestEspritecGetShipmentListByExternalRef(esito.Substring(0, 10), 100, 1, token_UNITEX);
                            var shDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(sh.Content).shipments.LastOrDefault();
                            if (shDes == null)
                            {

                            }
                            var geo = GeoSpec.GeoList.FirstOrDefault(x => x.citta.ToLower().Trim() == shDes.lastStopLocation.ToLower().Trim());
                            if (geo == null)
                            {
                                geo = GeoSpec.GeoList.FirstOrDefault(x => x.cap.ToLower().Trim() == shDes.lastStopZipCode.ToLower().Trim());
                            }
                            if (geo == null)
                            {
                                raddrizzati.Add(esito);
                                continue;
                                //throw new Exception($"impossibile determinare la geo: città: {shDes.lastStopLocation.ToLower().Trim()} cap: {shDes.lastStopZipCode.ToLower().Trim()}");
                            }

                            var rMax = TempiResa.TempiResaUtils.RecuperaOreResaOttimali(geo, shDes.ownerAgency, false);
                            int OreResa = LocalGoogleCalendar.GiorniDiResaEffettivi(shDes.docDate.Date, dataEsitoDT.Date) * 24;

                            Random rand = new Random();
                            bool RNGCheck = false;


                            // Claudio Cambio raddrizzamento GXO (rand.Next(1, 101) <= 80)
                            if (rand.Next(1, 101) <= 95)
                            {
                                RNGCheck = true;
                            }



                            var dataRaddrizzata = ValutaERaddrizzaIlDato(rMax, OreResa, dataEsitoDT);
                            var Ritardo = OreResa - rMax;

                            if ((dataEsitoDT <= dataRaddrizzata))
                            {
                                raddrizzati.Add(esito);
                            }
                            else
                            {
                                var trks = EspritecShipment.RestEspritecGetTracking(shDes.id, token_UNITEX);
                                var trksDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentTracking>(trks.Content);
                                var consegnato = trksDes.events.FirstOrDefault(x => x.statusID == 30);
                                var HasGiacenza = trksDes.events.Any(x => x.statusID == 50);
                                var IsPrenotata = trksDes.events.Any(x => x.statusID == 61);

                                if (HasGiacenza || IsPrenotata)
                                {
                                    continue;
                                }

                                var dataTSOrig = DateTime.MinValue;
                                DateTime.TryParseExact(consegnato.timeStamp, "dd/MM/yyyy HH:mm:ss", null, DateTimeStyles.None, out dataTSOrig);
                                string info = $"RDZ{dataRaddrizzata.ToString("yyMMdd")}";// dataRaddrizzata.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                                var rdm = new Random().Next(100, 999);
                                var Trupd = new EspritecShipment.RootobjectTrackingUpdate()
                                {
                                    tracking = new EspritecShipment.TrackingUpdate()
                                    {
                                        id = consegnato.id,
                                        info = info,
                                        signature = "",
                                        timeStamp = dataTSOrig.ToString($"yyyy-MM-ddTHH:mm:ss.{rdm}Z")
                                    }
                                };

                                if (string.IsNullOrEmpty(geo.provincia) || string.IsNullOrEmpty(geo.regione))
                                {
                                    try
                                    {
                                        geo.provincia = shDes.lastStopDistrict;
                                        geo.regione = GeoSpec.RecuperaRegioneDaProvincia(geo.provincia);
                                    }
                                    catch (NullReferenceException ee)
                                    {
                                        GestoreMail.SegnalaErroreDev("Errore nel determinare la geo nella funzione di recovery", ee);
                                    }
                                    finally
                                    {
                                        geo.provincia = "ND";
                                        geo.regione = "ND";
                                    }
                                }

                                //if (number < 20) //Se è un numero tra 80 e 100 non la toccare, 20% di chance che non venga toccata.
                                //{
                                //    //Brutto, brutto, brutto, brutto.
                                //    RNGCheck = false;
                                //    string NonRaddrizzataPerCaso = $"{shDes.id};{shDes.docNumber};{cust.NOME};{RNGCheck};{shDes.pickupDateTime.Value.ToShortDateString()};{dataTSOrig.ToShortDateString()};{dataRaddrizzata.ToShortDateString()};{Ritardo};{DateTime.Now};{geo.provincia};{geo.isCapoluogo};{geo.regione}";
                                //    List<string> daComunicare = new List<string>();
                                //    daComunicare.Add(NonRaddrizzataPerCaso);
                                //    File.AppendAllLines(FileRaddrizzatiDaComunicare, daComunicare);
                                //    daComunicare.Clear();
                                //    continue;
                                //}

                                var ok = EspritecShipment.RestEspritecUpdateTracking(Trupd, token_UNITEX);
                                var okDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectTrackingUpdateResponse>(ok.Content);

                                //Brutto
                                string Raddrizzata = $"{shDes.id};{shDes.docNumber};{cust.NOME};{RNGCheck};{shDes.pickupDateTime.Value.ToShortDateString()};{dataTSOrig.ToShortDateString()};{dataRaddrizzata.ToShortDateString()};{Ritardo};{DateTime.Now};{geo.provincia};{geo.isCapoluogo};{geo.regione}";
                                List<string> daComunicare2 = new List<string>();
                                daComunicare2.Add(Raddrizzata);
                                File.AppendAllLines(FileRaddrizzatiDaComunicare, daComunicare2);
                                daComunicare2.Clear();

                                if (okDes.status)
                                {
                                    raddizzato = true;
                                    string initEsito = esito.Substring(0, 26);
                                    string endEsito = dataRaddrizzata.ToString("yyyyMMddHHmm");
                                    string raddrizzato = initEsito + endEsito;
                                    raddrizzati.Add(raddrizzato);
                                    righeRaddrizzate.Add(idRiga.ToString());
                                }
                                else
                                {
                                    throw new Exception("Errore comunicazione con gespe");
                                }
                            }
                        }
                        else
                        {
                            string msg = $"Errore nel calcolo della data esito: {dataEsito} per la riga da correggere {idRiga} del file {Path.GetFileName(fdr)}";
                            _loggerCode.Error(msg);
                            throw new Exception(msg);
                        }
                        #endregion
                    }
                    File.WriteAllLines(fdr, raddrizzati);


                    var justName = Path.GetFileName(fdr);
                    var dest = Path.Combine(cust.PathEsitiDelCliente, justName);
                    var storeAT = Path.Combine(dir, "Inviati");
                    if (!Directory.Exists(storeAT))
                    {
                        Directory.CreateDirectory(storeAT);
                    }

                    string unicaRiga = "";
                    foreach (var r in righeRaddrizzate)
                    {
                        unicaRiga += r + " - ";
                    }

                    string fileFDRF = Path.Combine(storeAT, justName);  // Inviati/Filename

                    string fileBKM = Path.Combine(storeAT, justName + ".bk"); // Inviati/Filename.BK
                    var nl = new List<string>() { fileFDRF, fileBKM };


                    File.Move(fdr, fileFDRF); //Filename => inviati/filename

                    if (!File.Exists(fileBKM)) 
                        File.Move(fileBK, fileBKM); //Filename.BK => Inviati/Filename.BK


                    //if (raddizzato)
                    //{
                    //    GestoreMail.SendMail(nl, "r.ninno@xcmhealthcare.com", "RaddrizzatoGXO", $"In allegato il file da controllare manualmente, inviato in FTP (credenziali in customerspec)\r\n\r\nrighe modificate: {unicaRiga}");
                    //}

                    ConnectionInfo connectionInfo = new PasswordConnectionInfo(cust.sftpAddress, cust.sftpPort, cust.sftpUsername, cust.sftpPassword);
                    using (var sftp = new SftpClient(connectionInfo))
                    {
                        sftp.Connect();
                        using (var file = File.OpenRead(fileFDRF))
                        {
                            sftp.ChangeDirectory(cust.PathEsitiDelCliente);
                            sftp.UploadFile(file, justName);
                        }
                        sftp.Disconnect();
                    }
                }
                catch (Exception ee)
                {

                    if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                    {
                        DateLastException = DateTime.Now;
                        GestoreMail.SegnalaErroreDev("RaddrizzaEsitiGXO", ee);
                    }

                    LastException = ee;
                }
            }
        }      
        private void RaddrizzaEsitiGXO(int NumeroDaRDZ)
        {
            var cust = CustomerConnections.GXO;
            var FilesDaRaddrizzare = Directory.GetFiles(cust.PathEsiti);
            int LocalNumeroDaRDZ = NumeroDaRDZ;
            foreach (var fdr in FilesDaRaddrizzare)
            {
                try
                {
                    LOGCNTGXO.ScriviLogManuale("RaddrizzaEsitiGXO (" + NumeroDaRDZ.ToString() +")");
                    var dir = Path.GetDirectoryName(fdr);
                    List<string> raddrizzati = new List<string>();
                    List<string> righeRaddrizzate = new List<string>();

                    string fileBK = fdr + ".bk";
                    File.Copy(fdr, fileBK);

                    var esiti = File.ReadAllLines(fdr);
                    int idRiga = 0;
                    bool raddizzato = false;
                    int tot = esiti.Count();

                    foreach (var esito in esiti)
                    {
                        idRiga++;

                        Debug.WriteLine($"{idRiga} - {tot}");

                        #region daMettereInProd
                        var dataEsito = esito.Substring(26, 12);
                        var dataEsitoDT = DateTime.MinValue;
                        DateTime.TryParseExact(dataEsito, "yyyyMMddHHmm", null, DateTimeStyles.None, out dataEsitoDT);

                        if (dataEsitoDT != DateTime.MinValue)
                        {
                            var sh = EspritecShipment.RestEspritecGetShipmentListByExternalRef(esito.Substring(0, 10), 100, 1, token_UNITEX);
                            var shDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(sh.Content).shipments.LastOrDefault();
                            if (shDes == null)
                            {

                            }
                            var geo = GeoSpec.GeoList.FirstOrDefault(x => x.citta.ToLower().Trim() == shDes.lastStopLocation.ToLower().Trim());
                            if (geo == null)
                            {
                                geo = GeoSpec.GeoList.FirstOrDefault(x => x.cap.ToLower().Trim() == shDes.lastStopZipCode.ToLower().Trim());
                            }
                            if (geo == null)
                            {
                                raddrizzati.Add(esito);
                                continue;
                                //throw new Exception($"impossibile determinare la geo: città: {shDes.lastStopLocation.ToLower().Trim()} cap: {shDes.lastStopZipCode.ToLower().Trim()}");
                            }

                            var rMax = TempiResa.TempiResaUtils.RecuperaOreResaOttimali(geo, shDes.ownerAgency, false);
                            int OreResa = LocalGoogleCalendar.GiorniDiResaEffettivi(shDes.docDate.Date, dataEsitoDT.Date) * 24;

                            Random rand = new Random();
                            bool RNGCheck = false;


                            //// Claudio Cambio raddrizzamento GXO (rand.Next(1, 101) <= 80)
                            //if (rand.Next(1, 101) <= 95)
                            //{
                            //    RNGCheck = true;
                            //}



                            var dataRaddrizzata = ValutaERaddrizzaIlDato(rMax, OreResa, dataEsitoDT);
                            var Ritardo = OreResa - rMax;

                            if ((dataEsitoDT <= dataRaddrizzata))
                            {
                                raddrizzati.Add(esito);
                            }
                            else
                            {
                                if (LocalNumeroDaRDZ <=0 )
                                {
                                    LOGCNTGXO.ScriviLogManuale("NON RDZ (Raggiunta %): " + shDes.docNumber.ToString());
                                    continue;
                                }

                                var trks = EspritecShipment.RestEspritecGetTracking(shDes.id, token_UNITEX);
                                var trksDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentTracking>(trks.Content);
                                var consegnato = trksDes.events.FirstOrDefault(x => x.statusID == 30);
                                var HasGiacenza = trksDes.events.Any(x => x.statusID == 50);
                                var IsPrenotata = trksDes.events.Any(x => x.statusID == 61);

                                if (HasGiacenza || IsPrenotata)
                                {
                                    LOGCNTGXO.ScriviLogManuale("NON RDZ : " + shDes.docNumber.ToString() + "( Giacenza = " +HasGiacenza.ToString() + " Prenotata = " + IsPrenotata.ToString() + ")" );
                                    continue;
                                }

                                var dataTSOrig = DateTime.MinValue;
                                DateTime.TryParseExact(consegnato.timeStamp, "dd/MM/yyyy HH:mm:ss", null, DateTimeStyles.None, out dataTSOrig);
                                string info = $"RDZ{dataRaddrizzata.ToString("yyMMdd")}";// dataRaddrizzata.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                                var rdm = new Random().Next(100, 999);
                                var Trupd = new EspritecShipment.RootobjectTrackingUpdate()
                                {
                                    tracking = new EspritecShipment.TrackingUpdate()
                                    {
                                        id = consegnato.id,
                                        info = info,
                                        signature = "",
                                        timeStamp = dataTSOrig.ToString($"yyyy-MM-ddTHH:mm:ss.{rdm}Z")
                                    }
                                };

                                if (string.IsNullOrEmpty(geo.provincia) || string.IsNullOrEmpty(geo.regione))
                                {
                                    try
                                    {
                                        geo.provincia = shDes.lastStopDistrict;
                                        geo.regione = GeoSpec.RecuperaRegioneDaProvincia(geo.provincia);
                                    }
                                    catch (NullReferenceException ee)
                                    {
                                        GestoreMail.SegnalaErroreDev("Errore nel determinare la geo nella funzione di recovery", ee);
                                    }
                                    finally
                                    {
                                        geo.provincia = "ND";
                                        geo.regione = "ND";
                                    }
                                }

                                //if (number < 20) //Se è un numero tra 80 e 100 non la toccare, 20% di chance che non venga toccata.
                                //{
                                //    //Brutto, brutto, brutto, brutto.
                                //    RNGCheck = false;
                                //    string NonRaddrizzataPerCaso = $"{shDes.id};{shDes.docNumber};{cust.NOME};{RNGCheck};{shDes.pickupDateTime.Value.ToShortDateString()};{dataTSOrig.ToShortDateString()};{dataRaddrizzata.ToShortDateString()};{Ritardo};{DateTime.Now};{geo.provincia};{geo.isCapoluogo};{geo.regione}";
                                //    List<string> daComunicare = new List<string>();
                                //    daComunicare.Add(NonRaddrizzataPerCaso);
                                //    File.AppendAllLines(FileRaddrizzatiDaComunicare, daComunicare);
                                //    daComunicare.Clear();
                                //    continue;
                                //}

                                var ok = EspritecShipment.RestEspritecUpdateTracking(Trupd, token_UNITEX);
                                var okDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectTrackingUpdateResponse>(ok.Content);

                                //Brutto
                                string Raddrizzata = $"{shDes.id};{shDes.docNumber};{cust.NOME};{RNGCheck};{shDes.pickupDateTime.Value.ToShortDateString()};{dataTSOrig.ToShortDateString()};{dataRaddrizzata.ToShortDateString()};{Ritardo};{DateTime.Now};{geo.provincia};{geo.isCapoluogo};{geo.regione}";
                                List<string> daComunicare2 = new List<string>();
                                daComunicare2.Add(Raddrizzata);
                                File.AppendAllLines(FileRaddrizzatiDaComunicare, daComunicare2);
                                daComunicare2.Clear();

                                if (okDes.status)
                                {
                                    raddizzato = true;
                                    string initEsito = esito.Substring(0, 26);
                                    string endEsito = dataRaddrizzata.ToString("yyyyMMddHHmm");
                                    string raddrizzato = initEsito + endEsito;
                                    raddrizzati.Add(raddrizzato);
                                    righeRaddrizzate.Add(idRiga.ToString());
                                    LocalNumeroDaRDZ--;
                                   
                                    LOGCNTGXO.ScriviLogManuale("RDZ (" + (NumeroDaRDZ - LocalNumeroDaRDZ).ToString() + "/" + NumeroDaRDZ + "): " + shDes.docNumber.ToString() + " [" + dataEsitoDT.ToString("dd/MM/YYYY") + "=>" +dataRaddrizzata.ToString("dd/MM/YYYY") + "]");
                                }
                                else
                                {
                                    throw new Exception("Errore comunicazione con gespe");
                                }
                            }
                        }
                        else
                        {
                            string msg = $"Errore nel calcolo della data esito: {dataEsito} per la riga da correggere {idRiga} del file {Path.GetFileName(fdr)}";
                            _loggerCode.Error(msg);
                            throw new Exception(msg);
                        }
                        #endregion
                    }
                    File.WriteAllLines(fdr, raddrizzati);


                    var justName = Path.GetFileName(fdr);
                    var dest = Path.Combine(cust.PathEsitiDelCliente, justName);
                    var storeAT = Path.Combine(dir, "Inviati");
                    if (!Directory.Exists(storeAT))
                    {
                        Directory.CreateDirectory(storeAT);
                    }

                    string unicaRiga = "";
                    foreach (var r in righeRaddrizzate)
                    {
                        unicaRiga += r + " - ";
                    }

                    string fileFDRF = Path.Combine(storeAT, justName);  // Inviati/Filename

                    string fileBKM = Path.Combine(storeAT, justName + ".bk"); // Inviati/Filename.BK
                    var nl = new List<string>() { fileFDRF, fileBKM };


                    File.Move(fdr, fileFDRF); //Filename => inviati/filename

                    if (!File.Exists(fileBKM))
                        File.Move(fileBK, fileBKM); //Filename.BK => Inviati/Filename.BK


                    //if (raddizzato)
                    //{
                    //    GestoreMail.SendMail(nl, "r.ninno@xcmhealthcare.com", "RaddrizzatoGXO", $"In allegato il file da controllare manualmente, inviato in FTP (credenziali in customerspec)\r\n\r\nrighe modificate: {unicaRiga}");
                    //}

                    ConnectionInfo connectionInfo = new PasswordConnectionInfo(cust.sftpAddress, cust.sftpPort, cust.sftpUsername, cust.sftpPassword);
                    using (var sftp = new SftpClient(connectionInfo))
                    {
                        sftp.Connect();
                        using (var file = File.OpenRead(fileFDRF))
                        {
                            sftp.ChangeDirectory(cust.PathEsitiDelCliente);
                            sftp.UploadFile(file, justName);
                        }
                        sftp.Disconnect();
                    }
                }
                catch (Exception ee)
                {

                    if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                    {
                        DateLastException = DateTime.Now;
                        GestoreMail.SegnalaErroreDev("RaddrizzaEsitiGXO", ee);
                    }

                    LastException = ee;
                }
            }
        }

        private void ProduciEsiti3C()
        {
            var esitiRaggruppati = EsitiDaComunicare_3C.GroupBy(x => x.NumeroBolla).ToList();
            var esitiPiuRecenti = new List<_3C_EsitiOUT>();
            var rettificate = new List<_3C_EsitiOUT>();

            if (esitiRaggruppati.Count > 0)
            {
                foreach (var rag in esitiRaggruppati)
                {
                    foreach (var r in rag)
                    {
                        var esiste = rettificate.FirstOrDefault(x => x.NumeroBolla == r.NumeroBolla);
                        if (esiste != null)
                        {
                            if (esiste.statoUNITEX == 30)
                            {
                                break;
                            }
                            else if (r.statoUNITEX == 30)
                            {
                                rettificate.Remove(esiste);
                                rettificate.Add(r);

                                break;
                            }
                            else if (r.statoUNITEX > esiste.statoUNITEX)
                            {
                                rettificate.Remove(esiste);
                                rettificate.Add(r);
                            }
                        }
                        else
                        {
                            rettificate.Add(r);
                        }
                    }
                }

                var outPath = $@"C:\FTP\CLIENTI\3CSRLS\ESITI\ESITI_{DateTime.Now.ToString("yyyMMddHHmmss")}.txt";
                var justName = Path.GetFileName(outPath);
                if (!Directory.Exists($@"C:\FTP\CLIENTI\3CSRLS\ESITI\"))
                {
                    Directory.CreateDirectory($@"C:\FTP\CLIENTI\3CSRLS\ESITI");
                }
                if (File.Exists(outPath))
                {
                    File.Delete(outPath);
                }
                var cust = CustomerConnections._3CS;

                File.WriteAllLines(outPath, ProduciFileTracking3C(rettificate));
                _ftp = CreaClientFTPperIlCliente(cust);
                var dest = Path.Combine("/OUT/Esiti", justName);
                _ftp.UploadFile(outPath, dest, FtpRemoteExists.Overwrite);
                _ftp.Disconnect();
                if (!Directory.Exists(@"C:\FTP\CLIENTI\3CSRLS\ESITI\inviati"))
                {
                    Directory.CreateDirectory(@"C:\FTP\CLIENTI\3CSRLS\ESITI\inviati");
                }
                File.Move(outPath, Path.Combine(@"C:\FTP\CLIENTI\3CSRLS\ESITI\inviati", justName));
            }
        }
        private List<string> ProduciFileTracking3C(List<_3C_EsitiOUT> esitiPiuRecenti)
        {
            //bozza
            var resp = new List<string>();

            foreach (var ele in esitiPiuRecenti)
            {
                var NumeroBolla = ele.NumeroBolla.PadRight(15, ' ');
                var Filler = "".PadRight(9, ' ');
                var CodiceMittente = "".PadRight(6, ' ');
                var RagioneSocialeMittente = ele.RagioneSocialeMittente.Length > 40 ? ele.RagioneSocialeMittente.Substring(0, 40) : ele.RagioneSocialeMittente.PadRight(40, ' ');
                var RagioneSocialeDestinatario = ele.RagioneSocialeDestinatario.PadRight(40, ' ');
                var LocalitaDestinatario = ele.LocalitaDestinatario.PadRight(30, ' ');
                var ProvDestinatario = ele.ProvDestinatario.Length > 2 ? ele.ProvDestinatario.Substring(0, 2) : ele.ProvDestinatario.PadRight(2, ' ');
                var NumeroColli = ele.NumeroColli.ToString().PadLeft(5, '0');//verificare maschera
                var Peso1D = ele.Peso1D.ToString().Replace(",", "").Substring(0, ele.Peso1D.Length - 5).PadLeft(7, '0');//verificare maschera
                var DataEvento = ele.DataEvento;
                var TipoEvento = ele.TipoEvento;
                var DataPrenotazione = ele.DataPrenotazione.PadRight(1, ' ');
                var DescrizioneEvento = ele.DescrizioneEvento.PadRight(60, ' ');
                var Progressivo = "0".PadLeft(3, ' ');
                var OrarioEvento = "    ";
                var BarcodeSegnacolloLetto = "".PadRight(30, ' ');
                var Filler2 = "".PadRight(38, ' ');

                string line = $"{NumeroBolla}{Filler}{CodiceMittente}{RagioneSocialeMittente}{RagioneSocialeDestinatario}{LocalitaDestinatario}{ProvDestinatario}{NumeroColli}{Peso1D}{DataEvento}{TipoEvento}" +
                    $"{DataPrenotazione}{DescrizioneEvento}{Progressivo}{OrarioEvento}{BarcodeSegnacolloLetto}{Filler2}{ele.FineRecord}";
                resp.Add(line);
            }

            return resp;
        }
        private void ProduciEsitiChiapparoli()
        {
            //bozza
            var esitiRaggruppati = EsitiDaComunicareChiapparoli.GroupBy(x => x.NumeroProgressivo).ToList();
            //var esitiPiuRecenti = new List<Chiapparoli_EsitiOUT>();
            var rettificate = new List<Chiapparoli_EsitiOUT>();

            if (esitiRaggruppati.Count > 0)
            {
                foreach (var rag in esitiRaggruppati)
                {
                    foreach (var r in rag)
                    {
                        var esiste = rettificate.FirstOrDefault(x => x.NumeroProgressivo == r.NumeroProgressivo);
                        if (esiste != null)
                        {
                            if (esiste.statoUNITEX == 30)
                            {
                                break;
                            }
                            else if (r.statoUNITEX == 30)
                            {
                                rettificate.Remove(esiste);
                                rettificate.Add(r);

                                break;
                            }
                            else if (r.statoUNITEX > esiste.statoUNITEX)
                            {
                                rettificate.Remove(esiste);
                                rettificate.Add(r);
                            }
                        }
                        else
                        {
                            rettificate.Add(r);
                        }
                    }
                }

                var outPath = $@"C:\FTP\CLIENTI\CHIAPPAROLI\OUT\ESITI\Unitex_{DateTime.Now.ToString("yyyMMddHHmmss")}.txt";
                var justName = Path.GetFileName(outPath);
                if (!Directory.Exists($@"C:\FTP\CLIENTI\CHIAPPAROLI\OUT\ESITI\"))
                {
                    Directory.CreateDirectory($@"C:\FTP\CLIENTI\CHIAPPAROLI\OUT\ESITI");
                }
                if (File.Exists(outPath))
                {
                    File.Delete(outPath);
                }
                var cust = CustomerConnections.CHIAPPAROLI;

                File.WriteAllLines(outPath, ProduciFileTrackingChiapparoli(rettificate));

                ConnectionInfo connectionInfo = new PasswordConnectionInfo(cust.sftpAddress, cust.sftpPort, cust.sftpUsername, cust.sftpPassword);

                using (var sftp = new SftpClient(connectionInfo))
                {
                    sftp.Connect();
                    using (var file = File.OpenRead(outPath))
                    {

                        sftp.ChangeDirectory(cust.PathEsitiDelCliente);

                        sftp.UploadFile(file, justName);
                    }
                    sftp.Disconnect();
                }
                if (!Directory.Exists(@"C:\FTP\CLIENTI\CHIAPPAROLI\OUT\ESITI\inviati"))
                {
                    Directory.CreateDirectory(@"C:\FTP\CLIENTI\CHIAPPAROLI\OUT\ESITI\inviati");
                }
                File.Move(outPath, Path.Combine(@"C:\FTP\CLIENTI\CHIAPPAROLI\OUT\ESITI\inviati", justName));
            }

            EsitiDaComunicareChiapparoli.Clear();
        }
        private List<string> ProduciFileTrackingChiapparoli(List<Chiapparoli_EsitiOUT> esitiPiuRecenti)
        {
            var resp = new List<string>();

            foreach (var ele in esitiPiuRecenti)
            {
                var SedeChiapparoli = ele.SedeChiapparoli.Length > 2 ? ele.SedeChiapparoli.Substring(0, 2) : ele.SedeChiapparoli.PadRight(2, ' ');
                var CodiceDitta = ele.CodiceDitta.Length > 2 ? ele.CodiceDitta.Substring(0, 2) : ele.CodiceDitta.PadRight(2, ' ');
                var NumeroProgressivo = ele.NumeroProgressivo.Length > 9 ? ele.NumeroProgressivo.Substring(0, 9) : ele.NumeroProgressivo.PadRight(9, ' ');
                var PosizioneRiga = ele.PosizioneRiga.Length > 3 ? ele.PosizioneRiga.Substring(0, 3) : ele.PosizioneRiga.PadRight(3, '0');
                var CodiceResa = ele.CodiceResa.Length > 4 ? ele.CodiceResa.Substring(0, 4) : ele.CodiceResa.PadRight(4, ' ');
                var DataResa = ele.DataResaAAMMGG.Length > 6 ? ele.DataResaAAMMGG.Substring(0, 6) : ele.DataResaAAMMGG.PadRight(6, ' ');
                var OraResa = ele.OraResa.Length > 6 ? ele.OraResa.Substring(0, 6) : ele.OraResa.PadRight(6, '0');
                var RigaNote = ele.RigaNote.Length > 40 ? ele.RigaNote.Substring(0, 40) : ele.RigaNote.PadRight(40, ' ');
                var RiferimentoVettore = ele.RiferimentoVettore.Length > 15 ? ele.RiferimentoVettore.Substring(0, 15) : ele.RiferimentoVettore.PadRight(15, ' ');
                var DataRiferimentoVettore = ele.DataRiferimentoVettoreAAMMGG.Length > 6 ? ele.DataRiferimentoVettoreAAMMGG.Substring(0, 6) : ele.DataRiferimentoVettoreAAMMGG.PadRight(6, ' ');
                var RiferimentoSubVettore = ele.RiferimentoSubVettore.PadRight(15, ' ');
                var DataRiferimentoSubVettore = ele.DataRiferimentoSubVettoreAAMMGG.PadRight(6, ' ');
                var Colli = ele.Colli.PadLeft(6, '0');
                var Peso = ele.Peso2d.PadRight(8, '0');
                var Volume = ele.Volume3d.PadRight(8, '0');
                var PesoTassato = ele.Peso2d.PadRight(6, '0');//peso tassato non cambia mai in quanto viene modificato in gespe in fase di pesaggio
                var ImportoTotaleSpedizione = ele.ImportoTotaleSpedizione2d.PadRight(9, '0');
                var Filler = ele.Filler.PadRight(9, ' ');
                string line = $"{SedeChiapparoli}{CodiceDitta}{NumeroProgressivo}{PosizioneRiga}{CodiceResa}{DataResa}{OraResa}{RigaNote}{RiferimentoVettore}{DataRiferimentoVettore}{RiferimentoSubVettore}" +
                    $"{DataRiferimentoSubVettore}{Colli}{Peso}{Volume}{PesoTassato}{ImportoTotaleSpedizione}{Filler}";
                resp.Add(line);
            }

            return resp;
        }
        private void ProduciEsitiDamora()
        {
            var esitiRaggruppati = EsitiDaComunicareDamora.GroupBy(x => x.rifExt).ToList();
            var esitiPiuRecenti = new List<DAMORA_EsitiOUT>();
            if (esitiRaggruppati.Count > 0)
            {
                foreach (var ultimoEsito in esitiRaggruppati)
                {
                    var cons = ultimoEsito.FirstOrDefault(x => x.DescrizioneEsito == "CONSEGNATA");
                    if (cons != null)
                    {
                        esitiPiuRecenti.Add(cons);
                    }
                    else
                    {
                        esitiPiuRecenti.Add(ultimoEsito.Last());
                    }
                }

                var outPath = $@"C:\FTP\CLIENTI\DAMORA\OUT\ESITI\DAMORA_{DateTime.Now.ToString("yyyMMddHHmmss")}.txt";
                var justName = Path.GetFileName(outPath);
                if (!Directory.Exists($@"C:\FTP\CLIENTI\DAMORA\OUT\ESITI\"))
                {
                    Directory.CreateDirectory($@"C:\FTP\CLIENTI\DAMORA\OUT\ESITI\");
                }
                if (File.Exists(outPath))
                {
                    File.Delete(outPath);
                }
                var cust = CustomerConnections.DAMORA;
                var esitiConsegnati = esitiPiuRecenti.Where(x => x.DescrizioneEsito == "CONSEGNATA").ToList();
                File.WriteAllLines(outPath, ProduciFileTrackingDAmora(esitiConsegnati));
                _ftp = CreaClientFTPperIlCliente(cust);
                var dest = Path.Combine("/OUT/Esiti", justName);
                _ftp.UploadFile(outPath, dest, FtpRemoteExists.Overwrite);
                _ftp.Disconnect();
                if (!Directory.Exists(@"C:\FTP\CLIENTI\DAMORA\OUT\ESITI\inviati")) Directory.CreateDirectory(@"C:\FTP\CLIENTI\DAMORA\OUT\ESITI\inviati");
                File.Move(outPath, Path.Combine(@"C:\FTP\CLIENTI\DAMORA\OUT\ESITI\inviati", justName));
            }
        }
        private List<string> ProduciFileTrackingDAmora(List<DAMORA_EsitiOUT> esitiRaggruppati)
        {
            List<string> fileContent = new List<string>();

            foreach (var ele in esitiRaggruppati)
            {
                if (string.IsNullOrEmpty(ele.rifExt)) continue;
                var rifExt = ele.rifExt.Length > 20 ? ele.rifExt.Substring(0, 20) : ele.rifExt.PadRight(20, ' ');
                var dataEsito = ele.dataEsito.Length > 6 ? ele.dataEsito.Substring(0, 6) : ele.dataEsito.PadRight(6, ' ');

                string line = $"{rifExt}{dataEsito}";
                fileContent.Add(line);
            }

            return fileContent;
        }
        private void ProduciEsitiLoreal()
        {
            try
            {
                var cust = CustomerConnections.Logistica93;
                var esitiRaggruppati = EsitiDaComunicareLoreal.GroupBy(x => x.E_NumeroDDT).ToList();//.Select(x => x.FirstOrDefault()).ToList();
                var rettificate = new List<LorealEsiti>();

                foreach (var rag in esitiRaggruppati)
                {
                    if (rag.Count() > 1)
                    {

                    }
                    foreach (var r in rag)
                    {

                        var esiste = rettificate.FirstOrDefault(x => x.E_NumeroDDT == r.E_NumeroDDT);
                        if (esiste != null)
                        {
                            if (esiste.statoUNITEX == 30)
                            {
                                break;
                            }
                            else if (r.statoUNITEX == 30)
                            {
                                rettificate.Remove(esiste);
                                rettificate.Add(r);

                                break;
                            }
                            else if (r.statoUNITEX > esiste.statoUNITEX)
                            {
                                rettificate.Remove(esiste);
                                rettificate.Add(r);
                            }
                        }
                        else
                        {
                            rettificate.Add(r);
                        }
                    }
                }


                if (rettificate.Count > 0)
                {

                    var outPath = $@"C:\FTP\CLIENTI\Logistica93\OUT\ESITI\LOREAL_{DateTime.Now.ToString("yyyMMddHHmmss")}.txt";
                    var justName = Path.GetFileName(outPath);
                    var pathInviati = Path.Combine(Path.GetDirectoryName(outPath), "inviati");
                    if (!Directory.Exists(pathInviati))
                    {
                        Directory.CreateDirectory(pathInviati);
                    }
                    if (!Directory.Exists($@"C:\FTP\CLIENTI\Logistica93\OUT\ESITI\"))
                    {
                        Directory.CreateDirectory($@"C:\FTP\CLIENTI\Logistica93\OUT\ESITI\");
                    }

                    File.WriteAllLines(outPath, ProduciFileTrackingLoreal(rettificate));

                    var sftp = CreaClientSFTPperIlCliente(cust);
                    //sftp.Connect();
                    sftp.ChangeDirectory(cust.PathEsitiDelCliente);

                    var tt = sftp.WorkingDirectory;
                    var ls = sftp.ListDirectory(tt);
                    using (var fileStream = new FileStream(outPath, FileMode.Open))
                    {
                        //sftp.BufferSize = 4 * 1024; // bypass Payload error large files

                        sftp.UploadFile(fileStream, Path.GetFileName(outPath));
                        fileStream.Close();
                    }

                    var fdest = Path.Combine(pathInviati, justName);
                    File.Move(outPath, fdest);
                    sftp.Disconnect();

                }
            }
            finally
            {
                EsitiDaComunicareLoreal.Clear();
            }

        }
        private List<string> ProduciFileTrackingLoreal(List<LorealEsiti> esitiRaggruppati)
        {
            List<string> fileContent = new List<string>();

            foreach (var ele in esitiRaggruppati)
            {
                while (ele.E_NumeroDDT.Length < 10)
                {
                    ele.E_NumeroDDT = "0" + ele.E_NumeroDDT;
                }
                var NumDDT = ele.E_NumeroDDT.Length > 10 ? ele.E_NumeroDDT.Substring(0, 10) : ele.E_NumeroDDT.PadRight(10, ' ');
                var RifConsSap = ele.E_RiferimentoNumeroConsegnaSAP.Length > 10 ? ele.E_RiferimentoNumeroConsegnaSAP.Substring(0, 10) : ele.E_RiferimentoNumeroConsegnaSAP.PadRight(10, ' ');
                var DataConsegna = ele.E_DataConsegnaADestino.Length > 8 ? ele.E_DataConsegnaADestino.Substring(0, 8) : ele.E_DataConsegnaADestino.PadRight(8, ' ');
                var Causale = ele.E_Causale.Length > 2 ? ele.E_Causale.Substring(0, 2) : ele.E_Causale.PadRight(2, ' ');
                var sottoCausale = ele.E_SottoCausale.Length > 3 ? ele.E_SottoCausale.Substring(0, 3) : ele.E_SottoCausale.PadRight(3, ' ');
                var shipperref = ele.E_RiferimentoCorriere.Length > 20 ? ele.E_RiferimentoCorriere.Substring(0, 20) : ele.E_RiferimentoCorriere.PadRight(20, ' ');
                //var filler = "".PadRight(18, ' ');
                //var Note = "".PadRight(200, ' ');
                //var filler2 = "".PadRight(5, ' ');

                string line = $"{NumDDT}{RifConsSap}{DataConsegna}{Causale}{sottoCausale}{shipperref}";// {filler}{Note}{filler2}";
                fileContent.Add(line);
            }

            return fileContent;
        }
        private void ProduciEsitiSTM()
        {
            var esitiRaggruppatiPerRegione = EsitiDaCoumicareSTM.GroupBy(x => x.regione).ToList();
            var cust = CustomerConnections.STMGroup;
            if (esitiRaggruppatiPerRegione.Count > 0)
            {
                foreach (var esitiRegione in esitiRaggruppatiPerRegione)
                {
                    try
                    {
                        List<STM_EsitiOut> esitiRegionali = new List<STM_EsitiOut>();
                        var reg = esitiRegione.FirstOrDefault().regione.ToLower();
                        if (reg == "Trentino-Alto Adige/Südtirol".ToLower())
                        {
                            reg = "triveneto";
                        }
                        else if (reg == "friuli-venezia giulia")
                        {
                            reg = "friuli";
                        }
                        var outPath = $@"{CustomerConnections.STMGroup.PathEsiti}\esitistm_{reg}_{DateTime.Now.ToString("ddMMyyyyHHmm")}.txt";
                        var pathInviati = Path.Combine(Path.GetDirectoryName(outPath), "inviati");
                        foreach (var esito in esitiRegione)
                        {
                            if (esito.Descrizione_Tracking == "CONSEGNATA")
                            {
                                esitiRegionali.Add(esito);
                            }
                        }
                        var justFileName = Path.GetFileName(outPath);
                        var remoteFTPPath = Path.Combine(cust.PathEsitiDelCliente, justFileName);
                        var regE = ProduciFileTrackingSTM(esitiRegionali);
                        //File.WriteAllLines(outPath, regE);
                        if (regE.Count() > 0)
                        {
                            File.WriteAllLines(outPath, regE);
                            var sftp = CreaClientSFTPperIlCliente(cust);
                            sftp.ChangeDirectory(cust.PathEsitiDelCliente);
                            using (var fileStream = new FileStream(outPath, FileMode.Open))
                            {
                                sftp.BufferSize = 4 * 1024; // bypass Payload error large files
                                sftp.UploadFile(fileStream, Path.GetFileName(outPath));
                                fileStream.Close();
                                sftp.Disconnect();
                            }
                            //_loggerCode.Debug($"{Path.GetFileName(outPath)} inviato all'FTP STM");
                            if (!Directory.Exists(pathInviati))
                            {
                                Directory.CreateDirectory(pathInviati);
                            }
                            var fdest = Path.Combine(pathInviati, justFileName);
                            File.Move(outPath, fdest);
                            sftp.Disconnect();
                        }
                    }
                    catch (Exception ee)
                    {

                    }
                    finally
                    {

                    }

                }
                //var respT = new List<string>();

                //foreach (var esito in EsitiDaCoumicareSTM)
                //{
                //    respT.Add($"{esito.ProgressivoSpedizione}|{esito.Descrizione_Tracking}");
                //}
                //File.WriteAllLines("EsitiSTMT.txt", respT);

                EsitiDaCoumicareSTM.Clear();
            }
        }
        private List<string> ProduciFileTrackingSTM(List<STM_EsitiOut> esitiRegionali)
        {
            List<string> fileContent = new List<string>();

            foreach (var element in esitiRegionali)
            {

                var NumDDT = element.NumDDT.Length > 20 ? element.NumDDT.Substring(0, 20) : element.NumDDT.PadLeft(20, ' ');// String.Format($"{{{"0,"}-{20}}}".PadLeft(20), );
                var DataConsegnaEffettiva = element.DataConsegnaEffettiva.Length > 8 ? element.DataConsegnaTassativa.Substring(0, 8) : element.DataConsegnaEffettiva.PadLeft(8, ' ');//String.Format($"{{{"0,"}-{8}}}", element.DataConsegnaEffettiva);
                var DataSpedizione = element.DataSpedizione.Length > 8 ? element.DataSpedizione.Substring(0, 8) : element.DataSpedizione.PadLeft(8); //String.Format($"{{{"0,"}-{8}}}", element.DataSpedizione);
                var CittaDestinatario = element.CittaDestinatario.Length > 20 ? element.CittaDestinatario.Substring(0, 20) : element.CittaDestinatario.PadRight(20, ' '); //String.Format($"{{{"0,"}-{20}}}", element.CittaDestinatario);
                var DataConsegnaTassativa = element.DataConsegnaTassativa.Length > 8 ? element.DataConsegnaTassativa.Substring(0, 8) : element.DataConsegnaTassativa.PadLeft(8, ' '); //String.Format($"{{{"0,"}-{8}}}", element.DataConsegnaTassativa);
                var ID_Tracking = element.ID_Tracking.Length > 2 ? element.ID_Tracking.Substring(0, 2) : element.ID_Tracking.PadRight(2, ' '); //String.Format($"{{{"0,"}-{2}}}", element.ID_Tracking);
                var Descrizione_Tracking = element.Descrizione_Tracking.Length > 20 ? element.Descrizione_Tracking.Substring(0, 20) : element.Descrizione_Tracking.PadRight(20, ' '); //String.Format($"{{{"0,"}-{31}}}", element.Descrizione_Tracking);
                var DataTracking = element.DataTracking.Length > 8 ? element.DataTracking.Substring(0, 8) : element.DataTracking.PadRight(8, ' '); //String.Format($"{{{"0,"}-{8}}}", element.DataTracking);
                var ProgressivoSpedizione = element.ProgressivoSpedizione.Length > 8 ? element.ProgressivoSpedizione.Substring(0, 8) : element.ProgressivoSpedizione.PadRight(8, '0');// String.Format($"{{{"0,"}-{8}}}", element.ProgressivoSpedizione);

                //string ss2 = "".PadRight(1000, ' ');
                //ss2 = ss2.Insert(10, element.NumDDT);
                //ss2 = ss2.Insert(20, element.DataConsegnaEffettiva);
                //ss2 = ss2.Insert(29, element.DataSpedizione);
                //ss2 = ss2.Insert(37, element.CittaDestinatario);
                //ss2 = ss2.Insert(58, element.DataConsegnaTassativa);
                //ss2 = ss2.Insert(67, element.ID_Tracking);
                //ss2 = ss2.Insert(69, element.Descrizione_Tracking);
                //ss2 = ss2.Insert(89, element.DataTracking);
                //ss2 = ss2.Insert(97, element.ProgressivoSpedizione);
                //ss2 = ss2.Substring(0, 105);
                string line = $"{NumDDT}{DataConsegnaEffettiva} {DataSpedizione}{CittaDestinatario}{DataConsegnaTassativa}  {ID_Tracking}{Descrizione_Tracking}{DataTracking}{ProgressivoSpedizione}";

                fileContent.Add(line);
            }

            return fileContent;
        }
        private void ProduciEsitiCDGroup()
        {
            try
            {
                var raggruppatePerRifCliente = EsitiDaCoumicareCDGroup.GroupBy(x => x.NUMERO_BOLLA);
                var rettificate = new List<CDGROUP_EsitiOUT>();

                foreach (var rag in raggruppatePerRifCliente)
                {
                    foreach (var r in rag)
                    {
                        var esiste = rettificate.FirstOrDefault(x => x.NUMERO_BOLLA == r.NUMERO_BOLLA);
                        if (esiste != null)
                        {
                            if (esiste.statoUNITEX == 30)
                            {
                                break;
                            }
                            else if (r.statoUNITEX == 30)
                            {
                                rettificate.Remove(esiste);
                                rettificate.Add(r);

                                break;
                            }
                            else if (r.statoUNITEX > esiste.statoUNITEX)
                            {
                                rettificate.Remove(esiste);
                                rettificate.Add(r);
                            }
                        }
                        else
                        {
                            rettificate.Add(r);
                        }
                    }
                }

                if (rettificate.Count > 0)
                {
                    var outPath = $@"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT\TRACK_{DateTime.Now.ToString("yyyMMddHHmmss")}.txt";

                    if (!Directory.Exists($@"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT"))
                    {
                        Directory.CreateDirectory($@"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT");
                    }
                    if (File.Exists(outPath))
                    {
                        File.Delete(outPath);
                    }

                    File.WriteAllLines(outPath, ProduciFileTrackingCDGroup(rettificate));
                    //_loggerCode.Info($"NUOVO FILE ESITI CDGroup: {outPath}");
                }
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);
            }
            finally
            {
                EsitiDaCoumicareCDGroup.Clear();
            }

        }
        public static List<string> ProduciFileTrackingCDGroup(List<CDGROUP_EsitiOUT> list)
        {
            List<string> fileContent = new List<string>();

            foreach (var element in list)
            {
                string numeroBolla = "";
                if (element.NUMERO_BOLLA.Length < 10)
                {
                    numeroBolla = element.NUMERO_BOLLA.PadLeft(10, '0');
                }
                else
                {
                    numeroBolla = element.NUMERO_BOLLA;
                }

                var loc = "";
                if (element.LOCALITA == null)
                {
                    loc = "";
                }
                else if (element.LOCALITA.Length > 15)
                {
                    loc = element.LOCALITA.Substring(0, 15);
                }
                else
                {
                    loc = element.LOCALITA;
                }

                var mandante = String.Format($"{{{"0,"}-{element.idxMANDANTE[1]}}}", element.MANDANTE);
                numeroBolla = String.Format($"{{{"0,"}-{element.idxNUMERO_BOLLA[1]}}}", numeroBolla);
                var dataBolla = String.Format($"{{{"0,"}-{element.idxDATA_BOLLA[1]}}}", element.DATA_BOLLA);
                var ragioneSociale = String.Format($"{{{"0,"}-{element.idxRAGIONE_SOCIALE_VETTORE[1]}}}", element.RAGIONE_SOCIALE_VETTORE);
                var dataPresaCons = String.Format($"{{{"0,"}-{element.idxDATA_PRESA_CONS[1]}}}", element.DATA_PRESA_CONS);
                var statoConsegna = String.Format($"{{{"0,"}-{element.idxSTATO_CONSEGNA[1]}}}", element.STATO_CONSEGNA);
                var descrizioneConsegna = String.Format($"{{{"0,"}-{element.idxDESCRIZIONE_STATO_CONSEGNA[1]}}}", element.DESCRIZIONE_STATO_CONSEGNA);
                var data = String.Format($"{{{"0,"}-{element.idxDATA[1]}}}", element.DATA);
                var localita = String.Format($"{{{"0,"}-{element.idxLOCALITA[1]}}}", loc);
                var rifVettore = String.Format($"{{{"0,"}-{element.idxRIFVETTORE[1]}}}", element.RIFVETTORE);

                //string line = $"{mandante}{numeroBolla}{dataBolla}{ragioneSociale}{dataPresaCons}{statoConsegna}{descrizioneConsegna}{data}{localita}{rifVettore}";
                string line2 = $"{mandante}{numeroBolla}{dataBolla}{ragioneSociale}{dataPresaCons}{statoConsegna}{descrizioneConsegna}{data}{localita}{rifVettore}";
                fileContent.Add(line2);
            }

            return fileContent;
        }
        List<long> CambiTackingGiaNotificati = new List<long>();
        private List<EventTracking> CambiTMSDelCliente(CustomerSpec customer, string fromTimestamp)
        {
            try
            {
                //List<string> trakingIDalreadeCheched = File.ReadAllLines("checkedIDTraking").ToList();

                var tuttiItracking = new List<EventTracking>();
                var pageNumber = 1;
                var pageRows = 500;
                RestClient client = null;

                client = new RestClient(endpointAPI_UNITEX + $"/api/tms/shipment/tracking/changes/{pageRows}/{pageNumber}?FromTimeStamp={fromTimestamp}");
                LastCheckChangesTMS = DateTime.Now;

                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", $"Bearer {customer.tokenAPI}");
                IRestResponse response = client.Execute(request);

                var TrackingUnitex = JsonConvert.DeserializeObject<RootobjectShipmentTracking>(response.Content);

                if (TrackingUnitex.events != null)
                {
                    tuttiItracking.AddRange(TrackingUnitex.events.ToList());

                    var maxPages = TrackingUnitex.result.maxPages;

                    while (maxPages > 1)
                    {
                        pageNumber++;
                        maxPages--;
                        Debug.WriteLine(maxPages);
                        client = new RestClient(endpointAPI_UNITEX + $"/api/tms/shipment/tracking/changes/{pageRows}/{pageNumber}?FromTimeStamp={fromTimestamp}");
                        request = new RestRequest(Method.GET);
                        request.AddHeader("Authorization", $"Bearer {customer.tokenAPI}");
                        request.AlwaysMultipartFormData = true;
                        response = client.Execute(request);
                        var resp = JsonConvert.DeserializeObject<RootobjectShipmentTracking>(response.Content);

                        if (resp != null && resp.events != null)
                        {
                            tuttiItracking.AddRange(resp.events.ToList());
                        }

                    }
                }
                return tuttiItracking;


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
                return null;
            }
        }
        private bool ValutaSeDeveAvereUnoSLA24(EspritecShipment.RootobjectShipmentTracking trkSH, Shipment shipGespe, GeoSpec.GeoClass geo)
        {
            if (trkSH.events.Any(x => x.statusID == 103 || x.statusID == 104 || x.statusID == 110 || x.statusID == 111 || x.statusID == 112 || x.statusID == 113 || x.statusID == 114 || x.statusID == 115 || x.statusID == 116 || x.statusID == 117
                 || x.statusID == 118 || x.statusID == 119 || x.statusID == 120 || x.statusID == 121 || x.statusID == 122 || x.statusID == 123 || x.statusID == 124 || x.statusID == 125 || x.statusID == 127 || x.statusID == 128
                 || x.statusID == 129 || x.statusID == 130 || x.statusID == 131 || x.statusID == 132 || x.statusID == 133 || x.statusID == 134 || x.statusID == 135 || x.statusID == 136 || x.statusID == 137 || x.statusID == 138 || x.statusID == 139
                 || x.statusID == 141 || x.statusID == 143))
            {
                return true;
            }

            if (geo == null)
            {
                return false;
            }

            if (geo.isDisagiata)
            {
                return true;
            }
            return false;
        }




        private EventTracking ValutaRegistrazioneSpedizione(EventTracking shipTrackingUnitexNR, CustomerSpec cust, Shipment shipUnitex)
        {
            bool ReturnRegState = false;
            try
            {              
                if (shipTrackingUnitexNR.statusID == 30 && cust.EsitiDaRaddrizzare)
                    ReturnRegState = RegistraSpedizioni(shipTrackingUnitexNR, cust, shipUnitex);
                if (ReturnRegState) return null;
                else return shipTrackingUnitexNR;
            }
            catch(Exception ex)
            {
                return null;
            }
        }



        //TODO: Claudio RDZ  Registra e leggi files e Raddrizza
        private bool RegistraSpedizioni(EventTracking shipTrackingUnitexNR, CustomerSpec cust, Shipment shipUnitex)
        {
            GestObjectToFile.Json JF = new GestObjectToFile.Json();
            string Percorso = string.Empty, PrefixFile = DateTime.Now.ToString("hhmmss");
            string FileNameTracking = string.Empty, fileNameShipment = string.Empty;
            try
            {
                Percorso = Path.Combine(".", cust.NOME, shipUnitex.docNumber);
                if (!Directory.Exists(Percorso)) Directory.CreateDirectory(Percorso); //Creo Directory per salvare i dati

                FileNameTracking = Percorso + "/" + PrefixFile + "_" + "EventTracking.txt";
                fileNameShipment = Percorso + "/" + PrefixFile + "_" + "ShipUnitex.txt";

                JF.WriteToFile<Shipment>(FileNameTracking, shipUnitex, false);
                JF.WriteToFile<EventTracking>(fileNameShipment, shipTrackingUnitexNR, false);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private void LeggiSpedizioni()
        {

        }


        /// <summary>
        /// Raddrizza la spedizione che gli viene passata
        /// va chiamata dalla funzione che decide se la spedizione e da RDZ (in funzione delle percentuali dion RDZ)
        /// </summary>
        /// <param name="shipTrackingUnitexNR"></param>
        /// <param name="shipUnitex"></param>
        /// <param name="cust"></param>
        private void RaddrizzaSpedizioneRegistrata(EventTracking shipTrackingUnitexNR, Shipment shipUnitex, CustomerSpec cust)
        {
            try
            {
                DateTime dataConsegna = DateTime.MinValue;
                DateTime.TryParseExact(shipTrackingUnitexNR.timeStamp, "yyyy-MM-ddTHH:mm:ss", null, DateTimeStyles.None, out dataConsegna);

                DateTime dataCarico = DateTime.MinValue;
                if (!shipUnitex.pickupDateTime.HasValue) dataCarico = shipUnitex.docDate.Value;
                else dataCarico = shipUnitex.pickupDateTime.Value.Date;

                DateTime? DaConfrontare = null;
                if (dataCarico.Date >= shipUnitex.docDate.Value.Date) DaConfrontare = dataCarico;
                else DaConfrontare = shipUnitex.docDate.Value;

                var geoSpec = GeoSpec.GeoList.FirstOrDefault(x => x.citta.ToLower() == shipUnitex.lastStopLocation.ToLower());
                if (geoSpec == null) geoSpec = GeoSpec.GeoList.FirstOrDefault(x => x.cap == shipUnitex.lastStopZipCode);

                if (string.IsNullOrEmpty(geoSpec.provincia) || string.IsNullOrEmpty(geoSpec.regione))
                {
                    try
                    {
                        geoSpec.provincia = shipUnitex.lastStopDistrict;
                        geoSpec.regione = GeoSpec.RecuperaRegioneDaProvincia(geoSpec.provincia);
                    }
                    catch (NullReferenceException ee)
                    {
                        GestoreMail.SegnalaErroreDev("Errore nel determinare la geo nella funzione di recovery", ee);
                    }
                    finally
                    {
                        geoSpec.provincia = "ND";
                        geoSpec.regione = "ND";
                    }
                }

                var trkSH = EspritecShipment.RestEspritecGetTracking(shipUnitex.id, cust.tokenAPI);
                var trkSHDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentTracking>(trkSH.Content);

                bool sla24 = ValutaSeDeveAvereUnoSLA24(trkSHDes, shipUnitex, geoSpec); //Valuta se è località disagiata
                var OreResaOttimali = TempiResa.TempiResaUtils.RecuperaOreResaOttimali(geoSpec, shipUnitex.ownerAgency, sla24);
                int OreResa = LocalGoogleCalendar.GiorniDiResaEffettivi(DaConfrontare.Value.Date, dataConsegna.Date) * 24;

                int Ritardo = OreResa - OreResaOttimali;

                //Se passa eseguo il raddrizzamento
                if (OreResa > OreResaOttimali && Ritardo <= 96)
                {
                    DateTime DataRaddOriginale = dataConsegna - TimeSpan.FromHours(Ritardo);
                    DateTime DataRadd = DataRaddOriginale;

                    if (DataRaddOriginale.DayOfWeek == DayOfWeek.Saturday) DataRadd = DataRaddOriginale.AddDays(-1);
                    else if (DataRaddOriginale.DayOfWeek == DayOfWeek.Sunday) DataRadd = DataRaddOriginale.AddDays(1);

                    string info = $"RDZ{DataRadd.ToString("yyMMdd")}";
                    var Trupd = new EspritecShipment.RootobjectTrackingUpdate()
                    {
                        tracking = new EspritecShipment.TrackingUpdate()
                        {
                            id = shipTrackingUnitexNR.id,
                            info = info,
                            signature = shipTrackingUnitexNR.signature,
                            timeStamp = shipTrackingUnitexNR.timeStamp,
                        }
                    };

                    string NonRaddrizzataPerCaso = $"{shipUnitex.id};{shipUnitex.docNumber};{cust.NOME};{"True"};{dataCarico.ToShortDateString()};{dataConsegna.ToShortDateString()};{DataRadd.ToShortDateString()};{Ritardo};{DateTime.Now};{geoSpec.provincia};{geoSpec.isCapoluogo};{geoSpec.regione}";
                    List<string> daComunicare = new List<string>();
                    daComunicare.Add(NonRaddrizzataPerCaso);
                    File.AppendAllLines(FileRaddrizzatiDaComunicare, daComunicare);
                    daComunicare.Clear();
                    shipTrackingUnitexNR.timeStamp = DataRadd.ToString("yyyy-MM-ddTHH:mm:ss");
                }

                if (cust == CustomerConnections.PHARDIS || cust == CustomerConnections.DIFARCO || cust == CustomerConnections.StockHouse)//cdgroup
                {
                    if (string.IsNullOrEmpty(shipUnitex.insideRef)) return;
                    var checkCodiceStato = statiDocumemtoCDGroup.FirstOrDefault(x => x.IdUnitex == shipTrackingUnitexNR.statusID);
                    if (checkCodiceStato != null)
                    {
                        var codiceStato = checkCodiceStato.CodiceStato;

                        var elem = new CDGROUP_EsitiOUT()
                        {
                            MANDANTE = shipUnitex.insideRef,
                            NUMERO_BOLLA = shipUnitex.externRef,
                            DATA_BOLLA = (shipUnitex.docDate != null) ? shipUnitex.docDate.Value.ToString("yyyyMMdd") : "        ",
                            RAGIONE_SOCIALE_VETTORE = "UNITEXPRESS",
                            DATA_PRESA_CONS = (shipUnitex.docDate != null) ? shipUnitex.docDate.Value.ToString("yyyyMMdd") : "        ",
                            STATO_CONSEGNA = codiceStato,
                            DESCRIZIONE_STATO_CONSEGNA = shipTrackingUnitexNR.statusDes,
                            //DATA = dataEsito.ToString("yyyyMMdd"),
                            DATA = DateTime.ParseExact(shipTrackingUnitexNR.timeStamp, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyyMMdd"),
                            RIFVETTORE = shipUnitex.docNumber,
                            statoUNITEX = shipTrackingUnitexNR.statusID
                        };
                        EsitiDaCoumicareCDGroup.Add(elem);
                    }
                }
                else if (cust == CustomerConnections.STMGroup)
                {
                    var checkCodiceStato = statiDocumemtoSTM.FirstOrDefault(x => x.IdUnitex == shipTrackingUnitexNR.statusID);
                    if (checkCodiceStato != null)
                    {
                        var codiceStato = checkCodiceStato.CodiceStato;

                        if (string.IsNullOrEmpty(shipUnitex.insideRef)) return;

                        var elem = new STM_EsitiOut()
                        {
                            CittaDestinatario = shipUnitex.firstStopLocation,
                            DataConsegnaEffettiva = (shipTrackingUnitexNR.statusID == 30) ? DateTime.ParseExact(shipTrackingUnitexNR.timeStamp, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyyMMdd") : "        ",
                            DataConsegnaTassativa = "        ",//TODO: verificare data tassativa da API
                            DataSpedizione = shipUnitex.docDate.Value.ToString("ddMMyyyy"),
                            DataTracking = DateTime.ParseExact(shipTrackingUnitexNR.timeStamp, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyyMMdd"),
                            Descrizione_Tracking = shipTrackingUnitexNR.statusDes,
                            ID_Tracking = codiceStato,
                            NumDDT = shipUnitex.externRef,
                            ProgressivoSpedizione = shipUnitex.docNumber.Split('/')[0],
                            regione = (geoSpec != null) ? geoSpec.regione : "ND"
                        };
                        EsitiDaCoumicareSTM.Add(elem);
                    }
                }
                else if (cust == CustomerConnections.Logistica93)
                {
                    var checkCodiceStato = statiDocumemtoLoreal.FirstOrDefault(x => x.IdUnitex == shipTrackingUnitexNR.statusID);
                    if (checkCodiceStato != null)
                    {
                        string sottoCausale = "000";
                        if (checkCodiceStato.CodiceStato == "03")
                        {
                            sottoCausale = "602";
                        }
                        if (checkCodiceStato.CodiceStato == "03" && shipTrackingUnitexNR.statusID == 61)
                        {
                            sottoCausale = "604";
                        }
                        var elem = new LorealEsiti()
                        {
                            E_NumeroDDT = shipUnitex.externRef,
                            E_RiferimentoNumeroConsegnaSAP = shipUnitex.insideRef,
                            E_DataConsegnaADestino = DateTime.ParseExact(shipTrackingUnitexNR.timeStamp, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyyMMdd"),
                            E_Causale = checkCodiceStato.CodiceStato,
                            E_SottoCausale = sottoCausale,
                            E_RiferimentoCorriere = shipUnitex.docNumber,
                            E_Filler1 = "",
                            E_Note = shipTrackingUnitexNR.info,
                            E_Filler2 = "",
                            statoUNITEX = shipTrackingUnitexNR.statusID
                        };
                        EsitiDaComunicareLoreal.Add(elem);
                    }
                }
                else if (cust == CustomerConnections.DAMORA)
                {
                    var elem = new DAMORA_EsitiOUT()
                    {
                        dataEsito = DateTime.ParseExact(shipTrackingUnitexNR.timeStamp, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyyMMdd"),
                        DescrizioneEsito = shipTrackingUnitexNR.statusDes,
                        rifExt = shipUnitex.externRef,
                    };
                    EsitiDaComunicareDamora.Add(elem);
                }
                else if (cust == CustomerConnections.CHIAPPAROLI)//chiapparoli
                {
                    //recupera i costi di spedizione
                    if (!shipUnitex.insideRef.Contains("|"))
                    {
                        return;
                    }
                    var specCHC = shipUnitex.insideRef.Split('|');
                    string CodiceDitta = "";
                    string SedeCHC = "";

                    if (specCHC.Count() > 1)
                    {
                        CodiceDitta = specCHC[1];
                        SedeCHC = specCHC[2];
                    }
                    var statoCHC = statiDocumemtoChiapparoli.FirstOrDefault(x => x.IdUnitex == shipTrackingUnitexNR.statusID);
                    if (statoCHC != null)
                    {
                        var elem = new Chiapparoli_EsitiOUT()
                        {
                            CodiceDitta = CodiceDitta,
                            CodiceResa = statoCHC.CodiceStato,//tabella codici stato esiti
                            Colli = shipUnitex.packs.ToString(),
                            DataResaAAMMGG = DateTime.ParseExact(shipTrackingUnitexNR.timeStamp, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyyMMdd"),
                            DataRiferimentoVettoreAAMMGG = DateTime.Now.ToString("yyMMdd"),
                            DataRiferimentoSubVettoreAAMMGG = "",
                            Filler = "",
                            ImportoTotaleSpedizione2d = "",//inserisci la somma dei costi
                            NumeroProgressivo = shipUnitex.externRef,
                            OraResa = DateTime.ParseExact(shipTrackingUnitexNR.timeStamp, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString("HHmm"),
                            Volume3d = Helper.GetVolumeChiapparoli(shipUnitex.cube, 3),
                            Peso2d = Helper.GetPesoChiapparoli(shipUnitex.grossWeight, 2),
                            PesoTassato = "",//formula calcolo
                            PosizioneRiga = "",//cos'è??
                            RiferimentoSubVettore = "",
                            RiferimentoVettore = shipUnitex.docNumber,
                            RigaNote = shipTrackingUnitexNR.statusDes,
                            SedeChiapparoli = SedeCHC,
                            statoUNITEX = shipTrackingUnitexNR.statusID
                        };
                        EsitiDaComunicareChiapparoli.Add(elem);
                    }
                }
            }
            catch
            {

            }
        }
        
        
        //--------------------------------

        //---------------------------------
        private void AggiungiAllaListaLesito(CustomerSpec cust, EventTracking shipTrackingUnitexNR, Shipment shipUnitex)
        {
            //if (string.IsNullOrEmpty(shipUnitex.insideRef)) return;           
            var geoSpec = GeoSpec.GeoList.FirstOrDefault(x => x.citta.ToLower() == shipUnitex.lastStopLocation.ToLower());
            if (geoSpec == null)
            {
                geoSpec = GeoSpec.GeoList.FirstOrDefault(x => x.cap == shipUnitex.lastStopZipCode);

            }
            //if (geoSpec == null)
            //{

            //}
            //----------------TODO: Claudio RDZ 1 Step verifica se registrare su file ----------------------
            EventTracking shipTrackingUnitex = /*shipTrackingUnitexNR (se non devo raddrizzare)*/ RaddrizzaTracking(shipTrackingUnitexNR, cust, shipUnitex, geoSpec);

            //Valuto se la spedizione e da registrare su file "if (shipTrackingUnitexNR.statusID == 30 && cust.EsitiDaRaddrizzare)" 
            //EventTracking shipTrackingUnitex = ValutaRegistrazioneSpedizione(shipTrackingUnitexNR, cust, shipUnitex);
            //if (shipTrackingUnitex == null) return;            
            //-----------------------------------------------------------------------------------------------


            DateTime dataEsito = DateTime.MinValue;
            DateTime.TryParseExact(shipTrackingUnitex.timeStamp, "yyyy-MM-ddTHH:mm:ss", null, DateTimeStyles.None, out dataEsito);


            if (cust == CustomerConnections.PHARDIS || cust == CustomerConnections.DIFARCO || cust == CustomerConnections.StockHouse)//cdgroup
            {
                if (string.IsNullOrEmpty(shipUnitex.insideRef)) return;
                var checkCodiceStato = statiDocumemtoCDGroup.FirstOrDefault(x => x.IdUnitex == shipTrackingUnitex.statusID);
                if (checkCodiceStato != null)
                {
                    var codiceStato = checkCodiceStato.CodiceStato;

                    var elem = new CDGROUP_EsitiOUT()
                    {
                        MANDANTE = shipUnitex.insideRef,
                        NUMERO_BOLLA = shipUnitex.externRef,
                        DATA_BOLLA = (shipUnitex.docDate != null) ? shipUnitex.docDate.Value.ToString("yyyyMMdd") : "        ",
                        RAGIONE_SOCIALE_VETTORE = "UNITEXPRESS",
                        DATA_PRESA_CONS = (shipUnitex.docDate != null) ? shipUnitex.docDate.Value.ToString("yyyyMMdd") : "        ",
                        STATO_CONSEGNA = codiceStato,
                        DESCRIZIONE_STATO_CONSEGNA = shipTrackingUnitex.statusDes,
                        DATA = dataEsito.ToString("yyyyMMdd"),
                        RIFVETTORE = shipUnitex.docNumber,
                        statoUNITEX = shipTrackingUnitex.statusID
                    };
                    EsitiDaCoumicareCDGroup.Add(elem);
                }

            }
            else if (cust == CustomerConnections.STMGroup)
            {
                var checkCodiceStato = statiDocumemtoSTM.FirstOrDefault(x => x.IdUnitex == shipTrackingUnitex.statusID);
                if (checkCodiceStato != null)
                {
                    var codiceStato = checkCodiceStato.CodiceStato;

                    if (string.IsNullOrEmpty(shipUnitex.insideRef)) return;

                    var elem = new STM_EsitiOut()
                    {
                        CittaDestinatario = shipUnitex.firstStopLocation,
                        DataConsegnaEffettiva = (shipTrackingUnitex.statusID == 30) ? dataEsito.ToString("yyyyMMdd") : "        ",
                        DataConsegnaTassativa = "        ",//TODO: verificare data tassativa da API
                        DataSpedizione = shipUnitex.docDate.Value.ToString("ddMMyyyy"),
                        DataTracking = dataEsito.ToString("ddMMyyyy"),
                        Descrizione_Tracking = shipTrackingUnitex.statusDes,
                        ID_Tracking = codiceStato,
                        NumDDT = shipUnitex.externRef,
                        ProgressivoSpedizione = shipUnitex.docNumber.Split('/')[0],
                        regione = (geoSpec != null) ? geoSpec.regione : "ND"
                    };
                    EsitiDaCoumicareSTM.Add(elem);
                }
            }
            else if (cust == CustomerConnections.Logistica93)
            {
                var checkCodiceStato = statiDocumemtoLoreal.FirstOrDefault(x => x.IdUnitex == shipTrackingUnitex.statusID);
                if (checkCodiceStato != null)
                {
                    string sottoCausale = "000";
                    if (checkCodiceStato.CodiceStato == "03")
                    {
                        sottoCausale = "602";
                    }
                    if (checkCodiceStato.CodiceStato == "03" && shipTrackingUnitex.statusID == 61)
                    {
                        sottoCausale = "604";
                    }
                    var elem = new LorealEsiti()
                    {
                        E_NumeroDDT = shipUnitex.externRef,
                        E_RiferimentoNumeroConsegnaSAP = shipUnitex.insideRef,
                        E_DataConsegnaADestino = dataEsito.ToString("yyyyMMdd"),
                        E_Causale = checkCodiceStato.CodiceStato,
                        E_SottoCausale = sottoCausale,
                        E_RiferimentoCorriere = shipUnitex.docNumber,
                        E_Filler1 = "",
                        E_Note = shipTrackingUnitex.info,
                        E_Filler2 = "",
                        statoUNITEX = shipTrackingUnitex.statusID
                    };
                    EsitiDaComunicareLoreal.Add(elem);
                }
            }
            else if (cust == CustomerConnections.DAMORA)
            {
                var elem = new DAMORA_EsitiOUT()
                {
                    dataEsito = dataEsito.ToString("ddMMyy"),
                    DescrizioneEsito = shipTrackingUnitex.statusDes,
                    rifExt = shipUnitex.externRef,
                };
                EsitiDaComunicareDamora.Add(elem);
            }
            else if (cust == CustomerConnections.CHIAPPAROLI)//chiapparoli
            {
                //recupera i costi di spedizione
                if (!shipUnitex.insideRef.Contains("|"))
                {
                    return;
                }
                var specCHC = shipUnitex.insideRef.Split('|');
                string CodiceDitta = "";
                string SedeCHC = "";

                if (specCHC.Count() > 1)
                {
                    CodiceDitta = specCHC[1];
                    SedeCHC = specCHC[2];
                }
                var statoCHC = statiDocumemtoChiapparoli.FirstOrDefault(x => x.IdUnitex == shipTrackingUnitex.statusID);
                if (statoCHC != null)
                {
                    var elem = new Chiapparoli_EsitiOUT()
                    {
                        CodiceDitta = CodiceDitta,
                        CodiceResa = statoCHC.CodiceStato,//tabella codici stato esiti
                        Colli = shipUnitex.packs.ToString(),
                        DataResaAAMMGG = dataEsito.ToString("yyMMdd"),
                        DataRiferimentoVettoreAAMMGG = DateTime.Now.ToString("yyMMdd"),
                        DataRiferimentoSubVettoreAAMMGG = "",
                        Filler = "",
                        ImportoTotaleSpedizione2d = "",//inserisci la somma dei costi
                        NumeroProgressivo = shipUnitex.externRef,
                        OraResa = dataEsito.ToString("HHmm"),
                        Volume3d = Helper.GetVolumeChiapparoli(shipUnitex.cube, 3),
                        Peso2d = Helper.GetPesoChiapparoli(shipUnitex.grossWeight, 2),
                        PesoTassato = "",//formula calcolo
                        PosizioneRiga = "",//cos'è??
                        RiferimentoSubVettore = "",
                        RiferimentoVettore = shipUnitex.docNumber,
                        RigaNote = shipTrackingUnitex.statusDes,
                        SedeChiapparoli = SedeCHC,
                        statoUNITEX = shipTrackingUnitex.statusID
                    };
                    EsitiDaComunicareChiapparoli.Add(elem);
                }
            }
            
            
            //else if (cust == CustomerConnections._3CS)
            //{
            //    return;
            //    var dataEsito = DateTime.Parse(shipTrackingUnitex.timeStamp.ToString());
            //    string dataPren = "";

            //    if (shipTrackingUnitex.statusID == 61)
            //    {
            //        dataPren = shipUnitex.deliveryDateTime.Value.ToString();//verificare maschera
            //    }

            //    string tipoEvento3C = DecodificaTipoEsito3C(shipTrackingUnitex.statusID);

            //    if (string.IsNullOrEmpty(tipoEvento3C))
            //    {
            //        return;
            //    }
            //    string mandante = shipUnitex.senderDes.Substring(3);//i primi 3 caratteri sono 3C-

            //    var elem = new _3C_EsitiOUT()
            //    {
            //        NumeroBolla = shipUnitex.insideRef,
            //        Filler = "".PadRight(9, ' '),
            //        CodiceMittente = "",
            //        RagioneSocialeMittente = mandante.Length > 40 ? mandante.Substring(0, 40) : mandante.PadRight(40, ' '),
            //        RagioneSocialeDestinatario = shipUnitex.lastStopDes,
            //        LocalitaDestinatario = shipUnitex.lastStopLocation,
            //        ProvDestinatario = shipUnitex.lastStopDistrict.Length > 2 ? shipUnitex.lastStopDistrict.Substring(0, 2) : shipUnitex.lastStopDistrict.PadRight(2, ' '),
            //        NumeroColli = shipUnitex.packs.ToString().PadRight(5, '0'),//verificare maschera
            //        Peso1D = shipUnitex.grossWeight.ToString().PadRight(7, '0'),//verificare maschera
            //        DataEvento = dataEsito.ToString("yyyyMMdd"),
            //        TipoEvento = tipoEvento3C,
            //        DataPrenotazione = dataPren,
            //        DescrizioneEvento = shipTrackingUnitex.statusDes,
            //        Progressivo = "0",
            //        OrarioEvento = "    ",
            //        BarcodeSegnacolloLetto = "".PadRight(30, ' '),
            //        Filler2 = "".PadRight(38, ' '),
            //        statoUNITEX = shipTrackingUnitex.statusID
            //    };
            //    EsitiDaComunicare_3C.Add(elem);
            //}
        }

        private EventTracking RaddrizzaTracking(EventTracking shipTrackingUnitexNR, CustomerSpec cust, Shipment shipUnitex, GeoSpec.GeoClass geoSpec)
        {
            if (shipTrackingUnitexNR.statusID == 30 && cust.EsitiDaRaddrizzare)
            {
                //recupera tutti i tracking della LDV
                var trkSH = EspritecShipment.RestEspritecGetTracking(shipUnitex.id, cust.tokenAPI);
                var trkSHDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentTracking>(trkSH.Content);

                bool HasGiacenza = trkSHDes.events.Any(x => x.statusID == 50);
                if (HasGiacenza)
                {
                    return shipTrackingUnitexNR;
                }

                bool SLAPreno = trkSHDes.events.Any(x => x.statusID == 61);
                if (SLAPreno)
                {
                    return shipTrackingUnitexNR;
                }

                DateTime dataConsegna = DateTime.MinValue;
                DateTime.TryParseExact(shipTrackingUnitexNR.timeStamp, "yyyy-MM-ddTHH:mm:ss", null, DateTimeStyles.None, out dataConsegna);

                DateTime dataCarico = DateTime.MinValue;
                if (!shipUnitex.pickupDateTime.HasValue)
                {
                    dataCarico = shipUnitex.docDate.Value;
                }
                else
                {
                    dataCarico = shipUnitex.pickupDateTime.Value.Date;
                }

                DateTime? DaConfrontare = null;
                if (dataCarico.Date >= shipUnitex.docDate.Value.Date)
                {
                    DaConfrontare = dataCarico;
                }
                else
                {
                    DaConfrontare = shipUnitex.docDate.Value;
                }
                
                bool sla24 = ValutaSeDeveAvereUnoSLA24(trkSHDes, shipUnitex, geoSpec); //Valuta se è località disagiata
                var OreResaOttimali = TempiResa.TempiResaUtils.RecuperaOreResaOttimali(geoSpec, shipUnitex.ownerAgency, sla24);
                int OreResa = LocalGoogleCalendar.GiorniDiResaEffettivi(DaConfrontare.Value.Date, dataConsegna.Date) * 24;

                //Raddrizza il timestamp dell'esito consegnata
                int Ritardo = OreResa - OreResaOttimali;
                if (OreResa > OreResaOttimali && Ritardo <= 96)
                {

                    DateTime DataRaddOriginale = dataConsegna - TimeSpan.FromHours(Ritardo);
                    DateTime DataRadd = DataRaddOriginale;

                    if (DataRaddOriginale.DayOfWeek == DayOfWeek.Saturday)
                    {
                        DataRadd = DataRaddOriginale.AddDays(-1);
                    }
                    else if (DataRaddOriginale.DayOfWeek == DayOfWeek.Sunday)
                    {
                        DataRadd = DataRaddOriginale.AddDays(1);
                    }

                    //80% di chance che venga raddrizzato
                    Random rand = new Random();
                    int number = rand.Next(1, 101);
                    bool RNGCheck = true;

                    if (string.IsNullOrEmpty(geoSpec.provincia) || string.IsNullOrEmpty(geoSpec.regione))
                    {
                        try
                        {
                            geoSpec.provincia = shipUnitex.lastStopDistrict;
                            geoSpec.regione = GeoSpec.RecuperaRegioneDaProvincia(geoSpec.provincia);
                        }
                        catch (NullReferenceException ee)
                        {
                            GestoreMail.SegnalaErroreDev("Errore nel determinare la geo nella funzione di recovery", ee);
                        }
                        finally
                        {
                            geoSpec.provincia = "ND";
                            geoSpec.regione = "ND";
                        }
                    }

                    //TODO: Claudio Raddrizzatura STM
                    if (cust.ID_GESPE == "00177") //Se è STM, 85% -- Claudio 93%
                    {
                        if (number < 7) //Se è un numero tra 1 e 15 non la toccare, 15% di chance che non venga toccata. (--7)
                        {
                            //Brutto, brutto, brutto, brutto.
                            RNGCheck = false;
                            string NonRaddrizzataPerCaso = $"{shipUnitex.id};{shipUnitex.docNumber};{cust.NOME};{RNGCheck};{dataCarico.ToShortDateString()};{dataConsegna.ToShortDateString()};{DataRadd.ToShortDateString()};{Ritardo};{DateTime.Now};{geoSpec.provincia};{geoSpec.isCapoluogo};{geoSpec.regione}";
                            List<string> daComunicare = new List<string>();
                            daComunicare.Add(NonRaddrizzataPerCaso);
                            File.AppendAllLines(FileRaddrizzatiDaComunicare, daComunicare);
                            daComunicare.Clear();
                            return shipTrackingUnitexNR;
                        }
                    }
                    else if (number < 20) //Se è un numero tra 1 e 20 non la toccare, 20% di chance che non venga toccata.
                    {
                        //Brutto, brutto, brutto, brutto.
                        RNGCheck = false;
                        string NonRaddrizzataPerCaso = $"{shipUnitex.id};{shipUnitex.docNumber};{cust.NOME};{RNGCheck};{dataCarico.ToShortDateString()};{dataConsegna.ToShortDateString()};{DataRadd.ToShortDateString()};{Ritardo};{DateTime.Now};{geoSpec.provincia};{geoSpec.isCapoluogo};{geoSpec.regione}";
                        List<string> daComunicare = new List<string>();
                        daComunicare.Add(NonRaddrizzataPerCaso);
                        File.AppendAllLines(FileRaddrizzatiDaComunicare, daComunicare);
                        daComunicare.Clear();
                        return shipTrackingUnitexNR;
                    }

                    string info = $"RDZ{DataRadd.ToString("yyMMdd")}";
                    var Trupd = new EspritecShipment.RootobjectTrackingUpdate()
                    {
                        tracking = new EspritecShipment.TrackingUpdate()
                        {
                            id = shipTrackingUnitexNR.id,
                            info = info,
                            signature = shipTrackingUnitexNR.signature,
                            timeStamp = shipTrackingUnitexNR.timeStamp,
                        }
                    };
                    var ok = EspritecShipment.RestEspritecUpdateTracking(Trupd, token_UNITEX);
                    var okDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectTrackingUpdateResponse>(ok.Content);

                    string Raddrizzata = $"{shipUnitex.id};{shipUnitex.docNumber};{cust.NOME};{RNGCheck};{dataCarico.ToShortDateString()};{dataConsegna.ToShortDateString()};{DataRadd.ToShortDateString()};{Ritardo};{DateTime.Now}";
                    List<string> daComunicare2 = new List<string>();
                    daComunicare2.Add(Raddrizzata);
                    File.AppendAllLines(FileRaddrizzatiDaComunicare, daComunicare2);
                    daComunicare2.Clear();

                    string Comunicazione = $"{cust.NOME}; Data carico: {dataCarico.Date}; Raddrizzato ID:{shipUnitex.id}; DocNum:{shipUnitex.docNumber}; Rif.Cli:{shipUnitex.externRef} da {dataConsegna.Date} a {DataRadd.Date};";
                    _loggerCode.Debug(Comunicazione);

                    //if (!okDes.status)
                    //{
                    //    throw new Exception("Errore comunicazione con gespe");
                    //}
                    shipTrackingUnitexNR.timeStamp = DataRadd.ToString("yyyy-MM-ddTHH:mm:ss");
                }
            }
            return shipTrackingUnitexNR;
        }
        //----------------------------------



        private DateTime ValutaERaddrizzaIlDato(int rMax, int oreResa, DateTime dataEsito)
        {
            if (oreResa > rMax && oreResa <= 96)
            {
                int oreDiff = oreResa - rMax;
                var raddrizzata = dataEsito - TimeSpan.FromHours(oreDiff);
                if (raddrizzata.DayOfWeek == DayOfWeek.Saturday)
                {
                    raddrizzata = raddrizzata.AddDays(-1);
                }
                else if (raddrizzata.DayOfWeek == DayOfWeek.Sunday)
                {
                    raddrizzata = raddrizzata.AddDays(-2);
                }
                else
                {
                    return raddrizzata;

                }

                if (dataEsito == raddrizzata)
                {
                    return dataEsito;
                }
                return raddrizzata;

                //aggiorna Gespe
            }
            else
            {
                return dataEsito;
            }
        }
        private int RecuperaGiorniResaOttimali(Model.GeoClass geo, string ownerAgency)
        {
            if (geo.regione.ToLower() == "abruzzo")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "basilicata")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "calabria")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "campania")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "emilia-romagna")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "friuli-venezia giulia")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "lazio")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "liguria")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "lombardia")
            {
                if (ownerAgency == "01")
                {
                    return geo.isCapoluogo ? 2 : 3;

                }
                else
                {
                    return 1;
                }
            }
            else if (geo.regione.ToLower() == "marche")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "molise")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "piemonte")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "puglia")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "sardegna")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "sicilia")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "toscana")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "trentino-alto adige")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "umbria")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "valle d'aosta")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else if (geo.regione.ToLower() == "veneto")
            {
                return geo.isCapoluogo ? 2 : 3;
            }
            else
            {
                return geo.isCapoluogo ? 2 : 3;
            }
        }
        #endregion
        private string DecodificaTipoEsito3C(int statusID)
        {
            if (statusID == 30)
            {
                return "D";
            }
            else if (statusID == 61)
            {
                return "T";
            }
            else if (statusID == 50)
            {
                return "G";
            }
            else if (statusID == 55)
            {
                return "G";
            }
            else if (statusID == 1)
            {
                return "Q";
            }
            else if (statusID == 10)
            {
                return "P";
            }
            else
            {
                return "";
            }
        }
        private DateTime RecuperaTSDaStringa(string v)
        {
            var resp = DateTime.MinValue;
            var dataShort = v.Split(' ')[0];

            var pzData = dataShort.Split('/');
            if (pzData.Count() == 3)
            {
                DateTime.TryParseExact(dataShort, "dd/MM/yyyy", null, DateTimeStyles.None, out resp);
            }
            return resp;

        }
        private void ScriviLastCheckChangesTMS(bool append)
        {
            DateTime daScrivere = new DateTime(LastCheckChangesTMS.Year, LastCheckChangesTMS.Month, LastCheckChangesTMS.Day, LastCheckChangesTMS.Hour, LastCheckChangesTMS.Minute, 00);
            if (!append)
            {
                File.WriteAllText(PathLastCheckChangesFileTMS, daScrivere.ToString());
            }
            else
            {
                File.AppendAllText(PathLastCheckChangesFileTMS, "\r\n" + daScrivere.ToString());
            }
        }
        private Shipment RecuperaShipUnitexByShipmentID(int shipID)
        {
            var resp = new Shipment();
            var clientLink = new RestClient(endpointAPI_UNITEX + $"/api/tms/shipment/get/{shipID}");
            clientLink.Timeout = 5000;
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
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            timerAggiornamentoCiclo.Stop();

            try
            {
                RecuperaConnessione();
                ControllaSeIClientiCiHannoInviatoQualcosa();


                //CorreggiVolumeCDLTraDate(new DateTime(2022, 09, 01), new DateTime(2022, 09, 30));
                //PopolaDatiStoriciTracking();
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
        private void CorreggiVolumeCDLTraDate(DateTime dateTime1, DateTime dateTime2)
        {
            var tutteLeShipmentTraLeDate = GetShipments(dateTime1, dateTime2);

            foreach (var ship in tutteLeShipmentTraLeDate)
            {
                var actualGoodsShip = RecuperaLeRigheLDV(ship.id);
                if (actualGoodsShip != null && actualGoodsShip.goods.cube > 0.1M)
                {
                    var upd = new GoodNewShipmentTMS()
                    {
                        cube = 0,
                        depth = 0,
                        height = 0,
                        meters = 0,
                        width = 0
                    };

                    AggiornaGoods(upd);
                }
            }
        }
        private void AggiornaGoods(GoodNewShipmentTMS upd)
        {
            var resource = $"/api/tms/shipment/goods/update";
            var client = new RestClient(endpointAPI_UNITEX);
            var request = new RestRequest(resource, Method.POST);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddJsonBody(upd);

            client.Timeout = -1;
            request.AddHeader("Authorization", $"Bearer {token_UNITEX}");
            request.AlwaysMultipartFormData = true;
            client.Execute(request);
        }
        private RootobjectGoodsUpdate RecuperaLeRigheLDV(int id)
        {
            var resource = $"/api/tms/shipment/goods/list/{id}";
            var client = new RestClient(endpointAPI_UNITEX);
            var request = new RestRequest(resource, Method.GET);

            client.Timeout = -1;
            request.AddHeader("Authorization", $"Bearer {token_UNITEX}");
            request.AlwaysMultipartFormData = true;
            IRestResponse response = client.Execute(request);

            return JsonConvert.DeserializeObject<RootobjectGoodsUpdate>(response.Content);
        }
        public List<Model.Espritec_API.UNITEX.Shipment> GetShipments(DateTime dateTime1, DateTime dateTime2)
        {
            var result = new List<Model.Espritec_API.UNITEX.Shipment>();

            RecuperaConnessione();

            try
            {
                var pageNumber = 1;
                var pageRows = 50;
                var resource = $"/api/tms/shipment/list/{pageRows}/{pageNumber}?StartDate={dateTime1.ToString("MM-dd-yyyy")}&EndDate={dateTime2.ToString("MM-dd-yyyy")}";
                var client = new RestClient(endpointAPI_UNITEX);
                var request = new RestRequest(resource, Method.GET);

                client.Timeout = -1;
                request.AddHeader("Authorization", $"Bearer {token_UNITEX}");
                request.AlwaysMultipartFormData = true;
                IRestResponse response = client.Execute(request);

                var resp = JsonConvert.DeserializeObject<TmsShipmentList>(response.Content);

                if (resp != null && resp.shipments != null)
                {
                    result = resp.shipments.ToList();
                    var maxPages = resp.result.maxPages;

                    while (maxPages > 1)
                    {
                        pageNumber++;
                        maxPages--;
                        resource = $"/api/tms/shipment/list/{pageRows}/{pageNumber}?StartDate={dateTime1.ToString("MM-dd-yyyy")}&EndDate={dateTime2.ToString("MM-dd-yyyy")}";
                        request = new RestRequest(resource, Method.GET);
                        request.AddHeader("Authorization", $"Bearer {token_UNITEX}");
                        request.AlwaysMultipartFormData = true;
                        response = client.Execute(request);
                        resp = JsonConvert.DeserializeObject<TmsShipmentList>(response.Content);

                        if (resp != null && resp.shipments != null)
                        {
                            result.AddRange(resp.shipments.ToList());
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee, "EspritecAPI_UNITEX.GetShipments");
            }
            return result;
        }
        private void RecuperaConnessione()
        {
            if ((DateTime.Now + TimeSpan.FromHours(1)) > DataScadenzaToken_UNITEX)
            {
                UnitexGespeAPILogin(userAPIADMIN, passwordAPIADMIN, out token_UNITEX, out DataScadenzaToken_UNITEX);
            }

            foreach (var Cust in CustomerConnections.customers)
            {
                if (!string.IsNullOrEmpty(Cust.userAPI) && (DateTime.Now + TimeSpan.FromHours(1)) > Cust.scadenzaTokenAPI)
                {
                    UnitexGespeAPILogin(Cust.userAPI, Cust.pswAPI, out string token, out DateTime scad);
                    Cust.tokenAPI = token;
                    Cust.scadenzaTokenAPI = scad;
                }
            }

            #region Deprecated
            //if ((DateTime.Now + TimeSpan.FromHours(1)) > CustomerConnections.DIFARCO.scadenzaTokenAPI)
            //{
            //    UnitexGespeAPILogin(CustomerConnections.DIFARCO.userAPI, CustomerConnections.DIFARCO.pswAPI, out string token, out DateTime scad);
            //    CustomerConnections.DIFARCO.tokenAPI = token;
            //    CustomerConnections.DIFARCO.scadenzaTokenAPI = scad;
            //}
            //if ((DateTime.Now + TimeSpan.FromHours(1)) > CustomerConnections.PHARDIS.scadenzaTokenAPI)
            //{
            //    UnitexGespeAPILogin(CustomerConnections.PHARDIS.userAPI, CustomerConnections.PHARDIS.pswAPI, out string token, out DateTime scad);
            //    CustomerConnections.PHARDIS.tokenAPI = token;
            //    CustomerConnections.PHARDIS.scadenzaTokenAPI = scad;
            //}
            //if ((DateTime.Now + TimeSpan.FromHours(1)) > CustomerConnections.StockHouse.scadenzaTokenAPI)
            //{
            //    UnitexGespeAPILogin(CustomerConnections.StockHouse.userAPI, CustomerConnections.StockHouse.pswAPI, out string token, out DateTime scad);
            //    CustomerConnections.StockHouse.tokenAPI = token;
            //    CustomerConnections.StockHouse.scadenzaTokenAPI = scad;
            //}
            //if ((DateTime.Now + TimeSpan.FromHours(1)) > CustomerConnections.STMGroup.scadenzaTokenAPI)
            //{
            //    UnitexGespeAPILogin(CustomerConnections.STMGroup.userAPI, CustomerConnections.STMGroup.pswAPI, out string token, out DateTime scad);
            //    CustomerConnections.STMGroup.tokenAPI = token;
            //    CustomerConnections.STMGroup.scadenzaTokenAPI = scad;
            //}
            //if ((DateTime.Now + TimeSpan.FromHours(1)) > CustomerConnections.Logistica93.scadenzaTokenAPI)
            //{
            //    UnitexGespeAPILogin(CustomerConnections.Logistica93.userAPI, CustomerConnections.Logistica93.pswAPI, out string token, out DateTime scad);
            //    CustomerConnections.Logistica93.tokenAPI = token;
            //    CustomerConnections.Logistica93.scadenzaTokenAPI = scad;
            //}
            //if ((DateTime.Now + TimeSpan.FromHours(1)) > CustomerConnections.DAMORA.scadenzaTokenAPI)
            //{
            //    UnitexGespeAPILogin(CustomerConnections.DAMORA.userAPI, CustomerConnections.DAMORA.pswAPI, out string token, out DateTime scad);
            //    CustomerConnections.DAMORA.tokenAPI = token;
            //    CustomerConnections.DAMORA.scadenzaTokenAPI = scad;
            //}
            //if ((DateTime.Now + TimeSpan.FromHours(1)) > CustomerConnections.CEVA.scadenzaTokenAPI)
            //{
            //    UnitexGespeAPILogin(CustomerConnections.CEVA.userAPI, CustomerConnections.CEVA.pswAPI, out string token, out DateTime scad);
            //    CustomerConnections.CEVA.tokenAPI = token;
            //    CustomerConnections.CEVA.scadenzaTokenAPI = scad;
            //}
            //if ((DateTime.Now + TimeSpan.FromHours(1)) > CustomerConnections.CHIAPPAROLI.scadenzaTokenAPI)
            //{
            //    UnitexGespeAPILogin(CustomerConnections.CHIAPPAROLI.userAPI, CustomerConnections.CHIAPPAROLI.pswAPI, out string token, out DateTime scad);
            //    CustomerConnections.CHIAPPAROLI.tokenAPI = token;
            //    CustomerConnections.CHIAPPAROLI.scadenzaTokenAPI = scad;
            //}
            //if ((DateTime.Now + TimeSpan.FromHours(1)) > CustomerConnections.CHIAPPAROLI.scadenzaTokenAPI)
            //{
            //    UnitexGespeAPILogin(CustomerConnections.CEVA.userAPI, CustomerConnections.CEVA.pswAPI, out string token, out DateTime scad);
            //    CustomerConnections.CEVA.tokenAPI = token;
            //    CustomerConnections.CEVA.scadenzaTokenAPI = scad;
            //}
            //if ((DateTime.Now + TimeSpan.FromHours(1)) > CustomerConnections.GXO.scadenzaTokenAPI)
            //{
            //    UnitexGespeAPILogin(CustomerConnections.GXO.userAPI, CustomerConnections.GXO.pswAPI, out string token, out DateTime scad);
            //    CustomerConnections.GXO.tokenAPI = token;
            //}
            //if ((DateTime.Now + TimeSpan.FromHours(1)) > CustomerConnections._3CS.scadenzaTokenAPI)
            //{
            //    UnitexGespeAPILogin(CustomerConnections._3CS.userAPI, CustomerConnections._3CS.pswAPI, out string token, out DateTime scad);
            //    CustomerConnections._3CS.tokenAPI = token;
            //    CustomerConnections._3CS.scadenzaTokenAPI = scad;
            //} 
            #endregion
        }

        public static string appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UNITEX_DOCUMENT_SERVICE");

        object semaphoro = new object();
        private void ControllaSeIClientiCiHannoInviatoQualcosa()
        {
            //_loggerCode.Debug("recupero file in corso");
            foreach (var cust in CustomerConnections.customers)
            {

                //DEBUGITERATOR
                //if (cust.ID_GESPE != "00327") continue; //Claudio Commentata


                _loggerCode.Debug($"Analizzo se il cliente {cust.NOME} ci ha inviato qualcosa");

                try
                {
                    var daCancellare = Directory.GetFiles(cust.LocalWorkPath);
                    foreach (var dc in daCancellare)
                    {
                        File.Delete(dc);
                    }

                    #region sftp
                    if (cust == CustomerConnections.Logistica93 || cust == CustomerConnections.CHIAPPAROLI)
                    {
                        _loggerCode.Debug($"recupero file per {cust.NOME}");
                        var sftp = CreaClientSFTPperIlCliente(cust);
                        sftp.ChangeDirectory(cust.RemoteINCustomerPath);
                        var remoteFiles = sftp.ListDirectory(cust.RemoteINCustomerPath).Where(x => x.FullName.ToLower().EndsWith(".txt")).ToList();
                        _loggerCode.Debug($"rilevati {remoteFiles.Count()}");

                        try
                        {
                            foreach (var r in remoteFiles)
                            {
                                using (Stream fileStream = File.Create(Path.Combine(cust.LocalInFilePath, Path.GetFileName(r.FullName))))
                                {
                                    sftp.DownloadFile(r.FullName, fileStream);
                                    _loggerAPI.Debug($"Scaricato file: {Path.GetFileName(r.FullName)}");
                                }
                                try
                                {

                                    sftp.DeleteFile(r.FullName);
                                }
                                catch (Exception ee)
                                {
                                    string msg = $"non sono riuscito a cancellare il file {r.FullName} dal sftp del cliente {cust.NOME}";
                                    _loggerCode.Debug(msg);
                                    //GestoreMail.SegnalaErroreDev(msg, ee);
                                }

                            }
                        }
                        catch (Exception ee)
                        {
                            if (!ee.Message.StartsWith("Local path must specify a file path and not a folder path."))
                            {
                                GestoreMail.SegnalaErroreDev($"sftp errore produzione file {cust.NOME} ", ee);
                            }
                        }
                        finally
                        {
                            sftp.Disconnect();
                        }

                    }
                    #endregion

                    #region ftp
                    //if (cust == CustomerConnections.CHIAPPAROLI)
                    //{
                    //    var ftpClient = CreaClientFTPperIlCliente(cust);
                    //    var remoteFiles = ftpClient.GetListing(cust.RemoteINCustomerPath);
                    //    try
                    //    {
                    //        lock (semaphoro)
                    //        {
                    //            foreach (var r in remoteFiles)
                    //            {
                    //                var inDest = Path.Combine(cust.LocalInFilePath, r.Name);
                    //                ftpClient.DownloadFile(inDest, r.FullName);
                    //                ftpClient.DeleteFile(r.FullName);
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ee)
                    //    {
                    //        if (!ee.Message.StartsWith("Local path must specify a file path and not a folder path."))
                    //        {
                    //            GestoreMail.SegnalaErroreDev($"errore produzione file {cust.ID_GESPE}", ee);
                    //        }
                    //    }
                    //    var daCancellare = Directory.GetFiles(cust.LocalWorkPath);
                    //    foreach (var dc in daCancellare)
                    //    {
                    //        File.Delete(dc);
                    //    }
                    //    ftpClient.Disconnect();
                    //}
                    #endregion

                    var filesDaProcessare = Directory.GetFiles(cust.LocalInFilePath, "*.*", SearchOption.TopDirectoryOnly);

                    if (filesDaProcessare.Count() > 0)
                    {
                        _loggerCode.Debug($"{cust.NOME} trovati {filesDaProcessare.Count()} da processare");
                        ProcessaIFileRecuperati(cust, filesDaProcessare.ToList());
                    }
                }
                catch (Exception rr)
                {
                    _loggerCode.Debug(rr);
                    GestoreMail.SegnalaErroreDev($"ControllaSeIClientiCiHannoInviatoQualcosa ", rr);
                }
            }
        }

        private void ProcessaIFileRecuperati(CustomerSpec cust, List<string> filesDaProcessare)
        {
            foreach (var fr in filesDaProcessare)
            {
                var justFileName = Path.GetFileName(fr);
                _loggerCode.Debug($"Processo il file {justFileName}");
                try
                {
                    if (!File.Exists(fr))
                    {
                        continue;
                    }
                    
                    InterpretaIlFileEdIserisciloInGespe(cust, fr, out List<string> filesProcessati);
                    
                    foreach (var f in filesProcessati)
                    {
                        var jn = Path.GetFileName(f);
                        if (!File.Exists(f)) continue;
                        _loggerCode.Debug($"Conservo il file {f}");
                        var dest = Path.Combine(cust.LocalInFilePath, "Elaborati");
                        if (!Directory.Exists(dest)) Directory.CreateDirectory(dest);

                        var pathStored = Path.Combine(dest, jn + "_" + DateTime.Now.ToString("ddMMyyyyHHssmm") + "_Elaborato");
                        if (File.Exists(pathStored))
                        {
                            File.Move(pathStored, pathStored + ".bk");
                        }
                        File.Move(f, pathStored);
                    }
                    _loggerCode.Debug($"Processo il file {justFileName} terminato");
                }
                catch (Exception rr)
                {
                    _loggerCode.Debug(rr);
                    GestoreMail.SegnalaErroreDev($"cloudftp {justFileName}", rr);
                    File.Move(fr, Path.Combine(cust.LocalErrorFilePath, justFileName));
                }
            }

        }
        private void InterpretaIlFileEdIserisciloInGespe(CustomerSpec cust, string fr, out List<string> fileProcessati)
        {

            fileProcessati = new List<string>();
            fileProcessati.Add(fr);
            //fileProcessati.Add()
            List<RootobjectNewShipmentTMS> ListShip = new List<RootobjectNewShipmentTMS>();
            List<string> RigheCSV = new List<string>();
            int iiDebug = 0;

            if (cust == CustomerConnections.GUNA)
            {
                var tutteLeRighe = File.ReadAllLines(fr);
                if (tutteLeRighe.Count() > 0)
                {
                    bool testataTrovata = false;
                    GUNA_ShipmentIN ShipGuna = new GUNA_ShipmentIN();
                    for (int i = 0; i < tutteLeRighe.Count(); i++)
                    {
                        try
                        {
                            RootobjectNewShipmentTMS shipmentTMS = new RootobjectNewShipmentTMS();
                            List<ParcelNewShipmentTMS> parcelNewShipment = new List<ParcelNewShipmentTMS>();
                            List<StopNewShipmentTMS> destinazione = new List<StopNewShipmentTMS>();
                            List<GoodNewShipmentTMS> merce = new List<GoodNewShipmentTMS>();
                            var r = tutteLeRighe[i];
                            if (i < tutteLeRighe.Count())
                            {
                                if (r.StartsWith("2"))//E la testata della spedizione
                                {
                                    if (!testataTrovata)
                                    {
                                        testataTrovata = true;
                                        ShipGuna = new GUNA_ShipmentIN()
                                        {
                                            AASPED = r.Substring(ShipGuna.idxAASPED[0], ShipGuna.idxAASPED[1]).Trim(),
                                            MMGGSPED = r.Substring(ShipGuna.idxMMGGSPED[0], ShipGuna.idxMMGGSPED[1]).Trim(),
                                            NRSPED = r.Substring(ShipGuna.idxNRSPED[0], ShipGuna.idxNRSPED[1]).Trim(),
                                            RAGSOC1 = r.Substring(ShipGuna.idxRAGSOC1[0], ShipGuna.idxRAGSOC1[1]).Trim(),
                                            RAGSOC2 = r.Substring(ShipGuna.idxRAGSOC2[0], ShipGuna.idxRAGSOC2[1]).Trim(),
                                            ADDRESS = r.Substring(ShipGuna.idxADDRESS[0], ShipGuna.idxADDRESS[1]).Trim(),
                                            PTCODE = r.Substring(ShipGuna.idxPTCODE[0], ShipGuna.idxPTCODE[1]).Trim(),
                                            CITY = r.Substring(ShipGuna.idxCITY[0], ShipGuna.idxCITY[1]).Trim(),
                                            REGION = r.Substring(ShipGuna.idxREGION[0], ShipGuna.idxREGION[1]).Trim(),
                                            COUNTRY = r.Substring(ShipGuna.idxCOUNTRY[0], ShipGuna.idxCOUNTRY[1]).Trim(),
                                            NCOLLI = r.Substring(ShipGuna.idxNCOLLI[0], ShipGuna.idxNCOLLI[1]).Trim(),
                                            PESO = r.Substring(ShipGuna.idxPESO[0], ShipGuna.idxPESO[1]).Trim(),
                                            VOLUME = r.Substring(ShipGuna.idxVOLUME[0], ShipGuna.idxVOLUME[1]).Trim(),
                                            C_ASS = r.Substring(ShipGuna.idxC_ASS[0], ShipGuna.idxC_ASS[1]).Trim(),
                                            TP_INCASSO = r.Substring(ShipGuna.idxTP_INCASSO[0], ShipGuna.idxTP_INCASSO[1]).Trim(),
                                            DIVISA_C_ASS = r.Substring(ShipGuna.idxDIVISA_C_ASS[0], ShipGuna.idxDIVISA_C_ASS[1]).Trim(),
                                            RIFMITT_C = r.Substring(ShipGuna.idxRIFMITT_C[0], ShipGuna.idxRIFMITT_C[1]).Trim(),
                                            TEL1 = r.Substring(ShipGuna.idxTEL1[0], ShipGuna.idxTEL1[1]).Trim(),
                                            TEL2 = r.Substring(ShipGuna.idxTEL2[0], ShipGuna.idxTEL2[1]).Trim(),
                                            TEL3 = r.Substring(ShipGuna.idxTEL3[0], ShipGuna.idxTEL3[1]).Trim(),
                                            NOTE1 = r.Substring(ShipGuna.idxNOTE1[0], ShipGuna.idxNOTE1[1]).Trim(),
                                            NOTE2 = r.Substring(ShipGuna.idxNOTE2[0], ShipGuna.idxNOTE2[1]).Trim(),
                                            EOL = r.Substring(ShipGuna.idxEOL[0], ShipGuna.idxEOL[1]).Trim()
                                        };
                                    }
                                    else
                                    {

                                        //invia ad api unitex la spedizione e reinizializza la testata
                                        var headerNewShipment = new HeaderNewShipmentTMS();

                                        headerNewShipment.docDate = ShipGuna.AASPED + "-" + ShipGuna.MMGGSPED.Substring(0, 2) + "-" + ShipGuna.MMGGSPED.Substring(2, 2);
                                        headerNewShipment.publicNote = $"{ShipGuna.NOTE1.Trim()} {ShipGuna.NOTE2.Trim()} {ShipGuna.TEL1.Trim()} {ShipGuna.TEL2.Trim()} {ShipGuna.TEL3.Trim()}".Trim();
                                        headerNewShipment.customerID = cust.ID_GESPE;
                                        headerNewShipment.cashCurrency = ShipGuna.DIVISA_C_ASS;
                                        headerNewShipment.cashValue = Helper.GetDecimalFromString(ShipGuna.C_ASS, 2);
                                        headerNewShipment.externRef = ShipGuna.NRSPED;
                                        headerNewShipment.carrierType = "EDI";//"COLLO";
                                        headerNewShipment.serviceType = "S";
                                        headerNewShipment.incoterm = "PF";
                                        headerNewShipment.transportType = "8-25";
                                        headerNewShipment.cashPayment = ShipGuna.TP_INCASSO;
                                        headerNewShipment.type = "Groupage";
                                        headerNewShipment.cashNote = "";
                                        headerNewShipment.insideRef = ShipGuna.RIFMITT_C; //TODO: da controllare
                                        headerNewShipment.internalNote = "";

                                        var stop = new StopNewShipmentTMS();

                                        stop.address = "VIA PALMANOVA 71";
                                        stop.country = "IT";
                                        stop.description = $"GUNA S.P.A.";
                                        stop.district = "MI";
                                        stop.zipCode = "20132";
                                        stop.location = "MILANO";
                                        stop.date = DateTime.Now.ToString("yyyy-MM-dd");
                                        stop.type = "P";
                                        //stop.region = Helper.GetRegionByZipCode(ShipGuna.PTCODE);
                                        stop.time = "";

                                        destinazione.Add(stop);

                                        var stop2 = new StopNewShipmentTMS();

                                        stop2.address = ShipGuna.ADDRESS.Replace("\"", "");
                                        stop2.country = ShipGuna.COUNTRY;
                                        stop2.description = $"{ShipGuna.RAGSOC1.Trim().Replace("\"", "")}{ShipGuna.RAGSOC2.Trim().Replace("\"", "")}";
                                        stop2.district = ShipGuna.REGION;
                                        stop2.zipCode = ShipGuna.PTCODE;
                                        stop2.location = ShipGuna.CITY;
                                        stop2.type = "D";
                                        //stop2.region = Helper.GetRegionByZipCode(ShipGuna.PTCODE);


                                        destinazione.Add(stop2);

                                        var goods = new GoodNewShipmentTMS();

                                        goods.grossWeight = Helper.GetDecimalFromString(ShipGuna.PESO, 3);
                                        goods.cube = Helper.GetDecimalFromString(ShipGuna.VOLUME, 3);
                                        goods.packs = int.Parse(ShipGuna.NCOLLI);


                                        merce.Add(goods);

                                        foreach (var s in ShipGuna.Segnacolli)
                                        {
                                            var ss = new ParcelNewShipmentTMS()
                                            {
                                                barcodeExt = s
                                            };
                                            parcelNewShipment.Add(ss);
                                        }

                                        shipmentTMS.header = headerNewShipment;
                                        shipmentTMS.parcels = parcelNewShipment.ToArray();
                                        shipmentTMS.goods = merce.ToArray();
                                        shipmentTMS.stops = destinazione.ToArray();
                                        ListShip.Add(shipmentTMS);
                                        //InviaNuovaShipmentAPI_UNITEX(shipmentTMS);
                                        testataTrovata = false;
                                        i--;
                                    }
                                }
                                else//sono segnacolli
                                {
                                    ShipGuna.Segnacolli.Add(r.Substring(1, r.Length).Trim());
                                }
                            }
                            else
                            {
                                //ultimo segnacollo, colleziona ed invia ad api unitex la nuova spedizione
                                ShipGuna.Segnacolli.Add(r.Substring(1, r.Length).Trim());

                                //invia ad api unitex la spedizione e reinizializza la testata
                                var headerNewShipment = new HeaderNewShipmentTMS()
                                {
                                    docDate = DateTime.Parse(ShipGuna.AASPED + ShipGuna.MMGGSPED).ToString("o")
                                };
                                foreach (var s in ShipGuna.Segnacolli)
                                {
                                    var ss = new ParcelNewShipmentTMS()
                                    {
                                        barcodeExt = s
                                    };
                                    parcelNewShipment.Add(ss);
                                }
                                shipmentTMS.header = headerNewShipment;
                                shipmentTMS.parcels = parcelNewShipment.ToArray();
                                ListShip.Add(shipmentTMS);
                                //InviaNuovaShipmentAPI_UNITEX(shipmentTMS);
                            }
                        }
                        catch (Exception ee)
                        {
                            _loggerCode.Error(ee);
                        }
                    }
                }
            }
            else if (cust == CustomerConnections.Logistica93)
            {

                var pzFr = fr.Split('_');
                bool isVichi = pzFr[1] == "ZCAI";
                var fInfo = new FileInfo(fr);
                var files = Directory.GetFiles(Path.GetDirectoryName(fr));
                if (files.Count() < 2)
                {
                    fileProcessati.Remove(fr);
                    _loggerCode.Debug($"non c'è il file collegato");
                }
                //if (DateTime.Now - fInfo.CreationTime < TimeSpan.FromMinutes(30))
                //{
                //    fileProcessati.Remove(fr);
                //    _loggerCode.Debug($"aspetto per processarlo");
                //}
                //FBEXX_ZLCA_69957_741960
                //FSE_ZLCA_69957_741960.TXT
                var corretto = files.Where(x => x.Contains("FSE") && x.Contains(pzFr[2]) && x.Contains(pzFr[3].Split('.')[0])).FirstOrDefault();

                var tutteLeRighe = File.ReadAllLines(fr);//Encoding.Default.GetString(File.ReadAllBytes(fr)).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var FileSegnacolli = File.ReadAllLines(corretto);//Encoding.Unicode.GetString(File.ReadAllBytes(corretto)).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                fileProcessati.Add(corretto);

                //if (isVichi)
                //{
                //    tutteLeRighe = AnalizzaRaggruppate(tutteLeRighe);
                //}

                var ListShipLoreal = new List<Logistica93_ShipmentIN>();

                for (int i = 0; i < tutteLeRighe.Count(); i++)
                {
                    //try
                    //{
                    var rigaFile = tutteLeRighe[i];
                    iiDebug++;
                    Debug.WriteLine(iiDebug);

                    var nl = new Logistica93_ShipmentIN();
                    #region InterpretaTestata
                    nl.TipoRecord = rigaFile.Substring(nl.idxTipoRecord[0], nl.idxTipoRecord[1]).Trim();
                    nl.NumeroBorderau = rigaFile.Substring(nl.idxNumeroBorderau[0], nl.idxNumeroBorderau[1]).Trim();
                    nl.DataSpedizione = rigaFile.Substring(nl.idxDataSpedizione[0], nl.idxDataSpedizione[1]).Trim();
                    nl.NumeroDDT = rigaFile.Substring(nl.idxNumeroDDT[0], nl.idxNumeroDDT[1]).Trim();
                    nl.DataDDT = rigaFile.Substring(nl.idxDataDDT[0], nl.idxDataDDT[1]).Trim();
                    nl.RifNConsegna = rigaFile.Substring(nl.idxRifNConsegna[0], nl.idxRifNConsegna[1]).Trim();
                    nl.LuogoSpedizione = rigaFile.Substring(nl.idxLuogoSpedizione[0], nl.idxLuogoSpedizione[1]).Trim();
                    nl.PesoDelivery = rigaFile.Substring(nl.idxPesoDelivery[0], nl.idxPesoDelivery[1]).Trim();
                    nl.TipoCliente = rigaFile.Substring(nl.idxTipoCliente[0], nl.idxTipoCliente[1]).Trim();
                    nl.Destinatario = rigaFile.Substring(nl.idxDestinatario[0], nl.idxDestinatario[1]).Trim();
                    nl.Indirizzo = rigaFile.Substring(nl.idxIndirizzo[0], nl.idxIndirizzo[1]).Trim();
                    nl.Localita = rigaFile.Substring(nl.idxLocalita[0], nl.idxLocalita[1]).Trim();
                    nl.CAP = rigaFile.Substring(nl.idxCAP[0], nl.idxCAP[1]).Trim();
                    nl.SiglaProvDestinazione = rigaFile.Substring(nl.idxSiglaProvDestinazione[0], nl.idxSiglaProvDestinazione[1]).Trim();
                    nl.PIVA_CODF = rigaFile.Substring(nl.idxPIVA_CODF[0], nl.idxPIVA_CODF[1]).Trim();
                    nl.DataConsegna = rigaFile.Substring(nl.idxDataConsegna[0], nl.idxDataConsegna[1]).Trim();
                    nl.TipoDataConsegna = rigaFile.Substring(nl.idxTipoDataConsegna[0], nl.idxTipoDataConsegna[1]).Trim();
                    nl.TipoSpedizione = rigaFile.Substring(nl.idxTipoSpedizione[0], nl.idxTipoSpedizione[1]).Trim();
                    nl.ImportoContrassegno = rigaFile.Substring(nl.idxImportoContrassegno[0], nl.idxImportoContrassegno[1]).Trim();
                    nl.NotaModalitaDiConsegna = rigaFile.Substring(nl.idxNotaModalitaDiConsegna[0], nl.idxNotaModalitaDiConsegna[1]).Trim();
                    nl.NotaCommentiTempiConsegna = rigaFile.Substring(nl.idxNotaCommentiTempiConsegna[0], nl.idxNotaCommentiTempiConsegna[1]).Trim();
                    nl.NotaEPAL = rigaFile.Substring(nl.idxNotaEPAL[0], nl.idxNotaEPAL[1]).Trim();
                    nl.NotaBolla = rigaFile.Substring(nl.idxNotaBolla[0], nl.idxNotaBolla[1]).Trim();
                    nl.NumeroColliDettaglio = rigaFile.Substring(nl.idxNumeroColliDettaglio[0], nl.idxNumeroColliDettaglio[1]).Trim();
                    nl.NumeroColliStandard = rigaFile.Substring(nl.idxNumeroColliStandard[0], nl.idxNumeroColliStandard[1]).Trim();
                    nl.NumeroEspositoriPLV = rigaFile.Substring(nl.idxNumeroEspositoriPLV[0], nl.idxNumeroEspositoriPLV[1]).Trim();
                    nl.NumeroPedane = rigaFile.Substring(nl.idxNumeroPedane[0], nl.idxNumeroPedane[1]).Trim();
                    nl.CodiceCorriere = rigaFile.Substring(nl.idxCodiceCorriere[0], nl.idxCodiceCorriere[1]).Trim();
                    nl.ItinerarioCorriere = rigaFile.Substring(nl.idxItinerarioCorriere[0], nl.idxItinerarioCorriere[1]).Trim();
                    nl.SottoZonaCorriere = rigaFile.Substring(nl.idxSottoZonaCorriere[0], nl.idxSottoZonaCorriere[1]).Trim();
                    nl.NumeroPedaneEPAL = rigaFile.Substring(nl.idxNumeroPedaneEPAL[0], nl.idxNumeroPedaneEPAL[1]).Trim();
                    nl.TipoTrasporto = rigaFile.Substring(nl.idxTipoTrasporto[0], nl.idxTipoTrasporto[1]).Trim();
                    nl.ZonaCorriere = rigaFile.Substring(nl.idxZonaCorriere[0], nl.idxZonaCorriere[1]).Trim();
                    nl.PedanaDirezionale = rigaFile.Substring(nl.idxPedanaDirezionale[0], nl.idxPedanaDirezionale[1]).Trim();
                    nl.CodiceAbbinamento = rigaFile.Substring(nl.idxCodiceAbbinamento[0], nl.idxCodiceAbbinamento[1]).Trim();
                    nl.NumeroOrdineCliente = rigaFile.Substring(nl.idxNumeroOrdineCliente[0], nl.idxNumeroOrdineCliente[1]).Trim();
                    nl.ContrattoCorriere = rigaFile.Substring(nl.idxContrattoCorriere[0], nl.idxContrattoCorriere[1]).Trim();
                    nl.Via3 = rigaFile.Substring(nl.idxVia3[0], nl.idxVia3[1]).Trim();
                    nl.NumeroFattura = rigaFile.Substring(nl.idxNumeroFattura[0], nl.idxNumeroFattura[1]).Trim();
                    nl.PesoPolveri = rigaFile.Substring(nl.idxPesoPolveri[0], nl.idxPesoPolveri[1]).Trim();
                    nl.NumeroFiliale = rigaFile.Substring(nl.idxNumeroFiliale[0], nl.idxNumeroFiliale[1]).Trim();
                    nl.TipoClienteIntestazione = rigaFile.Substring(nl.idxTipoClienteIntestazione[0], nl.idxTipoClienteIntestazione[1]).Trim();
                    nl.DestinatarioFiliale = rigaFile.Substring(nl.idxDestinatarioFiliale[0], nl.idxDestinatarioFiliale[1]).Trim();
                    nl.IndirizzoFiliale = rigaFile.Substring(nl.idxIndirizzoFiliale[0], nl.idxIndirizzoFiliale[1]).Trim();
                    nl.LocalitaFiliale = rigaFile.Substring(nl.idxLocalitaFiliale[0], nl.idxLocalitaFiliale[1]).Trim();
                    nl.CAPFiliale = rigaFile.Substring(nl.idxCAPFiliale[0], nl.idxCAPFiliale[1]).Trim();
                    nl.SiglaProvDestinazioneFiliale = rigaFile.Substring(nl.idxSiglaProvDestinazioneFiliale[0], nl.idxSiglaProvDestinazioneFiliale[1]).Trim();
                    nl.Filler = rigaFile.Substring(nl.idxFiller[0], nl.idxFiller[1]).Trim();
                    nl.DeliveryVolume = rigaFile.Substring(nl.idxDeliveryVolume[0], nl.idxDeliveryVolume[1]).Trim();
                    nl.VolumeUnit = rigaFile.Substring(nl.idxVolumeUnit[0], nl.idxVolumeUnit[1]).Trim();
                    nl.PrioritàConsegna = rigaFile.Substring(nl.idxPrioritàConsegna[0], nl.idxPrioritàConsegna[1]).Trim();
                    //ShipLoreal93.SMSPreavviso = r.Substring(ShipLoreal93.idxSMSPreavviso[0], ShipLoreal93.idxSMSPreavviso[1]).Trim();
                    ListShipLoreal.Add(nl);
                    #endregion
                }
                List<Logistica93_ShipmentIN> ListaRaggruppataLoreal = RaggruppaTestateLoreal(ListShipLoreal);

                foreach (var ShipLoreal93 in ListaRaggruppataLoreal)
                {
                    RootobjectNewShipmentTMS shipmentTMS = new RootobjectNewShipmentTMS();
                    List<ParcelNewShipmentTMS> parcelNewShipment = new List<ParcelNewShipmentTMS>();
                    List<StopNewShipmentTMS> destinazione = new List<StopNewShipmentTMS>();
                    List<GoodNewShipmentTMS> merce = new List<GoodNewShipmentTMS>();

                    #region TipoSpedizione
                    string incoterm = "";
                    if (ShipLoreal93.TipoSpedizione == "F")//Porto franco
                    {
                        incoterm = "PF";
                    }
                    else if (ShipLoreal93.TipoSpedizione == "C")//Conto Servizio
                    {
                        incoterm = "CS";
                    }
                    else// == A //Porto assegnato
                    {
                        incoterm = "PA";
                    }
                    #endregion

                    #region Tassativa
                    string unloadDateType = "";
                    string unloadDate = "";
                    //TODO: non so come mandare il tipo data di consegna tramite api
                    bool isTassativa = false;
                    if (!string.IsNullOrEmpty(ShipLoreal93.DataConsegna))
                    {
                        unloadDate = DateTime.ParseExact(ShipLoreal93.DataConsegna, "yyyyMMdd", null).ToString("MM-dd-yyyy");

                        if (ShipLoreal93.TipoDataConsegna.Trim().ToUpper() == "TASSATIVA")
                        {
                            isTassativa = true;
                        }
                        else if (ShipLoreal93.TipoDataConsegna.Trim().ToUpper() == "ENTRO IL")
                        {
                            isTassativa = true;
                        }
                        else if (ShipLoreal93.TipoDataConsegna.Trim().ToUpper() == "DOPO IL")
                        {
                            isTassativa = true;
                        }

                    }
                    else
                    {
                        //stabilisti ETA
                        string localita = ShipLoreal93.Localita.ToLower().Trim();
                        string cap = ShipLoreal93.Localita.ToLower().Trim();
                        string hub = "02";
                        unloadDate = StabilisciETA(localita, cap, hub);

                    }
                    #endregion

                    #region TipoTrasporto
                    string serviceType = "";
                    if (ShipLoreal93.TipoTrasporto == "ZCOR")
                    {
                        serviceType = "S";
                    }
                    else if (ShipLoreal93.TipoTrasporto == "ZESP")
                    {
                        serviceType = "P";
                    }
                    else if (ShipLoreal93.TipoTrasporto == "ZDIR")
                    {
                        serviceType = "D";
                    }
                    else if (ShipLoreal93.TipoTrasporto == "ZAGE")
                    {
                        serviceType = "AGE";
                    }
                    else if (ShipLoreal93.TipoTrasporto == "ZCOM")
                    {
                        serviceType = "COM";
                    }
                    else if (ShipLoreal93.TipoTrasporto == "ZINF")
                    {
                        serviceType = "INF";
                    }
                    else if (ShipLoreal93.TipoTrasporto == "ZMKT")
                    {
                        serviceType = "MKT";
                    }
                    else if (ShipLoreal93.TipoTrasporto == "ZTRD")
                    {
                        serviceType = "TRD";
                    }
                    else
                    {
                        serviceType = "S";
                    }
                    #endregion

                    #region DatiTestata
                    var headerNewShipment = new HeaderNewShipmentTMS()
                    {
                        carrierType = "EDI",//"COLLO", // non spediscono mai pallet?
                        serviceType = serviceType,
                        incoterm = incoterm,
                        transportType = "8-25",
                        type = "Groupage",
                        insideRef = ShipLoreal93.NumeroDDT, //Server per il tracciato di Esiti
                        internalNote = ShipLoreal93.NotaCommentiTempiConsegna + " " + ShipLoreal93.NotaBolla + " " + ShipLoreal93.NotaEPAL,
                        externRef = ShipLoreal93.RifNConsegna,  //Server per il tracciato di Esiti
                        publicNote = ShipLoreal93.NotaModalitaDiConsegna + " " + ShipLoreal93.NotaEPAL,
                        docDate = ShipLoreal93.DataDDT,
                        customerID = cust.ID_GESPE,
                        cashCurrency = "EUR",
                        cashValue = Helper.GetDecimalFromString(ShipLoreal93.ImportoContrassegno, 2),
                        cashPayment = "",
                        cashNote = "",
                    };
                    #endregion

                    #region IdentificaLorealVichi
                    //RifNConsegna sarebbe uguale al NumeroDDT in testata
                    List<string> tuttiISegnacolliDellaSpedizione = new List<string>();
                    if (isVichi)
                    {
                        tuttiISegnacolliDellaSpedizione = FileSegnacolli.Where(x => x.Substring(10, 10) == ShipLoreal93.NumeroDDT).ToList();
                    }
                    else
                    {
                        tuttiISegnacolliDellaSpedizione = FileSegnacolli.Where(x => x.Substring(10, 10) == ShipLoreal93.RifNConsegna).ToList();
                    }
                    #endregion

                    #region SegnacolliEDettaglioMerce
                    List<LorealSegnacolli> dettaglioMerce = new List<LorealSegnacolli>();
                    foreach (var s in tuttiISegnacolliDellaSpedizione)
                    {
                        var segnaCollo = new LorealSegnacolli();
                        //segnaCollo.S_BoxBREIT = s.Substring(segnaCollo.idxS_BoxBREIT[0], segnaCollo.idxS_BoxBREIT[1]).Trim();
                        //segnaCollo.S_BoxHOEHE = s.Substring(segnaCollo.idxS_BoxHOEHE[0], segnaCollo.idxS_BoxHOEHE[1]).Trim();
                        //segnaCollo.S_BoxLAENG = s.Substring(segnaCollo.idxS_BoxLAENG[0], segnaCollo.idxS_BoxLAENG[1]).Trim();
                        segnaCollo.S_CheckDigit = s.Substring(segnaCollo.idxS_CheckDigit[0], segnaCollo.idxS_CheckDigit[1]).Trim();
                        segnaCollo.S_CodiceCliente = s.Substring(segnaCollo.idxS_CodiceCliente[0], segnaCollo.idxS_CodiceCliente[1]).Trim();
                        segnaCollo.S_CodiceProdotto = s.Substring(segnaCollo.idxS_CodiceProdotto[0], segnaCollo.idxS_CodiceProdotto[1]).Trim();
                        segnaCollo.S_DescrizioneProdotto = s.Substring(segnaCollo.idxS_DescrizioneProdotto[0], segnaCollo.idxS_DescrizioneProdotto[1]).Trim();
                        //segnaCollo.S_Filler1 = s.Substring(segnaCollo.idxS_Filler1[0], segnaCollo.idxS_Filler1[1]).Trim();
                        //segnaCollo.S_Filler2 = s.Substring(segnaCollo.idxS_Filler2[0], segnaCollo.idxS_Filler2[1]).Trim();
                        //segnaCollo.S_Filler3 = s.Substring(segnaCollo.idxS_Filler3[0], segnaCollo.idxS_Filler3[1]).Trim();
                        segnaCollo.S_Marca = s.Substring(segnaCollo.idxS_Marca[0], segnaCollo.idxS_Marca[1]).Trim();
                        segnaCollo.S_NumeroCollo = s.Substring(segnaCollo.idxS_NumeroCollo[0], segnaCollo.idxS_NumeroCollo[1]).Trim();
                        segnaCollo.S_NumeroDDT = s.Substring(segnaCollo.idxS_NumeroDDT[0], segnaCollo.idxS_NumeroDDT[1]).Trim();
                        segnaCollo.S_Peso = s.Substring(segnaCollo.idxS_Peso[0], segnaCollo.idxS_Peso[1]).Trim();
                        segnaCollo.S_TipoElaborazione = s.Substring(segnaCollo.idxS_TipoElaborazione[0], segnaCollo.idxS_TipoElaborazione[1]).Trim();
                        segnaCollo.S_TipoImballo = s.Substring(segnaCollo.idxS_TipoImballo[0], segnaCollo.idxS_TipoImballo[1]).Trim();
                        segnaCollo.S_TipoImballoMagazzino = s.Substring(segnaCollo.idxS_TipoImballoMagazzino[0], segnaCollo.idxS_TipoImballoMagazzino[1]).Trim();
                        segnaCollo.S_ZonaSpedizione = s.Substring(segnaCollo.idxS_ZonaSpedizione[0], segnaCollo.idxS_ZonaSpedizione[1]).Trim();
                        //segnaCollo.S_DimensionUnit = s.Substring(segnaCollo.idxS_DimensionUnit[0], segnaCollo.idxS_DimensionUnit[1]).Trim();
                        dettaglioMerce.Add(segnaCollo);

                    }
                    int iiDBG = 0;
                    foreach (var sc in dettaglioMerce)
                    {
                        iiDBG++;
                        Debug.WriteLine(iiDBG);

                        var goods = new GoodNewShipmentTMS();
                        {
                            //inserire specifiche dei colli
                            goods.description = sc.S_DescrizioneProdotto;
                            goods.depth = Helper.GetDecimalFromString(sc.S_BoxBREIT, 3);
                            goods.height = Helper.GetDecimalFromString(sc.S_BoxHOEHE, 3);
                            goods.width = Helper.GetDecimalFromString(sc.S_BoxLAENG, 3);
                            goods.grossWeight = Helper.GetDecimalFromString(sc.S_Peso, 3);
                            goods.packs = 1;
                        }

                        var parcel = new ParcelNewShipmentTMS()
                        {
                            barcodeExt = $"00{sc.S_NumeroCollo}"
                        };

                        parcelNewShipment.Add(parcel);
                        merce.Add(goods);
                    }
                    var pesoDelivery = Helper.GetDecimalFromString(ShipLoreal93.PesoDelivery, 2);//merce.Sum(x => x.grossWeight); 
                    var colliDelivery = int.Parse(ShipLoreal93.NumeroColliDettaglio) + int.Parse(ShipLoreal93.NumeroColliStandard);
                    var pedaneDelivery = int.Parse(ShipLoreal93.NumeroPedane);
                    #endregion

                    #region CaricoEScarico
                    var stop = new StopNewShipmentTMS()
                    {

                        address = "VIA PRIMATICCIO 155",//ShipLoreal93.IndirizzoFiliale,
                        country = "IT",
                        description = "L'OREAL ITALIA SPA",//ShipLoreal93.DestinatarioFiliale.Trim(),
                        district = "MI",//ShipLoreal93.SiglaProvDestinazioneFiliale,
                        zipCode = "20147",//ShipLoreal93.CAPFiliale,
                        location = "MILANO",//ShipLoreal93.LocalitaFiliale,
                        date = DateTime.Now.ToString("yyyy-MM-dd"),
                        type = "P",
                        region = "Lombardia",
                        time = "",
                    };
                    destinazione.Add(stop);

                    var regione = GeoTab.FirstOrDefault(x => x.cap == ShipLoreal93.CAP);
                    var stop2 = new StopNewShipmentTMS()
                    {

                        address = ShipLoreal93.Indirizzo.Replace("\"", ""),
                        country = "IT",
                        description = ShipLoreal93.Destinatario.Trim().Replace("\"", ""),
                        district = ShipLoreal93.SiglaProvDestinazione.Trim(),
                        zipCode = ShipLoreal93.CAP.Trim(),
                        location = ShipLoreal93.Localita.Trim(),
                        date = unloadDate, //UnloadDate già considera l'ETA
                        type = "D",
                        region = regione != null ? regione.regione : "",
                        time = "",
                        obligatoryType = isTassativa ? "Date" : "Nothing"
                    };

                    destinazione.Add(stop2);
                    #endregion

                    merce[0].cube = Helper.GetDecimalFromString(ShipLoreal93.DeliveryVolume, 3);
                    shipmentTMS.header = headerNewShipment;
                    shipmentTMS.parcels = parcelNewShipment.ToArray();
                    shipmentTMS.goods = merce.ToArray();
                    shipmentTMS.stops = destinazione.ToArray();
                    shipmentTMS.isTassativa = isTassativa;

                    RigheCSV.AddRange(ConvertiSpedizioneAPIinEDILoreal(shipmentTMS, cust, pesoDelivery, colliDelivery, pedaneDelivery));
                    //ListShip.Add(shipmentTMS);
                    //InviaNuovaShipmentAPI_UNITEX(shipmentTMS);
                }
                //}
                //catch (Exception ee)
                //{
                //    _loggerCode.Error(ee);
                //}



                InviaCSVAlServiceManagerByFTP(cust, RigheCSV, fr);
                return;
            }
            else if (cust == CustomerConnections.PoolPharma || cust == CustomerConnections.DLF)
            {

                var pzF = Path.GetFileNameWithoutExtension(fr).Split('_');

                bool isDLF = pzF[1] == "DLF";
                var tutteLeRighe = File.ReadAllLines(fr);
                var ShipPoolPharmaDLF = new PoolPharmaDLF_ShipmentIN();

                foreach (var r in tutteLeRighe)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(r))
                        {
                            continue;
                        }
                        iiDebug++;
                        Debug.WriteLine(iiDebug);
                        RootobjectNewShipmentTMS shipmentTMS = new RootobjectNewShipmentTMS();
                        List<ParcelNewShipmentTMS> parcelNewShipment = new List<ParcelNewShipmentTMS>();
                        List<StopNewShipmentTMS> destinazione = new List<StopNewShipmentTMS>();
                        List<GoodNewShipmentTMS> merce = new List<GoodNewShipmentTMS>();

                        ShipPoolPharmaDLF.CAPDestinatario = r.Substring(ShipPoolPharmaDLF.idxCAPDestinatario[0], ShipPoolPharmaDLF.idxCAPDestinatario[1]).Trim();
                        ShipPoolPharmaDLF.CAPDestinazione = r.Substring(ShipPoolPharmaDLF.idxCAPDestinazione[0], ShipPoolPharmaDLF.idxCAPDestinazione[1]).Trim();
                        ShipPoolPharmaDLF.CittaDestinatario = r.Substring(ShipPoolPharmaDLF.idxCittaDestinatario[0], ShipPoolPharmaDLF.idxCittaDestinatario[1]).Trim();
                        ShipPoolPharmaDLF.CittaDestinazione = r.Substring(ShipPoolPharmaDLF.idxCittaDestinazione[0], ShipPoolPharmaDLF.idxCittaDestinazione[1]).Trim();
                        ShipPoolPharmaDLF.DataBolla = r.Substring(ShipPoolPharmaDLF.idxDataBolla[0], ShipPoolPharmaDLF.idxDataBolla[1]).Trim();
                        ShipPoolPharmaDLF.ImportoContrassegno = r.Substring(ShipPoolPharmaDLF.idxImportoContrassegno[0], ShipPoolPharmaDLF.idxImportoContrassegno[1]).Trim();
                        ShipPoolPharmaDLF.IndirizzoDestinatario = r.Substring(ShipPoolPharmaDLF.idxIndirizzoDestinatario[0], ShipPoolPharmaDLF.idxIndirizzoDestinatario[1]).Trim();
                        ShipPoolPharmaDLF.IndirizzoDestinazione = r.Substring(ShipPoolPharmaDLF.idxIndirizzoDestinazione[0], ShipPoolPharmaDLF.idxIndirizzoDestinazione[1]).Trim();
                        ShipPoolPharmaDLF.MerceFragile = r.Substring(ShipPoolPharmaDLF.idxMerceFragile[0], ShipPoolPharmaDLF.idxMerceFragile[1]).Trim();
                        ShipPoolPharmaDLF.NazioneDestinatario = r.Substring(ShipPoolPharmaDLF.idxNazioneDestinatario[0], ShipPoolPharmaDLF.idxNazioneDestinatario[1]).Trim();
                        ShipPoolPharmaDLF.Note = r.Substring(ShipPoolPharmaDLF.idxNote[0], ShipPoolPharmaDLF.idxNote[1]).Trim();
                        ShipPoolPharmaDLF.Note1 = r.Substring(ShipPoolPharmaDLF.idxNote1[0], ShipPoolPharmaDLF.idxNote1[1]).Trim();
                        ShipPoolPharmaDLF.Note2 = r.Substring(ShipPoolPharmaDLF.idxNote2[0], ShipPoolPharmaDLF.idxNote2[1]).Trim();
                        ShipPoolPharmaDLF.NumeroColli = r.Substring(ShipPoolPharmaDLF.idxNumeroColli[0], ShipPoolPharmaDLF.idxNumeroColli[1]).Trim();
                        ShipPoolPharmaDLF.NumeroDocumento = r.Substring(ShipPoolPharmaDLF.idxNumeroDocumento[0], ShipPoolPharmaDLF.idxNumeroDocumento[1]).Trim();
                        ShipPoolPharmaDLF.Peso = r.Substring(ShipPoolPharmaDLF.idxPeso[0], ShipPoolPharmaDLF.idxPeso[1]).Trim();
                        ShipPoolPharmaDLF.ProvDestinatario = r.Substring(ShipPoolPharmaDLF.idxProvDestinatario[0], ShipPoolPharmaDLF.idxProvDestinatario[1]).Trim();
                        ShipPoolPharmaDLF.ProvDestinazione = r.Substring(ShipPoolPharmaDLF.idxProvDestinazione[0], ShipPoolPharmaDLF.idxProvDestinazione[1]).Trim();
                        ShipPoolPharmaDLF.RagioneSocialeDestinatario = r.Substring(ShipPoolPharmaDLF.idxRagioneSocialeDestinatario[0], ShipPoolPharmaDLF.idxRagioneSocialeDestinatario[1]).Trim();
                        ShipPoolPharmaDLF.RagioneSocialeDestinazione = r.Substring(ShipPoolPharmaDLF.idxRagioneSocialeDestinazione[0], ShipPoolPharmaDLF.idxRagioneSocialeDestinazione[1]).Trim();
                        ShipPoolPharmaDLF.RiferimentoEsterno = r.Substring(ShipPoolPharmaDLF.idxRiferimentoEsterno[0], ShipPoolPharmaDLF.idxRiferimentoEsterno[1]).Trim();
                        ShipPoolPharmaDLF.TemperaturaControllata = r.Substring(ShipPoolPharmaDLF.idxTemperaturaControllata[0], ShipPoolPharmaDLF.idxTemperaturaControllata[1]).Trim();
                        ShipPoolPharmaDLF.TemperaturaMinoreDi25 = r.Substring(ShipPoolPharmaDLF.idxTemperaturaMinoreDi25[0], ShipPoolPharmaDLF.idxTemperaturaMinoreDi25[1]).Trim();

                        string localita = ShipPoolPharmaDLF.CittaDestinatario.ToLower().Trim();
                        string cap = ShipPoolPharmaDLF.CAPDestinatario.ToLower().Trim();
                        string hub = "02";
                        string unloadDate = StabilisciETA(localita, cap, hub);

                        var headerNewShipment = new HeaderNewShipmentTMS()
                        {
                            docDate = DateTime.ParseExact(ShipPoolPharmaDLF.DataBolla, "yyyyMMdd", null).ToString("MM-dd-yyyy"),
                            publicNote = $"{ShipPoolPharmaDLF.Note.Trim()} {ShipPoolPharmaDLF.Note1.Trim()} {ShipPoolPharmaDLF.Note2.Trim()}".Trim(),
                            customerID = cust.ID_GESPE,
                            cashCurrency = "EUR",
                            cashValue = Helper.GetDecimalFromString(ShipPoolPharmaDLF.ImportoContrassegno, 2),
                            externRef = ShipPoolPharmaDLF.NumeroDocumento,
                            carrierType = "COLLO",//carrierType = "COLLO",
                            serviceType = "S",
                            incoterm = "PF",
                            transportType = ShipPoolPharmaDLF.TemperaturaControllata == "Y" ? "2-8" : "8-25",
                            type = "Groupage",
                            cashNote = "",
                            insideRef = ShipPoolPharmaDLF.RiferimentoEsterno,
                            internalNote = "",
                            cashPayment = ""
                        };

                        var stop = new StopNewShipmentTMS();
                        if (isDLF)
                        {
                            stop.address = "VIA BASILICATA 9 FRAZ SESTO ULTERIANO";
                            stop.country = "IT";
                            stop.description = "D.L.F. SPA";
                            stop.district = "MI";
                            stop.zipCode = "20098";
                            stop.location = "San Giuliano Milanese";
                            stop.date = DateTime.Now.ToString("yyyy-MM-dd");
                            stop.type = "P";
                            stop.time = "";
                        }
                        else
                        {
                            stop.address = "VIA BASILICATA 9";
                            stop.country = "IT";
                            stop.description = "POOL PHARMA SRL";
                            stop.district = "MI";
                            stop.zipCode = "20098";
                            stop.location = "San Giuliano Milanese";
                            stop.date = DateTime.Now.ToString("yyyy-MM-dd");
                            stop.type = "P";
                            stop.time = "";
                        }
                        destinazione.Add(stop);

                        var stop2 = new StopNewShipmentTMS()
                        {

                            address = ShipPoolPharmaDLF.IndirizzoDestinazione,
                            country = ShipPoolPharmaDLF.NazioneDestinazione,
                            description = ShipPoolPharmaDLF.RagioneSocialeDestinazione.Trim(),
                            district = ShipPoolPharmaDLF.ProvDestinazione,
                            zipCode = ShipPoolPharmaDLF.CAPDestinazione,
                            location = ShipPoolPharmaDLF.CittaDestinazione,
                            type = "D",
                            //region = Helper.GetRegionByZipCode(ShipPoolPharmaDLF.CAPDestinazione),
                            time = "",
                            date = unloadDate
                        };
                        destinazione.Add(stop2);


                        var goods = new GoodNewShipmentTMS()
                        {
                            grossWeight = Helper.GetDecimalFromString(ShipPoolPharmaDLF.Peso, 0),
                            packs = int.Parse(ShipPoolPharmaDLF.NumeroColli),
                        };

                        merce.Add(goods);

                        shipmentTMS.header = headerNewShipment;
                        shipmentTMS.goods = merce.ToArray();
                        shipmentTMS.stops = destinazione.ToArray();
                        ListShip.Add(shipmentTMS);
                        //InviaNuovaShipmentAPI_UNITEX(shipmentTMS);
                    }
                    catch (Exception ee)
                    {
                        _loggerCode.Error(ee);
                    }
                }
            }
            else if (cust == CustomerConnections.STMGroup)
            {
                //Credo non gli passino i segnacolli
                //Da rivedere tipo consegna
                //Note da rivedere

                //TODO: ATTENZIONE QUANDO ESCE CANCELLA IL FILE!!!!
                if (fr.ToLower().EndsWith("_c.txt"))
                {
                    return;
                }


                var tutteLeRighe = File.ReadAllLines(fr);
                var tuttiIFile = Directory.GetFiles(Path.GetDirectoryName(fr));
                var fn = Path.GetFileName(fr).Split('.')[0];
                var rr = tuttiIFile.FirstOrDefault(x => x.Contains(fn) && x.ToLower().EndsWith("_c.txt"));
                var relativiSegnacolli = File.ReadAllLines(rr);
                var ShipSTM = new STMGroup_ShipmentIN();

                foreach (var r in tutteLeRighe)
                {
                    try
                    {
                        iiDebug++;
                        Debug.WriteLine(iiDebug);
                        RootobjectNewShipmentTMS shipmentTMS = new RootobjectNewShipmentTMS();
                        List<ParcelNewShipmentTMS> parcelNewShipment = new List<ParcelNewShipmentTMS>();
                        List<StopNewShipmentTMS> destinazione = new List<StopNewShipmentTMS>();
                        List<GoodNewShipmentTMS> merce = new List<GoodNewShipmentTMS>();

                        if (string.IsNullOrEmpty(r) || r.Length < 573) //cosa sono i file che finiscono con C? colli? es: LOMBAR_C.txt
                        {
                            continue;
                        }
                        Debug.WriteLine($"\r\n---------------------------------------------------------------\r\n{r}\r\n---------------------------------------------------------------------\r\n");

                        #region Interpreta Testata
                        ShipSTM.AnnoDiRitiro = r.Substring(ShipSTM.idxDKABO[0], ShipSTM.idxDKABO[1]).Trim();
                        ShipSTM.AnnoBollettazione = r.Substring(ShipSTM.idxDKANB[0], ShipSTM.idxDKANB[1]).Trim();
                        ShipSTM.NumeroBancali = r.Substring(ShipSTM.idxDKBAN[0], ShipSTM.idxDKBAN[1]).Trim();
                        ShipSTM.CodiceCorriere = r.Substring(ShipSTM.idxDKBOR[0], ShipSTM.idxDKBOR[1]).Trim();
                        ShipSTM.CodiceVettoreRitiro = r.Substring(ShipSTM.idxDKCDR[0], ShipSTM.idxDKCDR[1]).Trim();
                        ShipSTM.GiornoChiusura = r.Substring(ShipSTM.idxDKCHI[0], ShipSTM.idxDKCHI[1]).Trim();
                        ShipSTM.CampoLibero = r.Substring(ShipSTM.idxDKCNM[0], ShipSTM.idxDKCNM[1]).Trim();
                        ShipSTM.FasciaSegnacolli = r.Substring(ShipSTM.idxDKCOD[0], ShipSTM.idxDKCOD[1]).Trim();
                        ShipSTM.Colli = r.Substring(ShipSTM.idxDKCOL[0], ShipSTM.idxDKCOL[1]).Trim();
                        ShipSTM.DKDAE = r.Substring(ShipSTM.idxDKDAE[0], ShipSTM.idxDKDAE[1]).Trim();
                        ShipSTM.Raggruppamento = r.Substring(ShipSTM.idxDKDBO[0], ShipSTM.idxDKDBO[1]).Trim();
                        ShipSTM.CapDestinatario = r.Substring(ShipSTM.idxDKDCP[0], ShipSTM.idxDKDCP[1]).Trim();
                        ShipSTM.DataConsegnaTassativa = r.Substring(ShipSTM.idxDKDCV[0], ShipSTM.idxDKDCV[1]).Trim();
                        ShipSTM.DivisaContrassegno = r.Substring(ShipSTM.idxDKDI1[0], ShipSTM.idxDKDI1[1]).Trim();
                        ShipSTM.DivisaAnticipata = r.Substring(ShipSTM.idxDKDI2[0], ShipSTM.idxDKDI2[1]).Trim();
                        ShipSTM.DivisaValoreMerce = r.Substring(ShipSTM.idxDKDI3[0], ShipSTM.idxDKDI3[1]).Trim();
                        ShipSTM.DKDIF = r.Substring(ShipSTM.idxDKDIF[0], ShipSTM.idxDKDIF[1]).Trim();
                        ShipSTM.IndirizzoDestinatario = r.Substring(ShipSTM.idxDKDIN[0], ShipSTM.idxDKDIN[1]).Trim();
                        ShipSTM.RagioneSocialeDestinatario = r.Substring(ShipSTM.idxDKDIT[0], ShipSTM.idxDKDIT[1]).Trim();
                        ShipSTM.LocalitaDestinatario = r.Substring(ShipSTM.idxDKDLO[0], ShipSTM.idxDKDLO[1]).Trim();
                        ShipSTM.CodiceMezzo = r.Substring(ShipSTM.idxDKDNM[0], ShipSTM.idxDKDNM[1]).Trim();
                        ShipSTM.SiglaPartDest = r.Substring(ShipSTM.idxDKDPR[0], ShipSTM.idxDKDPR[1]).Trim();
                        ShipSTM.OraConsegna = r.Substring(ShipSTM.idxDKDST[0], ShipSTM.idxDKDST[1]).Trim();
                        ShipSTM.DataBollaAAAAAMMGG = r.Substring(ShipSTM.idxDKDTB[0], ShipSTM.idxDKDTB[1]).Trim();
                        ShipSTM.DKDTF = r.Substring(ShipSTM.idxDKDTF[0], ShipSTM.idxDKDTF[1]).Trim();
                        ShipSTM.DataRitiro_AAAAMMGG = r.Substring(ShipSTM.idxDKDTR[0], ShipSTM.idxDKDTR[1]).Trim();
                        ShipSTM.DataXAB_AAAAMMGG = r.Substring(ShipSTM.idxDKDTX[0], ShipSTM.idxDKDTX[1]).Trim();
                        ShipSTM.DKEBO = r.Substring(ShipSTM.idxDKEBO[0], ShipSTM.idxDKEBO[1]).Trim();
                        ShipSTM.FilialeBollettazione = r.Substring(ShipSTM.idxDKFBO[0], ShipSTM.idxDKFBO[1]).Trim();
                        ShipSTM.DKFIL = r.Substring(ShipSTM.idxDKFIL[0], ShipSTM.idxDKFIL[1]).Trim();
                        ShipSTM.DKFPA = r.Substring(ShipSTM.idxDKFPA[0], ShipSTM.idxDKFPA[1]).Trim();
                        ShipSTM.NumeroAnticipata = r.Substring(ShipSTM.idxDKFTA[0], ShipSTM.idxDKFTA[1]).Trim();
                        ShipSTM.ImportoContrassegno3Dec = r.Substring(ShipSTM.idxDKIF1[0], ShipSTM.idxDKIF1[1]).Trim();
                        ShipSTM.ImportoAnticipata3Dec = r.Substring(ShipSTM.idxDKIF2[0], ShipSTM.idxDKIF2[1]).Trim();
                        ShipSTM.ImportoValoreMerce3Dec = r.Substring(ShipSTM.idxDKIF3[0], ShipSTM.idxDKIF3[1]).Trim();
                        ShipSTM.DKITF = r.Substring(ShipSTM.idxDKITF[0], ShipSTM.idxDKITF[1]).Trim();
                        ShipSTM.DKKBO = r.Substring(ShipSTM.idxDKKBO[0], ShipSTM.idxDKKBO[1]).Trim();
                        ShipSTM.ProgressivoChiave = r.Substring(ShipSTM.idxDKKEY[0], ShipSTM.idxDKKEY[1]).Trim();
                        ShipSTM.MagazzinoDiPartenza = r.Substring(ShipSTM.idxDKMAE[0], ShipSTM.idxDKMAE[1]).Trim();
                        ShipSTM.ContrattoMittente = r.Substring(ShipSTM.idxDKMCN[0], ShipSTM.idxDKMCN[1]).Trim();
                        ShipSTM.CodiceCliente = r.Substring(ShipSTM.idxDKMCO[0], ShipSTM.idxDKMCO[1]).Trim();
                        ShipSTM.CapMittente = r.Substring(ShipSTM.idxDKMCP[0], ShipSTM.idxDKMCP[1]).Trim();
                        ShipSTM.Filiale = r.Substring(ShipSTM.idxDKMFI[0], ShipSTM.idxDKMFI[1]).Trim();
                        ShipSTM.IndirizzoMittente = r.Substring(ShipSTM.idxDKMIN[0], ShipSTM.idxDKMIN[1]).Trim();
                        ShipSTM.RagioneSocialeMittente = r.Substring(ShipSTM.idxDKMIT[0], ShipSTM.idxDKMIT[1]).Trim();
                        ShipSTM.LocalitaMittente = r.Substring(ShipSTM.idxDKMLO[0], ShipSTM.idxDKMLO[1]).Trim();
                        ShipSTM.DKMPR = r.Substring(ShipSTM.idxDKMPR[0], ShipSTM.idxDKMPR[1]).Trim();
                        ShipSTM.OraViaggio = r.Substring(ShipSTM.idxDKMST[0], ShipSTM.idxDKMST[1]).Trim();
                        ShipSTM.NoteConsegna = r.Substring(ShipSTM.idxDKNOT[0], ShipSTM.idxDKNOT[1]).Trim();
                        ShipSTM.DKNRF = r.Substring(ShipSTM.idxDKNRF[0], ShipSTM.idxDKNRF[1]).Trim();
                        ShipSTM.Peso2Dec = r.Substring(ShipSTM.idxDKPES[0], ShipSTM.idxDKPES[1]).Trim();
                        ShipSTM.DKREC = r.Substring(ShipSTM.idxDKREC[0], ShipSTM.idxDKREC[1]).Trim();
                        ShipSTM.DKRM2 = r.Substring(ShipSTM.idxDKRM2[0], ShipSTM.idxDKRM2[1]).Trim();
                        ShipSTM.RifInterno = r.Substring(ShipSTM.idxDKRMI[0], ShipSTM.idxDKRMI[1]).Trim();
                        ShipSTM.RiferimentoOperatoreLogistico = r.Substring(ShipSTM.idxDKRUL[0], ShipSTM.idxDKRUL[1]).Trim();
                        ShipSTM.SegnacolloAl = r.Substring(ShipSTM.idxDKSEA[0], ShipSTM.idxDKSEA[1]).Trim();
                        ShipSTM.SegnacolloDal = r.Substring(ShipSTM.idxDKSED[0], ShipSTM.idxDKSED[1]).Trim();
                        ShipSTM.DKSOC = r.Substring(ShipSTM.idxDKSOC[0], ShipSTM.idxDKSOC[1]).Trim();
                        ShipSTM.DKT01 = r.Substring(ShipSTM.idxDKT01[0], ShipSTM.idxDKT01[1]).Trim();
                        ShipSTM.TipoServizio = r.Substring(ShipSTM.idxDKT02[0], ShipSTM.idxDKT02[1]).Trim();
                        ShipSTM.TipoConsegna = r.Substring(ShipSTM.idxDKT03[0], ShipSTM.idxDKT03[1]).Trim();
                        ShipSTM.DKT04 = r.Substring(ShipSTM.idxDKT04[0], ShipSTM.idxDKT04[1]).Trim();
                        ShipSTM.DKT05 = r.Substring(ShipSTM.idxDKT05[0], ShipSTM.idxDKT05[1]).Trim();
                        ShipSTM.DKT06 = r.Substring(ShipSTM.idxDKT06[0], ShipSTM.idxDKT06[1]).Trim();
                        ShipSTM.DKT07 = r.Substring(ShipSTM.idxDKT07[0], ShipSTM.idxDKT07[1]).Trim();
                        ShipSTM.DKT08 = r.Substring(ShipSTM.idxDKT08[0], ShipSTM.idxDKT08[1]).Trim();
                        ShipSTM.DKTCH = r.Substring(ShipSTM.idxDKTCH[0], ShipSTM.idxDKTCH[1]).Trim();
                        ShipSTM.TipoBolla = r.Substring(ShipSTM.idxDKTIP[0], ShipSTM.idxDKTIP[1]).Trim();
                        ShipSTM.DKTPA = r.Substring(ShipSTM.idxDKTPA[0], ShipSTM.idxDKTPA[1]).Trim();
                        ShipSTM.CodiceVettoreAnticipata = r.Substring(ShipSTM.idxDKVE1[0], ShipSTM.idxDKVE1[1]).Trim();
                        ShipSTM.MetriCubi2Dec = r.Substring(ShipSTM.idxDKVOL[0], ShipSTM.idxDKVOL[1]).Trim();
                        ShipSTM.DKZON = r.Substring(ShipSTM.idxDKZON[0], ShipSTM.idxDKZON[1]).Trim();

                        #endregion

                        //string unloadDateType = "";
                        string unloadDate = "";// (DateTime.Now + TimeSpan.FromDays(2)).ToString("MM-dd-yyyy");
                        DateTime Tassativa = DateTime.MinValue;
                        bool isTassativa = false;
                        if (!string.IsNullOrEmpty(ShipSTM.TipoConsegna))
                        {

                            if (ShipSTM.TipoConsegna == "1")//TASSATIVA
                            {
                                DateTime.TryParseExact(ShipSTM.DataConsegnaTassativa, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out Tassativa);
                                isTassativa = true;
                            }
                            else if (ShipSTM.TipoConsegna == "2")//prenotazione
                            {
                                DateTime.TryParseExact(ShipSTM.DataConsegnaTassativa, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out Tassativa);
                            }
                            else if (ShipSTM.TipoConsegna == "3")//entro il
                            {
                                DateTime.TryParseExact(ShipSTM.DataConsegnaTassativa, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out Tassativa);
                            }
                            else if (ShipSTM.TipoConsegna == "4")//a partire dal
                            {
                                DateTime.TryParseExact(ShipSTM.DataConsegnaTassativa, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out Tassativa);
                            }
                            else if (ShipSTM.TipoConsegna == "5")//tassativa consegna + blocco spedizione
                            {
                                DateTime.TryParseExact(ShipSTM.DataConsegnaTassativa, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out Tassativa);
                                isTassativa = true;
                            }

                        }
                        else
                        {
                            string localita = ShipSTM.LocalitaDestinatario.ToLower().Trim();
                            string cap = ShipSTM.CapDestinatario.ToLower().Trim();
                            string hub = "02";
                            unloadDate = StabilisciETA(localita, cap, hub);
                        }

                        var headerNewShipment = new HeaderNewShipmentTMS()
                        {
                            docDate = DateTime.ParseExact(ShipSTM.DataBollaAAAAAMMGG, "yyyyMMdd", null).ToString("MM-dd-yyyy"),
                            publicNote = $"{ShipSTM.NoteConsegna.Trim()}".Trim(), //altre cose da inserire?
                            customerID = cust.ID_GESPE,
                            cashCurrency = "EUR",
                            cashValue = Helper.GetDecimalFromString(ShipSTM.ImportoContrassegno3Dec, 3),
                            externRef = $"{ShipSTM.RiferimentoOperatoreLogistico}",
                            carrierType = "EDI",//int.Parse(ShipSTM.NumeroBancali) > 0 ? "PLT" : "COLLO", //TODO: chiedere conferma sulla priorità su pallet e bancali
                            serviceType = "S", //Docs: E’ da impostare solo se si desidera modificare l’automatismo attribuzione linea e zona è Vostra cura caricare valori accettabili. NON CI INTERESSA
                            incoterm = "PF",
                            transportType = "8-25",
                            type = "Groupage",
                            cashNote = "",
                            insideRef = ShipSTM.RifInterno, //Dove lo mettiamo? Serve per il tracciato di output
                            internalNote = "",
                            cashPayment = ""
                        };



                        var stop = new StopNewShipmentTMS();

                        if (ShipSTM.MagazzinoDiPartenza == "CM")
                        {
                            stop.address = "VIA RIO DEL VALLONE 20";
                            stop.country = "IT";
                            stop.description = "STM PHARMA PRO SRL - " + ShipSTM.RagioneSocialeMittente;
                            stop.district = "MI";
                            stop.zipCode = "20040";
                            stop.location = "CAMBIAGO";
                        }
                        else if (ShipSTM.MagazzinoDiPartenza == "GR")
                        {
                            stop.address = "VIA ABRUZZI SNC";
                            stop.country = "IT";
                            stop.description = "STM PHARMA PRO SRL - " + ShipSTM.RagioneSocialeMittente;
                            stop.district = "MI";
                            stop.zipCode = "20056";
                            stop.location = "GREZZAGO";
                        }
                        else if (ShipSTM.MagazzinoDiPartenza == "G1")
                        {
                            stop.address = "VIA UMBRIA 15";
                            stop.country = "IT";
                            stop.description = "STM PHARMA PRO SRL - " + ShipSTM.RagioneSocialeMittente;
                            stop.district = "MI";
                            stop.zipCode = "20056";
                            stop.location = "GREZZAGO";
                        }
                        else
                        {
                            stop.address = "VIA XXV APRILE 56";
                            stop.country = "IT";
                            stop.description = "STM PHARMA PRO SRL - " + ShipSTM.RagioneSocialeMittente;
                            stop.district = "MI";
                            stop.zipCode = "20040";
                            stop.location = "CAMBIAGO";
                        }
                        stop.date = DateTime.Now.ToString("yyyy-MM-dd");
                        stop.type = "D";
                        stop.time = "";


                        destinazione.Add(stop);

                        var stop2 = new StopNewShipmentTMS()
                        {
                            address = ShipSTM.IndirizzoDestinatario,
                            country = "IT",
                            description = ShipSTM.RagioneSocialeDestinatario.Trim(),
                            district = ShipSTM.SiglaPartDest,
                            zipCode = ShipSTM.CapDestinatario,
                            location = ShipSTM.LocalitaDestinatario,
                            date = (Tassativa != DateTime.MinValue) ? Tassativa.ToString("MM-dd-yyyy") : unloadDate,
                            type = "P",
                            //region = Helper.GetRegionByZipCode(ShipSTM.CapMittente),
                            time = "",
                            obligatoryType = Tassativa != DateTime.MinValue ? "Date" : "Nothing",
                        };
                        destinazione.Add(stop2);

                        var goods = new GoodNewShipmentTMS()
                        {
                            grossWeight = Helper.GetDecimalFromString(ShipSTM.Peso2Dec, 2),
                            cube = Helper.GetDecimalFromString(ShipSTM.MetriCubi2Dec, 2),
                            packs = int.Parse(ShipSTM.Colli),
                            totalPallet = int.Parse(ShipSTM.NumeroBancali),
                            floorPallet = int.Parse(ShipSTM.NumeroBancali)
                        };
                        merce.Add(goods);

                        var RigheColliSpedizione = relativiSegnacolli.Where(x => x.Contains(ShipSTM.RifInterno)).ToList();

                        foreach (var p in RigheColliSpedizione)
                        {
                            var sc = p.Substring(116, 14);

                            var np = new ParcelNewShipmentTMS()
                            {
                                barcodeExt = sc
                            };
                            parcelNewShipment.Add(np);
                        }

                        if (goods.packs > parcelNewShipment.Count)
                        {
                            if (goods.floorPallet == 0)
                            {
                                //goods.packs = 0;
                                goods.totalPallet = parcelNewShipment.Count;
                                goods.floorPallet = parcelNewShipment.Count;
                            }
                        }

                        shipmentTMS.header = headerNewShipment;
                        shipmentTMS.parcels = parcelNewShipment.ToArray();
                        shipmentTMS.goods = merce.ToArray();
                        shipmentTMS.stops = destinazione.ToArray();
                        shipmentTMS.isTassativa = isTassativa;
                        ListShip.Add(shipmentTMS);
                    }
                    catch (Exception ee)
                    {
                        _loggerCode.Error(ee);
                    }
                }
            }
            else if (cust == CustomerConnections.DIFARCO || cust == CustomerConnections.PHARDIS || cust == CustomerConnections.StockHouse)
            {
                //TODO: chiedere se il suffisso dei barcode dei segnacoli è sempre la sottostringa della prebolla
                var tutteLeRighe = File.ReadAllLines(fr);
                if (tutteLeRighe.Count() > 0)
                {
                    StockHouse_Shipment_IN CdGroup = new StockHouse_Shipment_IN();
                    for (int i = 0; i < tutteLeRighe.Count(); i++)
                    {
                        try
                        {
                            Debug.WriteLine(i);
                            RootobjectNewShipmentTMS shipmentTMS = new RootobjectNewShipmentTMS();
                            List<ParcelNewShipmentTMS> parcelNewShipment = new List<ParcelNewShipmentTMS>();
                            List<StopNewShipmentTMS> destinazione = new List<StopNewShipmentTMS>();
                            List<GoodNewShipmentTMS> merce = new List<GoodNewShipmentTMS>();

                            var r = tutteLeRighe[i];

                            #region decodifica
                            CdGroup.MANDANTE = r.Substring(CdGroup.idxMANDANTE[0], CdGroup.idxMANDANTE[1]).Trim();
                            CdGroup.NR_BOLLA = r.Substring(CdGroup.idxNRBOLLA[0], CdGroup.idxNRBOLLA[1]).Trim();
                            CdGroup.DATA_BOLLA = r.Substring(CdGroup.idxDATA_BOLLA[0], CdGroup.idxDATA_BOLLA[1]).Trim();
                            CdGroup.NR_SHIPMENT = r.Substring(CdGroup.idxNR_SHIPMENT[0], CdGroup.idxNR_SHIPMENT[1]).Trim();
                            CdGroup.RAG_SOC_MITTENTE = r.Substring(CdGroup.idxRAG_SOC_MITTENTE[0], CdGroup.idxRAG_SOC_MITTENTE[1]).Trim();
                            CdGroup.INDIRIZZO_MITTENTE = r.Substring(CdGroup.idxINDIRIZZO_MITTENTE[0], CdGroup.idxINDIRIZZO_MITTENTE[1]).Trim();
                            CdGroup.CAP_MITTENTE = r.Substring(CdGroup.idxCAP_MITTENTE[0], CdGroup.idxCAP_MITTENTE[1]).Trim();
                            CdGroup.LOC_MITTENTE = r.Substring(CdGroup.idxLOC_MITTENTE[0], CdGroup.idxLOC_MITTENTE[1]).Trim();
                            CdGroup.PROV_MITTENTE = r.Substring(CdGroup.idxPROV_MITTENTE[0], CdGroup.idxPROV_MITTENTE[1]).Trim();
                            CdGroup.NAZIONE_MITTENTE = r.Substring(CdGroup.idxNAZIONE_MITTENTE[0], CdGroup.idxNAZIONE_MITTENTE[1]).Substring(0, 2);
                            CdGroup.RAG_SOC_DESTINATARIO = r.Substring(CdGroup.idxRAG_SOC_DESTINATARIO[0], CdGroup.idxRAG_SOC_DESTINATARIO[1]).Trim().Replace("\"", "");
                            CdGroup.INDIRIZZO_DESTINATARIO = r.Substring(CdGroup.idxINDIRIZZO_DESTINATARIO[0], CdGroup.idxINDIRIZZO_DESTINATARIO[1]).Trim().Replace("\"", "");
                            CdGroup.CAP_DESTINATARIO = r.Substring(CdGroup.idxCAP_DESTINATARIO[0], CdGroup.idxCAP_DESTINATARIO[1]).Trim();
                            CdGroup.LOC_DESTINATARIO = r.Substring(CdGroup.idxLOC_DESTINATARIO[0], CdGroup.idxLOC_DESTINATARIO[1]).Trim();
                            CdGroup.PROV_DESTINATARIO = r.Substring(CdGroup.idxPROV_DESTINATARIO[0], CdGroup.idxPROV_DESTINATARIO[1]).Trim();
                            CdGroup.NAZIONE_DESTINATARIO = r.Substring(CdGroup.idxNAZIONE_DESTINATARIO[0], CdGroup.idxNAZIONE_DESTINATARIO[1]).Trim();
                            CdGroup.PESO_SPEDIZIONE = r.Substring(CdGroup.idxPESO_SPEDIZIONE[0], CdGroup.idxPESO_SPEDIZIONE[1]).Trim();
                            CdGroup.VOLUME_SPEDIZIONE = r.Substring(CdGroup.idxVOLUME_SPEDIZIONE[0], CdGroup.idxVOLUME_SPEDIZIONE[1]).Trim();
                            CdGroup.N_CARTONI_CT = r.Substring(CdGroup.idxN_CARTONI_CT[0], CdGroup.idxN_CARTONI_CT[1]).Trim();
                            CdGroup.N_BANCALI_BA = Helper.StringIntString(r.Substring(CdGroup.idxN_BANCALI_BA[0], CdGroup.idxN_BANCALI_BA[1]).Trim());
                            CdGroup.N_BANCALI_COLLETTAME_BB = Helper.StringIntString(r.Substring(CdGroup.idxN_BANCALI_COLLETTAME_BB[0], CdGroup.idxN_BANCALI_COLLETTAME_BB[1]).Trim());
                            CdGroup.N_BA_BB = Helper.StringIntString(r.Substring(CdGroup.idxN_BA_BB[0], CdGroup.idxN_BA_BB[1]).Trim());
                            CdGroup.PESO_CARTONI_CT = r.Substring(CdGroup.idxPESO_CARTONI_CT[0], CdGroup.idxPESO_CARTONI_CT[1]).Trim();
                            CdGroup.VALUTA_CONTRASS = r.Substring(CdGroup.idxVALUTA_CONTRASS[0], CdGroup.idxVALUTA_CONTRASS[1]).Trim();
                            CdGroup.IMPORTO_CONTRASS = r.Substring(CdGroup.idxIMPORTO_CONTRASS[0], CdGroup.idxIMPORTO_CONTRASS[1]).Trim();
                            CdGroup.NUMERO_COLLI_SPED = Helper.StringIntString(r.Substring(CdGroup.idxNUMERO_COLLI_SPED[0], CdGroup.idxNUMERO_COLLI_SPED[1]).Trim());
                            CdGroup.DA_SEGNACOLLO = r.Substring(CdGroup.idxDA_SEGNACOLLO[0], CdGroup.idxDA_SEGNACOLLO[1]).Trim();
                            CdGroup.A_SEGNACOLLO = r.Substring(CdGroup.idxA_SEGNACOLLO[0], CdGroup.idxA_SEGNACOLLO[1]).Trim();
                            CdGroup.NOTE = r.Substring(CdGroup.idxNOTE[0], CdGroup.idxNOTE[1]).Trim();
                            CdGroup.VETTORE = r.Substring(CdGroup.idxVETTORE[0], CdGroup.idxVETTORE[1]).Trim();
                            CdGroup.NR_DISTINTA = r.Substring(CdGroup.idxNR_DISTINTA[0], CdGroup.idxNR_DISTINTA[1]).Trim();
                            CdGroup.DT_DISTINTA = r.Substring(CdGroup.idxDT_DISTINTA[0], CdGroup.idxDT_DISTINTA[1]).Trim();
                            CdGroup.COND_PAG = r.Substring(CdGroup.idxCOND_PAG[0], CdGroup.idxCOND_PAG[1]).Trim();
                            CdGroup.CONS_PIANI = r.Substring(CdGroup.idxCONS_PIANI[0], CdGroup.idxCONS_PIANI[1]).Trim();
                            CdGroup.TEL_PRIMA_CONS = r.Substring(CdGroup.idxTEL_PRIMA_CONS[0], CdGroup.idxTEL_PRIMA_CONS[1]).Trim();
                            CdGroup.DT_CONS_TASSAT_1 = r.Substring(CdGroup.idxDT_CONS_TASSAT_1[0], CdGroup.idxDT_CONS_TASSAT_1[1]).Trim();
                            CdGroup.DT_CONS_TASSAT_2 = r.Substring(CdGroup.idxDT_CONS_TASSAT_2[0], CdGroup.idxDT_CONS_TASSAT_2[1]).Trim();
                            CdGroup.NOTE_1 = r.Substring(CdGroup.idxNOTE_1[0], CdGroup.idxNOTE_1[1]).Trim();
                            CdGroup.NOTE_2 = r.Substring(CdGroup.idxNOTE_2[0], CdGroup.idxNOTE_2[1]).Trim();
                            CdGroup.NOTE_3 = r.Substring(CdGroup.idxNOTE_3[0], CdGroup.idxNOTE_3[1]).Trim();
                            CdGroup.NOTE_4 = r.Substring(CdGroup.idxNOTE_4[0], CdGroup.idxNOTE_4[1]).Trim();
                            CdGroup.NOTE_5 = r.Substring(CdGroup.idxNOTE_5[0], CdGroup.idxNOTE_5[1]).Trim();
                            CdGroup.Libero = r.Substring(CdGroup.idxLibero[0], CdGroup.idxLibero[1]).Trim();
                            //ShipStockHouse.Libero_1 = r.Substring(ShipStockHouse.idxLibero_1[0], ShipStockHouse.idxLibero_1[1]).Trim();
                            CdGroup.N_PALLETTS = Helper.StringIntString(r.Substring(CdGroup.idxN_PALLETTS[0], CdGroup.idxN_PALLETTS[1]).Trim());
                            CdGroup.N_CHEP = Helper.StringIntString(r.Substring(CdGroup.idxN_CHEP[0], CdGroup.idxN_CHEP[1]).Trim());
                            CdGroup.N_EPAL = Helper.StringIntString(r.Substring(CdGroup.idxN_EPAL[0], CdGroup.idxN_EPAL[1]).Trim());
                            CdGroup.AANN = r.Substring(CdGroup.idxAANN[0], CdGroup.idxAANN[1]).Trim();
                            CdGroup.TTRAS = r.Substring(CdGroup.idxTTRAS[0], CdGroup.idxTTRAS[1]).Trim();
                            CdGroup.M_A = r.Substring(CdGroup.idxM_A[0], CdGroup.idxM_A[1]).Trim();
                            CdGroup.NR_PREBOLLA = r.Substring(CdGroup.idxNR_PREBOLLA[0], CdGroup.idxNR_PREBOLLA[1]).Trim();
                            //ShipStockHouse.LEAD_TIME = r.Substring(ShipStockHouse.idxLEAD_TIME[0], ShipStockHouse.idxLEAD_TIME[1]).Trim();
                            //ShipStockHouse.Libero_2 = r.Substring(ShipStockHouse.idxLibero_2[0], ShipStockHouse.idxLibero_2[1]).Trim();
                            #endregion

                            #region apimodel
                            var headerNewShipment = new HeaderNewShipmentTMS();
                            {
                                headerNewShipment.docDate = DateTime.ParseExact(CdGroup.DATA_BOLLA, "yyyyMMdd", null).ToString("MM-dd-yyyy");
                                headerNewShipment.publicNote = $"{CdGroup.NOTE.Trim()} {CdGroup.NOTE_1.Trim()} {CdGroup.NOTE_2.Trim()} {CdGroup.NOTE_3.Trim()} {CdGroup.NOTE_4.Trim()} {CdGroup.NOTE_5.Trim()}".Trim();
                                headerNewShipment.customerID = cust.ID_GESPE;
                                headerNewShipment.cashCurrency = "EUR";
                                headerNewShipment.cashValue = Helper.GetDecimalFromString(CdGroup.IMPORTO_CONTRASS, 2);
                                headerNewShipment.externRef = CdGroup.NR_BOLLA;
                                headerNewShipment.carrierType = "EDI";//int.Parse(CdGroup.N_BANCALI_BA) > 0 ? "PLT" : "COLLO"; //TODO: chiedere conferma sulla priorità su pallet e colli
                                headerNewShipment.serviceType = "S";
                                headerNewShipment.incoterm = "PF";
                                headerNewShipment.transportType = "8-25";
                                headerNewShipment.type = "Groupage";
                                headerNewShipment.cashNote = "";
                                headerNewShipment.insideRef = CdGroup.MANDANTE; //Dove lo mettiamo? Serve per il tracciato di output
                                headerNewShipment.internalNote = $"{CdGroup.NOTE} {CdGroup.NOTE_1} {CdGroup.NOTE_2} {CdGroup.NOTE_3} {CdGroup.NOTE_4} {CdGroup.NOTE_5}";
                                headerNewShipment.cashPayment = "";

                            }

                            DateTime dataTassativa = DateTime.MinValue;

                            DateTime.TryParseExact(CdGroup.DT_CONS_TASSAT_1, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dataTassativa);

                            if (dataTassativa != DateTime.MinValue)
                            {
                                if (dataTassativa < DateTime.Now)
                                {
                                    dataTassativa = DateTime.MinValue;
                                }
                            }

                            Model.CDGROUP.MagazzinoCDGroup MagazzinoCarico = Model.CDGROUP.SediCaricoCDGroup.RecuperaLaSedeCDGroup(CdGroup.MANDANTE);
                            if (MagazzinoCarico == null)
                            {
                                MagazzinoCarico = Model.CDGROUP.SediCaricoCDGroup.SedeLegale;
                            }

                            string ETA = StabilisciETA(CdGroup.LOC_DESTINATARIO.ToLower().Trim(), CdGroup.CAP_DESTINATARIO.Trim(), "02");

                            var stop = new StopNewShipmentTMS();
                            {
                                stop.address = MagazzinoCarico.address;
                                stop.country = MagazzinoCarico.country;
                                stop.description = (CdGroup.RAG_SOC_MITTENTE.Trim() == "F70") ? "PHARDIS LIFE" : CdGroup.RAG_SOC_MITTENTE.Trim();
                                stop.district = MagazzinoCarico.district;
                                stop.zipCode = MagazzinoCarico.zipCode;
                                stop.location = MagazzinoCarico.location;
                                stop.date = DateTime.Now.ToString("MM-dd-yyyy");//headerNewShipment.docDate;
                                stop.type = "P";
                                //stop.region = Helper.GetRegionByZipCode(CdGroup.CAP_MITTENTE);
                                stop.time = "";
                            }
                            destinazione.Add(stop);
                            var regione = GeoTab.FirstOrDefault(x => x.cap == CdGroup.CAP_DESTINATARIO);

                            //!string.IsNullOrEmpty(unloadDate) ? unloadDate : ETA,

                            var stop2 = new StopNewShipmentTMS();
                            {
                                stop2.address = CdGroup.INDIRIZZO_DESTINATARIO;
                                stop2.country = CdGroup.NAZIONE_DESTINATARIO;
                                stop2.description = CdGroup.RAG_SOC_DESTINATARIO.Trim();
                                stop2.district = CdGroup.PROV_DESTINATARIO;
                                stop2.zipCode = CdGroup.CAP_DESTINATARIO;
                                stop2.location = CdGroup.LOC_DESTINATARIO;
                                stop2.date = dataTassativa != DateTime.MinValue ? dataTassativa.ToString("MM-dd-yyyy") : ETA;
                                stop2.type = "D";
                                stop2.region = regione != null ? regione.regione : "";
                                stop2.time = "";
                                stop2.obligatoryType = dataTassativa != DateTime.MinValue ? "Date" : "Nothing";
                            }
                            destinazione.Add(stop2);

                            var goods = new GoodNewShipmentTMS();
                            {
                                goods.grossWeight = Helper.GetDecimalFromString(CdGroup.PESO_SPEDIZIONE, 3);
                                goods.cube = Helper.GetDecimalFromString(CdGroup.VOLUME_SPEDIZIONE, 3);
                                goods.packs = int.Parse(CdGroup.NUMERO_COLLI_SPED);
                                goods.totalPallet = int.Parse(CdGroup.N_BANCALI_BA);
                                goods.floorPallet = int.Parse(CdGroup.N_BANCALI_BA);
                            }

                            merce.Add(goods);

                            #endregion

                            var suffissoBarCode = CdGroup.NR_PREBOLLA.Substring(3);

                            var daSegnacollo = int.Parse(CdGroup.DA_SEGNACOLLO);
                            var aSegnacollo = int.Parse(CdGroup.A_SEGNACOLLO);

                            //sh = lunghezza 10
                            //df ph = lunghezza 9
                            for (int z = daSegnacollo; z <= aSegnacollo; z++)
                            {
                                if (cust == CustomerConnections.StockHouse)
                                {
                                    string BC = z.ToString();
                                    while (BC.Length < 3)
                                    {
                                        BC = "0" + BC;
                                    }
                                    var barCode = new ParcelNewShipmentTMS()
                                    {
                                        barcodeExt = $"{suffissoBarCode}{BC}",
                                    };
                                    parcelNewShipment.Add(barCode);
                                }
                                else
                                {
                                    string BC = z.ToString();
                                    while (BC.Length < 9)
                                    {
                                        BC = "0" + BC;
                                    }
                                    var barCode = new ParcelNewShipmentTMS()
                                    {
                                        barcodeExt = $"{BC}",
                                    };
                                    parcelNewShipment.Add(barCode);
                                }
                            }

                            var totSegnacolli = parcelNewShipment.Count;
                            if (goods.packs > totSegnacolli)
                            {
                                if (goods.grossWeight <= 200)
                                {
                                    goods.totalPallet = 0;
                                    goods.floorPallet = 0;
                                    goods.packs = totSegnacolli;
                                    //goods.width = 80;
                                    //goods.height = 140;
                                    //goods.depth = 90;
                                    goods.cube = RecuperaIlVolumeInBaseAlPeso(goods.grossWeight);
                                    goods.description = "Volume inserito automaticamente in quanto non comunicato dal cliente";
                                }
                                else if (goods.floorPallet == 0)
                                {
                                    //goods.packs = 0;
                                    goods.totalPallet = totSegnacolli;
                                    goods.floorPallet = totSegnacolli;
                                }
                            }

                            shipmentTMS.header = headerNewShipment;
                            shipmentTMS.parcels = parcelNewShipment.ToArray();
                            shipmentTMS.goods = merce.ToArray();
                            shipmentTMS.stops = destinazione.ToArray();
                            ListShip.Add(shipmentTMS);
                            //InviaNuovaShipmentAPI_UNITEX(shipmentTMS);
                        }
                        catch (Exception ee)
                        {
                            _loggerCode.Error(ee);
                        }

                    }
                }
            }
            else if (cust == CustomerConnections.CHIAPPAROLI)
            {
                //DEBUG
                if (!Path.GetFileName(fr).StartsWith("UNIT"))
                {
                    fileProcessati.Remove(fr);
                    return;
                }
                //var pzFr = Path.GetFileName(fr).Split('_');
                var sede = Path.GetFileName(fr).Substring(4, 2);
                var dettData = Path.GetFileName(fr).Split('_')[1];
                var files = Directory.GetFiles(Path.GetDirectoryName(fr));
                //var fileColli = files.Where(x => Path.GetFileName(x).StartsWith("UNID") && Path.GetFileName(x).Substring(4, 2) == sede).FirstOrDefault();
                //var fileAccesori = files.Where(x => Path.GetFileName(x).StartsWith("UNIA") && Path.GetFileName(x).Substring(4, 2) == sede).FirstOrDefault();
                var fileColli = files.Where(x => Path.GetFileName(x).StartsWith("UNID") && Path.GetFileName(x).Split('_')[1] == dettData).FirstOrDefault();
                var fileAccesori = files.Where(x => Path.GetFileName(x).StartsWith("UNIA") && Path.GetFileName(x).Split('_')[1] == dettData).FirstOrDefault();

                var righeTestata = new List<string>();
                var righeSegnacolli = new List<string>();
                var righeAccessori = new List<string>();

                if (!string.IsNullOrEmpty(fr)) { righeTestata = File.ReadAllLines(fr).ToList(); }
                if (!string.IsNullOrEmpty(fileColli)) { righeSegnacolli = File.ReadAllLines(fileColli).ToList(); fileProcessati.Add(fileColli); }
                if (!string.IsNullOrEmpty(fileAccesori)) { righeAccessori = File.ReadAllLines(fileAccesori).ToList(); fileProcessati.Add(fileAccesori); }


                var ListShipChiapparoli = new List<Model.Chiapparoli.Chiapparoli_ShipmentIN>();

                for (int i = 0; i < righeTestata.Count(); i++)
                {
                    var rigaFile = righeTestata[i];
                    iiDebug++;
                    Debug.WriteLine(iiDebug);

                    #region Interpreta Testata
                    var nl = new Model.Chiapparoli.Chiapparoli_ShipmentIN();
                    nl.CodiceSocieta = rigaFile.Substring(nl.idxCodiceSocieta[0], nl.idxCodiceSocieta[1]).Trim();
                    nl.SedeChiapparoli = rigaFile.Substring(nl.idxSedeChiapparoli[0], nl.idxSedeChiapparoli[1]).Trim();
                    nl.CodiceDitta = rigaFile.Substring(nl.idxCodiceDitta[0], nl.idxCodiceDitta[1]).Trim();
                    nl.NumeroProgressivoCHC = rigaFile.Substring(nl.idxNumeroProgressivoCHC[0], nl.idxNumeroProgressivoCHC[1]).Trim();
                    nl.NumeroBordero = rigaFile.Substring(nl.idxNumeroBordero[0], nl.idxNumeroBordero[1]).Trim();
                    nl.DataBordero = rigaFile.Substring(nl.idxDataBordero[0], nl.idxDataBordero[1]).Trim();
                    nl.OraBordero = rigaFile.Substring(nl.idxOraBordero[0], nl.idxOraBordero[1]).Trim();
                    nl.AnnoDDT = rigaFile.Substring(nl.idxAnnoDDT[0], nl.idxAnnoDDT[1]).Trim();
                    nl.NumeroDDT = rigaFile.Substring(nl.idxNumeroDDT[0], nl.idxNumeroDDT[1]).Trim();
                    nl.DataDDT = rigaFile.Substring(nl.idxDataDDT[0], nl.idxDataDDT[1]).Trim();
                    nl.SerieBolla = rigaFile.Substring(nl.idxSerieBolla[0], nl.idxSerieBolla[1]).Trim();
                    nl.Causale = rigaFile.Substring(nl.idxCausale[0], nl.idxCausale[1]).Trim();
                    nl.DescrizioneCausale = rigaFile.Substring(nl.idxDescrizioneCausale[0], nl.idxDescrizioneCausale[1]).Trim();
                    nl.NumDDTMandante = rigaFile.Substring(nl.idxNumDDTMandante[0], nl.idxNumDDTMandante[1]).Trim();
                    nl.NumRifOrdine = rigaFile.Substring(nl.idxNumRifOrdine[0], nl.idxNumRifOrdine[1]).Trim();
                    nl.RiferimentoOrdini = rigaFile.Substring(nl.idxRiferimentoOrdini[0], nl.idxRiferimentoOrdini[1]).Trim();
                    nl.DataOrdineCliente = rigaFile.Substring(nl.idxDataOrdineCliente[0], nl.idxDataOrdineCliente[1]).Trim();
                    nl.Linea = rigaFile.Substring(nl.idxLinea[0], nl.idxLinea[1]).Trim();
                    nl.CodClienteIntestatario = rigaFile.Substring(nl.idxCodClienteIntestatario[0], nl.idxCodClienteIntestatario[1]).Trim();
                    nl.CodClienteDestinatario = rigaFile.Substring(nl.idxCodClienteDestinatario[0], nl.idxCodClienteDestinatario[1]).Trim();
                    nl.CodiceClienteDestinatarioCHC = rigaFile.Substring(nl.idxCodiceClienteDestinatarioCHC[0], nl.idxCodiceClienteDestinatarioCHC[1]).Trim();
                    nl.RagSocialeDestinatario = rigaFile.Substring(nl.idxRagSocialeDestinatario[0], nl.idxRagSocialeDestinatario[1]).Trim();
                    nl.IndirizzoDestinatario = rigaFile.Substring(nl.idxIndirizzoDestinatario[0], nl.idxIndirizzoDestinatario[1]).Trim();
                    nl.LocalitaDestinatario = rigaFile.Substring(nl.idxLocalitaDestinatario[0], nl.idxLocalitaDestinatario[1]).Trim();
                    nl.CAPDestinatario = rigaFile.Substring(nl.idxCAPDestinatario[0], nl.idxCAPDestinatario[1]).Trim();
                    nl.ProvDestinatario = rigaFile.Substring(nl.idxProvDestinatario[0], nl.idxProvDestinatario[1]).Trim();
                    nl.RegioneDestinatario = rigaFile.Substring(nl.idxRegioneDestinatario[0], nl.idxRegioneDestinatario[1]).Trim();
                    nl.NazioneDestinatario = rigaFile.Substring(nl.idxNazioneDestinatario[0], nl.idxNazioneDestinatario[1]).Trim();
                    nl.Inoltro = rigaFile.Substring(nl.idxInoltro[0], nl.idxInoltro[1]).Trim();
                    nl.CodiceVettore = rigaFile.Substring(nl.idxCodiceVettore[0], nl.idxCodiceVettore[1]).Trim();
                    nl.DescrizioneVettore = rigaFile.Substring(nl.idxDescrizioneVettore[0], nl.idxDescrizioneVettore[1]).Trim();
                    nl.Valuta = rigaFile.Substring(nl.idxValuta[0], nl.idxValuta[1]).Trim();
                    nl.ValoreOrdineContrassegno = rigaFile.Substring(nl.idxValoreOrdineContrassegno[0], nl.idxValoreOrdineContrassegno[1]).Trim();
                    nl.PortoFA = rigaFile.Substring(nl.idxPortoFA[0], nl.idxPortoFA[1]).Trim();
                    nl.NumeroRigheDDT = rigaFile.Substring(nl.idxNumeroRigheDDT[0], nl.idxNumeroRigheDDT[1]).Trim();
                    nl.NumeroPezziDDT = rigaFile.Substring(nl.idxNumeroPezziDDT[0], nl.idxNumeroPezziDDT[1]).Trim();
                    nl.NumeroColliDDT = rigaFile.Substring(nl.idxNumeroColliDDT[0], nl.idxNumeroColliDDT[1]).Trim();
                    nl.PesoKG = rigaFile.Substring(nl.idxPesoKG[0], nl.idxPesoKG[1]).Trim();
                    nl.VolumeM3 = rigaFile.Substring(nl.idxVolumeM3[0], nl.idxVolumeM3[1]).Trim();
                    nl.DataConsegnaTassativa = rigaFile.Substring(nl.idxDataConsegnaTassativa[0], nl.idxDataConsegnaTassativa[1]).Trim();
                    nl.SegnacolloIniziale = rigaFile.Substring(nl.idxSegnacolloIniziale[0], nl.idxSegnacolloIniziale[1]).Trim();
                    nl.SegnacolloFinale = rigaFile.Substring(nl.idxSegnacolloFinale[0], nl.idxSegnacolloFinale[1]).Trim();
                    nl.ValutaValCostoSpedizione = rigaFile.Substring(nl.idxValutaValCostoSpedizione[0], nl.idxValutaValCostoSpedizione[1]).Trim();
                    nl.CostoSpedizione = rigaFile.Substring(nl.idxCostoSpedizione[0], nl.idxCostoSpedizione[1]).Trim();
                    nl.ValoreSpedizioneCorriere = rigaFile.Substring(nl.idxValoreSpedizioneCorriere[0], nl.idxValoreSpedizioneCorriere[1]).Trim();
                    nl.SimulazioneValoreSpedizione = rigaFile.Substring(nl.idxSimulazioneValoreSpedizione[0], nl.idxSimulazioneValoreSpedizione[1]).Trim();
                    nl.DataConsegnaCliente = rigaFile.Substring(nl.idxDataConsegnaCliente[0], nl.idxDataConsegnaCliente[1]).Trim();
                    nl.DefinizioneIMS = rigaFile.Substring(nl.idxDefinizioneIMS[0], nl.idxDefinizioneIMS[1]).Trim();
                    nl.CampoTest1 = rigaFile.Substring(nl.idxCampoTest1[0], nl.idxCampoTest1[1]).Trim();
                    nl.CampoTest2 = rigaFile.Substring(nl.idxCampoTest2[0], nl.idxCampoTest2[1]).Trim();
                    nl.CampoTest3 = rigaFile.Substring(nl.idxCampoTest3[0], nl.idxCampoTest3[1]).Trim();
                    nl.CampoTest4 = rigaFile.Substring(nl.idxCampoTest4[0], nl.idxCampoTest4[1]).Trim();
                    nl.CampoTest5 = rigaFile.Substring(nl.idxCampoTest5[0], nl.idxCampoTest5[1]).Trim();
                    nl.CampoTest6 = rigaFile.Substring(nl.idxCampoTest6[0], nl.idxCampoTest6[1]).Trim();
                    nl.NoteConsegna = rigaFile.Substring(nl.idxNoteConsegna[0], nl.idxNoteConsegna[1]).Trim();
                    #endregion

                    #region Interpreta Accessori
                    var accessoriDellaSpedizione = righeAccessori.Where(x => x.Substring(6, 9) == nl.NumeroProgressivoCHC).ToList();
                    List<Model.Chiapparoli.Chiapparoli_DatiAccessori> ServiziAccessoriSpedizione = new List<Chiapparoli_DatiAccessori>();

                    foreach (var acc in accessoriDellaSpedizione)
                    {
                        var accessori = new Model.Chiapparoli.Chiapparoli_DatiAccessori();

                        accessori.CodiceDitta = acc.Substring(accessori.idxCodiceDitta[0], accessori.idxCodiceDitta[1]).Trim();
                        accessori.CodiceMagazzino = acc.Substring(accessori.idxCodiceMagazzino[0], accessori.idxCodiceMagazzino[1]).Trim();
                        //accessori.CostoConcordato = acc.Substring(accessori.idxCostoConcordato[0], accessori.idxCostoConcordato[1]).Trim();
                        accessori.DataBordero = acc.Substring(accessori.idxDataBordero[0], accessori.idxDataBordero[1]).Trim();
                        accessori.Descrizione = acc.Substring(accessori.idxDescrizione[0], accessori.idxDescrizione[1]).Trim();
                        accessori.IDServizioAggiuntivo = acc.Substring(accessori.idxIDServizioAggiuntivo[0], accessori.idxIDServizioAggiuntivo[1]).Trim();
                        //accessori.NoteRigaServizio = acc.Substring(accessori.idxNoteRigaServizio[0], accessori.idxNoteRigaServizio[1]).Trim();
                        accessori.NumeroBordero = acc.Substring(accessori.idxNumeroBordero[0], accessori.idxNumeroBordero[1]).Trim();
                        accessori.NumeroProgressivoCHC = acc.Substring(accessori.idxNumeroProgressivoCHC[0], accessori.idxNumeroProgressivoCHC[1]).Trim();
                        accessori.OraBordero = acc.Substring(accessori.idxOraBordero[0], accessori.idxOraBordero[1]).Trim();

                        ServiziAccessoriSpedizione.Add(accessori);
                    }

                    //inserire i servizi accessori in testata
                    //MAX 10 inseribili in EDI
                    bool sponda = false;
                    bool facchinaggio = false;
                    bool preavviso = false;

                    foreach (var vincolo in ServiziAccessoriSpedizione)
                    {
                        if (vincolo.IDServizioAggiuntivo == "TASSATI") { }  //a che serve se la tassativa me la mettono in testata?!?!?!?               
                        else if (vincolo.IDServizioAggiuntivo == "APEDOS") { }
                        else if (vincolo.IDServizioAggiuntivo == "GGSOST") { }
                        else if (vincolo.IDServizioAggiuntivo == "RICONS") { }
                        else if (vincolo.IDServizioAggiuntivo == "RIENTR") { }
                        else if (vincolo.IDServizioAggiuntivo == "NORIT") { }
                        else if (vincolo.IDServizioAggiuntivo == "SPONDA") { sponda = true; }
                        else if (vincolo.IDServizioAggiuntivo == "CONTRP") { }
                        else if (vincolo.IDServizioAggiuntivo == "CONTRF") { }
                        else if (vincolo.IDServizioAggiuntivo == "POD") { }
                        else if (vincolo.IDServizioAggiuntivo == "DDTCAR") { }
                        else if (vincolo.IDServizioAggiuntivo == "ISOMIN") { }
                        else if (vincolo.IDServizioAggiuntivo == "DISAGI") { }
                        else if (vincolo.IDServizioAggiuntivo == "VOLUME") { }
                        else if (vincolo.IDServizioAggiuntivo == "TRACCI") { }
                        else if (vincolo.IDServizioAggiuntivo == "DOGANA") { }
                        else if (vincolo.IDServizioAggiuntivo == "PIANO") { facchinaggio = true; }
                        else if (vincolo.IDServizioAggiuntivo == "TELEFO") { preavviso = true; }
                        else if (vincolo.IDServizioAggiuntivo == "FACCHI") { facchinaggio = true; }
                        else if (vincolo.IDServizioAggiuntivo == "SMS") { }
                        else if (vincolo.IDServizioAggiuntivo == "APPUNT") { }
                        else if (vincolo.IDServizioAggiuntivo == "TASSATI") { }
                        else if (vincolo.IDServizioAggiuntivo == "GDO") { }
                        else if (vincolo.IDServizioAggiuntivo == "GDOWEB") { }
                        else if (vincolo.IDServizioAggiuntivo == "CARSCA") { }
                        else if (vincolo.IDServizioAggiuntivo == "FUEL") { }
                        else if (vincolo.IDServizioAggiuntivo == "ASSICU") { }
                        else if (vincolo.IDServizioAggiuntivo == "URGENZ") { }
                        else if (vincolo.IDServizioAggiuntivo == "DEDICA") { }
                        else if (vincolo.IDServizioAggiuntivo == "TRIANG") { }
                        else if (vincolo.IDServizioAggiuntivo == "EXTRA") { }
                        else if (vincolo.IDServizioAggiuntivo == "RICPCO") { }
                        else if (vincolo.IDServizioAggiuntivo == "IMBARN") { }
                        else if (vincolo.IDServizioAggiuntivo == "IMS") { }
                        else if (vincolo.IDServizioAggiuntivo == "ADR") { }
                        else if (vincolo.IDServizioAggiuntivo == "SPOT") { }
                        else if (vincolo.IDServizioAggiuntivo == "PAZIENTE") { }
                        else if (vincolo.IDServizioAggiuntivo == "DIRFIS") { }
                        else if (vincolo.IDServizioAggiuntivo == "CAPRI") { }
                        else if (vincolo.IDServizioAggiuntivo == "ISCHIA") { }
                        else if (vincolo.IDServizioAggiuntivo == "PROCIDA") { }
                        else if (vincolo.IDServizioAggiuntivo == "GENOVA") { }
                        else if (vincolo.IDServizioAggiuntivo == "ROMAGNA") { }
                        else if (vincolo.IDServizioAggiuntivo == "REGGIO") { }
                        else if (vincolo.IDServizioAggiuntivo == "SERA") { }
                        else if (vincolo.IDServizioAggiuntivo == "SABATO") { }
                        else if (vincolo.IDServizioAggiuntivo == "GOLD") { }
                        else if (vincolo.IDServizioAggiuntivo == "PRIORITARIA") { }
                    }
                    if (facchinaggio)
                    {
                        nl.Vincoli.Add("FACC");
                    }
                    if (sponda)
                    {
                        nl.Vincoli.Add("SPONDA");
                    }
                    if (preavviso)
                    {
                        nl.Vincoli.Add("PREAVV");
                    }
                    #endregion

                    ListShipChiapparoli.Add(nl);
                }

                //FANNO RAGGRUPPATE???
                foreach (var shipChiapparoli in ListShipChiapparoli)
                {
                    RootobjectNewShipmentTMS shipmentTMS = new RootobjectNewShipmentTMS();
                    List<ParcelNewShipmentTMS> parcelNewShipment = new List<ParcelNewShipmentTMS>();
                    List<StopNewShipmentTMS> destinazione = new List<StopNewShipmentTMS>();
                    List<GoodNewShipmentTMS> merce = new List<GoodNewShipmentTMS>();

                    #region TipoSpedizione
                    string incoterm = "";
                    if (shipChiapparoli.PortoFA == "F")//Porto franco
                    {
                        incoterm = "PF";
                    }
                    else// == A //Porto assegnato
                    {
                        incoterm = "PA";
                    }
                    #endregion

                    #region Tassativa
                    string unloadDate = "";
                    //TODO: non so come mandare il tipo data di consegna tramite api
                    bool isTassativa = false;
                    if (!string.IsNullOrEmpty(shipChiapparoli.DataConsegnaTassativa))
                    {
                        DateTime unlDta = DateTime.MinValue;
                        DateTime.TryParseExact(shipChiapparoli.DataConsegnaTassativa, "yyMMdd", null, DateTimeStyles.AssumeLocal, out unlDta);//
                        if (unlDta != DateTime.MinValue)
                        {
                            isTassativa = true;
                            unloadDate = unlDta.ToString("MM-dd-yyyy");
                        }

                    }
                    #endregion

                    #region TipoTrasporto
                    string serviceType = "S";

                    #endregion

                    #region DatiTestata
                    string docDateGespe = DateTime.ParseExact(shipChiapparoli.DataDDT, "yyMMdd", null).ToString("MM-dd-yyyy");
                    var headerNewShipment = new HeaderNewShipmentTMS()
                    {
                        carrierType = "EDI",
                        serviceType = serviceType,
                        incoterm = incoterm,
                        transportType = RilevaTransportTypeChiapparoli(shipChiapparoli.Linea), //"8-25",//FANNO 2/8 ???
                        type = "Groupage",
                        insideRef = $"{shipChiapparoli.NumeroDDT}|{shipChiapparoli.CodiceDitta}|{shipChiapparoli.SedeChiapparoli}", //Serve per il tracciato di Esiti
                        internalNote = "",
                        externRef = shipChiapparoli.NumeroProgressivoCHC,  //Server per il tracciato di Esiti
                        publicNote = shipChiapparoli.NoteConsegna,
                        docDate = docDateGespe,
                        customerID = cust.ID_GESPE,
                        cashCurrency = shipChiapparoli.Valuta,
                        cashValue = Helper.GetDecimalFromString(shipChiapparoli.ValoreOrdineContrassegno, 2),
                        cashPayment = "",
                        cashNote = "",
                    };
                    #endregion

                    #region SegnacolliEDettaglioMerce
                    List<string> SegnacolliDellaSpedizione = new List<string>();
                    if (shipChiapparoli.NumeroProgressivoCHC.StartsWith("00"))
                    {
                        SegnacolliDellaSpedizione = righeSegnacolli.Where(x => x.Substring(8, 7) == shipChiapparoli.NumeroProgressivoCHC.Substring(2)).ToList();
                    }
                    else
                    {
                        SegnacolliDellaSpedizione = righeSegnacolli.Where(x => x.Substring(6, 9) == shipChiapparoli.NumeroProgressivoCHC).ToList();
                    }
                    List<Chiapparoli_DettaglioColli> dettaglioMerce = new List<Chiapparoli_DettaglioColli>();

                    foreach (var s in SegnacolliDellaSpedizione)
                    {
                        //attendere conferma per i segnacolli, se devo stimarli io da iniziale e finale o mi passano N righe nel file Dettaglio già con i segnacolli corretti

                        var segnaCollo = new Chiapparoli_DettaglioColli();
                        segnaCollo.CodiceSocieta = s.Substring(segnaCollo.idxCodiceSocieta[0], segnaCollo.idxCodiceSocieta[1]).Trim();
                        segnaCollo.CodiceMagazzino = s.Substring(segnaCollo.idxCodiceMagazzino[0], segnaCollo.idxCodiceMagazzino[1]).Trim();
                        segnaCollo.CodiceDitta = s.Substring(segnaCollo.idxCodiceDitta[0], segnaCollo.idxCodiceDitta[1]).Trim();
                        segnaCollo.NumeroProgressivoCHC = s.Substring(segnaCollo.idxNumeroProgressivoCHC[0], segnaCollo.idxNumeroProgressivoCHC[1]).Trim();
                        segnaCollo.IDCollo = s.Substring(segnaCollo.idxIDCollo[0], segnaCollo.idxIDCollo[1]).Trim();
                        segnaCollo.IDBancale = s.Substring(segnaCollo.idxIDBancale[0], segnaCollo.idxIDBancale[1]).Trim();

                        dettaglioMerce.Add(segnaCollo);

                    }

                    //RAGGRUPPARE I SEGNACOLLI PEDANE E SCARTARE I SEGNACOLLI COLLO ABBINATI

                    var DettagliRaggruppatiPerBancale = dettaglioMerce.GroupBy(x => x.IDBancale).ToList();
                    var goodsBancali = new List<GoodNewShipmentTMS>();
                    var goodsColli = new List<GoodNewShipmentTMS>();
                    var pesoDelivery = int.Parse(shipChiapparoli.PesoKG);

                    if (dettaglioMerce.Count() > 0)
                    {
                        foreach (var rag in dettaglioMerce)
                        {
                            var goods = new GoodNewShipmentTMS();
                            var parcel = new ParcelNewShipmentTMS();
                            //collo
                            if (rag.IDCollo.StartsWith("COLLO") && rag.IDBancale.StartsWith("BAN"))
                            {
                                parcel.barcodeExt = $"{rag.IDCollo}";
                                parcelNewShipment.Add(parcel);
                                goods.packs = 1;
                                goodsColli.Add(goods);
                            }
                            //??
                            else if (rag.IDCollo.StartsWith("COLLI") && rag.IDBancale.StartsWith("BAN"))
                            {
                                parcel.barcodeExt = $"{rag.IDCollo}";
                                parcelNewShipment.Add(parcel);
                                goods.packs = 1;
                                goodsColli.Add(goods);
                            }
                            //??
                            else if (rag.IDCollo.StartsWith("COLLO") && rag.IDBancale.StartsWith("LINEA"))
                            {
                                parcel.barcodeExt = $"{rag.IDCollo}";
                                parcelNewShipment.Add(parcel);
                                goods.packs = 1;
                                goodsColli.Add(goods);
                            }
                            //bancale+
                            else if (rag.IDCollo.StartsWith("BAN") && rag.IDBancale.StartsWith("BAN"))
                            {
                                parcel.barcodeMaster = $"{rag.IDBancale}";
                                parcelNewShipment.Add(parcel);
                                goods.floorPallet = goods.totalPallet = 1;
                                goodsBancali.Add(goods);
                            }
                            //condizione non segnalata dal cliente
                            else
                            {
                                //if(goods.packs == 0 && goods.floorPallet == 0)
                            }
                        }

                        var totColliSped = goodsColli.Sum(x => x.packs);
                        var totPltSped = parcelNewShipment.Where(x => x.barcodeMaster != null).GroupBy(y => y.barcodeMaster).Count();

                        var DaAggiungereRaggruppata = new GoodNewShipmentTMS()
                        {
                            floorPallet = totPltSped,
                            totalPallet = totPltSped,
                            packs = totColliSped,
                            grossWeight = pesoDelivery
                        };
                        merce.Add(DaAggiungereRaggruppata);
                    }
                    else
                    {
                        var DaAggiungereRaggruppata = new GoodNewShipmentTMS()
                        {
                            floorPallet = 0,
                            totalPallet = 0,
                            packs = int.Parse(shipChiapparoli.NumeroColliDDT),
                            grossWeight = pesoDelivery
                        };
                        merce.Add(DaAggiungereRaggruppata);
                    }
                    //goods.packs = SegnacolliDellaSpedizione.Count();
                    //var rigaRaggruppata = new GoodNewShipmentTMS()
                    //{
                    //    packs = goodsL.Sum(x => x.packs),
                    //    floorPallet = goodsL.Sum(x => x.floorPallet),
                    //    totalPallet = goodsL.Sum(x => x.floorPallet),
                    //    grossWeight = pesoDelivery
                    //};
                    //merce.Add(rigaRaggruppata);
                    //var colliDelivery = SegnacolliDellaSpedizione.Count();
                    //var pedaneDelivery = int.Parse(shipChiapparoli.NumeroPedane);
                    #endregion

                    #region CaricoEScarico
                    var stop = new StopNewShipmentTMS()
                    {

                        address = "Via Cascina Nuova",
                        country = "IT",
                        description = "Chiapparoli Logistica",
                        district = "LO",
                        zipCode = "26814",
                        location = "Livraga",
                        date = DateTime.Now.ToString("MM-dd-yyyy"),
                        type = "P",
                        region = "Lombardia",
                        time = shipChiapparoli.OraBordero.Insert(2, ":") + ":00",
                    };
                    destinazione.Add(stop);

                    var regione = GeoTab.FirstOrDefault(x => x.cap == shipChiapparoli.CAPDestinatario);
                    string ETA = StabilisciETA(shipChiapparoli.LocalitaDestinatario.ToLower().Trim(), shipChiapparoli.CAPDestinatario.Trim(), "02");

                    var stop2 = new StopNewShipmentTMS()
                    {

                        address = shipChiapparoli.IndirizzoDestinatario.Replace("\"", ""),
                        country = shipChiapparoli.NazioneDestinatario,
                        description = shipChiapparoli.RagSocialeDestinatario.Trim().Replace("\"", ""),
                        district = shipChiapparoli.ProvDestinatario,
                        zipCode = shipChiapparoli.CAPDestinatario,
                        location = shipChiapparoli.LocalitaDestinatario,
                        date = !string.IsNullOrEmpty(unloadDate) ? unloadDate : ETA,
                        type = "D",
                        region = regione != null ? regione.regione : "",
                        time = "",
                        obligatoryType = isTassativa ? "Date" : "Nothing"
                    };

                    destinazione.Add(stop2);
                    #endregion

                    merce[0].cube = Helper.GetDecimalFromString(shipChiapparoli.VolumeM3, 3);
                    shipmentTMS.header = headerNewShipment;
                    shipmentTMS.parcels = parcelNewShipment.ToArray();
                    shipmentTMS.goods = merce.ToArray();
                    shipmentTMS.stops = destinazione.ToArray();
                    shipmentTMS.isTassativa = isTassativa;

                    ListShip.Add(shipmentTMS);
                }

            }
            else if (cust == CustomerConnections._3CS)
            {
                var tutteLeRighe = File.ReadAllLines(fr);
                if (tutteLeRighe.Count() > 0)
                {
                    List<_3C_ShipmentIN> Ships3CdaRaggruppare = new List<_3C_ShipmentIN>();
                    for (int i = 0; i < tutteLeRighe.Count(); i++)
                    {

                        var r = tutteLeRighe[i];
                        if (i < tutteLeRighe.Count())
                        {
                            #region Interpreta File
                            var rigadoc = new _3C_ShipmentIN();
                            rigadoc.Annotazioni1 = r.Substring(rigadoc.idxAnnotazioni1[0], rigadoc.idxAnnotazioni1[1]).Trim();
                            rigadoc.Annotazioni2 = r.Substring(rigadoc.idxAnnotazioni2[0], rigadoc.idxAnnotazioni2[1]).Trim();
                            rigadoc.Annotazioni3 = r.Substring(rigadoc.idxAnnotazioni3[0], rigadoc.idxAnnotazioni3[1]).Trim();
                            rigadoc.BarcodeSegnacollo = r.Substring(rigadoc.idxBarcodeSegnacollo[0], rigadoc.idxBarcodeSegnacollo[1]).Trim();
                            rigadoc.CAPDestinatario = r.Substring(rigadoc.idxCAPDestinatario[0], rigadoc.idxCAPDestinatario[1]).Trim();
                            rigadoc.Colli = r.Substring(rigadoc.idxColli[0], rigadoc.idxColli[1]).Trim();
                            rigadoc.Contrassegno2D = r.Substring(rigadoc.idxContrassegno2D[0], rigadoc.idxContrassegno2D[1]).Trim();
                            rigadoc.DataBolla = r.Substring(rigadoc.idxDataBolla[0], rigadoc.idxDataBolla[1]).Trim();
                            rigadoc.DataConsegnaTassativa = r.Substring(rigadoc.idxDataConsegnaTassativa[0], rigadoc.idxDataConsegnaTassativa[1]).Trim();
                            rigadoc.Filler = r.Substring(rigadoc.idxFiller[0], rigadoc.idxFiller[1]).Trim();
                            rigadoc.GiorniChiusura = r.Substring(rigadoc.idxGiorniChiusura[0], rigadoc.idxGiorniChiusura[1]).Trim();
                            rigadoc.IDBarcodeSegnacollo = r.Substring(rigadoc.idxIDBarcodeSegnacollo[0], rigadoc.idxIDBarcodeSegnacollo[1]).Trim();
                            rigadoc.IndirizzoDestinatario = r.Substring(rigadoc.idxIndirizzoDestinatario[0], rigadoc.idxIndirizzoDestinatario[1]).Trim();
                            rigadoc.IndirizzoMittenteOriginale = r.Substring(rigadoc.idxIndirizzoMittenteOriginale[0], rigadoc.idxIndirizzoMittenteOriginale[1]).Trim();
                            rigadoc.LocalitaDestinatario = r.Substring(rigadoc.idxLocalitaDestinatario[0], rigadoc.idxLocalitaDestinatario[1]).Trim();
                            rigadoc.LocalitaMittenteOriginale = r.Substring(rigadoc.idxLocalitaMittenteOriginale[0], rigadoc.idxLocalitaMittenteOriginale[1]).Trim();
                            rigadoc.NumeroBolla = r.Substring(rigadoc.idxNumeroBolla[0], rigadoc.idxNumeroBolla[1]).Trim();
                            rigadoc.NumeroBorderau = r.Substring(rigadoc.idxNumeroBorderau[0], rigadoc.idxNumeroBorderau[1]).Trim();
                            rigadoc.NumeroSegnacolliStampati = r.Substring(rigadoc.idxNumeroSegnacolliStampati[0], rigadoc.idxNumeroSegnacolliStampati[1]).Trim();
                            rigadoc.NumeroTelefonoPreavviso = r.Substring(rigadoc.idxNumeroTelefonoPreavviso[0], rigadoc.idxNumeroTelefonoPreavviso[1]).Trim();
                            rigadoc.Peso1D = r.Substring(rigadoc.idxPeso1D[0], rigadoc.idxPeso1D[1]).Trim();
                            rigadoc.PrimoSegnacollo = r.Substring(rigadoc.idxPrimoSegnacollo[0], rigadoc.idxPrimoSegnacollo[1]).Trim();
                            rigadoc.ProvDestinatario = r.Substring(rigadoc.idxProvDestinatario[0], rigadoc.idxProvDestinatario[1]).Trim();
                            rigadoc.RagioneSocialeDestinatario = r.Substring(rigadoc.idxRagioneSocialeDestinatario[0], rigadoc.idxRagioneSocialeDestinatario[1]).Trim();
                            rigadoc.RagioneSocialeMittenteOriginale = r.Substring(rigadoc.idxRagioneSocialeMittenteOriginale[0], rigadoc.idxRagioneSocialeMittenteOriginale[1]).Trim();
                            rigadoc.RiferimentoMittenteOriginale = r.Substring(rigadoc.idxRiferimentoMittenteOriginale[0], rigadoc.idxRiferimentoMittenteOriginale[1]).Trim();
                            rigadoc.TipoConsegnaTassativa = r.Substring(rigadoc.idxTipoConsegnaTassativa[0], rigadoc.idxTipoConsegnaTassativa[1]).Trim();
                            rigadoc.TipoTrasporto = r.Substring(rigadoc.idxTipoTrasporto[0], rigadoc.idxTipoTrasporto[1]).Trim();
                            rigadoc.TotaleDaIncassare2D = r.Substring(rigadoc.idxTotaleDaIncassare2D[0], rigadoc.idxTotaleDaIncassare2D[1]).Trim();
                            rigadoc.UltimoSegnacollo = r.Substring(rigadoc.idxUltimoSegnacollo[0], rigadoc.idxUltimoSegnacollo[1]).Trim();
                            rigadoc.Volume2D = r.Substring(rigadoc.idxVolume2D[0], rigadoc.idxVolume2D[1]).Trim();
                            rigadoc.ValoreMerce2D = r.Substring(rigadoc.idxValoreMerce2D[0], rigadoc.idxValoreMerce2D[1]).Trim();
                            rigadoc.ZonaDiConsegna = r.Substring(rigadoc.idxZonaDiConsegna[0], rigadoc.idxZonaDiConsegna[1]).Trim();
                            #endregion
                            Ships3CdaRaggruppare.Add(rigadoc);
                        }
                    }

                    //raggruppa le spedizioni con la stessa chiave
                    var raggruppate = Ships3CdaRaggruppare.GroupBy(x => x.NumeroBolla).ToList();
                    //per ogni chiave recupera i segnacolli
                    foreach (var Ships3C in raggruppate)
                    {
                        RootobjectNewShipmentTMS shipmentTMS = new RootobjectNewShipmentTMS();
                        List<ParcelNewShipmentTMS> parcelNewShipment = new List<ParcelNewShipmentTMS>();
                        List<StopNewShipmentTMS> destinazione = new List<StopNewShipmentTMS>();
                        List<GoodNewShipmentTMS> merce = new List<GoodNewShipmentTMS>();
                        var Ship3C = Ships3C.FirstOrDefault();

                        #region TipoSpedizione
                        string incoterm = "";
                        if (Ship3C.TipoTrasporto == "F")//Porto franco
                        {
                            incoterm = "PF";
                        }
                        else// == A //Porto assegnato
                        {
                            incoterm = "PA";
                        }
                        #endregion

                        #region Tassativa
                        string unloadDateType = "";
                        string unloadDate = "";
                        //TODO: non so come mandare il tipo data di consegna tramite EDI
                        bool isTassativa = false;
                        if (!string.IsNullOrEmpty(Ship3C.DataConsegnaTassativa) && Ship3C.DataConsegnaTassativa != "00000000")
                        {
                            unloadDate = DateTime.ParseExact(Ship3C.DataConsegnaTassativa, "yyyyMMdd", null).ToString("MM-dd-yyyy");

                            if (Ship3C.TipoConsegnaTassativa.Trim().ToUpper() == "T")
                            {
                                isTassativa = true;
                            }
                            else if (Ship3C.TipoConsegnaTassativa.Trim().ToUpper() == "E")
                            {
                                isTassativa = true;
                            }
                            else if (Ship3C.TipoConsegnaTassativa.Trim().ToUpper() == "P")
                            {
                                isTassativa = true;
                            }

                        }
                        #endregion

                        #region DatiTestata
                        var headerNewShipment = new HeaderNewShipmentTMS()
                        {
                            carrierType = "EDI",
                            serviceType = "S",
                            incoterm = incoterm,
                            transportType = "8-25",
                            type = "Groupage",
                            insideRef = Ship3C.NumeroBorderau,
                            internalNote = $"{Ship3C.Annotazioni1} {Ship3C.Annotazioni2} {Ship3C.Annotazioni3}",
                            externRef = Ship3C.NumeroBolla,
                            publicNote = $"{Ship3C.Annotazioni1} {Ship3C.Annotazioni2} {Ship3C.Annotazioni3}",
                            docDate = DateTime.Now.ToString("MM-dd-yyyy"),
                            customerID = cust.ID_GESPE,
                            cashCurrency = "EUR",
                            cashValue = Helper.GetDecimalFromString(Ship3C.TotaleDaIncassare2D, 2),
                            cashPayment = "",
                            cashNote = "",
                        };
                        #endregion

                        #region SegnacolliEDettaglioMerce
                        List<_3C_ShipmentINColli> dettaglioMerce = Ships3C.Select(x => new _3C_ShipmentINColli { segnacollo = x.BarcodeSegnacollo }).ToList();

                        dettaglioMerce = dettaglioMerce.Skip(1).ToList();

                        int iiDBG = 0;
                        foreach (var sc in dettaglioMerce)
                        {
                            iiDBG++;
                            Debug.WriteLine(iiDBG);

                            var goods = new GoodNewShipmentTMS();
                            {
                                goods.packs = 1;

                            }

                            var parcel = new ParcelNewShipmentTMS()
                            {
                                barcodeExt = $"{sc.segnacollo}"
                            };

                            parcelNewShipment.Add(parcel);
                            merce.Add(goods);
                        }

                        #endregion

                        #region CaricoEScarico
                        var stop = new StopNewShipmentTMS()
                        {
                            address = "Via Luciano Lama",
                            country = "IT",
                            description = $"3C-{Ship3C.RagioneSocialeMittenteOriginale}",
                            district = "PV",
                            zipCode = "27012",
                            location = "CERTOSA DI PAVIA",
                            date = DateTime.Now.ToString("yyyy-MM-dd"),
                            type = "P",
                            region = "Lombardia",
                            time = "",
                        };
                        destinazione.Add(stop);

                        var regione = GeoTab.FirstOrDefault(x => x.cap == Ship3C.CAPDestinatario);
                        var stop2 = new StopNewShipmentTMS()
                        {

                            address = Ship3C.IndirizzoDestinatario.Replace("\"", ""),
                            country = "IT",
                            description = Ship3C.RagioneSocialeDestinatario.Trim().Replace("\"", ""),
                            district = Ship3C.ProvDestinatario,
                            zipCode = Ship3C.CAPDestinatario,
                            location = Ship3C.LocalitaDestinatario,
                            date = !string.IsNullOrEmpty(unloadDate) ? unloadDate : "",
                            type = "D",
                            region = regione != null ? regione.regione : "",
                            time = "",
                            obligatoryType = !string.IsNullOrEmpty(unloadDate) ? "Date" : "Nothing"
                        };

                        destinazione.Add(stop2);
                        #endregion

                        merce[0].cube = Helper.GetDecimalFromString(Ship3C.Volume2D, 2);
                        merce[0].grossWeight = Helper.GetDecimalFromString(Ship3C.Peso1D, 1);
                        shipmentTMS.header = headerNewShipment;
                        shipmentTMS.parcels = parcelNewShipment.ToArray();
                        shipmentTMS.goods = merce.ToArray();
                        shipmentTMS.stops = destinazione.ToArray();
                        shipmentTMS.isTassativa = isTassativa;


                        ListShip.Add(shipmentTMS);
                        //InviaNuovaShipmentAPI_UNITEX(shipmentTMS);
                    }
                }
            }
            else//altro cliente non informatizzato
            {
                return;
            }

            if (ListShip.Count() > 0)
            {
                foreach (var ls in ListShip)
                {
                    RigheCSV.AddRange(ConvertiSpedizioneAPIinEDI(ls, cust));
                }
            }
            InviaCSVAlServiceManagerByFTP(cust, RigheCSV, fr); //Il service manager è il task che gira ogni tot minuti del TMS Gespe Unitex, che elabora i file nelle directory

        }

        private string StabilisciETA(string Localita, string CAP, string hub)
        {
            var geo = GeoSpec.GeoList.FirstOrDefault(x => x.citta.ToLower().Trim() == Localita.ToLower().Trim());
            if (geo == null)
            {
                geo = GeoSpec.GeoList.FirstOrDefault(x => x.cap.ToLower().Trim() == CAP.ToLower().Trim());
            }
            if (geo == null)
            {
                return DateTime.Now.AddHours(48).ToString("MM-dd-yyyy");
            }
            else
            {
                var oreMax = TempiResa.TempiResaUtils.RecuperaOreResaOttimali(geo, hub, false);
                return DateTime.Now.AddHours(oreMax).ToString("MM-dd-yyyy");
            }
        }

        private string RilevaTransportTypeChiapparoli(string linea)
        {
            if (linea == "01")
            {
                return "8-25";
            }
            else if (linea == "05")
            {
                return "2-8";
            }
            else
            {
                return "8-25";
            }
        }

        private string RecuperaData3C(string dataBolla)
        {
            DateTime ok = DateTime.MinValue;

            DateTime.TryParseExact(dataBolla, "yyyyMMdd", null, DateTimeStyles.None, out ok);

            if (ok != DateTime.MinValue)
            {
                return ok.ToString("MM-dd-yyyy");
            }
            else
            {
                return DateTime.Now.ToString("MM-dd-yyyy");
            }
        }

        #region SpecLoreal
        private List<Logistica93_ShipmentIN> RaggruppaTestateLoreal(List<Logistica93_ShipmentIN> listShipLoreal)
        {
            var resp = new List<Logistica93_ShipmentIN>();
            var raggruppate = listShipLoreal.GroupBy(x => x.NumeroDDT).ToList();
            foreach (var rr in raggruppate)
            {
                if (rr.Count() == 1)
                {
                    resp.Add(rr.First());
                }
                else
                {
                    var ls = new Logistica93_ShipmentIN();
                    ls = rr.First();
                    ls.NumeroColliStandard = SommaColliLoreal(rr);
                    ls.NumeroPedaneEPAL = SommaPedaneEPALLoreal(rr);
                    ls.NumeroPedane = SommaPedaneLoreal(rr);
                    ls.NumeroColliDettaglio = SommaColliDettaglioLoreal(rr);
                    ls.PesoDelivery = SommaPesoDeliveryLoreal(rr);
                    resp.Add(ls);
                }
            }
            return resp;
        }
        private string SommaPesoDeliveryLoreal(IGrouping<string, Logistica93_ShipmentIN> rr)
        {
            List<decimal> DaSommare = new List<decimal>();
            int l = rr.First().PesoDelivery.Length;
            string resp = "";
            foreach (var r in rr)
            {
                DaSommare.Add(Helper.GetDecimalFromString(r.PesoDelivery, 2));
            }
            decimal sommati = DaSommare.Sum(x => x);
            resp = sommati.ToString().Replace(",", "");
            while (resp.Length < l)
            {
                resp = resp.Insert(0, "0");
            }
            return resp;
        }
        private string SommaColliDettaglioLoreal(IGrouping<string, Logistica93_ShipmentIN> rr)
        {
            List<int> DaSommare = new List<int>();
            int l = rr.First().NumeroColliDettaglio.Length;
            string resp = "";
            foreach (var r in rr)
            {
                DaSommare.Add(int.Parse(r.NumeroColliDettaglio));
            }
            int sommati = DaSommare.Sum(x => x);
            resp = sommati.ToString();
            while (resp.Length < l)
            {
                resp = resp.Insert(0, "0");
            }
            return resp;
        }
        private string SommaPedaneLoreal(IGrouping<string, Logistica93_ShipmentIN> rr)
        {
            List<int> DaSommare = new List<int>();
            int l = rr.First().NumeroPedane.Length;
            string resp = "";
            foreach (var r in rr)
            {
                DaSommare.Add(int.Parse(r.NumeroPedane));
            }
            int sommati = DaSommare.Sum(x => x);
            resp = sommati.ToString();
            while (resp.Length < l)
            {
                resp = resp.Insert(0, "0");
            }
            return resp;
        }
        private string SommaPedaneEPALLoreal(IGrouping<string, Logistica93_ShipmentIN> rr)
        {
            List<int> DaSommare = new List<int>();
            int l = rr.First().NumeroPedaneEPAL.Length;
            string resp = "";
            foreach (var r in rr)
            {
                DaSommare.Add(int.Parse(r.NumeroPedaneEPAL));
            }
            int sommati = DaSommare.Sum(x => x);
            resp = sommati.ToString();
            while (resp.Length < l)
            {
                resp = resp.Insert(0, "0");
            }
            return resp;
        }
        private string SommaColliLoreal(IGrouping<string, Logistica93_ShipmentIN> rr)
        {
            List<int> DaSommare = new List<int>();
            int l = rr.First().NumeroColliStandard.Length;
            string resp = "";
            foreach (var r in rr)
            {
                DaSommare.Add(int.Parse(r.NumeroColliStandard));
            }
            int sommati = DaSommare.Sum(x => x);
            resp = sommati.ToString();
            while (resp.Length < l)
            {
                resp = resp.Insert(0, "0");
            }
            return resp;
        }
        private string[] AnalizzaRaggruppate(string[] tutteLeRighe)
        {
            var resp = new List<string>();

            for (int i = 0; i < tutteLeRighe.Count(); i++)
            {
                //36-44 ddt
                if (resp.Count > 0)
                {
                    var ultimaInserita = resp.Last();
                    if (tutteLeRighe[i].Substring(36, 8) == ultimaInserita.Substring(36, 8))
                    {

                    }
                }
                else
                {
                    resp.Add(tutteLeRighe[i]);
                }

            }
            return resp.ToArray();
        }
        #endregion

        private void InviaCSVAlServiceManagerByFTP(CustomerSpec cust, List<string> righeCSV, string fn)
        {
            var phardisCust = CustomerConnections.PHARDIS;
            var difarcoCust = CustomerConnections.DIFARCO;

            if (cust == phardisCust || cust == difarcoCust)
            {
                List<string> difarco = new List<string>();
                List<string> phardis = new List<string>();

                bool lastPhardis = false;
                foreach (var riga in righeCSV)
                {
                    if (riga.StartsWith("RTST"))
                    {
                        var pz = riga.Split(';');
                        if (pz[16].StartsWith("PHAR"))
                        {
                            phardis.Add(riga);
                            lastPhardis = true;
                        }
                        else
                        {
                            difarco.Add(riga);
                            lastPhardis = false;
                        }
                    }
                    else
                    {
                        if (lastPhardis)
                        {
                            phardis.Add(riga);
                        }
                        else
                        {
                            difarco.Add(riga);
                        }
                    }

                }

                if (phardis.Count() > 0)
                {
                    var tempphardis = $@"{Path.GetFileNameWithoutExtension(fn)}_{DateTime.Now.ToString("yyyyMMddmmss")}_phardis.csv";
                    File.AppendAllLines(tempphardis, phardis);
                    var destphardis = Path.Combine(phardisCust.RemoteINPath, "DaElaborareGespe", tempphardis);

                    try
                    {
                        _ftp = CreaClientFTPperIlCliente(phardisCust);
                        if (_ftp != null)
                        {
                            _ftp.UploadFile(tempphardis, destphardis);
                        }
                        else
                        {
                            throw new Exception("Errore FTP");
                        }
                    }
                    catch (Exception rr)
                    {
                        var allgato = new List<string>() { tempphardis };
                        GestoreMail.SendMail(allgato, "filemelzo@unitexpress.it,c.mazzone@xcmhealthcare.com", "Errore caricamento FTP PHARDIS", "per un problema di connessione non si è riuscito a caricare automaticamente il file in allegato. provvedere manualmente");
                    }
                    finally
                    {
                        File.Delete(tempphardis);
                    }

                }
                if (difarco.Count() > 0)
                {
                    var tempdifarco = $@"{Path.GetFileNameWithoutExtension(fn)}_{DateTime.Now.ToString("yyyyMMddmmss")}_difarco.csv";
                    File.AppendAllLines(tempdifarco, difarco);
                    var destdifarco = Path.Combine(difarcoCust.RemoteINPath, "DaElaborareGespe", tempdifarco);
                    try
                    {
                        _ftp = CreaClientFTPperIlCliente(difarcoCust);
                        if (_ftp != null)
                        {
                            _ftp.UploadFile(tempdifarco, destdifarco);
                        }
                        else
                        {
                            throw new Exception("Errore FTP");
                        }

                    }
                    catch (Exception rr)
                    {
                        var allgato = new List<string>() { tempdifarco };
                        GestoreMail.SendMail(allgato, "filemelzo@unitexpress.it,p.disa@xcmhealthcare.com", "Errore caricamento FTP DI FARCO", "per un problema di connessione non si è riuscito a caricare automaticamente il file in allegato. provvedere manualmente");
                    }
                    finally
                    {
                        File.Delete(tempdifarco);
                    }
                }

            }
            else
            {
                var temp = $@"{Path.GetFileNameWithoutExtension(fn)}_{DateTime.Now.ToString("yyyyMMddmmss")}.csv";
                File.AppendAllLines(temp, righeCSV);
                var dest = Path.Combine(cust.RemoteINPath, "DaElaborareGespe", temp);
                try
                {
                    _ftp = CreaClientFTPperIlCliente(cust);
                    if (_ftp != null)
                    {
                        _ftp.UploadFile(temp, dest);
                    }
                    else
                    {
                        throw new Exception("Errore FTP");
                    }
                }
                catch (Exception rr)
                {
                    var allgato = new List<string>() { temp };
                    GestoreMail.SendMail(allgato, "filemelzo@unitexpress.it,p.disa@xcmhealthcare.com", $"Errore caricamento FTP {cust.NOME}", "per un problema di connessione non si è riuscito a caricare automaticamente il file in allegato. provvedere manualmente");
                }
                finally
                {
                    File.Delete(temp);
                }
            }

        }

        private IEnumerable<string> ConvertiSpedizioneAPIinEDI(RootobjectNewShipmentTMS ls, CustomerSpec cust)
        {
            var resp = new List<string>();
            var StandardCSV = new Model.UNITEX.UnitexDefaultShipmentHeader();
            {
                StandardCSV.Barcode = "";
                StandardCSV.CashValue = ls.header.cashValue.ToString().Replace(",", ".");
                StandardCSV.CarrierType = ls.header.carrierType;
                StandardCSV.CashCurrency = ls.header.cashCurrency;
                StandardCSV.CashPaymentType = ls.header.cashPayment;
                StandardCSV.externalRef = (cust.ID_GESPE != "00342") ? ls.header.externRef : ls.header.insideRef;
                StandardCSV.GrossWeight = (ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.grossWeight).ToString() : 0.ToString();
                StandardCSV.GoodsValue = "";
                StandardCSV.Incoterm = ls.header.incoterm;
                StandardCSV.Info = ls.header.publicNote.Replace("\"", "").Replace(";", "").Trim();
                StandardCSV.info0 = ls.header.internalNote.Replace("\"", "").Replace(";", "").Trim();
                StandardCSV.InsideRef = (cust.ID_GESPE != "00342") ? ls.header.insideRef : ls.header.externRef;
                StandardCSV.LoadAddress = ls.stops[0].address;
                StandardCSV.LoadDate = ls.stops[0].date;
                StandardCSV.LoadTime = ls.stops[0].time;
                StandardCSV.LoadTown = ls.stops[0].location;
                StandardCSV.LoadZipCode = ls.stops[0].zipCode;
                StandardCSV.Meters = (ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.meters).ToString() : 0.ToString();
                StandardCSV.Packs = (ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.packs).ToString() : 0.ToString();
                StandardCSV.Plts = (ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.floorPallet).ToString() : 0.ToString();
                StandardCSV.PltsTotal = (ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.totalPallet).ToString() : 0.ToString();
                StandardCSV.SegmentName = "RTST";
                StandardCSV.ServiceType = ls.header.serviceType;
                StandardCSV.UnloadAddress = ls.stops[1].address;
                StandardCSV.UnloadCountry = ls.stops[1].country;
                StandardCSV.UnloadDistrict = ls.stops[1].district;
                StandardCSV.UnloadName = ls.stops[1].description;
                StandardCSV.UnloadTown = ls.stops[1].location;
                StandardCSV.UnloadZipCode = ls.stops[1].zipCode;
                StandardCSV.UnloadDate = (ls.stops[1].date == "00000000") ? "" : ls.stops[1].date;
                StandardCSV.UnloadTime = ls.stops[1].time;
                StandardCSV.GoodsDeep = (ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.depth).ToString() : 0.ToString();
                StandardCSV.GoodsHeight = (ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.height).ToString() : 0.ToString();
                StandardCSV.GoodsWidth = (ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.width).ToString() : 0.ToString();
                StandardCSV.Cube = (ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.cube).ToString() : 0.ToString();
                StandardCSV.LoadCountry = ls.stops[0].country;
                StandardCSV.LoadDiscrict = ls.stops[0].district;
                StandardCSV.LoadName = ls.stops[0].description;
                StandardCSV.TransportType = ls.header.transportType;
                StandardCSV.DataTassativa = (ls.stops[1].obligatoryType == "Date") ? "1" : "";
                StandardCSV.DocDate = ls.header.docDate;
                StandardCSV.SpondaIdraulica = ValutaSeOccorreLaSponda(ls, cust);

                if (!string.IsNullOrEmpty(StandardCSV.SpondaIdraulica))
                {
                    GestoreMail.SegnalaInserimentoSpondaIdraulica(StandardCSV);
                }


            }


            resp.Add(StandardCSV.ToString());
            if (ls.parcels != null)
            {
                foreach (var p in ls.parcels)
                {
                    if (string.IsNullOrEmpty(p.barcodeExt))
                    {
                        resp.Add($"RSCI;{p.barcodeMaster}");
                    }
                    else
                    {
                        resp.Add($"RSCI;{p.barcodeExt}");
                    }
                }
            }


            return resp;
        }

        private string ValutaSeOccorreLaSponda(RootobjectNewShipmentTMS ls, CustomerSpec cust)
        {
            if (!CustomerConnections.CDGroup.Contains(cust))
            {
                return "";
            }

            var dest = ls.stops[1].description;

            if (dest.ToLower().Contains("f.cia") || dest.ToLower().Contains("f.cie") ||
                dest.ToLower().Contains("farmacia") || dest.ToLower().Contains("farmacie") ||
                dest.ToLower().Contains("osp.") || dest.ToLower().Contains("ospedale"))
            {
                if (ls.goods[0].floorPallet > 0)
                {
                    return "1";
                }
                else if (ls.goods[0].packs >= 50)
                {
                    return "1";
                }
                else
                {
                    return "";
                }
            }
            return "";
        }

        private IEnumerable<string> ConvertiSpedizioneAPIinEDILoreal(RootobjectNewShipmentTMS ls, CustomerSpec cust, decimal PesoDelyvery, int ColliDelivery, int PltDelivery)
        {
            var resp = new List<string>();
            var StandardCSV = new Model.UNITEX.UnitexDefaultShipmentHeader();
            {
                StandardCSV.Barcode = "";
                StandardCSV.CashValue = ls.header.cashValue.ToString();
                StandardCSV.CarrierType = ls.header.carrierType;
                StandardCSV.CashCurrency = ls.header.cashCurrency;
                StandardCSV.CashPaymentType = ls.header.cashPayment;
                StandardCSV.externalRef = (cust.ID_GESPE != "00342") ? ls.header.externRef : ls.header.insideRef;
                StandardCSV.GrossWeight = PesoDelyvery.ToString();//(ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.grossWeight).ToString() : 0.ToString();
                StandardCSV.GoodsValue = "";
                StandardCSV.Incoterm = ls.header.incoterm;
                StandardCSV.Info = ls.header.publicNote;
                StandardCSV.info0 = ls.header.internalNote;
                StandardCSV.InsideRef = (cust.ID_GESPE != "00342") ? ls.header.insideRef : ls.header.externRef;
                StandardCSV.LoadAddress = ls.stops[0].address;
                StandardCSV.LoadDate = ls.stops[0].date;
                StandardCSV.LoadTime = ls.stops[0].time;
                StandardCSV.LoadTown = ls.stops[0].location;
                StandardCSV.LoadZipCode = ls.stops[0].zipCode;
                StandardCSV.Meters = "0";// (ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.meters).ToString() : 0.ToString();
                StandardCSV.Packs = ColliDelivery.ToString();//(ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.packs).ToString() : 0.ToString();
                StandardCSV.Plts = PltDelivery.ToString();//(ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.floorPallet).ToString() : 0.ToString();
                StandardCSV.PltsTotal = PltDelivery.ToString();//(ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.totalPallet).ToString() : 0.ToString();
                StandardCSV.SegmentName = "RTST";
                StandardCSV.ServiceType = ls.header.serviceType;
                StandardCSV.UnloadAddress = ls.stops[1].address;
                StandardCSV.UnloadCountry = ls.stops[1].country;
                StandardCSV.UnloadDistrict = ls.stops[1].district;
                StandardCSV.UnloadName = ls.stops[1].description;
                StandardCSV.UnloadTown = ls.stops[1].location;
                StandardCSV.UnloadZipCode = ls.stops[1].zipCode;
                StandardCSV.UnloadDate = ls.stops[1].date;
                StandardCSV.UnloadTime = ls.stops[1].time;
                StandardCSV.GoodsDeep = "0"; //(ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.depth).ToString() : 0.ToString();
                StandardCSV.GoodsHeight = "0"; //(ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.height).ToString() : 0.ToString();
                StandardCSV.GoodsWidth = "0";// (ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.width).ToString() : 0.ToString();
                StandardCSV.Cube = (ls.goods != null && ls.goods.Count() > 0) ? ls.goods.Sum(x => x.cube).ToString() : 0.ToString();
                StandardCSV.LoadCountry = ls.stops[0].country;
                StandardCSV.LoadDiscrict = ls.stops[0].district;
                StandardCSV.LoadName = ls.stops[0].description;
                StandardCSV.TransportType = ls.header.transportType;
                StandardCSV.DataTassativa = (ls.stops[1].obligatoryType == "Date") ? "1" : "";

            }


            resp.Add(StandardCSV.ToString());
            if (ls.parcels != null)
            {
                foreach (var p in ls.parcels)
                {
                    resp.Add($"RSCI;{p.barcodeExt}");
                }
            }


            return resp;
        }

        private FtpClient CreaClientFTPperIlCliente(CustomerSpec cust)
        {
            try
            {
                var ftp = new FtpClient(cust.FTP_Address, cust.FTP_Port, cust.userFTP, cust.pswFTP);
                ftp.Connect();
                return ftp;
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev($"CreaClientFTPperIlCliente-{cust.NOME}", ee);
                }

                LastException = ee;
                return null;
            }
        }
        private SftpClient CreaClientSFTPperIlCliente(CustomerSpec cust)
        {
            /*
             * Quando veniva messo in "release" spesso e volentieri non riusciva collegarsi e dava exception, in debug
             * non ha mai dato questo problema, si pensa che la causa si trovi nella libreria terzi FluentFTP che usiamo
             * per connetterci ai server, la soluzione più elegante al problema è questa
             */
            int attempts = 0;

            do
            {
                try
                {
                    attempts++;

                    var ssh = new SftpClient(cust.sftpAddress, cust.sftpUsername, cust.sftpPassword);
                    ssh.OperationTimeout = TimeSpan.FromSeconds(30);
                    ssh.Connect();

                    //ssh.ChangeDirectory(cust.PathEsitiDelCliente);
                    return ssh;
                }
                catch (System.Net.Sockets.SocketException)
                {
                    continue;
                }
            }
            while (attempts <= 5);

            string errorMsg = $"Non sono riuscito a connettermi all'FTP di {cust.NOME}";

            _loggerCode.Error(errorMsg);

            //if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
            //{
            //    DateLastException = DateTime.Now;
            GestoreMail.SegnalaErroreDev($"CreaClientFTPperIlCliente-{cust.NOME}", errorMsg);
            //}

            //LastException = ee;
            return null;
        }

        #region OBSOLETO
        //private bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //{
        //    return true;
        //}

        //private void OnValidateCertificate(FtpClient control, FtpSslValidationEventArgs e)
        //{
        //    e.Accept = true;
        //}

        //private List<string> PrimoEdUltimoSegnacollo(EspritecParcel.RootobjectParcel parDes, EspritecShipment.RootobjectEspritecShipment shDes, EspritecTrip.TripStop s)
        //{
        //    var resp = new List<string>();
        //    string intSH = shDes.shipment.docNumber.Split('/')[0];
        //    var primo = "";
        //    if (intSH.Length == 5)
        //    {
        //        primo = intSH + "001";
        //    }
        //    else if (intSH.Length > 5)
        //    {
        //        primo = intSH.Substring(intSH.Length - 5, intSH.Length) + "001";
        //    }

        //    string scl = parDes.parcel.Count().ToString();

        //    while (scl.Length < 3)
        //    {
        //        scl = scl.Insert(0, "0");
        //    }
        //    resp.Add(primo);
        //    resp.Add(intSH + scl);
        //    return resp;
        //}

        //private FtpClient CostruisciFTPperilFornitore(string carrierID)
        //{
        //    try
        //    {
        //        var tp = TPConnection.TPs.FirstOrDefault(X => X.ID_FORNITORE_GESPE == carrierID);
        //        if (tp != null)
        //        {
        //            var ftp = new FtpClient(tp.FTP_Address, tp.FTP_Port, tp.user, tp.psw);
        //            ftp.Connect();
        //            return ftp;
        //        }
        //        else
        //        {
        //            throw new Exception($"ID {carrierID} fornitore non trovato nei fornitori anagrafati");
        //        }

        //    }
        //    catch (Exception ee)
        //    {
        //        _loggerCode.Error(ee);

        //        if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
        //        {
        //            DateLastException = DateTime.Now;
        //            GestoreMail.SegnalaErroreDev("CostruisciFTPperilFornitore", ee);
        //        }

        //        LastException = ee;
        //        return null;
        //    };
        //}

        //private void InviaNuovaShipmentAPI_UNITEX(RootobjectNewShipmentTMS shipmentTMS)
        //{
        //    var ch = JsonConvert.SerializeObject(shipmentTMS, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        //    var client = new RestClient(endpointAPI_UNITEX + "/api/tms/shipment/new");
        //    client.Timeout = -1;
        //    var request = new RestRequest(Method.PUT);
        //    request.AddHeader("Content-Type", "application/json-patch+json");
        //    request.AddHeader("Cache-Control", "no-cache");
        //    request.AddHeader("Authorization", $"Bearer {token_UNITEX}");
        //    request.AddJsonBody(ch);

        //    //request.AddParameter("application/json-patch+json", ParameterType.RequestBody);
        //    client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
        //    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        //    IRestResponse response = client.Execute(request);

        //    //var resp = JsonConvert.DeserializeObject<RootobjectNewShipmentTMSRespone>(response.Content);

        //}
        //public int DiffTraDateEsclusiSabatoEDomeniche(DateTime sdt, DateTime edt)
        //{
        //    int amount = 0;
        //    int sdayIndex = (int)sdt.DayOfWeek;
        //    int edayIndex = (int)edt.DayOfWeek;

        //    amount += (sdayIndex == 0) ? 5 : (6 - sdayIndex);
        //    amount += (edayIndex == 6) ? 5 : edayIndex;

        //    sdt = sdt.AddDays(7 - sdayIndex);
        //    edt = edt.AddDays(-edayIndex);

        //    if (sdt > edt)
        //        amount -= 5;
        //    else
        //        amount += (edt.Subtract(sdt)).Days / 7 * 5;

        //    return amount - 1;
        //} 
        #endregion

        private void UnitexGespeAPILogin(string userAPI, string passwordAPI, out string token, out DateTime scadenzaToken)
        {
            try
            {
                _loggerAPI.Info($"Autenticazione {userAPI} in corso su endpoint {endpointAPI_UNITEX}");
                var client = new RestClient(endpointAPI_UNITEX + "/api/token");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json-patch+json");
                request.AddHeader("Cache-Control", "no-cache");
                var body = @"{" + "\n" +
                $@"  ""username"": ""{userAPI}""," + "\n" +
                $@"  ""password"": ""{passwordAPI}""," + "\n" +
                @"  ""tenant"": ""UNITEX""" + "\n" +
                @"}";
                request.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse response = client.Execute(request);
                var resp = JsonConvert.DeserializeObject<RootobjectLoginUNITEX>(response.Content);

                scadenzaToken = resp.user.expire;
                token = resp.user.token;

                //_loggerAPI.Info($"Nuovo token: {token_UNITEX}");

            }
            catch (Exception ee)
            {
                scadenzaToken = DateTime.MinValue;
                token = "";
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("UnitexGespeAPILogin", ee);
                }
                LastException = ee;
            }
        }
        public void Stop()
        {
            timerAggiornamentoCiclo.Stop();
        }
    }

    public static class DataRowConverter
    {
        public static T ToObject<T>(this DataRow dataRow)
    where T : new()
        {
            T item = new T();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                PropertyInfo property = GetProperty(typeof(T), column.ColumnName);

                if (property != null && dataRow[column] != DBNull.Value && dataRow[column].ToString() != "NULL")
                {
                    property.SetValue(item, ChangeType(dataRow[column], property.PropertyType), null);
                }
            }

            return item;
        }
        public static object ChangeType(object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
            }

            return Convert.ChangeType(value, type);
        }
        private static PropertyInfo GetProperty(Type type, string attributeName)
        {
            PropertyInfo property = type.GetProperty(attributeName);

            if (property != null)
            {
                return property;
            }

            return type.GetProperties()
                 .Where(p => p.IsDefined(typeof(DisplayAttribute), false) && p.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single().Name == attributeName)
                 .FirstOrDefault();
        }
    }
    public class ModelTempiResa
    {
        internal string Cliente { get; set; }
        internal string CodiceHubUnitex { get; set; }
        internal DateTime DataCarico { get; set; }
        internal string RifEsterno { get; set; }
        internal DateTime DataConsegna { get; set; }
        internal int TempoResaReale { get; set; }
        internal string LocalitaConsegna { get; set; }
        internal string Destinatario { get; set; }
        internal int Colli { get; set; }
        internal int Pallet { get; set; }
        internal decimal Peso { get; set; }
        internal string NDocumento { get; set; }
        internal string StatoConsegna { get; set; }
        internal string ProvinciaConsegna { get; set; }
        internal string VettoreConsegna { get; set; }
        internal string TipoSLA { get; set; }
        internal bool SLAKPI { get; set; }
        internal string Regione
        {
            get
            {
                var regGT = Automazione.GeoTab.FirstOrDefault(x => x.provincia == this.ProvinciaConsegna);
                if (regGT != null)
                {
                    return regGT.regione;
                }
                return "N/D";
            }
        }
        internal string Mandante { get; set; }
        //internal bool ConsegnaInTempo { get { return this.TempoResaReale <= ResaMax; } }
        //internal int ResaMax { get { return (int)(DataConsegnaPrevistaMax - DataCarico).TotalDays; } }
        //internal DateTime DataConsegnaPrevistaMax
        //{
        //    get
        //    {
        //        return LocalGoogleCalendar.CalcolaDataConsegnaPrevista(DataCarico.Date, this.ResaMax);
        //    }
        //}
        internal int KPIOK => (TempoResaReale <= ResaMax || SLAKPI) ? 1 : 0;
        internal int KPIKO => (TempoResaReale > ResaMax && !SLAKPI) ? 1 : 0;
        internal int ResaMax { get; set; }
        internal DateTime DataConsegnaPrevistaMax { get; set; }
        internal bool LocalitaDisagiata { get; set; }

        public override string ToString()
        {
            return $"{this.Cliente};{this.Mandante};{this.CodiceHubUnitex};{this.RifEsterno};{this.NDocumento};{this.DataCarico.ToString("dd/MM/yyyy")};{this.StatoConsegna};{this.DataConsegna.ToString("dd/MM/yyyy")};{this.TempoResaReale};{this.ResaMax};{this.KPIOK};{this.KPIKO};{this.LocalitaDisagiata};{this.Regione};{this.ProvinciaConsegna};{this.LocalitaConsegna};{this.Destinatario};{this.Colli};{this.Pallet};{this.Peso};{this.VettoreConsegna};{this.SLAKPI};{this.TipoSLA}";
        }

    }
}