namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSetting_IdtoCmsSettingInstance : DbMigration
    {
        public override void Up()
        {
			DropForeignKey("dbo.CmsSettingInstance", "Id", "dbo.CmsSetting");
			DropIndex("dbo.CmsSettingInstance", new[] { "Id" });
			AddColumn("dbo.CmsSettingInstance", "Setting_Id", c => c.Int());
			AddColumn("dbo.CmsSetting", "CmsSettingInstance_Id", c => c.Int());
			AlterColumn("dbo.CmsSettingInstance", "Name", c => c.String());
			CreateIndex("dbo.CmsSetting", "CmsSettingInstance_Id");
			AddForeignKey("dbo.CmsSetting", "CmsSettingInstance_Id", "dbo.CmsSettingInstance", "Id");
        }
        
        public override void Down()
        {
			DropForeignKey("dbo.CmsSetting", "CmsSettingInstance_Id", "dbo.CmsSettingInstance");
			DropIndex("dbo.CmsSetting", new[] { "CmsSettingInstance_Id" });
			AlterColumn("dbo.CmsSettingInstance", "Name", c => c.Int(nullable: false));
			DropColumn("dbo.CmsSetting", "CmsSettingInstance_Id");
			DropColumn("dbo.CmsSettingInstance", "Setting_Id");
			CreateIndex("dbo.CmsSettingInstance", "Id");
			AddForeignKey("dbo.CmsSettingInstance", "Id", "dbo.CmsSetting", "Id");
        }
    }
}
