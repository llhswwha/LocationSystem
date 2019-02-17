using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.Location.Manage
{
    /// <summary>
    /// 版本信息
    /// </summary>
    [DataContract]
    public class VersionInfo
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [DataMember]
        public string Version { get; set; }

        /// <summary>
        /// 最新3D安装包URL
        /// </summary>
        [DataMember]
        public string LocationURL { get; set; }
    }
}
