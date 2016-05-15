namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdIsReported : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ad", "IsReported", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ad", "IsReported");
        }
    }
}
