using DbModel.LocationHistory.AreaAndDev;
using DbModel.Tools;
using Location.IModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Tools;

namespace DbModel.Location.AreaAndDev
{
    /// <summary>
    /// 设备信息
    /// </summary>
    public class DevInfo : INode
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        [Display(Name = "名称")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 区域ID
        /// </summary>
        [DataMember]
        [Display(Name = "区域ID")]
        public int? ParentId { get; set; }

        public virtual Area Parent { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [DataMember]
        [Display(Name = "编码")]
        public string Code { get; set; }

        /// <summary>
        /// KKS编码
        /// </summary>
        [DataMember]
        [Display(Name = "KKS编码")]
        public string KKS { get; set; }


        /// <summary>
        /// 本地设备ID
        /// </summary>
        [DataMember]
        [Display(Name = "本地设备ID")]
        public string Local_DevID { get; set; }

        /// <summary>
        /// 本地机柜ID
        /// </summary>
        [DataMember]
        [Display(Name = "本地机柜ID")]
        public string Local_CabinetID { get; set; }

        /// <summary>
        /// 本地设备类型编号
        /// </summary>
        [DataMember]
        [Display(Name = "本地设备类型编号")]
        public int Local_TypeCode { get; set; }

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
        public Abutment_Status Status{ get; set; }

        /// <summary>
        /// 设备运行状态
        /// </summary>
        [DataMember]
        [Display(Name = "设备运行状态")]
        public Abutment_RunStatus RunStatus { get; set; }

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
        /// 用户名
        /// </summary>
        [DataMember]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [DataMember]
        [Display(Name = "IP")]
        public string IP { get; set; }

        #region DevPos信息
        /// <summary>
        /// PosX
        /// </summary>
        [DataMember]
        [Display(Name = "PosX")]
        public float PosX { get; set; }

        /// <summary>
        /// PosY
        /// </summary>
        [DataMember]
        [Display(Name = "PosY")]
        public float PosY { get; set; }

        /// <summary>
        /// PosZ
        /// </summary>
        [DataMember]
        [Display(Name = "PosZ")]
        public float PosZ { get; set; }

        /// <summary>
        /// RotationX
        /// </summary>
        [DataMember]
        [Display(Name = "RotationX")]
        public float RotationX { get; set; }

        /// <summary>
        /// RotationY
        /// </summary>
        [DataMember]
        [Display(Name = "RotationY")]
        public float RotationY { get; set; }

        /// <summary>
        /// RotationZ
        /// </summary>
        [DataMember]
        [Display(Name = "RotationZ")]
        public float RotationZ { get; set; }

        /// <summary>
        /// ScaleX
        /// </summary>
        [DataMember]
        [Display(Name = "ScaleX")]
        public float ScaleX { get; set; }

        /// <summary>
        /// ScaleY
        /// </summary>
        [DataMember]
        [Display(Name = "ScaleY")]
        public float ScaleY { get; set; }

        /// <summary>
        /// ScaleZ
        /// </summary>
        [DataMember]
        [Display(Name = "ScaleZ")]
        public float ScaleZ { get; set; }

        public void SetPos(DevPos pos)
        {
            if (pos == null) return;
            this.PosX = pos.PosX;
            this.PosY = pos.PosY;
            this.PosZ = pos.PosZ;
            this.ScaleX = pos.ScaleX;
            this.ScaleY = pos.ScaleY;
            this.ScaleZ = pos.ScaleZ;
            this.RotationX = pos.RotationX;
            this.RotationY = pos.RotationY;
            this.RotationZ = pos.RotationZ;
        }

        public DevPos GetPos()
        {
            DevPos pos=new DevPos();
            pos.DevID = this.Local_DevID;
            pos.PosX = this.PosX;
            pos.PosY = this.PosY;
            pos.PosZ = this.PosZ;
            pos.ScaleX = this.ScaleX;
            pos.ScaleY = this.ScaleY;
            pos.ScaleZ = this.ScaleZ;
            pos.RotationX = this.RotationX;
            pos.RotationY = this.RotationY;
            pos.RotationZ = this.RotationZ;
            return pos;
        }

        #endregion


        public DevInfo()
        {
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;

            CreateTimeStamp = TimeConvert.DateTimeToTimeStamp(CreateTime);
            ModifyTimeStamp = TimeConvert.DateTimeToTimeStamp(ModifyTime);
        }

        public DevInfo Clone()
        {
            DevInfo copy = new DevInfo();
            copy = this.CloneObjectByBinary();
            return copy;
        }

        public DevInfoHistory RemoveToHistory()
        {
            DevInfoHistory history = new DevInfoHistory();
            history.Id = this.Id;
            history.Name = this.Name;
            history.ParentId = this.ParentId;
            history.Code = this.Code;
            history.KKS = this.KKS;
            history.Local_DevID = this.Local_DevID;
            history.Local_CabinetID = this.Local_CabinetID;
            history.Local_TypeCode = this.Local_TypeCode;
            history.Abutment_Id = this.Abutment_Id;
            history.Abutment_DevID = this.Abutment_DevID;
            history.Abutment_Type = this.Abutment_Type;
            history.Status = this.Status;
            history.RunStatus = this.RunStatus;
            history.Placed = this.Placed;
            history.ModelName = this.ModelName;
            history.CreateTime = this.CreateTime;
            history.CreateTimeStamp = this.CreateTimeStamp;
            history.ModifyTime = this.ModifyTime;
            history.ModifyTimeStamp = this.ModifyTimeStamp;
            history.UserName = this.UserName;
            history.IP = this.IP;
            history.PosX = this.PosX;
            history.PosY = this.PosY;
            history.PosZ = this.PosZ;
            history.RotationX = this.RotationX;
            history.RotationY = this.RotationY;
            history.RotationZ = this.RotationZ;
            history.ScaleX = this.ScaleX;
            history.ScaleY = this.ScaleY;
            history.ScaleZ = this.ScaleZ;
            history.HistoryTime = DateTime.Now;
            history.HistoryTimeStamp = TimeConvert.DateTimeToTimeStamp(history.HistoryTime);

            return history;
        }
    }
}
