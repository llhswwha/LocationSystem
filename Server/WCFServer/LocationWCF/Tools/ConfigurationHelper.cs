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
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static int GetIntValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToInt();
        }
    }
}
