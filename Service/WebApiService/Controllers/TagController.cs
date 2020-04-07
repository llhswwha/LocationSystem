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
using BLL;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/tags")]
    public class TagController : ApiController, ITagService//, IDisposable
    {

        ITagService service;

        public TagController()
        {
            Bll bll = Bll.NewBllNoRelation();//没有关联数据的bll,使用关联数据的bll的话，获取列表会很慢很慢
            service = new TagService(bll);
        }

        public bool AddList(List<TEntity> entities)
        {
            return service.AddList(entities);
        }
        public bool DeleteAll()
        {
            return service.DeleteAll();
        }

        [Route("delete/{id}")]
        public TEntity Delete(string id)
        {
            return service.Delete(id);
        }
        [Route("delete/rtnBool/{id}")]
        public bool DeleteTag(int id)
        {
            return service.DeleteTag(id);
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
            Log.Info(LogTags.WebApi, "TagController.GetList:" +count);
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
            var list = service.GetList(true);
            var listCount = 0;
            if (list != null)
            {
                listCount = list.Count();
            }
            Log.Info(LogTags.WebApi, string.Format("[{0}]TagController.GetListWithDetail: listCount={1} , getCount={2}", Request.GetClientIpAddress(), listCount, count));
            count++;
            return list;
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

        [Route("add")]
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
        [HttpPut]
        [Route("edit")]
        public TEntity Put(TEntity item)
        {
            return service.Put(item);
        }
        [HttpPut]
        [Route("edit/rtnBool")]
        public bool EditTag(TEntity tag)
        {
            return service.EditTag(tag);
        }
        [HttpPut]
        [Route("edit/renBool/id/{id}")]
        public bool EditTagById(TEntity Tag, int? id)
        {
            return service.EditTagById(Tag,id);
        }
    }
}
