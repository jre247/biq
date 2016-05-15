module.exports = FieldText = React.createClass
  displayName: 'FieldText'
  
  propTypes: 
    name: React.PropTypes.string.isRequired
    values: React.PropTypes.array
    formatterPre: React.PropTypes.func
    formatterPost: React.PropTypes.func
    multiline: React.PropTypes.bool
  mixins: [React.addons.PureRenderMixin]


  onChange: (syntheticEvent) ->
    value = syntheticEvent.currentTarget.value
    valuePre = if @props.formatterPre then @props.formatterPre(value) else value
    values = [valuePre]
    @props.onChange(values) if @props.onChange


  render: ->
    value = if @props.values then @props.values[0] else ''
    valuePost = if @props.formatterPost then @props.formatterPost(value) else value
        
    attributes = 
      className: "form-control"
      value: valuePost
      placeholder: @props.placeholder
      onChange: @onChange
      name: @props.name

    if @props.readOnly
      attributes.disabled = 'disabled'

    if @props.multiline
      return (
        <textarea {...attributes}/>
      )
    else
      return (
        <input type="text" {...attributes}/>
      )
  
