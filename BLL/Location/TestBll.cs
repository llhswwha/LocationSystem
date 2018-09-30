using Location.BLL.Blls;
using Location.BLL.topviewxpBlls;
using Location.DAL;
using Location.Model;
using Location.Model.topviewxp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using Location.Model.LocationTables;

namespace Location.BLL
{
    public partial class TestBll : IDisposable
    {
        private static LocationBll Singleton;

        public LocationDb Db;

        public LocationHistoryDb DbHistory = new LocationHistoryDb();

        public topviewxpDb Dbtopviewxp = new topviewxpDb();

        public MapBll Maps { get; set; }

        public AreaBll Areas { get; set; }

        public PositionBll Position { get; set; }

        public TagPositionBll TagPositions { get; set; }

        public DepartmentBll Departments { get; set; }

        public UserBll Users { get; set; }
        
        public TagBll Tags { get; set; }

        public ArchorBll Archors { get; set; }

        public KKSCodeBll KKSCodes { get; set; }

        public t_KKSCodeBll t_KKSCodes { get; set; }

        public t_SetModelBll t_SetModels { get; set; }

        public t_Template_TypePropertyBll t_TypeProperties { get; set; }

        public TransformMBll TransformMs { get; set; }

        public DevInfoBll DevInfos { get; set; }

        public DevPosBll DevPos { get; set; }

        public static LocationBll Instance()
        {
            if (Singleton == null)
            {
                Singleton = new LocationBll();
            }

            return Singleton;
        }
        public PhysicalTopologyBll PhysicalTopologys { get; set; }

        public NodeKKSBll NodeKKSs { get; set; }

        public PersonnelBll Personnels { get; set; }

        public PostBll Posts { get; set; }
        //public static LocationBll Instance()
        //{
        //    if (Singleton == null)
        //    {
        //        Singleton = new LocationBll();
        //    }

        //    return Singleton;
        //}

        public U3DPositionBll U3DPositions { get; set; }

        public JurisDictionBll JurisDictions { get; set; }

        public JurisDictionRecordBll JurisDictionRecords { get; set; }

        public MeterialBll Meterials { get; set; }

        public ConfigArgBll ConfigArgs { get; set; }

        public BoundBll Bounds { get; set; }

        public PointBll Points { get; set; }

        public OperationTicketBll OperationTickets { get; set; }

        public OperationItemBll OperationItems { get; set; }

        public WorkTicketBll WorkTickets { get; set; }

        public SafetyMeasuresBll SafetyMeasuress { get; set; }

        public MobileInspectionBll MobileInspections { get; set; }

        public MobileInspectionItemBll MobileInspectionItems { get; set; }

        public MobileInspectionContentBll MobileInspectionContents { get; set; }

        public MobileInspectionDevBll MobileInspectionDevs { get; set; }

        public PersonnelMobileInspectionBll PersonnelMobileInspections { get; set; }

        public PersonnelMobileInspectionItemBll PersonnelMobileInspectionItems { get; set; }

        public PersonnelMobileInspectionHistoryBll PersonnelMobileInspectionHistorys { get; set; }

        public PersonnelMobileInspectionItemHistoryBll PersonnelMobileInspectionItemHistorys { get; set; }

        public OperationTicketHistoryBll OperationTicketHistorys { get; set; }

        public OperationItemHistoryBll OperationItemHistorys { get; set; }

        public WorkTicketHistoryBll WorkTicketHistorys { get; set; }

        public SafetyMeasuresHistoryBll SafetyMeasuresHistorys { get; set; }

        public TargetBll Targets { get; set; }

        public RoleBll Roles { get; set; }

        public MenuBll Menus { get; set; }

        public TestBll() : this(false, true,true)
        {

        }

        public TestBll(bool autoDetectChangesEnabled,bool lazyLoadingEnabled,bool isCreateDb)
        {
            Db = new LocationDb(isCreateDb);
            Db.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
            Db.Configuration.LazyLoadingEnabled = lazyLoadingEnabled; //关闭延迟加载

            Maps = new MapBll(Db);
            Areas = new AreaBll(Db);
            Position = new PositionBll(DbHistory);
            TagPositions = new TagPositionBll(Db);
            Departments = new DepartmentBll(Db);
            Users = new UserBll(Db);
            Tags = new TagBll(Db);
            Archors = new ArchorBll(Db);
            KKSCodes = new KKSCodeBll(Db);
            t_KKSCodes = new t_KKSCodeBll(Dbtopviewxp);
            t_SetModels = new t_SetModelBll(Db);
            t_TypeProperties = new t_Template_TypePropertyBll(Db);
            TransformMs = new TransformMBll(Db);
            DevInfos = new DevInfoBll(Db);
            DevPos = new DevPosBll(Db);
            U3DPositions = new U3DPositionBll(DbHistory);
            PhysicalTopologys = new PhysicalTopologyBll(Db);
            NodeKKSs = new NodeKKSBll(Db);
            Personnels = new PersonnelBll(Db);
            Posts = new PostBll(Db);
            JurisDictions = new JurisDictionBll(Db);
            JurisDictionRecords = new JurisDictionRecordBll(Db);
            Meterials = new MeterialBll(Db);
            //U3DPositions.ToList();
            ConfigArgs = new ConfigArgBll(Db);
            Bounds = new BoundBll(Db);
            Points = new PointBll(Db);
            OperationTickets = new OperationTicketBll(Db);
            OperationItems = new OperationItemBll(Db);
            WorkTickets = new WorkTicketBll(Db);
            SafetyMeasuress = new SafetyMeasuresBll(Db);
            MobileInspections = new MobileInspectionBll(Db);
            MobileInspectionItems = new MobileInspectionItemBll(Db);
            MobileInspectionContents = new MobileInspectionContentBll(Db);
            MobileInspectionDevs = new MobileInspectionDevBll(Db);
            PersonnelMobileInspections = new PersonnelMobileInspectionBll(Db);
            PersonnelMobileInspectionItems = new PersonnelMobileInspectionItemBll(Db);
            PersonnelMobileInspectionHistorys = new PersonnelMobileInspectionHistoryBll(DbHistory);
            PersonnelMobileInspectionItemHistorys = new PersonnelMobileInspectionItemHistoryBll(DbHistory);
            OperationTicketHistorys = new OperationTicketHistoryBll(DbHistory);
            OperationItemHistorys = new OperationItemHistoryBll(DbHistory);
            WorkTicketHistorys = new WorkTicketHistoryBll(DbHistory);
            SafetyMeasuresHistorys = new SafetyMeasuresHistoryBll(DbHistory);
            Targets = new TargetBll(Db);
            Roles = new RoleBll(Db);
            Menus = new MenuBll(Db);

            Z.EntityFramework.Extensions.LicenseManager.AddLicense("34;100-LLHSWWHA", "384799A60700037CBFC0EB5E03A62474");
        }

        public void Dispose()
        {
            Db.Dispose();
            DbHistory.Dispose();
            Dbtopviewxp.Dispose();
        }
    }
}
