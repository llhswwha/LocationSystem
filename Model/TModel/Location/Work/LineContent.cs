using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.Location.Work
{
    //返回的键值对
   public class LineContent
    {
       public List<KeyValue> Content { get; set; }
    }

    public class KeyValue
    {
        public string key { get; set; }
        public string value { get; set; }

    }
}
