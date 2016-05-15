CREATE PROCEDURE [dbo].[CMS_InsertVideos]
	@campaignId int
AS
	begin transaction

		delete from dbo.Video where CampaignId = @campaignId;

		insert into dbo.Video
			   ([VideoId]
			   ,[CampaignId]
			   ,[Theme]
			   ,[DateAdded]
			   ,[DateRemoved]
			   ,[TotalRunTime]
			   ,[Display])
	
		select Id, Campaign_Id, coalesce(REPLACE(REPLACE(Theme, CHAR(13), ''), CHAR(10), ''), 'N/A'), DateAdded, DateRemoved, coalesce(Length, 0), REPLACE(REPLACE(Display, CHAR(13), ''), CHAR(10), '')
		from dbo.vwVideo
		where Campaign_Id = @campaignId;
		
		commit
