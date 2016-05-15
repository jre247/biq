using BrightLine.Common.Models;

namespace BrightLine.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class Metrics : DbMigration
	{
		public override void Up()
		{
			Sql(@"
set identity_insert [dbo].[Metric] on
if not exists (select 1 from [dbo].[Metric] where [Id] = 1)
	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (1, 'Total Impressions', 0, 'Total Impressions', 'Impressions', 0, getdate())
if not exists (select 1 from [dbo].[Metric] where [Id] = 2)
	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (2, 'Total Clicks', 0, 'Total Clicks', 'Clicks', 0, getdate())
if not exists (select 1 from [dbo].[Metric] where [Id] = 3)
	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (3, 'CTR', 2, 'CTR', 'CTR', 0, getdate())
if not exists (select 1 from [dbo].[Metric] where [Id] = 4)
	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (4, 'Total Sessions', 0, 'Total Sessions', 'Sessions', 0, getdate())
if not exists (select 1 from [dbo].[Metric] where [Id] = 5)
	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (5, 'Avg. Time Spent', 5, 'Avg. Time Spent', 'Time Spent', 0, getdate())
if not exists (select 1 from [dbo].[Metric] where [Id] = 6)
	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (6, 'Total Bounces', 2, 'Total Bounces', 'Total Bounces', 0, getdate())
if not exists (select 1 from [dbo].[Metric] where [Id] = 7)
	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (7, 'Unique Users', 0, 'Unique Users', 'Uniques', 0, getdate())
if not exists (select 1 from [dbo].[Metric] where [Id] = 8)
	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (8, '% Returning Users', 2, '% Returning Users', 'Returning', 0, getdate())
if not exists (select 1 from [dbo].[Metric] where [Id] = 9)
	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (9, 'Total Video Views', 0, 'Total Video Views', 'Video Views', 0, getdate())
if not exists (select 1 from [dbo].[Metric] where [Id] = 10)
	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (10, 'Avg. Video Views/Session', 1, 'Avg. Video Views/Session', 'Videos/Session', 0, getdate())
if not exists (select 1 from [dbo].[Metric] where [Id] = 11)
	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (11, 'Total Pageviews', 0, 'Total Pageviews', 'Pageviews', 0, getdate())
if not exists (select 1 from [dbo].[Metric] where [Id] = 12)
	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (12, 'Avg. Pageviews/Session', 1, 'Avg. Pageviews/Session', 'Pageviews/Session', 0, getdate())

if not exists (select 1 from [dbo].[Metric] where [Id] = 13)
	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (13, 'Avg. Video Duration', 0, 'Average Video Duration', 'Average Duration', 0, getdate())
if not exists (select 1 from [dbo].[Metric] where [Id] = 14)
	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (14, 'Video % Avg.', 5, 'Video % Avg.', 'Video %', 0, getdate())
--if not exists (select 1 from [dbo].[Metric] where [Id] = 15)
--	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
--		values (15, 'Average Completion Rate', 0, 'Average Completion Rate', 'Average Completion Rate', 0, getdate())
--if not exists (select 1 from [dbo].[Metric] where [Id] = 16)
--	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
--		values (16, 'Engagement Rate', 2, 'Engagement Rate', 'Engagement', 0, getdate())
--if not exists (select 1 from [dbo].[Metric] where [Id] = 17)
--	insert [dbo].[Metric] ([Id], [Name], [Type], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
--		values (17, 'Qualified Session Count', 0, 'Qualified Session Count', 'Session Count', 0, getdate())
set identity_insert [dbo].[Metric] off
");
		}

		public override void Down()
		{
			Sql("delete from [dbo].[Metric]");
		}
	}
}
