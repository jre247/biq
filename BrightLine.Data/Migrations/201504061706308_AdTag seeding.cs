namespace BrightLine.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class AdTagseeding : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.AdTag_tmp",
				c => new
				{
					OldId = c.Int(),
					Tag = c.Long(),
				});
			Sql(@"
alter table [dbo].[AdTag_tmp] add [Id] int identity(120277, 189187) not null
alter table [dbo].[AdTag_tmp] add [IsDeleted] [bit] not null
alter table [dbo].[AdTag_tmp] add [DateCreated] [datetime] not null
alter table [dbo].[AdTag_tmp] add [DateUpdated] [datetime]
alter table [dbo].[AdTag_tmp] add [DateDeleted] [datetime]
alter table [dbo].[AdTag_tmp] add [Display] [nvarchar](max)
alter table [dbo].[AdTag_tmp] add [ShortDisplay] [nvarchar](max)
alter table [dbo].[AdTag_tmp] add [Ad_Id] [int] not null
alter table [dbo].[AdTag_tmp] add [Placement_Id] [int]
");
			Sql(@"
insert into [dbo].[AdTag_tmp] ([OldId], [Tag], [IsDeleted], [DateCreated], [DateUpdated], [DateDeleted],
		[Display], [ShortDisplay], [Ad_Id], [Placement_Id])
	select [Id] as OldId, [Tag], [IsDeleted], [DateCreated], [DateUpdated], [DateDeleted],
			[Display], [ShortDisplay], [Ad_Id], [Placement_Id]
		from [dbo].[AdTag]
		");

			DropForeignKey("dbo.Ad", "AdTag_Id", "dbo.AdTag");
			DropIndex("dbo.Ad", new[] { "AdTag_Id" });
			DropColumn("dbo.Ad", "AdTag_Id");
			AddColumn("dbo.Ad", "AdTag_Id", c => c.Int());
			Sql(@"
-- match up the old ids to the new ids
update [dbo].[Ad] set
		Ad.AdTag_Id = at.Id
	from AdTag_tmp at
	where at.OldId = Ad.AdTag_Id
");
			DropTable("dbo.AdTag");
			Sql(@"exec sp_rename @objname ='AdTag_tmp', @newname = 'AdTag'");
			DropColumn("dbo.AdTag", "OldId");
			DropColumn("dbo.AdTag", "Tag");
			AddPrimaryKey("dbo.AdTag", "Id");
			AddForeignKey("dbo.Ad", "AdTag_Id", "dbo.AdTag");
			CreateIndex("dbo.Ad", "AdTag_Id");
			AddForeignKey("dbo.AdTag", "Ad_Id", "dbo.Ad");
			AddForeignKey("dbo.AdTag", "Placement_Id", "dbo.Placement");
		}


		public override void Down()
		{
			AddColumn("dbo.AdTag", "Tag", c => c.Long(nullable: true));
			Sql(@"update dbo.AdTag set Tag = Id");
			AlterColumn("dbo.AdTag", "Tag", c => c.Long(nullable: false));
		}
	}
}
