namespace Location.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PhysicalTopologies", "TestName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PhysicalTopologies", "TestName");
        }
    }
}
