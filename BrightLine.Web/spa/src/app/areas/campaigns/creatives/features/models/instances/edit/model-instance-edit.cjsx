cache = (require 'services/index').cache
Loader = require 'components/loader/loader'

CampaignLayout                  = require 'areas/campaigns/shared/campaign-layout'
InstanceEdit                    = require 'areas/campaigns/creatives/features/components/instance-edit'
IEHelper                        = require 'areas/campaigns/creatives/features/components/instance-edit-helper'

module.exports = ModelInstanceEdit = React.createClass
  displayName: 'ModelInstanceEdit'
  
  propTypes: 
    someProp: React.PropTypes.object
  
  mixins: [React.addons.PureRenderMixin]

  getInitialState: ->
    params = @props.params
    return {
      campaignId: parseInt(params.campaignId)
      creativeId: parseInt(params.creativeId)
      featureId:  parseInt(params.featureId)
      modelId:    parseInt(params.modelId)
      instanceId: parseInt(params.instanceId) || 0
    }


  componentWillMount: ->

    deferredApiLayout = RSVP.defer()

    cache.hash
      lookups:                  ['Campaigns.getLookups']
      summary:                  ['Campaigns.Summary', @state.campaignId]
    .then (apiData) =>
      
      titlePrefix = if @state.instanceId == 0 then 'Create' else 'Edit'
      utility.adjustTitle("#{titlePrefix} Model Instance - #{apiData.summary.name}")

      @setState apiData

      deferredApiLayout.resolve()
    .catch App.onError

    deferredApiEdit = RSVP.defer()  
    cache.hash
      cmsModels:        ['Campaigns.CmsModels', @state.creativeId]
      modelDefinitions: ['Campaigns.CmsModelDefinitions']
      instanceOriginal: ['Campaigns.CmsModelInstance', @state.instanceId]
    .then (apiData) =>
      _.extend(@state, apiData, {
        entityName: apiData.cmsModels.features[@state.featureId].models[@state.modelId].display
      })

      deferredApiEdit.resolve()
    .catch App.onError

    RSVP.all([deferredApiLayout.promise, deferredApiEdit.promise])
    .then =>
      # ModelInstance is the same format as ModelDefinition. Both could be used to create fields.
      # When creating, use the modelDefinition. When editing, use the instance.
      instance = null

      if @state.instanceId == 0
        # Get the model lookup object
        model = @state.cmsModels.features[@state.featureId].models[@state.modelId]

        # Get the modelDefinition Object that the model implements
        modelDefinitionId = model.modelDefinitionId
        modelDefinition = @state.modelDefinitions[modelDefinitionId]

        # Use the modelDefinition as a starting point for the instance
        instance = modelDefinition
      else
        # Use instance, as is.
        instance = @state.instanceOriginal

      do logger.log 'instance/modelDefinition', instance

      # Create a clone, so that changes doesn't affect future use
      @state.instance = _.cloneDeep(instance)

      # Convert certain fields' values into int, etc as needed
      IEHelper.parseAllValidationValues(@state.instance)
      IEHelper.prepareDefaultValues(@state.instance)
      IEHelper.nameFieldInsert(@state.instance.fields, @state.instanceId, @state.instance.name)

      @forceUpdate()
    .catch App.onError


  saveToServer: (fields) ->
    instanceName = IEHelper.nameFieldExtract(fields)

    instanceData = 
      id:       @state.instanceId
      name:     instanceName
      modelId:  @state.modelId
      fields:   fields

    url = "/api/cms/modelInstances/save"

    return $.ajax({
      url: url,
      type: "POST",
      data: JSON.stringify(instanceData),
      contentType: "application/json",
      dataType: "json"
    })

  
  clearCache: ->
    cache.clear('Campaigns.CmsModelInstances', @state.modelId)
    cache.clear('Campaigns.CmsModelInstances', @state.modelId, true)
    cache.clear('Campaigns.CmsModelInstance', @state.instanceId)


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



