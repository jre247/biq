cache           = (require 'services/index').cache
Loader          = require 'components/loader/loader'

FieldsGeneralMixin    = require 'components/field/mixins/fields-general-mixin'
Field                 = require 'components/field/field'
FieldList             = require 'components/field/field-list'
FieldHelper           = require 'components/field/field-helper'
FieldValidationHelper = require 'components/field/field-validation-helper'
ValidationSummary     = require 'components/field/validation-summary'
constants             = (require 'helpers/config').constants

formatters            = FieldHelper.formatters
formatterNumberPre    = formatters.number.general.formatterPre
formatterNumberPost   = formatters.number.general.formatterPost
formatterSelect       = formatters.select.getItemsFromLookup

formatterSelectOptionResolver = formatters.select.SelectOptionResolver
formatterSelectValueResolver = formatters.select.SelectValueResolver

AdEditMissingCreatives  = require './ad-edit-missing-creatives'
AdEditCreativeOption    = require './ad-edit-creative-option'
AutoGenerateAdName      = require './ad-edit-auto-generate-ad-name'

CampaignLayout          = require 'areas/campaigns/shared/campaign-layout'

AdEditHelper =
  getPrimaryFieldsMap:(ad, campaignId) ->
    primaryFieldsMap = {
        adType: {
          type: 'select'
          name: 'adType'
          displayName: 'Ad Type'
          validations:
            required: true
          items: @state.adTypes
          values: _.filter(@state.adTypes, (at) -> at.id == ad.adTypeId)
          searchable: true
          readOnlyUpdater: =>
            return @state.adId != 0 # Disable AdType when editing an Ad
        }

        creative: {
          type: 'select'
          name: 'creative'
          displayName: 'Creative'
          validations:
            required: true
          items: []
          itemsUpdater: =>
            return formatterSelect(@getCreativesOfAdType())

          values: _.filter(@state.creatives, (at) -> at.id == ad.creativeId)
          searchable: true
          SelectValueComponent:   formatterSelectValueResolver(AdEditCreativeOption)
          SelectOptionComponent:  formatterSelectOptionResolver(AdEditCreativeOption)
        }

        deliveryGroup: {
          type: 'select'
          name: 'deliveryGroup'
          displayName: 'Delivery Group'
          items: @state.deliveryGroups
          values: _.filter(@state.deliveryGroups, (dg) -> dg.id == ad.deliveryGroupId)
          searchable: true
          validations: {}
          validationsUpdater: (config) =>
            dgField = @state.fieldsMap.primaryFields.deliveryGroup
            adTypeField = @state.fieldsMap.primaryFields.adType

            # Return its past validation configuration if adType is not being edited, and this isn't on initialize
            return dgField.validations if !adTypeField.isBeingEdited && !config.initialize

            # adType is being edited. Require DeliveryGroup to be required for certain AdTypes
            requiredOnAdTypes = [10001, 10017] # 10001: Image Banner; 10017: Overlay
            if requiredOnAdTypes.indexOf(adTypeField.values[0]?.id) >= 0
              # Make DeliveryGroup required
              return {
                required: true
              }
            else
              # Make DeliveryGroup unrequired
              return {}
        }

        placement: {
          type: 'select'
          name: 'placement'
          displayName: 'Placement'
          validations:
            required: true
          items: []
          itemsUpdater: =>
            deliveryGroupId = @state.fieldsMap.primaryFields.deliveryGroup.values[0]?.id || -1
            if deliveryGroupId > -1
              # find all placements where they have the same media partner as the selected delivery group's media partner
              deliveryGroup = _.find(@state.deliveryGroups, (d) -> return d.id == deliveryGroupId)
              placements = @state.placements
              placementsForMediaPartner = _.filter(placements, (p) -> p.mediaPartnerId == deliveryGroup.mediaPartnerId)
              #placementsForMediaPartner = _.orderBy(placementsForMediaPartner, ['name'], ['asc'])

            # This gathers all the Placements belonging to all the AdTypeGroups of the current AdType.
            adTypeId = @state.fieldsMap.primaryFields.adType.values[0]?.id || -1

            # immediately return placements filtered by delivery group's
            # media partner if AdType hasn't been selected yet
            return (placementsForMediaPartner || []) if adTypeId == -1

            adTypeGroupIds = @state.lookups.adTypes[adTypeId]?.adTypeGroupId || []
            placements = @state.placements

            placementsOfAdTypeGroups = _.chain(adTypeGroupIds)
              # Gather all the placements that belong to this adTypeGroupId
              .map((adTypeGroupId) -> _.filter(placements, (p) -> p.adTypeGroupId == adTypeGroupId))
              .flattenDeep()  # Flatten the array of arrays of placements
              .unique()   # Don't consider duplicate placements
              .value()

            #find the overlap between the placements for media partner and also placements for ad type group
            intersectedPlacements = _.intersection(placementsForMediaPartner || placementsOfAdTypeGroups, placementsOfAdTypeGroups)

            return intersectedPlacements

          values: _.filter(@state.placements, (p) -> p.id == ad.placementId)
          searchable: true
        }

        creativeDestination: {
          type: 'select'
          name: 'creativeDestination'
          displayName: 'Destination Creative'
          validations:
            required: true
          hide: false
          hideUpdater: => # Hide this field, when AdFunction is not Click To Jump
            creativeId = @state.fieldsMap.primaryFields.creative.values[0]?.id
            if !creativeId
              return true
            creative = @state.creativesOriginal[creativeId]
            adFunctionIsClickToJump = creative.adFunctionId == 4 # Ad Function 4 is Click To Jump
            return !adFunctionIsClickToJump
          values: _.filter(@state.creatives, (c) -> c.id == ad.destinationAdCreativeId)
          items: []
          itemsUpdater: =>
            if @state.fieldsMap.primaryFields.creativeDestination.hide
              return []
            else
              return @state.creativesDestination

          searchable: true
        }

        companionAd: {
          type: 'select'
          name: 'companionAd'
          displayName: 'Companion Ad'
          validations:
            required: true
          hide: false
          hideUpdater: => # Hide this field when AdType is not Commercial Spots
            adTypeId = @state.fieldsMap.primaryFields.adType.values[0]?.id || -1

            adTypeIsCommercialSpot = adTypeId == 10014 # AdType 10014  is Commercial Spot
            return !adTypeIsCommercialSpot
          items: []
          itemsUpdater: =>
            if @state.fieldsMap.primaryFields.companionAd.hide
              return []
            else
              return @state.companionAds

          values: _.filter(@state.ads, (a) -> a.id == ad.companionAdId)
          searchable: true
        }

        beginDate: {
          type: 'datetime'
          name: 'beginDate'
          displayName: 'Begin Date'
          calendars: 1
          values: [ad.beginDateMoment]
          validations:
            max: =>
              endDate = @state.fieldsMap.primaryFields.endDate.values[0]
              return endDate if endDate
        }

        endDate: {
          type: 'datetime'
          name: 'endDate'
          displayName: 'End Date'
          calendars: 1
          values: [ad.endDateMoment]
          validations:
            min: =>
              beginDate = @state.fieldsMap.primaryFields.beginDate.values[0]
              return beginDate if beginDate
        }

        platform: {
          type: 'select'
          name: 'platform'
          displayName: 'Platform'
          validations:
            required: true
          items: @state.platforms
          values: _.filter(@state.platforms, (p) -> p.id == ad.platformId)
          searchable: true
        }

        name: {
          type: 'string'
          name: 'name'
          placeholder: 'Enter an Ad name'
          displayName: 'Name'
          validations:
            required: true
            maxLength: 255
            unique: do =>
              uniqueValidator = =>
                nameCurrent = @state.fieldsMap.primaryFields.name.values[0].trim()
                return {
                  entity: 'Ad'
                  uniquePromise: cache.getFresh('Campaigns.Ads.isNameDuplicate', campaignId, @state.adId, nameCurrent)
                }
              uniqueValidator.prototype.debounceDelay = 500
              return uniqueValidator
          values: [ad.name || '']

          footer: AutoGenerateAdName
          onAutoGenerateAdName: =>
            fieldMap = @state.fieldsMap.primaryFields
            # Get the selected creative's name. If no creative is selected, fall back to an empty string
            creativeSelected = fieldMap.creative.values[0]
            creativeName = if creativeSelected then "#{creativeSelected.name} " else ''

            platformSelected = fieldMap.platform.values[0]
            platformName = if platformSelected then "#{platformSelected.name}" else ''

            placementSelected = fieldMap.placement.values[0]
            placementName = if placementSelected then placementSelected.name else ''

            nameField = fieldMap.name
            nameField.values = ["#{creativeName} (#{_.filter([platformName, placementName]).join(' - ')})"]

            @validateFields([nameField]) # Validate and rerender
        }

        isReported: {
          type: 'bool'
          name: 'isReported'
          displayName: 'Is Reported'
          values: [ad.isReported || false]
        }
      }

    primaryFieldsMap.fields = [
      primaryFieldsMap.adType
      primaryFieldsMap.creative
      primaryFieldsMap.deliveryGroup
      primaryFieldsMap.placement
      primaryFieldsMap.companionAd
      primaryFieldsMap.creativeDestination
      primaryFieldsMap.beginDate
      primaryFieldsMap.endDate
      primaryFieldsMap.platform
      primaryFieldsMap.name
      primaryFieldsMap.isReported
    ]

    return primaryFieldsMap

  getCoordinatesFieldsMap:(ad, campaignId) ->
    coordinatesFieldMap = fieldsMap = {
        xCoordinateRokuSd: {
          type: 'number'
          name: 'xCoordinateRokuSd'
          displayName: 'X:'
          values: [ad.xCoordinateSd]
          validations:
            min: 0
            max: 720
          hideUpdater: => # Hide this field when Platform selection is not Roku
            platformId = @state.fieldsMap.primaryFields.platform?.values[0]?.id || -1

            platformIsRoku = platformId == 25 # AdType 25  is Roku
            if (!platformIsRoku)
              @state.fieldsMap.coordinates.xCoordinateRokuSd?.values[0] = null

            return !platformIsRoku
        }

        xCoordinateRokuHd: {
          type: 'number'
          name: 'xCoordinateRokuHd'
          displayName: 'X:'
          values: [ad.xCoordinateHd]
          validations:
            min: 0
            max: 1280
          hideUpdater: => # Hide this field when Platform selection is not Roku
            platformId = @state.fieldsMap.primaryFields.platform?.values[0]?.id || -1

            platformIsRoku = platformId == 25 # AdType 25  is Roku
            if (!platformIsRoku)
              @state.fieldsMap.coordinates.xCoordinateRokuHd?.values[0] = null

            return !platformIsRoku
        }

        # Note that a platform that is not Roku will only have the user input HD X/Y coordinates and the resolution will be 1920x1080
        xCoordinateHtml5Hd: {
          type: 'number'
          name: 'xCoordinateHtml5Hd'
          displayName: 'X:'
          values: [ad.xCoordinateHd]
          validations:
            min: 0
            max: 1920
          hideUpdater: => # Hide this field when Platform selection is not Roku
            platformId = @state.fieldsMap.primaryFields.platform?.values[0]?.id || -1

            platformIsRoku = platformId == 25 # AdType 25  is Roku
            if (platformIsRoku)
              @state.fieldsMap.coordinates.xCoordinateHtml5Hd?.values[0] = null

            return platformIsRoku
        }

        yCoordinateRokuSd: {
          type: 'number'
          name: 'yCoordinateRokuSd'
          displayName: 'Y:'
          values: [ad.yCoordinateSd]
          validations:
            min: 0
            max: 480
          hideUpdater: => # Hide this field when Platform selection is not Roku
            platformId = @state.fieldsMap.primaryFields.platform?.values[0]?.id || -1

            platformIsRoku = platformId == 25 # AdType 25  is Roku
            if (!platformIsRoku)
              @state.fieldsMap.coordinates.yCoordinateRokuSd?.values[0] = null

            return !platformIsRoku
        }

        yCoordinateRokuHd: {
          type: 'number'
          name: 'yCoordinateRokuHd'
          displayName: 'Y:'
          values: [ad.yCoordinateHd]
          validations:
            min: 0
            max: 720
          hideUpdater: => # Hide this field when Platform selection is not Roku
            platformId = @state.fieldsMap.primaryFields.platform?.values[0]?.id || -1

            platformIsRoku = platformId == 25 # AdType 25  is Roku
            if (!platformIsRoku)
              @state.fieldsMap.coordinates.yCoordinateRokuHd?.values[0] = null

            return !platformIsRoku
        }

        # Note that a platform that is not Roku will only have the user input HD X/Y coordinates and the resolution will be 1920x1080
        yCoordinateHtml5Hd: {
          type: 'number'
          name: 'yCoordinateHtml5Hd'
          displayName: 'Y:'
          values: [ad.yCoordinateHd]
          validations:
            min: 0
            max: 1080
          hideUpdater: => # Hide this field when Platform selection is not Roku
            platformId = @state.fieldsMap.primaryFields.platform?.values[0]?.id || -1

            platformIsRoku = platformId == 25 # AdType 25  is Roku
            if (platformIsRoku)
              @state.fieldsMap.coordinates.yCoordinateHtml5Hd?.values[0] = null

            return platformIsRoku
        }
    }

    coordinatesFieldMap.fields = [
      fieldsMap.xCoordinateRokuSd,
      fieldsMap.xCoordinateRokuHd,
      fieldsMap.xCoordinateHtml5Hd,
      fieldsMap.yCoordinateRokuSd,
      fieldsMap.yCoordinateRokuHd,
      fieldsMap.yCoordinateHtml5Hd
    ]

    return coordinatesFieldMap

  getTrackingEventFieldsMap: (trackingEvent) ->
    # need to get the ad trackingEvent item in order to get the trackingUrl,
    # since the trackingEvent item passed into this method only has formatted select option properties
    adTrackingEvents = _.filter(@state.ad.adTrackingEvents, (t) -> t.trackingEventId == trackingEvent.id) || []

    trackingEvent.fieldsMap = fieldsMap =
      trackingEvent:
          type: 'select'
          name: 'trackingEvent'
          displayName: 'Tracking Events'
          validations:
            required: true
          items: @state.trackingEvents
          values: _.filter(@state.trackingEvents, (t) -> t.id == trackingEvent.id)
          searchable: true

      trackingUrl:
          type: 'string'
          name: 'trackingUrl'
          displayName: 'Tracking Url'
          placeholder: 'Enter Tracking Url'
          validations:
            required: true
            maxLength: 1028
          values: [adTrackingEvents[0]?.trackingUrl || '']

      ad:
        id: @state.adId

      id:
        adTrackingEvents[0]?.id


    trackingEvent.fields = [
      fieldsMap.trackingEvent
      fieldsMap.trackingUrl
    ]

    _.each(trackingEvent.fields, (field, index) =>
      field.onUpdate = @onUpdate.bind(@, field)
      field.key = index
      field.classNameLeft = "col-xs-3"
      field.classNameRight = "col-xs-9"
    )

    return trackingEvent

