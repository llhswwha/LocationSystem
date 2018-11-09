using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using DbModel.Location.Authorizations;
using TEntity = DbModel.Location.Authorizations.CardRole;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/tagRoles")]
    public class TagRoleController : ApiController, ITagRoleService
    {
        ITagRoleService service;
        public TagRoleController()
        {
            service = new TagRoleService();
        }

        [Route("{id}")]
        public TEntity Delete(string id)
        {
            return service.Delete(id);
        }

        [Route("")]//area?id=1
        [Route("{id}")]
        public TEntity GetEntity(string id)
        {
            return service.GetEntity(id);
        }

        [Route("")]//area?tag=1
        [Route("~/api/tags/{tag}/role")]
        public TEntity GetEntityByTag(string tag)
        {
            return service.GetEntityByTag(tag);
        }

        [Route("")]//area?person=1
        [Route("~/api/persons/{person}/role")]
        public TEntity GetEntityByPerson(string person)
        {
            return service.GetEntityByPerson(person);
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
    }
}
