FieldGroupRow                 = require 'components/field/field-group-row'
Field                         = require 'components/field/field'

module.exports = FieldList = React.createClass
  buildFieldRowsWithoutGroups: (fields) ->
    fieldRows = []
    _.each(fields, (field, i) =>
      if (!field.isGroupMember)
        columns = []
        column =
          index: i
          field: field

        columns.push column

        row =
          columns: columns,

        fieldRows.push row
    )

    return fieldRows

  buildFieldGroups: (fields) ->
    fieldGroups = {}
    _.each(fields, (field, i) =>
      if (field.isGroupMember)
        groupData = field.groupData
        groupNumber = groupData.groupNumber
        groupLabel = groupData.groupLabel
        rowNumber = groupData.rowNumber

        if (!fieldGroups[groupNumber])
          fieldGroups[groupNumber] = {
            rows: {}
          }

        fieldGroups[groupNumber].id = groupNumber
        fieldGroups[groupNumber].label = groupLabel

        if (!fieldGroups[groupNumber].rows[rowNumber])
          fieldGroups[groupNumber].rows[rowNumber] = {fields: []}

        fieldGroups[groupNumber].rows[rowNumber].fields.push field
    )

    return fieldGroups

  addFieldGroupsToFieldRows: (fieldGroups, fieldRows)->
    _.each(fieldGroups, (group, i) =>
      groupLabel = group.label

      _.each(group.rows, (row, r) =>
        columns = []
        rowLabel = row.fields[0].groupData.rowLabel

        _.each(row.fields, (field, f) =>
          column =
            index: f
            field: field

          columns.push column
        )

        row =
          columns: columns
          label: rowLabel
          groupLabel: groupLabel
          isGroupMember: true

        fieldRows.push row
      )
    )

    return fieldRows

  renderFieldList: ->
    fields = @props.fields

    fieldRows = @buildFieldRowsWithoutGroups(fields)

    fieldGroups = @buildFieldGroups(fields)

    fieldRows = @addFieldGroupsToFieldRows(fieldGroups, fieldRows)

    groupLabelsHash = {}

    _.map(fieldRows, (row, i) =>
      if (row.isGroupMember)
        # Only want to display the group label once per group
        groupLabel = row.groupLabel
        if (groupLabel)
          groupLabelExists = groupLabelsHash[groupLabel]
          if (!groupLabelsHash[groupLabel])
            groupLabelsHash[groupLabel] = true

        additionalProps =
          rowIndex: i
          displayGroupLabel: !groupLabelExists

        return (
          <div className="fields-container clearfix" key={i}>
            <FieldGroupRow {...@props} {...row} {...additionalProps} />
          </div>
        )

      else
        field = row.columns[0].field
        additionalProps =
          onUpdate: @props.onUpdate.bind(@props.instance, field)
          
        return (
          <div className="fields-container clearfix" key={i}>
            <Field {...@props} {...field} {...additionalProps} />
          </div>
        )

   )

  render: ->
    return(
      <div className='field-list'>
        {@renderFieldList()}
      </div>
    )
