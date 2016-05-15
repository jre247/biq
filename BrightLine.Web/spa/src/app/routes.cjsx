NoHandler = class NoHandler extends React.Component
  render: ->
    return (
      <div/>
    )

class Area
  areaPropsToCopy = ['action', 'actions', 'roles']

  cssClasses:
    allowed: 'action-allowed'
    denied: 'action-denied'


  constructor: (areaJson, areaParentJson) ->

    # First, copy over only certain propes
    for prop in areaPropsToCopy
      @[prop] = areaJson[prop]

    # Set up an unique client id for debugging purposes.
    @id = +_.uniqueId()

    @paths = areaJson.paths || ['']
    @handler = areaJson.handler || NoHandler

    @name = areaJson.name || @handler.name

    # Keep a reference to the parent
    @parent = areaParentJson

    # First, copy over only certain propes
    for prop in areaPropsToCopy
      @[prop] = areaJson[prop]

    # Now, go through sub-areas, and convert them into instances of Area
    @areas = []
    for area in areaJson.areas || []
      @areas.push(new Area(area, @))


  traverse: (callback) ->
    # Depth-first traversal

    # Callback on the current area
    callback(@, parent)

    # Callback on all the sub-areas (recursive)
    for area in @areas
      area.traverse(callback, @)


  findByProp: (propName, propVal) ->
    matches = []

    @traverse((area) ->
      if area[propName] == propVal
        matches.push area
    )

    return matches

  isAllowedForUser: ->

    if @roles
      rolesBlacklist = @roles.blacklist
      rolesWhitelist = @roles.whitelist

      if utility.user.is(rolesBlacklist)
        return false

      if rolesWhitelist && rolesWhitelist.length > 0
        if utility.user.is(rolesWhitelist)
          return true

        # A whitelist was defined, and the current user is not in it. Deny access
        return false
      else
        # Whitelist hasn't been set up. Allow everyone who weren't blacklisted earlier
        return true

    else
      # Roles wasn't defined. Allow everyone
      return true


class SitemapProvider
  sitemapSchema:
    areas: [
      {
        paths: ['/campaigns']
        handler: require 'areas/campaigns/listing/campaigns-listing'
        areas: [
          {
            paths: ['/campaigns/create2', '/campaigns/:campaignId/edit']
            handler: require 'areas/campaigns/edit/campaign-edit'
          }
          {
            paths: ['/campaigns/:campaignId/summary']
            handler: require 'areas/campaigns/summary/campaign-summary'
            areas: [
              {
                paths: ['/campaigns/:campaignId/deliverygroups/create', '/campaigns/:campaignId/deliverygroups/:deliveryGroupId/edit']
                handler: require 'areas/campaigns/deliverygroups/edit/deliverygroup-edit'
                roles:
                  blacklist: ['AgencyPartner']
              }
              {
                paths: ['/campaigns/:campaignId/ads']
                handler: require 'areas/campaigns/ads/listing/ads-listing'
                roles:
                  blacklist: ['AgencyPartner']
                areas: [
                  {
                    paths: ['/campaigns/:campaignId/ads/create', '/campaigns/:campaignId/ads/:adId/edit']
                    handler: require 'areas/campaigns/ads/edit/ad-edit'
                  }
                ]
              }
              {
                paths: ['/campaigns/:campaignId/creatives']
                handler: require 'areas/campaigns/creatives/listing/creatives-listing'
                roles:
                  blacklist: ['AgencyPartner']
                areas: [
                  {
                    paths: [
                      '/campaigns/:campaignId/creatives/promotionals/create', '/campaigns/:campaignId/creatives/promotionals/:creativeId/edit']
                    handler: require 'areas/campaigns/creatives/promotionals/edit/edit'
                  }
                  {
                    paths: [
                      '/campaigns/:campaignId/creatives/destinations/create'
                      '/campaigns/:campaignId/creatives/destinations/create/:step'
                      '/campaigns/:campaignId/creatives/destinations/:creativeId/edit'
                      '/campaigns/:campaignId/creatives/destinations/:creativeId/edit/:step'
                    ]
                    handler: require 'areas/campaigns/creatives/destinations/edit/edit'
                  }
                  {
                    paths: [
                      '/campaigns/:campaignId/creatives/:creativeId/features/:featureId/models/:modelId/instances/create'
                      '/campaigns/:campaignId/creatives/:creativeId/features/:featureId/models/:modelId/instances/:instanceId/edit'
                    ]
                    handler: require 'areas/campaigns/creatives/features/models/instances/edit/model-instance-edit'
                  }
                  {
                    paths: ['/campaigns/:campaignId/creatives/:creativeId/features/:featureId/settings/:settingId/instances/:instanceId/edit']
                    handler: require 'areas/campaigns/creatives/features/settings/instances/edit/setting-instance-edit'
                  }
                ]
              }
              {
                paths: ['/campaigns/:campaignId/analytics']
                handler: require 'areas/campaigns/analytics/analytics'
                action: 'overview'
                areas: [
                  {                    
                    paths: ['/campaigns/:campaignId/analytics/content']
                    handler: require 'areas/campaigns/analytics/analytics'
                    action: 'content'
                    roles:
                      blacklist: ['AgencyPartner']
                  }
                  {                    
                    paths: ['/campaigns/:campaignId/analytics/promotional']
                    handler: require 'areas/campaigns/analytics/analytics'
                    action: 'promotional'
                    roles:
                      blacklist: ['AgencyPartner']
                  }
                ]
              }
            ]
          }

        ]
      }
      {
        paths: ['/campaigns/error']
        handler: require 'components/error/error-general'
      }
      {
        paths: ['/campaigns/404']
        handler: require 'components/error/error-not-found'
      }
    ]
    roles:
      blacklist: [] # Don't set up this area for these users
      whitelist: [] # Set up this area for ONLY these users
      # blacklist and whitelist are BOTH undefined: Set up this area for all users

  constructor: ->

    @sitemap = new Area(@sitemapSchema, null)

sitemap = new SitemapProvider().sitemap


do logger.log '[Routes]', 'sitemap', sitemap

module.exports =
  sitemap: sitemap
