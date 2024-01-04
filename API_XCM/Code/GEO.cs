using API_XCM.Models;
using API_XCM.Models.XCM;
using API_XCM.Models.XCM.TEST;
using DevExpress.Spreadsheet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace API_XCM.Code
{
    public class GEO
    {
        internal static Logger _loggerCode = LogManager.GetLogger("loggerCode");

        private XCM_WMSEntities dbXCMwms = new XCM_WMSEntities();

        private GnCommonXcmEntities dbXCMCommon = new GnCommonXcmEntities();

        public string riceviNomeRegioneDalCap(string cap)
        {
            var regione = "";
            try
            {
                var isPresente = dbXCMwms.GEO_IT.FirstOrDefault(x => x.CAP == cap);
                if (isPresente != null)
                {
                    regione = isPresente.REGIONE != null ? isPresente.REGIONE : "";
                }
            }
            catch (Exception RiceviNomeRegioneDallaProvinciaException)
            {
                _loggerCode.Error(RiceviNomeRegioneDallaProvinciaException, RiceviNomeRegioneDallaProvinciaException.Message);
            }
            return regione;
        }

        public void FlagLocalitaDisagiateOnGespe()
        {
            //GnCommonXcmEntitiesTEST dbXCMCommon = new GnCommonXcmEntitiesTEST();

            var fileName = @"C:\XCM\localitaDisagiateCAP.CSV";

            List<CAPDisagiati> list = File.ReadAllLines(fileName)
                                           .Select(v => CAPDisagiati.FromCsv(v))
                                           .ToList();

            foreach(var cap in list)
            {
                var daAggiornare = dbXCMCommon.GeoZipCode.Where(x => x.ItemCode == cap.Cap).ToList();
                if(daAggiornare != null && daAggiornare.Count() > 0)
                {
                    foreach(var elem in daAggiornare)
                    {
                        elem.ItemDown = true;
                    }
                    dbXCMCommon.SaveChanges();
                }
            }
        }

        public void CapLocalitaDisagiate(string sourcePath, string destinationPath)
        {

            Workbook wokbook = new Workbook();
            //wokbook.LoadDocument(@"C:\XCM\LocalitaDisagiateSenzaCAP.xlsx");
            wokbook.LoadDocument(sourcePath);
            Worksheet wksheet = wokbook.Worksheets[0];

            List<string> localitaSenzaCap = new List<string>();
            List<string> localitaConCap = new List<string>();

            var docRange = wksheet.GetUsedRange();
            var totRighe = docRange.RowCount;

            for (int i = 1; i < totRighe; i++)
            {
                var cap = wksheet.Cells[$"A{i}"].Value.ToString();
                if (string.IsNullOrEmpty(cap))
                {
                    var localita = wksheet.Cells[$"B{i}"].Value.ToString().ToLower().Trim();
                    var capOnDB = dbXCMwms.GEO_IT.FirstOrDefault(x => x.CITTA.ToLower().Trim() == localita);
                    if (capOnDB != null)
                    {
                        wksheet.Cells[$"A{i}"].Value = capOnDB.CAP.ToString();
                        localitaConCap.Add(localita);
                    }
                    else
                    {
                        var capFromNominatimAPI = GetCapByLocationNominatimAPI(localita);
                        if (!string.IsNullOrEmpty(capFromNominatimAPI))
                        {
                            wksheet.Cells[$"A{i}"].Value = capFromNominatimAPI;
                            localitaConCap.Add(localita);
                        }
                        else
                        {
                            localitaSenzaCap.Add(localita);
                        }

                    }

                }
            }
            //var dest = @"C:\XCM\localitaDisagiateCAP2.xlsx";

            wokbook.SaveDocument(destinationPath, DocumentFormat.Xlsx);

        }

        public string GetCapByLocationNominatimAPI(string localita)
        {
            var q = $"q={localita}";
            var outputformat = $"format=json";
            var details = $"addressdetails=1&limit=1";

            var url = $"https://nominatim.openstreetmap.org/search?{q}&{outputformat}&{details}";

            var client = new RestClient(url);
            client.Timeout = -1;

            var request = new RestRequest(Method.GET);
            request.AddParameter("text/plain", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            JArray json = JArray.Parse(response.Content.ToString());

            return json[0]["address"]["postcode"].ToString();
        }

        public string GetRegionNameByDistrict(string provincia)
        {
            var regione = "";
            try
            {
                var isPresente = dbXCMwms.GEO_IT.FirstOrDefault(x => x.PROVINCIA == provincia);
                if(isPresente != null)
                {
                    regione = isPresente.REGIONE != null ? isPresente.REGIONE : "";
                }
            }
            catch (Exception RiceviNomeRegioneDallaProvinciaException)
            {
                _loggerCode.Error(RiceviNomeRegioneDallaProvinciaException, RiceviNomeRegioneDallaProvinciaException.Message);
            }
            return regione;
        }
    }

    public class CAPDisagiati
    {
        public string Cap { get; set; }
        public string Localita { get; set; }
        public string Provincia { get; set; }

        public static CAPDisagiati FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(';');
            CAPDisagiati list = new CAPDisagiati();
            list.Cap = Convert.ToString(values[0]);
            list.Localita = Convert.ToString(values[1]);
            list.Provincia = Convert.ToString(values[2]);
            return list;
        }
    }
}