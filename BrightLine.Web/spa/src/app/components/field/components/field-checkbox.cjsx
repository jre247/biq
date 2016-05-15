module.exports = FieldCheckbox = React.createClass
  displayName: 'FieldCheckbox'
  
  mixins: [React.addons.PureRenderMixin]

  render: ->
    checked = @props.values?[0] || false

    onChange = (e) =>
      @props.onChange([e.target.checked])

    return (
      <input type="checkbox" name={@props.name} className="form-control" 
        checked={checked} onChange={onChange} ref="subField" />
    )
    
