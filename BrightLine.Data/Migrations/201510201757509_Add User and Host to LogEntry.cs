namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserandHosttoLogEntry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogEntry", "User", c => c.String(maxLength: 255));
            AddColumn("dbo.LogEntry", "Host", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LogEntry", "Host");
            DropColumn("dbo.LogEntry", "User");
        }
    }
}
