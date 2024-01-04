using API_XCM.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace API_XCM.Controllers
{
    [Authorize]
    public class GeoController : ApiController
    {
        GEO geo = new GEO();

        // GET: Geo
        public string GetRegionName(string provincia)
        {
            return geo.GetRegionNameByDistrict(provincia);
        }

        [AllowAnonymous]
        public string GetRegionNameFromCap(string cap)
        {
            return geo.riceviNomeRegioneDalCap(cap);
        }

        public string GetCapFromDistrict(string district)
        {
            return geo.GetCapByLocationNominatimAPI(district);
        }

        [AllowAnonymous]
        [HttpGet]
        public void FLD()
        {
            //geo.FlagLocalitaDisagiate();
        }
    }
}