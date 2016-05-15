module.exports = CreativeOption = React.createClass
  displayName: 'CreativeOption'

  render: ->
    option = @props.option
    return (
      <div className="option-thumb-and-text">
        <div className="option-thumb thumb-md">
          <div className="img" style={backgroundImage: "url('#{utility.resource.getSrc(option.resource)}')"} ></div>
        </div>
        <span className="option-text">{option.name}</span>
      </div>
    )
