using BLL;
using BLL.Blls.Location;
using DbModel.Tools;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.AreaAndDev;
using TModel.Tools;
using DbEntity = DbModel.Location.AreaAndDev.Archor;
using TEntity = TModel.Location.AreaAndDev.Archor;

namespace LocationServices.Locations.Services
{
    public interface IArchorService:IEntityService<TEntity>
    {

    }
    public class ArchorService : IArchorService
    {
        private Bll db;

        private ArchorBll dbSet;

        public ArchorService()
        {
            db = new Bll(false, false, false, false);
            dbSet = db.Archors;
        }

        public ArchorService(Bll bll)
        {
            this.db = bll;
            dbSet = db.Archors;
        }

        public TEntity Delete(string id)
        {
            var item = dbSet.DeleteById(id.ToInt());
            return item.ToTModel();
        }

        public TEntity GetEntity(string id)
        {
            var item = dbSet.Find(id.ToInt());
            return item.ToTModel();
        }

        public IList<TEntity> GetList()
        {
            var devInfoList = dbSet.ToList().ToTModel();
            return devInfoList.ToWCFList();
        }

        public IList<TEntity> GetListByName(string name)
        {
            var devInfoList = dbSet.GetListByName(name).ToTModel();
            return devInfoList.ToWCFList();
        }

        public TEntity Post(TEntity item)
        {
            var dbItem = item.ToDbModel();
            var dev = new DbModel.Location.AreaAndDev.DevInfo();
            dev.Name = item.Name;
            bool r1=db.DevInfos.Add(dev);
            if (r1)
            {
                dbItem.DevInfo = dev;
                dbItem.DevInfoId = dev.Id;
                var result = dbSet.Add(dbItem);
                return result ? dbItem.ToTModel() : null;
            }
            else
            {
                return null;
            }
        }

        public TEntity Put(TEntity item)
        {
            var dbItem = item.ToDbModel();
            var dbItemOld = dbSet.Find(item.Id);
            dbItemOld.Name = dbItem.Name;
            
            var result = dbSet.Edit(dbItemOld);
            if (result)
            {
                dbItemOld.DevInfo.Name = item.Name;
                db.DevInfos.Edit(dbItemOld.DevInfo);
            }
            return result ? dbItemOld.ToTModel() : null;
        }

        //todo:1.添加基站到某个区域下；2.获取某个区域下的所有基站
    }
}
