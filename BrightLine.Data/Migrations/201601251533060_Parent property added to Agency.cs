namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParentpropertyaddedtoAgency : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agency", "Parent_Id", c => c.Int());
            CreateIndex("dbo.Agency", "Parent_Id");
            AddForeignKey("dbo.Agency", "Parent_Id", "dbo.Agency", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Agency", "Parent_Id", "dbo.Agency");
            DropIndex("dbo.Agency", new[] { "Parent_Id" });
            DropColumn("dbo.Agency", "Parent_Id");
        }
    }
}
