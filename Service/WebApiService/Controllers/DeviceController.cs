using LocationServices.Locations.Services;
using System.Collections.Generic;
using System.Web.Http;
using TEntity = Location.TModel.Location.AreaAndDev.DevInfo;
using TPEntity = Location.TModel.Location.AreaAndDev.PhysicalTopology;
using Location.TModel.Location.AreaAndDev;
using System;

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
        [HttpDelete]
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

        [Route("detail/id/{id}")]
        public TEntity GetEntityById(int id)
        {
            return service.GetEntityById(id);
        }
        [Route("detail/devId/{devId}")]//detail?devId=guid
        public TEntity GetEntityByDevId(string devId)
        {
            return service.GetEntityByDevId(devId);
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
            if (item == null) return null;
            return service.Post(pid, item);
        }

        [Route]
        public TEntity Put(TEntity item)
        {
            return service.Put(item);
        }

        [Route("list")]
        [HttpPost]
        public IList<TEntity> GetListByTypes(int[] typeList)
        {
            return service.GetListByTypes(typeList);
        }
        [Route("detail/devInfo")]
        [HttpPost]
        public TEntity GetDevByGameName(TEntity devTemp)
        {
            string modelName = devTemp.ModelName;
            return service.GetDevByGameName(modelName);
        }
        [Route("detail/devModel")]
        [HttpGet]
        public TEntity GetDevByGameName(string modelName)
        {
            return service.GetDevByGameName(modelName);
        }
    }
}
