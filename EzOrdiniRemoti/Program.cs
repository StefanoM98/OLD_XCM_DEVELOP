using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EzOrdiniRemoti
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (SingleApplication.IsAlreadyRunning())
            {
                SingleApplication.SwitchToCurrentInstance();
                Environment.Exit(0);
            }

            Application.Run(new FormOrdini());
        }
    }
}
