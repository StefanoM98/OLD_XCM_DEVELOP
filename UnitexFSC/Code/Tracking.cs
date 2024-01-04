using DevExpress.Spreadsheet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnitexFSC.Code.APIs;

namespace UnitexFSC.Code
{
    public class Tracking
    {
        public static ResponseTracking TrackingNewUNITEX(List<string> esitiList, string user, string startDate)
        {
            List<string> nonEsitate = new List<string>();
            List<string> esitate = new List<string>();
            List<string> diFarco = new List<string>();

            if (user == "CDL")
            {
                EspritecAPI_UNITEX.Init("cdlApi", "!Cdl-IT@2022", "UNITEX");
            }
            else if (user == "GLS")
            {
                EspritecAPI_UNITEX.Init("glsApi", "GLS.IT@2022!1a", "UNITEX");

            }
            else if (user == "ALLWAYS")
            {
                EspritecAPI_UNITEX.Init("allwaysApi", "Aw$2022!", "UNITEX");

            }
            else if (user == "FG")
            {
                EspritecAPI_UNITEX.Init("fgApi", "Fg.IT@2022!", "UNITEX");
            }
            else if (user == "COTRAF")
            {
                EspritecAPI_UNITEX.Init("cotrafApi", "!Cotraf-IT@2022", "UNITEX");
            }
            else if (user == "EMMEA")
            {
                EspritecAPI_UNITEX.Init("emmeaApi", "Em$2022!", "UNITEX");
            }
            else if (user == "TLI")
            {
                EspritecAPI_UNITEX.Init("tliApi", "Tl$2022!", "UNITEX");
            }
            else
            {
                EspritecAPI_UNITEX.Init("dvalitutti", "Dv$2022!", "UNITEX");
            }

            var shipments = EspritecAPI_UNITEX.TmsShipmentList(startDate, "").Where(x => x.statusDes != "CONSEGNATA").ToList();


            var esiti = esitiList.Select(x => EsitiModel.FromCsv(x)).ToList();

            foreach (var elem in esiti)
            {
                Shipment exist = null;
                if (!string.IsNullOrEmpty(elem.UnitexId))
                {
                    exist = shipments.FirstOrDefault(x => x.docNumber == elem.UnitexId);
                }
                else
                {
                    
                    exist = shipments.FirstOrDefault(x => x.externRef == $"202201{elem.ExternalRef}");

                    if (exist == null)
                    {
                        exist = shipments.FirstOrDefault(x => x.externRef == elem.ExternalRef);

                        if(exist == null)
                        {
                            exist = shipments.FirstOrDefault(x => x.insideRef == elem.ExternalRef);
                        }
                    }
                }

                if (exist != null)
                {
                    if (exist.statusId == 30) continue;

                    var bodyNewTracking = new TmsShipmentTrackingNew();
                    bodyNewTracking.shipID = exist.id;
                    bodyNewTracking.stopID = 0;
                    bodyNewTracking.statusID = 30;
                    bodyNewTracking.timeStamp = elem.DataTracking;

                    var response = EspritecAPI_UNITEX.TmsShipmentTrackingNew(bodyNewTracking);

                    int i = 1;
                    var newRow = "";
                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        var resp = JsonConvert.DeserializeObject<TmsShipmentTrackingNewResponse>(response.Content);
                        if (!resp.result.status)
                        {
                            newRow = $"{exist.docNumber};{elem.DataTracking};{exist.statusId};";
                            nonEsitate.Add(newRow);
                        }
                        else
                        {
                            shipments.Remove(exist);
                            newRow = $"{exist.docNumber}";
                            esitate.Add(newRow);
                        }
                    }
                    else
                    {
                        newRow = $"{exist.docNumber};{elem.DataTracking};{exist.statusId}";
                        nonEsitate.Add(newRow);
                    }
                }
                else
                {
                    var newRow = "";
                    var fir = !string.IsNullOrEmpty(elem.UnitexId) ? elem.UnitexId : !string.IsNullOrEmpty(elem.ExternalRef) ? elem.ExternalRef : "NOT";
                    newRow = $"{fir};{elem.DataTracking}";
                    nonEsitate.Add(newRow);
                }


            }

            Workbook workbook = null;
            if (shipments.Count > 0)
            {
                workbook = ProduciFileExcelEsitiMancanti(shipments);
            }

