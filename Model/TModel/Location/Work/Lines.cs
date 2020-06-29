using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.Location.Work
{
  public  class LinesGet
    {
        public string name { get; set; }

        public List<Dictionary<string, string>> lineContentList { get; set; }

        public LinesGet(string name, List<Dictionary<string, string>> lineContentList)
        {
            this.name = name;
            this.lineContentList = lineContentList;
        }

    }

    public class LinesSet
    {
        public string name { get; set; }//LIST:执行条目 DO:执行情况  REMARKS:备注 ACTION:审批历史记录  DANGERS:危险点预控卡

        public List<LineContent> lineContentList { get; set; }

        public LinesSet(string name, List<LineContent> lineContentList)
        {
            this.name = name;
            this.lineContentList = lineContentList;
        }

        public LinesSet() { }
    }

}
