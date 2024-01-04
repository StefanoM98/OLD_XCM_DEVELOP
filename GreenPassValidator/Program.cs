using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenPassValidator
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);

            if (SingleApplication.IsAlreadyRunning())
            {
                SingleApplication.SwitchToCurrentInstance();
                Environment.Exit(0);
            }



            //Application.Run(new Form1()); //Dispositivo di lettura QR
            Application.Run(new GestioneControlloAccessi()); //Impostazione Dario 
            //Application.Run(new GeneratoreQRCode());//Generatore QR
        }
    }
}
