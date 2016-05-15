namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BeginDateisrequiredforAd : DbMigration
    {
        public override void Up()
        {
			Sql(@"
				update [Ad]
				set BeginDate = 0
				where BeginDate is null
			");

            AlterColumn("dbo.Ad", "BeginDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ad", "BeginDate", c => c.DateTime());
        }
    }
}
