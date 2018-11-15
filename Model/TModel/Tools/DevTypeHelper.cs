using IModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.Tools
{
    public static class DevTypeHelper
    {
        public static string GetTypeName(int code)
        {
            if (code == TypeCodes.Archor)
            {
                return "基站";
            }
            else if(code == 20180825)
            {
                return "摄像头";
            }
            else
            {
                return "生产设备";
            }
            //todo:这里改成设备类型表中获取的设备类型信息
        }
    }
}
