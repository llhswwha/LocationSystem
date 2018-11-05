using Location.Model.InitInfos;

namespace BLL
{
    public class TestBll
    {
        public Bll db = new Bll();

        public void ClearTopoTable()
        {
            var initializer = new AreaTreeInitializer(db);
            initializer.ClearTopoTable();
        }

        public void InitTopo()
        {
            var initializer = new AreaTreeInitializer(db);
            initializer.InitAreaAndDev();
        }

        public void InitTopo(TopoInfo topoInfo)
        {
            var initializer = new AreaTreeInitializer(db);
            initializer.InitTopo(topoInfo);
        }

        public void InitTopoFromXml()
        {
            var initializer = new AreaTreeInitializer(db);
            initializer.InitTopoFromXml();
        }

        public void InitDbData(int mode, bool isForce = false)
        {
            db.InitDbData(mode, isForce);
        }
    }
}
