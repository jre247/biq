Info                          = require 'components/info/info'
FieldValidationRequirements   = require './field-validation-requirements'
FieldValidationErrors         = require './field-validation-errors'
FieldValidationHelper         = require './field-validation-helper'
FieldText                     = require './components/field-text'
FieldDatepicker               = require './components/field-datepicker'
FieldFileUpload               = require './components/field-fileupload'
FieldSelect                   = require './components/field-select'
FieldCheckbox                 = require './components/field-checkbox'
FieldRef                      = require './components/field-ref'

FieldHelper           = require 'components/field/field-helper'
formatters            = FieldHelper.formatters
formatterNumberPre    = formatters.number.general.formatterPre
formatterNumberPost   = formatters.number.general.formatterPost

module.exports = Field = React.createClass
  displayName: 'Field'

  propTypes:
    # Commonly used by every field
    name: React.PropTypes.string.isRequired
    displayName: React.PropTypes.string.isRequired
    description: React.PropTypes.string
    type: React.PropTypes.string.isRequired
    values: React.PropTypes.array

    # Used to determine whether to show the readOnly Value
    readOnly: React.PropTypes.bool

    # Used to determine whether to show the entire field. Can be bool or function
    hide: React.PropTypes.any

    # Used by selects (text/image based)
    options: React.PropTypes.object
    items: React.PropTypes.array
    isList: React.PropTypes.bool
    isMultiple: React.PropTypes.bool

    # Used by resources
    onFileSelect: React.PropTypes.func
    resourceType: React.PropTypes.number

    validations: React.PropTypes.object

    onUpdate: React.PropTypes.func

  mixins: [React.addons.PureRenderMixin]


  getDefaultProps: ->
    return {
      readOnly: false
      classNameLeft: "col-xs-2"
      classNameRight: "col-xs-8"
      validations: {}
    }


  getInitialState: ->
    return {
      isValid: true
    }


  onChange: (values) ->
    ###
    # Receives updates from sub components on user updates.
    # Sends the update back to the parent view immediately.
    # Then, it runs validations, and sends that back to the parent view
    ###
    onUpdate = @props.onUpdate
    # fieldIndex = @props.index

    if !onUpdate
      return

    # Send the updates up to the parent immediately.
    onUpdate({values: values})


  renderFieldComponent: ->
    fieldType = @props.type

    fieldTypeToFieldComponentMap =
      string:       @renderFieldText
      number:       @renderFieldInteger
      integer:      @renderFieldInteger
      float:        @renderFieldInteger
      select:       @renderSelect
      bool:         @renderCheckbox
      datetime:     @renderDatepicker
      image:        @renderFileUpload
      video:        @renderFileUpload
      refToModel:   @renderFieldRef
      refToPage:    @renderFieldRef


    return fieldTypeToFieldComponentMap[fieldType]()


  renderFieldRef: ->
    return <FieldRef {...@props} onChange={@onChange} ref="subField"/>


  renderFieldText: ->
    return <FieldText {...@props} onChange={@onChange} ref="subField"/>


  renderFieldInteger: ->
    props =
      formatterPre:   @props.formatterNumberPre || formatterNumberPre
      formatterPost:  @props.formatterNumberPost || formatterNumberPost
      onChange:       @onChange

    return <FieldText {...@props} {...props} ref="subField"/>


  renderCheckbox: ->
    return <FieldCheckbox {...@props} onChange={@onChange} ref="subField" />


  renderSelect: ->
    return <FieldSelect {...@props} onChange={@onChange} ref="subField"/>


  renderDatepicker: ->
    return <FieldDatepicker {...@props} onChange={@onChange} ref="subField" />


  renderFileUpload: ->
    ###
    FileUpload requires validation to occur in a different way than other fields.
    It can have a resource associated with it, along with a file(selected by the user)
    Thus, its value should contain both objects: the resource, and the file.
    This allows required validation to be skipped, if a resource exists, and file doesn't.
    However, if a file does exist, then extension validators would trigger
    ###

    # Initialize value by including resource (if it exists)
    values = @props.values
    if !values || values && values.length == 0
      values = [{resource: null}]
    fileUploadProps = _.extend({}, @props,
      onChange: @onChange
      ref: 'subField'
      values: values
    )
    return <FieldFileUpload {...fileUploadProps} />


  render: ->

    props = @props
    state = @state

    validitiesErrors = _.filter(@props.validities, (v) -> v && !v.valid)

    fieldContainerDataNW = _.extend({}, props['dataNW'], {
      "data-nw-fieldname": props.name
    })

    return (
      <form className={cs({
          "col-xs-12 fieldview": true
          readonly: @props.readOnly
          hide: if _.isFunction(@props.hide) then @props.hide() else @props.hide

        })} >
        <div className={cs({'form-group clearfix': true, required: props.validations.required})}>
          <label className={"#{@props.classNameLeft} control-label"}>
            {props.displayName}
            <i className={cs({"glyphicon glyphicon-info-sign": true, hide: !!!props.description})} data-toggle="tooltip" data-placement="top" title={props.description || ''}></i>
          </label>
          <div className={@props.classNameRight}>

            <div className={cs({
                "field-container": true,
                error: validitiesErrors.length>0,
                "field-type-#{props.type}": true,
                "field-readonly": props.readOnly
              })}
              {...fieldContainerDataNW}>
              {@renderFieldComponent()}
            </div>

            <div className="validation-container">
              <FieldValidationErrors validitiesErrors={validitiesErrors} fieldIndex={@props.index} />
            </div>

            <div className="validation-requirements-container">
              <FieldValidationRequirements {...props} />
            </div>

            <div>
              {<@props.footer {...@props}/> if @props.footer}
            </div>
          </div>
        </div>
        {<@props.footerOuter {...@props}/> if @props.footerOuter}
      </form>
    )
