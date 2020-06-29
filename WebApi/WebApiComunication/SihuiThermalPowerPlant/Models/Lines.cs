using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
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
        public string name { get; set; }

        public List<LineContent> lineContentList { get; set; }

        public LinesSet(string name, List<LineContent> lineContentList)
        {
            this.name = name;
            this.lineContentList = lineContentList;
        }

        public LinesSet() { }
    }

}
