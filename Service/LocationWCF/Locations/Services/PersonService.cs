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
using Location.TModel.LocationHistory.Data;

namespace LocationServices.Locations.Services
{
    public interface IPersonServie : ILeafEntityService<TEntity, TPEntity>
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

        List<Personnel> FindPersonList(string key);

        List<NearbyPerson> GetNearbyPerson_Currency(int id, float fDis);
        int AddPerson(Personnel p);
      
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
            try
            {
                var item = dbSet.Find(id.ToInt());
                if (item == null) return null;
                var tItem = item.ToTModel();
                if (tItem == null) return null;
                tItem.Tag = GetTag(id);
                tItem.Pos = GetPositon(id);

                return tItem;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetEntity", ex.ToString());
                return null;
            }
        }

        public Tag GetTag(string personId)
        {
            try
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
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetTag", ex.ToString());
                return null;
            }
        }

        public TEntity BindWithTag(PersonTag pt)
        {
            try
            {
                if (BindWithTag(pt.Id, pt.Tag) == false) return null;
                return GetEntity(pt.Id.ToString());
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "BindWithTag", ex.ToString());
                return null;
            }
        }

        public bool BindWithTag(int personId, int tagId)
        {
            try
            {
                var relation = db.LocationCardToPersonnels.Find(i => i.PersonnelId == personId);
                if (relation != null)
                {                   
                    if (tagId == 0)//解除绑定
                    {
                        bool r = RemoveTagPersonnenlRelation(relation.LocationCardId);
                        return r;
                    }
                    else
                    {
                        if (relation.LocationCardId == tagId) return true;//tagID相同，删除再创建，会导致实时定位的Position丢失
                        //以前找到关联关系，直接更改卡Id会报错（直接在MySql workBench中用update语句不会错）。所以改成删除关联关系，建立新的关系2019/10/31 wk
                        RemoveTagPersonnenlRelation(relation.LocationCardId);
                        return AddTagPersonnenlRelation(personId,tagId);
                    }

                }
                else
                {
                    if (tagId != 0)//解除绑定
                    {
                        return AddTagPersonnenlRelation(personId, tagId);
                    }
                    else
                    {
                        return true;
                    }

                }
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "BindWithTag", ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// 移除老卡的关联关系
        /// </summary>
        /// <param name="locationCardId"></param>
        /// <returns></returns>
        private bool RemoveTagPersonnenlRelation(int locationCardId)
        {
            var relation = db.LocationCardToPersonnels.Find(i => i.LocationCardId == locationCardId);
            var tagPos = db.LocationCardPositions.FirstOrDefault(i => i.CardId != null && i.CardId == locationCardId);
            if (tagPos != null)
            {
                db.LocationCardPositions.Remove(tagPos);
            }
            var tag = db.LocationCards.Find(locationCardId);
            bool value = true;
            if (tag != null)
            {
                tag.IsActive = false;//解除卡则不激活
                db.LocationCards.Edit(tag);
                if(relation!=null) value = db.LocationCardToPersonnels.Remove(relation);//删除关联关系
            }
            else//找不到卡 应该不可能
            {
                if(relation!=null)value = db.LocationCardToPersonnels.Remove(relation);//删除关联关系
            }
            return value;
        }
        /// <summary>
        /// 添加卡和人的关联关系
        /// </summary>
        /// <param name="locationCardId"></param>
        /// <param name="personId"></param>
        /// <returns></returns>
        private bool AddTagPersonnenlRelation(int personId, int locationCardId)
        {
            try
            {
                var relation = db.LocationCardToPersonnels.Find(i => i.PersonnelId == personId);
                if (relation != null)
                {
                    Log.Error(string.Format("Error:PersonService.AddTagPersonnenlRelation->Relation is already exist.PersonId:{0} LocationCardId:{1} RelationId:{2}", personId, locationCardId, relation.Id));
                    RemoveTagPersonnenlRelation(relation.LocationCardId);
                    return AddTagPersonnenlRelation(locationCardId, personId);
                }
                else
                {
                    bool r = db.LocationCardToPersonnels.Add(new LocationCardToPersonnel()
                    {
                        PersonnelId = personId,
                        LocationCardId = locationCardId
                    });

                    var tag = db.LocationCards.Find(locationCardId);
                    if (tag != null && !tag.IsActive)
                    {
                        tag.IsActive = true;//发卡则激活
                        db.LocationCards.Edit(tag);
                    }
                    return r;
                }
            }
            catch(Exception e)
            {
                Log.Error("Error.PersonService.AddTagPersonnenlRelation:"+e.ToString());
                return false;
            }           
        }
        public TagPosition GetPositon(string personId)
        {
            try
            {
                var query = from r in db.LocationCardToPersonnels.DbSet
                            join tag in db.LocationCards.DbSet on r.LocationCardId equals tag.Id
                            join pos in db.LocationCardPositions.DbSet on tag.Code equals pos.Id
                            where r.PersonnelId + "" == personId
                            select pos;
                TagPosition tagPos = query.FirstOrDefault().ToTModel();
                return tagPos;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetPositon", ex.ToString());
                return null;
            }
        }


        public IList<TEntity> GetListByArea(string areaId)
        {
            try
            {
                var query = from r in db.LocationCardToPersonnels.DbSet
                            join p in dbSet.DbSet on r.PersonnelId equals p.Id
                            join tag in db.LocationCards.DbSet on r.LocationCardId equals tag.Id
                            join pos in db.LocationCardPositions.DbSet on tag.Code equals pos.Id
                            where pos.AreaId + "" == areaId
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
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetListByArea", ex.ToString());
                return null;
            }
        }

        public IList<TEntity> GetListByRole(string role)
        {
            try
            {
                var roleId = role.ToInt();
                var query = from r in db.LocationCardToPersonnels.DbSet
                            join p in dbSet.DbSet on r.PersonnelId equals p.Id
                            join tag in db.LocationCards.DbSet on r.LocationCardId equals tag.Id
                            join pos in db.LocationCardPositions.DbSet on tag.Code equals pos.Id
                            where tag.CardRoleId == roleId
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
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetListByRole", ex.ToString());
                return null;
            }
        }

        public List<TEntity> GetListEx(bool isFilterByTag)
        {
            try
            {
                
                var list = db.Personnels.ToList();
                if (list == null) return null;
                var tagToPersons = db.LocationCardToPersonnels.ToList();
                var postList = db.Posts.ToList();//职位
                var tagList = db.LocationCards.ToList();//关联标签
                var departList = db.Departments.ToList();//部门
                var cardpositionList = db.LocationCardPositions.ToList();//卡位置
                var areaList = db.Areas.ToList();//区域
                var ps = list.ToTModel();
                var ps2 = new List<Personnel>();
                foreach (var p in ps)
                {
                    var ttp = tagToPersons.Find(i => i.PersonnelId == p.Id);
                    if (ttp != null)
                    {
                        p.Tag = tagList.Find(i => i.Id == ttp.LocationCardId).ToTModel();
                        p.TagId = ttp.LocationCardId;
                        var lcp = cardpositionList.Find(i => i.CardId == p.TagId);
                        if (lcp != null && lcp.AreaId != null)
                        {
                            p.AreaId = lcp.AreaId;
                            var area = areaList.Find(i => i.Id == p.AreaId);
                            if (area != null)
                            {
                                p.AreaName = area.Name;
                            }
                        }
                        ps2.Add(p);
                    }
                    else
                    {
                        if (!isFilterByTag)//如果不过滤的话，显示全部人员列表；过滤的话，只返回有绑定标签的人员列表
                            ps2.Add(p);
                    }
                    //p.Tag = tagList.Find(i => i.Id == p.TagId).ToTModel();
                    p.Parent = departList.Find(i => i.Id == p.ParentId).ToTModel();
                }
                var r = ps2.OrderByDescending(i => i.TagId != null).ThenBy(i => i.ParentId).ThenBy(i => i.Name).ToList();
                var p1 = r.Find(i => i.Name == "邱秀丽");
                return r.ToWCFList();
            }
            catch (Exception ex)
            {
                LogEvent.Error(ex);
                return null;
            }

        }

        public List<TEntity> GetList(bool detail, bool showAll)
        {
            try
            {
                if (detail)
                {
                    return GetListEx(false);
                }
                else
                {
                    var list = new List<TEntity>();
                    var list1 = dbSet.ToList();
                    var query = from p in dbSet.DbSet
                                join r in db.LocationCardToPersonnels.DbSet on p.Id equals r.PersonnelId
                                    into rList
                                from r2 in rList.DefaultIfEmpty() //left join
                                join tag in db.LocationCards.DbSet on r2.LocationCardId equals tag.Id
                                    into tagList
                                from tag2 in tagList.DefaultIfEmpty()
                                join pos in db.LocationCardPositions.DbSet on tag2.Code equals pos.Id
                                    into posList
                                from pos2 in posList.DefaultIfEmpty() //left join
                                select new { Person = p, Tag = tag2, Pos = pos2 };

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
                        catch (Exception ex1)
                        {
                            Log.Error("PersonService.GetList Item:" + ex1);
                        }

                    }
                    return list;
                }             
            }
            catch (Exception ex)
            {
                Log.Error(tag, "PersonService.GetList:" + ex);
                return null;
            }
        }

        public static string tag = "PersonService";

        public List<TEntity> GetList()
        {
            return GetList(false, true);
        }

        public IList<TEntity> GetListByName(string name)
        {
            try
            {
                var devInfoList = GetList(true, true).Where(i => i.Name.Contains(name)).ToList();
                return devInfoList.ToWCFList();
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetListByName", "Exception:" + ex);
                return null;
            }
        }

        public List<TEntity> GetListByPid(string pid)
        {
            try
            {
                return dbSet.GetListByPid(pid.ToInt()).ToWcfModelList();
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetListByPid", "Exception:" + ex);
                return null;
            }
        }

        public TPEntity GetParent(string id)
        {
            try
            {
                var item = dbSet.Find(id.ToInt());
                if (item == null) return null;
                return new DepartmentService(db).GetEntity(item.ParentId + "");
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetParent", "Exception:" + ex);
                return null;
            }
        }

        public TEntity Post(TEntity item)
        {
            try
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
            catch (System.Exception ex)
            {
                Log.Error(tag, "Post", "Exception:" + ex);
                return null;
            }
        }

        public TEntity Post(string pid, TEntity item)
        {
            try
            {
                item.ParentId = pid.ToInt();
                var dbItem = item.ToDbModel();
                var result = dbSet.Add(dbItem);
                return result ? dbItem.ToTModel() : null;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "Post", "Exception:" + ex);
                return null;
            }
        }

        public TEntity Put(TEntity item)
        {
            try
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
            catch (System.Exception ex)
            {
                Log.Error(tag, "Put", "Exception:" + ex);
                return null;
            }
        }

        public TEntity SetRole(string personId, string roleId)
        {
            try
            {
                int id = personId.ToInt();
                var tp = db.LocationCardToPersonnels.Find(i => i.PersonnelId == id);
                if (tp == null) return null;
                TagService tagService = new TagService(db);
                var tag = tagService.SetRole(tp.LocationCardId + "", roleId);
                if (tag == null) return null;
                return GetEntity(personId);
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "SetRole", "Exception:" + ex);
                return null;
            }
        }

        public List<NearbyPerson> GetNearbyPerson_Currency(int id, float fDis)
        {
            try
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
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetNearbyPerson_Currency", "Exception:" + ex);
                return null;
            }
        }

        public List<NearbyPerson> GetNearbyPerson_Alarm(int id, float fDis)
        {
            try
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
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetNearbyPerson_Alarm", "Exception:" + ex);
                return null;
            }

        }

        public List<TEntity> FindPersonList(string key)
        {
            return db.Personnels.GetListByName(key).ToWcfModelList();
        }

        public int AddPerson(TEntity p)
        {
            try
            {
                var dbP = p.ToDbModel();
                bool r = db.Personnels.Add(dbP);
                if (r == false)
                {
                    return -1;
                }
                else
                {
                    if (p.TagId != null)//如果新增的人，设置了定位卡ID。得把关系添加到cardToPersonnel
                    {
                        var s = new PersonService(db);
                        var value = s.BindWithTag(dbP.Id, (int)p.TagId);
                    }
                }
                return dbP.Id;
            }
            catch (Exception ex)
            {
                LogEvent.Error(ex);
                return -1;
            }
        }
    }
}
