namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveExecutionStageandProductionStep : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ExecutionStage");
            DropTable("dbo.ProductionStep");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductionStep",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsFiltered = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExecutionStage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SortOrder = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
