using BrightLine.Common.Models;

namespace BrightLine.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class ProductLines : DbMigration
	{
		public override void Up()
		{
		Sql(@"
set identity_insert [dbo].[ProductLine] on
if not exists (select 1 from [dbo].[ProductLine] where [Id] = 1)
	insert into [dbo].[ProductLine] ([Id], [Name], [Description], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (1, 'Build your own', 'Build your own personalized mix.', 'Build your own', 'Build your own', 0, getdate())
if not exists (select 1 from [dbo].[ProductLine] where [Id] = 2)
	insert into [dbo].[ProductLine] ([Id], [Name], [Description], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (2, 'Retail', 'Drive consumers towards retail.', 'Retail', 'Retail', 0, getdate())
if not exists (select 1 from [dbo].[ProductLine] where [Id] = 3)
	insert into [dbo].[ProductLine] ([Id], [Name], [Description], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (3, 'Distribute Content', 'Achieve mass distribution of branded content.', 'Distribute Content', 'Distribute Content', 0, getdate())
if not exists (select 1 from [dbo].[ProductLine] where [Id] = 4)
	insert into [dbo].[ProductLine] ([Id], [Name], [Description], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (4, 'Product Trial', 'Encourage consumer product trial.', 'Product Trial', 'Product Trial', 0, getdate())
if not exists (select 1 from [dbo].[ProductLine] where [Id] = 5)
	insert into [dbo].[ProductLine] ([Id], [Name], [Description], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (5, 'Awareness', 'Product/Brand awareness.', 'Awareness', 'Awareness', 0, getdate())
if not exists (select 1 from [dbo].[ProductLine] where [Id] = 6)
	insert into [dbo].[ProductLine] ([Id], [Name], [Description], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (6, 'Education', 'Facilitate product/brand education.', 'Education', 'Education', 0, getdate())
if not exists (select 1 from [dbo].[ProductLine] where [Id] = 7)
	insert into [dbo].[ProductLine] ([Id], [Name], [Description], [Display], [ShortDisplay], [IsDeleted], [DateCreated])
		values (7, 'Introduction', 'Introduce a new product/brand.', 'Introduction', 'Introduction', 0, getdate())
set identity_insert [dbo].[ProductLine] off
");
		}

		public override void Down()
		{
			Sql("delete from [dbo].[ProductLine]");
		}
	}
}
