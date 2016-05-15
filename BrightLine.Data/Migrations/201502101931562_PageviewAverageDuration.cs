namespace BrightLine.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class PageviewAverageDuration : DbMigration
	{
		public override void Up()
		{
			Sql(@"
if not exists (select 1 from [dbo].[Metric] where Id = 15)
begin
	set identity_insert [dbo].[Metric] on
	insert into [dbo].[Metric] (Id, Display, ShortDisplay, IsDeleted, Name, [Type], DateCreated) values
		(15, 'Avg. Pageview Duration', 'Avg. Pageview Duration', 0, 'Avg. Pageview Duration', 5, getdate())
	set identity_insert [dbo].[Metric] off
end
else
	update [dbo].[Metric] set
		Display = 'Avg. Pageview Duration',
		ShortDisplay = 'Avg. Pageview Duration',
		IsDeleted = 0,
		Name = 'Avg. Pageview Duration',
		[Type] = 5
	where Id = 15
");
		}

		public override void Down()
		{
		}
	}
}
