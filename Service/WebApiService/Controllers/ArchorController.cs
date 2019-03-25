using LocationServices.Locations.Services;
using System.Collections.Generic;
using System.Web.Http;
using TEntity = TModel.Location.AreaAndDev.Archor;

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

        [Route("{id}")]
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

        [Route]
        public TEntity Post(TEntity item)
        {
            return service.Post(item);
        }

        [Route]
        public TEntity Put(TEntity item)
        {
            return service.Put(item);
        }
    }
}
