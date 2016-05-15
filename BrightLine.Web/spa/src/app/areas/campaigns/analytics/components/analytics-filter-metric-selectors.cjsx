FieldSelect       = require 'components/field/components/field-select'
FieldHelper       = require 'components/field/field-helper'
formatterSelect   = FieldHelper.formatters.select.getItemsFromLookup

module.exports = AnalyticsFilterMetricSelectors = React.createClass
  displayName: 'AnalyticsFilterMetricSelectors'

  propTypes:
    lookups: React.PropTypes.object.isRequired
    paramsInfo: React.PropTypes.object.isRequired
    metricIds: React.PropTypes.array.isRequired

  mixins: [React.addons.PureRenderMixin]


  renderMetricSelector: (metricKey) ->
    paramsInfo = @props.paramsInfo
    metrics = @props.lookups.metrics
    metricIdCurrent = paramsInfo[metricKey]
    metricCurrent = metrics[metricIdCurrent]

    metricsItems = formatterSelect(_.chain(@props.metricIds)
      .without(paramsInfo.m1, paramsInfo.m2)
      .map((metricId) -> metrics[metricId])
      .value()
    , {sort: false})

    metricSelectorProps =
      onChange: (selections) ->
        metricSelectedId = selections[0].id
        url = utility.updateQueryString(metricKey, metricSelectedId).replace(location.origin, '')
        page(url)

      placeholder: metricCurrent.name
      items: metricsItems
      values: formatterSelect([metrics[metricCurrent.id]])
      searchable: false

    return <FieldSelect  {...metricSelectorProps} />


  render: ->
    return null if @props.metricIds.length == 0

    paramsInfo = @props.paramsInfo

    return (
      <div id="analytics-filter-metric-selectors" className="clearfix">
        <div className="analytics-filter-metric-selector-container" data-nw="m1-filter-selector">
          {@renderMetricSelector('m1')}
        </div>
        <div> vs. </div>
        <div className="analytics-filter-metric-selector-container" data-nw="m2-filter-selector">
          {@renderMetricSelector('m2')}
        </div>
      </div>
    )
