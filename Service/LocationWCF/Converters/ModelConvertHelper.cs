using System.Collections.Generic;
using DbModel.Location.Person;
using DbModel.Tools;
using System;
using Location.TModel.Tools;

namespace LocationServices.Converters
{
    public static class ModelConvertHelper
    {

        #region ConvertCodeGenerator自动生成代码

        //未找到DbModel的类(9):
        //TransferOfAxesConfig; DevPos; TransformM; AlarmSearchArg; ModelTypeItem; ObjectAddList; ObjectAddList_Type; ObjectAddList_ChildType; ObjectAddList_Model; 
        //未找到TModel的类(37):
        //JurisDiction; JurisDictionRecord; MobileInspection; MobileInspectionContent; MobileInspectionDev; MobileInspectionItem; OperationItem; OperationTicket; PersonnelMobileInspection; PersonnelMobileInspectionItem; SafetyMeasures; WorkTicket; EntranceGuardCardToPersonnel; LocationCardToPersonnel; Role; DevInstantData; Archor; NodeKKS; DevModel; DevType; EntranceGuardCard; OperationItemHistory; OperationTicketHistory; PersonnelMobileInspectionHistory; PersonnelMobileInspectionItemHistory; SafetyMeasuresHistory; WorkTicketHistory; EntranceGuardCardToPersonnelHistory; LocationCardToPersonnelHistory; PersonnelHistory; DevInstantDataHistory; DevInfoHistory; EntranceGuardCardHistory; LocationCardHistory; DevEntranceGuardCardAction; DevAlarmHistory; LocationAlarmHistory; 


        #region Location.TModel.Location.AreaAndDev.Bound <=> DbModel.Location.AreaAndDev.Bound

