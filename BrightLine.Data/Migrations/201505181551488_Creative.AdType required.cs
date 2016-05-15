namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreativeAdTyperequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Creative", "AdType_Id", "dbo.AdType");
            DropIndex("dbo.Creative", new[] { "AdType_Id" });
            AlterColumn("dbo.Creative", "AdType_Id", c => c.Int(nullable: false));
            AddForeignKey("dbo.Creative", "AdType_Id", "dbo.AdType", "Id", cascadeDelete: true);
            CreateIndex("dbo.Creative", "AdType_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Creative", new[] { "AdType_Id" });
            DropForeignKey("dbo.Creative", "AdType_Id", "dbo.AdType");
            AlterColumn("dbo.Creative", "AdType_Id", c => c.Int());
            CreateIndex("dbo.Creative", "AdType_Id");
            AddForeignKey("dbo.Creative", "AdType_Id", "dbo.AdType", "Id");
        }
    }
}
