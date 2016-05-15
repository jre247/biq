module.exports = Info = React.createClass
  displayName: 'Info'
  
  propTypes: 
    title: React.PropTypes.any
    description: React.PropTypes.any
    classNames: React.PropTypes.string
    style: React.PropTypes.object

    classNamesTitle: React.PropTypes.string
    styleTitle: React.PropTypes.object
    
    classNamesDescription: React.PropTypes.string
    styleDescription: React.PropTypes.object
  
  mixins: [React.addons.PureRenderMixin]

  getDefaultProps: ->
    return {
      classNames: ''
      style: {}

      classNamesTitle: ''
      styleTitle: {}
      
      classNamesDescription: ''
      styleDescription: {}
    }


  render: ->

    return (
      <div className="info #{@props.classNames}" 
        style={@props.style}>
        <div className="title #{@props.classNamesTitle}"
           style={@props.styleTitle}>{@props.title}</div>
        <div className="description desc #{@props.classNamesDescription}"
           style={@props.styleDescription}>{@props.description}</div>
      </div>
    )
    
