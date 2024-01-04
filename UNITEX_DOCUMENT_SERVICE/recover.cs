// Decompiled with JetBrains decompiler
// Type: UNITEX_DOCUMENT_SERVICE.Automazione
// Assembly: UNITEX_DOCUMENT_SERVICE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AFD06402-D347-4472-9A07-97957C1F62F3
// Assembly location: C:\Users\Piero\Desktop\UNITEX_DOCUMENT_SERVICE.exe

using CommonAPITypes.ESPRITEC;
using CommonAPITypes.UNITEX;
using FluentFTP;
using Newtonsoft.Json;
using NLog;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Timers;
using UNITEX_DOCUMENT_SERVICE.Code;
using UNITEX_DOCUMENT_SERVICE.Model;
using UNITEX_DOCUMENT_SERVICE.Model._3C;
using UNITEX_DOCUMENT_SERVICE.Model.CDGROUP;
using UNITEX_DOCUMENT_SERVICE.Model.Chiapparoli;
using UNITEX_DOCUMENT_SERVICE.Model.DAMORA;
using UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX;
using UNITEX_DOCUMENT_SERVICE.Model.GUNA;
using UNITEX_DOCUMENT_SERVICE.Model.Logistica93;
using UNITEX_DOCUMENT_SERVICE.Model.PoolPharmaDLF;
using UNITEX_DOCUMENT_SERVICE.Model.STMGroup;
using UNITEX_DOCUMENT_SERVICE.Model.StockHouse;
using UNITEX_DOCUMENT_SERVICE.Model.UNITEX;

namespace UNITEX_DOCUMENT_SERVICE2
{
    public class Automazione
    {
        private static string endpointAPI_UNITEX = "https://010761.espritec.cloud:9500";
        private static string userAPIADMIN = "pdisa";
        private static string passwordAPIADMIN = "Pd$2022!";
        private Exception LastException = new Exception("AVVIO");
        private DateTime DateLastException = DateTime.MinValue;
        private DateTime LastCheckChangesTMS = DateTime.MinValue;
        private DateTime DataScadenzaToken_UNITEX = DateTime.MinValue;
        private string token_UNITEX = "";
        private FtpClient _ftp = (FtpClient)null;
        internal static Logger _loggerCode = LogManager.GetLogger("loggerCode");
        internal static Logger _loggerAPI = LogManager.GetLogger("LogAPI");
        public static string FileEsitiDaRecuperareCDGroup = "RecuperaEsitiCDGroup.txt";
        public static string FileEsitiDaRecuperareGIMA = "RecuperaEsitiGima.txt";
        public static string FileEsitiDaRecuperareLoreal = "RecuperaEsitiLoreal.txt";
        public static string FileEsitiDaRecuperareSTM = "RecuperaEsitiSTM.txt";
        private List<CDGROUP_EsitiOUT> EsitiDaCoumicareCDGroup = new List<CDGROUP_EsitiOUT>();
        private List<CDGROUP_EsitiOUT> EsitiDaRecuperareParzialiCDGroup = ((IEnumerable<string>)System.IO.File.ReadAllLines(Automazione.FileEsitiDaRecuperareCDGroup)).Select<string, CDGROUP_EsitiOUT>((Func<string, CDGROUP_EsitiOUT>)(x => CDGROUP_EsitiOUT.FromCsv(x))).ToList<CDGROUP_EsitiOUT>();
        private List<STM_EsitiOut> EsitiDaRecuperareParzialiSTM = ((IEnumerable<string>)System.IO.File.ReadAllLines(Automazione.FileEsitiDaRecuperareSTM)).Select<string, STM_EsitiOut>((Func<string, STM_EsitiOut>)(x => STM_EsitiOut.FromCsv(x))).ToList<STM_EsitiOut>();
        private List<LorealEsiti> EsitiDaRecuperareParzialiLoreal = ((IEnumerable<string>)System.IO.File.ReadAllLines(Automazione.FileEsitiDaRecuperareLoreal)).Select<string, LorealEsiti>((Func<string, LorealEsiti>)(x => LorealEsiti.FromCsv(x))).ToList<LorealEsiti>();
        private List<STM_EsitiOut> EsitiDaCoumicareSTM = new List<STM_EsitiOut>();
        private List<LorealEsiti> EsitiDaComunicareLoreal = new List<LorealEsiti>();
        private List<DAMORA_EsitiOUT> EsitiDaComunicareDamora = new List<DAMORA_EsitiOUT>();
        private List<_3C_EsitiOUT> EsitiDaComunicare_3C = new List<_3C_EsitiOUT>();
        private List<Chiapparoli_EsitiOUT> EsitiDaComunicareChiapparoli = new List<Chiapparoli_EsitiOUT>();
        public static string CodiciStatoFileNameCDGroup = "CodiciStatoCDGROUP.csv";
        public static string CodiciStatoFileNameSTM = "CodiciStatoSTM.csv";
        public static string CodiciStatoFileNameLoreal = "CodiciStatoLoreal.csv";
        public static string CodiciStatoFilenameChiapparoli = "CodiciStatoChiapparoli.csv";
        public static string GEO_TAB = "GEO.csv";
        private List<GeoClass> GeoTab = ((IEnumerable<string>)System.IO.File.ReadAllLines(Automazione.GEO_TAB)).Select<string, GeoClass>((Func<string, GeoClass>)(x => GeoClass.FromCsv(x))).ToList<GeoClass>();
        private List<CDGROUP_StatiDocumento> statiDocumemtoCDGroup = ((IEnumerable<string>)System.IO.File.ReadAllLines(Automazione.CodiciStatoFileNameCDGroup)).Select<string, CDGROUP_StatiDocumento>((Func<string, CDGROUP_StatiDocumento>)(x => CDGROUP_StatiDocumento.FromCsv(x))).ToList<CDGROUP_StatiDocumento>();
        private List<STM_StatiDocumento> statiDocumemtoSTM = ((IEnumerable<string>)System.IO.File.ReadAllLines(Automazione.CodiciStatoFileNameSTM)).Select<string, STM_StatiDocumento>((Func<string, STM_StatiDocumento>)(x => STM_StatiDocumento.FromCsv(x))).ToList<STM_StatiDocumento>();
        private List<Logistica93_StatiDocumento> statiDocumemtoLoreal = ((IEnumerable<string>)System.IO.File.ReadAllLines(Automazione.CodiciStatoFileNameLoreal)).Select<string, Logistica93_StatiDocumento>((Func<string, Logistica93_StatiDocumento>)(x => Logistica93_StatiDocumento.FromCsv(x))).ToList<Logistica93_StatiDocumento>();
        private List<Chiapparoli_StatiDocumento> statiDocumemtoChiapparoli = ((IEnumerable<string>)System.IO.File.ReadAllLines(Automazione.CodiciStatoFilenameChiapparoli)).Select<string, Chiapparoli_StatiDocumento>((Func<string, Chiapparoli_StatiDocumento>)(x => Chiapparoli_StatiDocumento.FromCsv(x))).ToList<Chiapparoli_StatiDocumento>();
        private System.Timers.Timer timerAggiornamentoCiclo = new System.Timers.Timer();
        private double cicloTimer = 60000.0;
        private System.Timers.Timer timerEsiti = new System.Timers.Timer();
        private double cicloTimerEsitiCDGroup = 600000.0;
        public static string config = "config.ini";
        private string PathLastCheckChangesFileTMS = "LastCheckChangesTMS.txt";
        private List<long> CambiTackingGiaNotificati = new List<long>();
        public static string appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UNITEX_DOCUMENT_SERVICE");
        private object semaphoro = new object();

        public void Start()
        {
            this.CaricaConfigurazioni();
            this.CheckCustomerPath();
            this.RecuperaConnessione();
            this.SetTimer();
            this.OnTimedEvent((object)null, (ElapsedEventArgs)null);
            this.OnTimedEventCambiTMS((object)null, (ElapsedEventArgs)null);
        }

        private void Test() => this.RecuperaEsitiCdGroup();

        private void VerificaTrasmissioneEsitiCDGroup()
        {
            List<string> source1 = new List<string>();
            List<string> contents = new List<string>();
            string[] files = Directory.GetFiles("C:\\FTP\\CLIENTI\\CD_GROUP_ESITI\\OUT\\Save");
            string[] source2 = System.IO.File.ReadAllLines("C:\\FTP\\CLIENTI\\CD_GROUP_ESITI\\OUT\\TRACK_20230418120610.txt");
            string str1 = "Data DDT|Rif. Cliente|Stato|Nome File trasmesso|Data Esito|Data Trasmissione|Diff giorni Esito/Trasm|Vettore";
            contents.Add(str1);
            foreach (string path in files)
            {
                string fileName = Path.GetFileName(path);
                foreach (string readAllLine in System.IO.File.ReadAllLines(path))
                {
                    DateTime exact = DateTime.ParseExact(fileName.Split('_')[1].Split('.')[0], "yyyyMMddHHmmss", (IFormatProvider)null);
                    string str2 = readAllLine.Substring(79, 30).Trim();
                    DateTime result1 = DateTime.MinValue;
                    DateTime.TryParseExact(readAllLine.Substring(30, 8), "yyyyMMdd", (IFormatProvider)null, DateTimeStyles.None, out result1);
                    DateTime result2 = DateTime.MinValue;
                    DateTime.TryParseExact(readAllLine.Substring(109, 8), "yyyyMMdd", (IFormatProvider)null, DateTimeStyles.None, out result2);
                    if (!(str2.ToUpper() != "CONSEGNATA") && !(result2 == DateTime.MinValue))
                    {
                        int days = (exact - result2).Days;
                        string str3 = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", (object)result1, (object)readAllLine.Substring(15, 10), (object)str2, (object)fileName, (object)result2.ToString("dd/MM/yyyy"), (object)exact.ToString("dd/MM/yyyy"), (object)days);
                        source1.Add(str3);
                    }
                }
            }
            int num1 = 0;
            int num2 = ((IEnumerable<string>)source2).Count<string>();
            foreach (string str4 in source2)
            {
                ++num1;
                Debug.WriteLine(string.Format("{0}-{1}", (object)num1, (object)num2));
                string str = str4.Substring(15, 10);
                string str5 = source1.FirstOrDefault<string>((Func<string, bool>)(x => x.Contains("|" + str + "|")));
                if (str5 != null)
                {
                    EspritecShipment.RootobjectShipmentList rootobjectShipmentList = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(EspritecShipment.RestEspritecGetShipmentListByExternalRef(str, 1, 1, this.token_UNITEX).Content);
                    string str6 = "";
                    if (rootobjectShipmentList.shipments != null)
                        str6 = rootobjectShipmentList.shipments[0].deliverySupplierDes;
                    contents.Add(str5 + "|" + str6);
                    Debug.WriteLine(str5 + "|" + str6);
                }
            }
            System.IO.File.WriteAllLines("EsitiComunicati.txt", (IEnumerable<string>)contents);
        }

        private void VerificaTempiDiResaUNITEX(CustomerSpec cust, DateTime dataDa, DateTime dataA)
        {
            List<string> contents = new List<string>();
            List<EspritecShipment.ShipmentList> source = new List<EspritecShipment.ShipmentList>();
            int num1 = 500;
            int num2 = 1;
            for (int index = 2; num2 <= index; ++num2)
            {
                EspritecShipment.RootobjectShipmentList rootobjectShipmentList = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(EspritecShipment.RestEspritecGetShipmentListTraDate(dataDa, dataA, num1, num2, cust.tokenAPI).Content);
                source.AddRange((IEnumerable<EspritecShipment.ShipmentList>)rootobjectShipmentList.shipments);
                if (index == 2)
                    index = rootobjectShipmentList.result.maxPages;
                Debug.WriteLine(string.Format("{0}-{1}", (object)num2, (object)index));
            }
            int num3 = ((IEnumerable<EspritecShipment.ShipmentList>)source).Count<EspritecShipment.ShipmentList>();
            int num4 = 1;
            GoogleCalendar googleCalendar = new GoogleCalendar();
            foreach (EspritecShipment.ShipmentList shipmentList in source)
            {
                EspritecShipment.RootobjectShipmentTracking shipmentTracking1 = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentTracking>(EspritecShipment.RestEspritecGetTracking((long)shipmentList.id, this.token_UNITEX).Content);
                if (shipmentTracking1.events == null)
                {
                    Debug.WriteLine("NO TRACKING");
                }
                else
                {
                    EspritecShipment.EventShipmentTracking shipmentTracking2 = ((IEnumerable<EspritecShipment.EventShipmentTracking>)shipmentTracking1.events).FirstOrDefault<EspritecShipment.EventShipmentTracking>((Func<EspritecShipment.EventShipmentTracking, bool>)(x => x.statusID == 30));
                    if (shipmentTracking2 != null)
                    {
                        EspritecShipmentStops.RootobjectShipmentStops rootobjectShipmentStops = JsonConvert.DeserializeObject<EspritecShipmentStops.RootobjectShipmentStops>(EspritecShipmentStops.RestEspritecGetShipStop((long)shipmentList.id, this.token_UNITEX).Content);
                        DateTime dateTime1 = DateTime.Parse(shipmentTracking2.timeStamp.ToString());
                        int num5 = googleCalendar.GiorniDiResaEffettivi(shipmentList.docDate, dateTime1.Date);
                        Automazione.ModelTempiResa modelTempiResa1 = new Automazione.ModelTempiResa();
                        DateTime? date = rootobjectShipmentStops.stops[0].date;
                        DateTime dateTime2;
                        string str;
                        if (!date.HasValue)
                        {
                            dateTime2 = shipmentList.docDate;
                            dateTime2 = dateTime2.Date;
                            str = dateTime2.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            date = rootobjectShipmentStops.stops[0].date;
                            dateTime2 = date.Value;
                            dateTime2 = dateTime2.Date;
                            str = dateTime2.ToString("dd/MM/yyyy");
                        }
                        modelTempiResa1.DataCarico = str;
                        modelTempiResa1.DataConsegna = dateTime1.ToString("dd/MM/yyyy");
                        modelTempiResa1.RifEsterno = shipmentList.externRef;
                        modelTempiResa1.TempiResa = num5.ToString();
                        modelTempiResa1.LocalitaConsegna = shipmentList.lastStopLocation;
                        modelTempiResa1.ProvinciaConsegna = shipmentList.lastStopDistrict;
                        modelTempiResa1.VettoreConsegna = shipmentList.deliverySupplierDes;
                        Automazione.ModelTempiResa modelTempiResa2 = modelTempiResa1;
                        contents.Add(modelTempiResa2.ToString());
                        Debug.WriteLine(modelTempiResa2.ToString());
                        Debug.WriteLine(string.Format("{0}-{1}", (object)num4, (object)num3));
                        ++num4;
                    }
                }
            }
            System.IO.File.WriteAllLines("ConsuntivoTempiDiResa_" + cust.NOME + "_" + dataDa.ToString("dd_MM_yyyy") + "_" + dataA.ToString("dd_MM_yyyy") + ".txt", (IEnumerable<string>)contents);
        }

        private void RecuperaRitiriCDGroup()
        {
            string[] strArray1 = System.IO.File.ReadAllLines("C:\\Users\\Piero\\Desktop\\RecuperoCDGroup\\rit\\rit.txt");
            List<Automazione.RitiriCDGroupBK> source1 = new List<Automazione.RitiriCDGroupBK>();
            foreach (string str in strArray1)
            {
                char[] chArray = new char[1] { ';' };
                string[] strArray2 = str.Split(chArray);
                Automazione.RitiriCDGroupBK ritiriCdGroupBk = new Automazione.RitiriCDGroupBK()
                {
                    data = strArray2[0],
                    extref = strArray2[1],
                    colli = strArray2[2],
                    peso = strArray2[3],
                    volume = strArray2[4],
                    plt = strArray2[5]
                };
                source1.Add(ritiriCdGroupBk);
            }
            string[] source2 = System.IO.File.ReadAllLines("cdGroupSHID.txt");
            int num1 = ((IEnumerable<string>)source2).Count<string>();
            int num2 = 0;
            foreach (string s in source2)
            {
                ++num2;
                int num3 = int.Parse(s);
                EspritecShipment.RootobjectEspritecShipment shipDes = JsonConvert.DeserializeObject<EspritecShipment.RootobjectEspritecShipment>(EspritecShipment.RestEspritecGetShipment((long)num3, this.token_UNITEX).Content);
                if (shipDes.result.status)
                {
                    EspritecGoods.RootobjectGoodsList rootobjectGoodsList = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsList>(EspritecGoods.RestEspritecGetGoodListOfShipment(num3, this.token_UNITEX).Content);
                    if (rootobjectGoodsList.goods != null && ((IEnumerable<EspritecGoods.GoodList>)rootobjectGoodsList.goods).Count<EspritecGoods.GoodList>() > 0)
                    {
                        Automazione.RitiriCDGroupBK ritiriCdGroupBk = source1.FirstOrDefault<Automazione.RitiriCDGroupBK>((Func<Automazione.RitiriCDGroupBK, bool>)(x => x.extref == shipDes.shipment.externRef));
                        Debug.WriteLine(string.Format("{0}-{1}", (object)num2, (object)num1));
                        if (ritiriCdGroupBk == null)
                        {
                            Automazione._loggerAPI.Info(string.Format("ID NON TROVATO|{0}|{1}", (object)shipDes.shipment.id, (object)shipDes.shipment.externRef));
                            Debug.WriteLine(string.Format("ID NON TROVATO|{0}|{1}", (object)shipDes.shipment.id, (object)shipDes.shipment.externRef));
                        }
                        else if (((IEnumerable<EspritecGoods.GoodList>)rootobjectGoodsList.goods).Sum<EspritecGoods.GoodList>((Func<EspritecGoods.GoodList, Decimal>)(x => x.grossWeight)) > 0M)
                        {
                            Debug.WriteLine(string.Format("PESO PRESENTE{0}|{1}", (object)shipDes.shipment.id, (object)shipDes.shipment.externRef));
                        }
                        else
                        {
                            foreach (EspritecGoods.GoodList good in rootobjectGoodsList.goods)
                            {
                                Debug.WriteLine(string.Format("{0}-{1}-{2}", (object)num2, (object)num1, (object)good.cube));
                                EspritecGoods.RootobjectGoodsUpdate rootobjectGoodsUpdate = new EspritecGoods.RootobjectGoodsUpdate();
                                EspritecGoods.GoodsUpdate goodsUpdate = new EspritecGoods.GoodsUpdate();
                                double num4 = new Random().NextDouble(0.045, 0.069);
                                good.packs = goodsUpdate.packs = int.Parse(ritiriCdGroupBk.colli);
                                Decimal num5 = (Decimal)num4 * (Decimal)good.packs;
                                string peso = ritiriCdGroupBk.peso;
                                string plt = ritiriCdGroupBk.plt;
                                goodsUpdate.id = good.id;
                                goodsUpdate.cube = num5;
                                goodsUpdate.grossWeight = Decimal.Parse(peso);
                                goodsUpdate.floorPallet = goodsUpdate.totalPallet = int.Parse(plt);
                                Debug.WriteLine(string.Format("{0}-{1}-{2}-{3}-{4}-{5}", (object)num2, (object)num1, (object)goodsUpdate.cube, (object)goodsUpdate.grossWeight, (object)goodsUpdate.packs, (object)goodsUpdate.totalPallet));
                                rootobjectGoodsUpdate.goods = goodsUpdate;
                                EspritecGoods.RootobjectGoodsUpdateResponse goodsUpdateResponse = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsUpdateResponse>(EspritecGoods.RestEspritecUpdateGoods(rootobjectGoodsUpdate, this.token_UNITEX).Content);
                                if (goodsUpdateResponse == null || goodsUpdateResponse.status)
                                    ;
                            }
                        }
                    }
                }
            }
        }

        private void RecuperSpedizioniDaGespe(DateTime DaData)
        {
            List<EspritecShipment.ShipmentList> source = new List<EspritecShipment.ShipmentList>();
            foreach (CustomerSpec customer in CustomerConnections.customers)
            {
                CustomerSpec cust = customer;
                if (CustomerConnections.CDGroup.Any<CustomerSpec>((Func<CustomerSpec, bool>)(x => x.ID_GESPE == cust.ID_GESPE)))
                {
                    int num = 1;
                    IRestResponse shipmentList = EspritecShipment.RestEspritecGetShipmentList(DaData, 1000, num, cust.tokenAPI);
                    EspritecShipment.RootobjectShipmentList rootobjectShipmentList = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(shipmentList.Content);
                    int maxPages = rootobjectShipmentList.result.maxPages;
                    source.AddRange((IEnumerable<EspritecShipment.ShipmentList>)rootobjectShipmentList.shipments);
                    while (num < maxPages)
                    {
                        ++num;
                        Debug.WriteLine(num.ToString() + "-" + maxPages.ToString());
                        EspritecShipment.RestEspritecGetShipmentList(DaData, 100, num, cust.tokenAPI);
                        JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(shipmentList.Content);
                        source.AddRange((IEnumerable<EspritecShipment.ShipmentList>)rootobjectShipmentList.shipments);
                    }
                }
            }
            List<EspritecShipment.ShipmentList> list = ((IEnumerable<EspritecShipment.ShipmentList>)source).Where<EspritecShipment.ShipmentList>((Func<EspritecShipment.ShipmentList, bool>)(x => x.firstStopLocation.ToLower() == "vimercate" && x.firstStopDistrict.ToLower() == "mi")).ToList<EspritecShipment.ShipmentList>();
            List<string> contents = new List<string>();
            foreach (EspritecShipment.ShipmentList shipmentList in list)
                contents.Add(shipmentList.id.ToString());
            System.IO.File.WriteAllLines("idRilevati.txt", (IEnumerable<string>)contents);
        }

        private void RecuperaPesoVolumePalletCDGroup()
        {
            List<string> contents = new List<string>();
            List<int> list = ((IEnumerable<string>)System.IO.File.ReadAllLines("cdGroupSHID.txt")).Select<string, int>((Func<string, int>)(x => int.Parse(x))).ToList<int>();
            Random random = new Random();
            foreach (int num1 in list)
            {
                EspritecGoods.RootobjectGoodsList rootobjectGoodsList = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsList>(EspritecGoods.RestEspritecGetGoodListOfShipment(num1, this.token_UNITEX).Content);
                EspritecShipment.RootobjectEspritecShipment espritecShipment = JsonConvert.DeserializeObject<EspritecShipment.RootobjectEspritecShipment>(EspritecShipment.RestEspritecGetShipment((long)num1, this.token_UNITEX).Content);
                if (rootobjectGoodsList != null)
                {
                    int num2 = ((IEnumerable<EspritecGoods.GoodList>)rootobjectGoodsList.goods).Sum<EspritecGoods.GoodList>((Func<EspritecGoods.GoodList, int>)(x => x.floorPallet));
                    Decimal num3 = ((IEnumerable<EspritecGoods.GoodList>)rootobjectGoodsList.goods).Sum<EspritecGoods.GoodList>((Func<EspritecGoods.GoodList, Decimal>)(x => x.grossWeight));
                    Decimal num4 = ((IEnumerable<EspritecGoods.GoodList>)rootobjectGoodsList.goods).Sum<EspritecGoods.GoodList>((Func<EspritecGoods.GoodList, Decimal>)(x => x.cube));
                    bool flag = num4 > 0M;
                    List<double> source1 = new List<double>();
                    List<Decimal> source2 = new List<Decimal>();
                    for (int index = 0; index < num2; ++index)
                    {
                        double num5 = random.NextDouble(1.3, 1.5);
                        Decimal num6 = flag ? num4 : (Decimal)num5;
                        if (!flag)
                            source2.Add(num6);
                        else if (index == 0)
                            source2.Add(num6);
                        source1.Add(random.NextDouble(197.0, 199.98));
                    }
                    if (JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsUpdateResponse>(EspritecGoods.RestEspritecUpdateGoods(new EspritecGoods.RootobjectGoodsUpdate()
                    {
                        goods = new EspritecGoods.GoodsUpdate()
                        {
                            id = rootobjectGoodsList.goods[0].id,
                            cube = source2.Sum(),
                            grossWeight = (Decimal)source1.Sum(),
                            totalPallet = 0,
                            floorPallet = 0,
                            packs = num2
                        }
                    }, this.token_UNITEX).Content).status)
                    {
                        string message = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", (object)num1, (object)espritecShipment.shipment.docNumber, (object)espritecShipment.shipment.externRef, (object)num3, (object)source1.Sum(), (object)num4, (object)source2.Sum(), (object)num2);
                        Debug.WriteLine(message);
                        contents.Add(message);
                    }
                }
            }
            System.IO.File.WriteAllLines("modificheVolCDGroup.txt", (IEnumerable<string>)contents);
        }

        private void RecuperaVolumeCDGroup()
        {
            string[] source = System.IO.File.ReadAllLines("cdGroupSHID.txt");
            int num1 = ((IEnumerable<string>)source).Count<string>();
            int num2 = 0;
            foreach (string s in source)
            {
                ++num2;
                int num3 = int.Parse(s);
                if (JsonConvert.DeserializeObject<EspritecShipment.RootobjectEspritecShipment>(EspritecShipment.RestEspritecGetShipment((long)num3, this.token_UNITEX).Content).result.status)
                {
                    EspritecGoods.RootobjectGoodsList rootobjectGoodsList = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsList>(EspritecGoods.RestEspritecGetGoodListOfShipment(num3, this.token_UNITEX).Content);
                    if (rootobjectGoodsList.goods != null && ((IEnumerable<EspritecGoods.GoodList>)rootobjectGoodsList.goods).Count<EspritecGoods.GoodList>() > 0)
                    {
                        foreach (EspritecGoods.GoodList good in rootobjectGoodsList.goods)
                        {
                            Debug.WriteLine(string.Format("{0}-{1}-{2}", (object)num2, (object)num1, (object)good.cube));
                            EspritecGoods.RootobjectGoodsUpdate rootobjectGoodsUpdate = new EspritecGoods.RootobjectGoodsUpdate();
                            EspritecGoods.GoodsUpdate goodsUpdate = new EspritecGoods.GoodsUpdate();
                            Decimal num4 = (Decimal)new Random().NextDouble(0.045, 0.069) * (Decimal)good.packs;
                            goodsUpdate.id = good.id;
                            goodsUpdate.cube = num4;
                            Debug.WriteLine(string.Format("{0}-{1}-{2}-{3}-{4}-{5}", (object)num2, (object)num1, (object)goodsUpdate.cube, (object)goodsUpdate.grossWeight, (object)goodsUpdate.packs, (object)goodsUpdate.totalPallet));
                            rootobjectGoodsUpdate.goods = goodsUpdate;
                            EspritecGoods.RootobjectGoodsUpdateResponse goodsUpdateResponse = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsUpdateResponse>(EspritecGoods.RestEspritecUpdateGoods(rootobjectGoodsUpdate, this.token_UNITEX).Content);
                            if (goodsUpdateResponse == null || goodsUpdateResponse.status)
                                ;
                        }
                    }
                }
            }
            List<string> stringList = new List<string>();
        }

        private Decimal RecuperaIlVolumeInBaseAlPeso(Decimal grossWeight)
        {
            Random random = new Random();
            if (grossWeight > 150M && grossWeight < 200M)
                return (Decimal)random.NextDouble(1.3, 1.49);
            if (grossWeight > 100M && grossWeight <= 150M)
                return (Decimal)random.NextDouble(0.9, 1.29);
            if (grossWeight > 70M && grossWeight <= 100M)
                return (Decimal)random.NextDouble(0.8, 1.09);
            if (grossWeight > 50M && grossWeight <= 70M)
                return (Decimal)random.NextDouble(0.07, 0.08);
            return grossWeight <= 50M ? (Decimal)random.NextDouble(0.6, 0.7) : 0M;
        }

        private void RecuperaEsitiSTM()
        {
            int num1 = 0;
            int num2 = this.EsitiDaRecuperareParzialiSTM.Count<STM_EsitiOut>();
            List<string> stringList = new List<string>();
            foreach (STM_EsitiOut esitoParziale in this.EsitiDaRecuperareParzialiSTM)
            {
                ++num1;
                Debug.WriteLine(string.Format("{0}-{1}", (object)num1, (object)num2));
                this.FinalizzaEsitoSTM(esitoParziale);
                string message = esitoParziale.NumDDT + ";" + esitoParziale.DataTracking + ";" + esitoParziale.Descrizione_Tracking + ";" + esitoParziale.ProgressivoSpedizione + "/SH";
                Debug.WriteLine(message);
                stringList.Add(message);
            }
            this.EsitiDaCoumicareSTM.AddRange((IEnumerable<STM_EsitiOut>)this.EsitiDaRecuperareParzialiSTM);
            this.EsitiDaCoumicareSTM.Where<STM_EsitiOut>((Func<STM_EsitiOut, bool>)(x => x.Descrizione_Tracking == "CONSEGNATA")).ToList<STM_EsitiOut>();
            this.ProduciEsitiSTM();
        }

        private void FinalizzaEsitoSTM(STM_EsitiOut esitoParziale)
        {
            TmsShipmentList shipDes = JsonConvert.DeserializeObject<TmsShipmentList>(EspritecShipment.RestEspritecGetShipmentListByExternalRef(esitoParziale.NumDDT, 50, 1, CustomerConnections.STMGroup.tokenAPI).Content);
            if (shipDes != null && shipDes.shipments == null)
            {
                esitoParziale.Descrizione_Tracking = "ND";
            }
            else
            {
                RootobjectTestShip rootobjectTestShip = JsonConvert.DeserializeObject<RootobjectTestShip>(EspritecShipment.RestEspritecGetTracking((long)shipDes.shipments[0].id, CustomerConnections.STMGroup.tokenAPI).Content);
                if (rootobjectTestShip != null && rootobjectTestShip.events == null)
                {
                    esitoParziale.Descrizione_Tracking = "ND";
                }
                else
                {
                    EventTestShip consegnato = ((IEnumerable<EventTestShip>)rootobjectTestShip.events).FirstOrDefault<EventTestShip>((Func<EventTestShip, bool>)(x => x.statusID == 30));
                    if (consegnato == null)
                    {
                        esitoParziale.Descrizione_Tracking = ((IEnumerable<EventTestShip>)rootobjectTestShip.events).Last<EventTestShip>().statusDes;
                        esitoParziale.DataTracking = ((IEnumerable<EventTestShip>)rootobjectTestShip.events).Last<EventTestShip>().timeStamp.ToString().Split(' ')[0];
                    }
                    else
                    {
                        DateTime result = DateTime.MinValue;
                        DateTime.TryParse(consegnato.timeStamp.ToString(), out result);
                        STM_StatiDocumento stmStatiDocumento = this.statiDocumemtoSTM.FirstOrDefault<STM_StatiDocumento>((Func<STM_StatiDocumento, bool>)(x => x.IdUnitex == consegnato.statusID));
                        if (stmStatiDocumento == null)
                            return;
                        string codiceStato = stmStatiDocumento.CodiceStato;
                        GeoClass geoClass = this.GeoTab.FirstOrDefault<GeoClass>((Func<GeoClass, bool>)(x => x.cap == shipDes.shipments[0].lastStopZipCode));
                        esitoParziale.DataConsegnaEffettiva = consegnato.statusID == 30 ? result.ToString("yyyyMMdd") : "        ";
                        esitoParziale.DataConsegnaTassativa = "        ";
                        esitoParziale.DataSpedizione = shipDes.shipments[0].docDate.ToString("ddMMyyyy");
                        esitoParziale.DataTracking = result.ToString("ddMMyyyy");
                        esitoParziale.Descrizione_Tracking = consegnato.statusDes;
                        esitoParziale.ID_Tracking = codiceStato;
                        esitoParziale.regione = geoClass != null ? geoClass.regione : "ND";
                    }
                }
            }
        }

        private void RecuperaEsitiLoreal()
        {
            ((IEnumerable<string>)System.IO.File.ReadAllLines(Automazione.FileEsitiDaRecuperareLoreal)).ToList<string>().Count<string>();
            int num1 = this.EsitiDaRecuperareParzialiLoreal.Count<LorealEsiti>();
            int num2 = 0;
            List<string> contents = new List<string>();
            foreach (LorealEsiti esitoParziale in this.EsitiDaRecuperareParzialiLoreal)
            {
                ++num2;
                Debug.WriteLine(string.Format("{0}-{1}", (object)num2, (object)num1));
                this.FinalizzaEsitoLoreal(esitoParziale);
                if (!string.IsNullOrEmpty(esitoParziale.E_RiferimentoCorriere))
                    this.EsitiDaComunicareLoreal.Add(esitoParziale);
                string message = esitoParziale.E_NumeroDDT + ";" + esitoParziale.E_Causale + ";" + esitoParziale.E_DataConsegnaADestino + ";" + esitoParziale.E_DataConsegnaADestino;
                Debug.WriteLine(message);
                contents.Add(message);
            }
            this.ProduciEsitiLoreal();
            System.IO.File.WriteAllLines("EsitiLoreal.txt", (IEnumerable<string>)contents);
        }

        private void FinalizzaEsitoLoreal(LorealEsiti esitoParziale)
        {
            TmsShipmentList tmsShipmentList = JsonConvert.DeserializeObject<TmsShipmentList>(EspritecShipment.RestEspritecGetShipmentListByExternalRef(esitoParziale.E_NumeroDDT, 50, 1, this.token_UNITEX).Content);
            if (tmsShipmentList == null || !tmsShipmentList.result.status)
                return;
            RootobjectTestShip rootobjectTestShip = JsonConvert.DeserializeObject<RootobjectTestShip>(EspritecShipment.RestEspritecGetTracking((long)tmsShipmentList.shipments[0].id, this.token_UNITEX).Content);
            if (rootobjectTestShip != null && rootobjectTestShip.events != null)
            {
                EventTestShip eventTestShip = ((IEnumerable<EventTestShip>)rootobjectTestShip.events).FirstOrDefault<EventTestShip>((Func<EventTestShip, bool>)(x => x.statusID == 30));
                if (eventTestShip != null)
                {
                    DateTime dateTime = DateTime.Parse(eventTestShip.timeStamp);
                    esitoParziale.E_NumeroDDT = ((IEnumerable<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>)tmsShipmentList.shipments).First<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>().externRef;
                    esitoParziale.E_RiferimentoNumeroConsegnaSAP = ((IEnumerable<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>)tmsShipmentList.shipments).First<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>().insideRef;
                    esitoParziale.E_DataConsegnaADestino = dateTime.ToString("yyyyMMdd");
                    esitoParziale.E_Causale = "00";
                    esitoParziale.E_SottoCausale = "00";
                    esitoParziale.E_RiferimentoCorriere = ((IEnumerable<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>)tmsShipmentList.shipments).First<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>().docNumber;
                    esitoParziale.E_Filler1 = "";
                    esitoParziale.E_Note = eventTestShip.statusDes;
                    esitoParziale.E_Filler2 = "";
                }
            }
        }

