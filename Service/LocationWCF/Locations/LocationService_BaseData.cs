using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using Location.BLL.ServiceHelpers;
using Location.Model.DataObjects.ObjectAddList;
using LocationServices.Converters;
using TModel.BaseData;
using WebApiLib.Clients;
using TModel.Location.Work;
using TModel.LocationHistory.Work;

namespace LocationServices.Locations
{
    //基础平台相关的接口
    public partial class LocationService : ILocationService, IDisposable
    {
        private BaseDataClient GetClient()
        {
            //return new BaseDataClient("localhost","9347");
            return new BaseDataClient("ipms-demo.datacase.io", "api");
        }

        public Ticket GetTicketDetial(int id)
        {
            var client = GetClient();
            var ticket=client.GetTicketsDetail(id);
            if (ticket == null)
            {
                return null;
            }
            return ticket.ToTModel();
        }

        public List<Ticket> GetTicketList(int type, DateTime start, DateTime end)
        {
            var client = GetClient();
            var re=client.GetTicketsList(type+"", start.ToString("yyyyMMdd"), end.ToString("yyyyMMdd"));
            if (re == null)
            {
                return null;
            }
            return re.data.ToWcfModelList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtBeginTime">起始时间</param>
        /// <param name="dtEndTime">结束时间</param>
        /// <param name="bFlag">值为True获取所有历史记录，值为False，按起始时间和结束时间获取历史记录</param>
        /// <returns></returns>
        public List<InspectionTrackHistory> Getinspectionhistorylist(DateTime dtBeginTime, DateTime dtEndTime, bool bFlag)
        {
            List<DbModel.LocationHistory.Work.InspectionTrackHistory> lst = new List<DbModel.LocationHistory.Work.InspectionTrackHistory>();
            if (bFlag)
            {
                lst = db2.InspectionTrackHistorys.ToList();
            }
            else
            {
                long lBeginTime = Location.TModel.Tools.TimeConvert.DateTimeToTimeStamp(dtBeginTime);
                long lEndTime = Location.TModel.Tools.TimeConvert.DateTimeToTimeStamp(dtEndTime);

                lst = db2.InspectionTrackHistorys.Where(p=>p.StartTime >= lBeginTime && p.EndTime <= lEndTime).ToList();

            }

            return lst.ToWcfModelList();
        }
    }
}
