namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeliveryGroup_ImpressionCountImpressionGoal : DbMigration
    {
        public override void Up()
        {
			RenameColumn("dbo.DeliveryGroup", "ImpressionCount", "ImpressionGoal");
        }
        
        public override void Down()
        {
			RenameColumn("dbo.DeliveryGroup",  "ImpressionGoal", "ImpressionCount");
        }
    }
}
