using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes.XCM
{
	public class CustomersXCM
	{
		public static CustomerSpec TESTFLUSSI = new CustomerSpec()
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
			pswAPI = "YG5W1mu^",
			scadenzaTokenAPI = DateTime.MinValue,
			tokenAPI = "",
			userAPI = "testflussiAPI",
			EsitiDaRaddrizzare = false
		};

		public static CustomerSpec IRENAERIS = new CustomerSpec()
		{
			NOME = "IrenaEris",
			FTP_Address = "94.152.11.24",
			FTP_Port = 21,
			userFTP = "ita",
			pswFTP = "v1Q!f9J%v4",
			LocalWorkPath = @"C:\FTP\IRENA\IN\WorkPath\",
			LocalInFilePath = @"C:\FTP\IRENA\IN\",
			LocalOUTFilePath = @"C:\FTP\IRENA\IN\CSV\",
			LocalErrorFilePath = @"C:\FTP\IRENA\IN\Errori\",
			LocalBackupPath = @"C:\FTP\IRENA\IN\Originali\",
			RemoteINPath = @"/OUT",
			RemoteOUTPath = "/IN",
			ID_GESPE = "00031",
			ClienteDaEsitare = false,
			PathEsiti = @"",
			pswAPI = "Irena$Api!5",
			scadenzaTokenAPI = DateTime.MinValue,
			tokenAPI = "",
			userAPI = "IRENA_Api",
			EsitiDaRaddrizzare = false
		};

	}

	public class CustomerSpec
	{
		public string NOME { get; set; }
		public string FTP_Address { get; set; }
		public int FTP_Port { get; set; }
		public string userFTP { get; set; }
		public string pswFTP { get; set; }
		public string LocalWorkPath { get; set; }
		public string LocalInFilePath { get; set; }
		public string LocalOUTFilePath { get; set; }
		public string LocalErrorFilePath { get; set; }
		public string LocalBackupPath { get; set; }
		public string RemoteINPath { get; set; }
		public string RemoteOUTPath { get; set; }
		public string ID_GESPE { get; set; }
		public bool ClienteDaEsitare { get; set; }
		public string PathEsiti { get; set; }
		public string userAPI { get; set; }
		public string pswAPI { get; set; }
		public string tokenAPI { get; set; }
		public DateTime scadenzaTokenAPI { get; set; }
		public string PathEsitiDelCliente { get; set; }
		public string sftpAddress { get; set; }
		public string sftpUsername { get; set; }
		public string sftpPassword { get; set; }
		public int sftpPort { get; set; }
		public string RemoteINCustomerPath { get; set; }
		public bool EsitiDaRaddrizzare { get; set; }
	}
}
