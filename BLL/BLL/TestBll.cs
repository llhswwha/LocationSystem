using Location.Model.InitInfos;

namespace BLL
{
    public class TestBll
    {
        public Bll db = new Bll();

        public void ClearTopoTable()
        {
            var initializer = new DbInitializerAreaTree(db);
            initializer.ClearTopoTable();
        }

        public void InitTopo()
        {
            var initializer = new DbInitializerAreaTree(db);
            initializer.InitAreaAndDev();
        }

        public void InitTopo(TopoInfo topoInfo)
        {
            var initializer = new DbInitializerAreaTree(db);
            initializer.InitTopo(topoInfo);
        }

        public void InitTopoFromXml()
        {
            var initializer = new DbInitializerAreaTree(db);
            initializer.InitTopoFromXml();
        }

        public void InitDbData(int mode, bool isForce = false)
        {
            db.InitDbData(mode, isForce);
        }
    }
}
