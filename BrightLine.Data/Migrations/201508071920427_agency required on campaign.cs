namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class agencyrequiredoncampaign : DbMigration
    {
        public override void Up()
        {
			DropForeignKey("dbo.Campaign", "Agency_Id", "dbo.Agency");
			DropIndex("dbo.Campaign", new[] { "Agency_Id" });
			AlterColumn("dbo.Campaign", "Agency_Id", c => c.Int(nullable: false));
			CreateIndex("dbo.Campaign", "Agency_Id");
			AddForeignKey("dbo.Campaign", "Agency_Id", "dbo.Agency", "Id", cascadeDelete: true);
		}
        
        public override void Down()
        {
			DropForeignKey("dbo.Campaign", "Agency_Id", "dbo.Agency");
			DropIndex("dbo.Campaign", new[] { "Agency_Id" });
			AlterColumn("dbo.Campaign", "Agency_Id", c => c.Int());
			CreateIndex("dbo.Campaign", "Agency_Id");
			AddForeignKey("dbo.Campaign", "Agency_Id", "dbo.Agency", "Id");
        }
    }
}
