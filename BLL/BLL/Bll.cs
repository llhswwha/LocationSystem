using BLL.Blls.Engine;
using BLL.Blls.Location;
using BLL.Blls.LocationHistory;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.BLL.Tool;

namespace BLL
{
    public partial class Bll : IDisposable
    {
        private static Bll Singleton;

        public LocationDb Db;

        public LocationHistoryDb DbHistory = new LocationHistoryDb();

        public EngineDb DbE = new EngineDb();

        /*******************Location****************************************/

        public ArchorBll Archors { get; set; }

        public AreaBll Areas { get; set; }

        public BoundBll Bounds { get; set; }

        //public ShapeBll Shapes { get; set; }

        public ConfigArgBll ConfigArgs { get; set; }

        public DepartmentBll Departments { get; set; }

        public DevAlarmBll DevAlarms { get; set; }

        public DevInfoBll DevInfos { get; set; }

        public Dev_DoorAccessBll Dev_DoorAccess { get; set; }

        public Dev_CameraInfoBll Dev_CameraInfos { get; set; }

        public DevInstantDataBll DevInstantDatas { get; set; }

        public DevModelBll DevModels { get; set; }

        public DevTypeBll DevTypes { get; set; }

        public EntranceGuardCardBll EntranceGuardCards { get; set; }

        public EntranceGuardCardToPersonnelBll EntranceGuardCardToPersonnels { get; set; }

        public AreaAuthorizationBll AreaAuthorizations { get; set; }

        public AreaAuthorizationRecordBll AreaAuthorizationRecords { get; set; }

        public KKSCodeBll KKSCodes { get; set; }

        public LocationAlarmBll LocationAlarms { get; set; }

        public LocationCardBll LocationCards { get; set; }

        public LocationCardPositionBll LocationCardPositions { get; set; }

        public LocationCardToPersonnelBll LocationCardToPersonnels { get; set; }

        public MobileInspectionBll MobileInspections { get; set; }

        public MobileInspectionContentBll MobileInspectionContents { get; set; }

        public MobileInspectionDevBll MobileInspectionDevs { get; set; }

        public MobileInspectionItemBll MobileInspectionItems { get; set; }

        public NodeKKSBll NodeKKSs { get; set; }

        public OperationItemBll OperationItems { get; set; }

        public OperationTicketBll OperationTickets { get; set; }

        public PersonnelBll Personnels { get; set; }

        public PersonnelMobileInspectionBll PersonnelMobileInspections { get; set; }

        public PersonnelMobileInspectionItemBll PersonnelMobileInspectionItems { get; set; }

        public PointBll Points { get; set; }

        //public ShapePointBll ShapePoints { get; set; }

        public PostBll Posts { get; set; }

        public RoleBll Roles { get; set; }

        public SafetyMeasuresBll SafetyMeasuress { get; set; }

        public WorkTicketBll WorkTickets { get; set; }

        public PictureBll Pictures { get; set; }

        public ArchorSettingBll ArchorSettings { get; set; }

        public CardRoleBll CardRoles { get; set; }


        /********************LocationHistory********************************/

        public DevAlarmHistoryBll DevAlarmHistorys { get; set; }

        public DevEntranceGuardCardActionBll DevEntranceGuardCardActions { get; set; }

        public DevInfoHistoryBll DevInfoHistorys { get; set; }

        public DevInstantDataHistoryBll DevInstantDataHistorys { get; set; }

        public EntranceGuardCardHistoryBll EntranceGuardCardHistorys { get; set; }

        public EntranceGuardCardToPersonnelHistoryBll EntranceGuardCardToPersonnelHistorys { get; set; }

        public LocationAlarmHistoryBll LocationAlarmHistorys { get; set; }

        public LocationCardHistoryBll LocationCardHistorys { get; set; }

        public LocationCardToPersonnelHistoryBll LocationCardToPersonnelHistorys { get; set; }

        public OperationItemHistoryBll OperationItemHistorys { get; set; }

        public OperationTicketHistoryBll OperationTicketHistorys { get; set; }

        public PersonnelHistoryBll PersonnelHistorys { get; set; }

        public PersonnelMobileInspectionHistoryBll PersonnelMobileInspectionHistorys { get; set; }

        public PersonnelMobileInspectionItemHistoryBll PersonnelMobileInspectionItemHistorys { get; set; }

        public PositionBll Positions { get; set; }

        public SafetyMeasuresHistoryBll SafetyMeasuresHistorys { get; set; }

        public U3DPositionBll U3DPositions { get; set; }

        public WorkTicketHistoryBll WorkTicketHistorys { get; set; }


        /********************Engine********************************/
        public bus_anchorBll bus_anchors { get; set; }

        public bus_tagBll bus_tags { get; set; }

        public Bll() : this(false, true, true,true)
        {

        }

        public static bool ShowLog { get; set; }

