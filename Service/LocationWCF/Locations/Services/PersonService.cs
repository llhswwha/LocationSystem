using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEntity = Location.TModel.Location.Person.Personnel;
using DbEntity = DbModel.Location.Person.Personnel;
using TPEntity = Location.TModel.Location.Person.Department;
using BLL;
using BLL.Blls.Location;
using Location.TModel.Location.Person;
using TModel.Tools;
using LocationServices.Converters;
using DbModel.Tools;

namespace LocationServices.Locations.Services
{
    public interface IPersonServie: ILeafEntityService<TEntity, TPEntity>
    {

    }
    public class PersonService : IPersonServie
    {
        private Bll db;

        private PersonnelBll dbSet;

        public PersonService()
        {
            db = new Bll(false, false, false, false);
            dbSet = db.Personnels;
        }

        public PersonService(Bll bll)
        {
            this.db = bll;
            dbSet = db.Personnels;
        }

        public TEntity Delete(string id)
        {
            var item = dbSet.DeleteById(id.ToInt());
            return item.ToTModel();
        }

        public IList<TEntity> DeleteListByPid(string pid)
        {
            return dbSet.DeleteListByPid(pid.ToInt()).ToWcfModelList();
        }

        public TEntity GetEntity(string id)
        {
            var item = dbSet.Find(id.ToInt());
            return item.ToTModel();
        }

        public IList<TEntity> GetList()
        {
            var devInfoList = dbSet.DbSet.ToList().ToTModel();
            return devInfoList.ToWCFList();
        }

        public IList<TEntity> GetListByName(string name)
        {
            var devInfoList = dbSet.GetListByName(name).ToTModel();
            return devInfoList.ToWCFList();
        }

        public IList<TEntity> GetListByPid(string pid)
        {
            return dbSet.GetListByPid(pid.ToInt()).ToWcfModelList();
        }

        public TPEntity GetParent(string id)
        {
            var item = dbSet.Find(id.ToInt());
            if (item == null) return null;
            return new DepartmentService(db).GetEntity(item.ParentId + "");
        }

        public TEntity Post(TEntity item)
        {
            var dbItem = item.ToDbModel();
            var result = dbSet.Add(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public TEntity Post(string pid, TEntity item)
        {
            item.ParentId = pid.ToInt();
            var dbItem = item.ToDbModel();
            var result = dbSet.Add(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public TEntity Put(TEntity item)
        {
            var dbItem = item.ToDbModel();
            var result = dbSet.Edit(dbItem);
            return result ? dbItem.ToTModel() : null;
        }
    }
}
