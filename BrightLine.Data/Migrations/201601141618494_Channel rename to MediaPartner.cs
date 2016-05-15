namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChannelrenametoMediaPartner : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Channel", newName: "MediaPartner");
            RenameTable(name: "dbo.ChannelPlatform", newName: "MediaPartnerPlatform");
            RenameColumn(table: "dbo.Contract", name: "Channel_Id", newName: "MediaPartner_Id");
            RenameColumn(table: "dbo.MediaPartnerPlatform", name: "Channel_Id", newName: "MediaPartner_Id");
            RenameColumn(table: "dbo.DeliveryGroup", name: "Channel_Id", newName: "MediaPartner_Id");
            RenameColumn(table: "dbo.App", name: "Channel_Id", newName: "MediaPartner_Id");
            RenameColumn(table: "dbo.Placement", name: "Channel_Id", newName: "MediaPartner_Id");
            RenameIndex(table: "dbo.Contract", name: "IX_Channel_Id", newName: "IX_MediaPartner_Id");
            RenameIndex(table: "dbo.DeliveryGroup", name: "IX_Channel_Id", newName: "IX_MediaPartner_Id");
            RenameIndex(table: "dbo.Placement", name: "IX_Channel_Id", newName: "IX_MediaPartner_Id");
            RenameIndex(table: "dbo.App", name: "IX_Channel_Id", newName: "IX_MediaPartner_Id");
            RenameIndex(table: "dbo.MediaPartnerPlatform", name: "IX_Channel_Id", newName: "IX_MediaPartner_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.MediaPartnerPlatform", name: "IX_MediaPartner_Id", newName: "IX_Channel_Id");
            RenameIndex(table: "dbo.App", name: "IX_MediaPartner_Id", newName: "IX_Channel_Id");
            RenameIndex(table: "dbo.Placement", name: "IX_MediaPartner_Id", newName: "IX_Channel_Id");
            RenameIndex(table: "dbo.DeliveryGroup", name: "IX_MediaPartner_Id", newName: "IX_Channel_Id");
            RenameIndex(table: "dbo.Contract", name: "IX_MediaPartner_Id", newName: "IX_Channel_Id");
            RenameColumn(table: "dbo.Placement", name: "MediaPartner_Id", newName: "Channel_Id");
            RenameColumn(table: "dbo.App", name: "MediaPartner_Id", newName: "Channel_Id");
            RenameColumn(table: "dbo.DeliveryGroup", name: "MediaPartner_Id", newName: "Channel_Id");
            RenameColumn(table: "dbo.MediaPartnerPlatform", name: "MediaPartner_Id", newName: "Channel_Id");
            RenameColumn(table: "dbo.Contract", name: "MediaPartner_Id", newName: "Channel_Id");
            RenameTable(name: "dbo.MediaPartnerPlatform", newName: "ChannelPlatform");
            RenameTable(name: "dbo.MediaPartner", newName: "Channel");
        }
    }
}
