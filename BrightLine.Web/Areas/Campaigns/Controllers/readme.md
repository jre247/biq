#Campaign Apis
#####Get a listing of all campaigns.
* /api/campaigns/campaigndata

#####Get a summary of the campaign
* /api/campaigns/{id:int}/summary

#####Get the basic performance of a campaign
* /api/campaigns/{id:int}/performance

#####Get the placements associated with a campaign
* /api/campaigns/{id:int}/placements

#####Get the features associated with a campaign
* /api/campaigns/{id:int}/features

#####Get the platforms associated with a campaign
* /api/campaigns/{id:int}/platforms

#####Get the creatives associated with a campaign
* /api/campaigns/{id:int}/creatives

#####Get the promotional creatives associated with a campaign
* /api/campaigns/{id:int}/creatives/promotional

#####Get the destination creatives associated with a campaign
* /api/campaigns/{id:int}/creatives/destinations

#####Get the ads associated with a campaign
* /api/campaigns/{id:int}/ads

#####Get the promotional ads associated with a campaign
* /api/campaigns/{id:int}/ads/promotional

#####Get the destination ads associated with a campaign
* /api/campaigns/{id:int}/ads/destinations

#####Get the datetime array for the analytics
* /api/campaigns/{id:int}/analytics/datetime/{bd1?}/{ed1?}/{int?}
  * bd1: begin date in yyyyMMdd format
  * ed1: end date in yyyyMMdd format
  * int: time interval, (Hour|Day|Week|Month)
  
#####Get the platform analytics for the campaign
* /api/campaigns/{id:int}/analytics/platforms/{bd1?}/{ed1?}/{int?}
  * bd1: begin date in yyyyMMdd format
  * ed1: end date in yyyyMMdd format
  * int: time interval, (Hour|Day|Week|Month)
  
#####Get the content (feature and video) analytics for the campaign
* /api/campaigns/{id:int}/analytics/content/{bd1?}/{ed1?}/{int?}
  * bd1: begin date in yyyyMMdd format
  * ed1: end date in yyyyMMdd format
  * int: time interval, (Hour|Day|Week|Month)
  
#####Get the promotional (category and placement) analytics for the campaign
* /api/campaigns/{id:int}/analytics/promotional/{bd1?}/{ed1?}/{int?}
  * bd1: begin date in yyyyMMdd format
  * ed1: end date in yyyyMMdd format
  * int: time interval, (Hour|Day|Week|Month)
  
#####Get lookups useful for all campaigns
* /api/campaigns/lookups

#####Post to toggle campaign as user favorite
* /api/campaigns/togglefavorite/{id:int}

#####Get a preview of the campaign manifest to send to app team
* /api/campaigns/preview/{id:int}/{fullManifest:bool=false}

#####Send the campaign manifest to app team
* /api/campaigns/publish/{id:int}/{force:bool=false}

#####Checks for duplicate creative name inside the campaign
* /api/campaigns/isduplicatecreativename/{campaignId:int}/{name}

#####Checks for duplicate ad name inside the campaign
* /api/campaigns/isduplicateadname/{campaignId:int}/{name}
