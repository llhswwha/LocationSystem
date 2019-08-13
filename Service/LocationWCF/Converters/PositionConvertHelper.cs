using DbModel.Tools;
using Location.TModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Converters
{
    public static class PositionConvertHelper
    {

        #region Location.TModel.Location.Data.TagPosition <=> DbModel.Location.Data.LocationCardPosition

        public static List<Location.TModel.Location.Data.TagPosition> ToWcfModelList(
            this List<DbModel.Location.Data.LocationCardPosition> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.Data.TagPosition ToTModel(
            this DbModel.Location.Data.LocationCardPosition item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.Data.TagPosition();
            item2.Tag = item1.Id;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.Time = item1.DateTimeStamp;
            item2.DateTime = item1.DateTime;//在TagPosition添加DateTime
            item2.Power = item1.Power;
            item2.Number = item1.Number;
            item2.Flag = item1.Flag;
            if (!string.IsNullOrEmpty(item1.ArchorsText))
            {
                item2.Archors = item1.ArchorsText.Split('@').ToList();
            }
            else
            {
                item2.Archors = null;
            }
            item2.AreaId = item1.AreaId;
            //item2.AreaPath = item1.AreaPath;
            //item2.PersonId = item1.PersonId;
            item2.AreaState = item1.AreaState;
            item2.PowerState = item1.PowerState;
            item2.MoveState = item1.MoveState;
            return item2;
        }

        public static List<Location.TModel.Location.Data.TagPosition> ToTModel(
            this List<DbModel.Location.Data.LocationCardPosition> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.Data.TagPosition>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.Data.LocationCardPosition ToDbModel(
            this Location.TModel.Location.Data.TagPosition item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Data.LocationCardPosition();
            item2.Id = item1.Tag;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.DateTime = TimeConvert.ToDateTime(item1.Time);
            item2.DateTimeStamp = item1.Time;
            item2.Power = item1.Power;
            item2.Number = item1.Number;
            item2.Flag = item1.Flag;
            item2.Archors = item1.Archors;
            item2.AreaId = item1.AreaId;
            item2.AreaState = item1.AreaState;
            item2.PowerState = item1.PowerState;
            item2.MoveState = item1.MoveState;
            return item2;
        }

        public static List<DbModel.Location.Data.LocationCardPosition> ToDbModel(
            this List<Location.TModel.Location.Data.TagPosition> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Data.LocationCardPosition>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region Location.TModel.LocationHistory.Data.Position <=> DbModel.LocationHistory.Data.Position

        public static List<Location.TModel.LocationHistory.Data.Position> ToWcfModelList(
            this List<DbModel.LocationHistory.Data.Position> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.LocationHistory.Data.Position ToTModel(
            this DbModel.LocationHistory.Data.Position item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.LocationHistory.Data.Position();
            item2.Id = item1.Id;
            item2.PersonnelID = item1.PersonnelID;
            item2.Code = item1.Code;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.DateTime = item1.DateTime;
            //item2.DateTime = TimeConvert.TimeStampToDateTime(item1.DateTimeStamp);
            item2.Time = item1.DateTimeStamp;
            item2.Power = item1.Power;
            item2.Number = item1.Number;
            item2.Flag = item1.Flag;
            item2.Archors = item1.Archors;
            item2.ArchorsText = item1.ArchorsText;
            if (!string.IsNullOrEmpty(item1.ArchorsText))
            {
                item2.Archors = item1.ArchorsText.Split('@').ToList();
            }
            item2.TopoNodeId = item1.AreaId;
            item2.AreaState = item1.AreaState;
            item2.PowerState = item1.PowerState;
            item2.MoveState = item1.MoveState;
            return item2;
        }

        public static List<Location.TModel.LocationHistory.Data.Position> ToTModel(
            this List<DbModel.LocationHistory.Data.Position> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.LocationHistory.Data.Position>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.LocationHistory.Data.Position ToDbModel(
            this Location.TModel.LocationHistory.Data.Position item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.LocationHistory.Data.Position();
            item2.Id = item1.Id;
            item2.PersonnelID = item1.PersonnelID;
            item2.Code = item1.Code;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.DateTime = item1.DateTime;
            item2.DateTimeStamp = item1.Time;
            item2.Power = item1.Power;
            item2.Number = item1.Number;
            item2.Flag = item1.Flag;
            item2.Archors = item1.Archors;
            item2.ArchorsText = item1.ArchorsText;
            item2.AreaId = item1.TopoNodeId;
            item2.AreaState = item1.AreaState;
            item2.PowerState = item1.PowerState;
            item2.MoveState = item1.MoveState;
            return item2;
        }

        public static List<Location.TModel.LocationHistory.Data.Pos> ToPos(
            this List<DbModel.LocationHistory.Data.Position> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.LocationHistory.Data.Pos>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToPos());
            }
            return list2;
        }

        public static Location.TModel.LocationHistory.Data.Pos ToPos(
            this DbModel.LocationHistory.Data.Position item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.LocationHistory.Data.Pos();
            //item2.Id = item1.Id;
            //item2.PersonnelID = item1.PersonnelID;
            item2.Code = item1.Code;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            //item2.DateTime = item1.DateTime;
            item2.Time = item1.DateTimeStamp;
            //item2.Power = item1.Power;
            //item2.Number = item1.Number;
            //item2.Flag = item1.Flag;
            //item2.Archors = item1.Archors;
            //item2.ArchorsText = item1.ArchorsText;
            //if (!string.IsNullOrEmpty(item1.ArchorsText))
            //{
            //    item2.Archors = item1.ArchorsText.Split('@').ToList();
            //}
            //item2.TopoNodeId = item1.AreaId;
            //item2.AreaState = item1.AreaState;
            //item2.PowerState = item1.PowerState;
            //item2.MoveState = item1.MoveState;
            return item2;
        }



        public static List<DbModel.LocationHistory.Data.Position> ToDbModel(
            this List<Location.TModel.LocationHistory.Data.Position> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.LocationHistory.Data.Position>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region Location.TModel.LocationHistory.Data.U3DPosition <=> DbModel.LocationHistory.Data.U3DPosition

        public static List<Location.TModel.LocationHistory.Data.U3DPosition> ToWcfModelList(
            this List<DbModel.LocationHistory.Data.U3DPosition> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.LocationHistory.Data.U3DPosition ToTModel(
            this DbModel.LocationHistory.Data.U3DPosition item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.LocationHistory.Data.U3DPosition();
            item2.Id = item1.Id;
            item2.Tag = item1.Code;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.DateTime = item1.DateTime;
            item2.Time = item1.DateTimeStamp;
            item2.Power = item1.Power;
            item2.Number = item1.Number;
            item2.Flag = item1.Flag;
            return item2;
        }

        public static List<Location.TModel.LocationHistory.Data.U3DPosition> ToTModel(
            this List<DbModel.LocationHistory.Data.U3DPosition> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.LocationHistory.Data.U3DPosition>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.LocationHistory.Data.U3DPosition ToDbModel(
            this Location.TModel.LocationHistory.Data.U3DPosition item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.LocationHistory.Data.U3DPosition();
            item2.Id = item1.Id;
            item2.Code = item1.Tag;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.DateTime = item1.DateTime;
            item2.DateTimeStamp = item1.Time;
            item2.Power = item1.Power;
            item2.Number = item1.Number;
            item2.Flag = item1.Flag;
            return item2;
        }

        public static List<DbModel.LocationHistory.Data.U3DPosition> ToDbModel(
            this List<Location.TModel.LocationHistory.Data.U3DPosition> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.LocationHistory.Data.U3DPosition>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region Location.TModel.LocationHistory.Data.PositionList <=> DbModel.LocationHistory.Data.PositionList
        public static List<Location.TModel.LocationHistory.Data.PositionList> ToWcfModelList(this List<DbModel.LocationHistory.Data.PositionList> list1)
        {
            return list1.ToTModel().ToWCFList();
        }
        public static Location.TModel.LocationHistory.Data.PositionList ToTModel(this DbModel.LocationHistory.Data.PositionList item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.LocationHistory.Data.PositionList();
            item2.Name = item1.Name;
            item2.Count = item1.Count;
            //item2.Items = item1.Items.ToTModel();
            return item2;
        }
        public static List<Location.TModel.LocationHistory.Data.PositionList> ToTModel(this List<DbModel.LocationHistory.Data.PositionList> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.LocationHistory.Data.PositionList>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static Location.TModel.LocationHistory.Data.PositionList ToTModel(this DbModel.LocationHistory.Data.PosInfoList item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.LocationHistory.Data.PositionList();
            item2.Name = item1.Name;
            item2.Count = item1.Count;
            //item2.Items = item1.Items.ToTModel();
            return item2;
        }
        public static List<Location.TModel.LocationHistory.Data.PositionList> ToTModel(this List<DbModel.LocationHistory.Data.PosInfoList> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.LocationHistory.Data.PositionList>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }


        public static DbModel.LocationHistory.Data.PositionList ToDbModel(this Location.TModel.LocationHistory.Data.PositionList item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.LocationHistory.Data.PositionList(item1.Name);
            item2.Name = item1.Name;
            //item2.Count = item1.Count;
            //item2.Items = item1.Items.ToDbModel();
            return item2;
        }
        public static List<DbModel.LocationHistory.Data.PositionList> ToDbModel(this List<Location.TModel.LocationHistory.Data.PositionList> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.LocationHistory.Data.PositionList>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }
        #endregion

    }
}
