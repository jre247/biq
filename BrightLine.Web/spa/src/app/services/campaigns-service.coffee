services = require './services'
cache = (require 'services/index').cache

cacher = services.cacher

# Campaign common
cacher.addService
  name: 'Campaigns.getLookups'
  method: ->
    return $.getJSON("/api/campaigns/lookups")

# Campaign List
cacher.addService
  name: 'Campaigns.List'
  method: (campaignId) ->
    return $.getJSON("/api/campaigns/listing")

# Campaign Summary
cacher.addService
  name: 'Campaigns.Summary'
  method: (campaignId) ->
    return $.getJSON("/api/campaigns/#{campaignId}/summary")

# Campaign Performance
cacher.addService
  name: 'Campaigns.Performance'
  method: (campaignId) ->
    return $.getJSON("/api/campaigns/#{campaignId}/performance")

# Campaign Platforms
cacher.addService
  name: 'Campaigns.Platforms'
  method: (campaignId) ->
    return $.getJSON("/api/campaigns/#{campaignId}/platforms")

# Campaign Features
cacher.addService
  name: 'Campaigns.Features'
  method: (campaignId) ->
    return $.getJSON("/api/campaigns/#{campaignId}/features")

# Campaign Placements
cacher.addService
  name: 'Campaigns.Placements'
  method: (campaignId) ->
    return $.getJSON("/api/campaigns/#{campaignId}/placements")

# Campaign Creatives
cacher.addService
  name: 'Campaigns.Creatives'
  method: (campaignId) ->
    deferred = RSVP.defer()
    services.cache.hash
      'promotionals:promotional': ['Campaigns.Creatives.Promotionals', campaignId]
      'destinations:destination': ['Campaigns.Creatives.Destinations', campaignId]
    .then (store) ->
      creatives = _.extend({}, store.promotionals, store.destinations)
      deferred.resolve creatives
    return deferred.promise

cacher.addService
  name: 'Campaigns.Creatives.Promotionals'
  method: (campaignId) ->
    return $.getJSON("/api/campaigns/#{campaignId}/creatives/promotional")
  invalidates: ['Campaigns.Creatives']

cacher.addService
  name: 'Campaigns.Creatives.Destinations'
  method: (campaignId) ->
    return $.getJSON("/api/campaigns/#{campaignId}/creatives/destinations")
  invalidates: ['Campaigns.Creatives']


# Campaign Promotional Creative
cacher.addService
  name: 'Campaigns.Creatives.Promotional'
  method: (creativeId) ->
    deferred = RSVP.defer()
    $.getJSON("/api/creatives/promotional/#{creativeId}")
    .then (obj) ->
      deferred.resolve utility.json.toCamelCasing(obj)
    return deferred.promise
  invalidates: ['Campaigns.Creatives.Promotionals']

cacher.addService
  name: 'Campaigns.Creatives.isNameDuplicate'
  method: (campaignId, creativeId, creativeName) ->
    # creativeName = creativeName.replace(/\./g, '_')
    creativeNameEscaped = escape(creativeName)
    return $.getJSON("/api/campaigns/isduplicatecreativename/?campaignId=#{campaignId}&creativeId=#{creativeId}&name=#{creativeNameEscaped}")

# Campaign Destination
cacher.addService
  name: 'Campaigns.Productlines.get'
  method: ->
    return $.getJSON("/api/campaigns/planning/productlines")

cacher.addService
  name: 'Campaigns.Creatives.Destination'
  method: (destinationId) ->
    deferred = RSVP.defer()
    if destinationId == 0
      deferred.resolve null
      return deferred.promise
    else
      $.getJSON("/api/creatives/destination/#{destinationId}")
      .then (obj) ->
        clonedCopy = utility.json.toCamelCasing obj
        deferred.resolve clonedCopy
    return deferred.promise
  invalidates: ['Campaigns.Creatives.Destinations', 'Campaigns.Features']

# Campaign Ads
cacher.addService
  name: 'Campaigns.Ads'
  method: (campaignId) ->
    return $.getJSON("/api/campaigns/#{campaignId}/ads")

# The following are not being used as of now
# cacher.addService
#   name: 'Campaigns.Ads.Promotionals'
#   method: (campaignId) ->
#     return $.getJSON("/api/campaigns/#{campaignId}/ads/promotional")
#   invalidates: ['Campaigns.Ads']

# cacher.addService
#   name: 'Campaigns.Ads.Destinations'
#   method: (campaignId) ->
#     return $.getJSON("/api/campaigns/#{campaignId}/ads/destinations")

cacher.addService
  name: 'Campaigns.Ad'
  method: (adId) ->
    deferred = RSVP.defer()
    if adId && adId != '0'
      $.getJSON("/api/ads/#{adId}")
      .then (obj) ->
        deferred.resolve obj
    else
      deferred.resolve {}

    return deferred.promise
  invalidates: ['Campaigns.Ads']

