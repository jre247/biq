namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InternalpropertyaddedtoUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Internal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "Internal");
        }
    }
}
