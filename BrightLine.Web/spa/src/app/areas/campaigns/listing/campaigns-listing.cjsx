cache = (require 'services/index').cache
Loader = require 'components/loader/loader'

CampaignListingNav = require './campaigns-listing-nav'
CampaignListingItem = require './campaigns-listing-item'

module.exports = CampaignsListing = React.createClass
  displayName: 'CampaignsListing'

  getInitialState: ->
    @userSettingsCookie = new utility.Persist {expires: 7, key: 'Campaigns'}
    userSettings = @userSettingsCookie.get()
    
    if !_.isObject(userSettings)
      userSettings =
        filterFavoritesActive: false
        filterSearchTerm: ''
        filterStatus: ''
        gridlist: 'list'
        sortOptions: [
          {
            name: 'Begin Date'
            param: 'beginDate'
            active: true
            reverse: true
          }
          {
            name: 'A-Z'
            param: 'name'
            active: false
            reverse: false
          }
        ]

      @updateUserSettings(userSettings)

    state = 
      loaded: false
      campaigns: []
      lengthInfo:
        displaying: 0
        total: 0
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
    utility.adjustTitle("Campaigns")

    cache.get('Campaigns.List')
    .then (campaigns) =>
      return if @_isUnMounted
      
      # If user is an Agency Partner, or a client, filter out all completed campaigns
      hideCompletedOnUser = _bl.user.isAgencyPartner || _bl.user.isClient || false
      if hideCompletedOnUser
        campaigns = _.filter(campaigns, (c) -> return c.stage != "Completed")
      
      # IQ353/IQ-356: Hide internal campaigns by default. Only show it, if 'internal' flag is found in querystring
      qs = window.location.search.toLowerCase()
      hideInternalCampaigns = qs.indexOf('internal') == -1
      if hideInternalCampaigns
        # This only shows external campaigns. 
        # This is in the normal flow (internal campaigns aren't shown)
        # When querystring contains 'internal' flag, this branch isn't taken.
        campaigns = _.filter(campaigns, (c) -> !c.internal)
        
      @setState
        campaigns: _.cloneDeep(campaigns)
        loaded: true
    .catch App.onError


  onToggleSorts: (sortOption, sortOptionIndex) ->
    _sortOptions = @state.userSettings.sortOptions

    for _sortOption, _sortOptionIndex in _sortOptions
      if _sortOptionIndex == sortOptionIndex
        if _sortOption.active
          # this is already active. Switch the order 
          _sortOption.reverse = !_sortOption.reverse
        else
          # Set current option to active
          _sortOption.active = true
      else
        _sortOption.active = false

    @updateUserSettings
      sortOptions: _sortOptions

    @forceUpdate()


  onToggleFavorites: ->
    userSettings = @state.userSettings

    @updateUserSettings
      filterFavoritesActive: !userSettings.filterFavoritesActive

    @forceUpdate()


  onToggleCampaignFavorite: (campaignId) ->
    # Save the preference in the server asynchronously
    $.get "/api/campaigns/togglefavorite/#{campaignId}"

    campaign = _.find(@state.campaigns, (c) -> return c.id == campaignId)

    campaign.isFavorite = !campaign.isFavorite

    @forceUpdate()


  onToggleGridList: (gridlistState) ->
    @updateUserSettings
      gridlist: gridlistState

    @forceUpdate()


  onToggleStatus: (filterStatus) ->
    @updateUserSettings
      filterStatus: filterStatus

    @forceUpdate()


  getCampaignsSorted: (campaigns, sortOption) ->
    param = sortOption.param

    #do logger.time('total sort time')
    # for sorting by certain properties(name), lowercase first before sorting
    campaigns = _.sortBy(campaigns, (c) ->
      val = c[param]
      if param == 'name'
        return val.toLowerCase()
      else if param == 'beginDate'
        date = val
        if date
          return utility.moment.allToMoment(date).format('YYYYMMDDHHmmSS')
        else
          # Some campaigns have begin dates set as the year 1899. Use something earlier.
          return '18000101000000' 
      else
        return val
    )


    if sortOption.reverse
      #do logger.time('sorting Reverse')
      campaigns = _(campaigns).reverse().value()
      #do logger.timeEnd('sorting Reverse')

    return campaigns


  getCampaignsFilteredBySearch: (campaigns, filterSearchTerm) ->
    filterSearchTerm = filterSearchTerm.toLowerCase()
    searchableProperties = ['name', 'brandName', 'advertiserName', 'verticalName', 'productName', 'id']

    if filterSearchTerm != ''
      campaigns = _.filter(campaigns, (campaign) ->
        for prop in searchableProperties
          propVal = campaign[prop]
          if propVal != null && propVal.toString().toLowerCase().indexOf(filterSearchTerm) != -1
            return true

        return false
      )

    return campaigns


  getCampaignsFilteredByFavorite: (campaigns, filterFavoritesActive) ->
    if filterFavoritesActive
      campaigns = _.filter(campaigns, (campaign) ->
        return campaign.isFavorite
      )

    return campaigns


  getCampaignsFilteredByStatus: (campaigns, filterStatus) ->
    if filterStatus
      campaigns = _.filter(campaigns, (campaign) ->
        return campaign.status == filterStatus
      )

    return campaigns


  getCampaigns: ->
    campaigns = @state.campaigns
    userSettings = @state.userSettings

    # Filter by favorites
    campaigns = @getCampaignsFilteredByFavorite(campaigns, userSettings.filterFavoritesActive)

    # Filter by search
    campaigns = @getCampaignsFilteredBySearch(campaigns, userSettings.filterSearchTerm)

    # Filter by status
    campaigns = @getCampaignsFilteredByStatus(campaigns, userSettings.filterStatus)

    # Sort
    sortOption = _.find(userSettings.sortOptions, (so) -> return so.active)
    campaigns = @getCampaignsSorted(campaigns, sortOption)

    @state.lengthInfo =
      displaying: campaigns.length
      total: @state.campaigns.length

    return campaigns


  onSearch: (searchValue, searchEvent) ->
    @updateUserSettings
      filterSearchTerm: searchValue

    @forceUpdate()


  render: ->
    return <Loader/> if !@state.loaded

    userSettings = @state.userSettings

    campaigns = @getCampaigns()

    campaignListingNavProps =
      onSearch:@onSearch
      lengthInfo: @state.lengthInfo
      userSettings: userSettings
      onToggleSorts: @onToggleSorts
      onToggleFavorites: @onToggleFavorites
      onToggleGridList: @onToggleGridList
      onToggleStatus: @onToggleStatus

    return (
      <div id='campaign-listing' className='cfww'>
        <CampaignListingNav {...campaignListingNavProps} />
        <div id='listing-content'>
          <div id='campaigns-list' className='gridlist-items clearfix'>
            {_.map(campaigns, (campaign) =>
              campaignListingItemProps =
                campaign: campaign
                key: campaign.id
                onToggleCampaignFavorite: @onToggleCampaignFavorite
                gridlist: userSettings.gridlist

              return <CampaignListingItem {...campaignListingItemProps} />
            )}
          </div>
        </div>
      </div>
    )
