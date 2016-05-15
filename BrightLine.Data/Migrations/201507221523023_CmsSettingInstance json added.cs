namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CmsSettingInstancejsonadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmsSettingInstance", "Json", c => c.String());
            AlterColumn("dbo.CmsSettingInstance", "Setting_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CmsSettingInstance", "Setting_Id", c => c.Int());
            DropColumn("dbo.CmsSettingInstance", "Json");
        }
    }
}
