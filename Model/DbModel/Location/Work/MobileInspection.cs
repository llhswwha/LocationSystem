using DbModel.Tools;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;
using System;

namespace DbModel.Location.Work
{
    /// <summary>
    /// 移动巡检轨迹列表
    /// </summary>
    public class MobileInspection:IEntity
    {
        /// <summary>
        /// 巡检轨迹Id
        /// </summary>
        [DataMember]
        [Display(Name = "巡检轨迹Id")]
        public int Id { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        [DataMember]
        [Display(Name = "顺序")]
        public int nOrder { get; set; }

        /// <summary>
        /// 巡检轨迹名称
        /// </summary>
        [DataMember]
        [Display(Name = "巡检轨迹名称")]
        [MaxLength(64)]
        public string Name { get; set; }

        /// <summary>
        /// 操作项
        /// </summary>
        [DataMember]
        [Display(Name = "操作项")]
        [ForeignKey("PID")]
        public virtual List<MobileInspectionItem> Items { get; set; }


        ///// <summary>
        ///// 对接巡检单Id
        ///// </summary>
        //[DataMember]
        //[Display(Name = "对接巡检单Id")]
        //public int? Abutment_Id { get; set; }

        ///// <summary>
        ///// 巡检单编号
        ///// </summary>
        //[DataMember]
        //[Display(Name = "巡检单编号")]
        //public string Code { get; set; }

        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //[DataMember]
        //[Display(Name = "创建时间")]
        //public DateTime CreateTime { get; set; }

        ///// <summary>
        ///// 创建时间戳
        ///// </summary>
        //[DataMember]
        //[Display(Name = "创建时间戳")]
        //public long CreateTimeStamp { get; set; }

        ///// <summary>
        ///// 工单状态
        ///// </summary>
        //public string State { get; set; }











        public MobileInspection Clone()
        {
            MobileInspection copy = new MobileInspection();
            copy = this.CloneObjectByBinary();

            return copy;
        }
    }
}
