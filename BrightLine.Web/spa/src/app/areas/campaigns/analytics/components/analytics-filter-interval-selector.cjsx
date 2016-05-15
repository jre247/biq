module.exports = React.createClass
  displayName: 'AnalyticsFilterIntervalSelector'

  mixins: [React.addons.PureRenderMixin]
  
  onChangeInterval: (intervalId) ->
    url = utility.updateQueryString("int", intervalId, window.location.pathname + window.location.search)
    
    @setState
      interval: intervalId

    page(url)


  getInitialState: ->
    return {
      interval: @props.paramsInfo.interval
    }


  componentWillReceiveProps: (newProps) ->
    @setState
      interval: newProps.paramsInfo.interval


  render: ->
    intervals = [
      {
        id: 'hour'
        title: 'Hour'
      }
      {
        id: 'day'
        title: 'Day'
      }
      {
        id: 'week'
        title: 'Week'
      }
      {
        id: 'month'
        title: 'Month'
      }
    ]

    self = @
    onChangeInterval = @onChangeInterval
    currentIntervalId = @state.interval

    currentInterval = _.find(intervals, (i) -> i.id == currentIntervalId)

    return (
      <div id="analytics-interval-selector">
        <div className="dropdown">
          <div className="dropdown pull-right align-right">
            <button id="analytics-interval-selected" className="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
              <span>{currentInterval.title}</span>
              <span className="caret" style={marginLeft: 5}></span>
            </button>
            <ul id="analytics-interval-items" className="dropdown-menu">
              {_.map(intervals, (interval, i) =>
                return <li key={i}><a tabIndex="-1" onClick={onChangeInterval.bind(this, interval.id)}> {interval.title} </a></li>
              )}
            </ul>
          </div>
        </div>
      </div>
    )
    
