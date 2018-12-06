using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiService.Controllers
{
    public class EntityController<T> : ApiController, INameEntityService<T>
    {
        protected INameEntityService<T> service;

        [Route("{id}")]
        public T Delete(string id)
        {
            return service.Delete(id);
        }

        [Route("")]//areas/?id=1
        [Route("{id}")]
        public T GetEntity(string id)
        {
            return service.GetEntity(id);
        }

        [Route("")]
        [Route("list")]
        public IList<T> GetList()
        {
            return service.GetList();
        }

        [Route("")]//search/?name=主
        [Route("search/{name}")]//search/1,直接中文不行
        public IList<T> GetListByName(string name)
        {
            return service.GetListByName(name);
        }

        [Route]
        public T Post(T item)
        {
            return service.Post(item);
        }

        [Route]
        public T Put(T item)
        {
            return service.Put(item);
        }
    }
}
