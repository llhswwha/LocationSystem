using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Tools;

namespace LocationDaemon
{
    public static class ConfigurationHelper
    {
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static int GetIntValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToInt();
        }

        public static bool GetBoolValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToBoolean();
        }

        public static double GetDoubleValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToDouble();
        }
    }
}
