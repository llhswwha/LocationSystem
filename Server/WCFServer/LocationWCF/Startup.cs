using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServer
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.UseCors(CorsOptions.AllowAll);
            //app.MapSignalR();

            //app.UseCors(CorsOptions.AllowAll);
            //app.MapSignalR("/realtime", new HubConfiguration() { });

            app.Map("/realtime", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var config = new HubConfiguration()
                {
                    EnableJSONP = true,
                   // EnableJavaScriptProxies = false
                };
#if DEBUG
                config.EnableDetailedErrors = true;
#endif
                map.RunSignalR(config);
            });
        }
    }
}
