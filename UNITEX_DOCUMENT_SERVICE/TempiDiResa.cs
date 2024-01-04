using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE
{

	public class ObjectTempiResa
	{
		public static List<ObjectTempiResa> TempiResaHUB = PopolaTempiResaHUB();
		public static List<ObjectTempiResa> TempiResaDisagiate = PopolaTempiResaDisagiate();

		public string CAP { get; set; }

		public string Descrizione { get; set; }

		public string Provincia { get; set; }

		public string Regione { get; set; }

		public int ResaHUBNordMax { get; set; }

		public int ResaHUBNordMin { get; set; }

		public int ResaHUBSudMax { get; set; }

		public int ResaHUBSudMin { get; set; }

		private static List<ObjectTempiResa> PopolaTempiResaDisagiate()
		{
			var Disagiate = new List<ObjectTempiResa>();

			var AnagraficaResiDisagiate = File.ReadAllLines("TempiResaCAPDisagiati.txt");

			foreach (var Riga in AnagraficaResiDisagiate)
			{
				var pcs = Riga.Split(';');
				var SingoloResa = new ObjectTempiResa()
				{
					CAP = pcs[0],
					Descrizione = "",
					Provincia = "",
					Regione = "",
					ResaHUBNordMin = int.Parse(pcs[1]),
					ResaHUBNordMax = int.Parse(pcs[2]),
					ResaHUBSudMin = int.Parse(pcs[3]),
					ResaHUBSudMax = int.Parse(pcs[4]),
				};

				Disagiate.Add(SingoloResa);
			}

			return Disagiate;
		}

		private static List<ObjectTempiResa> PopolaTempiResaHUB()
		{
			var resp = new List<ObjectTempiResa>();

			var AnagraficaResi = File.ReadAllLines("TempiResaHUB.txt");

			foreach (var Riga in AnagraficaResi)
			{
				var pcs = Riga.Split(';');
				var SingoloResa = new ObjectTempiResa()
				{
					CAP = "",
					Descrizione = pcs[0],
					Provincia = pcs[1],
					Regione = pcs[2],
					ResaHUBNordMin = int.Parse(pcs[3]),
					ResaHUBNordMax = int.Parse(pcs[4]),
					ResaHUBSudMin = int.Parse(pcs[5]),
					ResaHUBSudMax = int.Parse(pcs[6]),
				};
				resp.Add(SingoloResa);
			}

			return resp;
		}

		public int[] TempiResa(string CAP, string Provincia, string AgencyCode)
		{
			int[] MinMaxResa = new int[2];

			var isDisagiata = TempiResaDisagiate.FirstOrDefault(x => x.CAP == CAP);

			if (isDisagiata != null && AgencyCode == "01")
			{
				MinMaxResa[0] = isDisagiata.ResaHUBSudMin;
				MinMaxResa[1] = isDisagiata.ResaHUBSudMax;
				return MinMaxResa;
			}
			else if (isDisagiata != null)
			{
				MinMaxResa[0] = isDisagiata.ResaHUBNordMin;
				MinMaxResa[1] = isDisagiata.ResaHUBNordMax;
				return MinMaxResa;
			}

			var Normale = TempiResaHUB.FirstOrDefault(x => x.Provincia.ToLower() == Provincia.ToLower());
			
			if(Normale != null && AgencyCode == "01")
			{
				MinMaxResa[0] = Normale.ResaHUBSudMin;
				MinMaxResa[1] = Normale.ResaHUBSudMax;
				return MinMaxResa;
			}
			else if(Normale != null)
			{
				MinMaxResa[0] = Normale.ResaHUBNordMin;
				MinMaxResa[1] = Normale.ResaHUBNordMax;
				return MinMaxResa;
			}

			else
			{
				MinMaxResa[0] = 0;
				MinMaxResa[1] = 0;
				return MinMaxResa;
			}
		}
	}
}
