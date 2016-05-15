cache = (require 'services/index').cache
Loader = require 'components/loader/loader'

FieldsGeneralMixin    = require 'components/field/mixins/fields-general-mixin'
Field                 = require 'components/field/field'
FieldHelper           = require 'components/field/field-helper'
FieldValidationHelper = require 'components/field/field-validation-helper'
ValidationSummary     = require 'components/field/validation-summary'
constants             = (require 'helpers/config').constants

CampaignLayout              = require 'areas/campaigns/shared/campaign-layout'

module.exports = DeliveryGroupEdit = React.createClass
  displayName: 'DeliveryGroupEdit'

  mixins: [FieldsGeneralMixin]

  getInitialState: ->
    params = @props.params
    campaignId = params.campaignId

    return {
      deliveryGroupId: parseInt(params.deliveryGroupId || 0)
      loadedCampaignLayout: false
      loadedDeliveryGroup: false
      validationsInfo: FieldValidationHelper.getFieldsValidationsInfo([])
      buttonStates:
        create: 'Create Delivery Group'
        update: 'Update Delivery Group'
        saving: 'Saving...'
        validating: 'Validating...'
        uploading: 'Uploading files...'
        original: ''
        current: ''
      goBackUrl: "/campaigns/#{campaignId}/summary"
    }


  componentDidMount: ->
    params = @props.params

    campaignId = params.campaignId
    deliveryGroupId = @state.deliveryGroupId

    deferredApiLayout = RSVP.defer()
    cache.hash
      lookups:                    ['Campaigns.getLookups']
      summary:                    ['Campaigns.Summary', campaignId]
    .then (apiData) =>
      apiData.loadedCampaignLayout = true
      @setState apiData

      titlePrefix = if deliveryGroupId == 0 then 'Create' else 'Edit'
      utility.adjustTitle("#{titlePrefix} Delivery Group - #{apiData.summary.name}")

      deferredApiLayout.resolve()
    .catch App.onError

    deferredApiDG = RSVP.defer()
    cache.hash
      deliveryGroups:           ['Campaigns.DeliveryGroups', campaignId]
    .then (apiData) =>
      apiData.deliveryGroups = _.clone(apiData.deliveryGroups)

      _.extend(@state, apiData)

      deferredApiDG.resolve()
    .catch App.onError

    RSVP.all([deferredApiLayout.promise, deferredApiDG.promise])
    .then =>

      if deliveryGroupId == 0
        @state.title = "New Delivery Group"
        @state.buttonStates.original = 'create'
      else
        @state.title = "Edit Delivery Group"
        @state.buttonStates.original = 'update'
      @state.buttonStates.current = @state.buttonStates.original

      @state.deliveryGroup = deliveryGroup =
        _.find(@state.deliveryGroups, (dg) => return dg.id == deliveryGroupId) || {}

      formatters = FieldHelper.formatters
      formatterNumberPre = formatters.number.general.formatterPre
      formatterNumberPost = formatters.number.general.formatterPost

      mediaPartnerItems = FieldHelper.formatters.select.getItemsFromLookup(@state.lookups.mediaPartners)
      @state.fields = [
        {
          type: 'string'
          name: 'name'
          displayName: 'Name'
          validations:
            required: true
            maxLength: 255
          values: [deliveryGroup.name || '']
        }
        {
          type: 'select'
          name: 'mediapartner'
          displayName: 'Media Partner'
          values: _.filter(mediaPartnerItems, (c) -> c.id == deliveryGroup.mediaPartnerId)
          validations:
            required: true
          items: mediaPartnerItems
          searchable: true
        }
        {
          type: 'integer'
          name: 'impressionGoal'
          displayName: 'Impression Goal'
          values: [deliveryGroup.impressionGoal || null]
          validations:
            max: constants.intMax
          formatterPre: formatterNumberPre
          formatterPost: formatterNumberPost
        }
        {
          type: 'integer'
          name: 'mediaSpend'
          displayName: 'Media Spend'
          values: [deliveryGroup.mediaSpend || null]
          validations:
            max: constants.intMax
          formatterPre: formatterNumberPre
          formatterPost: formatterNumberPost
        }
      ]

      @state.loadedDeliveryGroups = true
      @forceUpdate()
    .catch App.onError


  saveToServer: ->
    # Update, and disable save button
    @state.buttonStates.current = 'saving'
    @forceUpdate()

    params = @props.params
    fields = @state.fields

    saveData =
      id: params.deliveryGroupId || '0'
      campaign: params.campaignId

    for field in fields
      if field.type == 'select'
        saveData[field.name] = field.values[0].id
      else
        saveData[field.name] = field.values[0] || ''

    do logger.log fields, saveData

    formData = new FormData()
    for key, val of saveData
      formData.append(key, val)

    return $.ajax
      url: "/api/save/DeliveryGroup"
      type: "POST"
      data: formData
      cache: false
      contentType: false
      enctype: "multipart/form-data"
      processData: false
      success: (savedDeliveryGroupId) =>
        do logger.log 'savedDeliveryGroupId', savedDeliveryGroupId

        # Add this item to HighlightUpdates list
        if @state.deliveryGroupId != 0
          hu = new utility.HighlightUpdates
            key: 'Campaigns.DeliveryGroups'
          hu.add("#deliverygroups-items .gridlist-item[data-id='#{savedDeliveryGroupId}']")

        # Clear cache
        cache.clear('Campaigns.DeliveryGroups', @props.params.campaignId)

        # Redirect to Summary
        url = "/campaigns/#{@props.params.campaignId}/summary"
        page(url)
      error: =>
        # Restore, and enable save button
        @state.buttonStates.current = @state.buttonStates.original
        @forceUpdate()


  renderBody: ->
    return <Loader/> if !@state.loadedDeliveryGroups

    return (
      <div className="row" id="deliverygroup-edit">
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
    campaignId = summary.id
    lookups = @state.lookups

    return (
      <CampaignLayout summary={summary} lookups={lookups} navCurrent="summary">
        {@renderBody()}
      </CampaignLayout>
    )
