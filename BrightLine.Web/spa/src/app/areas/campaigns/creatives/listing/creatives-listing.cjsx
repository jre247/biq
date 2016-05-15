cache = (require 'services/index').cache
Loader = require 'components/loader/loader'

CreativesListingPromotional = require './creatives-listing-promotional'
CreativesListingDestination = require './creatives-listing-destination'

CampaignLayout              = require 'areas/campaigns/shared/campaign-layout'

module.exports = class CreativesListing extends React.Component
  constructor: ->
    super

    params = @props.params

    campaignId = params.campaignId

    @state =
      campaignId: campaignId
      loadedCampaignLayout: false
      loadedCreatives: false
      filterSearchTerm: ''

    cache.hash
      lookups:                    ['Campaigns.getLookups']
      summary:                    ['Campaigns.Summary', campaignId]
    .then (apiData) =>
      return if @_isUnMounted

      apiData.loadedCampaignLayout = true

      utility.adjustTitle("Creatives - #{apiData.summary.name}")

      @setState apiData
    .catch App.onError

    cache.hash
      'promotionals:promotional': ['Campaigns.Creatives.Promotionals', campaignId]
      'destinations:destination': ['Campaigns.Creatives.Destinations', campaignId]
      'features:features':        ['Campaigns.Features', campaignId]

    .then (apiData) =>
      return if @_isUnMounted
      
      apiData.loadedCreatives = true
      @setState apiData
    .catch App.onError


  componentWillUnmount: ->
    this._isUnMounted = true


  renderCreatives: ->
    return <Loader/> if !@state.loadedCreatives

    campaignSummary = @state.summary
    campaignId = campaignSummary.id
    lookups = @state.lookups
    features = @state.features

    creativesPromotionalActive = _.filter(@state.promotionals, (c) -> return !c.isDeleted)
    creativesDestinationActive = _.chain(@state.destinations)
      .filter((c) -> return !c.isDeleted)
      .cloneDeep()
      .map((creative) ->
        creativefeatureNames = []

        for featureId in creative.features || {}
          feature = features[featureId.toString()]

          if feature && !feature.isDeleted
            creativefeatureNames.push feature.name

        creative.featureNames = creativefeatureNames
        return creative
      )
      .value()

    setTimeout =>
      # Highlight any items in childviews, which may have been updated since last time.
      hu = new utility.HighlightUpdates
        key: 'Campaigns.Creatives'
      hu.highlight($(ReactDOM.findDOMNode(@)))
    , 0

    return (
      <div id="campaign-creatives">
        <CreativesListingPromotional items={creativesPromotionalActive} campaignId={campaignId}/>
        <CreativesListingDestination items={creativesDestinationActive} campaignId={campaignId}/>
      </div>
    )


  render: ->

    return <Loader/> if !@state.loadedCampaignLayout
    
    summary = @state.summary
    lookups = @state.lookups

    return (
      <CampaignLayout summary={summary} lookups={lookups} navCurrent='creatives'>
        {@renderCreatives()}
      </CampaignLayout>
    )






