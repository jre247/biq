namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CmsModelInstanceaddPublishJsonpropertyCmsSettingInstanceaddPublishJsonproperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmsModelInstance", "PublishedJson", c => c.String());
            AddColumn("dbo.CmsSettingInstance", "PublishedJson", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CmsSettingInstance", "PublishedJson");
            DropColumn("dbo.CmsModelInstance", "PublishedJson");
        }
    }
}
