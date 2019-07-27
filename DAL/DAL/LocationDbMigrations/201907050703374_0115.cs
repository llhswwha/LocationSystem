namespace DAL.LocationDbMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0115 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.CameraAlarmJsons",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Json = c.Binary(),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.Users",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(maxLength: 128, storeType: "nvarchar"),
            //            Password = c.String(maxLength: 128, storeType: "nvarchar"),
            //            IsEncrypted = c.Boolean(nullable: false),
            //            Session = c.String(maxLength: 128, storeType: "nvarchar"),
            //            Authority = c.String(maxLength: 128, storeType: "nvarchar"),
            //            Result = c.Boolean(nullable: false),
            //            ClientIp = c.String(maxLength: 128, storeType: "nvarchar"),
            //            ClientPort = c.Int(nullable: false),
            //            LoginTime = c.DateTime(nullable: false, precision: 0),
            //            LiveTime = c.DateTime(nullable: false, precision: 0),
            //        })
            //    .PrimaryKey(t => t.Id);

            AddColumn("dbo.Bounds", "ZeroX", c => c.Single());
            AddColumn("dbo.Bounds", "ZeroY", c => c.Single());
            AddColumn("dbo.ArchorSettings", "MeasureX", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.ArchorSettings", "MeasureY", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.Dev_CameraInfo", "RtspUrl", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.DevMonitorNodes", "ParseResult", c => c.String(maxLength: 10, storeType: "nvarchar"));
            AddColumn("dbo.KKSCodes", "RawCode", c => c.String(nullable: false, maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.DevInfoes", "KKS", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.Areas", "KKS", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.Personnels", "WorkNumber", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AlterColumn("dbo.DevMonitorNodes", "Time", c => c.Long(nullable: false));
            AlterColumn("dbo.PatrolPoints", "KksCode", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.PatrolPointItems", "KksCode", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.KKSCodes", "Code", c => c.String(nullable: false, maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.KKSCodes", "ParentCode", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.NodeKKS", "KKS", c => c.String(nullable: false, maxLength: 128, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NodeKKS", "KKS", c => c.String(nullable: false, maxLength: 32, storeType: "nvarchar"));
            AlterColumn("dbo.KKSCodes", "ParentCode", c => c.String(maxLength: 32, storeType: "nvarchar"));
            AlterColumn("dbo.KKSCodes", "Code", c => c.String(nullable: false, maxLength: 32, storeType: "nvarchar"));
            AlterColumn("dbo.PatrolPointItems", "KksCode", c => c.String(maxLength: 32, storeType: "nvarchar"));
            AlterColumn("dbo.PatrolPoints", "KksCode", c => c.String(maxLength: 16, storeType: "nvarchar"));
            AlterColumn("dbo.DevMonitorNodes", "Time", c => c.Int(nullable: false));
            AlterColumn("dbo.Personnels", "WorkNumber", c => c.Int());
            AlterColumn("dbo.Areas", "KKS", c => c.String(maxLength: 32, storeType: "nvarchar"));
            AlterColumn("dbo.DevInfoes", "KKS", c => c.String(maxLength: 32, storeType: "nvarchar"));
            DropColumn("dbo.KKSCodes", "RawCode");
            DropColumn("dbo.DevMonitorNodes", "ParseResult");
            DropColumn("dbo.Dev_CameraInfo", "RtspUrl");
            DropColumn("dbo.ArchorSettings", "MeasureY");
            DropColumn("dbo.ArchorSettings", "MeasureX");
            DropColumn("dbo.Bounds", "ZeroY");
            DropColumn("dbo.Bounds", "ZeroX");
            //DropTable("dbo.Users");
            //DropTable("dbo.CameraAlarmJsons");
        }
    }
}
