using System.Data.Entity;
using Location.DAL.Migrations;
using Location.Model;
using Location.Model.Base;
using Location.Model.Locations;
using Location.Model.Work;

namespace Location.DAL
{
    public class LocationDb : DbContext
    {
        public DbSet<Map> Map { get; set; }
        public LocationDb() : base("LocationConnection")
        {
            //Database.SetInitializer<LocationDb>(new DropCreateDatabaseIfModelChanges<LocationDb>());
            //this.Configuration.ProxyCreationEnabled = false;

            //Database.SetInitializer<LocationDb>(null);
            Database.SetInitializer<LocationDb>(new MigrateDatabaseToLatestVersion<LocationDb, Configuration>());//自动数据迁移
        }

        public LocationDb(bool isCreateDb) : base("LocationConnection")
        {
            if (isCreateDb)
            {
                //Database.SetInitializer<LocationDb>(new DropCreateDatabaseIfModelChanges<LocationDb>());//数据模型发生变化是重新创建数据库
                Database.SetInitializer<LocationDb>(new MigrateDatabaseToLatestVersion<LocationDb, Configuration>());//自动数据迁移
            }
            else
            {
                Database.SetInitializer<LocationDb>(null);
            }
        }

        public System.Data.Entity.DbSet<Area> Areas { get; set; }


        public DbSet<TagPosition> TagPosition { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Archor> Archors { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Tag> Tags { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Target> Targets { get; set; }

        public System.Data.Entity.DbSet<Location.Model.LocationAlarm> Alarms { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<Location.Model.User> Users { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Role> Roles { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Menu> Menus { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Manage.KKSCode> KKSCodes { get; set; }

        public System.Data.Entity.DbSet<Location.Model.TransformM> TransformMs { get; set; }

        public DbSet<Model.LocationTables.DevInfo> DevInfos { get; set; }

        public DbSet<Model.LocationTables.DevPos> DevPos{ get; set; }

        public System.Data.Entity.DbSet<Location.Model.PhysicalTopology> PhysicalTopologys { get; set; }

        public System.Data.Entity.DbSet<Location.Model.NodeKKS> NodeKKSs { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Personnel> Personnels { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Post> Posts { get; set; }

        public System.Data.Entity.DbSet<Location.Model.JurisDiction> JurisDictions { get; set; }

        public System.Data.Entity.DbSet<Location.Model.JurisDictionRecord> JurisDictionRecords { get; set; }

        public DbSet<Model.topviewxp.t_SetModel> t_SetModelS { get; set; }

        public DbSet<Model.topviewxp.t_Template_TypeProperty> t_Template_TypeProperties { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Meterial> Meterials { get; set; }

        public System.Data.Entity.DbSet<ConfigArg> ConfigArgs { get; set; }

        public System.Data.Entity.DbSet<Bound> Bounds { get; set; }

        public System.Data.Entity.DbSet<Point> Points { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.OperationTicket> OperationTickets { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.OperationItem> OperationItems { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.WorkTicket> WorkTickets { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.SafetyMeasures> SafetyMeasuress { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.MobileInspection> MobileInspections { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.MobileInspectionItem> MobileInspectionItems { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.MobileInspectionDev> MobileInspectionDevs { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.MobileInspectionContent> MobileInspectionContents { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.PersonnelMobileInspection> PersonnelMobileInspections { get; set; }

        public System.Data.Entity.DbSet<Location.Model.Work.PersonnelMobileInspectionItem> PersonnelMobileInspectionItems { get; set; }


    }
}