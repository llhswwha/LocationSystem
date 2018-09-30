using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using BLL;
using BLL.ServiceHelpers;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using Location.BLL.ServiceHelpers;
using Location.Model.DataObjects.ObjectAddList;
using LocationServices.Converters;
using TModel.BaseData;
using WebApiLib.Clients;

namespace LocationServices.Locations
{
    //基础平台相关的接口
    public partial class LocationService : ILocationService, IDisposable
    {
        private BaseDataClient GetClient()
        {
            return new BaseDataClient("localhost","9347");
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
    }
}
