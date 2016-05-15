namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlacementandAd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ad", "AppPlatformPlacement_Id", "dbo.AppPlatformPlacement");
            DropForeignKey("dbo.Ad", "PlatformPlacement_Id", "dbo.PlatformPlacement");
            DropForeignKey("dbo.Ad", "NetworkPlacement_Id", "dbo.NetworkPlatformPlacement");
            DropForeignKey("dbo.Ad", "Network_Id", "dbo.Network");
            DropForeignKey("dbo.Ad", "App_Id", "dbo.App");
            DropForeignKey("dbo.AppPlatformPlacement", "AppPlatform_Id", "dbo.AppPlatform");
            DropForeignKey("dbo.AppPlatformPlacement", "Placement_Id", "dbo.Placement");
            DropForeignKey("dbo.PlatformPlacement", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.PlatformPlacement", "Placement_Id", "dbo.Placement");
            DropForeignKey("dbo.NetworkPlatformPlacement", "Network_Id", "dbo.Network");
            DropForeignKey("dbo.NetworkPlatformPlacement", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.NetworkPlatformPlacement", "Placement_Id", "dbo.Placement");
            AddColumn("dbo.Ad", "Placement_Id", c => c.Int());
            AddColumn("dbo.Placement", "Network_Id", c => c.Int());
            AddColumn("dbo.Placement", "App_Id", c => c.Int());
            AddColumn("dbo.Placement", "Publisher_Id", c => c.Int());
            AddForeignKey("dbo.Ad", "Placement_Id", "dbo.Placement", "Id");
            AddForeignKey("dbo.Placement", "Network_Id", "dbo.Network", "Id");
            AddForeignKey("dbo.Placement", "App_Id", "dbo.App", "Id");
            AddForeignKey("dbo.Placement", "Publisher_Id", "dbo.Publisher", "Id");
            CreateIndex("dbo.Ad", "Placement_Id");
            CreateIndex("dbo.Placement", "Network_Id");
            CreateIndex("dbo.Placement", "App_Id");
            CreateIndex("dbo.Placement", "Publisher_Id");

			// transfer placement data to placement and ad
			Sql(@"
update Ad set
		Placement_Id = coalesce(Ad.Placement_Id, AppPlatformPlacement.Placement_Id)
	from AppPlatformPlacement
	left join AppPlatform ap on ap.Id = AppPlatformPlacement.AppPlatform_Id
	where AppPlatformPlacement.Id = Ad.AppPlatformPlacement_Id

update Ad set
		Placement_Id = coalesce(Ad.Placement_Id, NetworkPlatformPlacement.Placement_Id)
	from NetworkPlatformPlacement
	where NetworkPlatformPlacement.Id = Ad.NetworkPlacement_Id

update Ad set
		Campaign_Id = Experience.Campaign_Id,
		Platform_Id = Experience.Platform_Id
	from Experience 
	inner join DistributionPoint on DistributionPoint.Id = Experience.DistributionPoint_Id
	where Experience.Id = Ad.Experience_Id

update Placement set
		Network_Id = Ad.Network_Id,
		App_Id = Ad.App_Id
	from Ad
	inner join Experience on Experience.ID = Ad.Experience_Id
	inner join DistributionPoint on DistributionPoint.Id = Experience.DistributionPoint_Id
	where Ad.Placement_Id = Placement.Id
");

			//DropIndex("dbo.Ad", new[] { "AppPlatformPlacement_Id" });
			//DropIndex("dbo.Ad", new[] { "PlatformPlacement_Id" });
			//DropIndex("dbo.Ad", new[] { "NetworkPlacement_Id" });
			//DropIndex("dbo.Ad", new[] { "Network_Id" });
			//DropIndex("dbo.Ad", new[] { "App_Id" });
			//DropIndex("dbo.AppPlatformPlacement", new[] { "AppPlatform_Id" });
			//DropIndex("dbo.AppPlatformPlacement", new[] { "Placement_Id" });
			//DropIndex("dbo.PlatformPlacement", new[] { "Platform_Id" });
			//DropIndex("dbo.PlatformPlacement", new[] { "Placement_Id" });
			//DropIndex("dbo.NetworkPlatformPlacement", new[] { "Network_Id" });
			//DropIndex("dbo.NetworkPlatformPlacement", new[] { "Platform_Id" });
			//DropIndex("dbo.NetworkPlatformPlacement", new[] { "Placement_Id" });
			//DropColumn("dbo.Ad", "AppPlatformPlacement_Id");
			//DropColumn("dbo.Ad", "PlatformPlacement_Id");
			//DropColumn("dbo.Ad", "NetworkPlacement_Id");
			//DropColumn("dbo.Ad", "Network_Id");
			//DropColumn("dbo.Ad", "App_Id");
			//DropTable("dbo.AppPlatformPlacement");
			//DropTable("dbo.PlatformPlacement");
			//DropTable("dbo.NetworkPlatformPlacement");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.NetworkPlatformPlacement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Network_Id = c.Int(nullable: false),
                        Platform_Id = c.Int(),
                        Placement_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlatformPlacement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Platform_Id = c.Int(nullable: false),
                        Placement_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppPlatformPlacement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        AppPlatform_Id = c.Int(nullable: false),
                        Placement_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Ad", "App_Id", c => c.Int());
            AddColumn("dbo.Ad", "Network_Id", c => c.Int());
            AddColumn("dbo.Ad", "NetworkPlacement_Id", c => c.Int());
            AddColumn("dbo.Ad", "PlatformPlacement_Id", c => c.Int());
            AddColumn("dbo.Ad", "AppPlatformPlacement_Id", c => c.Int());
            DropIndex("dbo.Placement", new[] { "Publisher_Id" });
            DropIndex("dbo.Placement", new[] { "App_Id" });
            DropIndex("dbo.Placement", new[] { "Network_Id" });
            DropIndex("dbo.Ad", new[] { "Placement_Id" });
            DropForeignKey("dbo.Placement", "Publisher_Id", "dbo.Publisher");
            DropForeignKey("dbo.Placement", "App_Id", "dbo.App");
            DropForeignKey("dbo.Placement", "Network_Id", "dbo.Network");
            DropForeignKey("dbo.Ad", "Placement_Id", "dbo.Placement");
            DropColumn("dbo.Placement", "Publisher_Id");
            DropColumn("dbo.Placement", "App_Id");
            DropColumn("dbo.Placement", "Network_Id");
            DropColumn("dbo.Ad", "Placement_Id");
            CreateIndex("dbo.NetworkPlatformPlacement", "Placement_Id");
            CreateIndex("dbo.NetworkPlatformPlacement", "Platform_Id");
            CreateIndex("dbo.NetworkPlatformPlacement", "Network_Id");
            CreateIndex("dbo.PlatformPlacement", "Placement_Id");
            CreateIndex("dbo.PlatformPlacement", "Platform_Id");
            CreateIndex("dbo.AppPlatformPlacement", "Placement_Id");
            CreateIndex("dbo.AppPlatformPlacement", "AppPlatform_Id");
            CreateIndex("dbo.Ad", "App_Id");
            CreateIndex("dbo.Ad", "Network_Id");
            CreateIndex("dbo.Ad", "NetworkPlacement_Id");
            CreateIndex("dbo.Ad", "PlatformPlacement_Id");
            CreateIndex("dbo.Ad", "AppPlatformPlacement_Id");
            AddForeignKey("dbo.NetworkPlatformPlacement", "Placement_Id", "dbo.Placement", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NetworkPlatformPlacement", "Platform_Id", "dbo.Platform", "Id");
            AddForeignKey("dbo.NetworkPlatformPlacement", "Network_Id", "dbo.Network", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PlatformPlacement", "Placement_Id", "dbo.Placement", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PlatformPlacement", "Platform_Id", "dbo.Platform", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AppPlatformPlacement", "Placement_Id", "dbo.Placement", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AppPlatformPlacement", "AppPlatform_Id", "dbo.AppPlatform", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Ad", "App_Id", "dbo.App", "Id");
            AddForeignKey("dbo.Ad", "Network_Id", "dbo.Network", "Id");
            AddForeignKey("dbo.Ad", "NetworkPlacement_Id", "dbo.NetworkPlatformPlacement", "Id");
            AddForeignKey("dbo.Ad", "PlatformPlacement_Id", "dbo.PlatformPlacement", "Id");
            AddForeignKey("dbo.Ad", "AppPlatformPlacement_Id", "dbo.AppPlatformPlacement", "Id");
        }
    }
}
