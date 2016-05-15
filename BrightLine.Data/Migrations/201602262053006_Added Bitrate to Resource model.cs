namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBitratetoResourcemodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resource", "Bitrate", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Resource", "Bitrate");
        }
    }
}
