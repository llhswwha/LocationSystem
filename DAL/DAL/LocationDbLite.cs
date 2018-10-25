using System.Collections.Generic;
using System.Data.Entity;
using DbModel.Location.Person;
using SQLite.CodeFirst;
namespace DAL
{
    public class LocationDbLite : DbContext
    {
        public static string Name = "LocationLite";

        public LocationDbLite() : base(Name)
        {
            Configure();
        }

        private void Configure()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Book> Books { get; set; }

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var initializer = new SqliteDropCreateDatabaseWhenModelChanges<LocationDbLite>(modelBuilder);
            Database.SetInitializer(initializer);
        }
    }

    public class Book
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
    }
}
