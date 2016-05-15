module.exports = CreativesListingItemDestination = React.createClass
  propTypes:
    campaignId: React.PropTypes.number.isRequired
    item: React.PropTypes.object.isRequired
    gridlistState: React.PropTypes.string.isRequired

  mixins: [React.addons.PureRenderMixin]

  render: ->
    campaignId = @props.campaignId
    creative = @props.item
    id = creative.id

    urlEdit = "/campaigns/#{campaignId}/creatives/destinations/#{id}/edit/"
    urlCreateAdFromCreative = "/campaigns/#{campaignId}/ads/create/?creative=#{id}&adType=#{creative.adTypeId}"

    if @props.gridlistState == 'grid'
      return (
        <div className="gridlist-item gridlist-item-grid overlay-item" data-id={id}>
          <div className="overlay-item-body">
            <div className="overlay-item-body-logo" style={backgroundImage: "url('#{utility.resource.getSrc creative.resource}')"}>
            </div>
            <a className="overlay-item-body-overlay" href={urlEdit}>
              <span className="overlay-featureTypes">
                <div>Features:</div>
                {_.map(creative.featureNames, (featureName, i) -> return <span key={i}>{featureName}</span>)}
              </span>
              <span className="overlay-modified">{"Modified: #{utility.moment.format(creative.lastModified, 'MM/DD/YY hh:mm A')}"}</span>
            </a>
          </div>
          <div className="overlay-item-footer">
            <div className="overlay-item-footer-name">{creative.name}</div>
            <div className="overlay-item-footer-tools">
              <span>
                <a href={urlEdit}>
                  <i className="glyphicon glyphicon-edit" title="Edit" data-toggle="tooltip" data-placement="top"></i>
                </a>
              </span>
            </div>
          </div>
        </div>
      )

    if @props.gridlistState == 'list'

      # Set up featureNames
      if creative.featureNames.length > 2
        creative.featureNames = creative.featureNames.slice(0,3)
        creative.featureNames.push("...")
      creative.featureNamesJoined = creative.featureNames.join(', ')

      return (
        <div className="gridlist-item gridlist-item-list" data-id={id}>
            <div className="col-thumb">
              <a href={urlEdit}>
                <div className="col-thumb-img" style={backgroundImage: "url('#{utility.resource.getSrc creative.resource}')"}>
                </div>
              </a>
            </div>
            <div className="col-name">
              <a className="ellipsis" href={urlEdit}>{creative.name}</a>
            </div>
            <div className="col-with-label col-features">
              <span className='label'>Features:</span><br/>
              {creative.featureNamesJoined}
            </div>
            <div className="col-with-label col-modified-on">
              <span className='label'>Modified On:</span><br/>
              {utility.moment.format(creative.lastModified, 'MM/DD/YY hh:mm A')}
            </div>
            <div className="col-actions">
              <span>
                <a href={urlEdit}>
                  <i className="glyphicon glyphicon-edit" title="Edit" data-toggle="tooltip" data-placement="top"></i>
                </a>
              </span>
            </div>
          </div>
      )
