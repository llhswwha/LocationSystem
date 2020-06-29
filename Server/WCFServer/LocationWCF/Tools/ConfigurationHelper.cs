using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Tools;

namespace LocationServer.Tools
{
    public static class ConfigurationHelper
    {
        public static string GetValue(string key,string defaultValue="04:00")
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }
            return ConfigurationManager.AppSettings[key];
        }

        public static int GetIntValue(string key, int defaultValue=0)
        {
            return ConfigurationManager.AppSettings[key].ToInt(defaultValue);
        }

        public static bool GetBoolValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToBoolean();
        }

        public static double GetDoubleValue(string key, double defaultValue = 0)
        {
            return ConfigurationManager.AppSettings[key].ToDouble(defaultValue);
        }

        public static float GetFloatValue(string key, float defaultValue = 0)
        {
            return ConfigurationManager.AppSettings[key].ToFloat(defaultValue);
        }
    }
}
