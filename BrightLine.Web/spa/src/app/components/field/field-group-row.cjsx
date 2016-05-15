Field                 = require 'components/field/field'

module.exports = FieldGroupRow = React.createClass
  renderColumns: (columns) ->
    groupLabel = @props.groupLabel

    if (@props.displayGroupLabel)
      displayGroupLabel = true

    fieldRowIndex = @props.rowIndex

    columnElements = @getColumnElements(columns)

    return (
      <div>
        <div className={cs({
            hide: if !displayGroupLabel then true else false
            'field-group-label': true
          })}>
          {groupLabel}
        </div>

        <div className={cs({
            hide: if _.isEmpty(columnElements) then true else false
            "col-xs-12 field-group-row field-group-row-#{fieldRowIndex}": true
          })}>
          <div className='form-group clearfix'>
            <label className="col-xs-2 control-label field-group-row-label">
              <span> {@props.label} </span>
            </label>

            {columnElements}
          </div>
        </div>
      </div>
    )

  getColumnElements: (columns) ->
    columnsFiltered = _.filter(columns, (column, i) -> return !column.field.hide)
    columnElements = _.map(columnsFiltered, (column, i) =>
      field = column.field
      fieldIndex = column.index
      columnIndex = i

      additionalProps =
        key: field.name
        ref: "field_#{fieldIndex}"
        onUpdate: @props.onUpdate.bind(@props.instance, field)
        index: fieldIndex

      return (
        <div key={columnIndex}
          className={cs({
            'field-group-column': true
            "field-group-column-#{columnIndex}": true
            'col-xs-3': columns.length == 2
            'col-xs-2': columns.length == 3
          })}>
            <Field {...field} {...additionalProps} />
        </div>
      )
    )

    return columnElements

  renderFieldGroupRow: ->
    columns = @props.columns
    @renderColumns(columns)

  render: ->
    return (
      <div className="field-group-row-container">
        {@renderFieldGroupRow()}
      </div>
    )
