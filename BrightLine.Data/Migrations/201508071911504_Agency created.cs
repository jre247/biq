namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Agencycreated : DbMigration
    {
        public override void Up()
        {
			CreateTable(
				"dbo.Agency",
				c => new
					{
						Id = c.Int(nullable: false, identity: true),
						Name = c.String(),
						IsDeleted = c.Boolean(nullable: false),
						DateCreated = c.DateTime(nullable: false),
						DateUpdated = c.DateTime(),
						DateDeleted = c.DateTime(),
						Display = c.String(),
						ShortDisplay = c.String(),
					})
				.PrimaryKey(t => t.Id);

			AddColumn("dbo.Campaign", "Agency_Id", c => c.Int());
			CreateIndex("dbo.Campaign", "Agency_Id");
			AddForeignKey("dbo.Campaign", "Agency_Id", "dbo.Agency", "Id");

			Sql(@"
				insert into Agency (Name, IsDeleted, DateCreated)
				Values
				('Test Agency 1', 0, GetDate())

				update Campaign
				set Agency_Id = 1
				where Agency_Id is null
			");
        }
        
        public override void Down()
        {
			DropForeignKey("dbo.Campaign", "Agency_Id", "dbo.Agency");
			DropIndex("dbo.Campaign", new[] { "Agency_Id" });
			DropColumn("dbo.Campaign", "Agency_Id");
			DropTable("dbo.Agency");
        }
    }
}
