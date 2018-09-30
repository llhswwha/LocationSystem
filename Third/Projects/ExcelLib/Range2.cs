using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelLib
{
    public class Range2
    {
        public int Start { get; set; }
        public int End { get; set; }

        public Range2()
        {
            
        }

        public Range2(int s, int e)
        {
            Start = s;
            End = e;
        }
    }
}
