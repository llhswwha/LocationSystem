using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using LocationServices.Locations.Services;
using TEntity = DbModel.Location.Work.AreaAuthorizationRecord;
using AEntity = DbModel.Location.AreaAndDev.Area;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/areaAuzItem")]
    public class AreaAuthorizationRecordController:ApiController, IAreaAuthorizationRecordService
    {
        IAreaAuthorizationRecordService service;
        public AreaAuthorizationRecordController()
        {
            service = new AreaAuthorizationRecordService();
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

        [Route("")]//search?area=2
        public IList<TEntity> GetListByArea(string area)
        {
            return service.GetListByArea(area);
        }

        [Route("")]
        public IList<TEntity> GetListByTag(string tag)
        {
            return service.GetListByTag(tag);
        }

        [Route("")]
        public IList<TEntity> GetListByPerson(string person)
        {
            return service.GetListByPerson(person);
        }

        [Route("")]//search?role=2
        public IList<TEntity> GetListByRole(string role)
        {
            return service.GetListByRole(role);
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

        [Route("search/AreaTree/{role}")]
        public IList<AEntity> GetAreaTreeByRole(string role)
        {
            return service.GetAreaTreeByRole(role);
        }
    }
}
