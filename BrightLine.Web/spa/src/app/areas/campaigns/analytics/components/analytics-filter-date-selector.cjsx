DatePicker = require 'components/datepicker/date-picker'
AnalyticsHelper = require 'areas/campaigns/analytics/helpers/campaign-analytics-helper'

module.exports = AnalyticsFilterDateSelector = React.createClass
  displayName: 'AnalyticsFilterDateSelector'
  
  propTypes: 
    summary: React.PropTypes.object.isRequired
    paramsInfo: React.PropTypes.object.isRequired
    campaignDates: React.PropTypes.object.isRequired

  mixins: [React.addons.PureRenderMixin]

  getInitialState: ->
    return {
      datePickerVisible: false
      campaignDates: @props.campaignDates
    }


  getSelections: (summary) ->
    selections = []

    # Note: moment() creates an instance. Any modification to that instance persists. 
    #   Always create a new instance before modifying a moment variable that's used elsewhere (eg: now, beginDate, endDate, etc)

    campaignDates = @state.campaignDates
    now = campaignDates.now
    yesterday = campaignDates.yesterday
    absBd = campaignDates.bd
    absEd = campaignDates.ed

    # Returns whether the date (in moment) lies within the campaign's timespan (beginDate and endDate ) (inclusive)
    isInCampaignTimespan = (date) ->
      return absBd <= date && date <= absEd
    
    # Note: If a campaign has ended, it should only return 'Full campaign' and 'custom', regardless of when it has ended.
    if campaignDates.isCampaignRunning
      # Yesterday: If yesterday is WITHIN the campaign timespan
      if isInCampaignTimespan(yesterday)
        selections.push {id: "yesterday", display: 'Yesterday'}

      # Last 7 days: If Either the Starting or Ending dates of the last 7 days are WITHIN the campaign timespan
      last7Bd = moment(yesterday).add(-7, 'days')
      last7Ed = yesterday
      if isInCampaignTimespan(last7Bd) || isInCampaignTimespan(last7Ed)
        selections.push {id: "last7days", display: 'Last 7 Days'}

      # Last 30 days: If Either the Starting or Ending dates of the last 30 days are WITHIN the campaign timespan
      last30Bd = moment(yesterday).add(-30, 'days')
      last30Ed = yesterday
      if isInCampaignTimespan(last30Bd) || isInCampaignTimespan(last30Ed)
        selections.push {id: "last30days", display: 'Last 30 Days'}

      # Last Week: If either last weekend's end(last sunday) or start(monday before that last sunday) is WITHIN the campaign timespan
      lastWeekBd = @getLastWeekStart(now)
      lastWeekEd = @getLastWeekEnd(now)
      if isInCampaignTimespan(lastWeekBd) || isInCampaignTimespan(lastWeekEd)
        selections.push {id: "lastweek", display: 'Last Week'}

      # Last Month: If Either the Starting or Ending dates of the last month is WITHIN the campaign timespan
      lastMonthBd = moment(now).subtract(1, 'months').startOf('month')  # First day of last month
      lastMonthEd = moment(lastMonthBd).endOf('month')                  # Last day of last month
      if isInCampaignTimespan(lastMonthBd) || isInCampaignTimespan(lastMonthEd)
        selections.push {id: "lastmonth", display: 'Last Month'}

      # Current Week: If Either the Starting or Ending dates of the current week is WITHIN the campaign timespan
      #   and today isn't a monday (i.e. both begin and end dates are monday on a monday)
      currentWeekBd = moment(now).startOf('isoweek')
      currentWeekEd = yesterday
      if (isInCampaignTimespan(currentWeekBd) || isInCampaignTimespan(currentWeekEd)) && currentWeekBd < currentWeekEd
        selections.push {id: "currentweek", display: 'Current Week'}

      # Current Month: If this month's starting date falls WITHIN the campaign timespan && today is not the 1st of the month
      currentMonthBd = moment(now).startOf('month')
      currentMonthEd = yesterday
      if isInCampaignTimespan(currentMonthBd) || isInCampaignTimespan(currentMonthEd)
        selections.push {id: "currentmonth", display: 'Current Month'}
    
    # Full Campaign: Should always be available
    selections.push {id: "fullcampaign", display: "Full Campaign"}

    return selections


  onDateSelect: (selectionId) ->
    campaignDates = @state.campaignDates
    now = campaignDates.now
    yesterday = campaignDates.yesterday
    absBd = campaignDates.bd
    absEd = campaignDates.ed
    
    if selectionId == "custom"
      @setState
        datePickerVisible: true

    else
      bd1 = null
      ed1 = null

      switch selectionId
        when 'yesterday'
          bd1 = yesterday
          ed1 = yesterday
        when 'last7days'
          bd1 = moment(now).subtract(7, 'day')
          ed1 = yesterday
        when 'last30days'
          bd1 = moment(now).subtract(31, 'day')
          ed1 = yesterday
        when 'lastweek'
          bd1 = @getLastWeekStart()
          ed1 = @getLastWeekEnd()
        when 'lastmonth'
          bd1 = moment(now).subtract(1, 'month').startOf('month')
          ed1 = moment(now).subtract(1, 'month').endOf('month')
        when 'currentweek'
          bd1 = moment(now).startOf('isoweek')
          ed1 = yesterday
        when 'currentmonth'
          bd1 = moment(now).startOf('month')
          ed1 = yesterday
        when 'fullcampaign'
          bd1 = campaignDates.bd
          ed1 = campaignDates.ed

      dateFormat = 'YYYYMMDD'

      datesBounded = AnalyticsHelper.getCampaignDatesBounded([bd1, ed1], campaignDates)

      bd1Formatted = datesBounded[0].format(dateFormat)
      ed1Formatted = datesBounded[1].format(dateFormat)
      @dateChange(bd1Formatted, ed1Formatted)


  dateChange: (bd, ed) =>
    #Set up a default/less-granular interval for first-time use. User can still select a more granular interval.
    interval = null
    dateFormat = 'YYYYMMDD'

    bd1 = moment(bd, dateFormat)
    ed1 = moment(ed, dateFormat)

    timespan = ed1.diff(bd1, 'days') #in days
    if timespan <= 1
      interval = 'hour'
    else if 1 < timespan && timespan <= 180
      interval = 'day'
    else if 180 <  timespan && timespan <= 360
      interval = 'week'
    else if 360 <  timespan
      interval = 'month'

    url = window.location.pathname  + window.location.search
    url = utility.updateQueryString("bd1", bd, url)
    url = utility.updateQueryString("ed1", ed, url)
    url = utility.updateQueryString("int", interval, url)

    page(url)


  getLastWeekStart: (now) ->
    # moment().startOf('isoweek') returns this week's monday
    lastMonday = moment(now).startOf('isoweek').add(-7, 'days') # last monday = 7 days before this week's monday
    return lastMonday


  getLastWeekEnd: (now) ->
    # moment().startOf('isoweek') returns this week's monday
    lastSunday = moment(now).startOf('isoweek').add(-1, "days") # lastSunday = 1 day before this week's monday
    return lastSunday


  getCurrentDateInMoment: ->
    format = 'YYYYMMDD'

    return {
      now: utility.moment.allToMoment(@props.summary.databaseDate)
      bd1: utility.moment.allToMoment(@props.paramsInfo.bd1)
      ed1: utility.moment.allToMoment(@props.paramsInfo.ed1)
    }


  datePickerClose: ->
    @setState
      datePickerVisible: false


  datePickerApply: ->
    @datePickerClose()

    datePickerDates = @state.datePickerDates

    @dateChange(datePickerDates.bd1, datePickerDates.ed1)


  renderDatePicker:  ->
    return <div/> if !@state.datePickerVisible

    currentDateInMoment = @getCurrentDateInMoment()
    campaignDates = @state.campaignDates

    # Set today's date (now) as a relevant date, so that user doesn't have to scroll back
    # several months for a campaign that has completed.
    todaysDateAsRelevantMoment = currentDateInMoment.now
    if !AnalyticsHelper.isDateWithinCampaignBounds(currentDateInMoment.now, campaignDates)
      todaysDateAsRelevantMoment = currentDateInMoment.ed1

    datePickerOptions = 
      dates: [currentDateInMoment.bd1, currentDateInMoment.ed1]
      calendars: 2
      mode: 'range'
      current: currentDateInMoment.now.toDate()

      onChange: (dates, el) =>

        @state.datePickerDates = 
          bd1: utility.moment.allToString(dates[0])
          ed1: utility.moment.allToString(dates[1])

      onRenderCell: (div, date) =>
        dateInMoment = utility.moment.allToMoment(date)
        disabled = !AnalyticsHelper.isDateWithinCampaignBounds(dateInMoment, campaignDates)

        opts = 
          className: if disabled then 'datepickerInvalid' else ''
          disabled: disabled
        return opts

      onClose: @datePickerClose

    return (
      <div id='analytics-date-picker-container' className="date-picker-container">
        <DatePicker {...datePickerOptions}/>
        <div className="date-picker-footer">
          <button className="btn btn-default apply" type="button" onClick={@datePickerApply}>Apply</button>
        </div>
      </div>
    )


  render: ->
    summary = @props.summary

    selections = @getSelections(summary)

    currentDate = @getCurrentDateInMoment()

    currentDateformat = "MMM. DD, YYYY"
    currentDateString =
      bd1: currentDate.bd1.format(currentDateformat)
      ed1: currentDate.ed1.format(currentDateformat)

    return (
      <div className="dropdown">
        <div className="dropdown pull-right align-right">
          <button className="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
            <span id="selected-date">{"#{currentDateString.bd1} - #{currentDateString.ed1}"}</span>
            <span className="caret" style={marginLeft: 5}></span>
          </button>
          <ul id="date-selector" className="dropdown-menu">
            {_.map(selections, (selection, i) =>
              return <li key={i}><a tabIndex="-1" onClick={@onDateSelect.bind(this, selection.id)}> {selection.display} </a></li>
            )}
            
            <li className="divider"></li>
            <li><a tabIndex="-1" onClick={@onDateSelect.bind(this, 'custom')}>Custom</a></li>
          </ul>
          {@renderDatePicker()}
        </div>
      </div>
    )
