AdsListingItemAdView = require './ads-listing-item-ad'

module.exports = AdsListingItemPlacement = React.createClass
  propTypes:
    campaignId: React.PropTypes.string.isRequired
    item: React.PropTypes.object.isRequired
    onViewAdTag: React.PropTypes.func.isRequired

  mixins: [React.addons.PureRenderMixin]

  render: ->
    item = @props.item
    campaignId = @props.campaignId
    onViewAdTag = @props.onViewAdTag

    # Build classname for placement name to reference from Nightwatch tests
    #   *Note: replacing spaces with underscores since it is easier to reference this kind of classname from Nightwatch tests
    placementCloned = _.clone(item.name)
    placementNameForClassname = placementCloned.replace(/\s/g, '_')

    return (
      <div className="placement-container">
        <div className="placement">
          <div className="gridlist-item gridlist-item-list treelist-item-level0">
            <div className="col-with-label col-name">
              <span className='label'>Placement:</span><br/>
              <span className="placement-label-#{placementNameForClassname}"> {item.name} </span>
            </div>
            <div className={cs({
              "col-with-label col-ad-dimensions": true
              hide: !item.appNetowrk
            })}>
              <span className='label'>App/Network:</span><br/>
              {item.appNetwork}
            </div>
            <div className="col-with-label col-ad-type-group">
              <span className='label'>Ad Type Group:</span><br/>
              {item.adTypeGroupName}
            </div>

            <div className="col-with-label col-ad-dimensions">
              <span className='label'>Dimensions:</span><br/>
              {item.width}<span className={cs({
                hide: !item.width || !item.height
              })}>x</span>{item.height}
            </div>
          </div>
        </div>
        <div className="placement-ads clearfix placement-ads-#{placementNameForClassname}">
          {_.map(item.ads, (ad) ->
            return <AdsListingItemAdView item={ad} campaignId={campaignId} key={ad.id} onViewAdTag={onViewAdTag}/>
          )}
        </div>
      </div>
    )
