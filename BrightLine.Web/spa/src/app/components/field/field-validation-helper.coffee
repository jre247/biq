class BoolMessage
  constructor: (options) ->
    @options = options

    for propKey, propVal of options
      @[propKey] = propVal

    # @msg = @msg || ''


getBoolMessage = (valid, messageInvalid, messageValid) ->
  messageInvalid || messageInvalid = ''
  messageValid || messageValid = ''

  if valid
    return new BoolMessage
      valid: true
      type: 'success'
  else
    return new BoolMessage
      valid: false
      type: 'error'
      msg: messageInvalid


validators =
  validateMinLength: (fieldName, fieldValue, lengthMin) ->
    if !fieldValue
      return

    valid = lengthMin <= fieldValue.length
    return utility.promise.resolved getBoolMessage(valid, "#{fieldName} cannot be lesser than #{lengthMin} characters")

  validateMaxLength: (fieldName, fieldValue, lengthMax) ->
    if !fieldValue
      return

    valid = fieldValue.length <= lengthMax
    return utility.promise.resolved getBoolMessage(valid, "#{fieldName} cannot be longer than #{lengthMax} characters")

  validateMin: (fieldName, fieldValue, min) ->
    if !fieldValue
      return

    if typeof fieldValue == 'string'
      fieldValue = parseInt(fieldValue)
    valid = min <= fieldValue
    return utility.promise.resolved getBoolMessage(valid, "#{fieldName} cannot be lesser than #{min}")

  validateMax: (fieldName, fieldValue, max) ->
    if !fieldValue
      return

    if typeof fieldValue == 'string'
      fieldValue = parseInt(fieldValue)
    valid = fieldValue <= max
    return utility.promise.resolved getBoolMessage(valid, "#{fieldName} cannot be greater than #{max}")

  validateRequired: (fieldName, fieldValue) ->
    if typeof fieldValue == 'string'
      fieldValue = fieldValue.trim()

    valid = fieldValue && fieldValue != null
    return utility.promise.resolved getBoolMessage(valid, "#{fieldName} is required")

  validateFileUploadExtension: (fieldName, fieldValue, extensions) ->
    extensions = _.filter(extensions)
    if extensions.length == 0
      return

    if !fieldValue || !fieldValue.file
      return

    file = fieldValue.file

    return if !file

    fileName = file.name
    fileNameSplitOnDots = fileName.split('.')
    fileExtension = fileNameSplitOnDots[fileNameSplitOnDots.length - 1]

    if !fileExtension
      return

    fileExtension = fileExtension.toLowerCase()
    valid = extensions.indexOf(fileExtension) != -1
    return utility.promise.resolved getBoolMessage(valid, "#{fieldName} cannot be of type #{fileExtension}. Supported extensions are #{extensions.join(', ')}")


  validateFileUploadWidth: (fieldName, fieldValue, width) ->
    if !fieldValue || !fieldValue.file
      return

    deferred = RSVP.defer()
    img = new Image()
    img.onload = ->
      fileWidth = this.width
      valid = fileWidth == width
      deferred.resolve getBoolMessage(valid, "#{fieldName} needs a width of #{width}px")
    img.src = window.URL.createObjectURL(fieldValue.file)
    return deferred.promise

  validateFileUploadMinWidth: (fieldName, fieldValue, minWidth) ->
    if !fieldValue || !fieldValue.file
      return

    deferred = RSVP.defer()
    img = new Image()
    img.onload = ->
      fileWidth = this.width
      valid = minWidth <= fileWidth
      deferred.resolve getBoolMessage(valid, "#{fieldName} needs a minimum width of #{minWidth}px")
    img.src = window.URL.createObjectURL(fieldValue.file)
    return deferred.promise

  validateFileUploadMaxWidth: (fieldName, fieldValue, maxWidth) ->
    if !fieldValue || !fieldValue.file
      return

    deferred = RSVP.defer()
    img = new Image()
    img.onload = ->
      fileWidth = this.width
      valid = fileWidth <= maxWidth
      deferred.resolve getBoolMessage(valid, "#{fieldName} needs a maximum width of #{maxWidth}px")
    img.src = window.URL.createObjectURL(fieldValue.file)
    return deferred.promise


  validateFileUploadHeight: (fieldName, fieldValue, height) ->
    if !fieldValue || !fieldValue.file
      return

    deferred = RSVP.defer()
    img = new Image()
    img.onload = ->
      fileHeight = this.height
      valid = fileHeight == height
      deferred.resolve getBoolMessage(valid, "#{fieldName} needs a height of #{height}px")
    img.src = window.URL.createObjectURL(fieldValue.file)
    return deferred.promise


  validateFileUploadMinHeight: (fieldName, fieldValue, minHeight) ->
    if !fieldValue || !fieldValue.file
      return

    deferred = RSVP.defer()
    img = new Image()
    img.onload = ->
      fileHeight = this.height
      valid = minHeight <= fileHeight
      deferred.resolve getBoolMessage(valid, "#{fieldName} needs a minimum height of #{minHeight}px")
    img.src = window.URL.createObjectURL(fieldValue.file)
    return deferred.promise


  validateFileUploadMaxHeight: (fieldName, fieldValue, maxHeight) ->
    if !fieldValue || !fieldValue.file
      return

    deferred = RSVP.defer()
    img = new Image()
    img.onload = ->
      fileHeight = this.height
      valid = fileHeight <= maxHeight
      deferred.resolve getBoolMessage(valid, "#{fieldName} needs a maximum height of #{maxHeight}px")
    img.src = window.URL.createObjectURL(fieldValue.file)
    return deferred.promise


  validateFileUploadRequired: (fieldName, fieldValue) ->
    # Expect fieldValue to be a hash, containing resource, or file

    # First, transform the value array object into true/undefined.
    fieldValueTransformed = fieldValue?.resource || fieldValue?.file

    # Next, funnel the request into the Required validator
    return validators.validateRequired(fieldName, fieldValueTransformed)


  validateFileSizeMax: (fieldName, fieldValue, maxSizeInBytes) ->
    if !fieldValue || !fieldValue.file
      return

    valid = fieldValue.file.size <= maxSizeInBytes
    maxSizeInKb = Math.floor(maxSizeInBytes/1024)
    return utility.promise.resolved getBoolMessage(valid, "#{fieldName} must be less than #{maxSizeInKb}kb")


  validateSelectionRequired: (fieldName, fieldValue) ->
    # Expect fieldValue to be an array of hash objects

    # First, transform the value array object into true/undefined.
    fieldValueTransformed = null

    if fieldValue.length > 0 && fieldValue.indexOf('') == -1
      fieldValueTransformed = true
    else
      fieldValueTransformed = null

    # Next, funnel the request into the Required validator
    return validators.validateRequired(fieldName, fieldValueTransformed)



  validateSelectionMin: (fieldName, fieldValue, min) ->
    # Expect fieldValue to be an array of hash objects

    valid = fieldValue.length >= min
    return utility.promise.resolved getBoolMessage(valid, "#{fieldName} must have at least #{min} selections")


  validateSelectionMax: (fieldName, fieldValue, max) ->
    # Expect fieldValue to be an array of hash objects

    valid = fieldValue.length <= max
    return utility.promise.resolved getBoolMessage(valid, "#{fieldName} must have at most #{max} selections")


  validateUnique: (fieldName, fieldValue, configFn) ->
    fieldValue = fieldValue?.trim() || ''
    config = configFn()
    return config?.uniquePromise
    .then (isDuplicate) ->
      errorMessage = "#{config.entity} with the name '#{fieldValue}' already exists. Please enter an unique name."
      return getBoolMessage(!isDuplicate, errorMessage)


