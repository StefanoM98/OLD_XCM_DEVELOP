using System;

namespace ZPLLibUtils
{
    public class ZPLLibUtilsClass
    {
        public string _ultimoBCStampato { get; set; }
        public string _ipAddress { get; set; }//= "192.168.2.201";
        public int _port { get; set; }//= 9100;
        public int _qtaDaStampare { get; set; }

        /// <summary>
        /// Provvede all'invio del barcode pallet progressivo ed univoco.
        /// </summary>
        /// <param name="ultimoBCStampato">ultima etichetta pallet stampata es. XCM000000001</param>
        /// <param name="ipAddress">ip della stampante zebra</param>
        /// <param name="port">porta della stampante zebra</param>
        /// <param name="qtaDaStampare">quantità progressiva da stampare</param>
        /// <returns></returns>
        public void stampaBC128Vivisol(string idDocumento)
        {
            CheckValues(idDocumento);
            Stampa(idDocumento);
        }
        public void stampaBC128VivisolNew(string idDocumento, string idPaziente, string numeroInterno)
        {
            CheckValues(idDocumento);
            Stampa(idDocumento, idPaziente, numeroInterno);
        }

        private void Stampa(string barCode, string idPaziente, string numeroInterno)
        {
            string ZPLString =
                  "^XA" +
                  "^FO150,100^BY3" +
                  "^BCN,400,Y,N,N" +
                  $"^FD{barCode}^FS" +
                  "^XZ" +
                  "^FO50,900^GB700,250,3^FS" +
                  "^FO400,900^GB3,250,3^FS" +
                  "^CF0,40" +
                  "^FO100,960^ID Paziente^FS" +
                  $"^FO100,1010^{idPaziente}^FS" +
                  "^CF0,190" +
                  $"^FO470,955^FD{numeroInterno}^FS";

            // Open connection
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            client.Connect(_ipAddress, _port);

            // Write ZPL String to connection
            System.IO.StreamWriter writer = new System.IO.StreamWriter(client.GetStream());
            writer.Write(ZPLString);
            writer.Flush();

            // Close Connection
            writer.Close();
            client.Close();

            System.Threading.Thread.Sleep(100);
        }
        private void Stampa(string barCode)
        {
            string ZPLString =
                   "^XA" +
                   "^FO150,100^BY3" +
                   "^BCN,400,Y,N,N" +
                   $"^FD{barCode}^FS" +
                   "^XZ";

            // Open connection
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            client.Connect(_ipAddress, _port);

            // Write ZPL String to connection
            System.IO.StreamWriter writer = new System.IO.StreamWriter(client.GetStream());
            writer.Write(ZPLString);
            writer.Flush();

            // Close Connection
            writer.Close();
            client.Close();

            System.Threading.Thread.Sleep(100);
        }

        /// <summary>
        /// Verifica che la stringa in ingresso non sia bianca o nulla, inoltre controlla che l'indirizzo ip e la porta siano corrette
        /// </summary>
        /// <param name="valid"></param>
        private void CheckValues(string valid)
        {
            if (string.IsNullOrWhiteSpace(valid))
            {
                throw new Exception("Paramentro in ingresso vuoto o solo spazi");
            }
            else if (string.IsNullOrWhiteSpace(_ipAddress))
            {
                throw new Exception("ipAddress vuoto o solo spazi");
            }
            else if (_port <= 0 && _port.ToString().Length < 4)
            {
                throw new Exception("port <= 0 o lunghezza meno di 4");
            }
        }
    }
}
