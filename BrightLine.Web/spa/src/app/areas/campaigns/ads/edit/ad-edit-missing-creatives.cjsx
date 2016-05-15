Info = require 'components/info/info'

module.exports = AdEditMissingCreatives = React.createClass
  displayName: 'AdEditMissingCreatives'
  
  propTypes: 
    campaignId: React.PropTypes.any.isRequired
  
  mixins: [React.addons.PureRenderMixin]

  render: ->

    campaignId = @props.campaignId
    info = 
      title: "There are no Creatives in this Campaign."
      description: (<span>Create a <a href={"/campaigns/#{campaignId}/creatives/promotionals/create"}>Promotional Creative</a> or a <a href={"/campaigns/#{campaignId}/creatives/destinations/create/step/1"}>Destination Creative</a> before creating a new Ad.</span>)
      classNames: 'collection_empty'
      styleTitle: {marginBottom: 10}
    return <Info {...info}/>

    
