Info = require 'components/info/info'

module.exports = FieldValidationRequirements = React.createClass
  displayName: 'FieldValidationRequirements'
  
  propTypes: 
    displayName: React.PropTypes.string.isRequired
    validations: React.PropTypes.object
  
  mixins: [React.addons.PureRenderMixin]

  render: ->
    validations = @props.validations

    return <div/> if !validations

    # Clone to not pollute the original validations config
    validations = _.cloneDeep(validations)

    # Try merging validation pairs into a single message.
    if validations.width && validations.height
      validations.widthHeight = "#{validations.width}px by #{validations.height}px"
      delete validations.width
      delete validations.height

    if validations.minWidth && validations.minHeight
      validations.minWidthMinHeight = "#{validations.minWidth}px by #{validations.minHeight}px"
      delete validations.minWidth
      delete validations.minHeight

    if validations.maxWidth && validations.maxHeight
      validations.maxWidthMaxHeight = "#{validations.maxWidth}px by #{validations.maxHeight}px"
      delete validations.maxWidth
      delete validations.maxHeight

    # Set up requirements messages' format for each validation
    fieldName = @props.displayName
    getValidationReqMsg = (validation) ->
      # if validation.name == 'required'
      #   return "#{fieldName} is required"
      if validation.name == 'width'
        return "#{fieldName} must have a width of #{validation.config}px"
      if validation.name == 'height'
        return "#{fieldName} must have a height of #{validation.config}px"
      if validation.name == 'widthHeight'
        return "#{fieldName} must have a resolution of #{validation.config}"
      if validation.name == 'minWidthMinHeight'
        return "#{fieldName} must have a resolution of at least #{validation.config}"
      if validation.name == 'maxWidthMaxHeight'
        return "#{fieldName} must have a resolution of at most #{validation.config}"

    validationReqs = _.chain(validations)
      .map((vConfig, vName) ->
        return { 
          name: vName
          config: vConfig
        }
      )

      # Show Validation requirements for only certain validations
      .filter((v)->
        return ['width', 'height', 'widthHeight', 
        'minWidthMinHeight', 'maxWidthMaxHeight'].indexOf(v.name) >= 0
      )

      # Map these validations with messages that are in appropriate format.
      .map((v) ->
        return {
          title: ''
          description: getValidationReqMsg(v)
        }
      )

      .value()

    return <div/> if validationReqs.length == 0

    return (
      <div className="validation-requirements bg-warning">
        {_.map(validationReqs, (v, i) -> <Info {...v} key={i} />)}
      </div>
    )
        
