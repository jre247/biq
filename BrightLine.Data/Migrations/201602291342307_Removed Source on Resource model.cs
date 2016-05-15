namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedSourceonResourcemodel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Resource", "Source");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Resource", "Source", c => c.String());
        }
    }
}