        public static List<Location.TModel.Location.AreaAndDev.Bound> ToWcfModelList(
            this List<DbModel.Location.AreaAndDev.Bound> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.AreaAndDev.Bound ToTModel(this DbModel.Location.AreaAndDev.Bound item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.AreaAndDev.Bound();
            item2.Id = item1.Id;
            item2.MinX = item1.MinX;
            item2.MaxX = item1.MaxX;
            item2.MinY = item1.MinY;
            item2.MaxY = item1.MaxY;
            item2.MinZ = item1.MinZ;
            item2.MaxZ = item1.MaxZ;
            item2.ZeroX = item1.ZeroX;
            item2.ZeroY = item1.ZeroY;
            item2.Shape = item1.Shape;
            item2.IsRelative = item1.IsRelative;
            item2.Points = item1.Points.ToTModel();
            return item2;
        }

        public static List<Location.TModel.Location.AreaAndDev.Bound> ToTModel(
            this List<DbModel.Location.AreaAndDev.Bound> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.AreaAndDev.Bound>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.AreaAndDev.Bound ToDbModel(this Location.TModel.Location.AreaAndDev.Bound item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.AreaAndDev.Bound();
            item2.Update(item1);
            return item2;
        }

        public static DbModel.Location.AreaAndDev.Bound Update(this DbModel.Location.AreaAndDev.Bound item2,Location.TModel.Location.AreaAndDev.Bound item1)
        {
            if (item1 == null) return null;
            item2.Id = item1.Id;
            item2.MinX = item1.MinX;
            item2.MaxX = item1.MaxX;
            item2.MinY = item1.MinY;
            item2.MaxY = item1.MaxY;
            item2.MinZ = item1.MinZ;
            item2.MaxZ = item1.MaxZ;
            item2.ZeroX = item1.ZeroX;
            item2.ZeroY = item1.ZeroY;
            item2.Shape = item1.Shape;
            item2.IsRelative = item1.IsRelative;
            item2.Points = item1.Points.ToDbModel();
            return item2;
        }

        public static List<DbModel.Location.AreaAndDev.Bound> ToDbModel(
            this List<Location.TModel.Location.AreaAndDev.Bound> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.AreaAndDev.Bound>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region Location.TModel.Location.AreaAndDev.ConfigArg <=> DbModel.Location.AreaAndDev.ConfigArg

        public static List<Location.TModel.Location.AreaAndDev.ConfigArg> ToWcfModelList(
            this List<DbModel.Location.AreaAndDev.ConfigArg> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.AreaAndDev.ConfigArg ToTModel(
            this DbModel.Location.AreaAndDev.ConfigArg item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.AreaAndDev.ConfigArg();
            item2.Id = item1.Id;
            item2.Name = item1.Name;
            item2.Key = item1.Key;
            item2.Value = item1.Value;
            item2.ValueType = item1.ValueType;
            item2.Describe = item1.Describe;
            item2.Classify = item1.Classify;
            return item2;
        }

        public static List<Location.TModel.Location.AreaAndDev.ConfigArg> ToTModel(
            this List<DbModel.Location.AreaAndDev.ConfigArg> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.AreaAndDev.ConfigArg>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.AreaAndDev.ConfigArg ToDbModel(
            this Location.TModel.Location.AreaAndDev.ConfigArg item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.AreaAndDev.ConfigArg();
            item2.Id = item1.Id;
            item2.Name = item1.Name;
            item2.Key = item1.Key;
            item2.Value = item1.Value;
            item2.ValueType = item1.ValueType;
            item2.Describe = item1.Describe;
            item2.Classify = item1.Classify;
            return item2;
        }

        public static List<DbModel.Location.AreaAndDev.ConfigArg> ToDbModel(
            this List<Location.TModel.Location.AreaAndDev.ConfigArg> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.AreaAndDev.ConfigArg>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion


        #region Location.TModel.Location.AreaAndDev.KKSCode <=> DbModel.Location.AreaAndDev.KKSCode

        public static List<Location.TModel.Location.AreaAndDev.KKSCode> ToWcfModelList(
            this List<DbModel.Location.AreaAndDev.KKSCode> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.AreaAndDev.KKSCode ToTModel(
            this DbModel.Location.AreaAndDev.KKSCode item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.AreaAndDev.KKSCode();
            item2.Id = item1.Id;
            item2.Serial = item1.Serial;
            item2.Name = item1.Name;
            item2.Code = item1.Code;
            item2.ParentCode = item1.ParentCode;
            item2.DesinCode = item1.DesinCode;
            item2.MainType = item1.MainType;
            item2.SubType = item1.SubType;
            item2.System = item1.System;
            return item2;
        }

        public static List<Location.TModel.Location.AreaAndDev.KKSCode> ToTModel(
            this List<DbModel.Location.AreaAndDev.KKSCode> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.AreaAndDev.KKSCode>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.AreaAndDev.KKSCode ToDbModel(
            this Location.TModel.Location.AreaAndDev.KKSCode item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.AreaAndDev.KKSCode();
            item2.Id = item1.Id;
            item2.Serial = item1.Serial;
            item2.Name = item1.Name;
            item2.Code = item1.Code;
            item2.ParentCode = item1.ParentCode;
            item2.DesinCode = item1.DesinCode;
            item2.MainType = item1.MainType;
            item2.SubType = item1.SubType;
            item2.System = item1.System;
            return item2;
        }

        public static List<DbModel.Location.AreaAndDev.KKSCode> ToDbModel(
            this List<Location.TModel.Location.AreaAndDev.KKSCode> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.AreaAndDev.KKSCode>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion


        #region Location.TModel.Location.AreaAndDev.TransformM <=> DbModel.Tools.InitInfos.TransformM

        public static List<Location.TModel.Location.AreaAndDev.TransformM> ToWcfModelList(
            this List<DbModel.Tools.InitInfos.TransformM> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.AreaAndDev.TransformM ToTModel(
            this DbModel.Tools.InitInfos.TransformM item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.AreaAndDev.TransformM();
            item2.Id = item1.Id;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.RX = item1.RX;
            item2.RY = item1.RY;
            item2.RZ = item1.RZ;
            item2.SX = item1.SX;
            item2.SY = item1.SY;
            item2.SZ = item1.SZ;
            item2.IsOnAlarmArea = item1.IsOnAlarmArea;
            item2.IsOnLocationArea = item1.IsOnLocationArea;
            item2.IsCreateAreaByData = item1.IsCreateAreaByData;
            item2.IsRelative = item1.IsRelative;
            return item2;
        }

        public static List<Location.TModel.Location.AreaAndDev.TransformM> ToTModel(
            this List<DbModel.Tools.InitInfos.TransformM> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.AreaAndDev.TransformM>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Tools.InitInfos.TransformM ToDbModel(
            this Location.TModel.Location.AreaAndDev.TransformM item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Tools.InitInfos.TransformM();
            item2.Id = item1.Id;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.RX = item1.RX;
            item2.RY = item1.RY;
            item2.RZ = item1.RZ;
            item2.SX = item1.SX;
            item2.SY = item1.SY;
            item2.SZ = item1.SZ;
            item2.IsOnAlarmArea = item1.IsOnAlarmArea;
            item2.IsOnLocationArea = item1.IsOnLocationArea;
            item2.IsCreateAreaByData = item1.IsCreateAreaByData;
            item2.IsRelative = item1.IsRelative;
            return item2;
        }

        public static List<DbModel.Tools.InitInfos.TransformM> ToDbModel(
            this List<Location.TModel.Location.AreaAndDev.TransformM> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Tools.InitInfos.TransformM>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region Location.TModel.Location.AreaAndDev.Point <=> DbModel.Location.AreaAndDev.Point

        public static List<Location.TModel.Location.AreaAndDev.Point> ToWcfModelList(
            this List<DbModel.Location.AreaAndDev.Point> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.AreaAndDev.Point ToTModel(this DbModel.Location.AreaAndDev.Point item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.AreaAndDev.Point();
            item2.Id = item1.Id;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.Index = item1.Index;
            item2.BoundId = item1.BoundId;
            //item2.Bound = item1.Bound.ToTModel();
            return item2;
        }

        public static List<Location.TModel.Location.AreaAndDev.Point> ToTModel(
            this List<DbModel.Location.AreaAndDev.Point> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.AreaAndDev.Point>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.AreaAndDev.Point ToDbModel(this Location.TModel.Location.AreaAndDev.Point item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.AreaAndDev.Point();
            item2.Update(item1);
            return item2;
        }

        public static DbModel.Location.AreaAndDev.Point Update(this DbModel.Location.AreaAndDev.Point item2, Location.TModel.Location.AreaAndDev.Point item1)
        {
            if (item1 == null) return null;
            item2.Id = item1.Id;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.Index = item1.Index;
            item2.BoundId = item1.BoundId;
            //item2.Bound = item1.Bound.ToDbModel();
            return item2;
        }

        public static List<DbModel.Location.AreaAndDev.Point> ToDbModel(
            this List<Location.TModel.Location.AreaAndDev.Point> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.AreaAndDev.Point>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region Location.TModel.Location.AreaAndDev.Post <=> DbModel.Location.AreaAndDev.Post

        public static List<Location.TModel.Location.AreaAndDev.Post> ToWcfModelList(
            this List<DbModel.Location.AreaAndDev.Post> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.AreaAndDev.Post ToTModel(this DbModel.Location.AreaAndDev.Post item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.AreaAndDev.Post();
            item2.Id = item1.Id;
            item2.Name = item1.Name;
            return item2;
        }

        public static List<Location.TModel.Location.AreaAndDev.Post> ToTModel(
            this List<DbModel.Location.AreaAndDev.Post> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.AreaAndDev.Post>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.AreaAndDev.Post ToDbModel(this Location.TModel.Location.AreaAndDev.Post item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.AreaAndDev.Post();
            item2.Id = item1.Id;
            item2.Name = item1.Name;
            return item2;
        }

        public static List<DbModel.Location.AreaAndDev.Post> ToDbModel(
            this List<Location.TModel.Location.AreaAndDev.Post> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.AreaAndDev.Post>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region Location.TModel.Location.Alarm.DeviceAlarm <=> DbModel.Location.Alarm.DevAlarm

        public static List<Location.TModel.Location.Alarm.DeviceAlarm> ToWcfModelList(
            this List<DbModel.Location.Alarm.DevAlarm> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.Alarm.DeviceAlarm ToTModel(this DbModel.Location.Alarm.DevAlarm item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.Alarm.DeviceAlarm();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.Title = item1.Title;
            item2.Message = item1.Msg;
            item2.Level = item1.Level;
            item2.Code = item1.Code;
            item2.Src = item1.Src;
            item2.DevId = item1.DevInfoId;
            item2.SetDev(item1.DevInfo.ToTModel());
            //item2.Dev = item1.DevInfo.ToTModel();
            item2.Device_desc = item1.Device_desc;
            item2.CreateTime = item1.AlarmTime;
            item2.AlarmTimeStamp = item1.AlarmTimeStamp;
            if(item1.DevInfo!=null)
                item2.AreaId = item1.DevInfo.ParentId ?? 0;
            return item2;
        }

        public static List<Location.TModel.Location.Alarm.DeviceAlarm> ToTModel(
            this List<DbModel.Location.Alarm.DevAlarm> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.Alarm.DeviceAlarm>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.Alarm.DevAlarm ToDbModel(this Location.TModel.Location.Alarm.DeviceAlarm item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Alarm.DevAlarm();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.Title = item1.Title;
            item2.Msg = item1.Message;
            item2.Level = item1.Level;
            item2.Code = item1.Code;
            item2.Src = item1.Src;
            item2.DevInfoId = item1.DevId;
            //item2.DevInfo = item1.Dev.ToDbModel();
            item2.Device_desc = item1.Device_desc;
            item2.AlarmTime = item1.CreateTime;
            item2.AlarmTimeStamp = item1.AlarmTimeStamp;
            return item2;
        }

        public static List<DbModel.Location.Alarm.DevAlarm> ToDbModel(
            this List<Location.TModel.Location.Alarm.DeviceAlarm> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Alarm.DevAlarm>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region Location.TModel.Location.AreaAndDev.Dev_DoorAccess <=> DbModel.Location.AreaAndDev.Dev_DoorAccess

        public static List<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> ToWcfModelList(
            this List<DbModel.Location.AreaAndDev.Dev_DoorAccess> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.AreaAndDev.Dev_DoorAccess ToTModel(
            this DbModel.Location.AreaAndDev.Dev_DoorAccess item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.AreaAndDev.Dev_DoorAccess();
            item2.Id = item1.Id;
            item2.DevInfoId = item1.DevInfoId;
            item2.DevID = item1.Local_DevID;
            item2.ParentId = item1.ParentId;
            item2.DoorId = item1.DoorId;
            item2.DevInfo = item1.DevInfo.ToTModel();
            return item2;
        }

        public static List<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> ToTModel(
            this List<DbModel.Location.AreaAndDev.Dev_DoorAccess> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.AreaAndDev.Dev_DoorAccess>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.AreaAndDev.Dev_DoorAccess ToDbModel(
            this Location.TModel.Location.AreaAndDev.Dev_DoorAccess item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.AreaAndDev.Dev_DoorAccess();
            item2.Id = item1.Id;
            item2.ParentId = item1.ParentId;
            item2.DoorId = item1.DoorId;
            item2.DevInfoId = item1.DevInfoId;
            item2.Local_DevID = item1.DevID;
            item2.DevInfo = item1.DevInfo.ToDbModel();
            return item2;
        }

        public static List<DbModel.Location.AreaAndDev.Dev_DoorAccess> ToDbModel(
            this List<Location.TModel.Location.AreaAndDev.Dev_DoorAccess> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.AreaAndDev.Dev_DoorAccess>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.Location.AreaAndDev.Dev_CameraInfo <=> DbModel.Location.AreaAndDev.Dev_CameraInfossss

        public static List<TModel.Location.AreaAndDev.Dev_CameraInfo> ToWcfModelList(
            this List<DbModel.Location.AreaAndDev.Dev_CameraInfo> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.Location.AreaAndDev.Dev_CameraInfo ToTModel(
            this DbModel.Location.AreaAndDev.Dev_CameraInfo item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.AreaAndDev.Dev_CameraInfo();
            item2.Id = item1.Id;
            item2.Ip = item1.Ip;
            item2.UserName = item1.UserName;           
            item2.PassWord = item1.PassWord;
            item2.Port = item1.Port;
            item2.CameraIndex = item1.CameraIndex;
            item2.ParentId = item1.ParentId;
            item2.DevInfoId = item1.DevInfoId;
            item2.Local_DevID = item1.Local_DevID;
            item2.RtspUrl = item1.RtspUrl;
            item2.DevInfo = item1.DevInfo.ToTModel();
            item2.RtspUrl = item1.RtspUrl;
            return item2;
        }

        public static List<TModel.Location.AreaAndDev.Dev_CameraInfo> ToTModel(
            this List<DbModel.Location.AreaAndDev.Dev_CameraInfo> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.AreaAndDev.Dev_CameraInfo>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.AreaAndDev.Dev_CameraInfo ToDbModel(
            this TModel.Location.AreaAndDev.Dev_CameraInfo item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.AreaAndDev.Dev_CameraInfo();
            item2.Id = item1.Id;
            item2.Ip = item1.Ip;
            item2.UserName = item1.UserName;
            item2.PassWord = item1.PassWord;
            item2.Port = item1.Port;
            item2.CameraIndex = item1.CameraIndex;
            item2.ParentId = item1.ParentId;
            item2.DevInfoId = item1.DevInfoId;
            item2.Local_DevID = item1.Local_DevID;
            item2.RtspUrl = item1.RtspUrl;
            item2.DevInfo = item1.DevInfo.ToDbModel();
            item2.RtspUrl = item1.RtspUrl;
            return item2;
        }

        public static List<DbModel.Location.AreaAndDev.Dev_CameraInfo> ToDbModel(
            this List<TModel.Location.AreaAndDev.Dev_CameraInfo> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.AreaAndDev.Dev_CameraInfo>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #endregion

        #region TModel.Location.Work.OperationItem <=> DbModel.Location.Work.OperationItem

        public static List<TModel.Location.Work.OperationItem> ToWcfModelList(
            this List<DbModel.Location.Work.OperationItem> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.Location.Work.OperationItem ToTModel(this DbModel.Location.Work.OperationItem item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.Work.OperationItem();
            item2.Id = item1.Id;
            item2.TicketId = item1.TicketId;
            item2.OperationTime = item1.OperationTime;
            item2.Mark = item1.Mark;
            item2.OrderNum = item1.OrderNum;
            item2.Item = item1.Item;
            return item2;
        }

        public static List<TModel.Location.Work.OperationItem> ToTModel(
            this List<DbModel.Location.Work.OperationItem> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.Work.OperationItem>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.Work.OperationItem ToDbModel(this TModel.Location.Work.OperationItem item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Work.OperationItem();
            item2.Id = item1.Id;
            item2.TicketId = item1.TicketId;
            item2.OperationTime = item1.OperationTime;
            item2.Mark = item1.Mark;
            item2.OrderNum = item1.OrderNum;
            item2.Item = item1.Item;
            return item2;
        }

        public static List<DbModel.Location.Work.OperationItem> ToDbModel(
            this List<TModel.Location.Work.OperationItem> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Work.OperationItem>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.Location.Work.OperationTicket <=> DbModel.Location.Work.OperationTicket

        public static List<TModel.Location.Work.OperationTicket> ToWcfModelList(
            this List<DbModel.Location.Work.OperationTicket> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.Location.Work.OperationTicket ToTModel(this DbModel.Location.Work.OperationTicket item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.Work.OperationTicket();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.No = item1.No;
            item2.OperationTask = item1.OperationTask;
            item2.OperationStartTime = item1.OperationStartTime;
            item2.OperationEndTime = item1.OperationEndTime;
            item2.OperationItems = item1.OperationItems.ToTModel();
            item2.Guardian = item1.Guardian;
            item2.Operator = item1.Operator;
            item2.DutyOfficer = item1.DutyOfficer;
            item2.Dispatch = item1.Dispatch;
            item2.Remark = item1.Remark;
            return item2;
        }

        public static List<TModel.Location.Work.OperationTicket> ToTModel(
            this List<DbModel.Location.Work.OperationTicket> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.Work.OperationTicket>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.Work.OperationTicket ToDbModel(this TModel.Location.Work.OperationTicket item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Work.OperationTicket();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.No = item1.No;
            item2.OperationTask = item1.OperationTask;
            item2.OperationStartTime = item1.OperationStartTime;
            item2.OperationEndTime = item1.OperationEndTime;
            item2.OperationItems = item1.OperationItems.ToDbModel();
            item2.Guardian = item1.Guardian;
            item2.Operator = item1.Operator;
            item2.DutyOfficer = item1.DutyOfficer;
            item2.Dispatch = item1.Dispatch;
            item2.Remark = item1.Remark;
            return item2;
        }

        public static List<DbModel.Location.Work.OperationTicket> ToDbModel(
            this List<TModel.Location.Work.OperationTicket> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Work.OperationTicket>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.Location.Work.SafetyMeasures <=> DbModel.Location.Work.SafetyMeasures

        public static List<TModel.Location.Work.SafetyMeasures> ToWcfModelList(
            this List<DbModel.Location.Work.SafetyMeasures> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.Location.Work.SafetyMeasures ToTModel(this DbModel.Location.Work.SafetyMeasures item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.Work.SafetyMeasures();
            item2.Id = item1.Id;
            item2.No = item1.No;
            item2.LssuerContent = item1.LssuerContent;
            item2.LicensorContent = item1.LicensorContent;
            item2.WorkTicketId = item1.WorkTicketId;
            return item2;
        }

        public static List<TModel.Location.Work.SafetyMeasures> ToTModel(
            this List<DbModel.Location.Work.SafetyMeasures> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.Work.SafetyMeasures>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.Work.SafetyMeasures ToDbModel(this TModel.Location.Work.SafetyMeasures item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Work.SafetyMeasures();
            item2.Id = item1.Id;
            item2.No = item1.No;
            item2.LssuerContent = item1.LssuerContent;
            item2.LicensorContent = item1.LicensorContent;
            item2.WorkTicketId = item1.WorkTicketId;
            return item2;
        }

        public static List<DbModel.Location.Work.SafetyMeasures> ToDbModel(
            this List<TModel.Location.Work.SafetyMeasures> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Work.SafetyMeasures>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.Location.Work.WorkTicket <=> DbModel.Location.Work.WorkTicket

        public static List<TModel.Location.Work.WorkTicket> ToWcfModelList(
            this List<DbModel.Location.Work.WorkTicket> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.Location.Work.WorkTicket ToTModel(this DbModel.Location.Work.WorkTicket item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.Work.WorkTicket();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.No = item1.No;
            item2.PersonInCharge = item1.PersonInCharge;
            item2.HeadOfWorkClass = item1.HeadOfWorkClass;
            item2.WorkPlace = item1.WorkPlace;
            item2.JobContent = item1.JobContent;
            item2.SafetyMeasuress = item1.SafetyMeasuress.ToTModel();
            item2.StartTimeOfPlannedWork = item1.StartTimeOfPlannedWork;
            item2.EndTimeOfPlannedWork = item1.EndTimeOfPlannedWork;
            item2.WorkCondition = item1.WorkCondition;
            item2.Lssuer = item1.Lssuer;
            item2.Licensor = item1.Licensor;
            item2.Approver = item1.Approver;
            item2.Comment = item1.Comment;
            return item2;
        }

        public static List<TModel.Location.Work.WorkTicket> ToTModel(this List<DbModel.Location.Work.WorkTicket> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.Work.WorkTicket>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.Work.WorkTicket ToDbModel(this TModel.Location.Work.WorkTicket item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Work.WorkTicket();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.No = item1.No;
            item2.PersonInCharge = item1.PersonInCharge;
            item2.HeadOfWorkClass = item1.HeadOfWorkClass;
            item2.WorkPlace = item1.WorkPlace;
            item2.JobContent = item1.JobContent;
            item2.SafetyMeasuress = item1.SafetyMeasuress.ToDbModel();
            item2.StartTimeOfPlannedWork = item1.StartTimeOfPlannedWork;
            item2.EndTimeOfPlannedWork = item1.EndTimeOfPlannedWork;
            item2.WorkCondition = item1.WorkCondition;
            item2.Lssuer = item1.Lssuer;
            item2.Licensor = item1.Licensor;
            item2.Approver = item1.Approver;
            item2.Comment = item1.Comment;
            return item2;
        }

        public static List<DbModel.Location.Work.WorkTicket> ToDbModel(this List<TModel.Location.Work.WorkTicket> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Work.WorkTicket>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.Location.Work.MobileInspectionContent <=> DbModel.Location.Work.MobileInspectionContent

        public static List<TModel.Location.Work.MobileInspectionContent> ToWcfModelList(
            this List<DbModel.Location.Work.MobileInspectionContent> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.Location.Work.MobileInspectionContent ToTModel(
            this DbModel.Location.Work.MobileInspectionContent item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.Work.MobileInspectionContent();
            item2.Id = item1.Id;
            item2.ParentId = item1.ParentId;
            item2.Content = item1.Content;
            item2.nOrder = item1.nOrder;
            return item2;
        }

        public static List<TModel.Location.Work.MobileInspectionContent> ToTModel(
            this List<DbModel.Location.Work.MobileInspectionContent> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.Work.MobileInspectionContent>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.Work.MobileInspectionContent ToDbModel(
            this TModel.Location.Work.MobileInspectionContent item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Work.MobileInspectionContent();
            item2.Id = item1.Id;
            item2.ParentId = item1.ParentId;
            item2.Content = item1.Content;
            item2.nOrder = item1.nOrder;
            return item2;
        }

        public static List<DbModel.Location.Work.MobileInspectionContent> ToDbModel(
            this List<TModel.Location.Work.MobileInspectionContent> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Work.MobileInspectionContent>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.Location.Work.MobileInspectionDev <=> DbModel.Location.Work.MobileInspectionDev

        public static List<TModel.Location.Work.MobileInspectionDev> ToWcfModelList(
            this List<DbModel.Location.Work.MobileInspectionDev> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.Location.Work.MobileInspectionDev ToTModel(
            this DbModel.Location.Work.MobileInspectionDev item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.Work.MobileInspectionDev();
            item2.Id = item1.Id;
            item2.Name = item1.Name;
            item2.MobileInspectionContents = item1.MobileInspectionContents.ToTModel();
            return item2;
        }

        public static List<TModel.Location.Work.MobileInspectionDev> ToTModel(
            this List<DbModel.Location.Work.MobileInspectionDev> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.Work.MobileInspectionDev>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.Work.MobileInspectionDev ToDbModel(
            this TModel.Location.Work.MobileInspectionDev item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Work.MobileInspectionDev();
            item2.Id = item1.Id;
            item2.Name = item1.Name;
            item2.MobileInspectionContents = item1.MobileInspectionContents.ToDbModel();
            return item2;
        }

        public static List<DbModel.Location.Work.MobileInspectionDev> ToDbModel(
            this List<TModel.Location.Work.MobileInspectionDev> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Work.MobileInspectionDev>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.Location.Work.MobileInspectionItem <=> DbModel.Location.Work.MobileInspectionItem

        public static List<TModel.Location.Work.MobileInspectionItem> ToWcfModelList(
            this List<DbModel.Location.Work.MobileInspectionItem> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.Location.Work.MobileInspectionItem ToTModel(
            this DbModel.Location.Work.MobileInspectionItem item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.Work.MobileInspectionItem();
            item2.Id = item1.Id;
            item2.ItemName = item1.ItemName;
            item2.nOrder = item1.nOrder;
            item2.PID = item1.PID;
            item2.DevId = item1.DevId;
            item2.DevName = item1.DevName;
            return item2;
        }

        public static List<TModel.Location.Work.MobileInspectionItem> ToTModel(
            this List<DbModel.Location.Work.MobileInspectionItem> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.Work.MobileInspectionItem>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.Work.MobileInspectionItem ToDbModel(
            this TModel.Location.Work.MobileInspectionItem item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Work.MobileInspectionItem();
            item2.Id = item1.Id;
            item2.ItemName = item1.ItemName;
            item2.nOrder = item1.nOrder;
            item2.PID = item1.PID;
            item2.DevId = item1.DevId;
            item2.DevName = item1.DevName;
            return item2;
        }

        public static List<DbModel.Location.Work.MobileInspectionItem> ToDbModel(
            this List<TModel.Location.Work.MobileInspectionItem> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Work.MobileInspectionItem>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.Location.Work.MobileInspection <=> DbModel.Location.Work.MobileInspection

        public static List<TModel.Location.Work.MobileInspection> ToWcfModelList(
            this List<DbModel.Location.Work.MobileInspection> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.Location.Work.MobileInspection ToTModel(this DbModel.Location.Work.MobileInspection item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.Work.MobileInspection();
            item2.Id = item1.Id;
            item2.nOrder = item1.nOrder;
            item2.Name = item1.Name;
            item2.Items = item1.Items.ToTModel();
            return item2;
        }

        public static List<TModel.Location.Work.MobileInspection> ToTModel(
            this List<DbModel.Location.Work.MobileInspection> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.Work.MobileInspection>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.Work.MobileInspection ToDbModel(this TModel.Location.Work.MobileInspection item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Work.MobileInspection();
            item2.Id = item1.Id;
            item2.nOrder = item1.nOrder;
            item2.Name = item1.Name;
            item2.Items = item1.Items.ToDbModel();
            return item2;
        }

        public static List<DbModel.Location.Work.MobileInspection> ToDbModel(
            this List<TModel.Location.Work.MobileInspection> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Work.MobileInspection>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.Location.Work.PersonnelMobileInspectionItem <=> DbModel.Location.Work.PersonnelMobileInspectionItem

        public static List<TModel.Location.Work.PersonnelMobileInspectionItem> ToWcfModelList(
            this List<DbModel.Location.Work.PersonnelMobileInspectionItem> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.Location.Work.PersonnelMobileInspectionItem ToTModel(
            this DbModel.Location.Work.PersonnelMobileInspectionItem item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.Work.PersonnelMobileInspectionItem();
            item2.Id = item1.Id;
            item2.ItemId = item1.ItemId;
            item2.PID = item1.PID;
            item2.ItemName = item1.ItemName;
            item2.nOrder = item1.nOrder;
            item2.DevId = item1.DevId;
            item2.DevName = item1.DevName;
            item2.PunchTime = item1.PunchTime;
            return item2;
        }

        public static List<TModel.Location.Work.PersonnelMobileInspectionItem> ToTModel(
            this List<DbModel.Location.Work.PersonnelMobileInspectionItem> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.Work.PersonnelMobileInspectionItem>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.Work.PersonnelMobileInspectionItem ToDbModel(
            this TModel.Location.Work.PersonnelMobileInspectionItem item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Work.PersonnelMobileInspectionItem();
            item2.Id = item1.Id;
            item2.ItemId = item1.ItemId;
            item2.PID = item1.PID;
            item2.ItemName = item1.ItemName;
            item2.nOrder = item1.nOrder;
            item2.DevId = item1.DevId;
            item2.DevName = item1.DevName;
            item2.PunchTime = item1.PunchTime;
            return item2;
        }

        public static List<DbModel.Location.Work.PersonnelMobileInspectionItem> ToDbModel(
            this List<TModel.Location.Work.PersonnelMobileInspectionItem> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Work.PersonnelMobileInspectionItem>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.Location.Work.PersonnelMobileInspection <=> DbModel.Location.Work.PersonnelMobileInspection

        public static List<TModel.Location.Work.PersonnelMobileInspection> ToWcfModelList(
            this List<DbModel.Location.Work.PersonnelMobileInspection> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.Location.Work.PersonnelMobileInspection ToTModel(
            this DbModel.Location.Work.PersonnelMobileInspection item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.Work.PersonnelMobileInspection();
            item2.Id = item1.Id;
            item2.PersonnelId = item1.PersonnelId;
            item2.PersonnelName = item1.PersonnelName;
            item2.MobileInspectionId = item1.MobileInspectionId;
            item2.MobileInspectionName = item1.MobileInspectionName;
            item2.PlanStartTime = item1.PlanStartTime;
            item2.PlanEndTime = item1.PlanEndTime;
            item2.StartTime = item1.StartTime;
            item2.bOverTime = item1.bOverTime;
            item2.Remark = item1.Remark;
            item2.list = item1.list.ToTModel();
            return item2;
        }

        public static List<TModel.Location.Work.PersonnelMobileInspection> ToTModel(
            this List<DbModel.Location.Work.PersonnelMobileInspection> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.Work.PersonnelMobileInspection>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.Work.PersonnelMobileInspection ToDbModel(
            this TModel.Location.Work.PersonnelMobileInspection item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Work.PersonnelMobileInspection();
            item2.Id = item1.Id;
            item2.PersonnelId = item1.PersonnelId;
            item2.PersonnelName = item1.PersonnelName;
            item2.MobileInspectionId = item1.MobileInspectionId;
            item2.MobileInspectionName = item1.MobileInspectionName;
            item2.PlanStartTime = item1.PlanStartTime;
            item2.PlanEndTime = item1.PlanEndTime;
            item2.StartTime = item1.StartTime;
            item2.bOverTime = item1.bOverTime;
            item2.Remark = item1.Remark;
            item2.list = item1.list.ToDbModel();
            return item2;
        }

        public static List<DbModel.Location.Work.PersonnelMobileInspection> ToDbModel(
            this List<TModel.Location.Work.PersonnelMobileInspection> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Work.PersonnelMobileInspection>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.LocationHistory.Work.OperationItemHistory <=> DbModel.LocationHistory.Work.OperationItemHistory

        public static List<TModel.LocationHistory.Work.OperationItemHistory> ToWcfModelList(
            this List<DbModel.LocationHistory.Work.OperationItemHistory> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.LocationHistory.Work.OperationItemHistory ToTModel(
            this DbModel.LocationHistory.Work.OperationItemHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.LocationHistory.Work.OperationItemHistory();
            item2.Id = item1.Id;
            item2.TicketId = item1.TicketId;
            item2.OperationTime = item1.OperationTime;
            item2.Mark = item1.Mark;
            item2.OrderNum = item1.OrderNum;
            item2.Item = item1.Item;
            return item2;
        }

        public static List<TModel.LocationHistory.Work.OperationItemHistory> ToTModel(
            this List<DbModel.LocationHistory.Work.OperationItemHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.LocationHistory.Work.OperationItemHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.LocationHistory.Work.OperationItemHistory ToDbModel(
            this TModel.LocationHistory.Work.OperationItemHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.LocationHistory.Work.OperationItemHistory();
            item2.Id = item1.Id;
            item2.TicketId = item1.TicketId;
            item2.OperationTime = item1.OperationTime;
            item2.Mark = item1.Mark;
            item2.OrderNum = item1.OrderNum;
            item2.Item = item1.Item;
            return item2;
        }

        public static List<DbModel.LocationHistory.Work.OperationItemHistory> ToDbModel(
            this List<TModel.LocationHistory.Work.OperationItemHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.LocationHistory.Work.OperationItemHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.LocationHistory.Work.OperationTicketHistory <=> DbModel.LocationHistory.Work.OperationTicketHistory

        public static List<TModel.LocationHistory.Work.OperationTicketHistory> ToWcfModelList(
            this List<DbModel.LocationHistory.Work.OperationTicketHistory> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.LocationHistory.Work.OperationTicketHistory ToTModel(
            this DbModel.LocationHistory.Work.OperationTicketHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.LocationHistory.Work.OperationTicketHistory();
            item2.Id = item1.Id;
            item2.No = item1.No;
            item2.OperationTask = item1.OperationTask;
            item2.OperationStartTime = item1.OperationStartTime;
            item2.OperationEndTime = item1.OperationEndTime;
            item2.OperationItems = item1.OperationItems.ToTModel();
            item2.Guardian = item1.Guardian;
            item2.Operator = item1.Operator;
            item2.DutyOfficer = item1.DutyOfficer;
            item2.Dispatch = item1.Dispatch;
            item2.Remark = item1.Remark;
            return item2;
        }

        public static List<TModel.LocationHistory.Work.OperationTicketHistory> ToTModel(
            this List<DbModel.LocationHistory.Work.OperationTicketHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.LocationHistory.Work.OperationTicketHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.LocationHistory.Work.OperationTicketHistory ToDbModel(
            this TModel.LocationHistory.Work.OperationTicketHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.LocationHistory.Work.OperationTicketHistory();
            item2.Id = item1.Id;
            item2.No = item1.No;
            item2.OperationTask = item1.OperationTask;
            item2.OperationStartTime = item1.OperationStartTime;
            item2.OperationEndTime = item1.OperationEndTime;
            item2.OperationItems = item1.OperationItems.ToDbModel();
            item2.Guardian = item1.Guardian;
            item2.Operator = item1.Operator;
            item2.DutyOfficer = item1.DutyOfficer;
            item2.Dispatch = item1.Dispatch;
            item2.Remark = item1.Remark;
            return item2;
        }

        public static List<DbModel.LocationHistory.Work.OperationTicketHistory> ToDbModel(
            this List<TModel.LocationHistory.Work.OperationTicketHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.LocationHistory.Work.OperationTicketHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory <=> DbModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory

        public static List<TModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory> ToWcfModelList(
            this List<DbModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory ToTModel(
            this DbModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory();
            item2.Id = item1.Id;
            item2.ItemId = item1.ItemId;
            item2.PID = item1.PID;
            item2.ItemName = item1.ItemName;
            item2.nOrder = item1.nOrder;
            item2.DevId = item1.DevId;
            item2.DevName = item1.DevName;
            item2.PunchTime = item1.PunchTime;
            return item2;
        }

        public static List<TModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory> ToTModel(
            this List<DbModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory ToDbModel(
            this TModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory();
            item2.Id = item1.Id;
            item2.ItemId = item1.ItemId;
            item2.PID = item1.PID;
            item2.ItemName = item1.ItemName;
            item2.nOrder = item1.nOrder;
            item2.DevId = item1.DevId;
            item2.DevName = item1.DevName;
            item2.PunchTime = item1.PunchTime;
            return item2;
        }

        public static List<DbModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory> ToDbModel(
            this List<TModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.LocationHistory.Work.PersonnelMobileInspectionHistory <=> DbModel.LocationHistory.Work.PersonnelMobileInspectionHistory

        public static List<TModel.LocationHistory.Work.PersonnelMobileInspectionHistory> ToWcfModelList(
            this List<DbModel.LocationHistory.Work.PersonnelMobileInspectionHistory> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.LocationHistory.Work.PersonnelMobileInspectionHistory ToTModel(
            this DbModel.LocationHistory.Work.PersonnelMobileInspectionHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.LocationHistory.Work.PersonnelMobileInspectionHistory();
            item2.Id = item1.Id;
            item2.PersonnelId = item1.PersonnelId;
            item2.PersonnelName = item1.PersonnelName;
            item2.MobileInspectionId = item1.MobileInspectionId;
            item2.MobileInspectionName = item1.MobileInspectionName;
            item2.PlanStartTime = item1.PlanStartTime;
            item2.PlanEndTime = item1.PlanEndTime;
            item2.StartTime = item1.StartTime;
            item2.EndTime = item1.EndTime;
            item2.bOverTime = item1.bOverTime;
            item2.Remark = item1.Remark;
            item2.list = item1.list.ToTModel();
            return item2;
        }

        public static List<TModel.LocationHistory.Work.PersonnelMobileInspectionHistory> ToTModel(
            this List<DbModel.LocationHistory.Work.PersonnelMobileInspectionHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.LocationHistory.Work.PersonnelMobileInspectionHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.LocationHistory.Work.PersonnelMobileInspectionHistory ToDbModel(
            this TModel.LocationHistory.Work.PersonnelMobileInspectionHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.LocationHistory.Work.PersonnelMobileInspectionHistory();
            item2.Id = item1.Id;
            item2.PersonnelId = item1.PersonnelId;
            item2.PersonnelName = item1.PersonnelName;
            item2.MobileInspectionId = item1.MobileInspectionId;
            item2.MobileInspectionName = item1.MobileInspectionName;
            item2.PlanStartTime = item1.PlanStartTime;
            item2.PlanEndTime = item1.PlanEndTime;
            item2.StartTime = item1.StartTime;
            item2.EndTime = item1.EndTime;
            item2.bOverTime = item1.bOverTime;
            item2.Remark = item1.Remark;
            item2.list = item1.list.ToDbModel();
            return item2;
        }

        public static List<DbModel.LocationHistory.Work.PersonnelMobileInspectionHistory> ToDbModel(
            this List<TModel.LocationHistory.Work.PersonnelMobileInspectionHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.LocationHistory.Work.PersonnelMobileInspectionHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.LocationHistory.Work.SafetyMeasuresHistory <=> DbModel.LocationHistory.Work.SafetyMeasuresHistory

        public static List<TModel.LocationHistory.Work.SafetyMeasuresHistory> ToWcfModelList(
            this List<DbModel.LocationHistory.Work.SafetyMeasuresHistory> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.LocationHistory.Work.SafetyMeasuresHistory ToTModel(
            this DbModel.LocationHistory.Work.SafetyMeasuresHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.LocationHistory.Work.SafetyMeasuresHistory();
            item2.Id = item1.Id;
            item2.No = item1.No;
            item2.LssuerContent = item1.LssuerContent;
            item2.LicensorContent = item1.LicensorContent;
            item2.WorkTicketId = item1.WorkTicketId;
            return item2;
        }

        public static List<TModel.LocationHistory.Work.SafetyMeasuresHistory> ToTModel(
            this List<DbModel.LocationHistory.Work.SafetyMeasuresHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.LocationHistory.Work.SafetyMeasuresHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.LocationHistory.Work.SafetyMeasuresHistory ToDbModel(
            this TModel.LocationHistory.Work.SafetyMeasuresHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.LocationHistory.Work.SafetyMeasuresHistory();
            item2.Id = item1.Id;
            item2.No = item1.No;
            item2.LssuerContent = item1.LssuerContent;
            item2.LicensorContent = item1.LicensorContent;
            item2.WorkTicketId = item1.WorkTicketId;
            return item2;
        }

        public static List<DbModel.LocationHistory.Work.SafetyMeasuresHistory> ToDbModel(
            this List<TModel.LocationHistory.Work.SafetyMeasuresHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.LocationHistory.Work.SafetyMeasuresHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.LocationHistory.Work.WorkTicketHistory <=> DbModel.LocationHistory.Work.WorkTicketHistory

        public static List<TModel.LocationHistory.Work.WorkTicketHistory> ToWcfModelList(
            this List<DbModel.LocationHistory.Work.WorkTicketHistory> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TModel.LocationHistory.Work.WorkTicketHistory ToTModel(
            this DbModel.LocationHistory.Work.WorkTicketHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.LocationHistory.Work.WorkTicketHistory();
            item2.Id = item1.Id;
            item2.No = item1.No;
            item2.PersonInCharge = item1.PersonInCharge;
            item2.HeadOfWorkClass = item1.HeadOfWorkClass;
            item2.WorkPlace = item1.WorkPlace;
            item2.JobContent = item1.JobContent;
            item2.SafetyMeasuress = item1.SafetyMeasuress.ToTModel();
            item2.StartTimeOfPlannedWork = item1.StartTimeOfPlannedWork;
            item2.EndTimeOfPlannedWork = item1.EndTimeOfPlannedWork;
            item2.WorkCondition = item1.WorkCondition;
            item2.Lssuer = item1.Lssuer;
            item2.Licensor = item1.Licensor;
            item2.Approver = item1.Approver;
            item2.Comment = item1.Comment;
            return item2;
        }

        public static List<TModel.LocationHistory.Work.WorkTicketHistory> ToTModel(
            this List<DbModel.LocationHistory.Work.WorkTicketHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.LocationHistory.Work.WorkTicketHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.LocationHistory.Work.WorkTicketHistory ToDbModel(
            this TModel.LocationHistory.Work.WorkTicketHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.LocationHistory.Work.WorkTicketHistory();
            item2.Id = item1.Id;
            item2.No = item1.No;
            item2.PersonInCharge = item1.PersonInCharge;
            item2.HeadOfWorkClass = item1.HeadOfWorkClass;
            item2.WorkPlace = item1.WorkPlace;
            item2.JobContent = item1.JobContent;
            item2.SafetyMeasuress = item1.SafetyMeasuress.ToDbModel();
            item2.StartTimeOfPlannedWork = item1.StartTimeOfPlannedWork;
            item2.EndTimeOfPlannedWork = item1.EndTimeOfPlannedWork;
            item2.WorkCondition = item1.WorkCondition;
            item2.Lssuer = item1.Lssuer;
            item2.Licensor = item1.Licensor;
            item2.Approver = item1.Approver;
            item2.Comment = item1.Comment;
            return item2;
        }

        public static List<DbModel.LocationHistory.Work.WorkTicketHistory> ToDbModel(
            this List<TModel.LocationHistory.Work.WorkTicketHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.LocationHistory.Work.WorkTicketHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion

        #region TModel.Location.AreaAndDev.Picture <=> DbModel.Location.AreaAndDev.Picture
        public static List<TModel.Location.AreaAndDev.Picture> ToWcfModelList(this List<DbModel.Location.AreaAndDev.Picture> list1)
        {
            return list1.ToTModel().ToWCFList();
        }
        public static TModel.Location.AreaAndDev.Picture ToTModel(this DbModel.Location.AreaAndDev.Picture item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.AreaAndDev.Picture();
            item2.Name = item1.Name;
            item2.Info = item1.Info;
            return item2;
        }
        public static List<TModel.Location.AreaAndDev.Picture> ToTModel(this List<DbModel.Location.AreaAndDev.Picture> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.AreaAndDev.Picture>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }
        public static DbModel.Location.AreaAndDev.Picture ToDbModel(this TModel.Location.AreaAndDev.Picture item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.AreaAndDev.Picture();
            item2.Name = item1.Name;
            item2.Info = item1.Info;
            return item2;
        }
        public static List<DbModel.Location.AreaAndDev.Picture> ToDbModel(this List<TModel.Location.AreaAndDev.Picture> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.AreaAndDev.Picture>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }
        #endregion

        #region TModel.Location.AreaAndDev.DevMonitorNode <=> DbModel.Location.AreaAndDev.DevMonitorNode
        public static List<TModel.Location.AreaAndDev.DevMonitorNode> ToWcfModelList(this List<DbModel.Location.AreaAndDev.DevMonitorNode> list1)
        {
            return list1.ToTModel().ToWCFList();
        }
        public static TModel.Location.AreaAndDev.DevMonitorNode ToTModel(this DbModel.Location.AreaAndDev.DevMonitorNode item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.AreaAndDev.DevMonitorNode();
            item2.Id = item1.Id;
            item2.TagName = item1.TagName;
            item2.DbTagName = item1.DbTagName;
            item2.Describe = item1.Describe;
            item2.Value = item1.Value;
            item2.Unit = item1.Unit;
            item2.DataType = item1.DataType;
            item2.KKS = item1.KKS;
            item2.ParentKKS = item1.ParentKKS;
            item2.Time = item1.Time;
            return item2;
        }
        public static List<TModel.Location.AreaAndDev.DevMonitorNode> ToTModel(this List<DbModel.Location.AreaAndDev.DevMonitorNode> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.AreaAndDev.DevMonitorNode>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }
        public static DbModel.Location.AreaAndDev.DevMonitorNode ToDbModel(this TModel.Location.AreaAndDev.DevMonitorNode item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.AreaAndDev.DevMonitorNode();
            item2.Id = item1.Id;
            item2.TagName = item1.TagName;
            item2.DbTagName = item1.DbTagName;
            item2.Describe = item1.Describe;
            item2.Value = item1.Value;
            item2.Unit = item1.Unit;
            item2.DataType = item1.DataType;
            item2.KKS = item1.KKS;
            item2.ParentKKS = item1.ParentKKS;
            item2.Time = item1.Time;
            return item2;
        }
        public static List<DbModel.Location.AreaAndDev.DevMonitorNode> ToDbModel(this List<TModel.Location.AreaAndDev.DevMonitorNode> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.AreaAndDev.DevMonitorNode>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }
        #endregion


    }
}
