using System;

namespace ZebraUtils
{
    public class ZPLLibUtilsClass
    {
        public string _ultimoBCStampato { get; set; }
        public string _ipAddress = "192.168.1.159";
        public int _port = 9100;
        public int _qtaDaStampare { get; set; }

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
                "^FO50,50^GB700,450,3^FS" +
                "^FO150,100^BY3" +
                "^BCN,300,Y,N,N" +
                $"^FD{barCode}^FS" +
                "^FO50,500^GB700,250,3^FS" +
                "^FO400,500^GB3,250,3^FS" +
                "^CF0,40" +
                "^FO160,570^FDPaziente^FS" +
                "^CF0,55" +
                $"^FO80,620^FD{idPaziente}^FS" +
                "^CF0,190" +
                $"^FO470,555^FD{numeroInterno}^FS" +
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
        /// Provvede all'invio del barcode pallet progressivo ed univoco.
        /// </summary>
        /// <param name="ultimoBCStampato">ultima etichetta pallet stampata es. XCM000000001</param>
        /// <param name="ipAddress">ip della stampante zebra</param>
        /// <param name="port">porta della stampante zebra</param>
        /// <param name="qtaDaStampare">quantità progressiva da stampare</param>
        /// <returns></returns>
        public string stampaBC128Vivisol(string ultimoBCStampato, string ipAddress, int port, int qtaDaStampare)
        {

            #region Assegnazione attributi
            this._ultimoBCStampato = ultimoBCStampato;
            this._ipAddress = ipAddress;
            this._port = port;
            this._qtaDaStampare = qtaDaStampare;
            #endregion

            #region CheckValues
            CheckValues(ultimoBCStampato);
            #endregion

            var ultimaStampata = int.Parse(ultimoBCStampato.Substring(3));
            string CodBC128 = "";
            for (int i = 0; i < qtaDaStampare; i++)
            {
                CodBC128 = (ultimaStampata + 1).ToString();
                while (CodBC128.Length < 9)
                {
                    CodBC128 = CodBC128.Insert(0, "0");
                }
                CodBC128 = CodBC128.Insert(0, "XCM");
                ultimaStampata++;
                Stampa(CodBC128);
            }
            return CodBC128;
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
        public void StampaFSC(string barCode)
        {
            var bbs = barCode.Split('\t');
            string ZPLString =
                   "^XA" +
                   "^FO50,10^BY3" +
                   "^BCN,100,Y,N,N" +
                   $"^FD{bbs[0]}^FS" +
                   "^FO50,200^BY3" +
                   "^BCN,100,Y,N,N" +
                   $"^FD{bbs[1]}^FS" +
                   "^FO50,400^BY3" +
                   "^BCN,100,Y,N,N" +
                   $"^FD{bbs[2]}^FS" +
                   "^FO50,600^BY3" +
                   "^BCN,100,Y,N,N" +
                   $"^FD{bbs[3]}^FS" +
                   "^FO450,10^BY3" +
                   "^BCN,100,Y,N,N" +
                   $"^FD{bbs[4]}^FS" +
                   "^FO450,200^BY3" +
                   "^BCN,100,Y,N,N" +
                   $"^FD{bbs[5]}^FS" +
                   "^FO450,400^BY3" +
                   "^BCN,100,Y,N,N" +
                   $"^FD{bbs[6]}^FS" +
                   "^FO450,600^BY3" +
                   "^BCN,100,Y,N,N" +
                   $"^FD{bbs[7]}^FS" +
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

