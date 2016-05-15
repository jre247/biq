AdsListingItemAdView = require './ads-listing-item-ad'

module.exports = AdsListingItemDeliveryGroup = React.createClass
  propTypes:
    campaignId: React.PropTypes.string.isRequired
    item: React.PropTypes.object.isRequired
    onViewAdTag: React.PropTypes.func.isRequired

  mixins: [React.addons.PureRenderMixin]

  render: ->
    item = @props.item
    campaignId = @props.campaignId
    onViewAdTag = @props.onViewAdTag

    return (
      <div className="deliverygroup-container">
        <div className="deliverygroup">
          <div className="gridlist-item gridlist-item-list treelist-item-level0">
            <div className="col-with-label col-name">
              <span className='label'>Delivery Group:</span><br/>
              <span className="deliverygroup-label-#{item.name}"> {item.name} </span>
            </div>
          </div>
        </div>
        <div className="deliverygroup-ads clearfix deliverygroup-ads-#{item.name}">
          {_.map(item.ads, (ad) ->
            return <AdsListingItemAdView item={ad} campaignId={campaignId} key={ad.id} onViewAdTag={onViewAdTag}/>
          )}
        </div>
      </div>
    )
