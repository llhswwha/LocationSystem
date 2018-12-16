using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Blls;
using BLL.Blls.Location;
using DAL;
using DbModel.Location.Work;
using Location.IModel;
using TModel.Tools;

namespace LocationServices.Locations.Services
{
    public abstract class EntityService<T> : IEntityService<T> where T : class, IId,new()
    {
        protected Bll db;

        protected BaseBll<T, LocationDb> dbSet;

        public EntityService()
        {
            db = new Bll(false, false, false, false);
            SetDbSet();
        }

        public EntityService(Bll bll)
        {
            this.db = bll;
            SetDbSet();
        }

        protected abstract void SetDbSet();

        public T Delete(string id)
        {
            var item = dbSet.DeleteById(id.ToInt());
            return item;
        }

        public T GetEntity(string id)
        {
            return dbSet.Find(id.ToInt());
        }

        public IList<T> GetList()
        {
            return dbSet.ToList();
        }

        public T Post(T item)
        {
            bool result = dbSet.Add(item);
            return result ? item : null;
        }

        public T Put(T item)
        {
            bool result = dbSet.Edit(item);
            return result ? item : null;
        }
    }

    public abstract class NameEntityService<T>:EntityService<T>,INameEntityService<T> where T :  class, IEntity, new()
    {

        public NameEntityService():base()
        {
            
        }

        public NameEntityService(Bll bll):base(bll)
        {
            
        }

        public IList<T> GetListByName(string name)
        {
            return dbSet.Where(i => i.Name.Contains(name));
        }
    }
}
