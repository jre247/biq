namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ad", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ad", "Name");
        }
    }
}
