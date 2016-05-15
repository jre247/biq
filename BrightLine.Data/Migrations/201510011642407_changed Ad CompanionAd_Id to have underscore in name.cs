namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedAdCompanionAd_Idtohaveunderscoreinname : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Ad", name: "CompanionAdId", newName: "CompanionAd_Id");
            RenameIndex(table: "dbo.Ad", name: "IX_CompanionAdId", newName: "IX_CompanionAd_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Ad", name: "IX_CompanionAd_Id", newName: "IX_CompanionAdId");
            RenameColumn(table: "dbo.Ad", name: "CompanionAd_Id", newName: "CompanionAdId");
        }
    }
}
