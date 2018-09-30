namespace Location.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PhysicalTopologies", "TestName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PhysicalTopologies", "TestName", c => c.String());
        }
    }
}
