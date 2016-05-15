module.exports = SelectOptionDefault = React.createClass
  displayName: 'SelectOptionDefault'

  render: ->
    option = @props.option
    return (
      <div className="option-nothumb-and-text">
        <span className="option-text">{option.name}</span>
      </div>
    )

