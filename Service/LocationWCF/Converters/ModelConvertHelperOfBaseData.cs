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

    }
}
