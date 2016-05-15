module.exports = ErrorGeneral = React.createClass
  displayName: 'ErrorGeneral'
  
  propTypes: 
    someProp: React.PropTypes.object
  
  mixins: [React.addons.PureRenderMixin]

  render: ->
    return (
      <div id="error-page-container">
        <h1>Oops!</h1>
        <p>There was an error. Please try again.</p>
      </div>
    )
    
