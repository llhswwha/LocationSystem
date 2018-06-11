using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.DAL;

namespace Location.BLL
{
    public class BaseBll:IDisposable
    {
        public LocationDb Db { get; set; }

        public BaseBll()
        {
            Db=new LocationDb();
        }

        public BaseBll(LocationDb db)
        {
            Db = db;
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }

    public abstract class BaseBll<T>: BaseBll where T :class 
    {
        public DbSet<T> DbSet { get; set; }

        protected abstract void InitDbSet();

        public BaseBll():base()
        {
            InitDbSet();
        }

        public BaseBll(LocationDb db) : base(db)
        {
            InitDbSet();
        }

        public virtual bool Create(T entity)
        {
            if (DbSet == null) return false;

            DbSet.Add(entity);
            int r = Db.SaveChanges();
            return r > 0;
        }

        public virtual T Get(int? id)
        {
            if (DbSet == null) return default(T);

            if (id == null)
            {
                return default(T);
            }
            T map = DbSet.Find(id);
            return map;
        }

        public virtual List<T> GetList()
        {
            if (DbSet == null) return null;

            return DbSet.ToList();
        }

        public virtual bool Delete(int id)
        {
            if (DbSet == null) return false;

            T map = DbSet.Find(id);
            DbSet.Remove(map);
            int r = Db.SaveChanges();
            return r > 0;
        }

        public virtual bool Edit(T entity)
        {
            if (DbSet == null) return false;

            Db.Entry<T>(entity).State = EntityState.Modified;
            int r = Db.SaveChanges();
            return r > 0;
        }
    }
}
