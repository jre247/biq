namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xycoordinatesonAd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ad", "XCoordinateSd", c => c.Int());
            AddColumn("dbo.Ad", "XCoordinateHd", c => c.Int());
            AddColumn("dbo.Ad", "YCoordinateSd", c => c.Int());
            AddColumn("dbo.Ad", "YCoordinateHd", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ad", "YCoordinateHd");
            DropColumn("dbo.Ad", "YCoordinateSd");
            DropColumn("dbo.Ad", "XCoordinateHd");
            DropColumn("dbo.Ad", "XCoordinateSd");
        }
    }
}
