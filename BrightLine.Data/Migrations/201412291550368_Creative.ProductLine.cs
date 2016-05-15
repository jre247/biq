namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreativeProductLine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Creative", "ProductLine_Id", c => c.Int());
            AddForeignKey("dbo.Creative", "ProductLine_Id", "dbo.ProductLine", "Id");
            CreateIndex("dbo.Creative", "ProductLine_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Creative", new[] { "ProductLine_Id" });
            DropForeignKey("dbo.Creative", "ProductLine_Id", "dbo.ProductLine");
            DropColumn("dbo.Creative", "ProductLine_Id");
        }
    }
}
