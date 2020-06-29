using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiLib.Clients.OpcCliect
{
    /// <summary>
    /// OPC服务器的参数信息
    /// </summary>
    public class OPCInformation
    {
        /// <summary>
        /// ip地址
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// opc服务器名称
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// 服务器句柄
        /// </summary>
        public int itmHandleServer { get; set; }
        /// <summary>
        /// opc服务器对象
        /// </summary>
        public OPCServer KepServer { get; set; }
        /// <summary>
        /// opc组别集合对象
        /// </summary>
        public OPCGroups KepGroups { get; set; }
        /// <summary>
        /// opc组别对象
        /// </summary>
        public OPCGroup KepGroup { get; set; }
        /// <summary>
        /// opc项集合对象
        /// </summary>
        public OPCItems KepItems { get; set; }
        /// <summary>
        /// opc项对象
        /// </summary>
        public OPCItem KepItem { get; set; }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool ConnectState { get; set; }
        /// <summary>
        /// 连接内容
        /// </summary>
        public string ConnectContents { get; set; }
        /// <summary>
        /// 创建群组是否成功
        /// </summary>
        public bool GroupsState { get; set; }


        public OPCInformation()
        {
            this.Ip = string.Empty;
            this.HostName = string.Empty;
            this.ConnectState = false;
            this.GroupsState = false;
            this.ConnectContents = "Opc Failed";
        }



    }
}
