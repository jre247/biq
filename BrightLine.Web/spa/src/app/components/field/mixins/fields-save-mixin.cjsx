FieldValidationHelper = require 'components/field/field-validation-helper'

FieldsSaveMixin = module.exports =
 # saveToServer must be implimented

  onSave: ->
    # First, validate all fields to have validations run at least once.
    @validateFields()
    .then (validationsInfo) =>
      if validationsInfo.validitiesErrors.length > 0
        do logger.log 'validitiesErrors', validationsInfo.validitiesErrors
        return

      # Everything is validated at this point. Continue
      @resourcesRegisterStart()
      .catch @resourcesRegisterFailure

      .then @resourcesUploadStart
      .catch @resourcesUploadFailure

      .then @saveToServer
      .then @saveToServerSuccess
      .catch @saveToServerFailure
    .catch App.onError


  resourcesRegisterStart: ->
    # Update, and disable save button
    @state.buttonStates.current = 'uploading'
    @forceUpdate()

    fields = @state.fields
    creativeId = @state.creativeId

    # ResourceHelper.prototype.resourcesRegister(@promotionalId, @resourceFields)
    typesResource = ['video', 'image']

    registrationPromises = []

    _.each(fields, (field) ->
      fieldType = field.type
      isResource = typesResource.indexOf(fieldType) >= 0
      # Skip if this is not a resource field.
      if !isResource
        return

      fieldValue = field.values?[0]

      # If the field is empty, return.
      if !fieldValue
        return

      # If there's a resource, and no file was selected, skip registering. Reuse the previous one
      if fieldValue.resource && !fieldValue.file
        return

      else if fieldValue.file
        # If a file was selected (regardless if the field had a past resource), register this new file

        registrationDeferred = RSVP.defer()
        registrationPromises.push registrationDeferred.promise

        resource = null

        if fieldType == 'image'
          resource =
            fileName:       fieldValue.file.name
            size:           fieldValue.file.size
            width:          fieldValue.dimensions.width
            height:         fieldValue.dimensions.height
            resourceType:   field.resourceType
            creativeId:     creativeId

        else if fieldType == 'video'
          resource =
            fileName:     fieldValue.file.name
            size:         fieldValue.file.size
            width:        fieldValue.width
            height:       fieldValue.height
            duration:     fieldValue.duration
            resourceType: field.resourceType
            creativeId:   creativeId

        $.ajax({
          url: "/api/cms/resources/register",
          type: "POST",
          data: JSON.stringify(resource),
          contentType: "application/json",
          dataType: "json"
        })
        .then (resource) ->
          # Store the resource in the fieldValue
          fieldValue.resource = resource
          do logger.log 'resourse registered', resource

          # Resolve the promise
          registrationDeferred.resolve('file registered')
    )
    return RSVP.all(registrationPromises)


  resourcesRegisterFailure: (resp) ->
    return utility.promise.rejected 1


  resourcesUploadStart: ->
    typesResource = ['image', 'video']
    resourceFields = _.filter(@state.fields, (field) ->
      # Field is a resource, ResourceField is being used, and It also contains a file
      return typesResource.indexOf(field.type) >= 0 && !field.hide && field.values?[0]?.file
    )

    resourcesUploadedPromises = []
    _.each(resourceFields, (field) =>
      fieldValue = field.values[0]

      resourceUploadDeferred = RSVP.defer()
      resourcesUploadedPromises.push resourceUploadDeferred.promise

      isCms = @state.isCms || false;

      url = "/api/cms/resources/upload/#{fieldValue.resource.id}/campaign/#{@state.campaignId}?isCms=#{isCms}"

      formData = new FormData()
      formData.append("UploadedResource", fieldValue.file)

      $.ajax
        url: url
        type: 'POST'
        data: formData
        xhr: ->
          uxhr = $.ajaxSettings.xhr()
          if uxhr.upload
            uxhr.upload.addEventListener('progress', ((e) ->
              # if e.lengthComputable
                # if e.total > 0
                  # 1==1
                  # do logger.log 'upload progress', e.load, e.total
              return
            ), false)
          return uxhr
        success: (response) ->
          do logger.log 'upload success', arguments
          resourceUploadDeferred.resolve 'file uploaded'
        error: (response) ->
          do logger.log 'upload error', arguments
          resourceUploadDeferred.reject 'file not uploaded'
        complete: ->
          do logger.log 'upload complete', arguments
        cache: false
        contentType: false
        enctype: 'multipart/form-data'
        processData: false
    )
    return RSVP.all(resourcesUploadedPromises)


  resourcesUploadFailure: (resp) ->
    utility.promise.rejected 1


  saveToServerFailure: (xhr, status, message) ->
    do logger.error "Error saving entity to server, while no errors were caught in the frontend"

    # Restore, and enable save button
    @state.buttonStates.current = @state.buttonStates.original

    # Show error message returned by server. Fallback to a default, when nothing is found
    message = message || xhr.responseJSON?.Message || "Error processing request"
    validitiesArr = [FieldValidationHelper.getBoolMessage(false, message)]
    @state.validationsInfo = FieldValidationHelper.getValidationsInfo(validitiesArr)

    # Rerender
    @forceUpdate()
