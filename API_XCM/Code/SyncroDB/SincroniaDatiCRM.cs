using API_XCM.Models.XCM;
using API_XCM.Models.XCM.CRM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace API_XCM.Code.SyncroDB
{
    public static class SincroniaDatiCRM
    {
        static string lastCheck = "lastCheck.txt";

        public static void Sincronizza()
        {
            var dataDa = RecuperaUltimaSicronia();

            AggiornaOrdini(dataDa);
            AggiornaTracking(dataDa);
        }

        private static void AggiornaTracking(DateTime dataDa)
        {
            try
            {
                var db = new GnXcmEntities();
                var dbxcm = new XCM_CRMEntities();

                if (DateTime.Now.Hour > 0 && DateTime.Now.Hour < 1)
                {

                }

            }
            catch (Exception ee)
            {

                throw;
            }
        }

        private static void AggiornaOrdini(DateTime dataDa)
        {
            try
            {
                #region OLD
                //var db = new GnXcmEntities();
                //var dbxcm = new XCM_CRMEntities();
                //var docs = db.uvwWmsDocument.Where(x => x.RecCreate >= dataDa && x.RecChange >= dataDa).ToList();
                //var docsPresenti = dbxcm.Orders;
                //foreach (var doc in docs)
                //{
                //    var esiste = docsPresenti.FirstOrDefault(x => x.Orders_GespeID == doc.uniq);
                //    if (esiste != null)
                //    {
                //        //aggiorna record
                //        esiste.Orders_consigneeDes = doc.ConsigneeName;
                //        esiste.Orders_customerDes = doc.CustomerDes;
                //        esiste.Orders_customerID = doc.CustomerID;
                //        esiste.Orders_docDate = (doc.DocDta != null) ? doc.DocDta.Value : DateTime.MinValue;
                //        esiste.Orders_docNumber = doc.DocNum;
                //        esiste.Orders_GespeID = doc.uniq;
                //        esiste.Orders_reference = doc.Reference;
                //        esiste.Orders_referenceDate = (doc.RefDta != null) ? doc.RefDta.Value : DateTime.MinValue;
                //        esiste.Orders_reTypeID = doc.RegTypeID;
                //        esiste.Orders_senderDes = doc.SenderName;
                //        esiste.Orders_statusDes = doc.StatusDes;
                //        esiste.Orders_unloadDes = doc.UnloadName;
                //    }
                //    else
                //    {
                //        //crea record
                //        var nr = new Orders()
                //        {
                //            Orders_consigneeDes = doc.ConsigneeName,
                //            Orders_customerDes = doc.CustomerDes,
                //            Orders_customerID = doc.CustomerID,
                //            Orders_docDate = (doc.DocDta != null) ? doc.DocDta.Value : DateTime.MinValue,
                //            Orders_docNumber = doc.DocNum,
                //            Orders_GespeID = doc.uniq,
                //            Orders_reference = doc.Reference,
                //            Orders_referenceDate = (doc.RefDta != null) ? doc.RefDta.Value : DateTime.MinValue,
                //            Orders_reTypeID = doc.RegTypeID,
                //            Orders_senderDes = doc.SenderName,
                //            Orders_statusDes = doc.StatusDes,
                //            Orders_unloadDes = doc.UnloadName
                //        };
                //    }
                //}
                //dbxcm.SaveChanges(); 
                #endregion

                if (DateTime.Now.Hour > 0 && DateTime.Now.Hour < 1)
                {
                    var dbxcm = new XCM_CRMEntities();
                    var db = new GnXcmEntities();
                    List<Orders> daRimuovere = new List<Orders>();
                    var daControllare = dbxcm.Orders.ToList();

                    foreach (var dc in daControllare)
                    {
                        var esiste = db.uvwWmsDocument.FirstOrDefault(x => x.uniq == dc.Orders_GespeUniq);
                        if (esiste == null)
                        {
                            daRimuovere.Add(dc);
                        }
                    }
                    dbxcm.Orders.RemoveRange(daRimuovere);
                    dbxcm.SaveChanges();
                }
            }
            catch (Exception ee)
            {

                throw;
            }
        }

        private static DateTime RecuperaUltimaSicronia()
        {
            if (File.Exists(lastCheck))
            {
                var righe = File.ReadAllLines(lastCheck);
                return DateTime.Parse(righe.Last());
            }
            else
            {
                var lc = DateTime.Now - TimeSpan.FromDays(7);
                File.WriteAllText(lastCheck, lc.ToString());
                return lc;
            }
        }

        private static void scriviUltimaSincronia(DateTime lc)
        {
            File.WriteAllText(lastCheck, lc.ToString());
        }
    }
}