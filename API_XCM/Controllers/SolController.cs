using API_XCM.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace API_XCM.Controllers
{
    [Authorize(Roles = "Client")]
    public class SolController : ApiController
    {
        // GET: /api/sol/GetDocuments?dataDa=01/01/2014
        //public List<DocumentList> GetDocuments(DateTime dataDa)
        //{
        //    return VIVISOL.GetVivisolDocuments(dataDa);
        //}
    }
}