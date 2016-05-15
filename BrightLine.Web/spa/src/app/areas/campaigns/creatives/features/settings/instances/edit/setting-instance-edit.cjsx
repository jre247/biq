cache = (require 'services/index').cache
Loader = require 'components/loader/loader'

CampaignLayout                  = require 'areas/campaigns/shared/campaign-layout'
InstanceEdit                    = require 'areas/campaigns/creatives/features/components/instance-edit'
IEHelper                        = require 'areas/campaigns/creatives/features/components/instance-edit-helper'

module.exports = SettingInstanceEdit = React.createClass
  displayName: 'SettingInstanceEdit'
  
  propTypes: 
    someProp: React.PropTypes.object
  
  mixins: [React.addons.PureRenderMixin]

  getInitialState: ->
    params = @props.params
    return {
      campaignId: parseInt(params.campaignId)
      creativeId: parseInt(params.creativeId)
      featureId:  parseInt(params.featureId)
      settingId:  parseInt(params.settingId)
      instanceId: parseInt(params.instanceId) || 0
      entityName: "Settings"
    }


  componentWillMount: ->

    deferredApiLayout = RSVP.defer()

    cache.hash
      lookups:                  ['Campaigns.getLookups']
      summary:                  ['Campaigns.Summary', @state.campaignId]
    .then (apiData) =>
      
      utility.adjustTitle("Edit Setting Instance - #{apiData.summary.name}")

      @setState apiData

      deferredApiLayout.resolve()
    .catch App.onError

    deferredApiEdit = RSVP.defer()  
    cache.hash
      cmsSettings:      ['Campaigns.CmsSettings', @state.creativeId]
      instanceOriginal: ['Campaigns.CmsSettingInstance', @state.instanceId]
    .then (apiData) =>
      
      _.extend(@state, apiData)

      deferredApiEdit.resolve()
    .catch App.onError

    RSVP.all([deferredApiLayout.promise, deferredApiEdit.promise])
    .then =>
      # Create a clone, so that changes doesn't affect future use
      @state.instance = _.cloneDeep(@state.instanceOriginal)

      # Convert certain fields' values into int, etc as needed
      IEHelper.parseAllValidationValues(@state.instance)
      IEHelper.prepareDefaultValues(@state.instance)

      @forceUpdate()
    .catch App.onError


  saveToServer: (fields) ->
    instanceData = 
      id:         @state.instanceId
      settingId:  @state.settingId
      fields:     fields

    url = "/api/cms/settingInstances/save"

    return $.ajax({
      url: url,
      type: "POST",
      data: JSON.stringify(instanceData),
      contentType: "application/json",
      dataType: "json"
    })

  
  clearCache: ->
    cache.clear('Campaigns.CmsSettingInstance', @state.instanceId)


  renderBody: ->
    return <Loader /> if !@state.instance

    <InstanceEdit {...@state} saveToServer={@saveToServer} clearCache={@clearCache}/>


  render: ->
    return <Loader /> if !@state.summary

    return (
      <CampaignLayout summary={@state.summary} lookups={@state.lookups} navCurrent='creatives'>
        {@renderBody()}
      </CampaignLayout>
    )



