using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;

namespace LocationServer
{
    public static class AppContext
    {
        public static Bll GetLocationBll()
        {
            return new Bll(false, true, false);
        }
    }
}
