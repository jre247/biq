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

module.exports = AnalyticsPromotional = React.createClass
  displayName: 'AnalyticsPromotional'

  getInitialState: ->
    return {
      dims: AnalyticsFiltersHelper.extractDimsFromFiltersUrlFragment(this.props.paramsInfo.filters)
    }


  componentWillReceiveProps: (newProps) ->
    @getData(newProps)


  componentDidMount: ->
    @getData(@props)


  getData: (props) ->
    campaignId = @props.campaignId
    paramsInfo = @props.paramsInfo
    lookups = @props.lookups
    metrics = lookups.metrics

    cache.get('Campaigns.Analytics.Chart', campaignId, paramsInfo.bd1, paramsInfo.ed1, paramsInfo.interval, "campaign;" + paramsInfo.filters, paramsInfo.m1, paramsInfo.m2)
    .then (chartData) =>
      @setState
        chartData: chartData
    .catch App.onError

    dimsAdTypeAd = if utility.user.is(['Employee']) then 'adtype;ad' else 'adtype'
    cache.hash
      'placements:placements':  ['Campaigns.Placements', campaignId]
      catPlacement:             ['Campaigns.Analytics.Overview', campaignId, paramsInfo.bd1, paramsInfo.ed1, "#{paramsInfo.filters}&dims=campaign;category;placement"]
      'ads:ads':                ['Campaigns.Ads', campaignId]
      adTypeAd:                 ['Campaigns.Analytics.Overview', campaignId, paramsInfo.bd1, paramsInfo.ed1, "#{paramsInfo.filters}&dims=campaign;adtype;ad"]
    .then (apiContent) =>
      @setState apiContent
    .catch App.onError


  renderChart: ->
    return <Loader/> if !@state.chartData

    config = ChartsHelper.getOverviewOptions
      dateData: @props.dateData
      chartData: @state.chartData
      lookups: @props.lookups
      paramsInfo: @props.paramsInfo

    return (
      <HighchartsChart config={config}/>
    )


  renderTableCategoriesPlacements: ->
    dimensionNameToDimensionLookupMap =
      MediaPartner: @props.lookups.mediaPartners
      Platform: @props.lookups.platforms
      Category: @props.lookups.categories
      Placement: @state.placements

    metricIds = AnalyticsHelper.formatMetricIdsForComponentTable(@props.metricIds)

    groupingTableProps =
      filters: @props.paramsInfo.filters
      dims: @state.dims
      rootGroupings: @state.catPlacement.values
      metricsLookup: @props.lookups.metrics
      metricsLookupCustom: {}
      metricIds: metricIds
      dimensionNameToDimensionLookupMap: dimensionNameToDimensionLookupMap

    return (
      <GroupingTable {...groupingTableProps} />
    )


  renderTableAdTypesAds: ->
    dimensionNameToDimensionLookupMap =
      MediaPartner: @props.lookups.mediaPartners
      Platform: @props.lookups.platforms
      AdType: @props.lookups.adTypes
      Ad: @state.ads

    metricIds = AnalyticsHelper.formatMetricIdsForComponentTable(@props.metricIds)

    groupingTableProps =
      filters: @props.paramsInfo.filters
      dims: @state.dims
      rootGroupings: @state.adTypeAd.values
      metricsLookup: @props.lookups.metrics
      metricsLookupCustom: {}
      metricIds: metricIds
      dimensionNameToDimensionLookupMap: dimensionNameToDimensionLookupMap

    return (
      <GroupingTable {...groupingTableProps} />
    )


  renderTables: ->
    return <Loader/> if !@state.catPlacement
    adTypeAdsTitle = if utility.user.is(['Employee']) then "Ads grouped by AdTypes" else "AdTypes"

    <div>
      <Widget classNames="empty-content" title="Placements grouped by Categories">
        {@renderTableCategoriesPlacements()}
      </Widget>

      <Widget classNames="empty-content" title={adTypeAdsTitle}>
        {@renderTableAdTypesAds()}
      </Widget>
    </div>


  render: ->

    return (
      <div id="analytics-promotional">
        <div id="chart">
          <Widget classNames="no-header chart-container">
            {@renderChart()}
          </Widget>
        </div>
        {@renderTables()}
      </div>
    )
