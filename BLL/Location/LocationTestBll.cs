using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.BLL.Tool;
using Location.Model;
using Location.Model.InitInfos;
using Location.Model.LocationTables;

namespace Location.BLL
{
    public class LocationTestBll
    {
        public TestBll db = new TestBll();

        public void ClearTopoTable()
        {
            db.ClearTopoTable();
        }

        public void InitTopo()
        {
            db.InitPhysicalTopologys();
        }

        public void InitTopo(TopoInfo topoInfo)
        {
            db.InitTopo(topoInfo);
        }

        public void InitTopoFromXml()
        {
            db.InitTopoFromXml();
        }

        public void InitDbData(int mode, bool isForce = false)
        {

            db.InitDbData(mode, isForce);
        }
    }
}
