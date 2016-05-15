namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetCommercialSpotAdTypeManifestNameinSql : DbMigration
    {
        public override void Up()
        {
			Sql(
				@"update AdType set ManifestName = 'commercial-spot' where id = 10014"
			);
        }
        
        public override void Down()
        {
			Sql(
				@"update AdType set ManifestName = null' where id = 10014"
			);
        }
    }
}
