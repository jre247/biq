namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Resourcenewcolumnsadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StorageSource",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Resource", "MD5Hash", c => c.String());
            AddColumn("dbo.Resource", "Duration", c => c.Int());
            AddColumn("dbo.Resource", "Parent_Id", c => c.Int());
            AddColumn("dbo.Resource", "ResourceType_Id", c => c.Int());
            AddColumn("dbo.Resource", "StorageSource_Id", c => c.Int());
            AddForeignKey("dbo.Resource", "Parent_Id", "dbo.Resource", "Id");
            AddForeignKey("dbo.Resource", "ResourceType_Id", "dbo.ResourceType", "Id");
            AddForeignKey("dbo.Resource", "StorageSource_Id", "dbo.StorageSource", "Id");
            CreateIndex("dbo.Resource", "Parent_Id");
            CreateIndex("dbo.Resource", "ResourceType_Id");
            CreateIndex("dbo.Resource", "StorageSource_Id");

        }
        
        public override void Down()
        {
            DropIndex("dbo.Resource", new[] { "StorageSource_Id" });
            DropIndex("dbo.Resource", new[] { "ResourceType_Id" });
            DropIndex("dbo.Resource", new[] { "Parent_Id" });
            DropForeignKey("dbo.Resource", "StorageSource_Id", "dbo.StorageSource");
            DropForeignKey("dbo.Resource", "ResourceType_Id", "dbo.ResourceType");
            DropForeignKey("dbo.Resource", "Parent_Id", "dbo.Resource");
            DropColumn("dbo.Resource", "StorageSource_Id");
            DropColumn("dbo.Resource", "ResourceType_Id");
            DropColumn("dbo.Resource", "Parent_Id");
            DropColumn("dbo.Resource", "Duration");
            DropColumn("dbo.Resource", "MD5Hash");
            DropTable("dbo.StorageSource");
        }
    }
}
