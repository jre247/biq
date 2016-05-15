namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampaignAdsandAdPlatformNetworkApp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ad", "BeginDate", c => c.DateTime());
            AddColumn("dbo.Ad", "EndDate", c => c.DateTime());
            AddColumn("dbo.Ad", "Campaign_Id", c => c.Int());
            AddColumn("dbo.Ad", "Platform_Id", c => c.Int());
            AddColumn("dbo.Ad", "Network_Id", c => c.Int());
            AddColumn("dbo.Ad", "App_Id", c => c.Int());
            AddForeignKey("dbo.Ad", "Campaign_Id", "dbo.Campaign", "Id");
            AddForeignKey("dbo.Ad", "Platform_Id", "dbo.Platform", "Id");
            AddForeignKey("dbo.Ad", "Network_Id", "dbo.Network", "Id");
            AddForeignKey("dbo.Ad", "App_Id", "dbo.App", "Id");
            CreateIndex("dbo.Ad", "Campaign_Id");
            CreateIndex("dbo.Ad", "Platform_Id");
            CreateIndex("dbo.Ad", "Network_Id");
			CreateIndex("dbo.Ad", "App_Id");
			Sql(@"
update [dbo].[Ad]
	set
		[dbo].[Ad].[Platform_Id] = [dbo].[Experience].[Platform_Id],
		[dbo].[Ad].[Campaign_Id] = [dbo].[Experience].[Campaign_Id]
	from [dbo].[Experience]
	where [dbo].[Ad].[Experience_Id] = [dbo].[Experience].[Id]
	
update [dbo].[Ad] set
	[dbo].[Ad].[BeginDate] = coalesce([dbo].[Flight].[BeginDate], [dbo].[Flight].[ExpectedBeginDate]),
	[dbo].[Ad].[EndDate] = coalesce([dbo].[Flight].[EndDate], [dbo].[Flight].[ExpectedEndDate])
	from [dbo].[Flight]
	where [dbo].[Ad].[Flight_Id] = [dbo].[Flight].[Id]
");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Ad", new[] { "App_Id" });
            DropIndex("dbo.Ad", new[] { "Network_Id" });
            DropIndex("dbo.Ad", new[] { "Platform_Id" });
            DropIndex("dbo.Ad", new[] { "Campaign_Id" });
            DropForeignKey("dbo.Ad", "App_Id", "dbo.App");
            DropForeignKey("dbo.Ad", "Network_Id", "dbo.Network");
            DropForeignKey("dbo.Ad", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.Ad", "Campaign_Id", "dbo.Campaign");
            DropColumn("dbo.Ad", "App_Id");
            DropColumn("dbo.Ad", "Network_Id");
            DropColumn("dbo.Ad", "Platform_Id");
            DropColumn("dbo.Ad", "Campaign_Id");
            DropColumn("dbo.Ad", "EndDate");
            DropColumn("dbo.Ad", "BeginDate");
        }
    }
}
