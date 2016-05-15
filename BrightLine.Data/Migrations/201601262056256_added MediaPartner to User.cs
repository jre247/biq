namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedMediaPartnertoUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "MediaPartner_Id", c => c.Int());
            CreateIndex("dbo.User", "MediaPartner_Id");
            AddForeignKey("dbo.User", "MediaPartner_Id", "dbo.MediaPartner", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "MediaPartner_Id", "dbo.MediaPartner");
            DropIndex("dbo.User", new[] { "MediaPartner_Id" });
            DropColumn("dbo.User", "MediaPartner_Id");
        }
    }
}
