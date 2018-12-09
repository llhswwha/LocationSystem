using Location.TModel.ConvertCodes;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.Location.AreaAndDev
{
    [DataContract][Serializable]
    public class Dev_CameraInfo
    {
        /// <summary>
        /// 摄像头Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 摄像头IP
        /// </summary>
        [DataMember]
        public string Ip { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        public string PassWord { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        [DataMember]
        public int Port { get; set; }

        /// <summary>
        /// 通道号
        /// </summary>
        [DataMember]
        public int CameraIndex { get; set; }

        /// <summary>
        /// 对应设备，所属区域ID
        /// </summary>
        [DataMember]
        public int? ParentId { get; set; }


        /// <summary>
        /// 基站对应的设备主键Id
        /// </summary>
        [DataMember]
        public int DevInfoId { get; set; }

        /// <summary>
        /// 门禁对应设备的Local_DevID
        /// </summary>
        [DataMember]
        public string Local_DevID { get; set; }
        /// <summary>
        /// 门禁设备
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
