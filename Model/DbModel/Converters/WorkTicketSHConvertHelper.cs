using DbModel.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.LocationHistory.Work;

namespace DbModel.Converters
{
   public static  class WorkTicketSHConvertHelper
    {
        public static  WorkTicketHistorySH ToTModel(this LocationHistory.Work.WorkTicketHistorySH item)
        {
            if (item == null) return null;
            WorkTicketHistorySH ticket = new WorkTicketHistorySH();
            ticket.id = item.Abutment_Id;
            ticket.fromOrder = item.fromOrder;
            ticket.ticketCode = item.ticketCode;
            ticket.type = item.type;
            ticket.state = item.state;
            ticket.createTime = item.createTime.ToString();
            ticket.startTime = item.startTime.ToString();
            ticket.endTime = item.endTime.ToString();
            ticket.detail = item.detail;
            ticket.ticketName = item.ticketName;
            return ticket;
        }

        public static  List<WorkTicketHistorySH> ToTModelList(this List<LocationHistory.Work.WorkTicketHistorySH> dList)
        {
            if (dList == null) return null;
            List<WorkTicketHistorySH> tList = new List<WorkTicketHistorySH>();
            foreach (DbModel.LocationHistory.Work.WorkTicketHistorySH entity in dList)
            {
                WorkTicketHistorySH ticket = entity.ToTModel();
                ticket.workTicket = null;
                ticket.detail = "";
                tList.Add(ticket);
            }

            return tList;
        }

        public static  LocationHistory.Work.WorkTicketHistorySH ToDbModel(this WorkTicketHistorySH item)
        {
            if (item == null) return null;
            DbModel.LocationHistory.Work.WorkTicketHistorySH db = new DbModel.LocationHistory.Work.WorkTicketHistorySH();
            db.Abutment_Id = item.id;
            db.fromOrder = item.fromOrder;
            db.ticketCode = item.ticketCode;
            db.type = item.type;
            db.state = item.state;
            string createT = item.createTime;
            db.createTime = Convert.ToDateTime(createT);
            string startT = item.startTime;
            db.startTime = Convert.ToDateTime(startT);
            string endT = item.endTime;
            db.endTime = Convert.ToDateTime(endT);
            db.detail = item.detail;
            db.ticketName = item.ticketName;
            return db;
        }

        public static List<LocationHistory.Work.WorkTicketHistorySH> ToDbModelList(this List<WorkTicketHistorySH> tList)
        {
            if (tList == null) return null;
            List<LocationHistory.Work.WorkTicketHistorySH> dLsit = new List<LocationHistory.Work.WorkTicketHistorySH>();
            foreach (WorkTicketHistorySH ticket in tList)
            {
                LocationHistory.Work.WorkTicketHistorySH entity = ticket.ToDbModel();
                dLsit.Add(entity);
            }
            return dLsit;
        }
    }
}