module.exports =
  validators: validators
  getFieldValidators: (fieldType) ->
    fieldTypeToValidatorsMap =
      string:
        required: validators.validateRequired
        unique: validators.validateUnique
        minLength: validators.validateMinLength
        maxLength: validators.validateMaxLength

      number:
        required: validators.validateRequired
        min: validators.validateMin
        max: validators.validateMax

      integer:
        required: validators.validateRequired
        min: validators.validateMin
        max: validators.validateMax

      float:
        required: validators.validateRequired
        min: validators.validateMin
        max: validators.validateMax

      bool:
        required: validators.validateRequired

      datetime:
        required: validators.validateRequired

      image:
        required:   validators.validateFileUploadRequired
        extension:  validators.validateFileUploadExtension
        width:      validators.validateFileUploadWidth
        minWidth:   validators.validateFileUploadMinWidth
        maxWidth:   validators.validateFileUploadMaxWidth
        height:     validators.validateFileUploadHeight
        minHeight:  validators.validateFileUploadMinHeight
        maxHeight:  validators.validateFileUploadMaxHeight

      video:
        required:   validators.validateFileUploadRequired
        extension:  validators.validateFileUploadExtension

      refToModel:
        required: validators.validateSelectionRequired
        minLength: validators.validateSelectionMin
        maxLength: validators.validateSelectionMax

      refToPage:
        required: validators.validateRequired

      select:
        required: validators.validateSelectionRequired

    fieldValidators = fieldTypeToValidatorsMap[fieldType]

    return fieldValidators


  prepareValueForValidation: (fieldType, fieldValues) ->

    fieldsValueIsArray = ['datetime', 'refToModel', 'refToPage', 'select']

    if fieldsValueIsArray.indexOf(fieldType) >= 0
      return fieldValues
    else
      if fieldValues
        return fieldValues[0] || null
      else
        return null


  getFieldsValidationsInfo: (fields) ->
    validitiesArr = _.chain(fields)
      .filter((f) -> f.validities && !f.hide)
      .map((f) -> f.validities)
      .flattenDeep()
      .value()
    return @getValidationsInfo(validitiesArr)


  getValidationsInfo: (validitiesArr) ->
    validitiesErrors = @getValiditiesErrors(validitiesArr)
    validitiesErrorsMsgs = _.map(validitiesErrors, (v) -> v.msg)

    validationsInfo =
      validitiesArr         : validitiesArr
      validitiesErrors      : validitiesErrors
      validitiesErrorsMsgs  : validitiesErrorsMsgs

    return validationsInfo

  getValiditiesErrors: (validitiesArr) ->
    return _.filter(validitiesArr, (v) -> v && !v.valid)

  getBoolMessage: getBoolMessage
