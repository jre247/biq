namespace BrightLine.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class ProductLineHexBackgroundColorAbbreviation : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.ProductLine", "HexBackgroundColor", c => c.String());
			AddColumn("dbo.ProductLine", "Abbreviation", c => c.String());
			Sql(@"update [dbo].[ProductLine] set HexBackgroundColor = '#A3B0AF', Abbreviation='X' where Id=1");
			Sql(@"update [dbo].[ProductLine] set HexBackgroundColor = '#3599DA', Abbreviation='R' where Id=2");
			Sql(@"update [dbo].[ProductLine] set HexBackgroundColor = '#FDBE27', Abbreviation='D', ShortDisplay='Distribution' where Id=3");
			Sql(@"update [dbo].[ProductLine] set HexBackgroundColor = '#E73449', Abbreviation='T', ShortDisplay='Trial' where Id=4");
			Sql(@"update [dbo].[ProductLine] set HexBackgroundColor = '#6CB63C', Abbreviation='A' where Id=5");
			Sql(@"update [dbo].[ProductLine] set HexBackgroundColor = '#5A4390', Abbreviation='E' where Id=6");
			Sql(@"update [dbo].[ProductLine] set HexBackgroundColor = '#EA6523', Abbreviation='I' where Id=7");
			Sql(@"
if not exists (select 1 from [dbo].[ProductLine] where [Id] = 8)
begin
set identity_insert [dbo].[ProductLine] on
	insert into [dbo].[ProductLine] ([Id], [Name], [Description], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (8, 'Retail Line (for auto brands)', 'Drive consumers towards retail (for auto brands).', 'Retail Line', 'Retail Line', 0, getdate())
set identity_insert [dbo].[ProductLine] off
end
update [dbo].[ProductLine] set HexBackgroundColor = '#3599DA', Abbreviation='R' where Id=8");
			Sql(@"
if not exists (select 1 from [dbo].[ProductLine] where [Id] = 9)
begin
set identity_insert [dbo].[ProductLine] on
	insert into [dbo].[ProductLine] ([Id], [Name], [Description], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (9, 'Distribute Content (for entertainment brands)', 'Achieve mass distribution of branded content (for entertainment brands)', 'Distribute Content', 'Distribution', 0, getdate())
set identity_insert [dbo].[ProductLine] off
end
update [dbo].[ProductLine] set HexBackgroundColor = '#FDBE27', Abbreviation='D' where Id=9");
		}

		public override void Down()
		{
			DropColumn("dbo.ProductLine", "Abbreviation");
			DropColumn("dbo.ProductLine", "HexBackgroundColor");
		}
	}
}
