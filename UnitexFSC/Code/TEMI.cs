using API_XCM.Models.UNITEX;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet.Model.CopyOperation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnitexFSC.Code.APIs;

namespace UnitexFSC.Code
{
    public class TEMI
    {

        public static List<TmsTripListTrip> GetTrips()
        {
            EspritecAPI_UNITEX.Init("dvalitutti", "Dv$2022!", "UNITEX");

            return EspritecAPI_UNITEX.TmsTripList().Where(x => x.carrierID == "00013").ToList();
        }

        public static Workbook GetExcelFileContent(string tripNumber)
        {
            try
            {
                List<string> csvContent = new List<string>();
                List<ShipmentGLS> shipmentGLS = new List<ShipmentGLS>();
                var trip = EspritecAPI_UNITEX.TmsTripList().FirstOrDefault(x => x.docNumber == tripNumber);

                var shipments = EspritecAPI_UNITEX.TmsTripStopList(trip.id);

                foreach (var shipment in shipments)
                {
                    var parcelList = EspritecAPI_UNITEX.TmsShipmentParcelList(shipment.shipID);

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
                        Contrassegno = shipDetail.cashValue,
                        Barcodes = barcodes,
                    };

                    shipmentGLS.Add(glsShip);

                    var csvElement = $"{glsShip.DocNum};{glsShip.DataDoc};{glsShip.ExternalRef};{glsShip.UnloadDes};{glsShip.UnloadAddress};{glsShip.UnloadZipCode};{glsShip.UnloadLocation};{glsShip.UnloadDistrict};{glsShip.UnloadCountry};{glsShip.Packs};{glsShip.GrossWeight};{glsShip.Info};{glsShip.Contrassegno};{glsShip.Sprinter};{glsShip.TripNum};{glsShip.Barcodes}";

                    csvContent.Add(csvElement);
                }


                //TODO: produce il loro excel che gli sta inviando Luigi fin quando non entra in produzione il loro csv
                Workbook workbook = new Workbook();

                //workbook.LoadDocument(@"C:\XCM\OutGLS-27102022.xlsx");

                var wksheet = workbook.Worksheets[0];

                var i = 1;

                wksheet.Cells[$"A{i}"].Value = "Azienda";
                wksheet.Cells[$"B{i}"].Value = "Num. Doc.";
                wksheet.Cells[$"C{i}"].Value = "Data doc.";
                wksheet.Cells[$"D{i}"].Value = "Rif. esterni";
                wksheet.Cells[$"E{i}"].Value = "Consignee";
                wksheet.Cells[$"F{i}"].Value = "Indirizzo";
                wksheet.Cells[$"G{i}"].Value = "CAP Scarico";
                wksheet.Cells[$"H{i}"].Value = "Località Scarico";
                wksheet.Cells[$"I{i}"].Value = "Pv";
                wksheet.Cells[$"J{i}"].Value = "Colli";
                wksheet.Cells[$"K{i}"].Value = "Peso lordo";
                wksheet.Cells[$"L{i}"].Value = "Info";
                wksheet.Cells[$"M{i}"].Value = "Viaggio cons.";
                wksheet.Cells[$"N{i}"].Value = "Data";
                wksheet.Cells[$"O{i}"].Value = "Fornitore";

                i++;
                foreach (var elem in shipmentGLS)
                {
                    wksheet.Cells[$"A{i}"].Value = "Unitex";
                    wksheet.Cells[$"B{i}"].Value = elem.DocNum;
                    wksheet.Cells[$"C{i}"].Value = elem.DataDoc;
                    wksheet.Cells[$"D{i}"].Value = elem.ExternalRef;
                    wksheet.Cells[$"E{i}"].Value = elem.UnloadDes;
                    wksheet.Cells[$"F{i}"].Value = elem.UnloadAddress;
                    wksheet.Cells[$"G{i}"].Value = elem.UnloadZipCode;
                    wksheet.Cells[$"H{i}"].Value = elem.UnloadLocation;
                    wksheet.Cells[$"I{i}"].Value = elem.UnloadDistrict;
                    wksheet.Cells[$"J{i}"].Value = elem.Packs;
                    wksheet.Cells[$"K{i}"].Value = elem.GrossWeight;
                    wksheet.Cells[$"L{i}"].Value = elem.Info;
                    wksheet.Cells[$"M{i}"].Value = tripNumber;
                    wksheet.Cells[$"N{i}"].Value = DateTime.Now.Date.ToString();
                    wksheet.Cells[$"O{i}"].Value = "TEMI S.P.A";

                    i++;

                }

                //workbook.SaveDocument(@"C:\XCM\01874-TR.xlsx");

                //var path = @"C:\GLS\";

                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                //var numbNormalized = tripNumber.Replace('/', '-');

                //File.WriteAllLines($@"C:\GLS\{numbNormalized}.csv", csvContent);

                return workbook;
            }
            catch (Exception ee)
            {

            }
            return new Workbook();
        }

