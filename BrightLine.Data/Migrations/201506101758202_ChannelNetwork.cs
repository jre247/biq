namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChannelNetwork : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Channel", "IsNetwork", c => c.Boolean());
			Sql("update channel set isnetwork = 1 where network_id is not null");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Channel", "IsNetwork");
        }
    }
}
