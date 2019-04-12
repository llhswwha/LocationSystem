using Location.IModel;
using Location.TModel.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Location.AreaAndDev
{
    [DataContract]
    public class Dev_CameraInfo:IId
    {
        /// <summary>
        /// 摄像头Id
        /// </summary>
        [DataMember]
        [Display(Name = "摄像头Id")]
        public int Id { get; set; }

        /// <summary>
        /// 摄像头IP
        /// </summary>
        [DataMember]
        [Display(Name = "摄像头IP")]
        public string Ip { get; set; }    
            
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        [Display(Name = "密码")]
        public string PassWord { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        [DataMember]
        [Display(Name = "端口号")]
        public int Port { get; set; }

        /// <summary>
        /// 通道号
        /// </summary>
        [DataMember]
        [Display(Name = "通道号")]
        public int CameraIndex { get; set; }

        /// <summary>
        /// 对应设备，所属区域ID
        /// </summary>
        [DataMember]
        [Display(Name = "设备所属区域Id")]
        public int? ParentId { get; set; }


        /// <summary>
        /// 摄像头在设备表中的ID
        /// </summary>
        [DataMember]
        [Display(Name = "摄像头在设备表中的ID")]
        public int DevInfoId { get; set; }

        /// <summary>
        /// 门禁对应设备的Local_DevID
        /// </summary>
        [DataMember]
        [Display(Name = "门禁对应的本地设备ID")]
        [MaxLength(64)]
        public string Local_DevID { get; set; }
        /// <summary>
        /// 摄像头设备
        /// </summary>
        [DataMember]
        public virtual DevInfo DevInfo { get; set; }

        public Dev_CameraInfo Clone()
        {
            Dev_CameraInfo copy = new Dev_CameraInfo();
            copy = this.CloneObjectByBinary();
            if (this.DevInfo != null)
            {
                copy.DevInfo = this.DevInfo;
            }
            return copy;
        }
    }
}
