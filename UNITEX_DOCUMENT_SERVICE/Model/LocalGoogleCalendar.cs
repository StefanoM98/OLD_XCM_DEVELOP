using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace UNITEX_DOCUMENT_SERVICE.Model
{
    public static class LocalGoogleCalendar
    {
        static List<string> GiorniNonFestiviSummary = new List<string>() { "san silvestro", "mercoledì delle ceneri", "festa del papà", "venerdi santo", "la festa della mamma", "pasqua" };
        internal static readonly string API_KEY = "AIzaSyA9QlsfHF8N0F07vIQyX1e0Byt_kgP3No4";
        internal static readonly string CalendarID = "it.italian#holiday@group.v.calendar.google.com";
        public static Events CalendarioGoogle = null;

        public static DateTime CalcolaDataConsegnaPrevista(DateTime dataDa, int resaMax)
        {
            var dataA = dataDa.AddHours(resaMax);
            var ResponseList = CalendarioGoogle.Items.Where(x => Convert.ToDateTime(x.Start.Date) >= dataDa && Convert.ToDateTime(x.End.Date) <= dataA).ToList();
            var festivitaTraLeDateL = ResponseList.Where(x => !GiorniNonFestiviSummary.Contains(x.Summary.ToLower())).GroupBy(x => x.Start.Date).ToList();

            int festivitaDaConsiderare = festivitaTraLeDateL.Count();

            List<DateTime> Days = new List<DateTime>();
            int DaysDifference = (int)new TimeSpan((dataA - dataDa).Ticks).TotalDays;
            for (int i = 1; i <= DaysDifference; i++)
            {
                var Day = dataDa.AddDays(i);               
                Days.Add(Day);
            }

            int SabEDom = Days.Where(x => x.DayOfWeek == DayOfWeek.Sunday || x.DayOfWeek == DayOfWeek.Saturday).Count();

            int giorniStimaConsegna = (int)(resaMax/24) + SabEDom + festivitaDaConsiderare;

            return dataDa.AddDays(giorniStimaConsegna);
        }

        public static int GiorniDiResaEffettivi(DateTime dataDa, DateTime dataA)
        {
            if(CalendarioGoogle == null)
            {
                CalendarService service = new CalendarService(new BaseClientService.Initializer()
                {
                    ApiKey = LocalGoogleCalendar.API_KEY,
                    ApplicationName = "Test"
                });

                var request = service.Events.List(LocalGoogleCalendar.CalendarID);
                LocalGoogleCalendar.CalendarioGoogle = request.Execute();
            }

            var ResponseList = CalendarioGoogle.Items.Where(x => Convert.ToDateTime(x.Start.Date) >= dataDa && Convert.ToDateTime(x.End.Date) <= dataA).ToList();
            var festivitaTraLeDateL = ResponseList.Where(x => !GiorniNonFestiviSummary.Contains(x.Summary.ToLower())).GroupBy(x => x.Start.Date).ToList();

            int festivitaDaNonConsiderare = 0;

            foreach (var fs in festivitaTraLeDateL)
            {
                var dt = DateTime.MinValue;
                var day = DateTime.TryParse(fs.Key, out dt);

                if (dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    festivitaDaNonConsiderare += 1;
                }

            }

            int festivitaTraLeDate = festivitaTraLeDateL.Count() - festivitaDaNonConsiderare;
            List<DateTime> Days = new List<DateTime>();
            //Days.Add(dataDa);
            int DaysDifference = (int)new TimeSpan((dataA - dataDa).Ticks).TotalDays;
            for (int i = 1; i <= DaysDifference; i++)
            {
                var Day = dataDa.AddDays(i);
                Days.Add(Day);
            }

            var WorkDays = Days.Where(x => x.DayOfWeek != DayOfWeek.Saturday && x.DayOfWeek != DayOfWeek.Sunday && x.Date != dataDa.Date).Count();

            int GiorniReso = WorkDays - festivitaTraLeDate;
            if (GiorniReso == 0)
            {
                GiorniReso = 1;
            }
            return GiorniReso;
        }
    }
}
