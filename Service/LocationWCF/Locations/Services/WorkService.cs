using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.Work;
using TModel.LocationHistory.Work;
using BLL;
using LocationServices.Converters;
using Location.BLL.Tool;

namespace LocationServices.Locations.Services
{
    public interface IWorkService
    {
        List<OperationTicket> GetOperationTicketList();

        List<WorkTicket> GetWorkTicketList();

        List<MobileInspectionDev> GetMobileInspectionDevList();

        List<MobileInspection> GetMobileInspectionList();

        List<PersonnelMobileInspection> GetPersonnelMobileInspectionList();

        List<OperationTicketHistory> GetOperationTicketHistoryList();

        List<WorkTicketHistory> GetWorkTicketHistoryList();

        List<PersonnelMobileInspectionHistory> GetPersonnelMobileInspectionHistoryList();

        InspectionTrack GetInspectionTrack();
    }

    public class WorkService : IWorkService
    {
        private Bll db;

        public WorkService()
        {
            db= Bll.NewBllNoRelation();
        }
        public InspectionTrack GetInspectionTrack()
        {
            return new InspectionTrack();
        }

        public List<MobileInspectionDev> GetMobileInspectionDevList()
        {
            var MobileInspectionDev = db.MobileInspectionDevs.ToList();
            return MobileInspectionDev.ToWcfModelList();
        }

        public List<MobileInspection> GetMobileInspectionList()
        {
            var MobileInspection = db.MobileInspections.ToList();
            return MobileInspection.ToWcfModelList();
        }

        public List<OperationTicketHistory> GetOperationTicketHistoryList()
        {
            try
            {
                var OperationTicketHistory = db.OperationTicketHistorys.ToList();
                //return OperationTicketHistory.ToWcfModelList();
                OperationTicketHistory o1 = new OperationTicketHistory() { Id = 1, No = "100001", Guardian = "李新风G" };
                o1.OperationTask = "操作任务";
                o1.OperationStartTime = DateTime.Now.AddHours(-2);
                o1.OperationEndTime = DateTime.Now;
                o1.Operator = "刘华名";
                o1.DutyOfficer = "车马风";
                o1.Dispatch = "调度";
                o1.Remark = "备注";
                //o1.OperatorPersonelId = 7;
                OperationItemHistory oi1 = new OperationItemHistory() { Id = 1, OrderNum = 1, Item = "操作项1" };
                OperationItemHistory oi2 = new OperationItemHistory() { Id = 2, OrderNum = 2, Item = "操作项2" };
                OperationItemHistory oi3 = new OperationItemHistory() { Id = 3, OrderNum = 3, Item = "操作项3" };
                OperationItemHistory oi4 = new OperationItemHistory() { Id = 4, OrderNum = 4, Item = "操作项4" };
                o1.OperationItems = new List<OperationItemHistory>() { oi1, oi2, oi3, oi4 };

                OperationTicketHistory o2 = new OperationTicketHistory() { Id = 2, No = "100002", Guardian = "赵一含G" };
                o2.OperationStartTime = DateTime.Now.AddHours(-2);
                o2.OperationEndTime = DateTime.Now;
                o2.Operator = "刘华名22";
                o2.DutyOfficer = "车马风22";
                OperationTicketHistory o3 = new OperationTicketHistory() { Id = 3, No = "100003", Guardian = "刘国柱G" };
                o3.OperationStartTime = DateTime.Now.AddHours(-2);
                o3.OperationEndTime = DateTime.Now;
                o3.Operator = "刘华名33";
                o3.DutyOfficer = "车马风33";
                OperationTicketHistory o4 = new OperationTicketHistory() { Id = 4, No = "100004", Guardian = "陈浩然G" };
                o4.OperationStartTime = DateTime.Now.AddHours(-2);
                o4.OperationEndTime = DateTime.Now;
                o4.Operator = "刘华名44";
                o4.DutyOfficer = "车马风44";
                OperationTicketHistory o5 = new OperationTicketHistory() { Id = 5, No = "100005", Guardian = "李一样G" };
                o5.OperationStartTime = DateTime.Now.AddHours(-2);
                o5.OperationEndTime = DateTime.Now;
                o5.Operator = "刘华名55";
                o5.DutyOfficer = "车马风55";
                List<OperationTicketHistory> os = new List<OperationTicketHistory>() { o1, o2, o3, o4, o5 };
                for (int i = 0; i < 20; i++)
                {
                    OperationTicketHistory wT = new OperationTicketHistory() { Id = 6 + i, No = "000005" + i, Guardian = "赵小刚"+i };
                    wT.OperationStartTime = DateTime.Now.AddHours(-2);
                    wT.OperationEndTime = DateTime.Now;
                    wT.Operator = "刘华名"+i;
                    wT.DutyOfficer = "车马风"+i;
                    os.Add(wT);
                }
                return os;
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.DbGet, "GetOperationTicketHistoryList", "Exception:" + ex);
                return null;
            }
        }

