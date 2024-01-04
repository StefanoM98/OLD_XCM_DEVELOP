using API_XCM.Code;
using API_XCM.Code.APIs;
using API_XCM.Models;
using CommonAPITypes.UNITEX;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace API_XCM.Controllers
{
	[Authorize]
	public class UnitexController : ApiController
	{

		[HttpGet]
		public List<TmsTripListTrip> GetTrips(string supplierId)
		{
			return UNITEX.GetTrips().Where(x => x.supplierID == supplierId).ToList();
		}

		[HttpGet]
		public List<string> GetGLSFileContent(string tripNumber)
		{
			return UNITEX.GLS(tripNumber);
		}
	}
}
