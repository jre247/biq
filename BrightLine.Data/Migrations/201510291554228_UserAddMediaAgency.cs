namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddMediaAgency : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "MediaAgency_Id", c => c.Int());
            CreateIndex("dbo.User", "MediaAgency_Id");
            AddForeignKey("dbo.User", "MediaAgency_Id", "dbo.Agency", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "MediaAgency_Id", "dbo.Agency");
            DropIndex("dbo.User", new[] { "MediaAgency_Id" });
            DropColumn("dbo.User", "MediaAgency_Id");
        }
    }
}
