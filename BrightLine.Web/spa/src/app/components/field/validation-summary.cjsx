module.exports = ValidationSummary = React.createClass
  displayName: 'ValidationSummary'
  
  propTypes: 
    errors: React.PropTypes.array # containing strings
  
  mixins: [React.addons.PureRenderMixin]

  render: ->
    return <div/> if @props.errors.length == 0

    return (
      <div className="bg-danger validation-summary-errors" data-valmsg-summary="true">
        <ul>
          {_.map(@props.errors, (e, i) -> 
            <li key={i}>{e}</li>
          )}
        </ul>
      </div>
    )
