namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddManifestNameonResourceType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResourceType", "ManifestName", c => c.String());

			Sql(
				@"
					update ResourceType set ManifestName = 'sd-image' where id = 1
					update ResourceType set ManifestName = 'hd-image' where id = 2
					update ResourceType set ManifestName = 'sd-video' where id = 3
					update ResourceType set ManifestName = 'hd-video' where id = 4
				"
			);
        }
        
        public override void Down()
        {
            DropColumn("dbo.ResourceType", "ManifestName");
        }
    }
}
