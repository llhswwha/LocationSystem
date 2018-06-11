using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{
    /// <summary>
    /// 标签 即（定位卡）
    /// </summary>
    public class Tag
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Describe { get; set; }
    }
}
