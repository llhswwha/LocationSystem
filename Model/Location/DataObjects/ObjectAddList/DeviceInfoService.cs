using Location.Model.LocationTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model.DataObjects.ObjectAddList
{
    /// <summary>
    /// 设备信息帮助类
    /// </summary>
    public static class DeviceInfoService
    {
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
                    item.Pos = pos;
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
                    item.Parent = node;
                    item.Path = GetPath(node);
                    Console.WriteLine("path：{0} ", item.Path);
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
    }
}
