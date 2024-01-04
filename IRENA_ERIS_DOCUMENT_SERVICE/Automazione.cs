using CommonAPITypes.XCM;
using CommonTypes.XCM;
using FluentFTP;
using IRENA_ERIS_DOCUMENT_SERVICE.Data;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using System.Xml.Serialization;
using static CommonAPITypes.ESPRITEC.EspritecDocuments;

namespace IRENA_ERIS_DOCUMENT_SERVICE
{
    public class Automazione
    {
        public static string endpointAPI_Espritec = "https://192.168.2.254:9500";

        DateTime DataScadenzaToken_Irena = DateTime.Now;
        string tokenIrena = "";
        string PathLastCheckChangesFileWMS = @"LastCheckChangesWMSIrena.txt";

        CommonTypes.XCM.CustomerSpec IrenaCUST = CustomersXCM.IRENAERIS;

        Exception LastException = new Exception("AVVIO");
        DateTime DateLastException = DateTime.MinValue;
        DateTime LastCheckChangesWMS = DateTime.MinValue;

        System.Timers.Timer timerAggiornamentoCiclo = new System.Timers.Timer();

        //List<RootobjectOrder> DaComunicare = new List<RootobjectOrder>();
        #region Logger
        internal static Logger _loggerCode = LogManager.GetLogger("loggerCode");
        internal static Logger _loggerAPI = LogManager.GetLogger("LogAPI");
        #endregion

