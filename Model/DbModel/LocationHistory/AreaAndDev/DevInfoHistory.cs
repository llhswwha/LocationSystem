using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;

namespace DbModel.LocationHistory.AreaAndDev
{
    /// <summary>
    /// 设备历史表
    /// </summary>
    public class DevInfoHistory:IEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        [Display(Name = "名称")]
        [MaxLength(128)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 区域ID
        /// </summary>
        [DataMember]
        [Display(Name = "区域ID")]
        public int? ParentId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [DataMember]
        [Display(Name = "编码")]
        [MaxLength(32)]
        public string Code { get; set; }

        /// <summary>
        /// KKS编码
        /// </summary>
        [DataMember]
        [Display(Name = "KKS编码")]
        [MaxLength(32)]
        public string KKS { get; set; }


        /// <summary>
        /// 本地设备ID
        /// </summary>
        [DataMember]
        [Display(Name = "本地设备ID")]
        [MaxLength(64)]
        public string Local_DevID { get; set; }

        /// <summary>
        /// 本地机柜ID
        /// </summary>
        [DataMember]
        [Display(Name = "本地机柜ID")]
        [MaxLength(64)]
        public string Local_CabinetID { get; set; }

        /// <summary>
        /// 本地设备类型编号
        /// </summary>
        [DataMember]
        [Display(Name = "本地设备类型编号")]
        public int? Local_TypeCode { get; set; }

        /// <summary>
        /// 对接Id
        /// </summary>
        [DataMember]
        [Display(Name = "对接Id")]
        public int? Abutment_Id { get; set; }

        /// <summary>
        /// 对接原始设备Id
        /// </summary>
        [DataMember]
        [Display(Name = "对接原始设备Id")]
        [MaxLength(64)]
        public string Abutment_DevID { get; set; }

        /// <summary>
        /// 对接分类
        /// </summary>
        [DataMember]
        [Display(Name = "对接分类")]
        public Abutment_DevTypes Abutment_Type { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        [DataMember]
        [Display(Name = "设备状态")]
        public Abutment_Status? Status { get; set; }

        /// <summary>
        /// 设备运行状态
        /// </summary>
        [DataMember]
        [Display(Name = "设备运行状态")]
        public Abutment_RunStatus? RunStatus { get; set; }

        /// <summary>
        /// 是否已指定点位
        /// </summary>
        [DataMember]
        [Display(Name = "是否已指定点位")]
        public bool? Placed { get; set; }

        /// <summary>
        /// 模型名称
        /// </summary>
        [DataMember]
        [Display(Name = "模型名称")]
        [MaxLength(128)]
        public string ModelName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建时间戳
        /// </summary>
        [DataMember]
        [Display(Name = "创建时间戳")]
        public long CreateTimeStamp { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [DataMember]
        [Display(Name = "修改时间")]
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 修改时间戳
        /// </summary>
        [DataMember]
        [Display(Name = "修改时间戳")]
        public long ModifyTimeStamp { get; set; }

        /// <summary>
        /// 历史记录产生时间
        /// </summary>
        [DataMember]
        [Display(Name = "历史记录产生时间")]
        public DateTime HistoryTime { get; set; }

        /// <summary>
        /// 历史记录时间戳
        /// </summary>
        [DataMember]
        [Display(Name = "历史记录时间戳")]
        public long HistoryTimeStamp { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        [Display(Name = "用户名")]
        [MaxLength(128)]
        public string UserName { get; set; }

        /// <summary>
        /// 厂家
        /// </summary>
        [DataMember]
        [Display(Name = "厂家")]
        [MaxLength(128)]
        public string Manufactor { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [DataMember]
        [Display(Name = "IP")]
        [MaxLength(32)]
        public string IP { get; set; }

        /// <summary>
        /// PosX
        /// </summary>
        [DataMember]
        [Display(Name = "PosX")]
        public float? PosX { get; set; }

        /// <summary>
        /// PosY
        /// </summary>
        [DataMember]
        [Display(Name = "PosY")]
        public float? PosY { get; set; }

        /// <summary>
        /// PosZ
        /// </summary>
        [DataMember]
        [Display(Name = "PosZ")]
        public float? PosZ { get; set; }

        /// <summary>
        /// RotationX
        /// </summary>
        [DataMember]
        [Display(Name = "RotationX")]
        public float? RotationX { get; set; }

        /// <summary>
        /// RotationY
        /// </summary>
        [DataMember]
        [Display(Name = "RotationY")]
        public float? RotationY { get; set; }

        /// <summary>
        /// RotationZ
        /// </summary>
        [DataMember]
        [Display(Name = "RotationZ")]
        public float? RotationZ { get; set; }

        /// <summary>
        /// ScaleX
        /// </summary>
        [DataMember]
        [Display(Name = "ScaleX")]
        public float? ScaleX { get; set; }

        /// <summary>
        /// ScaleY
        /// </summary>
        [DataMember]
        [Display(Name = "ScaleY")]
        public float? ScaleY { get; set; }

        /// <summary>
        /// ScaleZ
        /// </summary>
        [DataMember]
        [Display(Name = "ScaleZ")]
        public float? ScaleZ { get; set; }

        public DevInfoHistory Clone()
        {
            DevInfoHistory copy = new DevInfoHistory();
            copy = this.CloneObjectByBinary();
            return copy;
        }
    }
}
