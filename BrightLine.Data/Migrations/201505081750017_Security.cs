namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

	public partial class Security : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SecurableAction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Operation = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Target_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SecurableArea", t => t.Target_Id)
                .Index(t => t.Target_Id);
            
            CreateTable(
                "dbo.SecurableArea",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Route = c.String(),
                        Url = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        SecurableArea_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SecurableArea", t => t.SecurableArea_Id)
                .Index(t => t.SecurableArea_Id);
            
            AddColumn("dbo.User", "SecurableAction_Id", c => c.Int());
            AddColumn("dbo.User", "SecurableArea_Id", c => c.Int());
            AddColumn("dbo.Role", "SecurableAction_Id", c => c.Int());
            AddColumn("dbo.Role", "SecurableArea_Id", c => c.Int());
            AddForeignKey("dbo.User", "SecurableAction_Id", "dbo.SecurableAction", "Id");
            AddForeignKey("dbo.User", "SecurableArea_Id", "dbo.SecurableArea", "Id");
            AddForeignKey("dbo.Role", "SecurableAction_Id", "dbo.SecurableAction", "Id");
            AddForeignKey("dbo.Role", "SecurableArea_Id", "dbo.SecurableArea", "Id");
            CreateIndex("dbo.User", "SecurableAction_Id");
            CreateIndex("dbo.User", "SecurableArea_Id");
            CreateIndex("dbo.Role", "SecurableAction_Id");
            CreateIndex("dbo.Role", "SecurableArea_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SecurableArea", new[] { "SecurableArea_Id" });
            DropIndex("dbo.SecurableAction", new[] { "Target_Id" });
            DropIndex("dbo.Role", new[] { "SecurableArea_Id" });
            DropIndex("dbo.Role", new[] { "SecurableAction_Id" });
            DropIndex("dbo.User", new[] { "SecurableArea_Id" });
            DropIndex("dbo.User", new[] { "SecurableAction_Id" });
            DropForeignKey("dbo.SecurableArea", "SecurableArea_Id", "dbo.SecurableArea");
            DropForeignKey("dbo.SecurableAction", "Target_Id", "dbo.SecurableArea");
            DropForeignKey("dbo.Role", "SecurableArea_Id", "dbo.SecurableArea");
            DropForeignKey("dbo.Role", "SecurableAction_Id", "dbo.SecurableAction");
            DropForeignKey("dbo.User", "SecurableArea_Id", "dbo.SecurableArea");
            DropForeignKey("dbo.User", "SecurableAction_Id", "dbo.SecurableAction");
            DropColumn("dbo.Role", "SecurableArea_Id");
            DropColumn("dbo.Role", "SecurableAction_Id");
            DropColumn("dbo.User", "SecurableArea_Id");
            DropColumn("dbo.User", "SecurableAction_Id");
            DropTable("dbo.SecurableArea");
            DropTable("dbo.SecurableAction");
        }
    }
}
