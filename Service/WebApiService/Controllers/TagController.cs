using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using DbModel.Location.Authorizations;
using TEntity = Location.TModel.Location.AreaAndDev.Tag;
using Location.TModel.Location.AreaAndDev;
using Location.BLL.Tool;
using System.Net.Http;
using System.Web;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/tags")]
    public class TagController : ApiController, ITagService//, IDisposable
    {

        ITagService service;

        public TagController()
        {
            service = new TagService();
        }

        public bool AddList(List<TEntity> entities)
        {
            return service.AddList(entities);
        }
        public bool DeleteAll()
        {
            return service.DeleteAll();
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

        [Route("")]
        [Route("list")]
        public List<TEntity> GetList()
        {
            Log.Info("TagController.GetList:"+count);
            count++;
            return service.GetList();
        }

        [Route("")]
        [Route("list")]
        public List<TEntity> GetList(bool detail)
        {
            return service.GetList(detail);
        }

        static int count = 0;
        [Route("detail")]
        [Route("list/detail")]
        public IList<TEntity> GetListWithDetail()
        {
            Log.Info(string.Format("[{0}]TagController.GetListWithDetail:{1}", Request.GetClientIpAddress(), count));
            count++;
            return service.GetList(true);
        }

        [Route("")]//search?name=主
        [Route("search/{name}")]//search/1,直接中文不行
        public IList<TEntity> GetListByName(string name)
        {
            return service.GetListByName(name);
        }

        [Route("")]//?role=主
        [Route("~/api/tagRoles/{role}/tags")]
        public IList<TEntity> GetListByRole(string role)
        {
            return service.GetListByRole(role);
        }

        [Route]
        public TEntity Post(TEntity item)
        {
            return service.Post(item);
        }

        public class Tag2Role
        {
            public string Tag { get; set; }
            public string Role { get; set; }
        }

        //[Route]
        //[HttpPut]
        //public TEntity SetRole(Tag2Role tag2Role)
        //{
        //    return service.SetRole(tag2Role.Tag, tag2Role.Role);
        //}

        [HttpPut]
        [Route("{id}/role/{role}")]
        public TEntity SetRole(string id, string role)
        {
            return service.SetRole(id, role);
        }

        [Route]
        public TEntity Put(TEntity item)
        {
            return service.Put(item);
        }
    }
}
