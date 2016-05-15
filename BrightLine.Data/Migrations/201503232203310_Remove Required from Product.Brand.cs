namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredfromProductBrand : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Product", "Brand_Id", "dbo.Brand");
            DropIndex("dbo.Product", new[] { "Brand_Id" });
            AlterColumn("dbo.Product", "Brand_Id", c => c.Int());
            AddForeignKey("dbo.Product", "Brand_Id", "dbo.Brand", "Id");
            CreateIndex("dbo.Product", "Brand_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Product", new[] { "Brand_Id" });
            DropForeignKey("dbo.Product", "Brand_Id", "dbo.Brand");
            AlterColumn("dbo.Product", "Brand_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Product", "Brand_Id");
            AddForeignKey("dbo.Product", "Brand_Id", "dbo.Brand", "Id", cascadeDelete: true);
        }
    }
}
