using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoolingFileDaElaborare
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if eDEBUG
            var test = File.ReadAllLines(@"E:\ORD_20210622_175309.txt_20210623124714.txt");

            InterpreteOrdiniPHPH_Header pHPH_Header = new InterpreteOrdiniPHPH_Header();
            pHPH_Header.TestoFileHeader = test[0];
            var phPH_row = new InterpreteOrdiniPHPH_Row();
            phPH_row.TestoFileRow = test[2];

            Debug.WriteLine(string.Join("|", pHPH_Header.FattNome, pHPH_Header.FattIndirizzo, pHPH_Header.FattCitta, pHPH_Header.FattCAP, pHPH_Header.FattProvincia, pHPH_Header.FattPIVA));
            Debug.WriteLine(string.Join("|", phPH_row.DeliveryNO, phPH_row.CodiceArticolo, phPH_row.Lotto, phPH_row.Quantita, phPH_row));
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
