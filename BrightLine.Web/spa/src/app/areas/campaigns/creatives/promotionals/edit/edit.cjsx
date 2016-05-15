cache                   = (require 'services/index').cache
Loader                  = require 'components/loader/loader'

FieldsGeneralMixin    = require 'components/field/mixins/fields-general-mixin'
Field                 = require 'components/field/field'
FieldHelper           = require 'components/field/field-helper'
FieldValidationHelper = require 'components/field/field-validation-helper'
ValidationSummary     = require 'components/field/validation-summary'

formatters            = FieldHelper.formatters
formatterNumberPre    = formatters.number.general.formatterPre
formatterNumberPost   = formatters.number.general.formatterPost
formatterSelect       = formatters.select.getItemsFromLookup

CampaignLayout          = require 'areas/campaigns/shared/campaign-layout'


module.exports = CreativesPromotionalsEdit = React.createClass
  displayName: 'CreativesPromotionalsEdit'

  mixins: [FieldsGeneralMixin]

  getInitialState: ->
    params = @props.params
    campaignId = parseInt(params.campaignId)
    return {
      campaignId: campaignId
      creativeId: parseInt(params.creativeId || 0)
      loadedCampaignLayout: false
      loadedCreativeData: false
      validationsInfo: FieldValidationHelper.getFieldsValidationsInfo([])
      buttonStates:
        create: 'Create Promotional Creative'
        update: 'Update Promotional Creative'
        saving: 'Saving...'
        validating: 'Validating...'
        uploading: 'Uploading files...'
        original: ''
        current: ''
      goBackUrl: "/campaigns/#{campaignId}/creatives"
    }


  areVideoFieldsHidden: ->
    adTypeValue = @state.fieldsMap.adType.values[0]
    # Hide Video Fields, when Image fields are shown
    # Image fields are shown, if AdType is not selected, or if its selected value is not 10014[Commercial Spot]
    imageFieldsShouldBeShown = !adTypeValue || adTypeValue && adTypeValue.id != 10014
    return imageFieldsShouldBeShown


  componentWillUnmount: ->
    this._isUnMounted = true


  componentDidMount: ->
    params = @props.params

    campaignId = @state.campaignId
    creativeId = @state.creativeId

    deferredApiLayout = RSVP.defer()
    cache.hash
      lookups:                    ['Campaigns.getLookups']
      summary:                    ['Campaigns.Summary', campaignId]
    .then (apiData) =>
      return if @_isUnMounted

      apiData.loadedCampaignLayout = true
      @setState apiData

      titlePrefix = if creativeId == 0 then 'Create' else 'Edit'
      utility.adjustTitle("#{titlePrefix} Promotional Creative - #{apiData.summary.name}")

      deferredApiLayout.resolve()
    .catch App.onError

    deferredApiCreative = RSVP.defer()
    cache.hash
      creative:  ['Campaigns.Creatives.Promotional', creativeId]
    .then (apiData) =>
      apiData.creative = _.cloneDeep(apiData.creative) || {}

      _.extend(@state, apiData)

      deferredApiCreative.resolve()
    .catch App.onError

    RSVP.all([deferredApiLayout.promise, deferredApiCreative.promise])
    .then =>
      return if @_isUnMounted

      @state.loadedCreativeData = true

      if creativeId == 0
        @state.title = "New Promotional Creative"
        @state.buttonStates.original = 'create'
      else
        @state.title = "Edit Promotional Creative"
        @state.buttonStates.original = 'update'
      @state.buttonStates.current = @state.buttonStates.original

      creative = @state.creative
      resources = creative.resources || []
      resources = resources.reverse()

      adTypesItems = formatterSelect(_.filter(@state.lookups.adTypes, (at) -> at.isPromo))
      adFunctionsItems = formatterSelect(@state.lookups.adFunctions)

      adFormatsItems = formatterSelect(@state.lookups.adFormats)

      @state.fieldsMap = fieldsMap = {
        name: {
          type: 'string'
          name: 'name'
          displayName: 'Name'
          validations:
            required: true
            maxLength: 255
            unique: do =>
              uniqueValidator = =>
                nameCurrent = @state.fieldsMap.name.values[0].trim()
                return {
                  entity: 'Creative'
                  uniquePromise: cache.getFresh('Campaigns.Creatives.isNameDuplicate', campaignId, creativeId, nameCurrent)
                }
              uniqueValidator.prototype.debounceDelay = 500
              return uniqueValidator

          values: [creative.name || '']
          valuesUpdater: =>
            fieldsMap = @state.fieldsMap
            nameField = fieldsMap.name
            nameFieldValue = nameField.values[0]

            # Try setting the Promotional name from resource filename, if the name field is empty
            if nameFieldValue.length == 0
              resourceFieldEdited = _.find(@state.fields, (f) -> ['image', 'video'].indexOf(f.type) >= 0 && f.isBeingEdited)
              if resourceFieldEdited
                return [resourceFieldEdited.values[0]?.file?.name || '']

            return nameField.values
        }

        adType: {
          type: 'select'
          name: 'adType'
          displayName: 'Ad Type'
          validations:
            required: true
          # Only consider Promotional AdTypes
          values: _.filter(adTypesItems, (adType) -> adType.id == creative.adType?.id)
          items: adTypesItems
          searchable: true
          readOnlyUpdater: =>
            return @state.creativeId != 0 # Disable AdType when editing a Creative
        }

        adFormat: {
          type: 'select'
          name: 'adFormat'
          displayName: 'Ad Format'
          validations:
            required: true
          items: adFormatsItems
          values: _.filter(adFormatsItems, (af) -> af.id == creative.adFormat?.id)
          searchable: true
          readOnlyUpdater: =>
            return @state.creativeId != 0 # Disable AdFormat when editing a Creative
        }

        adFunction: {
          type: 'select'
          name: 'adFunction'
          displayName: 'Ad Function'
          validations:
            required: true
          values: _.filter(adFunctionsItems, (adFunction) -> adFunction.id == creative.adFunction?.id)
          valuesUpdater: =>
            adFunctionField = @state.fieldsMap.adFunction
            adFunctionValues = adFunctionField.values

            adTypeField = @state.fieldsMap.adType

            # Default value, if AdTypeField is not being edited
            if adFunctionField.isBeingEdited || !adTypeField.isBeingEdited
              return adFunctionValues

            # AdTypeField is being edited. Try setting up default value.
            adTypeValue = adTypeField.values[0]
            return adFunctionValues if !adTypeValue

            adTypeToAdFunctionDefaultValMap =
              '10001': '4' # Image Banner     => Click to Jump
              '10002': '4' # Animated Banner  => Click to Jump
              '10014': '1' # Commercial Spot  => Not Clickable
              '10017': '4' # Overlay          => Click to Jump
            defaultAdFunctionId = parseInt(adTypeToAdFunctionDefaultValMap[adTypeValue.id])
            return _.filter(adFunctionField.items, (adFunction) -> adFunction.id == defaultAdFunctionId)

          items: adFunctionsItems
          searchable: true
        }

        description: {
          type: 'string'
          name: 'description'
          displayName: 'Description'
          multiline: true
          values: [creative.description || '']
        }

        resourceVideoHD: {
          type: 'video'
          name: 'resourceVideoHD'
          displayName: 'Video (HD)'
          description: 'Upload an HD video'
          resourceType: 4
          values: [{resource: _.find(resources, (r) -> return (r.resourceType == 4))}]
          validations:
            extension: ['mp4']
          hideUpdater: @areVideoFieldsHidden
        }

        resourceVideoSD: {
          type: 'video'
          name: 'resourceVideoSD'
          displayName: 'Video (SD)'
          description: 'Upload an SD video'
          resourceType: 3
          values: [{resource: _.find(resources, (r) -> return (r.resourceType == 3))}]
          validations:
            extension: ['mp4']
          hideUpdater: @areVideoFieldsHidden
        }

        resourceImageHD: {
          type: 'image'
          name: 'resourceImageHD'
          displayName: 'Image (HD)'
          description: 'Upload an HD image'
          resourceType: 2
          values: [{resource: _.find(resources, (r) -> return (r.resourceType == 2))}]
          validations:
            extension: ['jpg', 'gif', 'png']
          hideUpdater: =>
            return !@areVideoFieldsHidden()
        }

        resourceImageSD: {
          type: 'image'
          name: 'resourceImageSD'
          displayName: 'Image (SD)'
          description: 'Upload an SD image'
          resourceType: 1
          values: [{resource: _.find(resources, (r) -> return (r.resourceType == 1))}]
          validations:
            extension: ['jpg', 'gif', 'png']
          hideUpdater: =>
            return !@areVideoFieldsHidden()
        }
      }

      @state.fields = [
        fieldsMap.name
        fieldsMap.adType
        fieldsMap.adFormat
        fieldsMap.adFunction
        fieldsMap.resourceVideoHD
        fieldsMap.resourceVideoSD
        fieldsMap.resourceImageHD
        fieldsMap.resourceImageSD
        fieldsMap.description
      ]

      @updateFields()
      do logger.log @state.fields, @state.fieldsMap

      @forceUpdate()
    .catch App.onError


  saveToServer: ->
    # There are no errors at this point. Save to backend

    # Update, and disable save button
    @state.buttonStates.current = 'saving'
    @forceUpdate()

    fields = @state.fields

    # First, prepare resourceIds for saving.
    resourceIds = _.chain(fields)
      .filter((field) -> !field.hide && field.values?[0]?.resource)   # Get resource fields with a resource that aren't hidden
      .map((field) -> field.values[0].resource.id)                    # Get their resource ids
      .value()
    resourceIdsString = resourceIds.join(',')
    do logger.log 'Resource ids', resourceIdsString

    saveData =
      id:       @state.creativeId
      campaign: @state.campaignId
      ResourceIds: resourceIds.join(',')

    for field in fields
      if field.type == 'select'
        saveData[field.name] = field.values[0].id
      else if ['image', 'video'].indexOf(field.type) < 0
        saveData[field.name] = field.values[0] || ''

    do logger.log 'fields and saveData', fields, saveData

    formData = new FormData()
    for key, val of saveData
      formData.append(key, val)

    return $.ajax
      url: "/api/save/Creative"
      type: "POST"
      data: formData
      cache: false
      contentType: false
      enctype: "multipart/form-data"
      processData: false
      success: (savedCreativeId) =>
        do logger.log 'savedCreativeId', savedCreativeId

        # Add this item to HighlightUpdates list
        hu = new utility.HighlightUpdates
          key: 'Campaigns.Creatives'
        hu.add("#promotionals-items .gridlist-item[data-id='#{savedCreativeId}']")

        # Clear cache
        cache.clear('Campaigns.Creatives.Promotional', @state.creativeId)
        cache.clear('Campaigns.Creatives.Promotional', savedCreativeId)

        # Redirect to Creatives
        url = "/campaigns/#{@props.params.campaignId}/creatives"
        page(url)
      error: =>
        # Restore, and enable save button
        @state.buttonStates.current = @state.buttonStates.original
        @forceUpdate()


  renderBody: ->
    return <Loader/> if !@state.loadedCreativeData

    return (
      <div className="row" id="creative-promotional-edit">
        <div className="widget stacked">
          <div className="widget-header">
            <i className="icon-tasks"></i>
            <h3>{@state.title}</h3>
          </div>
          <div className="widget-content">
            {@renderForm()}
          </div>
        </div>
      </div>
    )


  render: ->
    return <Loader/> if !@state.loadedCampaignLayout

    summary = @state.summary
    lookups = @state.lookups

    return (
      <CampaignLayout summary={summary} lookups={lookups} navCurrent="creatives">
        {@renderBody()}
      </CampaignLayout>
    )
