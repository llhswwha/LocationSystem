namespace DAL.LocationDbMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitInsert : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Archors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 16, storeType: "nvarchar"),
                        Ip = c.String(maxLength: 16, storeType: "nvarchar"),
                        Name = c.String(maxLength: 128, storeType: "nvarchar"),
                        ParentId = c.Int(),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        Type = c.Int(nullable: false),
                        IsAutoIp = c.Boolean(nullable: false),
                        ServerIp = c.String(maxLength: 16, storeType: "nvarchar"),
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
                        Name = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        ParentId = c.Int(),
                        Code = c.String(maxLength: 32, storeType: "nvarchar"),
                        KKS = c.String(maxLength: 32, storeType: "nvarchar"),
                        Local_DevID = c.String(maxLength: 64, storeType: "nvarchar"),
                        Local_CabinetID = c.String(maxLength: 64, storeType: "nvarchar"),
                        Local_TypeCode = c.Int(nullable: false),
                        Abutment_Id = c.Int(),
                        Abutment_DevID = c.String(maxLength: 64, storeType: "nvarchar"),
                        Abutment_Type = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        RunStatus = c.Int(nullable: false),
                        Placed = c.Boolean(),
                        ModelName = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        CreateTimeStamp = c.Long(nullable: false),
                        ModifyTime = c.DateTime(nullable: false, precision: 0),
                        ModifyTimeStamp = c.Long(nullable: false),
                        UserName = c.String(maxLength: 128, storeType: "nvarchar"),
                        IP = c.String(maxLength: 32, storeType: "nvarchar"),
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
                        Name = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        KKS = c.String(maxLength: 32, storeType: "nvarchar"),
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
                        IsCreateAreaByData = c.Boolean(nullable: false),
                        IsOnAlarmArea = c.Boolean(nullable: false),
                        IsOnLocationArea = c.Boolean(nullable: false),
                        Number = c.Int(),
                        Type = c.Int(nullable: false),
                        Describe = c.String(maxLength: 128, storeType: "nvarchar"),
                        InitBoundId = c.Int(),
                        EditBoundId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bounds", t => t.EditBoundId)
                .ForeignKey("dbo.Bounds", t => t.InitBoundId)
                .ForeignKey("dbo.Areas", t => t.ParentId)
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
                        Shape = c.Int(nullable: false),
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
                "dbo.ArchorSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 16, storeType: "nvarchar"),
                        ArchorId = c.Int(nullable: false),
                        Error = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 32, storeType: "nvarchar"),
                        RelativeHeight = c.Double(nullable: false),
                        AbsoluteHeight = c.Double(nullable: false),
                        RoomName = c.String(maxLength: 128, storeType: "nvarchar"),
                        RoomMinX = c.String(maxLength: 64, storeType: "nvarchar"),
                        RoomMinY = c.String(maxLength: 64, storeType: "nvarchar"),
                        FloorName = c.String(maxLength: 64, storeType: "nvarchar"),
                        FloorMinX = c.String(maxLength: 64, storeType: "nvarchar"),
                        FloorMinY = c.String(maxLength: 64, storeType: "nvarchar"),
                        BuildingName = c.String(maxLength: 64, storeType: "nvarchar"),
                        BuildingMinX = c.String(maxLength: 64, storeType: "nvarchar"),
                        BuildingMinY = c.String(maxLength: 64, storeType: "nvarchar"),
                        RelativeMode = c.Int(nullable: false),
                        ZeroX = c.String(maxLength: 64, storeType: "nvarchar"),
                        ZeroY = c.String(maxLength: 64, storeType: "nvarchar"),
                        RelativeX = c.String(maxLength: 64, storeType: "nvarchar"),
                        RelativeY = c.String(maxLength: 64, storeType: "nvarchar"),
                        AbsoluteX = c.String(maxLength: 64, storeType: "nvarchar"),
                        AbsoluteY = c.String(maxLength: 64, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AreaAuthorizationRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Description = c.String(maxLength: 128, storeType: "nvarchar"),
                        TimeType = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false, precision: 0),
                        EndTime = c.DateTime(nullable: false, precision: 0),
                        TimeSpan = c.Int(nullable: false),
                        DelayTime = c.Int(nullable: false),
                        ErrorDistance = c.Int(nullable: false),
                        RepeatDay = c.Int(nullable: false),
                        AreaId = c.Int(nullable: false),
                        AccessType = c.Int(nullable: false),
                        RangeType = c.Int(nullable: false),
                        CreateTime = c.DateTime(precision: 0),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteTime = c.DateTime(precision: 0),
                        CardRoleId = c.Int(nullable: false),
                        AuthorizationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AreaAuthorizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Description = c.String(maxLength: 128, storeType: "nvarchar"),
                        TimeType = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false, precision: 0),
                        EndTime = c.DateTime(nullable: false, precision: 0),
                        TimeSpan = c.Int(nullable: false),
                        DelayTime = c.Int(nullable: false),
                        ErrorDistance = c.Int(nullable: false),
                        RepeatDay = c.Int(nullable: false),
                        AreaId = c.Int(nullable: false),
                        AccessType = c.Int(nullable: false),
                        RangeType = c.Int(nullable: false),
                        CreateTime = c.DateTime(precision: 0),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CardRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 32, storeType: "nvarchar"),
                        Description = c.String(maxLength: 64, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConfigArgs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 16, storeType: "nvarchar"),
                        Key = c.String(maxLength: 32, storeType: "nvarchar"),
                        Value = c.String(maxLength: 32, storeType: "nvarchar"),
                        ValueType = c.String(maxLength: 8, storeType: "nvarchar"),
                        Describe = c.String(maxLength: 32, storeType: "nvarchar"),
                        Classify = c.String(maxLength: 8, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abutment_Id = c.Int(),
                        Name = c.String(maxLength: 16, storeType: "nvarchar"),
                        ParentId = c.Int(),
                        ShowOrder = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Description = c.String(maxLength: 128, storeType: "nvarchar"),
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
                        Name = c.String(nullable: false, maxLength: 16, storeType: "nvarchar"),
                        Sex = c.Int(nullable: false),
                        Photo = c.String(maxLength: 128, storeType: "nvarchar"),
                        BirthDay = c.DateTime(nullable: false, precision: 0),
                        BirthTimeStamp = c.Long(nullable: false),
                        Nation = c.String(maxLength: 64, storeType: "nvarchar"),
                        Address = c.String(maxLength: 512, storeType: "nvarchar"),
                        WorkNumber = c.Int(),
                        Email = c.String(maxLength: 64, storeType: "nvarchar"),
                        Phone = c.String(maxLength: 16, storeType: "nvarchar"),
                        Mobile = c.String(maxLength: 16, storeType: "nvarchar"),
                        Enabled = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                        Pst = c.String(maxLength: 16, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Dev_CameraInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ip = c.String(unicode: false),
                        UserName = c.String(unicode: false),
                        PassWord = c.String(unicode: false),
                        Port = c.Int(nullable: false),
                        CameraIndex = c.Int(nullable: false),
                        ParentId = c.Int(),
                        DevInfoId = c.Int(nullable: false),
                        Local_DevID = c.String(maxLength: 64, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DevInfoes", t => t.DevInfoId, cascadeDelete: true)
                .Index(t => t.DevInfoId);
            
            CreateTable(
                "dbo.Dev_DoorAccess",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(),
                        DoorId = c.String(maxLength: 64, storeType: "nvarchar"),
                        DevInfoId = c.Int(nullable: false),
                        Local_DevID = c.String(maxLength: 64, storeType: "nvarchar"),
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
                        Title = c.String(maxLength: 64, storeType: "nvarchar"),
                        Msg = c.String(maxLength: 512, storeType: "nvarchar"),
                        Level = c.Int(nullable: false),
                        Code = c.String(maxLength: 32, storeType: "nvarchar"),
                        Src = c.Int(nullable: false),
                        DevInfoId = c.Int(nullable: false),
                        Device_desc = c.String(maxLength: 128, storeType: "nvarchar"),
                        AlarmTime = c.DateTime(nullable: false, precision: 0),
                        AlarmTimeStamp = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DevInfoes", t => t.DevInfoId, cascadeDelete: true)
                .Index(t => t.DevInfoId);
            
            CreateTable(
                "dbo.DevInstantDatas",
                c => new
                    {
                        KKS = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        Name = c.String(maxLength: 128, storeType: "nvarchar"),
                        Value = c.String(maxLength: 32, storeType: "nvarchar"),
                        DateTime = c.DateTime(nullable: false, precision: 0),
                        DateTimeStamp = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.KKS);
            
            CreateTable(
                "dbo.DevModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModelId = c.String(maxLength: 128, storeType: "nvarchar"),
                        Items = c.String(maxLength: 16, storeType: "nvarchar"),
                        Class = c.String(maxLength: 32, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DevTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(maxLength: 128, storeType: "nvarchar"),
                        Class = c.String(maxLength: 32, storeType: "nvarchar"),
                        TypeCode = c.Long(nullable: false),
                        FrontElevation = c.String(maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EntranceGuardCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abutment_Id = c.Int(),
                        Code = c.String(maxLength: 64, storeType: "nvarchar"),
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
                "dbo.KKSCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Serial = c.String(nullable: false, maxLength: 8, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Code = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        ParentCode = c.String(maxLength: 32, storeType: "nvarchar"),
                        DesinCode = c.String(maxLength: 32, storeType: "nvarchar"),
                        MainType = c.String(nullable: false, maxLength: 16, storeType: "nvarchar"),
                        SubType = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        System = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LocationAlarms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AlarmId = c.String(maxLength: 40, storeType: "nvarchar"),
                        AlarmType = c.Int(nullable: false),
                        AlarmLevel = c.Int(nullable: false),
                        LocationCardId = c.Int(),
                        PersonnelId = c.Int(),
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LocationCards", t => t.LocationCardId)
                .ForeignKey("dbo.Personnels", t => t.PersonnelId)
                .Index(t => t.LocationCardId)
                .Index(t => t.PersonnelId);
            
            CreateTable(
                "dbo.LocationCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abutment_Id = c.Int(),
                        Code = c.String(nullable: false, maxLength: 16, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Describe = c.String(maxLength: 128, storeType: "nvarchar"),
                        CardRoleId = c.Int(),
                        Power = c.Int(nullable: false),
                        PowerState = c.Int(nullable: false),
                        Flag = c.String(maxLength: 16, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CardRoles", t => t.CardRoleId)
                .Index(t => t.CardRoleId);
            
            CreateTable(
                "dbo.LocationCardPositions",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        CardId = c.Int(),
                        X = c.Single(nullable: false),
                        Y = c.Single(nullable: false),
                        Z = c.Single(nullable: false),
                        DateTime = c.DateTime(nullable: false, precision: 0),
                        DateTimeStamp = c.Long(nullable: false),
                        Power = c.Int(nullable: false),
                        PowerState = c.Int(nullable: false),
                        AreaState = c.Int(nullable: false),
                        MoveState = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Flag = c.String(maxLength: 16, storeType: "nvarchar"),
                        ArchorsText = c.String(maxLength: 128, storeType: "nvarchar"),
                        AreaId = c.Int(),
                        PersonId = c.Int(),
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
                        Content = c.String(maxLength: 128, storeType: "nvarchar"),
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
                        Name = c.String(maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MobileInspectionItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemName = c.String(maxLength: 128, storeType: "nvarchar"),
                        nOrder = c.Int(nullable: false),
                        PID = c.Int(nullable: false),
                        DevId = c.Int(nullable: false),
                        DevName = c.String(maxLength: 128, storeType: "nvarchar"),
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
                        Name = c.String(maxLength: 64, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NodeKKS",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NodeId = c.Int(nullable: false),
                        KKSId = c.Int(nullable: false),
                        KKS = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        NodeType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OperationItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketId = c.Int(),
                        OperationTime = c.DateTime(nullable: false, precision: 0),
                        Mark = c.Boolean(nullable: false),
                        OrderNum = c.Int(nullable: false),
                        Item = c.String(maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OperationTickets", t => t.TicketId)
                .Index(t => t.TicketId);
            
            CreateTable(
                "dbo.OperationTickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abutment_Id = c.Int(),
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
                "dbo.PersonnelMobileInspectionItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        PID = c.Int(nullable: false),
                        ItemName = c.String(maxLength: 128, storeType: "nvarchar"),
                        nOrder = c.Int(nullable: false),
                        DevId = c.Int(nullable: false),
                        DevName = c.String(maxLength: 128, storeType: "nvarchar"),
                        PunchTime = c.DateTime(precision: 0),
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
                        PersonnelName = c.String(maxLength: 128, storeType: "nvarchar"),
                        MobileInspectionId = c.Int(nullable: false),
                        MobileInspectionName = c.String(maxLength: 128, storeType: "nvarchar"),
                        PlanStartTime = c.DateTime(nullable: false, precision: 0),
                        PlanEndTime = c.DateTime(nullable: false, precision: 0),
                        StartTime = c.DateTime(precision: 0),
                        bOverTime = c.Boolean(nullable: false),
                        Remark = c.String(maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Info = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 8, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 32, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 64, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SafetyMeasures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        No = c.Int(nullable: false),
                        LssuerContent = c.String(maxLength: 256, storeType: "nvarchar"),
                        LicensorContent = c.String(maxLength: 256, storeType: "nvarchar"),
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
                        Abutment_Id = c.Int(),
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
            DropForeignKey("dbo.SafetyMeasures", "WorkTicketId", "dbo.WorkTickets");
            DropForeignKey("dbo.PersonnelMobileInspectionItems", "PID", "dbo.PersonnelMobileInspections");
            DropForeignKey("dbo.OperationItems", "TicketId", "dbo.OperationTickets");
            DropForeignKey("dbo.MobileInspectionItems", "PID", "dbo.MobileInspections");
            DropForeignKey("dbo.MobileInspectionContents", "ParentId", "dbo.MobileInspectionDevs");
            DropForeignKey("dbo.LocationCardToPersonnels", "PersonnelId", "dbo.Personnels");
            DropForeignKey("dbo.LocationCardToPersonnels", "LocationCardId", "dbo.LocationCards");
            DropForeignKey("dbo.LocationAlarms", "PersonnelId", "dbo.Personnels");
            DropForeignKey("dbo.LocationAlarms", "LocationCardId", "dbo.LocationCards");
            DropForeignKey("dbo.LocationCards", "CardRoleId", "dbo.CardRoles");
            DropForeignKey("dbo.EntranceGuardCardToPersonnels", "PersonnelId", "dbo.Personnels");
            DropForeignKey("dbo.EntranceGuardCardToPersonnels", "EntranceGuardCardId", "dbo.EntranceGuardCards");
            DropForeignKey("dbo.DevAlarms", "DevInfoId", "dbo.DevInfoes");
            DropForeignKey("dbo.Dev_DoorAccess", "DevInfoId", "dbo.DevInfoes");
            DropForeignKey("dbo.Dev_CameraInfo", "DevInfoId", "dbo.DevInfoes");
            DropForeignKey("dbo.Personnels", "ParentId", "dbo.Departments");
            DropForeignKey("dbo.Departments", "ParentId", "dbo.Departments");
            DropForeignKey("dbo.Archors", "DevInfoId", "dbo.DevInfoes");
            DropForeignKey("dbo.DevInfoes", "ParentId", "dbo.Areas");
            DropForeignKey("dbo.Areas", "ParentId", "dbo.Areas");
            DropForeignKey("dbo.Areas", "InitBoundId", "dbo.Bounds");
            DropForeignKey("dbo.Areas", "EditBoundId", "dbo.Bounds");
            DropForeignKey("dbo.Points", "BoundId", "dbo.Bounds");
            DropIndex("dbo.SafetyMeasures", new[] { "WorkTicketId" });
            DropIndex("dbo.PersonnelMobileInspectionItems", new[] { "PID" });
            DropIndex("dbo.OperationItems", new[] { "TicketId" });
            DropIndex("dbo.MobileInspectionItems", new[] { "PID" });
            DropIndex("dbo.MobileInspectionContents", new[] { "ParentId" });
            DropIndex("dbo.LocationCardToPersonnels", new[] { "PersonnelId" });
            DropIndex("dbo.LocationCardToPersonnels", new[] { "LocationCardId" });
            DropIndex("dbo.LocationCards", new[] { "CardRoleId" });
            DropIndex("dbo.LocationAlarms", new[] { "PersonnelId" });
            DropIndex("dbo.LocationAlarms", new[] { "LocationCardId" });
            DropIndex("dbo.EntranceGuardCardToPersonnels", new[] { "PersonnelId" });
            DropIndex("dbo.EntranceGuardCardToPersonnels", new[] { "EntranceGuardCardId" });
            DropIndex("dbo.DevAlarms", new[] { "DevInfoId" });
            DropIndex("dbo.Dev_DoorAccess", new[] { "DevInfoId" });
            DropIndex("dbo.Dev_CameraInfo", new[] { "DevInfoId" });
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
            DropTable("dbo.Pictures");
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
            DropTable("dbo.LocationCards");
            DropTable("dbo.LocationAlarms");
            DropTable("dbo.KKSCodes");
            DropTable("dbo.EntranceGuardCardToPersonnels");
            DropTable("dbo.EntranceGuardCards");
            DropTable("dbo.DevTypes");
            DropTable("dbo.DevModels");
            DropTable("dbo.DevInstantDatas");
            DropTable("dbo.DevAlarms");
            DropTable("dbo.Dev_DoorAccess");
            DropTable("dbo.Dev_CameraInfo");
            DropTable("dbo.Personnels");
            DropTable("dbo.Departments");
            DropTable("dbo.ConfigArgs");
            DropTable("dbo.CardRoles");
            DropTable("dbo.AreaAuthorizations");
            DropTable("dbo.AreaAuthorizationRecords");
            DropTable("dbo.ArchorSettings");
            DropTable("dbo.Points");
            DropTable("dbo.Bounds");
            DropTable("dbo.Areas");
            DropTable("dbo.DevInfoes");
            DropTable("dbo.Archors");
        }
    }
}
