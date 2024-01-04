using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolingFileDaElaborare
{
    public class PdfClass
    {
        #region iTextSharp
        public static string ReadPDF(string pdfPath)
        {
            var resp = new List<string>();
            string strText = string.Empty;
            try
            {
                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(pdfPath);

                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
                    string s = PdfTextExtractor.GetTextFromPage(reader, page, its);

                    s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                    strText += s;
                }
                reader.Close();


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return strText;
        }
        #endregion

    }
}
