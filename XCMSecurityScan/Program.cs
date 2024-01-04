using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace XCMSecurityScan
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
                configure.SetServiceName("XCMSyncroAPI");
                configure.SetDisplayName("XCMSyncroAPI");
                configure.SetDescription("Questo servizio è dedicato all'interscambio e monitoraggio tramite API");
            });
        }
    }
}
