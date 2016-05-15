/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
if exists (select * from sys.objects where name='ConcatFeatureTypes' and schema_id = 1 and type='FN')
	drop function [dbo].[ConcatFeatureTypes]
go
if exists (select 1 from sys.procedures where name='CampaignDetails' and schema_id = 1 and type='P')
	drop procedure [dbo].[CampaignDetails];
go
if exists (select 1 from sys.procedures where name='CampaignAnalytics' and schema_id = 1 and type='P')
	drop procedure [dbo].[CampaignAnalytics];
go
if exists (select 1 from sys.procedures where name='CampaignSummary' and schema_id = 1 and type='P')
	drop procedure [dbo].[CampaignSummary];
go
if exists (select 1 from sys.procedures where name='CampaignsListing' and schema_id = 1 and type='P')
	drop procedure [dbo].[CampaignsListing];
go
if exists (select 1 from sys.procedures where name='Campaign_AnalyticsPlatformDetail' and schema_id = 1 and type='P')
	drop procedure [dbo].[Campaign_AnalyticsPlatformDetail];
go


if exists (select * from sys.objects where name='ParsePlatformRollupMetrics' and schema_id = 1 and type='FN')
	drop function [dbo].[ParsePlatformRollupMetrics]
go
if exists (select 1 from information_schema.columns where table_name = 'Campaign' and column_name = 'BeginDate')
	alter table [dbo].[Campaign] drop column [BeginDate] 
go
if exists (select 1 from information_schema.columns where table_name = 'Campaign' and column_name = 'EndDate')
	alter table [dbo].[Campaign] drop column [EndDate] 
go
if exists (select * from sys.objects where name='GetCampaignDate' and schema_id = 1 and type='FN')
	drop function [dbo].[GetCampaignDate]
go
create function [dbo].[GetCampaignDate] (@campaignId int, @begin bit)
	returns date
	as
	begin
		declare @date date = null
		if (@begin = 1)
			select @date = min(a.BeginDate) from [dbo].[Ad] a where a.Campaign_Id = @campaignId
		else if not exists (select 1 from [dbo].[Ad] a where a.Campaign_Id = @campaignId and a.EndDate is null)
			select @date = max(a.EndDate) from [dbo].[Ad] a where a.Campaign_Id = @campaignId
		return @date;
	end
go
alter table [dbo].[Campaign] add [BeginDate] as ([dbo].[GetCampaignDate]([Id],(1)))
go
alter table [dbo].[Campaign] add [EndDate] as ([dbo].[GetCampaignDate]([Id],(0)))
go

delete from [dbo].[Metric] where Id = 1 -- remove Total Impressions
update [dbo].[Metric] set [Type] = 0 where [Id] = 6

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
	
if not exists (select 1 from [dbo].[Metric] where Id = 16)
begin
	set identity_insert [dbo].[Metric] on
	insert into [dbo].[Metric] (Id, Display, ShortDisplay, IsDeleted, Name, [Type], DateCreated) values
		(16, 'Interactive Impressions', 'Interactive Impressions', 0, 'Interactive Impressions', 0, getdate())
	set identity_insert [dbo].[Metric] off
end
else
	update [dbo].[Metric] set
		Display = 'Interactive Impressions',
		ShortDisplay = 'Interactive Impressions',
		IsDeleted = 0,
		Name = 'Interactive Impressions',
		[Type] = 0
	where Id = 16
	
if not exists (select 1 from [dbo].[Metric] where Id = 17)
begin
	set identity_insert [dbo].[Metric] on
	insert into [dbo].[Metric] (Id, Display, ShortDisplay, IsDeleted, Name, [Type], DateCreated) values
		(17, 'Duration Sum', 'Duration Sum', 0, 'Duration Sum', 5, getdate())
	set identity_insert [dbo].[Metric] off
end
else
	update [dbo].[Metric] set
		Display = 'Duration Sum',
		ShortDisplay = 'Duration Sum',
		IsDeleted = 0,
		Name = 'Duration Sum',
		[Type] = 5
	where Id = 17
	
