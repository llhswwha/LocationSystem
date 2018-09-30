using System.Data.Entity;

namespace DAL
{
    public class LocationHistoryDb : DbContext
    {
        public LocationHistoryDb():base("LocationHistoryConnection")
        {
            Database.SetInitializer<LocationHistoryDb>(new DropCreateDatabaseIfModelChanges<LocationHistoryDb>());//数据模型发生变化是重新创建数据库
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

    }
}
