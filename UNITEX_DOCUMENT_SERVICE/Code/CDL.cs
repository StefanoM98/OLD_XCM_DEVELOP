using iAnywhere.Data.SQLAnywhere;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UNITEX_DOCUMENT_SERVICE.EF;
using UNITEX_DOCUMENT_SERVICE.Model;
using UNITEX_DOCUMENT_SERVICE.Model.CDL;

namespace UNITEX_DOCUMENT_SERVICE.Code
{
    public class CDL
    {
        #region OLD
        public void Test()
        {
            var connectionString = "UID=cdl_xcm;PWD=unitex;HOST=213.92.99.42:2639";
            var db = new CDLEntities();
            DataTable dt = new DataTable();
            var datamin = new DateTime(1900, 01, 01);
            using (var _conn = new SAConnection(connectionString))
            {
                _conn.Open();
                //using (SADataAdapter da = new SADataAdapter("SELECT * FROM dba.vs_bolle_sat where data_ora_inserimento >= '2022-09-30'", _conn))
                using (var da = new SADataAdapter("SELECT * FROM dba.vs_bolle_sat where data_ora_inserimento > '2022-10-13' AND data_ora_inserimento < '2022-10-18'", _conn))
                {
                    da.Fill(dt);
                    _conn.Close();
                }
            }

            var daAggiungere = new List<va_bolle_sat>();

            using (var con = new SqlConnection("Data Source=192.168.1.145;uid=sa_gespe;pwd=gespe;database=CDL"))
            {
                con.Open();
                using (SqlBulkCopy bulkcopy = new SqlBulkCopy(con))
                {
                    bulkcopy.DestinationTableName = "va_bolle_sat";

                    try
                    {
                        //var dtC = dt.Clone()
                        //prendere valore cella id 
                        System.Diagnostics.Debug.WriteLine($"-------------------------------------------------------------------------------------------------------------------------------trovati {dt.Rows.Count} record");
                        foreach (DataRow dr in dt.Rows)
                        {

                            var mio = DataRowConverter.ToObject<va_bolle_sat>(dr);
                            if (mio != null)
                            {
                                var esiste = db.va_bolle_sat.FirstOrDefault(x => x.id_bol_trasporto == mio.id_bol_trasporto);
                                if (esiste != null)
                                {
                                    dr.Delete();
                                }
                                else if (mio.data_cons_richiesta != null && mio.data_cons_richiesta < datamin)
                                {
                                    dr.Delete();
                                }
                                else
                                {

                                }
                                //daAggiungere.Add(mio);
                            }

                            else
                            {

                            }

                        }
                        bulkcopy.WriteToServer(dt);
                        con.Close();

                        //var pulita = daAggiungere.Where(x => x.data_appuntamento < datamin || x.data_attesa_pren < datamin || x.data_cons_richiesta < datamin || x.data_cons_tassativa < datamin
                        //|| x.data_ddt < datamin || x.data_dist_cliente < datamin || x.data_ora_inserimento < datamin || x.data_ora_ult_variazione < datamin || x.data_pronto_merce < datamin
                        //|| x.data_spedizione < datamin).ToList();
                        //if(pulita.Count > 0)
                        //{
                        //    foreach (var p in pulita)
                        //    {
                        //        daAggiungere.First(x => x.id_bol_trasporto == p.id_bol_trasporto).data_appuntamento = new DateTime(1910, 01, 01);
                        //    }
                        //}
                        //db.va_bolle_sat.AddRange(daAggiungere);
                        //db.SaveChanges();
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            //var connectionString = "UID=cdl_xcm;PWD=unitex;HOST=213.92.99.42:2639";
            //var db = new CDLEntities();
            //SAConnection _conn = new SAConnection(connectionString);

            //try
            //{

            //    _conn.Open();

            //    //SACommand cmd = new SACommand("SELECT * FROM dba.vs_bolle_sat", _conn);
            //    //SACommand cmd = new SACommand("SELECT * FROM dba.vs_bolle_sat where data_ora_inserimento > '2022-04-01'", _conn);


            //    //SADataReader reader = cmd.ExecuteReader();
            //    SADataAdapter da = new SADataAdapter("SELECT * FROM dba.vs_bolle_sat where data_ora_inserimento > '2022-05-18'", _conn);
            //    DataTable dt = new DataTable();
            //    da.Fill(dt);

            //    SqlConnection con = new SqlConnection("Data Source=192.168.1.145;uid=sa_gespe;pwd=gespe;database=CDL");
            //    con.Open();
            //    SqlDataAdapter sqlu = new SqlDataAdapter("Select * from va_bolle_sat", con);
            //    var builder = new SqlCommandBuilder(sqlu);

            //    sqlu.InsertCommand = builder.GetInsertCommand();
            //    sqlu.Update(dt);
            //    con.Close();




            //    List<va_bolle_sat> bolle = CreaOggettoCDLBolla(null);
            //    db.va_bolle_sat.AddRange(bolle);
            //    db.SaveChanges();

            //    //listEmployees.EndUpdate();
            //    //reader.Close();
            //    _conn.Close();

            //}
            //catch (SAException ex)
            //{
            //    Console.WriteLine(ex.Errors[0].Source + " : "
            //         + ex.Errors[0].Message + " (" +
            //         ex.Errors[0].NativeError.ToString() + ")",
            //         "Failed to connect");
            //}
            //catch (Exception ex2)
            //{

            //}
            //finally
            //{
            //    _conn.Close();
            //}
        }

        private List<va_bolle_sat> CreaOggettoCDLBolla(SADataReader reader)
        {
            List<va_bolle_sat> listBolle = new List<va_bolle_sat>();
            CDLEntities cdlDB = new CDLEntities();
            var t = 0;
            try
            {
                while (reader.Read())
                {

                    Object[] item = new Object[79];
                    reader.GetValues(item);


                    var ii = new va_bolle_sat();
                    var i = 0;

                    ii.id_bol_trasporto = long.Parse(item.GetValue(i).ToString());
                    ii.id_esercizio = checkValueCDL(item.GetValue(++i)) ? long.Parse(item.GetValue(i).ToString()) : 0;
                    ii.id_filiale = checkValueCDL(item.GetValue(++i)) ? long.Parse(item.GetValue(i).ToString()) : 0;
                    ii.cod_esercizio = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.cod_filiale = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.prog_spedizione = checkValueCDL(item.GetValue(++i)) ? long.Parse(item.GetValue(i).ToString()) : 0;
                    ii.data_spedizione = checkValueCDL(item.GetValue(++i)) ? DateTime.Parse(item.GetValue(i).ToString()).Date : new DateTime(1950, 01, 01).Date;
                    ii.espresso = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.rag_soc_mittente = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.ind_mittente = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.cap_mittente = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.citta_mittente = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.prov_mittente = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.citta_sede_mittente = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.rag_soc_destinatario = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.ind_destinatario = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.cap_destinatario = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.citta_destinatario = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.prov_destinatario = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.citta_sede_destinatario = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.id_sog_com_fatturazione = checkValueCDL(item.GetValue(++i)) ? long.Parse(item.GetValue(i).ToString()) : 0;
                    ii.cod_cliente = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.rag_cliente = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.tipo_consegna = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.num_ddt = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.data_ddt = checkValueCDL(item.GetValue(++i)) ? DateTime.Parse(item.GetValue(i).ToString()) : new DateTime(1950, 01, 01);
                    ii.altro_riferimento = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.num_colli = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.peso_effettivo = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.metri_cubi = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.mis_colli = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.note_bolla = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.data_cons_tassativa = checkValueCDL(item.GetValue(++i)) ? DateTime.Parse(item.GetValue(i).ToString()) : new DateTime(1950, 01, 01);
                    //ii.data_appuntamento = checkValueCDL(item.GetValue(++i)) ? DateTime.Parse(item.GetValue(i).ToString()) : new DateTime(1950, 01, 01);
                    ii.num_dist_cliente = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.data_dist_cliente = checkValueCDL(item.GetValue(++i)) ? DateTime.Parse(item.GetValue(i).ToString()) : new DateTime(1950, 01, 01);
                    ii.data_cons_richiesta = checkValueCDL(item.GetValue(++i)) ? DateTime.Parse(item.GetValue(i).ToString()) : new DateTime(1950, 01, 01);
                    ii.note_web = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.bancali = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.tipo_data_tassativa = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.attesa_pren = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.data_attesa_pren = checkValueCDL(item.GetValue(++i)) ? DateTime.Parse(item.GetValue(i).ToString()) : new DateTime(1950, 01, 01);
                    ii.id_loc_provenienza = checkValueCDL(item.GetValue(++i)) ? long.Parse(item.GetValue(i).ToString()) : 0;
                    ii.id_loc_destinazione = checkValueCDL(item.GetValue(++i)) ? long.Parse(item.GetValue(i).ToString()) : 0;
                    ii.des_contenuto = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.peso_volumizzato = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.contrassegno = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.anticipate = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.id_tab_iva = checkValueCDL(item.GetValue(++i)) ? long.Parse(item.GetValue(i).ToString()) : 0;
                    ii.data_ora_inserimento = checkValueCDL(item.GetValue(++i)) ? DateTime.Parse(item.GetValue(i).ToString()) : new DateTime(1950, 01, 01);
                    ii.data_ora_ult_variazione = checkValueCDL(item.GetValue(++i)) ? DateTime.Parse(item.GetValue(i).ToString()) : new DateTime(1950, 01, 01);
                    ii.note_fattura = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.tariffa_forfait = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.prima_fascia_oraria = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.seconda_fascia_oraria = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.nazione_mittente = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.nazione_destinatario = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.tel_destinatario = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.note_incasso = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.epal = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.tipo_inc_richiesto = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.email = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.metri_lineari = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.collegamento_tc = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.num_colli_tc = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.peso_effettivo_tc = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.metri_cubi_tc = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.bancali_tc = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.sec_riferimento = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.adr = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.mit_longitudine = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.mit_latitudine = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.dest_longitudine = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.dest_latitudine = checkValueCDL(item.GetValue(++i)) ? Decimal.Parse(item.GetValue(i).ToString()) : 0M;
                    ii.note_ritiro = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.data_pronto_merce = checkValueCDL(item.GetValue(++i)) ? DateTime.Parse(item.GetValue(i).ToString()) : new DateTime(1950, 01, 01);
                    ii.tel_mittente = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.regione_mit = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";
                    ii.regione_dest = checkValueCDL(item.GetValue(++i)) ? item.GetValue(i).ToString() : "";

                    listBolle.Add(ii);
                    System.Threading.Thread.Sleep(100);
                    t++;
                    Console.WriteLine($"{t}");

                    //cdlDB.va_bolle_sat.Add(ii);
                    //cdlDB.SaveChanges();

                }
            }
            catch (Exception ee)
            {


            }


            return listBolle;
        }
        #endregion

        #region Logger
        internal static Logger _loggerCode = LogManager.GetLogger("loggerCode");
        internal static Logger _loggerAPI = LogManager.GetLogger("LogAPI");
        #endregion

        #region API
        private static string endpointAPI_UNITEX = "https://010761.espritec.cloud:9500";
        private static string userAPI = "dvalitutti";
        private static string passwordAPI = "Dv$2022!";


        private static DateTime DataScadenzaToken_UNITEX = DateTime.Now;
        private static string token_UNITEX = "";
        Exception LastException = new Exception("AVVIO");
        DateTime DateLastException = DateTime.MinValue;


        private void RecuperaConnessione()
        {
            if ((DateTime.Now + TimeSpan.FromHours(1)) > DataScadenzaToken_UNITEX)
            {
                UnitexGespeAPILogin();
            }

        }

        private void UnitexGespeAPILogin()
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
                $@"  ""username"": ""{userAPI}""," + "\n" +
                $@"  ""password"": ""{passwordAPI}""," + "\n" +
                @"  ""tenant"": ""UNITEX""" + "\n" +
                @"}";
                request.AddParameter("application/json-patch+json", body, ParameterType.RequestBody);
                client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse response = client.Execute(request);
                var resp = JsonConvert.DeserializeObject<RootobjectLoginUNITEX>(response.Content);

                DataScadenzaToken_UNITEX = resp.user.expire;
                token_UNITEX = resp.user.token;

                _loggerAPI.Info($"Nuovo token: {token_UNITEX}");

            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    //GestoreMail.SegnalaErroreDev("XcmLogin", ee);
                }
                LastException = ee;
            }
        }
        #endregion

