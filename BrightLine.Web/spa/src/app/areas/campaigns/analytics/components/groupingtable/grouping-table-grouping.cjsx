AnalyticsHelper = require 'areas/campaigns/analytics/helpers/campaign-analytics-helper'
GroupingTableHelper     = require './grouping-table-helper'

module.exports = GroupingTableGrouping = React.createClass
  displayName: 'GroupingTableGrouping'

  propTypes: {}
    # metricIds: metricIds
    # metricsLookupCustom: @props.metricsLookupCustom
    # grouping: grouping
    # parentGroupingNames: []
    # depthCurrent: 1
    # depthTotal: dimsLength
    # dimensionNameToDimensionLookupMap: @props.dimensionNameToDimensionLookupMap


  mixins: [React.addons.PureRenderMixin]

  getDefaultProps: ->
    return {
      expanded: true
    }


  onToggle: (expandable) ->
    return if !expandable

    @props.onGroupingToggle(@props.grouping)

  render: ->
    grouping = @props.grouping
    depthCurrent = @props.depthCurrent

    # Use Campaign as the name of the root/campaign grouping
    groupingName = grouping.name
    if !groupingName
      dimensionName = grouping.dimension.name
      if dimensionName == 'Campaign'
        groupingName = dimensionName
      else
        # For other groupings, use the lookup to find the name
        dimensionLookup = @props.dimensionNameToDimensionLookupMap[grouping.dimension.name]
        groupingName = dimensionLookup[grouping.id]?.name || "#{dimensionName} #{grouping.id}"

    cellDimension =
      className: 'col-dimension'
      title: groupingName
      value: groupingName
      width: @props.widths.dimension

    # Set up all the metrics
    cellMetrics = _.map(@props.metricIds, (metricId) =>
      metricLookup = @props.metricsLookup[metricId]
      metricValue = grouping.metric[metricId]?.total

      metricNameCustom = @props.metricsLookupCustom?[metricId]?.name
      metricNameDefault = metricLookup.name
      metricName = metricNameCustom || metricNameDefault

      if typeof metricValue == 'undefined'
        title = ''
        value = ''
      else
        metricValueFormatted = AnalyticsHelper.formattedMetric(metricLookup.type, metricValue)
        title = "#{metricName}: #{metricValueFormatted}"
        value = metricValueFormatted

      return {
        className: 'col-metric'
        title: title,
        value: value
        width: @props.metricWidthHash[metricId]
      }
    )

    cells = _.flattenDeep([cellDimension, cellMetrics])
    expandable = grouping.values?.length > 0
    expanded = grouping.expanded

    return (
      <div className={cs({
          grouping: true
          hide: !!!cells
          expandable: expandable
          undexpandable: !expandable
          expanded: expanded
        })}
        data-grouping-id={grouping.id}
        data-grouping-dimension={grouping.dimension.name}
        data-grouping-dimension-item={dimensionName}
        data-depth={depthCurrent}
        data-parity={grouping.parity}
        >

        <div className="grouping-data clearfix" onClick={@onToggle.bind(@, expandable)}>
          {_.map(cells, (cell, index) ->
            <div className={"cell #{cell.className}"} style={width: cell.width} key={index}>
              <div
                className={cs({
                  "valueWrapper ellipsis": true
                  firstValueWrapper: index == 0
                })}
                title={cell.title}>

                {if index == 0
                  <span>
                    <span className="spacers">
                      {_.map(_.range(0, depthCurrent-1), (d, i) ->
                        <span className="spacer" key={i} />
                      )}
                    </span>
                    <span className="expander-container">
                      <i className={cs({
                        "expander fa": true
                        "fa-chevron-right": !expanded
                        "fa-chevron-down": expanded
                        })} />
                    </span>
                  </span>
                }

                {cell.value}
              </div>
            </div>
          )}
        </div>

        <div className="grouping-sub-groupings">
          {_.map(grouping.values, (grouping, index) =>
            groupingTableGroupingProps =
              metricIds:            @props.metricIds
              metricsLookup:        @props.metricsLookup
              metricsLookupCustom:  @props.metricsLookupCustom
              grouping:             grouping
              parentGroupingNames:  []
              depthCurrent:         depthCurrent + 1
              depthTotal:           @props.depthTotal
              dimensionNameToDimensionLookupMap: @props.dimensionNameToDimensionLookupMap
              widths:               @props.widths
              metricWidthHash:      @props.metricWidthHash
              key:                  index
              onGroupingToggle:     @props.onGroupingToggle

            <GroupingTableGrouping {...groupingTableGroupingProps} />
          )}
        </div>
      </div>
    )
