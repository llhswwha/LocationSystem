using BLL;
using DbModel.Tools;
using Location.Model.DataObjects.ObjectAddList;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Tools;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Tools;

namespace LocationServices.Locations.Services
{
    public interface IDeviceService:IEntityService<DevInfo>
    {

    }
    public class DeviceService : IDeviceService
    {
        private Bll db;

        public DeviceService()
        {
            db = new Bll(false, false, false, false);
        }

        public DeviceService(Bll bll)
        {
            this.db = bll;
        }

        public DevInfo Delete(string id)
        {
            var item= db.DevInfos.DeleteById(id.ToInt());
            return item.ToTModel();
        }

        public DevInfo GetEntity(string id)
        {
            var item = db.DevInfos.Find(id.ToInt());
            return item.ToTModel();
        }

        public DevInfo GetEntityByDevId(string id)
        {
            List<DevInfo> devInfo = db.DevInfos.DbSet.Where(item => item.Local_DevID == id).ToList().ToTModel();
            if (devInfo != null && devInfo.Count != 0) return devInfo[0];
            else return null;
        }

        public IList<DevInfo> GetList()
        {
            var devInfoList = db.DevInfos.DbSet.ToList().ToTModel();
            BindingDev(devInfoList);
            return devInfoList.ToWCFList();
        }

        public IList<DevInfo> GetListByName(string name)
        {          
            var devInfoList = db.DevInfos.DbSet.Where(i => i.Name.Contains(name)).ToList().ToTModel();
            BindingDev(devInfoList);
            return devInfoList.ToWCFList();
        }

        public DevInfo Post(DevInfo item)
        {
            var dbItem = item.ToDbModel();
            var result = db.DevInfos.Add(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public DevInfo Post(string pid, DevInfo item)
        {
            item.ParentId = pid.ToInt();
            var dbItem = item.ToDbModel();
            var result = db.DevInfos.Add(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public DevInfo Put(DevInfo item)
        {
            var dbItem = item.ToDbModel();
            dbItem.ModifyTime = DateTime.Now;
            dbItem.ModifyTimeStamp = TimeConvert.DateTimeToTimeStamp(dbItem.ModifyTime);
            var result = db.DevInfos.Edit(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        /// <summary>
        /// 获取所有的设备基本信息
        /// </summary>
        /// <returns></returns>
        public IList<DevInfo> GetListByTypes(int[] types)
        {
            List<DevInfo> devInfoList = null;
            if (types == null || types.Length == 0)
            {
                devInfoList = db.DevInfos.ToList().ToTModel();
            }
            else
            {
                devInfoList = db.DevInfos.DbSet.Where(i => types.Contains(i.Local_TypeCode)).ToList().ToTModel();
            }

            BindingDev(devInfoList);
            return devInfoList.ToWCFList();
        }

        /// <summary>
        /// 通过区域ID,获取区域下所有设备
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public IList<DevInfo> GetListByPids(int[] pidList)
        {
            List<DevInfo> devInfoList = new List<DevInfo>();
            foreach (var pid in pidList)
            {
                devInfoList.AddRange(db.DevInfos.DbSet.Where(item => item.ParentId == pid).ToList().ToTModel());
                //BindingDev(devInfoList);
            }
            return devInfoList.ToWCFList();
        }


        private void BindingDev(List<DevInfo> devInfoList)
        {
            BindingDevParent(devInfoList, db.Areas.ToList().ToTModel());
        }

        #region static 
        //public static bool IsBindingPos;

        public static void BindingDevPos(List<DevInfo> devInfoList, List<DevPos> devPosList)
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
        public static void BindingDevParent(List<DevInfo> devInfoList, List<PhysicalTopology> nodeList)
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
