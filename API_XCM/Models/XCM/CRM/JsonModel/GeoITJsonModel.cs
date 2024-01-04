using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.JsonModel
{
    public class GeoITJsonModel
    {
        [Key]
        public long GeoIT_Id { get; set; }
        public string GeoIT_Cap { get; set; }
        public string GeoIT_Localita { get; set; }
        public string GeoIT_Region { get; set; }
        public string GeoIT_Provincia { get; set; }
        public string GeoIT_Nazione { get; set; }
    }
}