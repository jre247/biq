Info                    = require 'components/info/info'
GroupingTableGrouping   = require './grouping-table-grouping'
GroupingTableHelper     = require './grouping-table-helper'
MetricConfig     = require './metric-config'
AnalyticsHelper = require 'areas/campaigns/analytics/helpers/campaign-analytics-helper'
AnalyticsLookupsHelper = require 'areas/campaigns/analytics/helpers/campaign-analytics-lookups-helper'

module.exports = GroupingTable = React.createClass
  displayName: 'GroupingTable'

  propTypes:
    dims:                               React.PropTypes.string.isRequired
    rootGroupings:                      React.PropTypes.array.isRequired
    metricsLookup:                      React.PropTypes.object.isRequired
    metricsLookupCustom:                React.PropTypes.object
    metricIds:                          React.PropTypes.array.isRequired
    dimensionNameToDimensionLookupMap:  React.PropTypes.object.isRequired


  mixins: [React.addons.PureRenderMixin]

  getDefaultProps: ->
    return {
      metricsLookupCustom: {}
      widthTotal: 1130
      widthMetric: 100
    }


  getInitialState: ->
    rootGroupings = _.cloneDeep(@props.rootGroupings)

    GroupingTableHelper.iterateGroupings(rootGroupings, (grouping) ->
      # Set all groupings as expanded for now
      if grouping.values?.length
        grouping.expanded = true
    )

    @applyParity(rootGroupings)

    return {
      rootGroupings: rootGroupings
    }


  onGroupingToggle: (grouping) ->
    grouping.expanded = !grouping.expanded

    @applyParity(@state.rootGroupings)

    @forceUpdate()


  applyParity: (rootGroupings) ->
    GroupingTableHelper.iterateGroupings(rootGroupings, ((options, grouping, groupingParent) ->
      # Don't consider groupings, whose parent is not expanded
      return if groupingParent && !groupingParent.expanded

      # Set up parity of the grouping
      if options.row % 2 == 0
        grouping.parity = 'even'
      else
        grouping.parity = 'odd'

      # Increment row count for parity calculations for the next visible grouping
      options.row++
    ).bind(@, {row: 0}))

  renderNoData: ->
    return <Info
      classNames="collection_empty"
      title="No data was found"
      description="Please try a broader set of filters" />

  # Build a hash of widths for each metricWidthHash
  # This hash will either contain default metric widths that are calculated based on total number of metrics,
  # or a special metric width that is predefined in a mapping object that is located in AnalyticsHelper
  buildMetricWidthHash: (metricIds) ->
    tableWidth = 800
    metricWidthHash = {}
    metricSpecialWidthHash = {}
    metricDefaultWidth = Math.ceil(tableWidth / metricIds.length)
    metricDefaultWidthSum = 0
    metricSpecialWidthSum = 0

    _.each(metricIds, (metricId) =>
      metricName = @props.metricsLookup[metricId].name

      specialWidth = AnalyticsLookupsHelper.getMetricWidthForMetricName(metricName)

      if specialWidth
        metricSpecialWidthHash[metricId] = specialWidth
        metricSpecialWidthSum += specialWidth
      else
        metricWidthHash[metricId] = metricDefaultWidth
        metricDefaultWidthSum += metricDefaultWidth
    )

    MetricWidthHashFinal = @adjustMetricWidthsForHash(metricSpecialWidthSum, metricDefaultWidthSum, tableWidth, metricWidthHash, metricSpecialWidthHash)
    return MetricWidthHashFinal

  # Adjust each metric width, depending if there are any special metric widths.
  # Without this adjustment, special metric widths would overflow the table
  adjustMetricWidthsForHash: (metricSpecialWidthSum, metricDefaultWidthSum, tableWidth, metricWidthHash, metricSpecialWidthHash) ->
    offsetWidthTotal = (metricSpecialWidthSum + metricDefaultWidthSum) - tableWidth
    offsetWidthForEachMetric = offsetWidthTotal / Object.keys(metricWidthHash).length
    metricDefaultWidthHashFormatted = {}
    if (offsetWidthTotal > 0)
      _.each(metricWidthHash, (metricWidth, key) =>
        metricDefaultWidthHashFormatted[key] = metricWidth - offsetWidthForEachMetric
      )
    else
      metricDefaultWidthHashFormatted = _.clone(metricWidthHash)

    MetricWidthHashFinal = _.extend(metricDefaultWidthHashFormatted, metricSpecialWidthHash)

    return MetricWidthHashFinal

  renderGroupingTable: ->
    # The following Ids are available to this user role
    metricIdsAvailable = _.keys(@state.rootGroupings[0].metric)

    # The following IDs are intended to be shown
    metricIds = @props.metricIds

    # However, not all of the above metrics may be available depending on role.
    # Use the intersection of what's available and what's intended.
    metricIds = _.intersection(metricIds, metricIdsAvailable)

    dimsLength = @props.dims.split(';').length

    widths =
      dimension: 330

    metricWidthHash = @buildMetricWidthHash(metricIds)

    # Set up Dimension column. Should be empty
    columnDimension =
      className: 'col-dimension'
      value: ''
      width: widths.dimension

    # Set up metric columns
    columnsMetrics = _.map(metricIds, (metricId) =>
      metricNameLookup = @props.metricsLookup[metricId].name
      metricNameCustom = @props.metricsLookupCustom[metricId]?.name
      metricNameDefault = metricNameLookup.replace('Total', '').replace('Impressions', 'Imps.').replace('Avg. ', '').replace('Interactive','Int.').trim()
      metricName = metricNameCustom || metricNameDefault
      return {
        className: 'col-metric'
        title: metricName
        value: metricName
        width: metricWidthHash[metricId]
      }
    )
    headers = _.flattenDeep([columnDimension, columnsMetrics])

    return (
      <div className="groupingtable-container">
        <div className="groupingtable" ref="groupingtable">

          <div className="headers clearfix">
            {_.map(headers, (header, index) ->
              <div className={"cell header #{header.className}"} style={width: header.width} key={index}>
                <div className="valueWrapper ellipsis" title={header.title} data-nw={header.value}>{header.value}</div>
              </div>
            )}
          </div>

          <div className="groupings">
            {_.map(@state.rootGroupings, (grouping, index) =>
              groupingTableGroupingProps =
                metricIds:            metricIds
                metricsLookup:        @props.metricsLookup
                metricsLookupCustom:  @props.metricsLookupCustom
                grouping:             grouping
                parentGroupingNames:  []
                depthCurrent:         1
                depthTotal:           dimsLength
                widths:               widths
                metricWidthHash:      metricWidthHash
                dimensionNameToDimensionLookupMap: @props.dimensionNameToDimensionLookupMap
                key:                  index
                onGroupingToggle:     @onGroupingToggle

              <GroupingTableGrouping {...groupingTableGroupingProps} />
            )}
          </div>

        </div>
      </div>
    )


  render: ->
    if @state.rootGroupings.length == 0
      @renderNoData()
    else
      @renderGroupingTable()
