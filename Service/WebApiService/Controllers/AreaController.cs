using System.Collections.Generic;
using System.Web.Http;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Locations.Services;
using TEntity = Location.TModel.Location.AreaAndDev.PhysicalTopology;

namespace WebApiService.Controllers
{

    [RoutePrefix("api/areas")]
    public class AreaController: ApiController, IAreaService
    {
        IAreaService service;

        public AreaController()
        {
            service = new AreaService();
        }

        [Route("")]
        [Route("list")]
        public IList<TEntity> GetList()
        {
            return service.GetList();
        }

        [Route("persons")]
        [Route("list/persons")]
        public IList<TEntity> GetListWithPerson()
        {
            return service.GetListWithPerson();
        }

        [Route("tree")]
        public TEntity GetTree()
        {
            return service.GetTree();
        }

        [Route("tree/{id}")]
        public TEntity GetTree(string id)
        {
            return service.GetTree(id);
        }

        [Route("")]//search/?name=主
        [Route("search/{name}")]//search/1,直接中文不行
        public IList<TEntity> GetListByName(string name)
        {
            return service.GetListByName(name);
        }

        [Route("{id}/children")]
        public IList<TEntity> GetListByPid(string id)
        {
            return service.GetListByPid(id);
        }

        [Route("{id}/parent")]
        public TEntity GetParent(string id)
        {
            return service.GetParent(id);
        }

        [Route("")]//area/?id=1
        [Route("{id}")]
        public TEntity GetEntity(string id)
        {
            return service.GetEntity(id);
        }

        [Route("")]
        [Route("{id}")]
        public TEntity GetEntity(string id,bool getChildren)
        {
            return service.GetEntity(id, getChildren);
        }

        [Route]
        public TEntity Post(TEntity item)
        {
            return service.Post(item);
        }

        [Route("{pid}")]
        public TEntity Post(string pid,TEntity item)
        {
            return service.Post(pid, item);
        }

        [Route]
        public TEntity Put(TEntity item)
        {
            return service.Put(item);
        }

        [Route("{id}")]
        public TEntity Delete(string id)
        {
            return service.Delete(id);
        }

        [Route("{pid}/children")]
        public IList<TEntity> DeleteListByPid(string pid)
        {
            return service.DeleteListByPid(pid);
        }
    }
}
