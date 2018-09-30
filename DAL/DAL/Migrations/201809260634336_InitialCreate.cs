namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Archors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        Type = c.Int(nullable: false),
                        IsAutoIp = c.Boolean(nullable: false),
                        Ip = c.String(),
                        ServerIp = c.String(),
                        ServerPort = c.Int(nullable: false),
                        Power = c.Double(nullable: false),
                        AliveTime = c.Double(nullable: false),
                        Enable = c.Int(nullable: false),
                        DevInfoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DevInfoes", t => t.DevInfoId, cascadeDelete: true)
                .Index(t => t.DevInfoId);
            
            CreateTable(
                "dbo.DevInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ParentId = c.Int(),
                        Code = c.String(),
                        KKS = c.String(),
                        Local_DevID = c.String(),
                        Local_CabinetID = c.String(),
                        Local_TypeCode = c.Int(nullable: false),
                        Abutment_Id = c.Int(),
                        Abutment_DevID = c.String(),
                        Abutment_Type = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        RunStatus = c.Int(nullable: false),
                        Placed = c.Boolean(),
                        ModelName = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        CreateTimeStamp = c.Long(nullable: false),
                        ModifyTime = c.DateTime(nullable: false),
                        ModifyTimeStamp = c.Long(nullable: false),
                        UserName = c.String(),
                        IP = c.String(),
                        PosX = c.Single(nullable: false),
                        PosY = c.Single(nullable: false),
                        PosZ = c.Single(nullable: false),
                        RotationX = c.Single(nullable: false),
                        RotationY = c.Single(nullable: false),
                        RotationZ = c.Single(nullable: false),
                        ScaleX = c.Single(nullable: false),
                        ScaleY = c.Single(nullable: false),
                        ScaleZ = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abutment_Id = c.Int(),
                        Name = c.String(nullable: false),
                        KKS = c.String(),
                        ParentId = c.Int(),
                        Abutment_ParentId = c.Int(),
                        X = c.Single(),
                        Y = c.Single(),
                        Z = c.Single(),
                        RX = c.Single(),
                        RY = c.Single(),
                        RZ = c.Single(),
                        SX = c.Single(),
                        SY = c.Single(),
                        SZ = c.Single(),
                        IsRelative = c.Boolean(nullable: false),
                        IsOnNormalArea = c.Boolean(nullable: false),
                        IsOnAlarmArea = c.Boolean(nullable: false),
                        IsOnLocationArea = c.Boolean(nullable: false),
                        Number = c.Int(),
                        Type = c.Int(nullable: false),
                        Describe = c.String(),
                        InitBoundId = c.Int(),
                        EditBoundId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.ParentId)
                .ForeignKey("dbo.Bounds", t => t.EditBoundId)
                .ForeignKey("dbo.Bounds", t => t.InitBoundId)
                .Index(t => t.ParentId)
                .Index(t => t.InitBoundId)
                .Index(t => t.EditBoundId);
            
            CreateTable(
                "dbo.Bounds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MinX = c.Single(nullable: false),
                        MaxX = c.Single(nullable: false),
                        MinY = c.Single(nullable: false),
                        MaxY = c.Single(nullable: false),
                        MinZ = c.Single(nullable: false),
                        MaxZ = c.Single(nullable: false),
                        IsRectangle = c.Boolean(nullable: false),
                        IsRelative = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Points",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        X = c.Single(nullable: false),
                        Y = c.Single(nullable: false),
                        Z = c.Single(nullable: false),
                        Index = c.Int(nullable: false),
                        BoundId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bounds", t => t.BoundId, cascadeDelete: true)
                .Index(t => t.BoundId);
            
            CreateTable(
                "dbo.ConfigArgs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Key = c.String(),
                        Value = c.String(),
                        ValueType = c.String(),
                        Describe = c.String(),
                        Classify = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abutment_Id = c.Int(),
                        Name = c.String(),
                        ParentId = c.Int(),
                        ShowOrder = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Personnels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abutment_Id = c.Int(),
                        Name = c.String(nullable: false),
                        Sex = c.Int(nullable: false),
                        Photo = c.String(),
                        BirthDay = c.DateTime(nullable: false),
                        BirthTimeStamp = c.Long(nullable: false),
                        Nation = c.String(),
                        Address = c.String(),
                        WorkNumber = c.Int(),
                        Email = c.String(),
                        Phone = c.String(),
                        Mobile = c.String(),
                        Enabled = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                        Pst = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Dev_DoorAccess",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(),
                        DoorId = c.String(),
                        DevInfoId = c.Int(nullable: false),
                        Local_DevID = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DevInfoes", t => t.DevInfoId, cascadeDelete: true)
                .Index(t => t.DevInfoId);
            
            CreateTable(
                "dbo.DevAlarms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abutment_Id = c.Int(),
                        Title = c.String(),
                        Msg = c.String(),
                        Level = c.Int(nullable: false),
                        Code = c.String(),
                        Src = c.Int(nullable: false),
                        DevInfoId = c.Int(nullable: false),
                        Device_desc = c.String(),
                        AlarmTime = c.DateTime(nullable: false),
                        AlarmTimeStamp = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DevInfoes", t => t.DevInfoId, cascadeDelete: true)
                .Index(t => t.DevInfoId);
            
            CreateTable(
                "dbo.DevInstantDatas",
                c => new
                    {
                        KKS = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Value = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        DateTimeStamp = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.KKS);
            
            CreateTable(
                "dbo.DevModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ModelId = c.String(),
                        Items = c.String(),
                        Class = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DevTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        Class = c.String(),
                        TypeCode = c.Long(nullable: false),
                        FrontElevation = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EntranceGuardCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abutment_Id = c.Int(),
                        Code = c.String(),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EntranceGuardCardToPersonnels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntranceGuardCardId = c.Int(nullable: false),
                        PersonnelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EntranceGuardCards", t => t.EntranceGuardCardId, cascadeDelete: true)
                .ForeignKey("dbo.Personnels", t => t.PersonnelId, cascadeDelete: true)
                .Index(t => t.EntranceGuardCardId)
                .Index(t => t.PersonnelId);
            
            CreateTable(
                "dbo.JurisDictionRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Describe = c.String(),
                        nFlag = c.Int(nullable: false),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        nTimeLength = c.Int(),
                        DelayTime = c.Int(nullable: false),
                        ErrorDistance = c.Int(nullable: false),
                        RepeatType = c.String(nullable: false),
                        AreaId = c.Int(nullable: false),
                        LocationCardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JurisDictions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Describe = c.String(),
                        nFlag = c.Int(nullable: false),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        nTimeLength = c.Int(),
                        DelayTime = c.Int(nullable: false),
                        ErrorDistance = c.Int(nullable: false),
                        RepeatType = c.String(nullable: false),
                        AreaId = c.Int(nullable: false),
                        LocationCardId = c.Int(nullable: false),
                        CreateTime = c.DateTime(),
                        ModifyTime = c.DateTime(),
                        DeleteTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.AreaId, cascadeDelete: true)
                .ForeignKey("dbo.LocationCards", t => t.LocationCardId, cascadeDelete: true)
                .Index(t => t.AreaId)
                .Index(t => t.LocationCardId);
            
            CreateTable(
                "dbo.LocationCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abutment_Id = c.Int(),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Describe = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.KKSCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Serial = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Code = c.String(nullable: false),
                        ParentCode = c.String(),
                        DesinCode = c.String(),
                        MainType = c.String(nullable: false),
                        SubType = c.String(nullable: false),
                        System = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LocationAlarms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AlarmType = c.Int(nullable: false),
                        AlarmLevel = c.Int(nullable: false),
                        LocationCardId = c.Int(nullable: false),
                        PersonnelId = c.Int(nullable: false),
                        Content = c.String(),
                        AlarmTime = c.DateTime(nullable: false),
                        AlarmTimeStamp = c.Long(nullable: false),
                        HandleTime = c.DateTime(nullable: false),
                        HandleTimeStamp = c.Long(nullable: false),
                        Handler = c.String(),
                        HandleType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LocationCards", t => t.LocationCardId, cascadeDelete: true)
                .ForeignKey("dbo.Personnels", t => t.PersonnelId, cascadeDelete: true)
                .Index(t => t.LocationCardId)
                .Index(t => t.PersonnelId);
            
            CreateTable(
                "dbo.LocationCardPositions",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        X = c.Single(nullable: false),
                        Y = c.Single(nullable: false),
                        Z = c.Single(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        DateTimeStamp = c.Long(nullable: false),
                        Power = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Flag = c.String(),
                        TopoNodes = c.Int(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.LocationCardToPersonnels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationCardId = c.Int(nullable: false),
                        PersonnelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LocationCards", t => t.LocationCardId, cascadeDelete: true)
                .ForeignKey("dbo.Personnels", t => t.PersonnelId, cascadeDelete: true)
                .Index(t => t.LocationCardId)
                .Index(t => t.PersonnelId);
            
            CreateTable(
                "dbo.MobileInspectionContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(nullable: false),
                        Content = c.String(),
                        nOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MobileInspectionDevs", t => t.ParentId, cascadeDelete: true)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.MobileInspectionDevs",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MobileInspectionItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemName = c.String(),
                        nOrder = c.Int(nullable: false),
                        PID = c.Int(nullable: false),
                        DevId = c.Int(nullable: false),
                        DevName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MobileInspections", t => t.PID, cascadeDelete: true)
                .Index(t => t.PID);
            
            CreateTable(
                "dbo.MobileInspections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        nOrder = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NodeKKS",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NodeId = c.Int(nullable: false),
                        KKSId = c.Int(nullable: false),
                        KKS = c.String(nullable: false),
                        NodeType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OperationItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketId = c.Int(),
                        OperationTime = c.DateTime(nullable: false),
                        Mark = c.Boolean(nullable: false),
                        OrderNum = c.Int(nullable: false),
                        Item = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OperationTickets", t => t.TicketId)
                .Index(t => t.TicketId);
            
            CreateTable(
                "dbo.OperationTickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        No = c.String(),
                        OperationTask = c.String(),
                        OperationStartTime = c.DateTime(nullable: false),
                        OperationEndTime = c.DateTime(nullable: false),
                        Guardian = c.String(),
                        Operator = c.String(),
                        DutyOfficer = c.String(),
                        Dispatch = c.String(),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonnelMobileInspectionItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        PID = c.Int(nullable: false),
                        ItemName = c.String(),
                        nOrder = c.Int(nullable: false),
                        DevId = c.Int(nullable: false),
                        DevName = c.String(),
                        PunchTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PersonnelMobileInspections", t => t.PID, cascadeDelete: true)
                .Index(t => t.PID);
            
            CreateTable(
                "dbo.PersonnelMobileInspections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PersonnelId = c.Int(nullable: false),
                        PersonnelName = c.String(),
                        MobileInspectionId = c.Int(nullable: false),
                        MobileInspectionName = c.String(),
                        PlanStartTime = c.DateTime(nullable: false),
                        PlanEndTime = c.DateTime(nullable: false),
                        StartTime = c.DateTime(),
                        bOverTime = c.Boolean(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SafetyMeasures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        No = c.Int(nullable: false),
                        LssuerContent = c.String(),
                        LicensorContent = c.String(),
                        WorkTicketId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkTickets", t => t.WorkTicketId)
                .Index(t => t.WorkTicketId);
            
            CreateTable(
                "dbo.WorkTickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        No = c.String(),
                        PersonInCharge = c.String(),
                        HeadOfWorkClass = c.String(),
                        WorkPlace = c.String(),
                        JobContent = c.String(),
                        StartTimeOfPlannedWork = c.DateTime(nullable: false),
                        EndTimeOfPlannedWork = c.DateTime(nullable: false),
                        WorkCondition = c.String(),
                        Lssuer = c.String(),
                        Licensor = c.String(),
                        Approver = c.String(),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SafetyMeasures", "WorkTicketId", "dbo.WorkTickets");
            DropForeignKey("dbo.PersonnelMobileInspectionItems", "PID", "dbo.PersonnelMobileInspections");
            DropForeignKey("dbo.OperationItems", "TicketId", "dbo.OperationTickets");
            DropForeignKey("dbo.MobileInspectionItems", "PID", "dbo.MobileInspections");
            DropForeignKey("dbo.MobileInspectionContents", "ParentId", "dbo.MobileInspectionDevs");
            DropForeignKey("dbo.LocationCardToPersonnels", "PersonnelId", "dbo.Personnels");
            DropForeignKey("dbo.LocationCardToPersonnels", "LocationCardId", "dbo.LocationCards");
            DropForeignKey("dbo.LocationAlarms", "PersonnelId", "dbo.Personnels");
            DropForeignKey("dbo.LocationAlarms", "LocationCardId", "dbo.LocationCards");
            DropForeignKey("dbo.JurisDictions", "LocationCardId", "dbo.LocationCards");
            DropForeignKey("dbo.JurisDictions", "AreaId", "dbo.Areas");
            DropForeignKey("dbo.EntranceGuardCardToPersonnels", "PersonnelId", "dbo.Personnels");
            DropForeignKey("dbo.EntranceGuardCardToPersonnels", "EntranceGuardCardId", "dbo.EntranceGuardCards");
            DropForeignKey("dbo.DevAlarms", "DevInfoId", "dbo.DevInfoes");
            DropForeignKey("dbo.Dev_DoorAccess", "DevInfoId", "dbo.DevInfoes");
            DropForeignKey("dbo.Personnels", "ParentId", "dbo.Departments");
            DropForeignKey("dbo.Departments", "ParentId", "dbo.Departments");
            DropForeignKey("dbo.DevInfoes", "ParentId", "dbo.Areas");
            DropForeignKey("dbo.Areas", "InitBoundId", "dbo.Bounds");
            DropForeignKey("dbo.Areas", "EditBoundId", "dbo.Bounds");
            DropForeignKey("dbo.Points", "BoundId", "dbo.Bounds");
            DropForeignKey("dbo.Areas", "ParentId", "dbo.Areas");
            DropForeignKey("dbo.Archors", "DevInfoId", "dbo.DevInfoes");
            DropIndex("dbo.SafetyMeasures", new[] { "WorkTicketId" });
            DropIndex("dbo.PersonnelMobileInspectionItems", new[] { "PID" });
            DropIndex("dbo.OperationItems", new[] { "TicketId" });
            DropIndex("dbo.MobileInspectionItems", new[] { "PID" });
            DropIndex("dbo.MobileInspectionContents", new[] { "ParentId" });
            DropIndex("dbo.LocationCardToPersonnels", new[] { "PersonnelId" });
            DropIndex("dbo.LocationCardToPersonnels", new[] { "LocationCardId" });
            DropIndex("dbo.LocationAlarms", new[] { "PersonnelId" });
            DropIndex("dbo.LocationAlarms", new[] { "LocationCardId" });
            DropIndex("dbo.JurisDictions", new[] { "LocationCardId" });
            DropIndex("dbo.JurisDictions", new[] { "AreaId" });
            DropIndex("dbo.EntranceGuardCardToPersonnels", new[] { "PersonnelId" });
            DropIndex("dbo.EntranceGuardCardToPersonnels", new[] { "EntranceGuardCardId" });
            DropIndex("dbo.DevAlarms", new[] { "DevInfoId" });
            DropIndex("dbo.Dev_DoorAccess", new[] { "DevInfoId" });
            DropIndex("dbo.Personnels", new[] { "ParentId" });
            DropIndex("dbo.Departments", new[] { "ParentId" });
            DropIndex("dbo.Points", new[] { "BoundId" });
            DropIndex("dbo.Areas", new[] { "EditBoundId" });
            DropIndex("dbo.Areas", new[] { "InitBoundId" });
            DropIndex("dbo.Areas", new[] { "ParentId" });
            DropIndex("dbo.DevInfoes", new[] { "ParentId" });
            DropIndex("dbo.Archors", new[] { "DevInfoId" });
            DropTable("dbo.WorkTickets");
            DropTable("dbo.SafetyMeasures");
            DropTable("dbo.Roles");
            DropTable("dbo.Posts");
            DropTable("dbo.PersonnelMobileInspections");
            DropTable("dbo.PersonnelMobileInspectionItems");
            DropTable("dbo.OperationTickets");
            DropTable("dbo.OperationItems");
            DropTable("dbo.NodeKKS");
            DropTable("dbo.MobileInspections");
            DropTable("dbo.MobileInspectionItems");
            DropTable("dbo.MobileInspectionDevs");
            DropTable("dbo.MobileInspectionContents");
            DropTable("dbo.LocationCardToPersonnels");
            DropTable("dbo.LocationCardPositions");
            DropTable("dbo.LocationAlarms");
            DropTable("dbo.KKSCodes");
            DropTable("dbo.LocationCards");
            DropTable("dbo.JurisDictions");
            DropTable("dbo.JurisDictionRecords");
            DropTable("dbo.EntranceGuardCardToPersonnels");
            DropTable("dbo.EntranceGuardCards");
            DropTable("dbo.DevTypes");
            DropTable("dbo.DevModels");
            DropTable("dbo.DevInstantDatas");
            DropTable("dbo.DevAlarms");
            DropTable("dbo.Dev_DoorAccess");
            DropTable("dbo.Personnels");
            DropTable("dbo.Departments");
            DropTable("dbo.ConfigArgs");
            DropTable("dbo.Points");
            DropTable("dbo.Bounds");
            DropTable("dbo.Areas");
            DropTable("dbo.DevInfoes");
            DropTable("dbo.Archors");
        }
    }
}
