
	-- 1. Delete existing temp tables
	IF OBJECT_ID('tempdb..#tmpModelInstances') IS NOT NULL DROP TABLE #tmpModelInstances
	IF OBJECT_ID('tempdb..#tmpModelValues') IS NOT NULL DROP TABLE #tmpModelValues
	

	-- 2. setup some variables 
	declare @ndxRecord as int
	declare @ndxProperty as int
	declare @totalInstances as int
	declare @totalProperties as int
	declare @totalVertRecords as int
	declare @modelId as int
	declare @campaignId as int
		
	declare @instanceId     as int

	-- AUTO-GENERATE THE PROPERTIES BELOW
	{{cmsFieldDeclarations}}
	
	/* 
	-- Example of declared fields.
	declare @FileName		as varchar(200)
	declare @Theme			as varchar(200)
	declare @DateAdded		as datetime
	declare @DateRemoved	as datetime
	declare @YoutubeURL		as varchar(200)
	declare @BitlyURL		as varchar(200)
	declare @TotalRunTime	as int
	declare @Display		as varchar(200)
	declare @ShortDisplay	as varchar(200)
	*/
	set @campaignId = {{cmsCampaignId}}
	set @modelId    = {{cmsModelId}}

	-- 3. create temp tables for for looping 
	CREATE TABLE #tmpModelInstances
	(
		row INT IDENTITY(1, 1) primary key ,
		InstanceId int
	);

	-- 4. Get distinct instances
	INSERT INTO #tmpModelInstances  (instanceid ) SELECT distinct(instance_id) FROM CampaignContentModelPropertyValue where campaign_id = @campaignid and model_id = @modelId order by instance_id;
	SELECT * into #tmpModelValues FROM CampaignContentModelPropertyValue where campaign_id = @campaignid and model_id = @modelId order by instance_id;
	create nonclustered index idx_modelinstance_id on #tmpModelValues (Instance_id)
	create nonclustered index idx_modelproperty_id on #tmpModelValues (Property_Id)
	
	-- 5. figure out totals instances to process.
	select @totalVertRecords = count(*) from #tmpModelInstances
	select @totalInstances = count(*) from #tmpModelInstances
	select @totalProperties = count(*) from dbo.CampaignContentModelProperty where model_id = @modelId
	set @ndxRecord = 1
	set @ndxProperty = 0

	-- 6. Now loop through instances to fetch and insert into temp table.
	declare @dynamicInsert as nvarchar(2000)

	while @ndxRecord <= @totalInstances
	begin
		select @instanceId = InstanceId from #tmpModelInstances where row = @ndxRecord
	
		-- AUTO-GENERATE THE PROPERTIES BELOW
		{{cmsFieldSelections}}

		/* Example of declared fields
		-- Each PROPERTY
		select @FileName	 = StringVal 		from #tmpModelValues where CmsModelInstance_id = @instanceId and CmsModelProperty_Id = 1; 
		select @Theme		 = StringVal	 	from #tmpModelValues where CmsModelInstance_id = @instanceId and CmsModelProperty_Id = 2; 
		select @DateAdded	 = DateVal 	        from #tmpModelValues where CmsModelInstance_id = @instanceId and CmsModelProperty_Id = 3; 
		select @DateRemoved	 = DateVal	 	    from #tmpModelValues where CmsModelInstance_id = @instanceId and CmsModelProperty_Id = 4; 
		select @YoutubeURL	 = StringVal	 	from #tmpModelValues where CmsModelInstance_id = @instanceId and CmsModelProperty_Id = 5; 
		select @BitlyURL	 = StringVal		from #tmpModelValues where CmsModelInstance_id = @instanceId and CmsModelProperty_Id = 6; 
		select @TotalRunTime = NumberVal        from #tmpModelValues where CmsModelInstance_id = @instanceId and CmsModelProperty_Id = 7; 
		select @Display		 = StringVal        from #tmpModelValues where CmsModelInstance_id = @instanceId and CmsModelProperty_Id = 8; 
		select @ShortDisplay = StringVal        from #tmpModelValues where CmsModelInstance_id = @instanceId and CmsModelProperty_Id = 9;
		*/

		INSERT INTO #tmpModel 
		({{cmsFieldNames}})
		 VALUES
		( {{cmsFieldVariables}} )

		 execute sp_executesql @dynamicInsert

		set @ndxRecord = @ndxRecord + 1
	end


	--print @totalInstances
	--print @totalProperties
	drop table #tmpModelInstances
	drop table #tmpModelValues