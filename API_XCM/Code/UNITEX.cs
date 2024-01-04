using API_XCM.Code.APIs;
using API_XCM.EF;
using API_XCM.Models.UNITEX;
using CommonAPITypes.ESPRITEC;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet.Model.CopyOperation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using static CommonAPITypes.UNITEX.UNITEXVolumeUpdate;

namespace API_XCM.Code
{
	public class UNITEX
	{

		public static List<API_XCM.Code.APIs.TmsTripListTrip> GetTrips()
		{
			return EspritecAPI_UNITEX.TmsTripList();
		}

		public static List<string> GLS(string tripNumber)
		{
			List<string> csvContent = new List<string>();

			var trip = EspritecAPI_UNITEX.TmsTripList().FirstOrDefault(x => x.docNumber == tripNumber);

			var shipments = EspritecAPI_UNITEX.TmsTripStopList(trip.id);

			foreach (var shipment in shipments)
			{
				var parcelList = EspritecAPI_UNITEX.TmsShipmentParcelList(shipment.shipID);

				if (shipment.shipDocNumber == "20505/SH")
				{

				}
				var shipDetail = EspritecAPI_UNITEX.TmsShipmentGet(shipment.shipID);

				string barcodes = "";

				foreach (var parcel in parcelList)
				{
					if (!string.IsNullOrEmpty(parcel.barcode))
					{
						barcodes += $"{parcel.barcode},";
					}

				}

				if (!string.IsNullOrEmpty(barcodes))
				{
					barcodes = barcodes.Remove(barcodes.Length - 1);
				}
				else
				{
					foreach (var parcel in parcelList)
					{
						if (!string.IsNullOrEmpty(parcel.barcodeMaster))
						{
							barcodes += $"{parcel.barcodeMaster},";
						}
					}

					if (!string.IsNullOrEmpty(barcodes))
					{
						barcodes = barcodes.Remove(barcodes.Length - 1);
					}
					else
					{
						foreach (var parcel in parcelList)
						{
							if (!string.IsNullOrEmpty(parcel.barcodeExt))
							{
								barcodes += $"{parcel.barcodeExt},";
							}
						}

						if (!string.IsNullOrEmpty(barcodes))
						{
							barcodes = barcodes.Remove(barcodes.Length - 1);
						}
					}

				}


				var glsShip = new ShipmentGLS()
				{
					DocNum = shipment.shipDocNumber,
					DataDoc = shipment.date != null ? shipment.date.ToString() : shipDetail.docDate != null ? shipDetail.docDate.ToString() : "",
					ExternalRef = shipment.shipExternRef,
					Info = Helper.ripulisciStringa(shipDetail.publicNote, " "),
					Packs = shipment.packs,
					TripNum = trip.docNumber,
					UnloadDes = shipment.description,
					UnloadAddress = shipment.address,
					UnloadCountry = shipment.country,
					UnloadDistrict = shipment.district,
					UnloadLocation = shipment.location,
					UnloadZipCode = shipment.zipCode,
					GrossWeight = shipment.grossWeight,
					Barcodes = barcodes,
				};

				var csvElement = $"{glsShip.DocNum};{glsShip.DataDoc};{glsShip.ExternalRef};{glsShip.UnloadDes};{glsShip.UnloadAddress};{glsShip.UnloadZipCode};{glsShip.UnloadLocation};{glsShip.UnloadDistrict};{glsShip.UnloadCountry};{glsShip.Packs};{glsShip.GrossWeight};{glsShip.Info};{glsShip.TripNum};{glsShip.Barcodes}";

				csvContent.Add(csvElement);
			}

			//var path = @"C:\GLS\";

			//if (!Directory.Exists(path))
			//{
			//    Directory.CreateDirectory(path);
			//}
			var numbNormalized = tripNumber.Replace('/', '-');

			File.WriteAllLines($@"C:\GLS\{numbNormalized}.csv", csvContent);

			return csvContent;
		}

		public static void TrackingNewUNITEX(string esitiFileName, string user, string startDate)
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

