module.exports = SelectOptionResolver = (SelectOptionComponent) ->
  return React.createClass
    displayName: 'SelectOption'

    handleMouseDown: (e) ->
      @props.mouseDown(@props.option, e)
    

    handleMouseEnter: (e) ->
      @props.mouseEnter(@props.option, e)
    

    handleMouseLeave: (e) ->
      @props.mouseLeave(@props.option, e)
    

    render: ->
      props = @props

      attributes = 
        className:    props.className
        onMouseEnter: @handleMouseEnter
        onMouseLeave: @handleMouseLeave
        onMouseDown:  @handleMouseDown
        onClick:      @handleMouseDown

      if props.option
        attributes["data-nw-optionid"] = props.option.id

      return (
        <div {...attributes} >
          <SelectOptionComponent option={props.option} />
        </div>        
      )
  
