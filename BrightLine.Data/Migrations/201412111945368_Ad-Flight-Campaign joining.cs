namespace BrightLine.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class AdFlightCampaignjoining : DbMigration
	{
		public override void Up()
		{
			// Create function
			Sql(@"
if not exists (select 1 from sys.objects where name = 'GetCampaignDate' and schema_id = 1 and type = 'FN')
	exec sp_executesql N'create function [dbo].[GetCampaignDate] (@campaignId int, @begin bit)
	returns date
	as
	begin
		declare @date date
		if (@begin = 1)
			select @date = min(BeginDate)
				from [dbo].[Flight]
				where Experience_Id in (select Id from [dbo].[Experience] where Campaign_Id = @campaignId)
		else
			select @date = max(EndDate)
				from [dbo].[Flight]
				where Experience_Id in (select Id from [dbo].[Experience] where Campaign_Id = @campaignId)
		return @date;
	end'", true);

			DropForeignKey("dbo.Flight", "ProductLine_Id", "dbo.ProductLine");
			DropIndex("dbo.Flight", new[] { "ProductLine_Id" });
			Sql(@"alter table [dbo].[Campaign] add [BeginDate] as ([dbo].[GetCampaignDate](Id, 1))");
			Sql(@"alter table [dbo].[Campaign] add [EndDate] as ([dbo].[GetCampaignDate](Id, 0))");
			AddColumn("dbo.Flight", "Ad_Id", c => c.Int());
			AddForeignKey("dbo.Flight", "Ad_Id", "dbo.Ad", "Id", cascadeDelete: true);
			CreateIndex("dbo.Flight", "Ad_Id");
			DropColumn("dbo.Flight", "ProductLine_Id");
		}

		public override void Down()
		{
			Sql(@"drop function [dbo].[GetCampaignDate]");
			AddColumn("dbo.Flight", "ProductLine_Id", c => c.Int());
			DropIndex("dbo.Flight", new[] { "Ad_Id" });
			DropForeignKey("dbo.Flight", "Ad_Id", "dbo.Ad");
			DropColumn("dbo.Flight", "Ad_Id");
			DropColumn("dbo.Campaign", "EndDate");
			DropColumn("dbo.Campaign", "BeginDate");
			CreateIndex("dbo.Flight", "ProductLine_Id");
			AddForeignKey("dbo.Flight", "ProductLine_Id", "dbo.ProductLine", "Id");
		}
	}
}
