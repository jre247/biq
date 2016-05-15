FieldHelper                   = require 'components/field/field-helper'
formatterSelect               = FieldHelper.formatters.select
formatterSelectOptionResolver = formatterSelect.SelectOptionResolver
formatterSelectValueResolver  = formatterSelect.SelectValueResolver

RefOption             = require './ref-option'

module.exports = 
  parseAllValidationValues: (instance) ->
    fieldTypesToNotParse = ["datetime"]
    keysToParseToInt = ['width', 'height', 'maxLength', 'minLength', 'min', 'max']
    keysToParseToBool = ['required']
    keysToParseToArray = ['extension']

    _.each(instance.fields, (field) ->
      fieldType = field.type

      if fieldTypesToNotParse.indexOf(fieldType) >= 0
        return

      fieldValidations = field.validations
      if fieldValidations
        for vKey, vVal of fieldValidations

          if keysToParseToInt.indexOf(vKey) >= 0
            fieldValidations[vKey] = parseInt(vVal)
            continue

          if keysToParseToBool.indexOf(vKey) >= 0
            fieldValidations[vKey] = vVal == 'true'
            continue

          if keysToParseToArray.indexOf(vKey) >= 0
            fieldValidations[vKey] = vVal.split(',')
            continue
    )

  prepareDefaultValues: (instance) ->
    typesText = ['string']
    typesNumber = ['integer']
    typesBool = ['bool']
    typesResource = ['image', 'video']
    typeRefs = ['refToModel', 'refToPage']

    _.each(instance.fields, (field) ->
      if typesText.indexOf(field.type) >= 0
        field.values = [''] if field.values == null

      if typesBool.indexOf(field.type) >= 0
        if field.values && field.values.length > 0
          fieldVal = ["True", "true"].indexOf(field.values[0]) >= 0
          field.values = [fieldVal]
        else
          field.values = [false]

      if typesNumber.indexOf(field.type) >= 0
        field.values = [0] if field.values == null

      if field.type == typesResource[0]
        field.resourceType = 2 if !field.resourceType

      if field.type == typesResource[1]
        field.resourceType = 4 if !field.resourceType


      if typesResource.indexOf(field.type) >= 0
        if field.values && field.values.length > 0
          field.values = [{resource: field.values[0]}]

      if typeRefs.indexOf(field.type) >= 0 
        if field.list
          field.isMultiple = true

        # Set up thumbnail based select for better UX
        _.extend(field, {
          SelectValueComponent:   formatterSelectValueResolver(RefOption)
          SelectOptionComponent:  formatterSelectOptionResolver(RefOption)
        })

      return
    )

  prepareRefValuesString: (items) ->


  nameFieldInsert: (fields, instanceId, instanceName) ->
    # This inserts the name field at the very beginning. It needs to be extracted
    #  during onSave, and placed at the root of the object posted to the server.
    fields.unshift
      name: 'name'
      displayName: 'Name'
      type: 'string'
      description: "The name of this Model Instance"
      id: 0
      values: [if instanceId == 0 then '' else instanceName]
      validations: 
        required: true


  nameFieldExtract: (fields) ->
    # This extracts out the name field(which was inserted to the front via @nameFieldInsert)
    #  and returns the value. THe value is then added to the object that's posted to the server for saving.
    nameField = fields.shift()
    name = nameField.value[0]
    return name


  getSaveData: (fields) ->

    typesResource = ['video', 'image']
    typesDate = ['datetime']
    typesRef = ['refToModel', 'refToPage']

    fieldsData = []
    for field in fields
      # Set up the default structure
      fieldData = 
        id: field.id
        name: field.name
        value: field.values

      # Make any further changes depending on field's type
      if typesResource.indexOf(field.type) >= 0
        if fieldData.value && fieldData.value.length > 0
          fieldData.value = [fieldData.value[0].resource.id]
        else
          fieldData.value = [undefined]

      if typesRef.indexOf(field.type) >= 0
        values = []

        if field.type == typesRef[0] # Type is model
          for selectedInstance in field.values
            values.push
              model: field.refModel.model       # Name of the cms model. eg: 'choice'
              instanceId: selectedInstance.id   # Eg: Id of instances in Campaigns.CmsModelInstances api

        if field.type == typesRef[1] # Type is page
          for selectedInstance in field.values
            values.push
              pageId: selectedInstance.id       # Eg: Id of instances in Campaigns.CmsModelInstances api

        # The c# api expects the value to be as List<string>. So, stringify each object in the array.
        valuesStringified = _.map(values, (obj) -> return JSON.stringify(obj))
        fieldData.value = valuesStringified
        
      fieldsData.push fieldData

    return fieldsData
