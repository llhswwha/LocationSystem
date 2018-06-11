using System.Data.Entity;
using Location.Model;

namespace Location.DAL
{
    public class LocationDb:DbContext
    {
        public DbSet<Map> Map { get; set; }
        public LocationDb():base("LocationConnection")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<LocationDb>());//数据模型发生变化是重新创建数据库
        }

        public System.Data.Entity.DbSet<Area> Areas { get; set; }

        public DbSet<Position> Position { get; set; }

        public DbSet<TagPosition> TagPosition { get; set; }
    }
}