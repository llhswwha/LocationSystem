using Location.IModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.LocationHistory.AreaAndDev
{
 public  class DevEntranceGuardCardsHistroy
    {
        //DevInfoId,EntranceGuardCardId,OperateTime,OperateTimeStamp,`Code`,description,device_id,card_code,personnelAbutment_Id,b.id as PersonnelId,b.`Name` 
        
            public int id { get; set; }
            /// <summary>
        /// 设备Id
        /// </summary>
        public int? DevInfoId { get; set; }
        /// <summary>
        /// 门禁标识
        /// </summary>
        public int EntranceGuardCardId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }

        public long OperateTimeStamp { get; set; }

        /// <summary>
        /// 结果码(字典：操作状态)，0成功，其他为失败
        /// </summary>
        public int Code { get; set; }
        public string description { get; set; }
        /// <summary>
        /// 设备Id(对接标识)
        /// </summary>
        public string device_id { get; set; }
        /// <summary>
        /// 门禁卡号
        /// </summary>
        public string card_code { get; set; }
        /// <summary>
        /// 人员对接ID
        /// </summary>
        public string personnelAbutment_Id { get; set; }
        /// <summary>
        /// 人员ID
        /// </summary>
        public int? PersonnelId { get; set; }
        /// <summary>
        /// 人员名称
        /// </summary>
        public string Name { get; set; }

    }
}