        private void RecuperaEsitiGima()
        {
            List<string> list = ((IEnumerable<string>)System.IO.File.ReadAllLines(Automazione.FileEsitiDaRecuperareGIMA)).ToList<string>();
            List<string> contents = new List<string>();
            contents.Add("Riferimento;Data Tracking;Descrizione Tracking");
            int num1 = 0;
            int num2 = list.Count<string>();
            foreach (string str1 in list)
            {
                string str2 = str1;
                string message;
                try
                {
                    ++num1;
                    Debug.WriteLine(string.Format("{0}-{1}", (object)num1, (object)num2));
                    TmsShipmentList tmsShipmentList = JsonConvert.DeserializeObject<TmsShipmentList>(EspritecShipment.RestEspritecGetShipmentListByExternalRef(str1, 50, 1, this.token_UNITEX).Content);
                    if (tmsShipmentList != null && tmsShipmentList.result.status)
                    {
                        RootobjectTestShip rootobjectTestShip = JsonConvert.DeserializeObject<RootobjectTestShip>(EspritecShipment.RestEspritecGetTracking((long)tmsShipmentList.shipments[0].id, this.token_UNITEX).Content);
                        if (rootobjectTestShip != null)
                        {
                            EventTestShip eventTestShip1 = ((IEnumerable<EventTestShip>)rootobjectTestShip.events).FirstOrDefault<EventTestShip>((Func<EventTestShip, bool>)(x => x.statusID == 30));
                            if (eventTestShip1 != null)
                            {
                                message = str2 + ";" + eventTestShip1.timeStamp + ";" + eventTestShip1.statusDes;
                            }
                            else
                            {
                                EventTestShip eventTestShip2 = ((IEnumerable<EventTestShip>)rootobjectTestShip.events).OrderBy<EventTestShip, string>((Func<EventTestShip, string>)(x => x.timeStamp)).Last<EventTestShip>();
                                message = str2 + ";" + eventTestShip2.timeStamp + ";" + eventTestShip2.statusDes;
                            }
                        }
                        else
                            message = str2 + ";ND;";
                    }
                    else
                        message = str2 + ";ND;";
                }
                catch (Exception ex)
                {
                    message = str2 + ";ND;";
                }
                Debug.WriteLine(message);
                contents.Add(message);
            }
            System.IO.File.WriteAllLines("EsitiGima.txt", (IEnumerable<string>)contents);
        }

        private void RecuperaEsitiCdGroup()
        {
            List<CDGROUP_EsitiOUT> source = new List<CDGROUP_EsitiOUT>();
            int num1 = this.EsitiDaRecuperareParzialiCDGroup.Count<CDGROUP_EsitiOUT>();
            int num2 = 0;
            List<string> contents = new List<string>();
            foreach (CDGROUP_EsitiOUT esitoParziale in this.EsitiDaRecuperareParzialiCDGroup)
            {
                ++num2;
                Debug.WriteLine(string.Format("{0}-{1}", (object)num2, (object)num1));
                string vettore;
                this.FinalizzaEsitoCDGroup(esitoParziale, out vettore);
                source.Add(esitoParziale);
                string message = esitoParziale.RIFVETTORE + ";" + esitoParziale.DESCRIZIONE_STATO_CONSEGNA + ";" + esitoParziale.DATA + ";" + vettore;
                Debug.WriteLine(message);
                contents.Add(message);
            }
            System.IO.File.WriteAllLines("ConsuntivoRecuperoCDGroup.txt", (IEnumerable<string>)contents);
            List<CDGROUP_EsitiOUT> list1 = source.ToList<CDGROUP_EsitiOUT>();
            source.Where<CDGROUP_EsitiOUT>((Func<CDGROUP_EsitiOUT, bool>)(x => x.DESCRIZIONE_STATO_CONSEGNA == "ND")).GroupBy<CDGROUP_EsitiOUT, string>((Func<CDGROUP_EsitiOUT, string>)(x => x.NUMERO_BOLLA)).Select<IGrouping<string, CDGROUP_EsitiOUT>, CDGROUP_EsitiOUT>((Func<IGrouping<string, CDGROUP_EsitiOUT>, CDGROUP_EsitiOUT>)(x => x.FirstOrDefault<CDGROUP_EsitiOUT>())).ToList<CDGROUP_EsitiOUT>();
            if (list1.Count <= 0)
                return;
            string path = "C:\\FTP\\CLIENTI\\CD_GROUP_ESITI\\OUT\\TRACK_" + DateTime.Now.ToString("yyyMMddHHmmss") + ".txt";
            if (!Directory.Exists("C:\\FTP\\CLIENTI\\CD_GROUP_ESITI\\OUT"))
                Directory.CreateDirectory("C:\\FTP\\CLIENTI\\CD_GROUP_ESITI\\OUT");
            if (list1.Count > 0)
            {
                List<CDGROUP_EsitiOUT> list2 = list1.Where<CDGROUP_EsitiOUT>((Func<CDGROUP_EsitiOUT, bool>)(x => x.statoUNITEX == 30 || x.statoUNITEX == 55)).ToList<CDGROUP_EsitiOUT>();
                System.IO.File.WriteAllLines(path, (IEnumerable<string>)Automazione.ProduciFileTrackingCDGroup(list2));
            }
        }

