using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPCAutomation;
using Location.BLL.Tool;
using WebApiLib.Clients.OpcCliect;

namespace WebApiLib.Clients
{
 public   class OPCClient
    {

        #region 全局变量
        /// <summary>
        /// OPC对应PLC的位置
        /// </summary>
        public static Dictionary<string, OPCItemParameter> dtOpcToPlc = new Dictionary<string, OPCItemParameter>();
        /// <summary>
        /// opc服务器信息
        /// </summary>
        public static OPCInformation opcInformation = new OPCInformation();
        #endregion
        /// <summary>
        /// 初始化
        /// </summary>
        public OPCClient()
        { }
    }

}

