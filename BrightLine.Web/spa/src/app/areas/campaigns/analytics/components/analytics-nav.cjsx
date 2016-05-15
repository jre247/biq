
AnalyticsHelper = require 'areas/campaigns/analytics/helpers/campaign-analytics-helper'

module.exports = React.createClass
  displayName: 'AnalyticsNav'

  propTypes:
    campaignId: React.PropTypes.any.isRequired
    action: React.PropTypes.string.isRequired
    paramsInfo: React.PropTypes.object.isRequired
    summary: React.PropTypes.object.isRequired

  mixins: [React.addons.PureRenderMixin]

  getLink: (view) ->
    linkRoot = "/campaigns/#{@props.campaignId}/analytics"
    viewConfigMap =
      overview:
        root: ''
        title: 'Overview'
      # Hiding Content page for now (reference Jira task: "BL-320: Remove/hide Content Detail page in dashboard")
      # content:
      #   root: '/content'
      #   title: 'Content Detail'
      promotional:
        root: '/promotional'
        title: 'Promotional Detail'

    viewConfig = viewConfigMap[view]
    urlRoot = linkRoot + viewConfig.root

    paramsInfoCustom =
      action: view
      url: urlRoot
      bd1: utility.getQueryValue('bd1')
      ed1: utility.getQueryValue('ed1')
      interval: utility.getQueryValue('int')
      dims: utility.getQueryValue('dims')

    redirectInfo = AnalyticsHelper.getRedirectInfo(@props.summary, paramsInfoCustom)

    return {
      view: view
      url: redirectInfo.url
      title: viewConfig.title
    }

  render: ->
    view = @props.action

    links = [
      @getLink('overview')
    ]

    if utility.user.isnt(['AgencyPartner', 'MediaPartner'])
      # Hiding Content page for now (reference Jira task: "BL-320: Remove/hide Content Detail page in dashboard")
      #links.push @getLink('content')
      links.push @getLink('promotional')


    return (
      <div id="analytics-nav-view" className="col-md-12">
        <ul id='analytics-nav' className="nav nav-pills">
          {_.map(links, (link) ->
            <li className={cs({active: link.view == view})} data-nw={link.title} key={link.view}>
              <a href={"#{link.url}"}>{link.title}</a>
            </li>
          )}
        </ul>
      </div>
    )
