GenericListing            = require 'areas/campaigns/shared/generic-listing/listing'
AnalyticsHelper           = require 'areas/campaigns/analytics/helpers/campaign-analytics-helper'
DeliveryGroupsListingItem = require './deliverygroups-listing-item'

module.exports = DeliveryGroupsListing = React.createClass
  displayName: 'DeliveryGroupsListing'

  propTypes:
    campaignId: React.PropTypes.any.isRequired
    deliveryGroups: React.PropTypes.object.isRequired

  mixins: [React.addons.PureRenderMixin]

  getInitialState: ->
    campaignId = @props.campaignId

    @userSettingsCookie = new utility.Persist {expires: 7, key: "Campaigns.#{campaignId}.Summary.DeliveryGroups"}
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


  onSearch: (searchValue, searchEvent) ->
    @updateUserSettings
      filterSearchTerm: searchValue

    @forceUpdate()


  componentDidMount: ->
    # Highlight any items in childviews, which may have been updated since last time.
    hu = new utility.HighlightUpdates
      key: 'Campaigns.DeliveryGroups'
    hu.highlight($(ReactDOM.findDOMNode(this)))


  render: ->
    mediaPartners = @props.lookups.mediaPartners

    analyticsOverviewCampaignDGMap = {}
    analyticsOverviewCampaignDG = @props.analyticsOverviewCampaignDG?.values[0]
    if analyticsOverviewCampaignDG
      for grouping in analyticsOverviewCampaignDG.values
        analyticsOverviewCampaignDGMap[grouping.id] = grouping

    impressionMetricId = '16'

    restricted = !utility.user.is(['Employee'])

    deliveryGroups = _.chain(@props.deliveryGroups)
      # Clone it to not pollute cached data
      .cloneDeep()

      # Set up additional properties
      .map((dg) ->
        mediaPartner = mediaPartners[dg.mediaPartnerId]
        if mediaPartner
          dg.mediaPartnerName = mediaPartner.name
        else
          dg.mediaPartnerName = ''
          do logger.error "DeliveryGroup #{dg.id} has mediaPartnerId as null"

        dg.impressionGoal = dg.impressionGoal || 0
        dg.impressionGoalFormatted = AnalyticsHelper.formattedMetric('Integer', dg.impressionGoal || 0,)

        dgAnalyticsData = analyticsOverviewCampaignDGMap[dg.id]

        dg.impressionCount = dgAnalyticsData?.metric?[impressionMetricId]?.total || 0
        dg.impressionCountFormatted = AnalyticsHelper.formattedMetric('Integer', dg.impressionCount || 0)

        impressionPacingRatio = (dg.impressionCount / dg.impressionGoal)

        if isNaN(impressionPacingRatio)
          dg.impressionPacing = '0%'
        else
          impressionPacingRatioCapped = Math.min(impressionPacingRatio, 1)
          impressionPacingPercentage = numeral(impressionPacingRatio).format('000%')
          impressionPacingPercentageCapped = AnalyticsHelper.formattedMetric('Percentage', impressionPacingRatioCapped)
          dg.impressionPacing = impressionPacingPercentageCapped

        dg.impressionPacingDescription = "This Delivery Group has served #{impressionPacingPercentage} of its #{dg.impressionGoalFormatted} Impression Goal"

        dg.mediaSpend = AnalyticsHelper.formattedMetric('Integer', dg.mediaSpend || 0)

        dg.restricted = restricted

        return dg
      )

      # Sort by names in lowercase
      .sortBy((dg) -> return dg.name.toLowerCase())

      # Evaluate the lodash chain
      .value()

    userSettings = @state.userSettings

    genericListingProps =
      campaignId: @props.campaignId

      filterSearchTerm: userSettings.filterSearchTerm
      gridlistState: userSettings.gridlistState
      collapsableState: userSettings.collapsableState

      listingItemType: 'Delivery Groups'
      idContainer: 'campaign-summary-delivery-groups-container'
      idListing: 'campaign-deliverygroups'
      idListingItems: 'deliverygroups-items'
      searchableProperties: ['name']

      ListingItemView: DeliveryGroupsListingItem
      items: deliveryGroups

      navOptions:
        title: 'Delivery Groups'

        searchboxEnabled: true
        searchValue: userSettings.filterSearchTerm
        searchPlaceholder: 'Search Delivery Groups...'
        onSearch: @onSearch

        gridlistifierEnabled: false
        # gridlistState: @state.gridlistState
        # onToggleGridList: @onToggleGridList

        collapserEnabled: true
        collapsableState: userSettings.collapsableState
        onToggleCollapser: @onToggleCollapser

        createEnabled: !restricted
        btnCreateClass: 'btn-create-deliverygroup btn btn-success'
        btnCreateHref: "/campaigns/#{@props.campaignId}/deliverygroups/create"
        btnCreateTitle: 'New Delivery Group'

    return <GenericListing {...genericListingProps}/>