        public List<OperationTicket> GetOperationTicketList()
        {
            try
            {
                var operationTickets = db.OperationTickets.ToList();
                //return operationTickets.ToWcfModelList();
                OperationTicket o1 = new OperationTicket() { Id = 1, No = "100001", Guardian = "李新风G" };
                o1.OperationTask = "操作任务";
                o1.OperationStartTime = DateTime.Now.AddHours(-2);
                o1.OperationEndTime = DateTime.Now;
                o1.Operator = "刘华名";
                o1.DutyOfficer = "车马风";
                o1.Dispatch = "调度";
                o1.Remark = "备注";
                o1.OperatorPersonelId = 7;
                OperationItem oi1 = new OperationItem() { Id = 1, OrderNum = 1, Item = "操作项1", DevId = "103cef6e-3155-4be8-a166-41ac567b7b01" };
                OperationItem oi2 = new OperationItem() { Id = 2, OrderNum = 2, Item = "操作项2", DevId = "e65ac4c1-0936-409f-9f09-894f5ee20fb8" };
                //OperationItem oi3 = new OperationItem() { Id = 3, OrderNum = 3, Item = "操作项3", DevId = "7ac0698a-8360-483e-b827-ba6e512ccdb2" };
                //OperationItem oi4 = new OperationItem() { Id = 4, OrderNum = 4, Item = "操作项4", DevId = "4c764e74-c03d-4967-9b6d-46c151a4fa23" };
                OperationItem oi3 = new OperationItem() { Id = 3, OrderNum = 3, Item = "操作项3", DevId = "56ae2f57-0068-4539-b496-f5198a216ef2" };
                OperationItem oi4 = new OperationItem() { Id = 4, OrderNum = 4, Item = "操作项4", DevId = "f2abcb56-8382-459e-b89f-2e1afd4043f4" };
                o1.OperationItems = new List<OperationItem>() { oi1, oi2, oi3, oi4 };

                OperationTicket o2 = new OperationTicket() { Id = 2, No = "100002", Guardian = "赵一含G" };
                OperationTicket o3 = new OperationTicket() { Id = 3, No = "100003", Guardian = "刘国柱G" };
                OperationTicket o4 = new OperationTicket() { Id = 4, No = "100004", Guardian = "陈浩然G" };
                OperationTicket o5 = new OperationTicket() { Id = 5, No = "100005", Guardian = "李一样G" };
                List<OperationTicket> os = new List<OperationTicket>() { o1, o2, o3, o4, o5 };
                return os;
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.DbGet, "GetOperationTicketList", "Exception:" + ex);
                return null;
            }
        }

        public List<PersonnelMobileInspectionHistory> GetPersonnelMobileInspectionHistoryList()
        {
            try
            {
                var PersonnelMobileInspectionHistory = db.PersonnelMobileInspectionHistorys.ToList();
                //return PersonnelMobileInspectionHistory.ToWcfModelList();
                PersonnelMobileInspectionHistory o1 = new PersonnelMobileInspectionHistory() { Id = 1, MobileInspectionId = 100001, MobileInspectionName = "巡检轨迹1", PersonnelName = "李风A" };
                o1.PlanStartTime = DateTime.Now.AddHours(-2);
                o1.PlanEndTime = DateTime.Now;
                o1.StartTime = DateTime.Now.AddHours(-2);
                o1.Remark = "备注";
                //o1.OperatorPersonelId = 7;
                PersonnelMobileInspectionItemHistory oi1 = new PersonnelMobileInspectionItemHistory() { Id = 1, nOrder = 1, ItemName = "项1", DevName = "设备1", DevId = 1 };
                PersonnelMobileInspectionItemHistory oi2 = new PersonnelMobileInspectionItemHistory() { Id = 2, nOrder = 2, ItemName = "项2", DevName = "设备2", DevId = 2 };
                PersonnelMobileInspectionItemHistory oi3 = new PersonnelMobileInspectionItemHistory() { Id = 3, nOrder = 3, ItemName = "项3", DevName = "设备3", DevId = 3 };
                PersonnelMobileInspectionItemHistory oi4 = new PersonnelMobileInspectionItemHistory() { Id = 4, nOrder = 4, ItemName = "项4", DevName = "设备4", DevId = 4 };
                PersonnelMobileInspectionItemHistory oi5 = new PersonnelMobileInspectionItemHistory() { Id = 5, nOrder = 5, ItemName = "项5", DevName = "设备5", DevId = 5 };
                o1.list = new List<PersonnelMobileInspectionItemHistory>() { oi1, oi2, oi3, oi4, oi5 };

                PersonnelMobileInspectionHistory o2 = new PersonnelMobileInspectionHistory() { Id = 2, MobileInspectionId = 100002, MobileInspectionName = "巡检轨迹2", PersonnelName = "赵一含" };
                PersonnelMobileInspectionHistory o3 = new PersonnelMobileInspectionHistory() { Id = 3, MobileInspectionId = 100003, MobileInspectionName = "巡检轨迹3", PersonnelName = "刘国柱" };
                PersonnelMobileInspectionHistory o4 = new PersonnelMobileInspectionHistory() { Id = 4, MobileInspectionId = 100004, MobileInspectionName = "巡检轨迹4", PersonnelName = "陈浩然" };
                PersonnelMobileInspectionHistory o5 = new PersonnelMobileInspectionHistory() { Id = 5, MobileInspectionId = 100005, MobileInspectionName = "巡检轨迹5", PersonnelName = "李一样" };
                List<PersonnelMobileInspectionHistory> os = new List<PersonnelMobileInspectionHistory>() { o1, o2, o3, o4, o5 };
                for (int i = 0; i < 20; i++)
                {
                    PersonnelMobileInspectionHistory wT = new PersonnelMobileInspectionHistory() { Id = 6 + i, MobileInspectionId = 100005 + i, MobileInspectionName = "巡检轨迹" + (5 + i).ToString() };
                    os.Add(wT);
                }
                return os;
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.DbGet, "GetPersonnelMobileInspectionHistoryList", "Exception:" + ex);
                return null;
            }
        }

        public List<PersonnelMobileInspection> GetPersonnelMobileInspectionList()
        {
            try
            {
                var PersonnelMobileInspection = db.PersonnelMobileInspections.ToList();
                //return PersonnelMobileInspection.ToWcfModelList();

                PersonnelMobileInspection o1 = new PersonnelMobileInspection() { Id = 1, MobileInspectionId = 100001, MobileInspectionName = "巡检轨迹1", PersonnelName = "李风A" };
                //o1.OperationTask = "操作任务";
                o1.PlanStartTime = DateTime.Now.AddHours(-2);
                o1.PlanEndTime = DateTime.Now;
                o1.StartTime = DateTime.Now.AddHours(-2);
                o1.Remark = "备注";
                PersonnelMobileInspectionItem oi1 = new PersonnelMobileInspectionItem() { Id = 1, nOrder = 1, ItemName = "操作项1", DevId = 1 };
                PersonnelMobileInspectionItem oi2 = new PersonnelMobileInspectionItem() { Id = 2, nOrder = 2, ItemName = "操作项2", DevId = 2 };
                PersonnelMobileInspectionItem oi3 = new PersonnelMobileInspectionItem() { Id = 3, nOrder = 3, ItemName = "操作项3", DevId = 3 };
                PersonnelMobileInspectionItem oi4 = new PersonnelMobileInspectionItem() { Id = 4, nOrder = 4, ItemName = "操作项4", DevId = 4 };
                PersonnelMobileInspectionItem oi5 = new PersonnelMobileInspectionItem() { Id = 5, nOrder = 5, ItemName = "操作项5", DevId = 5 };
                o1.list = new List<PersonnelMobileInspectionItem>() { oi1, oi2, oi3, oi4, oi5 };

                PersonnelMobileInspection o2 = new PersonnelMobileInspection() { Id = 2, MobileInspectionId = 100002, MobileInspectionName = "巡检轨迹2", PersonnelName = "赵一含" };
                PersonnelMobileInspection o3 = new PersonnelMobileInspection() { Id = 3, MobileInspectionId = 100003, MobileInspectionName = "巡检轨迹3", PersonnelName = "刘国柱" };
                PersonnelMobileInspection o4 = new PersonnelMobileInspection() { Id = 4, MobileInspectionId = 100004, MobileInspectionName = "巡检轨迹4", PersonnelName = "陈浩然" };
                PersonnelMobileInspection o5 = new PersonnelMobileInspection() { Id = 5, MobileInspectionId = 100005, MobileInspectionName = "巡检轨迹5", PersonnelName = "李一样" };

                List<PersonnelMobileInspection> os = new List<PersonnelMobileInspection>() { o1, o2, o3, o4, o5 };
                return os;
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.DbGet, "GetPersonnelMobileInspectionList", "Exception:" + ex);
                return null;
            }
        }

        public List<WorkTicketHistory> GetWorkTicketHistoryList()
        {
            try
            {
                var WorkTicketHistory = db.WorkTicketHistorys.ToList();
                //return WorkTicketHistory.ToWcfModelList();
                WorkTicketHistory w1 = new WorkTicketHistory() { Id = 1, No = "000001", PersonInCharge = "李新风" };
                w1.HeadOfWorkClass = "李新";
                w1.WorkPlace = "主厂房";
                w1.JobContent = "干活";
                w1.StartTimeOfPlannedWork = DateTime.Now.AddHours(-2);
                w1.EndTimeOfPlannedWork = DateTime.Now;
                w1.WorkCondition = "工作条件，在什么...";
                w1.Lssuer = "叶路";
                w1.Licensor = "方城";
                w1.Approver = "吴发展";
                w1.Comment = "已经可以正常使用";
                //w1.PersonInChargePersonelId = 7;

                SafetyMeasuresHistory s1 = new SafetyMeasuresHistory() { Id = 1, No = 1, LssuerContent = "小心触电", LicensorContent = "小心漏电" };
                SafetyMeasuresHistory s2 = new SafetyMeasuresHistory() { Id = 2, No = 2, LssuerContent = "禁止明火", LicensorContent = "易燃易爆" };
                SafetyMeasuresHistory s3 = new SafetyMeasuresHistory() { Id = 3, No = 3, LssuerContent = "轻拿轻放", LicensorContent = "设备已损坏" };
                w1.SafetyMeasuress = new List<SafetyMeasuresHistory>() { s1, s2, s3 };

                WorkTicketHistory w2 = new WorkTicketHistory() { Id = 2, No = "000002", PersonInCharge = "赵一含" };
                w2.StartTimeOfPlannedWork = DateTime.Now.AddHours(-2);
                w2.EndTimeOfPlannedWork = DateTime.Now;
                w2.Lssuer = "叶路22";
                w2.Licensor = "方城22";
                WorkTicketHistory w3 = new WorkTicketHistory() { Id = 3, No = "000003", PersonInCharge = "刘国柱" };
                w3.StartTimeOfPlannedWork = DateTime.Now.AddHours(-2);
                w3.EndTimeOfPlannedWork = DateTime.Now;
                w3.Lssuer = "叶路33";
                w3.Licensor = "方城33";
                WorkTicketHistory w4 = new WorkTicketHistory() { Id = 4, No = "000004", PersonInCharge = "陈浩然" };
                w4.StartTimeOfPlannedWork = DateTime.Now.AddHours(-2);
                w4.EndTimeOfPlannedWork = DateTime.Now;
                w4.Lssuer = "叶路44";
                w4.Licensor = "方城44";
                WorkTicketHistory w5 = new WorkTicketHistory() { Id = 5, No = "000005", PersonInCharge = "李一样" };
                w5.StartTimeOfPlannedWork = DateTime.Now.AddHours(-2);
                w5.EndTimeOfPlannedWork = DateTime.Now;
                w5.Lssuer = "叶路55";
                w5.Licensor = "方城55";
                List<WorkTicketHistory> ws = new List<WorkTicketHistory>() { w1, w2, w3, w4, w5 };
                for (int i = 0; i < 20; i++)
                {
                    WorkTicketHistory wT = new WorkTicketHistory() { Id = 6 + i, No = "000005" + i, PersonInCharge = "马路峰"+i };
                    wT.StartTimeOfPlannedWork = DateTime.Now.AddHours(-2);
                    wT.EndTimeOfPlannedWork = DateTime.Now;
                    wT.Lssuer = "叶路"+i;
                    wT.Licensor = "方城" + i;
                    ws.Add(wT);
                }
                return ws;
            }
            catch (Exception ex)
            {
                Log.Error(LogTags.DbGet, "GetWorkTicketHistoryList", "Exception:" + ex);
                return null;
            }
        }

        public List<WorkTicket> GetWorkTicketList()
        {
            try
            {
                var workTickets = db.WorkTickets.ToList();
                //return workTickets.ToWcfModelList();

                WorkTicket w1 = new WorkTicket() { Id = 1, No = "000001", PersonInCharge = "李新风", AreaId = 6 };
                w1.HeadOfWorkClass = "李新";
                w1.WorkPlace = "主厂房";
                w1.JobContent = "干活";
                w1.StartTimeOfPlannedWork = DateTime.Now.AddHours(-2);
                w1.EndTimeOfPlannedWork = DateTime.Now;
                w1.WorkCondition = "工作条件，在什么...";
                w1.Lssuer = "叶路";
                w1.Licensor = "方城";
                w1.Approver = "吴发展";
                w1.Comment = "已经可以正常使用";
                w1.PersonInChargePersonelId = 7;

                SafetyMeasures s1 = new SafetyMeasures() { Id = 1, No = 1, LssuerContent = "小心触电", LicensorContent = "小心漏电" };
                SafetyMeasures s2 = new SafetyMeasures() { Id = 2, No = 2, LssuerContent = "禁止明火", LicensorContent = "易燃易爆" };
                SafetyMeasures s3 = new SafetyMeasures() { Id = 3, No = 3, LssuerContent = "轻拿轻放", LicensorContent = "设备已损坏" };
                w1.SafetyMeasuress = new List<SafetyMeasures>() { s1, s2, s3 };

                WorkTicket w2 = new WorkTicket() { Id = 2, No = "000002", PersonInCharge = "赵一含", AreaId = 38 };
                WorkTicket w3 = new WorkTicket() { Id = 3, No = "000003", PersonInCharge = "刘国柱", AreaId = 85 };
                WorkTicket w4 = new WorkTicket() { Id = 4, No = "000004", PersonInCharge = "陈浩然" };
                WorkTicket w5 = new WorkTicket() { Id = 5, No = "000005", PersonInCharge = "李一样" };
                List<WorkTicket> ws = new List<WorkTicket>() { w1, w2, w3, w4, w5 };
                return ws;
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.DbGet, "GetWorkTicketList", "Exception:" + ex);
                return null;
            }
        }
    }
}
