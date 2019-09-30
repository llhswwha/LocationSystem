using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Location.TModel.Location;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Locations.Services;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/config")]
    public class ConfigArgController : ApiController, IConfigArgService
    {
        protected IConfigArgService service;

        public ConfigArgController()
        {
            service = new ConfigArgService();
        }

        public bool AddConfigArg(ConfigArg config)
        {
            return service.AddConfigArg(config);
        }

        public bool DeleteConfigArg(ConfigArg config)
        {
            return service.DeleteConfigArg(config);
        }
        [Route("edit")]
        [HttpPut]
        public bool EditConfigArg(ConfigArg config)
        {
            return service.EditConfigArg(config);
        }

        public List<ConfigArg> FindConfigArgListByClassify(string key)
        {
            return service.FindConfigArgListByClassify(key);
        }

        public List<ConfigArg> FindConfigArgListByKey(string key)
        {
            return FindConfigArgListByKey(key);
        }

        public ConfigArg GetConfigArg(int id)
        {
            return service.GetConfigArg(id);
        }
        [Route("getByKey/{key}")]
        public ConfigArg GetConfigArgByKey(string key)
        {
            return service.GetConfigArgByKey(key);
        }

        public List<ConfigArg> GetConfigArgList()
        {
            return service.GetConfigArgList();
        }
        [Route("getTran")]
        public TransferOfAxesConfig GetTransferOfAxesConfig()
        {
            return service.GetTransferOfAxesConfig();
        }
        [Route("setTran")]
        [HttpPut]
        public bool SetTransferOfAxesConfig(TransferOfAxesConfig config)
        {
            return service.SetTransferOfAxesConfig(config);
        }
    }
}
