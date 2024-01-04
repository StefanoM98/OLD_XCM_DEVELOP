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
    internal static class DocORDER
    {
        public static string DirectorySalvataggio = Automazione.WorkPath;
        public static string TemplateDocxOrder = "XCMORDER.docx";
        static string DESTINATARIO = "XCM Healthcare";
        static string INDIRIZZO_DESTINATARIO = "S.S. 87 Km 20+700 ex Area Ind. 3ò";
        static string CAPD = "81020";
        static string PROVD = "CE";
        static string CITTAD = "San Marco Evangelista";
        static string COUNTRYD = "IT";

        internal static string produciDocumentoORDER_DAFNE(RootobjectXCMRowsNEW RigheDocumentoAPIXCM, RootobjectXCMOrderNEW DocXCM)
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
            docs.Document.LoadDocument(TemplateDocxOrder);
            var doc = docs.Document;

            docs.LoadDocument(TemplateDocxOrder);

            foreach (var s in doc.Sections)
            {
                s.Page.PaperKind = System.Drawing.Printing.PaperKind.A4;
            }

            doc.ReplaceAll("$$ORDERXCM$$", docNum, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$REFERENCE$$", DocXCM.header.reference, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$DATEREFERENCE$$", DocXCM.header.referenceDate.ToString("dd/MM/yyyy"), DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$DATA$$", DateTime.Now.ToString("dd/MM/yyyy"), DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);

            #region Dati Mittente
            doc.ReplaceAll("$$MITTENTE$$", DocXCM.header.consigneeDes, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$INDIRIZZO_MITTENTE$$", $"{DocXCM.header.consigneeAddress}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$CAPM$$", DocXCM.header.consigneeZipCode, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$PROVM$$", DocXCM.header.consigneeDistrict, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$CITTAM$$", DocXCM.header.consigneeLocation, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$COUNTRYM$$", DocXCM.header.consigneeCountry, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            #endregion

            #region Dati Destinatario
            doc.ReplaceAll("$$DESTINATARIO$$", DocXCM.header.unLoadDes, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$INDIRIZZO_DESTINATARIO$$", DocXCM.header.unloadAddress, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$CAPD$$", DocXCM.header.unloadZipCode, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$PROVD$$", DocXCM.header.unloadDistrict, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$CITTAD$$", DocXCM.header.unloadLocation, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            doc.ReplaceAll("$$COUNTRYD$$", DocXCM.header.unloadCountry, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            #endregion
            doc.ReplaceAll("$$NOTE$$", "", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);
            #region Produci Righe
            var ZONA = doc.Tables[1].Rows[1].Range;
            for (int i = 1; i < RigheDocumentoAPIXCM.rows.Length; ++i)
            {
                var RR = doc.Tables[1].Rows.InsertAfter(1);
                for (int k = 0; k < doc.Tables[1].Rows[1].Cells.Count; ++k)
                {
                    var SS = doc.GetRtfText(doc.Tables[1].Rows[1].Cells[k].ContentRange);
                    doc.InsertRtfText(RR.Cells[k].ContentRange.Start, SS);
                }
            }
            #endregion

            #region Articoli
            for (int i = 0; i < RigheDocumentoAPIXCM.rows.Length; ++i)
            {
                ZONA = doc.Tables[1].Rows[1 + i].Range;
                var rr = RigheDocumentoAPIXCM.rows[i];
                DateTime dataScadenza = DateTime.MinValue;

              
                doc.ReplaceAll("$$CODPRD$$", rr.partNumber, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                doc.ReplaceAll("$$DESPRD$$", $"{rr.partNumberDes}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                doc.ReplaceAll("$$QTA$$", $"{rr.qty:0.00}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
               

            }
            #endregion

            var saveAs = Path.Combine(finalDest, $"ORD_{docNum.Substring(0, 5)}.pdf");
            if (File.Exists(saveAs))
            {
                var jn = Path.ChangeExtension(saveAs, $"{DateTime.Now.Ticks}.old");
                File.Move(saveAs, jn);
            }
            docs.ExportToPdf(saveAs);
            //docs.SaveDocument(saveAs, DocumentFormat.OpenXml);

            return saveAs;
        }

    }
}
