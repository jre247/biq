cache = (require 'services/index').cache
Loader = require 'components/loader/loader'

FieldsGeneralMixin    = require 'components/field/mixins/fields-general-mixin'
Field                 = require 'components/field/field'
FieldHelper           = require 'components/field/field-helper'
FieldValidationHelper = require 'components/field/field-validation-helper'
ValidationSummary     = require 'components/field/validation-summary'
constants             = (require 'helpers/config').constants

formatters            = FieldHelper.formatters
formatterSelect       = formatters.select.getItemsFromLookup
formatterSelectOptionResolver = formatters.select.SelectOptionResolver
formatterSelectValueResolver  = formatters.select.SelectValueResolver

IEHelper              = require './instance-edit-helper'

module.exports = InstanceEdit = React.createClass
  displayName: 'InstanceEdit'

  propTypes:
    someProp: React.PropTypes.object

  mixins: [FieldsGeneralMixin]

  getInitialState: ->
    # entityName = @props.cmsModels.features[@props.featureId].models[@props.modelId].display
    entityName = @props.entityName

    state = _.extend({}, @props, {
      validationsInfo: FieldValidationHelper.getFieldsValidationsInfo([])
      buttonStates:
        create: "Create #{entityName}"
        update: "Update #{entityName}"
        saving: 'Saving...'
        validating: 'Validating...'
        uploading: 'Uploading files...'
        original: ''
        current: ''
      goBackUrl: "/campaigns/#{@props.campaignId}/creatives/destinations/#{@props.creativeId}/edit"
      processed: false,
      isCms: true
    })

    if state.instanceId == 0
      state.title = "New #{entityName}"
      state.buttonStates.original = 'create'
    else
      state.title = "Edit #{entityName}"
      state.buttonStates.original = 'update'
    state.buttonStates.current = state.buttonStates.original

    fields = state.instance.fields

    state.fields = fields

    return state


  componentDidMount: ->
    @prepareRefFieldApiData()


  prepareRefFieldApiData: ->

    getModelByName = (modelName, models) ->
      for modelId, modelObj of models
        if modelObj.name == modelName
          return modelObj

      # Nothing was found. return null
      return null

    cacheHash = {}
    [fieldsRefToPage, fieldsRefToModel] = [[], []]
    _.each(@state.fields, (field) =>
      if field.type == 'refToPage'
        fieldsRefToPage.push field
        cacheHash.cmsPages = ['Campaigns.CmsPages', @state.creativeId]
      if field.type == 'refToModel'
        cacheHash.cmsModels = ['Campaigns.CmsModels', @state.creativeId]
        fieldsRefToModel.push field
    )

    if fieldsRefToModel.length + fieldsRefToPage.length == 0
      # There are no ref fields. Render right away
      @setState
        processed: true
    else
      # There are ref fields. Fetch the referenced models/pages first
      cache.hash(cacheHash)
      .then (apiData) =>
        _.extend(@state, apiData)

        # Deal with pages
        if cacheHash.cmsPages
          _.each(fieldsRefToPage, (field) =>
            field.cmsPages = apiData.cmsPages
            field.items = formatterSelect(apiData.cmsPages)
            field.values = _.filter(field.items, (i) -> i.id == field.values?[0].id)
          )
          @setState
            processed: true

        # Deal with models
        if cacheHash.cmsModels
          promiseCmsModelInstances = []

          _.each(fieldsRefToModel, (field) =>
            field.cmsModels = apiData.cmsModels

            modelName = field.refModel.model

            # First, find the model with the name
            if field.refModel.type == 'known'
              # Since model is known, the model belongs in this current feature.
              models = apiData.cmsModels.features[@state.featureId].models
              cmsModel = getModelByName(modelName, models)

            else
              # Model is unknown to this feature. Find the model in all other features of this creative.
              for featureId, featureObj of apiData.cmsModels.features when featureId != @state.featureId
                cmsModel = getModelByName(modelName, featureObj.models)

                # Don't continue if a model was found
                if cmsModel
                  break

            if !cmsModel
              do logger.error "A refModel with name #{modelName} was not found in features of API ('Campaigns.CmsPages', #{@_state.creativeId})"
              @setState
                processed: true
                error: true

            # Get all the Instances (belonging to the referenced cmsModel), and create a Select
            deferred = RSVP.defer()
            promiseCmsModelInstances.push deferred.promise
            cache.get('Campaigns.CmsModelInstances', cmsModel.id, true)
            .then (cmsModelInstances) =>
              field.cmsModelInstances = cmsModelInstances

              field.items = formatterSelect(cmsModelInstances)
              field.values = _.filter(field.items, (i) -> i.id == field.values?[0].id || field.values?[0].instanceId)

              deferred.resolve()
            .catch App.onError
          )
          RSVP.all(promiseCmsModelInstances)
          .then =>
            @setState
              processed: true
          .catch App.onError
      .catch App.onError


  saveToServer: ->
    # Update, and disable save button
    @state.buttonStates.current = 'saving'
    @forceUpdate()

    # The following must return a promise
    fields = IEHelper.getSaveData(@state.fields)
    return @props.saveToServer(fields)


  saveToServerSuccess: (saveResponse) ->
    do logger.log 'saveToServerSuccess', saveResponse

    validityRaw = utility.json.toCamelCasing(saveResponse.validity || saveResponse)


    validity = FieldValidationHelper.getBoolMessage(validityRaw.success, validityRaw.message)
    if validity.valid

      @props.clearCache()

      # # Clear cache
      # cache.clear('Campaigns.CmsSettingInstances', @settingId)
      # cache.clear('Campaigns.CmsSettingInstances', @settingId, true)
      # cache.clear('Campaigns.CmsSettingInstance', @instanceId)


      # Redirect back to the Destination Creative Edit page
      url = "/campaigns/#{@props.campaignId}/creatives/destinations/#{@props.creativeId}/edit"
      page(url)
      return utility.promise.resolved 1

    else
      # Show the error and validation

      @state.validationsInfo = FieldValidationHelper.getFieldsValidationsInfo([
        validities: [validity]
      ])
      @state.validationsInfo.enableSave = true

      # Update, and restore save button
      @state.buttonStates.current = @state.buttonStates.original

      @forceUpdate()

      return utility.promise.empty()


  render: ->

    return <Loader delay={1}/> if !@state.processed

    return <div>error</div> if @state.error

    return (
      <div className="row" id="instance-edit">
        <div className="widget stacked">
          <div className="widget-header">
            <h3>{@state.title}</h3>
          </div>
          <div className="widget-content">
            {@renderForm()}
          </div>
        </div>
      </div>
    )
