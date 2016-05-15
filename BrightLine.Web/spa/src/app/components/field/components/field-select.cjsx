FieldHelper = require 'components/field/field-helper'

module.exports = FieldSelect = React.createClass
  displayName: 'FieldSelect'
  
  mixins: [React.addons.PureRenderMixin]

  render: ->
    props = @props

    # if props.readOnly
      # return <span className="fieldAsSpan">{props.values?[0]?.label}</span>

    isMultiple = props.isMultiple || props.isList || false
    placeholder = props.placeholder || ''
    if !placeholder
      displayName = props.displayName
      if ['a','e', 'i', 'o', 'u'].indexOf(displayName[0].toLowerCase()) >= 0 
        article = 'an'
      else
        article = 'a'
      
      if isMultiple
        placeholder = "Select multiple #{props.displayName}"
      else
        placeholder = "Select #{article} #{props.displayName}"

    selectOptions = 
      options: props.items
      value: props.values
      multi: isMultiple
      searchable: props.searchable || false
      clearable: props.clearable || false
      placeholder: placeholder
      singleValueComponent: props.SelectValueComponent    || FieldHelper.formatters.select.SelectValueDefault
      optionComponent:      props.SelectOptionComponent   || FieldHelper.formatters.select.SelectOptionDefault
      disabled: props.readOnly
      onChange: (values, selections) =>
        props.onChange(selections)

    return (
      <Select {...selectOptions} ref="subField"/>
    )
    
