using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace API_XCM.Code
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

        public static int DiffTraDateEsclusiSabatoEDomeniche(DateTime sdt, DateTime edt)
        {
            int amount = 0;
            int sdayIndex = (int)sdt.DayOfWeek;
            int edayIndex = (int)edt.DayOfWeek;

            amount += (sdayIndex == 0) ? 5 : (6 - sdayIndex);
            amount += (edayIndex == 6) ? 5 : edayIndex;

            sdt = sdt.AddDays(7 - sdayIndex);
            edt = edt.AddDays(-edayIndex);

            if (sdt > edt)
                amount -= 5;
            else
                amount += (edt.Subtract(sdt)).Days / 7 * 5;

            return amount - 1;
        }

        public static DateTime TempiDiResa72(DateTime sdt, DateTime edt)
        {
            var tempiResa = DiffTraDateEsclusiSabatoEDomeniche(sdt, edt) * 24;
            DateTime final = edt;

            var totalDays = tempiResa / 24;

            if (totalDays > 3)
            {
                var sDays = totalDays - 3;



                for (int i = 0; i < sDays; i++)
                {
                    final = final.AddDays(-1);

                    int dayOfWeek = (int)final.DayOfWeek;
                    if (dayOfWeek == 0)
                    {
                        final = final.AddDays(-2);
                    }
                    else if (dayOfWeek == 5)
                    {
                        final = final.AddDays(-1);
                    }
                }

            }

            return final;

        }

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
    }
}