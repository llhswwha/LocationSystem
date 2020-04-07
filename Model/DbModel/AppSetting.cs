using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel
{
    public static class AppSetting
    {
        /// <summary>
        /// 是否将定位引擎获取的数据写入日志
        /// </summary>
        public static bool WritePositionLog { get; set; }

        public static double PositionMoveStateWaitTime { get; set; }

        public static double PositionMoveStateOfflineTime { get; set; }

        /// <summary>
        /// 园区节点名称
        /// </summary>
        public static string ParkName { get; set; }

        /// <summary>
        /// 基础平台对接ApiURL
        /// </summary>
        public static string DatacaseWebApiUrl { get; set; }
        public static string ExtremeVisionListenerIP { get; set; }
        public static int CameraAlarmPicSaveMode { get; set; }
        public static string CameraAlarmPicSaveDir { get; set; }
        public static int CameraAlarmKeepDay { get; set; }
        public static bool DeleteAlarmKeepPictureFile { get; set; }
        public static float PositionPower { get; set; }

        public static int UrlMaxLength = 200;

        public static int LowPowerFlag = 370;

        public static int AddHisPositionInterval = 10;
    }
}
