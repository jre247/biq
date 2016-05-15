module.exports = Error404 = React.createClass
  displayName: 'ErrorNotFound'
  
  propTypes: 
    someProp: React.PropTypes.object
  
  mixins: [React.addons.PureRenderMixin]

  render: ->

    return (
      <div id="error-page-container" className='not-found-page-container'>
        <h1>Oops, 404!</h1>
        <p>This page was not found. Try going back to <a href='/campaigns/'>Campaigns</a></p>
      </div>
    )
    
