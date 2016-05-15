namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppPlatformIdentifier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppPlatform", "Identifier", c => c.String(maxLength: 12));
			// Add a unique identifier to each app before applying the unique index
			Sql("update dbo.AppPlatform set Identifier = substring(convert(nvarchar(36),newid()), 25, 12)");
            CreateIndex("dbo.AppPlatform", "Identifier", unique: true, name: "IX_AppPlatform_Identifier");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AppPlatform", "IX_AppPlatform_Identifier");
            DropColumn("dbo.AppPlatform", "Identifier");
        }
    }
}
