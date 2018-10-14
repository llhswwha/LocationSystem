using BLL;
using BLL.Blls.Location;
using DbModel.Tools;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Tools;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var item= dbSet.DeleteById(id.ToInt());
            return item.ToTModel();
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
            var devInfoList = dbSet.DbSet.ToList().ToTModel();
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
        public IList<TEntity> GetListByPids(int[] pidList)
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
        public IList<TEntity> GetListByPid(string pid)
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
    }
}
