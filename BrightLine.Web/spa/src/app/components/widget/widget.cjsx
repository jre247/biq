module.exports = Widget = React.createClass
  displayName: 'Widget'
  
  propTypes: 
    title: React.PropTypes.string
    classNames: React.PropTypes.string
    classNamesHeader: React.PropTypes.string
    classNamesContent: React.PropTypes.string

  mixins: [React.addons.PureRenderMixin]

  getDefaultProps: ->
    return {
      title: ''
      classNames: ''
      classNamesHeader: ''
      classNamesContent: ''
    }

  render: ->
    return (
      <div className={"widget #{@props.classNames}"}>
        <div className={"widget-header #{@props.classNamesHeader}"}>
          {@props.title}
        </div>
        <div className={"widget-content #{@props.classNamesContent}"}>
          {@props.children}
        </div>
      </div>
    )
    
