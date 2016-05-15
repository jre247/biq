namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePublisher_IdandNetwork_IdfromChannel : DbMigration
    {
        public override void Up()
        {
			Sql(@"
				alter table channel
				drop column publisher_id

				alter table channel
				drop column network_id
			");
        }
        
        public override void Down()
        {
        }
    }
}
