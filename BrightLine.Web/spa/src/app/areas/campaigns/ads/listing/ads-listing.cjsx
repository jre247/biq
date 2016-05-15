cache = (require 'services/index').cache
Loader = require 'components/loader/loader'
ListingNav    = require 'areas/campaigns/shared/generic-listing/listing-nav'
Treelistifier = require './treelistifier'

CampaignLayout                  = require 'areas/campaigns/shared/campaign-layout'

AdsListingItemAdView            = require './ads-listing-item-ad'
AdsListingItemPlacementView     = require './ads-listing-item-placement'
AdsListingItemDeliveryGroupView = require './ads-listing-item-deliverygroup'

AdTagModal                      = require './components/ad-tag-modal'
AdTagExport                     = require './components/ad-tag-export'
module.exports = CampaignAdsListing = React.createClass
  displayName: 'CampaignAdsListing'

  getInitialState: ->
    campaignId = @props.params.campaignId

    @userSettingsCookie = new utility.Persist {expires: 7, key: "Campaigns.#{campaignId}.Ads"}
    userSettings = @userSettingsCookie.get()

    if !_.isObject(userSettings)
      userSettings =
        filterSearchTerm: ''
        treelistState: 'list'
        collapsableState: false

      @updateUserSettings(userSettings)

    state =
      loaded: false

      searchableProps: ['name', 'adTypeName', 'id', 'platformName', 'placementName']

      treelistStates:
        ads: 'list'
        placements: 'treeByPlacements'
        deliveryGroups: 'treeByDeliveryGroups'

      adTagsModalAd: null

      userSettings: userSettings

    return state


  updateUserSettings: (userSettingsUpdates) ->
    userSettings = @state?.userSettings || {}

    # Update state's userSettings.
    _.extend(userSettings, userSettingsUpdates)

    # Persist the settings in a cookie
    @userSettingsCookie.set userSettings


  componentWillUnmount: ->
    this._isUnMounted = true


  componentDidMount: ->

    params = @props.params

    campaignId = params.campaignId

    apiLayoutDeferred = RSVP.defer()

    cache.hash
      lookups:                  ['Campaigns.getLookups']
      summary:                  ['Campaigns.Summary', campaignId]
    .then (apiData) =>
      return if @_isUnMounted

      apiData.loadedLayout = true

      utility.adjustTitle("Ads - #{apiData.summary.name}")

      @setState apiData
      apiLayoutDeferred.resolve()
    .catch App.onError

    cache.hash
      'ads:ads':                ['Campaigns.Ads', campaignId]
      'placements:placements':  ['Campaigns.Placements', campaignId]
      creatives:                ['Campaigns.Creatives', campaignId]
      deliveryGroups:           ['Campaigns.DeliveryGroups', campaignId]
    .then (apiData) =>
      apiLayoutDeferred.promise.then =>
        return if @_isUnMounted

        lookups = @state.lookups

        # Set up shorthands
        creatives = apiData.creatives
        placements = apiData.placements
        adTypeGroups = lookups.adTypeGroups
        platforms = lookups.platforms
        apps = lookups.apps
        networks = lookups.networks
        now = moment()

        # Set up ads that aren't deleted.
        apiData.ads = _.chain(apiData.ads)
          .filter((ad) ->
            return !ad.isDeleted
          )
          .cloneDeep()
          .map((ad) ->
            ad.campaignId = campaignId

            if ad.beginDate
              ad.beginDateMoment = utility.moment.allToMoment(ad.beginDate)

              if now < ad.beginDateMoment
                ad.statusDescription =  'Upcoming'

              else
                # now is after beginDate. It's either delivering, or completed (calculated, based on endDate).
                if ad.endDate
                  ad.endDateMoment = utility.moment.allToMoment(ad.endDate)

                  if now <= ad.endDateMoment
                    ad.statusDescription = 'Delivering'
                  else
                    ad.statusDescription = 'Completed'
                else
                  # If enddate is missing, fall back to 'Delivering'
                  ad.statusDescription = 'Delivering'
            else
              ad.statusDescription =  'Upcoming'

            ad.platformName = platforms[ad.platformId.toString()].name

            ad.placementName = placements[ad.placementId?.toString() || '']?.name || ''

            # Set up fallback thumbnail
            if ad.resource == null && ad.creativeId
              adCreative = creatives[ad.creativeId.toString()]
              if adCreative && adCreative.resource
                ad.resource = adCreative.resource

            return ad
          )
          .sort((ad) -> return utility.moment.allToString(ad.lastModified, 'YYYYMMDDHHmmSS'))
          .value()

        # Set up placements that the above ads belong to
        apiData.placements = _.chain(apiData.placements)
          # Prevent polluting the cached object
          .clone()
          .filter((placement) ->
            # Set up all the associated ads on this placement object.
            placement.ads = _.filter(apiData.ads, (ad) ->
              return ad.placementId == placement.id
            )

            # Only continue setting up other properties, if the placement has ads
            return placement.ads.length > 0
          )
          .map((placement) ->
            placement.appNetwork = ""
            if placement.appId
              app = apps[placement.appId.toString()]
              if app
                placement.appNetwork = app.name
            else if placement.networkId
              network = networks[placement.networkId]
              if network
                placement.appNetwork = network.name

            if placement.adTypeGroupId
              placement.adTypeGroupName = adTypeGroups[placement.adTypeGroupId.toString()].name

            return placement
          )
          .value()

        # Set up deliverygroups that the ads belong to
        apiData.deliveryGroups = _.chain(apiData.deliveryGroups)
          # Prevent polluting the cached object
          .clone()
          .filter((deliveryGroup) ->
            # Set up all the associated ads on this placement object.
            deliveryGroup.ads = _.filter(apiData.ads, (ad) ->
              return ad.deliveryGroupId == deliveryGroup.id
            )

            # Only return placements that have ads.
            return deliveryGroup.ads.length
          )
          .value()
          # @adjustTitle(store.summary.name)


        apiData.loadedListing = true
        @setState apiData
    .catch App.onError


  onToggleTreelistifier: (treelistState) ->
    @updateUserSettings
      treelistState: treelistState

    @forceUpdate()


  onToggleCollapser: (collapsableState) ->
    @updateUserSettings
      collapsableState: collapsableState

    @forceUpdate()


  onSearch: (searchValue, searchEvent) ->
    @updateUserSettings
      filterSearchTerm: searchValue

    @forceUpdate()


  setAdTagModalAd: (ad) ->
    @setState
      adTagsModalAd: ad


  onAdTagClose: ->
    @setState
      adTagsModalAd: null



  getFiltrateItems: ->
    userSettings = @state.userSettings

    treelistStates = @state.treelistStates
    treelistState = userSettings.treelistState

    if treelistState == treelistStates.deliveryGroups
      deliveryGroups = _.cloneDeep(@state.deliveryGroups)
      filtrateItems = @filterGroupedAdsBySearchTerm(deliveryGroups, userSettings.filterSearchTerm)
    else if treelistState == treelistStates.placements
      placements = _.cloneDeep(@state.placements)
      filtrateItems = @filterGroupedAdsBySearchTerm(placements, userSettings.filterSearchTerm)
    else
      ads = _.cloneDeep(@state.ads)
      filtrateItems = @filterAdsBySearchTerm(ads, userSettings.filterSearchTerm)

    return filtrateItems


  filterGroupedAdsBySearchTerm: (groupedAds, filterSearchTerm) ->
    searchableProps = @state.searchableProps

    filtrateItems = groupedAds
    self = @

    if filterSearchTerm != ''
      filterSearchTerm = filterSearchTerm.toLowerCase()
      filtrateItems = _.map(groupedAds, (item) ->
        # Item could either be deliveryGroups, or placements, depending on the collection received
        item.ads = self.filterAdsBySearchTerm(item.ads, filterSearchTerm)
        return item
      )

    return filtrateItems


  filterAdsBySearchTerm: (ads, filterSearchTerm) ->
    searchableProps = @state.searchableProps

    filtrateItems = ads

    if filterSearchTerm != ''
      filterSearchTerm = filterSearchTerm.toLowerCase()
      filtrateItems = _.filter(ads, (ad) ->
        for prop in searchableProps
          propVal = ad[prop]
          if propVal && propVal.toString().toLowerCase().indexOf(filterSearchTerm) >= 0
            return true
        return false
      )

    return filtrateItems


  getItemViewAgainstTreelistState: () ->

    treelistStates = @state.treelistStates

    userSettings = @state.userSettings
    treelistState = userSettings.treelistState

    return AdsListingItemAdView if treelistState == treelistStates.ads
    return AdsListingItemPlacementView if treelistState == treelistStates.placements
    return AdsListingItemDeliveryGroupView if treelistState == treelistStates.deliveryGroups


  renderGroupedAdItems: ->
    campaignId = @props.params.campaignId
    filtrateItems = @getFiltrateItems()
    ItemView = @getItemViewAgainstTreelistState()
    setAdTagModalAd = @setAdTagModalAd

    return _.map(filtrateItems, (filtrateItem) ->
      return <ItemView item={filtrateItem} campaignId={campaignId} key={filtrateItem.id} onViewAdTag={setAdTagModalAd}/>
    )


  renderBody: ->
    return <Loader loaded={@state.loaded} /> if !@state.loadedListing

    campaignSummary = @state.summary
    campaignId = campaignSummary.id
    lookups = @state.lookups


    ads = @state.ads

    filtrateItems = @getFiltrateItems()

    userSettings = @state.userSettings

    listingNavProps =
      title: 'Ads'

      gridlistifierEnabled: false

      searchboxEnabled: true
      searchValue: userSettings.filterSearchTerm
      searchPlaceholder: 'Search Ads...'
      onSearch: @onSearch

      createEnabled: true
      btnCreateClass: 'btn-create-promotional btn btn-success'
      btnCreateHref: "/campaigns/#{campaignId}/ads/create"
      btnCreateTitle: 'New Ad'

      collapserEnabled: true
      collapsableState: userSettings.collapsableState
      onToggleCollapser: @onToggleCollapser
      customLinks: (
        <span>
          <Treelistifier states={@state.treelistStates} state={userSettings.treelistState} onToggleTreelistifier={@onToggleTreelistifier}/>
          <AdTagExport campaignId={campaignId}/>
        </span>
      )

    setTimeout =>
      # Highlight any items in childviews, which may have been updated since last time.
      hu = new utility.HighlightUpdates
        key: 'Campaigns.Ads'
      hu.highlight()
    , 0

    return (
      <div id="campaign-ads" className={cs({
          "campaign-generic-listing container-fluid collapsable-container whitebox": true
          "collapsed": listingNavProps.collapsableState
        })}>
        <div className='listing-nav-container container-fluid'>
          <ListingNav {...listingNavProps} />
        </div>
        <div className='listing-nav-sub-container container-fluid'></div>
        <div className='listing-items-container'>
          <div id="ads-listing-items-view">
            <div className={cs({
              "collection_empty collection_empty_original collapsable-item": true
              hide: !(ads.length == 0)
            })} >
              <p className="title">There are no Ads in this campaign</p>
            </div>
            <div className={cs({
              "collection_empty collection_empty_filtrate collapsable-item" : true
              hide: !(ads.length > 0 && filtrateItems.length == 0)
            })}>
              <p className="title">0 Ads were found</p>
              <p className="desc">Try using a broader search term</p>
            </div>
            <div className="listing-items overlay-items gridlist-items collapsable-item container-fluid treelist-state-#{userSettings.treelistState} " id="ads-items">
              {@renderGroupedAdItems()}
            </div>
            <div className="modal-tag-container">

            </div>
          </div>
        </div>
      </div>
    )


  render: ->
    return <Loader /> if !@state.loadedLayout

    campaignSummary = @state.summary
    lookups = @state.lookups

    return (
      <CampaignLayout summary={campaignSummary} lookups={lookups} navCurrent='ads'>
        {@renderBody()}
        <AdTagModal ad={@state.adTagsModalAd} onClose={@setAdTagModalAd.bind(this, null)}/>
      </CampaignLayout>
    )
