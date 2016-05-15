namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeliveryGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeliveryGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        ImpressionCount = c.Int(),
                        MediaSpend = c.Double(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Campaign_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaign", t => t.Campaign_Id)
                .Index(t => t.Campaign_Id);
            
            AddColumn("dbo.Ad", "DeliveryGroup_Id", c => c.Int());
            AddForeignKey("dbo.Ad", "DeliveryGroup_Id", "dbo.DeliveryGroup", "Id");
            CreateIndex("dbo.Ad", "DeliveryGroup_Id");
			
			// Set Seed to be 17000
	        Sql(@"DBCC CHECKIDENT ('DeliveryGroup', RESEED, 17000)");
        }
        
        public override void Down()
        {
            DropIndex("dbo.DeliveryGroup", new[] { "Campaign_Id" });
            DropIndex("dbo.Ad", new[] { "DeliveryGroup_Id" });
            DropForeignKey("dbo.DeliveryGroup", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.Ad", "DeliveryGroup_Id", "dbo.DeliveryGroup");
            DropColumn("dbo.Ad", "DeliveryGroup_Id");
            DropTable("dbo.DeliveryGroup");
        }
    }
}
