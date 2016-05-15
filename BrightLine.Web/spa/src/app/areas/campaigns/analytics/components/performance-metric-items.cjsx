module.exports = React.createClass
  displayName: 'PerformanceMetricItems'
  propTypes:
    metricPerformanceDatas: React.PropTypes.array.isRequired

  mixins: [React.addons.PureRenderMixin]

  render: ->

    return (
      <div id="performance-items" className="container-fluid whitebox">
        {_.map(@props.metricPerformanceDatas, (metricPerformanceData) ->
          return (
            <div className="performance-item cursor-pointer" data-toggle="tooltip" data-placement="bottom" title={metricPerformanceData.metricValue} data-metric-id={metricPerformanceData.id} key={metricPerformanceData.id}>
              <span className="metric-value">{metricPerformanceData.metricValueFormatted}</span>
              <span className="metric-name">{metricPerformanceData.metricName}</span>
            </div>
          )
        )}
      </div>
    )
