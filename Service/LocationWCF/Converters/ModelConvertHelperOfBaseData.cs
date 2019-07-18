using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModel.Tools;

namespace LocationServices.Converters
{
    public static class ModelConvertHelperOfBaseData
    {
        #region TModel.BaseData.Ticket <=> CommunicationClass.SihuiThermalPowerPlant.Models.tickets
        public static List<TModel.BaseData.Ticket> ToWcfModelList(this List<CommunicationClass.SihuiThermalPowerPlant.Models.tickets> list1)
        {
            return list1.ToTModel().ToWCFList();
        }
        public static TModel.BaseData.Ticket ToTModel(this CommunicationClass.SihuiThermalPowerPlant.Models.tickets item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.BaseData.Ticket();
            item2.Id = item1.id;
            item2.Code = item1.code;
            item2.Type = item1.type;
            item2.State = item1.state;
            item2.WorkerIds = item1.worker_ids;
            item2.ZoneIds = item1.zone_ids;
            item2.Detail = item1.detail;
            return item2;
        }
        public static List<TModel.BaseData.Ticket> ToTModel(this List<CommunicationClass.SihuiThermalPowerPlant.Models.tickets> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.BaseData.Ticket>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }
        public static CommunicationClass.SihuiThermalPowerPlant.Models.tickets ToDbModel(this TModel.BaseData.Ticket item1)
        {
            if (item1 == null) return null;
            var item2 = new CommunicationClass.SihuiThermalPowerPlant.Models.tickets();
            item2.id = item1.Id;
            item2.code = item1.Code;
            item2.type = item1.Type;
            item2.state = item1.State;
            item2.worker_ids = item1.WorkerIds;
            item2.zone_ids = item1.ZoneIds;
            item2.detail = item1.Detail;
            return item2;
        }
        public static List<CommunicationClass.SihuiThermalPowerPlant.Models.tickets> ToDbModel(this List<TModel.BaseData.Ticket> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<CommunicationClass.SihuiThermalPowerPlant.Models.tickets>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }
        #endregion

        #region TModel.Location.Work.InspectionTrack <=> DbModel.Location.Work.InspectionTrack
        public static List<TModel.Location.Work.InspectionTrack> ToWcfModelList(this List<DbModel.Location.Work.InspectionTrack> list1)
        {
            return list1.ToTModel().ToWCFList();
        }
        public static TModel.Location.Work.InspectionTrack ToTModel(this DbModel.Location.Work.InspectionTrack item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.Work.InspectionTrack();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.Code = item1.Code;
            item2.Name = item1.Name;
            item2.dtCreateTime = item1.dtCreateTime;
            item2.CreateTime = item1.CreateTime;
            item2.State = item1.State;
            item2.dtStartTime = item1.dtStartTime;
            item2.StartTime = item1.StartTime;
            item2.dtEndTime = item1.dtEndTime;
            item2.EndTime = item1.EndTime;
            item2.Route = item1.Route.ToTModel();
            return item2;
        }
        public static List<TModel.Location.Work.InspectionTrack> ToTModel(this List<DbModel.Location.Work.InspectionTrack> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.Work.InspectionTrack>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }
        public static DbModel.Location.Work.InspectionTrack ToDbModel(this TModel.Location.Work.InspectionTrack item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Work.InspectionTrack();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.Code = item1.Code;
            item2.Name = item1.Name;
            item2.dtCreateTime = item1.dtCreateTime;
            item2.CreateTime = item1.CreateTime;
            item2.State = item1.State;
            item2.dtStartTime = item1.dtStartTime;
            item2.StartTime = item1.StartTime;
            item2.dtEndTime = item1.dtEndTime;
            item2.EndTime = item1.EndTime;
            item2.Route = item1.Route.ToDbModel();
            return item2;
        }
        public static List<DbModel.Location.Work.InspectionTrack> ToDbModel(this List<TModel.Location.Work.InspectionTrack> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Work.InspectionTrack>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }
        #endregion

        #region TModel.Location.Work.PatrolPoint <=> DbModel.Location.Work.PatrolPoint
        public static List<TModel.Location.Work.PatrolPoint> ToWcfModelList(this List<DbModel.Location.Work.PatrolPoint> list1)
        {
            return list1.ToTModel().ToWCFList();
        }
        public static TModel.Location.Work.PatrolPoint ToTModel(this DbModel.Location.Work.PatrolPoint item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.Work.PatrolPoint();
            item2.Id = item1.Id;
            item2.ParentId = item1.ParentId;
            item2.StaffCode = item1.StaffCode;
            item2.KksCode = item1.KksCode;
            item2.DevId = item1.DevId;
            item2.DeviceCode = item1.DeviceCode;
            item2.DeviceId = item1.DeviceId;
            item2.Checks = item1.Checks.ToTModel();
            return item2;
        }
        public static List<TModel.Location.Work.PatrolPoint> ToTModel(this List<DbModel.Location.Work.PatrolPoint> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.Work.PatrolPoint>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }
        public static DbModel.Location.Work.PatrolPoint ToDbModel(this TModel.Location.Work.PatrolPoint item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Work.PatrolPoint();
            item2.Id = item1.Id;
            item2.ParentId = item1.ParentId;
            item2.StaffCode = item1.StaffCode;
            item2.KksCode = item1.KksCode;
            item2.DevId = item2.DevId;
            item2.DeviceCode = item1.DeviceCode;
            item2.DeviceId = item1.DeviceId;
            item2.Checks = item1.Checks.ToDbModel();
            return item2;
        }
        public static List<DbModel.Location.Work.PatrolPoint> ToDbModel(this List<TModel.Location.Work.PatrolPoint> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Work.PatrolPoint>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }
        #endregion