        private void RiallineaLDVLoreal()
        {
            List<Logistica93_ShipmentIN> lorealNonTrovate = new List<Logistica93_ShipmentIN>();
            string[] files = Directory.GetFiles("DaRecuperareLoreal");
            CustomerSpec logistica93 = CustomerConnections.Logistica93;
            foreach (string path1 in files)
            {
                try
                {
                    System.IO.File.ReadAllLines(path1);
                    string[] pzFr = path1.Split('_');
                    bool flag1 = pzFr[1] == "ZCAI";
                    string path2 = ((IEnumerable<string>)Directory.GetFiles(Path.GetDirectoryName(path1))).Where<string>((Func<string, bool>)(x =>
                    {
                        if (!x.Contains("FSE") || !x.Contains(pzFr[2]))
                            return false;
                        return x.Contains(pzFr[3].Split('.')[0]);
                    })).FirstOrDefault<string>();
                    string[] source = System.IO.File.ReadAllLines(path1);
                    System.IO.File.ReadAllLines(path2);
                    List<Logistica93_ShipmentIN> listShipLoreal = new List<Logistica93_ShipmentIN>();
                    int num1 = 0;
                    for (int index = 0; index < ((IEnumerable<string>)source).Count<string>(); ++index)
                    {
                        string str = source[index];
                        ++num1;
                        Debug.WriteLine((object)num1);
                        Logistica93_ShipmentIN logistica93ShipmentIn = new Logistica93_ShipmentIN();
                        logistica93ShipmentIn.TipoRecord = str.Substring(logistica93ShipmentIn.idxTipoRecord[0], logistica93ShipmentIn.idxTipoRecord[1]).Trim();
                        logistica93ShipmentIn.NumeroBorderau = str.Substring(logistica93ShipmentIn.idxNumeroBorderau[0], logistica93ShipmentIn.idxNumeroBorderau[1]).Trim();
                        logistica93ShipmentIn.DataSpedizione = str.Substring(logistica93ShipmentIn.idxDataSpedizione[0], logistica93ShipmentIn.idxDataSpedizione[1]).Trim();
                        logistica93ShipmentIn.NumeroDDT = str.Substring(logistica93ShipmentIn.idxNumeroDDT[0], logistica93ShipmentIn.idxNumeroDDT[1]).Trim();
                        logistica93ShipmentIn.DataDDT = str.Substring(logistica93ShipmentIn.idxDataDDT[0], logistica93ShipmentIn.idxDataDDT[1]).Trim();
                        logistica93ShipmentIn.RifNConsegna = str.Substring(logistica93ShipmentIn.idxRifNConsegna[0], logistica93ShipmentIn.idxRifNConsegna[1]).Trim();
                        logistica93ShipmentIn.LuogoSpedizione = str.Substring(logistica93ShipmentIn.idxLuogoSpedizione[0], logistica93ShipmentIn.idxLuogoSpedizione[1]).Trim();
                        logistica93ShipmentIn.PesoDelivery = str.Substring(logistica93ShipmentIn.idxPesoDelivery[0], logistica93ShipmentIn.idxPesoDelivery[1]).Trim();
                        logistica93ShipmentIn.TipoCliente = str.Substring(logistica93ShipmentIn.idxTipoCliente[0], logistica93ShipmentIn.idxTipoCliente[1]).Trim();
                        logistica93ShipmentIn.Destinatario = str.Substring(logistica93ShipmentIn.idxDestinatario[0], logistica93ShipmentIn.idxDestinatario[1]).Trim();
                        logistica93ShipmentIn.Indirizzo = str.Substring(logistica93ShipmentIn.idxIndirizzo[0], logistica93ShipmentIn.idxIndirizzo[1]).Trim();
                        logistica93ShipmentIn.Localita = str.Substring(logistica93ShipmentIn.idxLocalita[0], logistica93ShipmentIn.idxLocalita[1]).Trim();
                        logistica93ShipmentIn.CAP = str.Substring(logistica93ShipmentIn.idxCAP[0], logistica93ShipmentIn.idxCAP[1]).Trim();
                        logistica93ShipmentIn.SiglaProvDestinazione = str.Substring(logistica93ShipmentIn.idxSiglaProvDestinazione[0], logistica93ShipmentIn.idxSiglaProvDestinazione[1]).Trim();
                        logistica93ShipmentIn.PIVA_CODF = str.Substring(logistica93ShipmentIn.idxPIVA_CODF[0], logistica93ShipmentIn.idxPIVA_CODF[1]).Trim();
                        logistica93ShipmentIn.DataConsegna = str.Substring(logistica93ShipmentIn.idxDataConsegna[0], logistica93ShipmentIn.idxDataConsegna[1]).Trim();
                        logistica93ShipmentIn.TipoDataConsegna = str.Substring(logistica93ShipmentIn.idxTipoDataConsegna[0], logistica93ShipmentIn.idxTipoDataConsegna[1]).Trim();
                        logistica93ShipmentIn.TipoSpedizione = str.Substring(logistica93ShipmentIn.idxTipoSpedizione[0], logistica93ShipmentIn.idxTipoSpedizione[1]).Trim();
                        logistica93ShipmentIn.ImportoContrassegno = str.Substring(logistica93ShipmentIn.idxImportoContrassegno[0], logistica93ShipmentIn.idxImportoContrassegno[1]).Trim();
                        logistica93ShipmentIn.NotaModalitaDiConsegna = str.Substring(logistica93ShipmentIn.idxNotaModalitaDiConsegna[0], logistica93ShipmentIn.idxNotaModalitaDiConsegna[1]).Trim();
                        logistica93ShipmentIn.NotaCommentiTempiConsegna = str.Substring(logistica93ShipmentIn.idxNotaCommentiTempiConsegna[0], logistica93ShipmentIn.idxNotaCommentiTempiConsegna[1]).Trim();
                        logistica93ShipmentIn.NotaEPAL = str.Substring(logistica93ShipmentIn.idxNotaEPAL[0], logistica93ShipmentIn.idxNotaEPAL[1]).Trim();
                        logistica93ShipmentIn.NotaBolla = str.Substring(logistica93ShipmentIn.idxNotaBolla[0], logistica93ShipmentIn.idxNotaBolla[1]).Trim();
                        logistica93ShipmentIn.NumeroColliDettaglio = str.Substring(logistica93ShipmentIn.idxNumeroColliDettaglio[0], logistica93ShipmentIn.idxNumeroColliDettaglio[1]).Trim();
                        logistica93ShipmentIn.NumeroColliStandard = str.Substring(logistica93ShipmentIn.idxNumeroColliStandard[0], logistica93ShipmentIn.idxNumeroColliStandard[1]).Trim();
                        logistica93ShipmentIn.NumeroEspositoriPLV = str.Substring(logistica93ShipmentIn.idxNumeroEspositoriPLV[0], logistica93ShipmentIn.idxNumeroEspositoriPLV[1]).Trim();
                        logistica93ShipmentIn.NumeroPedane = str.Substring(logistica93ShipmentIn.idxNumeroPedane[0], logistica93ShipmentIn.idxNumeroPedane[1]).Trim();
                        logistica93ShipmentIn.CodiceCorriere = str.Substring(logistica93ShipmentIn.idxCodiceCorriere[0], logistica93ShipmentIn.idxCodiceCorriere[1]).Trim();
                        logistica93ShipmentIn.ItinerarioCorriere = str.Substring(logistica93ShipmentIn.idxItinerarioCorriere[0], logistica93ShipmentIn.idxItinerarioCorriere[1]).Trim();
                        logistica93ShipmentIn.SottoZonaCorriere = str.Substring(logistica93ShipmentIn.idxSottoZonaCorriere[0], logistica93ShipmentIn.idxSottoZonaCorriere[1]).Trim();
                        logistica93ShipmentIn.NumeroPedaneEPAL = str.Substring(logistica93ShipmentIn.idxNumeroPedaneEPAL[0], logistica93ShipmentIn.idxNumeroPedaneEPAL[1]).Trim();
                        logistica93ShipmentIn.TipoTrasporto = str.Substring(logistica93ShipmentIn.idxTipoTrasporto[0], logistica93ShipmentIn.idxTipoTrasporto[1]).Trim();
                        logistica93ShipmentIn.ZonaCorriere = str.Substring(logistica93ShipmentIn.idxZonaCorriere[0], logistica93ShipmentIn.idxZonaCorriere[1]).Trim();
                        logistica93ShipmentIn.PedanaDirezionale = str.Substring(logistica93ShipmentIn.idxPedanaDirezionale[0], logistica93ShipmentIn.idxPedanaDirezionale[1]).Trim();
                        logistica93ShipmentIn.CodiceAbbinamento = str.Substring(logistica93ShipmentIn.idxCodiceAbbinamento[0], logistica93ShipmentIn.idxCodiceAbbinamento[1]).Trim();
                        logistica93ShipmentIn.NumeroOrdineCliente = str.Substring(logistica93ShipmentIn.idxNumeroOrdineCliente[0], logistica93ShipmentIn.idxNumeroOrdineCliente[1]).Trim();
                        logistica93ShipmentIn.ContrattoCorriere = str.Substring(logistica93ShipmentIn.idxContrattoCorriere[0], logistica93ShipmentIn.idxContrattoCorriere[1]).Trim();
                        logistica93ShipmentIn.Via3 = str.Substring(logistica93ShipmentIn.idxVia3[0], logistica93ShipmentIn.idxVia3[1]).Trim();
                        logistica93ShipmentIn.NumeroFattura = str.Substring(logistica93ShipmentIn.idxNumeroFattura[0], logistica93ShipmentIn.idxNumeroFattura[1]).Trim();
                        logistica93ShipmentIn.PesoPolveri = str.Substring(logistica93ShipmentIn.idxPesoPolveri[0], logistica93ShipmentIn.idxPesoPolveri[1]).Trim();
                        logistica93ShipmentIn.NumeroFiliale = str.Substring(logistica93ShipmentIn.idxNumeroFiliale[0], logistica93ShipmentIn.idxNumeroFiliale[1]).Trim();
                        logistica93ShipmentIn.TipoClienteIntestazione = str.Substring(logistica93ShipmentIn.idxTipoClienteIntestazione[0], logistica93ShipmentIn.idxTipoClienteIntestazione[1]).Trim();
                        logistica93ShipmentIn.DestinatarioFiliale = str.Substring(logistica93ShipmentIn.idxDestinatarioFiliale[0], logistica93ShipmentIn.idxDestinatarioFiliale[1]).Trim();
                        logistica93ShipmentIn.IndirizzoFiliale = str.Substring(logistica93ShipmentIn.idxIndirizzoFiliale[0], logistica93ShipmentIn.idxIndirizzoFiliale[1]).Trim();
                        logistica93ShipmentIn.LocalitaFiliale = str.Substring(logistica93ShipmentIn.idxLocalitaFiliale[0], logistica93ShipmentIn.idxLocalitaFiliale[1]).Trim();
                        logistica93ShipmentIn.CAPFiliale = str.Substring(logistica93ShipmentIn.idxCAPFiliale[0], logistica93ShipmentIn.idxCAPFiliale[1]).Trim();
                        logistica93ShipmentIn.SiglaProvDestinazioneFiliale = str.Substring(logistica93ShipmentIn.idxSiglaProvDestinazioneFiliale[0], logistica93ShipmentIn.idxSiglaProvDestinazioneFiliale[1]).Trim();
                        logistica93ShipmentIn.Filler = str.Substring(logistica93ShipmentIn.idxFiller[0], logistica93ShipmentIn.idxFiller[1]).Trim();
                        logistica93ShipmentIn.DeliveryVolume = str.Substring(logistica93ShipmentIn.idxDeliveryVolume[0], logistica93ShipmentIn.idxDeliveryVolume[1]).Trim();
                        logistica93ShipmentIn.VolumeUnit = str.Substring(logistica93ShipmentIn.idxVolumeUnit[0], logistica93ShipmentIn.idxVolumeUnit[1]).Trim();
                        logistica93ShipmentIn.PrioritàConsegna = str.Substring(logistica93ShipmentIn.idxPrioritàConsegna[0], logistica93ShipmentIn.idxPrioritàConsegna[1]).Trim();
                        listShipLoreal.Add(logistica93ShipmentIn);
                    }
                    foreach (Logistica93_ShipmentIN logistica93ShipmentIn in this.RaggruppaTestateLoreal(listShipLoreal))
                    {
                        Decimal decimalFromString = Helper.GetDecimalFromString(logistica93ShipmentIn.PesoDelivery, 2);
                        int num2 = int.Parse(logistica93ShipmentIn.NumeroColliDettaglio) + int.Parse(logistica93ShipmentIn.NumeroColliStandard);
                        int num3 = int.Parse(logistica93ShipmentIn.NumeroPedane);
                        EspritecShipment.RootobjectShipmentList rootobjectShipmentList = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(EspritecShipment.RestEspritecGetShipmentListByExternalRef(logistica93ShipmentIn.NumeroDDT, 1, 50, logistica93.tokenAPI).Content);
                        if (!rootobjectShipmentList.result.status)
                        {
                            rootobjectShipmentList = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(EspritecShipment.RestEspritecGetShipmentListByExternalRef(logistica93ShipmentIn.NumeroDDT.Substring(3), 1, 50, logistica93.tokenAPI).Content);
                            if (!rootobjectShipmentList.result.status)
                            {
                                rootobjectShipmentList = JsonConvert.DeserializeObject<EspritecShipment.RootobjectShipmentList>(EspritecShipment.RestEspritecGetShipmentListByExternalRef("222" + logistica93ShipmentIn.NumeroBorderau.Substring(3), 1, 50, logistica93.tokenAPI).Content);
                                if (!rootobjectShipmentList.result.status)
                                    lorealNonTrovate.Add(logistica93ShipmentIn);
                            }
                        }
                        bool flag2 = false;
                        if (rootobjectShipmentList != null && rootobjectShipmentList.result.status)
                        {
                            if (rootobjectShipmentList.shipments != null && ((IEnumerable<EspritecShipment.ShipmentList>)rootobjectShipmentList.shipments).Count<EspritecShipment.ShipmentList>() == 1)
                            {
                                EspritecShipment.ShipmentList shipmentList = ((IEnumerable<EspritecShipment.ShipmentList>)rootobjectShipmentList.shipments).First<EspritecShipment.ShipmentList>();
                                if (decimalFromString != shipmentList.grossWeight)
                                    flag2 = true;
                                if (shipmentList.packs != num2)
                                    flag2 = true;
                                if (shipmentList.floorPallets != (Decimal)num3)
                                    flag2 = true;
                                if (!flag2)
                                    continue;
                            }
                        }
                        else
                            Debug.WriteLine(logistica93ShipmentIn.NumeroDDT + " non trovato in gespe");
                        if (flag2)
                        {
                            EspritecGoods.RootobjectGoodsList rootobjectGoodsList = JsonConvert.DeserializeObject<EspritecGoods.RootobjectGoodsList>(EspritecGoods.RestEspritecGetGoodListOfShipment(((IEnumerable<EspritecShipment.ShipmentList>)rootobjectShipmentList.shipments).First<EspritecShipment.ShipmentList>().id, this.token_UNITEX).Content);
                            EspritecGoods.RestEspritecUpdateGoods(new EspritecGoods.RootobjectGoodsUpdate()
                            {
                                goods = new EspritecGoods.GoodsUpdate()
                                {
                                    grossWeight = decimalFromString,
                                    packs = num2,
                                    floorPallet = num3,
                                    totalPallet = num3,
                                    id = ((IEnumerable<EspritecGoods.GoodList>)rootobjectGoodsList.goods).First<EspritecGoods.GoodList>().id
                                }
                            }, this.token_UNITEX);
                            Debug.WriteLine(logistica93ShipmentIn.NumeroDDT + " aggiornato");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(path1 + "errore lettura file");
                }
            }
            this.ScrivimiLeLDVNonTrovate(lorealNonTrovate);
        }

        private void ScrivimiLeLDVNonTrovate(List<Logistica93_ShipmentIN> lorealNonTrovate)
        {
            string path = "lorealND.txt";
            foreach (Logistica93_ShipmentIN logistica93ShipmentIn in lorealNonTrovate)
                System.IO.File.AppendAllText(path, logistica93ShipmentIn.ToString() + "\r\n");
        }

        private void FinalizzaEsitoCDGroup(CDGROUP_EsitiOUT esitoParziale, out string vettore)
        {
            vettore = "";
            try
            {
                TmsShipmentList tmsShipmentList = JsonConvert.DeserializeObject<TmsShipmentList>(EspritecShipment.RestEspritecGetShipmentListByExternalRef(esitoParziale.NUMERO_BOLLA, 50, 1, this.token_UNITEX).Content);
                if (tmsShipmentList == null || !tmsShipmentList.result.status)
                    tmsShipmentList = JsonConvert.DeserializeObject<TmsShipmentList>(EspritecShipment.RestEspritecGetShipmentListByExternalRef(esitoParziale.NUMERO_BOLLA.Substring(3), 50, 1, this.token_UNITEX).Content);
                if (tmsShipmentList != null && tmsShipmentList.result.status)
                {
                    if (((IEnumerable<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>)tmsShipmentList.shipments).Count<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>() == 1)
                    {
                        vettore = ((IEnumerable<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>)tmsShipmentList.shipments).First<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>().deliverySupplierDes;
                        if (tmsShipmentList.shipments[0].customerID == "00551" || tmsShipmentList.shipments[0].customerID == "00035" || tmsShipmentList.shipments[0].customerID == "00032")
                        {
                            RootobjectShipmentTracking shipmentTracking = JsonConvert.DeserializeObject<RootobjectShipmentTracking>(EspritecShipment.RestEspritecGetTracking((long)tmsShipmentList.shipments[0].id, this.token_UNITEX).Content);
                            if (shipmentTracking != null && shipmentTracking.events != null)
                            {
                                EventTracking consegnato = ((IEnumerable<EventTracking>)shipmentTracking.events).FirstOrDefault<EventTracking>((Func<EventTracking, bool>)(x => x.statusID == 30));
                                if (consegnato != null)
                                {
                                    CDGROUP_StatiDocumento cdgroupStatiDocumento = this.statiDocumemtoCDGroup.FirstOrDefault<CDGROUP_StatiDocumento>((Func<CDGROUP_StatiDocumento, bool>)(x => x.IdUnitex == consegnato.statusID));
                                    if (cdgroupStatiDocumento != null)
                                    {
                                        DateTime dateTime = DateTime.Parse(consegnato.timeStamp.ToString());
                                        esitoParziale.DESCRIZIONE_STATO_CONSEGNA = consegnato.statusDes;
                                        if (string.IsNullOrEmpty(esitoParziale.DATA_BOLLA))
                                            esitoParziale.DATA_BOLLA = tmsShipmentList.shipments[0].docDate.ToString("yyyyMMdd");
                                        esitoParziale.STATO_CONSEGNA = cdgroupStatiDocumento.CodiceStato;
                                        esitoParziale.LOCALITA = consegnato.stopLocation.ToString();
                                        esitoParziale.DATA_PRESA_CONS = string.IsNullOrEmpty(esitoParziale.DATA_PRESA_CONS) ? tmsShipmentList.shipments[0].docDate.ToString("yyyyMMdd") : esitoParziale.DATA_PRESA_CONS;
                                        esitoParziale.RIFVETTORE = tmsShipmentList.shipments[0].docNumber;
                                        esitoParziale.DATA = string.IsNullOrEmpty(esitoParziale.DATA) ? dateTime.ToString("yyyyMMdd") : esitoParziale.DATA;
                                        esitoParziale.MANDANTE = string.IsNullOrEmpty(esitoParziale.MANDANTE) ? tmsShipmentList.shipments[0].insideRef : esitoParziale.MANDANTE;
                                        esitoParziale.LOCALITA = tmsShipmentList.shipments[0].lastStopLocation;
                                        esitoParziale.statoUNITEX = consegnato.statusID;
                                        esitoParziale.RAGIONE_SOCIALE_VETTORE = "UNITEXPRESS";
                                    }
                                    else
                                    {
                                        esitoParziale.DESCRIZIONE_STATO_CONSEGNA = "ND";
                                        esitoParziale.RIFVETTORE = tmsShipmentList.shipments[0].docNumber;
                                        esitoParziale.statoUNITEX = 9999;
                                    }
                                }
                                else
                                {
                                    EventTracking ultimoEvento = ((IEnumerable<EventTracking>)shipmentTracking.events).OrderBy<EventTracking, object>((Func<EventTracking, object>)(x => x.timeStamp)).Last<EventTracking>();
                                    CDGROUP_StatiDocumento cdgroupStatiDocumento = this.statiDocumemtoCDGroup.FirstOrDefault<CDGROUP_StatiDocumento>((Func<CDGROUP_StatiDocumento, bool>)(x => x.IdUnitex == ultimoEvento.statusID));
                                    if (cdgroupStatiDocumento != null)
                                    {
                                        DateTime dateTime = DateTime.Parse(ultimoEvento.timeStamp.ToString());
                                        esitoParziale.DESCRIZIONE_STATO_CONSEGNA = ultimoEvento.statusDes;
                                        if (string.IsNullOrEmpty(esitoParziale.DATA_BOLLA))
                                            esitoParziale.DATA_BOLLA = tmsShipmentList.shipments[0].docDate.ToString("yyyyMMdd");
                                        esitoParziale.STATO_CONSEGNA = cdgroupStatiDocumento.CodiceStato;
                                        esitoParziale.LOCALITA = ultimoEvento.stopLocation.ToString();
                                        esitoParziale.DATA_PRESA_CONS = string.IsNullOrEmpty(esitoParziale.DATA_PRESA_CONS) ? dateTime.ToString("yyyyMMdd") : esitoParziale.DATA_PRESA_CONS;
                                        esitoParziale.RIFVETTORE = tmsShipmentList.shipments[0].docNumber;
                                        esitoParziale.DATA = string.IsNullOrEmpty(esitoParziale.DATA) ? tmsShipmentList.shipments[0].docDate.ToString("yyyyMMdd") : esitoParziale.DATA;
                                        esitoParziale.MANDANTE = string.IsNullOrEmpty(esitoParziale.MANDANTE) ? tmsShipmentList.shipments[0].insideRef : esitoParziale.MANDANTE;
                                        esitoParziale.LOCALITA = tmsShipmentList.shipments[0].lastStopLocation;
                                        esitoParziale.RAGIONE_SOCIALE_VETTORE = "UNITEXPRESS";
                                        esitoParziale.statoUNITEX = ultimoEvento.statusID;
                                    }
                                    else
                                    {
                                        DateTime dateTime = DateTime.Parse(ultimoEvento.timeStamp.ToString());
                                        esitoParziale.DESCRIZIONE_STATO_CONSEGNA = ultimoEvento.statusDes;
                                        if (string.IsNullOrEmpty(esitoParziale.DATA_BOLLA))
                                            esitoParziale.DATA_BOLLA = tmsShipmentList.shipments[0].docDate.ToString("yyyyMMdd");
                                        esitoParziale.DATA_PRESA_CONS = string.IsNullOrEmpty(esitoParziale.DATA_PRESA_CONS) ? dateTime.ToString("yyyyMMdd") : esitoParziale.DATA_PRESA_CONS;
                                        esitoParziale.RIFVETTORE = tmsShipmentList.shipments[0].docNumber;
                                        esitoParziale.RAGIONE_SOCIALE_VETTORE = "UNITEXPRESS";
                                        esitoParziale.statoUNITEX = ultimoEvento.statusID;
                                    }
                                }
                            }
                            else
                            {
                                esitoParziale.DESCRIZIONE_STATO_CONSEGNA = "ND";
                                esitoParziale.RIFVETTORE = tmsShipmentList.shipments[0].docNumber;
                                esitoParziale.statoUNITEX = 9999;
                            }
                        }
                        else
                            Debug.WriteLine("cliente non interessato");
                    }
                    else
                    {
                        for (int index = 0; index < ((IEnumerable<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>)tmsShipmentList.shipments).Count<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>(); ++index)
                        {
                            vettore = tmsShipmentList.shipments[index].deliverySupplierDes;
                            if (tmsShipmentList.shipments[index].customerID == "00551" || tmsShipmentList.shipments[index].customerID == "00035" || tmsShipmentList.shipments[index].customerID == "00032")
                            {
                                RootobjectShipmentTracking shipmentTracking = JsonConvert.DeserializeObject<RootobjectShipmentTracking>(EspritecShipment.RestEspritecGetTracking((long)tmsShipmentList.shipments[index].id, this.token_UNITEX).Content);
                                if (shipmentTracking != null && shipmentTracking.events != null)
                                {
                                    EventTracking consegnato = ((IEnumerable<EventTracking>)shipmentTracking.events).FirstOrDefault<EventTracking>((Func<EventTracking, bool>)(x => x.statusID == 30));
                                    DateTime docDate;
                                    if (consegnato != null)
                                    {
                                        CDGROUP_StatiDocumento cdgroupStatiDocumento = this.statiDocumemtoCDGroup.FirstOrDefault<CDGROUP_StatiDocumento>((Func<CDGROUP_StatiDocumento, bool>)(x => x.IdUnitex == consegnato.statusID));
                                        if (cdgroupStatiDocumento != null)
                                        {
                                            DateTime dateTime = DateTime.Parse(consegnato.timeStamp.ToString());
                                            esitoParziale.DESCRIZIONE_STATO_CONSEGNA = consegnato.statusDes;
                                            esitoParziale.STATO_CONSEGNA = cdgroupStatiDocumento.CodiceStato;
                                            if (string.IsNullOrEmpty(esitoParziale.DATA_BOLLA))
                                            {
                                                CDGROUP_EsitiOUT cdgroupEsitiOut = esitoParziale;
                                                docDate = tmsShipmentList.shipments[index].docDate;
                                                string str = docDate.ToString("yyyyMMdd");
                                                cdgroupEsitiOut.DATA_BOLLA = str;
                                            }
                                            esitoParziale.LOCALITA = consegnato.stopLocation.ToString();
                                            esitoParziale.DATA_PRESA_CONS = string.IsNullOrEmpty(esitoParziale.DATA_PRESA_CONS) ? dateTime.ToString("yyyyMMdd") : esitoParziale.DATA_PRESA_CONS;
                                            esitoParziale.RIFVETTORE = tmsShipmentList.shipments[index].docNumber;
                                            CDGROUP_EsitiOUT cdgroupEsitiOut1 = esitoParziale;
                                            string data;
                                            if (!string.IsNullOrEmpty(esitoParziale.DATA))
                                            {
                                                data = esitoParziale.DATA;
                                            }
                                            else
                                            {
                                                docDate = tmsShipmentList.shipments[index].docDate;
                                                data = docDate.ToString("yyyyMMdd");
                                            }
                                            cdgroupEsitiOut1.DATA = data;
                                            esitoParziale.MANDANTE = string.IsNullOrEmpty(esitoParziale.MANDANTE) ? tmsShipmentList.shipments[index].insideRef : esitoParziale.MANDANTE;
                                            esitoParziale.LOCALITA = tmsShipmentList.shipments[index].lastStopLocation;
                                            esitoParziale.RAGIONE_SOCIALE_VETTORE = "UNITEXPRESS";
                                            esitoParziale.statoUNITEX = consegnato.statusID;
                                            break;
                                        }
                                        esitoParziale.DESCRIZIONE_STATO_CONSEGNA = "ND";
                                    }
                                    else
                                    {
                                        EventTracking ultimoEvento = ((IEnumerable<EventTracking>)shipmentTracking.events).OrderBy<EventTracking, object>((Func<EventTracking, object>)(x => x.timeStamp)).Last<EventTracking>();
                                        CDGROUP_StatiDocumento cdgroupStatiDocumento = this.statiDocumemtoCDGroup.FirstOrDefault<CDGROUP_StatiDocumento>((Func<CDGROUP_StatiDocumento, bool>)(x => x.IdUnitex == ultimoEvento.statusID));
                                        if (cdgroupStatiDocumento != null)
                                        {
                                            DateTime dateTime = DateTime.Parse(ultimoEvento.timeStamp.ToString());
                                            esitoParziale.DESCRIZIONE_STATO_CONSEGNA = ultimoEvento.statusDes;
                                            if (string.IsNullOrEmpty(esitoParziale.DATA_BOLLA))
                                            {
                                                CDGROUP_EsitiOUT cdgroupEsitiOut = esitoParziale;
                                                docDate = tmsShipmentList.shipments[index].docDate;
                                                string str = docDate.ToString("yyyyMMdd");
                                                cdgroupEsitiOut.DATA_BOLLA = str;
                                            }
                                            esitoParziale.STATO_CONSEGNA = cdgroupStatiDocumento.CodiceStato;
                                            esitoParziale.LOCALITA = ultimoEvento.stopLocation.ToString();
                                            esitoParziale.DATA_PRESA_CONS = string.IsNullOrEmpty(esitoParziale.DATA_PRESA_CONS) ? dateTime.ToString("yyyyMMdd") : esitoParziale.DATA_PRESA_CONS;
                                            esitoParziale.RIFVETTORE = tmsShipmentList.shipments[index].docNumber;
                                            CDGROUP_EsitiOUT cdgroupEsitiOut2 = esitoParziale;
                                            string data;
                                            if (!string.IsNullOrEmpty(esitoParziale.DATA))
                                            {
                                                data = esitoParziale.DATA;
                                            }
                                            else
                                            {
                                                docDate = tmsShipmentList.shipments[index].docDate;
                                                data = docDate.ToString("yyyyMMdd");
                                            }
                                            cdgroupEsitiOut2.DATA = data;
                                            esitoParziale.MANDANTE = string.IsNullOrEmpty(esitoParziale.MANDANTE) ? tmsShipmentList.shipments[index].insideRef : esitoParziale.MANDANTE;
                                            esitoParziale.LOCALITA = tmsShipmentList.shipments[index].lastStopLocation;
                                            esitoParziale.RAGIONE_SOCIALE_VETTORE = "UNITEXPRESS";
                                            esitoParziale.statoUNITEX = ultimoEvento.statusID;
                                        }
                                        else
                                        {
                                            esitoParziale.DESCRIZIONE_STATO_CONSEGNA = ultimoEvento.statusDes;
                                            esitoParziale.RIFVETTORE = tmsShipmentList.shipments[index].docNumber;
                                            esitoParziale.statoUNITEX = ultimoEvento.statusID;
                                        }
                                    }
                                }
                                else
                                {
                                    esitoParziale.DESCRIZIONE_STATO_CONSEGNA = "ND";
                                    esitoParziale.RIFVETTORE = tmsShipmentList.shipments[index].docNumber;
                                    esitoParziale.statoUNITEX = 9999;
                                }
                            }
                            else
                                Debug.WriteLine("cliente non interessato");
                        }
                    }
                }
                else
                {
                    esitoParziale.DESCRIZIONE_STATO_CONSEGNA = "ND";
                    esitoParziale.statoUNITEX = 99999;
                }
            }
            catch (Exception ex)
            {
                esitoParziale.DESCRIZIONE_STATO_CONSEGNA = "ND";
            }
        }

        private void CheckCustomerPath()
        {
            foreach (CustomerSpec customer in CustomerConnections.customers)
            {
                if (!Directory.Exists(customer.LocalInFilePath))
                    Directory.CreateDirectory(customer.LocalInFilePath);
                if (!Directory.Exists(customer.LocalWorkPath))
                    Directory.CreateDirectory(customer.LocalWorkPath);
                if (!Directory.Exists(customer.LocalErrorFilePath))
                    Directory.CreateDirectory(customer.LocalErrorFilePath);
                if (!Directory.Exists(customer.PathEsiti))
                    Directory.CreateDirectory(customer.PathEsiti);
            }
        }

        private void CaricaConfigurazioni() => this.cicloTimer = double.Parse(System.IO.File.ReadAllLines(Automazione.config)[5]);

        private void SetTimer()
        {
            this.timerAggiornamentoCiclo = new System.Timers.Timer(this.cicloTimer);
            this.timerAggiornamentoCiclo.Elapsed += new ElapsedEventHandler(this.OnTimedEvent);
            this.timerAggiornamentoCiclo.AutoReset = true;
            this.timerAggiornamentoCiclo.Enabled = true;
            this.timerEsiti = new System.Timers.Timer(this.cicloTimerEsitiCDGroup);
            this.timerEsiti.Elapsed += new ElapsedEventHandler(this.OnTimedEventCambiTMS);
            this.timerEsiti.AutoReset = true;
            this.timerEsiti.Enabled = true;
            Automazione._loggerCode.Info(string.Format("Timer ciclo settato {0} ms", (object)this.timerAggiornamentoCiclo.Interval));
            Automazione._loggerCode.Info(string.Format("Timer esiti settato {0} ms", (object)this.timerEsiti.Interval));
        }

        private void RecuperaLastCheckChangesTMS() => this.LastCheckChangesTMS = DateTime.Parse(System.IO.File.ReadAllLines(this.PathLastCheckChangesFileTMS)[0]);

        private void OnTimedEventCambiTMS(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.timerEsiti.Stop();
                this.RecuperaConnessione();
                Automazione._loggerCode.Info("recupero cambi tms in corso...");
                this.RecuperaLastCheckChangesTMS();
                string fromTimestamp = this.LastCheckChangesTMS.ToString("s", (IFormatProvider)CultureInfo.InvariantCulture);
                int num1 = 0;
                this.LastCheckChangesTMS = DateTime.Now;
                Automazione._loggerCode.Info("Recupero Cambi TMS da API");
                foreach (CustomerSpec customerSpec in CustomerConnections.customers.Where<CustomerSpec>((Func<CustomerSpec, bool>)(x => !string.IsNullOrEmpty(x.tokenAPI))))
                {
                    List<EventTracking> source = this.CambiTMSDelCliente(customerSpec, fromTimestamp);
                    num1 += source.Count<EventTracking>();
                    if (source.Count<EventTracking>() > 0)
                        Automazione._loggerCode.Info(string.Format("Trovati {0} cambi spedizioni per il cliente {1}", (object)source.Count<EventTracking>(), (object)customerSpec.NOME));
                    int num2 = 0;
                    foreach (EventTracking shipTrackingUnitexNR in source)
                    {
                        ++num2;
                        Debug.WriteLine(num2.ToString() + " - " + source.Count<EventTracking>().ToString());
                        try
                        {
                            Shipment shipUnitex = this.RecuperaShipUnitexByShipmentID(shipTrackingUnitexNR.shipID);
                            this.AggiungiAllaListaLesito(customerSpec, shipTrackingUnitexNR, shipUnitex);
                        }
                        catch (Exception ex)
                        {
                            Automazione._loggerCode.Error<Exception>(ex);
                        }
                    }
                }
                Automazione._loggerCode.Info(string.Format("LastTimeCheckTMS: {0} - Cambi recuperati {1}", (object)this.LastCheckChangesTMS.ToString("dd/MM/yyyy HH:mm:ss"), (object)num1));
                this.ScriviLastCheckChangesTMS(false);
                this.ProduciEsitiCDGroup();
                this.ProduciEsitiSTM();
                this.ProduciEsitiLoreal();
                this.ProduciEsitiDamora();
                Automazione._loggerCode.Info("recupero cambi tms completato");
            }
            catch (Exception ex)
            {
                Automazione._loggerCode.Error<Exception>(ex);
                if (ex.Message != this.LastException.Message || this.DateLastException + TimeSpan.FromHours(1.0) < DateTime.Now)
                {
                    this.DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("OnTimedEventEsitiCDGroup", ex);
                }
                this.LastException = ex;
            }
            finally
            {
                this.timerEsiti.Start();
            }
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

        private void ProduciEsiti3C()
        {
            List<IGrouping<string, _3C_EsitiOUT>> list = this.EsitiDaComunicare_3C.GroupBy<_3C_EsitiOUT, string>((Func<_3C_EsitiOUT, string>)(x => x.NumeroBolla)).ToList<IGrouping<string, _3C_EsitiOUT>>();
            List<_3C_EsitiOUT> objList1 = new List<_3C_EsitiOUT>();
            List<_3C_EsitiOUT> objList2 = new List<_3C_EsitiOUT>();
            if (list.Count <= 0)
                return;
            foreach (IEnumerable<_3C_EsitiOUT> objs in list)
            {
                foreach (_3C_EsitiOUT obj1 in objs)
                {
                    _3C_EsitiOUT r = obj1;
                    _3C_EsitiOUT obj2 = objList2.FirstOrDefault<_3C_EsitiOUT>((Func<_3C_EsitiOUT, bool>)(x => x.NumeroBolla == r.NumeroBolla));
                    if (obj2 != null)
                    {
                        if (obj2.statoUNITEX != 30)
                        {
                            if (r.statoUNITEX == 30)
                            {
                                objList2.Remove(obj2);
                                objList2.Add(r);
                                break;
                            }
                            if (r.statoUNITEX > obj2.statoUNITEX)
                            {
                                objList2.Remove(obj2);
                                objList2.Add(r);
                            }
                        }
                        else
                            break;
                    }
                    else
                        objList2.Add(r);
                }
            }
            string str = "C:\\FTP\\CLIENTI\\3CSRLS\\ESITI\\ESITI_" + DateTime.Now.ToString("yyyMMddHHmmss") + ".txt";
            string fileName = Path.GetFileName(str);
            if (!Directory.Exists("C:\\FTP\\CLIENTI\\3CSRLS\\ESITI\\"))
                Directory.CreateDirectory("C:\\FTP\\CLIENTI\\3CSRLS\\ESITI");
            if (System.IO.File.Exists(str))
                System.IO.File.Delete(str);
            CustomerSpec cust = CustomerConnections._3CS;
            System.IO.File.WriteAllLines(str, (IEnumerable<string>)this.ProduciFileTracking3C(objList2));
            this._ftp = this.CreaClientFTPperIlCliente(cust);
            string remotePath = Path.Combine("/OUT/Esiti", fileName);
            int num = (int)this._ftp.UploadFile(str, remotePath, FtpRemoteExists.Overwrite, false, FtpVerify.None, (Action<FtpProgress>)null);
            this._ftp.Disconnect();
            if (!Directory.Exists("C:\\FTP\\CLIENTI\\3CSRLS\\ESITI\\inviati"))
                Directory.CreateDirectory("C:\\FTP\\CLIENTI\\3CSRLS\\ESITI\\inviati");
            System.IO.File.Move(str, Path.Combine("C:\\FTP\\CLIENTI\\3CSRLS\\ESITI\\inviati", fileName));
        }

        private List<string> ProduciFileTracking3C(List<_3C_EsitiOUT> esitiPiuRecenti)
        {
            List<string> stringList = new List<string>();
            foreach (_3C_EsitiOUT obj in esitiPiuRecenti)
            {
                string str = obj.NumeroBolla.PadRight(15, ' ') + "".PadRight(9, ' ') + "".PadRight(6, ' ') + (obj.RagioneSocialeMittente.Length > 40 ? obj.RagioneSocialeMittente.Substring(0, 40) : obj.RagioneSocialeMittente.PadRight(40, ' ')) + obj.RagioneSocialeDestinatario.PadRight(40, ' ') + obj.LocalitaDestinatario.PadRight(30, ' ') + (obj.ProvDestinatario.Length > 2 ? obj.ProvDestinatario.Substring(0, 2) : obj.ProvDestinatario.PadRight(2, ' ')) + obj.NumeroColli.ToString().PadLeft(5, '0') + obj.Peso1D.ToString().Replace(",", "").Substring(0, obj.Peso1D.Length - 5).PadLeft(7, '0') + obj.DataEvento + obj.TipoEvento + obj.DataPrenotazione.PadRight(1, ' ') + obj.DescrizioneEvento.PadRight(60, ' ') + "0".PadLeft(3, ' ') + "    " + "".PadRight(30, ' ') + "".PadRight(38, ' ') + obj.FineRecord;
                stringList.Add(str);
            }
            return stringList;
        }

        private void ProduciEsitiChiapparoli()
        {
            List<IGrouping<string, Chiapparoli_EsitiOUT>> list = this.EsitiDaComunicareChiapparoli.GroupBy<Chiapparoli_EsitiOUT, string>((Func<Chiapparoli_EsitiOUT, string>)(x => x.NumeroProgressivo)).ToList<IGrouping<string, Chiapparoli_EsitiOUT>>();
            List<Chiapparoli_EsitiOUT> esitiPiuRecenti = new List<Chiapparoli_EsitiOUT>();
            List<Chiapparoli_EsitiOUT> source = new List<Chiapparoli_EsitiOUT>();
            if (list.Count <= 0)
                return;
            foreach (IEnumerable<Chiapparoli_EsitiOUT> chiapparoliEsitiOuts in list)
            {
                foreach (Chiapparoli_EsitiOUT chiapparoliEsitiOut1 in chiapparoliEsitiOuts)
                {
                    Chiapparoli_EsitiOUT r = chiapparoliEsitiOut1;
                    Chiapparoli_EsitiOUT chiapparoliEsitiOut2 = source.FirstOrDefault<Chiapparoli_EsitiOUT>((Func<Chiapparoli_EsitiOUT, bool>)(x => x.NumeroProgressivo == r.NumeroProgressivo));
                    if (chiapparoliEsitiOut2 != null)
                    {
                        if (chiapparoliEsitiOut2.statoUNITEX != 30)
                        {
                            if (r.statoUNITEX == 30)
                            {
                                source.Remove(chiapparoliEsitiOut2);
                                source.Add(r);
                                break;
                            }
                            if (r.statoUNITEX > chiapparoliEsitiOut2.statoUNITEX)
                            {
                                source.Remove(chiapparoliEsitiOut2);
                                source.Add(r);
                            }
                        }
                        else
                            break;
                    }
                    else
                        source.Add(r);
                }
            }
            string str = "C:\\FTP\\CLIENTI\\CHIAPPAROLI\\OUT\\ESITI\\CHC_" + DateTime.Now.ToString("yyyMMddHHmmss") + ".txt";
            string fileName = Path.GetFileName(str);
            if (!Directory.Exists("C:\\FTP\\CLIENTI\\CHIAPPAROLI\\OUT\\ESITI\\"))
                Directory.CreateDirectory("C:\\FTP\\CLIENTI\\CHIAPPAROLI\\OUT\\ESITI");
            if (System.IO.File.Exists(str))
                System.IO.File.Delete(str);
            CustomerSpec chiapparoli = CustomerConnections.CHIAPPAROLI;
            System.IO.File.WriteAllLines(str, (IEnumerable<string>)this.ProduciFileTrackingChiapparoli(esitiPiuRecenti));
            this._ftp = this.CreaClientFTPperIlCliente(chiapparoli);
            string remotePath = Path.Combine("/OUT/Esiti", fileName);
            int num = (int)this._ftp.UploadFile(str, remotePath, FtpRemoteExists.Overwrite, false, FtpVerify.None, (Action<FtpProgress>)null);
            this._ftp.Disconnect();
            if (!Directory.Exists("C:\\FTP\\CLIENTI\\CHIAPPAROLI\\OUT\\ESITI\\inviati"))
                Directory.CreateDirectory("C:\\FTP\\CLIENTI\\CHIAPPAROLI\\OUT\\ESITI\\inviati");
            System.IO.File.Move(str, Path.Combine("C:\\FTP\\CLIENTI\\CHIAPPAROLI\\OUT\\ESITI\\inviati", fileName));
        }

        private List<string> ProduciFileTrackingChiapparoli(
          List<Chiapparoli_EsitiOUT> esitiPiuRecenti)
        {
            List<string> stringList = new List<string>();
            foreach (Chiapparoli_EsitiOUT chiapparoliEsitiOut in esitiPiuRecenti)
            {
                string str = (chiapparoliEsitiOut.SedeChiapparoli.Length > 2 ? chiapparoliEsitiOut.SedeChiapparoli.Substring(0, 2) : chiapparoliEsitiOut.SedeChiapparoli.PadRight(2, ' ')) + (chiapparoliEsitiOut.CodiceDitta.Length > 2 ? chiapparoliEsitiOut.CodiceDitta.Substring(0, 2) : chiapparoliEsitiOut.CodiceDitta.PadRight(2, ' ')) + (chiapparoliEsitiOut.NumeroProgressivo.Length > 9 ? chiapparoliEsitiOut.NumeroProgressivo.Substring(0, 9) : chiapparoliEsitiOut.NumeroProgressivo.PadRight(9, ' ')) + (chiapparoliEsitiOut.PosizioneRiga.Length > 3 ? chiapparoliEsitiOut.PosizioneRiga.Substring(0, 3) : chiapparoliEsitiOut.PosizioneRiga.PadRight(3, ' ')) + (chiapparoliEsitiOut.CodiceResa.Length > 4 ? chiapparoliEsitiOut.CodiceResa.Substring(0, 4) : chiapparoliEsitiOut.CodiceResa.PadRight(4, ' ')) + (chiapparoliEsitiOut.DataResaAAMMGG.Length > 6 ? chiapparoliEsitiOut.DataResaAAMMGG.Substring(0, 6) : chiapparoliEsitiOut.DataResaAAMMGG.PadRight(6, ' ')) + (chiapparoliEsitiOut.OraResa.Length > 6 ? chiapparoliEsitiOut.OraResa.Substring(0, 6) : chiapparoliEsitiOut.OraResa.PadRight(6, ' ')) + (chiapparoliEsitiOut.RigaNote.Length > 40 ? chiapparoliEsitiOut.RigaNote.Substring(0, 40) : chiapparoliEsitiOut.RigaNote.PadRight(40, ' ')) + (chiapparoliEsitiOut.RiferimentoVettore.Length > 15 ? chiapparoliEsitiOut.RiferimentoVettore.Substring(0, 15) : chiapparoliEsitiOut.RiferimentoVettore.PadRight(15, ' ')) + (chiapparoliEsitiOut.DataRiferimentoVettoreAAMMGG.Length > 6 ? chiapparoliEsitiOut.DataRiferimentoVettoreAAMMGG.Substring(0, 6) : chiapparoliEsitiOut.DataRiferimentoVettoreAAMMGG.PadRight(6, ' ')) + chiapparoliEsitiOut.RiferimentoSubVettore.PadRight(15, ' ') + chiapparoliEsitiOut.DataRiferimentoSubVettoreAAMMGG.PadRight(6, ' ') + chiapparoliEsitiOut.Colli.PadRight(6, '0') + chiapparoliEsitiOut.Peso2d.PadRight(8, '0') + chiapparoliEsitiOut.Volume3d.PadRight(8, '0') + chiapparoliEsitiOut.PesoTassato.PadRight(6, '0') + chiapparoliEsitiOut.ImportoTotaleSpedizione2d.PadRight(9, '0') + chiapparoliEsitiOut.Filler.PadRight(9, ' ');
                stringList.Add(str);
            }
            return stringList;
        }

        private void ProduciEsitiDamora()
        {
            List<IGrouping<string, DAMORA_EsitiOUT>> list1 = this.EsitiDaComunicareDamora.GroupBy<DAMORA_EsitiOUT, string>((Func<DAMORA_EsitiOUT, string>)(x => x.rifExt)).ToList<IGrouping<string, DAMORA_EsitiOUT>>();
            List<DAMORA_EsitiOUT> source1 = new List<DAMORA_EsitiOUT>();
            if (list1.Count <= 0)
                return;
            foreach (IGrouping<string, DAMORA_EsitiOUT> source2 in list1)
            {
                DAMORA_EsitiOUT damoraEsitiOut = source2.FirstOrDefault<DAMORA_EsitiOUT>((Func<DAMORA_EsitiOUT, bool>)(x => x.DescrizioneEsito == "CONSEGNATA"));
                if (damoraEsitiOut != null)
                    source1.Add(damoraEsitiOut);
                else
                    source1.Add(source2.Last<DAMORA_EsitiOUT>());
            }
            string str = "C:\\FTP\\CLIENTI\\DAMORA\\OUT\\ESITI\\DAMORA_" + DateTime.Now.ToString("yyyMMddHHmmss") + ".txt";
            string fileName = Path.GetFileName(str);
            if (!Directory.Exists("C:\\FTP\\CLIENTI\\DAMORA\\OUT\\ESITI\\"))
                Directory.CreateDirectory("C:\\FTP\\CLIENTI\\DAMORA\\OUT\\ESITI\\");
            if (System.IO.File.Exists(str))
                System.IO.File.Delete(str);
            CustomerSpec damora = CustomerConnections.DAMORA;
            List<DAMORA_EsitiOUT> list2 = source1.Where<DAMORA_EsitiOUT>((Func<DAMORA_EsitiOUT, bool>)(x => x.DescrizioneEsito == "CONSEGNATA")).ToList<DAMORA_EsitiOUT>();
            System.IO.File.WriteAllLines(str, (IEnumerable<string>)this.ProduciFileTrackingDAmora(list2));
            this._ftp = this.CreaClientFTPperIlCliente(damora);
            string remotePath = Path.Combine("/OUT/Esiti", fileName);
            int num = (int)this._ftp.UploadFile(str, remotePath, FtpRemoteExists.Overwrite, false, FtpVerify.None, (Action<FtpProgress>)null);
            this._ftp.Disconnect();
            if (!Directory.Exists("C:\\FTP\\CLIENTI\\DAMORA\\OUT\\ESITI\\inviati"))
                Directory.CreateDirectory("C:\\FTP\\CLIENTI\\DAMORA\\OUT\\ESITI\\inviati");
            System.IO.File.Move(str, Path.Combine("C:\\FTP\\CLIENTI\\DAMORA\\OUT\\ESITI\\inviati", fileName));
        }

        private List<string> ProduciFileTrackingDAmora(List<DAMORA_EsitiOUT> esitiRaggruppati)
        {
            List<string> stringList = new List<string>();
            foreach (DAMORA_EsitiOUT damoraEsitiOut in esitiRaggruppati)
            {
                if (!string.IsNullOrEmpty(damoraEsitiOut.rifExt))
                {
                    string str = (damoraEsitiOut.rifExt.Length > 20 ? damoraEsitiOut.rifExt.Substring(0, 20) : damoraEsitiOut.rifExt.PadRight(20, ' ')) + (damoraEsitiOut.dataEsito.Length > 6 ? damoraEsitiOut.dataEsito.Substring(0, 6) : damoraEsitiOut.dataEsito.PadRight(6, ' '));
                    stringList.Add(str);
                }
            }
            return stringList;
        }

        private void ProduciEsitiLoreal()
        {
            try
            {
                CustomerSpec logistica93 = CustomerConnections.Logistica93;
                IEnumerable<IGrouping<string, LorealEsiti>> groupings = this.EsitiDaComunicareLoreal.GroupBy<LorealEsiti, string>((Func<LorealEsiti, string>)(x => x.E_NumeroDDT));
                List<LorealEsiti> lorealEsitiList = new List<LorealEsiti>();
                foreach (IEnumerable<LorealEsiti> lorealEsitis in groupings)
                {
                    foreach (LorealEsiti lorealEsiti1 in lorealEsitis)
                    {
                        LorealEsiti r = lorealEsiti1;
                        LorealEsiti lorealEsiti2 = lorealEsitiList.FirstOrDefault<LorealEsiti>((Func<LorealEsiti, bool>)(x => x.E_NumeroDDT == r.E_NumeroDDT));
                        if (lorealEsiti2 != null)
                        {
                            if (lorealEsiti2.statoUNITEX != 30)
                            {
                                if (r.statoUNITEX == 30)
                                {
                                    lorealEsitiList.Remove(lorealEsiti2);
                                    lorealEsitiList.Add(r);
                                    break;
                                }
                                if (r.statoUNITEX > lorealEsiti2.statoUNITEX)
                                {
                                    lorealEsitiList.Remove(lorealEsiti2);
                                    lorealEsitiList.Add(r);
                                }
                            }
                            else
                                break;
                        }
                        else
                            lorealEsitiList.Add(r);
                    }
                }
                if (lorealEsitiList.Count <= 0)
                    return;
                string str1 = "C:\\FTP\\CLIENTI\\Logistica93\\OUT\\ESITI\\LOREAL_" + DateTime.Now.ToString("yyyMMddHHmmss") + ".txt";
                string fileName = Path.GetFileName(str1);
                string str2 = Path.Combine(Path.GetDirectoryName(str1), "inviati");
                if (!Directory.Exists(str2))
                    Directory.CreateDirectory(str2);
                if (!Directory.Exists("C:\\FTP\\CLIENTI\\Logistica93\\OUT\\ESITI\\"))
                    Directory.CreateDirectory("C:\\FTP\\CLIENTI\\Logistica93\\OUT\\ESITI\\");
                System.IO.File.WriteAllLines(str1, (IEnumerable<string>)this.ProduciFileTrackingLoreal(lorealEsitiList));
                SftpClient sftpClient = this.CreaClientSFTPperIlCliente(logistica93);
                sftpClient.ChangeDirectory(logistica93.PathEsitiDelCliente);
                string workingDirectory = sftpClient.WorkingDirectory;
                sftpClient.ListDirectory(workingDirectory, (Action<int>)null);
                using (FileStream fileStream = new FileStream(str1, FileMode.Open))
                {
                    sftpClient.UploadFile((Stream)fileStream, Path.GetFileName(str1), (Action<ulong>)null);
                    fileStream.Close();
                }
                string destFileName = Path.Combine(str2, fileName);
                System.IO.File.Move(str1, destFileName);
                ((BaseClient)sftpClient).Disconnect();
            }
            finally
            {
                this.EsitiDaComunicareLoreal.Clear();
            }
        }

        private List<string> ProduciFileTrackingLoreal(List<LorealEsiti> esitiRaggruppati)
        {
            List<string> stringList = new List<string>();
            foreach (LorealEsiti lorealEsiti in esitiRaggruppati)
            {
                while (lorealEsiti.E_NumeroDDT.Length < 10)
                    lorealEsiti.E_NumeroDDT = "0" + lorealEsiti.E_NumeroDDT;
                string str = (lorealEsiti.E_NumeroDDT.Length > 10 ? lorealEsiti.E_NumeroDDT.Substring(0, 10) : lorealEsiti.E_NumeroDDT.PadRight(10, ' ')) + (lorealEsiti.E_RiferimentoNumeroConsegnaSAP.Length > 10 ? lorealEsiti.E_RiferimentoNumeroConsegnaSAP.Substring(0, 10) : lorealEsiti.E_RiferimentoNumeroConsegnaSAP.PadRight(10, ' ')) + (lorealEsiti.E_DataConsegnaADestino.Length > 8 ? lorealEsiti.E_DataConsegnaADestino.Substring(0, 8) : lorealEsiti.E_DataConsegnaADestino.PadRight(8, ' ')) + (lorealEsiti.E_Causale.Length > 2 ? lorealEsiti.E_Causale.Substring(0, 2) : lorealEsiti.E_Causale.PadRight(2, ' ')) + (lorealEsiti.E_SottoCausale.Length > 3 ? lorealEsiti.E_SottoCausale.Substring(0, 3) : lorealEsiti.E_SottoCausale.PadRight(3, ' ')) + (lorealEsiti.E_RiferimentoCorriere.Length > 20 ? lorealEsiti.E_RiferimentoCorriere.Substring(0, 20) : lorealEsiti.E_RiferimentoCorriere.PadRight(20, ' '));
                stringList.Add(str);
            }
            return stringList;
        }

        private void ProduciEsitiSTM()
        {
            List<IGrouping<string, STM_EsitiOut>> list = this.EsitiDaCoumicareSTM.GroupBy<STM_EsitiOut, string>((Func<STM_EsitiOut, string>)(x => x.regione)).ToList<IGrouping<string, STM_EsitiOut>>();
            CustomerSpec stmGroup = CustomerConnections.STMGroup;
            if (list.Count <= 0)
                return;
            foreach (IGrouping<string, STM_EsitiOut> source in list)
            {
                try
                {
                    List<STM_EsitiOut> esitiRegionali = new List<STM_EsitiOut>();
                    string str1 = source.First<STM_EsitiOut>().regione.ToLower();
                    if (str1 == "Trentino-Alto Adige/Südtirol".ToLower())
                        str1 = "triveneto";
                    else if (str1 == "friuli-venezia giulia")
                        str1 = "friuli";
                    string str2 = CustomerConnections.STMGroup.PathEsiti + "\\esitistm_" + str1 + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".txt";
                    string str3 = Path.Combine(Path.GetDirectoryName(str2), "inviati");
                    foreach (STM_EsitiOut stmEsitiOut in (IEnumerable<STM_EsitiOut>)source)
                    {
                        if (stmEsitiOut.Descrizione_Tracking == "CONSEGNATA")
                            esitiRegionali.Add(stmEsitiOut);
                    }
                    string fileName = Path.GetFileName(str2);
                    Path.Combine(stmGroup.PathEsitiDelCliente, fileName);
                    List<string> stringList = this.ProduciFileTrackingSTM(esitiRegionali);
                    if (stringList.Count<string>() > 0)
                    {
                        System.IO.File.WriteAllLines(str2, (IEnumerable<string>)stringList);
                        SftpClient sftpClient = this.CreaClientSFTPperIlCliente(stmGroup);
                        sftpClient.ChangeDirectory(stmGroup.PathEsitiDelCliente);
                        using (FileStream fileStream = new FileStream(str2, FileMode.Open))
                        {
                            sftpClient.BufferSize = 4096U;
                            sftpClient.UploadFile((Stream)fileStream, Path.GetFileName(str2), (Action<ulong>)null);
                            fileStream.Close();
                            ((BaseClient)sftpClient).Disconnect();
                        }
                        if (!Directory.Exists(str3))
                            Directory.CreateDirectory(str3);
                        string destFileName = Path.Combine(str3, fileName);
                        System.IO.File.Move(str2, destFileName);
                        ((BaseClient)sftpClient).Disconnect();
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                }
            }
            this.EsitiDaCoumicareSTM.Clear();
        }

        private List<string> ProduciFileTrackingSTM(List<STM_EsitiOut> esitiRegionali)
        {
            List<string> stringList = new List<string>();
            foreach (STM_EsitiOut stmEsitiOut in esitiRegionali)
            {
                string str = (stmEsitiOut.NumDDT.Length > 20 ? stmEsitiOut.NumDDT.Substring(0, 20) : stmEsitiOut.NumDDT.PadLeft(20, ' ')) + (stmEsitiOut.DataConsegnaEffettiva.Length > 8 ? stmEsitiOut.DataConsegnaTassativa.Substring(0, 8) : stmEsitiOut.DataConsegnaEffettiva.PadLeft(8, ' ')) + " " + (stmEsitiOut.DataSpedizione.Length > 8 ? stmEsitiOut.DataSpedizione.Substring(0, 8) : stmEsitiOut.DataSpedizione.PadLeft(8)) + (stmEsitiOut.CittaDestinatario.Length > 20 ? stmEsitiOut.CittaDestinatario.Substring(0, 20) : stmEsitiOut.CittaDestinatario.PadRight(20, ' ')) + (stmEsitiOut.DataConsegnaTassativa.Length > 8 ? stmEsitiOut.DataConsegnaTassativa.Substring(0, 8) : stmEsitiOut.DataConsegnaTassativa.PadLeft(8, ' ')) + "  " + (stmEsitiOut.ID_Tracking.Length > 2 ? stmEsitiOut.ID_Tracking.Substring(0, 2) : stmEsitiOut.ID_Tracking.PadRight(2, ' ')) + (stmEsitiOut.Descrizione_Tracking.Length > 20 ? stmEsitiOut.Descrizione_Tracking.Substring(0, 20) : stmEsitiOut.Descrizione_Tracking.PadRight(20, ' ')) + (stmEsitiOut.DataTracking.Length > 8 ? stmEsitiOut.DataTracking.Substring(0, 8) : stmEsitiOut.DataTracking.PadRight(8, ' ')) + (stmEsitiOut.ProgressivoSpedizione.Length > 8 ? stmEsitiOut.ProgressivoSpedizione.Substring(0, 8) : stmEsitiOut.ProgressivoSpedizione.PadRight(8, '0'));
                stringList.Add(str);
            }
            return stringList;
        }

        private void ProduciEsitiCDGroup()
        {
            try
            {
                IEnumerable<IGrouping<string, CDGROUP_EsitiOUT>> groupings = this.EsitiDaCoumicareCDGroup.GroupBy<CDGROUP_EsitiOUT, string>((Func<CDGROUP_EsitiOUT, string>)(x => x.NUMERO_BOLLA));
                List<CDGROUP_EsitiOUT> cdgroupEsitiOutList = new List<CDGROUP_EsitiOUT>();
                foreach (IEnumerable<CDGROUP_EsitiOUT> cdgroupEsitiOuts in groupings)
                {
                    foreach (CDGROUP_EsitiOUT cdgroupEsitiOut1 in cdgroupEsitiOuts)
                    {
                        CDGROUP_EsitiOUT r = cdgroupEsitiOut1;
                        CDGROUP_EsitiOUT cdgroupEsitiOut2 = cdgroupEsitiOutList.FirstOrDefault<CDGROUP_EsitiOUT>((Func<CDGROUP_EsitiOUT, bool>)(x => x.NUMERO_BOLLA == r.NUMERO_BOLLA));
                        if (cdgroupEsitiOut2 != null)
                        {
                            if (cdgroupEsitiOut2.statoUNITEX != 30)
                            {
                                if (r.statoUNITEX == 30)
                                {
                                    cdgroupEsitiOutList.Remove(cdgroupEsitiOut2);
                                    cdgroupEsitiOutList.Add(r);
                                    break;
                                }
                                if (r.statoUNITEX > cdgroupEsitiOut2.statoUNITEX)
                                {
                                    cdgroupEsitiOutList.Remove(cdgroupEsitiOut2);
                                    cdgroupEsitiOutList.Add(r);
                                }
                            }
                            else
                                break;
                        }
                        else
                            cdgroupEsitiOutList.Add(r);
                    }
                }
                if (cdgroupEsitiOutList.Count <= 0)
                    return;
                string path = "C:\\FTP\\CLIENTI\\CD_GROUP_ESITI\\OUT\\TRACK_" + DateTime.Now.ToString("yyyMMddHHmmss") + ".txt";
                if (!Directory.Exists("C:\\FTP\\CLIENTI\\CD_GROUP_ESITI\\OUT"))
                    Directory.CreateDirectory("C:\\FTP\\CLIENTI\\CD_GROUP_ESITI\\OUT");
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                System.IO.File.WriteAllLines(path, (IEnumerable<string>)Automazione.ProduciFileTrackingCDGroup(cdgroupEsitiOutList));
            }
            catch (Exception ex)
            {
                Automazione._loggerCode.Error<Exception>(ex);
            }
            finally
            {
                this.EsitiDaCoumicareCDGroup.Clear();
            }
        }

        public static List<string> ProduciFileTrackingCDGroup(List<CDGROUP_EsitiOUT> list)
        {
            List<string> stringList = new List<string>();
            foreach (CDGROUP_EsitiOUT cdgroupEsitiOut in list)
            {
                string str1 = cdgroupEsitiOut.NUMERO_BOLLA.Length >= 10 ? cdgroupEsitiOut.NUMERO_BOLLA : cdgroupEsitiOut.NUMERO_BOLLA.PadLeft(10, '0');
                string str2 = cdgroupEsitiOut.LOCALITA != null ? (cdgroupEsitiOut.LOCALITA.Length <= 15 ? cdgroupEsitiOut.LOCALITA : cdgroupEsitiOut.LOCALITA.Substring(0, 15)) : "";
                string str3 = string.Format(string.Format("{{{0}-{1}}}", (object)"0,", (object)cdgroupEsitiOut.idxMANDANTE[1]), (object)cdgroupEsitiOut.MANDANTE) + string.Format(string.Format("{{{0}-{1}}}", (object)"0,", (object)cdgroupEsitiOut.idxNUMERO_BOLLA[1]), (object)str1) + string.Format(string.Format("{{{0}-{1}}}", (object)"0,", (object)cdgroupEsitiOut.idxDATA_BOLLA[1]), (object)cdgroupEsitiOut.DATA_BOLLA) + string.Format(string.Format("{{{0}-{1}}}", (object)"0,", (object)cdgroupEsitiOut.idxRAGIONE_SOCIALE_VETTORE[1]), (object)cdgroupEsitiOut.RAGIONE_SOCIALE_VETTORE) + string.Format(string.Format("{{{0}-{1}}}", (object)"0,", (object)cdgroupEsitiOut.idxDATA_PRESA_CONS[1]), (object)cdgroupEsitiOut.DATA_PRESA_CONS) + string.Format(string.Format("{{{0}-{1}}}", (object)"0,", (object)cdgroupEsitiOut.idxSTATO_CONSEGNA[1]), (object)cdgroupEsitiOut.STATO_CONSEGNA) + string.Format(string.Format("{{{0}-{1}}}", (object)"0,", (object)cdgroupEsitiOut.idxDESCRIZIONE_STATO_CONSEGNA[1]), (object)cdgroupEsitiOut.DESCRIZIONE_STATO_CONSEGNA) + string.Format(string.Format("{{{0}-{1}}}", (object)"0,", (object)cdgroupEsitiOut.idxDATA[1]), (object)cdgroupEsitiOut.DATA) + string.Format(string.Format("{{{0}-{1}}}", (object)"0,", (object)cdgroupEsitiOut.idxLOCALITA[1]), (object)str2) + string.Format(string.Format("{{{0}-{1}}}", (object)"0,", (object)cdgroupEsitiOut.idxRIFVETTORE[1]), (object)cdgroupEsitiOut.RIFVETTORE);
                stringList.Add(str3);
            }
            return stringList;
        }

        private List<EventTracking> CambiTMSDelCliente(
          CustomerSpec customer,
          string fromTimestamp)
        {
            try
            {
                ((IEnumerable<string>)System.IO.File.ReadAllLines("checkedIDTraking")).ToList<string>();
                List<EventTracking> eventTrackingList = new List<EventTracking>();
                int num1 = 1;
                int num2 = 500;
                RestClient restClient1 = new RestClient(Automazione.endpointAPI_UNITEX + string.Format("/api/tms/shipment/tracking/changes/{0}/{1}?FromTimeStamp={2}", (object)num2, (object)num1, (object)fromTimestamp));
                this.LastCheckChangesTMS = DateTime.Now;
                restClient1.Timeout = -1;
                RestRequest request1 = new RestRequest(Method.GET);
                request1.AddHeader("Authorization", "Bearer " + customer.tokenAPI);
                RootobjectShipmentTracking shipmentTracking1 = JsonConvert.DeserializeObject<RootobjectShipmentTracking>(restClient1.Execute((IRestRequest)request1).Content);
                if (shipmentTracking1.events != null)
                {
                    eventTrackingList.AddRange((IEnumerable<EventTracking>)((IEnumerable<EventTracking>)shipmentTracking1.events).ToList<EventTracking>());
                    int maxPages = shipmentTracking1.result.maxPages;
                    while (maxPages > 1)
                    {
                        ++num1;
                        --maxPages;
                        Debug.WriteLine((object)maxPages);
                        RestClient restClient2 = new RestClient(Automazione.endpointAPI_UNITEX + string.Format("/api/tms/shipment/tracking/changes/{0}/{1}?FromTimeStamp={2}", (object)num2, (object)num1, (object)fromTimestamp));
                        RestRequest request2 = new RestRequest(Method.GET);
                        request2.AddHeader("Authorization", "Bearer " + customer.tokenAPI);
                        request2.AlwaysMultipartFormData = true;
                        RootobjectShipmentTracking shipmentTracking2 = JsonConvert.DeserializeObject<RootobjectShipmentTracking>(restClient2.Execute((IRestRequest)request2).Content);
                        if (shipmentTracking2 != null && shipmentTracking2.events != null)
                            eventTrackingList.AddRange((IEnumerable<EventTracking>)((IEnumerable<EventTracking>)shipmentTracking2.events).ToList<EventTracking>());
                    }
                }
                return eventTrackingList;
            }
            catch (Exception ex)
            {
                Automazione._loggerCode.Error<Exception>(ex);
                if (ex.Message != this.LastException.Message || this.DateLastException + TimeSpan.FromHours(1.0) < DateTime.Now)
                {
                    this.DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("CambiTMS", ex);
                }
                this.LastException = ex;
                return (List<EventTracking>)null;
            }
        }

        private void AggiungiAllaListaLesito(
          CustomerSpec cust,
          EventTracking shipTrackingUnitexNR,
          Shipment shipUnitex)
        {
            DateTime result = DateTime.MinValue;
            DateTime.TryParse(shipTrackingUnitexNR.timeStamp.ToString(), out result);
            DateTime? nullable;
            if (result.Year < 2000)
            {
                shipTrackingUnitexNR.timeStamp = (object)shipTrackingUnitexNR.creation;
                DateTime.TryParse(shipTrackingUnitexNR.creation.ToString(), out result);
                if (result.Year < 2000)
                {
                    if (!string.IsNullOrEmpty(shipTrackingUnitexNR.timeStamp.ToString()))
                        result = this.RecuperaTSDaStringa(shipTrackingUnitexNR.timeStamp.ToString());
                    if (result.Year < 2000)
                    {
                        nullable = shipTrackingUnitexNR.creation;
                        if (!string.IsNullOrEmpty(nullable.ToString()))
                        {
                            nullable = shipTrackingUnitexNR.creation;
                            result = this.RecuperaTSDaStringa(nullable.ToString());
                        }
                        else
                            Automazione._loggerCode.Error(string.Format("non è stato possibile convertire il timestamp in datetime per il tracking trackid:{0} shipid:{1}", (object)shipTrackingUnitexNR.id, (object)shipTrackingUnitexNR.shipID));
                    }
                }
            }
            EventTracking shipTrackingUnitex = shipTrackingUnitexNR;
            if (cust == CustomerConnections.PHARDIS || cust == CustomerConnections.DIFARCO || cust == CustomerConnections.StockHouse)
            {
                if (string.IsNullOrEmpty(shipUnitex.insideRef))
                    return;
                CDGROUP_StatiDocumento cdgroupStatiDocumento = this.statiDocumemtoCDGroup.FirstOrDefault<CDGROUP_StatiDocumento>((Func<CDGROUP_StatiDocumento, bool>)(x => x.IdUnitex == shipTrackingUnitex.statusID));
                if (cdgroupStatiDocumento == null)
                    return;
                string codiceStato = cdgroupStatiDocumento.CodiceStato;
                CDGROUP_EsitiOUT cdgroupEsitiOut = new CDGROUP_EsitiOUT();
                cdgroupEsitiOut.MANDANTE = shipUnitex.insideRef;
                cdgroupEsitiOut.NUMERO_BOLLA = shipUnitex.externRef;
                nullable = shipUnitex.docDate;
                DateTime dateTime;
                string str1;
                if (!nullable.HasValue)
                {
                    str1 = "        ";
                }
                else
                {
                    nullable = shipUnitex.docDate;
                    dateTime = nullable.Value;
                    str1 = dateTime.ToString("yyyyMMdd");
                }
                cdgroupEsitiOut.DATA_BOLLA = str1;
                cdgroupEsitiOut.RAGIONE_SOCIALE_VETTORE = "UNITEXPRESS";
                nullable = shipUnitex.docDate;
                string str2;
                if (!nullable.HasValue)
                {
                    str2 = "        ";
                }
                else
                {
                    nullable = shipUnitex.docDate;
                    dateTime = nullable.Value;
                    str2 = dateTime.ToString("yyyyMMdd");
                }
                cdgroupEsitiOut.DATA_PRESA_CONS = str2;
                cdgroupEsitiOut.STATO_CONSEGNA = codiceStato;
                cdgroupEsitiOut.DESCRIZIONE_STATO_CONSEGNA = shipTrackingUnitex.statusDes;
                cdgroupEsitiOut.DATA = result.ToString("yyyyMMdd");
                cdgroupEsitiOut.RIFVETTORE = shipUnitex.docNumber;
                cdgroupEsitiOut.statoUNITEX = shipTrackingUnitex.statusID;
                this.EsitiDaCoumicareCDGroup.Add(cdgroupEsitiOut);
            }
            else if (cust == CustomerConnections.STMGroup)
            {
                STM_StatiDocumento stmStatiDocumento = this.statiDocumemtoSTM.FirstOrDefault<STM_StatiDocumento>((Func<STM_StatiDocumento, bool>)(x => x.IdUnitex == shipTrackingUnitex.statusID));
                if (stmStatiDocumento == null)
                    return;
                string codiceStato = stmStatiDocumento.CodiceStato;
                if (string.IsNullOrEmpty(shipUnitex.insideRef))
                    return;
                if (result.Year < 2000)
                    shipTrackingUnitex.timeStamp = (object)DateTime.Now;
                GeoClass geoClass = this.GeoTab.FirstOrDefault<GeoClass>((Func<GeoClass, bool>)(x => x.cap == shipUnitex.lastStopZipCode));
                STM_EsitiOut stmEsitiOut1 = new STM_EsitiOut();
                stmEsitiOut1.CittaDestinatario = shipUnitex.firstStopLocation;
                stmEsitiOut1.DataConsegnaEffettiva = shipTrackingUnitex.statusID == 30 ? result.ToString("yyyyMMdd") : "        ";
                stmEsitiOut1.DataConsegnaTassativa = "        ";
                STM_EsitiOut stmEsitiOut2 = stmEsitiOut1;
                nullable = shipUnitex.docDate;
                string str = nullable.Value.ToString("ddMMyyyy");
                stmEsitiOut2.DataSpedizione = str;
                stmEsitiOut1.DataTracking = result.ToString("ddMMyyyy");
                stmEsitiOut1.Descrizione_Tracking = shipTrackingUnitex.statusDes;
                stmEsitiOut1.ID_Tracking = codiceStato;
                stmEsitiOut1.NumDDT = shipUnitex.externRef;
                stmEsitiOut1.ProgressivoSpedizione = shipUnitex.docNumber.Split('/')[0];
                stmEsitiOut1.regione = geoClass != null ? geoClass.regione : "ND";
                this.EsitiDaCoumicareSTM.Add(stmEsitiOut1);
            }
            else if (cust == CustomerConnections.Logistica93)
            {
                Logistica93_StatiDocumento logistica93StatiDocumento = this.statiDocumemtoLoreal.FirstOrDefault<Logistica93_StatiDocumento>((Func<Logistica93_StatiDocumento, bool>)(x => x.IdUnitex == shipTrackingUnitex.statusID));
                if (logistica93StatiDocumento == null)
                    return;
                string str = "000";
                if (logistica93StatiDocumento.CodiceStato == "03")
                    str = "602";
                if (logistica93StatiDocumento.CodiceStato == "03" && shipTrackingUnitex.statusID == 61)
                    str = "604";
                this.EsitiDaComunicareLoreal.Add(new LorealEsiti()
                {
                    E_NumeroDDT = shipUnitex.externRef,
                    E_RiferimentoNumeroConsegnaSAP = shipUnitex.insideRef,
                    E_DataConsegnaADestino = result.ToString("yyyyMMdd"),
                    E_Causale = logistica93StatiDocumento.CodiceStato,
                    E_SottoCausale = str,
                    E_RiferimentoCorriere = shipUnitex.docNumber,
                    E_Filler1 = "",
                    E_Note = shipTrackingUnitex.info,
                    E_Filler2 = "",
                    statoUNITEX = shipTrackingUnitex.statusID
                });
            }
            else if (cust == CustomerConnections.DAMORA)
                this.EsitiDaComunicareDamora.Add(new DAMORA_EsitiOUT()
                {
                    dataEsito = result.ToString("ddMMyy"),
                    DescrizioneEsito = shipTrackingUnitex.statusDes,
                    rifExt = shipUnitex.externRef
                });
            else if (cust != CustomerConnections.CHIAPPAROLI)
                ;
        }

        private EventTracking RaddrizzaTracking(
          EventTracking shipTrackingUnitexNR,
          CustomerSpec cust,
          Shipment shipUnitex,
          DateTime DTTimestamp)
        {
            if (shipTrackingUnitexNR.statusID == 30 && cust.EsitiDaRaddrizzare)
            {
                JsonConvert.DeserializeObject<EspritecShipment.ResultShipmentTracking>(EspritecShipment.RestEspritecGetTracking((long)shipUnitex.id, cust.tokenAPI).Content);
                int num = this.RecuperaGiorniResaOttimali(this.GeoTab.FirstOrDefault<GeoClass>((Func<GeoClass, bool>)(x => x.cap == shipUnitex.lastStopZipCode)), shipUnitex.ownerAgency);
                if (!(DTTimestamp.Date + TimeSpan.FromDays((double)num) > shipUnitex.pickupDateTime.Value.Date))
                    ;
            }
            return shipTrackingUnitexNR;
        }

        private int RecuperaGiorniResaOttimali(GeoClass geo, string ownerAgency)
        {
            if (geo.regione.ToLower() == "abruzzo" || geo.regione.ToLower() == "basilicata" || geo.regione.ToLower() == "calabria" || geo.regione.ToLower() == "campania" || geo.regione.ToLower() == "emilia-romagna" || geo.regione.ToLower() == "friuli-venezia giulia" || geo.regione.ToLower() == "lazio" || geo.regione.ToLower() == "liguria")
                return geo.isCapoluogo ? 2 : 3;
            return geo.regione.ToLower() == "lombardia" ? (ownerAgency == "01" ? (geo.isCapoluogo ? 2 : 3) : 1) : (geo.regione.ToLower() == "marche" || geo.regione.ToLower() == "molise" || geo.regione.ToLower() == "piemonte" || geo.regione.ToLower() == "puglia" || geo.regione.ToLower() == "sardegna" || geo.regione.ToLower() == "sicilia" || geo.regione.ToLower() == "toscana" || geo.regione.ToLower() == "trentino-alto adige" || geo.regione.ToLower() == "umbria" || geo.regione.ToLower() == "valle d'aosta" || !(geo.regione.ToLower() == "veneto") ? (geo.isCapoluogo ? 2 : 3) : (geo.isCapoluogo ? 2 : 3));
        }

        private string DecodificaTipoEsito3C(int statusID)
        {
            switch (statusID)
            {
                case 1:
                    return "Q";
                case 10:
                    return "P";
                case 30:
                    return "D";
                case 50:
                    return "G";
                case 55:
                    return "G";
                case 61:
                    return "T";
                default:
                    return "";
            }
        }

        private DateTime RecuperaTSDaStringa(string v)
        {
            DateTime result = DateTime.MinValue;
            string s = v.Split(' ')[0];
            if (((IEnumerable<string>)s.Split('/')).Count<string>() == 3)
                DateTime.TryParseExact(s, "dd/MM/yyyy", (IFormatProvider)null, DateTimeStyles.None, out result);
            return result;
        }

        private void ScriviLastCheckChangesTMS(bool append)
        {
            if (!append)
                System.IO.File.WriteAllText(this.PathLastCheckChangesFileTMS, this.LastCheckChangesTMS.ToString());
            else
                System.IO.File.AppendAllText(this.PathLastCheckChangesFileTMS, "\r\n" + this.LastCheckChangesTMS.ToString());
        }

        private Shipment RecuperaShipUnitexByShipmentID(int shipID)
        {
            Shipment shipment = new Shipment();
            RestClient restClient = new RestClient(Automazione.endpointAPI_UNITEX + string.Format("/api/tms/shipment/get/{0}", (object)shipID));
            restClient.Timeout = 5000;
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + this.token_UNITEX);
            IRestResponse restResponse = restClient.Execute((IRestRequest)request);
            if (restResponse.IsSuccessful)
                shipment = JsonConvert.DeserializeObject<RootobjectShipment>(restResponse.Content).shipment;
            return shipment;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            this.timerAggiornamentoCiclo.Stop();
            try
            {
                this.RecuperaConnessione();
                this.ControllaSeIClientiCiHannoInviatoQualcosa();
            }
            catch (Exception ex)
            {
                Automazione._loggerCode.Error<Exception>(ex);
                if (ex.Message != this.LastException.Message || this.DateLastException + TimeSpan.FromHours(1.0) < DateTime.Now)
                {
                    this.DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev(nameof(OnTimedEvent), ex);
                }
                this.LastException = ex;
            }
            finally
            {
                this.timerAggiornamentoCiclo.Start();
            }
        }

        private void CorreggiVolumeCDLTraDate(DateTime dateTime1, DateTime dateTime2)
        {
            foreach (UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment shipment in this.GetShipments(dateTime1, dateTime2))
            {
                RootobjectGoodsUpdate rootobjectGoodsUpdate = this.RecuperaLeRigheLDV(shipment.id);
                if (rootobjectGoodsUpdate != null && rootobjectGoodsUpdate.goods.cube > 0.1M)
                    this.AggiornaGoods(new GoodNewShipmentTMS()
                    {
                        cube = 0M,
                        depth = 0M,
                        height = 0M,
                        meters = 0M,
                        width = 0M
                    });
            }
        }

        private void AggiornaGoods(GoodNewShipmentTMS upd)
        {
            string str = "/api/tms/shipment/goods/update";
            RestClient restClient = new RestClient(Automazione.endpointAPI_UNITEX);
            RestRequest request = new RestRequest(str, Method.POST);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddJsonBody((object)upd);
            restClient.Timeout = -1;
            request.AddHeader("Authorization", "Bearer " + this.token_UNITEX);
            request.AlwaysMultipartFormData = true;
            restClient.Execute((IRestRequest)request);
        }

        private RootobjectGoodsUpdate RecuperaLeRigheLDV(int id)
        {
            string str = string.Format("/api/tms/shipment/goods/list/{0}", (object)id);
            RestClient restClient = new RestClient(Automazione.endpointAPI_UNITEX);
            RestRequest request = new RestRequest(str, Method.GET);
            restClient.Timeout = -1;
            request.AddHeader("Authorization", "Bearer " + this.token_UNITEX);
            request.AlwaysMultipartFormData = true;
            return JsonConvert.DeserializeObject<RootobjectGoodsUpdate>(restClient.Execute((IRestRequest)request).Content);
        }

        public List<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment> GetShipments(
          DateTime dateTime1,
          DateTime dateTime2)
        {
            List<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment> shipments = new List<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>();
            this.RecuperaConnessione();
            try
            {
                int num1 = 1;
                int num2 = 50;
                string str = string.Format("/api/tms/shipment/list/{0}/{1}?StartDate={2}&EndDate={3}", (object)num2, (object)num1, (object)dateTime1.ToString("MM-dd-yyyy"), (object)dateTime2.ToString("MM-dd-yyyy"));
                RestClient restClient = new RestClient(Automazione.endpointAPI_UNITEX);
                RestRequest request1 = new RestRequest(str, Method.GET);
                restClient.Timeout = -1;
                request1.AddHeader("Authorization", "Bearer " + this.token_UNITEX);
                request1.AlwaysMultipartFormData = true;
                TmsShipmentList tmsShipmentList1 = JsonConvert.DeserializeObject<TmsShipmentList>(restClient.Execute((IRestRequest)request1).Content);
                if (tmsShipmentList1 != null && tmsShipmentList1.shipments != null)
                {
                    shipments = ((IEnumerable<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>)tmsShipmentList1.shipments).ToList<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>();
                    int maxPages = tmsShipmentList1.result.maxPages;
                    while (maxPages > 1)
                    {
                        ++num1;
                        --maxPages;
                        RestRequest request2 = new RestRequest(string.Format("/api/tms/shipment/list/{0}/{1}?StartDate={2}&EndDate={3}", (object)num2, (object)num1, (object)dateTime1.ToString("MM-dd-yyyy"), (object)dateTime2.ToString("MM-dd-yyyy")), Method.GET);
                        request2.AddHeader("Authorization", "Bearer " + this.token_UNITEX);
                        request2.AlwaysMultipartFormData = true;
                        TmsShipmentList tmsShipmentList2 = JsonConvert.DeserializeObject<TmsShipmentList>(restClient.Execute((IRestRequest)request2).Content);
                        if (tmsShipmentList2 != null && tmsShipmentList2.shipments != null)
                            shipments.AddRange((IEnumerable<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>)((IEnumerable<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>)tmsShipmentList2.shipments).ToList<UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX.Shipment>());
                    }
                }
            }
            catch (Exception ex)
            {
                Automazione._loggerCode.Error(ex, "EspritecAPI_UNITEX.GetShipments");
            }
            return shipments;
        }

        private void RecuperaConnessione()
        {
            if (DateTime.Now + TimeSpan.FromHours(1.0) > this.DataScadenzaToken_UNITEX)
                this.UnitexGespeAPILogin(Automazione.userAPIADMIN, Automazione.passwordAPIADMIN, out this.token_UNITEX, out this.DataScadenzaToken_UNITEX);
            if (DateTime.Now + TimeSpan.FromHours(1.0) > CustomerConnections.DIFARCO.scadenzaTokenAPI)
            {
                string token;
                DateTime scadenzaToken;
                this.UnitexGespeAPILogin(CustomerConnections.DIFARCO.userAPI, CustomerConnections.DIFARCO.pswAPI, out token, out scadenzaToken);
                CustomerConnections.DIFARCO.tokenAPI = token;
                CustomerConnections.DIFARCO.scadenzaTokenAPI = scadenzaToken;
            }
            if (DateTime.Now + TimeSpan.FromHours(1.0) > CustomerConnections.PHARDIS.scadenzaTokenAPI)
            {
                string token;
                DateTime scadenzaToken;
                this.UnitexGespeAPILogin(CustomerConnections.PHARDIS.userAPI, CustomerConnections.PHARDIS.pswAPI, out token, out scadenzaToken);
                CustomerConnections.PHARDIS.tokenAPI = token;
                CustomerConnections.PHARDIS.scadenzaTokenAPI = scadenzaToken;
            }
            if (DateTime.Now + TimeSpan.FromHours(1.0) > CustomerConnections.StockHouse.scadenzaTokenAPI)
            {
                string token;
                DateTime scadenzaToken;
                this.UnitexGespeAPILogin(CustomerConnections.StockHouse.userAPI, CustomerConnections.StockHouse.pswAPI, out token, out scadenzaToken);
                CustomerConnections.StockHouse.tokenAPI = token;
                CustomerConnections.StockHouse.scadenzaTokenAPI = scadenzaToken;
            }
            if (DateTime.Now + TimeSpan.FromHours(1.0) > CustomerConnections.STMGroup.scadenzaTokenAPI)
            {
                string token;
                DateTime scadenzaToken;
                this.UnitexGespeAPILogin(CustomerConnections.STMGroup.userAPI, CustomerConnections.STMGroup.pswAPI, out token, out scadenzaToken);
                CustomerConnections.STMGroup.tokenAPI = token;
                CustomerConnections.STMGroup.scadenzaTokenAPI = scadenzaToken;
            }
            if (DateTime.Now + TimeSpan.FromHours(1.0) > CustomerConnections.Logistica93.scadenzaTokenAPI)
            {
                string token;
                DateTime scadenzaToken;
                this.UnitexGespeAPILogin(CustomerConnections.Logistica93.userAPI, CustomerConnections.Logistica93.pswAPI, out token, out scadenzaToken);
                CustomerConnections.Logistica93.tokenAPI = token;
                CustomerConnections.Logistica93.scadenzaTokenAPI = scadenzaToken;
            }
            if (!(DateTime.Now + TimeSpan.FromHours(1.0) > CustomerConnections.DAMORA.scadenzaTokenAPI))
                return;
            string token1;
            DateTime scadenzaToken1;
            this.UnitexGespeAPILogin(CustomerConnections.DAMORA.userAPI, CustomerConnections.DAMORA.pswAPI, out token1, out scadenzaToken1);
            CustomerConnections.DAMORA.tokenAPI = token1;
            CustomerConnections.DAMORA.scadenzaTokenAPI = scadenzaToken1;
        }

        private void ControllaSeIClientiCiHannoInviatoQualcosa()
        {
            foreach (CustomerSpec customer in CustomerConnections.customers)
            {
                try
                {
                    foreach (string file in Directory.GetFiles(customer.LocalWorkPath))
                        System.IO.File.Delete(file);
                    if (customer == CustomerConnections.Logistica93)
                    {
                        try
                        {
                            SftpClient sftpClient = this.CreaClientSFTPperIlCliente(customer);
                            sftpClient.ChangeDirectory(customer.RemoteINCustomerPath);
                            List<SftpFile> list = sftpClient.ListDirectory(customer.RemoteINCustomerPath, (Action<int>)null).Where<SftpFile>((Func<SftpFile, bool>)(x => x.FullName.ToLower().EndsWith(".txt"))).ToList<SftpFile>();
                            if (((IEnumerable<SftpFile>)list).Any<SftpFile>((Func<SftpFile, bool>)(x => x.LastWriteTime < DateTime.Now + TimeSpan.FromMinutes(2.0))))
                            {
                                lock (this.semaphoro)
                                {
                                    foreach (SftpFile sftpFile in list)
                                    {
                                        using (Stream stream = (Stream)System.IO.File.Create(Path.Combine(customer.LocalInFilePath, Path.GetFileName(sftpFile.FullName))))
                                            sftpClient.DownloadFile(sftpFile.FullName, stream, (Action<ulong>)null);
                                    }
                                }
                            }
                          ((BaseClient)sftpClient).Disconnect();
                        }
                        catch (Exception ex)
                        {
                            if (!ex.Message.StartsWith("Local path must specify a file path and not a folder path."))
                                GestoreMail.SegnalaErroreDev("errore produzione file " + customer.ID_GESPE, ex);
                        }
                    }
                    string[] files = Directory.GetFiles(customer.LocalInFilePath, "*.*", SearchOption.TopDirectoryOnly);
                    try
                    {
                        if (((IEnumerable<string>)files).Count<string>() > 0)
                        {
                            Automazione._loggerCode.Debug(string.Format("{0} trovati {1} da processare", (object)customer.NOME, (object)((IEnumerable<string>)files).Count<string>()));
                            this.ProcessaIFileRecuperati(customer, ((IEnumerable<string>)files).ToList<string>());
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void ProcessaIFileRecuperati(CustomerSpec cust, List<string> filesDaProcessare)
        {
            foreach (string str1 in filesDaProcessare)
            {
                string fileName = Path.GetFileName(str1);
                Automazione._loggerCode.Debug("Processo il file " + fileName);
                try
                {
                    if (System.IO.File.Exists(str1))
                    {
                        List<string> fileProcessati;
                        this.InterpretaIlFileEdIserisciloInGespe(cust, str1, out fileProcessati);
                        foreach (string str2 in fileProcessati)
                        {
                            if (System.IO.File.Exists(str2))
                            {
                                Automazione._loggerCode.Debug("Conservo il file " + str2);
                                string str3 = Path.Combine(cust.LocalInFilePath, "Elaborati");
                                if (!Directory.Exists(str3))
                                    Directory.CreateDirectory(str3);
                                string str4 = Path.Combine(str3, fileName + "_" + DateTime.Now.ToString("ddMMyyyyHHssmm") + "_Elaborato");
                                if (System.IO.File.Exists(str4))
                                    System.IO.File.Move(str4, str4 + ".bk");
                                System.IO.File.Move(str2, str4);
                            }
                        }
                        Automazione._loggerCode.Debug("Processo il file " + fileName + " terminato");
                    }
                }
                catch (Exception ex)
                {
                    Automazione._loggerCode.Debug<Exception>(ex);
                    GestoreMail.SegnalaErroreDev("cloudftp " + fileName, ex);
                    System.IO.File.Move(str1, Path.Combine(cust.LocalErrorFilePath, fileName));
                }
            }
        }

        private void InterpretaIlFileEdIserisciloInGespe(
          CustomerSpec cust,
          string fr,
          out List<string> fileProcessati)
        {
            fileProcessati = new List<string>();
            fileProcessati.Add(fr);
            List<RootobjectNewShipmentTMS> source1 = new List<RootobjectNewShipmentTMS>();
            List<string> righeCSV = new List<string>();
            int num1 = 0;
            if (cust == CustomerConnections.GUNA)
            {
                string[] source2 = System.IO.File.ReadAllLines(fr);
                if (((IEnumerable<string>)source2).Count<string>() > 0)
                {
                    bool flag = false;
                    GUNA_ShipmentIN gunaShipmentIn = new GUNA_ShipmentIN();
                    for (int index = 0; index < ((IEnumerable<string>)source2).Count<string>(); ++index)
                    {
                        try
                        {
                            RootobjectNewShipmentTMS rootobjectNewShipmentTms = new RootobjectNewShipmentTMS();
                            List<ParcelNewShipmentTMS> parcelNewShipmentTmsList = new List<ParcelNewShipmentTMS>();
                            List<StopNewShipmentTMS> stopNewShipmentTmsList = new List<StopNewShipmentTMS>();
                            List<GoodNewShipmentTMS> goodNewShipmentTmsList = new List<GoodNewShipmentTMS>();
                            string str1 = source2[index];
                            if (index < ((IEnumerable<string>)source2).Count<string>())
                            {
                                if (str1.StartsWith("2"))
                                {
                                    if (!flag)
                                    {
                                        flag = true;
                                        gunaShipmentIn = new GUNA_ShipmentIN()
                                        {
                                            AASPED = str1.Substring(gunaShipmentIn.idxAASPED[0], gunaShipmentIn.idxAASPED[1]).Trim(),
                                            MMGGSPED = str1.Substring(gunaShipmentIn.idxMMGGSPED[0], gunaShipmentIn.idxMMGGSPED[1]).Trim(),
                                            NRSPED = str1.Substring(gunaShipmentIn.idxNRSPED[0], gunaShipmentIn.idxNRSPED[1]).Trim(),
                                            RAGSOC1 = str1.Substring(gunaShipmentIn.idxRAGSOC1[0], gunaShipmentIn.idxRAGSOC1[1]).Trim(),
                                            RAGSOC2 = str1.Substring(gunaShipmentIn.idxRAGSOC2[0], gunaShipmentIn.idxRAGSOC2[1]).Trim(),
                                            ADDRESS = str1.Substring(gunaShipmentIn.idxADDRESS[0], gunaShipmentIn.idxADDRESS[1]).Trim(),
                                            PTCODE = str1.Substring(gunaShipmentIn.idxPTCODE[0], gunaShipmentIn.idxPTCODE[1]).Trim(),
                                            CITY = str1.Substring(gunaShipmentIn.idxCITY[0], gunaShipmentIn.idxCITY[1]).Trim(),
                                            REGION = str1.Substring(gunaShipmentIn.idxREGION[0], gunaShipmentIn.idxREGION[1]).Trim(),
                                            COUNTRY = str1.Substring(gunaShipmentIn.idxCOUNTRY[0], gunaShipmentIn.idxCOUNTRY[1]).Trim(),
                                            NCOLLI = str1.Substring(gunaShipmentIn.idxNCOLLI[0], gunaShipmentIn.idxNCOLLI[1]).Trim(),
                                            PESO = str1.Substring(gunaShipmentIn.idxPESO[0], gunaShipmentIn.idxPESO[1]).Trim(),
                                            VOLUME = str1.Substring(gunaShipmentIn.idxVOLUME[0], gunaShipmentIn.idxVOLUME[1]).Trim(),
                                            C_ASS = str1.Substring(gunaShipmentIn.idxC_ASS[0], gunaShipmentIn.idxC_ASS[1]).Trim(),
                                            TP_INCASSO = str1.Substring(gunaShipmentIn.idxTP_INCASSO[0], gunaShipmentIn.idxTP_INCASSO[1]).Trim(),
                                            DIVISA_C_ASS = str1.Substring(gunaShipmentIn.idxDIVISA_C_ASS[0], gunaShipmentIn.idxDIVISA_C_ASS[1]).Trim(),
                                            RIFMITT_C = str1.Substring(gunaShipmentIn.idxRIFMITT_C[0], gunaShipmentIn.idxRIFMITT_C[1]).Trim(),
                                            TEL1 = str1.Substring(gunaShipmentIn.idxTEL1[0], gunaShipmentIn.idxTEL1[1]).Trim(),
                                            TEL2 = str1.Substring(gunaShipmentIn.idxTEL2[0], gunaShipmentIn.idxTEL2[1]).Trim(),
                                            TEL3 = str1.Substring(gunaShipmentIn.idxTEL3[0], gunaShipmentIn.idxTEL3[1]).Trim(),
                                            NOTE1 = str1.Substring(gunaShipmentIn.idxNOTE1[0], gunaShipmentIn.idxNOTE1[1]).Trim(),
                                            NOTE2 = str1.Substring(gunaShipmentIn.idxNOTE2[0], gunaShipmentIn.idxNOTE2[1]).Trim(),
                                            EOL = str1.Substring(gunaShipmentIn.idxEOL[0], gunaShipmentIn.idxEOL[1]).Trim()
                                        };
                                    }
                                    else
                                    {
                                        HeaderNewShipmentTMS headerNewShipmentTms = new HeaderNewShipmentTMS();
                                        headerNewShipmentTms.docDate = gunaShipmentIn.AASPED + "-" + gunaShipmentIn.MMGGSPED.Substring(0, 2) + "-" + gunaShipmentIn.MMGGSPED.Substring(2, 2);
                                        headerNewShipmentTms.publicNote = (gunaShipmentIn.NOTE1.Trim() + " " + gunaShipmentIn.NOTE2.Trim() + " " + gunaShipmentIn.TEL1.Trim() + " " + gunaShipmentIn.TEL2.Trim() + " " + gunaShipmentIn.TEL3.Trim()).Trim();
                                        headerNewShipmentTms.customerID = cust.ID_GESPE;
                                        headerNewShipmentTms.cashCurrency = gunaShipmentIn.DIVISA_C_ASS;
                                        headerNewShipmentTms.cashValue = Helper.GetDecimalFromString(gunaShipmentIn.C_ASS, 2);
                                        headerNewShipmentTms.externRef = gunaShipmentIn.NRSPED;
                                        headerNewShipmentTms.carrierType = "EDI";
                                        headerNewShipmentTms.serviceType = "S";
                                        headerNewShipmentTms.incoterm = "PF";
                                        headerNewShipmentTms.transportType = "8-25";
                                        headerNewShipmentTms.cashPayment = gunaShipmentIn.TP_INCASSO;
                                        headerNewShipmentTms.type = "Groupage";
                                        headerNewShipmentTms.cashNote = "";
                                        headerNewShipmentTms.insideRef = gunaShipmentIn.RIFMITT_C;
                                        headerNewShipmentTms.internalNote = "";
                                        stopNewShipmentTmsList.Add(new StopNewShipmentTMS()
                                        {
                                            address = "VIA PALMANOVA 71",
                                            country = "IT",
                                            description = "GUNA S.P.A.",
                                            district = "MI",
                                            zipCode = "20132",
                                            location = "MILANO",
                                            date = DateTime.Now.ToString("yyyy-MM-dd"),
                                            type = "P",
                                            time = ""
                                        });
                                        stopNewShipmentTmsList.Add(new StopNewShipmentTMS()
                                        {
                                            address = gunaShipmentIn.ADDRESS.Replace("\"", ""),
                                            country = gunaShipmentIn.COUNTRY,
                                            description = gunaShipmentIn.RAGSOC1.Trim().Replace("\"", "") + gunaShipmentIn.RAGSOC2.Trim().Replace("\"", ""),
                                            district = gunaShipmentIn.REGION,
                                            zipCode = gunaShipmentIn.PTCODE,
                                            location = gunaShipmentIn.CITY,
                                            type = "D"
                                        });
                                        goodNewShipmentTmsList.Add(new GoodNewShipmentTMS()
                                        {
                                            grossWeight = Helper.GetDecimalFromString(gunaShipmentIn.PESO, 3),
                                            cube = Helper.GetDecimalFromString(gunaShipmentIn.VOLUME, 3),
                                            packs = int.Parse(gunaShipmentIn.NCOLLI)
                                        });
                                        foreach (string str2 in gunaShipmentIn.Segnacolli)
                                        {
                                            ParcelNewShipmentTMS parcelNewShipmentTms = new ParcelNewShipmentTMS()
                                            {
                                                barcodeExt = str2
                                            };
                                            parcelNewShipmentTmsList.Add(parcelNewShipmentTms);
                                        }
                                        rootobjectNewShipmentTms.header = headerNewShipmentTms;
                                        rootobjectNewShipmentTms.parcels = parcelNewShipmentTmsList.ToArray();
                                        rootobjectNewShipmentTms.goods = goodNewShipmentTmsList.ToArray();
                                        rootobjectNewShipmentTms.stops = stopNewShipmentTmsList.ToArray();
                                        source1.Add(rootobjectNewShipmentTms);
                                        flag = false;
                                        --index;
                                    }
                                }
                                else
                                    gunaShipmentIn.Segnacolli.Add(str1.Substring(1, str1.Length).Trim());
                            }
                            else
                            {
                                gunaShipmentIn.Segnacolli.Add(str1.Substring(1, str1.Length).Trim());
                                HeaderNewShipmentTMS headerNewShipmentTms = new HeaderNewShipmentTMS()
                                {
                                    docDate = DateTime.Parse(gunaShipmentIn.AASPED + gunaShipmentIn.MMGGSPED).ToString("o")
                                };
                                foreach (string str3 in gunaShipmentIn.Segnacolli)
                                {
                                    ParcelNewShipmentTMS parcelNewShipmentTms = new ParcelNewShipmentTMS()
                                    {
                                        barcodeExt = str3
                                    };
                                    parcelNewShipmentTmsList.Add(parcelNewShipmentTms);
                                }
                                rootobjectNewShipmentTms.header = headerNewShipmentTms;
                                rootobjectNewShipmentTms.parcels = parcelNewShipmentTmsList.ToArray();
                                source1.Add(rootobjectNewShipmentTms);
                            }
                        }
                        catch (Exception ex)
                        {
                            Automazione._loggerCode.Error<Exception>(ex);
                        }
                    }
                }
            }
            else
            {
                if (cust == CustomerConnections.Logistica93)
                {
                    string[] pzFr = fr.Split('_');
                    bool flag1 = pzFr[1] == "ZCAI";
                    string path = ((IEnumerable<string>)Directory.GetFiles(Path.GetDirectoryName(fr))).Where<string>((Func<string, bool>)(x =>
                    {
                        if (!x.Contains("FSE") || !x.Contains(pzFr[2]))
                            return false;
                        return x.Contains(pzFr[3].Split('.')[0]);
                    })).FirstOrDefault<string>();
                    string[] source3 = System.IO.File.ReadAllLines(fr);
                    string[] source4 = System.IO.File.ReadAllLines(path);
                    fileProcessati.Add(path);
                    List<Logistica93_ShipmentIN> listShipLoreal = new List<Logistica93_ShipmentIN>();
                    for (int index = 0; index < ((IEnumerable<string>)source3).Count<string>(); ++index)
                    {
                        string str = source3[index];
                        ++num1;
                        Debug.WriteLine((object)num1);
                        Logistica93_ShipmentIN logistica93ShipmentIn = new Logistica93_ShipmentIN();
                        logistica93ShipmentIn.TipoRecord = str.Substring(logistica93ShipmentIn.idxTipoRecord[0], logistica93ShipmentIn.idxTipoRecord[1]).Trim();
                        logistica93ShipmentIn.NumeroBorderau = str.Substring(logistica93ShipmentIn.idxNumeroBorderau[0], logistica93ShipmentIn.idxNumeroBorderau[1]).Trim();
                        logistica93ShipmentIn.DataSpedizione = str.Substring(logistica93ShipmentIn.idxDataSpedizione[0], logistica93ShipmentIn.idxDataSpedizione[1]).Trim();
                        logistica93ShipmentIn.NumeroDDT = str.Substring(logistica93ShipmentIn.idxNumeroDDT[0], logistica93ShipmentIn.idxNumeroDDT[1]).Trim();
                        logistica93ShipmentIn.DataDDT = str.Substring(logistica93ShipmentIn.idxDataDDT[0], logistica93ShipmentIn.idxDataDDT[1]).Trim();
                        logistica93ShipmentIn.RifNConsegna = str.Substring(logistica93ShipmentIn.idxRifNConsegna[0], logistica93ShipmentIn.idxRifNConsegna[1]).Trim();
                        logistica93ShipmentIn.LuogoSpedizione = str.Substring(logistica93ShipmentIn.idxLuogoSpedizione[0], logistica93ShipmentIn.idxLuogoSpedizione[1]).Trim();
                        logistica93ShipmentIn.PesoDelivery = str.Substring(logistica93ShipmentIn.idxPesoDelivery[0], logistica93ShipmentIn.idxPesoDelivery[1]).Trim();
                        logistica93ShipmentIn.TipoCliente = str.Substring(logistica93ShipmentIn.idxTipoCliente[0], logistica93ShipmentIn.idxTipoCliente[1]).Trim();
                        logistica93ShipmentIn.Destinatario = str.Substring(logistica93ShipmentIn.idxDestinatario[0], logistica93ShipmentIn.idxDestinatario[1]).Trim();
                        logistica93ShipmentIn.Indirizzo = str.Substring(logistica93ShipmentIn.idxIndirizzo[0], logistica93ShipmentIn.idxIndirizzo[1]).Trim();
                        logistica93ShipmentIn.Localita = str.Substring(logistica93ShipmentIn.idxLocalita[0], logistica93ShipmentIn.idxLocalita[1]).Trim();
                        logistica93ShipmentIn.CAP = str.Substring(logistica93ShipmentIn.idxCAP[0], logistica93ShipmentIn.idxCAP[1]).Trim();
                        logistica93ShipmentIn.SiglaProvDestinazione = str.Substring(logistica93ShipmentIn.idxSiglaProvDestinazione[0], logistica93ShipmentIn.idxSiglaProvDestinazione[1]).Trim();
                        logistica93ShipmentIn.PIVA_CODF = str.Substring(logistica93ShipmentIn.idxPIVA_CODF[0], logistica93ShipmentIn.idxPIVA_CODF[1]).Trim();
                        logistica93ShipmentIn.DataConsegna = str.Substring(logistica93ShipmentIn.idxDataConsegna[0], logistica93ShipmentIn.idxDataConsegna[1]).Trim();
                        logistica93ShipmentIn.TipoDataConsegna = str.Substring(logistica93ShipmentIn.idxTipoDataConsegna[0], logistica93ShipmentIn.idxTipoDataConsegna[1]).Trim();
                        logistica93ShipmentIn.TipoSpedizione = str.Substring(logistica93ShipmentIn.idxTipoSpedizione[0], logistica93ShipmentIn.idxTipoSpedizione[1]).Trim();
                        logistica93ShipmentIn.ImportoContrassegno = str.Substring(logistica93ShipmentIn.idxImportoContrassegno[0], logistica93ShipmentIn.idxImportoContrassegno[1]).Trim();
                        logistica93ShipmentIn.NotaModalitaDiConsegna = str.Substring(logistica93ShipmentIn.idxNotaModalitaDiConsegna[0], logistica93ShipmentIn.idxNotaModalitaDiConsegna[1]).Trim();
                        logistica93ShipmentIn.NotaCommentiTempiConsegna = str.Substring(logistica93ShipmentIn.idxNotaCommentiTempiConsegna[0], logistica93ShipmentIn.idxNotaCommentiTempiConsegna[1]).Trim();
                        logistica93ShipmentIn.NotaEPAL = str.Substring(logistica93ShipmentIn.idxNotaEPAL[0], logistica93ShipmentIn.idxNotaEPAL[1]).Trim();
                        logistica93ShipmentIn.NotaBolla = str.Substring(logistica93ShipmentIn.idxNotaBolla[0], logistica93ShipmentIn.idxNotaBolla[1]).Trim();
                        logistica93ShipmentIn.NumeroColliDettaglio = str.Substring(logistica93ShipmentIn.idxNumeroColliDettaglio[0], logistica93ShipmentIn.idxNumeroColliDettaglio[1]).Trim();
                        logistica93ShipmentIn.NumeroColliStandard = str.Substring(logistica93ShipmentIn.idxNumeroColliStandard[0], logistica93ShipmentIn.idxNumeroColliStandard[1]).Trim();
                        logistica93ShipmentIn.NumeroEspositoriPLV = str.Substring(logistica93ShipmentIn.idxNumeroEspositoriPLV[0], logistica93ShipmentIn.idxNumeroEspositoriPLV[1]).Trim();
                        logistica93ShipmentIn.NumeroPedane = str.Substring(logistica93ShipmentIn.idxNumeroPedane[0], logistica93ShipmentIn.idxNumeroPedane[1]).Trim();
                        logistica93ShipmentIn.CodiceCorriere = str.Substring(logistica93ShipmentIn.idxCodiceCorriere[0], logistica93ShipmentIn.idxCodiceCorriere[1]).Trim();
                        logistica93ShipmentIn.ItinerarioCorriere = str.Substring(logistica93ShipmentIn.idxItinerarioCorriere[0], logistica93ShipmentIn.idxItinerarioCorriere[1]).Trim();
                        logistica93ShipmentIn.SottoZonaCorriere = str.Substring(logistica93ShipmentIn.idxSottoZonaCorriere[0], logistica93ShipmentIn.idxSottoZonaCorriere[1]).Trim();
                        logistica93ShipmentIn.NumeroPedaneEPAL = str.Substring(logistica93ShipmentIn.idxNumeroPedaneEPAL[0], logistica93ShipmentIn.idxNumeroPedaneEPAL[1]).Trim();
                        logistica93ShipmentIn.TipoTrasporto = str.Substring(logistica93ShipmentIn.idxTipoTrasporto[0], logistica93ShipmentIn.idxTipoTrasporto[1]).Trim();
                        logistica93ShipmentIn.ZonaCorriere = str.Substring(logistica93ShipmentIn.idxZonaCorriere[0], logistica93ShipmentIn.idxZonaCorriere[1]).Trim();
                        logistica93ShipmentIn.PedanaDirezionale = str.Substring(logistica93ShipmentIn.idxPedanaDirezionale[0], logistica93ShipmentIn.idxPedanaDirezionale[1]).Trim();
                        logistica93ShipmentIn.CodiceAbbinamento = str.Substring(logistica93ShipmentIn.idxCodiceAbbinamento[0], logistica93ShipmentIn.idxCodiceAbbinamento[1]).Trim();
                        logistica93ShipmentIn.NumeroOrdineCliente = str.Substring(logistica93ShipmentIn.idxNumeroOrdineCliente[0], logistica93ShipmentIn.idxNumeroOrdineCliente[1]).Trim();
                        logistica93ShipmentIn.ContrattoCorriere = str.Substring(logistica93ShipmentIn.idxContrattoCorriere[0], logistica93ShipmentIn.idxContrattoCorriere[1]).Trim();
                        logistica93ShipmentIn.Via3 = str.Substring(logistica93ShipmentIn.idxVia3[0], logistica93ShipmentIn.idxVia3[1]).Trim();
                        logistica93ShipmentIn.NumeroFattura = str.Substring(logistica93ShipmentIn.idxNumeroFattura[0], logistica93ShipmentIn.idxNumeroFattura[1]).Trim();
                        logistica93ShipmentIn.PesoPolveri = str.Substring(logistica93ShipmentIn.idxPesoPolveri[0], logistica93ShipmentIn.idxPesoPolveri[1]).Trim();
                        logistica93ShipmentIn.NumeroFiliale = str.Substring(logistica93ShipmentIn.idxNumeroFiliale[0], logistica93ShipmentIn.idxNumeroFiliale[1]).Trim();
                        logistica93ShipmentIn.TipoClienteIntestazione = str.Substring(logistica93ShipmentIn.idxTipoClienteIntestazione[0], logistica93ShipmentIn.idxTipoClienteIntestazione[1]).Trim();
                        logistica93ShipmentIn.DestinatarioFiliale = str.Substring(logistica93ShipmentIn.idxDestinatarioFiliale[0], logistica93ShipmentIn.idxDestinatarioFiliale[1]).Trim();
                        logistica93ShipmentIn.IndirizzoFiliale = str.Substring(logistica93ShipmentIn.idxIndirizzoFiliale[0], logistica93ShipmentIn.idxIndirizzoFiliale[1]).Trim();
                        logistica93ShipmentIn.LocalitaFiliale = str.Substring(logistica93ShipmentIn.idxLocalitaFiliale[0], logistica93ShipmentIn.idxLocalitaFiliale[1]).Trim();
                        logistica93ShipmentIn.CAPFiliale = str.Substring(logistica93ShipmentIn.idxCAPFiliale[0], logistica93ShipmentIn.idxCAPFiliale[1]).Trim();
                        logistica93ShipmentIn.SiglaProvDestinazioneFiliale = str.Substring(logistica93ShipmentIn.idxSiglaProvDestinazioneFiliale[0], logistica93ShipmentIn.idxSiglaProvDestinazioneFiliale[1]).Trim();
                        logistica93ShipmentIn.Filler = str.Substring(logistica93ShipmentIn.idxFiller[0], logistica93ShipmentIn.idxFiller[1]).Trim();
                        logistica93ShipmentIn.DeliveryVolume = str.Substring(logistica93ShipmentIn.idxDeliveryVolume[0], logistica93ShipmentIn.idxDeliveryVolume[1]).Trim();
                        logistica93ShipmentIn.VolumeUnit = str.Substring(logistica93ShipmentIn.idxVolumeUnit[0], logistica93ShipmentIn.idxVolumeUnit[1]).Trim();
                        logistica93ShipmentIn.PrioritàConsegna = str.Substring(logistica93ShipmentIn.idxPrioritàConsegna[0], logistica93ShipmentIn.idxPrioritàConsegna[1]).Trim();
                        listShipLoreal.Add(logistica93ShipmentIn);
                    }
                    foreach (Logistica93_ShipmentIN logistica93ShipmentIn in this.RaggruppaTestateLoreal(listShipLoreal))
                    {
                        Logistica93_ShipmentIN ShipLoreal93 = logistica93ShipmentIn;
                        RootobjectNewShipmentTMS ls = new RootobjectNewShipmentTMS();
                        List<ParcelNewShipmentTMS> parcelNewShipmentTmsList = new List<ParcelNewShipmentTMS>();
                        List<StopNewShipmentTMS> stopNewShipmentTmsList = new List<StopNewShipmentTMS>();
                        List<GoodNewShipmentTMS> goodNewShipmentTmsList = new List<GoodNewShipmentTMS>();
                        string str4 = !(ShipLoreal93.TipoSpedizione == "F") ? (!(ShipLoreal93.TipoSpedizione == "C") ? "PA" : "CS") : "PF";
                        string str5 = "";
                        bool flag2 = false;
                        DateTime dateTime;
                        if (!string.IsNullOrEmpty(ShipLoreal93.DataConsegna))
                        {
                            dateTime = DateTime.ParseExact(ShipLoreal93.DataConsegna, "yyyyMMdd", (IFormatProvider)null);
                            str5 = dateTime.ToString("MM-dd-yyyy");
                            if (ShipLoreal93.TipoDataConsegna.Trim().ToUpper() == "TASSATIVA")
                                flag2 = true;
                            else if (ShipLoreal93.TipoDataConsegna.Trim().ToUpper() == "ENTRO IL")
                                flag2 = true;
                            else if (ShipLoreal93.TipoDataConsegna.Trim().ToUpper() == "DOPO IL")
                                flag2 = true;
                        }
                        string str6 = !(ShipLoreal93.TipoTrasporto == "ZCOR") ? (!(ShipLoreal93.TipoTrasporto == "ZESP") ? (!(ShipLoreal93.TipoTrasporto == "ZDIR") ? (!(ShipLoreal93.TipoTrasporto == "ZAGE") ? (!(ShipLoreal93.TipoTrasporto == "ZCOM") ? (!(ShipLoreal93.TipoTrasporto == "ZINF") ? (!(ShipLoreal93.TipoTrasporto == "ZMKT") ? (!(ShipLoreal93.TipoTrasporto == "ZTRD") ? "S" : "TRD") : "MKT") : "INF") : "COM") : "AGE") : "D") : "P") : "S";
                        HeaderNewShipmentTMS headerNewShipmentTms = new HeaderNewShipmentTMS()
                        {
                            carrierType = "EDI",
                            serviceType = str6,
                            incoterm = str4,
                            transportType = "8-25",
                            type = "Groupage",
                            insideRef = ShipLoreal93.NumeroDDT,
                            internalNote = ShipLoreal93.NotaCommentiTempiConsegna,
                            externRef = ShipLoreal93.RifNConsegna,
                            publicNote = ShipLoreal93.NotaModalitaDiConsegna,
                            docDate = ShipLoreal93.DataDDT,
                            customerID = cust.ID_GESPE,
                            cashCurrency = "EUR",
                            cashValue = Helper.GetDecimalFromString(ShipLoreal93.ImportoContrassegno, 2),
                            cashPayment = "",
                            cashNote = ""
                        };
                        List<string> stringList1 = new List<string>();
                        List<string> stringList2 = !flag1 ? ((IEnumerable<string>)source4).Where<string>((Func<string, bool>)(x => x.Substring(10, 10) == ShipLoreal93.RifNConsegna)).ToList<string>() : ((IEnumerable<string>)source4).Where<string>((Func<string, bool>)(x => x.Substring(10, 10) == ShipLoreal93.NumeroDDT)).ToList<string>();
                        List<LorealSegnacolli> lorealSegnacolliList = new List<LorealSegnacolli>();
                        foreach (string str7 in stringList2)
                        {
                            LorealSegnacolli lorealSegnacolli = new LorealSegnacolli();
                            lorealSegnacolli.S_CheckDigit = str7.Substring(lorealSegnacolli.idxS_CheckDigit[0], lorealSegnacolli.idxS_CheckDigit[1]).Trim();
                            lorealSegnacolli.S_CodiceCliente = str7.Substring(lorealSegnacolli.idxS_CodiceCliente[0], lorealSegnacolli.idxS_CodiceCliente[1]).Trim();
                            lorealSegnacolli.S_CodiceProdotto = str7.Substring(lorealSegnacolli.idxS_CodiceProdotto[0], lorealSegnacolli.idxS_CodiceProdotto[1]).Trim();
                            lorealSegnacolli.S_DescrizioneProdotto = str7.Substring(lorealSegnacolli.idxS_DescrizioneProdotto[0], lorealSegnacolli.idxS_DescrizioneProdotto[1]).Trim();
                            lorealSegnacolli.S_Marca = str7.Substring(lorealSegnacolli.idxS_Marca[0], lorealSegnacolli.idxS_Marca[1]).Trim();
                            lorealSegnacolli.S_NumeroCollo = str7.Substring(lorealSegnacolli.idxS_NumeroCollo[0], lorealSegnacolli.idxS_NumeroCollo[1]).Trim();
                            lorealSegnacolli.S_NumeroDDT = str7.Substring(lorealSegnacolli.idxS_NumeroDDT[0], lorealSegnacolli.idxS_NumeroDDT[1]).Trim();
                            lorealSegnacolli.S_Peso = str7.Substring(lorealSegnacolli.idxS_Peso[0], lorealSegnacolli.idxS_Peso[1]).Trim();
                            lorealSegnacolli.S_TipoElaborazione = str7.Substring(lorealSegnacolli.idxS_TipoElaborazione[0], lorealSegnacolli.idxS_TipoElaborazione[1]).Trim();
                            lorealSegnacolli.S_TipoImballo = str7.Substring(lorealSegnacolli.idxS_TipoImballo[0], lorealSegnacolli.idxS_TipoImballo[1]).Trim();
                            lorealSegnacolli.S_TipoImballoMagazzino = str7.Substring(lorealSegnacolli.idxS_TipoImballoMagazzino[0], lorealSegnacolli.idxS_TipoImballoMagazzino[1]).Trim();
                            lorealSegnacolli.S_ZonaSpedizione = str7.Substring(lorealSegnacolli.idxS_ZonaSpedizione[0], lorealSegnacolli.idxS_ZonaSpedizione[1]).Trim();
                            lorealSegnacolliList.Add(lorealSegnacolli);
                        }
                        int num2 = 0;
                        foreach (LorealSegnacolli lorealSegnacolli in lorealSegnacolliList)
                        {
                            ++num2;
                            Debug.WriteLine((object)num2);
                            GoodNewShipmentTMS goodNewShipmentTms = new GoodNewShipmentTMS();
                            goodNewShipmentTms.description = lorealSegnacolli.S_DescrizioneProdotto;
                            goodNewShipmentTms.depth = Helper.GetDecimalFromString(lorealSegnacolli.S_BoxBREIT, 3);
                            goodNewShipmentTms.height = Helper.GetDecimalFromString(lorealSegnacolli.S_BoxHOEHE, 3);
                            goodNewShipmentTms.width = Helper.GetDecimalFromString(lorealSegnacolli.S_BoxLAENG, 3);
                            goodNewShipmentTms.grossWeight = Helper.GetDecimalFromString(lorealSegnacolli.S_Peso, 3);
                            goodNewShipmentTms.packs = 1;
                            ParcelNewShipmentTMS parcelNewShipmentTms = new ParcelNewShipmentTMS()
                            {
                                barcodeExt = "00" + lorealSegnacolli.S_NumeroCollo
                            };
                            parcelNewShipmentTmsList.Add(parcelNewShipmentTms);
                            goodNewShipmentTmsList.Add(goodNewShipmentTms);
                        }
                        Decimal decimalFromString = Helper.GetDecimalFromString(ShipLoreal93.PesoDelivery, 2);
                        int ColliDelivery = int.Parse(ShipLoreal93.NumeroColliDettaglio) + int.Parse(ShipLoreal93.NumeroColliStandard);
                        int PltDelivery = int.Parse(ShipLoreal93.NumeroPedane);
                        StopNewShipmentTMS stopNewShipmentTms1 = new StopNewShipmentTMS();
                        stopNewShipmentTms1.address = "VIA PRIMATICCIO 155";
                        stopNewShipmentTms1.country = "IT";
                        stopNewShipmentTms1.description = "L'OREAL ITALIA SPA";
                        stopNewShipmentTms1.district = "MI";
                        stopNewShipmentTms1.zipCode = "20147";
                        stopNewShipmentTms1.location = "MILANO";
                        dateTime = DateTime.Now;
                        stopNewShipmentTms1.date = dateTime.ToString("yyyy-MM-dd");
                        stopNewShipmentTms1.type = "P";
                        stopNewShipmentTms1.region = "Lombardia";
                        stopNewShipmentTms1.time = "";
                        StopNewShipmentTMS stopNewShipmentTms2 = stopNewShipmentTms1;
                        stopNewShipmentTmsList.Add(stopNewShipmentTms2);
                        GeoClass geoClass = this.GeoTab.FirstOrDefault<GeoClass>((Func<GeoClass, bool>)(x => x.cap == ShipLoreal93.CAP));
                        StopNewShipmentTMS stopNewShipmentTms3 = new StopNewShipmentTMS()
                        {
                            address = ShipLoreal93.Indirizzo.Replace("\"", ""),
                            country = "IT",
                            description = ShipLoreal93.Destinatario.Trim().Replace("\"", ""),
                            district = ShipLoreal93.SiglaProvDestinazione,
                            zipCode = ShipLoreal93.CAP,
                            location = ShipLoreal93.Localita,
                            date = !string.IsNullOrEmpty(str5) ? str5 : "",
                            type = "D",
                            region = geoClass != null ? geoClass.regione : "",
                            time = "",
                            obligatoryType = !string.IsNullOrEmpty(str5) ? "Date" : "Nothing"
                        };
                        stopNewShipmentTmsList.Add(stopNewShipmentTms3);
                        goodNewShipmentTmsList[0].cube = Helper.GetDecimalFromString(ShipLoreal93.DeliveryVolume, 3);
                        ls.header = headerNewShipmentTms;
                        ls.parcels = parcelNewShipmentTmsList.ToArray();
                        ls.goods = goodNewShipmentTmsList.ToArray();
                        ls.stops = stopNewShipmentTmsList.ToArray();
                        ls.isTassativa = new bool?(flag2);
                        righeCSV.AddRange(this.ConvertiSpedizioneAPIinEDILoreal(ls, cust, decimalFromString, ColliDelivery, PltDelivery));
                    }
                    this.CreaInviaCSVAlServiceManagerByFTP(cust, righeCSV, fr);
                    return;
                }
                if (cust == CustomerConnections.PoolPharma || cust == CustomerConnections.DLF)
                {
                    bool flag = Path.GetFileNameWithoutExtension(fr).Split('_')[1] == "DLF";
                    string[] strArray = System.IO.File.ReadAllLines(fr);
                    PoolPharmaDLF_ShipmentIN pharmaDlfShipmentIn = new PoolPharmaDLF_ShipmentIN();
                    foreach (string str in strArray)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                ++num1;
                                Debug.WriteLine((object)num1);
                                RootobjectNewShipmentTMS rootobjectNewShipmentTms = new RootobjectNewShipmentTMS();
                                List<ParcelNewShipmentTMS> parcelNewShipmentTmsList = new List<ParcelNewShipmentTMS>();
                                List<StopNewShipmentTMS> stopNewShipmentTmsList = new List<StopNewShipmentTMS>();
                                List<GoodNewShipmentTMS> goodNewShipmentTmsList = new List<GoodNewShipmentTMS>();
                                pharmaDlfShipmentIn.CAPDestinatario = str.Substring(pharmaDlfShipmentIn.idxCAPDestinatario[0], pharmaDlfShipmentIn.idxCAPDestinatario[1]).Trim();
                                pharmaDlfShipmentIn.CAPDestinazione = str.Substring(pharmaDlfShipmentIn.idxCAPDestinazione[0], pharmaDlfShipmentIn.idxCAPDestinazione[1]).Trim();
                                pharmaDlfShipmentIn.CittaDestinatario = str.Substring(pharmaDlfShipmentIn.idxCittaDestinatario[0], pharmaDlfShipmentIn.idxCittaDestinatario[1]).Trim();
                                pharmaDlfShipmentIn.CittaDestinazione = str.Substring(pharmaDlfShipmentIn.idxCittaDestinazione[0], pharmaDlfShipmentIn.idxCittaDestinazione[1]).Trim();
                                pharmaDlfShipmentIn.DataBolla = str.Substring(pharmaDlfShipmentIn.idxDataBolla[0], pharmaDlfShipmentIn.idxDataBolla[1]).Trim();
                                pharmaDlfShipmentIn.ImportoContrassegno = str.Substring(pharmaDlfShipmentIn.idxImportoContrassegno[0], pharmaDlfShipmentIn.idxImportoContrassegno[1]).Trim();
                                pharmaDlfShipmentIn.IndirizzoDestinatario = str.Substring(pharmaDlfShipmentIn.idxIndirizzoDestinatario[0], pharmaDlfShipmentIn.idxIndirizzoDestinatario[1]).Trim();
                                pharmaDlfShipmentIn.IndirizzoDestinazione = str.Substring(pharmaDlfShipmentIn.idxIndirizzoDestinazione[0], pharmaDlfShipmentIn.idxIndirizzoDestinazione[1]).Trim();
                                pharmaDlfShipmentIn.MerceFragile = str.Substring(pharmaDlfShipmentIn.idxMerceFragile[0], pharmaDlfShipmentIn.idxMerceFragile[1]).Trim();
                                pharmaDlfShipmentIn.NazioneDestinatario = str.Substring(pharmaDlfShipmentIn.idxNazioneDestinatario[0], pharmaDlfShipmentIn.idxNazioneDestinatario[1]).Trim();
                                pharmaDlfShipmentIn.Note = str.Substring(pharmaDlfShipmentIn.idxNote[0], pharmaDlfShipmentIn.idxNote[1]).Trim();
                                pharmaDlfShipmentIn.Note1 = str.Substring(pharmaDlfShipmentIn.idxNote1[0], pharmaDlfShipmentIn.idxNote1[1]).Trim();
                                pharmaDlfShipmentIn.Note2 = str.Substring(pharmaDlfShipmentIn.idxNote2[0], pharmaDlfShipmentIn.idxNote2[1]).Trim();
                                pharmaDlfShipmentIn.NumeroColli = str.Substring(pharmaDlfShipmentIn.idxNumeroColli[0], pharmaDlfShipmentIn.idxNumeroColli[1]).Trim();
                                pharmaDlfShipmentIn.NumeroDocumento = str.Substring(pharmaDlfShipmentIn.idxNumeroDocumento[0], pharmaDlfShipmentIn.idxNumeroDocumento[1]).Trim();
                                pharmaDlfShipmentIn.Peso = str.Substring(pharmaDlfShipmentIn.idxPeso[0], pharmaDlfShipmentIn.idxPeso[1]).Trim();
                                pharmaDlfShipmentIn.ProvDestinatario = str.Substring(pharmaDlfShipmentIn.idxProvDestinatario[0], pharmaDlfShipmentIn.idxProvDestinatario[1]).Trim();
                                pharmaDlfShipmentIn.ProvDestinazione = str.Substring(pharmaDlfShipmentIn.idxProvDestinazione[0], pharmaDlfShipmentIn.idxProvDestinazione[1]).Trim();
                                pharmaDlfShipmentIn.RagioneSocialeDestinatario = str.Substring(pharmaDlfShipmentIn.idxRagioneSocialeDestinatario[0], pharmaDlfShipmentIn.idxRagioneSocialeDestinatario[1]).Trim();
                                pharmaDlfShipmentIn.RagioneSocialeDestinazione = str.Substring(pharmaDlfShipmentIn.idxRagioneSocialeDestinazione[0], pharmaDlfShipmentIn.idxRagioneSocialeDestinazione[1]).Trim();
                                pharmaDlfShipmentIn.RiferimentoEsterno = str.Substring(pharmaDlfShipmentIn.idxRiferimentoEsterno[0], pharmaDlfShipmentIn.idxRiferimentoEsterno[1]).Trim();
                                pharmaDlfShipmentIn.TemperaturaControllata = str.Substring(pharmaDlfShipmentIn.idxTemperaturaControllata[0], pharmaDlfShipmentIn.idxTemperaturaControllata[1]).Trim();
                                pharmaDlfShipmentIn.TemperaturaMinoreDi25 = str.Substring(pharmaDlfShipmentIn.idxTemperaturaMinoreDi25[0], pharmaDlfShipmentIn.idxTemperaturaMinoreDi25[1]).Trim();
                                HeaderNewShipmentTMS headerNewShipmentTms = new HeaderNewShipmentTMS()
                                {
                                    docDate = DateTime.ParseExact(pharmaDlfShipmentIn.DataBolla, "yyyyMMdd", (IFormatProvider)null).ToString("MM-dd-yyyy"),
                                    publicNote = (pharmaDlfShipmentIn.Note.Trim() + " " + pharmaDlfShipmentIn.Note1.Trim() + " " + pharmaDlfShipmentIn.Note2.Trim()).Trim(),
                                    customerID = cust.ID_GESPE,
                                    cashCurrency = "EUR",
                                    cashValue = Helper.GetDecimalFromString(pharmaDlfShipmentIn.ImportoContrassegno, 2),
                                    externRef = pharmaDlfShipmentIn.NumeroDocumento,
                                    carrierType = "COLLO",
                                    serviceType = "S",
                                    incoterm = "PF",
                                    transportType = pharmaDlfShipmentIn.TemperaturaControllata == "Y" ? "2-8" : "8-25",
                                    type = "Groupage",
                                    cashNote = "",
                                    insideRef = pharmaDlfShipmentIn.RiferimentoEsterno,
                                    internalNote = "",
                                    cashPayment = ""
                                };
                                StopNewShipmentTMS stopNewShipmentTms4 = new StopNewShipmentTMS();
                                if (flag)
                                {
                                    stopNewShipmentTms4.address = "VIA BASILICATA 9 FRAZ SESTO ULTERIANO";
                                    stopNewShipmentTms4.country = "IT";
                                    stopNewShipmentTms4.description = "D.L.F. SPA";
                                    stopNewShipmentTms4.district = "MI";
                                    stopNewShipmentTms4.zipCode = "20098";
                                    stopNewShipmentTms4.location = "San Giuliano Milanese";
                                    stopNewShipmentTms4.date = DateTime.Now.ToString("yyyy-MM-dd");
                                    stopNewShipmentTms4.type = "P";
                                    stopNewShipmentTms4.time = "";
                                }
                                else
                                {
                                    stopNewShipmentTms4.address = "VIA BASILICATA 9";
                                    stopNewShipmentTms4.country = "IT";
                                    stopNewShipmentTms4.description = "POOL PHARMA SRL";
                                    stopNewShipmentTms4.district = "MI";
                                    stopNewShipmentTms4.zipCode = "20098";
                                    stopNewShipmentTms4.location = "San Giuliano Milanese";
                                    stopNewShipmentTms4.date = DateTime.Now.ToString("yyyy-MM-dd");
                                    stopNewShipmentTms4.type = "P";
                                    stopNewShipmentTms4.time = "";
                                }
                                stopNewShipmentTmsList.Add(stopNewShipmentTms4);
                                StopNewShipmentTMS stopNewShipmentTms5 = new StopNewShipmentTMS()
                                {
                                    address = pharmaDlfShipmentIn.IndirizzoDestinazione,
                                    country = pharmaDlfShipmentIn.NazioneDestinazione,
                                    description = pharmaDlfShipmentIn.RagioneSocialeDestinazione.Trim(),
                                    district = pharmaDlfShipmentIn.ProvDestinazione,
                                    zipCode = pharmaDlfShipmentIn.CAPDestinazione,
                                    location = pharmaDlfShipmentIn.CittaDestinazione,
                                    type = "D",
                                    time = ""
                                };
                                stopNewShipmentTmsList.Add(stopNewShipmentTms5);
                                GoodNewShipmentTMS goodNewShipmentTms = new GoodNewShipmentTMS()
                                {
                                    grossWeight = Helper.GetDecimalFromString(pharmaDlfShipmentIn.Peso, 0),
                                    packs = int.Parse(pharmaDlfShipmentIn.NumeroColli)
                                };
                                goodNewShipmentTmsList.Add(goodNewShipmentTms);
                                rootobjectNewShipmentTms.header = headerNewShipmentTms;
                                rootobjectNewShipmentTms.goods = goodNewShipmentTmsList.ToArray();
                                rootobjectNewShipmentTms.stops = stopNewShipmentTmsList.ToArray();
                                source1.Add(rootobjectNewShipmentTms);
                            }
                        }
                        catch (Exception ex)
                        {
                            Automazione._loggerCode.Error<Exception>(ex);
                        }
                    }
                }
                else if (cust == CustomerConnections.STMGroup)
                {
                    if (fr.ToLower().EndsWith("_c.txt"))
                        return;
                    string[] strArray = System.IO.File.ReadAllLines(fr);
                    string[] files = Directory.GetFiles(Path.GetDirectoryName(fr));
                    string fn = Path.GetFileName(fr).Split('.')[0];
                    string[] source5 = System.IO.File.ReadAllLines(((IEnumerable<string>)files).FirstOrDefault<string>((Func<string, bool>)(x => x.Contains(fn) && x.ToLower().EndsWith("_c.txt"))));
                    STMGroup_ShipmentIN ShipSTM = new STMGroup_ShipmentIN();
                    foreach (string str8 in strArray)
                    {
                        try
                        {
                            ++num1;
                            Debug.WriteLine((object)num1);
                            RootobjectNewShipmentTMS rootobjectNewShipmentTms = new RootobjectNewShipmentTMS();
                            List<ParcelNewShipmentTMS> parcelNewShipmentTmsList = new List<ParcelNewShipmentTMS>();
                            List<StopNewShipmentTMS> stopNewShipmentTmsList = new List<StopNewShipmentTMS>();
                            List<GoodNewShipmentTMS> goodNewShipmentTmsList = new List<GoodNewShipmentTMS>();
                            if (!string.IsNullOrEmpty(str8) && str8.Length >= 573)
                            {
                                Debug.WriteLine("\r\n---------------------------------------------------------------\r\n" + str8 + "\r\n---------------------------------------------------------------------\r\n");
                                ShipSTM.AnnoDiRitiro = str8.Substring(ShipSTM.idxDKABO[0], ShipSTM.idxDKABO[1]).Trim();
                                ShipSTM.AnnoBollettazione = str8.Substring(ShipSTM.idxDKANB[0], ShipSTM.idxDKANB[1]).Trim();
                                ShipSTM.NumeroBancali = str8.Substring(ShipSTM.idxDKBAN[0], ShipSTM.idxDKBAN[1]).Trim();
                                ShipSTM.CodiceCorriere = str8.Substring(ShipSTM.idxDKBOR[0], ShipSTM.idxDKBOR[1]).Trim();
                                ShipSTM.CodiceVettoreRitiro = str8.Substring(ShipSTM.idxDKCDR[0], ShipSTM.idxDKCDR[1]).Trim();
                                ShipSTM.GiornoChiusura = str8.Substring(ShipSTM.idxDKCHI[0], ShipSTM.idxDKCHI[1]).Trim();
                                ShipSTM.CampoLibero = str8.Substring(ShipSTM.idxDKCNM[0], ShipSTM.idxDKCNM[1]).Trim();
                                ShipSTM.FasciaSegnacolli = str8.Substring(ShipSTM.idxDKCOD[0], ShipSTM.idxDKCOD[1]).Trim();
                                ShipSTM.Colli = str8.Substring(ShipSTM.idxDKCOL[0], ShipSTM.idxDKCOL[1]).Trim();
                                ShipSTM.DKDAE = str8.Substring(ShipSTM.idxDKDAE[0], ShipSTM.idxDKDAE[1]).Trim();
                                ShipSTM.Raggruppamento = str8.Substring(ShipSTM.idxDKDBO[0], ShipSTM.idxDKDBO[1]).Trim();
                                ShipSTM.CapDestinatario = str8.Substring(ShipSTM.idxDKDCP[0], ShipSTM.idxDKDCP[1]).Trim();
                                ShipSTM.DataConsegnaTassativa = str8.Substring(ShipSTM.idxDKDCV[0], ShipSTM.idxDKDCV[1]).Trim();
                                ShipSTM.DivisaContrassegno = str8.Substring(ShipSTM.idxDKDI1[0], ShipSTM.idxDKDI1[1]).Trim();
                                ShipSTM.DivisaAnticipata = str8.Substring(ShipSTM.idxDKDI2[0], ShipSTM.idxDKDI2[1]).Trim();
                                ShipSTM.DivisaValoreMerce = str8.Substring(ShipSTM.idxDKDI3[0], ShipSTM.idxDKDI3[1]).Trim();
                                ShipSTM.DKDIF = str8.Substring(ShipSTM.idxDKDIF[0], ShipSTM.idxDKDIF[1]).Trim();
                                ShipSTM.IndirizzoDestinatario = str8.Substring(ShipSTM.idxDKDIN[0], ShipSTM.idxDKDIN[1]).Trim();
                                ShipSTM.RagioneSocialeDestinatario = str8.Substring(ShipSTM.idxDKDIT[0], ShipSTM.idxDKDIT[1]).Trim();
                                ShipSTM.LocalitaDestinatario = str8.Substring(ShipSTM.idxDKDLO[0], ShipSTM.idxDKDLO[1]).Trim();
                                ShipSTM.CodiceMezzo = str8.Substring(ShipSTM.idxDKDNM[0], ShipSTM.idxDKDNM[1]).Trim();
                                ShipSTM.SiglaPartDest = str8.Substring(ShipSTM.idxDKDPR[0], ShipSTM.idxDKDPR[1]).Trim();
                                ShipSTM.OraConsegna = str8.Substring(ShipSTM.idxDKDST[0], ShipSTM.idxDKDST[1]).Trim();
                                ShipSTM.DataBollaAAAAAMMGG = str8.Substring(ShipSTM.idxDKDTB[0], ShipSTM.idxDKDTB[1]).Trim();
                                ShipSTM.DKDTF = str8.Substring(ShipSTM.idxDKDTF[0], ShipSTM.idxDKDTF[1]).Trim();
                                ShipSTM.DataRitiro_AAAAMMGG = str8.Substring(ShipSTM.idxDKDTR[0], ShipSTM.idxDKDTR[1]).Trim();
                                ShipSTM.DataXAB_AAAAMMGG = str8.Substring(ShipSTM.idxDKDTX[0], ShipSTM.idxDKDTX[1]).Trim();
                                ShipSTM.DKEBO = str8.Substring(ShipSTM.idxDKEBO[0], ShipSTM.idxDKEBO[1]).Trim();
                                ShipSTM.FilialeBollettazione = str8.Substring(ShipSTM.idxDKFBO[0], ShipSTM.idxDKFBO[1]).Trim();
                                ShipSTM.DKFIL = str8.Substring(ShipSTM.idxDKFIL[0], ShipSTM.idxDKFIL[1]).Trim();
                                ShipSTM.DKFPA = str8.Substring(ShipSTM.idxDKFPA[0], ShipSTM.idxDKFPA[1]).Trim();
                                ShipSTM.NumeroAnticipata = str8.Substring(ShipSTM.idxDKFTA[0], ShipSTM.idxDKFTA[1]).Trim();
                                ShipSTM.ImportoContrassegno3Dec = str8.Substring(ShipSTM.idxDKIF1[0], ShipSTM.idxDKIF1[1]).Trim();
                                ShipSTM.ImportoAnticipata3Dec = str8.Substring(ShipSTM.idxDKIF2[0], ShipSTM.idxDKIF2[1]).Trim();
                                ShipSTM.ImportoValoreMerce3Dec = str8.Substring(ShipSTM.idxDKIF3[0], ShipSTM.idxDKIF3[1]).Trim();
                                ShipSTM.DKITF = str8.Substring(ShipSTM.idxDKITF[0], ShipSTM.idxDKITF[1]).Trim();
                                ShipSTM.DKKBO = str8.Substring(ShipSTM.idxDKKBO[0], ShipSTM.idxDKKBO[1]).Trim();
                                ShipSTM.ProgressivoChiave = str8.Substring(ShipSTM.idxDKKEY[0], ShipSTM.idxDKKEY[1]).Trim();
                                ShipSTM.MagazzinoDiPartenza = str8.Substring(ShipSTM.idxDKMAE[0], ShipSTM.idxDKMAE[1]).Trim();
                                ShipSTM.ContrattoMittente = str8.Substring(ShipSTM.idxDKMCN[0], ShipSTM.idxDKMCN[1]).Trim();
                                ShipSTM.CodiceCliente = str8.Substring(ShipSTM.idxDKMCO[0], ShipSTM.idxDKMCO[1]).Trim();
                                ShipSTM.CapMittente = str8.Substring(ShipSTM.idxDKMCP[0], ShipSTM.idxDKMCP[1]).Trim();
                                ShipSTM.Filiale = str8.Substring(ShipSTM.idxDKMFI[0], ShipSTM.idxDKMFI[1]).Trim();
                                ShipSTM.IndirizzoMittente = str8.Substring(ShipSTM.idxDKMIN[0], ShipSTM.idxDKMIN[1]).Trim();
                                ShipSTM.RagioneSocialeMittente = str8.Substring(ShipSTM.idxDKMIT[0], ShipSTM.idxDKMIT[1]).Trim();
                                ShipSTM.LocalitaMittente = str8.Substring(ShipSTM.idxDKMLO[0], ShipSTM.idxDKMLO[1]).Trim();
                                ShipSTM.DKMPR = str8.Substring(ShipSTM.idxDKMPR[0], ShipSTM.idxDKMPR[1]).Trim();
                                ShipSTM.OraViaggio = str8.Substring(ShipSTM.idxDKMST[0], ShipSTM.idxDKMST[1]).Trim();
                                ShipSTM.NoteConsegna = str8.Substring(ShipSTM.idxDKNOT[0], ShipSTM.idxDKNOT[1]).Trim();
                                ShipSTM.DKNRF = str8.Substring(ShipSTM.idxDKNRF[0], ShipSTM.idxDKNRF[1]).Trim();
                                ShipSTM.Peso2Dec = str8.Substring(ShipSTM.idxDKPES[0], ShipSTM.idxDKPES[1]).Trim();
                                ShipSTM.DKREC = str8.Substring(ShipSTM.idxDKREC[0], ShipSTM.idxDKREC[1]).Trim();
                                ShipSTM.DKRM2 = str8.Substring(ShipSTM.idxDKRM2[0], ShipSTM.idxDKRM2[1]).Trim();
                                ShipSTM.RifInterno = str8.Substring(ShipSTM.idxDKRMI[0], ShipSTM.idxDKRMI[1]).Trim();
                                ShipSTM.RiferimentoOperatoreLogistico = str8.Substring(ShipSTM.idxDKRUL[0], ShipSTM.idxDKRUL[1]).Trim();
                                ShipSTM.SegnacolloAl = str8.Substring(ShipSTM.idxDKSEA[0], ShipSTM.idxDKSEA[1]).Trim();
                                ShipSTM.SegnacolloDal = str8.Substring(ShipSTM.idxDKSED[0], ShipSTM.idxDKSED[1]).Trim();
                                ShipSTM.DKSOC = str8.Substring(ShipSTM.idxDKSOC[0], ShipSTM.idxDKSOC[1]).Trim();
                                ShipSTM.DKT01 = str8.Substring(ShipSTM.idxDKT01[0], ShipSTM.idxDKT01[1]).Trim();
                                ShipSTM.TipoServizio = str8.Substring(ShipSTM.idxDKT02[0], ShipSTM.idxDKT02[1]).Trim();
                                ShipSTM.TipoConsegna = str8.Substring(ShipSTM.idxDKT03[0], ShipSTM.idxDKT03[1]).Trim();
                                ShipSTM.DKT04 = str8.Substring(ShipSTM.idxDKT04[0], ShipSTM.idxDKT04[1]).Trim();
                                ShipSTM.DKT05 = str8.Substring(ShipSTM.idxDKT05[0], ShipSTM.idxDKT05[1]).Trim();
                                ShipSTM.DKT06 = str8.Substring(ShipSTM.idxDKT06[0], ShipSTM.idxDKT06[1]).Trim();
                                ShipSTM.DKT07 = str8.Substring(ShipSTM.idxDKT07[0], ShipSTM.idxDKT07[1]).Trim();
                                ShipSTM.DKT08 = str8.Substring(ShipSTM.idxDKT08[0], ShipSTM.idxDKT08[1]).Trim();
                                ShipSTM.DKTCH = str8.Substring(ShipSTM.idxDKTCH[0], ShipSTM.idxDKTCH[1]).Trim();
                                ShipSTM.TipoBolla = str8.Substring(ShipSTM.idxDKTIP[0], ShipSTM.idxDKTIP[1]).Trim();
                                ShipSTM.DKTPA = str8.Substring(ShipSTM.idxDKTPA[0], ShipSTM.idxDKTPA[1]).Trim();
                                ShipSTM.CodiceVettoreAnticipata = str8.Substring(ShipSTM.idxDKVE1[0], ShipSTM.idxDKVE1[1]).Trim();
                                ShipSTM.MetriCubi2Dec = str8.Substring(ShipSTM.idxDKVOL[0], ShipSTM.idxDKVOL[1]).Trim();
                                ShipSTM.DKZON = str8.Substring(ShipSTM.idxDKZON[0], ShipSTM.idxDKZON[1]).Trim();
                                string str9 = "";
                                DateTime result = DateTime.MinValue;
                                bool flag = false;
                                if (!string.IsNullOrEmpty(ShipSTM.TipoConsegna))
                                {
                                    if (ShipSTM.TipoConsegna == "1")
                                    {
                                        DateTime.TryParseExact(ShipSTM.DataConsegnaTassativa, "yyyyMMdd", (IFormatProvider)null, DateTimeStyles.None, out result);
                                        flag = true;
                                    }
                                    else if (ShipSTM.TipoConsegna == "2")
                                        DateTime.TryParseExact(ShipSTM.DataConsegnaTassativa, "yyyyMMdd", (IFormatProvider)null, DateTimeStyles.None, out result);
                                    else if (ShipSTM.TipoConsegna == "3")
                                        DateTime.TryParseExact(ShipSTM.DataConsegnaTassativa, "yyyyMMdd", (IFormatProvider)null, DateTimeStyles.None, out result);
                                    else if (ShipSTM.TipoConsegna == "4")
                                        DateTime.TryParseExact(ShipSTM.DataConsegnaTassativa, "yyyyMMdd", (IFormatProvider)null, DateTimeStyles.None, out result);
                                    else if (ShipSTM.TipoConsegna == "5")
                                    {
                                        DateTime.TryParseExact(ShipSTM.DataConsegnaTassativa, "yyyyMMdd", (IFormatProvider)null, DateTimeStyles.None, out result);
                                        flag = true;
                                    }
                                }
                                HeaderNewShipmentTMS headerNewShipmentTms = new HeaderNewShipmentTMS()
                                {
                                    docDate = DateTime.ParseExact(ShipSTM.DataBollaAAAAAMMGG, "yyyyMMdd", (IFormatProvider)null).ToString("MM-dd-yyyy"),
                                    publicNote = (ShipSTM.NoteConsegna.Trim() ?? "").Trim(),
                                    customerID = cust.ID_GESPE,
                                    cashCurrency = "EUR",
                                    cashValue = Helper.GetDecimalFromString(ShipSTM.ImportoContrassegno3Dec, 3),
                                    externRef = ShipSTM.RiferimentoOperatoreLogistico ?? "",
                                    carrierType = "EDI",
                                    serviceType = "S",
                                    incoterm = "PF",
                                    transportType = "8-25",
                                    type = "Groupage",
                                    cashNote = "",
                                    insideRef = ShipSTM.RifInterno,
                                    internalNote = "",
                                    cashPayment = ""
                                };
                                StopNewShipmentTMS stopNewShipmentTms6 = new StopNewShipmentTMS();
                                if (ShipSTM.MagazzinoDiPartenza == "CM")
                                {
                                    stopNewShipmentTms6.address = "VIA RIO DEL VALLONE 20";
                                    stopNewShipmentTms6.country = "IT";
                                    stopNewShipmentTms6.description = "STM PHARMA PRO SRL - " + ShipSTM.RagioneSocialeMittente;
                                    stopNewShipmentTms6.district = "MI";
                                    stopNewShipmentTms6.zipCode = "20040";
                                    stopNewShipmentTms6.location = "CAMBIAGO";
                                }
                                else if (ShipSTM.MagazzinoDiPartenza == "GR")
                                {
                                    stopNewShipmentTms6.address = "VIA ABRUZZI SNC";
                                    stopNewShipmentTms6.country = "IT";
                                    stopNewShipmentTms6.description = "STM PHARMA PRO SRL - " + ShipSTM.RagioneSocialeMittente;
                                    stopNewShipmentTms6.district = "MI";
                                    stopNewShipmentTms6.zipCode = "20056";
                                    stopNewShipmentTms6.location = "GREZZAGO";
                                }
                                else if (ShipSTM.MagazzinoDiPartenza == "G1")
                                {
                                    stopNewShipmentTms6.address = "VIA UMBRIA 15";
                                    stopNewShipmentTms6.country = "IT";
                                    stopNewShipmentTms6.description = "STM PHARMA PRO SRL - " + ShipSTM.RagioneSocialeMittente;
                                    stopNewShipmentTms6.district = "MI";
                                    stopNewShipmentTms6.zipCode = "20056";
                                    stopNewShipmentTms6.location = "GREZZAGO";
                                }
                                else
                                {
                                    stopNewShipmentTms6.address = "VIA XXV APRILE 56";
                                    stopNewShipmentTms6.country = "IT";
                                    stopNewShipmentTms6.description = "STM PHARMA PRO SRL - " + ShipSTM.RagioneSocialeMittente;
                                    stopNewShipmentTms6.district = "MI";
                                    stopNewShipmentTms6.zipCode = "20040";
                                    stopNewShipmentTms6.location = "CAMBIAGO";
                                }
                                stopNewShipmentTms6.date = DateTime.Now.ToString("yyyy-MM-dd");
                                stopNewShipmentTms6.type = "D";
                                stopNewShipmentTms6.time = "";
                                stopNewShipmentTmsList.Add(stopNewShipmentTms6);
                                StopNewShipmentTMS stopNewShipmentTms7 = new StopNewShipmentTMS()
                                {
                                    address = ShipSTM.IndirizzoDestinatario,
                                    country = "IT",
                                    description = ShipSTM.RagioneSocialeDestinatario.Trim(),
                                    district = ShipSTM.SiglaPartDest,
                                    zipCode = ShipSTM.CapDestinatario,
                                    location = ShipSTM.LocalitaDestinatario,
                                    date = result != DateTime.MinValue ? result.ToString("MM-dd-yyyy") : str9,
                                    type = "P",
                                    time = ""
                                };
                                stopNewShipmentTmsList.Add(stopNewShipmentTms7);
                                GoodNewShipmentTMS goodNewShipmentTms = new GoodNewShipmentTMS()
                                {
                                    grossWeight = Helper.GetDecimalFromString(ShipSTM.Peso2Dec, 2),
                                    cube = Helper.GetDecimalFromString(ShipSTM.MetriCubi2Dec, 2),
                                    packs = int.Parse(ShipSTM.Colli),
                                    totalPallet = int.Parse(ShipSTM.NumeroBancali),
                                    floorPallet = int.Parse(ShipSTM.NumeroBancali)
                                };
                                goodNewShipmentTmsList.Add(goodNewShipmentTms);
                                foreach (string str10 in ((IEnumerable<string>)source5).Where<string>((Func<string, bool>)(x => x.Contains(ShipSTM.RifInterno))).ToList<string>())
                                {
                                    string str11 = str10.Substring(116, 14);
                                    ParcelNewShipmentTMS parcelNewShipmentTms = new ParcelNewShipmentTMS()
                                    {
                                        barcodeExt = str11
                                    };
                                    parcelNewShipmentTmsList.Add(parcelNewShipmentTms);
                                }
                                if (goodNewShipmentTms.packs > parcelNewShipmentTmsList.Count && goodNewShipmentTms.floorPallet == 0)
                                {
                                    goodNewShipmentTms.totalPallet = parcelNewShipmentTmsList.Count;
                                    goodNewShipmentTms.floorPallet = parcelNewShipmentTmsList.Count;
                                }
                                rootobjectNewShipmentTms.header = headerNewShipmentTms;
                                rootobjectNewShipmentTms.parcels = parcelNewShipmentTmsList.ToArray();
                                rootobjectNewShipmentTms.goods = goodNewShipmentTmsList.ToArray();
                                rootobjectNewShipmentTms.stops = stopNewShipmentTmsList.ToArray();
                                rootobjectNewShipmentTms.isTassativa = new bool?(flag);
                                source1.Add(rootobjectNewShipmentTms);
                            }
                        }
                        catch (Exception ex)
                        {
                            Automazione._loggerCode.Error<Exception>(ex);
                        }
                    }
                }
                else if (cust == CustomerConnections.DIFARCO || cust == CustomerConnections.PHARDIS || cust == CustomerConnections.StockHouse)
                {
                    string[] source6 = System.IO.File.ReadAllLines(fr);
                    if (((IEnumerable<string>)source6).Count<string>() > 0)
                    {
                        StockHouse_Shipment_IN CdGroup = new StockHouse_Shipment_IN();
                        for (int index1 = 0; index1 < ((IEnumerable<string>)source6).Count<string>(); ++index1)
                        {
                            try
                            {
                                Debug.WriteLine((object)index1);
                                RootobjectNewShipmentTMS rootobjectNewShipmentTms = new RootobjectNewShipmentTMS();
                                List<ParcelNewShipmentTMS> parcelNewShipmentTmsList = new List<ParcelNewShipmentTMS>();
                                List<StopNewShipmentTMS> stopNewShipmentTmsList = new List<StopNewShipmentTMS>();
                                List<GoodNewShipmentTMS> goodNewShipmentTmsList = new List<GoodNewShipmentTMS>();
                                string str12 = source6[index1];
                                CdGroup.MANDANTE = str12.Substring(CdGroup.idxMANDANTE[0], CdGroup.idxMANDANTE[1]).Trim();
                                CdGroup.NR_BOLLA = str12.Substring(CdGroup.idxNRBOLLA[0], CdGroup.idxNRBOLLA[1]).Trim();
                                CdGroup.DATA_BOLLA = str12.Substring(CdGroup.idxDATA_BOLLA[0], CdGroup.idxDATA_BOLLA[1]).Trim();
                                CdGroup.NR_SHIPMENT = str12.Substring(CdGroup.idxNR_SHIPMENT[0], CdGroup.idxNR_SHIPMENT[1]).Trim();
                                CdGroup.RAG_SOC_MITTENTE = str12.Substring(CdGroup.idxRAG_SOC_MITTENTE[0], CdGroup.idxRAG_SOC_MITTENTE[1]).Trim();
                                CdGroup.INDIRIZZO_MITTENTE = str12.Substring(CdGroup.idxINDIRIZZO_MITTENTE[0], CdGroup.idxINDIRIZZO_MITTENTE[1]).Trim();
                                CdGroup.CAP_MITTENTE = str12.Substring(CdGroup.idxCAP_MITTENTE[0], CdGroup.idxCAP_MITTENTE[1]).Trim();
                                CdGroup.LOC_MITTENTE = str12.Substring(CdGroup.idxLOC_MITTENTE[0], CdGroup.idxLOC_MITTENTE[1]).Trim();
                                CdGroup.PROV_MITTENTE = str12.Substring(CdGroup.idxPROV_MITTENTE[0], CdGroup.idxPROV_MITTENTE[1]).Trim();
                                CdGroup.NAZIONE_MITTENTE = str12.Substring(CdGroup.idxNAZIONE_MITTENTE[0], CdGroup.idxNAZIONE_MITTENTE[1]).Substring(0, 2);
                                CdGroup.RAG_SOC_DESTINATARIO = str12.Substring(CdGroup.idxRAG_SOC_DESTINATARIO[0], CdGroup.idxRAG_SOC_DESTINATARIO[1]).Trim().Replace("\"", "");
                                CdGroup.INDIRIZZO_DESTINATARIO = str12.Substring(CdGroup.idxINDIRIZZO_DESTINATARIO[0], CdGroup.idxINDIRIZZO_DESTINATARIO[1]).Trim().Replace("\"", "");
                                CdGroup.CAP_DESTINATARIO = str12.Substring(CdGroup.idxCAP_DESTINATARIO[0], CdGroup.idxCAP_DESTINATARIO[1]).Trim();
                                CdGroup.LOC_DESTINATARIO = str12.Substring(CdGroup.idxLOC_DESTINATARIO[0], CdGroup.idxLOC_DESTINATARIO[1]).Trim();
                                CdGroup.PROV_DESTINATARIO = str12.Substring(CdGroup.idxPROV_DESTINATARIO[0], CdGroup.idxPROV_DESTINATARIO[1]).Trim();
                                CdGroup.NAZIONE_DESTINATARIO = str12.Substring(CdGroup.idxNAZIONE_DESTINATARIO[0], CdGroup.idxNAZIONE_DESTINATARIO[1]).Trim();
                                CdGroup.PESO_SPEDIZIONE = str12.Substring(CdGroup.idxPESO_SPEDIZIONE[0], CdGroup.idxPESO_SPEDIZIONE[1]).Trim();
                                CdGroup.VOLUME_SPEDIZIONE = str12.Substring(CdGroup.idxVOLUME_SPEDIZIONE[0], CdGroup.idxVOLUME_SPEDIZIONE[1]).Trim();
                                CdGroup.N_CARTONI_CT = str12.Substring(CdGroup.idxN_CARTONI_CT[0], CdGroup.idxN_CARTONI_CT[1]).Trim();
                                CdGroup.N_BANCALI_BA = Helper.StringIntString(str12.Substring(CdGroup.idxN_BANCALI_BA[0], CdGroup.idxN_BANCALI_BA[1]).Trim());
                                CdGroup.N_BANCALI_COLLETTAME_BB = Helper.StringIntString(str12.Substring(CdGroup.idxN_BANCALI_COLLETTAME_BB[0], CdGroup.idxN_BANCALI_COLLETTAME_BB[1]).Trim());
                                CdGroup.N_BA_BB = Helper.StringIntString(str12.Substring(CdGroup.idxN_BA_BB[0], CdGroup.idxN_BA_BB[1]).Trim());
                                CdGroup.PESO_CARTONI_CT = str12.Substring(CdGroup.idxPESO_CARTONI_CT[0], CdGroup.idxPESO_CARTONI_CT[1]).Trim();
                                CdGroup.VALUTA_CONTRASS = str12.Substring(CdGroup.idxVALUTA_CONTRASS[0], CdGroup.idxVALUTA_CONTRASS[1]).Trim();
                                CdGroup.IMPORTO_CONTRASS = str12.Substring(CdGroup.idxIMPORTO_CONTRASS[0], CdGroup.idxIMPORTO_CONTRASS[1]).Trim();
                                CdGroup.NUMERO_COLLI_SPED = Helper.StringIntString(str12.Substring(CdGroup.idxNUMERO_COLLI_SPED[0], CdGroup.idxNUMERO_COLLI_SPED[1]).Trim());
                                CdGroup.DA_SEGNACOLLO = str12.Substring(CdGroup.idxDA_SEGNACOLLO[0], CdGroup.idxDA_SEGNACOLLO[1]).Trim();
                                CdGroup.A_SEGNACOLLO = str12.Substring(CdGroup.idxA_SEGNACOLLO[0], CdGroup.idxA_SEGNACOLLO[1]).Trim();
                                CdGroup.NOTE = str12.Substring(CdGroup.idxNOTE[0], CdGroup.idxNOTE[1]).Trim();
                                CdGroup.VETTORE = str12.Substring(CdGroup.idxVETTORE[0], CdGroup.idxVETTORE[1]).Trim();
                                CdGroup.NR_DISTINTA = str12.Substring(CdGroup.idxNR_DISTINTA[0], CdGroup.idxNR_DISTINTA[1]).Trim();
                                CdGroup.DT_DISTINTA = str12.Substring(CdGroup.idxDT_DISTINTA[0], CdGroup.idxDT_DISTINTA[1]).Trim();
                                CdGroup.COND_PAG = str12.Substring(CdGroup.idxCOND_PAG[0], CdGroup.idxCOND_PAG[1]).Trim();
                                CdGroup.CONS_PIANI = str12.Substring(CdGroup.idxCONS_PIANI[0], CdGroup.idxCONS_PIANI[1]).Trim();
                                CdGroup.TEL_PRIMA_CONS = str12.Substring(CdGroup.idxTEL_PRIMA_CONS[0], CdGroup.idxTEL_PRIMA_CONS[1]).Trim();
                                CdGroup.DT_CONS_TASSAT_1 = str12.Substring(CdGroup.idxDT_CONS_TASSAT_1[0], CdGroup.idxDT_CONS_TASSAT_1[1]).Trim();
                                CdGroup.DT_CONS_TASSAT_2 = str12.Substring(CdGroup.idxDT_CONS_TASSAT_2[0], CdGroup.idxDT_CONS_TASSAT_2[1]).Trim();
                                CdGroup.NOTE_1 = str12.Substring(CdGroup.idxNOTE_1[0], CdGroup.idxNOTE_1[1]).Trim();
                                CdGroup.NOTE_2 = str12.Substring(CdGroup.idxNOTE_2[0], CdGroup.idxNOTE_2[1]).Trim();
                                CdGroup.NOTE_3 = str12.Substring(CdGroup.idxNOTE_3[0], CdGroup.idxNOTE_3[1]).Trim();
                                CdGroup.NOTE_4 = str12.Substring(CdGroup.idxNOTE_4[0], CdGroup.idxNOTE_4[1]).Trim();
                                CdGroup.NOTE_5 = str12.Substring(CdGroup.idxNOTE_5[0], CdGroup.idxNOTE_5[1]).Trim();
                                CdGroup.Libero = str12.Substring(CdGroup.idxLibero[0], CdGroup.idxLibero[1]).Trim();
                                CdGroup.N_PALLETTS = Helper.StringIntString(str12.Substring(CdGroup.idxN_PALLETTS[0], CdGroup.idxN_PALLETTS[1]).Trim());
                                CdGroup.N_CHEP = Helper.StringIntString(str12.Substring(CdGroup.idxN_CHEP[0], CdGroup.idxN_CHEP[1]).Trim());
                                CdGroup.N_EPAL = Helper.StringIntString(str12.Substring(CdGroup.idxN_EPAL[0], CdGroup.idxN_EPAL[1]).Trim());
                                CdGroup.AANN = str12.Substring(CdGroup.idxAANN[0], CdGroup.idxAANN[1]).Trim();
                                CdGroup.TTRAS = str12.Substring(CdGroup.idxTTRAS[0], CdGroup.idxTTRAS[1]).Trim();
                                CdGroup.M_A = str12.Substring(CdGroup.idxM_A[0], CdGroup.idxM_A[1]).Trim();
                                CdGroup.NR_PREBOLLA = str12.Substring(CdGroup.idxNR_PREBOLLA[0], CdGroup.idxNR_PREBOLLA[1]).Trim();
                                HeaderNewShipmentTMS headerNewShipmentTms = new HeaderNewShipmentTMS();
                                headerNewShipmentTms.docDate = DateTime.ParseExact(CdGroup.DATA_BOLLA, "yyyyMMdd", (IFormatProvider)null).ToString("MM-dd-yyyy");
                                headerNewShipmentTms.publicNote = (CdGroup.NOTE.Trim() + " " + CdGroup.NOTE_1.Trim() + " " + CdGroup.NOTE_2.Trim() + " " + CdGroup.NOTE_3.Trim() + " " + CdGroup.NOTE_4.Trim() + " " + CdGroup.NOTE_5.Trim()).Trim();
                                headerNewShipmentTms.customerID = cust.ID_GESPE;
                                headerNewShipmentTms.cashCurrency = "EUR";
                                headerNewShipmentTms.cashValue = Helper.GetDecimalFromString(CdGroup.IMPORTO_CONTRASS, 2);
                                headerNewShipmentTms.externRef = CdGroup.NR_BOLLA;
                                headerNewShipmentTms.carrierType = "EDI";
                                headerNewShipmentTms.serviceType = "S";
                                headerNewShipmentTms.incoterm = "PF";
                                headerNewShipmentTms.transportType = "8-25";
                                headerNewShipmentTms.type = "Groupage";
                                headerNewShipmentTms.cashNote = "";
                                headerNewShipmentTms.insideRef = CdGroup.MANDANTE;
                                headerNewShipmentTms.internalNote = CdGroup.NOTE + " " + CdGroup.NOTE_1 + " " + CdGroup.NOTE_2 + " " + CdGroup.NOTE_3 + " " + CdGroup.NOTE_4 + " " + CdGroup.NOTE_5;
                                headerNewShipmentTms.cashPayment = "";
                                DateTime result = DateTime.MinValue;
                                DateTime.TryParseExact(CdGroup.DT_CONS_TASSAT_1, "yyyyMMdd", (IFormatProvider)null, DateTimeStyles.None, out result);
                                if (result != DateTime.MinValue && result < DateTime.Now)
                                    result = DateTime.MinValue;
                                MagazzinoCDGroup magazzinoCdGroup = SediCaricoCDGroup.RecuperaLaSedeCDGroup(CdGroup.MANDANTE) ?? SediCaricoCDGroup.SedeLegale;
                                stopNewShipmentTmsList.Add(new StopNewShipmentTMS()
                                {
                                    address = magazzinoCdGroup.address,
                                    country = magazzinoCdGroup.country,
                                    description = CdGroup.RAG_SOC_MITTENTE.Trim(),
                                    district = magazzinoCdGroup.district,
                                    zipCode = magazzinoCdGroup.zipCode,
                                    location = magazzinoCdGroup.location,
                                    date = DateTime.Now.ToString("MM-dd-yyyy"),
                                    type = "P",
                                    time = ""
                                });
                                GeoClass geoClass = this.GeoTab.FirstOrDefault<GeoClass>((Func<GeoClass, bool>)(x => x.cap == CdGroup.CAP_DESTINATARIO));
                                stopNewShipmentTmsList.Add(new StopNewShipmentTMS()
                                {
                                    address = CdGroup.INDIRIZZO_DESTINATARIO,
                                    country = CdGroup.NAZIONE_DESTINATARIO,
                                    description = CdGroup.RAG_SOC_DESTINATARIO.Trim(),
                                    district = CdGroup.PROV_DESTINATARIO,
                                    zipCode = CdGroup.CAP_DESTINATARIO,
                                    location = CdGroup.LOC_DESTINATARIO,
                                    date = result != DateTime.MinValue ? result.ToString("yyyyMMdd") : "",
                                    type = "D",
                                    region = geoClass != null ? geoClass.regione : "",
                                    time = "",
                                    obligatoryType = string.IsNullOrEmpty(CdGroup.DT_CONS_TASSAT_1) || !(CdGroup.DT_CONS_TASSAT_1 != "00000000") ? "Nothing" : "Date"
                                });
                                GoodNewShipmentTMS goodNewShipmentTms = new GoodNewShipmentTMS();
                                goodNewShipmentTms.grossWeight = Helper.GetDecimalFromString(CdGroup.PESO_SPEDIZIONE, 3);
                                goodNewShipmentTms.cube = Helper.GetDecimalFromString(CdGroup.VOLUME_SPEDIZIONE, 3);
                                goodNewShipmentTms.packs = int.Parse(CdGroup.NUMERO_COLLI_SPED);
                                goodNewShipmentTms.totalPallet = int.Parse(CdGroup.N_BANCALI_BA);
                                goodNewShipmentTms.floorPallet = int.Parse(CdGroup.N_BANCALI_BA);
                                goodNewShipmentTmsList.Add(goodNewShipmentTms);
                                string str13 = CdGroup.NR_PREBOLLA.Substring(3);
                                int num3 = int.Parse(CdGroup.DA_SEGNACOLLO);
                                int num4 = int.Parse(CdGroup.A_SEGNACOLLO);
                                for (int index2 = num3; index2 <= num4; ++index2)
                                {
                                    if (cust == CustomerConnections.StockHouse)
                                    {
                                        string str14 = index2.ToString();
                                        while (str14.Length < 3)
                                            str14 = "0" + str14;
                                        ParcelNewShipmentTMS parcelNewShipmentTms = new ParcelNewShipmentTMS()
                                        {
                                            barcodeExt = str13 + str14
                                        };
                                        parcelNewShipmentTmsList.Add(parcelNewShipmentTms);
                                    }
                                    else
                                    {
                                        string str15 = index2.ToString();
                                        while (str15.Length < 9)
                                            str15 = "0" + str15;
                                        ParcelNewShipmentTMS parcelNewShipmentTms = new ParcelNewShipmentTMS()
                                        {
                                            barcodeExt = str15 ?? ""
                                        };
                                        parcelNewShipmentTmsList.Add(parcelNewShipmentTms);
                                    }
                                }
                                int count = parcelNewShipmentTmsList.Count;
                                if (goodNewShipmentTms.packs > count)
                                {
                                    if (goodNewShipmentTms.grossWeight <= 200M)
                                    {
                                        goodNewShipmentTms.totalPallet = 0;
                                        goodNewShipmentTms.floorPallet = 0;
                                        goodNewShipmentTms.packs = count;
                                        goodNewShipmentTms.cube = this.RecuperaIlVolumeInBaseAlPeso(goodNewShipmentTms.grossWeight);
                                        goodNewShipmentTms.description = "Volume inserito automaticamente in quanto non comunicato dal cliente";
                                    }
                                    else if (goodNewShipmentTms.floorPallet == 0)
                                    {
                                        goodNewShipmentTms.totalPallet = count;
                                        goodNewShipmentTms.floorPallet = count;
                                    }
                                }
                                rootobjectNewShipmentTms.header = headerNewShipmentTms;
                                rootobjectNewShipmentTms.parcels = parcelNewShipmentTmsList.ToArray();
                                rootobjectNewShipmentTms.goods = goodNewShipmentTmsList.ToArray();
                                rootobjectNewShipmentTms.stops = stopNewShipmentTmsList.ToArray();
                                source1.Add(rootobjectNewShipmentTms);
                            }
                            catch (Exception ex)
                            {
                                Automazione._loggerCode.Error<Exception>(ex);
                            }
                        }
                    }
                }
                else if (cust == CustomerConnections.CHIAPPAROLI)
                {
                    if (!Path.GetFileName(fr).StartsWith("UNIT"))
                    {
                        fileProcessati.Remove(fr);
                        return;
                    }
                    string sede = Path.GetFileName(fr).Substring(4, 2);
                    string[] files = Directory.GetFiles(Path.GetDirectoryName(fr));
                    string path1 = ((IEnumerable<string>)files).Where<string>((Func<string, bool>)(x => Path.GetFileName(x).StartsWith("UNID") && Path.GetFileName(x).Contains(sede))).FirstOrDefault<string>();
                    string path2 = ((IEnumerable<string>)files).Where<string>((Func<string, bool>)(x => Path.GetFileName(x).StartsWith("UNIA") && Path.GetFileName(x).Contains(sede))).FirstOrDefault<string>();
                    List<string> source7 = new List<string>();
                    List<string> source8 = new List<string>();
                    List<string> source9 = new List<string>();
                    if (!string.IsNullOrEmpty(fr))
                        source7 = ((IEnumerable<string>)System.IO.File.ReadAllLines(fr)).ToList<string>();
                    if (!string.IsNullOrEmpty(path1))
                    {
                        source8 = ((IEnumerable<string>)System.IO.File.ReadAllLines(path1)).ToList<string>();
                        fileProcessati.Add(path1);
                    }
                    if (!string.IsNullOrEmpty(path2))
                    {
                        source9 = ((IEnumerable<string>)System.IO.File.ReadAllLines(path2)).ToList<string>();
                        fileProcessati.Add(path2);
                    }
                    List<Chiapparoli_ShipmentIN> chiapparoliShipmentInList = new List<Chiapparoli_ShipmentIN>();
                    for (int index = 0; index < source7.Count<string>(); ++index)
                    {
                        string str16 = source7[index];
                        ++num1;
                        Debug.WriteLine((object)num1);
                        Chiapparoli_ShipmentIN nl = new Chiapparoli_ShipmentIN();
                        nl.CodiceSocieta = str16.Substring(nl.idxCodiceSocieta[0], nl.idxCodiceSocieta[1]).Trim();
                        nl.SedeChiapparoli = str16.Substring(nl.idxSedeChiapparoli[0], nl.idxSedeChiapparoli[1]).Trim();
                        nl.CodiceDitta = str16.Substring(nl.idxCodiceDitta[0], nl.idxCodiceDitta[1]).Trim();
                        nl.NumeroProgressivoCHC = str16.Substring(nl.idxNumeroProgressivoCHC[0], nl.idxNumeroProgressivoCHC[1]).Trim();
                        nl.NumeroBordero = str16.Substring(nl.idxNumeroBordero[0], nl.idxNumeroBordero[1]).Trim();
                        nl.DataBordero = str16.Substring(nl.idxDataBordero[0], nl.idxDataBordero[1]).Trim();
                        nl.OraBordero = str16.Substring(nl.idxOraBordero[0], nl.idxOraBordero[1]).Trim();
                        nl.AnnoDDT = str16.Substring(nl.idxAnnoDDT[0], nl.idxAnnoDDT[1]).Trim();
                        nl.NumeroDDT = str16.Substring(nl.idxNumeroDDT[0], nl.idxNumeroDDT[1]).Trim();
                        nl.DataDDT = str16.Substring(nl.idxDataDDT[0], nl.idxDataDDT[1]).Trim();
                        nl.SerieBolla = str16.Substring(nl.idxSerieBolla[0], nl.idxSerieBolla[1]).Trim();
                        nl.Causale = str16.Substring(nl.idxCausale[0], nl.idxCausale[1]).Trim();
                        nl.DescrizioneCausale = str16.Substring(nl.idxDescrizioneCausale[0], nl.idxDescrizioneCausale[1]).Trim();
                        nl.NumDDTMandante = str16.Substring(nl.idxNumDDTMandante[0], nl.idxNumDDTMandante[1]).Trim();
                        nl.NumRifOrdine = str16.Substring(nl.idxNumRifOrdine[0], nl.idxNumRifOrdine[1]).Trim();
                        nl.RiferimentoOrdini = str16.Substring(nl.idxRiferimentoOrdini[0], nl.idxRiferimentoOrdini[1]).Trim();
                        nl.DataOrdineCliente = str16.Substring(nl.idxDataOrdineCliente[0], nl.idxDataOrdineCliente[1]).Trim();
                        nl.Linea = str16.Substring(nl.idxLinea[0], nl.idxLinea[1]).Trim();
                        nl.CodClienteIntestatario = str16.Substring(nl.idxCodClienteIntestatario[0], nl.idxCodClienteIntestatario[1]).Trim();
                        nl.CodClienteDestinatario = str16.Substring(nl.idxCodClienteDestinatario[0], nl.idxCodClienteDestinatario[1]).Trim();
                        nl.CodiceClienteDestinatarioCHC = str16.Substring(nl.idxCodiceClienteDestinatarioCHC[0], nl.idxCodiceClienteDestinatarioCHC[1]).Trim();
                        nl.RagSocialeDestinatario = str16.Substring(nl.idxRagSocialeDestinatario[0], nl.idxRagSocialeDestinatario[1]).Trim();
                        nl.IndirizzoDestinatario = str16.Substring(nl.idxIndirizzoDestinatario[0], nl.idxIndirizzoDestinatario[1]).Trim();
                        nl.LocalitaDestinatario = str16.Substring(nl.idxLocalitaDestinatario[0], nl.idxLocalitaDestinatario[1]).Trim();
                        nl.CAPDestinatario = str16.Substring(nl.idxCAPDestinatario[0], nl.idxCAPDestinatario[1]).Trim();
                        nl.ProvDestinatario = str16.Substring(nl.idxProvDestinatario[0], nl.idxProvDestinatario[1]).Trim();
                        nl.RegioneDestinatario = str16.Substring(nl.idxRegioneDestinatario[0], nl.idxRegioneDestinatario[1]).Trim();
                        nl.NazioneDestinatario = str16.Substring(nl.idxNazioneDestinatario[0], nl.idxNazioneDestinatario[1]).Trim();
                        nl.Inoltro = str16.Substring(nl.idxInoltro[0], nl.idxInoltro[1]).Trim();
                        nl.CodiceVettore = str16.Substring(nl.idxCodiceVettore[0], nl.idxCodiceVettore[1]).Trim();
                        nl.DescrizioneVettore = str16.Substring(nl.idxDescrizioneVettore[0], nl.idxDescrizioneVettore[1]).Trim();
                        nl.Valuta = str16.Substring(nl.idxValuta[0], nl.idxValuta[1]).Trim();
                        nl.ValoreOrdineContrassegno = str16.Substring(nl.idxValoreOrdineContrassegno[0], nl.idxValoreOrdineContrassegno[1]).Trim();
                        nl.PortoFA = str16.Substring(nl.idxPortoFA[0], nl.idxPortoFA[1]).Trim();
                        nl.NumeroRigheDDT = str16.Substring(nl.idxNumeroRigheDDT[0], nl.idxNumeroRigheDDT[1]).Trim();
                        nl.NumeroPezziDDT = str16.Substring(nl.idxNumeroPezziDDT[0], nl.idxNumeroPezziDDT[1]).Trim();
                        nl.NumeroColliDDT = str16.Substring(nl.idxNumeroColliDDT[0], nl.idxNumeroColliDDT[1]).Trim();
                        nl.PesoKG = str16.Substring(nl.idxPesoKG[0], nl.idxPesoKG[1]).Trim();
                        nl.VolumeM3 = str16.Substring(nl.idxVolumeM3[0], nl.idxVolumeM3[1]).Trim();
                        nl.DataConsegnaTassativa = str16.Substring(nl.idxDataConsegnaTassativa[0], nl.idxDataConsegnaTassativa[1]).Trim();
                        nl.SegnacolloIniziale = str16.Substring(nl.idxSegnacolloIniziale[0], nl.idxSegnacolloIniziale[1]).Trim();
                        nl.SegnacolloFinale = str16.Substring(nl.idxSegnacolloFinale[0], nl.idxSegnacolloFinale[1]).Trim();
                        nl.ValutaValCostoSpedizione = str16.Substring(nl.idxValutaValCostoSpedizione[0], nl.idxValutaValCostoSpedizione[1]).Trim();
                        nl.CostoSpedizione = str16.Substring(nl.idxCostoSpedizione[0], nl.idxCostoSpedizione[1]).Trim();
                        nl.ValoreSpedizioneCorriere = str16.Substring(nl.idxValoreSpedizioneCorriere[0], nl.idxValoreSpedizioneCorriere[1]).Trim();
                        nl.SimulazioneValoreSpedizione = str16.Substring(nl.idxSimulazioneValoreSpedizione[0], nl.idxSimulazioneValoreSpedizione[1]).Trim();
                        nl.DataConsegnaCliente = str16.Substring(nl.idxDataConsegnaCliente[0], nl.idxDataConsegnaCliente[1]).Trim();
                        nl.DefinizioneIMS = str16.Substring(nl.idxDefinizioneIMS[0], nl.idxDefinizioneIMS[1]).Trim();
                        nl.CampoTest1 = str16.Substring(nl.idxCampoTest1[0], nl.idxCampoTest1[1]).Trim();
                        nl.CampoTest2 = str16.Substring(nl.idxCampoTest2[0], nl.idxCampoTest2[1]).Trim();
                        nl.CampoTest3 = str16.Substring(nl.idxCampoTest3[0], nl.idxCampoTest3[1]).Trim();
                        nl.CampoTest4 = str16.Substring(nl.idxCampoTest4[0], nl.idxCampoTest4[1]).Trim();
                        nl.CampoTest5 = str16.Substring(nl.idxCampoTest5[0], nl.idxCampoTest5[1]).Trim();
                        nl.CampoTest6 = str16.Substring(nl.idxCampoTest6[0], nl.idxCampoTest6[1]).Trim();
                        nl.NoteConsegna = str16.Substring(nl.idxNoteConsegna[0], nl.idxNoteConsegna[1]).Trim();
                        List<string> list = source9.Where<string>((Func<string, bool>)(x => x.Substring(6, 9) == nl.NumeroProgressivoCHC)).ToList<string>();
                        List<Chiapparoli_DatiAccessori> chiapparoliDatiAccessoriList = new List<Chiapparoli_DatiAccessori>();
                        foreach (string str17 in list)
                        {
                            Chiapparoli_DatiAccessori chiapparoliDatiAccessori = new Chiapparoli_DatiAccessori();
                            chiapparoliDatiAccessori.CodiceDitta = str17.Substring(chiapparoliDatiAccessori.idxCodiceDitta[0], chiapparoliDatiAccessori.idxCodiceDitta[1]).Trim();
                            chiapparoliDatiAccessori.CodiceMagazzino = str17.Substring(chiapparoliDatiAccessori.idxCodiceMagazzino[0], chiapparoliDatiAccessori.idxCodiceMagazzino[1]).Trim();
                            chiapparoliDatiAccessori.DataBordero = str17.Substring(chiapparoliDatiAccessori.idxDataBordero[0], chiapparoliDatiAccessori.idxDataBordero[1]).Trim();
                            chiapparoliDatiAccessori.Descrizione = str17.Substring(chiapparoliDatiAccessori.idxDescrizione[0], chiapparoliDatiAccessori.idxDescrizione[1]).Trim();
                            chiapparoliDatiAccessori.IDServizioAggiuntivo = str17.Substring(chiapparoliDatiAccessori.idxIDServizioAggiuntivo[0], chiapparoliDatiAccessori.idxIDServizioAggiuntivo[1]).Trim();
                            chiapparoliDatiAccessori.NumeroBordero = str17.Substring(chiapparoliDatiAccessori.idxNumeroBordero[0], chiapparoliDatiAccessori.idxNumeroBordero[1]).Trim();
                            chiapparoliDatiAccessori.NumeroProgressivoCHC = str17.Substring(chiapparoliDatiAccessori.idxNumeroProgressivoCHC[0], chiapparoliDatiAccessori.idxNumeroProgressivoCHC[1]).Trim();
                            chiapparoliDatiAccessori.OraBordero = str17.Substring(chiapparoliDatiAccessori.idxOraBordero[0], chiapparoliDatiAccessori.idxOraBordero[1]).Trim();
                            chiapparoliDatiAccessoriList.Add(chiapparoliDatiAccessori);
                        }
                        foreach (Chiapparoli_DatiAccessori chiapparoliDatiAccessori in chiapparoliDatiAccessoriList)
                        {
                            if (chiapparoliDatiAccessori.IDServizioAggiuntivo == "TASSATI" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "APEDOS" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "GGSOST" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "RICONS" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "RIENTR" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "NORIT" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "SPONDA" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "CONTRP" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "CONTRF" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "POD" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "DDTCAR" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "ISOMIN" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "DISAGI" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "VOLUME" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "TRACCI" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "DOGANA" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "PIANO" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "TELEFO" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "FACCHI" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "SMS" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "APPUNT" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "TASSATI" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "GDO" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "GDOWEB" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "CARSCA" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "FUEL" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "ASSICU" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "URGENZ" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "DEDICA" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "TRIANG" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "EXTRA" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "RICPCO" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "IMBARN" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "IMS" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "ADR" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "SPOT" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "PAZIENTE" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "DIRFIS" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "CAPRI" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "ISCHIA" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "PROCIDA" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "GENOVA" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "ROMAGNA" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "REGGIO" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "SERA" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "SABATO" || chiapparoliDatiAccessori.IDServizioAggiuntivo == "GOLD" || !(chiapparoliDatiAccessori.IDServizioAggiuntivo == "PRIORITARIA"))
                                ;
                        }
                        chiapparoliShipmentInList.Add(nl);
                    }
                    foreach (Chiapparoli_ShipmentIN chiapparoliShipmentIn in chiapparoliShipmentInList)
                    {
                        Chiapparoli_ShipmentIN shipChiapparoli = chiapparoliShipmentIn;
                        RootobjectNewShipmentTMS rootobjectNewShipmentTms = new RootobjectNewShipmentTMS();
                        List<ParcelNewShipmentTMS> parcelNewShipmentTmsList = new List<ParcelNewShipmentTMS>();
                        List<StopNewShipmentTMS> stopNewShipmentTmsList = new List<StopNewShipmentTMS>();
                        List<GoodNewShipmentTMS> goodNewShipmentTmsList = new List<GoodNewShipmentTMS>();
                        string str18 = !(shipChiapparoli.PortoFA == "F") ? "PA" : "PF";
                        string str19 = "";
                        bool flag = false;
                        if (!string.IsNullOrEmpty(shipChiapparoli.DataConsegnaTassativa))
                        {
                            DateTime result = DateTime.MinValue;
                            DateTime.TryParseExact(shipChiapparoli.DataConsegnaTassativa, "yyMMdd", (IFormatProvider)null, DateTimeStyles.AssumeLocal, out result);
                            if (result != DateTime.MinValue)
                            {
                                flag = true;
                                str19 = result.ToString("yyyyMMdd");
                            }
                        }
                        string str20 = "S";
                        DateTime dateTime = DateTime.ParseExact(shipChiapparoli.DataDDT, "yyMMdd", (IFormatProvider)null);
                        string str21 = dateTime.ToString("MM-dd-yyyy");
                        HeaderNewShipmentTMS headerNewShipmentTms = new HeaderNewShipmentTMS()
                        {
                            carrierType = "EDI",
                            serviceType = str20,
                            incoterm = str18,
                            transportType = "8-25",
                            type = "Groupage",
                            insideRef = shipChiapparoli.NumeroDDT + "|" + shipChiapparoli.CodiceDitta + "|" + shipChiapparoli.SedeChiapparoli,
                            externRef = shipChiapparoli.NumeroProgressivoCHC,
                            publicNote = shipChiapparoli.NoteConsegna,
                            docDate = str21,
                            customerID = cust.ID_GESPE,
                            cashCurrency = shipChiapparoli.Valuta,
                            cashValue = Helper.GetDecimalFromString(shipChiapparoli.ValoreOrdineContrassegno, 2),
                            cashPayment = "",
                            cashNote = ""
                        };
                        List<string> list = source8.Where<string>((Func<string, bool>)(x => x.Substring(6, 9) == shipChiapparoli.NumeroProgressivoCHC)).ToList<string>();
                        List<Chiapparoli_DettaglioColli> chiapparoliDettaglioColliList = new List<Chiapparoli_DettaglioColli>();
                        foreach (string str22 in list)
                        {
                            Chiapparoli_DettaglioColli chiapparoliDettaglioColli = new Chiapparoli_DettaglioColli();
                            chiapparoliDettaglioColli.CodiceSocieta = str22.Substring(chiapparoliDettaglioColli.idxCodiceSocieta[0], chiapparoliDettaglioColli.idxCodiceSocieta[1]).Trim();
                            chiapparoliDettaglioColli.CodiceMagazzino = str22.Substring(chiapparoliDettaglioColli.idxCodiceMagazzino[0], chiapparoliDettaglioColli.idxCodiceMagazzino[1]).Trim();
                            chiapparoliDettaglioColli.CodiceDitta = str22.Substring(chiapparoliDettaglioColli.idxCodiceDitta[0], chiapparoliDettaglioColli.idxCodiceDitta[1]).Trim();
                            chiapparoliDettaglioColli.NumeroProgressivoCHC = str22.Substring(chiapparoliDettaglioColli.idxNumeroProgressivoCHC[0], chiapparoliDettaglioColli.idxNumeroProgressivoCHC[1]).Trim();
                            chiapparoliDettaglioColli.IDCollo = str22.Substring(chiapparoliDettaglioColli.idxIDCollo[0], chiapparoliDettaglioColli.idxIDCollo[1]).Trim();
                            chiapparoliDettaglioColli.IDBancale = str22.Substring(chiapparoliDettaglioColli.idxIDBancale[0], chiapparoliDettaglioColli.idxIDBancale[1]).Trim();
                            chiapparoliDettaglioColliList.Add(chiapparoliDettaglioColli);
                        }
                        int num5 = 0;
                        foreach (Chiapparoli_DettaglioColli chiapparoliDettaglioColli in chiapparoliDettaglioColliList)
                        {
                            ++num5;
                            Debug.WriteLine((object)num5);
                            GoodNewShipmentTMS goodNewShipmentTms = new GoodNewShipmentTMS();
                            goodNewShipmentTms.packs = 1;
                            ParcelNewShipmentTMS parcelNewShipmentTms = new ParcelNewShipmentTMS()
                            {
                                barcodeExt = chiapparoliDettaglioColli.IDCollo ?? ""
                            };
                            parcelNewShipmentTmsList.Add(parcelNewShipmentTms);
                            goodNewShipmentTmsList.Add(goodNewShipmentTms);
                        }
                        Helper.GetDecimalFromString(shipChiapparoli.PesoKG, 2);
                        list.Count<string>();
                        StopNewShipmentTMS stopNewShipmentTms8 = new StopNewShipmentTMS();
                        stopNewShipmentTms8.address = "";
                        stopNewShipmentTms8.country = "";
                        stopNewShipmentTms8.description = "";
                        stopNewShipmentTms8.district = "";
                        stopNewShipmentTms8.zipCode = "";
                        stopNewShipmentTms8.location = "";
                        dateTime = DateTime.Now;
                        stopNewShipmentTms8.date = dateTime.ToString("MM-dd-yyyy");
                        stopNewShipmentTms8.type = "P";
                        stopNewShipmentTms8.region = "";
                        stopNewShipmentTms8.time = shipChiapparoli.OraBordero.Insert(2, ":") + ":00";
                        StopNewShipmentTMS stopNewShipmentTms9 = stopNewShipmentTms8;
                        stopNewShipmentTmsList.Add(stopNewShipmentTms9);
                        GeoClass geoClass = this.GeoTab.FirstOrDefault<GeoClass>((Func<GeoClass, bool>)(x => x.cap == shipChiapparoli.CAPDestinatario));
                        StopNewShipmentTMS stopNewShipmentTms10 = new StopNewShipmentTMS()
                        {
                            address = shipChiapparoli.IndirizzoDestinatario.Replace("\"", ""),
                            country = shipChiapparoli.NazioneDestinatario,
                            description = shipChiapparoli.RagSocialeDestinatario.Trim().Replace("\"", ""),
                            district = shipChiapparoli.ProvDestinatario,
                            zipCode = shipChiapparoli.CAPDestinatario,
                            location = shipChiapparoli.LocalitaDestinatario,
                            date = !string.IsNullOrEmpty(str19) ? str19 : "",
                            type = "D",
                            region = geoClass != null ? geoClass.regione : "",
                            time = "",
                            obligatoryType = !string.IsNullOrEmpty(str19) ? "Date" : "Nothing"
                        };
                        stopNewShipmentTmsList.Add(stopNewShipmentTms10);
                        goodNewShipmentTmsList[0].cube = Helper.GetDecimalFromString(shipChiapparoli.VolumeM3, 3);
                        rootobjectNewShipmentTms.header = headerNewShipmentTms;
                        rootobjectNewShipmentTms.parcels = parcelNewShipmentTmsList.ToArray();
                        rootobjectNewShipmentTms.goods = goodNewShipmentTmsList.ToArray();
                        rootobjectNewShipmentTms.stops = stopNewShipmentTmsList.ToArray();
                        rootobjectNewShipmentTms.isTassativa = new bool?(flag);
                        source1.Add(rootobjectNewShipmentTms);
                    }
                }
                else
                {
                    if (cust != CustomerConnections._3CS)
                        return;
                    string[] source10 = System.IO.File.ReadAllLines(fr);
                    if (((IEnumerable<string>)source10).Count<string>() > 0)
                    {
                        List<_3C_ShipmentIN> source11 = new List<_3C_ShipmentIN>();
                        for (int index = 0; index < ((IEnumerable<string>)source10).Count<string>(); ++index)
                        {
                            string str = source10[index];
                            if (index < ((IEnumerable<string>)source10).Count<string>())
                            {
                                _3C_ShipmentIN obj = new _3C_ShipmentIN();
                                obj.Annotazioni1 = str.Substring(obj.idxAnnotazioni1[0], obj.idxAnnotazioni1[1]).Trim();
                                obj.Annotazioni2 = str.Substring(obj.idxAnnotazioni2[0], obj.idxAnnotazioni2[1]).Trim();
                                obj.Annotazioni3 = str.Substring(obj.idxAnnotazioni3[0], obj.idxAnnotazioni3[1]).Trim();
                                obj.BarcodeSegnacollo = str.Substring(obj.idxBarcodeSegnacollo[0], obj.idxBarcodeSegnacollo[1]).Trim();
                                obj.CAPDestinatario = str.Substring(obj.idxCAPDestinatario[0], obj.idxCAPDestinatario[1]).Trim();
                                obj.Colli = str.Substring(obj.idxColli[0], obj.idxColli[1]).Trim();
                                obj.Contrassegno2D = str.Substring(obj.idxContrassegno2D[0], obj.idxContrassegno2D[1]).Trim();
                                obj.DataBolla = str.Substring(obj.idxDataBolla[0], obj.idxDataBolla[1]).Trim();
                                obj.DataConsegnaTassativa = str.Substring(obj.idxDataConsegnaTassativa[0], obj.idxDataConsegnaTassativa[1]).Trim();
                                obj.Filler = str.Substring(obj.idxFiller[0], obj.idxFiller[1]).Trim();
                                obj.GiorniChiusura = str.Substring(obj.idxGiorniChiusura[0], obj.idxGiorniChiusura[1]).Trim();
                                obj.IDBarcodeSegnacollo = str.Substring(obj.idxIDBarcodeSegnacollo[0], obj.idxIDBarcodeSegnacollo[1]).Trim();
                                obj.IndirizzoDestinatario = str.Substring(obj.idxIndirizzoDestinatario[0], obj.idxIndirizzoDestinatario[1]).Trim();
                                obj.IndirizzoMittenteOriginale = str.Substring(obj.idxIndirizzoMittenteOriginale[0], obj.idxIndirizzoMittenteOriginale[1]).Trim();
                                obj.LocalitaDestinatario = str.Substring(obj.idxLocalitaDestinatario[0], obj.idxLocalitaDestinatario[1]).Trim();
                                obj.LocalitaMittenteOriginale = str.Substring(obj.idxLocalitaMittenteOriginale[0], obj.idxLocalitaMittenteOriginale[1]).Trim();
                                obj.NumeroBolla = str.Substring(obj.idxNumeroBolla[0], obj.idxNumeroBolla[1]).Trim();
                                obj.NumeroBorderau = str.Substring(obj.idxNumeroBorderau[0], obj.idxNumeroBorderau[1]).Trim();
                                obj.NumeroSegnacolliStampati = str.Substring(obj.idxNumeroSegnacolliStampati[0], obj.idxNumeroSegnacolliStampati[1]).Trim();
                                obj.NumeroTelefonoPreavviso = str.Substring(obj.idxNumeroTelefonoPreavviso[0], obj.idxNumeroTelefonoPreavviso[1]).Trim();
                                obj.Peso1D = str.Substring(obj.idxPeso1D[0], obj.idxPeso1D[1]).Trim();
                                obj.PrimoSegnacollo = str.Substring(obj.idxPrimoSegnacollo[0], obj.idxPrimoSegnacollo[1]).Trim();
                                obj.ProvDestinatario = str.Substring(obj.idxProvDestinatario[0], obj.idxProvDestinatario[1]).Trim();
                                obj.RagioneSocialeDestinatario = str.Substring(obj.idxRagioneSocialeDestinatario[0], obj.idxRagioneSocialeDestinatario[1]).Trim();
                                obj.RagioneSocialeMittenteOriginale = str.Substring(obj.idxRagioneSocialeMittenteOriginale[0], obj.idxRagioneSocialeMittenteOriginale[1]).Trim();
                                obj.RiferimentoMittenteOriginale = str.Substring(obj.idxRiferimentoMittenteOriginale[0], obj.idxRiferimentoMittenteOriginale[1]).Trim();
                                obj.TipoConsegnaTassativa = str.Substring(obj.idxTipoConsegnaTassativa[0], obj.idxTipoConsegnaTassativa[1]).Trim();
                                obj.TipoTrasporto = str.Substring(obj.idxTipoTrasporto[0], obj.idxTipoTrasporto[1]).Trim();
                                obj.TotaleDaIncassare2D = str.Substring(obj.idxTotaleDaIncassare2D[0], obj.idxTotaleDaIncassare2D[1]).Trim();
                                obj.UltimoSegnacollo = str.Substring(obj.idxUltimoSegnacollo[0], obj.idxUltimoSegnacollo[1]).Trim();
                                obj.Volume2D = str.Substring(obj.idxVolume2D[0], obj.idxVolume2D[1]).Trim();
                                obj.ValoreMerce2D = str.Substring(obj.idxValoreMerce2D[0], obj.idxValoreMerce2D[1]).Trim();
                                obj.ZonaDiConsegna = str.Substring(obj.idxZonaDiConsegna[0], obj.idxZonaDiConsegna[1]).Trim();
                                source11.Add(obj);
                            }
                        }
                        foreach (IGrouping<string, _3C_ShipmentIN> source12 in source11.GroupBy<_3C_ShipmentIN, string>((Func<_3C_ShipmentIN, string>)(x => x.NumeroBolla)).ToList<IGrouping<string, _3C_ShipmentIN>>())
                        {
                            RootobjectNewShipmentTMS rootobjectNewShipmentTms = new RootobjectNewShipmentTMS();
                            List<ParcelNewShipmentTMS> parcelNewShipmentTmsList = new List<ParcelNewShipmentTMS>();
                            List<StopNewShipmentTMS> stopNewShipmentTmsList = new List<StopNewShipmentTMS>();
                            List<GoodNewShipmentTMS> goodNewShipmentTmsList = new List<GoodNewShipmentTMS>();
                            _3C_ShipmentIN Ship3C = source12.FirstOrDefault<_3C_ShipmentIN>();
                            string str23 = !(Ship3C.TipoTrasporto == "F") ? "PA" : "PF";
                            string str24 = "";
                            bool flag = false;
                            DateTime dateTime;
                            if (!string.IsNullOrEmpty(Ship3C.DataConsegnaTassativa) && Ship3C.DataConsegnaTassativa != "00000000")
                            {
                                dateTime = DateTime.ParseExact(Ship3C.DataConsegnaTassativa, "yyyyMMdd", (IFormatProvider)null);
                                str24 = dateTime.ToString("MM-dd-yyyy");
                                if (Ship3C.TipoConsegnaTassativa.Trim().ToUpper() == "T")
                                    flag = true;
                                else if (Ship3C.TipoConsegnaTassativa.Trim().ToUpper() == "E")
                                    flag = true;
                                else if (Ship3C.TipoConsegnaTassativa.Trim().ToUpper() == "P")
                                    flag = true;
                            }
                            HeaderNewShipmentTMS headerNewShipmentTms1 = new HeaderNewShipmentTMS();
                            headerNewShipmentTms1.carrierType = "EDI";
                            headerNewShipmentTms1.serviceType = "S";
                            headerNewShipmentTms1.incoterm = str23;
                            headerNewShipmentTms1.transportType = "8-25";
                            headerNewShipmentTms1.type = "Groupage";
                            headerNewShipmentTms1.insideRef = Ship3C.NumeroBorderau;
                            headerNewShipmentTms1.internalNote = Ship3C.Annotazioni1 + " " + Ship3C.Annotazioni2 + " " + Ship3C.Annotazioni3;
                            headerNewShipmentTms1.externRef = Ship3C.NumeroBolla;
                            headerNewShipmentTms1.publicNote = Ship3C.Annotazioni1 + " " + Ship3C.Annotazioni2 + " " + Ship3C.Annotazioni3;
                            HeaderNewShipmentTMS headerNewShipmentTms2 = headerNewShipmentTms1;
                            dateTime = DateTime.Now;
                            string str25 = dateTime.ToString("MM-dd-yyyy");
                            headerNewShipmentTms2.docDate = str25;
                            headerNewShipmentTms1.customerID = cust.ID_GESPE;
                            headerNewShipmentTms1.cashCurrency = "EUR";
                            headerNewShipmentTms1.cashValue = Helper.GetDecimalFromString(Ship3C.TotaleDaIncassare2D, 2);
                            headerNewShipmentTms1.cashPayment = "";
                            headerNewShipmentTms1.cashNote = "";
                            HeaderNewShipmentTMS headerNewShipmentTms3 = headerNewShipmentTms1;
                            List<_3C_ShipmentINColli> list = source12.Select<_3C_ShipmentIN, _3C_ShipmentINColli>((Func<_3C_ShipmentIN, _3C_ShipmentINColli>)(x => new _3C_ShipmentINColli()
                            {
                                segnacollo = x.BarcodeSegnacollo
                            })).ToList<_3C_ShipmentINColli>().Skip<_3C_ShipmentINColli>(1).ToList<_3C_ShipmentINColli>();
                            int num6 = 0;
                            foreach (_3C_ShipmentINColli cShipmentInColli in list)
                            {
                                ++num6;
                                Debug.WriteLine((object)num6);
                                GoodNewShipmentTMS goodNewShipmentTms = new GoodNewShipmentTMS();
                                goodNewShipmentTms.packs = 1;
                                ParcelNewShipmentTMS parcelNewShipmentTms = new ParcelNewShipmentTMS()
                                {
                                    barcodeExt = cShipmentInColli.segnacollo ?? ""
                                };
                                parcelNewShipmentTmsList.Add(parcelNewShipmentTms);
                                goodNewShipmentTmsList.Add(goodNewShipmentTms);
                            }
                            StopNewShipmentTMS stopNewShipmentTms11 = new StopNewShipmentTMS();
                            stopNewShipmentTms11.address = "Via Luciano Lama";
                            stopNewShipmentTms11.country = "IT";
                            stopNewShipmentTms11.description = "3C-" + Ship3C.RagioneSocialeMittenteOriginale;
                            stopNewShipmentTms11.district = "PV";
                            stopNewShipmentTms11.zipCode = "27012";
                            stopNewShipmentTms11.location = "CERTOSA DI PAVIA";
                            dateTime = DateTime.Now;
                            stopNewShipmentTms11.date = dateTime.ToString("yyyy-MM-dd");
                            stopNewShipmentTms11.type = "P";
                            stopNewShipmentTms11.region = "Lombardia";
                            stopNewShipmentTms11.time = "";
                            StopNewShipmentTMS stopNewShipmentTms12 = stopNewShipmentTms11;
                            stopNewShipmentTmsList.Add(stopNewShipmentTms12);
                            GeoClass geoClass = this.GeoTab.FirstOrDefault<GeoClass>((Func<GeoClass, bool>)(x => x.cap == Ship3C.CAPDestinatario));
                            StopNewShipmentTMS stopNewShipmentTms13 = new StopNewShipmentTMS()
                            {
                                address = Ship3C.IndirizzoDestinatario.Replace("\"", ""),
                                country = "IT",
                                description = Ship3C.RagioneSocialeDestinatario.Trim().Replace("\"", ""),
                                district = Ship3C.ProvDestinatario,
                                zipCode = Ship3C.CAPDestinatario,
                                location = Ship3C.LocalitaDestinatario,
                                date = !string.IsNullOrEmpty(str24) ? str24 : "",
                                type = "D",
                                region = geoClass != null ? geoClass.regione : "",
                                time = "",
                                obligatoryType = !string.IsNullOrEmpty(str24) ? "Date" : "Nothing"
                            };
                            stopNewShipmentTmsList.Add(stopNewShipmentTms13);
                            goodNewShipmentTmsList[0].cube = Helper.GetDecimalFromString(Ship3C.Volume2D, 2);
                            goodNewShipmentTmsList[0].grossWeight = Helper.GetDecimalFromString(Ship3C.Peso1D, 1);
                            rootobjectNewShipmentTms.header = headerNewShipmentTms3;
                            rootobjectNewShipmentTms.parcels = parcelNewShipmentTmsList.ToArray();
                            rootobjectNewShipmentTms.goods = goodNewShipmentTmsList.ToArray();
                            rootobjectNewShipmentTms.stops = stopNewShipmentTmsList.ToArray();
                            rootobjectNewShipmentTms.isTassativa = new bool?(flag);
                            source1.Add(rootobjectNewShipmentTms);
                        }
                    }
                }
            }
            if (source1.Count<RootobjectNewShipmentTMS>() > 0)
            {
                foreach (RootobjectNewShipmentTMS ls in source1)
                    righeCSV.AddRange(this.ConvertiSpedizioneAPIinEDI(ls, cust));
            }
            this.CreaInviaCSVAlServiceManagerByFTP(cust, righeCSV, fr);
        }

        private string RecuperaData3C(string dataBolla)
        {
            DateTime result = DateTime.MinValue;
            DateTime.TryParseExact(dataBolla, "yyyyMMdd", (IFormatProvider)null, DateTimeStyles.None, out result);
            return result != DateTime.MinValue ? result.ToString("MM-dd-yyyy") : DateTime.Now.ToString("MM-dd-yyyy");
        }

        private List<Logistica93_ShipmentIN> RaggruppaTestateLoreal(
          List<Logistica93_ShipmentIN> listShipLoreal)
        {
            List<Logistica93_ShipmentIN> logistica93ShipmentInList = new List<Logistica93_ShipmentIN>();
            foreach (IGrouping<string, Logistica93_ShipmentIN> grouping in listShipLoreal.GroupBy<Logistica93_ShipmentIN, string>((Func<Logistica93_ShipmentIN, string>)(x => x.NumeroDDT)).ToList<IGrouping<string, Logistica93_ShipmentIN>>())
            {
                if (grouping.Count<Logistica93_ShipmentIN>() == 1)
                {
                    logistica93ShipmentInList.Add(grouping.First<Logistica93_ShipmentIN>());
                }
                else
                {
                    Logistica93_ShipmentIN logistica93ShipmentIn1 = new Logistica93_ShipmentIN();
                    Logistica93_ShipmentIN logistica93ShipmentIn2 = grouping.First<Logistica93_ShipmentIN>();
                    logistica93ShipmentIn2.NumeroColliStandard = this.SommaColliLoreal(grouping);
                    logistica93ShipmentIn2.NumeroPedaneEPAL = this.SommaPedaneEPALLoreal(grouping);
                    logistica93ShipmentIn2.NumeroPedane = this.SommaPedaneLoreal(grouping);
                    logistica93ShipmentIn2.NumeroColliDettaglio = this.SommaColliDettaglioLoreal(grouping);
                    logistica93ShipmentIn2.PesoDelivery = this.SommaPesoDeliveryLoreal(grouping);
                    logistica93ShipmentInList.Add(logistica93ShipmentIn2);
                }
            }
            return logistica93ShipmentInList;
        }

        private string SommaPesoDeliveryLoreal(IGrouping<string, Logistica93_ShipmentIN> rr)
        {
            List<Decimal> source = new List<Decimal>();
            int length = rr.First<Logistica93_ShipmentIN>().PesoDelivery.Length;
            foreach (Logistica93_ShipmentIN logistica93ShipmentIn in (IEnumerable<Logistica93_ShipmentIN>)rr)
                source.Add(Helper.GetDecimalFromString(logistica93ShipmentIn.PesoDelivery, 2));
            string str = source.Sum<Decimal>((Func<Decimal, Decimal>)(x => x)).ToString().Replace(",", "");
            while (str.Length < length)
                str = str.Insert(0, "0");
            return str;
        }

        private string SommaColliDettaglioLoreal(IGrouping<string, Logistica93_ShipmentIN> rr)
        {
            List<int> source = new List<int>();
            int length = rr.First<Logistica93_ShipmentIN>().NumeroColliDettaglio.Length;
            foreach (Logistica93_ShipmentIN logistica93ShipmentIn in (IEnumerable<Logistica93_ShipmentIN>)rr)
                source.Add(int.Parse(logistica93ShipmentIn.NumeroColliDettaglio));
            string str = source.Sum<int>((Func<int, int>)(x => x)).ToString();
            while (str.Length < length)
                str = str.Insert(0, "0");
            return str;
        }

        private string SommaPedaneLoreal(IGrouping<string, Logistica93_ShipmentIN> rr)
        {
            List<int> source = new List<int>();
            int length = rr.First<Logistica93_ShipmentIN>().NumeroPedane.Length;
            foreach (Logistica93_ShipmentIN logistica93ShipmentIn in (IEnumerable<Logistica93_ShipmentIN>)rr)
                source.Add(int.Parse(logistica93ShipmentIn.NumeroPedane));
            string str = source.Sum<int>((Func<int, int>)(x => x)).ToString();
            while (str.Length < length)
                str = str.Insert(0, "0");
            return str;
        }

        private string SommaPedaneEPALLoreal(IGrouping<string, Logistica93_ShipmentIN> rr)
        {
            List<int> source = new List<int>();
            int length = rr.First<Logistica93_ShipmentIN>().NumeroPedaneEPAL.Length;
            foreach (Logistica93_ShipmentIN logistica93ShipmentIn in (IEnumerable<Logistica93_ShipmentIN>)rr)
                source.Add(int.Parse(logistica93ShipmentIn.NumeroPedaneEPAL));
            string str = source.Sum<int>((Func<int, int>)(x => x)).ToString();
            while (str.Length < length)
                str = str.Insert(0, "0");
            return str;
        }

        private string SommaColliLoreal(IGrouping<string, Logistica93_ShipmentIN> rr)
        {
            List<int> source = new List<int>();
            int length = rr.First<Logistica93_ShipmentIN>().NumeroColliStandard.Length;
            foreach (Logistica93_ShipmentIN logistica93ShipmentIn in (IEnumerable<Logistica93_ShipmentIN>)rr)
                source.Add(int.Parse(logistica93ShipmentIn.NumeroColliStandard));
            string str = source.Sum<int>((Func<int, int>)(x => x)).ToString();
            while (str.Length < length)
                str = str.Insert(0, "0");
            return str;
        }

        private string[] AnalizzaRaggruppate(string[] tutteLeRighe)
        {
            List<string> source = new List<string>();
            for (int index = 0; index < ((IEnumerable<string>)tutteLeRighe).Count<string>(); ++index)
            {
                if (source.Count > 0)
                {
                    string str = source.Last<string>();
                    if (!(tutteLeRighe[index].Substring(36, 8) == str.Substring(36, 8)))
                        ;
                }
                else
                    source.Add(tutteLeRighe[index]);
            }
            return source.ToArray();
        }

        private void CreaInviaCSVAlServiceManagerByFTP(
          CustomerSpec cust,
          List<string> righeCSV,
          string fn)
        {
            CustomerSpec phardis = CustomerConnections.PHARDIS;
            CustomerSpec difarco = CustomerConnections.DIFARCO;
            if (cust == phardis || cust == difarco)
            {
                List<string> stringList1 = new List<string>();
                List<string> stringList2 = new List<string>();
                bool flag = false;
                foreach (string str in righeCSV)
                {
                    if (str.StartsWith("RTST"))
                    {
                        if (str.Split(';')[16].StartsWith("PHAR"))
                        {
                            stringList2.Add(str);
                            flag = true;
                        }
                        else
                        {
                            stringList1.Add(str);
                            flag = false;
                        }
                    }
                    else if (flag)
                        stringList2.Add(str);
                    else
                        stringList1.Add(str);
                }
                if (stringList2.Count<string>() > 0)
                {
                    string str = Path.GetFileNameWithoutExtension(fn) + "_" + DateTime.Now.ToString("yyyyMMddmmss") + "_phardis.csv";
                    System.IO.File.AppendAllLines(str, (IEnumerable<string>)stringList2);
                    string remotePath = Path.Combine(phardis.RemoteINPath, "DaElaborareGespe", str);
                    try
                    {
                        this._ftp = this.CreaClientFTPperIlCliente(phardis);
                        if (this._ftp == null)
                            throw new Exception("Errore FTP");
                        int num = (int)this._ftp.UploadFile(str, remotePath, FtpRemoteExists.Overwrite, false, FtpVerify.None, (Action<FtpProgress>)null);
                    }
                    catch (Exception ex)
                    {
                        GestoreMail.SendMail(new List<string>()
            {
              str
            }, "filemelzo@unitexpress.it,p.disa@xcmhealthcare.com", "Errore caricamento FTP PHARDIS", "per un problema di connessione non si è riuscito a caricare automaticamente il file in allegato. provvedere manualmente");
                    }
                    finally
                    {
                    }
                }
                if (stringList1.Count<string>() <= 0)
                    return;
                string str1 = Path.GetFileNameWithoutExtension(fn) + "_" + DateTime.Now.ToString("yyyyMMddmmss") + "_difarco.csv";
                System.IO.File.AppendAllLines(str1, (IEnumerable<string>)stringList1);
                string remotePath1 = Path.Combine(difarco.RemoteINPath, "DaElaborareGespe", str1);
                try
                {
                    this._ftp = this.CreaClientFTPperIlCliente(difarco);
                    if (this._ftp == null)
                        throw new Exception("Errore FTP");
                    int num = (int)this._ftp.UploadFile(str1, remotePath1, FtpRemoteExists.Overwrite, false, FtpVerify.None, (Action<FtpProgress>)null);
                }
                catch (Exception ex)
                {
                    GestoreMail.SendMail(new List<string>()
          {
            str1
          }, "filemelzo@unitexpress.it,p.disa@xcmhealthcare.com", "Errore caricamento FTP DI FARCO", "per un problema di connessione non si è riuscito a caricare automaticamente il file in allegato. provvedere manualmente");
                }
                finally
                {
                }
            }
            else
            {
                string str = Path.GetFileNameWithoutExtension(fn) + "_" + DateTime.Now.ToString("yyyyMMddmmss") + ".csv";
                System.IO.File.AppendAllLines(str, (IEnumerable<string>)righeCSV);
                string remotePath = Path.Combine(cust.RemoteINPath, "DaElaborareGespe", str);
                try
                {
                    this._ftp = this.CreaClientFTPperIlCliente(cust);
                    if (this._ftp == null)
                        throw new Exception("Errore FTP");
                    int num = (int)this._ftp.UploadFile(str, remotePath, FtpRemoteExists.Overwrite, false, FtpVerify.None, (Action<FtpProgress>)null);
                }
                catch (Exception ex)
                {
                    GestoreMail.SendMail(new List<string>()
          {
            str
          }, "filemelzo@unitexpress.it,p.disa@xcmhealthcare.com", "Errore caricamento FTP " + cust.NOME, "per un problema di connessione non si è riuscito a caricare automaticamente il file in allegato. provvedere manualmente");
                }
                finally
                {
                }
            }
        }

        private IEnumerable<string> ConvertiSpedizioneAPIinEDI(
          RootobjectNewShipmentTMS ls,
          CustomerSpec cust)
        {
            List<string> stringList = new List<string>();
            UnitexDefaultShipmentHeader standardCSV = new UnitexDefaultShipmentHeader();
            standardCSV.Barcode = "";
            standardCSV.CashValue = ls.header.cashValue.ToString().Replace(",", ".");
            standardCSV.CarrierType = ls.header.carrierType;
            standardCSV.CashCurrency = ls.header.cashCurrency;
            standardCSV.CashPaymentType = ls.header.cashPayment;
            standardCSV.externalRef = cust.ID_GESPE != "00342" ? ls.header.externRef : ls.header.insideRef;
            UnitexDefaultShipmentHeader defaultShipmentHeader1 = standardCSV;
            int num1;
            Decimal num2;
            string str1;
            if (ls.goods == null || ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Count<GoodNewShipmentTMS>() <= 0)
            {
                num1 = 0;
                str1 = num1.ToString();
            }
            else
            {
                num2 = ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Sum<GoodNewShipmentTMS>((Func<GoodNewShipmentTMS, Decimal>)(x => x.grossWeight));
                str1 = num2.ToString();
            }
            defaultShipmentHeader1.GrossWeight = str1;
            standardCSV.GoodsValue = "";
            standardCSV.Incoterm = ls.header.incoterm;
            standardCSV.Info = ls.header.publicNote.Replace("\"", "").Replace(";", "");
            standardCSV.info0 = ls.header.internalNote.Replace("\"", "").Replace(";", "");
            standardCSV.InsideRef = cust.ID_GESPE != "00342" ? ls.header.insideRef : ls.header.externRef;
            standardCSV.LoadAddress = ls.stops[0].address;
            standardCSV.LoadDate = ls.stops[0].date;
            standardCSV.LoadTime = ls.stops[0].time;
            standardCSV.LoadTown = ls.stops[0].location;
            standardCSV.LoadZipCode = ls.stops[0].zipCode;
            UnitexDefaultShipmentHeader defaultShipmentHeader2 = standardCSV;
            string str2;
            if (ls.goods == null || ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Count<GoodNewShipmentTMS>() <= 0)
            {
                num1 = 0;
                str2 = num1.ToString();
            }
            else
            {
                num2 = ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Sum<GoodNewShipmentTMS>((Func<GoodNewShipmentTMS, Decimal>)(x => x.meters));
                str2 = num2.ToString();
            }
            defaultShipmentHeader2.Meters = str2;
            UnitexDefaultShipmentHeader defaultShipmentHeader3 = standardCSV;
            string str3;
            if (ls.goods == null || ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Count<GoodNewShipmentTMS>() <= 0)
            {
                num1 = 0;
                str3 = num1.ToString();
            }
            else
            {
                num1 = ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Sum<GoodNewShipmentTMS>((Func<GoodNewShipmentTMS, int>)(x => x.packs));
                str3 = num1.ToString();
            }
            defaultShipmentHeader3.Packs = str3;
            UnitexDefaultShipmentHeader defaultShipmentHeader4 = standardCSV;
            string str4;
            if (ls.goods == null || ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Count<GoodNewShipmentTMS>() <= 0)
            {
                num1 = 0;
                str4 = num1.ToString();
            }
            else
            {
                num1 = ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Sum<GoodNewShipmentTMS>((Func<GoodNewShipmentTMS, int>)(x => x.floorPallet));
                str4 = num1.ToString();
            }
            defaultShipmentHeader4.Plts = str4;
            UnitexDefaultShipmentHeader defaultShipmentHeader5 = standardCSV;
            string str5;
            if (ls.goods == null || ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Count<GoodNewShipmentTMS>() <= 0)
            {
                num1 = 0;
                str5 = num1.ToString();
            }
            else
            {
                num1 = ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Sum<GoodNewShipmentTMS>((Func<GoodNewShipmentTMS, int>)(x => x.totalPallet));
                str5 = num1.ToString();
            }
            defaultShipmentHeader5.PltsTotal = str5;
            standardCSV.SegmentName = "RTST";
            standardCSV.ServiceType = ls.header.serviceType;
            standardCSV.UnloadAddress = ls.stops[1].address;
            standardCSV.UnloadCountry = ls.stops[1].country;
            standardCSV.UnloadDistrict = ls.stops[1].district;
            standardCSV.UnloadName = ls.stops[1].description;
            standardCSV.UnloadTown = ls.stops[1].location;
            standardCSV.UnloadZipCode = ls.stops[1].zipCode;
            standardCSV.UnloadDate = ls.stops[1].date == "00000000" ? "" : ls.stops[1].date;
            standardCSV.UnloadTime = ls.stops[1].time;
            UnitexDefaultShipmentHeader defaultShipmentHeader6 = standardCSV;
            string str6;
            if (ls.goods == null || ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Count<GoodNewShipmentTMS>() <= 0)
            {
                num1 = 0;
                str6 = num1.ToString();
            }
            else
            {
                num2 = ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Sum<GoodNewShipmentTMS>((Func<GoodNewShipmentTMS, Decimal>)(x => x.depth));
                str6 = num2.ToString();
            }
            defaultShipmentHeader6.GoodsDeep = str6;
            UnitexDefaultShipmentHeader defaultShipmentHeader7 = standardCSV;
            string str7;
            if (ls.goods == null || ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Count<GoodNewShipmentTMS>() <= 0)
            {
                num1 = 0;
                str7 = num1.ToString();
            }
            else
            {
                num2 = ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Sum<GoodNewShipmentTMS>((Func<GoodNewShipmentTMS, Decimal>)(x => x.height));
                str7 = num2.ToString();
            }
            defaultShipmentHeader7.GoodsHeight = str7;
            UnitexDefaultShipmentHeader defaultShipmentHeader8 = standardCSV;
            string str8;
            if (ls.goods == null || ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Count<GoodNewShipmentTMS>() <= 0)
            {
                num1 = 0;
                str8 = num1.ToString();
            }
            else
            {
                num2 = ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Sum<GoodNewShipmentTMS>((Func<GoodNewShipmentTMS, Decimal>)(x => x.width));
                str8 = num2.ToString();
            }
            defaultShipmentHeader8.GoodsWidth = str8;
            UnitexDefaultShipmentHeader defaultShipmentHeader9 = standardCSV;
            string str9;
            if (ls.goods == null || ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Count<GoodNewShipmentTMS>() <= 0)
            {
                num1 = 0;
                str9 = num1.ToString();
            }
            else
            {
                num2 = ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Sum<GoodNewShipmentTMS>((Func<GoodNewShipmentTMS, Decimal>)(x => x.cube));
                str9 = num2.ToString();
            }
            defaultShipmentHeader9.Cube = str9;
            standardCSV.LoadCountry = ls.stops[0].country;
            standardCSV.LoadDiscrict = ls.stops[0].district;
            standardCSV.LoadName = ls.stops[0].description;
            standardCSV.TransportType = ls.header.transportType;
            standardCSV.DataTassativa = ls.stops[1].obligatoryType == "Date" ? "1" : "";
            standardCSV.DocDate = ls.header.docDate;
            standardCSV.SpondaIdraulica = this.ValutaSeOccorreLaSponda(ls, cust);
            if (!string.IsNullOrEmpty(standardCSV.SpondaIdraulica))
                GestoreMail.SegnalaInserimentoSpondaIdraulica(standardCSV);
            stringList.Add(standardCSV.ToString());
            if (ls.parcels != null)
            {
                foreach (ParcelNewShipmentTMS parcel in ls.parcels)
                    stringList.Add("RSCI;" + parcel.barcodeExt);
            }
            return (IEnumerable<string>)stringList;
        }

        private string ValutaSeOccorreLaSponda(RootobjectNewShipmentTMS ls, CustomerSpec cust)
        {
            if (!CustomerConnections.CDGroup.Contains(cust))
                return "";
            string description = ls.stops[1].description;
            return (description.ToLower().Contains("f.cia") || description.ToLower().Contains("f.cie") || description.ToLower().Contains("farmacia") || description.ToLower().Contains("farmacie") || description.ToLower().Contains("osp.") || description.ToLower().Contains("ospedale")) && (ls.goods[0].floorPallet > 0 || ls.goods[0].grossWeight > 100M || ls.goods[0].packs > 30) ? "1" : "";
        }

        private IEnumerable<string> ConvertiSpedizioneAPIinEDILoreal(
          RootobjectNewShipmentTMS ls,
          CustomerSpec cust,
          Decimal PesoDelyvery,
          int ColliDelivery,
          int PltDelivery)
        {
            List<string> stringList = new List<string>();
            stringList.Add(new UnitexDefaultShipmentHeader()
            {
                Barcode = "",
                CashValue = ls.header.cashValue.ToString(),
                CarrierType = ls.header.carrierType,
                CashCurrency = ls.header.cashCurrency,
                CashPaymentType = ls.header.cashPayment,
                externalRef = (cust.ID_GESPE != "00342" ? ls.header.externRef : ls.header.insideRef),
                GrossWeight = PesoDelyvery.ToString(),
                GoodsValue = "",
                Incoterm = ls.header.incoterm,
                Info = ls.header.publicNote,
                info0 = ls.header.internalNote,
                InsideRef = (cust.ID_GESPE != "00342" ? ls.header.insideRef : ls.header.externRef),
                LoadAddress = ls.stops[0].address,
                LoadDate = ls.stops[0].date,
                LoadTime = ls.stops[0].time,
                LoadTown = ls.stops[0].location,
                LoadZipCode = ls.stops[0].zipCode,
                Meters = "0",
                Packs = ColliDelivery.ToString(),
                Plts = PltDelivery.ToString(),
                PltsTotal = PltDelivery.ToString(),
                SegmentName = "RTST",
                ServiceType = ls.header.serviceType,
                UnloadAddress = ls.stops[1].address,
                UnloadCountry = ls.stops[1].country,
                UnloadDistrict = ls.stops[1].district,
                UnloadName = ls.stops[1].description,
                UnloadTown = ls.stops[1].location,
                UnloadZipCode = ls.stops[1].zipCode,
                UnloadDate = ls.stops[1].date,
                UnloadTime = ls.stops[1].time,
                GoodsDeep = "0",
                GoodsHeight = "0",
                GoodsWidth = "0",
                Cube = (ls.goods == null || ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Count<GoodNewShipmentTMS>() <= 0 ? 0.ToString() : ((IEnumerable<GoodNewShipmentTMS>)ls.goods).Sum<GoodNewShipmentTMS>((Func<GoodNewShipmentTMS, Decimal>)(x => x.cube)).ToString()),
                LoadCountry = ls.stops[0].country,
                LoadDiscrict = ls.stops[0].district,
                LoadName = ls.stops[0].description,
                TransportType = ls.header.transportType,
                DataTassativa = (ls.stops[1].obligatoryType == "Date" ? "1" : "")
            }.ToString());
            if (ls.parcels != null)
            {
                foreach (ParcelNewShipmentTMS parcel in ls.parcels)
                    stringList.Add("RSCI;" + parcel.barcodeExt);
            }
            return (IEnumerable<string>)stringList;
        }

        private FtpClient CreaClientFTPperIlCliente(CustomerSpec cust)
        {
            try
            {
                FtpClient ftpClient = new FtpClient(cust.FTP_Address, cust.FTP_Port, cust.userFTP, cust.pswFTP);
                ftpClient.Connect();
                return ftpClient;
            }
            catch (Exception ex)
            {
                Automazione._loggerCode.Error<Exception>(ex);
                if (ex.Message != this.LastException.Message || this.DateLastException + TimeSpan.FromHours(1.0) < DateTime.Now)
                {
                    this.DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("CreaClientFTPperIlCliente-" + cust.NOME, ex);
                }
                this.LastException = ex;
                return (FtpClient)null;
            }
        }

        private SftpClient CreaClientSFTPperIlCliente(CustomerSpec cust)
        {
            try
            {
                SftpClient sftpClient = new SftpClient(cust.sftpAddress, cust.sftpUsername, cust.sftpPassword);
                sftpClient.OperationTimeout = TimeSpan.FromSeconds(30.0);
                ((BaseClient)sftpClient).Connect();
                return sftpClient;
            }
            catch (Exception ex)
            {
                Automazione._loggerCode.Error<Exception>(ex);
                if (ex.Message != this.LastException.Message || this.DateLastException + TimeSpan.FromHours(1.0) < DateTime.Now)
                {
                    this.DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("CreaClientFTPperIlCliente-" + cust.NOME, ex);
                }
                this.LastException = ex;
                return (SftpClient)null;
            }
        }

        private void UnitexGespeAPILogin(
          string userAPI,
          string passwordAPI,
          out string token,
          out DateTime scadenzaToken)
        {
            try
            {
                Automazione._loggerAPI.Info("Autenticazione " + userAPI + " in corso su endpoint " + Automazione.endpointAPI_UNITEX);
                RestClient restClient = new RestClient(Automazione.endpointAPI_UNITEX + "/api/token");
                restClient.Timeout = -1;
                RestRequest request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json-patch+json");
                request.AddHeader("Cache-Control", "no-cache");
                string str = "{\n  \"username\": \"" + userAPI + "\",\n  \"password\": \"" + passwordAPI + "\",\n  \"tenant\": \"UNITEX\"\n}";
                request.AddParameter("application/json-patch+json", (object)str, ParameterType.RequestBody);
                restClient.ClientCertificates = new X509CertificateCollection();
                ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback)((sender, certificate, chain, sslPolicyErrors) => true);
                RootobjectLoginUNITEX rootobjectLoginUnitex = JsonConvert.DeserializeObject<RootobjectLoginUNITEX>(restClient.Execute((IRestRequest)request).Content);
                scadenzaToken = rootobjectLoginUnitex.user.expire;
                token = rootobjectLoginUnitex.user.token;
            }
            catch (Exception ex)
            {
                scadenzaToken = DateTime.MinValue;
                token = "";
                Automazione._loggerCode.Error<Exception>(ex);
                if (ex.Message != this.LastException.Message || this.DateLastException + TimeSpan.FromHours(1.0) < DateTime.Now)
                {
                    this.DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev(nameof(UnitexGespeAPILogin), ex);
                }
                this.LastException = ex;
            }
        }

        public void Stop() => this.timerAggiornamentoCiclo.Stop();

        private class ModelTempiResa
        {
            internal string DataCarico { get; set; }

            internal string RifEsterno { get; set; }

            internal string DataConsegna { get; set; }

            internal string TempiResa { get; set; }

            internal string LocalitaConsegna { get; set; }

            internal string ProvinciaConsegna { get; set; }

            internal string VettoreConsegna { get; set; }

            public override string ToString() => this.DataCarico + "|" + this.RifEsterno + "|" + this.ProvinciaConsegna + "|" + this.LocalitaConsegna + "|" + this.DataConsegna + "|" + this.TempiResa + "|" + this.VettoreConsegna;
        }

        private class RitiriCDGroupBK
        {
            public string data { get; set; }

            public string extref { get; set; }

            public string colli { get; set; }

            public string peso { get; set; }

            public string volume { get; set; }

            public string plt { get; set; }
        }
    }
}
