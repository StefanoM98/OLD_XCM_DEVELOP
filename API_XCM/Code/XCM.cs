using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Timers;
using System.Web;
using System.Web.UI.WebControls;
using API_XCM.Code.APIs;
using API_XCM.Models;
using API_XCM.Models.XCM;
using DevExpress.Spreadsheet;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Newtonsoft.Json;
using NLog;
using RestSharp;

namespace API_XCM.Code
{
    public class Model
    {
        public string CAP { get; set; }
        public string Localita { get; set; }

        public static Model FromCSV(string csvLine)
        {
            var values = csvLine.Split(';');
            Model mod = new Model();
            mod.CAP = Convert.ToString(values[0]);
            mod.Localita = Convert.ToString(values[1]);
            return mod;
        }
    }

    public class XCM
    {

        #region LoginXCMApi
        private static string endpointAPI_XCM = "https://api.xcmhealthcare.com:9500";
        private static string userAPIAmministrativa = "Administrator";
        private static string passwordAPIAmministrativa = "admin";
        private DateTime DataScadenzaToken_XCM = DateTime.Now;

        private string token_XCM = "";

        //private Dictionary<string, string> clienti = new Dictionary<string, string>();
        private void XcmLogin()
        {
            try
            {
                var client = new RestClient(endpointAPI_XCM + "/api/token");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json-patch+json");
                request.AddHeader("Cache-Control", "no-cache");
                var body = @"{" + "\n" +
                $@"  ""username"": ""{userAPIAmministrativa}""," + "\n" +
                $@"  ""password"": ""{passwordAPIAmministrativa}""," + "\n" +
                @"  ""tenant"": """"" + "\n" +
                @"}";
                request.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse response = client.Execute(request);
                var resp = JsonConvert.DeserializeObject<RootobjectLoginXCM>(response.Content);

                DataScadenzaToken_XCM = resp.user.expire;
                token_XCM = resp.user.token;

            }
            catch (Exception ee)
            {

            }
        }
        private void RecuperaConnessione()
        {

            if ((DateTime.Now + TimeSpan.FromHours(1)) > DataScadenzaToken_XCM)
            {
                XcmLogin();
            }
        }
        #endregion

        #region InterscambioUNITEX
        internal static Logger _loggerCode = LogManager.GetLogger("loggerCode");
        private Helper helper = new Helper();
        private GnCommonXcmEntities db = new GnCommonXcmEntities();
        private static GnXcmEntities dbd = new GnXcmEntities();

        FTPClass _FTP = new FTPClass();
        string WorkDir = Path.Combine(@"C:\UnitexStorico\Clienti", "XCM");

        public bool GetShipments(string GespeDocNum)
        {
            try
            {
                var trip = db.TmsTrip.FirstOrDefault(x => x.DocNum == GespeDocNum && x.DocDta.Value.Year == DateTime.Now.Year);

                var ships = db.TmsLeg.Where(x => x.LinkTripID == trip.Uniq).ToList();

                List<Shipment> shipments = new List<Shipment>();
                foreach (var s in ships)
                {
                    var idS = s.UniqFrom;
                    var shDetails = db.TmsShipment.FirstOrDefault(x => x.Uniq == idS);
                    var shDocument = dbd.uvwWmsDocument.FirstOrDefault(x => x.ShipUniq == idS && x.DocTip == 204);
                    var segnaColli1 = dbd.TmsParcel.Where(x => x.UniqFrom == shDetails.Uniq).ToList();

                    //if (shDetails.InsideRef != "33414/ORM" && shDetails.InsideRef != "33417/ORM") continue;

                    var ns = new Shipment()
                    {
                        SegmentName = "RTST",
                        Colli = s.Packs != null ? s.Packs.Value : 0,
                        DataDocumento = shDetails.DocDta != null ? shDetails.DocDta.Value : DateTime.Now,
                        DocNumber = GespeDocNum,
                        IdCorriere = s.AnaID,
                        Pallet = (shDocument != null && shDocument.PltFloor != null) ? shDocument.PltFloor.Value : 0,
                        Peso = s.Weight != null ? s.Weight.Value : 0,
                        UnloadAddress = Helper.ripulisciStringa(s.EndAddress, " "),
                        UnloadCountry = s.EndCountry,
                        UnloadDes = Helper.ripulisciStringa(s.EndDes, " "),
                        UnloadLocation = Helper.ripulisciStringa(s.EndLocation, " "),
                        UnloadDistrict = s.EndDistrict,
                        UnloadRegion = s.EndRegion,
                        UnloadZipCode = (s.EndZipCode != null) ? (s.EndZipCode.Contains("xx") ? s.EndZipCode.Replace("xx", "00") : s.EndZipCode) : "",
                        Volume = 0, //s.Cube != null ? s.Cube.Value : 0,
                        Id = s.Uniq,
                        TipoTrasporto = shDetails.TransportType,
                        RifCliente = shDetails.InsideRef,
                        Note = $"{shDetails.Info} {shDetails.Info1} {shDetails.Info2}",
                        NumDTT = (shDocument != null && !string.IsNullOrEmpty(shDocument.DocNum2)) ? shDocument.DocNum2 : "",
                        DataDTT = (shDocument != null && shDocument.DocDta != null) ? shDocument.DocDta.Value : DateTime.Now,
                        LoadName = $"{shDocument.CustomerDes}"
                    };
                    var scl = new List<string>();
                    scl.AddRange(segnaColli1.Select(x => x.BarCodeMaster).Distinct());
                    ns.ParcelBarcode = scl;
                    shipments.Add(ns);
                }


                //TODO: RECUPERARI I SEGNACOLLI DI XCM
                //var trip = db.TmsTrip.FirstOrDefault(x => x.DocNum == GespeDocNum && x.DocDta.Value.Year == DateTime.Now.Year);

                //var ships = db.TmsLeg.Where(x => x.LinkTripID == trip.Uniq).ToList();

                //List<RootobjectShip> shipments = new List<RootobjectShip>();
                //foreach (var s in ships)
                //{
                //    var idS = s.UniqFrom;
                //    var shDetails = db.TmsShipment.FirstOrDefault(x => x.Uniq == idS);
                //    var shDocument = dbd.uvwWmsDocument.FirstOrDefault(x => x.ShipUniq == s.UniqFrom && x.DocTip == 204);

                //    //if (shDetails.InsideRef != "33414/ORM" && shDetails.InsideRef != "33417/ORM") continue;

                //    var ns = new RootobjectShip()
                //    {
                //        Colli = s.Packs != null ? s.Packs.Value : 0,
                //        DataDocumento = shDetails.DocDta != null ? shDetails.DocDta.Value : DateTime.Now,
                //        DocNumber = GespeDocNum,
                //        IdCorriere = s.AnaID,
                //        Pallet = (shDocument != null && shDocument.PltFloor != null) ? shDocument.PltFloor.Value : 0,
                //        Peso = s.Weight != null ? s.Weight.Value : 0,
                //        UnloadAddress = Helper.ripulisciStringa(s.EndAddress, " "),
                //        UnloadCountry = s.EndCountry,
                //        UnloadDes = Helper.ripulisciStringa(s.EndDes, " "),
                //        UnloadLocation = Helper.ripulisciStringa(s.EndLocation, " "),
                //        UnloadDistrict = s.EndDistrict,
                //        UnloadRegion = s.EndRegion,
                //        UnloadZipCode = (s.EndZipCode != null) ? (s.EndZipCode.Contains("xx") ? s.EndZipCode.Replace("xx", "00") : s.EndZipCode) : "",
                //        Volume = s.Cube != null ? s.Cube.Value : 0,
                //        Id = s.Uniq,
                //        TipoTrasporto = shDetails.TransportType,
                //        RifCliente = shDetails.InsideRef,
                //        Note = $"{shDetails.Info} {shDetails.Info1} {shDetails.Info2}",
                //        NumDTT = (shDocument != null && !string.IsNullOrEmpty(shDocument.DocNum2)) ? shDocument.DocNum2 : "",
                //        DataDTT = (shDocument != null && shDocument.DocDta != null) ? shDocument.DocDta.Value : DateTime.Now
                //    };
                //    var parcels = CommonAPITypes.ESPRITEC.EspritecParcel.RestEspritecGetParcel(s.Uniq, token_XCM);
                //    var parDes = JsonConvert.DeserializeObject<CommonAPITypes.ESPRITEC.EspritecParcel.RootobjectParcel>(parcels.Content);

                //    shipments.Add(ns);
                //}


                return ProduciCsvXCM(shipments, GespeDocNum);
            }
            catch (Exception GetShipmentsException)
            {
                _loggerCode.Error(GetShipmentsException, GetShipmentsException.Message);
                return false;
            }
        }

        public List<TripXCM> GetTrips(string fromDate)
        {
            try
            {
                var da = DateTime.Parse(fromDate);
                var trips = db.TmsTrip.Where(x => x.DocDta >= da && x.CarrierID == "00021").ToList();
#if DEBUG
                trips.Clear();
                trips.AddRange(db.TmsTrip.Where(x => x.DocNum == "01773/TR").ToList());
                trips.AddRange(db.TmsTrip.Where(x => x.DocNum == "01791/TR").ToList());
#endif
                var listTrip = new List<TripXCM>();
                foreach (var t in trips)
                {
                    var nt = new TripXCM()
                    {
                        carrierID = t.CarrierID,
                        docDate = t.DocDta.Value.ToString("u"),
                        id = t.Uniq,
                        docNumber = t.DocNum,
                        transportType = t.TransportType,
                    };
                    listTrip.Add(nt);
                }
                return listTrip;
            }
            catch (Exception GetTripsException)
            {
                _loggerCode.Error(GetTripsException, GetTripsException.Message);
                return new List<TripXCM>();
            }


        }

        public bool ProduciCsvXCM(List<Shipment> spedizioni, string viaggio)
        {
            List<string> righeCsv = new List<string>();
            List<Shipment> righeAccorpate = new List<Shipment>();
            try
            {
                foreach (var s in spedizioni)
                {                    
                    var isPresente = righeAccorpate.FirstOrDefault(x => x.UnloadAddress == s.UnloadAddress && x.UnloadDistrict == s.UnloadDistrict && x.TipoTrasporto == s.TipoTrasporto);

                    int colli = s.Colli;
                    int pallet = s.Pallet;
                    decimal peso = s.Peso;
                    decimal volume = s.Volume;

                    if (isPresente != null)
                    {
                        if (isPresente.LoadName != s.LoadName)
                        {
                            isPresente.LoadName = "XCM HEALTHCARE";
                        }
                        if (pallet > 0)
                        {
                            isPresente.Pallet += pallet;
                            isPresente.Peso += peso;
                            //isPresente.Volume += volume;
                        }
                        if (colli > 0)
                        {
                            isPresente.Colli += colli;
                            isPresente.Peso += peso;
                            //isPresente.Volume += volume;
                        }
                        if (isPresente.DataDocumento < s.DataDocumento)
                        {
                            isPresente.DataDocumento = s.DataDocumento;
                        }
                        isPresente.RifCliente = $"{isPresente.RifCliente}-{s.RifCliente}";

                        if (isPresente.Peso <= 50.0M)
                        {
                            isPresente.Volume = 0.0M;
                        }
                        if (isPresente.ParcelBarcode.Count() > 0)
                        {
                            isPresente.ParcelBarcode.AddRange(s.ParcelBarcode);
                        }
                    }
                    else
                    {
                        if (s.Peso <= 50.0M)
                        {
                            s.Volume = 0.0M;
                        }
                        righeAccorpate.Add(s);
                    }

                }

                foreach (var s in righeAccorpate)
                {
                    if (s.Pallet > 0)
                    {
                        s.Carrier = "PLT";
                    }
                    else
                    {
                        s.Carrier = "COLLO";
                    }
                    if (s.TipoTrasporto == "CO")
                    {
                        s.TipoTrasporto = "8-25";
                    }
                    //Se loadname = XCM-APS note deve contenere tassativa urgente

                    var note = "";

                    string mandante = s.LoadName;

                    if(mandante != "XCM HEALTHCARE")
                    {
                        mandante = $"{mandante} C/O XCM HEALTHCARE";
                    }

                    if (s.LoadName.ToLower() == "aps")
                    {
                        note = $"TASSATIVA URGENTE {s.Note.Trim().Replace("\r\n", " ")}";
                    }
                    else
                    {
                        note = s.Note.Trim().Replace("\r\n", " ");
                    }

                    string newLine = $"{s.SegmentName};{s.RifCliente};{s.Colli};{s.Pallet};{s.Peso};{s.Volume};{s.UnloadDes};{s.UnloadAddress};{s.UnloadLocation};{s.UnloadDistrict};{s.UnloadZipCode};{s.TipoTrasporto};{s.Carrier};{s.NumDTT};{s.DataDTT};{note};{mandante}";

                    righeCsv.Add(newLine);

                    foreach (var scl in s.ParcelBarcode)
                    {
                        righeCsv.Add($"RSCI;{scl}");
                    }
                }

                var fn = $"XCM_VIAGGIO_{viaggio.Replace("/", "_")}.csv";

                if (!Directory.Exists(WorkDir))
                {
                    Directory.CreateDirectory(WorkDir);
                }

                var dest = Path.Combine(WorkDir, $"XCM_VIAGGIO_{viaggio.Replace("/", "_")}.csv");
                var _fclient = _FTP.CreaClientFTP("unitexXCM", "!Unitex.IT@2022");

                if (File.Exists(dest))
                {
                    File.Delete(dest);
                }
                File.WriteAllLines(dest, righeCsv);

                _fclient.UploadFile(dest, Path.Combine("IN", fn), FluentFTP.FtpRemoteExists.Overwrite);
                return true;
            }
            catch (Exception ProduciCsvXCMException)
            {
                _loggerCode.Error(ProduciCsvXCMException, ProduciCsvXCMException.Message);
                return false;
            }
        }
        #endregion

        #region NINO

        #region ResocontoDocumentiIN_OUT_TEST
        //public List<ResocontoDocumentiInOutModel> GetResocontoDocumenti()
        //{
        //    List<ResocontoDocumentiInOutModel> result = new List<ResocontoDocumentiInOutModel>();

        //    var anagraficaClienti = new XCM_WMSEntities().ANAGRAFICA_CLIENTI.ToList();
        //    try
        //    {
        //        foreach (var cli in anagraficaClienti)
        //        {
        //            var resp = GetDocumentiInOutDaAPIespritec(cli.ID_GESPE);
        //            if (resp != null && resp.Count > 0)
        //            {
        //                var respOUT = resp.Where(x => x.docType == "DeliveryOUT");
        //                var respIN = resp.Where(x => x.docType == "DeliveryIN");
        //                var totalQtyOUT = int.Parse(respOUT.Sum(x => x.totalQty).ToString());
        //                var totalQtyIN = int.Parse(respIN.Sum(x => x.totalQty).ToString());

        //                result.Add(new ResocontoDocumentiInOutModel()
        //                {
        //                    Mandante = cli.RAGIONE_SOCIALE,
        //                    DeliveryOUT = respOUT != null ? respOUT.Count() : 0,
        //                    QuantitaTotaleDeliveryOUT = totalQtyOUT,
        //                    DeliveryIN = respIN != null ? respIN.Count() : 0,
        //                    QuantitaTotaleDeliveryIN = totalQtyIN
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception GetResocontoDocumentiException)
        //    {

        //        _loggerCode.Error(GetResocontoDocumentiException, GetResocontoDocumentiException.Message);

        //    }
        //    //this.CreaReport(result, true, "gaetano.colella@xcmhealthcare.com");
        //    //this.CreaReport(result, true, "d.valitutti@xcmhealthcare.com");
        //    return result.OrderByDescending(x => x.DeliveryOUT).ToList();
        //}
        //public List<Document> GetDocumentiInOutDaAPIespritec(string customerID)
        //{
        //    RecuperaConnessione();
        //    var result = new List<Document>();
        //    var fromDate = DateTime.Today.AddDays(-1).ToUniversalTime().ToString("u");
        //    var pageNumber = 1;
        //    var docTypeOUT = "DeliveryOUT";
        //    var docTypeIN = "DeliveryIN";

        //    var client = new RestClient(endpointAPI_XCM);
        //    var request = new RestRequest($"/api/wms/document/list/1000/{pageNumber}?FromDate={fromDate}&ToDate={fromDate}&CustomerID={customerID}", Method.GET);


        //    client.Timeout = -1;
        //    request.AddHeader("Authorization", $"Bearer {token_XCM}");
        //    request.AlwaysMultipartFormData = true;
        //    IRestResponse response = client.Execute(request);

        //    var resp = JsonConvert.DeserializeObject<RootobjectDocument>(response.Content);

        //    if (resp.documents != null)
        //    {
        //        var maxPages = resp.result.maxPages;
        //        result = resp.documents.Where(x => x.docType == "DeliveryOUT" || x.docType == "DeliveryIN").ToList();
        //        while (maxPages > 1)
        //        {
        //            pageNumber++;
        //            maxPages--;
        //            request = new RestRequest($"/api/wms/document/list/1000/{pageNumber}?FromDate={fromDate}&ToDate={fromDate}&CustomerID={customerID}", Method.GET);
        //            request.AddHeader("Authorization", $"Bearer {token_XCM}");
        //            request.AlwaysMultipartFormData = true;
        //            response = client.Execute(request);
        //            resp = JsonConvert.DeserializeObject<RootobjectDocument>(response.Content);

        //            if (resp.documents != null)
        //            {
        //                result.AddRange(resp.documents.Where(x => x.docType == "DeliveryOUT" || x.docType == "DeliveryIN").ToList());
        //            }

        //        }
        //    }
        //    return result;
        //}
        private void CreaReport(List<ResocontoDocumentiInOutModel> movs, bool invioEmail, string mailTo)
        {
            var sourceFileName = @"C:\XCM\MovimentazioniGiornaliere.xlsx";
            var fn = $"MOV_{DateTime.Now.ToString("yyyyMMdd_HHmmssffff")}.xlsx";

            Workbook workbook = new Workbook();
            workbook.LoadDocument(sourceFileName);
            var wksheet = workbook.Worksheets[0];
            int i = 2;

            foreach (var elem in movs)
            {
                wksheet.Cells[$"A{i}"].Value = elem.Mandante;
                wksheet.Cells[$"B{i}"].Value = elem.DeliveryOUT;
                wksheet.Cells[$"C{i}"].Value = elem.QuantitaTotaleDeliveryOUT;
                wksheet.Cells[$"D{i}"].Value = elem.DeliveryIN;
                wksheet.Cells[$"E{i}"].Value = elem.QuantitaTotaleDeliveryIN;
                i++;
            }
            workbook.SaveDocument(Path.Combine(@"C:\XCM\", fn), DocumentFormat.Xlsx);
            if (invioEmail && movs.Count > 0)
            {
                this.InviaEmail(mailTo, fn);
            }
        }
        private void InviaEmail(string mailTo, string pathFile)
        {
            var fromAddress = new MailAddress("itsupport@xcmhealthcare.com", "XCM Healthcare");

            string fromPassword = "AdminIT.Manager";
            string subject = $"Resoconto giornaliero";
            var body = string.Format(
@"Gentile Gaetano Colella,
in allegato il documento con il resoconto del giorno {0} per quanto concerne gli ordini
", DateTime.Today.AddDays(-1).ToShortDateString());

            try
            {
                if (!string.IsNullOrWhiteSpace(mailTo))
                {
                    var toa = new MailAddress(mailTo);
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("itsupport@xcmhealthcare.com", fromPassword)
                    };

                    var message = new MailMessage(fromAddress, toa);

                    message.Subject = subject;
                    message.Body = body;

                    message.Attachments.Add(new Attachment(Path.Combine(@"C:\XCM\", pathFile)));


                    smtp.Send(message);
                }
            }
            catch (Exception SendMailException)
            {
                _loggerCode.Error(SendMailException, SendMailException.Message);

            }
        }

        #endregion

        #region ResocontoDocumentiNonSpediti
        public static List<DocumentiNonSpeditiModel> model = null;

        public static List<DocumentiNonSpeditiModel> GetResocontoDocumentiNonSpediti()
        {
            //List<DocumentiNonSpeditiModel> result = new List<DocumentiNonSpeditiModel>();
            if (model != null)
            {
                return model;
            }
            else
            {
                model = new List<DocumentiNonSpeditiModel>();
            }
            var now = DateTime.Now;

            //var timeSpan24 = DateTime.Now - TimeSpan.FromHours(24);
            var timeSpan24 = new DateTime(now.Year, now.Month, now.Day, 00, 00, 00);
            //var timeSpan24 = DateTime.Now - TimeSpan.FromHours(24);
            var prevMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);

            var documents = dbd.uvwWmsDocument.Where(x =>
             x.RecCreate < timeSpan24 &&
             x.ItemStatus != 40 &&
             x.ItemStatus != 50 &&
             x.RecCreate > prevMonth &&
             x.DocTip == 203)
                 .OrderByDescending(x => x.RecCreate).ToList();

            Dictionary<string, int> ordiniEvasi = new Dictionary<string, int>();

            if (documents != null && documents.Count > 0)
            {
                foreach (var doc in documents)
                {
                    bool add = true;

                    if (!string.IsNullOrEmpty(doc.ItemInfo) && doc.ItemInfo.StartsWith("GRO"))
                    {
                        if (doc.ItemStatus == 30)
                        {
                            add = false;
                        }
                    }
                    if (doc.ItemStatus == 30 && add) add = false;

                    if (add)
                    {
                        model.Add(new DocumentiNonSpeditiModel()
                        {
                            Mandante = doc.CustomerDes,
                            GespeID = doc.AnaId,
                            RiferimentoExt = doc.Reference != null ? doc.Reference : "",
                            Coverage = (int)Math.Floor((decimal)doc.Coverage),
                            Executed = (int)Math.Floor((decimal)doc.Executed),
                            CreatedOn = doc.RecCreate,
                            Quantity = doc.TotalQty != null ? doc.TotalQty.Value : 0,
                            StatoDocumento = doc.StatusDes
                        });

                    }

                }

            }
            return model;

        }

        //public List<Document> GetOrderOutDaAPIespritec(string customerID, string fromDate, string toDate)
        //{
        //    RecuperaConnessione();
        //    var result = new List<Document>();
        //    var pageNumber = 1;
        //    var docType = "DocType=OrderOUT";

        //    var client = new RestClient(endpointAPI_XCM);
        //    var request = new RestRequest($"/api/wms/document/list/1000/{pageNumber}?FromDate={fromDate}&ToDate={toDate}&{docType}&CustomerID={customerID}", Method.GET);


        //    client.Timeout = -1;
        //    request.AddHeader("Authorization", $"Bearer {token_XCM}");
        //    request.AlwaysMultipartFormData = true;
        //    IRestResponse response = client.Execute(request);

        //    var resp = JsonConvert.DeserializeObject<RootobjectDocument>(response.Content);

        //    if (resp.documents != null)
        //    {
        //        var maxPages = resp.result.maxPages;
        //        result = resp.documents.Where(x => x.docType == "OrderOUT").ToList();
        //        while (maxPages > 1)
        //        {
        //            pageNumber++;
        //            maxPages--;
        //            request = new RestRequest($"/api/wms/document/list/1000/{pageNumber}?FromDate={fromDate}&ToDate={fromDate}&{docType}&CustomerID={customerID}", Method.GET);
        //            request.AddHeader("Authorization", $"Bearer {token_XCM}");
        //            request.AlwaysMultipartFormData = true;
        //            response = client.Execute(request);
        //            resp = JsonConvert.DeserializeObject<RootobjectDocument>(response.Content);

        //            if (resp.documents != null)
        //            {
        //                result.AddRange(resp.documents.Where(x => x.docType == "OrderOUT").ToList());
        //            }

        //        }
        //    }
        //    return result;
        //}
        #endregion

        #region ReportNino
        public static List<Docs> docsModel = null;
        public List<Docs> GetDocs()
        {
            if (docsModel != null)
            {
                return docsModel;
            }
            else
            {
                docsModel = new List<Docs>();
            }

            var anagraficaClienti = new XCM_WMSEntities().ANAGRAFICA_CLIENTI.ToList();
            try
            {
                foreach (var cli in anagraficaClienti)
                {
                    if (cli.RAGIONE_SOCIALE == "ASLNANORD" || cli.RAGIONE_SOCIALE == "VIVISOLNA")
                    {
                        continue;
                    }

                    //TODO: considerare venerdi, sabato e domenica.
                    //TODO: deve prendere fino alle 11:30
                    var dayOfWeek = DateTime.Now.ToString("dddd");
                    var delta = 24;
                    if (dayOfWeek.Contains("luned"))
                    {
                        delta = 72;
                    }
                    else if (dayOfWeek.Contains("sabato") || dayOfWeek.Contains("domenica"))
                    {
                        break;
                    }
                    DateTime dateTest = DateTime.Now;

                    var fromDate = dateTest.AddHours(-delta);
                    var day = fromDate.Day;
                    if (day != 1)
                    {
                        day -= 1;
                    }
                    var toDate = new DateTime(fromDate.Year, fromDate.Month, day, 08, 30, 00);

                    var now = DateTime.Now;

                    //var timeSpan24 = DateTime.Now - TimeSpan.FromHours(24);
                    var timeSpan24 = new DateTime(now.Year, now.Month, now.Day, 00, 00, 00);
                    var prevMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);

                    if (cli.RAGIONE_SOCIALE == "APS")
                    {

                    }
                    //Prendi tutti i documenti creati dal mese scorso fino a 24 ore fa
                    var documents = dbd.uvwWmsDocument.Where(x =>
                        x.RecCreate < timeSpan24 &&
                        x.ItemStatus != 50 &&
                        x.RecCreate > prevMonth &&
                        x.DocTip == 203 &&
                        x.CustomerID == cli.ID_GESPE)
                            .OrderByDescending(x => x.RecCreate).ToList();

                    //Di questi documenti prendi gli evasi e spediti nelle ultime 24h
                    var ordiniEvasi = documents.Where(x =>
                        (x.ItemStatus == 30 || x.ItemStatus == 40) &&
                        x.RecChange > fromDate &&
                        x.RecChange < DateTime.Now)
                        .ToList();

                    //Prendi tutti quelli ancora non evasi
                    var nonEvasi = documents.Where(x =>
                        x.ItemStatus == 10 ||
                        x.ItemStatus == 20)
                            .ToList();

                    if (ordiniEvasi.Count() > 0 || nonEvasi.Count() > 0)
                    {
                        docsModel.Add(new Docs()
                        {
                            Mandante = cli.RAGIONE_SOCIALE,
                            GespeID = cli.ID_GESPE,
                            Evasi = ordiniEvasi.Count(),
                            NonEvasi = nonEvasi.Count(),
                            //NuoviOrdiniGiornalieri = nuoviOrdini.Count(),

                        });
                    }
                }
            }
            catch (Exception ee)
            {

            }

            return docsModel;
        }

        public static List<DocumentiNonSpeditiModel> ModelExist()
        {
            if (model == null)
            {
                model = GetResocontoDocumentiNonSpediti();
                return model;
            }
            else
            {
                return model;
            }
        }

        public static List<DocumentiNonSpeditiModel> GetResocontoDocumentiNonSpeditiDaIDGespe(string CustomerID)
        {
            return ModelExist().Where(x => x.GespeID == CustomerID).ToList();
        }

        public void RefreshModel()
        {
            docsModel = null;
            docsModel = GetDocs();
            model = null;
            model = GetResocontoDocumentiNonSpediti();
        }
        #endregion

        #region ExportDetailsGridView
        public static GridViewSettings CreateGeneralDetailGridSettings(string customerID)
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "detailGrid_" + customerID;
            settings.Width = Unit.Percentage(100);

            settings.KeyFieldName = "CreatedOn";

            settings.Columns.Add(c =>
            {
                c.FieldName = "Mandante";
                c.Caption = "Mandante";
                c.Visible = false;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "CreatedOn";
                c.Caption = "Data Creazione";
                c.AdaptivePriority = 0;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "RiferimentoExt";
                c.Caption = "Riferimento Esterno";
                c.AdaptivePriority = 5;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "StatoDocumento";
                c.Caption = "Stato";
                c.AdaptivePriority = 1;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Quantity";
                c.Caption = "Quantita";
                c.AdaptivePriority = 2;
                c.PropertiesEdit.DisplayFormatString = "#####";

            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Coverage";
                c.Caption = "Copertura";
                c.AdaptivePriority = 3;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Executed";
                c.Caption = "Eseguito";
                c.AdaptivePriority = 4;
            });

            settings.CustomColumnDisplayText = (s, e) =>
            {
                var grd = s as ASPxGridView;
                var idx = e.VisibleIndex;
                if (idx < 0) return;
                var row = grd.GetRow(idx) as API_XCM.Models.XCM.DocumentiNonSpeditiModel;
                if (e.Column.FieldName == "Coverage")
                {
                    e.DisplayText = $"{row.Coverage}%";

                }
                else if (e.Column.FieldName == "Executed")
                {
                    e.DisplayText = $"{row.Executed}%";

                }
            };

            settings.SettingsDetail.MasterGridName = "masterGrid";
            settings.Styles.Header.Wrap = DevExpress.Utils.DefaultBoolean.True;

            return settings;
        }
        #endregion

        #endregion

        #region APS
        //private string ApsCustomerID = "00024";

        //private static string userApsAPI = "apsapi";
        //private static string passwordApsAPI = "P}'5V14i";
        //private DateTime DataScadenzaToken_XCM_APS = DateTime.Now;

        //private string token_XCM_APS = "";

        //private void XcmLoginAPS()
        //{
        //    try
        //    {
        //        var client = new RestClient(endpointAPI_XCM + "/api/token");
        //        client.Timeout = -1;
        //        var request = new RestRequest(Method.POST);
        //        request.AddHeader("Content-Type", "application/json-patch+json");
        //        request.AddHeader("Cache-Control", "no-cache");
        //        var body = @"{" + "\n" +
        //        $@"  ""username"": ""{userApsAPI}""," + "\n" +
        //        $@"  ""password"": ""{passwordApsAPI}""," + "\n" +
        //        @"  ""tenant"": """"" + "\n" +
        //        @"}";
        //        request.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
        //        client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
        //        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        //        IRestResponse response = client.Execute(request);
        //        var resp = JsonConvert.DeserializeObject<RootobjectLoginXCM>(response.Content);
        //        //TODO: se il response è ok altrimenti no
        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            DataScadenzaToken_XCM_APS = resp.user.expire;
        //            token_XCM_APS = resp.user.token;
        //        }
        //    }
        //    catch (Exception ee)
        //    {

        //    }
        //}
        //private void RecuperaConnessioneAPS()
        //{

        //    if ((DateTime.Now + TimeSpan.FromHours(1)) > DataScadenzaToken_XCM_APS)
        //    {
        //        XcmLoginAPS();
        //    }
        //}

        //public List<ApsDafneResocontoModel> GetDocumentsAPS()
        //{
        //    List<ApsDafneResocontoModel> result = new List<ApsDafneResocontoModel>();


        //    try
        //    {
        //        var timeSpan24 = DateTime.Now - TimeSpan.FromHours(24);
        //        var prevMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);

        //        //var resp = dbd.uvwWmsDocument.Where(x => x.AnaId == ApsCustomerID && x.RecCreate > prevMonth && x.DocTip == 203 && !string.IsNullOrEmpty(x.Info1)).OrderByDescending(x => x.RecCreate).ToList();
        //        var resp = dbd.uvwWmsDocument.Where(x => x.AnaId == ApsCustomerID && x.ItemStatus == 10 && x.RecCreate > prevMonth && x.DocTip == 203 && !string.IsNullOrEmpty(x.Info1)).OrderByDescending(x => x.RecCreate).ToList();

        //        if (resp != null && resp.Count > 0)
        //        {
        //            foreach (var doc in resp)
        //            {
        //                var rows = GetDocumentRowESPRITEC(doc.uniq);
        //                foreach (var row in rows)
        //                {
        //                    if (row.partNumber == "039982018")
        //                    {
        //                        result.Add(new ApsDafneResocontoModel()
        //                        {
        //                            RiferimentoCliente = doc.Reference,
        //                            RiferimentoDafne = doc.Info1,
        //                            PartNumber = row.partNumber,
        //                            PartNumberDes = row.partNumberDes,
        //                            Quantita = int.Parse(row.qty.ToString()),
        //                            RowID = row.id,
        //                            DocumentID = doc.uniq
        //                        });
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception GetResocontoDocumentiException)
        //    {

        //        _loggerCode.Error(GetResocontoDocumentiException, GetResocontoDocumentiException.Message);

        //    }
        //    //this.CreaReport(result, true, "gaetano.colella@xcmhealthcare.com");
        //    //this.CreaReport(result, true, "d.valitutti@xcmhealthcare.com");
        //    return result;
        //}
        //public List<Document> GetOrderOutDaAPIespritecAPS(string customerID)
        //{
        //    RecuperaConnessioneAPS();
        //    var result = new List<Document>();
        //    var pageNumber = 1;
        //    var docType = "DocType=OrderOUT";

        //    var client = new RestClient(endpointAPI_XCM);
        //    var request = new RestRequest($"/api/wms/document/list/1000/{pageNumber}?{docType}&CustomerID={customerID}", Method.GET);


        //    client.Timeout = -1;
        //    request.AddHeader("Authorization", $"Bearer {token_XCM_APS}");
        //    request.AlwaysMultipartFormData = true;
        //    IRestResponse response = client.Execute(request);

        //    var resp = JsonConvert.DeserializeObject<RootobjectDocument>(response.Content);

        //    if (resp != null && resp.documents != null)
        //    {
        //        var maxPages = resp.result.maxPages;
        //        result = resp.documents.ToList();
        //        while (maxPages > 1)
        //        {
        //            pageNumber++;
        //            maxPages--;
        //            request = new RestRequest($"/api/wms/document/list/1000/{pageNumber}?{docType}&CustomerID={customerID}", Method.GET);
        //            request.AddHeader("Authorization", $"Bearer {token_XCM}");
        //            request.AlwaysMultipartFormData = true;
        //            response = client.Execute(request);
        //            resp = JsonConvert.DeserializeObject<RootobjectDocument>(response.Content);

        //            if (resp.documents != null)
        //            {
        //                result.AddRange(resp.documents.ToList());
        //            }

        //        }
        //    }
        //    return result;
        //}

        //public HeaderXCMOrderNEW GetDocumentByIdESPRITEC(int docID)
        //{
        //    RecuperaConnessioneAPS();
        //    var result = new List<HeaderXCMOrderNEW>();

        //    var client = new RestClient(endpointAPI_XCM);
        //    client.Timeout = -1;
        //    var request = new RestRequest($"/api/wms/document/get/{docID}", Method.GET);
        //    request.AddHeader("Authorization", $"Bearer {token_XCM_APS}");
        //    var response = client.Execute(request);
        //    var xcmDoc = JsonConvert.DeserializeObject<RootobjectXCMOrderNEW>(response.Content);

        //    if (xcmDoc != null && xcmDoc.header != null)
        //    {
        //        return xcmDoc.header;

        //    }
        //    else
        //    {
        //        return new HeaderXCMOrderNEW();
        //    }
        //}

        //public List<RowXCMRowsNew> GetDocumentRowESPRITEC(int docID)
        //{
        //    RecuperaConnessioneAPS();
        //    var client = new RestClient(endpointAPI_XCM);
        //    client.Timeout = -1;
        //    var request = new RestRequest($"/api/wms/document/row/list/{docID}", Method.GET);
        //    request.AddHeader("Authorization", $"Bearer {token_XCM_APS}");
        //    var response = client.Execute(request);
        //    var xcmRec = JsonConvert.DeserializeObject<RootobjectXCMRowsNEW>(response.Content);
        //    if (xcmRec != null && xcmRec.rows != null)
        //    {
        //        return xcmRec.rows.ToList();

        //    }
        //    else
        //    {
        //        return new List<RowXCMRowsNew>();
        //    }
        //}

        //public IRestResponse UpdateDocumentRowESPRITEC(UpdateDocumentRowRootobject bodyModel)
        //{
        //    var client = new RestClient(endpointAPI_XCM + "/api/wms/document/row/update");
        //    client.Timeout = -1;
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("Authorization", $"Bearer {token_XCM_APS}");
        //    request.AddHeader("Content-Type", "application/json");
        //    var body = JsonConvert.SerializeObject(bodyModel);

        //    request.AddParameter("application/json", body, ParameterType.RequestBody);
        //    IRestResponse response = client.Execute(request);

        //    return response;
        //}

        #endregion

        #region SettaDocumentSellPrice
        public string SettaValoriRighaDocumentoOrdine(int idRigha, decimal price)
        {
            try
            {
                var db = new GnXcmEntities();
                var qu = $"UPDATE WmsDocumentRow set SellPrice={price.ToString().Replace(',', '.')} WHERE RowID='" + idRigha.ToString() + "'";

                db.Database.ExecuteSqlCommand(qu, price);
                return "true";
            }
            catch (Exception ee)
            {
                return ee.Message;

            }


        }


        #endregion

        #region Ordini CRM
        public long InviaOrdineAdAPIXCM(Models.XCM.CRM.Orders nO)
        {
            var ch = JsonConvert.SerializeObject(nO, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var client = new RestClient(endpointAPI_XCM + "/api/wms/document/new");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json-patch+json");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {token_XCM}");
            request.AddJsonBody(ch);

            request.AddParameter("application/json-patch+json", ParameterType.RequestBody);
            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            IRestResponse response = client.Execute(request);

            var res = JsonConvert.DeserializeObject<Models.XCM.CRM.SyncroDB_CRM.ResultOrderViewModel>(response.Content);

            if (res.status)
            {
                return long.Parse(res.info);
            }
            else
            {
                return 0;
            }

        }
        public bool CancellaOrdine(long idDocApi)
        {
            try
            {
                var client = new RestClient(endpointAPI_XCM + $"/api/wms/document/delete/{idDocApi}");
                client.Timeout = -1;
                var request = new RestRequest(Method.DELETE);
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", $"Bearer {token_XCM}");
                client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse response = client.Execute(request);

                return response.IsSuccessful;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region TempiDiResa
        public static byte[] GetTempiResa(string startDate, string customerID)
        {
            try
            {
                List<TempiResaModel> output = new List<TempiResaModel>();

                EspritecAPI_UNITEX.Init("xcm", "Xcm@2022", "UNITEX");

                var shipments = EspritecAPI_UNITEX.TmsShipmentList(startDate, "");

                //EspritecAPI_UNITEX.Init("dvalitutti", "Dv$2022!", "UNITEX");

                //var trips = EspritecAPI_UNITEX.GetTrips();

                //EspritecAPI_XCM.Init("phphApi", "Testtest1!", "");

                //var documents = EspritecAPI_XCM.WmsDocumentList(startDate, "", "", "OrderOUT", 40);

                GnXcmEntities db = new GnXcmEntities();

                var fromDate = DateTime.ParseExact(startDate, "MM-dd-yyyy", null);

                var Alldocuments = db.uvwWmsDocument.Where(x =>
                    x.ItemStatus != 50 &&
                    x.RecCreate >= fromDate &&
                    x.DocTip == 203 &&
                    x.CustomerID == customerID)
                        .OrderByDescending(x => x.RecCreate).ToList();

                var documents = Alldocuments.Where(x => x.ItemStatus == 40).ToList();

                foreach (var ship in shipments)
                {
                    var riferimenti = ship.externRef.Split('-');

                    foreach (var rif in riferimenti)
                    {
                        var document = documents.FirstOrDefault(x => x.DocNum == rif);
                        if (document != null)
                        {
                            //var corr = "";

                            //foreach(var trip in trips)
                            //{
                            //    var tripShip = EspritecAPI_UNITEX.TmsTripStopList(trip.id);

                            //    if(tripShip != null)
                            //    {
                            //        var exist = tripShip.FirstOrDefault(x => x.shipDocNumber == ship.docNumber);

                            //        if(exist != null)
                            //        {
                            //            if (!string.IsNullOrEmpty(trip.carrierDes))
                            //            {
                            //                corr = $"{trip.carrierDes}";
                            //                break;

                            //            }
                            //        }
                            //    }
                            //}

                            var trackingDetail = EspritecAPI_UNITEX.TmsShipmentTrackingList(ship.id);
                            if (trackingDetail != null)
                            {
                                var lastTracking = trackingDetail.OrderByDescending(x => x.timeStamp).First();
                                var consegnata = trackingDetail.FirstOrDefault(x => x.statusID == 30);
                                var status = "IN DEPOSITO";
                                var dataTracking = "";
                                if (consegnata != null)
                                {
                                    status = consegnata.statusDes;
                                    dataTracking = consegnata.timeStamp.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    var inConsenga = trackingDetail.FirstOrDefault(x => x.statusID == 10);

                                    if (inConsenga != null)
                                    {
                                        status = inConsenga.statusDes;
                                    }
                                }

                                string DataConsegna = dataTracking;
                                string tempiResa = "";

                                if (dataTracking != "")
                                {
                                    var tempiResaValue = Helper.DiffTraDateEsclusiSabatoEDomeniche(document.DocDta.Value, lastTracking.timeStamp) * 24;

                                    DataConsegna = dataTracking;

                                    //Riporta i tempi di resa a massimo 72 ore
                                    //if (tempiResaValue > 72)
                                    //{
                                    //    DataConsegna = Helper.TempiDiResa72(document.DocDta.Value, lastTracking.timeStamp).ToString();
                                    //    tempiResaValue = 72;                                    
                                    //}

                                    tempiResa = $"{tempiResaValue} H";
                                }

                                var element = new TempiResaModel()
                                {
                                    DocDate = document.DocDta.Value.ToString("dd/MM/yyyy"),
                                    DocNumber = document.DocNum,
                                    DataConsegna = DataConsegna,
                                    StatusDes = status,
                                    ExternalRef = document.Reference,
                                    Bancali = int.Parse(ship.totalPallets.ToString()),
                                    Colli = Convert.ToInt32(decimal.Parse(ship.packs.ToString())),
                                    Peso = decimal.Parse(ship.grossWeight.ToString().Replace('.', ',')),
                                    UnloadAddress = document.UnloadAddress,
                                    UnloadDes = document.UnloadName,
                                    UnloadDistrict = document.UnloadDistrict,
                                    UnloadZipCode = document.UnloadZipCode,
                                    TempiResa = tempiResa,
                                    UnloadLocation = document.UnloadLocation,
                                    //Corrispondente = corr,
                                    //RiferimentoUnitex = ship.docNumber,
                                    //RiferimentoEsternoUnitex = ship.externRef,

                                };

                                output.Add(element);



                            }
                        }
                    }
                }

                Workbook workbook = new Workbook();
                var wksheet = workbook.Worksheets[0];


                int index = 2;
                wksheet.Cells[$"A{1}"].Value = "Data Documento";
                wksheet.Cells[$"B{1}"].Value = "Numero Documento";
                wksheet.Cells[$"C{1}"].Value = "Riferimento";
                wksheet.Cells[$"D{1}"].Value = "Stato Spedizione";
                wksheet.Cells[$"E{1}"].Value = "Data Consenga";
                wksheet.Cells[$"F{1}"].Value = "Tempi resa";
                wksheet.Cells[$"G{1}"].Value = "Destinazione";
                wksheet.Cells[$"H{1}"].Value = "Indirizzo";
                wksheet.Cells[$"I{1}"].Value = "CAP";
                wksheet.Cells[$"J{1}"].Value = "Provincia";
                wksheet.Cells[$"K{1}"].Value = "Località";
                wksheet.Cells[$"L{1}"].Value = "Colli";
                wksheet.Cells[$"M{1}"].Value = "Pallet";
                wksheet.Cells[$"N{1}"].Value = "Peso";
                //wksheet.Cells[$"O{1}"].Value = "Corrispondente";
                //wksheet.Cells[$"P{1}"].Value = "Riferimento Unitex";
                //wksheet.Cells[$"Q{1}"].Value = "Riferimento Ext Unitex";

                foreach (var elem in output)
                {
                    wksheet.Cells[$"A{index}"].Value = elem.DocDate;
                    wksheet.Cells[$"B{index}"].Value = elem.DocNumber;
                    wksheet.Cells[$"C{index}"].Value = elem.ExternalRef;
                    wksheet.Cells[$"D{index}"].Value = elem.StatusDes;
                    wksheet.Cells[$"E{index}"].Value = elem.DataConsegna;
                    wksheet.Cells[$"F{index}"].Value = elem.TempiResa;
                    wksheet.Cells[$"G{index}"].Value = elem.UnloadDes;
                    wksheet.Cells[$"H{index}"].Value = elem.UnloadAddress;
                    wksheet.Cells[$"I{index}"].Value = elem.UnloadZipCode;
                    wksheet.Cells[$"J{index}"].Value = elem.UnloadDistrict;
                    wksheet.Cells[$"K{index}"].Value = elem.UnloadLocation;
                    wksheet.Cells[$"L{index}"].Value = elem.Colli;
                    wksheet.Cells[$"M{index}"].Value = elem.Bancali;
                    wksheet.Cells[$"N{index}"].Value = elem.Peso;
                    //wksheet.Cells[$"O{index}"].Value = elem.Corrispondente;
                    //wksheet.Cells[$"P{index}"].Value = elem.RiferimentoUnitex;
                    //wksheet.Cells[$"Q{index}"].Value = elem.RiferimentoEsternoUnitex;

                    index++;
                }

                var finalDest = @"C:\XCM\";

                var saveAs = Path.Combine(finalDest, $"TempiResa_{customerID}_{DateTime.Now.Ticks}.xlsx");
                if (File.Exists(saveAs))
                {
                    var jn = Path.ChangeExtension(saveAs, $"{DateTime.Now.Ticks}.old");
                    File.Move(saveAs, jn);
                }
                workbook.SaveDocument(saveAs, DocumentFormat.Xlsx);

                byte[] bytes = System.IO.File.ReadAllBytes(saveAs);
                if (File.Exists(saveAs))
                {
                    File.Delete(saveAs);
                }

                return bytes;

            }
            catch (Exception)
            {

            }
            return new byte[0];
        }
        #endregion

        #region Stats Disagiate
        public static void StatsDisagiate()
        {
            GnXcmEntities entity = new GnXcmEntities();
            var fromDate = new DateTime(2022, 09, 01);
            var toDate = new DateTime(2022, 10, 01);
            var lists = File.ReadAllLines(@"C:\UNITEX\CapDisagiati.txt").ToList();
            var list = lists.Distinct().ToList();

            var documents = entity.uvwWmsDocument.Where(x => x.DocTip == 204 && x.RecCreate >= fromDate && x.RecCreate <= toDate).ToList();

            int count = 0;
            List<uvwWmsDocument> docs = new List<uvwWmsDocument>();
            foreach (var cap in list)
            {
                count += documents.Where(x => x.ConsigneeZipCode == cap).Count();
                docs.AddRange(documents.Where(x => x.ConsigneeZipCode == cap).ToList());

            }

            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];

            int i = 1;

            worksheet.Cells[$"A{i}"].Value = "Riferimento";
            worksheet.Cells[$"B{i}"].Value = "Data";
            worksheet.Cells[$"C{i}"].Value = "Corrispondente";
            worksheet.Cells[$"D{i}"].Value = "Item Info";
            worksheet.Cells[$"E{i}"].Value = "Destinatario";
            worksheet.Cells[$"F{i}"].Value = "Indirizzo";
            worksheet.Cells[$"G{i}"].Value = "Località";
            worksheet.Cells[$"H{i}"].Value = "Cap";
            worksheet.Cells[$"I{i}"].Value = "Cliente";
            worksheet.Cells[$"J{i}"].Value = "Uniq";
            i++;
            foreach (var doc in docs)
            {
                worksheet.Cells[$"A{i}"].Value = doc.DocNum;
                worksheet.Cells[$"B{i}"].Value = doc.DocDta;
                worksheet.Cells[$"C{i}"].Value = doc.ShipUnLoadCarrierDes;
                worksheet.Cells[$"D{i}"].Value = doc.ItemInfo;
                worksheet.Cells[$"E{i}"].Value = doc.ConsigneeName;
                worksheet.Cells[$"F{i}"].Value = doc.ConsigneeAddress;
                worksheet.Cells[$"G{i}"].Value = doc.ConsigneeLocation;
                worksheet.Cells[$"H{i}"].Value = doc.ConsigneeZipCode;
                worksheet.Cells[$"I{i}"].Value = doc.CustomerDes;

                i++;
            }

            workbook.SaveDocument(@"C:\UNITEX\StatsDisagiate_Settembre.xlsx", DocumentFormat.Xlsx);
        }
        #endregion
    }
}


