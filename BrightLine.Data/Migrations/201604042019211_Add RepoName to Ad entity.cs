namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRepoNametoAdentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ad", "RepoName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ad", "RepoName");
        }
    }
}
