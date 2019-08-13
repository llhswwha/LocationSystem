using System.Data.Entity;
using SQLite.CodeFirst;
using DbModel.LocationHistory.Work;
using DbModel.LocationHistory.AreaAndDev;

namespace DAL
{
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    //发现nuget有安装MySql.Data.Entity(6.9.2)的话就需要这个DbConfigurationType了
    public class LocationHistoryDb : DbContext
    {
        public static bool IsSqlite = false;

        public static string Name = "LocationHistory_MySql";

        public LocationHistoryDb():base(Name)
        {
           
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<LocationHistoryDb>(null);

            //if (IsSqlite)
            //{
            //    Database.SetInitializer(new SqliteDropCreateDatabaseWhenModelChanges<LocationHistoryDb>(modelBuilder));
            //}
            //else
            //{
            //    //Database.SetInitializer<LocationHistoryDb>(new DropCreateDatabaseIfModelChanges<LocationHistoryDb>());//数据模型发生变化是重新创建数据库
            //    //Database.SetInitializer<LocationHistoryDb>(new MigrateDatabaseToLatestVersion<LocationHistoryDb, DAL.LocationHistoryDbMigrations.Configuration>());//自动数据迁移
            //}
        }

        public DbSet<DbModel.LocationHistory.Alarm.DevAlarmHistory> DevAlarmHistorys { get; set; }

        public DbSet<DbModel.LocationHistory.Alarm.LocationAlarmHistory> LocationAlarmHistorys { get; set; }

        public DbSet<DbModel.LocationHistory.AreaAndDev.DevEntranceGuardCardAction> DevEntranceGuardCardActions { get; set; }

        public DbSet<DbModel.LocationHistory.AreaAndDev.DevInfoHistory> DevInfoHistorys { get; set; }

        public DbSet<DbModel.LocationHistory.AreaAndDev.EntranceGuardCardHistory> EntranceGuardCardHistorys { get; set; }

        public DbSet<DbModel.LocationHistory.AreaAndDev.LocationCardHistory> LocationCardHistorys { get; set; }

        public DbSet<DbModel.LocationHistory.Data.DevInstantDataHistory> DevInstantDataHistorys { get; set; }

        public DbSet<DbModel.LocationHistory.Data.Position> Positions { get; set; }

        public DbSet<DbModel.LocationHistory.Data.U3DPosition> U3DPositions { get; set; }

        public DbSet<DbModel.LocationHistory.Person.PersonnelHistory> PersonnelHistorys { get; set; }
        
        public DbSet<DbModel.LocationHistory.Relation.EntranceGuardCardToPersonnelHistory> EntranceGuardCardToPersonnelHistorys { get; set; }

        public DbSet<DbModel.LocationHistory.Relation.LocationCardToPersonnelHistory> LocationCardToPersonnelHistorys { get; set; }
        
        public DbSet<DbModel.LocationHistory.Work.OperationItemHistory> OperationItemHistorys { get; set; }

        public DbSet<DbModel.LocationHistory.Work.OperationTicketHistory> OperationTicketHistorys { get; set; }

        public DbSet<DbModel.LocationHistory.Work.PersonnelMobileInspectionHistory> PersonnelMobileInspectionHistorys { get; set; }

        public DbSet<DbModel.LocationHistory.Work.PersonnelMobileInspectionItemHistory> PersonnelMobileInspectionItemHistorys { get; set; }

        public DbSet<DbModel.LocationHistory.Work.SafetyMeasuresHistory> SafetyMeasuresHistorys { get; set; }

        public DbSet<DbModel.LocationHistory.Work.WorkTicketHistory> WorkTicketHistorys { get; set; }

        public DbSet<InspectionTrackHistory> InspectionTrackHistorys { get; set; }

        public DbSet<PatrolPointHistory> PatrolPointHistorys { get; set; }

        public DbSet<PatrolPointItemHistory> PatrolPointItemHistorys { get; set; }

        public DbSet<DevMonitorNodeHistory> DevMonitorNodeHistorys { get; set; }
        
    }
}