        #region TModel.Location.Work.PatrolPointItem <=> DbModel.Location.Work.PatrolPointItem
        public static List<TModel.Location.Work.PatrolPointItem> ToWcfModelList(this List<DbModel.Location.Work.PatrolPointItem> list1)
        {
            return list1.ToTModel().ToWCFList();
        }
        public static TModel.Location.Work.PatrolPointItem ToTModel(this DbModel.Location.Work.PatrolPointItem item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.Work.PatrolPointItem();
            item2.Id = item1.Id;
            item2.ParentId = item1.ParentId;
            item2.KksCode = item1.KksCode;
            item2.CheckItem = item1.CheckItem;
            item2.StaffCode = item1.StaffCode;
            item2.dtCheckTime = item1.dtCheckTime;
            item2.CheckTime = item1.CheckTime;
            item2.CheckId = item1.CheckId;
            item2.CheckResult = item1.CheckResult;
            return item2;
        }
        public static List<TModel.Location.Work.PatrolPointItem> ToTModel(this List<DbModel.Location.Work.PatrolPointItem> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.Work.PatrolPointItem>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }
        public static DbModel.Location.Work.PatrolPointItem ToDbModel(this TModel.Location.Work.PatrolPointItem item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Work.PatrolPointItem();
            item2.Id = item1.Id;
            item2.ParentId = item1.ParentId;
            item2.KksCode = item1.KksCode;
            item2.CheckItem = item1.CheckItem;
            item2.StaffCode = item1.StaffCode;
            item2.dtCheckTime = item1.dtCheckTime;
            item2.CheckTime = item1.CheckTime;
            item2.CheckId = item1.CheckId;
            item2.CheckResult = item1.CheckResult;
            return item2;
        }
        public static List<DbModel.Location.Work.PatrolPointItem> ToDbModel(this List<TModel.Location.Work.PatrolPointItem> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Work.PatrolPointItem>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }
        #endregion

        #region TModel.LocationHistory.Work.InspectionTrackHistory <=> DbModel.LocationHistory.Work.InspectionTrackHistory
        public static List<TModel.LocationHistory.Work.InspectionTrackHistory> ToWcfModelList(this List<DbModel.LocationHistory.Work.InspectionTrackHistory> list1)
        {
            return list1.ToTModel().ToWCFList();
        }
        public static TModel.LocationHistory.Work.InspectionTrackHistory ToTModel(this DbModel.LocationHistory.Work.InspectionTrackHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.LocationHistory.Work.InspectionTrackHistory();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.Code = item1.Code;
            item2.Name = item1.Name;
            item2.dtCreateTime = item1.dtCreateTime;
            item2.CreateTime = item1.CreateTime;
            item2.State = item1.State;
            item2.dtStartTime = item1.dtStartTime;
            item2.StartTime = item1.StartTime;
            item2.dtEndTime = item1.dtEndTime;
            item2.EndTime = item1.EndTime;
            item2.Route = item1.Route.ToTModel();
            return item2;
        }
        public static List<TModel.LocationHistory.Work.InspectionTrackHistory> ToTModel(this List<DbModel.LocationHistory.Work.InspectionTrackHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.LocationHistory.Work.InspectionTrackHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }
        public static DbModel.LocationHistory.Work.InspectionTrackHistory ToDbModel(this TModel.LocationHistory.Work.InspectionTrackHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.LocationHistory.Work.InspectionTrackHistory();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.Code = item1.Code;
            item2.Name = item1.Name;
            item2.dtCreateTime = item1.dtCreateTime;
            item2.CreateTime = item1.CreateTime;
            item2.State = item1.State;
            item2.dtStartTime = item1.dtStartTime;
            item2.StartTime = item1.StartTime;
            item2.dtEndTime = item1.dtEndTime;
            item2.EndTime = item1.EndTime;
            item2.Route = item1.Route.ToDbModel();
            return item2;
        }
        public static List<DbModel.LocationHistory.Work.InspectionTrackHistory> ToDbModel(this List<TModel.LocationHistory.Work.InspectionTrackHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.LocationHistory.Work.InspectionTrackHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }
        #endregion

