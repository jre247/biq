cache = (require 'services/index').cache
Loader = require 'components/loader/loader'
CampaignLayout = require 'areas/campaigns/shared/campaign-layout'
AnalyticsHelper = require 'areas/campaigns/analytics/helpers/campaign-analytics-helper'
AnalyticsLookupsHelper = require 'areas/campaigns/analytics/helpers/campaign-analytics-lookups-helper'
configLookups         = (require 'helpers/config').lookups

AnalyticsNav            = require './components/analytics-nav'
AnalyticsFilter         = require './components/analytics-filter'

AnalyticsOverview       = require './analytics-overview'
AnalyticsContent        = require './analytics-content'
AnalyticsPromotional    = require './analytics-promotional'

module.exports = React.createClass
  displayName: 'Analytics'

  getInitialState: ->
    metricsLookups = AnalyticsLookupsHelper.getMetricsLookups()

    state =
      campaignId: parseInt(@props.params.campaignId)
      loaded: false
      actionToAnalyticsBodyViewMap: {}

    state = @setUpAnalyticsBodyViewMapForUser(metricsLookups, state)

    return state

  setUpAnalyticsBodyViewMapForUser:(metricsLookups, state) ->
    if utility.user.is(['AgencyPartner', 'MediaPartner'])
      state.actionToAnalyticsBodyViewMap = @getActionToAnalyticsBodyViewMapForAgencyAndMediaPartner(metricsLookups)

    if utility.user.is('Employee')
      state.actionToAnalyticsBodyViewMap = @getActionToAnalyticsBodyViewMapForEmployee(metricsLookups)

    return state

  getActionToAnalyticsBodyViewMapForAgencyAndMediaPartner: (metricsLookups) ->
    mapping =
      overview:
        component: AnalyticsOverview
        metricIdsForFilters: [
          metricsLookups.SpotImpressions.id,
          metricsLookups.InteractiveImpressions.id,
          metricsLookups.TotalClicks.id,
          metricsLookups.CTR.id,
          metricsLookups.AvgTimeSpent.id
        ]
        metricIdsForComponent: [
          metricsLookups.SpotImpressions.id,
          metricsLookups.InteractiveImpressions.id,
          metricsLookups.TotalClicks.id,
          metricsLookups.CTR.id,
          metricsLookups.AvgTimeSpent.id
        ]

    return mapping

  getActionToAnalyticsBodyViewMapForEmployee: (metricsLookups) ->
    mapping =
      overview:
        component: AnalyticsOverview
        metricIdsForFilters:[
          metricsLookups.SpotImpressions.id,
          metricsLookups.InteractiveImpressions.id,
          metricsLookups.TotalClicks.id,
          metricsLookups.CTR.id,
          metricsLookups.TotalSessions.id,
          metricsLookups.AvgTimeSpent.id
        ]
        metricIdsForComponent: [
          metricsLookups.SpotImpressions.id,
          metricsLookups.InteractiveImpressions.id,
          metricsLookups.TotalClicks.id,
          metricsLookups.CTR.id,
          metricsLookups.TotalSessions.id,
          metricsLookups.AvgTimeSpent.id,
        ]
      # Hiding Content page for now (reference Jira task: "BL-320: Remove/hide Content Detail page in dashboard")
      # content:
      #   component: AnalyticsContent
      #   metricIdsForFilters: []
      #   metricIdsForComponent: []
      promotional:
        component: AnalyticsPromotional
        metricIdsForFilters: [
          metricsLookups.SpotImpressions.id,
          metricsLookups.InteractiveImpressions.id,
          metricsLookups.TotalClicks.id,
          metricsLookups.CTR.id
        ]
        metricIdsForComponent: [
          metricsLookups.SpotImpressions.id,
          metricsLookups.InteractiveImpressions.id,
          metricsLookups.TotalClicks.id,
          metricsLookups.CTR.id,
          metricsLookups.TotalSessions.id,
          metricsLookups.AvgTimeSpent.id
        ]

    return mapping

  componentWillReceiveProps: (newProps) ->
    @getData(newProps)

  componentDidMount: ->
    @getData(@props)

  componentWillUnmount: ->
    this._isUnMounted = true
    clearTimeout(@state.rerenderTimeout)

  getData: (props) ->
    campaignId = @state.campaignId
    paramsInfo = @state.paramsInfo = AnalyticsHelper.getParamsInfo(props.params, props.context, props.qsParsed)

    @state.loadedAnalytics = false

    promiseApiDataLayout = RSVP.defer()
    if @state.summary && @state.lookups
      promiseApiDataLayout.resolve()
    else
      cache.hash
        lookups: ['Campaigns.getLookups']
        summary: ['Campaigns.Summary', campaignId]
      .then (apiDataLayout) =>
        return if @_isUnMounted

        summary = apiDataLayout.summary

        utility.adjustTitle("Analytics - #{summary.name}")

        apiDataLayout.campaignDates = AnalyticsHelper.getCampaignDates(summary)

        redirectInfo = AnalyticsHelper.redirectInfo(paramsInfo, summary, apiDataLayout.campaignDates)

        if redirectInfo.redirect
          page(redirectInfo.url)
          promiseApiDataLayout.reject()
        else
          _.extend(@state, apiDataLayout)

          # We want to rerender to show the Analytics layout view
          # But, avoid rerendering if promiseApiDataAnalytics is also resolved,
          #  and both promises' resolution is waiting to fire another rerender
          @state.rerenderTimeout = setTimeout ( => @setState(@state)), 10
          promiseApiDataLayout.resolve()
      .catch App.onError

    promiseApiDataAnalytics = RSVP.defer()

    cache.hash
      adsData:        ['Campaigns.Summary', campaignId]
      deliveryGroups: ['Campaigns.DeliveryGroups', campaignId]
      dateData:       ['Campaigns.Analytics.DateTime', campaignId, paramsInfo.bd1, paramsInfo.ed1, paramsInfo.interval]
    .then (apiDataAnalytics) =>
      @state.loadedAnalytics = true

      # Don't setState() here. Resolve it, and set it once both sets of apis are resolved to allow preprocessing.
      _.extend(@state, apiDataAnalytics)
      promiseApiDataAnalytics.resolve()
    .catch App.onError

    RSVP.all([promiseApiDataLayout.promise, promiseApiDataAnalytics.promise])
    .then (apiDatas) =>
      return if @_isUnMounted

      # Clear any immediately following rerenders. This rerender will suffice.
      clearTimeout(@state.rerenderTimeout)
      # Both set of apis are resolved now. Rerender
      @forceUpdate()
    .catch App.onError


  renderDetails: ->
    return <Loader/> if !@state.loadedAnalytics

    paramsInfo = @state.paramsInfo

    AnalyticsBodyConfig = @state.actionToAnalyticsBodyViewMap[@props.context.action]

    return null if !AnalyticsBodyConfig

    return (
      <div>
        <div id='analytics-filter-container' className="row">
          <AnalyticsFilter {...@state} metricIds={AnalyticsBodyConfig.metricIdsForFilters}/>
        </div>

        <div id="analytics-body">
          <AnalyticsBodyConfig.component {...@state} metricIds={AnalyticsBodyConfig.metricIdsForComponent} />
        </div>
      </div>
    )


  renderBody: ->
    analyticsNavOptions =
      action: @props.context.action
      paramsInfo: @state.paramsInfo
      campaignId: @state.summary.id
      summary: @state.summary

    return (
      <div id='campaign-analytics' className='analytics'>
        <div id='analytics-nav-container' className="row">
          <AnalyticsNav {...analyticsNavOptions} />
        </div>

        {@renderDetails()}

      </div>
    )


  render: ->
    return <Loader/> if !(@state.summary && @state.lookups)
    campaignSummary = @state.summary
    lookups = @state.lookups

    return (
      <CampaignLayout summary={campaignSummary} lookups={lookups} navCurrent='analytics'>
        {@renderBody()}
      </CampaignLayout>
    )
