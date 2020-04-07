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

        /// <summary>
        /// 门禁
        /// </summary>
        public static int Door = 20190926;
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

        public static string ArchorDev = "基站";

        public static string CameraDev = "摄像头";

        public static string StaticDev = "生产设备";

        public static string AlarmDev = "警报设备";

        public static string DoorAccessDev = "门禁";

        public static string OtherDev = "其他设备";
    }

    public class TypeCodeHelper
    {
        private static Dictionary<string, string> CodeTypeNameDic = new Dictionary<string, string>();
        public static string GetTypeName(string code)
        {
            if (string.IsNullOrEmpty(code)) return TypeNames.OtherDev;
            if (CodeTypeNameDic.ContainsKey(code))
            {
                return CodeTypeNameDic[code];
            }
            else
            {
                if (IsLocationDev(code))
                {
                    AddCodeToDic(code,TypeNames.ArchorDev);
                    return TypeNames.ArchorDev;
                }
                else if (IsCamera(code))
                {
                    AddCodeToDic(code, TypeNames.CameraDev);
                    return TypeNames.CameraDev;
                }
                else if (IsStaticDev(code))
                {
                    AddCodeToDic(code, TypeNames.StaticDev);
                    return TypeNames.StaticDev;
                }
                else if (IsAlarmDev(code))
                {
                    AddCodeToDic(code, TypeNames.AlarmDev);
                    return TypeNames.AlarmDev;
                }
                else if (IsDoorAccess(code))
                {
                    AddCodeToDic(code, TypeNames.DoorAccessDev);
                    return TypeNames.DoorAccessDev;                   
                }
                else
                {
                    AddCodeToDic(code, TypeNames.OtherDev);
                    return TypeNames.OtherDev;
                }
            }           
        }
        /// <summary>
        /// 添加Code和对应的Type
        /// </summary>
        /// <param name="codeTemp"></param>
        /// <param name="typeTemp"></param>
        private static void AddCodeToDic(string codeTemp,string typeTemp)
        {
            if (!CodeTypeNameDic.ContainsKey(codeTemp)) CodeTypeNameDic.Add(codeTemp, typeTemp);
        }

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

        public static int DoorAccessTypecode = 20190926;

        private static string DoorAccess = "20190926|其他_单联单控开关_3D";
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


        private static string FireFightDevType = "104|";

        /// <summary>
        /// 是否包含消防设备
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public static bool IsFireFightDevType(string typeCode)
        {
            if (string.IsNullOrEmpty(typeCode)) return false;
            return IsTypeCodeContains(typeCode, FireFightDevType);
        }
        /// <summary>
        /// key是typecode,value是TypeName(通过ContainsKey，能快速判断typeCode是否属于这个TypeName)
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static  Dictionary<string,string>TryGetCodeByType(string typeName)
        {
            Dictionary<string, string> mList = new Dictionary<string, string>();
            if(typeName==TypeNames.ArchorDev)
            {
                return TyeGetTypeCodeDic(typeName, LocationDev);
            }
            else if(typeName == TypeNames.CameraDev)
            {
                return TyeGetTypeCodeDic(typeName, CameraType);
            }
            else if (typeName == TypeNames.StaticDev)
            {
                return TyeGetTypeCodeDic(typeName, StaticDevTypeCode);
            }
            else if (typeName == TypeNames.AlarmDev)
            {
                return TyeGetTypeCodeDic(typeName, AlarmDevTypeCodes);
            }
            else if (typeName == TypeNames.DoorAccessDev)
            {
                return TyeGetTypeCodeDic(typeName, DoorAccess);
            }
            else
            {
                mList.Add("0",TypeNames.OtherDev);
                return mList;
            }
        }
        /// <summary>
        /// 设备类型，对应的TypeCode
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> TypeNameToCodeDic = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 通过设备类型，获取设备类型下对应的所有Typecode
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="allCode"></param>
        /// <returns></returns>
        private static Dictionary<string,string>TyeGetTypeCodeDic(string typeName,string allCode)
        {
            if(TypeNameToCodeDic.ContainsKey(typeName))
            {
                return TypeNameToCodeDic[typeName];
            }
            else
            {
                string[] part = allCode.Split('|');
                Dictionary<string, string> typeNameCodes = new Dictionary<string, string>();
                foreach(var item in part)
                {
                    if(!string.IsNullOrEmpty(item)&&!typeNameCodes.ContainsKey(item))
                    {
                        typeNameCodes.Add(item,typeName);
                    }
                }
                if(!TypeNameToCodeDic.ContainsKey(typeName))
                {
                    TypeNameToCodeDic.Add(typeName,typeNameCodes);
                }
                return TypeNameToCodeDic[typeName];
            }
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
