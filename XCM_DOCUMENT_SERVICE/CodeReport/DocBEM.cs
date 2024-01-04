using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCM_DOCUMENT_SERVICE.CodeReport
{
    internal static class DocBEM
    {
        public static string DirectorySalvataggio = Automazione.WorkPath;
        public static string TemplateDocx = "XCMBEM.docx";
        public static string ngdReport = "Template_BEM_NGD.xls";
        static string DESTINATARIO = "XCM Healthcare";
        static string INDIRIZZO_DESTINATARIO = "S.S. 87 Km 20+700 ex Area Ind. 3ò";
        static string CAPD = "81020";
        static string PROVD = "CE";
        static string CITTAD = "San Marco Evangelista";
        static string COUNTRYD = "IT";

        internal static string produciDocumentoBEM(RootobjectXCMRowsNEW RigheDocumentoAPIXCM, RootobjectXCMOrderNEW DocXCM, RootobjectXCMRowsNEW RigheAPI)
        {
            if (!Directory.Exists(DirectorySalvataggio))
            {
                Directory.CreateDirectory(DirectorySalvataggio);
            }

            var cust = DocXCM.header.customerDes.Replace(".", "").Replace(" ", "_").Replace("'", "");
            var finalDest = Path.Combine(DirectorySalvataggio, cust);

            if (!Directory.Exists(finalDest))
            {
                Directory.CreateDirectory(finalDest);
            }

            var docNum = DocXCM.header.docNumber;

            RichEditDocumentServer docs = new RichEditDocumentServer();
            docs.Document.LoadDocument(TemplateDocx);
            var doc = docs.Document;

            docs.LoadDocument(TemplateDocx);

            foreach (var s in doc.Sections)
            {
                s.Page.PaperKind = System.Drawing.Printing.PaperKind.A4;
            }
   
            doc.ReplaceAll("$$BEMXCM$$", docNum, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$REFERENCE$$", DocXCM.header.reference, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$DATEREFERENCE$$", DocXCM.header.referenceDate.ToString("dd/MM/yyyy"), DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$DATA$$", DocXCM.header.docDate.ToString("dd/MM/yyyy"), DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);

            string ntt = "";
            if (!string.IsNullOrEmpty(ntt))
            {
                doc.ReplaceAll("$$NOTE$$", ntt, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);//itemnote da implementare
            }
            else
            {
                doc.ReplaceAll("$$NOTE$$", "", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);//itemnote da implementare
            }
            string proprietario = DocXCM.header.customerDes;

            if (string.IsNullOrEmpty(proprietario))
            {
                doc.ReplaceAll("$$PROPRIETARIO$$", "", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            }
            else
            {
                doc.ReplaceAll("$$PROPRIETARIO$$", $"Proprietario merce: {proprietario}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            }

            var maglog = RigheAPI.rows[0].logWareID;
            if (!string.IsNullOrEmpty(maglog))
            {
                doc.ReplaceAll("$$MAGLOGICO$$", $"Magazzino Logico {maglog}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            }
            else
            {
                doc.ReplaceAll("$$MAGLOGICO$$", "", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            }
            

            #region Dati Mittente
            doc.ReplaceAll("$$MITTENTE$$", DocXCM.header.senderDes, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$INDIRIZZO_MITTENTE$$", $"{DocXCM.header.senderAddress}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$CAPM$$", DocXCM.header.senderZipCode, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$PROVM$$", DocXCM.header.senderDistrict, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$CITTAM$$", DocXCM.header.senderLocation, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$COUNTRYM$$", DocXCM.header.senderCountry, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            #endregion

            #region Dati Destinatario
            doc.ReplaceAll("$$DESTINATARIO$$", DESTINATARIO, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$INDIRIZZO_DESTINATARIO$$", INDIRIZZO_DESTINATARIO, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$CAPD$$", CAPD, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$PROVD$$", PROVD, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$CITTAD$$", CITTAD, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$COUNTRYD$$", COUNTRYD, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            #endregion

            #region Produci Righe
            var ZONA = doc.Tables[1].Rows[1].Range;
            if(RigheDocumentoAPIXCM.rows != null)
            {
                for (int i = 1; i < RigheDocumentoAPIXCM.rows.Length; ++i)
                {
                    var RR = doc.Tables[1].Rows.InsertAfter(1);
                    for (int k = 0; k < doc.Tables[1].Rows[1].Cells.Count; ++k)
                    {
                        var SS = doc.GetRtfText(doc.Tables[1].Rows[1].Cells[k].ContentRange);
                        doc.InsertRtfText(RR.Cells[k].ContentRange.Start, SS);
                    }
                }
            }
            else
            {
                for (int i = 1; i < RigheAPI.rows.Length; ++i)
                {
                    var RR = doc.Tables[1].Rows.InsertAfter(1);
                    for (int k = 0; k < doc.Tables[1].Rows[1].Cells.Count; ++k)
                    {
                        var SS = doc.GetRtfText(doc.Tables[1].Rows[1].Cells[k].ContentRange);
                        doc.InsertRtfText(RR.Cells[k].ContentRange.Start, SS);
                    }
                }
            }

            #endregion

            #region Articoli
            if (RigheDocumentoAPIXCM.rows != null)
            {
                for (int i = 0; i < RigheDocumentoAPIXCM.rows.Length; ++i)
                {
                    ZONA = doc.Tables[1].Rows[1 + i].Range;
                    var rr = RigheDocumentoAPIXCM.rows[i];
                    DateTime dataScadenza = DateTime.MinValue;

                    if (rr.expireDate != null)
                    {
                        dataScadenza = rr.expireDate.Value;
                    }
                    doc.ReplaceAll("$$CODPRD$$", rr.partNumber, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                    doc.ReplaceAll("$$DESPRD$$", $"{rr.partNumberDes}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                    doc.ReplaceAll("$$QTA$$", $"{rr.qty:0.00}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                    doc.ReplaceAll("$$LOTTO$$", $"{rr.batchNo}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                    if (dataScadenza != DateTime.MinValue)
                    {
                        doc.ReplaceAll("$$SCAD$$", $"{dataScadenza.ToString("dd/MM/yyyy")}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                    }
                    else
                    {
                        doc.ReplaceAll("$$SCAD$$", $"Non rilevata", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                    }

                }
            }
            else
            {
                for (int i = 0; i < RigheAPI.rows.Length; ++i)
                {
                    ZONA = doc.Tables[1].Rows[1 + i].Range;
                    var rr = RigheAPI.rows[i];
                    DateTime dataScadenza = DateTime.MinValue;

                    if (rr.expireDate != null)
                    {
                        dataScadenza = rr.expireDate.Value;
                    }
                    doc.ReplaceAll("$$CODPRD$$", rr.partNumber, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                    doc.ReplaceAll("$$DESPRD$$", $"{rr.partNumberDes}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                    doc.ReplaceAll("$$QTA$$", $"{rr.qty:0.00}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                    doc.ReplaceAll("$$LOTTO$$", $"{rr.batchNo}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                    if (dataScadenza != DateTime.MinValue)
                    {
                        doc.ReplaceAll("$$SCAD$$", $"{dataScadenza.ToString("dd/MM/yyyy")}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                    }
                    else
                    {
                        doc.ReplaceAll("$$SCAD$$", $"Non rilevata", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                    }

                }
            }
            #endregion

            var saveAs = Path.Combine(finalDest, $"BEM_{docNum.Substring(0, 5)}.pdf");
            if (File.Exists(saveAs))
            {
                var jn = Path.ChangeExtension(saveAs, $"{DateTime.Now.Ticks}.old");
                File.Move(saveAs, jn);
            }
            docs.ExportToPdf(saveAs);
            //docs.SaveDocument(saveAs, DocumentFormat.OpenXml);

            return saveAs;
        }
        internal static string ProduciTracciatoXLSBEM(RootobjectXCMRowsNEW righeDoc, RootobjectXCMOrderNEW DocXCM)
        {
            if (!Directory.Exists(DirectorySalvataggio))
            {
                Directory.CreateDirectory(DirectorySalvataggio);
            }

            var cust = DocXCM.header.customerDes.Replace(".", "").Replace(" ", "_").Replace("'", "");
            var finalDest = Path.Combine(DirectorySalvataggio, cust);

            if (!Directory.Exists(finalDest))
            {
                Directory.CreateDirectory(finalDest);
            }

            Workbook workbook = new Workbook();
            workbook.LoadDocument(ngdReport);
            string mittente = DocXCM.header.senderDes;
            try
            {
                var wksheet = workbook.Worksheets[0];
                workbook.BeginUpdate();
              
                var totRighe = righeDoc.rows.Count();
                for (int i = 0; i < totRighe; i++)
                {
                   
                    string dataScadenza = "";
                    var dettRiga = righeDoc.rows[i];
                    

                    if (dettRiga.expireDate != null)
                    {
                        dataScadenza = dettRiga.expireDate.Value.ToString("dd/MM/yyyy");
                    }
                    else 
                    {
                        dataScadenza = "ND";
                    }

                    #region Scrittura dati
                    wksheet.Cells[$"A{i + 2}"].Value = dettRiga.partNumber;                   
                    wksheet.Cells[$"B{i + 2}"].Value = dettRiga.partNumberDes;
                    wksheet.Cells[$"C{i + 2}"].Value = dettRiga.qty;
                    wksheet.Cells[$"D{i + 2}"].Value = dettRiga.batchNo;                    
                    wksheet.Cells[$"E{i + 2}"].Value = dataScadenza;                    
                    wksheet.Cells[$"F{i + 2}"].Value = DocXCM.header.senderDes;                    
                    wksheet.Cells[$"G{i + 2}"].Value = DocXCM.header.senderLocation;                   
                    wksheet.Cells[$"H{i + 2}"].Value = DocXCM.header.customerDes;
                    wksheet.Cells[$"I{i + 2}"].Value = DocXCM.header.reference;
                    wksheet.Cells[$"J{i + 2}"].Value = DocXCM.header.referenceDate;
                    #endregion
                }

                
            }          
            finally
            {
                workbook.EndUpdate();
            }
            finalDest = Path.Combine(finalDest, $"{DocXCM.header.reference.Replace("/","_")}_{DateTime.Now.ToString("ddMMyyyyHHmmssfff")}.xlsx");
            if (File.Exists(finalDest)) { File.Delete(finalDest); }
            workbook.SaveDocument(finalDest, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
            return finalDest;
        }
    }
}
