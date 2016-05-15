------------------------------------------------------------------------------------------------------------------------
-- returns lookup tables in big union in order for web app to build a dictionary for each lookup
-- columns: Id, Name, TableName
------------------------------------------------------------------------------------------------------------------------
create procedure [dbo].[GetAllLookups]	
as
begin
	set nocount on;

	select id, name, 'Platform' as tableName
	from Platform

	union

	select id, name, 'FieldType' as tableName
	from fieldtype

	union

	select id, name, 'FileType' as tableName
	from filetype

	union

	select id, name, 'ResourceType' as tableName
	from ResourceType

	union

	select id, name, 'ResourceTypeImage' as tableName
	from ResourceType
	where name in ('SD Image', 'HD Image')

	union

	select id, name, 'ResourceTypeVideo' as tableName
	from ResourceType
	where name in ('SD Video', 'HD Video')

	union

	select id, name, 'ValidationType' as tableName
	from ValidationType

	union

	select id, name, 'AdType' as tableName
	from AdType

	union

	select id, name, 'Expose' as tableName
	from Expose

	union

	select id, name, 'CmsRefType' as tableName
	from CmsRefType

	union

	select id, name, 'StorageSource' as tableName
	from StorageSource

	union

	select id, name, 'Role' as tableName
	from Role

	union

	select id, name, 'CmsPublishStatus' as CmsPublishStatus
	from CmsPublishStatus

	union

	select id, name, 'AdFunction' as AdFunction
	from AdFunction

	union

	select id, name, 'Agency' as AdFunction
	from Agency

	union

	select id, name, 'Product' as AdFunction
	from Product

	union

	select id, name, 'AdTypeGroup' as AdTypeGroup
	from AdTypeGroup

	union

	select id, name, 'Brand' as Brand
	from Brand

	union

	select id, name, 'Advertiser' as Advertiser
	from Advertiser
	
	union

	select id, display, 'Metric' as Metric
	from Metric

	union

	select id, name, 'TrackingEvent' as TrackingEvent
	from TrackingEvent

	union

	select id, name, 'Placement' as Placement
	from Placement

	order by tableName


end