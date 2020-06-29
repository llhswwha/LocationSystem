using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.Location.AreaAndDev
{
    /// <summary>
    /// 区域关联定位卡
    /// </summary>
    [DataContract]
    [Serializable]
    public class TopologyToLocationCards
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        [DataMember]
        public int AreaId { get; set; }

        /// <summary>
        /// 关联定位卡
        /// </summary>
        [DataMember]
        public List<string> LocationCards { get; set; }

        /// <summary>
        /// 错误信息(修改信息后，传给客户端)
        /// </summary>
        [DataMember]
        public string ErrorInfo { get; set; }

    }
}
