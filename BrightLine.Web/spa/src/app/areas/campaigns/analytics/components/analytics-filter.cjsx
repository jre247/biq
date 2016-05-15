AnalyticsFilterFiltersSelector = require './analytics-filter-filters-selector/analytics-filter-filters-selector'
AnalyticsFilterIntervalSelector   = require './analytics-filter-interval-selector'
AnalyticsFilterDateSelector       = require './analytics-filter-date-selector'
AnalyticsFilterMetricSelectors    = require './analytics-filter-metric-selectors'
module.exports = React.createClass
  displayName: 'AnalyticsFilter'

  propTypes: 
    lookups: React.PropTypes.object.isRequired
    paramsInfo: React.PropTypes.object.isRequired
    metricIds: React.PropTypes.array
    adsData: React.PropTypes.object.isRequired
    deliveryGroups: React.PropTypes.object.isRequired

  mixins: [React.addons.PureRenderMixin]

  render: ->
    return (
      <div id='analytics-filter' className='col-md-12'>
        <div className="clearfix">
          <ul className="list-inline pull-left">
            <li id="filters-selector-container">
              {
                analyticsFilterFiltersSelectorProps = 
                  lookups: @props.lookups
                  adsData: @props.adsData
                  deliveryGroups: @props.deliveryGroups
                  filters: @props.paramsInfo.filters

                <AnalyticsFilterFiltersSelector {...analyticsFilterFiltersSelectorProps}/>
              }
            </li>
          </ul>
          <ul className="list-inline pull-right">
            <li id='date-selector-container'>
              <AnalyticsFilterDateSelector {...@props}/>
            </li>
            <li id='interval-selector-container'>
              <AnalyticsFilterIntervalSelector {...@props}/>
            </li>
          </ul>
        </div>
        <AnalyticsFilterMetricSelectors 
          lookups={@props.lookups} 
          paramsInfo={@props.paramsInfo} 
          metricIds={@props.metricIds}/>
      </div>
    )