cacher.addService
  name: 'Campaigns.Ads.isNameDuplicate'
  method: (campaignId, adId, adName) ->
    # adName = adName.replace(/\./g, '_')
    adNameEscaped = escape(adName)
    return $.getJSON("/api/campaigns/isduplicateadname/?campaignId=#{campaignId}&adId=#{adId}&name=#{adNameEscaped}")

cacher.addService
  name: 'Campaigns.Analytics.DateTime'
  method: (campaignId, bd1, ed1, int) ->
    return $.getJSON("/api/v2/campaigns/#{campaignId}/analytics/datetime?bd1=#{bd1}&ed1=#{ed1}&int=#{int}")

cacher.addService
  name: 'Campaigns.Analytics.Chart'
  method: (campaignId, bd1, ed1, int, filtersAndDims='', m1, m2) ->
    return $.getJSON("/api/v2/campaigns/#{campaignId}/analytics/chart?bd1=#{bd1}&ed1=#{ed1}&int=#{int}&filters=#{filtersAndDims}&m1=#{m1}&m2=#{m2}")

cacher.addService
  name: 'Campaigns.Analytics.Overview'
  method: (campaignId, bd1, ed1, filtersAndDims='') ->
    return $.getJSON("/api/v2/campaigns/#{campaignId}/analytics/overview?bd1=#{bd1}&ed1=#{ed1}&filters=#{filtersAndDims}")

cacher.addService
  name: 'Campaigns.Analytics.Categories'
  method: (campaignId, bd1, ed1, filtersAndDims='') ->
    return $.getJSON("/api/v2/campaigns/#{campaignId}/analytics/categories?bd1=#{bd1}&ed1=#{ed1}&filters=#{filtersAndDims}")

cacher.addService
  name: 'Campaigns.Analytics.AdTypes'
  method: (campaignId, bd1, ed1, filtersAndDims='') ->
    return $.getJSON("/api/v2/campaigns/#{campaignId}/analytics/adtypes?bd1=#{bd1}&ed1=#{ed1}&filters=#{filtersAndDims}")

cacher.addService
  name: 'Campaigns.Analytics.Page'
  method: (campaignId, bd1, ed1, int, filtersAndDims='') ->
    return $.getJSON("/api/v2/campaigns/#{campaignId}/analytics/page?bd1=#{bd1}&ed1=#{ed1}&int=#{int}&filters=#{filtersAndDims}")

cacher.addService
  name: 'Campaigns.Analytics.Video'
  method: (campaignId, bd1, ed1, int, filtersAndDims='') ->
    return $.getJSON("/api/v2/campaigns/#{campaignId}/analytics/video?bd1=#{bd1}&ed1=#{ed1}&int=#{int}&filters=#{filtersAndDims}")


cacher.addService
  name: 'Campaigns.Videos'
  method: (campaignId) ->
    return $.getJSON("/api/v2/campaigns/#{campaignId}/videos")


cacher.addService
  name: 'Campaigns.Analytics.Promotional'
  method: (campaignId, bd1, ed1, int, dims='') ->
    return $.getJSON("/api/v2/campaigns/#{campaignId}/analytics/promotional?bd1=#{bd1}&ed1=#{ed1}&int=#{int}&dims=#{dims}")


cacher.addService
  name: 'Campaigns.CmsModelDefinitions'
  method: () ->
    return $.getJSON("/api/cms/models/modelDefinitions")


cacher.addService
  name: 'Campaigns.CmsModels'
  method: (creativeId) ->
    if creativeId
      return $.getJSON("/api/cms/creatives/#{creativeId}/models")
    else
      return utility.promise.resolved null


cacher.addService
  name: 'Campaigns.CmsModelInstance'
  method: (modelInstanceId) ->
    if modelInstanceId
      return $.getJSON("/api/cms/modelInstances/#{modelInstanceId}")
    else
      return utility.promise.resolved null


cacher.addService
  name: 'Campaigns.CmsModelInstances'
  method: (modelId, verbose = false) ->
    if modelId
      return $.getJSON("/api/cms/#{modelId}/modelInstances/#{verbose}")
    else
      return utility.promise.resolved null


  cacher.addService
    name: 'Campaigns.Pages'
    method: (campaignId) ->
      return $.getJSON("/api/campaigns/#{campaignId}/pages")


  cacher.addService
    name: 'Campaigns.CmsPages'
    method: (creativeId) ->
      return $.getJSON(" /api/cms/#{creativeId}/pages")


  cacher.addService
    name: 'Campaigns.CmsSettings'
    method: (creativeId) ->
      return $.getJSON("/api/cms/creatives/#{creativeId}/settings")


  cacher.addService
    name: 'Campaigns.CmsSettingInstance'
    method: (settingInstancelId) ->
      return $.getJSON("/api/cms/settingInstances/#{settingInstancelId}")


  cacher.addService
    name: 'Campaigns.DeliveryGroups'
    method: (campaignId) ->
      return $.getJSON("/api/campaigns/#{campaignId}/deliverygroups")
