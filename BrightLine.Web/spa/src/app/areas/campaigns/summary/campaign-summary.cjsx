cache = (require 'services/index').cache
Loader = require 'components/loader/loader'
ListingNav = require 'areas/campaigns/shared/generic-listing/listing-nav'

CampaignLayout = require 'areas/campaigns/shared/campaign-layout'
campaignAnalyticsHelper = require 'areas/campaigns/analytics/helpers/campaign-analytics-helper'
PerformanceMetricItems = require 'areas/campaigns/analytics/shared/performance-metric-items'
DeliveryGroupsListing = require 'areas/campaigns/deliverygroups/listing/deliverygroups-listing'
AnalyticsHelper       = require 'areas/campaigns/analytics/helpers/campaign-analytics-helper'
AnalyticsLookupsHelper = require 'areas/campaigns/analytics/helpers/campaign-analytics-lookups-helper'

module.exports = CampaignSummary = React.createClass
  displayName: 'CampaignSummary'

  getInitialState: ->
    params = @props.params

    campaignId = params.campaignId

    return {
      campaignId: campaignId
      loadedCampaignLayout: false
      loadedPerformance: false
    }


  componentWillUnmount: ->
    this._isUnMounted = true


  componentDidMount: ->
    params = @props.params

    campaignId = params.campaignId

    cache.hash
      summary: ['Campaigns.Summary', campaignId]
      lookups: ['Campaigns.getLookups']
    .then (apiLayout) =>
      return if @_isUnMounted

      apiLayout.loadedCampaignLayout = true

      summary = apiLayout.summary

      utility.adjustTitle(summary.name)

      @setState apiLayout

      cacheHashObj =
        deliveryGroups: ['Campaigns.DeliveryGroups', campaignId]

      # Only call the overview api, if the campaign has analytics
      if summary.hasAnalytics
        paramsInfo = AnalyticsHelper.getRedirectInfo(summary).paramsInfo
        cacheHashObj.analyticsOverviewCampaignDG = ['Campaigns.Analytics.Overview', campaignId, paramsInfo.bd1, paramsInfo.ed1, '&dims=campaign;deliverygroup']

      return cache.hash cacheHashObj
    .catch App.onError

    .then (apiData) =>
      return if @_isUnMounted

      apiData.loadedData = true

      @setState apiData
    .catch App.onError


  renderPerformances: ->
    # Don't show, if the first begin date of any ad in the campaign has not passed
    return null if !@state.summary.hasAnalytics

    # Summary does have analytics. Don't show, if there's no data available yet.
    overview = @state.analyticsOverviewCampaignDG.values[0]?.metric
    return null if !overview

    metrics = @state.lookups.metrics

    metricsLookups = AnalyticsLookupsHelper.getMetricsLookups()

    # Show performance tiles
    metricsConfigIds = [
      metricsLookups.InteractiveImpressions.id,
      metricsLookups.TotalClicks.id,
      metricsLookups.CTR.id,
      metricsLookups.TotalSessions.id,
      metricsLookups.AvgTimeSpent.id
    ]

    if utility.user.is(['AgencyPartner','MediaPartner'])
      metricsConfigIds = [
        metricsLookups.InteractiveImpressions.id,
        metricsLookups.TotalClicks.id,
        metricsLookups.CTR.id,
        metricsLookups.AvgTimeSpent.id
      ]

    metricPerformanceDatas = _.map(metricsConfigIds, (metricId) ->
      metric = metrics[metricId]

      metricValue = overview[metricId].total
      metricValueFormatted = campaignAnalyticsHelper.formattedMetric(metric.type, metricValue)
      metricPerformanceData =
        id:                   metricId
        metricName:           metric.name
        metricValue:          metricValueFormatted
        metricValueFormatted: metricValueFormatted
    )

    return (
      <div id="campaign-summary-performance-listing-container">
        <PerformanceMetricItems metricPerformanceDatas={metricPerformanceDatas} />
      </div>
    )


  renderDeliveryGroups: ->
    setTimeout =>
      # Highlight any items in childviews, which may have been updated since last time.
      hu = new utility.HighlightUpdates
        key: 'Campaigns.DeliveryGroups'
      hu.highlight()
    , 0

    deliveryGroupsListingProps =
      deliveryGroups: @state.deliveryGroups
      analyticsOverviewCampaignDG: @state.analyticsOverviewCampaignDG
      lookups: @state.lookups
      campaignId: @state.campaignId
      ref: "deliveryGroupsListing"

    return (
      <div id="campaign-summary-delivery-groups-container">
        <DeliveryGroupsListing {...deliveryGroupsListingProps} />
      </div>
    )


  renderBody: ->
    return <Loader/> if !@state.loadedData

    <div id="campaign-summary">
      {@renderPerformances()}
      {@renderDeliveryGroups()}
    </div>


  render: ->
    return <Loader/> if !@state.loadedCampaignLayout

    campaignSummary = @state.summary
    lookups = @state.lookups

    return (
      <CampaignLayout summary={campaignSummary} lookups={lookups}>
        {@renderBody()}
      </CampaignLayout>
    )