        private bool checkValueCDL(object val)
        {
            return val != null && !string.IsNullOrEmpty(val.ToString());
        }

        #region EsitiWithExtenRef
        string WorkDir = Path.Combine(@"C:\UnitexStorico\Clienti", "CDL", "ESITI");

        public List<EsitiCDLExt> ReadFileEsitiEXT(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);

            if (fileExtension != ".csv")
            {
                Path.ChangeExtension(fileName, ".csv");
            }

            List<EsitiCDLExt> result = File.ReadAllLines(fileName)
                                           .Select(v => EsitiCDLExt.FromCsv(v))
                                           .ToList();
            return result;
        }

        public List<EsitiCDLExt> EsitiExternRef()
        {
            var result = new List<EsitiCDLExt>();
            var righeCsv = new List<string>();

            if (!Directory.Exists(WorkDir))
            {
                Directory.CreateDirectory(WorkDir);
            }

            try
            {
                var pathFTP = @"C:\FTP\FORNITORI\CDL\ESITI\IN";
                if (!Directory.Exists(pathFTP))
                {
                    return new List<EsitiCDLExt>();
                }

                var files = Directory.GetFiles(pathFTP);

                _loggerAPI.Info($"Trovati {files.Count()} files");


                if (files != null && files.Count() > 0)
                {
                    foreach (var f in files)
                    {
                        var fileName = Path.GetFileName(f);
                        _loggerAPI.Info($"Processo il file {fileName}");

                        if (fileName.StartsWith("UNITEX"))
                        {
                            result = ReadFileEsitiEXT(f);
                            if (result.Count() > 0)
                            {
                                foreach (var elem in result)
                                {
                                    if (elem.UnitexId == null)
                                    {
                                        var res = GetRiferimentoUnitexFromExtRef(elem.ExternalRef, true).Split(';');
                                        if (string.IsNullOrEmpty(res[0])) continue;
                                        elem.ShipID = int.Parse(res[0]);
                                        elem.UnitexId = res[1];
                                        elem.CustomerID = res[2];
                                        elem.StatudId = !string.IsNullOrEmpty(res[3]) ? int.Parse(res[3]) : 0;
                                    }
                                    else
                                    {
                                        var res = GetRiferimentoUnitexFromExtRef(elem.UnitexId, false).Split(';');
                                        if (string.IsNullOrEmpty(res[0])) continue;
                                        elem.ShipID = int.Parse(res[0]);
                                        elem.CustomerID = res[2];
                                        elem.StatudId = !string.IsNullOrEmpty(res[3]) ? int.Parse(res[3]) : 0;

                                    }

                                    //GESPE non considere se ha già quello stato la spedizone e duplica il tracking
                                    if (elem.StatudId == elem.Stato) continue;

                                    var bodyNewTracking = new TrackingNew();
                                    bodyNewTracking.shipID = elem.ShipID;
                                    bodyNewTracking.stopID = 0;
                                    bodyNewTracking.statusID = elem.Stato;
                                    bodyNewTracking.timeStamp = elem.DataTracking;

                                    var response = InsertNewTracking(bodyNewTracking);

                                    var newRow = "";
                                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                                    {
                                        var resp = JsonConvert.DeserializeObject<TrackingNewResponse>(response.Content);
                                        if (resp.result.status)
                                        {
                                            newRow = $"OK;{elem.CustomerID};{elem.UnitexId};{elem.DataTracking};{elem.Stato};";
                                        }
                                        else
                                        {
                                            newRow = $"KO;{elem.CustomerID};{elem.UnitexId};{elem.DataTracking};{elem.Stato};";
                                        }
                                    }
                                    else
                                    {
                                        newRow = $"KO;{elem.CustomerID};{elem.UnitexId};{elem.DataTracking};{elem.Stato};";
                                    }
                                    righeCsv.Add(newRow);
                                }

                                //_loggerCode.Info($"Esitate {result.Count()} spedizioni");


                                var fn = $"CDL_EXT_{DateTime.Now.ToString("yyyyMMdd_HHmmssffff")}.csv.old";
                                var dest = Path.Combine(WorkDir, fn);

                                if (File.Exists(dest))
                                {
                                    File.Delete(dest);
                                }

                                if (righeCsv.Count() > 0)
                                {
                                    File.WriteAllLines(dest, righeCsv);
                                }

                                var pathFileOut = Path.Combine(pathFTP, "Elaborati", fileName);

                                File.Move(f, pathFileOut);

                            }
                        }

                    }
                }
            }
            catch (Exception ee)
            {

                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("EsitiExternRef", ee);
                }
                LastException = ee;
            }
            return result;


        }

        public string GetEsitiFromRequestXCM(string rif, string cap)
        {
            var result = "";

            try
            {
                RecuperaConnessione();
                var parameter = $"ExternRef={rif}";

                var client = new RestClient(endpointAPI_UNITEX);
                var request = new RestRequest($"/api/tms/shipment/list/1000/1?{parameter}", Method.GET);


                client.Timeout = -1;
                request.AddHeader("Authorization", $"Bearer {token_UNITEX}");
                request.AlwaysMultipartFormData = true;
                IRestResponse response = client.Execute(request);

                var resp = JsonConvert.DeserializeObject<RootobjectShipment>(response.Content);

                if (resp != null && resp.shipments != null)
                {
                    var first = resp.shipments.FirstOrDefault();

                    //var client2 = new RestClient(endpointAPI_UNITEX);
                    var request2 = new RestRequest($"/api/tms/shipment/tracking/list/{first.id}", Method.GET);
                    client.Timeout = -1;
                    request2.AddHeader("Authorization", $"Bearer {token_UNITEX}");
                    request2.AlwaysMultipartFormData = true;
                    IRestResponse response2 = client.Execute(request2);
                    var resp2 = JsonConvert.DeserializeObject<RootobjectShipmentTrackingByID>(response2.Content);
                    if (resp2 != null && resp2.events != null)
                    {
                        var client3 = new RestClient("http://185.30.181.192:8092");
                        var request3 = new RestRequest($"/api/geo/GetRegionNameFromCap?cap={cap}", Method.GET);
                        client3.Timeout = -1;
                        //request2.AddHeader("Authorization", $"Bearer {token_UNITEX}");
                        string regione = "";
                        request3.AlwaysMultipartFormData = true;
                        IRestResponse response3 = client3.Execute(request3);
                        if (response3 != null && !string.IsNullOrEmpty(response3.Content))
                        {

                            regione = JsonConvert.DeserializeObject<string>(response3.Content);
                        }


                        result = $"{first.statusDes}|{first.docDate}|{resp2.events.Last().timeStamp}|{regione}";
                    }
                }
            }
            catch (Exception ee)
            {

                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("GetRiferimentoUnitexFromExtRef", ee);
                }
                LastException = ee;
            }
            return result;
        }
        public string GetRiferimentoUnitexFromExtRef(string rif, bool ext)
        {
            var result = "";

            try
            {
                RecuperaConnessione();
                var pageNumber = 1;
                var parameter = "";
                if (ext)
                {
                    parameter = $"ExternRef={rif}";

                }
                else
                {
                    parameter = $"DocNumber={rif}";
                }

                var client = new RestClient(endpointAPI_UNITEX);
                var request = new RestRequest($"/api/tms/shipment/list/1000/{pageNumber}?{parameter}", Method.GET);


                client.Timeout = -1;
                request.AddHeader("Authorization", $"Bearer {token_UNITEX}");
                request.AlwaysMultipartFormData = true;
                IRestResponse response = client.Execute(request);

                var resp = JsonConvert.DeserializeObject<RootobjectShipment>(response.Content);

                if (resp != null && resp.shipments != null)
                {
                    if (!ext)
                    {
                        var shipments = resp.shipments.OrderByDescending(x => x.docDate).ToList();
                        result = $"{shipments.FirstOrDefault().id};{shipments.FirstOrDefault().docNumber};{shipments.FirstOrDefault().customerID};{shipments.FirstOrDefault().statusId}";
                    }
                    else
                    {
                        result = $"{resp.shipments.FirstOrDefault().id};{resp.shipments.FirstOrDefault().docNumber};{resp.shipments.FirstOrDefault().customerID};{resp.shipments.FirstOrDefault().statusId}";
                    }
                }
            }
            catch (Exception ee)
            {

                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("GetRiferimentoUnitexFromExtRef", ee);
                }
                LastException = ee;
            }
            return result;
        }

        public IRestResponse InsertNewTracking(TrackingNew bodyModel)
        {
            IRestResponse response = null;
            try
            {
                var client = new RestClient(endpointAPI_UNITEX + "/api/tms/shipment/tracking/new");
                client.Timeout = -1;
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Authorization", $"Bearer {token_UNITEX}");
                request.AddHeader("Content-Type", "application/json");
                var body = JsonConvert.SerializeObject(bodyModel);

                request.AddParameter("application/json", body, ParameterType.RequestBody);
                response = client.Execute(request);
            }
            catch (Exception ee)
            {

                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("InsertNewTracking", ee);
                }
                LastException = ee;
            }

            return response;
        }
        #endregion
    }
}




