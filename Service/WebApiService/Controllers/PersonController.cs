using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Location.TModel.Location.Person;
using TEntity = Location.TModel.Location.Person.Personnel;
using TPEntity = Location.TModel.Location.Person.Department;
namespace WebApiService.Controllers
{
    [RoutePrefix("api/persons")]
    public class PersonController : ApiController, IPersonServie
    {
        protected IPersonServie service;

        public PersonController()
        {
            service = new PersonService();
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
        public IList<TEntity> GetList()
        {
            return service.GetList();
        }

        [Route("")]//search/?name=主
        [Route("search/{name}")]//search/1,直接中文不行
        public IList<TEntity> GetListByName(string name)
        {
            return service.GetListByName(name);
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

        [Route("")]//search/?pid=主
        [Route("~/api/deps/{pid}/persons")]
        public IList<TEntity> GetListByPid(string pid)
        {
            return service.GetListByPid(pid);
        }

        [Route("~/api/deps/{pid}/persons")]
        public TEntity Post(string pid, TEntity item)
        {
            return service.Post(pid, item);
        }

        [Route("~/api/deps/{pid}/persons")]
        public IList<TEntity> DeleteListByPid(string pid)
        {
            return service.DeleteListByPid(pid);
        }
    }
}
