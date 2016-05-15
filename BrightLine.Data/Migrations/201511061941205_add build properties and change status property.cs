namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addbuildpropertiesandchangestatusproperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NightwatchTransaction", "Status", c => c.String());
            AddColumn("dbo.NightwatchTransaction", "BuildVersion", c => c.String());
            AddColumn("dbo.NightwatchTransaction", "BuildCommitHash", c => c.String());
            DropColumn("dbo.NightwatchTransaction", "Success");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NightwatchTransaction", "Success", c => c.Boolean(nullable: false));
            DropColumn("dbo.NightwatchTransaction", "BuildCommitHash");
            DropColumn("dbo.NightwatchTransaction", "BuildVersion");
            DropColumn("dbo.NightwatchTransaction", "Status");
        }
    }
}
