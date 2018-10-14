using DAL;
using DbModel.Location.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class DevInfoBll : BaseBll<DevInfo, LocationDb>
    {
        public DevInfoBll():base()
        {

        }
        public DevInfoBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.DevInfos;
        }

        public List<DevInfo> FindListByPid(int pid)
        {
            return DbSet.Where(i => i.ParentId == pid).ToList();
        }

        public List<DevInfo> GetListByName(string name)
        {
            return DbSet.Where(i => i.Name.Contains(name)).ToList();
        }
    }
}
