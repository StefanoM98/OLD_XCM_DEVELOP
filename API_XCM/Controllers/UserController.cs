using API_XCM.Models;
using DevExpress.Spreadsheet;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace API_XCM.Controllers
{

    public class UserController : ApiController
    {
        // GET: User
        [AllowAnonymous]
        [HttpGet]
        [Route("api/data/forall")]
        public IHttpActionResult Get()
        {
            return Ok("Now server time is: " + DateTime.Now.ToString());
        }

        //[Authorize]
        //[HttpGet]
        //[Route("api/data/authenticate")]
        //public IHttpActionResult GetForAuthenticate()
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    return Ok();
        //}

        //[Authorize(Roles = "admin")]
        //[HttpGet]
        //[Route("api/data/authorize")]
        //public IHttpActionResult GetForAdmin()
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    var roles = identity.Claims
        //                .Where(c => c.Type == ClaimTypes.Role)
        //                .Select(c => c.Value);
        //    return Ok("Hello " + identity.Name + " Role: " + string.Join(",", roles.ToList()));
        //}

    }

}
