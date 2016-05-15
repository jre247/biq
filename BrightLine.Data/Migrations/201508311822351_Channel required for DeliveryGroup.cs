namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChannelrequiredforDeliveryGroup : DbMigration
    {
        public override void Up()
        {
			Sql(@"
				update DeliveryGroup
				set Channel_Id = 1
				where Channel_Id is null
			");

            DropForeignKey("dbo.DeliveryGroup", "Channel_Id", "dbo.Channel");
            DropIndex("dbo.DeliveryGroup", new[] { "Channel_Id" });
            AlterColumn("dbo.DeliveryGroup", "Channel_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.DeliveryGroup", "Channel_Id");
            AddForeignKey("dbo.DeliveryGroup", "Channel_Id", "dbo.Channel", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeliveryGroup", "Channel_Id", "dbo.Channel");
            DropIndex("dbo.DeliveryGroup", new[] { "Channel_Id" });
            AlterColumn("dbo.DeliveryGroup", "Channel_Id", c => c.Int());
            CreateIndex("dbo.DeliveryGroup", "Channel_Id");
            AddForeignKey("dbo.DeliveryGroup", "Channel_Id", "dbo.Channel", "Id");
        }
    }
}
