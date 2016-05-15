namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeAdGroup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ad", "AdGroup_Id", "dbo.AdGroup");
            DropIndex("dbo.Ad", new[] { "AdGroup_Id" });
            DropColumn("dbo.Ad", "AdGroup_Id");
            DropTable("dbo.AdGroup");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AdGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Ad", "AdGroup_Id", c => c.Int());
            CreateIndex("dbo.Ad", "AdGroup_Id");
            AddForeignKey("dbo.Ad", "AdGroup_Id", "dbo.AdGroup", "Id");
        }
    }
}
