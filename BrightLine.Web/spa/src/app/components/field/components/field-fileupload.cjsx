FieldHelper             = require 'components/field/field-helper'
FieldValidationHelper   = require 'components/field/field-validation-helper'
Info                    = require 'components/info/info'

module.exports = FileUpload = React.createClass
  displayName: 'FileUpload'

  propTypes:
    someProp: React.PropTypes.object

  mixins: [React.addons.PureRenderMixin]

  getInitialState: ->
    return {
      viewState: if @props.values[0].resource then 'existing' else 'unexisting'
      fileName: 'Select a file...'
    }


  goToViewState: (viewState) ->
    @setState
      viewState: viewState


  onFileSelect: (e) ->
    field = e.currentTarget
    file = field.files[0]
    @onChange(file, true)


  onChange: (file, rerender) ->
    @state.fileName = file?.name || 'Select a file...'

    # Maintain the resource object. Only update the file Object.
    # So, newValue should look like: {resource: {}, file: {}}
    newValue = @props.values[0]
    newValue.file = file # This could be null, if the user hits cancel.

    @props.onChange([newValue])

    if file
      # File exists. The fields view also needs to know other file properties for registration.
      if @props.type == 'image'
        # Make sure file is indeed an image
        FieldValidationHelper.validators.validateFileUploadExtension('', {file: file}, ['png', 'jpg', 'gif'])
        .then (validity) ->
          return if !validity.valid

          # File is an image. Get its dimensions
          return FieldHelper.misc.resource.image.getDimensions(file)
        .then (dimensions) =>
          newValue.dimensions = dimensions
          @props.onChange([newValue]) if @props.onChange
        .catch App.onError
      else if @props.type == 'video'
        # Make sure file is indeed a video
        FieldValidationHelper.validators.validateFileUploadExtension('', {file: file}, ['mp4'])
        .then (validity) ->
          return if !validity.valid

          # File is a video. Get its dimensions
          return FieldHelper.misc.resource.video.getMetadata(file)
        .then (metadata) =>
          _.extend(newValue, metadata)
          @props.onChange([newValue]) if @props.onChange
        .catch App.onError

    @forceUpdate() if rerender


  onCancel: ->
    # User selected something, but changed their mind. Don't consider the current file(ie, reset field)
    @onChange(null, false)

    @goToViewState('existing')


  renderFileInfo: ->
    dimensions = @props.values?[0]?.dimensions
    return null if !dimensions

    infoProps =
      title: "Dimensions"
      description: "#{dimensions.width}px X #{dimensions.height}px"
      classNames: "image-dimensions"
    return <Info {...infoProps} />


  renderResource: (resource) ->
    if !resource
      return null

    resourceSrc = utility.resource.getSrc(resource)

    if this.props.type == 'image'
      return (
        <a className="resource-image" href={resource.url} target="_blank">
          <div className="resource-image-name">{resource.filename}</div>
          <div className="resource-image-content">
            <img src={resourceSrc} alt={resource.filename}/>
          </div>
        </a>
      )

    if this.props.type == 'video'
      return (
        <a className="resource-video" href={resource.url} target="_blank">
          <div className="resource-video-name">{resource.filename}</div>
        </a>
      )


  render: ->
    resource = @props.values[0].resource
    if resource
      resourceSrc = utility.resource.getSrc(resource)

    return (
      <div className="file-upload-container">
        <div className="file-upload-state" data-state={@state.viewState}>
          <div className="existing-resource-precontainer">
            <div className="existing-resource-container">
              {@renderResource(resource)}
            </div>
            <a className={cs({"select-new": true, hide: this.props.readOnly})}
              onClick={@goToViewState.bind(null, 'unexisting')}>Select New...</a>
          </div>
          <div className={cs({"unexisting-resource-precontainer": true, hide: this.props.readOnly})}>
            <div className="row">
              <div className="col-sm-8">
                <div className="form-control">
                  <input type="file" key="file" name={@props.fieldName} className="resource-file" ref="resourceField" onChange={@onFileSelect}/>
                  <div id="resource-file-name" className="resource-file-name ellipsis" ref="fileName">
                    {@state.fileName}
                  </div>
                </div>
              </div>
              <div className="col-sm-4  progress-bar-container">
                {@renderFileInfo()}
              </div>
            </div>
            {(<a className="cancel" onClick={@onCancel}>Cancel</a>) if resource}
          </div>
        </div>

      </div>
    )
