namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PageDefinitionadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PageDefinition",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.Int(nullable: false),
						Name = c.String(nullable: false, maxLength: 255),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Blueprint_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blueprint", t => t.Blueprint_Id)
                .Index(t => t.Blueprint_Id);

            Sql(@"
                --Feature Diagnostic
                insert into PageDefinition
                ([Key], Blueprint_Id, Name, IsDeleted, DateCreated)
                values
                (1, 5, 'Result', 0, GetDate())
                insert into PageDefinition
                ([Key], Blueprint_Id, Name, IsDeleted, DateCreated)
                values
                (0, 5, 'Quiz', 0, GetDate())

                --Feature-rfi
                insert into PageDefinition
                ([Key], Blueprint_Id, Name, IsDeleted, DateCreated)
                values
                (1, 10, 'Result', 0, GetDate())
                insert into PageDefinition
                ([Key], Blueprint_Id, Name, IsDeleted, DateCreated)
                values
                (0, 10, 'RFI', 0, GetDate())

                --Feature-gallery-video
                insert into PageDefinition
                ([Key], Blueprint_Id, Name, IsDeleted, DateCreated)
                values
                (1, 7, 'Gallery Item', 0, GetDate())
                insert into PageDefinition
                ([Key], Blueprint_Id, Name, IsDeleted, DateCreated)
                values
                (0, 7, 'Gallery', 0, GetDate())
            ");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PageDefinition", new[] { "Blueprint_Id" });
            DropForeignKey("dbo.PageDefinition", "Blueprint_Id", "dbo.Blueprint");
            DropTable("dbo.PageDefinition");
        }
    }
}
