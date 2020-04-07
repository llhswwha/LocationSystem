using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using TModel.Tools;
using TEntity = DbModel.Location.Work.AreaAuthorizationRecord;
using AEntity = DbModel.Location.AreaAndDev.Area;
using DbModel.Location.Work;
using Location.BLL.Tool;

namespace LocationServices.Locations.Services
{
    public interface IAreaAuthorizationRecordService : INameEntityService<TEntity>
    {
        IList<TEntity> GetListByArea(string area);

        IList<TEntity> GetListByRole(string role);

        IList<TEntity> GetAccessListByRole(string role);

        IList<TEntity> GetListByTag(string role);

        IList<TEntity> GetListByPerson(string role);

        IList<AEntity> GetAreaTreeByRole(string role);
    }
    public class AreaAuthorizationRecordService
        : NameEntityService<TEntity>
        , IAreaAuthorizationRecordService
    {
        public AreaAuthorizationRecordService() : base()
        {
        }

        public AreaAuthorizationRecordService(Bll bll) : base(bll)
        {
        }
        protected override void SetDbSet()
        {
            dbSet = db.AreaAuthorizationRecords;
        }

        public IList<TEntity> GetListByArea(string area)
        {
            int areaId = area.ToInt();
            return dbSet.Where(i => i.AreaId == areaId);
        }

        public IList<TEntity> GetListByRole(string role)
        {
            try
            {
                int roleId = role.ToInt();
                //return dbSet.Where(i => i.CardRoleId == roleId);
                var query = from aar in dbSet.DbSet
                            join area in db.Areas.DbSet on aar.AreaId equals area.Id into AreaLst
                            from area2 in AreaLst.DefaultIfEmpty()
                            where aar.CardRoleId == roleId
                            select new { AreaAuzRcd = aar, Area = area2 };
                var result = query.ToList();
                IList<TEntity> list = new List<TEntity>();
                foreach (var item in result)
                {
                    item.AreaAuzRcd.Area = item.Area;
                    list.Add(item.AreaAuzRcd);
                }
                return list;
            }
            catch (Exception ex)
            {
                Log.Error(LogTags.DbGet, "Exception:" + ex);
                return null;
            }

        }

        /// <summary>
        /// 通过区域Id,移除权限记录
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="areaRecords"></param>
        /// <returns></returns>
        public bool RemoveListByAreaId(int areaId, IList<TEntity> areaRecords)
        {
            if (areaRecords == null) return true;
            List<TEntity> entities = areaRecords.ToList();
            entities.RemoveAll(i=>i.AreaId!=areaId);
            if (entities == null || entities.Count == 0) return true;
            bool result = dbSet.RemoveList(entities);
            return result;
        }

        public IList<TEntity> GetAccessListByRole(string role)
        {
            try
            {
                int roleId = role.ToInt();
                //return dbSet.Where(i => i.CardRoleId == roleId);
                var query = from aar in dbSet.DbSet
                            join area in db.Areas.DbSet on aar.AreaId equals area.Id into AreaLst
                            from area2 in AreaLst.DefaultIfEmpty()
                            where aar.CardRoleId == roleId && aar.AccessType == AreaAccessType.可以进出
                            select new { AreaAuzRcd = aar, Area = area2 };
                var result = query.ToList();
                IList<TEntity> list = new List<TEntity>();
                foreach (var item in result)
                {
                    item.AreaAuzRcd.Area = item.Area;
                    list.Add(item.AreaAuzRcd);
                }
                return list;
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.DbGet, "Exception:" + ex);
                return null;
            }

        }

        public IList<TEntity> GetListByTag(string tagId)
        {
            try
            {
                var tag = db.LocationCards.Find(tagId.ToInt());
                if (tag == null) return null;
                return dbSet.Where(i => i.CardRoleId == tag.CardRoleId);
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.DbGet, "Exception:" + ex);
                return null;
            }
        }

        public IList<TEntity> GetListByPerson(string personId)
        {
            try
            {
                var id = personId.ToInt();
                var tp = db.LocationCardToPersonnels.Find(i => i.PersonnelId == id);
                var tag = db.LocationCards.Find(tp.LocationCardId);
                if (tag == null) return null;
                return dbSet.Where(i => i.CardRoleId == tag.CardRoleId);
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.DbGet, "Exception:" + ex);
                return null;
            }
        }

        public IList<AEntity> GetAreaTreeByRole(string role)
        {
            try
            {
                int roleId = role.ToInt();
                IList<AEntity> list = new List<AEntity>();

                var query = from t1 in db.AreaAuthorizationRecords.DbSet
                            join t2 in db.Areas.DbSet on t1.AreaId equals t2.Id
                            where t1.CardRoleId == roleId
                            select t2;

                List<AEntity> alist = query.ToList();
                List<AEntity> alist2 = query.ToList();
                List<AEntity> ResultList = new List<AEntity>();

                foreach (AEntity item in alist)
                {
                    AEntity item2 = alist2.Find(p => p.Id == item.ParentId);
                    if (item2 == null)
                    {
                        ResultList.Add(item.Clone());
                    }
                }

                CreateAreaTree(ref alist2, ref ResultList);

                foreach (AEntity item in ResultList)
                {
                    list.Add(item);
                }

                return list;
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.DbGet, "Exception:" + ex);
                return null;
            }
        }

        public void CreateAreaTree(ref List<AEntity> ContrastList, ref List<AEntity> ResultList)
        {
            try
            {
                foreach (AEntity item in ResultList)
                {
                    List<AEntity> itemList = ContrastList.FindAll(p => p.ParentId == item.Id);
                    if (item.Children == null)
                    {
                        item.Children = new List<AEntity>();
                    }

                    if (itemList != null && itemList.Count > 0)
                    {
                        CreateAreaTree(ref ContrastList, ref itemList);
                        item.Children.AddRange(itemList);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.DbGet, "Exception:" + ex);

            }


        }



        //public TEntity PostByRole(string )
        //{

        //}
    }
}
