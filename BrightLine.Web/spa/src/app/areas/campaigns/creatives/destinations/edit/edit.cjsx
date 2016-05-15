cache = (require 'services/index').cache
Loader = require 'components/loader/loader'

# FieldsMixin           = require 'components/field/fields-mixin'
Field                 = require 'components/field/field'
FieldSelect           = require 'components/field/components/field-select'
FieldHelper           = require 'components/field/field-helper'
FieldValidationHelper = require 'components/field/field-validation-helper'
constants             = (require 'helpers/config').constants

formatters            = FieldHelper.formatters
formatterNumberPre    = formatters.number.general.formatterPre
formatterNumberPost   = formatters.number.general.formatterPost
formatterSelect       = formatters.select.getItemsFromLookup

formatterSelectOptionResolver = formatters.select.SelectOptionResolver
formatterSelectValueResolver  = formatters.select.SelectValueResolver

CampaignLayout = require 'areas/campaigns/shared/campaign-layout'

FeatureModelsInstancesListing = require './feature-models-instances-listing/feature-models-instances-listing'

FieldsGeneralMixin = require 'components/field/mixins/fields-general-mixin'

BlueprintOption = require './components/blueprint-option'

DCEditHelper =
  getNewFeature: (featureTypeId) ->
    # Create a new feature instance (id = 0), with the featureType
    feature = {
      id: 0
      name: ''
      featureType: @getFeatureType(featureTypeId)
      isDeleted: false
    }
    if feature.featureType == null
      return null

    feature = @hydrateFeatureFields(feature)
    return feature


  getFeatureType: (featureTypeId) ->
    # Find and clone the feature
    featureType = _.clone(@state.lookups.featureTypes[featureTypeId])
    if typeof featureType == "undefined"
      do logger.error "lookups.featureTypes is missing featureType(id=#{featureTypeId})."
      return null

    return featureType


  hydrateFeatureFields: (feature) ->
    blueprintItems = formatterSelect(_.map(feature.featureType.blueprints, (blueprintId) => @state.lookups.blueprints[blueprintId]))

    feature.fieldsMap = fieldsMap =
      featureTypeName:
        type: 'string'
        name: 'featureTypeName'
        displayName: 'Feature Type'
        values: [feature.featureType.name]
        readOnly: true
      featureName:
        type: 'string'
        name: 'featureName'
        displayName: 'Feature Name'
        placeholder: 'Enter Feature name'
        validations:
          required: true
          maxLength: 255
          unique: =>
            currentFeature = feature
            currentFeatureName = currentFeature.fieldsMap.featureName.values[0].trim()

            # Get an array of all other features' names
            otherFeaturesNames = _.chain(@state.creative.features)
              .without(currentFeature)
              .map((f) -> f.fieldsMap.featureName.values[0])
              .value()

            isDuplicate = otherFeaturesNames.indexOf(currentFeatureName) >= 0

            return {
              entity: 'Feature'
              uniquePromise: utility.promise.resolved(isDuplicate)
            }
        values: [feature.name]
      blueprint:
        type: 'select'
        name: 'blueprint'
        displayName: 'Blueprint'
        validations:
          required: true
        items: blueprintItems
        values: _.filter(blueprintItems, (blueprintItem) -> blueprintItem.id == feature.blueprintId)
        searchable: true
        SelectValueComponent:   formatterSelectValueResolver(BlueprintOption)
        SelectOptionComponent:  formatterSelectOptionResolver(BlueprintOption)
        onUpdatePost: (field) =>
          @hydrateFeaturePageFields(feature)

          @updateFieldsArray()

          @forceUpdate()


    feature.fields = [
      fieldsMap.featureTypeName
      fieldsMap.featureName
      fieldsMap.blueprint
    ]

    _.each(feature.fields, (field, index) =>
      field.onUpdate = @onUpdate.bind(@, field)
      field.key = index
      field.classNameLeft = "col-xs-3"
      field.classNameRight = "col-xs-9"
    )

    @hydrateFeaturePageFields(feature)

    return feature


  hydrateFeaturePageFields: (feature) ->
    selectedBlueprint = feature.fieldsMap.blueprint.values[0]

    if !selectedBlueprint
      feature.pagesFields = []
      return

    pageDefinitions = @state.lookups.pageDefinitions

    feature.pagesFields = _.map(selectedBlueprint.pageDefinitionIds, (pageDefinitionId) ->
      pageDefinition = pageDefinitions[pageDefinitionId]

      pageInstance = feature.pages?[pageDefinitionId]

      pageField =
        type: 'string'
        name: "pageDefinitionName"
        placeholder: "Enter Page Name"
        displayName: "#{pageDefinition.name} Name"
        pageInstance: pageInstance
        pageDefinition: pageDefinition
        values: [pageInstance?.name || '']
        validations:
          required: true
          maxLength: 255
          unique: =>
            currentPageField = pageField
            currentPageFieldName = currentPageField.values[0].trim()

            # Get an array of all other features' names
            otherPageFieldNames = _.chain(feature.pagesFields)
              .without(currentPageField)
              .map((p) -> p.values[0])
              .value()

            isDuplicate = otherPageFieldNames.indexOf(currentPageFieldName) >= 0

            return {
              entity: 'Page'
              uniquePromise: utility.promise.resolved(isDuplicate)
            }
        dataNW:
          "data-nw-pagedefinitionid": pageDefinition.id

      return pageField
    )
    _.each(feature.pagesFields, (field, index) =>
      field.onUpdate = @onUpdate.bind(@, field)
      field.key = "pageField#{index}"
      field.classNameLeft = "col-xs-3"
      field.classNameRight = "col-xs-9"
    )

    do logger.log 'selectedBlueprint', selectedBlueprint, feature.pagesFields


