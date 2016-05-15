module.exports = CampaignsListingItemList = React.createClass
  displayName: 'CampaignsListingItemList'
  
  propTypes: 
    someProp: React.PropTypes.object
  
  mixins: [React.addons.PureRenderMixin]

  render: ->

    campaign = @props.campaign
    campaignName = campaign.name

    return (
      <div className="gridlist-item gridlist-item-list" data-id={campaign.id}>
        <div className="col-thumb">
          <a href={campaign.overlayLinkHref}>
            <div className="col-thumb-img" style={backgroundImage: "url('#{utility.resource.getSrc campaign.resource}')"}>
            </div>
          </a>      
        </div>
        <div className="col-name">      
          <a href={campaign.overlayLinkHref}>{campaign.name}</a>
        </div>
        <div className="col-with-label col-advertiser-name">
          <span className='label'>Advertiser:</span><br/>
          {campaign.advertiserName}
        </div>
        <div className="col-with-label col-begin-date">
          <span className='label'>Begin Date:</span><br/>
          {utility.moment.format(campaign.beginDate, 'MM/DD/YY hh:mm A')}
        </div>
        <div className="col-with-label col-end-date">
          <span className='label'>End Date:</span><br/>
          {utility.moment.format(campaign.endDate, 'MM/DD/YY hh:mm A')}
        </div>
        <div className="col-actions">
          <span>
            <a className="campaigns-item-footer-tools-starred">
              <i className="glyphicon #{if campaign.isFavorite then 'glyphicon-star' else 'glyphicon-star-empty'}" title="Favorite" data-toggle="tooltip" data-placement="top" onClick={@props.onToggleCampaignFavorite}></i>
            </a>
          </span>
          <span>
            <a href="/cms/schemas/#{campaign.id}" className="#{if campaign.showCms then '' else 'hide'} campaigns-item-footer-tools-cms">
              <i className="glyphicon glyphicon-cog" title="CMS" data-toggle="tooltip" data-placement="top"></i>
            </a>
          </span>
          <span>
            <a href={campaign.overlayLinkHref} className="campaigns-item-footer-tools-details #{if campaign.showSummary then '' else 'hide'}">
              <i className="glyphicon glyphicon-align-left" title="Summary" data-toggle="tooltip" data-placement="top"></i>
            </a>
          </span>
        </div>
      </div>
    )
