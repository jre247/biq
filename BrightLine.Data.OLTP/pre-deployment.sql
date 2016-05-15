/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
use [$(DatabaseName)];
go
if exists (select 1 from sys.procedures where name = 'Campaign_Analytics')
	drop procedure [dbo].[Campaign_Analytics]
if exists (select 1 from sys.procedures where name = 'CampaignReport')
	drop procedure [dbo].[CampaignReport]