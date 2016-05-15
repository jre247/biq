namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addchannelonDeliveryGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeliveryGroup", "Channel_Id", c => c.Int());
            CreateIndex("dbo.DeliveryGroup", "Channel_Id");
            AddForeignKey("dbo.DeliveryGroup", "Channel_Id", "dbo.Channel", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeliveryGroup", "Channel_Id", "dbo.Channel");
            DropIndex("dbo.DeliveryGroup", new[] { "Channel_Id" });
            DropColumn("dbo.DeliveryGroup", "Channel_Id");
        }
    }
}
