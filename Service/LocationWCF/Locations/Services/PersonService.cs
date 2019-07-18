using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TEntity = Location.TModel.Location.Person.Personnel;
using DbEntity = DbModel.Location.Person.Personnel;
using TPEntity = Location.TModel.Location.Person.Department;
using BLL;
using BLL.Blls.Location;
using DbModel.Location.Relation;
using Location.TModel.Location.Person;
using TModel.Tools;
using LocationServices.Converters;
using DbModel.Tools;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Data;
using TModel.Location.Person;
using Location.BLL.Tool;

namespace LocationServices.Locations.Services
{
    public interface IPersonServie: ILeafEntityService<TEntity, TPEntity>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        List<TEntity> GetList(bool detail, bool showAll);

        /// <summary>
        /// 获取一个区域下的所有人员
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        IList<TEntity> GetListByArea(string areaId);

        /// <summary>
        /// 获取一个人员的位置
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        TagPosition GetPositon(string personId);

        /// <summary>
        /// 获取一个人员的标签
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Tag GetTag(string personId);

        /// <summary>
        /// 绑定人员和标签（发卡）
        /// </summary>
        /// <returns></returns>
        TEntity BindWithTag(PersonTag pt);


        IList<TEntity> GetListByRole(string role);

        TEntity SetRole(string id, string role);
    }

    public class PersonTag
    {
        public int Id { get; set; }

        public int Tag { get; set; }
    }
    public class PersonService : IPersonServie
    {
        private Bll db;

        private PersonnelBll dbSet;

        public PersonService()
        {
            db = Bll.NewBllNoRelation();
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
            var tItem= item.ToTModel();

            tItem.Tag = GetTag(id);
            tItem.Pos = GetPositon(id);

            return tItem;
        }

        public Tag GetTag(string personId)
        {
            Tag tag = null;
            var relation = db.LocationCardToPersonnels.Find(i => i.PersonnelId + "" == personId);
            if (relation != null)
            {
                int tagId = relation.LocationCardId;
                tag = db.LocationCards.Find(tagId).ToTModel();
            }
            return tag;
        }

        public TEntity BindWithTag(PersonTag pt)
        {
            if (BindWithTag(pt.Id, pt.Tag) == false) return null;
            return GetEntity(pt.Id.ToString());
        }

        public bool BindWithTag(int personId, int tagId)
        {
            var relation = db.LocationCardToPersonnels.Find(i => i.PersonnelId == personId);
            if (relation != null)
            {
                if (tagId == 0)//解除绑定
                {
                    var tagPos = db.LocationCardPositions.FirstOrDefault(i=>i.CardId!=null&&i.CardId==relation.LocationCardId);
                    if(tagPos!=null)
                    {
                        db.LocationCardPositions.Remove(tagPos);
                    }
                    var tag = db.LocationCards.Find(relation.LocationCardId);
                    if (tag != null)
                    {
                        tag.IsActive = false;//解除卡则不激活
                        db.LocationCards.Edit(tag);

                        bool r = db.LocationCardToPersonnels.Remove(relation);//删除关联关系
                        return r;
                    }
                    else//找不到卡 应该不可能
                    {
                        bool r = db.LocationCardToPersonnels.Remove(relation);//删除关联关系
                        return r;
                    }
                    
                }
                else
                {
                    var tag = db.LocationCards.Find(relation.LocationCardId);
                    if(tag.Id!= relation.LocationCardId)
                    {
                        Log.Error("tag.Id!= relation.LocationCardId");
                    }
                    if (tag != null)
                    {
                        

                        relation.LocationCardId = tagId;
                        bool r2= db.LocationCardToPersonnels.Edit(relation);//修改人员和标签卡的关联关系
                        if (r2 == false)
                        {
                            Log.Error("修改绑定失败:" + relation.LocationCardId);
                        }
                        if (tag.IsActive == false)
                        {
                            tag.IsActive = true;//激活
                            bool r1 = db.LocationCards.Edit(tag);
                            if (r1 == false)
                            {
                                Log.Error("修改激活失败:" + relation.LocationCardId);
                            }
                        }
                        return r2;
                    }
                    else
                    {
                        Log.Error("找不到定位卡:"+ relation.LocationCardId);
                        return false;
                    }
                }
                
            }
            else
            {
                if (tagId != 0)//解除绑定
                {
                    bool r = db.LocationCardToPersonnels.Add(new LocationCardToPersonnel()
                    {
                        PersonnelId = personId,
                        LocationCardId = tagId
                    });

                    var tag = db.LocationCards.Find(tagId);
                    if (tag != null)
                    {
                        tag.IsActive = true;//发卡则激活
                        db.LocationCards.Edit(tag);
                    }
                    return r;
                }
                else
                {
                    return true;

                }

            }
        }

        public TagPosition GetPositon(string personId)
        {
            var query = from r in db.LocationCardToPersonnels.DbSet
                join tag in db.LocationCards.DbSet on r.LocationCardId equals tag.Id 
                join pos in db.LocationCardPositions.DbSet on tag.Code equals pos.Id
                where r.PersonnelId+"" == personId 
                select pos;
            TagPosition tagPos = query.FirstOrDefault().ToTModel();
            return tagPos;
        }


        public IList<TEntity> GetListByArea(string areaId)
        {
            var query = from r in db.LocationCardToPersonnels.DbSet
                        join p in dbSet.DbSet on r.PersonnelId equals p.Id
                        join tag in db.LocationCards.DbSet on r.LocationCardId equals tag.Id
                        join pos in db.LocationCardPositions.DbSet on tag.Code equals pos.Id
                        where pos.AreaId + "" == areaId
                        select new {Person=p,Tag=tag,Pos=pos};
            string sql = query.ToString();
            var list=new List<TEntity>();
            foreach (var item in query.ToList())
            {
                var p = item.Person.ToTModel();
                p.Tag = item.Tag.ToTModel();
                p.Tag.Pos = item.Pos.ToTModel();
                list.Add(p);
            }
            return list.ToWCFList();
        }

        public IList<TEntity> GetListByRole(string role)
        {
            var roleId = role.ToInt();
            var query = from r in db.LocationCardToPersonnels.DbSet
                        join p in dbSet.DbSet on r.PersonnelId equals p.Id
                        join tag in db.LocationCards.DbSet on r.LocationCardId equals tag.Id
                        join pos in db.LocationCardPositions.DbSet on tag.Code equals pos.Id
                        where tag.CardRoleId  == roleId
                        select new { Person = p, Tag = tag, Pos = pos };
            string sql = query.ToString();
            var list = new List<TEntity>();
            foreach (var item in query.ToList())
            {
                var p = item.Person.ToTModel();
                p.Tag = item.Tag.ToTModel();
                p.Tag.Pos = item.Pos.ToTModel();
                list.Add(p);
            }
            return list.ToWCFList();
        }

        public List<TEntity> GetList(bool detail, bool showAll)
        {
            try
            {
                var list = new List<TEntity>();
                var query = from p in dbSet.DbSet
                    join r in db.LocationCardToPersonnels.DbSet on p.Id equals r.PersonnelId
                    join tag in db.LocationCards.DbSet on r.LocationCardId equals tag.Id
                    join pos in db.LocationCardPositions.DbSet on tag.Code equals pos.Id
                        into posList
                    from pos2 in posList.DefaultIfEmpty() //left join
                    select new {Person = p, Tag = tag, Pos = pos2};
                var pList = query.ToList();
                foreach (var item in pList)
                {
                    try
                    {
                        LocationCardPositionBll.SetPostionState(item.Pos);
                        TEntity p = item.Person.ToTModel();
                        var tag = item.Tag.ToTModel();
                        var pos = item.Pos.ToTModel();
                        if (pos != null)
                            p.AreaId = pos.AreaId ?? 2; //要是AreaId为空就改为四会电厂区域

                        if (showAll == false)
                        {
                            if (pos == null || (pos != null && pos.IsHide))
                            {
                                //隐藏待机的人员
                            }
                            else
                            {
                                list.Add(p);
                            }
                        }
                        else
                        {
                            list.Add(p);
                        }

                        if (detail)
                        {
                            p.Tag = tag;
                            p.Pos = pos;
                            //tag.Pos = pos;
                        }
                    }
                    catch (Exception  ex1)
                    {
                        Log.Error("PersonService.GetList Item:" + ex1);
                    }
                   
                }

                return list;
            }
            catch (Exception ex)
            {
                Log.Error("PersonService.GetList:" + ex);
                return null;
            }
        }

        public List<TEntity> GetList()
        {
            return GetList(false,true);
        }

        public IList<TEntity> GetListByName(string name)
        {
            var devInfoList = dbSet.GetListByName(name).ToTModel();
            return devInfoList.ToWCFList();
        }

        public List<TEntity> GetListByPid(string pid)
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
            if (result)
            {
                if (item.TagId != null)
                {
                    BindWithTag(item.Id, (int)item.TagId);
                }
            }
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
            if (result)
            {
                if (item.TagId != null)
                {
                    BindWithTag(item.Id, (int)item.TagId);
                }
            }
            return result ? dbItem.ToTModel() : null;
        }

        public TEntity SetRole(string personId, string roleId)
        {
            int id = personId.ToInt();
            var tp = db.LocationCardToPersonnels.Find(i => i.PersonnelId == id);
            if (tp == null) return null;
            TagService tagService = new TagService(db);
            var tag = tagService.SetRole(tp.LocationCardId + "", roleId);
            if (tag == null) return null;
            return GetEntity(personId);
        }

        public List<NearbyPerson> GetNearbyPerson_Currency(int id, float fDis)
        {
            List<NearbyPerson> lst = new List<NearbyPerson>();
            List<NearbyPerson> lst2 = new List<NearbyPerson>();
            DbModel.Location.AreaAndDev.DevInfo dev = db.DevInfos.Find(id);
            if (dev == null || dev.ParentId == null)
            {
                return lst;
            }

            int? AreadId = dev.ParentId;
            float PosX = dev.PosX;
            float PosY = dev.PosY;
            float PosZ = dev.PosZ;

            float PosX2 = 0;
            float PosY2 = 0;
            float PosZ2 = 0;

            float sqrtDistance = 0;
            float Distance = 0;

            var query = from t1 in db.LocationCardPositions.DbSet
                        join t2 in db.Personnels.DbSet on t1.PersonId equals t2.Id
                        join t3 in db.Departments.DbSet on t2.ParentId equals t3.Id
                        where t1.AreaId == AreadId
                        select new NearbyPerson { id = t2.Id, Name = t2.Name, WorkNumber = t2.WorkNumber, DepartMent = t3.Name, Position = t2.Pst, X = t1.X, Y = t1.Y, Z = t1.Z };
            if (query != null)
            {
                lst2 = query.ToList();
            }

            foreach (NearbyPerson item in lst2)
            {
                PosX2 = item.X - PosX;
                PosY2 = item.Y - PosY;
                PosZ2 = item.Z - PosZ;

                sqrtDistance = PosX2 * PosX2 + PosY2 * PosY2 + PosZ2 * PosZ2;
                Distance = (float)System.Math.Sqrt(sqrtDistance);
                item.Distance = Distance;

                if (Distance <= fDis)
                {
                    NearbyPerson item2 = item.Clone();
                    if (item2 != null)
                    {
                        lst.Add(item2);
                    }
                }


                PosX2 = 0;
                PosY2 = 0;
                PosZ2 = 0;
                sqrtDistance = 0;
                Distance = 0;
            }

            lst.Sort(new PersonDistanceCompare());

            return lst;
        }

        public List<NearbyPerson> GetNearbyPerson_Alarm(int id, float fDis)
        {
            List<NearbyPerson> lst = new List<NearbyPerson>();
            List<NearbyPerson> lst2 = new List<NearbyPerson>();
            DbModel.Location.AreaAndDev.DevInfo dev = db.DevInfos.Find(id);
            if (dev == null || dev.ParentId == null)
            {
                return lst;
            }

            int? AreadId = dev.ParentId;
            float PosX = dev.PosX;
            float PosY = dev.PosY;
            float PosZ = dev.PosZ;

            float PosX2 = 0;
            float PosY2 = 0;
            float PosZ2 = 0;

            float sqrtDistance = 0;
            float Distance = 0;

            var query = from t1 in db.LocationAlarms.DbSet
                        join t2 in db.LocationCardPositions.DbSet on t1.LocationCardId equals t2.CardId
                        join t3 in db.Personnels.DbSet on t1.PersonnelId equals t3.Id
                        join t4 in db.Departments.DbSet on t3.ParentId equals t4.Id
                        where t1.LocationCardId == AreadId
                        select new NearbyPerson { id = t3.Id, Name = t3.Name, WorkNumber = t3.WorkNumber, DepartMent = t4.Name, Position = t3.Pst, X = t2.X, Y = t2.Y, Z = t2.Z };
            if (query != null)
            {
                lst2 = query.ToList();
            }

            foreach (NearbyPerson item in lst2)
            {
                PosX2 = item.X - PosX;
                PosY2 = item.Y - PosY;
                PosZ2 = item.Z - PosZ;

                sqrtDistance = PosX2 * PosX2 + PosY2 * PosY2 + PosZ2 * PosZ2;
                Distance = (float)System.Math.Sqrt(sqrtDistance);
                item.Distance = Distance;
                if (Distance <= fDis)
                {
                    NearbyPerson item2 = item.Clone();
                    if (item2 != null)
                    {
                        lst.Add(item2);
                    }
                }

                PosX2 = 0;
                PosY2 = 0;
                PosZ2 = 0;
                sqrtDistance = 0;
                Distance = 0;
            }

            lst.Sort(new PersonDistanceCompare());

            return lst;

        }
    }
}
