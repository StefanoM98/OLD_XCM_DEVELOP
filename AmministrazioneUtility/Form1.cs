using System;
using CommonAPITypes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace AmministrazioneUtility
{
    public partial class Form1 : Form
    {
        string fileViaggi = "ElencoViaggi.txt";
        List<string> ElencoViaggi = new List<string>();
        public Form1()
        {
            InitializeComponent();

            if (!File.Exists(fileViaggi))
            {
                File.AppendAllText(fileViaggi, "");
            }


        }

        private void simpleButtonViaggi_Click(object sender, EventArgs e)
        {
            Process.Start(fileViaggi);
        }

        private void simpleButtonContaSpedizioni_Click(object sender, EventArgs e)
        {
            List<string> Resp = new List<string>();
            var Elenco = File.ReadAllLines(fileViaggi);

            var bb = new CommonAPITypes.ESPRITEC.EspritecLogin.UserLogin()
            {
                Password = "wEmU0ks_",
                Username = "UNITEX_API_ADMIN",
                Tenant = "UNITEX"
            };

            var login = CommonAPITypes.ESPRITEC.EspritecLogin.RestEspritecLogin(bb, false);
            var logDes = JsonConvert.DeserializeObject<CommonAPITypes.ESPRITEC.EspritecLogin.RootobjectResponseLogin>(login.Content);

            if (logDes.user != null && !string.IsNullOrEmpty(logDes.user.token))
            {
                foreach (var viaggio in Elenco)
                {
                    var trip = CommonAPITypes.ESPRITEC.EspritecTripList.RestEspritecGetTripByDocNum(viaggio.Trim(), logDes.user.token);
                    var tripDes = JsonConvert.DeserializeObject<CommonAPITypes.ESPRITEC.EspritecTripList.RootobjectTripList>(trip.Content);

                    if (tripDes.trips != null)
                    {
                        Resp.Add($"{viaggio};{tripDes.trips.OrderBy(x => x.id).LastOrDefault().shipcount}");
                    }
                    else
                    {
                        Resp.Add($"{viaggio};#ND");
                    }

                }
            }
            else
            {
                MessageBox.Show(this, "API NON RAGGIUNGIBILI", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            File.WriteAllLines("ElencoViaggiResult.txt", Resp);
            Process.Start("ElencoViaggiResult.txt");
        }
    }
}
