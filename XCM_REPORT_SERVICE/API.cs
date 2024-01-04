using Newtonsoft.Json;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace XCM_REPORT_SERVICE
{
    class API
    {

        //public string endpointAPI_XCM = $"https://localhost:44327";//$"https://localhost:44327";
        public string endpointAPI_XCM = $"http://192.168.2.254:8092";
        //public string endpointAPI_XCM = $"http://185.30.181.192:8092";

        #region Logger
        internal static Logger _loggerCode = LogManager.GetLogger("loggerCode");
        internal static Logger _loggerAPI = LogManager.GetLogger("LogAPI");
        #endregion

        private string accessToken { get; set; }
        public string GetAccessToken()
        {
            if (this.accessToken != null)
            {
                return this.accessToken;

            }
            else
            {
                return Login();
            }
        }

        private string Login()
        {
            try
            {
                _loggerAPI.Info($"Invio richiesta di login su API XCM");
                var client = new RestClient(this.endpointAPI_XCM + "/token");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);

                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("username", "gaetano.colella@xcmhealthcare.com");
                request.AddParameter("password", "Deni1112!");
                request.AddParameter("grant_type", "password");

                IRestResponse response = client.Execute(request);
                var resp = JsonConvert.DeserializeObject<LoginResponse>(response.Content);
                if (resp != null)
                {
                    this.accessToken = resp.access_token;
                    _loggerAPI.Info($"Nuovo token: {this.accessToken}");
                    return accessToken;

                }
                else
                {
                    return "";
                }


            }
            catch (Exception ee)
            {
                _loggerCode.Error(ee);
                GestoreMail.SegnalaErroreDev("Login", ee);

                return "";
            }
        }

        public string GetReportNino()
        {
            Login();
            var client = new RestClient(endpointAPI_XCM + $"/api/xcm/getreportnino");
            client.Timeout = -1;

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {this.GetAccessToken()}");
            

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {

                var resp = response.Content.ToString().Split(';');
                if(resp.Length > 1)
                {
                    var pathFile = resp[0];
                    var htmlTable = resp[1];
                    this.InviaEmail("gaetano.colella@xcmhealthcare.com", pathFile.Replace("\"", ""), htmlTable);
                    return resp[0];

                }
                else
                {
                    return "";
                }
            }
            else
            {
                _loggerCode.Error(response.StatusCode); ;
                return "";
            }
        }

        private static string SenderMail = "itsupport@xcmhealthcare.com";
        private static string SenderMailPassword = "AdminIT.Manager";
        private static string SenderMailName = "XCM Healthcare";
        private static string SenderSmtpHost = "smtp.gmail.com";
        private static int SenderSmtpPort = 587;

        private void InviaEmail(string mailTo, string pathFile, string htmlTable)
        {

            string subject = $"Resoconto giornaliero";
            var body = 
$@"Gentile Gaetano Colella,
di seguito il report del giorno {DateTime.Today.ToShortDateString()} per quanto concerne gli ordini evasi e non evasi.
<br><br>
{htmlTable}
<br><br>
In allegato il dettaglio per quanto concerne gli ordini non evasi.
<br><br>
Se desidera può controllare in tempo reale l'avanzamento del report al seguente indirizzo <a href='http://185.30.181.192:8092'>Portale Web XCM</a>";
            var fromAddress = new MailAddress(SenderMail, SenderMailName);

            try
            {
                var mails = mailTo.Split(',').ToList();

                foreach (var mail in mails)
                {
                    if (!string.IsNullOrWhiteSpace(mail))
                    {
                        var toa = new MailAddress(mail);
                        var smtp = new SmtpClient
                        {
                            Host = SenderSmtpHost,
                            Port = SenderSmtpPort,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(SenderMail, SenderMailPassword)
                        };

                        var message = new MailMessage(fromAddress, toa);

                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;


                        message.Attachments.Add(new Attachment(pathFile));
                        

                        smtp.Send(message);
                    }

                }
            }
            catch (Exception SendMailException)
            {
                _loggerCode.Error(SendMailException);
                GestoreMail.SegnalaErroreDev("InviaEmail", SendMailException);
            }
        }

    }
}
