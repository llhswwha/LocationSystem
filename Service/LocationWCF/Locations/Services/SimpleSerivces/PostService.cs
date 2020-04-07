using BLL;
using BLL.Blls.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEntity = Location.TModel.Location.AreaAndDev.Post;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Converters;

using Location.BLL.Tool;
namespace LocationServices.Locations.Services
{
    public interface IPostService : INameEntityService<TEntity>
    {
        
    }
    public class PostService 
        //: NameEntityService<TEntity>, 
        :IPostService
    {
        private Bll db;
        private PostBll dbSet;
        PostBll Posts;

        public PostService() : base()
        {
            db = Bll.NewBllNoRelation();
            dbSet = db.Posts;
        }

        public PostService(Bll bll) 
        {
            this.db = bll;
            dbSet = db.Posts;
        }

        public TEntity Delete(string id)
        {
            try
            {

                int keyId = int.Parse(id);
                DbModel.Location.AreaAndDev.Post post = dbSet.DeleteById(keyId);
                return post.ToTModel();
            }
            catch (Exception ex)
            {
                Log.Error("deletePost:" + id + ex.ToString());
                return null;
            }
        }

        public TEntity GetEntity(string id)
        {
            try
            {
                DbModel.Location.AreaAndDev.Post post = dbSet.FindById(int.Parse(id));
                return post.ToTModel();

            }
            catch (Exception ex)
            {
                Log.Error("getPostById:"+id+";"+ex.ToString());
                return null;
            }
        }

        public List<TEntity> GetList()
        {
            var listTemp = dbSet.GetList();
            if (listTemp == null||listTemp.Count==0) return null;
            else return listTemp.ToTModel() ;
        }

        public IList<TEntity> GetListByName(string name)
        {
            return dbSet.Where(i => i.Name.Contains(name)).ToList().ToTModel();
        }

        public TEntity Post(TEntity item)
        {
            try
            {
                DbModel.Location.AreaAndDev.Post post = item.ToDbModel();
                var result = dbSet.Add(post);
                return result ? post.ToTModel() : null;
            }
            catch (Exception ex)
            {
                Log.Error("AddPost:" + ex.ToString());
                return null;
            }

        }

        public TEntity Put(TEntity item)
        {
            try
            {
                DbModel.Location.AreaAndDev.Post post = item.ToDbModel();
                var result = dbSet.Edit(post);
                return result ? post.ToTModel() : null;
            }
            catch (Exception ex)
            {
                Log.Error("editPost:" + ex.ToString());
                return null;
            }
        }


    }
}