            return new ResponseTracking() { workbook = workbook, Esitate = esitate, NonEsitate = nonEsitate };

            //if (nonEsitate.Count > 0)
            //{
            //    File.WriteAllLines($@"C:\UNITEX\NonEsitate_{user}_{DateTime.Now:ddMM}.csv", nonEsitate);
            //}

            //if (esitate.Count > 0)
            //    File.WriteAllLines($@"C:\UNITEX\Esitate_{user}_{DateTime.Now:ddMM}.csv", esitate);
            ////File.WriteAllLines(@"C:\UNITEX\Esitate.csv", esitate);
            ////File.WriteAllLines(@"C:\UNITEX\RiferimentiDiFarco.csv", diFarco);

        }

        public static Workbook ProduciFileExcelEsitiMancanti(List<Shipment> shipments)
        {
            Workbook workbook = new Workbook();
            Worksheet wksheet = workbook.Worksheets[0];

            int i = 1;

            wksheet.Cells[$"A{i}"].Value = "DATA DOC";
            wksheet.Cells[$"B{i}"].Value = "DOC NUM";
            wksheet.Cells[$"C{i}"].Value = "DATA CONS";
            wksheet.Cells[$"D{i}"].Value = "RIF INT";
            wksheet.Cells[$"E{i}"].Value = "RIF EST";
            wksheet.Cells[$"F{i}"].Value = "MITTENTE";
            wksheet.Cells[$"G{i}"].Value = "DESTINAZIONE";
            wksheet.Cells[$"H{i}"].Value = "LOCALITA";
            wksheet.Cells[$"I{i}"].Value = "CAP";
            wksheet.Cells[$"J{i}"].Value = "PROV";
            wksheet.Cells[$"K{i}"].Value = "NZ";
            wksheet.Cells[$"L{i}"].Value = "COLLI";
            wksheet.Cells[$"M{i}"].Value = "PESO";
            wksheet.Cells[$"N{i}"].Value = "BANCALI";
            i++;

            foreach (var ship in shipments)
            {
                wksheet.Cells[$"A{i}"].Value = ship.docDate;
                wksheet.Cells[$"B{i}"].Value = ship.docNumber;
                wksheet.Cells[$"C{i}"].Value = "";
                wksheet.Cells[$"D{i}"].Value = ship.insideRef;
                wksheet.Cells[$"E{i}"].Value = ship.externRef;
                wksheet.Cells[$"F{i}"].Value = ship.firstStopDes;
                wksheet.Cells[$"G{i}"].Value = ship.lastStopDes;
                wksheet.Cells[$"H{i}"].Value = ship.lastStopLocation;
                wksheet.Cells[$"I{i}"].Value = ship.lastStopZipCode;
                wksheet.Cells[$"J{i}"].Value = ship.lastStopDistrict;
                wksheet.Cells[$"K{i}"].Value = ship.lastStopCountry;
                wksheet.Cells[$"L{i}"].Value = ship.packs;
                wksheet.Cells[$"M{i}"].Value = ship.grossWeight;
                wksheet.Cells[$"N{i}"].Value = ship.floorPallets;
                i++;

            }

            return workbook;
            //workbook.SaveDocument($@"C:\UNITEX\ESITI_MANCANTI_{user}_{DateTime.Now:ddMM}.xlsx", DocumentFormat.Xlsx);

        }


    }

    public class ResponseTracking
    {
        public Workbook workbook { get; set; }
        public List<string> NonEsitate { get; set; }
        public List<string> Esitate { get; set; }
    }

    public class EsitiModel
    {
        public string UnitexId { get; set; }
        public string ExternalRef { get; set; }
        public DateTime DataTracking { get; set; }
        public int Stato { get; set; }

        public string CustomerID { get; set; }
        public int ShipID { get; set; }
        public int StatudId { get; set; }

        public static EsitiModel FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(';');
            EsitiModel esiti = new EsitiModel();
            var firstElement = Convert.ToString(values[0]);
            if (firstElement.ToLower().Contains("/sh"))
            {
                esiti.UnitexId = firstElement;
            }
            else
            {

                esiti.ExternalRef = firstElement;
            }
            esiti.DataTracking = Convert.ToDateTime(values[1]);
            //esiti.StatudId = Convert.ToInt32(values[2]);

            return esiti;
        }

    }
}
