using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiLib.Clients.OpcCliect
{
    /// <summary>
    ///  opc参数信息
    /// </summary>
    public class OPCItemParameter
    {
        /// <summary>
        /// 客户端参数句柄
        /// </summary>
        public int ItemHandle { get; set; }
        /// <summary>
        /// 对应PLC值的参数名称(OPC server命名)
        /// </summary>
        public string ParameterName { get; set; }
        /// <summary>
        /// PLC位置名称
        /// </summary>
        public string PLCName { get; set; }
        /// <summary>
        /// 参数值 
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 品质
        /// </summary>
        public string Qualities { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamps { get; set; }
        /// <summary>
        /// 值发生变化的时间,用于后期任务优先级
        /// </summary>
        public DateTime ChangeTime { get; set; }
        /// <summary>
        /// 是否写入成功
        /// </summary>
        public bool IsWriteOk { get; set; }


        public OPCItemParameter()
        {
            this.ChangeTime = DateTime.Now;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pName">opc项名称</param>
        /// <param name="plcName">PLC命名</param>
        /// <param name="handle">客户端句柄</param>
        /// <param name="value">值</param>
        public OPCItemParameter(string pName, string plcName, int handle, int value)
        {
            this.ParameterName = pName;
            this.PLCName = plcName;
            this.ItemHandle = handle;
            this.Value = value;
            this.ChangeTime = DateTime.Now;
            this.IsWriteOk = false;
        }


    }
}
