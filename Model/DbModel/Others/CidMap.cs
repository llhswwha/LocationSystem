using System.Collections.Generic;
using System.Xml.Serialization;

namespace DbModel.Others
{
    /// <summary>
    /// 视频告警对接
    /// </summary>
    public class CidMap
    {
        /// <summary>
        /// 对接方的摄像头id
        /// </summary>
        [XmlAttribute]
        public string cid { get; set; }
        /// <summary>
        /// 我们的摄像头id
        /// </summary>
        [XmlAttribute]
        public string id { get; set; }

        public override string ToString()
        {
            return string.Format("{0},{1}", cid, id) ;
        }
    }

    [XmlRoot("CidMapList")]
    public class CidMapList : List<CidMap>
    {

    }
}
