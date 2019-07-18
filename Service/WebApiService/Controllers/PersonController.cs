using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Data;
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
        public List<TEntity> GetList()
        {
            return service.GetList();
        }

        [Route("")]
        [Route("list")]
        public List<TEntity> GetList(bool detail,bool showAll)
        {
            return service.GetList(detail, showAll);
        }

        [Route("detail")]
        [Route("list/detail")]
        public IList<TEntity> GetListWithDetail()
        {
            return service.GetList(true,true);
        }

        [Route("")]//search/?name=主
        [Route("search/{name}")]//search/1,直接中文不行
        public IList<TEntity> GetListByName(string name)
        {
            return service.GetListByName(name);
        }

        [Route("")]//?role=主
        [Route("~/api/tagRoles/{role}/persons")]
        public IList<TEntity> GetListByRole(string role)
        {
            return service.GetListByRole(role);
        }

        [HttpPut]
        [Route("{id}/role/{role}")]
        public TEntity SetRole(string id, string role)
        {
            return service.SetRole(id, role);
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
        public List<TEntity> GetListByPid(string pid)
        {
            return service.GetListByPid(pid);
        }

        [Route("")]//search/?areaId=主
        [Route("~/api/areas/{areaId}/persons")]
        public IList<TEntity> GetListByArea(string areaId)
        {
            return service.GetListByArea(areaId);
        }

        /// <summary>
        /// 获取一个人员的位置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}/pos")]
        public TagPosition GetPositon(string id)
        {
            return service.GetPositon(id);
        }

        /// <summary>
        /// 获取一个人员的标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}/tag")]
        public Tag GetTag(string id)
        {
            return service.GetTag(id);
        }

        /// <summary>
        /// 绑定人员和标签（发卡）
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        [Route("tag")]
        [HttpPost]
        public TEntity BindWithTag(PersonTag pt)
        {
            return service.BindWithTag(pt);
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
