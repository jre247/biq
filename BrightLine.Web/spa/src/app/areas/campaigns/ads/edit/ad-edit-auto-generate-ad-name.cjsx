module.exports = AutoGenerateAdName = React.createClass
  displayName: 'AutoGenerateAdName'

  mixins: [React.addons.PureRenderMixin]

  render: ->

    return (
      <a data-nw="auto-generate-ad-name" onClick={@props.onAutoGenerateAdName}>Auto generate Ad name</a>
    )
