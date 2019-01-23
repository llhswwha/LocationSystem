using DbModel.LocationHistory.Work;
using Location.IModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DbModel.Location.Work
{
    /// <summary>
    /// 巡检项
    /// </summary>
    public class PatrolPointItem: IId
    {
        /// <summary>
        /// 检查项Id
        /// </summary>
        [DataMember]
        [Display(Name = "检查项Id")]
        public int Id { get; set; }

        /// <summary>
        /// 所属巡检点ID
        /// </summary>
        [DataMember]
        [Display(Name = "所属巡检点ID")]
        public int ParentId { get; set; }

        /// <summary>
        /// 设备kks编码
        /// </summary>
        [DataMember]
        [Display(Name = "设备kks编码")]
        [MaxLength(32)]
        public string KksCode { get; set; }

        /// <summary>
        /// 巡检内容
        /// </summary>
        [DataMember]
        [Display(Name = "巡检内容")]
        [MaxLength(64)]
        public string CheckItem { get; set; }

        /// <summary>
        /// 巡检员工号
        /// </summary>
        [DataMember]
        [Display(Name = "巡检员工号")]
        [MaxLength(16)]
        public string StaffCode { get; set; }

        /// <summary>
        /// 巡检时间
        /// </summary>
        [DataMember]
        [Display(Name = "巡检时间")]
        public DateTime? dtCheckTime { get; set; }

        /// <summary>
        /// 巡检时间
        /// </summary>
        [DataMember]
        [Display(Name = "巡检时间")]
        public long? CheckTime { get; set; }

        /// <summary>
        /// 巡检项ID
        /// </summary>
        [DataMember]
        [Display(Name = "巡检项ID")]
        [MaxLength(16)]
        public string CheckId { get; set; }

        /// <summary>
        /// 检查结果
        /// </summary>
        [DataMember]
        [Display(Name = "检查结果")]
        [MaxLength(128)]
        public string CheckResult { get; set; }

        public PatrolPointItem()
        {
            KksCode = "";
            CheckItem = "";
            StaffCode = "";
            CheckTime = 0;
            CheckId = "";
            CheckResult = "";
        }

        public PatrolPointItemHistory ToHistory()
        {
            PatrolPointItemHistory history = new PatrolPointItemHistory();
            history.KksCode = KksCode;
            history.CheckItem = CheckItem;
            history.StaffCode = StaffCode;
            history.dtCheckTime = dtCheckTime;
            history.CheckTime = CheckTime;
            history.CheckId = CheckId;
            history.CheckResult = CheckResult;

            return history;
        }
    }
}
