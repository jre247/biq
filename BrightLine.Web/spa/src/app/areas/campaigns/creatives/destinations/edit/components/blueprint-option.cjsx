module.exports = BlueprintOption = React.createClass
  displayName: 'BlueprintOption'

  render: ->
    option = @props.option
    return (
      <div className="option-thumb-and-text">
        <div className="option-thumb thumb-md">
          <div className="img" style={backgroundImage: "url('#{option.thumbnail}')"} ></div>
        </div>
        <span className="option-text">{option.name}</span>
      </div>
    )

