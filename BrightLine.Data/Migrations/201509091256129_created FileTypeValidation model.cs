namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdFileTypeValidationmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileTypeValidation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        FileType_Id = c.Int(),
                        Validation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileType", t => t.FileType_Id)
                .ForeignKey("dbo.Validation", t => t.Validation_Id)
                .Index(t => t.FileType_Id)
                .Index(t => t.Validation_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FileTypeValidation", "Validation_Id", "dbo.Validation");
            DropForeignKey("dbo.FileTypeValidation", "FileType_Id", "dbo.FileType");
            DropIndex("dbo.FileTypeValidation", new[] { "Validation_Id" });
            DropIndex("dbo.FileTypeValidation", new[] { "FileType_Id" });
            DropTable("dbo.FileTypeValidation");
        }
    }
}
