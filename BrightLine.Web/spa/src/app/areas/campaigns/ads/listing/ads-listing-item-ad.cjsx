module.exports = AdsListingItemAd = React.createClass
  propTypes: 
    campaignId: React.PropTypes.string.isRequired
    item: React.PropTypes.object.isRequired
    onViewAdTag: React.PropTypes.func.isRequired

  mixins: [React.addons.PureRenderMixin]

  onViewAdTag: ->
    @props.onViewAdTag(@props.item) if @props.onViewAdTag


  componentDidMount: ->
    $el = $(ReactDOM.findDOMNode(this))
    $el.find("[data-toggle='tooltip']").tooltip()


  render: ->
    self = @
    campaignId = @props.campaignId
    item = @props.item

    return (
      <div className="gridlist-item gridlist-item-list treelist-item-level1" data-ad-id={item.id}>
        <div className="col-thumb">
          <a href="/campaigns/#{campaignId}/ads/#{item.id}/edit">
            <div className="col-thumb-img" style={backgroundImage: "url('#{utility.resource.getSrc(item.resource)}')"}>
            </div>
          </a>
        </div>
        <div className="col-name">
          <a href="/campaigns/#{campaignId}/ads/#{item.id}/edit" className="ellipsis">{item.name}</a>
        </div>

        <div className="col-with-label col-ad-type">
          <span className='label'>Ad Type:</span><br/>
          {item.adTypeName}
        </div>

        <div className="col-with-label col-ad-platform">
          <span className='label'>Platform:</span><br/>
          {item.platformName}
        </div>

        <div className="col-with-label col-ad-placement">
          <span className='label'>Placement:</span><br/>
          <span className="ellipsis">{item.placementName}</span>
        </div>

        <div className="col-with-label col-ad-datebegin">
          <span className='label'>Begin Date:</span>
          <br/>
          <span className={cs({hide: !item.beginDate})}>{utility.moment.format(item.beginDate, 'MM/DD/YY')}</span>
        </div>

        <div className="col-with-label col-ad-dateend">
          <span className='label'>End Date:</span>
          <br/>
          <span className={cs({hide: !item.endDate})}>{utility.moment.format(item.endDate, 'MM/DD/YY')}</span>
        </div>

        <div className="col-with-label col-ad-status">
          <span className='label'>Status:</span>
          <br/>
          {item.statusDescription}
        </div>

        <div className="col-actions">
          <span>
            <a href="/campaigns/#{campaignId}/ads/create?adType=#{item.adTypeId}&creative=#{item.creativeId}&platform=#{item.platformId}">
              <i className="fa fa-files-o" title="Create a similar Ad" data-toggle="tooltip" data-placement="top"></i>
            </a>
          </span>
          <span className={cs({hide: !item.adTag})}>
            <a className="view-tags" onClick={@onViewAdTag}>
              <i className="fa fa fa-tags" title="Tags" data-toggle="tooltip" data-placement="top"></i>
            </a>
          </span>
        </div>
      </div>

    )


