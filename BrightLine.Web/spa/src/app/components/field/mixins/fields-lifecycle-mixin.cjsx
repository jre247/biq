FieldValidationHelper = require 'components/field/field-validation-helper'

FieldsGeneralMixin = module.exports =

  onUpdate: (field, updates) ->
    ###
    This is fired by the field.cjsx component after an user updates a field. 
    Order of operations:
    1. Rerender. Show the update the user made
    2. Validate the field, and rerender to show field validity
    3. If valid, cross-update other fields. Run validation on affected fields.
    ###

    # Apply updates from user
    _.extend(field, updates)
    @forceUpdate()

    # Validate current field, and rerender
    debounceDelay = field.validations?.unique?.prototype.debounceDelay || 0
    clearTimeout(@state.fieldValidationTimeout)
    @state.fieldValidationTimeout = setTimeout =>
      @validateField(field)
      .then (fieldValidities) =>
        errs = FieldValidationHelper.getValiditiesErrors(fieldValidities)

        if errs.length
          # Show error msg, and rerender.
          @state.validationsInfo = FieldValidationHelper.getFieldsValidationsInfo(@state.fields)
          @forceUpdate()

          # Skip cross-field updates
          return

        else
          # No errors. Cross-update other fields, and revalidate

          # Custom cross-field updates
          field.isBeingEdited = true   # Set up a property to allow fields' updaters to know if a field is being edited
          @updateFields()              # Apply any other updates via fields' props' updaters
          field.isBeingEdited = false  # Set it as not being edited

          if field.onUpdatePost
            field.onUpdatePost(field)

          # Validities of all the fields could have changed. Reaggregate them to update the validation summary
          @state.validationsInfo = FieldValidationHelper.getFieldsValidationsInfo(@state.fields)
          # Update all the fields with their potentially new items, values, and validities 
          @forceUpdate()
      .catch App.onError

    , debounceDelay


  updateFields: (config = {}) ->
    # Cross-field updating.
    # This allows a field to have dynamic properties.
    # This is useful, when a field's properties can change over time
    # For example, changes could be triggered via user input on another field that's related

    updatableProps = ['hide', 'items', 'values', 'readOnly', 'validations']
    for field, fieldIndex in @state.fields
      configField = {
        field:      field
        fieldIndex: fieldIndex
      }
      _.defaultsDeep(configField, config)

      # If a field contains one of the updatableProps, apply the update
      for prop in updatableProps
        keyPropUpdater = "#{prop}Updater"
        if typeof field[keyPropUpdater] == 'function'
          # Execute the Updater
          field[prop] = field[keyPropUpdater](configField)
          
          # Clear out any previously existing validation, which is probably outdated
          delete field.validities

      # For single selects, ensure that the values contain items that belong in the items.
      if field.type == 'select'
        field.values = _.filter(field.values, (v) ->
          return (_.chain(field.items)
            .filter((i) -> i.value == v.value)
            .take(1)
            .value()
          ).length > 0
        )


  validateFields: (fields) ->
    # Update, and disable the Save button
    @state.buttonStates.current = 'validating'
    @forceUpdate()

    fields = fields || @state.fields

    fieldsValidationPromises = _.flattenDeep(_.map(fields, @validateField))

    RSVP.all(fieldsValidationPromises)
    .then =>
      # Validations of the given subset of fields are complete.
      # Consider all fields, when updating @state.validationsInfo
      @state.validationsInfo = FieldValidationHelper.getFieldsValidationsInfo(@state.fields)

      # Restore, and enable the Save button
      @state.buttonStates.current = @state.buttonStates.original

      @forceUpdate()

      return @state.validationsInfo
    .catch App.onError


  validateField: (field) ->
    fieldDisplayName        = field.displayName
    fieldValidationsConfig  = field.validations

    # Get this fieldType's validations
    fieldTypeValidators = FieldValidationHelper.getFieldValidators(field.type)
    fieldValue = FieldValidationHelper.prepareValueForValidation(field.type, field.values)

    # Have all promises gather in an array
    fieldValidationPromises = []

    # Loop through the validation config of the field, and call corresponding validator
    for vType, vConfig of fieldValidationsConfig
      # Get the reference to the validator function
      validator = fieldTypeValidators[vType]

      # Certain fields don't validate via validationHelper.
      #   For example, datepicker starts off right away with min and max, and don't require validation. Skip.
      if !validator
        continue

      # Gather each promises that are returned after running a validation.
      validationPromise = validator(fieldDisplayName, fieldValue, vConfig)
      fieldValidationPromises.push validationPromise

    # Return a promise, once all the field's validations are complete.
    return RSVP.all(fieldValidationPromises).then (validities) ->
      # Validations for this field have run at this point. Field could be valid/invalid.
      field.validities = validities
    .catch App.onError
