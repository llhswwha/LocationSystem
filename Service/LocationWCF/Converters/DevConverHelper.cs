using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModel.Tools;
using TModel.Location.Nodes;

namespace LocationServices.Converters
{
    public static class DevConverHelper
    {
        #region Location.TModel.Location.AreaAndDev.DevInfo <=> DbModel.Location.AreaAndDev.DevInfo

        public static List<Location.TModel.Location.AreaAndDev.DevInfo> ToWcfModelList(
            this List<DbModel.Location.AreaAndDev.DevInfo> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.AreaAndDev.DevInfo ToTModel(
            this DbModel.Location.AreaAndDev.DevInfo item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.AreaAndDev.DevInfo();
            item2.Id = item1.Id;
            item2.Name = item1.Name;
            item2.ParentId = item1.ParentId;
            item2.Code = item1.Code;
            item2.KKSCode = item1.KKS;
            item2.DevID = item1.Local_DevID;
            item2.Local_CabinetID = item1.Local_CabinetID;
            item2.TypeCode = item1.Local_TypeCode;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.Abutment_DevID = item1.Abutment_DevID;
            item2.Abutment_Type = item1.Abutment_Type;
            item2.Status = item1.Status;
            item2.RunStatus = item1.RunStatus;
            item2.Placed = item1.Placed;
            item2.ModelName = item1.ModelName;
            item2.CreateTime = item1.CreateTime;
            item2.CreateTimeStamp = item1.CreateTimeStamp;
            item2.ModifyTime = item1.ModifyTime;
            item2.ModifyTimeStamp = item1.ModifyTimeStamp;
            item2.UserName = item1.UserName;
            item2.IP = item1.IP;
            //item2.PosX = item1.PosX;
            //item2.PosY = item1.PosY;
            //item2.PosZ = item1.PosZ;
            //item2.RotationX = item1.RotationX;
            //item2.RotationY = item1.RotationY;
            //item2.RotationZ = item1.RotationZ;
            //item2.ScaleX = item1.ScaleX;
            //item2.ScaleY = item1.ScaleY;
            //item2.ScaleZ = item1.ScaleZ;
            item2.SetPos(item1.GetPos());
            return item2;
        }

        public static DevNode ToTModelS(
            this DbModel.Location.AreaAndDev.DevInfo item1)
        {
            if (item1 == null) return null;
            var item2 = new DevNode();
            item2.Id = item1.Id;
            item2.Name = item1.Name;
            item2.ParentId = item1.ParentId;
            item2.Code = item1.Code;
            item2.KKS = item1.KKS;
            item2.DevID = item1.Local_DevID;
            item2.TypeCode = item1.Local_TypeCode;
            return item2;
        }

        public static List<DevNode> ToTModelS(
    this List<DbModel.Location.AreaAndDev.DevInfo> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DevNode>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModelS());
            }
            return list2;
        }

        public static List<Location.TModel.Location.AreaAndDev.DevInfo> ToTModel(
            this List<DbModel.Location.AreaAndDev.DevInfo> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.AreaAndDev.DevInfo>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.AreaAndDev.DevInfo ToDbModel(
            this Location.TModel.Location.AreaAndDev.DevInfo item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.AreaAndDev.DevInfo();
            item2.Id = item1.Id;
            item2.Name = item1.Name;
            item2.ParentId = item1.ParentId;
            item2.Code = item1.Code;
            item2.KKS = item1.KKSCode;
            item2.Local_DevID = item1.DevID;
            item2.Local_CabinetID = item1.Local_CabinetID;
            item2.Local_TypeCode = item1.TypeCode;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.Abutment_DevID = item1.Abutment_DevID;
            item2.Abutment_Type = item1.Abutment_Type;
            item2.Status = item1.Status;
            item2.RunStatus = item1.RunStatus;
            item2.Placed = item1.Placed;
            item2.ModelName = item1.ModelName;
            item2.CreateTime = item1.CreateTime;
            item2.CreateTimeStamp = item1.CreateTimeStamp;
            item2.ModifyTime = item1.ModifyTime;
            item2.ModifyTimeStamp = item1.ModifyTimeStamp;
            item2.UserName = item1.UserName;
            item2.IP = item1.IP;
            //item2.PosX = item1.PosX;
            //item2.PosY = item1.PosY;
            //item2.PosZ = item1.PosZ;
            //item2.RotationX = item1.RotationX;
            //item2.RotationY = item1.RotationY;
            //item2.RotationZ = item1.RotationZ;
            //item2.ScaleX = item1.ScaleX;
            //item2.ScaleY = item1.ScaleY;
            //item2.ScaleZ = item1.ScaleZ;
            item2.SetPos(item1.Pos);
            return item2;
        }

        public static List<DbModel.Location.AreaAndDev.DevInfo> ToDbModel(
            this List<Location.TModel.Location.AreaAndDev.DevInfo> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.AreaAndDev.DevInfo>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

    }
}
