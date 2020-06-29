using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Locations.Services
{
   public class UrlData
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string code { get; set; }
        public string msg { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public Data data { get; set; }
       
    }

    public class Data
    {
        public string url { get; set; }
    }
}
