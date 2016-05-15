module.exports = CreativesListingItemPromotional = React.createClass
  propTypes:
    campaignId: React.PropTypes.number.isRequired
    item: React.PropTypes.object.isRequired
    gridlistState: React.PropTypes.string.isRequired

  mixins: [React.addons.PureRenderMixin]

  render: ->
    campaignId = @props.campaignId
    creative = @props.item
    id = creative.id

    if @props.gridlistState == 'grid'
      return (
        <div className="gridlist-item gridlist-item-grid overlay-item" data-id={id}>
          <div className="overlay-item-body">
            <div className="overlay-item-body-logo" style={backgroundImage: "url('#{utility.resource.getSrc creative.resource}')"}>
            </div>
            <a className="overlay-item-body-overlay" href="/campaigns/#{campaignId}/creatives/promotionals/#{id}/edit">
              <span className="overlay-adType">{ creative.adTypeName }</span>
              <span className="overlay-dimensions">{ creative.dimensions }</span>
              <span className="overlay-modified">Modified: {utility.moment.format(creative.lastModified, 'MM/DD/YY hh:mm A')}</span>
            </a>
          </div>
          <div className="overlay-item-footer">
            <div className="overlay-item-footer-name">{creative.name}</div>
            <div className="overlay-item-footer-tools">
              <span>
                <a href="/campaigns/#{campaignId}/creatives/promotionals/#{id}/edit">
                  <i className="glyphicon glyphicon-edit" title="Edit" data-toggle="tooltip" data-placement="top"></i>
                </a>
              </span>

            </div>
          </div>
        </div>
      )

    if @props.gridlistState == 'list'
      return (
        <div className="gridlist-item gridlist-item-list" data-id={id}>
            <div className="col-thumb">
              <a href="/campaigns/#{campaignId}/creatives/promotionals/#{id}/edit">
                <div className="col-thumb-img" style={backgroundImage: "url('#{utility.resource.getSrc creative.resource}')"}>
                </div>
              </a>
            </div>
            <div className="col-name">
              <a className="ellipsis" href="/campaigns/#{campaignId}/creatives/promotionals/#{id}/edit">{creative.name}</a>
            </div>
            <div className="col-with-label col-ad-type">
              <span className='label'>Ad Type:</span><br/>
              {creative.adTypeName}
            </div>
            <div className="col-with-label col-modified-on">
              <span className='label'>Modified On:</span><br/>
              {utility.moment.format(creative.lastModified, 'MM/DD/YY hh:mm A')}
            </div>
            <div className="col-actions">
              <span>
                <a href="/campaigns/#{campaignId}/creatives/promotionals/#{id}/edit">
                  <i className="glyphicon glyphicon-edit" title="Edit" data-toggle="tooltip" data-placement="top"></i>
                </a>
              </span>
            </div>
          </div>
      )
