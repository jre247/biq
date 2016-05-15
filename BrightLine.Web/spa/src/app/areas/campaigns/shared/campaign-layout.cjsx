AnalyticsHelper = require 'areas/campaigns/analytics/helpers/campaign-analytics-helper'
module.exports = CampaignLayout = React.createClass
  displayName: 'CampaignLayout'

  propTypes:
    summary: React.PropTypes.object.isRequired
    navCurrent: React.PropTypes.string

  mixins: [React.addons.PureRenderMixin]

  render: ->
    summary = @props.summary

    return (
      <div id="campaign">
        <div id="campaign-content-header">
          <CampaignLayoutHeader summary={summary} navCurrent={@props.navCurrent} />
        </div>
        <div id="campaign-content-precontainer" className="cfww">
          <div id="campaign-content-container">
            {@props.children}
          </div>
        </div>
      </div>
    )


CampaignLayoutHeader = React.createClass
  displayName: 'CampaignLayoutHeader'

  propTypes:
    summary: React.PropTypes.object.isRequired
    navCurrent: React.PropTypes.string

  mixins: [React.addons.PureRenderMixin]

  getDefaultProps: ->
    return {
      navCurrent: 'summary'
    }

  renderThumb: ->
    summary = @props.summary
    if !summary.resource
      return null

    return (
      <div className="thumb">
        <img src={utility.resource.getSrc(summary.resource)} />
      </div>
    )

  renderLinks: ->
    summary = @props.summary

    campaignId = summary.id

    menu = []
    menu.push
      name: 'summary'
      text: 'Summary'
      icon: 'icon fa fa fa-tachometer'
      link: "/campaigns/#{campaignId}/summary"

    if utility.user.isnt(['AgencyPartner', 'MediaPartner'])
      menu.push
        name: 'creatives'
        text: 'Creatives'
        icon: 'glyphicon glyphicon-picture'
        link: "/campaigns/#{campaignId}/creatives"

    if utility.user.isnt(['AgencyPartner', 'MediaPartner'])
      menu.push
          name: 'ads'
          text: 'Ads'
          icon: 'glyphicon glyphicon-send'
          link: "/campaigns/#{campaignId}/ads"

    if @props.summary.hasAnalytics
      menu.push
        name: 'analytics'
        text: 'Analytics'
        icon: 'fa-line-chart'
        link: AnalyticsHelper.getAnalyticsUrl(@props.summary)

    if utility.user.is('Employee')
      menu.push
        name: 'manifest'
        text: 'Manifest'
        icon: 'glyphicon glyphicon-info-sign'
        link: "/api/campaigns/#{campaignId}/preview"
        target: '_blank'

    if utility.user.is(['Admin', 'Developer', 'AdOps'])
      menu.push({
        name: 'campaign-edit'
        text: 'Edit'
        icon: 'glyphicon glyphicon-pencil'
        link: "/campaigns/edit/#{campaignId}"
      })

    menu.push({
      name: 'campaign-publish'
      text: 'Publish'
      icon: 'glyphicon glyphicon-pencil'
      link: "/campaigns/history/#{campaignId}"
    })

    navCurrent = @props.navCurrent
    navCurrentMatched = false

    menuLinks = _.map(menu, (m) ->
      linkProps =
        href: m.link
        title: m.text
        activeClassName: 'active'

      if m.target
        linkProps.target = m.target

      rootLink = m.link.split('?')[0]
      rootCurrent = App.context.path.split('?')[0]

      activeClassName = ''
      if !navCurrentMatched && ((navCurrent && m.name == navCurrent) || (rootLink.indexOf(rootCurrent) == 0 || rootCurrent.indexOf(rootLink) == 0))
        activeClassName = 'active'
        navCurrentMatched = true

      return (
        <li className="l1" key={m.name}>
          <a {...linkProps} className="#{activeClassName}">
            <i className={"icon fa #{m.icon}"}></i>
            <span>{m.text}</span>
          </a>
        </li>
      )
    )
    return (
      <ul id="campaign-nav">
        {menuLinks}
      </ul>
    )

  render: ->
    summary = @props.summary

    # Do not show the Advertiser name if one hasn't been associated yet(null).
    title = _.filter([summary.advertiserName, summary.name]).join(': ')

    return (
      <div className="cfww">
        <div className="title">
          {@renderThumb()}
          <span>{title}</span>
        </div>
        <div className="breadcrumbs-container"></div>
        <div id="campaign-nav-container">
          {@renderLinks()}
        </div>
      </div>
    )
