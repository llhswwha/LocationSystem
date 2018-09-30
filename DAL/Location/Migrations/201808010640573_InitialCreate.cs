namespace Location.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alarms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Tag = c.String(),
                        Target = c.String(),
                        Content = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        HandleTime = c.DateTime(nullable: false),
                        Handler = c.String(),
                        HandleType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Archors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        Z = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        IsAutoIp = c.Boolean(nullable: false),
                        Ip = c.String(),
                        ServerIp = c.String(),
                        ServerPort = c.Int(nullable: false),
                        Power = c.Double(nullable: false),
                        AliveTime = c.Double(nullable: false),
                        Enable = c.Int(nullable: false),
                        DevId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DevInfoes", t => t.DevId)
                .Index(t => t.DevId);
            
            CreateTable(
                "dbo.DevInfoes",
                c => new
                    {
                        DevID = c.String(nullable: false, maxLength: 128),
                        TypeCode = c.Int(nullable: false),
                        PDevID = c.String(),
                        Name = c.String(),
                        Status = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        ModifyTime = c.DateTime(nullable: false),
                        UserName = c.String(),
                        IP = c.String(),
                        ParentId = c.Int(),
                        PosId = c.String(),
                        Pos_DevID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DevID)
                .ForeignKey("dbo.PhysicalTopologies", t => t.ParentId)
                .ForeignKey("dbo.DevPos", t => t.Pos_DevID)
                .Index(t => t.ParentId)
                .Index(t => t.Pos_DevID);
            
            CreateTable(
                "dbo.PhysicalTopologies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ParentId = c.Int(),
                        Number = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Describe = c.String(),
                        Tag = c.String(),
                        TransfromId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PhysicalTopologies", t => t.ParentId)
                .ForeignKey("dbo.TransformMs", t => t.TransfromId)
                .Index(t => t.ParentId)
                .Index(t => t.TransfromId);
            
            CreateTable(
                "dbo.TransformMs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        X = c.Single(nullable: false),
                        Y = c.Single(nullable: false),
                        Z = c.Single(nullable: false),
                        RX = c.Single(nullable: false),
                        RY = c.Single(nullable: false),
                        RZ = c.Single(nullable: false),
                        SX = c.Single(nullable: false),
                        SY = c.Single(nullable: false),
                        SZ = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DevPos",
                c => new
                    {
                        DevID = c.String(nullable: false, maxLength: 128),
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
                .PrimaryKey(t => t.DevID);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MapId = c.Int(),
                        TransformId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Maps", t => t.MapId)
                .ForeignKey("dbo.TransformMs", t => t.TransformId, cascadeDelete: true)
                .Index(t => t.MapId)
                .Index(t => t.TransformId);
            
            CreateTable(
                "dbo.Maps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FileName = c.String(),
                        ShowOrder = c.Int(nullable: false),
                        DepId = c.Int(),
                        TopoNodeId = c.Int(),
                        IsMain = c.Boolean(nullable: false),
                        MinX = c.Single(nullable: false),
                        MaxX = c.Single(nullable: false),
                        MinY = c.Single(nullable: false),
                        MaxY = c.Single(nullable: false),
                        MinZ = c.Single(nullable: false),
                        MaxZ = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepId)
                .ForeignKey("dbo.PhysicalTopologies", t => t.TopoNodeId)
                .Index(t => t.DepId)
                .Index(t => t.TopoNodeId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShowOrder = c.Int(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Personnels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Sex = c.String(),
                        Photo = c.String(),
                        BirthDay = c.String(),
                        Nation = c.String(),
                        Address = c.String(),
                        TagId = c.Int(),
                        ParentId = c.Int(),
                        WorkNumber = c.Int(nullable: false),
                        PstId = c.Int(nullable: false),
                        PhoneNumber = c.String(),
                        MailBox = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.ParentId)
                .ForeignKey("dbo.Posts", t => t.PstId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId)
                .Index(t => t.TagId)
                .Index(t => t.ParentId)
                .Index(t => t.PstId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        Describe = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        PtId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PhysicalTopologies", t => t.PtId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.PtId)
                .Index(t => t.TagId);
            
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
                        PtId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                        CreateTime = c.DateTime(),
                        ModifyTime = c.DateTime(),
                        DeleteTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PhysicalTopologies", t => t.PtId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.PtId)
                .Index(t => t.TagId);
            
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
                "dbo.Menus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        Url = c.String(),
                        Order = c.Int(nullable: false),
                        PMenuId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Menus", t => t.PMenuId)
                .Index(t => t.PMenuId);
            
            CreateTable(
                "dbo.Functions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        Icon_Id = c.Int(),
                        Menu_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Icons", t => t.Icon_Id)
                .ForeignKey("dbo.Menus", t => t.Menu_Id)
                .Index(t => t.Icon_Id)
                .Index(t => t.Menu_Id);
            
            CreateTable(
                "dbo.Icons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Image = c.String(),
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
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_SetModel",
                c => new
                    {
                        RecordID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        strText = c.String(),
                        ID = c.String(),
                        strType = c.String(),
                        Visible = c.String(),
                        Items = c.String(),
                        Class = c.String(),
                        Obligate1 = c.String(),
                        Obligate2 = c.String(),
                    })
                .PrimaryKey(t => t.RecordID);
            
            CreateTable(
                "dbo.t_Template_TypeProperty",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        TypeCode = c.Long(nullable: false),
                        orientation = c.Int(),
                        InstantTime = c.Long(),
                        ReviseTime = c.Long(),
                        height = c.String(),
                        energy = c.String(),
                        weight = c.String(),
                        model = c.String(),
                        style = c.String(),
                        manufacturer = c.String(),
                        sizen = c.String(),
                        colour = c.String(),
                        refrigeration = c.String(),
                        FrontElevation = c.String(),
                        RearView = c.String(),
                        BackInstruction = c.String(),
                        Obligate3 = c.String(),
                        Obligate4 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagPositions",
                c => new
                    {
                        Tag = c.String(nullable: false, maxLength: 128),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        Time = c.Long(nullable: false),
                        Power = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Flag = c.String(),
                    })
                .PrimaryKey(t => t.Tag);
            
            CreateTable(
                "dbo.Targets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        TagId = c.Int(),
                        Type = c.Int(nullable: false),
                        DepId = c.Int(),
                        ImageFile = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepId)
                .ForeignKey("dbo.Tags", t => t.TagId)
                .Index(t => t.TagId)
                .Index(t => t.DepId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LoginName = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        DepId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepId)
                .Index(t => t.DepId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "DepId", "dbo.Departments");
            DropForeignKey("dbo.Targets", "TagId", "dbo.Tags");
            DropForeignKey("dbo.Targets", "DepId", "dbo.Departments");
            DropForeignKey("dbo.Menus", "PMenuId", "dbo.Menus");
            DropForeignKey("dbo.Functions", "Menu_Id", "dbo.Menus");
            DropForeignKey("dbo.Functions", "Icon_Id", "dbo.Icons");
            DropForeignKey("dbo.JurisDictions", "TagId", "dbo.Tags");
            DropForeignKey("dbo.JurisDictions", "PtId", "dbo.PhysicalTopologies");
            DropForeignKey("dbo.JurisDictionRecords", "TagId", "dbo.Tags");
            DropForeignKey("dbo.JurisDictionRecords", "PtId", "dbo.PhysicalTopologies");
            DropForeignKey("dbo.Areas", "TransformId", "dbo.TransformMs");
            DropForeignKey("dbo.Maps", "TopoNodeId", "dbo.PhysicalTopologies");
            DropForeignKey("dbo.Maps", "DepId", "dbo.Departments");
            DropForeignKey("dbo.Personnels", "TagId", "dbo.Tags");
            DropForeignKey("dbo.Personnels", "PstId", "dbo.Posts");
            DropForeignKey("dbo.Personnels", "ParentId", "dbo.Departments");
            DropForeignKey("dbo.Departments", "ParentId", "dbo.Departments");
            DropForeignKey("dbo.Areas", "MapId", "dbo.Maps");
            DropForeignKey("dbo.Archors", "DevId", "dbo.DevInfoes");
            DropForeignKey("dbo.DevInfoes", "Pos_DevID", "dbo.DevPos");
            DropForeignKey("dbo.PhysicalTopologies", "TransfromId", "dbo.TransformMs");
            DropForeignKey("dbo.DevInfoes", "ParentId", "dbo.PhysicalTopologies");
            DropForeignKey("dbo.PhysicalTopologies", "ParentId", "dbo.PhysicalTopologies");
            DropIndex("dbo.Users", new[] { "DepId" });
            DropIndex("dbo.Targets", new[] { "DepId" });
            DropIndex("dbo.Targets", new[] { "TagId" });
            DropIndex("dbo.Functions", new[] { "Menu_Id" });
            DropIndex("dbo.Functions", new[] { "Icon_Id" });
            DropIndex("dbo.Menus", new[] { "PMenuId" });
            DropIndex("dbo.JurisDictions", new[] { "TagId" });
            DropIndex("dbo.JurisDictions", new[] { "PtId" });
            DropIndex("dbo.JurisDictionRecords", new[] { "TagId" });
            DropIndex("dbo.JurisDictionRecords", new[] { "PtId" });
            DropIndex("dbo.Personnels", new[] { "PstId" });
            DropIndex("dbo.Personnels", new[] { "ParentId" });
            DropIndex("dbo.Personnels", new[] { "TagId" });
            DropIndex("dbo.Departments", new[] { "ParentId" });
            DropIndex("dbo.Maps", new[] { "TopoNodeId" });
            DropIndex("dbo.Maps", new[] { "DepId" });
            DropIndex("dbo.Areas", new[] { "TransformId" });
            DropIndex("dbo.Areas", new[] { "MapId" });
            DropIndex("dbo.PhysicalTopologies", new[] { "TransfromId" });
            DropIndex("dbo.PhysicalTopologies", new[] { "ParentId" });
            DropIndex("dbo.DevInfoes", new[] { "Pos_DevID" });
            DropIndex("dbo.DevInfoes", new[] { "ParentId" });
            DropIndex("dbo.Archors", new[] { "DevId" });
            DropTable("dbo.Users");
            DropTable("dbo.Targets");
            DropTable("dbo.TagPositions");
            DropTable("dbo.t_Template_TypeProperty");
            DropTable("dbo.t_SetModel");
            DropTable("dbo.Roles");
            DropTable("dbo.NodeKKS");
            DropTable("dbo.Icons");
            DropTable("dbo.Functions");
            DropTable("dbo.Menus");
            DropTable("dbo.KKSCodes");
            DropTable("dbo.JurisDictions");
            DropTable("dbo.JurisDictionRecords");
            DropTable("dbo.Tags");
            DropTable("dbo.Posts");
            DropTable("dbo.Personnels");
            DropTable("dbo.Departments");
            DropTable("dbo.Maps");
            DropTable("dbo.Areas");
            DropTable("dbo.DevPos");
            DropTable("dbo.TransformMs");
            DropTable("dbo.PhysicalTopologies");
            DropTable("dbo.DevInfoes");
            DropTable("dbo.Archors");
            DropTable("dbo.Alarms");
        }
    }
}