			var shipments = EspritecAPI_UNITEX.TmsShipmentList(startDate, "").ToList();

			//List<UnitexModel> unitex = File.ReadAllLines(@"C:\UNITEX\UNITEX_1711_3.CSV").Select(x => UnitexModel.FromCsv(x)).ToList();

			var esiti = File.ReadAllLines(esitiFileName).Select(x => Esiti.FromCsv(x)).ToList();

			//List<string> rifUnitex = File.ReadAllLines(@"C:\UNITEX\UnitexEsitiFG.txt").ToList();

			foreach (var elem in esiti)
			{
				//if (elem.StatudId != 30) continue;
				API_XCM.Code.APIs.Shipment exist = null;
				if (!string.IsNullOrEmpty(elem.UnitexId))
				{
					exist = shipments.FirstOrDefault(x => x.docNumber == elem.UnitexId);
				}
				else
				{
					exist = shipments.FirstOrDefault(x => x.externRef == $"202201{elem.ExternalRef}");

					if (exist == null)
					{
						exist = shipments.FirstOrDefault(x => x.insideRef == elem.ExternalRef);
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

			if (shipments.Count > 0)
			{
				ProduciFileExcelEsitiMancanti(shipments.Where(x => x.statusDes != "CONSEGNATA").ToList(), user);
			}

			if (nonEsitate.Count > 0)
			{
				File.WriteAllLines($@"C:\UNITEX\NonEsitate_{user}_{DateTime.Now:ddMM}.csv", nonEsitate);
			}

			if (esitate.Count > 0)
				File.WriteAllLines($@"C:\UNITEX\Esitate_{user}_{DateTime.Now:ddMM}.csv", esitate);
		}

		public static void TrackingNewALLWAYS(string esitiFileName)
		{
			EspritecAPI_UNITEX.Init("allwaysApi", "Aw$2022!", "UNITEX");

			var unitexShips = EspritecAPI_UNITEX.TmsShipmentList("10-01-2022", "").ToList();


			var esiti = File.ReadAllLines(esitiFileName).Select(x => Esiti.FromCsv(x)).ToList();

			//List<ClarityModel> cdlShip = File.ReadAllLines(@"C:\UNITEX\CDL_F01_1511.csv").Select(x => ClarityModel.FromCsv(x)).ToList();
			//List<ClarityModel> cdlShip = File.ReadAllLines(@"C:\UNITEX\GIULIANI_1711.csv").Select(x => ClarityModel.FromCsv(x)).ToList();
			List<ClarityModel> cdlShips = File.ReadAllLines(@"C:\UNITEX\ALL_0110_1711.csv").Select(x => ClarityModel.FromCsv(x)).ToList();
			//List<UnitexModel> unitex = File.ReadAllLines(@"C:\UNITEX\CDL_0811.CSV").Select(x => UnitexModel.FromCsv(x)).ToList();

			//List<ClarityModel> cdlShip = File.ReadAllLines(@"C:\UNITEX\CDL_2.CSV").Select(x => ClarityModel.FromCsv(x)).ToList();
			var cdlShip = cdlShips.Where(z => !string.IsNullOrWhiteSpace(z.RifCDL)).ToList();

			List<string> nonEsitate = new List<string>();
			List<string> apiFailed = new List<string>();
			List<string> esitate = new List<string>();
			UnitexModel ship = new UnitexModel();
			foreach (var elem in esiti)
			{

				API_XCM.Code.APIs.Shipment exist = null;
				if (!string.IsNullOrEmpty(elem.UnitexId))
				{
					exist = unitexShips.FirstOrDefault(x => x.docNumber == elem.UnitexId);
				}
				else
				{
					if (elem.ExternalRef.StartsWith("22"))
					{
						exist = unitexShips.FirstOrDefault(x => x.externRef == $"202201{elem.ExternalRef}");
					}
					else
					{
						exist = unitexShips.FirstOrDefault(x => x.insideRef == elem.ExternalRef);

						if (exist == null)
						{
							//    ship = unitex.FirstOrDefault(x => x.RifCdl == elem.ExternalRef);
							//}
							var shipCdl = cdlShip.FirstOrDefault(x => x.ExtRif == elem.ExternalRef);
							if (shipCdl != null)
							{
								exist = unitexShips.FirstOrDefault(x => x.externRef == $"202201{shipCdl.RifCDL}");
								if (exist == null)
								{
									elem.UnitexId = shipCdl.RifCDL;
								}
							}
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


					var newRow = "";
					if (response != null && response.StatusCode == HttpStatusCode.OK)
					{
						var resp = JsonConvert.DeserializeObject<TmsShipmentTrackingNewResponse>(response.Content);
						if (!resp.result.status)
						{
							newRow = $"{exist.docNumber};{elem.DataTracking};{exist.statusId};{exist.externRef}";
							apiFailed.Add(newRow);
						}
						else
						{
							unitexShips.Remove(exist);
							newRow = $"{exist.docNumber}";
							esitate.Add(newRow);
						}
					}
					else
					{

						newRow = $"{exist.docNumber};{elem.DataTracking};{exist.statusId};{exist.externRef}";
						apiFailed.Add(newRow);
					}
				}
				else
				{
					var newRow = "";
					//var tt = ship != null ? ship.RifUnitex : "";
					var fir = !string.IsNullOrEmpty(elem.UnitexId) ? elem.UnitexId : !string.IsNullOrEmpty(elem.ExternalRef) ? elem.ExternalRef : "NOT";
					if (!fir.ToLower().Contains("rit"))
					{
						newRow = $"{fir};{elem.DataTracking};30";
						nonEsitate.Add(newRow);
					}
				}

			}


			if (unitexShips.Count > 0)
			{
				ProduciFileExcelEsitiMancanti(unitexShips, "");
			}
			if (nonEsitate.Count > 0)
			{
				File.WriteAllLines($@"C:\UNITEX\NonEsitate_ALLWAYS_{DateTime.Now:ddMM}.csv", nonEsitate);
			}

			if (esitate.Count > 0)
				File.WriteAllLines($@"C:\UNITEX\Esitate_ALLWAYS_{DateTime.Now:ddMM}.csv", esitate);

		}

		public static void ProduciFileExcelEsitiMancanti(List<API_XCM.Code.APIs.Shipment> shipments, string user)
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

			workbook.SaveDocument($@"C:\UNITEX\ESITI_MANCANTI_{user}_{DateTime.Now:ddMM}_.xlsx", DocumentFormat.Xlsx);

		}

		public static void UnitexStats()
		{
			List<string> provinceTriveneto = File.ReadAllLines(@"C:\UNITEX\ProvincieTriveneto.txt").ToList();
			//EspritecAPI_UNITEX.Init("dvalitutti", "Dv$2022!", "UNITEX");

			//var AllShips = EspritecAPI_UNITEX.TmsShipmentList("09-01-2022", "");

			//var shipments = AllShips.Where(x => provinceTriveneto.Contains(x.consigneeDistrict)).ToList();


			Workbook workbook = new Workbook();

			workbook.LoadDocument(@"C:\UNITEX\Unitex_Triveneto.xlsx");

			var wksheet = workbook.Worksheets[0];

			var docRange = wksheet.GetUsedRange();
			var totRighe = docRange.RowCount;
			var rowIndex = 2;
			List<string> districts = new List<string>();

			for (int i = 2; i < totRighe; i++)
			{
				var district = wksheet.Cells[$"T{i}"].Value.ToString();
				districts.Add(district);

			}



			foreach (var pv in provinceTriveneto)
			{
				//var ships = AllShips.Where(x => x.consigneeDistrict == pv).ToList();

				var cnt = districts.Where(x => x == pv).ToList().Count();


				wksheet.Cells[$"A{rowIndex}"].Value = pv;
				wksheet.Cells[$"B{rowIndex}"].Value = cnt;
				rowIndex++;

			}

			workbook.SaveDocument(@"C:\UNITEX\StatsUnitex.xlsx", DocumentFormat.Xlsx);

		}
	}
}