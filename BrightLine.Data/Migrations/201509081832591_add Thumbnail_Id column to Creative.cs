namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addThumbnail_IdcolumntoCreative : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Creative", "Thumbnail_Id", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Creative", "Thumbnail_Id");
        }
    }
}