module.exports = AdEdit = React.createClass
  displayName: 'AdEdit'

  mixins: [FieldsGeneralMixin, AdEditHelper]

  getInitialState: ->
    params = @props.params
    campaignId = parseInt(params.campaignId)

    return {
      campaignId: campaignId
      adId:       parseInt(params.adId || 0)
      loadedCampaignLayout: false
      loadedDeliveryGroup: false
      validationsInfo: FieldValidationHelper.getFieldsValidationsInfo([])
      buttonStates:
        create: 'Create Ad'
        update: 'Update Ad'
        saving: 'Saving...'
        validating: 'Validating...'
        uploading: 'Uploading files...'
        original: ''
        current: ''
      goBackUrl: "/campaigns/#{campaignId}/ads"
    }

  componentWillUnmount: ->
    this._isUnMounted = true

  componentDidMount: ->
    params = @props.params

    campaignId = @state.campaignId
    adId = @state.adId

    deferredApiLayout = RSVP.defer()
    cache.hash
      lookups:                    ['Campaigns.getLookups']
      summary:                    ['Campaigns.Summary', campaignId]
    .then (apiData) =>
      return if @_isUnMounted

      apiData.loadedCampaignLayout = true

      titlePrefix = if adId == 0 then 'Create' else 'Edit'
      utility.adjustTitle("#{titlePrefix} Ad - #{apiData.summary.name}")

      @setState apiData

      deferredApiLayout.resolve()
    .catch App.onError

    deferredApiDG = RSVP.defer()
    cache.hash
      'ads:ads':                ['Campaigns.Ads', campaignId]
      'placements:placements':  ['Campaigns.Placements', campaignId]
      creativesOriginal:        ['Campaigns.Creatives', campaignId]
      ad:                       ['Campaigns.Ad', adId]
      deliveryGroupsOriginal:   ['Campaigns.DeliveryGroups', campaignId]
    .then (apiData) =>
      apiData.placements = formatterSelect(apiData.placements)
      apiData.ad = _.cloneDeep(apiData.ad)
      apiData.ads = formatterSelect(apiData.ads)
      apiData.creatives = formatterSelect(apiData.creativesOriginal)
      _.extend(@state, apiData)

      deferredApiDG.resolve()
    .catch App.onError

    RSVP.all([deferredApiLayout.promise, deferredApiDG.promise])
    .then =>
      return if @_isUnMounted

      @state.loadedAd = true

      if adId == 0
        @state.title = "New Ad"
        @state.buttonStates.original = 'create'
      else
        @state.title = "Edit Ad"
        @state.buttonStates.original = 'update'
      @state.buttonStates.current = @state.buttonStates.original

      ad = @state.ad

      # Update the model with any specified settings found in the querystring.
      settingsExtendable = [
        'adType'
        'creative'
        'platform'
        'placement'
      ]
      for setting in settingsExtendable
        valStr = utility.getQueryValue(setting, true)
        if valStr
          # Initialize the object, if it doesn't exist (typically happens for a new ad)
          ad[setting] || ad[setting] = {}
          ad[setting].id = parseInt(valStr)

      # Determine if this campaign has creatives (a Creative is required for an Ad to be created)
      # Skip the rest of this initialization if there are no creatives.
      @state.campaignHasCreatives = _.keys(@state.creatives).length > 0
      if !@state.campaignHasCreatives
        @forceUpdate()
        return

      # Campaign does have creatives. Continue

      # Only get adTypes that are referenced by this campaign's creatives
      adTypes = _.filter(@state.lookups.adTypes, (a) =>
        return @getCreativesOfAdType(a.id).length > 0
      )

      @state.adTypes = formatterSelect(adTypes)

      @state.platforms = formatterSelect(@state.lookups.platforms)

      ad.adTag = ad.adTag || 0

      # Initialize date into a workable format (moment).
      # Default beginDate to today, since all ads require a beginDate
      ad.beginDateMoment = if ad.beginDate then utility.moment.allToMoment(ad.beginDate) else moment()
      ad.endDateMoment = if ad.endDate then utility.moment.allToMoment(ad.endDate) else null

      # For a new creative, initialize the isReported if adType is set to Overlay via QS params
      if typeof ad.isReported == 'undefined' && ad.adType
        if ad.adType.id == 10017
          ad.isReported = true

      # Set up Destination Ads
      creativesDestination = _.filter(@state.creativesOriginal, (creative) -> creative.adTypeId == 10010) # Ad Type 10010 is Brand Destination
      @state.creativesDestination = formatterSelect(creativesDestination)

      # Set up Companion Ads
      companionAds = _.filter(@state.ads, (ad) -> ad.adTypeId == 10017)   # Ad Type 10010 is Overlay
      @state.companionAds = formatterSelect(companionAds)

      # Set up DeliveryGroup
      @state.deliveryGroups = formatterSelect(@state.deliveryGroupsOriginal)

      @state.trackingEventsOriginial = _.clone(@state.lookups.trackingEvents)
      @state.trackingEvents = formatterSelect(@state.lookups.trackingEvents)
    #  trackingEventsCloned = _.filter(@state.trackingEvents, (trackingEvent) -> trackingEvent.id == adTrackingEvent.trackingEvent)

      @state.fieldsMap = fieldsMap = {
        primaryFields: @getPrimaryFieldsMap(ad, campaignId, @state)
        coordinates: @getCoordinatesFieldsMap(ad, campaignId, @state)
        trackingEvents: _.chain(ad.adTrackingEvents)
          .map((adTrackingEvent) =>
            return _.cloneDeep(_.filter(@state.trackingEvents, (trackingEvent) ->
              trackingEvent.id == adTrackingEvent.trackingEventId
            )[0])
          )
          .map(@getTrackingEventFieldsMap)
          .value()
      }

      @updateFieldsArray()

      @updateFields({initialize: true})
      do logger.log @state.fields, @state.fieldsMap

      @forceUpdate()
    .catch App.onError

  getCreativesOfAdType: (adTypeId) ->
    adTypeId = adTypeId || @state.fieldsMap.primaryFields.adType.values[0]?.id || -1

    return _.filter(@state.creatives, (c) -> return c.adTypeId == adTypeId)

  updateFieldsArray: ->
    @state.fields = _.flattenDeep([
      @state.fieldsMap.primaryFields.fields,
      @state.fieldsMap.coordinates.fields,
      _.map(@state.fieldsMap.trackingEvents, (trackingEvent) -> return trackingEvent.fields)
    ])

    @state.validationsInfo = FieldValidationHelper.getFieldsValidationsInfo(@state.fields)

  onTrackingEventAdd: (selections) ->
    # Create a new/fake tracking event instance(id = 0)
    trackingEvent = @getNewTrackingEvent()

    @state.fieldsMap.trackingEvents.push trackingEvent

    @updateFieldsArray()

    # Rerender
    @forceUpdate()

  getNewTrackingEvent: () ->
    # Create a new feature instance (id = 0), with the featureType
    trackingEvent = {
      id: 0
      trackingUrl: ''
      ad: {
        id: @state.adId
      }
      trackingEvent: {
        id: 0,
        name: ''
      }
      isDeleted: false
    }

    trackingEvent = @getTrackingEventFieldsMap(trackingEvent)
    return trackingEvent

  onTrackingEventRemove: (trackingEvent) ->
    # Find the current index of the feature
    trackingEventIndex = @state.fieldsMap.trackingEvents.indexOf(trackingEvent)

    # Remove it from the array
    @state.fieldsMap.trackingEvents.splice(trackingEventIndex, 1)

    @updateFieldsArray()

    # Rerender
    @forceUpdate()

  saveToServer: ->
    # Update, and disable save button
    @state.buttonStates.current = 'saving'
    @forceUpdate()

    params = @props.params
    fields = @state.fields
    primaryFieldsMap = @state.fieldsMap.primaryFields
    coordinatesFieldMap = @state.fieldsMap.coordinates
    trackingEventsFieldMap = @state.fieldsMap.trackingEvents

    xCoordinateSd = coordinatesFieldMap.xCoordinateRokuSd.values[0]
    yCoordinateSd = coordinatesFieldMap.yCoordinateRokuSd.values[0]
    xCoordinateRokuHd = coordinatesFieldMap.xCoordinateRokuHd?.values[0]
    yCoordinateRokuHd = coordinatesFieldMap.yCoordinateRokuHd?.values[0]
    xCoordinateHtml5Hd = coordinatesFieldMap.xCoordinateHtml5Hd?.values[0]
    yCoordinateHtml5Hd = coordinatesFieldMap.yCoordinateHtml5Hd?.values[0]

    saveData =
      id:             @state.adId
      campaignId:       parseInt(@props.params.campaignId)
      adTagId:          @state.ad.adTag

      adTypeId:         primaryFieldsMap.adType        .values[0]?.value || null
      creativeId:       primaryFieldsMap.creative      .values[0]?.value || null
      companionAdId:    primaryFieldsMap.companionAd   .values[0]?.value || null
      destinationCreativeId:  primaryFieldsMap.creativeDestination .values[0]?.value || null

      beginDate:      primaryFieldsMap.beginDate     .values?[0]?.format('MM/DD/YYYY') || ''
      endDate:        primaryFieldsMap.endDate       .values?[0]?.format('MM/DD/YYYY') || ''

      platformId:       primaryFieldsMap.platform      .values[0]?.value || null
      placementId:      primaryFieldsMap.placement     .values[0]?.value || null
      deliveryGroupId:  primaryFieldsMap.deliveryGroup .values[0]?.value || null
      name:           primaryFieldsMap.name          .values[0]
      isReported:     primaryFieldsMap.isReported    .values[0]

      xCoordinateSd:  xCoordinateSd
      xCoordinateHd:  if _.isNull(xCoordinateRokuHd) then xCoordinateHtml5Hd else xCoordinateRokuHd
      yCoordinateSd:  yCoordinateSd
      yCoordinateHd:  if _.isNull(yCoordinateRokuHd) then yCoordinateHtml5Hd else yCoordinateRokuHd

      adTrackingEvents: _.map(trackingEventsFieldMap, (trackingEvent) ->
        trackingEventSaveObj =
          id:           trackingEvent.fieldsMap.id
          adId:           trackingEvent.fieldsMap.ad.id
          trackingEventId:  trackingEvent.fieldsMap.trackingEvent.values[0].id
          trackingUrl:    trackingEvent.fieldsMap.trackingUrl.values[0]
          isDeleted:    false
      )

    do logger.log saveData

    self = @
    $.ajax({
      url: "/api/ads/save",
      type: "POST",
      data: JSON.stringify(saveData),
      contentType: "application/json"
    })
    .then (adId) ->
      self.saveToServerSuccess(adId)

  saveToServerSuccess: (savedAdId) ->
    do logger.log 'savedAdId', savedAdId

    # Add this item to HighlightUpdates list
    hu = new utility.HighlightUpdates
      key: 'Campaigns.Ads'
    hu.add("#campaign-ads .gridlist-item[data-ad-id='#{savedAdId}']")

    creativeDestinationId = @state.fieldsMap.primaryFields?.creativeDestination?.values[0]?.id

    # Clear cache
    cache.clear('Campaigns.Ad', @state.adId)
    cache.clear('Campaigns.Ad', savedAdId)
    cache.clear('Campaigns.Creatives.Destination', creativeDestinationId)

    # Redirect to Summary
    url = "/campaigns/#{@props.params.campaignId}/ads"
    page(url)

  getAdditionalPropsForCoordinateField: (field) ->
    additionalProps =
      onUpdate: @onUpdate.bind(@, field)

    return additionalProps

  renderCoordinatesFields: () ->
    coordinates = @state.fieldsMap.coordinates
    platformId = @state.fieldsMap.primaryFields.platform?.values[0]?.id || -1
    isPlatformRoku = platformId == 25 # AdType 25  is Roku

    return (
      <div className="fields-container clearfix ad-coordinates">
        <div className="col-xs-12 fieldview">
          <div className='fields-container'>
            <div className="form-group">
              <div className="col-xs-12">
                <div className="col-xs-2 control-label">
                  <div className='field-group-label'>
                    Coordinates:
                  </div>
                </div>
              </div>
            </div>

            <div className={cs({
                'col-xs-12 field-group-row field-group-row-1': true
                hide: if !isPlatformRoku then true else false
              })}>

              <label className="col-xs-2 control-label field-group-row-label">
                <span> SD: </span>
              </label>

              <div className='form-group clearfix col-xs-3 field-group-column field-group-column-1'>
                <Field {...coordinates.xCoordinateRokuSd}
                  {...@getAdditionalPropsForCoordinateField(coordinates.xCoordinateRokuSd)} />
              </div>
              <div className='form-group clearfix col-xs-3 field-group-column field-group-column-2'>
                <Field {...coordinates.yCoordinateRokuSd}
                  {...@getAdditionalPropsForCoordinateField(coordinates.yCoordinateRokuSd)} />
              </div>
            </div>

            <div className={cs({
                'col-xs-12 field-group-row field-group-row-2': true
                hide: if !isPlatformRoku then true else false
              })}>
              <label className="col-xs-2 control-label field-group-row-label">
                <span> HD: </span>
              </label>

              <div className='form-group clearfix col-xs-3 field-group-column field-group-column-1'>
                <Field {...coordinates.xCoordinateRokuHd}
                  {...@getAdditionalPropsForCoordinateField(coordinates.xCoordinateRokuHd)} />
              </div>
              <div className='form-group clearfix col-xs-3 field-group-column field-group-column-2'>
                <Field {...coordinates.yCoordinateRokuHd}
                  {...@getAdditionalPropsForCoordinateField(coordinates.yCoordinateRokuHd)} />
              </div>
            </div>

            <div className={cs({
                'col-xs-12 field-group-row field-group-row-1': true
                hide: if isPlatformRoku then true else false
              })}>
              <label className="col-xs-2 control-label field-group-row-label">
                <span>  </span>
              </label>

              <div className='form-group clearfix col-xs-3 field-group-column field-group-column-1'>
                <Field {...coordinates.xCoordinateHtml5Hd}
                  {...@getAdditionalPropsForCoordinateField(coordinates.xCoordinateHtml5Hd)} />
              </div>
              <div className='form-group clearfix col-xs-3 field-group-column field-group-column-2'>
                <Field {...coordinates.yCoordinateHtml5Hd}
                  {...@getAdditionalPropsForCoordinateField(coordinates.yCoordinateHtml5Hd)} />
              </div>
            </div>

          </div>
        </div>
      </div>

    )

  renderTrackingEventFields: () ->
    _.map(@state.fieldsMap.trackingEvents, (field, index) =>
      return (
        <div className="form-group field-group-row field-group-row-#{index}" key={index}>
          <div className="col-xs-12">
            <label className="col-xs-1 control-label field-group-row-label">
              <span>  </span>
            </label>

            <div className="col-xs-4 field-group-column tracking-event">
              <div className="form-group">
                <Field {...@props} {...field.fieldsMap.trackingEvent}/>
              </div>
            </div>
            <div className="col-xs-5 field-group-column tracking-url">
              <div className="form-group">
                <Field {...@props} {...field.fieldsMap.trackingUrl} />
              </div>
            </div>
            <div className="col-xs-1">
              <a className={cs({"remove-feature": true})}
                  onClick={@onTrackingEventRemove.bind(@, field)} title="Remove this tracking event"><i className="fa fa-times"/></a>
            </div>
          </div>
        </div>
      )
    )

  renderBody: ->
    props = @props
    campaignId = props.campaignId || props.params?.campaignId

    validationsInfo = @state.validationsInfo
    saveBtnDisabled = !validationsInfo.enableSave && (validationsInfo.validitiesErrors.length > 0 || @state.buttonStates.current != @state.buttonStates.original)

    return <Loader/> if !@state.ad

    return (
      <div className="row" id="deliverygroup-edit">
        <div className="widget stacked">
          <div className="widget-header">
            <i className="icon-tasks"></i>
            <h3>{@state.title}</h3>
          </div>
          <div className="widget-content">
            {
              if @state.campaignHasCreatives
                <div className="form-horizontal col-md-12">
                  {_.map(@state.fieldsMap.primaryFields.fields, (field, index) =>
                    additionalProps =
                      onUpdate: @onUpdate.bind(@, field)

                    return (
                      <div className="fields-container clearfix" key={index}>
                        <Field {...@props} {...field} {...additionalProps} />
                      </div>
                    )
                  )}

                  {@renderCoordinatesFields()}

                  <div className="fields-container clearfix section">
                    <div className="col-xs-12 fieldview">
                      <div className="form-group">
                        <div className="col-xs-12">
                          <div className="col-xs-2 control-label">
                            <div className='field-group-label'>
                              Tracking Events:
                            </div>
                          </div>

                          <div className="col-xs-8">
                            <div className="add-tracking-event-btn">
                              <button data-nw="add-tracking-event-button" className="btn btn-success" onClick={@onTrackingEventAdd}>Add Tracking Event</button>
                            </div>
                          </div>

                        </div>
                      </div>
                    </div>

                    <div className="col-xs-12 tracking-events">
                      {@renderTrackingEventFields()}
                    </div>
                  </div>

                  <div className="form-group">
                    <div className="form-field col-sm-6 col-sm-offset-2">
                      <div id="notifier"></div>
                    </div>
                  </div>

                  <div className="col-xs-12">
                    {@renderValidationSummary()}
                  </div>

                  <div className="col-md-12">
                    <div className="form-group">
                      <div className="col-sm-offset-2 col-sm-8">
                        <input type="button" className={cs({
                            "btn btn-success save": true
                            disabled: saveBtnDisabled
                          })}
                          value={@state.buttonStates[@state.buttonStates.current]}
                          onClick={@onSave}
                          />
                        <a className="btn-cancel" href={@state.goBackUrl}>Go back</a>
                      </div>
                    </div>
                  </div>
                </div>
              else
                <AdEditMissingCreatives campaignId={@props.params.campaignId} />
            }
          </div>
        </div>
      </div>
    )


  render: ->
    return <Loader/> if !@state.loadedCampaignLayout

    summary = @state.summary
    lookups = @state.lookups

    return (
      <CampaignLayout summary={summary} lookups={lookups} navCurrent="ads">
        {@renderBody()}
      </CampaignLayout>
    )
