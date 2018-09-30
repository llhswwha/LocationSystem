namespace Location.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Meterials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        qty = c.Int(nullable: false),
                        unit = c.String(),
                        phtId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PhysicalTopologies", t => t.phtId)
                .Index(t => t.phtId);
            
            AddColumn("dbo.DevInfoes", "Id", c => c.Int());
            AddColumn("dbo.PhysicalTopologies", "NodekksId", c => c.Int());
            CreateIndex("dbo.PhysicalTopologies", "NodekksId");
            AddForeignKey("dbo.PhysicalTopologies", "NodekksId", "dbo.NodeKKS", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Meterials", "phtId", "dbo.PhysicalTopologies");
            DropForeignKey("dbo.PhysicalTopologies", "NodekksId", "dbo.NodeKKS");
            DropIndex("dbo.Meterials", new[] { "phtId" });
            DropIndex("dbo.PhysicalTopologies", new[] { "NodekksId" });
            DropColumn("dbo.PhysicalTopologies", "NodekksId");
            DropColumn("dbo.DevInfoes", "Id");
            DropTable("dbo.Meterials");
        }
    }
}
