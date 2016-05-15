module.exports = CampaignsListingItemGrid = React.createClass
  displayName: 'CampaignsListingItemGrid'
  
  propTypes: 
    campaign: React.PropTypes.object.isRequired
  
  mixins: [React.addons.PureRenderMixin]

  render: -> 
    campaign = @props.campaign
    campaignName = campaign.name
    return (
      <div className="gridlist-item gridlist-item-grid campaigns-item">
        <div className="campaigns-item-body">
          <div className="campaigns-item-body-logo" style={backgroundImage: "url('#{utility.resource.getSrc campaign.resource}')"}>
          </div>
          <a className="campaigns-item-body-overlay" href={campaign.overlayLinkHref}>
            <span>{campaignName}</span>
            <span>{campaign.advertiserName}</span>
            <span>Begin Date: {utility.moment.format(campaign.beginDate, 'MM/DD/YY hh:mm A')}</span>
            <span>End Date: {utility.moment.format(campaign.endDate, 'MM/DD/YY hh:mm A')}</span>
          </a>
        </div>
        <div className="campaigns-item-footer">
          <div className="campaigns-item-footer-name">{campaignName}</div>
          <div className="campaigns-item-footer-tools">
            <a className="col-sm-3 campaigns-item-footer-tools-starred">
              <i className="glyphicon #{if campaign.isFavorite then 'glyphicon-star' else 'glyphicon-star-empty'}" title="Favorite" data-toggle="tooltip" data-placement="top" onClick={@props.onToggleCampaignFavorite}></i>
            </a>   
            <a className="col-sm-3 campaigns-item-footer-tools-cms #{if campaign.showCms then '' else 'hide'}" href="/cms/schemas/#{campaign.id}">
              <i className="glyphicon glyphicon-cog" title="CMS" data-toggle="tooltip" data-placement="top"></i>
            </a>
            <a className="col-sm-3 campaigns-item-footer-tools-details  #{if campaign.showSummary then '' else 'hide'}" href={campaign.overlayLinkHref}>
              <i className="glyphicon glyphicon-align-left" title="Summary" data-toggle="tooltip" data-placement="top"></i>
            </a>
          </div>
        </div>
      </div>
    )
