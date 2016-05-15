namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGenerationtoCampaignmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaign", "Generation", c => c.Int());

			Sql(
				@"Update Campaign Set Generation = 1"
			);
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaign", "Generation");
        }
    }
}
