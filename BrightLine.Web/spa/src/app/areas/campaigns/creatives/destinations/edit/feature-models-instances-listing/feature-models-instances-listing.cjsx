cache = (require 'services/index').cache
Loader = require 'components/loader/loader'
Info = require 'components/info/info'

module.exports = FeatureModelsInstancesListing = React.createClass
  displayName: 'FeatureModelsInstancesListing'
  
  propTypes: 
    campaignId:         React.PropTypes.number
    creativeId:         React.PropTypes.number
    featureId:          React.PropTypes.number
    featureModels:      React.PropTypes.object
    featureSetting:    React.PropTypes.object
  
  mixins: [React.addons.PureRenderMixin]

  getCookie: ->
    # Use 1 cookie for saving the view mode of all the features in a campaign
    persistantSettings = new utility.Persist key: "Campaigns.#{@props.campaignId}.ModelsInstancesListing", expires: 365
    return persistantSettings


  getInitialState: ->
    viewModes =
      models: 'Models'
      instances: 'Instances'

    persistantSettings = @getCookie()

    # Create the cookie if it doesn't exist
    persistantSettings.set({}) if !persistantSettings.get()

    # Set up default settings for this featureId
    featureId = @props.featureId
    persistantSettingsObj = persistantSettings.get()
    if !persistantSettingsObj[featureId]
      persistantSettingsObj[featureId] = {viewMode: viewModes.models}
      persistantSettings.set(persistantSettingsObj)

    return {persistantSettings, viewModes}


  onGoToFeatureModelInstancesListing: (model) ->
    @setFeatureSetting({viewMode: @state.viewModes.instances, modelId: model.id})

    @forceUpdate()


  onGoToFeatureModelsListing: ->
    @setFeatureSetting({viewMode: @state.viewModes.models})

    @forceUpdate()


  setFeatureSetting: (setting) ->
    persistantSettings = @getCookie()
    persistantSettingsObj = persistantSettings.get()
    persistantSettingsObj[@props.featureId] = setting
    persistantSettings.set(persistantSettingsObj)


  render: ->
    featureViewSetting = @state.persistantSettings.get()[@props.featureId]

    if featureViewSetting.viewMode == @state.viewModes.models
      Component = FeatureModelsListing
      componentProps = _.extend({}, @props, {onGoToFeatureModelInstancesListing: @onGoToFeatureModelInstancesListing})
    else
      Component = FeatureModelInstancesListing
      modelId = featureViewSetting.modelId
      model = @props.featureModels[modelId]
      componentProps = _.extend({
          model: model
        }, @props, {onGoToFeatureModelsListing: @onGoToFeatureModelsListing})

    return (
      <div className="feature-models-instances-listing-container">
        <Component {...componentProps}/>
      </div>
    )
    

FeatureModelsListing = React.createClass
  displayName: 'FeatureModelsListing'
  
  propTypes: 
    someProp: React.PropTypes.object
  
  mixins: [React.addons.PureRenderMixin]

  onGoToFeatureModelInstancesListing: (model) ->
    @props.onGoToFeatureModelInstancesListing(model)

  render: ->
    featureSetting = @props.featureSetting
    featureSettingsEditUrl = "/campaigns/#{@props.campaignId}/creatives/#{@props.creativeId}/features/#{@props.featureId}/settings/#{featureSetting.id}/instances/#{featureSetting.settingInstanceId}/edit"

    return (
      <div className="feature-models-listing-container">
        <div className="widget stacked">
          <div className="widget-header">
            <h3>Models</h3>
            <a style={float: 'right'}
              className="instance-create" 
              href={featureSettingsEditUrl}>
              <i className="fa fa-cog"></i> Edit Settings
            </a>
          </div>
          <div className="widget-content feature-models-listing">
            <div>
              {_.map(@props.featureModels, (model, index) =>
                modelLinkAttrs = 
                  className: 'ellipsis'
                  onClick: @onGoToFeatureModelInstancesListing.bind(this, model)
                  "data-nw-modeldefinitionid": model.modelDefinitionId

                <div className={"row feature-models-listing-item #{if index % 2 == 0 then 'even' else 'odd'}"} key={index}>
                  <div className="col-xs-12">
                    <a {...modelLinkAttrs}>{model.display}</a>
                  </div>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    )

FeatureModelInstancesListing = React.createClass
  displayName: 'FeatureModelInstancesListing'
  
  propTypes: 
    someProp: React.PropTypes.object
  
  mixins: [React.addons.PureRenderMixin]

  getInitialState: ->
    return {cmsModelInstances: null}


  componentWillMount: ->
    cache.hash
      cmsModelInstances: ['Campaigns.CmsModelInstances', @props.model.id]
    .then (apiData) =>
      @setState apiData
    .catch App.onError


  renderInstances: ->
    model = @props.model
    cmsModelInstances = @state.cmsModelInstances
    return <Loader/> if !cmsModelInstances

    cmsModelInstancesArr = _.map(cmsModelInstances)
    if cmsModelInstancesArr.length == 0
      return <Info 
        classNames="collection_empty" 
        title={"There are no #{model.displayNamePlural}"}
        description="You can create a new #{model.displayNamePlural} by clicking the 'New #{model.display}' link above." />

    return (
      <div>
        {_.map(cmsModelInstancesArr, (modelInstance, index) =>
            modelInstanceEditUrl = "/campaigns/#{@props.campaignId}/creatives/#{@props.creativeId}/features/#{@props.featureId}/models/#{model.id}/instances/#{modelInstance.id}/edit"
            
            return (
              
                <div className={"row feature-model-instance-listing-item #{if index % 2 == 0 then 'even' else 'odd'}"} key={index}>
                  <div className="col-xs-12">
                    <a className="ellipsis" href={modelInstanceEditUrl}>{modelInstance.name}</a>
                  </div>
                </div>
              
            )
          )
        }
      </div>
    )


  render: ->
    onGoToFeatureModelsListing = @props.onGoToFeatureModelsListing
    model = @props.model

    modelCreateUrl = "/campaigns/#{@props.campaignId}/creatives/#{@props.creativeId}/features/#{@props.featureId}/models/#{model.id}/instances/create"

    return (
      <div className="feature-model-instances-listing-container">
        <div className="widget stacked">
          <div className="widget-header">
            <h3><a onClick={@props.onGoToFeatureModelsListing}>Models</a> > {model.displayNamePlural}</h3>
            <a style={float: 'right'}
              className="instance-create" 
              href={modelCreateUrl}>
              <i className="fa fa-plus"></i> New {model.display}
            </a>
          </div>
          <div className="widget-content feature-model-instances-listing">
            {@renderInstances()}
          </div>
        </div>
      </div>
    )
    
