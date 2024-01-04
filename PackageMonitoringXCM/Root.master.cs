using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DevExpress.Web;

namespace PackageMonitoringXCM
{
    public partial class Root : MasterPage {
        protected void Page_Load(object sender, EventArgs e) {
            if(!string.IsNullOrEmpty(Page.Header.Title))
                Page.Header.Title += " - ";
            Page.Header.Title = Page.Header.Title + "XCM HealthCare";

            Page.Header.DataBind();

        }

    }
}