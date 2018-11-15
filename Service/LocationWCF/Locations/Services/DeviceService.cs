using BLL;
using BLL.Blls.Location;
using DbModel.Tools;
using IModel.Enums;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Tools;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using TModel.Location.AreaAndDev;
using TModel.Tools;
using TEntity = Location.TModel.Location.AreaAndDev.DevInfo;
using TPEntity = Location.TModel.Location.AreaAndDev.PhysicalTopology;

namespace LocationServices.Locations.Services
{
    public interface IDeviceService : ILeafEntityService<TEntity, TPEntity>
    {
        
    }
    public class DeviceService : IDeviceService
    {
        private Bll db;
        private DevInfoBll dbSet;

        public DeviceService()
        {
            db = new Bll(false, false, false, false);
            dbSet = db.DevInfos;
        }

        public DeviceService(Bll bll)
        {
            this.db = bll;
            dbSet = db.DevInfos;
        }

        public TEntity Delete(string id)
        {
            var devId = id.ToInt();
            var dev = dbSet.Find(devId);
            if (dev.Local_TypeCode== TypeCodes.Archor)
            {
                var archor=db.Archors.DeleteByDev(devId);
                if (archor != null)
                {
                    var item = dbSet.DeleteById(id.ToInt());
                    return item.ToTModel();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                var item = dbSet.DeleteById(id.ToInt());
                return item.ToTModel();
            }
        }

        public TEntity GetEntity(string id)
        {
            var item = dbSet.Find(id.ToInt());
            return item.ToTModel();
        }

        public TPEntity GetParent(string id)
        {
            var item = dbSet.Find(id.ToInt());
            if (item == null) return null;
            return new AreaService(db).GetEntity(item.ParentId+"");
        }

        public TEntity GetEntityByDevId(string id)
        {
            List<TEntity> devInfo = dbSet.DbSet.Where(item => item.Local_DevID == id).ToList().ToTModel();
            if (devInfo != null && devInfo.Count != 0) return devInfo[0];
            else return null;
        }

        public IList<TEntity> GetList()
        {
            var devInfoList = dbSet.ToList().ToTModel();
            BindingDev(devInfoList);
            return devInfoList.ToWCFList();
        }

        public IList<TEntity> GetListByName(string name)
        {          
            var devInfoList = dbSet.GetListByName(name).ToTModel();
            BindingDev(devInfoList);
            return devInfoList.ToWCFList();
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
            dbItem.ModifyTime = DateTime.Now;
            dbItem.ModifyTimeStamp = TimeConvert.DateTimeToTimeStamp(dbItem.ModifyTime);
            var result = dbSet.Edit(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        public IList<TEntity> GetListByTypes(int[] types)
        {
            List<TEntity> devInfoList = null;
            if (types == null || types.Length == 0)
            {
                devInfoList = dbSet.ToList().ToTModel();
            }
            else
            {
                devInfoList = dbSet.DbSet.Where(i => types.Contains(i.Local_TypeCode)).ToList().ToTModel();
            }

            BindingDev(devInfoList);
            return devInfoList.ToWCFList();
        }

        /// <summary>
        /// 通过区域ID,获取区域下所有设备
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<TEntity> GetListByPids(int[] pidList)
        {
            List<TEntity> devInfoList = new List<TEntity>();
            foreach (var pid in pidList)
            {
                devInfoList.AddRange(dbSet.DbSet.Where(item => item.ParentId == pid).ToList().ToTModel());
                //BindingDev(devInfoList);
            }
            return devInfoList.ToWCFList();
        }

        /// <summary>
        /// 通过区域ID,获取区域下所有设备
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<TEntity> GetListByPid(string pid)
        {
            return dbSet.GetListByPid(pid.ToInt()).ToWcfModelList();
        }


        public IList<TEntity> DeleteListByPid(string pid)
        {
            return dbSet.DeleteListByPid(pid.ToInt()).ToWcfModelList();
        }


        private void BindingDev(List<TEntity> devInfoList)
        {
            BindingDevParent(devInfoList, db.Areas.ToList().ToTModel());
        }

        #region static 
        //public static bool IsBindingPos;

        public static void BindingDevPos(List<TEntity> devInfoList, List<DevPos> devPosList)
        {
            //if(IsBindingPos==true)return;
            //IsBindingPos = true;
            if (devInfoList == null || devInfoList.Count == 0)
            {
                Console.WriteLine("DevInfoList is null");
                return;
            }
            foreach (var item in devInfoList)
            {
                DevPos pos = devPosList.Find(o => o.DevID == item.DevID);
                if (pos == null)
                {
                    Console.WriteLine("设备：{0} 加载位置信息失败.", item.DevID);
                }
                else
                {
                    item.SetPos(pos);
                }
            }
        }
        public static void BindingDevParent(List<TEntity> devInfoList, List<TPEntity> nodeList)
        {
            //if(IsBindingPos==true)return;
            //IsBindingPos = true;
            if (devInfoList == null || devInfoList.Count == 0)
            {
                Console.WriteLine("DevInfoList is null");
                return;
            }
            if (nodeList != null)
                foreach (PhysicalTopology node in nodeList)
                {
                    node.Parent = nodeList.Find(i => i.Id == node.ParentId);
                }
            foreach (var item in devInfoList)
            {
                PhysicalTopology node = nodeList.Find(o => o.Id == item.ParentId);
                if (node == null)
                {
                    Console.WriteLine("设备：{0} 加载位置信息失败.", item.DevID);
                }
                else
                {
                    //item.Parent = node;
                    item.Path = GetPath(node);
                    //Console.WriteLine("path：{0} ", item.Path);
                }
            }
        }

        public static string GetPath(PhysicalTopology node)
        {
            if (node.Parent == null)
            {
                //return node.Name;
                return "";
            }
            else
            {
                string path = GetPath(node.Parent);
                if (string.IsNullOrEmpty(path)) return node.Name;
                else
                {
                    return path + "->" + node.Name;
                }
            }
        }

        #endregion

        public List<NearbyDev> GetNearbyDev_Currency(int id)
        {
            List<NearbyDev> lst = new List<NearbyDev>();
            DbModel.Location.Data.LocationCardPosition lcp = db.LocationCardPositions.DbSet.Where(p => p.PersonId == id).FirstOrDefault();
            if (lcp == null || lcp.AreaId == null)
            {
                return lst;
            }

            int? AreadId = lcp.AreaId;
            float PosX = lcp.X;
            float PosY = lcp.Y;
            float PosZ = lcp.Z;

            float PosX2 = 0;
            float PosY2 = 0;
            float PosZ2 = 0;

            float sqrtDistance = 0;
            float Distance = 0;

            var query = from t1 in db.DevInfos.DbSet
                        join t2 in db.DevTypes.DbSet on t1.Local_TypeCode equals t2.TypeCode
                        join t3 in db.Areas.DbSet on t1.ParentId equals t3.Id
                        where t1.ParentId == AreadId
                        select new NearbyDev { id = t1.Id, Name = t1.Name, TypeName = t2.TypeName, Area = t3.Name, X = t1.PosX, Y = t1.PosY, Z = t1.PosZ };
            if (query != null)
            {
                lst = query.ToList();
            }

            foreach (NearbyDev item in lst)
            {
                PosX2 = item.X - PosX;
                PosY2 = item.Y - PosY;
                PosZ2 = item.Z - PosZ;

                sqrtDistance = PosX2 * PosX2 + PosY2 * PosY2 + PosZ2 * PosZ2;
                Distance = (float)System.Math.Sqrt(sqrtDistance);
                item.Distance = Distance;

                PosX2 = 0;
                PosY2 = 0;
                PosZ2 = 0;
                sqrtDistance = 0;
                Distance = 0;
            }

            lst.Sort(new DevDistanceCompare());
            
            return lst;
        }

        public List<NearbyDev> GetNearbyCamera_Alarm(int id)
        {
            List<NearbyDev> lst = new List<NearbyDev>();
            DbModel.Location.Data.LocationCardPosition lcp = db.LocationCardPositions.DbSet.Where(p => p.PersonId == id).FirstOrDefault();
            if (lcp == null || lcp.AreaId == null)
            {
                return lst;
            }

            int? AreadId = lcp.AreaId;
            float PosX = lcp.X;
            float PosY = lcp.Y;
            float PosZ = lcp.Z;

            float PosX2 = 0;
            float PosY2 = 0;
            float PosZ2 = 0;

            float sqrtDistance = 0;
            float Distance = 0;

            var query = from t1 in db.DevAlarms.DbSet
                        join t2 in db.DevInfos.DbSet on t1.DevInfoId equals t2.Id
                        join t3 in db.DevTypes.DbSet on t2.Local_TypeCode equals t3.TypeCode
                        join t4 in db.Areas.DbSet on t2.ParentId equals t4.Id
                        where t2.ParentId == AreadId && (t2.Local_TypeCode == 3000201 || t2.Local_TypeCode == 14 || t2.Local_TypeCode == 3000610 || t2.Local_TypeCode == 1000102)
                        select new NearbyDev { id = t2.Id, Name = t2.Name, TypeName = t3.TypeName, Area = t4.Name, X = t2.PosX, Y = t2.PosY, Z = t2.PosZ };
            if (query != null)
            {
                lst = query.ToList();
            }

            foreach (NearbyDev item in lst)
            {
                PosX2 = item.X - PosX;
                PosY2 = item.Y - PosY;
                PosZ2 = item.Z - PosZ;

                sqrtDistance = PosX2 * PosX2 + PosY2 * PosY2 + PosZ2 * PosZ2;
                Distance = (float)System.Math.Sqrt(sqrtDistance);
                item.Distance = Distance;

                PosX2 = 0;
                PosY2 = 0;
                PosZ2 = 0;
                sqrtDistance = 0;
                Distance = 0;
            }

            lst.Sort(new DevDistanceCompare());

            return lst;
        }
    }
}
