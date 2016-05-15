namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CmsModelInstanceaddname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmsModelInstance", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CmsModelInstance", "Name");
        }
    }
}
