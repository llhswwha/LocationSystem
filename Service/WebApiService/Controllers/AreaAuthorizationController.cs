using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using LocationServices.Locations.Services;
using TEntity = DbModel.Location.Work.AreaAuthorization;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/areaAuz")]
    public class AreaAuthorizationController: ApiController, IAreaAuthorizationService
    {
        IAreaAuthorizationService service;
        public AreaAuthorizationController()
        {
            service = new AreaAuthorizationService();
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

        [Route("")]//search?area=主
        public IList<TEntity> GetListByArea(string area)
        {
            return service.GetListByArea(area);
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
        [Route("list/CardRoleAccessAreas/{role}")]
        public List<int> GetCardRoleAccessAreas(int role)
        {
            return service.GetCardRoleAccessAreas(role);
        }
        [Route("CardRoleAccessAreas/{roleId}")]
        public bool SetCardRoleAccessAreas(int roleId, List<int> areaIds)
        {
            return service.SetCardRoleAccessAreas(roleId,areaIds);
        }
    }
}
