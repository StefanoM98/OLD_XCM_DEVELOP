using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XCM_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenzaRemoteClientXCMController : ControllerBase
    {      

        private readonly ILogger<LicenzaRemoteClientXCMController> _logger;

        public LicenzaRemoteClientXCMController(ILogger<LicenzaRemoteClientXCMController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string GetLicenzaRemoteClientXCM(string UID)
        {
            //controlla scadenza licenza UID da DB
            return DateTime.Now.ToLongDateString() + "\n" + UID;
        }
    }
}
