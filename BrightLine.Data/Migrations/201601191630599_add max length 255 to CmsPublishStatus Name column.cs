namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmaxlength255toCmsPublishStatusNamecolumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CmsPublishStatus", "Name", c => c.String(maxLength: 255, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CmsPublishStatus", "Name", c => c.String());
        }
    }
}
