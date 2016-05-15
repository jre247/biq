FieldSelect                   = require './field-select'

module.exports = FieldRef = React.createClass
  displayName: 'FieldRef'
  
  propTypes: 
    someProp: React.PropTypes.object
  
  mixins: [React.addons.PureRenderMixin]

  render: ->

    return (
      <FieldSelect {...@props} ref="subField"/>
    )
    
