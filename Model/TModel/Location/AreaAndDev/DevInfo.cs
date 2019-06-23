using System;
using System.Runtime.Serialization;
using DbModel.Tools;
using Location.IModel;
using Location.TModel.ConvertCodes;
using Location.TModel.Tools;
using TModel.Tools;

namespace Location.TModel.Location.AreaAndDev
{
    /// <summary>
    /// 设备信息
    /// </summary>
    [DataContract] [Serializable]
    public class DevInfo : IEntity,INode
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        //[Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        //[Display(Name = "名称")]
        //[Required]
        public string Name { get; set; }

        /// <summary>
        /// 区域ID
        /// </summary>
        [DataMember]
        //[Display(Name = "区域ID")]
        public int? ParentId { get; set; }

        public PhysicalTopology Parent { get; set; }

        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [DataMember]
        //[Display(Name = "编码")]
        public string Code { get; set; }

        /// <summary>
        /// KKS编码
        /// </summary>
        [DataMember]
        [ByName("KKS")]
        public string KKSCode { get; set; }


        /// <summary>
        /// 本地设备ID
        /// </summary>
        [DataMember]
        [ByName("Local_DevID")]
        public string DevID { get; set; }

        /// <summary>
        /// 本地机柜ID
        /// </summary>
        [DataMember]
        public string Local_CabinetID { get; set; }

        /// <summary>
        /// 本地设备类型编号
        /// </summary>
        [DataMember]
        public int TypeCode { get; set; }

        private string _typeName = "";

        [DataMember]
        public string TypeName
        {
            get
            {
                if (_typeName == "")
                {
                    _typeName = DevTypeHelper.GetTypeName(TypeCode);
                }
                return _typeName;
            }
            set
            {
                _typeName = value;
            }
        }

        /// <summary>
        /// 对接Id
        /// </summary>
        [DataMember]
        public int? Abutment_Id { get; set; }

        /// <summary>
        /// 对接原始设备Id
        /// </summary>
        [DataMember]
        public string Abutment_DevID { get; set; }

