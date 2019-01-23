using Location.IModel;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DbModel.Tools;

namespace TModel.LocationHistory.Work
{
    /// <summary>
    /// 巡检轨迹历史记录
    /// </summary>
    public class InspectionTrackHistory
    {
        /// <summary>
        /// 巡检单Id
        /// </summary>
        [DataMember]
//        [Display(Name = "巡检单Id")]
        public int Id { get; set; }

        /// <summary>
        /// 对接Id
        /// </summary>
        [DataMember]
 //       [Display(Name = "对接Id")]
        public int? Abutment_Id { get; set; }

        /// <summary>
        /// 巡检单编号，移动巡检系统中的唯一编号或名称
        /// </summary>
        [DataMember]
 //       [Display(Name = "巡检单编号")]
        public string Code { get; set; }

        /// <summary>
        /// 巡检路线名称
        /// </summary>
        [DataMember]
  //      [Display(Name = "巡检路线名称")]
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
//        [Display(Name = "创建时间")]
        public DateTime dtCreateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
//        [Display(Name = "创建时间")]
        public long CreateTime { get; set; }

        /// <summary>
        /// 巡检状态，新建；已下达 ；已完成；已取消；执行中；已过期
        /// </summary>
        [DataMember]
//        [Display(Name = "巡检状态")]
        public string State { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
 //       [Display(Name = "开始时间")]
        public DateTime dtStartTime { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
 //       [Display(Name = "开始时间")]
        public long StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
 //       [Display(Name = "结束时间")]
        public DateTime dtEndTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
//        [Display(Name = "结束时间")]
        public long EndTime { get; set; }

        /// <summary>
        /// 巡检点列表
        /// </summary>
        [DataMember]
//        [ForeignKey("ParentId")]
//        [NotMapped]
        public virtual List<PatrolPointHistory> Route { get; set; }

        public InspectionTrackHistory()
        {
            Abutment_Id = 0;
            Code = "";
            Name = "";
            CreateTime = 0;
            State = "新建";
            StartTime = 0;
            EndTime = 0;
            Route = new List<PatrolPointHistory>();

        }
    }
}
