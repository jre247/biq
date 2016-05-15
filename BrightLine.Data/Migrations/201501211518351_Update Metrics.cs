namespace BrightLine.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class UpdateMetrics : DbMigration
	{
		public override void Up()
		{
			// milliseconds
			Sql("update [dbo].[Metric] set [Type] = 5 where id = 5"); // Avg time spent
			Sql("update [dbo].[Metric] set [Type] = 5 where id = 13"); // Avg video duration
			Sql("update [dbo].[Metric] set [Type] = 2 where id = 14"); // Video % average
		}

		public override void Down()
		{
		}
	}
}
