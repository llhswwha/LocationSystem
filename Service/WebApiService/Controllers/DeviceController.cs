using LocationServices.Locations.Services;
using System.Collections.Generic;
using System.Web.Http;
using TEntity = Location.TModel.Location.AreaAndDev.DevInfo;
using TPEntity = Location.TModel.Location.AreaAndDev.PhysicalTopology;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/devices")]
    public class DeviceController : ApiController, IDeviceService
    {
        protected IDeviceService service;

        public DeviceController()
        {
            service = new DeviceService();
        }

        [Route("{id}")]
        public TEntity Delete(string id)
        {
            return service.Delete(id);
        }

        [Route("")]//areas/?id=1
        [Route("{id}")]
        public TEntity GetEntity(string id)
        {
            return service.GetEntity(id);
        }

        [Route("{id}/parent")]
        public TPEntity GetParent(string id)
        {
            return service.GetParent(id);
        }

        [Route("")]
        [Route("list")]
        public List<TEntity> GetList()
        {
            return service.GetList();
        }

        [Route("")]//search/?name=主
        [Route("search/{name}")]//search/1,直接中文不行
        public IList<TEntity> GetListByName(string name)
        {
            return service.GetListByName(name);
        }

        [Route("")]//search/?pid=主
        [Route("~/api/areas/{pid}/devices")]
        public List<TEntity> GetListByPid(string pid)
        {
            return service.GetListByPid(pid);
        }

        [Route("~/api/areas/{pid}/devices")]
        public IList<TEntity> DeleteListByPid(string pid)
        {
            return service.DeleteListByPid(pid);
        }

        [Route]
        public TEntity Post(TEntity item)
        {
            return service.Post(item);
        }

        [Route("~/api/areas/{pid}/devices")]
        public TEntity Post(string pid, TEntity item)
        {
            return service.Post(pid, item);
        }

        [Route]
        public TEntity Put(TEntity item)
        {
            return service.Put(item);
        }
    }
}
