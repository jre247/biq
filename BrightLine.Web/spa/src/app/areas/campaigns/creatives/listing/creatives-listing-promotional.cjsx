GenericListing = require 'areas/campaigns/shared/generic-listing/listing'

module.exports = CreativesListingPromotional = React.createClass
  displayName: 'CreativesListingPromotional'
  
  propTypes: 
    campaignId: React.PropTypes.any.isRequired
  
  mixins: [React.addons.PureRenderMixin]

  getInitialState: ->
    campaignId = @props.campaignId

    @userSettingsCookie = new utility.Persist {expires: 7, key: "Campaigns.#{campaignId}.Creatives.Promotionals"}
    userSettings = @userSettingsCookie.get()
    
    if !_.isObject(userSettings)
      userSettings =
        filterSearchTerm: ''
        gridlistState: 'list'
        collapsableState: false

      @updateUserSettings(userSettings)
    
    state = 
      userSettings: userSettings

    return state

  
  updateUserSettings: (userSettingsUpdates) ->
    userSettings = @state?.userSettings || {}

    # Update state's userSettings.
    _.extend(userSettings, userSettingsUpdates)

    # Persist the settings in a cookie
    @userSettingsCookie.set userSettings


  onToggleCollapser: (collapsableState) ->
    @updateUserSettings
      collapsableState: collapsableState

    @forceUpdate()


  onToggleGridList: (gridlistState) ->
    @updateUserSettings
      gridlistState: gridlistState

    @forceUpdate()


  onSearch: (searchValue, searchEvent) ->
    @updateUserSettings
      filterSearchTerm: searchValue

    @forceUpdate()


  render: ->
    userSettings = @state.userSettings

    genericListingProps =
      campaignId: @props.campaignId
      
      filterSearchTerm: userSettings.filterSearchTerm
      gridlistState: userSettings.gridlistState
      collapsableState: userSettings.collapsableState

      listingItemType: 'Promotional Creatives'
      idContainer: 'campaign-creatives-promotionals-listing-container'
      idListing: 'campaign-creatives-promotionals-listing'
      idListingItems: 'promotionals-items'
      searchableProperties: ['name', 'adTypeName']

      ListingItemView: require './creatives-listing-item-promotional'
      items: @props.items

      navOptions:
        title: 'Promotional Creatives'

        searchboxEnabled: true
        searchValue: userSettings.filterSearchTerm
        searchPlaceholder: 'Search Promotional Creatives...'
        onSearch: @onSearch

        gridlistifierEnabled: true
        gridlistState: userSettings.gridlistState
        onToggleGridList: @onToggleGridList

        collapserEnabled: true
        collapsableState: userSettings.collapsableState
        onToggleCollapser: @onToggleCollapser

        createEnabled: true
        btnCreateClass: 'btn-create-promotional btn btn-success'
        btnCreateHref: "/campaigns/#{@props.campaignId}/creatives/promotionals/create"
        btnCreateTitle: 'New Promotional'

    return <GenericListing {...genericListingProps}/>
