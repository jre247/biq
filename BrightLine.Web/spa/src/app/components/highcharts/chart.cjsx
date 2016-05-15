Info                    = require 'components/info/info'

module.exports = HighchartsChart = React.createClass
  displayName: 'HighchartsChart'

  propTypes:
    config: React.PropTypes.object
    isPureConfig: React.PropTypes.bool

  mixins: [React.addons.PureRenderMixin]

  getDefaultProps: ->
    return {
      isPureConfig: false
      configDefault:

        title:
          text: ''

        subtitle:
          text: ''

        credits:
          enabled: false

        plotOptions:
          series:
            animation: false
            shadow: false

          plotLines: [
            value: 0
            width: 1
            color: "#808080"
          ]

        tooltip:
          shared: true

        exporting:
          enabled: false
        legend:
          layout: "horizontal"
          verticalAlign: "bottom"
          borderWidth: 0
    }


  renderNoData: ->
    return <Info
      classNames="collection_empty"
      title="No data was found"
      description="Please try a broader set of filters" />


  renderChart: (config) ->
    return <ReactHighcharts config={config} isPureConfig={@props.isPureConfig} />


  render: ->
    return null if !@props.config

    config = _.extend({}, @props.configDefault, @props.config)

    seriesWithData = _.filter(config.series, (series) -> series.data.length > 0)

    if seriesWithData.length > 0
      @renderChart(config)
    else
      @renderNoData()
