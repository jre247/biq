using System.Data.Entity.Migrations;

namespace BrightLine.Data.Migrations
{
	public partial class Creativepropertiesandcompositekey : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Creative", "Description", c => c.String());
			AddColumn("dbo.Creative", "Height", c => c.Int());
			AddColumn("dbo.Creative", "Width", c => c.Int());
			AddColumn("dbo.Creative", "AdType_Id", c => c.Int());
			AddColumn("dbo.Creative", "AdFunction_Id", c => c.Int());
			AlterColumn("dbo.Creative", "Name", c => c.String(nullable: false, maxLength: 255, unicode: false));
			AddForeignKey("dbo.Creative", "AdType_Id", "dbo.AdType", "Id");
			AddForeignKey("dbo.Creative", "AdFunction_Id", "dbo.AdFunction", "Id");
			CreateIndex("dbo.Creative", "AdType_Id");
			CreateIndex("dbo.Creative", "AdFunction_Id");
			CreateIndex("dbo.Creative", new[] { "Name", "Campaign_Id" }, unique: true, name: "UX_Creative_Name_Campaign");
		}

		public override void Down()
		{
			DropIndex("dbo.Creative", new[] { "AdFunction_Id" });
			DropIndex("dbo.Creative", new[] { "AdFunction_Id" });
			DropIndex("dbo.Creative", "UX_Creative_Name_Campaign");
			DropForeignKey("dbo.Creative", "AdFunction_Id", "dbo.AdFunction");
			DropForeignKey("dbo.Creative", "AdType_Id", "dbo.AdType");
			AlterColumn("dbo.Creative", "Name", c => c.String(nullable: false));
			DropColumn("dbo.Creative", "AdFunction_Id");
			DropColumn("dbo.Creative", "AdType_Id");
			DropColumn("dbo.Creative", "Width");
			DropColumn("dbo.Creative", "Height");
			DropColumn("dbo.Creative", "Description");
		}
	}
}
