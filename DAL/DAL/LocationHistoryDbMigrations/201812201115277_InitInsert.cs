namespace DAL.LocationHistoryDbMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitInsert : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DevAlarmHistories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Abutment_Id = c.Int(),
                        Title = c.String(maxLength: 64, storeType: "nvarchar"),
                        Msg = c.String(maxLength: 512, storeType: "nvarchar"),
                        Level = c.Int(),
                        Code = c.String(maxLength: 32, storeType: "nvarchar"),
                        Src = c.Int(nullable: false),
                        DevInfoId = c.Int(nullable: false),
                        Device_desc = c.String(maxLength: 128, storeType: "nvarchar"),
                        AlarmTime = c.DateTime(nullable: false, precision: 0),
                        AlarmTimeStamp = c.Long(nullable: false),
                        HistoryTime = c.DateTime(nullable: false, precision: 0),
                        HistoryTimeStamp = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DevEntranceGuardCardActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abutment_Id = c.Int(),
                        DevInfoId = c.Int(nullable: false),
                        EntranceGuardCardId = c.Int(nullable: false),
                        OperateTime = c.DateTime(precision: 0),
                        OperateTimeStamp = c.Long(),
                        code = c.Int(nullable: false),
                        nInOutState = c.Int(nullable: false),
                        description = c.String(maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DevInfoHistories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        ParentId = c.Int(),
                        Code = c.String(maxLength: 32, storeType: "nvarchar"),
                        KKS = c.String(maxLength: 32, storeType: "nvarchar"),
                        Local_DevID = c.String(maxLength: 64, storeType: "nvarchar"),
                        Local_CabinetID = c.String(maxLength: 64, storeType: "nvarchar"),
                        Local_TypeCode = c.Int(),
                        Abutment_Id = c.Int(),
                        Abutment_DevID = c.String(maxLength: 64, storeType: "nvarchar"),
                        Abutment_Type = c.Int(nullable: false),
                        Status = c.Int(),
                        RunStatus = c.Int(),
                        Placed = c.Boolean(),
                        ModelName = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        CreateTimeStamp = c.Long(nullable: false),
                        ModifyTime = c.DateTime(nullable: false, precision: 0),
                        ModifyTimeStamp = c.Long(nullable: false),
                        HistoryTime = c.DateTime(nullable: false, precision: 0),
                        HistoryTimeStamp = c.Long(nullable: false),
                        UserName = c.String(maxLength: 128, storeType: "nvarchar"),
                        IP = c.String(maxLength: 32, storeType: "nvarchar"),
                        PosX = c.Single(),
                        PosY = c.Single(),
                        PosZ = c.Single(),
                        RotationX = c.Single(),
                        RotationY = c.Single(),
                        RotationZ = c.Single(),
                        ScaleX = c.Single(),
                        ScaleY = c.Single(),
                        ScaleZ = c.Single(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DevInstantDataHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KKS = c.String(maxLength: 32, storeType: "nvarchar"),
                        Name = c.String(maxLength: 128, storeType: "nvarchar"),
                        Value = c.String(maxLength: 32, storeType: "nvarchar"),
                        DateTime = c.DateTime(nullable: false, precision: 0),
                        DateTimeStamp = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EntranceGuardCardHistories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Abutment_Id = c.Int(),
                        Name = c.String(maxLength: 128, storeType: "nvarchar"),
                        Code = c.String(maxLength: 64, storeType: "nvarchar"),
                        State = c.Int(nullable: false),
                        HistoryTime = c.DateTime(nullable: false, precision: 0),
                        HistoryTimeStamp = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EntranceGuardCardToPersonnelHistories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        EntranceGuardCardId = c.Int(nullable: false),
                        PersonnelId = c.Int(nullable: false),
                        HistoryTime = c.DateTime(nullable: false, precision: 0),
                        HistoryTimeStamp = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LocationAlarmHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AlarmId = c.String(maxLength: 40, storeType: "nvarchar"),
                        AlarmType = c.Int(nullable: false),
                        AlarmLevel = c.Int(nullable: false),
                        LocationCardId = c.Int(nullable: false),
                        PersonnelId = c.Int(nullable: false),
                        CardRoleId = c.Int(nullable: false),
                        AreadId = c.Int(),
                        AuzId = c.Int(),
                        AllAuzId = c.String(maxLength: 100, storeType: "nvarchar"),
                        Content = c.String(maxLength: 512, storeType: "nvarchar"),
                        AlarmTime = c.DateTime(nullable: false, precision: 0),
                        AlarmTimeStamp = c.Long(nullable: false),
                        HandleTime = c.DateTime(nullable: false, precision: 0),
                        HandleTimeStamp = c.Long(nullable: false),
                        Handler = c.String(maxLength: 128, storeType: "nvarchar"),
                        HandleType = c.Int(nullable: false),
                        HistoryTime = c.DateTime(nullable: false, precision: 0),
                        HistoryTimeStamp = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LocationCardHistories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Abutment_Id = c.Int(),
                        Code = c.String(nullable: false, maxLength: 16, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Describe = c.String(maxLength: 128, storeType: "nvarchar"),
                        HistoryTime = c.DateTime(nullable: false, precision: 0),
                        HistoryTimeStamp = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LocationCardToPersonnelHistories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        LocationCardId = c.Int(nullable: false),
                        PersonnelId = c.Int(nullable: false),
                        HistoryTime = c.DateTime(nullable: false, precision: 0),
                        HistoryTimeStamp = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OperationItemHistories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        TicketId = c.Int(),
                        OperationTime = c.DateTime(nullable: false, precision: 0),
                        Mark = c.Boolean(nullable: false),
                        OrderNum = c.Int(nullable: false),
                        Item = c.String(maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OperationTicketHistories", t => t.TicketId)
                .Index(t => t.TicketId);
            
            CreateTable(
                "dbo.OperationTicketHistories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        No = c.String(maxLength: 32, storeType: "nvarchar"),
                        OperationTask = c.String(maxLength: 128, storeType: "nvarchar"),
                        OperationStartTime = c.DateTime(nullable: false, precision: 0),
                        OperationEndTime = c.DateTime(nullable: false, precision: 0),
                        Guardian = c.String(maxLength: 128, storeType: "nvarchar"),
                        Operator = c.String(maxLength: 128, storeType: "nvarchar"),
                        DutyOfficer = c.String(maxLength: 128, storeType: "nvarchar"),
                        Dispatch = c.String(maxLength: 128, storeType: "nvarchar"),
                        Remark = c.String(maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonnelHistories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Abutment_Id = c.Int(),
                        Name = c.String(nullable: false, maxLength: 16, storeType: "nvarchar"),
                        Sex = c.Int(nullable: false),
                        Photo = c.String(maxLength: 128, storeType: "nvarchar"),
                        BirthDay = c.DateTime(precision: 0),
                        BirthTimeStamp = c.Long(),
                        Nation = c.String(maxLength: 64, storeType: "nvarchar"),
                        Address = c.String(maxLength: 512, storeType: "nvarchar"),
                        WorkNumber = c.Int(),
                        Email = c.String(maxLength: 64, storeType: "nvarchar"),
                        Phone = c.String(maxLength: 16, storeType: "nvarchar"),
                        Mobile = c.String(maxLength: 16, storeType: "nvarchar"),
                        Enabled = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                        Pst = c.String(maxLength: 16, storeType: "nvarchar"),
                        HistoryTime = c.DateTime(nullable: false, precision: 0),
                        HistoryTimeStamp = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonnelMobileInspectionHistories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PersonnelId = c.Int(nullable: false),
                        PersonnelName = c.String(maxLength: 128, storeType: "nvarchar"),
                        MobileInspectionId = c.Int(nullable: false),
                        MobileInspectionName = c.String(maxLength: 128, storeType: "nvarchar"),
                        PlanStartTime = c.DateTime(nullable: false, precision: 0),
                        PlanEndTime = c.DateTime(nullable: false, precision: 0),
                        StartTime = c.DateTime(precision: 0),
                        EndTime = c.DateTime(precision: 0),
                        bOverTime = c.Boolean(nullable: false),
                        Remark = c.String(maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonnelMobileInspectionItemHistories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        PID = c.Int(nullable: false),
                        ItemName = c.String(maxLength: 128, storeType: "nvarchar"),
                        nOrder = c.Int(nullable: false),
                        DevId = c.Int(nullable: false),
                        DevName = c.String(maxLength: 128, storeType: "nvarchar"),
                        PunchTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PersonnelMobileInspectionHistories", t => t.PID, cascadeDelete: true)
                .Index(t => t.PID);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTimeStamp = c.Long(nullable: false),
                        CardId = c.Int(),
                        RoleId = c.Int(),
                        PersonnelID = c.Int(),
                        Code = c.String(nullable: false, maxLength: 16, storeType: "nvarchar"),
                        X = c.Single(nullable: false),
                        Y = c.Single(nullable: false),
                        Z = c.Single(nullable: false),
                        DateTime = c.DateTime(nullable: false, precision: 0),
                        Power = c.Int(nullable: false),
                        PowerState = c.Int(nullable: false),
                        AreaState = c.Int(nullable: false),
                        MoveState = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Flag = c.String(maxLength: 16, storeType: "nvarchar"),
                        ArchorsText = c.String(maxLength: 64, storeType: "nvarchar"),
                        AreaId = c.Int(),
                        AreaPath = c.String(maxLength: 64, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.Id, t.DateTimeStamp })
                .Index(t => t.DateTimeStamp)
                .Index(t => t.CardId);
            
            CreateTable(
                "dbo.SafetyMeasuresHistories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        No = c.Int(nullable: false),
                        LssuerContent = c.String(maxLength: 256, storeType: "nvarchar"),
                        LicensorContent = c.String(maxLength: 256, storeType: "nvarchar"),
                        WorkTicketId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkTicketHistories", t => t.WorkTicketId)
                .Index(t => t.WorkTicketId);
            
            CreateTable(
                "dbo.U3DPosition",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, unicode: false),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        DateTime = c.DateTime(nullable: false, precision: 0),
                        DateTimeStamp = c.Long(nullable: false),
                        Power = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Flag = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkTicketHistories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        No = c.String(maxLength: 32, storeType: "nvarchar"),
                        PersonInCharge = c.String(maxLength: 128, storeType: "nvarchar"),
                        HeadOfWorkClass = c.String(maxLength: 128, storeType: "nvarchar"),
                        WorkPlace = c.String(maxLength: 128, storeType: "nvarchar"),
                        JobContent = c.String(maxLength: 512, storeType: "nvarchar"),
                        StartTimeOfPlannedWork = c.DateTime(nullable: false, precision: 0),
                        EndTimeOfPlannedWork = c.DateTime(nullable: false, precision: 0),
                        WorkCondition = c.String(maxLength: 128, storeType: "nvarchar"),
                        Lssuer = c.String(maxLength: 128, storeType: "nvarchar"),
                        Licensor = c.String(maxLength: 128, storeType: "nvarchar"),
                        Approver = c.String(maxLength: 128, storeType: "nvarchar"),
                        Comment = c.String(maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SafetyMeasuresHistories", "WorkTicketId", "dbo.WorkTicketHistories");
            DropForeignKey("dbo.PersonnelMobileInspectionItemHistories", "PID", "dbo.PersonnelMobileInspectionHistories");
            DropForeignKey("dbo.OperationItemHistories", "TicketId", "dbo.OperationTicketHistories");
            DropIndex("dbo.SafetyMeasuresHistories", new[] { "WorkTicketId" });
            DropIndex("dbo.Positions", new[] { "CardId" });
            DropIndex("dbo.Positions", new[] { "DateTimeStamp" });
            DropIndex("dbo.PersonnelMobileInspectionItemHistories", new[] { "PID" });
            DropIndex("dbo.OperationItemHistories", new[] { "TicketId" });
            DropTable("dbo.WorkTicketHistories");
            DropTable("dbo.U3DPosition");
            DropTable("dbo.SafetyMeasuresHistories");
            DropTable("dbo.Positions");
            DropTable("dbo.PersonnelMobileInspectionItemHistories");
            DropTable("dbo.PersonnelMobileInspectionHistories");
            DropTable("dbo.PersonnelHistories");
            DropTable("dbo.OperationTicketHistories");
            DropTable("dbo.OperationItemHistories");
            DropTable("dbo.LocationCardToPersonnelHistories");
            DropTable("dbo.LocationCardHistories");
            DropTable("dbo.LocationAlarmHistories");
            DropTable("dbo.EntranceGuardCardToPersonnelHistories");
            DropTable("dbo.EntranceGuardCardHistories");
            DropTable("dbo.DevInstantDataHistories");
            DropTable("dbo.DevInfoHistories");
            DropTable("dbo.DevEntranceGuardCardActions");
            DropTable("dbo.DevAlarmHistories");
        }
    }
}
