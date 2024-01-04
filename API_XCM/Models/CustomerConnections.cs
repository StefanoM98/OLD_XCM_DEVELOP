using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_XCM.Models
{
    public static class CustomerConnections
    {
        internal static CustomerSpec GUNA = new CustomerSpec()
        {
            NOME = "GUNA",
            FTP_Address = "185.30.181.203",
            FTP_Port = 21,
            pswFTP = "@guna!",
            userFTP = "guna",
            LocalWorkPath = @"C:\LocalWorkPath\GUNA",
            LocalInFilePath = @"C:\FTP\CLIENTI\GUNA\IN",
            LocalErrorFilePath = @"C:\FTP\CLIENTI\GUNA\ERRORI",
            RemoteINPath = "/IN",
            RemoteOUTPath = "/OUT",
            ID_GESPE = "00213",
            ClienteDaEsitare = false,
            PathEsiti = @"C:\FTP\CLIENTI\GUNA\OUT\ESITI",
            pswAPI = "",
            scadenzaTokenAPI = DateTime.MinValue,
            tokenAPI = "",
            userAPI = "",
            //EsitiDaComunicare = new List<object>()
        };
        internal static CustomerSpec Logistica93 = new CustomerSpec()
        {
            NOME = "LOGISTICA93",
            FTP_Address = "185.30.181.203",
            FTP_Port = 21,
            pswFTP = "@logistica93!",
            userFTP = "logistica93",
            LocalWorkPath = @"C:\LocalWorkPath\LOGISTICA93",
            LocalInFilePath = @"C:\FTP\CLIENTI\LOGISTICA93\IN",
            LocalErrorFilePath = @"C:\FTP\CLIENTI\LOGISTICA93\ERRORI",
            RemoteINPath = @"/IN",
            RemoteINCustomerPath = @"/custom/DOWNLOAD",
            RemoteOUTPath = "/OUT",
            ID_GESPE = "00342",
            ClienteDaEsitare = true,
            PathEsiti = @"C:\FTP\CLIENTI\Logistica93\OUT\ESITI",
            pswAPI = "X2]8,=[d",
            scadenzaTokenAPI = DateTime.MinValue,
            tokenAPI = "",
            userAPI = "lorealAPI",
            sftpAddress = "sftpbizp12.tgms.gxs.com",
            sftpPassword = "XDCLJHFY",
            sftpPort = 22,
            sftpUsername = "ANO47794",
            PathEsitiDelCliente = "/custom/UPLOAD_ESITI",
            //EsitiDaComunicare = new List<object>()
        };
        internal static CustomerSpec PoolPharma = new CustomerSpec()
        {
            NOME = "POOL_PHARMA",
            FTP_Address = "185.30.181.203",
            FTP_Port = 21,
            pswFTP = "@poolpharma!",
            userFTP = "poolpharma",
            LocalWorkPath = @"C:\LocalWorkPath\PoolPharma",
            LocalInFilePath = @"C:\FTP\CLIENTI\PoolPharma\IN",
            LocalErrorFilePath = @"C:\FTP\CLIENTI\PoolPharma\ERRORI",
            RemoteINPath = @"/IN",
            RemoteOUTPath = "/OUT",
            ID_GESPE = "00028",
            ClienteDaEsitare = false,
            PathEsiti = @"C:\FTP\CLIENTI\PoolPharma\OUT\ESITI",
            pswAPI = "",
            scadenzaTokenAPI = DateTime.MinValue,
            tokenAPI = "",
            userAPI = "",
            //EsitiDaComunicare = new List<object>()
        };
        internal static CustomerSpec DLF = new CustomerSpec()
        {
            NOME = "DLF",
            FTP_Address = "185.30.181.203",
            FTP_Port = 21,
            pswFTP = "@dlf!",
            userFTP = "dlf",
            LocalWorkPath = @"C:\LocalWorkPath\DLF",
            LocalInFilePath = @"C:\FTP\CLIENTI\DLF\IN",
            LocalErrorFilePath = @"C:\FTP\CLIENTI\DLF\ERRORI",
            RemoteINPath = @"/IN",
            RemoteOUTPath = "/OUT",
            ID_GESPE = "00027",
            ClienteDaEsitare = false,
            PathEsiti = @"C:\FTP\CLIENTI\DLF\OUT\ESITI",
            pswAPI = "",
            scadenzaTokenAPI = DateTime.MinValue,
            tokenAPI = "",
            userAPI = "",
            //EsitiDaComunicare = new List<object>()
        };
        internal static CustomerSpec STMGroup = new CustomerSpec()
        {
            NOME = "STM_GROUP",
            FTP_Address = "185.30.181.203",
            FTP_Port = 21,
            pswFTP = "@stmgroup!",
            userFTP = "stmgroup",
            LocalWorkPath = @"C:\LocalWorkPath\STM_GROUP",
            LocalInFilePath = @"C:\FTP\CLIENTI\STM_GROUP\IN",
            LocalErrorFilePath = @"C:\FTP\CLIENTI\STM_GROUP\ERRORI",
            RemoteINPath = @"/IN",
            RemoteOUTPath = "/OUT",
            ID_GESPE = "00177",
            ClienteDaEsitare = true,
            PathEsiti = @"C:\FTP\CLIENTI\STM_GROUP\OUT\ESITI",
            pswAPI = "CJ`}H`}h2",
            scadenzaTokenAPI = DateTime.MinValue,
            tokenAPI = "",
            userAPI = "stmAPI",
            sftpAddress = "sftp.stmpharmapro.com",
            sftpPassword = "Vetygkw71-",
            sftpUsername = "Vet_CDL",
            PathEsitiDelCliente = "/OUT",
            sftpPort = 22,
            //EsitiDaComunicare = new List<object>()
        };
        internal static CustomerSpec StockHouse = new CustomerSpec()
        {
            NOME = "STOCK_HOUSE",
            FTP_Address = "185.30.181.203",
            FTP_Port = 21,
            pswFTP = "@stockhouse!",
            userFTP = "stockhouse",
            LocalWorkPath = @"C:\LocalWorkPath\STOCK_HOUSE",
            LocalInFilePath = @"C:\FTP\CLIENTI\STOCK_HOUSE\IN",
            LocalErrorFilePath = @"C:\FTP\CLIENTI\STOCK_HOUSE\ERRORI",
            RemoteINPath = @"/IN",
            RemoteOUTPath = "/OUT",
            ID_GESPE = "00551",
            ClienteDaEsitare = true,
            PathEsiti = @"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT",
            pswAPI = "[YFnvDL8",
            scadenzaTokenAPI = DateTime.MinValue,
            tokenAPI = "",
            userAPI = "stockhouseAPI",
            PathEsitiDelCliente = $@"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT"
            //EsitiDaComunicare = new List<object>()
        };
        internal static CustomerSpec DIFARCO = new CustomerSpec()
        {
            NOME = "DIFARCO",
            FTP_Address = "185.30.181.203",
            FTP_Port = 21,
            pswFTP = "@difarco!",
            userFTP = "difarco",
            LocalWorkPath = @"C:\LocalWorkPath\DIFARCO",
            LocalInFilePath = @"C:\FTP\CLIENTI\DIFARCO\IN",
            LocalErrorFilePath = @"C:\FTP\CLIENTI\DIFARCO\ERRORI",
            RemoteINPath = @"/IN",
            RemoteOUTPath = "/OUT",
            ID_GESPE = "00035",
            ClienteDaEsitare = true,
            PathEsiti = @"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT",
            pswAPI = "[YFnvDL8",
            scadenzaTokenAPI = DateTime.MinValue,
            tokenAPI = "",
            userAPI = "difarcoAPI",
            PathEsitiDelCliente = $@"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT"
            //EsitiDaComunicare = new List<object>()
        };
        internal static CustomerSpec PHARDIS = new CustomerSpec()
        {
            NOME = "PHARDIS",
            FTP_Address = "185.30.181.203",
            FTP_Port = 21,
            pswFTP = "@phardis!",
            userFTP = "phardis",
            LocalWorkPath = @"C:\LocalWorkPath\PHARDIS",
            LocalInFilePath = @"C:\FTP\CLIENTI\PHARDIS\IN",
            LocalErrorFilePath = @"C:\FTP\CLIENTI\PHARDIS\ERRORI",
            RemoteINPath = @"/IN",
            RemoteOUTPath = "/OUT",
            ID_GESPE = "00032",
            ClienteDaEsitare = true,
            PathEsiti = @"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT",
            pswAPI = "[YFnvDL8",
            scadenzaTokenAPI = DateTime.MinValue,
            tokenAPI = "",
            userAPI = "phardisAPI",
            PathEsitiDelCliente = $@"C:\FTP\CLIENTI\CD_GROUP_ESITI\OUT"
            //EsitiDaComunicare = new List<object>()
        };
        internal static CustomerSpec DAMORA = new CustomerSpec()
        {
            NOME = "DAMORA",
            FTP_Address = "185.30.181.203",
            FTP_Port = 21,
            pswFTP = "HR5rMX4W!dkv",
            userFTP = "damora",
            LocalWorkPath = @"C:\LocalWorkPath\DAMORA",
            LocalInFilePath = @"C:\FTP\CLIENTI\DAMORA\IN",
            LocalErrorFilePath = @"C:\FTP\CLIENTI\DAMORA\ERRORI",
            RemoteINPath = @"/IN",
            RemoteOUTPath = "/OUT",
            ID_GESPE = "00237",
            ClienteDaEsitare = true,
            PathEsiti = @"C:\FTP\CLIENTI\DAMORA\OUT\ESITI",
            pswAPI = "Rjbum1w}",
            scadenzaTokenAPI = DateTime.MinValue,
            tokenAPI = "",
            userAPI = "DamoraCAPI",
            PathEsitiDelCliente = "/OUT/Esiti"
        };
        internal static CustomerSpec CHIAPPAROLI = new CustomerSpec()
        {
            NOME = "CHIAPPAROLI",
            FTP_Address = "",
            FTP_Port = 21,
            pswFTP = "",
            userFTP = "",
            LocalWorkPath = @"C:\LocalWorkPath\CHIAPPAROLI",
            LocalInFilePath = @"C:\FTP\CLIENTI\CHIAPPAROLI\IN",
            LocalErrorFilePath = @"C:\FTP\CLIENTI\CHIAPPAROLI\ERRORI",
            RemoteINPath = "/IN",
            RemoteOUTPath = "",
            ID_GESPE = "00327",
            ClienteDaEsitare = true,
            PathEsiti = @"C:\FTP\CLIENTI\CHIAPPAROLI\OUT\ESITI",
            pswAPI = "",
            scadenzaTokenAPI = DateTime.MinValue,
            tokenAPI = "",
            userAPI = "",
            sftpAddress = "sftp.unitexpress.chiapparolispa.it",
            sftpPassword = "v_Yac4BmY",
            sftpUsername = "Unitexpress",
            PathEsitiDelCliente = "/home/Unitexpress/OUT-ESITI",
            sftpPort = 22,
            RemoteINCustomerPath = "/home/Unitexpress/IN"
        };
        internal static CustomerSpec _3CS = new CustomerSpec()
        {
            NOME = "3CS",
            FTP_Address = "185.30.181.203",
            FTP_Port = 21,
            pswFTP = "X66SX4xxt7K$",
            userFTP = "3csrls",
            LocalWorkPath = @"C:\LocalWorkPath\3CS",
            LocalInFilePath = @"C:\FTP\CLIENTI\3CS\IN",
            LocalErrorFilePath = @"C:\FTP\CLIENTI\3CS\ERRORI",
            RemoteINPath = "/IN",
            RemoteOUTPath = "/OUT",
            ID_GESPE = "00558",
            ClienteDaEsitare = true,
            PathEsiti = @"C:\FTP\CLIENTI\3CS\OUT\ESITI",
            pswAPI = "A@q9gO8",
            scadenzaTokenAPI = DateTime.MinValue,
            tokenAPI = "",
            userAPI = "3csrlsAPI",
            sftpAddress = "185.30.181.203",
            sftpPassword = "X66SX4xxt7K$",
            sftpUsername = "3csrls",
            PathEsitiDelCliente = "/ESITI",
            sftpPort = 22,
        };
        internal static CustomerSpec TESTFLUSSI = new CustomerSpec()
        {
            NOME = "TESTFLUSSI",
            FTP_Address = "",
            FTP_Port = 0,
            pswFTP = "",
            userFTP = "",
            LocalWorkPath = @"C:\FTP\CLIENTI\TEST\TEST",
            LocalInFilePath = @"C:\FTP\CLIENTI\TEST\TEST\IN",
            LocalErrorFilePath = @"C:\FTP\CLIENTI\TEST\TEST\ERRORI",
            RemoteINPath = @"",
            RemoteOUTPath = "",
            ID_GESPE = "00026",
            ClienteDaEsitare = false,
            PathEsiti = @"C:\FTP\CLIENTI\TEST\TEST\OUT\ESITI",
            pswAPI = ";`9f`}$B",
            scadenzaTokenAPI = DateTime.MinValue,
            tokenAPI = "",
            userAPI = "TestFlussiInformatici",
        };

        public static List<CustomerSpec> customers = new List<CustomerSpec>() {TESTFLUSSI, CHIAPPAROLI, _3CS, GUNA, Logistica93, PoolPharma, DLF, STMGroup, StockHouse, PHARDIS, DIFARCO, DAMORA };

    }
    public class CustomerSpec
    {
        internal string NOME { get; set; }
        internal string FTP_Address { get; set; }
        internal int FTP_Port { get; set; }
        internal string userFTP { get; set; }
        internal string pswFTP { get; set; }
        internal string LocalWorkPath { get; set; }
        internal string LocalInFilePath { get; set; }
        internal string LocalErrorFilePath { get; set; }
        internal string RemoteINPath { get; set; }
        internal string RemoteOUTPath { get; set; }
        internal string ID_GESPE { get; set; }
        internal bool ClienteDaEsitare { get; set; }
        internal string PathEsiti { get; set; }
        internal string userAPI { get; set; }
        internal string pswAPI { get; set; }
        internal string tokenAPI { get; set; }
        internal DateTime scadenzaTokenAPI { get; set; }
        internal string PathEsitiDelCliente { get; set; }
        internal string sftpAddress { get; set; }
        internal string sftpUsername { get; set; }
        internal string sftpPassword { get; set; }
        internal int sftpPort { get; set; }
        internal string RemoteINCustomerPath { get; set; }
    }
}
