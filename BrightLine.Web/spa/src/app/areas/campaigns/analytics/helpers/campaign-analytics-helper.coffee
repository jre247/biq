DateHelper      = require 'helpers/date-helper'
FiltersHelper   = require 'areas/campaigns/analytics/components/analytics-filter-filters-selector/filters-helper'

class CampaignAnalyticsHelper
  redirectInfo: (paramsInfo, summary, campaignDates) ->
    # Redirects, if params are invalid, out of range, etc
    redirect = false
    url = paramsInfo.url

    # Redirect user to summary page if campaign has no analytics data
    if !summary.hasAnalytics
      redirect = true
      url = "/campaigns/#{summary.id}/summary"
    else
      # Validate params

      # Start with the original url and params in paramsInfo. Make updates to the url at the very end

      # Metric related validation
      metricDefaults = {
        overview: {
          m1: 4 # Total Sessions
          m2: 5 # Avg. Time Spent
        }
        content: {
          m1: 4 # Total Sessions
          m2: 5 # Avg. Time Spent
        }
        promotional: {
          m1: 16 # Interactive Impressions
          m2: 2 # Total Clicks
        }
      }

      if paramsInfo.action == 'overview'
        if utility.user.is(['AgencyPartner','MediaPartner'])
          metricDefaults.overview.m1 = 3 # CTR
          metricDefaults.overview.m2 = 5 # Avg. Time Spent

      actionDefaultMetrics = metricDefaults[paramsInfo.action]

      # Use default metrics if metrics are null
      if paramsInfo.m1 == null
        paramsInfo.m1 = actionDefaultMetrics.m1
        redirect = true

      # Note: m2 is valid with a value of 0 ('select a platform')
      if paramsInfo.m2 == null
        paramsInfo.m2 = actionDefaultMetrics.m2
        redirect = true

      # Prevent same metrics from being compared.
      if paramsInfo.m1 == paramsInfo.m2
        paramsInfo.m1 = actionDefaultMetrics.m1
        paramsInfo.m2 = actionDefaultMetrics.m2
        redirect = true

      # Limit metric selection in promotional tab
      if paramsInfo.action == 'promotional'
        metricIdsAllowed = [0, 21, 16, 2, 3, 19, 18]
        if !_.include(metricIdsAllowed, paramsInfo.m1)
          paramsInfo.m1 = actionDefaultMetrics.m1
          redirect = true
        if !_.include(metricIdsAllowed, paramsInfo.m2)
          paramsInfo.m2 = actionDefaultMetrics.m2
          redirect = true

      # Check dates
      dateFormat = 'YYYYMMDD'

      # Just convert most dates to string, since we are only comparing and not manupilating the dates.
      #   The descending format of dateFormat allows string comparison to work.
      dates =
        # As string
        campaignBegin:  campaignDates.bd.format(dateFormat)
        campaignEnd:    campaignDates.ed.format(dateFormat)

        # As moment, in order to check if it's a valid date
        urlBeginMoment: moment(paramsInfo.bd1, dateFormat)
        urlEndMoment:   moment(paramsInfo.ed1, dateFormat)

      # Make sure that the dates in the url are valid
      if !dates.urlBeginMoment.isValid()
        dates.urlBegin = dates.campaignBegin
        paramsInfo.bd1 = dates.campaignBegin
        redirect = true
      else
        dates.urlBegin = dates.urlBeginMoment.format(dateFormat)

      if !dates.urlEndMoment.isValid()
        dates.urlEnd = dates.campaignEnd
        paramsInfo.ed1 = dates.campaignEnd
        redirect = true
      else
        dates.urlEnd = dates.urlEndMoment.format(dateFormat)


      # If dates are OUTSIDE the campaign's range, bring them back to the bounds
      if dates.urlBegin < dates.campaignBegin || dates.urlBegin > dates.campaignEnd
        paramsInfo.bd1 = dates.campaignBegin
        redirect = true

      if dates.urlEnd < dates.campaignBegin || dates.urlEnd > dates.campaignEnd
        paramsInfo.ed1 = dates.campaignEnd
        redirect = true

      # Set an interval
      # Use the recommended interval from summary api, if there is no interval set
      if paramsInfo.interval == null
        paramsInfo.interval = summary.timeInterval.toLowerCase()
        redirect = true

      if paramsInfo.filters == null
        filters = FiltersHelper.getInitialFilters()
        paramsInfo.filters = FiltersHelper.getFiltersUrlFragment(filters)
        redirect = true

      # Update the url from paramsInfo, if redirect is needed
      if redirect
        url = paramsInfo.url
        url = utility.updateQueryString("bd1", paramsInfo.bd1, url)
        url = utility.updateQueryString("ed1", paramsInfo.ed1, url)
        url = utility.updateQueryString("int", paramsInfo.interval, url)
        url = utility.updateQueryString("m1", paramsInfo.m1, url)
        url = utility.updateQueryString("m2", paramsInfo.m2, url)
        url = utility.updateQueryString("filters", paramsInfo.filters, url)

    return {redirect, url, paramsInfo}


  getRedirectInfo: (summary, paramsInfoCustom) =>
    paramsInfoDefault =
      interval: null
      bd1: null
      ed1: null
      m1: null
      m2: null
      filters: null
      url: "/campaigns/#{summary.id}/analytics"
      action: 'overview'
    paramsInfo = _.extend(paramsInfoDefault, paramsInfoCustom || {})

    campaignDates = this.getCampaignDates(summary)
    redirectInfo = this.redirectInfo(paramsInfo, summary, campaignDates)
    return redirectInfo


  getAnalyticsUrl: (summary, m1 = null, m2 = null) =>
    paramsInfoCustom =
      m1: m1
      m2: m2

    return this.getRedirectInfo(summary, paramsInfoCustom).url

  # there is a requirement to convert the metric ids to an array of strings when being used to generate the overview table
  formatMetricIdsForComponentTable: (metricIds) ->
    metricIdsConverted = []
    _.each(metricIds, (f) -> metricIdsConverted.push f.toString())

    return metricIdsConverted

  getCampaignDates: (summary) ->
    ###
    # Gets all the analytics dates that are used for determminig valid urls/queries.
    # Returns everything as a moment object for consistency

    # Always consider 'now' to be the summary.databaseDate, which is in UTC.
    # All queries should be made to the api in EST. So, use EST for these dates: bd, ed, now, yesterday.
    ###

    timezones =
      UTC: 'Europe/London'
      EST: 'America/New_York'

    nowUTC = moment.tz(summary.databaseDate, timezones.UTC)
    nowEST = nowUTC.clone().tz(timezones.EST)
    now = nowEST

    yesterday     = now.clone().add(-1, 'days').endOf('day')

    # analyticsBeginDate is already in EST
    bdCampaignAnalyticsEST = moment.tz(new Date(summary.analyticsBeginDate), timezones.EST)
    bd = bdCampaignAnalyticsEST

    # analyticsEndDate is already in EST
    edCampaignAnalyticsEST = moment.tz(new Date(summary.analyticsEndDate), timezones.EST).endOf('hour')
    ed = edCampaignAnalyticsEST

    isCampaignRunning = summary.status == "Delivering"

    dates = {now, yesterday, bd, ed, isCampaignRunning}

    return dates


  getParamsInfo: (params, context, qsParsed) ->
    # this is used for validateAndRedirect, and also by the CampaignAnalyticsController actions
    paramsInfo =
      interval: null
      bd1: null
      ed1: null
      platformId: null
      m1: null
      m2: null
      filters: null
      url: window.location.pathname  + window.location.search
      params: params
      context: context
      action: context.action

    if qsParsed.int
      paramsInfo.interval = qsParsed.int.toLowerCase()

    if qsParsed.bd1
      paramsInfo.bd1 = parseInt(qsParsed.bd1)

    if qsParsed.ed1
      paramsInfo.ed1 = parseInt(qsParsed.ed1)

    if qsParsed.m1
      paramsInfo.m1 = parseInt(qsParsed.m1)

    if qsParsed.m2
      paramsInfo.m2 = parseInt(qsParsed.m2)

    if qsParsed.filters
      paramsInfo.filters = qsParsed.filters

    return paramsInfo


  setMetricRanges: (metrics) ->
    # Set custom max and minRange for certain metrics

    # CTR
    metrics['3'].max = 100

    # Interactive Impressions
    metrics['16'].minRange = 100

    # Spot Impressions
    metrics['21'].minRange = 100

    # Avg. Video Views/Session
    metrics['10'].minRange = 5


  formattedMetric: (type, value, abbreviate = false, dashify = true) ->
    if dashify && value == 0
      return '-'

    val = null
    switch type
      when "Integer"
        if abbreviate && value >= 1000000   #if >= 1 million, use decimals
          val = numeral(value).format('0.0a' ).toUpperCase()
        else                                      #otherwise, use commas
          val = numeral(value).format('0,0')
      when "Percentage"
        if value > 1
          val = numeral(value / 100).format('0.00%')
        else
          val = numeral(value).format('0.00%')
      when "Milliseconds"
        val = DateHelper.convertMilliToTimeStamp(value)
      when "Decimal"
        val = numeral(value).format('0,0.00')
        if val == '0.00' and dashify
          val = '-'
      else
        val = value
    return val

  hasMultiplePlatforms: (platforms) ->
    return _.keys(platforms).length > 1



  getCampaignDatesBounded: (dates, campaignDates) ->
    ###
    # Binds/limits given dates within the bounds of the campaign's valid dates
    # @param {array} dates - An array containing 1 or more dates
    # @param {ret val of @getCampaignDates()} campaignDates
    ###
    bd = campaignDates.bd
    ed = campaignDates.ed

    return _.map(dates, (d) ->
      dMoment = utility.moment.allToMoment(d)

      if dMoment < bd
        return bd.clone()
      else if ed < dMoment
        return ed.clone()
      else
        return dMoment
    )

  isDateWithinCampaignBounds: (date, campaignDates) ->
    ###
    # returns if a date is within the bounds of the campaign's valid dates
    # @param {array} date in moment
    # @param {ret val of @getCampaignDates()} campaignDates
    ###
    return campaignDates.bd <= date && date <= campaignDates.ed

module.exports = new CampaignAnalyticsHelper
