using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Blls.Location;
using DbModel.Location.Work;
using Location.IModel;
using TModel.Tools;
using TEntity = DbModel.Location.Work.AreaAuthorization;
using LocationServer;
using Location.BLL.Tool;

namespace LocationServices.Locations.Services
{
    public interface IAreaAuthorizationService:INameEntityService<TEntity>
    {
        IList<TEntity> GetListByArea(string area);
        List<int> GetCardRoleAccessAreas(int role);

        bool SetCardRoleAccessAreas(int role, List<int> areaIds);
    }
    public class AreaAuthorizationService
        : NameEntityService<TEntity>
        ,IAreaAuthorizationService
    {
        public AreaAuthorizationService():base()
        {
        }

        public AreaAuthorizationService(Bll bll) : base(bll)
        {
        }

        protected override void SetDbSet()
        {
            dbSet = db.AreaAuthorizations;
        }

        public IList<TEntity> GetListByArea(string area)
        {
            int areaId = area.ToInt();
            return dbSet.Where(i => i.AreaId== areaId);
        }

        public IList<TEntity> GetList(List<int> ids)
        {
            return dbSet.Where(i => ids.Contains(i.Id));
        }

        public static string tag = "LocationService";
        public List<int> GetCardRoleAccessAreas(int role)
        {
            try
            {
                var bll = AppContext.GetLocationBll();
                var aarService = new AreaAuthorizationRecordService(bll);
                var list1 = aarService.GetAccessListByRole(role + "");
                List<int> areas = new List<int>();
                foreach (var item in list1)
                {
                    if (item.Area != null)
                    {
                        int areaId = item.Area.Id;
                        if(!areas.Contains(areaId)) areas.Add(areaId);
                    }
                }
                if (areas.Count == 0)
                {
                    return null;
                }
                return areas;
            }
            catch (Exception ex)
            {
                Log.Error(tag, "GetCardRoleAccessAreas", ex.ToString());
                return null;
            }
        }

        public bool SetCardRoleAccessAreas(int roleId, List<int> areaIds)
        {
            try
            {
                HashSet<int> areaIdHs = new HashSet<int>(areaIds); //保证角色对应区域唯一性
                var bll = AppContext.GetLocationBll();
                var aarService = new AreaAuthorizationRecordService(bll);
                var aaService = new AreaAuthorizationService(bll);
                var roleService = new TagRoleService(bll);
                var role = roleService.GetEntity(roleId + "");

                var aarList = aarService.GetListByRole(roleId + "");


                List<int> removeList = new List<int>();
                List<int> newList = new List<int>();

                for (int i = 0; i < aarList.Count; i++)
                {
                    var aar = aarList[i];
                    if (aar.AccessType == AreaAccessType.不能进入)//设置的是可以进入的权限，同时要把不能进入的权限都删了
                    {
                        //removeList.Add(aarList[i].Id);
                        var r = aarService.Delete(aar.Id + "");
                        aarList.RemoveAt(i);
                        i--;
                    }
                }

                List<int> areasIds = new List<int>();
                foreach (var item in aarList)
                {
                    if (item.Area != null)
                    {
                        int areaId = item.Area.Id;
                        if (!areasIds.Contains(areaId))areasIds.Add(areaId);//保证areasIds唯一性
                    }
                }

                foreach (var item in areaIdHs)
                {
                    if (areasIds.Contains(item))
                    {
                        areasIds.Remove(item);
                    }
                    else
                    {
                        newList.Add(item);
                    }
                }
                removeList.AddRange(areasIds);

                foreach (var areaId in removeList)
                {
                    //var aar = aarList.FirstOrDefault(i => i.Area!=null&&i.Area.Id == areaId);
                    //if (aar != null)
                    //{
                    //    var r = aarService.Delete(aar.Id + "");
                    //}

                    //权限表中，出现过AreaId和cardId对应并重复的数据。现在取消权限时，把AreadId重复的都删除掉
                    aarService.RemoveListByAreaId(areaId,aarList);
                }

                foreach (var areaId in newList)
                {
                    var list = aaService.GetListByArea(areaId + "");
                    if (list.Count == 0)//新增的区域没有对应的权限设置数据
                    {
                        var aaNew = AreaAuthorization.New();
                        aaNew.AreaId = areaId;//根节点
                        aaNew.AccessType = AreaAccessType.可以进出; //可进入的权限
                        aaNew.RangeType = AreaRangeType.Single;
                        string areaType = "区域";
                        aaNew.Name = string.Format("权限[" + areaType + "]");
                        aaNew.Description = string.Format("权限：可以进入" + areaType + "。");
                        var aaNewR = aaService.Post(aaNew);
                        list.Add(aaNewR);//后续挪动到aaService里面。
                    }
                    var aa = list.FirstOrDefault(i => i.AccessType == AreaAccessType.可以进出);
                    if (aa != null)
                    {
                        var aar = new AreaAuthorizationRecord(aa, role);
                        var r = aarService.Post(aar);
                    }
                    else
                    {
                        Log.Error("SetCardRoleAccessAreas", "为找到区域对应的权限数据:" + areaId);
                    }
                }
                RefreshData();
            }
            catch (Exception ex)
            {
                Log.Error(tag, "SetCardRoleAccessAreas", ex.ToString());
                return false;
            }
            return true;
        }

        private void RefreshData()
        {
            try
            {
                BLL.Buffers.AuthorizationBuffer.Instance(db).PubUpdateData();
            }
            catch (Exception e)
            {

            }
        }
    }
}
