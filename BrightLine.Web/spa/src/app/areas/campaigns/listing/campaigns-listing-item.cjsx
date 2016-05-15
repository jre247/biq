CampaignsListingItemList = require './campaigns-listing-item-list'
CampaignsListingItemGrid = require './campaigns-listing-item-grid'

module.exports = CampaignsListingItem = React.createClass
  displayName: 'CampaignsListingItem'

  propTypes:
    campaign: React.PropTypes.object.isRequired

  restrictSummary: (campaign) ->
    campaign = @props.campaign

    id = campaign.id
    user = _bl.user

    urlSummary = "/campaigns/#{id}/summary"
    urlNowhere = "javascript:void(0)"

    campaignHasAnalytics = campaign.hasAnalytics

    # By default, overlay goes nowhere, and summary icon is hidden
    campaign.overlayLinkHref = urlNowhere
    campaign.showSummary = false

    # These users should be able to view the campaign (by entrance into summary)
    if utility.user.is(['Employee', 'AgencyPartner', 'MediaPartner'])
      campaign.overlayLinkHref = urlSummary
      campaign.showSummary = true

    return campaign


  restrictCMS: (campaign) ->
    user = _bl.user

    # CMS link should only be shown if the campaign has CMS, and the user has one of the following roles.
    if campaign.hasCms && utility.user.is('Employee')
      campaign.showCms = true

    return campaign


  render: ->
    campaign = @restrictSummary(campaign)
    campaign = @restrictCMS(campaign)

    campaignListingItemProps =
      campaign: campaign
      onToggleCampaignFavorite: @props.onToggleCampaignFavorite.bind(null, campaign.id)
      key: "#{campaign.id}-#{@props.gridlist}"

    if @props.gridlist == 'list'
      return <CampaignsListingItemList {...campaignListingItemProps}/>
    else
      return <CampaignsListingItemGrid {...campaignListingItemProps}/>
