using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TModel.Tools;

namespace WebLocation.Tools
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
    }
}