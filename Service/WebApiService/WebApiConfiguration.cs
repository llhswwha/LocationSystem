using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiContrib.Formatting.CollectionJson.Client;

namespace WebApiService
{
    public static class WebApiConfiguration
    {
        public static void Configure(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("Root", "", new { controller = "Home" });
            config.Routes.MapHttpRoute("DefaultApi", "{controller}/{id}", new { id = RouteParameter.Optional });
            ConfigureFormatters(config);
            ConfigureAutofac(config);
            EnableCors(config);
        }

        private static void EnableCors(HttpConfiguration config)
        {
            //throw new NotImplementedException();
        }

        private static void ConfigureAutofac(HttpConfiguration config)
        {

        }

        private static void ConfigureFormatters(HttpConfiguration config)
        {
            config.Formatters.Add(new CollectionJsonFormatter());
            var jsonSettings = config.Formatters.JsonFormatter.SerializerSettings;
            jsonSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            jsonSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.issue+json"));
        }
    }
}
