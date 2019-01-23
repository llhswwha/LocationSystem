using Location.IModel;
using System;
using System.Runtime.Serialization;

namespace TModel.LocationHistory.Work
{
    /// <summary>
    /// 巡检项
    /// </summary>
    public class PatrolPointItemHistory
    {
        /// <summary>
        /// 检查项Id
        /// </summary>
        [DataMember]
//        [Display(Name = "检查项Id")]
        public int Id { get; set; }

        /// <summary>
        /// 所属巡检点ID
        /// </summary>
        [DataMember]
 //       [Display(Name = "所属巡检点ID")]
        public int ParentId { get; set; }

        /// <summary>
        /// 设备kks编码
        /// </summary>
        [DataMember]
  //      [Display(Name = "设备kks编码")]
        public string KksCode { get; set; }

        /// <summary>
        /// 巡检内容
        /// </summary>
        [DataMember]
  //      [Display(Name = "巡检内容")]
        public string CheckItem { get; set; }

        /// <summary>
        /// 巡检员工号
        /// </summary>
        [DataMember]
 //       [Display(Name = "巡检员工号")]
        public string StaffCode { get; set; }

        /// <summary>
        /// 巡检时间
        /// </summary>
        [DataMember]
 //       [Display(Name = "巡检时间")]
        public DateTime? dtCheckTime { get; set; }

        /// <summary>
        /// 巡检时间
        /// </summary>
        [DataMember]
 //       [Display(Name = "巡检时间")]
        public long? CheckTime { get; set; }

        /// <summary>
        /// 巡检项ID
        /// </summary>
        [DataMember]
 //       [Display(Name = "巡检项ID")]
        public string CheckId { get; set; }

        /// <summary>
        /// 检查结果
        /// </summary>
        [DataMember]
 //       [Display(Name = "检查结果")]
        public string CheckResult { get; set; }

        public PatrolPointItemHistory()
        {
            KksCode = "";
            CheckItem = "";
            StaffCode = "";
            CheckTime = 0;
            CheckId = "";
            CheckResult = "";
        }
    }
}
