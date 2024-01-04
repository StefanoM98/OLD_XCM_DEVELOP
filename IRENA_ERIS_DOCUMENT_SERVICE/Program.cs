using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace IRENA_ERIS_DOCUMENT_SERVICE
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(configure =>
            {
                configure.Service<Automazione>(service =>
                {
                    service.ConstructUsing(s => new Automazione());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                configure.SetServiceName("XCM_IRENA_ERIS");
                configure.SetDisplayName("XCM_IRENA_ERIS");
                configure.SetDescription("Questo servizio è dedicato all'interscambio dati con il cliente IRENA ERIS");
            });
        }
    }
}
