ValidationSummary     = require 'components/field/validation-summary'
Field                 = require 'components/field/field'
FieldList             = require 'components/field/field-list'

module.exports = FieldsRenderMixin =
  renderValidationSummary: ->
    return (
      <div className="form-group validation-summary-container">
        <div className="form-field col-xs-6 col-xs-offset-2 validation-summary">
           {<ValidationSummary errors={@state.validationsInfo.validitiesErrorsMsgs}/>}
        </div>
      </div>
    )

  renderFields: ->
    propsData =
      instance: @
      onUpdate: @onUpdate
      fields: @state.fields

    return (
      <FieldList {...propsData} />
    )


  renderForm: ->
    props = @props
    campaignId = props.campaignId || props.params?.campaignId

    validationsInfo = @state.validationsInfo
    saveBtnDisabled = !validationsInfo.enableSave && (validationsInfo.validitiesErrors.length > 0 || @state.buttonStates.current != @state.buttonStates.original)

    return (
      <div className="form-horizontal col-md-12">

        {@renderFields()}

        <div className="form-group">
          <div className="form-field col-sm-6 col-sm-offset-2">
            <div id="notifier"></div>
          </div>
        </div>

        <div className="col-md-12">
          <div className="form-group validation-summary-container">
            <div className="form-field col-sm-6 col-sm-offset-2 validation-summary">
               <ValidationSummary errors={validationsInfo.validitiesErrorsMsgs}/>
            </div>
          </div>
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
    )
