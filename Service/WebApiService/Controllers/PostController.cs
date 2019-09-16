using LocationServices.Locations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Locations.Services;
using DbModel.Location.AreaAndDev;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/post")]
    public class PostController : ApiController, LocationServices.Locations.Services.IPostService
    {
        protected LocationServices.Locations.Services.IPostService service;

        public PostController()
        {
            service = new PostService();
        }


        [HttpDelete]
        [Route("")]
        public DbModel.Location.AreaAndDev.Post Delete(string id)
        {
            return service.Delete(id);
        }

        [Route("{id}")]
        public DbModel.Location.AreaAndDev.Post GetEntity(string id)
        {
            return service.GetEntity(id);
        }

        [Route("")]
        [Route("list")]
        public List<DbModel.Location.AreaAndDev.Post> GetList()
        {
            return service.GetList();
        }

        [Route("")]//search?name=主
        [Route("search/{name}")]//search/1,直接中文不行
        public IList<DbModel.Location.AreaAndDev.Post> GetListByName(string name)
        {
            return service.GetListByName(name);
        }

        [HttpPost]
        [Route("")]
        public DbModel.Location.AreaAndDev.Post Post(DbModel.Location.AreaAndDev.Post item)
        {
            return service.Post(item);
        }

        [HttpPut]
        [Route("")]
        public DbModel.Location.AreaAndDev.Post Put(DbModel.Location.AreaAndDev.Post item)
        {
            return service.Put(item);
        }
    }
}
