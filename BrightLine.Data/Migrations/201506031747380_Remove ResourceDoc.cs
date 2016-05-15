namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveResourceDoc : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ResourceDoc", "Id", "dbo.Resource");
            DropForeignKey("dbo.ResourceDoc", "ExecutionStage_Id", "dbo.ExecutionStage");
            DropForeignKey("dbo.ResourceDoc", "ProductionStep_Id", "dbo.ProductionStep");
            DropForeignKey("dbo.ResourceDoc", "Platform_Id", "dbo.Platform");
            DropIndex("dbo.ResourceDoc", new[] { "Id" });
            DropIndex("dbo.ResourceDoc", new[] { "ExecutionStage_Id" });
            DropIndex("dbo.ResourceDoc", new[] { "ProductionStep_Id" });
            DropIndex("dbo.ResourceDoc", new[] { "Platform_Id" });
            DropTable("dbo.ResourceDoc");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ResourceDoc",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ExecutionStage_Id = c.Int(),
                        ProductionStep_Id = c.Int(),
                        Platform_Id = c.Int(),
                        IsAllPlatforms = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ResourceDoc", "Platform_Id");
            CreateIndex("dbo.ResourceDoc", "ProductionStep_Id");
            CreateIndex("dbo.ResourceDoc", "ExecutionStage_Id");
            CreateIndex("dbo.ResourceDoc", "Id");
            AddForeignKey("dbo.ResourceDoc", "Platform_Id", "dbo.Platform", "Id");
            AddForeignKey("dbo.ResourceDoc", "ProductionStep_Id", "dbo.ProductionStep", "Id");
            AddForeignKey("dbo.ResourceDoc", "ExecutionStage_Id", "dbo.ExecutionStage", "Id");
            AddForeignKey("dbo.ResourceDoc", "Id", "dbo.Resource", "Id");
        }
    }
}
