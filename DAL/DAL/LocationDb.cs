using System.Data.Entity;
using DbModel.Location.Person;
using DAL.Migrations;
using DbModel.Location.Settings;
using SQLite.CodeFirst;

namespace DAL
{
    public class LocationDb : DbContext
    {
        public static bool IsSqlite = false;

        public static string Name = "LocationConnection";

        public LocationDb() : base(Name)
        {
            IsCreateDb = true;
        }

        public LocationDb(bool isCreateDb) : base(Name)
        {
            IsCreateDb = isCreateDb;
        }

        public bool IsCreateDb { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            if (IsSqlite)
            {
                Database.SetInitializer(new SqliteDropCreateDatabaseWhenModelChanges<LocationDb>(modelBuilder));
            }
            else
            {
                if (IsCreateDb)
                {
                    //Database.SetInitializer<LocationDb>(new DropCreateDatabaseIfModelChanges<LocationDb>());//数据模型发生变化是重新创建数据库
                    Database.SetInitializer<LocationDb>(new MigrateDatabaseToLatestVersion<LocationDb, Configuration>());//自动数据迁移
                }
                else
                {
                    Database.SetInitializer<LocationDb>(null);
                }
            }
        }

        public DbSet<DbModel.Location.Alarm.DevAlarm> DevAlarms { get; set; }

        public DbSet<DbModel.Location.Alarm.LocationAlarm> LocationAlarms { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.Archor> Archors { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.Area> Areas { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.Bound> Bounds { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.ConfigArg> ConfigArgs { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.DevInfo> DevInfos { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.DevModel> DevModels { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.DevType> DevTypes { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.EntranceGuardCard> EntranceGuardCards { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.KKSCode> KKSCodes { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.LocationCard> LocationCards { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.NodeKKS> NodeKKSs { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.Point> Points { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.Post> Posts { get; set; }

        public DbSet<DbModel.Location.Data.DevInstantData> DevInstantDatas { get; set; }

        public DbSet<DbModel.Location.Data.LocationCardPosition> LocationCardPositions { get; set; }
        
        public DbSet<DbModel.Location.Person.Personnel> Personnels { get; set; }

        public DbSet<DbModel.Location.Person.Role> Roles { get; set; }

        public DbSet<DbModel.Location.Relation.EntranceGuardCardToPersonnel> EntranceGuardCardToPersonnels { get; set; }

        public DbSet<DbModel.Location.Relation.LocationCardToPersonnel> LocationCardToPersonnels { get; set; }

        public DbSet<DbModel.Location.Work.JurisDiction> JurisDictions { get; set; }

        public DbSet<DbModel.Location.Work.JurisDictionRecord> JurisDictionRecords { get; set; }

        public DbSet<DbModel.Location.Work.MobileInspection> MobileInspections { get; set; }

        public DbSet<DbModel.Location.Work.MobileInspectionContent> MobileInspectionContents { get; set; }

        public DbSet<DbModel.Location.Work.MobileInspectionDev> MobileInspectionDevs { get; set; }

        public DbSet<DbModel.Location.Work.MobileInspectionItem> MobileInspectionItems { get; set; }

        public DbSet<DbModel.Location.Work.OperationItem> OperationItems { get; set; }

        public DbSet<DbModel.Location.Work.OperationTicket> OperationTickets { get; set; }

        public DbSet<DbModel.Location.Work.PersonnelMobileInspection> PersonnelMobileInspections { get; set; }

        public DbSet<DbModel.Location.Work.PersonnelMobileInspectionItem> PersonnelMobileInspectionItems { get; set; }

        public DbSet<DbModel.Location.Work.SafetyMeasures> SafetyMeasuress { get; set; }

        public DbSet<DbModel.Location.Work.WorkTicket> WorkTickets { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.Dev_DoorAccess> Dev_DoorAccess { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.Picture> Pictures { get; set; }

        public DbSet<ArchorSetting> ArchorSettings { get; set; }
    }
}