        #region TModel.LocationHistory.Work.PatrolPointHistory <=> DbModel.LocationHistory.Work.PatrolPointHistory
        public static List<TModel.LocationHistory.Work.PatrolPointHistory> ToWcfModelList(this List<DbModel.LocationHistory.Work.PatrolPointHistory> list1)
        {
            return list1.ToTModel().ToWCFList();
        }
        public static TModel.LocationHistory.Work.PatrolPointHistory ToTModel(this DbModel.LocationHistory.Work.PatrolPointHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.LocationHistory.Work.PatrolPointHistory();
            item2.Id = item1.Id;
            item2.ParentId = item1.ParentId;
            item2.StaffCode = item1.StaffCode;
            item2.KksCode = item1.KksCode;
            item2.DevId = item2.DevId;
            item2.DeviceCode = item1.DeviceCode;
            item2.DeviceId = item1.DeviceId;
            item2.Checks = item1.Checks.ToTModel();
            return item2;
        }
        public static List<TModel.LocationHistory.Work.PatrolPointHistory> ToTModel(this List<DbModel.LocationHistory.Work.PatrolPointHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.LocationHistory.Work.PatrolPointHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }
        public static DbModel.LocationHistory.Work.PatrolPointHistory ToDbModel(this TModel.LocationHistory.Work.PatrolPointHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.LocationHistory.Work.PatrolPointHistory();
            item2.Id = item1.Id;
            item2.ParentId = item1.ParentId;
            item2.StaffCode = item1.StaffCode;
            item2.KksCode = item1.KksCode;
            item2.DevId = item1.DevId;
            item2.DeviceCode = item1.DeviceCode;
            item2.DeviceId = item1.DeviceId;
            item2.Checks = item1.Checks.ToDbModel();
            return item2;
        }
        public static List<DbModel.LocationHistory.Work.PatrolPointHistory> ToDbModel(this List<TModel.LocationHistory.Work.PatrolPointHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.LocationHistory.Work.PatrolPointHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }
        #endregion

        #region TModel.LocationHistory.Work.PatrolPointItemHistory <=> DbModel.LocationHistory.Work.PatrolPointItemHistory
        public static List<TModel.LocationHistory.Work.PatrolPointItemHistory> ToWcfModelList(this List<DbModel.LocationHistory.Work.PatrolPointItemHistory> list1)
        {
            return list1.ToTModel().ToWCFList();
        }
        public static TModel.LocationHistory.Work.PatrolPointItemHistory ToTModel(this DbModel.LocationHistory.Work.PatrolPointItemHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.LocationHistory.Work.PatrolPointItemHistory();
            item2.Id = item1.Id;
            item2.ParentId = item1.ParentId;
            item2.KksCode = item1.KksCode;
            item2.CheckItem = item1.CheckItem;
            item2.StaffCode = item1.StaffCode;
            item2.dtCheckTime = item1.dtCheckTime;
            item2.CheckTime = item1.CheckTime;
            item2.CheckId = item1.CheckId;
            item2.CheckResult = item1.CheckResult;
            return item2;
        }
        public static List<TModel.LocationHistory.Work.PatrolPointItemHistory> ToTModel(this List<DbModel.LocationHistory.Work.PatrolPointItemHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.LocationHistory.Work.PatrolPointItemHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }
        public static DbModel.LocationHistory.Work.PatrolPointItemHistory ToDbModel(this TModel.LocationHistory.Work.PatrolPointItemHistory item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.LocationHistory.Work.PatrolPointItemHistory();
            item2.Id = item1.Id;
            item2.ParentId = item1.ParentId;
            item2.KksCode = item1.KksCode;
            item2.CheckItem = item1.CheckItem;
            item2.StaffCode = item1.StaffCode;
            item2.dtCheckTime = item1.dtCheckTime;
            item2.CheckTime = item1.CheckTime;
            item2.CheckId = item1.CheckId;
            item2.CheckResult = item1.CheckResult;
            return item2;
        }
        public static List<DbModel.LocationHistory.Work.PatrolPointItemHistory> ToDbModel(this List<TModel.LocationHistory.Work.PatrolPointItemHistory> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.LocationHistory.Work.PatrolPointItemHistory>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }
        #endregion

        #region TModel.Location.Work.InspectionTrackList <=> DbModel.Location.Work.InspectionTrackList
       
        public static TModel.Location.Work.InspectionTrackList ToTModel(this DbModel.Location.Work.InspectionTrackList item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.Work.InspectionTrackList();
            item2.AddTrack = item1.AddTrack.ToTModel();
            item2.ReviseTrack = item1.ReviseTrack.ToTModel();
            item2.DeleteTrack = item1.DeleteTrack.ToTModel();
            return item2;
        }
    
        public static DbModel.Location.Work.InspectionTrackList ToDbModel(this TModel.Location.Work.InspectionTrackList item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Work.InspectionTrackList();
            item2.AddTrack = item1.AddTrack.ToDbModel();
            item2.ReviseTrack = item1.ReviseTrack.ToDbModel();
            item2.DeleteTrack = item1.DeleteTrack.ToDbModel();
            return item2;
        }
       
        #endregion
    }
}
