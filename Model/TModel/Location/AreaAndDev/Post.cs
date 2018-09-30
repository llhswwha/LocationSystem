using System.Runtime.Serialization;
using Location.IModel.Locations;
using Location.TModel.Tools;
using System;

namespace Location.TModel.Location.AreaAndDev
{
    /// <summary>
    /// 岗位
    /// </summary>
    [DataContract] [Serializable]
    public class Post: IPost
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        public Post Clone()
        {
            return this.CloneObjectByBinary();
        }
    }
}
