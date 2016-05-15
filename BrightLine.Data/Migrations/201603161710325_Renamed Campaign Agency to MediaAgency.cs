namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedCampaignAgencytoMediaAgency : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Campaign", name: "Agency_Id", newName: "MediaAgency_Id");
            RenameIndex(table: "dbo.Campaign", name: "IX_Agency_Id", newName: "IX_MediaAgency_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Campaign", name: "IX_MediaAgency_Id", newName: "IX_Agency_Id");
            RenameColumn(table: "dbo.Campaign", name: "MediaAgency_Id", newName: "Agency_Id");
        }
    }
}
