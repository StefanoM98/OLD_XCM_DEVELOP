using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNITEX_DOCUMENT_SERVICE.EF;

namespace UNITEX_DOCUMENT_SERVICE.Code
{
    class Helper
    {
        public static decimal GetDecimalFromString(string value, int decimalPlace)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            if (value.Length < decimalPlace)
            {
                return decimal.Parse(value);
            }
            if (value.Contains(","))
            {
                return decimal.Parse(value);
            }
            else
            {
                return decimal.Parse(value.Insert(value.Length - decimalPlace, ","));
            }

        }


        public static string GetRegionByZipCode(string zipCode)
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            var check = entity.GEO.FirstOrDefault(x => x.Geo_ZipCode == zipCode);
            if (check != null)
            {
                return check.Geo_Region;
            }
            return "";
        }

        public static string GetDistrictByZipCode(string zipCode)
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            var check = entity.GEO.FirstOrDefault(x => x.Geo_ZipCode == zipCode);
            if (check != null)
            {
                return check.Geo_District;
            }
            return "";
        }
        public static string StringIntString(string v)
        {
            if (string.IsNullOrEmpty(v))
            {
                return "0";
            }
            else
            {
                var intok = int.TryParse(v, out int p);
                if (intok)
                {
                    return p.ToString();
                }
                else
                {
                    return "0";
                }
            }
        }

        public static string GetPesoChiapparoli(double grossWeight, int v)
        {
            string resp = "";
            var vStr = grossWeight.ToString();
            bool dec = vStr.Contains(",") || vStr.Contains(".");
            if (dec)
            {
                vStr.Replace(",", "");
                vStr.Replace(".", "");
            }
            else
            {
                resp = vStr + "00";
            }

            while (resp.Length < 8)
            {
                resp = resp.Insert(0, "0");
            }

            return resp;
        }

        public static string GetVolumeChiapparoli(double cube, int v)
        {
            string resp = "";
            var vStr = cube.ToString();
            bool dec = vStr.Contains(",") || vStr.Contains(".");
            if (dec)
            {
                vStr.Replace(",", "");
                vStr.Replace(".", "");
            }
            else
            {
                resp = vStr + "00";
            }
            while (resp.Length < 8)
            {
                resp = resp.Insert(0, "0");
            }

            return resp;
        }
    }
}
