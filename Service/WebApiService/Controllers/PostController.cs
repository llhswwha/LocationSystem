using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Locations.Services;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/post")]
    public class PostController : ApiController, IPostService
    {
        protected IPostService service;

        public PostController()
        {
            service = new PostService();
        }

        [Route("add")]
        public Post Post(Post item)
        {
            return service.Post(item);
        }
        [Route("edit")]
        public Post Put(Post item)
        {
            return service.Put(item);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public Post Delete(string id)
        {
            return service.Delete(id);
        }
        [Route("detail/{id}")]
        public Post GetEntity(string id)
        {
            return service.GetEntity(id);
        }
        [Route("list")]
        public List<Post> GetList()
        {
            return service.GetList();
        }
        [Route("list/name/{name}")]
        public  IList<Post> GetListByName(string name)
        {
            return service.GetListByName(name);
        }
    }
}
