using DbModel.Tools;
using Location.IModel;
using Location.TModel.ConvertCodes;
using Location.TModel.Tools;
using System.Runtime.Serialization;
using System;
using Location.TModel.Location.AreaAndDev;

namespace Location.TModel.Location.AreaAndDev
{
    [DataContract] [Serializable]
    public class Dev_DoorAccess
    {
        /// <summary>
        /// 门禁Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 基站对应的设备主键Id
        /// </summary>
        [DataMember]
        public int DevInfoId { get; set; }

        /// <summary>
        /// 本地设备ID
        /// </summary>
        [DataMember]
        [ByName("Local_DevID")]
        public string DevID { get; set; }
        /// <summary>
        /// 对应设备，所属区域ID
        /// </summary>
        [DataMember]
        public int? ParentId { get; set; }

        /// <summary>
        /// 设备关联的门ID
        /// </summary>
        [DataMember]
        public string DoorId { get; set; }

        /// <summary>
        /// 门禁设备
        /// </summary>
        [DataMember]
        public virtual DevInfo DevInfo { get; set; }


        public Dev_DoorAccess Clone()
        {
            Dev_DoorAccess copy = new Dev_DoorAccess();
            copy = this.CloneObjectByBinary();
            if (this.DevInfo != null)
            {
                copy.DevInfo = this.DevInfo;
            }
            return copy;
        }
    }
}
