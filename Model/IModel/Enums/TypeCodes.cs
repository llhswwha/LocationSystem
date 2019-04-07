using System;
using System.Collections.Generic;
using System.Text;

namespace IModel.Enums
{
    public class TypeCodes
    {
        /// <summary>
        /// 基站设备TypeCode
        /// </summary>
        public static int Archor = 20180821;

        /// <summary>
        /// 测点
        /// </summary>
        public static int TrackPoint = 100001;

        /// <summary>
        /// 测点
        /// </summary>
        public static int Camera = 3000201;
    }
    /// <summary>
    /// 区域名称
    /// </summary>
    public class DepNames
    {
        public static string FactoryName="四会热电厂";
    }
    public class TypeNames
    {
        /// <summary>
        /// 基站设备模型名称
        /// </summary>
        public static string Archor = "定位设备1_3D";

        public static string ArchorOutdoor = "定位设备2_3D";
    }

    public class TypeCodeHelper
    {
        private static string CameraType = "3000201|14|3000610|1000102";

        /// <summary>
        /// 根据TypeCode判断是否是摄像机
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public static bool IsCamera(string typeCode)
        {
            if (string.IsNullOrEmpty(typeCode)) return false;
            return IsTypeCodeContains(typeCode, CameraType);
        }

        private static string DoorAccess = "其他_单联单控开关_3D|";
        /// <summary>
        /// 根据TypeCode判断是否是门禁
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public static bool IsDoorAccess(string typeCode)
        {
            if (string.IsNullOrEmpty(typeCode)) return false;
            return IsTypeCodeContains(typeCode, DoorAccess);
        }

        private static string LocationDev = "20180821|";
        /// <summary>
        /// 是否定位设备
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public static bool IsLocationDev(string typeCode)
        {
            if (string.IsNullOrEmpty(typeCode)) return false;
            return IsTypeCodeContains(typeCode, LocationDev);
        }
        /// <summary>
        /// 静态设备Typecode
        /// </summary>
        public static string StaticDevTypeCode = "20181008|";
        /// <summary>
        /// 是否静态设备
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public static bool IsStaticDev(string typeCode)
        {
            if (string.IsNullOrEmpty(typeCode)) return false;
            return IsTypeCodeContains(typeCode, StaticDevTypeCode);
        }

        /// <summary>
        /// 警报设备
        /// </summary>
        private static string AlarmDevTypeCodes = "2018113001|2018113002|2018113003|2018113004|2018113005|2018113006|2018113007|2018113008|2018113009|";
        /// <summary>
        /// 是否警报设备（消防设备等）
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public static bool IsAlarmDev(string typeCode)
        {
            if (string.IsNullOrEmpty(typeCode)) return false;
            return IsTypeCodeContains(typeCode, AlarmDevTypeCodes);
        }

        private static string BorderAlarmDev = "20181203|";
        /// <summary>
        /// 是否边界告警设备
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public static bool IsBorderAlarmDev(string typeCode)
        {
            if (string.IsNullOrEmpty(typeCode)) return false;
            return IsTypeCodeContains(typeCode, BorderAlarmDev);
        }


        /// <summary>
        /// 是否包含TypeCode
        /// </summary>
        /// <param name="typeCode"></param>
        /// <param name="allCode"></param>
        /// <returns></returns>
        private static bool IsTypeCodeContains(string typeCode, string allCode)
        {
            string[] part = allCode.Split('|');
            foreach (var item in part)
            {
                if (item == typeCode)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
