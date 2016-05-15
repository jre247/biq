# React view that wraps http://foxrunsoftware.github.com/DatePicker/

module.exports = DatePicker = React.createClass
  displayName: 'DatePicker'

  propTypes: 
    dates: React.PropTypes.array.isRequired # Accepts a Date() or moment() object
    calendars: React.PropTypes.number.isRequired
    mode: React.PropTypes.string.isRequired
    current: React.PropTypes.object.isRequired
    onChange: React.PropTypes.func.isRequired
    onRenderCell: React.PropTypes.func
    useRelevantCurrent: React.PropTypes.bool

    onClose: React.PropTypes.func
  
  getDefaultProps: ->
    return {
      useRelevantCurrent: true
    }

  mixins: [React.addons.PureRenderMixin]

  render: ->

    return (
      <div className="date-picker-body" ref='datePickerBody'></div>
    )
    

  componentDidMount: ->
    $datePickerEl = $(ReactDOM.findDOMNode(@refs.datePickerBody))

    dates = @props.dates
    datesInDateFormat = _.map(dates, utility.moment.allToDate)

    # If current is provided, use it. Otherwise, try to use the last date provided in values
    current = @props.current
    if @props.useRelevantCurrent && dates.length > 0
      current = utility.moment.allToMoment(dates[dates.length - 1] || moment())
    currentInDateFormat = utility.moment.allToDate(current)
    
    $datePickerEl.DatePicker
      inline: true
      date: datesInDateFormat
      calendars: @props.calendars
      mode: @props.mode
      current: currentInDateFormat
      onChange: @props.onChange
      onRenderCell: @props.onRenderCell || (div, date) =>
        ###
        Set 'disabled' as true/false based on whether 
          a given date(of all calendar cells) lies within a range.
        Example: disabled = !_this.dateWithinValidRange(date)
        ###

        # Don't have a limit by default.
        disabled = false
        opt = {
          className: if disabled then 'datepickerInvalid' else ''
          disabled: disabled
        }

        return opt

    setTimeout =>
      $(document.body).on('click', null, @clickOutsideDatepicker)
    , 10


  clickOutsideDatepicker: (e) ->
    $el = $(e.target)

    #Hide the datepicker, if the user clicks outside it.
    if $el.parents(".date-picker-container").length == 0
      @unbindDatepicker()

      @props.onClose() if @props.onClose


  unbindDatepicker: ->
    #Remove the event handler
    $(document.body).off('click', null, @clickOutsideDatepicker)


  componentWillUnmount: ->
    @unbindDatepicker()
