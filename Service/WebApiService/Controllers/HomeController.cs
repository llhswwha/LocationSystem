using Location.TModel.Location.AreaAndDev;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Tavis;
using Tavis.Home;
using Tavis.IANA;

namespace WebApiService.Controllers
{
    public class HomeController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var home = GetHomeDocument();

            return new HttpResponseMessage()
            {
                RequestMessage = Request,
                Content = new HomeContent(home)
            };
        }

        private HomeDocument GetHomeDocument()
        {
            var home = new HomeDocument();

            string Prefix = Request.RequestUri+"/";
            Prefix = "";
            JToken listFormat = JToken.FromObject(new List<PhysicalTopology>() { new PhysicalTopology(), new PhysicalTopology() });
            JToken itemFormat = JToken.FromObject(new PhysicalTopology() { Id = 1, Name = "a" });
            JToken treeFormat = JToken.FromObject(new PhysicalTopology[] 
            { new PhysicalTopology { Children=new List<PhysicalTopology> {  } }
            });
            home.AddResource(GetLink(Prefix, "areaList", "area",listFormat));
            home.AddResource(GetLink(Prefix, "areaItem", "area/{id}", itemFormat, HttpMethod.Get, HttpMethod.Delete));
            home.AddResource(GetLink(Prefix, "areaTreeRoot", "area/tree", treeFormat));
            home.AddResource(GetLink(Prefix, "areaTree", "area/tree/{id}", treeFormat));
            
            home.AddResource(GetLink(Prefix, "areaSearch", "area/search?name={name}", listFormat));
            home.AddResource(GetLink(Prefix, "areaChildren", "area/{id}/children", listFormat));
            home.AddResource(GetLink(Prefix, "areaPost", "area", itemFormat,HttpMethod.Post));
            home.AddResource(GetLink(Prefix, "areaPut", "area", itemFormat, HttpMethod.Put));
            return home;
        }

        private Link GetLink(string prefix, string relation, string target, JToken format)
        {
            return GetLink(prefix, relation, target, format,HttpMethod.Get);
        }

        private Link GetLink(string prefix, string relation, string target, JToken format, params HttpMethod[] methods)
        {
            var link = new Link()
            {
                Relation = prefix + "" + relation,
                Target = new Uri("/" + target, UriKind.Relative)
            };

            link.AddHint<AllowHint>(h =>
            {
                foreach (var item in methods)
                {
                    h.AddMethod(HttpMethod.Get);
                }
            }
            );
            link.AddHint<FormatsHint>(h =>
            {
                h.AddMediaType("application/json");
                //h.AddMediaType("application/vnd.area+json");
                h.Content = JToken.FromObject(new PhysicalTopology());
            });
            return link;
        }
    }
}
