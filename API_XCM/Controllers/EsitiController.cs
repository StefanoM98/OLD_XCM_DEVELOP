using API_XCM.Code;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace API_XCM.Controllers
{
    [Authorize]
    public class EsitiController : ApiController
    {
        Helper help = new Helper();

        [Authorize]
        public void GetFile()
        {
            Workbook wk = new Workbook();
            wk.LoadDocument(@"C:\UnitexStorico\test.xlsx");
            Worksheet wksheet = wk.Worksheets[0];

            var docRange = wksheet.GetUsedRange();
            var totRighe = docRange.RowCount;
            var totCol = docRange.ColumnCount;

            List<string> list = new List<string>();

            for (int i = 0; i < totCol; i++)
            {
                var letterIndex = help.GetLetterOfIndex(i);
                var valu = wksheet.Cells[$"{letterIndex}2"].Value.ToString();
                if (help.checkDocNum(valu))
                {
                    list.Add(letterIndex);
                }
            }

        }
    }
}