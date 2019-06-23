using System.Collections.Generic;
using BLL;
using BLL.Blls.Location;
using LocationServices.Converters;
using DbModel.Tools;
using TModel.Tools;
using TEntity = Location.TModel.Location.AreaAndDev.Tag;
using DbEntity = DbModel.Location.AreaAndDev.LocationCard;
using System;
using System.Linq;
using DbModel.Location.Authorizations;
using BLL.Tools;
using DbModel.Location.Data;
using Location.BLL.Tool;

namespace LocationServices.Locations.Services
{
    public interface ITagService:INameEntityService<TEntity>
    {
        bool DeleteAll();

        bool AddList(List<TEntity> entities);

        List<TEntity> GetList(bool detail);

        IList<TEntity> GetListByRole(string role);

        TEntity SetRole(string id,string role);
    }
    public class TagService : ITagService
    {
        private Bll db;

        private LocationCardBll dbSet;

        public TagService()
        {
            db = new Bll();
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
            //var item = dbSet.Find(id.ToInt()).ToTModel();

            var query = from tag in dbSet.DbSet where tag.Id + "" == id
                        join pos in db.LocationCardPositions.DbSet on tag.Code equals pos.Id
                        join r in db.LocationCardToPersonnels.DbSet on tag.Id equals r.LocationCardId
                        join p in db.Personnels.DbSet on r.PersonnelId equals p.Id
                        select new { Tag = tag, Person = p, Pos = pos };
            var result = query.FirstOrDefault();
            if (result == null) return null;
            var item = result.Tag.ToTModel();
            item.Person = result.Person.ToTModel();
            item.Pos = result.Pos.ToTModel();
            return item;
        }

        public List<TEntity> GetList()
        {
            return GetList(false);
        }

        public List<TEntity> GetList(bool detail)
        {
            DateTime start = DateTime.Now;
            Log.Info(LogTags.DbGet, " >>>>>>> TagService.GetList");
            List<TEntity> returnList = null;
            if (detail)
            {
                var list = new List<TEntity>();
                //var list1 = dbSet.ToList();

                //var query = from tag in dbSet.DbSet
                //            join pos in db.LocationCardPositions.DbSet on tag.Code equals pos.Code
                //            join r in db.LocationCardToPersonnels.DbSet on tag.Id equals r.LocationCardId
                //              into c2pLst
                //            from r2 in c2pLst.DefaultIfEmpty()
                //            join p in db.Personnels.DbSet on r2.PersonnelId equals p.Id
                //              into pLst
                //            from p2 in pLst.DefaultIfEmpty()
                //            select tag;
                //var result = query.ToList();
                var query = from tag in dbSet.DbSet
                            join pos in db.LocationCardPositions.DbSet on tag.Code equals pos.Id
                                into posLst from pos2 in posLst.DefaultIfEmpty()
                            join r in db.LocationCardToPersonnels.DbSet on tag.Id equals r.LocationCardId
                                into c2pLst
                            from r2 in c2pLst.DefaultIfEmpty() //left join
                            join p in db.Personnels.DbSet on r2.PersonnelId equals p.Id
                            into pLst
                            from p2 in pLst.DefaultIfEmpty()
                            select new { Tag = tag, Person = p2, Pos = pos2 };
                var result = query.ToList();

                var list2 = new List<int>();

                foreach (var item in query)
                {
                    if (!list2.Contains(item.Tag.Id))
                    {
                        list2.Add(item.Tag.Id);
                        LocationCardPosition pos = item.Pos;
                        LocationCardPositionBll.SetPostionState(pos);
                        var entity = item.Tag.ToTModel();
                        entity.Person = item.Person.ToTModel();
                        //if (item.Pos != null)
                        //{
                        //    item.Pos.SetState();
                        //}
                        entity.Pos = item.Pos.ToTModel();
                        list.Add(entity);
                    }
                    
                }
                returnList= list.ToWCFList() ;
            }
            else
            {
                returnList= dbSet.ToList().ToWcfModelList();
            }
            TimeSpan time = DateTime.Now - start;
            Log.Info(LogTags.DbGet, " <<<<< TagService.GetList time:" + time);
            return returnList;
        }

        public IList<TEntity> GetListByName(string name)
        {
            var devInfoList = dbSet.GetListByName(name).ToTModel();
            return devInfoList.ToWCFList();
        }

        public IList<TEntity> GetListByRole(string role)
        {
            var roleId = role.ToInt();
            return dbSet.Where(i => i.CardRoleId == roleId).ToWcfModelList();
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
            var dbItem=dbSet.Find(item.Id);
            dbItem.Update(item);
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

        public TEntity SetRole(string tagId,string roleId)
        {
            if (string.IsNullOrEmpty(tagId)) return null;
            if (string.IsNullOrEmpty(roleId)) return null;
            var tag = GetEntity(tagId);
            if (tag == null) return null;
            tag.CardRoleId = roleId.ToInt();
            var entity= Put(tag);
            DataChangArg arg = new DataChangArg(this, entity);
            StaticEvents.OnDbDataChanged(arg);
            return entity;
        }
    }
}
