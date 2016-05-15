namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdCMSPublishandCMSPublishStatusentities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CmsPublish",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PublishId = c.Guid(nullable: false),
                        TargetEnvironment = c.String(nullable: false),
                        UserId = c.Int(nullable: false),
                        IPAddress = c.String(nullable: false),
                        TimeStarted = c.DateTime(nullable: false),
                        TimeEnded = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Campaign_Id = c.Int(nullable: false),
                        CmsPublishStatus_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaign", t => t.Campaign_Id, cascadeDelete: true)
                .ForeignKey("dbo.CmsPublishStatus", t => t.CmsPublishStatus_Id, cascadeDelete: true)
                .Index(t => t.Campaign_Id)
                .Index(t => t.CmsPublishStatus_Id);
            
            CreateTable(
                "dbo.CmsPublishStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CmsPublish", "CmsPublishStatus_Id", "dbo.CmsPublishStatus");
            DropForeignKey("dbo.CmsPublish", "Campaign_Id", "dbo.Campaign");
            DropIndex("dbo.CmsPublish", new[] { "CmsPublishStatus_Id" });
            DropIndex("dbo.CmsPublish", new[] { "Campaign_Id" });
            DropTable("dbo.CmsPublishStatus");
            DropTable("dbo.CmsPublish");
        }
    }
}
