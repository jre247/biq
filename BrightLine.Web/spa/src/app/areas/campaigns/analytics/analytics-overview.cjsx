cache = (require 'services/index').cache
Loader = require 'components/loader/loader'
Widget = require 'components/widget/widget'
Loader = require 'components/loader/loader'

ChartsHelper          = require 'areas/campaigns/analytics/helpers/charts-helper'
HighchartsChart       = require 'components/highcharts/chart'

GroupingTable         = require 'areas/campaigns/analytics/components/groupingtable/grouping-table'
GroupingTableHelper   = require 'areas/campaigns/analytics/components/groupingtable/grouping-table-helper'

AnalyticsFiltersHelper = require 'areas/campaigns/analytics/components/analytics-filter-filters-selector/filters-helper'
AnalyticsHelper = require 'areas/campaigns/analytics/helpers/campaign-analytics-helper'

module.exports = AnalyticsOverview = React.createClass
  displayName: 'AnalyticsOverview'

  propTypes:
    campaignId: React.PropTypes.any.isRequired
    lookups: React.PropTypes.object.isRequired
    paramsInfo: React.PropTypes.object.isRequired
    deliveryGroups: React.PropTypes.object.isRequired


  getInitialState: ->
    return {
      dims: AnalyticsFiltersHelper.extractDimsFromFiltersUrlFragment(this.props.paramsInfo.filters)
    }


  componentWillReceiveProps: (newProps) ->
    @getData(newProps)


  componentDidMount: ->
    @getData(@props)

  componentWillUnmount: ->
    this._isUnMounted = true

  getData: (props) ->
    campaignId = @props.campaignId
    paramsInfo = @props.paramsInfo

    @setState
      chartData: null
      overview: null

    cache.get('Campaigns.Analytics.Chart', campaignId, paramsInfo.bd1, paramsInfo.ed1, paramsInfo.interval, "campaign;" + paramsInfo.filters, paramsInfo.m1, paramsInfo.m2)
    .then (chartData) =>
      return if @_isUnMounted

      @setState
        chartConfig: ChartsHelper.getOverviewOptions
          dateData: @props.dateData
          chartData: chartData
          lookups: @props.lookups
          paramsInfo: @props.paramsInfo
        chartData: chartData
    .catch App.onError

    dimsForAnalyticsOverviewCache = "#{paramsInfo.filters}&dims=campaign;#{@state.dims}"

    if utility.user.is('AgencyPartner') || utility.user.is('MediaPartner')
      dimsForAnalyticsOverviewCache += ';adtypegroup'

    cache.get('Campaigns.Analytics.Overview', campaignId, paramsInfo.bd1, paramsInfo.ed1, dimsForAnalyticsOverviewCache)
    .then (overview) =>
      return if @_isUnMounted

      @setState
        overview: overview
    .catch App.onError

  renderChart: ->
    return <Loader/> if !@state.chartData

    return (
      <HighchartsChart config={@state.chartConfig}/>
    )


  renderTableData: ->
    return <Loader/> if !@state.overview

    dimensionNameToDimensionLookupMap =
      'MediaPartner': @props.lookups.mediaPartners
      'Platform': @props.lookups.platforms
      'DeliveryGroup': @props.deliveryGroups
      'AdTypeGroup': @props.lookups.adTypeGroups

    metricIds = AnalyticsHelper.formatMetricIdsForComponentTable(@props.metricIds)

    groupingTableProps =
      filters: @props.paramsInfo.filters
      dims: @state.dims
      rootGroupings: @state.overview.values
      metricsLookup: @props.lookups.metrics
      metricsLookupCustom: {}
      metricIds: metricIds
      dimensionNameToDimensionLookupMap: dimensionNameToDimensionLookupMap

    return (
      <GroupingTable {...groupingTableProps} />
    )


  renderTable: ->
    return (
      <Widget classNames="empty-content" title={GroupingTableHelper.getTitle(@state.dims)}>
        {@renderTableData()}
      </Widget>
    )


  render: ->
    return (
      <div id="analytics-overview">
        <div id="chart">
          <Widget classNames="no-header chart-container">
            {@renderChart()}
          </Widget>
        </div>
        <div id="overviewtable">
          {@renderTable()}
        </div>
      </div>
    )
