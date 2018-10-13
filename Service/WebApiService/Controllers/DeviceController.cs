using Location.TModel.Location.AreaAndDev;
using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/devices")]
    public class DeviceController : ApiController, IEntityService<DevInfo>
    {
        protected IEntityService<DevInfo> service;

        public DeviceController()
        {
            service = new DeviceService();
        }

        [Route("{id}")]
        public DevInfo Delete(string id)
        {
            return service.Delete(id);
        }

        [Route("")]//areas/?id=1
        [Route("{id}")]
        public DevInfo GetEntity(string id)
        {
            return service.GetEntity(id);
        }

        [Route("")]
        [Route("list")]
        public IList<DevInfo> GetList()
        {
            return service.GetList();
        }

        [Route("")]//search/?name=主
        [Route("search/{name}")]//search/1,直接中文不行
        public IList<DevInfo> GetListByName(string name)
        {
            return service.GetListByName(name);
        }

        [Route]
        public DevInfo Post(DevInfo item)
        {
            return service.Post(item);
        }

        [Route]
        public DevInfo Put(DevInfo item)
        {
            return service.Put(item);
        }
    }
}
