using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEntity = DbModel.Location.AreaAndDev.Post;


namespace LocationServices.Locations.Services
{
    public interface IPostService : INameEntityService<TEntity>
    {
        
    }
    public class PostService : NameEntityService<TEntity>, IPostService
    {
        public PostService() : base()
        {
        }

        public PostService(Bll bll) : base(bll)
        {
        }

        protected override void SetDbSet()
        {
            dbSet = db.Posts;
        }
    }
}
