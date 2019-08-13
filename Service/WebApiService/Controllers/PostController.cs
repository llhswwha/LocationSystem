using LocationServices.Locations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Location.TModel.Location.AreaAndDev;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/post")]
    public class PostController : ApiController, IPostService
    {
        protected IPostService service;

        [Route("")]
        [HttpPost]
        public int AddPost(Post p)
        {
            return service.AddPost(p);
        }

        [HttpDelete]
        [Route]
        public bool DeletePost(int id)
        {
            return service.DeletePost(id);
        }

        [HttpPut]
        [Route]
        public bool EditPost(Post p)
        {
            return service.EditPost(p);
        }
        [Route("{id}")]
        public Post GetPost(int id)
        {
            return service.GetPost(id);
        }

        [Route("List")]
        public List<Post> GetPostList()
        {
            return service.GetPostList();
        }
    }
}
