namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAdTagPlacement : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdTag", "Placement_Id", "dbo.Placement");
            //DropIndex("dbo.AdTag", new[] { "Placement_Id" });
            DropColumn("dbo.AdTag", "Placement_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdTag", "Placement_Id", c => c.Int());
            CreateIndex("dbo.AdTag", "Placement_Id");
            AddForeignKey("dbo.AdTag", "Placement_Id", "dbo.Placement", "Id");
        }
    }
}
