module.exports = FieldValidationErrors = React.createClass
  displayName: 'FieldValidationErrors'
  
  propTypes: 
    validitiesErrors: React.PropTypes.array.isRequired
  
  mixins: [React.addons.PureRenderMixin]

  render: ->

    validitiesErrors = @props.validitiesErrors
    return <div/> if validitiesErrors.length == 0

    return (
      <div className="validation-errors error">
        {_.map(validitiesErrors, (v, i) -> 
          <div className="validation-error" data-error key={i}>{v.msg}</div>
        )}
      </div>
    )
