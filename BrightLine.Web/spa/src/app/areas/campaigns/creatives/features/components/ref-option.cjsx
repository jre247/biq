module.exports = RefOption = React.createClass
  displayName: 'RefOption'

  render: ->
    option = @props.option

    optionResource = _.find(option.fields, (f) -> f.name == 'thumbnailSrc' && f.values?[0]?.url)?.resource

    return (
      <div className="option-thumb-and-text">
        <div className="option-thumb thumb-md">
          <div className="img" style={backgroundImage: "url('#{utility.resource.getSrc(optionResource)}')"} ></div>
        </div>
        <span className="option-text">{option.name}</span>
      </div>
    )