if not exists (select 1 from [dbo].[Metric] where Id = 18)
begin
	set identity_insert [dbo].[Metric] on
	insert into [dbo].[Metric] (Id, Display, ShortDisplay, IsDeleted, Name, [Type], DateCreated) values
		(18, 'Frequency', 'Frequency', 0, 'Frequency', 1, getdate())
	set identity_insert [dbo].[Metric] off
end
else
	update [dbo].[Metric] set
		Display = 'Frequency',
		ShortDisplay = 'Frequency',
		IsDeleted = 0,
		Name = 'Frequency',
		[Type] = 1
	where Id = 18
	
if not exists (select 1 from [dbo].[Metric] where Id = 19)
begin
	set identity_insert [dbo].[Metric] on
	insert into [dbo].[Metric] (Id, Display, ShortDisplay, IsDeleted, Name, [Type], DateCreated) values
		(19, 'Unique Impressions', 'Unique Impressions', 0, 'Unique Impressions', 0, getdate())
	set identity_insert [dbo].[Metric] off
end
else
	update [dbo].[Metric] set
		Display = 'Unique Impressions',
		ShortDisplay = 'Unique Impressions',
		IsDeleted = 0,
		Name = 'Unique Impressions',
		[Type] = 0
	where Id = 19
	
if not exists (select 1 from [dbo].[Metric] where Id = 20)
begin
	set identity_insert [dbo].[Metric] on
	insert into [dbo].[Metric] (Id, Display, ShortDisplay, IsDeleted, Name, [Type], DateCreated) values
		(20, 'Qualified Video Views', 'Qualified Video Views', 0, 'Qualified Video Views', 0, getdate())
	set identity_insert [dbo].[Metric] off
end
else
	update [dbo].[Metric] set
		Display = 'Qualified Video Views',
		ShortDisplay = 'Qualified Video Views',
		IsDeleted = 0,
		Name = 'Qualified Video Views',
		[Type] = 0
	where Id = 20
	
if not exists (select 1 from [dbo].[Metric] where Id = 21)
begin
	set identity_insert [dbo].[Metric] on
	insert into [dbo].[Metric] (Id, Display, ShortDisplay, IsDeleted, Name, [Type], DateCreated) values
		(21, 'Spot Impressions', 'Spot Impressions', 0, 'Spot Impressions', 0, getdate())
	set identity_insert [dbo].[Metric] off
end
else
	update [dbo].[Metric] set
		Display = 'Spot Impressions',
		ShortDisplay = 'Spot Impressions',
		IsDeleted = 0,
		Name = 'Spot Impressions',
		[Type] = 0
	where Id = 21
go

if not exists (select 1 from sys.indexes i where i.name = 'IX_CampaignId-VideoId-Theme-TotalRunTime-Display')
	create nonclustered index [IX_CampaignId-VideoId-Theme-TotalRunTime-Display] on [dbo].[Video] ([CampaignId]) include ([VideoId],[Theme],[TotalRunTime],[Display])
go

declare @sql nvarchar(4000)
declare permission cursor for
	select N'grant execute on [' + s.name + '].[' +p.name + '] to iq_web_app'
		from sys.procedures p
		inner join sys.schemas s on s.schema_id = p.schema_id
		where s.name in ('dbo')
open permission
fetch next from permission into @sql
while @@fetch_status = 0
begin
	exec sp_executesql  @stmt = @sql
	fetch next from permission into @sql
end
go

declare @sql nvarchar(4000)
declare permission cursor for
	select N'grant references on [' + s.name + '].[' + o.name + '] to iq_web_app'
		from sys.objects o
		inner join sys.schemas s on s.schema_id = o.schema_id
		where o.type = 'FN' and s.name in ('dbo')
open permission
fetch next from permission into @sql
while @@fetch_status = 0
begin
	exec sp_executesql  @stmt = @sql
	fetch next from permission into @sql
end
