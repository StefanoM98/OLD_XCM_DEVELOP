using API_XCM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using NLog;
using API_XCM.Code;
using DevExpress.Web.Mvc;
using System.IO;
using DevExpress.Web;
using System.Web.UI;
using API_XCM.Models.XCM;

namespace API_XCM.Controllers
{
    [Authorize]
    public class XcmController : ApiController
    {
        #region Logger
        internal static Logger _logger = LogManager.GetLogger("loggerCode");
        #endregion
        XCM xcm = new XCM();

        // GET api/xcm/getships?GespeDocNum=01044/TR
        [HttpGet]
        public bool GetShips(string GespeDocNum)
        {
            return xcm.GetShipments(GespeDocNum);
        }

        // GET api/xcm/gettrips?dts=05/05/2022
        [HttpGet]
        public List<TripXCM> GetTrips(string dts)
        {
            return xcm.GetTrips(dts);
        }

        // GET api/xcm/GetDocumentsInOutYesterday
        //[HttpGet]
        //public List<ResocontoDocumentiInOutModel> GetDocumentsInOutYesterday()
        //{
        //    return xcm.GetResocontoDocumenti();
        //}

        [HttpGet]
        public string GetSettaValoriRighaDocumentoOrdine(int rowID, decimal price)
        {
            return xcm.SettaValoriRighaDocumentoOrdine(rowID, price);
        }

        [HttpGet]
        public string GetReportNino()
        {
            try
            {
                var model = xcm.GetDocs();
                var response = "";

                if (model.Count() == 0)
                {
                    return response;
                }
                var test = GridViewExtension.ExportToXlsx(CreateMasterGridViewSettings(), model);

                var filePath = @"C:\XCM\StoricoReportNino";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                var fileName = $"Report_Non_Evasi_{DateTime.Now.ToString("dd-MM-yyyy")}.xlsx";

                if (File.Exists(Path.Combine(filePath, fileName)))
                {
                    File.Delete(Path.Combine(filePath, fileName));
                }

                using (var fileStream = System.IO.File.Create(Path.Combine(filePath, fileName)))
                {
                    var fileStreamResult = (System.Web.Mvc.FileStreamResult)test;
                    fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
                    fileStreamResult.FileStream.CopyTo(fileStream);
                    fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin); //reset position to beginning. If there's any chance the FileResult will be used by a future method, this will ensure it gets left in a usable state - Suggestion by Steven Liekens
                }

                response = $"{Path.Combine(filePath, fileName)};{GenerateHtmlTable(model)}";

                return response;
            }
            catch (Exception ee)
            {
                _logger.Error(ee);
            }
            return "";

        }

        private GridViewSettings CreateMasterGridViewSettings()
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = "masterGrid";
            settings.CallbackRouteValues = new { Controller = "Nino", Action = "MasterGridPartial" };

            settings.KeyFieldName = "GespeID";

            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);


            settings.SettingsExport.EnableClientSideExportAPI = true;
            settings.SettingsExport.ExcelExportMode = DevExpress.Export.ExportType.WYSIWYG;
            settings.SettingsDetail.ShowDetailRow = true;
            settings.SettingsDetail.ExportMode = GridViewDetailExportMode.All;

            settings.SettingsExport.FileName = "Report.xlsx";

            settings.SettingsExport.GetExportDetailGridViews = (s, e) =>
            {
                string customerID = (string)DataBinder.Eval(e.DataItem, "GespeID");
                GridViewExtension grid = new GridViewExtension(API_XCM.Code.XCM.CreateGeneralDetailGridSettings(customerID));
                grid.Bind(API_XCM.Code.XCM.GetResocontoDocumentiNonSpeditiDaIDGespe(customerID));
                e.DetailGridViews.Add(grid);
            };

            settings.Columns.Add(c =>
            {
                c.FieldName = "GespeID";
                c.Visible = false;
            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "Mandante";
                c.Caption = "Mandante";

            });

            settings.Columns.Add(c =>
            {
                c.FieldName = "Evasi";
                c.Caption = "Evasi";
                c.Width = System.Web.UI.WebControls.Unit.Percentage(10);
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "NonEvasi";
                c.Caption = "Non evasi";
                c.Width = System.Web.UI.WebControls.Unit.Percentage(10);

            });

            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;

            settings.SettingsExport.RenderBrick = (sender, e) =>
            {
                if (e.Column.Grid.ID.StartsWith("detailGrid"))
                {
                    return;
                }
                else
                {
                    if (e.RowType == GridViewRowType.Data)
                    {
                        e.BrickStyle.BackColor = System.Drawing.Color.Navy;
                        e.BrickStyle.ForeColor = System.Drawing.Color.White;
                    }
                }
            };

            return settings;
        }

        private string GenerateHtmlTable(List<Docs> model)
        {
            var htmlTable = @"<table><tr><th>Mandante</th><th>Evasi</th><th>Non Evasi</th></tr>";
            model = model.OrderBy(x => x.NonEvasi).ToList();
            foreach (var doc in model)
            {
                var htmlRow = $@"<tr><td>{doc.Mandante}</td><td>{doc.Evasi}</td><td>{doc.NonEvasi}</td></tr>";
                htmlTable = htmlTable + htmlRow;
            }
            return htmlTable + @"</table>";
        }
    }
}
