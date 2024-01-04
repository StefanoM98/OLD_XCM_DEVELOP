using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XCM_Remote_Client
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

            if (Properties.Settings.Default.callUpdate)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.callUpdate = false;
                Properties.Settings.Default.Save();
            }

            Application.Run(new FormWMS());
        }
    }
}
