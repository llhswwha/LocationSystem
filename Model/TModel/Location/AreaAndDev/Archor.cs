using System.Runtime.Serialization;
using DbModel.Tools;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Tools;

namespace TModel.Location.AreaAndDev
{
    /// <summary>
    /// 基站
    /// </summary>
    public class Archor
    {
        /// <summary>
        /// 基站Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 基站编号
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// 基站名
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 位置X
        /// </summary>
        [DataMember]
        public double X { get; set; }

        /// <summary>
        /// 位置Y
        /// </summary>
        [DataMember]
        public double Y { get; set; }

        /// <summary>
        /// 位置Z (?)
        /// </summary>
        [DataMember]
        public double Z { get; set; }

        /// <summary>
        /// 类型 : 0 副、1 主
        /// </summary>
        [DataMember]
        public ArchorTypes Type { get; set; }

        /// <summary>
        /// 自动获取IP
        /// </summary>
        [DataMember]
        public bool IsAutoIp { get; set; }

        /// <summary>
        /// 基站IP
        /// </summary>
        [DataMember]
        public string Ip { get; set; }

        /// <summary>
        /// 服务器IP
        /// </summary>
        [DataMember]
        public string ServerIp { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        [DataMember]
        public int ServerPort { get; set; }

        /// <summary>
        /// 发射功率
        /// </summary>
        [DataMember]
        public double Power { get; set; }

        /// <summary>
        /// 心跳间隔
        /// </summary>
        [DataMember]
        public double AliveTime { get; set; }

        /// <summary>
        /// 是否启动，0否，1是
        /// </summary>
        [DataMember]
        public IsStart Enable { get; set; }

        /// <summary>
        /// 基站对应的设备主键Id
        /// </summary>
        [DataMember]
        public int DevInfoId { get; set; }

        public virtual DevInfo DevInfo { get; set; }

        [DataMember]
        public int ParentId { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public Archor Clone()
        {
            Archor copy = this.CloneObjectByBinary();
            if (DevInfo != null)
            {
                copy.DevInfo = DevInfo;
            }
            return copy;
        }
    }
}
