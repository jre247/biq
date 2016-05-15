module.exports = SelectSelectedResolver = (SelectSelectedComponent) ->
  return React.createClass
    displayName: 'SelectValue'

    render: ->
      props = @props
      value = props.value

      attributes = 
        className: 'Select-placeholder'

      if value
        attributes["data-nw-optionid"] = value.id

      return (
        <div {...attributes} >
          {
            if value then <SelectSelectedComponent option={value} /> else props.placeholder
          }
        </div>
      )
