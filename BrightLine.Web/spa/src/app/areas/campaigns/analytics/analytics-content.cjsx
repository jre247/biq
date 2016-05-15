cache = (require 'services/index').cache
Loader = require 'components/loader/loader'
Widget = require 'components/widget/widget'
Loader = require 'components/loader/loader'

GroupingTable         = require 'areas/campaigns/analytics/components/groupingtable/grouping-table'
GroupingTableHelper   = require 'areas/campaigns/analytics/components/groupingtable/grouping-table-helper'

AnalyticsFiltersHelper = require 'areas/campaigns/analytics/components/analytics-filter-filters-selector/filters-helper'

AnalyticsHelper = require 'areas/campaigns/analytics/helpers/campaign-analytics-helper'

PerformanceMetricItems = require 'areas/campaigns/analytics/components/performance-metric-items'

module.exports = AnalyticsContent = React.createClass
  displayName: 'AnalyticsContent'

  propTypes:
    someProp: React.PropTypes.object

  
  getInitialState: ->
    return {
      dims: AnalyticsFiltersHelper.extractDimsFromFiltersUrlFragment(this.props.paramsInfo.filters)
    }


  componentWillReceiveProps: (newProps) ->
    @getData(newProps)


  componentDidMount: ->
    @getData(@props)

  componentWillMount: ->


  componentWillUnmount: ->
    this._isUnMounted = true

    
  getData: (props) ->
    campaignId = @props.campaignId
    paramsInfo = @props.paramsInfo
    lookups = @props.lookups
    metrics = lookups.metrics

    cache.hash
      overview: ['Campaigns.Analytics.Overview', campaignId, paramsInfo.bd1, paramsInfo.ed1, "#{paramsInfo.filters}&dims=campaign"]
    .then (apiContent) =>
      return if @_isUnMounted
      @setState apiContent
    .catch App.onError

    cache.hash
      'features:features':  ['Campaigns.Features', campaignId]
      'pages:pages':        ['Campaigns.Pages', campaignId]
      page:                 ['Campaigns.Analytics.Page', campaignId, paramsInfo.bd1, paramsInfo.ed1, paramsInfo.interval, "#{paramsInfo.filters}&dims=feature;page"]
      'videos:videos':      ['Campaigns.Videos', campaignId]
      video:                ['Campaigns.Analytics.Video', campaignId, paramsInfo.bd1, paramsInfo.ed1, paramsInfo.interval, "#{paramsInfo.filters}&dims=theme;video"]
    .then (apiContent) =>
      return if @_isUnMounted
      @setState apiContent
    .catch App.onError

  renderTableFeaturePage: ->
    dimensionNameToDimensionLookupMap =
      feature: @state.features
      page: @state.pages

    groupingTableProps =
      filters: @props.paramsInfo.filters
      dims: @state.dims
      rootGroupings: @state.page.values
      metricsLookup: @props.lookups.metrics
      metricsLookupCustom:
        15:
          name: 'Avg. Time Spent'
      metricIds: ['11', '9', '15']
      dimensionNameToDimensionLookupMap: dimensionNameToDimensionLookupMap
      widthMetric: 160

    return (
      <GroupingTable {...groupingTableProps} />
    )


  renderTableThemeVideos: ->
    videos = @state.videos

    GroupingTableHelper.iterateGroupings(@state.video.values, (grouping) =>
      groupingId = grouping.id

      if grouping.dimension.name == 'theme'
        grouping.name = groupingId
        grouping.metric[13] = {total: 0}
        grouping.metric[14] = {total: 0}
      else if grouping.dimension.name == 'video'
        video = videos[groupingId]
        grouping.name = video.name
        grouping.metric[13] = {total: video.totalRunTime * grouping.metric[14].total}
    )

    dimensionNameToDimensionLookupMap =
      video: @state.videos
      theme: {}

    groupingTableProps =
      filters: @props.paramsInfo.filters
      dims: @state.dims
      rootGroupings: @state.video.values
      metricsLookup: @props.lookups.metrics
      metricsLookupCustom:
        13:
          name: 'Avg. Video Duration'
      metricIds: ['9', '13', '14']
      dimensionNameToDimensionLookupMap: dimensionNameToDimensionLookupMap
      widthMetric: 160

    return (
      <GroupingTable {...groupingTableProps} />
    )


  renderTables: ->
    return <Loader/> if !@state.features

    <div>
      <Widget classNames="empty-content" title="Total Pageviews and Time Spent by Feature">
        {@renderTableFeaturePage()}
      </Widget>

      <Widget classNames="empty-content" title="Total Video Views and Completion Rate by Theme">
        {@renderTableThemeVideos()}
      </Widget>
    </div>


  renderPerformances: ->
    return <Loader/> if !@state.overview

    metrics = @props.lookups.metrics
    overviewMetrics = @state.overview.values[0].metric
    ###
    Show performance tiles for
      11: Total Pageviews
      12: Avg. Pageviews/Session
       9: Total Video Views
      10: Avg. Video Views/Session
    ###
    metricsConfigIds = [11, 12, 9, 10]
    performances = _.chain(metricsConfigIds)
      .map((metricId) ->
        metric = metrics[metricId]

        metricValue = overviewMetrics[metricId]?.total

        if _.isUndefined(metricValue)
          do logger.error "Metric object for metricId #{metricId}(#{metric.name}) is missing in Overview Api"
          return

        metricValueFormatted = AnalyticsHelper.formattedMetric(metric.type, metricValue)
        metricPerformanceData =
          id: metricId
          metricName: metric.name
          metricValue:          metricValueFormatted
          metricValueFormatted: metricValueFormatted

        return metricPerformanceData
      )
      .filter()
      .value()

    return <PerformanceMetricItems metricPerformanceDatas={performances} />

  render: ->

    return (
      <div id="analytics-content">
        <div id="performances">
          {@renderPerformances()}
        </div>
        {@renderTables()}
      </div>
    )