        public static List<string> GetGLSFileContent(string tripNumber)
        {
            try
            {
                List<string> csvContent = new List<string>();
                List<ShipmentGLS> shipmentGLS = new List<ShipmentGLS>();
                var trip = EspritecAPI_UNITEX.TmsTripList().FirstOrDefault(x => x.docNumber == tripNumber);

                var shipments = EspritecAPI_UNITEX.TmsTripStopList(trip.id);

                foreach (var shipment in shipments)
                {
                    var parcelList = EspritecAPI_UNITEX.TmsShipmentParcelList(shipment.shipID);

                    var shipDetail = EspritecAPI_UNITEX.TmsShipmentGet(shipment.shipID);

                    string barcodes = "";

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
                    else
                    {
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
                        Contrassegno = shipDetail.cashValue,
                        Barcodes = barcodes,
                    };

                    shipmentGLS.Add(glsShip);

                    var csvElement = $"{glsShip.DocNum};{glsShip.DataDoc};{glsShip.ExternalRef};{glsShip.UnloadDes};{glsShip.UnloadAddress};{glsShip.UnloadZipCode};{glsShip.UnloadLocation};{glsShip.UnloadDistrict};{glsShip.UnloadCountry};{glsShip.Packs};{glsShip.GrossWeight};{glsShip.Info};{glsShip.Contrassegno};{glsShip.Sprinter};{glsShip.TripNum};{glsShip.Barcodes}";

                    csvContent.Add(csvElement);
                }


                //TODO: produce il loro excel che gli sta inviando Luigi fin quando non entra in produzione il loro csv
                //Workbook workbook = new Workbook();

                //workbook.LoadDocument(@"C:\XCM\OutGLS-27102022.xlsx");

                //var wksheet = workbook.Worksheets[0];

                //var docRange = wksheet.GetUsedRange();
                //var totRighe = docRange.RowCount;

                //var i = 2;

                //foreach (var elem in shipmentGLS)
                //{
                //    wksheet.Cells[$"B{i}"].Value = elem.DocNum;
                //    wksheet.Cells[$"C{i}"].Value = elem.DataDoc;
                //    wksheet.Cells[$"D{i}"].Value = elem.ExternalRef;
                //    wksheet.Cells[$"E{i}"].Value = elem.UnloadDes;
                //    wksheet.Cells[$"F{i}"].Value = elem.UnloadAddress;
                //    wksheet.Cells[$"G{i}"].Value = elem.UnloadZipCode;
                //    wksheet.Cells[$"H{i}"].Value = elem.UnloadLocation;
                //    wksheet.Cells[$"I{i}"].Value = elem.UnloadDistrict;
                //    wksheet.Cells[$"J{i}"].Value = elem.Packs;
                //    wksheet.Cells[$"K{i}"].Value = elem.GrossWeight;
                //    wksheet.Cells[$"L{i}"].Value = elem.Info;
                //    wksheet.Cells[$"M{i}"].Value = tripNumber;
                //    wksheet.Cells[$"N{i}"].Value = DateTime.Now.Date.ToString();
                //    wksheet.Cells[$"O{i}"].Value = "TEMI S.P.A";

                //    i++;

                //}

                //workbook.SaveDocument(@"C:\XCM\01874-TR.xlsx");

                //var path = @"C:\GLS\";

                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                //var numbNormalized = tripNumber.Replace('/', '-');

                //File.WriteAllLines($@"C:\GLS\{numbNormalized}.csv", csvContent);

                return csvContent;
            }
            catch (Exception ee)
            {

            }
            return new List<string>();

        }
    }
}