        /// <summary>
        /// 对接分类
        /// </summary>
        [DataMember]
        public Abutment_DevTypes Abutment_Type { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        [DataMember]
        public Abutment_Status Status { get; set; }

        /// <summary>
        /// 设备运行状态
        /// </summary>
        [DataMember]
        public Abutment_RunStatus RunStatus { get; set; }

        /// <summary>
        /// 是否已指定点位
        /// </summary>
        [DataMember]
        public bool? Placed { get; set; }

        /// <summary>
        /// 模型名称
        /// </summary>
        [DataMember]
        public string ModelName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建时间戳
        /// </summary>
        [DataMember]
        public long CreateTimeStamp { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [DataMember]
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 修改时间戳
        /// </summary>
        [DataMember]
        public long ModifyTimeStamp { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// 厂家
        /// </summary>
        [DataMember]
        public string Manufactor { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [DataMember]
        public string IP { get; set; }

        #region DevPos信息

        ///// <summary>
        ///// PosX
        ///// </summary>
        //[DataMember]
        ////[Display(Name = "PosX")]
        //public float PosX { get; set; }

        ///// <summary>
        ///// PosY
        ///// </summary>
        //[DataMember]
        ////[Display(Name = "PosY")]
        //public float PosY { get; set; }

        ///// <summary>
        ///// PosZ
        ///// </summary>
        //[DataMember]
        ////[Display(Name = "PosZ")]
        //public float PosZ { get; set; }

        ///// <summary>
        ///// RotationX
        ///// </summary>
        //[DataMember]
        ////[Display(Name = "RotationX")]
        //public float RotationX { get; set; }

        ///// <summary>
        ///// RotationY
        ///// </summary>
        //[DataMember]
        ////[Display(Name = "RotationY")]
        //public float RotationY { get; set; }

        ///// <summary>
        ///// RotationZ
        ///// </summary>
        //[DataMember]
        ////[Display(Name = "RotationZ")]
        //public float RotationZ { get; set; }

        ///// <summary>
        ///// ScaleX
        ///// </summary>
        //[DataMember]
        ////[Display(Name = "ScaleX")]
        //public float ScaleX { get; set; }

        ///// <summary>
        ///// ScaleY
        ///// </summary>
        //[DataMember]
        ////[Display(Name = "ScaleY")]
        //public float ScaleY { get; set; }

        ///// <summary>
        ///// ScaleZ
        ///// </summary>
        //[DataMember]
        ////[Display(Name = "ScaleZ")]
        //public float ScaleZ { get; set; }

        [DataMember]
        public DevPos Pos { get; set; }

        public void SetPos(DevPos pos)
        {
            
            this.Pos = pos.CloneObjectByBinary();
            this.Pos.DevID = this.DevID;
            //this.PosX = pos.PosX;
            //this.PosY = pos.PosY;
            //this.PosZ = pos.PosZ;
            //this.ScaleX = pos.ScaleX;
            //this.ScaleY = pos.ScaleY;
            //this.ScaleZ = pos.PosZ;
            //this.RotationX = pos.RotationX;
            //this.RotationY = pos.RotationY;
            //this.RotationZ = pos.RotationZ;
        }

        #endregion

        /// <summary>
        /// 详细信息（如基站、摄像头等）
        /// </summary>
        public object DevDetail { get; set; }


        public DevInfo()
        {
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;

            CreateTimeStamp = TimeConvert.ToStamp(CreateTime);
            ModifyTimeStamp = TimeConvert.ToStamp(ModifyTime);
        }

        public DevInfo Clone()
        {
            DevInfo copy = new DevInfo();
            copy = this.CloneObjectByBinary();
            return copy;
        }

        //public DevInfoHistory RemoveToHistory()
        //{
        //    DevInfoHistory history = new DevInfoHistory();
        //    history.Id = this.Id;
        //    history.Name = this.Name;
        //    history.ParentId = this.ParentId;
        //    history.Code = this.Code;
        //    history.KKS = this.KKS;
        //    history.Local_DevID = this.Local_DevID;
        //    history.Local_CabinetID = this.Local_CabinetID;
        //    history.Local_TypeCode = this.Local_TypeCode;
        //    history.Abutment_Id = this.Abutment_Id;
        //    history.Abutment_DevID = this.Abutment_DevID;
        //    history.Abutment_Type = this.Abutment_Type;
        //    history.Status = this.Status;
        //    history.RunStatus = this.RunStatus;
        //    history.Placed = this.Placed;
        //    history.ModelName = this.ModelName;
        //    history.CreateTime = this.CreateTime;
        //    history.CreateTimeStamp = this.CreateTimeStamp;
        //    history.ModifyTime = this.ModifyTime;
        //    history.ModifyTimeStamp = this.ModifyTimeStamp;
        //    history.UserName = this.UserName;
        //    history.IP = this.IP;
        //    history.PosX = this.PosX;
        //    history.PosY = this.PosY;
        //    history.PosZ = this.PosZ;
        //    history.RotationX = this.RotationX;
        //    history.RotationY = this.RotationY;
        //    history.RotationZ = this.RotationZ;
        //    history.ScaleX = this.ScaleX;
        //    history.ScaleY = this.ScaleY;
        //    history.ScaleZ = this.ScaleZ;
        //    history.HistoryTime = DateTime.Now;
        //    history.HistoryTimeStamp = TimeConvert.DateTimeToTimeStamp(history.HistoryTime);

        //    return history;
        //}

        public override string ToString()
        {
            return string.Format("{0},{1}",Name,TypeName);
        }

        public void Refresh(DevInfo dev)
        {
            Name = dev.Name;
            Pos.Refresh(dev.Pos);
        }
    }
}