        private void SetTimer()
        {
            timerAggiornamentoCiclo = new System.Timers.Timer(600000);
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
                //recupera file FTP 
                _loggerCode.Info("Creo connessione FTP");
                var IrenaFTPClient = CreaClientFTPperIlCliente(IrenaCUST);
                if (IrenaFTPClient != null)
                {
                    _loggerCode.Info("Connessione FTP creata");
                    var OrdiniDaProcessare = IrenaFTPClient.GetListing("/OUT").Where(x => x.Type == FtpFileSystemObjectType.File).ToList();

                    _loggerCode.Info($"Ho trovato {OrdiniDaProcessare.Count()} ordini da scaricare");

                    int i = 1;

                    foreach (var daScaricare in OrdiniDaProcessare)
                    {
                        _loggerCode.Info($"Scarico il file {i} - {OrdiniDaProcessare.Count()}");

                        var dest = Path.Combine(IrenaCUST.LocalInFilePath, daScaricare.Name);
                        IrenaFTPClient.DownloadFile(dest, daScaricare.FullName, FtpLocalExists.Overwrite, FtpVerify.OnlyChecksum);
                        IrenaFTPClient.MoveFile($"{daScaricare.FullName}", $"{IrenaCUST.RemoteINPath}/Archive/{Path.GetFileName(daScaricare.FullName)}");

                        i++;
                    }

                    _loggerCode.Info($"Ho scaricato tutti i file");

                    //Messo per sicurezza, il GB dovrebbe ripulire in automatico tutto l'oggetto in assenza di questa chiamata
                    OrdiniDaProcessare.Clear();
                }
                else
                {
                    _loggerCode.Error("Errore FTP");
                    GestoreMail.SegnalaErroreDevRob("Errore FTP IRENA", new Exception("Non sono riuscito a connettermi all'FTP di IRENA"));
                }

                ConvertiOridiniRecuperati(); 
                RecuperaConnessione();
                RecuperaLastCheckChangesWMS();
                CambiWMS();
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDevRob("Errore in IRENA ERIS", ee);
                }
                LastException = ee;
            }
            finally
            {
                timerAggiornamentoCiclo.Start();
            }

        }

        private FtpClient CreaClientFTPperIlCliente(CommonTypes.XCM.CustomerSpec cust)
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
                    GestoreMail.SegnalaErroreDevRob("CreaClientFTPperIlCliente", ee);
                }

                LastException = ee;
                return null;
            }
        }



        //TODO: Claudio Recupero IrenaEris 

        private void RecuperoCambiWMSdaFileReference(DateTime fromTimestamp)
        {
            var pageNumber = 1;
            bool Cicla = true;
            RestClient client = null, client2 = null;
            IRestResponse response, response2;
            RootobjectTrackingDocument dtt;
            List<TrackingDocument> DocTrack;
            var request = new RestRequest(Method.GET);
            int index, Contatore, docId, docIdOld = -1;
            try
            {
                string ts = (fromTimestamp).ToString("s", CultureInfo.InvariantCulture);
                var daRecuperare = File.ReadAllLines("RecuperaReference.txt");
                request.AddHeader("Authorization", $"Bearer {tokenIrena}");
                while (Cicla)
                {
                    Console.WriteLine("PageNumber = " + pageNumber.ToString());
                    Console.Write("---");
                    Console.Write(" ");
                    client = new RestClient(endpointAPI_Espritec + $"/api/wms/document/tracking/changes/500/{pageNumber}?FromTimeStamp={ts}");
                    client.Timeout = -1;
                    response = client.Execute(request);
                    dtt = JsonConvert.DeserializeObject<RootobjectTrackingDocument>(response.Content);
                    if (dtt.trackings == null) return;
                    DocTrack = dtt.trackings.ToList();
                    docId = DocTrack[DocTrack.Count - 1].docID;
                    if (docId != docIdOld) docIdOld = docId;
                    else
                    {
                        Console.WriteLine("DOC NON RECUPERATI");
                        Console.WriteLine("-------------------------------");
                        Array.ForEach(daRecuperare, Console.WriteLine);
                        Console.WriteLine("-------------------------------");
                        break;
                    }
                    Contatore = 1;

                    int CurTop = Console.CursorTop;
                    foreach (var dt in DocTrack)
                    {
                        client2 = new RestClient(endpointAPI_Espritec + $"/api/wms/document/get/{dt.docID}");
                        client2.Timeout = -1;
                        response2 = client2.Execute(request);
                        RootobjectOrder docOsservato = JsonConvert.DeserializeObject<RootobjectOrder>(response2.Content);
                        index = Array.FindIndex(daRecuperare, s => s.StartsWith(docOsservato.header.reference, StringComparison.OrdinalIgnoreCase));

                        Console.SetCursorPosition(0, CurTop);
                        Console.Write(Contatore.ToString() + "/" + DocTrack.Count.ToString() +" - ");
                                             
                        if (index >= 0)
                        {
                            if (docOsservato != null && docOsservato.header == null) continue;
                            if (docOsservato.links != null) 
                                Console.WriteLine("Reference = " + docOsservato.header.reference + " DOC Number= " + docOsservato.header.docNumber + " N°Links  = " + docOsservato.links.Length.ToString());
                            var NomeFileOUT = $"{DateTime.Now:yyyyMMddHHmmss}_Inbound_{docOsservato.header.reference2}";
                            daRecuperare = daRecuperare.Where(o => o != docOsservato.header.reference).ToArray();
                            ComunicaEvasioneOrdine(docOsservato, NomeFileOUT);
                            System.Threading.Thread.Sleep(2000);
                            CurTop = Console.CursorTop;
                            if (daRecuperare.Length <= 0) { 
                                Cicla = false;
                                Console.WriteLine("Recupero completato");
                                break; 
                            }
                        }
                        Contatore++;
                    }
                    pageNumber++;
                }
            }
            catch(Exception ex)
            {
                Console.Write("Errore: " + ex.Message.ToString());
            }
        }
        private void RecuperoDaFile(DateTime dataStart)
        {
            RecuperaConnessione();
            RecuperoCambiWMSdaFileReference(dataStart);
        }

        private void CambiWMS()
        {
            _loggerCode.Info("Recupero Cambi WMS da API");

            RestClient client = null;

            //RecuperaLastCheckChangesWMS();
            string ts = LastCheckChangesWMS.ToString("s", CultureInfo.InvariantCulture);

            client = new RestClient(endpointAPI_Espritec + $"/api/wms/document/tracking/changes/500/1?FromTimeStamp={ts}");

            var dtn = DateTime.Now;
            LastCheckChangesWMS = new DateTime(dtn.Year, dtn.Month, dtn.Day, dtn.Hour, dtn.Minute, 00);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {tokenIrena}");
            IRestResponse response = client.Execute(request);


            //Utilizzo .Where per ridurre il numero di chiamate REST da fare più tardi a riga 105
            //altrimenti dovrei scansionarmi ogni singolo doc in maniera indipendente


            try
            {
                var dtt = JsonConvert.DeserializeObject<RootobjectTrackingDocument>(response.Content);
                if (dtt.trackings == null) 
                {
                    File.WriteAllText(PathLastCheckChangesFileWMS, LastCheckChangesWMS.ToString());
                    _loggerCode.Info($"Nessun cambio rilevato da {ts}, LCCDateTime adesso è {LastCheckChangesWMS}"); 
                    return; 
                }

                //var DocTrack = dtt.trackings.Where(x => x.statusID == 20 && x.doctype == "DeliveryOUT").ToList();
                var DocTrack = dtt.trackings.ToList();

                if (DocTrack.Count() > 0)
                {
                    var dttt = DocTrack;
                    _loggerCode.Info($"Trovati {dttt.Count()} cambi");
                    foreach (var dt in dttt)
                    {
                        try
                        {

                            _loggerCode.Info($"Analisi del documento {dt.docNumber}");

                            if (dt.docNumber.EndsWith("/INV"))
                            {
                                continue;
                            }

                            
                            var client2 = new RestClient(endpointAPI_Espritec + $"/api/wms/document/get/{dt.docID}");
                            client2.Timeout = -1;
                            var request2 = new RestRequest(Method.GET);
                            request2.AddHeader("Authorization", $"Bearer {tokenIrena}");
                            IRestResponse response2 = client2.Execute(request2);
                            var docOsservato = JsonConvert.DeserializeObject<RootobjectOrder>(response2.Content);

                            if (docOsservato != null && docOsservato.header == null) continue;//??????? errire da gestire                          

                            //ID IRENA ERIS = 00031
                            //In caso di confusione riferirsi a riga 99 a 101

                            //Il documento è pronto e và comunicato
                            _loggerCode.Info($"Il documento è pronto e va comunicato");
                            var NomeFileOUT = $"{DateTime.Now:yyyyMMddHHmmss}_Inbound_{docOsservato.header.reference2}";
                            ComunicaEvasioneOrdine(docOsservato, NomeFileOUT);

                            //VerificaCambiamentoDocWMS(docOsservato);
                            //VerificaSincronizzazioneCRMOrdini(docOsservato); //se ne occupa la sincronizzazione notturna
                        }
                        catch (Exception ee)
                        {
                            string msg = $"Errore durante l'analisi del documento {dt.docNumber}";
                            _loggerCode.Error(msg);
                            _loggerCode.Error(ee);
                            GestoreMail.SegnalaErroreDevRob(msg, ee);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee.Message);
                return;
            }


            File.WriteAllText(PathLastCheckChangesFileWMS, LastCheckChangesWMS.ToString());
        }

        private void ComunicaEvasioneOrdine(RootobjectOrder Doc, string fn)
        {
            try
            {
                //per ogni documento da comunicare convertilo in oggetto IRENA

                IConfOrder OutOrder = new IConfOrder();
                IConfOrderHead OutOrderHead = new IConfOrderHead();

                //Prendi le informazioni di Head
                #region Testata
                OutOrderHead.OPERATION = Doc.header.info2;
                OutOrderHead.STORE_ORD_NR = Doc.header.reference2;
                OutOrderHead.ORD_NR = Doc.header.reference;
                OutOrderHead.ORDER_TYPE = Doc.header.info5;//mappa!!
                OutOrderHead.M3_ORDER_TYPE = Doc.header.info3;

                OutOrder.IConfOrderHead = OutOrderHead;
                #endregion

                //api/wms/document/row/list/docId
                _loggerCode.Info($"Recupero Rows del Documento {Doc.header.docNumber}");

                #region API
                RestClient client = null;
                client = new RestClient(endpointAPI_Espritec + $"/api/wms/document/row/list/{Doc.header.id}");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", $"Bearer {tokenIrena}");
                IRestResponse response = client.Execute(request);
                var RootObjRows = JsonConvert.DeserializeObject<RootobjectEspritecRows>(response.Content);
                #endregion

                OutOrder.Pos = new IConfOrderPos[0];
                var OutOrderPosList = OutOrder.Pos.ToList();
                //foreach row in Document
                #region RecuperaRigheDoc
                foreach (var row in RootObjRows.rows)
                {
                    //Converto a list e poi ritorno ad array solo per semplicità di lettura
                    //Altrimenti for (int i = 0; i < RootObjRows.rows.Count(); i++)
                    //Crea nuovo oggetto e aggiungi all'array di OutOrderPos
                    IConfOrderPos OutOrderPos = new IConfOrderPos();

                    OutOrderPos.SNPOS_NR = row.info1;
                    OutOrderPos.SKU = row.partNumber;
                    OutOrderPos.PROD_NAME1 = string.IsNullOrEmpty(row.partNumberDes) ? $"{row.partNumber} - Generic Description" : row.partNumberDes;
                    OutOrderPos.NUMBER = ((int)row.qty).ToString();
                    OutOrderPos.MEAS_UNIT = "szt";
                    OutOrderPos.SERIA = row.batchNo;
                    OutOrderPos.VALID_DATE = row.expireDate.Value.ToString("yyyyMMdd");
                    OutOrderPos.PROD_DATE = "";
                    OutOrderPos.LOG_STORE1 = row.info2;
                    OutOrderPos.LOG_STORE2 = row.info3;
                    OutOrderPos.SHIP_DATE = "";

                    OutOrderPosList.Add(OutOrderPos);
                }
                #endregion
                OutOrder.Pos = OutOrderPosList.ToArray();
                var dest = Path.Combine(IrenaCUST.LocalWorkPath, fn);

                //TEST
                //XmlWriterSettings palle = new XmlWriterSettings
                //{
                //	Encoding = Encoding.UTF8,
                //	NamespaceHandling = NamespaceHandling.OmitDuplicates,
                //};
                //XmlSerializer serializer2 = new XmlSerializer(OutOrder.GetType(), "");
                //XmlSerializerNamespaces nms = new XmlSerializerNamespaces();
                //nms.Add("","");
                //dest = dest + ".xml"; 
                //serializer2.Serialize(XmlWriter.Create(dest, palle), OutOrder, nms, "");
                //TEST


                //using (StreamWriter writer = new StreamWriter(dest + ".xml", false, Encoding.UTF8))
                //{
                //    XmlSerializer serializer = new XmlSerializer(OutOrder.GetType(), "");
                //    serializer.Serialize(writer, OutOrder, null);
                //}

                //var xml = "";
                //using (var sww = new StringWriter())
                //{
                //	using (XmlTextWriter writer = XmlTextWriter.sww)
                //	{
                //		xsSubmit.Serialize(writer, OutOrder);
                //		writer.Formatting = System.Xml.Formatting.Indented;
                //		xml = sww.ToString(); // Your XML

                //		File.WriteAllText(dest, xml);
                //		_loggerCode.Info($"Nuovo file XML creato per l'ordine {OutOrder.IConfOrderHead.ORD_NR}");
                //	}
                //}

                //-------------------------------------------------------------------------
                using (var sw = new Utf8StringWriter())
                using (var xw = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true }))
                {
                    xw.WriteStartDocument(true); // that bool parameter is called "standalone"

                    var namespaces = new XmlSerializerNamespaces();
                    namespaces.Add(string.Empty, string.Empty);

                    var xmlSerializer = new XmlSerializer(typeof(IConfOrder));
                    xmlSerializer.Serialize(xw, OutOrder);
                    var tt = sw.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    tt[0] = "<?xml version=\"1.0\" standalone=\"yes\"?>";
                    tt[1] = "<IConfOrder>";
                    string resp = "";
                    foreach (var t in tt)
                    {
                        resp += t + "\r\n";
                    }

                    Console.WriteLine(resp);
                    dest = dest + ".xml";
                    File.WriteAllText(dest, resp);
                }

                //Invia gli XML uno ad uno
                //Mandare file su FTP


     
                var ftp = CreaClientFTPperIlCliente(IrenaCUST);

                ftp.UploadFile(dest, Path.Combine(IrenaCUST.RemoteOUTPath, fn + ".xml"));

                _loggerCode.Info($"File {fn} caricato con successo su FTP");
                File.Move(dest, Path.Combine(IrenaCUST.LocalWorkPath, "Archivio\\" + fn + ".xml"));
                
            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);
                var orig = Path.Combine(IrenaCUST.LocalWorkPath, fn);
                var destERR = Path.Combine(IrenaCUST.LocalErrorFilePath, fn);
                File.Move(orig, destERR);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDevRob($"IrenaComunicaEvasioneOrdine-{Doc.header.docNumber}-{Doc.header.id}", ee);
                }
                LastException = ee;
            }
        }

        public sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }

        private void RecuperaLastCheckChangesWMS()
        {
            LastCheckChangesWMS = DateTime.Parse(File.ReadAllLines(PathLastCheckChangesFileWMS)[0]);
            _loggerCode.Info($"L'ultimo check effettuato per cambiamenti nel WMS è avvenuto il {LastCheckChangesWMS}");
        }

        private void RecuperaConnessione()
        {
            if ((DateTime.Now + TimeSpan.FromHours(1)) > DataScadenzaToken_Irena)
            {
                XcmLogin();
            }
        }

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
                $@"  ""username"": ""{IrenaCUST.userAPI}""," + "\n" +
                $@"  ""password"": ""{IrenaCUST.pswAPI}""," + "\n" +
                @"  ""tenant"": """"" + "\n" +
                @"}";
                request.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse response = client.Execute(request);
                var resp = JsonConvert.DeserializeObject<XCMTypes>(response.Content);

                DataScadenzaToken_Irena = resp.user.expire;
                tokenIrena = resp.user.token;

                _loggerAPI.Info($"Nuovo token XCM: {tokenIrena}");

            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDevRob("XcmLogin", ee);
                }
                LastException = ee;
            }
        }

        //private void ConvertiEdInviaOrdiniLavorati()
        //{
        //	//per ogni documento da comunicare convertilo in oggetto IRENA
        //	var DaXMLAre = new List<IConfOrder>();

        //	IConfOrder OutOrder = new IConfOrder();
        //	IConfOrderHead OutOrderHead = new IConfOrderHead();

        //	foreach (var Doc in DaComunicare)
        //	{
        //		//Prendi le informazioni di Head
        //		OutOrderHead.OPERATION = Doc.header.info2;
        //		OutOrderHead.STORE_ORD_NR = Doc.header.reference2;
        //		OutOrderHead.ORD_NR = Doc.header.reference;
        //		OutOrderHead.ORDER_TYPE = Doc.header.regTypeID;
        //		OutOrderHead.M3_ORDER_TYPE = Doc.header.info3;

        //		OutOrder.IConfOrderHead = OutOrderHead;

        //		//api/wms/document/row/list/docId
        //		_loggerCode.Info($"Recupero Rows del Documento {Doc.header.docNumber}");

        //		RestClient client = null;
        //		client = new RestClient(endpointAPI_Espritec + $"/api/wms/document/row/list/{Doc.header.id}");
        //		client.Timeout = -1;
        //		var request = new RestRequest(Method.GET);
        //		request.AddHeader("Authorization", $"Bearer {tokenIrena}");
        //		IRestResponse response = client.Execute(request);
        //		var RootObjRows = JsonConvert.DeserializeObject<RootobjectEspritecRows>(response.Content);

        //		//foreach row in Document
        //		foreach (var row in RootObjRows.rows)
        //		{
        //			//Converto a list e poi ritorno ad array solo per semplicità di lettura
        //			//Altrimenti for (int i = 0; i < RootObjRows.rows.Count(); i++)

        //			//Crea nuovo oggetto e aggiungi all'array di OutOrderPos
        //			var OutOrderPosList = OutOrder.Pos.ToList();
        //			IConfOrderPos OutOrderPos = new IConfOrderPos();

        //			OutOrderPos.SNPOS_NR = row.info1;
        //			OutOrderPos.SKU = row.id.ToString();
        //			OutOrderPos.PROD_NAME1 = row.partNumberDes;
        //			OutOrderPos.NUMBER = row.qty.ToString();
        //			OutOrderPos.MEAS_UNIT = row.um;
        //			OutOrderPos.SERIA = row.batchNo;
        //			OutOrderPos.VALID_DATE = row.expireDate.ToString();
        //			OutOrderPos.PROD_DATE = "";
        //			OutOrderPos.LOG_STORE1 = row.info2;
        //			OutOrderPos.LOG_STORE2 = row.info3;
        //			OutOrderPos.SHIP_DATE = "";

        //			OutOrderPosList.Add(OutOrderPos);

        //			var OutOrderPosArray = OutOrderPosList.ToArray();
        //			OutOrder.Pos = OutOrderPosArray;

        //		}

        //		DaXMLAre.Add(OutOrder);
        //	}
        //	foreach (var item in DaXMLAre)
        //	{
        //		XmlSerializer xsSubmit = new XmlSerializer(item.GetType());
        //		var xml = "";

        //		using (var sww = new StringWriter())
        //		{
        //			using (XmlWriter writer = XmlWriter.Create(sww))
        //			{
        //				xsSubmit.Serialize(writer, item);
        //				xml = sww.ToString(); // Your XML

        //				//Scrivi l'oggetto irena (XML)
        //				File.WriteAllText(PathOutXMLFile + $"{DateTime.Now:yyyyMMddHHmmss}_Inbound_{item.IConfOrderHead.STORE_ORD_NR}", xml);
        //				_loggerCode.Info($"Nuovo file XML creato per l'ordine {item.IConfOrderHead.ORD_NR}");
        //			}
        //		}

        //		//Invia gli XML uno ad uno
        //		//Mandare file su FTP
        //	}


        //}

        string RecuperaProvincia(string FR_POST_CODE)
        {
            //Recupera provincia da DB XCM, dbo.GEO_IT, Entity Framework
            using (var db = new XCM_WMSEntities())
            {
                try
                {
                    string Provincia = db.GEO_IT.First(x => x.CAP == FR_POST_CODE).PROVINCIA;
                    return Provincia;
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        //List<XCMCSVRow> dbgRor = new List<XCMCSVRow>();
        private void ConvertiOridiniRecuperati()
        {
            var Files = Directory.GetFiles(IrenaCUST.LocalInFilePath).Where(x => x.ToLower().EndsWith(".xml")).ToList();

            _loggerCode.Info($"Recuperati {Files.Count()} file da lavorare");
            List<string> FileVuoti = new List<string>();
            foreach (var fl in Files)
            {
                //Se il file è vuoto manda mail a CustomerServiceXCM e IT support IRENA
                var isVuoto = File.ReadAllLines(fl).Length == 0;
                if (isVuoto)
                {
                    _loggerCode.Info($"Il file è vuoto, mando una mail al customer");
                    FileVuoti.Add(fl);
                    //GestoreMail.SegnalaAlCustomerCustom("Irena ERIS ha mandato un XML vuoto", $"Il file {Path.GetFileName(fl)} mandato da Irena ERIS risulta vuoto.");
                }
                else
                {
                    var justFN = Path.GetFileName(fl);
                    var WorkPath = Path.Combine(IrenaCUST.LocalWorkPath, justFN);
                    File.Move(fl, WorkPath);

                    _loggerCode.Info($"Lavoro il file {justFN}, TryCatch riga 548.");

                    try
                    {
                        XmlSerializer serializerIN = new XmlSerializer(typeof(IOrder));
                        //Prendi gli oggetti dal Serializer
                        IOrder orderIN;
                        using (StreamReader readerIN = new StreamReader(WorkPath))
                        {
                            //Deserializzalo in un oggetto conosciuto
                            orderIN = (IOrder)serializerIN.Deserialize(readerIN);
                        }

                        var HDR = orderIN.IOrdHead;
                        var XCMCSVList = new List<string>();

                        XCMCSVHeader Header = new XCMCSVHeader()
                        {
                            SegmentName = "HDR",
                            Reference = HDR.ORD_NR.ToString(),
                            Reference2 = HDR.STORE_ORD_NR,
                            HeaderInfo2 = HDR.OPERATION,
                            RegTypeID = HDR.ORDER_TYPE,
                            HeaderInfo3 = HDR.M3_ORDER_TYPE,
                            HeaderInfo4 = HDR.FR_CODE,
                            UnloadName = $"{HDR.FR_NAME1} {HDR.FR_NAME2}",
                            UnloadZipCode = HDR.FR_POST_CODE,
                            UnloadLocation = HDR.FR_CITY,
                            UnloadAddress = HDR.FR_STREET,
                            UnloadCountry = HDR.FR_COUNTRY,
                            UnloadDistrict = string.IsNullOrEmpty(HDR.FR_POST_CODE) ? RecuperaProvincia(HDR.FR_POST_CODE) : HDR.FR_POST_CODE,
                            //RefDta = HDR.DEPART_DFROM,
                            RefDta = DateTime.Now.ToString("yyyyMMdd"),
                            RefDta2 = HDR.DEPART_DTO,
                            HeaderInfo5 = HDR.ORDER_TYPE
                        };

                        XCMCSVList.Add(Header.ToString());

                        foreach (var Row in orderIN.Pos)
                        {
                            //Aggiungi riga al flusso

                            XCMCSVRow ROW = new XCMCSVRow()
                            {
                                RowInfo1 = Row.SNPOS_NR.ToString(),
                                PrdCod = Row.SKU,
                                Qty = Row.NUMBER.ToString(),
                                Batchno = Row.SERIA.ToString(),
                                DateExpire = Row.VALID_DATE,
                                DateProd = Row.PROD_DATE,
                                RowInfo2 = Row.LOG_STORE1,
                                RowInfo3 = Row.LOG_STORE2
                            };

                            XCMCSVList.Add(ROW.ToString());
                            //dbgRor.Add(ROW);
                        }

                        //Scrivi file nella cartella 
                        File.WriteAllLines(IrenaCUST.LocalInFilePath + $"\\CSV\\{Path.GetFileNameWithoutExtension(justFN)}.csv", XCMCSVList);
                        _loggerCode.Info($"Il file CSV 'File'{justFN} è stato scritto per il documento: {orderIN.MessageId}");
                        var dest = Path.Combine(IrenaCUST.LocalBackupPath, justFN);
                        File.Move(WorkPath, dest);
                        _loggerCode.Info($"Muovo il file XML in \\Originali");
                    }
                    catch (Exception ee)
                    {

                        _loggerCode.Error(ee);

                        if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                        {
                            DateLastException = DateTime.Now;
                            GestoreMail.SegnalaErroreDevRob("OnTimedEvent", ee);
                        }
                        LastException = ee;
                    }
                }
            }
            if (FileVuoti.Count > 0)
            {
                string resp = "";
                foreach (var fv in FileVuoti)
                {
                    resp += fv + "/r/n";
                }
                GestoreMail.SegnalaAlCustomerCustom("IRENA ERIS File Vuoti", $"Sono stati rilevati i seguenti file vuoti:\r\n{resp}");

            }
        }

        private void CheckPath()
        {
            if (!Directory.Exists(IrenaCUST.LocalInFilePath))
            {
                Directory.CreateDirectory(IrenaCUST.LocalInFilePath);
            }

            if (!Directory.Exists(IrenaCUST.LocalOUTFilePath))
            {
                Directory.CreateDirectory(IrenaCUST.LocalOUTFilePath);
            }

            if (!Directory.Exists(IrenaCUST.LocalErrorFilePath))
            {
                Directory.CreateDirectory(IrenaCUST.LocalErrorFilePath);
            }
            if (!Directory.Exists(IrenaCUST.LocalWorkPath))
            {
                Directory.CreateDirectory(IrenaCUST.LocalWorkPath);
            }
            if (!Directory.Exists(IrenaCUST.LocalBackupPath))
            {
                Directory.CreateDirectory(IrenaCUST.LocalBackupPath);
            }

        }

        public void Start()
        {
            CheckPath();

            //TODO: Claudio creata funzione per recuperare eventi da Riferimento 
            //RecuperoDaFile(new DateTime(2023, 10, 01));

            SetTimer();
            OnTimedEvent(null, null);
        }


        public void Stop()
        {
            timerAggiornamentoCiclo.Stop();
        }
    }
}
