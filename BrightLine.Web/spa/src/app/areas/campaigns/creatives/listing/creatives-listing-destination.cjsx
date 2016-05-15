GenericListing = require 'areas/campaigns/shared/generic-listing/listing'

module.exports = CreativesListingDestination = React.createClass
  displayName: 'CreativesListingDestination'
  
  propTypes: 
    someProp: React.PropTypes.object
  
  mixins: [React.addons.PureRenderMixin]

  getInitialState: ->
    campaignId = @props.campaignId

    @userSettingsCookie = new utility.Persist {expires: 7, key: "Campaigns.#{campaignId}.Creatives.Destinations"}
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

      listingItemType: 'Destination Creatives'
      idContainer: 'campaign-creatives-destinations-listing-container'
      idListing: 'campaign-creatives-destinations-listing'
      idListingItems: 'destinations-items'
      searchableProperties: ['name']

      ListingItemView: require './creatives-listing-item-destination'
      items: @props.items

      navOptions:
        title: 'Destination Creatives'

        searchboxEnabled: true
        searchValue: userSettings.filterSearchTerm
        searchPlaceholder: 'Search Destination Creatives...'
        onSearch: @onSearch

        gridlistifierEnabled: true
        gridlistState: userSettings.gridlistState
        onToggleGridList: @onToggleGridList

        collapserEnabled: true
        collapsableState: userSettings.collapsableState
        onToggleCollapser: @onToggleCollapser

        createEnabled: true
        btnCreateClass: 'btn-create-destination btn btn-success'
        btnCreateHref: "/campaigns/#{@props.campaignId}/creatives/destinations/create"
        btnCreateTitle: 'New Destination'

    return <GenericListing {...genericListingProps}/>