module.exports = CreativesDestinationsEdit = React.createClass
  displayName: 'CreativesDestinationsEdit'

  propTypes:
    someProp: React.PropTypes.object

  mixins: [React.addons.PureRenderMixin, DCEditHelper, FieldsGeneralMixin]

  getInitialState: ->
    campaignId = parseInt(@props.params.campaignId)
    creativeId = parseInt(@props.params.creativeId || 0)

    ###
    There are two modes: edit, and view.
      Creating a creative: edit
      Opening an existing creative:
        User is Adops or Admin: edit, view
        Everyone else: view
    ###
    # Set up the availability of the two modes
    modesAvailable =
      edit: false
      view: false

    if creativeId == 0
      modesAvailable.edit = true
    else
      if utility.user.is('Employee')
        modesAvailable.edit = true
        modesAvailable.view = true
      else
        modesAvailable.view = true


    return {
      campaignId: campaignId
      creativeId: creativeId
      readOnly: creativeId != 0
      validationsInfo: FieldValidationHelper.getFieldsValidationsInfo([])
      buttonStates:
        create: 'Create Destination Creative'
        update: 'Update Destination Creative'
        saving: 'Saving...'
        validating: 'Validating...'
        uploading: 'Uploading files...'
        original: ''
        current: ''
      modesAvailable: modesAvailable
      goBackUrl: "/campaigns/#{campaignId}/creatives"
    }


  componentWillUnmount: ->
    this._isUnMounted = true


  componentWillMount: ->
    campaignId = @state.campaignId
    creativeId = @state.creativeId

    deferredApiLayout = RSVP.defer()
    cache.hash
      lookups:                      ['Campaigns.getLookups']
      summary:                      ['Campaigns.Summary', campaignId]
    .then (apiData) =>
      return if @_isUnMounted

      apiData.loadedLayout = true

      titlePrefix = if creativeId == 0 then 'Create' else 'Edit'
      utility.adjustTitle("#{titlePrefix} Destination Creative - #{apiData.summary.name}")

      @setState apiData
      deferredApiLayout.resolve()
    .catch App.onError

    deferredApiCreative = RSVP.defer()
    cache.hash
      creative:             ['Campaigns.Creatives.Destination', creativeId]
      "features:features":  ['Campaigns.Features', campaignId]
      cmsModels:            ['Campaigns.CmsModels', creativeId]
      cmsSettings:          ['Campaigns.CmsSettings', creativeId]
    .then (apiData) =>
      _.extend(@state, apiData)
      deferredApiCreative.resolve()
    .catch App.onError

    RSVP.all([deferredApiLayout.promise, deferredApiCreative.promise])
    .then =>
      return if @_isUnMounted

      @state.loadedCreative = true

      if creativeId == 0
        @state.title = "New Destination Creative"
        @state.buttonStates.original = 'create'
      else
        @state.title = "Edit Destination Creative"
        @state.buttonStates.original = 'update'
      @state.buttonStates.current = @state.buttonStates.original

      creative = @state.creative = _.cloneDeep(@state.creative) ||
        id: 0
        name: ''
        resource: null
        features: []

      creative.featuresOriginal = creative.features || []
      creative.features = _.chain(creative.featuresOriginal)
        .map((feature) => _.cloneDeep(@state.features[feature.id]))
        .map(@hydrateFeatureFields)
        .value()

      creative.primaryFieldsMap =
        name:
          type: 'string'
          name: 'name'
          displayName: 'Name'
          placeholder: 'Enter a Destination Creative name'
          values: [creative.name || '']
          validations:
            required: true
            maxLength: 255
            unique: =>

              nameCurrent = @state.creative.primaryFieldsMap.name.values[0].trim()
              return {
                entity: 'Creative'
                uniquePromise: cache.getFresh('Campaigns.Creatives.isNameDuplicate', @state.campaignId, @state.creativeId, nameCurrent)
              }
        description:
          type: 'string'
          name: 'description'
          displayName: 'Description'
          placeholder: 'Enter a description'
          values: [creative.description || '']
        resource:
          type: 'image'
          name: 'thumbnail'
          displayName: 'Thumbnail'
          description: "Optimal image size is 260 width x 140 height"
          resourceType: 2
          values: [{resource: creative.resource}]
          validations:
            extension: ['jpg', 'gif', 'png']
        inactivityThreshold :
          type: 'number'
          name: 'inactivityThreshold'
          displayName: 'Inactivity Threshold '
          placeholder: if @state.readOnly then '' else 'Enter an Inactivity Threshold '
          values: [creative.inactivityThreshold]
          validations:
            max: constants.intMax

      creative.primaryFields = [
        creative.primaryFieldsMap.name
        creative.primaryFieldsMap.description
        creative.primaryFieldsMap.resource,
        creative.primaryFieldsMap.inactivityThreshold
      ]

      _.each(creative.primaryFields, (field, index) =>
        field.onUpdate = @onUpdate.bind(@, field)
        field.key = index
      )

      creative.ads = _.chain(creative.ads)
          .map(@getAdEditRepoNameFieldsMap)
          .value()

      @updateFieldsArray()

      # Start off with view mode, if it's available. This is used for the create/update button, until
      if @state.modesAvailable.view
        @setMode('view')
      else if @state.modesAvailable.edit
        @setMode('edit')

      @forceUpdate()
    .catch App.onError

  getAdEditRepoNameFieldsMap: (adToEditRepoName) ->
    adToEditRepoName.fieldsMap = fieldsMap =
      id:
          type: 'integer'
          name: 'repoNameEdit.id'
          displayName: 'Id: '
          values: [adToEditRepoName.id]
          readOnly: true
          readOnlyUpdater: =>
            return true

      name:
          type: 'string'
          name: 'repoNameEdit.name'
          displayName: 'Name: '
          values: [adToEditRepoName.name]
          readOnly: true
          readOnlyUpdater: =>
            return true

      platformId:
          type: 'integer'
          name: 'repoNameEdit.platformId'
          displayName: 'Platform Id: '
          values: [adToEditRepoName.platformId]
          readOnly: true
          readOnlyUpdater: =>
            return true

      platformName:
          type: 'string'
          name: 'repoNameEdit.platformName'
          displayName: 'Platform: '
          values: [adToEditRepoName.platformName]
          readOnly: true
          readOnlyUpdater: =>
            return true

      repoName:
          type: 'string'
          name: 'repoNameEdit.repoName'
          displayName: 'Repo Name: '
          values: [adToEditRepoName.repoName]

    adToEditRepoName.fields = [
      fieldsMap.id
      fieldsMap.name
      fieldsMap.platformName
      fieldsMap.repoName
    ]

    _.each(adToEditRepoName.fields, (field, index) =>
      field.onUpdate = @onUpdate.bind(@, field)
      field.key = index
      field.classNameLeft = "col-xs-5"
      field.classNameRight = "col-xs-7"
    )

    return adToEditRepoName


  setMode: (mode) ->
    # Only switch to a mode, if it's available
    if @state.modesAvailable[mode]
      @state.modeCurrent = mode

      @state.modeCurrentCssClass = "mode-#{mode}"

    readOnly = mode == 'view'

    # Build a list of fields that will always stay as read-only
    fieldsToIgnore = ['featureTypeName', 'repoNameEdit.id', 'repoNameEdit.name', 'repoNameEdit.platformId', 'repoNameEdit.platformName']

    for field in @state.fields when !_.includes(fieldsToIgnore, field.name)
      field.readOnly = readOnly


  onSetMode: ->
    currentModeToNextModeMap =
      edit: 'view'
      view: 'edit'
    nextMode = currentModeToNextModeMap[@state.modeCurrent]
    @setMode(nextMode)

    @forceUpdate()


  updateFieldsArray: ->
    @state.fields = _.flattenDeep([
      @state.creative.primaryFields,
      _.map(@state.creative.features, (feature) -> [feature.fields, feature.pagesFields]),
      _.map(@state.creative.ads, (ad) -> [ad.fields])
    ])

  onFeatureAdd: (selections) ->
    featureTypeId = selections[0].id

    # Create a new/fake feature instance(id = 0), housing the featureType object
    feature = @getNewFeature(featureTypeId)

    @state.creative.features.push feature

    @updateFieldsArray()

    # Rerender
    @forceUpdate()


  onFeatureRemove: (feature) ->
    # Find the current index of the feature
    featureIndex = @state.creative.features.indexOf(feature)

    # Remove it from the array
    @state.creative.features.splice(featureIndex, 1)

    @updateFieldsArray()

    # Rerender
    @forceUpdate()


  saveToServer: ->
    # Update, and disable save button
    @state.buttonStates.current = 'saving'
    @forceUpdate()

    fields = @state.fields

    # Prepare data for saving

    # First, prepare resourceIds for saving.
    resourceIds = _.chain(fields)
      .filter((field) -> !field.hide && field.values?[0]?.resource)   # Get resource fields with a resource that aren't hidden
      .map((field) -> field.values[0].resource.id)                    # Get their resource ids
      .value()
    resourceIdsString = resourceIds.join(',')
    do logger.log 'Resource ids', resourceIdsString

    primaryFieldsMap = @state.creative.primaryFieldsMap
    adsFieldMap = @state.creative.ads

    campaignId = @state.campaignId
    creativeId = @state.creativeId

    saveData =
      id:                   creativeId
      campaign:             campaignId
      name:                 primaryFieldsMap.name                 .values[0]
      description:          primaryFieldsMap.description          .values[0]
      ResourceIds:          resourceIdsString
      Thumbnail_Id:         resourceIdsString
      adType:               10010 #a lways 10010 (Brand Destination)
      adFunction:           5 # always 5 (App)
      inactivityThreshold:  primaryFieldsMap.inactivityThreshold  .values[0]
      features:     _.map(@state.creative.features, (feature) ->
          featureSaveObj =
            id:           feature.id
            name:         feature.fieldsMap.featureName.values[0]
            campaign:     campaignId
            featureType:  feature.featureType.id
            blueprint:    feature.fieldsMap.blueprint.values[0].id
            isDeleted:    false
            pages:        _.map(feature.pagesFields, (pageField) ->
                pageSaveObj =
                  id:               pageField.pageInstance?.id || 0
                  name:             pageField.values[0]
                  pageDefinition:   pageField.pageDefinition.id
                return pageSaveObj
              )

          return featureSaveObj
        )
      ads: _.map(adsFieldMap, (adToEditRepoName) ->
        adSaveObj =
          id:           adToEditRepoName.fieldsMap.id.values[0]
          name:         adToEditRepoName.fieldsMap.name.values[0]
          platformId:   adToEditRepoName.fieldsMap.platformId.values[0]
          platformName: adToEditRepoName.fieldsMap.platformName.values[0]
          repoName:     adToEditRepoName.fieldsMap.repoName.values[0]
          isDeleted:    false

        return adSaveObj
      )

    do logger.log saveData

    self = @
    $.ajax({
      url: "/api/creatives/destination/save",
      type: "POST",
      data: JSON.stringify(saveData),
      contentType: "application/json"
    })
    .then (creativeId) ->
      self.saveToServerSuccess(creativeId)

  saveToServerSuccess: (savedCreativeId) ->
    do logger.log 'savedCreativeId', savedCreativeId

    # Add this item to HighlightUpdates list
    hu = new utility.HighlightUpdates
      key: 'Campaigns.Creatives'
    hu.add("#campaign-creatives .gridlist-item[data-id='#{savedCreativeId}']")

    # Clear cache
    cache.clear('Campaigns.Creatives.Destination', @state.creativeId)
    cache.clear('Campaigns.Creatives.Destination', savedCreativeId)

    # Redirect to Summary
    url = "/campaigns/#{@props.params.campaignId}/creatives"
    page(url)

  renderAdsFields: () ->
    _.map(@state.creative.ads, (field, index) =>
      return (
        <div className="field-group-row field-group-row-#{index}" key={index}>
          <div className="col-xs-12">
            <div className="col-xs-4 field-group-column name">
              <div>
                <Field {...@props} {...field.fieldsMap.name}/>
              </div>
            </div>
            <div className="col-xs-3 field-group-column platformName">
              <div>
                <Field {...@props} {...field.fieldsMap.platformName} />
              </div>
            </div>
            <div className="col-xs-4 field-group-column repoName">
              <div>
                <Field {...@props} {...field.fieldsMap.repoName} />
              </div>
            </div>
          </div>
        </div>
      )
    )

  renderBody: ->
    return <Loader/> if !@state.loadedCreative

    return (
      <div className="row" id="destination-edit">
        <div className="widget stacked">
          <div className="widget-header">
            <i className="icon-tasks"></i>
            <h3>{@state.title}</h3>
          </div>
          <div className="widget-content">

            <div className="features-container section">
              <h4>Features</h4>

              <div id="features-and-blueprints" className="clearfix ">
                {_.map(@state.creative.features, (feature, index) =>
                  featureId = feature.id

                  return (
                    <div className="feature-fields-and-models-listing-container clearfix" key={index} data-feature-id={featureId} data-featuretype-id={feature.featureType.id}>
                      <a className={cs({"remove-feature": true, hide: @state.readOnly})}
                          onClick={@onFeatureRemove.bind(@, feature)} title="Remove this feature"><i className="fa fa-times"/></a>
                      <div className="feature-fields-container col-xs-7 form-horizontal">
                        {_.map(feature.fields, (field) -> <Field {...field} />)}
                        {_.map(feature.pagesFields, (field) -> <Field {...field} />)}
                      </div>
                      <div className="feature-models-listing-container col-xs-5">
                        {
                          if @state.modeCurrent == 'view'
                            featureProps =
                              campaignId:         @state.campaignId
                              creativeId:         @state.creativeId
                              featureId:          featureId
                              featureModels:      @state.cmsModels?.features?[featureId]?.models
                              featureSetting:    _.find(@state.cmsSettings?.features?[featureId]?.settings || {})

                            <FeatureModelsInstancesListing {...featureProps}/> if featureProps.featureModels && featureProps.featureSetting
                        }
                      </div>
                    </div>
                  )
                )}
              </div>

              <div id="add-feature-container" className={cs({clearfix: true, hide: @state.readOnly})}>
                Add Features to Creative
                <div id="add-feature">
                  <FieldSelect
                    items={formatterSelect(@state.lookups.featureTypes)}
                    values={[]}
                    clearable={false}
                    placeholder="Select a Feature Type"
                    onChange={@onFeatureAdd} />
                </div>
              </div>

            </div>

            <div className="section">
              <div className="form-horizontal col-xs-12">

                <div className="fields-container clearfix">
                  {_.map(@state.creative.primaryFields, (field) -> <Field {...field} />)}
                </div>

                <div className={cs({
                    "fields-container clearfix": true
                    "section": true
                    hidden: !@state.creative.id || @state.creative.ads?.length == 0
                  })}>
                  <h4>Ads</h4>

                  <div className="col-xs-12 ads">
                    {@renderAdsFields()}
                  </div>
                </div>

                <div className="col-xs-12">
                  {@renderValidationSummary()}
                </div>

                <div className="col-xs-12">
                  <div className="form-group">
                    <div className="col-xs-offset-2 col-xs-8">
                      <input type="button" className={cs({
                          "btn btn-success save": true
                          hide: @state.modeCurrent != 'edit'
                          disabled: @state.validationsInfo.validitiesErrors.length > 0 || @state.buttonStates.current != @state.buttonStates.original
                        })}
                        value={@state.buttonStates[@state.buttonStates.current]}
                        onClick={@onSave}
                        />

                      <span className={cs({hide: @state.creativeId == 0 || !@state.modesAvailable.edit})} style={marginLeft: 15}>
                        <input type="button" className={cs({"btn btn-info setMode": true, hide: @state.modeCurrent != 'edit'})} value="Cancel" onClick={@onSetMode} />
                        <input type="button" className={cs({"btn btn-info setMode": true, hide: @state.modeCurrent != 'view'})} value="Edit" onClick={@onSetMode} />
                      </span>

                      <a className="btn-cancel" href={@state.goBackUrl}>Go back</a>
                    </div>
                  </div>
                </div>

              </div>
            </div>

          </div>
        </div>
      </div>
    )


  render: ->
    return <Loader/> if !@state.loadedLayout

    return (
      <CampaignLayout summary={@state.summary} lookups={@state.lookups} navCurrent='creatives'>
        {@renderBody()}
      </CampaignLayout>
    )
