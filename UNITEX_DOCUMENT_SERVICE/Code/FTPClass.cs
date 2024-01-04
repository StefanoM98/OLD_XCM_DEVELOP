using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;

namespace UNITEX_DOCUMENT_SERVICE.Code
{
    public class FTPClass
    {
        public FtpClient _clientFTP = null;
        public FtpClient CreaClientFTP(string usr, string passw)
        {
            //var indirizzoFTP = Properties.Settings.Default.IndirizzoFTP;
            //if (string.IsNullOrEmpty(indirizzoFTP))
            //{
            //    MessageBox.Show("Indirizzo FTP non configurato, impossibile continuare", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //var userFTP = Properties.Settings.Default.UserFTP;
            //if (string.IsNullOrEmpty(userFTP))
            //{
            //    MessageBox.Show("User FTP non configurata, impossibile continuare", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //var passwordFTP = Properties.Settings.Default.PasswordFTP;
            //if (string.IsNullOrEmpty(passwordFTP))
            //{
            //    MessageBox.Show("Password FTP non configurata, impossibile continuare", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            _clientFTP = new FtpClient
            {
                Host = "185.30.181.203",
                Port = 21,
                Credentials = new System.Net.NetworkCredential(usr, passw)
            };
            _clientFTP.ValidateCertificate += OnValidateCertificate;
            _clientFTP.DataConnectionType = FtpDataConnectionType.PASV;

            _clientFTP.Connect();

            if (_clientFTP.IsConnected)
            {
                return _clientFTP;
            }
            else
            {
                return null;
            }
        }

        private void OnValidateCertificate(FtpClient control, FtpSslValidationEventArgs e)
        {
            e.Accept = true;
        }
    }
}
