using API_XCM.Models.XCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Code
{
    public class VIVISOL
    {

        public static List<DocumentList> GetVivisolDocuments(DateTime dataDa)
        {
            var db = new GnXcmEntities();
            var docs = db.uvwWmsDocument.Where(x => x.DocDta <= dataDa && x.DocTip == 204 && x.CustomerID == "00007").OrderByDescending(x => x.DocNum2).ToList();

            if (docs.Count > 0)
            {
                var resp = new List<DocumentList>();

                foreach (var doc in docs)
                {
                    resp.Add(CreaNuovoDocumentoAPIdaQueryDB(doc));
                }
                return resp;
            }
            return new List<DocumentList>();

        }
        private static DocumentList CreaNuovoDocumentoAPIdaQueryDB(uvwWmsDocument doc)
        {
            return new DocumentList()
            {
                docNum2 = doc.DocNum2,
                consigneeAddress = doc.ConsigneeAddress,
                consigneeCountry = doc.ConsigneeCountry,
                consigneeDes = doc.ConsigneeName,
                consigneeDistrict = doc.ConsigneeDistrict,
                consigneeID = (doc.ConsigneeID != null) ? doc.ConsigneeID.Value : 0,
                consigneeLocation = doc.ConsigneeLocation,
                consigneeRegion = doc.ConsigneeRegion,
                consigneeZipCode = doc.ConsigneeZipCode,
                coverage = doc.Coverage,
                customerDes = doc.CustomerDes,
                customerID = doc.CustomerID,
                deliveryNote = doc.ItemInfo,
                docDate = (doc.DocDta != null) ? doc.DocDta.Value : DateTime.MinValue,
                docNumber = doc.DocNum,
                docType = doc.DocType,
                executed = doc.Executed,
                externalID = doc.ExternalID,
                id = doc.uniq,
                info1 = doc.Info1,
                info2 = doc.Info2,
                info3 = doc.Info3,
                info4 = doc.Info4,
                info5 = doc.Info5,
                info6 = doc.Info6,
                info7 = doc.Info7,
                info8 = doc.Info8,
                info9 = doc.Info9,
                ownerDes = doc.OwnerDes,
                ownerID = doc.OwnerID,
                planned = doc.Planned,
                reference = doc.Reference,
                reference2 = doc.Reference2,
                reference2Date = (doc.RefDta2 != null) ? doc.RefDta2.Value : DateTime.MinValue,
                referenceDate = (doc.RefDta != null) ? doc.RefDta.Value : DateTime.MinValue,
                regTypeID = doc.RegTypeID,
                senderAddress = doc.SenderAddress,
                senderCountry = doc.SenderCountry,
                senderDes = doc.SenderName,
                senderDistrict = doc.SenderDistrict,
                senderID = (doc.SenderID != null) ? doc.SenderID.Value : 0,
                senderLocation = doc.SenderLocation,
                senderRegion = doc.SenderRegion,
                senderZipCode = doc.SenderZipCode,
                shipDocNumber = doc.ShipDocNum,
                shipID = (doc.ShipUniq != null) ? doc.ShipUniq.Value : 0,
                siteID = doc.SiteID,
                statusDes = doc.StatusDes,
                totalBoxes = (doc.TotalBoxes != null) ? doc.TotalBoxes.Value : 0,
                totalCube = (doc.TotalCube != null) ? doc.TotalCube.Value : 0,
                totalGrossWeight = (doc.TotalGrossWeight != null) ? doc.TotalGrossWeight.Value : 0,
                totalNetWeight = (doc.TotalNetWeight != null) ? doc.TotalNetWeight.Value : 0,
                totalPacks = (doc.TotalPacks != null) ? doc.TotalPacks.Value : 0,
                totalQty = (doc.TotalQty != null) ? doc.TotalQty.Value : 0,
                tripDocNumber = doc.TripDocNum,
                tripID = (doc.TripUniq != null) ? doc.TripUniq.Value : 0,
                unloadAddress = doc.UnloadAddress,
                unloadCountry = doc.UnloadCountry,
                unloadDistrict = doc.UnloadDistrict,
                unLoadDes = doc.UnloadName,
                unloadID = (doc.UnloadID != null) ? doc.UnloadID.Value : 0,
                unloadLocation = doc.UnloadLocation,
                unloadRegion = doc.UnloadRegion,
                unloadZipCode = doc.UnloadZipCode,
                //internalNote,
                //publicNode,
                //rowsNo,
                //statusId,
            };
        }
    }
}