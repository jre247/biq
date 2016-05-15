module.exports = ProgressBar = React.createClass
  displayName: 'ProgressBar'
  
  propTypes: 
    complete: React.PropTypes.number     # Pass a value, where 1 corresponds with 100%. Eg: 2.34 => 234%
    completePct: React.PropTypes.any  # Pass a value that has already been converted. Eg: 234 => 234%
  
  mixins: [React.addons.PureRenderMixin]

  render: ->
    complete = @props.complete

    width = if complete then "#{complete * 100}" else @props.completePct
    width = width.replace('%', '') + '%'

    return (
      <div className="progress">
        <div className="progress-bar" style={width: width}></div>
      </div>
    )
    
