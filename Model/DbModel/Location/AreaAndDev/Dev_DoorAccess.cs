using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DbModel.Location.AreaAndDev;
using Location.TModel.Tools;
using Location.IModel;

namespace DbModel.Location.AreaAndDev
{
    [DataContract]
    public class Dev_DoorAccess:IId
    {
        /// <summary>
        /// 基站Id
        /// </summary>
        [DataMember]
        [Display(Name = "门禁Id")]
        public int Id { get; set; }

       
        /// <summary>
        /// 对应设备，所属区域ID
        /// </summary>
        [DataMember]
        [Display(Name ="设备所属区域Id")]
        public int? ParentId { get; set; }

        /// <summary>
        /// 设备关联的门ID
        /// </summary>
        [DataMember]
        [Display(Name ="门禁所属门的Id")]
        [MaxLength(64)]
        public string DoorId { get; set; }

        /// <summary>
        /// 基站对应的设备主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "门禁对应的设备主键Id")]
        public int DevInfoId { get; set; }

        /// <summary>
        /// 门禁对应设备的Local_DevID
        /// </summary>
        [DataMember]
        [Display(Name = "门禁对应的本地设备ID")]
        [MaxLength(64)]
        public string Local_DevID { get; set; }
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
