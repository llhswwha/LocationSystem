using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Location.Model.LocationTables
{
    [DataContract]
    public class Dev_DoorAccess
    {
        [Key]
        [DataMember]
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
        public virtual DevInfo Dev { get; set; }
    }
}
