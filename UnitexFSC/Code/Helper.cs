using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace UnitexFSC.Code
{
    public class Helper
    {
        public static string ripulisciStringa(string str, string replaceWith)
        {
            var rx = @"[^0-9a-zA-Z.]+";
            return Regex.Replace(str, rx, replaceWith);
        }

        public string GetLetterOfIndex(int indice)
        {
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            return alpha[indice].ToString();
        }

        public bool checkDocNum(string str)
        {
            var rx = @"\d{3,10}/SH$";
            return Regex.IsMatch(str, rx);
        }

    }
}