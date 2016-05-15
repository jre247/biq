namespace BrightLine.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class Campaignstoredprocedures : DbMigration
	{
		public override void Up()
		{
			Sql(@"
if not exists (select 1 from sys.procedures where name='CampaignDetails')
	exec sp_executesql N'create procedure [dbo].[CampaignDetails] as select 1'
"); Sql(@"
if not exists (select 1 from sys.procedures where name='CampaignSummary')
	exec sp_executesql N'create procedure [dbo].[CampaignSummary] as select 1'
");
			Sql(@"
-- =============================================
-- Author:		mkozicki
-- =============================================
;alter procedure [dbo].[CampaignDetails]
	@campaignId int
as
begin
	set nocount on;

	-- first result set for basic campaign details
	select c.Id,
			c.Name,
			c.Description,
			a.Name as Advertiser,
			--r.Url as Thumbnail
			'/api/download/' + cast(r.Id as varchar) as Thumbnail
		from [dbo].[Campaign] c
			inner join [dbo].[Product] p on p.Id = c.Product_Id
			inner join [dbo].[Brand] b on b.Id = p.Brand_Id
			inner join [dbo].[Advertiser] a on a.Id = b.Advertiser_Id
			left join [dbo].[Resource] r on r.Id = c.Thumbnail_Id
		where c.Id = @campaignId and c.IsDeleted = 0

	-- second result set has performance data	
	declare @metrics table ([metricName] varchar(100), [metricValue] float, [metricType] varchar(100))
	insert into @metrics ([metricName], [metricValue], [metricType]) values
		('Impressions', 1234235, 'number')
	insert into @metrics ([metricName], [metricValue], [metricType]) values
		('Pacing', 4, 'percentage')
	insert into @metrics ([metricName], [metricValue], [metricType]) values
		('CTR', 1.85, 'percentage')
	insert into @metrics ([metricName], [metricValue], [metricType]) values
		('Sessions', 48100, 'number')
	insert into @metrics ([metricName], [metricValue], [metricType]) values
		('Uniques', 23090, 'number')
	select [metricName], [metricValue], [metricType] from @metrics

	-- third result set has creative data
	select cr.Id,
			cr.Name,
			coalesce(cr.DateUpdated, cr.DateCreated) as LastModified,
			at.Name as AdType,
			coalesce(cr.Height, r.Height) as Height,
			coalesce(cr.Width, r.Width) as Width,
			r.Size,
			--r.Url as Thumbnail
			'/api/download/' + cast(r.Id as varchar) as Thumbnail
		from [dbo].[Campaign] c
			left join [dbo].[Creative] cr on cr.Campaign_Id = c.Id
			left join [dbo].[Resource] r on r.Id = cr.Resource_Id
			left join [dbo].[AdType] at on at.Id = cr.AdType_Id
		where c.Id = @campaignId and c.IsDeleted = 0 and cr.IsDeleted = 0 and r.IsDeleted = 0

	-- fourth result set has destination data
	select distinct a.Id, e.Id as ExperienceId, 
			a.Display as Name,
			stuff((
				select distinct ''', ''' + ft.Name 
				from [dbo].[FeatureType] ft
				inner join [dbo].[Feature] f on f.FeatureType_Id = ft.Id
				inner join [FeatureAd] fa on fa.Feature_Id = f.Id
				inner join [Ad] ai on ai.Id = fa.Ad_Id 
				where ai.Id = a.Id
				for xml path('')) ,1,2,'') as FeatureTypes,
			coalesce(a.DateUpdated, a.DateCreated) as LastModified,
			--r.Url as Thumbnail
			'/api/download/' + cast(coalesce(r.Id, rc.Id) as varchar) as Thumbnail
		from [dbo].[Campaign] c
			inner join [dbo].[Experience] e on e.Campaign_Id = c.Id
			inner join [dbo].[Ad] a on a.Experience_Id = e.Id
			inner join [dbo].[FeatureAd] fa on fa.Ad_Id= a.Id
			inner join [dbo].[Feature] f on f.Id= fa.Feature_Id
			inner join [dbo].[FeatureType] ft on ft.Id= f.FeatureType_Id
			left join [dbo].[Creative] cr on cr.Id = a.Creative_Id -- ad resource as preferred
			left join [dbo].[Resource] r on r.Id = cr.Resource_Id
			left join [dbo].[Resource] rc on rc.Id = c.Thumbnail_Id -- campaign thumbnail as fallback
		where c.Id = @campaignId and c.IsDeleted = 0 and e.IsDeleted = 0 and a.IsDeleted = 0 and f.IsDeleted = 0
			--and e.ExperienceType_Id in (6,7) -- Branded Destination (Microsite & Interactive Channel) or Dedicated Brand App
		--group by a.Id, e.Id, a.Display, coalesce(a.DateUpdated, a.DateCreated), '/api/download/' + cast(r.Id as varchar)--r.Url--, ft.Name
end;");
			Sql(@";
-- =============================================
-- Author:		mkozicki
-- =============================================
;alter  procedure [dbo].[CampaignSummary]
	@userId int,
	@advertiserId int = null
as
begin
	set nocount on;

	-- restricted access client and agency partner
	declare	@openAccess bit = null--= case when (@advertiserId is null) then 0 else 1 end		
	if (@advertiserId is null)
		select top (1) @openAccess = 1 from [dbo].[RoleUser] ru where ru.User_Id = @userId and ru.Role_Id not in (9,10)

	-- figure out the campaigns with analytics
	;with analyzed as (select distinct c.Id as Campaign_Id
				from [dbo].[Campaign] c 
				inner join [dbo].[Experience] e on e.Campaign_Id = c.Id
				where e.ExperienceType_Id = 7 /* Dedicated Brand App*/ and
					c.IsDeleted = 0 and e.IsDeleted = 0 and
					(c.BeginDate is not null and c.BeginDate < getdate()))
	-- get the campaign summary data
	select distinct c.Id, c.Name,
				b.Id as BrandId,
				b.Name as BrandName,
				a.Id as AdvertiserId,
				a.Name as AdvertiserName,
				v.Id as VerticalId,
				v.Name as VerticalName,
				p.Id as ProductId,
				p.Name as ProductName,
				c.BeginDate as BeginDate,
				coalesce(c.DateUpdated, c.DateCreated) as LastModified,
				(case when (cu.Campaign_Id is not null) then 1 else 0 end) as IsFavorite,
				r.Id as ThumbnailId,
				r.Name as ThumbnailFileName,
				(case when (an.Campaign_Id is not null) then 1 else 0 end) as HasAnalytics,
				(case when (@openAccess = 1 and ccs.Campaign_Id is not null) then 1 else 0 end) as HasCms
		from [dbo].[Campaign] c 
			inner join [dbo].[Product] p on p.Id = c.Product_Id
			inner join [dbo].[Brand] b on b.Id = p.Brand_Id
			inner join [dbo].[Advertiser] a on a.Id = b.Advertiser_Id
			inner join [dbo].[SubSegment] ss on ss.Id = p.SubSegment_Id
			inner join [dbo].[Segment] s on s.Id = ss.Segment_Id
			inner join [dbo].[Vertical] v on v.Id = s.Vertical_Id
			left join [dbo].[Resource] r on r.Id = c.Thumbnail_Id
			left join [dbo].[CampaignContentSchema] ccs on ccs.Campaign_Id = c.Id
			left join [dbo].[User] u on u.Advertiser_Id = a.Id			
			left join [dbo].[CampaignUser] cu on cu.User_Id = @userId and cu.Campaign_Id = c.Id
			left join analyzed an on an.Campaign_Id = c.Id
		where ((u.Id = case when (@openAccess is null) then @userId else u.Id end and u.Id is not null) or
			(@openAccess = 1 and u.Id is null)) and 
			(a.Id = case when (@advertiserId is null) then a.Id else @advertiserId end)
		order by c.Id
end;");
		}

		public override void Down()
		{
		}
	}
}
