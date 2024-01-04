using Newtonsoft.Json;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace XCM_REPORT_SERVICE
{
    public class Automazione
    {

        Exception LastException = new Exception("AVVIO");
        DateTime DateLastException = DateTime.MinValue;
        DateTime LastCheckChangesTMS = DateTime.MinValue;

        #region Logger
        internal static Logger _loggerCode = LogManager.GetLogger("loggerCode");
        internal static Logger _loggerAPI = LogManager.GetLogger("LogAPI");
        #endregion

        Timer timerAggiornamentoCiclo = new Timer();
        double cicloTimer = 300000;

        API xcmAPI = new API();
        public static string config = "config.ini";

        public void Start()
        {
            //CaricaConfigurazioni();
            //SetTimer();
            OnTimedEvent(null, null);
        }
        private void CaricaConfigurazioni()
        {
            var conf = File.ReadAllLines(config);
            cicloTimer = double.Parse(conf[5]);
        }
        private void SetTimer()
        {
#if DEBUG
            timerAggiornamentoCiclo = new Timer(60000);
            //controlloDocumentiInCarico = new Timer(60000);
#else
            timerAggiornamentoCiclo = new Timer(cicloTimer);
            //controlloDocumentiInCarico = new Timer(3600000);

#endif

            timerAggiornamentoCiclo.Elapsed += OnTimedEvent;
            timerAggiornamentoCiclo.AutoReset = true;
            timerAggiornamentoCiclo.Enabled = true;
            //SW.Start();
            //controlloDocumentiInCarico.AutoReset = true;
            //controlloDocumentiInCarico.Enabled = true;
            _loggerCode.Info($"Timer ciclo settato {timerAggiornamentoCiclo.Interval} ms");

        }

        public bool InvioReport = true;
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            timerAggiornamentoCiclo.Stop();

            try
            {

                ReportNino();

            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);

                if (ee.Message != LastException.Message || DateLastException + TimeSpan.FromHours(1) < DateTime.Now)
                {
                    DateLastException = DateTime.Now;
                    GestoreMail.SegnalaErroreDev("OnTimedEvent", ee);
                }

                LastException = ee;
            }
            finally
            {
                timerAggiornamentoCiclo.Start();
            }
        }
        public void ReportNino()
        {
            DateTime scheduledRun = DateTime.Today.AddHours(8).AddMinutes(30);
            var filePath = @"C:\XCM\StoricoReportNino\lastRunTime.txt";
            DateTime lastRan = Convert.ToDateTime(File.ReadAllText(filePath));
            if (DateTime.Now > scheduledRun)
            {
                TimeSpan sinceLastRun = DateTime.Now - lastRan;
                
                if (sinceLastRun.TotalDays > 1)
                {
                    var response = xcmAPI.GetReportNino();
                    if (!string.IsNullOrEmpty(response))
                    {
                        var now = DateTime.Now.ToString();
                        File.WriteAllText(filePath, now);
                        _loggerAPI.Info($"Invio effettuato alle {now}");
                    }

                }
                else
                {
                    _loggerAPI.Info($"Ultimo invio {lastRan}");
                }

            }
        }
        public void Stop()
        {
            timerAggiornamentoCiclo.Stop();
        }
    }
}
