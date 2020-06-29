using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Converters
{
   public  class ObjectConvertToData
    {
        public static T ConvertObject<T>(object asObject)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            //obj转化为json；
            string json = JsonConvert.SerializeObject(asObject, setting);
            T data= JsonConvert.DeserializeObject<T>(json, setting);
            return data;
        }
    }
}
