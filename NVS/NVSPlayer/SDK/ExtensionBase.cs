using System;


namespace NVSPlayer
{
    internal static class ExtensionBase
    {
        /// <summary>
        /// 默认为false，字符串不等于"true"或"TRUE"或"True"等的话就返回false
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string input)
        {
            //input = input.ToLower();
            //bool r = input == "true";
            //return r;

            bool rec = false ;
            bool.TryParse(input, out rec);
            return rec;
        }

        /// <summary>
        /// defaultValue为默认值，当字符串为""时，返回defaultValue
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string input, bool defaultValue)
        {
            if (input == "")
                return defaultValue;
            return ToBoolean(input);
        }


        public static int IntCompareTo(this string input,string input2,int toBase)
        {
            long value1 = Convert.ToInt64(input, toBase);
            long value2 = Convert.ToInt64(input2, toBase);
            int r= value1.CompareTo(value2);
            return r;
        }

        public static int ToIntEx(this string input)
        {
            if (input.StartsWith("0x"))
            {
                return Convert.ToInt32(input, 16);
            }
            else
            {
                return ToInt(input);
            }
        }

        public static int ToInt(this string input, int defaultValue=0)
        {
            if (string.IsNullOrEmpty(input))
                return defaultValue;
            int rec;
            int.TryParse(input, out rec);
            return rec;
        }

        public static long ToLong(this string input, int defaultValue = 0)
        {
            if (string.IsNullOrEmpty(input))
                return defaultValue;
            long rec;
            long.TryParse(input, out rec);
            return rec;
        }

        public static float ToFloat(this string input, float defaultValue = 0)
        {
            if (string.IsNullOrEmpty(input))
                return defaultValue;
            float rec;
            float.TryParse(input, out rec);
            return rec;
        }

        public static double ToDouble(this string input, double defaultValue = 0)
        {
            if (string.IsNullOrEmpty(input))
                return defaultValue;
            double rec = 0;
            double.TryParse(input, out rec);
            return rec;
        }

        /// <summary>
        /// 将string转换成基本数字类型
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToType(this string input, Type type)
        {
            if (type == typeof(bool))
            {
                return input.ToBoolean();
            }
            else if (type == typeof(int))
            {
                return input.ToInt();
            }
            else if (type == typeof(float))
            {
                return input.ToFloat();
            }
            else if (type == typeof(double))
            {
                return input.ToDouble();
            }
            return input;
        }

        public static T ToType<T>(this string input)
        {
            Type type = typeof(T);
            return (T)ToType(input, type);
        }

        //public static Color ToColor(this string value)
        //{
        //    if (string.IsNullOrEmpty(value))
        //        return Color.Transparent;
        //    string[] values = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //    if (values.Length == 1)
        //    {
        //        return Color.FromName(values[0]);
        //    }
        //    else if (values.Length == 3)
        //    {
        //        return Color.FromArgb(values[0].ToInt(), values[1].ToInt(), values[2].ToInt());
        //    }
        //    else if (values.Length == 4)
        //    {
        //        return Color.FromArgb(values[0].ToInt(), values[1].ToInt(), values[2].ToInt(), values[3].ToInt());
        //    }
        //    return Color.Transparent;
        //}

        public static DateTime ToDateTime(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return DateTime.Now;
            DateTime rec;
            DateTime.TryParse(input, out rec);
            return rec;
        }
    }
}
