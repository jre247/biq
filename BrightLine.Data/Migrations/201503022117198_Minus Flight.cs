namespace BrightLine.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class MinusFlight : DbMigration
	{
		public override void Up()
		{
			DropForeignKey("dbo.Ad", "Flight_Id", "dbo.Flight");
			DropForeignKey("dbo.AdResult", "Flight_Id", "dbo.Flight");
			DropForeignKey("dbo.ImportAdResult", "Flight_Id", "dbo.Flight");
			DropForeignKey("dbo.Flight", "Ad_Id", "dbo.Ad");
			DropForeignKey("dbo.Flight", "Experience_Id", "dbo.Experience");
			DropIndex("dbo.Ad", new[] { "Flight_Id" });
			DropIndex("dbo.AdResult", new[] { "Flight_Id" });
			DropIndex("dbo.ImportAdResult", new[] { "Flight_Id" });
			DropIndex("dbo.Flight", new[] { "Ad_Id" });
			DropIndex("dbo.Flight", new[] { "Experience_Id" });
			// need to make sure all the flight dates get transferred over but don't smash ad dates.
			Sql(@"
update [dbo].[Ad] set
	BeginDate = flight.BeginDate
	from [dbo].[Flight] flight
	where ad.BeginDate is null and flight.Id = Ad.Flight_Id
update [dbo].[Ad] set
	EndDate = flight.EndDate
	from [dbo].[Flight] flight
	where ad.EndDate is null and flight.Id = Ad.Flight_Id
");
			DropColumn("dbo.Ad", "Flight_Id");
			DropColumn("dbo.AdResult", "Flight_Id");
			DropColumn("dbo.ImportAdResult", "Flight_Id");
			DropTable("dbo.Flight");
		}

		public override void Down()
		{
			CreateTable(
				"dbo.Flight",
				c => new
					{
						Id = c.Int(nullable: false, identity: true),
						BeginDate = c.DateTime(),
						EndDate = c.DateTime(),
						ExpectedBeginDate = c.DateTime(nullable: false),
						ExpectedEndDate = c.DateTime(nullable: false),
						Ad_Id = c.Int(nullable: false),
						IsDeleted = c.Boolean(nullable: false),
						DateCreated = c.DateTime(nullable: false),
						DateUpdated = c.DateTime(),
						DateDeleted = c.DateTime(),
						Display = c.String(),
						ShortDisplay = c.String(),
						Experience_Id = c.Int(),
					})
				.PrimaryKey(t => t.Id);

			AddColumn("dbo.ImportAdResult", "Flight_Id", c => c.Int());
			AddColumn("dbo.AdResult", "Flight_Id", c => c.Int());
			AddColumn("dbo.Ad", "Flight_Id", c => c.Int());
			CreateIndex("dbo.Flight", "Experience_Id");
			CreateIndex("dbo.Flight", "Ad_Id");
			CreateIndex("dbo.ImportAdResult", "Flight_Id");
			CreateIndex("dbo.AdResult", "Flight_Id");
			CreateIndex("dbo.Ad", "Flight_Id");
			AddForeignKey("dbo.Flight", "Experience_Id", "dbo.Experience", "Id");
			AddForeignKey("dbo.Flight", "Ad_Id", "dbo.Ad", "Id", cascadeDelete: true);
			AddForeignKey("dbo.ImportAdResult", "Flight_Id", "dbo.Flight", "Id");
			AddForeignKey("dbo.AdResult", "Flight_Id", "dbo.Flight", "Id");
			AddForeignKey("dbo.Ad", "Flight_Id", "dbo.Flight", "Id");
		}
	}
}
