module.exports = Treelistifier = React.createClass
  mixins: [React.addons.PureRenderMixin]

  render: ->
    state = @props.state
    states = @props.states

    isStateDeliveryGroups = state == states.deliveryGroups
    isStatePlacements = state == states.placements
    isStateAds = state == states.ads

    return (
      <div className="treelistifier">
        <a data-nw="delivery-groups-filter" className={cs({
            "fa fa-indent": true,
            active: isStateDeliveryGroups
            inactive: !isStateDeliveryGroups
          })}
          title={cs({
            'Showing Ads grouped by Delivery Groups': isStateDeliveryGroups
            'View Ads grouped by Delivery Groups': !isStateDeliveryGroups
            })}
          onClick={@props.onToggleTreelistifier.bind(null, states.deliveryGroups)}
          data-toggle="tooltip" data-placement="top"></a>


        <a data-nw="placements-filter" className={cs({
            "fa fa-indent": true
            active: isStatePlacements
            inactive: !isStatePlacements
          })}
          title={cs({
            'Showing Ads grouped by Placements': isStatePlacements
            'View Ads grouped by Placements': !isStatePlacements
          })}
          onClick={@props.onToggleTreelistifier.bind(null, states.placements)}
          data-toggle="tooltip" data-placement="top"></a>


        <a data-nw="all-ads-filter" className={cs({
            "fa fa-th-list": true
            active: isStateAds
            inactive: !isStateAds
          })}
          title={cs({
            'Showing Ads': isStateAds
            'View all Ads': !isStateAds
            })}
          onClick={@props.onToggleTreelistifier.bind(null, states.ads)}
          data-toggle="tooltip" data-placement="top"></a>
      </div>
    )
