namespace DAL.BaseDataDbMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.users", "dep_name", c => c.String(maxLength: 256, storeType: "nvarchar"));
            DropColumn("dbo.users", "dept_name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.users", "dept_name", c => c.String(maxLength: 256, storeType: "nvarchar"));
            DropColumn("dbo.users", "dep_name");
        }
    }
}
