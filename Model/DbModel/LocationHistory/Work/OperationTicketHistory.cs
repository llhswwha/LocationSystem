using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;

namespace DbModel.LocationHistory.Work
{
    /// <summary>
    /// 操作票历史记录
    /// </summary>
    public class OperationTicketHistory:IId
    {
        /// <summary>
        /// 操作票Id
        /// </summary>
        [DataMember]
        [Display(Name = "操作票Id")]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// 操作票编号
        /// </summary>
        [DataMember]
        [Display(Name = "操作票编号")]
        [MaxLength(32)]
        public string No { get; set; }

        /// <summary>
        /// 操作任务
        /// </summary>
        [DataMember]
        [Display(Name = "操作任务")]
        [MaxLength(128)]
        public string OperationTask { get; set; }

        /// <summary>
        /// 操作开始时间
        /// </summary>
        [DataMember]
        [Display(Name = "操作开始时间")]
        public DateTime OperationStartTime { get; set; }

        /// <summary>
        /// 操作结束时间
        /// </summary>
        [DataMember]
        [Display(Name = "操作结束时间")]
        public DateTime OperationEndTime { get; set; }

        /// <summary>
        /// 操作项
        /// </summary>
        [DataMember]
        [ForeignKey("TicketId")]
        public virtual List<OperationItemHistory> OperationItems { get; set; }

        /// <summary>
        /// 监护人
        /// </summary>
        [DataMember]
        [Display(Name = "监护人")]
        [MaxLength(128)]
        public string Guardian { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [DataMember]
        [Display(Name = "操作人")]
        [MaxLength(128)]
        public string Operator { get; set; }

        /// <summary>
        /// 值班负责人
        /// </summary>
        [DataMember]
        [Display(Name = "值班负责人")]
        [MaxLength(128)]
        public string DutyOfficer { get; set; }

        /// <summary>
        /// 调度
        /// </summary>
        [DataMember]
        [Display(Name = "调度")]
        [MaxLength(128)]
        public string Dispatch { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        [Display(Name = "备注")]
        [MaxLength(128)]
        public string Remark { get; set; }

        public OperationTicketHistory Clone()
        {
            OperationTicketHistory copy = new OperationTicketHistory();
            copy = this.CloneObjectByBinary();

            return copy;
        }
    }
}
