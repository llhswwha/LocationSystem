using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Location.DAL;
using Location.Model;

namespace LocationWCFServices
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“LocationService”。
    public class LocationService : ILocationService
    {
        LocationDb db = new LocationDb();
        public void DoWork()
        {
        }

        public List<Area> GetAreas()
        {
            try
            {
                List<Area> areas = db.Areas.ToList();
                return areas;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Map> GetMaps()
        {
            try
            {
                List<Map> maps = db.Map.ToList();
                return maps;
            }
            catch (Exception ex)
            {
                return null ;
            }
        }

        public User GetUser()
        {
            return new User() { Id = 1, Name = "Name" };
        }

        public string Hello()
        {
            return "Hello";
        }
    }
}
