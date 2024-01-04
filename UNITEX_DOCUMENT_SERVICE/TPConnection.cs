using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE
{
    public static class TPConnection
    {
        internal static TP DAMORA = new TP()
        {
            NOME = "DAMORA",
            FTP_Address = "185.30.181.203",
            FTP_Port = 21,
            psw = "damora",
            user = "damora",
            LocalWorkPath = @"..\..\LocalTestWorkPath\DAMORA",
            RemoteINPath = "/IN",
            RemoteOUTPath = "/OUT",
            ID_FORNITORE_GESPE = "00010"
        };

        internal static TP STURLA = new TP()
        {
            NOME = "STURLA",
            FTP_Address = "185.30.181.203",
            FTP_Port = 21,
            psw = "sturla",
            user = "sturla",
            LocalWorkPath = @"..\..\LocalTestWorkPath\STURLA",
            RemoteINPath = "/IN",
            RemoteOUTPath = "/OUT",
            ID_FORNITORE_GESPE = "00002"
        };

        public static List<TP> TPs = new List<TP>() { DAMORA, STURLA };
    }

    public class TP
    {
        internal string NOME { get; set; }
        internal string FTP_Address { get; set; }
        internal int FTP_Port { get; set; }
        internal string user { get; set; }
        internal string psw { get; set; }
        internal string LocalWorkPath { get; set; }
        internal string RemoteINPath { get; set; }
        internal string RemoteOUTPath { get; set; }
        internal string ID_FORNITORE_GESPE { get; set; }
    }
}
