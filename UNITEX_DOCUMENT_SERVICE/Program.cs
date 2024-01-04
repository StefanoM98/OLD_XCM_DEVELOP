using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace UNITEX_DOCUMENT_SERVICE
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
                configure.SetServiceName("UnitexSyncroAPI");
                configure.SetDisplayName("UnitexSyncroAPI");
                configure.SetDescription("Unitex - Questo servizio è dedicato all'interscambio e monitoraggio tramite API");
            });
        }
    }
}
