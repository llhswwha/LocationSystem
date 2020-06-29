using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
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

            var serializer = new JsonSerializer()
            {
                //PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                //TypeNameHandling = TypeNameHandling.Objects,
                NullValueHandling = NullValueHandling.Ignore
                //TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            };

            // register it so that signalr can pick it up
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);


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
