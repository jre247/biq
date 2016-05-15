module.exports = AdTagExport = React.createClass
  displayName: 'AdTagExport'

  render: ->
    campaignId = @props.campaignId

    return (
      <span>
        <a href="/api/campaigns/#{campaignId}/adTagExport" className="fa fa-tags view-tags" target="_blank"  title="Export Ad Tags" data-toggle="tooltip" data-placement="top">
          <i className="" />
        </a>
      </span>
    )
