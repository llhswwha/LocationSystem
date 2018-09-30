using Location.Model.InitInfos;

namespace BLL
{
    public class TestBll
    {
        public Bll db = new Bll();

        public void ClearTopoTable()
        {
            db.ClearTopoTable();
        }

        public void InitTopo()
        {
            db.InitAreas();
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
