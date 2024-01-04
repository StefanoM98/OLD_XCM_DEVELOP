using System;
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

namespace MovimentiMagazzinoFromGespe
{
	public partial class FormPalletIN : Form
	{

		#region AttributiGlobali
		List<PALLET_IN> DataLoad = new List<PALLET_IN>();
		#endregion

		public FormPalletIN()
		{
			InitializeComponent();
		}

		private void FormPalletIN_Load(object sender, EventArgs e)
		{
			buttonMeseScorso_Click(null, null);
			AggiornaDati();


		}

		private void AggiornaDati()
		{
			try
			{
				gridViewPalletIN.BeginDataUpdate();
				DataLoad.Clear();
				var db = new XCM_WMSEntities();
				var dl = db.PALLET_IN.ToList();
				DataLoad = dl.Where(x => x.DATA_INSERIMENTO >= dateEditAccessiDal.DateTime &&
															x.DATA_INSERIMENTO <= dateEditAccessiAl.DateTime).ToList();
				gridControlPalletIN.DataSource = DataLoad;
			}
			finally
			{
				gridViewPalletIN.EndDataUpdate();
			}




		}

		#region ButtonEvent
		private void buttonMeseScorso_Click(object sender, EventArgs e)
		{
			var DataDelMeseScorso = DateTime.Now.AddMonths(-1);
			var DataDal = new DateTime(DataDelMeseScorso.Year, DataDelMeseScorso.Month, 1);

			var dM = DateTime.DaysInMonth(DataDal.Year, DataDal.Month);
			var DataAl = new DateTime(DataDal.Year, DataDal.Month, dM, 23, 59, 59);


			dateEditAccessiDal.DateTime = DataDal;
			dateEditAccessiAl.DateTime = DataAl;
			AggiornaDati();
		}

		private void buttonOggi_Click(object sender, EventArgs e)
		{
			var DataOggi = DateTime.Today;

			dateEditAccessiDal.DateTime = DataOggi;
			dateEditAccessiAl.DateTime = new DateTime(DataOggi.Year, DataOggi.Month, DataOggi.Day, 23, 59, 59);
			AggiornaDati();
		}

		private void buttonMeseCorrente_Click(object sender, EventArgs e)
		{
			var DataDelMeseCorrente = DateTime.Now;

			dateEditAccessiDal.DateTime = new DateTime(DataDelMeseCorrente.Year, DataDelMeseCorrente.Month, 1);
			dateEditAccessiAl.DateTime = new DateTime(DataDelMeseCorrente.Year, DataDelMeseCorrente.Month, DataDelMeseCorrente.Day, 23, 59, 59);
			AggiornaDati();
		}
		#endregion

		private void buttonAggiornaDati_Click(object sender, EventArgs e)
		{
			AggiornaDati();
		}

		private void simpleButtonEsportaXslx_Click(object sender, EventArgs e)
		{
			var dsk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			var dest = Path.Combine(dsk, "Export_Pallet_IN");
			if (!Directory.Exists(dest))
			{
				Directory.CreateDirectory(dest);
			}
			var finalDest = Path.Combine(dest, $"Export_{DateTime.Now.ToString("ddMMyyyy")}.xlsx");
			gridViewPalletIN.ExportToXlsx(finalDest);
			Process.Start(dest);
		}
	}
}
