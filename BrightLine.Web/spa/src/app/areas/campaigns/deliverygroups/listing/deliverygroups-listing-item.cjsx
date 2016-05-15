
ProgressBar = require 'components/progressbar/progressbar'

module.exports = React.createClass
  displayName: 'DeliveryGroupsListingItem'
  propTypes:
    campaignId: React.PropTypes.any.isRequired
    item: React.PropTypes.object.isRequired

  mixins: [React.addons.PureRenderMixin]

  render: ->
    deliveryGroup = @props.item

    restrictedUserToDGEditLinkMap =
      true: "javascript:void(0)"
      false: "/campaigns/#{@props.campaignId}/deliverygroups/#{deliveryGroup.id}/edit"
    link = restrictedUserToDGEditLinkMap[deliveryGroup.restricted]

    return (
      <div className="gridlist-item gridlist-item-list" data-id={deliveryGroup.id}>
        <div className="col-name">
          <a href={link} className="ellipsis">{deliveryGroup.name}</a>
        </div>

        <div className="col-with-label col-125">
          <span className='label'>Media Partner:</span><br/>
          {deliveryGroup.mediaPartnerName}
        </div>

        <div className="col-with-label col-85">
          <span className='label'>Impression Goal:</span><br/>
          {deliveryGroup.impressionGoalFormatted}
        </div>

        <div className="col-with-label col-85">
          <span className='label'>Impression Count:</span><br/>
          {deliveryGroup.impressionCountFormatted}
        </div>

        <div className="col-with-label col-110" title={deliveryGroup.impressionPacingDescription} data-toggle="tooltip" data-placement="bottom">
          <span className='label'>Impression Pacing:</span><br/>
          <ProgressBar completePct={deliveryGroup.impressionPacing} />
        </div>

        <div className="col-with-label col-85">
          <span className='label'>Media Spend:</span><br/>
          {deliveryGroup.mediaSpend}
        </div>

        <div className="col-with-label col-date">
          <span className='label'>Begin Date:</span><br/>
          <span className={cs({hide: !!!deliveryGroup.beginDate})}>{utility.moment.format(deliveryGroup.beginDate,'MM/DD/YY')}</span>
        </div>

        <div className="col-with-label col-date">
          <span className='label'>End Date:</span><br/>
          <span className={cs({hide: !!!deliveryGroup.endDate})}>{utility.moment.format(deliveryGroup.endDate,'MM/DD/YY')}</span>
        </div>

        <div className="col-with-label">
          <span className='label'>Status:</span><br/>
          {deliveryGroup.Status}
        </div>
      </div>
    )
