using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.ObjToData
{
   public  class SisHistoryByUrl
    {
        // "\"SelectedTags\":[\"30HBK20CP305\",\"30HBK20CT002\",\"30HBK20CT004\"],\"StartTime\":\"2020 - 05 - 19 20:03:47\",\"EndTime\":\"2020 - 05 - 19 21:03:47\",\"Condition\":{\"IsRowData\":false,\"TimeSpace\":1,\"TimeUnit\":1,\"ValueMode\":1}";
        //该接口查询条件:
        /*  SelectedTags   测点数组
            StartTime  起始时间
            EndTime    结束时间
            Condition {IsRowData 不用管,  TimeSpace 取值间隔(0 5 10 15 20 25 30)秒，后面的不用管 }
             */
        public int Code { get; set; }
        public string Msg { get; set; }
        public List<TagData> Data { get; set; }
    }
    public class TagData
    {
        public string TagName { get; set; }
        public int Sort { get; set; }
        public bool IsRowData { get; set; }
        public object[] Values { get; set; }
    }

    public class TagValues
    {
        public object dateTime { get; set; }
        public string value { get; set; }
    }
}
