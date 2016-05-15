namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class channelIsNetworknotnullableboolanymore : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Channel", "IsNetwork", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Channel", "IsNetwork", c => c.Boolean());
        }
    }
}
