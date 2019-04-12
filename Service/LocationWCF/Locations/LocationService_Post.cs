using Location.TModel.Location.AreaAndDev;
using LocationServices.Converters;
using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Locations
{
    //岗位相关的接口
    public partial class LocationService : ILocationService, IDisposable
    {
        public List<Post> GetPostList()
        {
            ShowLogEx(">>>>> GetPostList");
            //var posts = db.Posts.ToList();
            //return posts.ToWcfModelList();
            return new PostService(db).GetList().ToTModel();
        }


        public Post GetPost(int id)
        {
            return new PostService(db).GetEntity(id+"").ToTModel();
        }

       
        public int AddPost(Post p)
        {
            var entity= new PostService(db).Post(p.ToDbModel());
            if (entity != null)
            {
                return entity.Id;
            }
            else
            {
                return -1;
            }
        }

        
        public bool EditPost(Post p)
        {
            var entity = new PostService(db).Put(p.ToDbModel());
            return entity != null;
        }

        
        public bool DeletePost(int id)
        {
            var entity = new PostService(db).Delete(id + "");
            return entity != null;
        }
    }
}
