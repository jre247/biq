namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSettingDefinitionstoBlueprint : DbMigration
    {
        public override void Up()
        {
			AddColumn("dbo.CmsSettingDefinition", "Blueprint_Id", c => c.Int());
			CreateIndex("dbo.CmsSettingDefinition", "Blueprint_Id");
			AddForeignKey("dbo.CmsSettingDefinition", "Blueprint_Id", "dbo.Blueprint", "Id");
        }
        
        public override void Down()
        {
			DropForeignKey("dbo.CmsSettingDefinition", "Blueprint_Id", "dbo.Blueprint");
			DropIndex("dbo.CmsSettingDefinition", new[] { "Blueprint_Id" });
			DropColumn("dbo.CmsSettingDefinition", "Blueprint_Id");
        }
    }
}
