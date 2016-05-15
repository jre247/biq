namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdAdTag : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdTag", "Ad_Id", "dbo.Ad");
            DropIndex("dbo.AdTag", new[] { "Ad_Id" });
            AddColumn("dbo.Ad", "AdTag_Id", c => c.Int());
            AlterColumn("dbo.AdTag", "Ad_Id", c => c.Int(nullable: false));
            AddForeignKey("dbo.Ad", "AdTag_Id", "dbo.AdTag", "Id");
            AddForeignKey("dbo.AdTag", "Ad_Id", "dbo.Ad", "Id", cascadeDelete: true);
            CreateIndex("dbo.Ad", "AdTag_Id");
            CreateIndex("dbo.AdTag", "Ad_Id");
			// set the AdTab Ids in the Ad table.
			Sql(@"update Ad set AdTag_Id = at.Id from (select ati.Id, ati.Ad_Id from AdTag ati) as at where Ad.Id = at.Ad_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AdTag", new[] { "Ad_Id" });
            DropIndex("dbo.Ad", new[] { "AdTag_Id" });
            DropForeignKey("dbo.AdTag", "Ad_Id", "dbo.Ad");
            DropForeignKey("dbo.Ad", "AdTag_Id", "dbo.AdTag");
            AlterColumn("dbo.AdTag", "Ad_Id", c => c.Int());
            DropColumn("dbo.Ad", "AdTag_Id");
            CreateIndex("dbo.AdTag", "Ad_Id");
            AddForeignKey("dbo.AdTag", "Ad_Id", "dbo.Ad", "Id");
        }
    }
}
