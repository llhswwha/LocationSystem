using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModel.Tools;
using TModel.Location.Nodes;

namespace LocationServices.Converters
{
    public static class AreaConvertHelper
    {
        #region Location.TModel.Location.AreaAndDev.PhysicalTopology <=> DbModel.Location.AreaAndDev.Area

        public static List<Location.TModel.Location.AreaAndDev.PhysicalTopology> ToWcfModelList(
            this List<DbModel.Location.AreaAndDev.Area> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.AreaAndDev.PhysicalTopology ToTModel(
            this DbModel.Location.AreaAndDev.Area item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.AreaAndDev.PhysicalTopology();
            item2.Id = item1.Id;
            item2.Name = item1.Name;
            item2.ParentId = item1.ParentId;
            //item2.Parent = item1.Parent;
            item2.Number = item1.Number;
            item2.Type = item1.Type;
            item2.Describe = item1.Describe;
            //item2.Tag = item1.Tag;
            item2.Children = item1.Children.ToTModel();
            item2.LeafNodes = item1.LeafNodes.ToTModel();
            item2.KKS = item1.KKS;

            item2.TransfromId = item1.Id;
            item2.Transfrom = item1.GetTransformM().ToTModel();

            item2.InitBoundId = item1.InitBoundId;
            item2.InitBound = item1.InitBound.ToTModel();
            item2.EditBoundId = item1.EditBoundId;
            item2.EditBound = item1.EditBound.ToTModel();

            item2.IsRelative = item1.IsRelative;
            item2.IsCreateAreaByData = item1.IsCreateAreaByData;
            item2.IsOnAlarmArea = item1.IsOnAlarmArea;
            item2.IsOnLocationArea = item1.IsOnLocationArea;
            return item2;
        }

        public static AreaNode ToTModelS(
            this DbModel.Location.AreaAndDev.Area item1)
        {
            if (item1 == null) return null;
            var item2 = new AreaNode();
            item2.Id = item1.Id;
            item2.Name = item1.Name;
            item2.ParentId = item1.ParentId;
            //item2.Parent = item1.Parent;
            item2.Type = item1.Type;
            item2.Children = item1.Children.ToTModelS();
            item2.LeafNodes = item1.LeafNodes.ToTModelS();
            item2.KKS = item1.KKS;

            return item2;
        }

        public static List<AreaNode> ToTModelS(
           this List<DbModel.Location.AreaAndDev.Area> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<AreaNode>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModelS());
            }
            return list2;
        }

        public static List<Location.TModel.Location.AreaAndDev.PhysicalTopology> ToTModel(
            this List<DbModel.Location.AreaAndDev.Area> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.AreaAndDev.PhysicalTopology>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.AreaAndDev.Area ToDbModel(
            this Location.TModel.Location.AreaAndDev.PhysicalTopology item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.AreaAndDev.Area();
            item2.Id = item1.Id;
            //item2.Abutment_Id = item1.Abutment_Id;
            item2.Name = item1.Name;
            item2.KKS = item1.KKS;
            item2.ParentId = item1.ParentId;
            //item2.Abutment_ParentId = item1.Abutment_ParentId;
            //item2.X = item1.X;
            //item2.Y = item1.Y;
            //item2.Z = item1.Z;
            //item2.RX = item1.RX;
            //item2.RY = item1.RY;
            //item2.RZ = item1.RZ;
            //item2.SX = item1.SX;
            //item2.SY = item1.SY;
            //item2.SZ = item1.SZ;
            //item2.IsRelative = item1.IsRelative;
            //item2.IsOnNormalArea = item1.IsOnNormalArea;
            //item2.IsOnAlarmArea = item1.IsOnAlarmArea;
            //item2.IsOnLocationArea = item1.IsOnLocationArea;
            item2.SetTransform(item1.Transfrom.ToDbModel());
            item2.Number = item1.Number;
            item2.Type = item1.Type;
            item2.Describe = item1.Describe;
            item2.Children = item1.Children.ToDbModel();
            item2.LeafNodes = item1.LeafNodes.ToDbModel();
            item2.InitBoundId = item1.InitBoundId;
            item2.InitBound = item1.InitBound.ToDbModel();
            item2.EditBoundId = item1.EditBoundId;
            item2.EditBound = item1.EditBound.ToDbModel();

            item2.IsRelative = item1.IsRelative;
            item2.IsCreateAreaByData = item1.IsCreateAreaByData;
            item2.IsOnAlarmArea = item1.IsOnAlarmArea;
            item2.IsOnLocationArea = item1.IsOnLocationArea;
            return item2;
        }

        public static List<DbModel.Location.AreaAndDev.Area> ToDbModel(
            this List<Location.TModel.Location.AreaAndDev.PhysicalTopology> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.AreaAndDev.Area>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

    }
}
