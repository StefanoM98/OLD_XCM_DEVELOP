using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitexRemoteClient
{
    public static class ConnectionManager
    {
        public static string userAPI = "";
        public static string pswAPI = "";
        public static string tokenAPI = "";
        public static string CustomerGespeID = "";
        public static DateTime scadenzaToken = DateTime.MinValue;
        public static bool firsRun = true;

        public static bool RecuperaConnessione()
        {
            bool connessioneUser = true;

            CaricaCredenzialiAPI();
            if(firsRun)
            {
                return false;
            }
            if (scadenzaToken - DateTime.Now < TimeSpan.FromHours(1))
            {
                var cred = EseguiLoginAPIUnitex();
                if (cred != null)
                {
                    var credDes = JsonConvert.DeserializeObject<CommonAPITypes.ESPRITEC.EspritecLogin.RootobjectResponseLogin>(cred.Content);
                    if (!credDes.result.status)
                    {
                        connessioneUser = false;
                    }
                    else
                    {
                        tokenAPI = credDes.user.token;
                        scadenzaToken = credDes.user.expire;
                        CustomerGespeID = credDes.user.filter;
                        connessioneUser = true;
                    }
                }
            }
            return connessioneUser;
        }
        private static void CaricaCredenzialiAPI()
        {
            if (firsRun)
            {
                var pass = cry.Decrypt(Properties.Settings.Default.PF);
                var usr = cry.Decrypt(Properties.Settings.Default.UF);
                FormLogin fl = new FormLogin(usr, pass);
                fl.ShowDialog();
                if (fl.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    firsRun = false;
                }
            }
        }
        private static RestSharp.IRestResponse EseguiLoginAPIUnitex()
        {
            var bb = new Utilities.loginXcmCredentials
            {
                password = pswAPI,
                username = userAPI,
                tenant = "UNITEX"
            };

            return CommonAPITypes.ESPRITEC.EspritecLogin.RestEspritecLogin(bb, false);
        }

        internal static void SalvaInfoCryptate(string userAPI, string pswdAPI)
        {
            Properties.Settings.Default.UF = cry.Encrypt(userAPI);
            Properties.Settings.Default.PF = cry.Encrypt(pswdAPI);
            Properties.Settings.Default.Save();
        }
    }
}
