namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsUploadedfieldtoResourcemodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resource", "IsUploaded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Resource", "IsUploaded");
        }
    }
}
