using Location.IModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Location.TModel.Tools;

namespace DbModel.LocationHistory.Door
{
    /// <summary>
    /// 门禁事件历史
    /// </summary>
  public    class DoorClick : IId
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }
        [DataMember]
        [Display(Name = "事件编号")]
        public string eventId { get; set; }
        [DataMember]
        [Display(Name = "事件名称")]
        public string eventName { get; set; }
        [DataMember]
        [Display(Name = "发生时间")]
        public DateTime eventTime { get; set; }
        [DataMember]
        [Display(Name = "发生时间戳")]
        public long eventTimeStmp { get; set; }
        [DataMember]
        [Display(Name = "人员Id")]
        public string personId { get; set; }
        [DataMember]
        [Display(Name = "卡号")]
        public string cardNo { get; set; }
        [DataMember]
        [Display(Name = "人员名称")]
        public string personName { get; set; }
        [DataMember]
        [Display(Name = "组织编码")]
        public string orgIndexCode { get; set; }
        [DataMember]
        [Display(Name = "门禁名称")]
        public string doorName { get; set; }
        [DataMember]
        [Display(Name = "门禁标识")]
        public string doorIndexCode { get; set; }
        [DataMember]
        [Display(Name = "门禁区域标识")]
        public string doorRegionIndexCode { get; set; }
        [DataMember]
        [Display(Name = "图片地址")]
        public string picUri { get; set; }
        [DataMember]
        [Display(Name = "图片存储服务的唯一标识")]
        public string svrIndexCode { get; set; }
        [DataMember]
        [Display(Name = "事件类型")] //198914:表示合法卡比对通过
        public int eventType { get; set; }
        [DataMember]
        [Display(Name = "进出类型")] //1：进，0：出，-1：未知，要求：进门读卡器拨码设置为1，出门读卡器拨码设置为2
        public int inAndOutType { get; set; }
    }
}
