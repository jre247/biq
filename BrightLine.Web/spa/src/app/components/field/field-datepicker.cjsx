Datepicker = require 'components/datepicker/date-picker'

module.exports = FieldDatepicker = React.createClass
  displayName: 'FieldDatepicker'
  
  propTypes: 
    calendars: React.PropTypes.number.isRequired
  
  mixins: [React.addons.PureRenderMixin]
  
  getDefaultProps: ->
    return {
      calendars: 1
      values: []
      datepickerVisible: false
      current: moment()
      format: 'MM/DD/YYYY'
    }


  getInitialState: ->
    datepickerVisible: @props.datepickerVisible


  onChange: (dateOrDates) ->
    if !_.isArray(dateOrDates)
      dates = [dateOrDates]
    else
      dates = dateOrDates

    @state.values = _.map(dates, utility.moment.allToMoment)
    

  onDateSelectorClick: ->
    @setState
      datepickerVisible: true


  datepickerClose: ->
    @clickOutsideDatepickerUnbind()
    @setState
      datepickerVisible: false

  
  clickOutsideDatepicker: (e) ->
    $el = $(e.target)

    #Hide the datepicker, if the user clicks outside it.
    if $el.parents(".date-picker-container").length == 0
      @datepickerClose()


  clickOutsideDatepickerUnbind: ->
    #Remove the event handler
    $(document.body).off('click', null, @clickOutsideDatepicker)


  componentWillUnmount: ->
    @clickOutsideDatepickerUnbind()


  datepickerApply: ->
    @datepickerClose()
    
    @props.onChange(@state.values) if @props.onChange


  isDateWithinValidRange: (dateInMoment) ->
    # All dates are valid by default if validations aren't provided
    valid = true

    # Check for lower and upper date bounds
    min = @props.validations?.min
    min = min() if typeof min == 'function'
    if min
      min = utility.moment.allToMoment(min)
      valid = min <= dateInMoment 

    # Date is valid so far. Continue checking for max
    max = @props.validations?.max
    max = max() if typeof max == 'function'
    if valid && max
      max = utility.moment.allToMoment(max)
      valid = dateInMoment <= max

    return valid


  renderDatepicker: (datepickerProps) ->
    return <div/> if !@state.datepickerVisible

    calendars = @props.calendars

    datepickerProps = 
      dates: @props.values
      calendars: @props.calendars
      mode: if @props.isMultiple then 'range' else 'single'
      current: @props.current
      onChange: @onChange
      onRenderCell: @props.onRenderCell || (div, date) =>
        dateInMoment = utility.moment.allToMoment(date)
        disabled = !@isDateWithinValidRange(dateInMoment)

        opts = 
          className: if disabled then 'datepickerInvalid' else ''
          disabled: disabled
        return opts

    setTimeout =>
      $(document.body).on('click', null, @clickOutsideDatepicker)
    , 10

    return (
      <div className="date-picker-precontainer">
        <div className="date-picker-container" data-calendars={@props.calendars} style={display: 'block'}>
          <Datepicker {...datepickerProps}/>
          <div className="date-picker-footer">
            <button className="btn btn-default apply" type="button" onClick={@datepickerApply}>Apply</button>
          </div>
        </div>
      </div>
      
    )

  render: ->
    valuesAsString = _.map(@props.values, (v) => utility.moment.format(v, @props.format)).join(' - ')
    
    return (
      <div className="date-picker-field-container">
        <span className="date-selector" onClick={@onDateSelectorClick}>
          <span className={cs({
              hide: !!@props.values[0]
            })}>Select a date...</span>
          <span>{valuesAsString}</span>
        </span>
        {@renderDatepicker()}
      </div>
    )
    
