using API_XCM.Code;
using API_XCM.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace API_XCM.Controllers
{
    public class FscController : ApiController
    {
        FSC fsc = new FSC();

        [Authorize]
        [HttpPost]
        public List<InterpreteFSC> PostShips(List<InterpreteFSC> nuoveRighe)
        {
            return fsc.ParseShipments(nuoveRighe);
        }
    }

}
