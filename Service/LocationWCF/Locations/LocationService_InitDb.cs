using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Locations
{
    /// <summary>
    /// 通过接口单独初始化某张表
    /// </summary>
    public partial class LocationService : ILocationService, IDisposable
    {
        public void InitKksTable()
        {
            Bll bll = Bll.NewBllNoRelation();
            DbInitializer initializer = new DbInitializer(bll);
            bll.Db.Database.ExecuteSqlCommand("TRUNCATE TABLE kkscodes");
            initializer.InitKKSCode();
        }
    }
}
