using System.Data.Entity;
using DbModel.Location.Person;
using DbModel.Location.Authorizations;
using DbModel.Location.Settings;
using SQLite.CodeFirst;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Work;
using DbModel.Location.Manage;
using DbModel.Location.Alarm;
using Location.BLL.Tool;
using System.Diagnostics;

namespace DAL
{
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    //发现nuget有安装MySql.Data.Entity(6.9.2)的话就需要这个DbConfigurationType了
    public class LocationDb : DbContext
    {
        public static bool IsSqlite = false;

        public static string Name = "Location_MySql";

        public LocationDb() : base(Name)
        {
            IsCreateDb = true;

            //if (Debugger.IsAttached)
            //{
            //    this.Database.Log = s => Log.Info(LogTags.EF, s);//调试EF需要
            //}
            
        }

        //public LocationDb(bool isCreateDb) : base(Name)
        //{
        //    IsCreateDb = isCreateDb;
        //    this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        //}

        public bool IsCreateDb = true;//固定为true

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
#if BaoXin
            Database.SetInitializer<LocationDb>(null);
#else
            if (IsSqlite)
            {
                Database.SetInitializer(new SqliteDropCreateDatabaseWhenModelChanges<LocationDb>(modelBuilder));
            }
            else
            {
                if (IsCreateDb)
                {
                    Database.SetInitializer<LocationDb>(new DropCreateDatabaseIfModelChanges<LocationDb>());//数据模型发生变化是重新创建数据库
                    Database.SetInitializer<LocationDb>(new MigrateDatabaseToLatestVersion<LocationDb, DAL.LocationDbMigrations.Configuration>());//自动数据迁移
                    从代码来看，这里面会创建LocationDb对象的，
                }
                else
                {
                    Database.SetInitializer<LocationDb>(null);
                }
            }
#endif
        }

        public DbSet<DbModel.Location.Alarm.DevAlarm> DevAlarms { get; set; }

        public DbSet<DbModel.Location.Alarm.LocationAlarm> LocationAlarms { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.Archor> Archors { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.Area> Areas { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.Bound> Bounds { get; set; }

        //public DbSet<DbModel.Location.AreaAndDev.Bound> Shapes { get; set; }

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

        //public DbSet<DbModel.Location.AreaAndDev.Point> ShapePoints { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.Post> Posts { get; set; }

        public DbSet<DbModel.Location.Data.DevInstantData> DevInstantDatas { get; set; }

        public DbSet<DbModel.Location.Data.LocationCardPosition> LocationCardPositions { get; set; }
        
        public DbSet<DbModel.Location.Person.Personnel> Personnels { get; set; }

        public DbSet<DbModel.Location.Person.Role> Roles { get; set; }

        public DbSet<DbModel.Location.Relation.EntranceGuardCardToPersonnel> EntranceGuardCardToPersonnels { get; set; }

        public DbSet<DbModel.Location.Relation.LocationCardToPersonnel> LocationCardToPersonnels { get; set; }
            
        public DbSet<DbModel.Location.Work.AreaAuthorization> AreaAuthorizations { get; set; }

        public DbSet<DbModel.Location.Work.AreaAuthorizationRecord> AreaAuthorizationRecords { get; set; }

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

        public DbSet<DbModel.Location.AreaAndDev.Dev_CameraInfo> Dev_CameraInfos { get; set; }

        public DbSet<DbModel.Location.AreaAndDev.Picture> Pictures { get; set; }

        public DbSet<ArchorSetting> ArchorSettings { get; set; }

        public DbSet<CardRole> CardRoles { get; set; }

        public DbSet<DevMonitorNode> DevMonitorNodes { get; set; }

        public DbSet<InspectionTrack> InspectionTracks { get; set; }

        public DbSet<PatrolPoint> PatrolPoints { get; set; }

        public DbSet<PatrolPointItem> PatrolPointItems { get; set; }

        public DbSet<HomePagePicture> HomePagePictures { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<CameraAlarmJson> CameraAlarmJsonBll { get; set; }
    }
}
