using System.Collections.Generic;
using BLL;
using BLL.Blls.Location;
using LocationServices.Converters;
using DbModel.Tools;
using TModel.Tools;
using TEntity = Location.TModel.Location.AreaAndDev.Tag;
using DbEntity = DbModel.Location.AreaAndDev.LocationCard;
using System;

namespace LocationServices.Locations.Services
{
    public interface ITagService:IEntityService<TEntity>
    {
        bool DeleteAll();

        bool AddList(List<TEntity> entities);
    }
    public class TagService : ITagService
    {
        private Bll db;

        private LocationCardBll dbSet;

        public TagService()
        {
            db = new Bll(false, true, false, true);
            dbSet = db.LocationCards;
        }

        public TagService(Bll bll)
        {
            this.db = bll;
            dbSet = db.LocationCards;
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
            if (item.Code == null)
            {
                item.Code = "000";
            }
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

        /// <summary>
        /// 清空标签数据库表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteAll()
        {
            bool r = true;
            try
            {
                string sql = "delete from Tags";
                db.Db.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception ex)
            {
                r = false;
                LogEvent.Info(ex.ToString());
            }
            return r;
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <returns></returns>
        public bool AddList(List<TEntity> entities)
        {
            var list = entities.ToDbModel();
            bool r = db.LocationCards.AddRange(list);
            db.AddTagPositionsByTags(list);
            return r;
        }
    }
}