        public Bll(bool autoDetectChangesEnabled, bool lazyLoadingEnabled, bool isCreateDb,bool useProxy=true)
        {
            Db = new LocationDb(isCreateDb);
            Db.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
            Db.Configuration.LazyLoadingEnabled = lazyLoadingEnabled; //关闭延迟加载
            Db.Configuration.ProxyCreationEnabled = useProxy;

            if(ShowLog)
                Db.Database.Log = Log.Debug;


            Archors = new ArchorBll(Db);
            Areas = new AreaBll(Db);
            Bounds = new BoundBll(Db);
            ConfigArgs = new ConfigArgBll(Db);
            Departments = new DepartmentBll(Db);
            DevAlarms = new DevAlarmBll(Db);
            DevInfos = new DevInfoBll(Db);
            Dev_DoorAccess = new Dev_DoorAccessBll(Db);
            Dev_CameraInfos = new Dev_CameraInfoBll(Db);
            DevInstantDatas = new DevInstantDataBll(Db);
            DevModels = new DevModelBll(Db);
            DevTypes = new DevTypeBll(Db);
            EntranceGuardCards = new EntranceGuardCardBll(Db);
            EntranceGuardCardToPersonnels = new EntranceGuardCardToPersonnelBll(Db);
            AreaAuthorizations = new AreaAuthorizationBll(Db);
            AreaAuthorizationRecords = new AreaAuthorizationRecordBll(Db);
            KKSCodes = new KKSCodeBll(Db);
            LocationAlarms = new LocationAlarmBll(Db);
            LocationCards = new LocationCardBll(Db);
            LocationCardPositions = new LocationCardPositionBll(Db);
            LocationCardToPersonnels = new LocationCardToPersonnelBll(Db);
            MobileInspections = new MobileInspectionBll(Db);
            MobileInspectionContents = new MobileInspectionContentBll(Db);
            MobileInspectionDevs = new MobileInspectionDevBll(Db);
            MobileInspectionItems = new MobileInspectionItemBll(Db);
            NodeKKSs = new NodeKKSBll(Db);
            OperationItems = new OperationItemBll(Db);
            OperationTickets = new OperationTicketBll(Db);
            Personnels = new PersonnelBll(Db);
            PersonnelMobileInspections = new PersonnelMobileInspectionBll(Db);
            PersonnelMobileInspectionItems = new PersonnelMobileInspectionItemBll(Db);
            Points = new PointBll(Db);
            Posts = new PostBll(Db);
            Roles = new RoleBll(Db);
            SafetyMeasuress = new SafetyMeasuresBll(Db);
            WorkTickets = new WorkTicketBll(Db);
            Pictures = new PictureBll(Db);
            ArchorSettings = new ArchorSettingBll(Db);
            CardRoles = new CardRoleBll(Db);
            //Shapes = new ShapeBll(Db);
            //ShapePoints = new ShapePointBll();

            DevAlarmHistorys = new DevAlarmHistoryBll(DbHistory);
            DevEntranceGuardCardActions = new DevEntranceGuardCardActionBll(DbHistory);
            DevInfoHistorys = new DevInfoHistoryBll(DbHistory);
            DevInstantDataHistorys = new DevInstantDataHistoryBll(DbHistory);
            EntranceGuardCardHistorys = new EntranceGuardCardHistoryBll(DbHistory);
            EntranceGuardCardToPersonnelHistorys = new EntranceGuardCardToPersonnelHistoryBll(DbHistory);
            LocationAlarmHistorys = new LocationAlarmHistoryBll(DbHistory);
            LocationCardHistorys = new LocationCardHistoryBll(DbHistory);
            LocationCardToPersonnelHistorys = new LocationCardToPersonnelHistoryBll(DbHistory);
            OperationItemHistorys = new OperationItemHistoryBll(DbHistory);
            OperationTicketHistorys = new OperationTicketHistoryBll(DbHistory);
            PersonnelHistorys = new PersonnelHistoryBll(DbHistory);
            PersonnelMobileInspectionHistorys = new PersonnelMobileInspectionHistoryBll(DbHistory);
            PersonnelMobileInspectionItemHistorys = new PersonnelMobileInspectionItemHistoryBll(DbHistory);
            Positions = new PositionBll(DbHistory);
            SafetyMeasuresHistorys = new SafetyMeasuresHistoryBll(DbHistory);
            U3DPositions = new U3DPositionBll(DbHistory);
            WorkTicketHistorys = new WorkTicketHistoryBll(DbHistory);


            bus_anchors = new bus_anchorBll(DbE);
            bus_tags = new bus_tagBll(DbE);


            //LocationCards.ToList();
            //DevEntranceGuardCardActions.ToList();
            //bus_archors.ToList();

            Z.EntityFramework.Extensions.LicenseManager.AddLicense("34;100-LLHSWWHA", "384799A60700037CBFC0EB5E03A62474");
        }


        public static Bll Instance()
        {
            if (Singleton == null)
            {
                Singleton = new Bll();
            }

            return Singleton;
        }

        public void Dispose()
        {
            Db.Dispose();
            DbHistory.Dispose();
        }
    }
}
