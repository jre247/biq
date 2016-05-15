namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Metrictolookup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Metric", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Metric", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.Metric", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Metric", "Description", c => c.String(nullable: false));
            DropColumn("dbo.Metric", "Type");
            DropColumn("dbo.Metric", "Name");
        }
    }
}
