using DbModel.Location.Work;
using DbModel.LocationHistory.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.Work;

namespace DbModel.Converters
{
   public  static  class OperationTicketSHConvertHelper
    {
        public static OperationTicketSH ToDbModel(this TwoTickets item)
        {
            if (item == null) return null;
            OperationTicketSH db = new OperationTicketSH();
            db.Abutment_Id = item.id;
            db.fromOrder = item.fromOrder;
            db.ticketCode = item.ticketCode;
            db.type = item.type;
            db.state = item.state;
            string createT = item.createTime;
            db.createTime = Convert.ToDateTime(createT);
            string startT = item.startTime;
            db.startTime = Convert.ToDateTime(startT); 
            string endT= item.endTime;
              db.endTime = Convert.ToDateTime(endT);
            db.detail = item.detail;
            db.ticketName = item.ticketName;
            return db;
        }

        public static  OperationTicketHistorySH ToDbHistoryModel(this TwoTickets item)
        {
            if (item == null) return null;
            OperationTicketHistorySH db = new OperationTicketHistorySH();
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


        public static List<OperationTicketSH> ToDbModelList(this List<TwoTickets> TList)
        {
            if (TList == null) return null;
            List<OperationTicketSH> dList = new List<OperationTicketSH>();
            foreach (TwoTickets ticket in TList)
            {
                OperationTicketSH db = ticket.ToDbModel();
                dList.Add(db);
            }
            return dList;
        }
        public static TwoTickets ToTModel(this OperationTicketSH item)
        {
            if (item == null) return null;
            TwoTickets ticket = new TwoTickets();
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

        public static TwoTickets ToTModel(this OperationTicketHistorySH item)
        {
            if (item == null) return null;
            TwoTickets ticket = new TwoTickets();
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



        public static List<TwoTickets> ToTModelList(this List<OperationTicketSH> dList)
        {
            if (dList == null) return null;
            List<TwoTickets> tList = new List<TwoTickets>();
            foreach (OperationTicketSH operation in dList)
            {
                TwoTickets ticket = operation.ToTModel();
                ticket.detailsSet = null;
                ticket.detail = "";
                tList.Add(ticket);
            }
            return tList;
        }

        public static List<TwoTickets> ToTModelList(this List<OperationTicketHistorySH> dList)
        {
            if (dList == null) return null;
            List<TwoTickets> tList = new List<TwoTickets>();
            foreach (OperationTicketHistorySH operationHis in dList)
            {
                TwoTickets ticket = operationHis.ToTModel();
                ticket.detailsSet = null;
                ticket.detail = "";
                tList.Add(ticket);
            }
            return tList;
        }
    }
}
