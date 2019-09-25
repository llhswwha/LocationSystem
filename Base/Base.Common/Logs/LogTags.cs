using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Location.BLL.Tool
{
    public static class LogTags
    {
        /// <summary>
        /// 获取数据库数据
        /// </summary>
        public static string DbGet = "[DbGet]";

        /// <summary>
        /// 客户端发送消息(WCF)
        /// </summary>
        public static string WCF = "[WCF]";

        /// <summary>
        /// 客户端发送消息(WebApi)
        /// </summary>
        public static string WebApi = "[WebApi]";

        /// <summary>
        /// 服务端消息(Server)
        /// </summary>
        public static string Server = "[Server]";

        /// <summary>
        /// 数据初始化
        /// </summary>
        public static string DbInit = "[DbInit]";

        /// <summary>
        /// 数据初始化
        /// </summary>
        public static string DbInfo = "[DbInfo]";

        /// <summary>
        /// 极视角
        /// </summary>
        public static string ExtremeVision = "[ExtremeVision]";

        /// <summary>
        /// 光谱基础数据平台
        /// </summary>
        public static string BaseData = "[BaseData]";

        /// <summary>
        /// 定位引擎
        /// </summary>
        public static string Engine = "[Engine]";

        /// <summary>
        /// 定位数据写入数据库
        /// </summary>
        public static string Engine2Db = "[Engine2Db]";

        /// <summary>
        /// KKS
        /// </summary>
        public static string KKS = "[KKS]";

        /// <summary>
        /// 告警事件测试
        /// </summary>
        public static string EventTest = "[EventTest]";

        /// <summary>
        /// 历史定位数据获取
        /// </summary>
        public static string HisPos = "[HisPos]";

        /// <summary>
        /// 历史统计数据缓存
        /// </summary>
        public static string HisPosBuffer = "[HisPosBuffer]";

        /// <summary>
        /// EF调试内容
        /// </summary>
        public static string EF = "[EF]";

        /// <summary>
        /// 移动巡检
        /// </summary>
        public static string Inspection = "[Inspection]";

        public static string RealAlarm = "[RealAlarm]";

        public static string AnchorScan = "[AnchorScan]";
    }

}
