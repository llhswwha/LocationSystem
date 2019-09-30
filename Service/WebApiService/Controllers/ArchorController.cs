using LocationServices.Locations.Services;
using System.Collections.Generic;
using System.Web.Http;
using TEntity = TModel.Location.AreaAndDev.Archor;
using System;
using TModel.Location.AreaAndDev;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/archors")]
    public class ArchorController : ApiController, IArchorService
    {
        IArchorService service;

        public ArchorController()
        {
            service = new ArchorService();
        }

        [Route("Delete/{id}")]
        public TEntity Delete(string id)
        {
            return service.Delete(id);
        }

        [Route("")]//area/?id=1
        [Route("{id}")]
        public TEntity GetEntity(string id)
        {
            return service.GetEntity(id);
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

        [HttpGet]
        [Route("search")]
        public IList<TEntity> Search(string key,string value)
        {
            return service.Search(key,value);
        }

        [Route("Add")]
        public TEntity Post(TEntity item)
        {
            return service.Post(item);
        }

        [Route("Edit")]
        public TEntity Put(TEntity item)
        {
            return service.Put(item);
        }
        [Route("devId/{devId}")]
        public TEntity GetArchorByDevId(int devId)
        {
            return service.GetArchorByDevId(devId);
        }
        [Route("EditArchor/parentId/{ParentId}")]
        public bool EditArchor(TEntity Archor, int ParentId)
        {
            return service.EditArchor(Archor,ParentId);
        }
        [Route("EditBusAnchor/parentid/{ParentId}")]
        public bool EditBusAnchor(TEntity archor, int ParentId)
        {
            return service.EditBusAnchor(archor,ParentId);
        }
    }
}
