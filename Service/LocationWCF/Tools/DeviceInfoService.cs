using System;
using System.Collections.Generic;
using Location.TModel.Location.AreaAndDev;

namespace Location.Model.DataObjects.ObjectAddList
{
    public class DeviceInfoService
    {
        //public static bool IsBindingPos;

        public void BindingDevPos(List<DevInfo> devInfoList, List<DevPos> devPosList)
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
        public void BindingDevParent(List<DevInfo> devInfoList, List<PhysicalTopology> nodeList)
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
    }
}
