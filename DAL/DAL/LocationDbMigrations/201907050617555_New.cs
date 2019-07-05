namespace DAL.LocationDbMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New : DbMigration
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
            
            //AddColumn("dbo.Bounds", "ZeroX", c => c.Single());
            //AddColumn("dbo.Bounds", "ZeroY", c => c.Single());
            AddColumn("dbo.Dev_CameraInfo", "RtspUrl", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.DevMonitorNodes", "ParseResult", c => c.String(maxLength: 10, storeType: "nvarchar"));
            AddColumn("dbo.KKSCodes", "RawCode", c => c.String(nullable: false, maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.DevMonitorNodes", "Time", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DevMonitorNodes", "Time", c => c.Int(nullable: false));
            DropColumn("dbo.KKSCodes", "RawCode");
            DropColumn("dbo.DevMonitorNodes", "ParseResult");
            DropColumn("dbo.Dev_CameraInfo", "RtspUrl");
            //DropColumn("dbo.Bounds", "ZeroY");
            //DropColumn("dbo.Bounds", "ZeroX");
            //DropTable("dbo.CameraAlarmJsons");
        }
    }
}
