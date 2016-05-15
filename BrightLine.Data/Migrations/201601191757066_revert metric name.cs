namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revertmetricname : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Metric", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Metric", "Name", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
