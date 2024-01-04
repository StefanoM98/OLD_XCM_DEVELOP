using DevExpress.XtraRichEdit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitexFSC
{
    class Bordero
    {
        private string TemplateDocx = @"C:\UnitexStorico\BorderoFSC.docx";
        
        public RichEditDocumentServer produciBorderoFSC(List<InterpreteFSC> righeFinali)
        {
            API api = new API();
            righeFinali.OrderBy(x => x.NumeroDocumento);
            RichEditDocumentServer docs = new RichEditDocumentServer();
            docs.Document.LoadDocument(TemplateDocx);
            var doc = docs.Document;

            docs.LoadDocument(TemplateDocx);

            foreach (var s in doc.Sections)
            {
                s.Page.PaperKind = System.Drawing.Printing.PaperKind.A4;
            }
            doc.ReplaceAll("$$DATEREFERENCE$$", DateTime.Now.ToString("dd/MM/yyyy"), DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive);

            #region Produci Righe
            var ZONA = doc.Tables[0].Rows[1].Range;
            for (int i = 1; i < righeFinali.Count; ++i)
            {
                var RR = doc.Tables[0].Rows.InsertAfter(1);
                for (int k = 0; k < doc.Tables[0].Rows[1].Cells.Count; ++k)
                {
                    var SS = doc.GetRtfText(doc.Tables[0].Rows[1].Cells[k].ContentRange);
                    doc.InsertRtfText(RR.Cells[k].ContentRange.Start, SS);
                }
            }
            #endregion

            for(int i = 0; i < righeFinali.Count; ++i)
            {
                ZONA = doc.Tables[0].Rows[1 + i].Range;
                var rr = righeFinali[i];
                var region = api.GetRegionName(rr.Provincia).Replace("\"","").ToUpper();
                doc.ReplaceAll("$$RIF$$", rr.NumeroDocumento, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                doc.ReplaceAll("$$REGIONE$$", region, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                doc.ReplaceAll("$$DES$$", rr.NomeClienteFornitore, DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                doc.ReplaceAll("$$ADDRESS$$", $"{rr.Indirizzo} {rr.CAP}\r\n{rr.Citta} {rr.Provincia}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                doc.ReplaceAll("$$COLLI$$", $"{rr.Colli}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
                doc.ReplaceAll("$$PALLET$$", $"{rr.Pallet}", DevExpress.XtraRichEdit.API.Native.SearchOptions.CaseSensitive, ZONA);
               
            }
            var fn = $"BORDERO_FSC_{DateTime.Now.ToString("yyyyMMdd_HHmmssffff")}.pdf";
            var outDir = @"C:\UnitexStorico\Clienti\FSC\OUT\";
            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }
            var saveAs = Path.Combine(outDir, fn);

            //docs.ExportToPdf(saveAs);
            return docs;
        }
    }
}
