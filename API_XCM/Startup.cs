using API_XCM.App_Start;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace API_XCM
{
    public class Startup
    {
        System.Timers.Timer syncro = new System.Timers.Timer();
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            var myProvider = new AuthorizationServerProviderAPI();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = myProvider,
            };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());


            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

            SetTimer();
            syncro.Start();
#if DEBUG
            OnTimedEvent(null, null);
#endif
        }
        private void SetTimer()
        {
            syncro = new System.Timers.Timer(3600000);
            syncro.Elapsed += OnTimedEvent;
            syncro.AutoReset = true;
            syncro.Enabled = true;
            //_loggerCode.Info($"Timer ciclo settato {timerAggiornamentoCiclo.Interval} ms");
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {           
            Task.Factory.StartNew(() =>
            {
                try
                {
                    syncro.Stop();
                    //API_XCM.Code.SyncroDB.SincroniaDatiCRM.Sincronizza();
                }
                catch (Exception ee)
                {

                }
                finally
                {
                    syncro.Start();
                }
            });
        }
    }
}