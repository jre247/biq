DateHelper            = require 'helpers/date-helper'

module.exports =
  getOverviewOptions: (data) ->
    dateData = data.dateData
    paramsInfo = data.paramsInfo
    baseSeries = null

    categories = dateData.xAxis

    options = @getOptions(data)
    options.xAxis.categories  = categories
    #options.xAxis.tickInterval = tickInterval

    # Chart 2 metrics (m1 and m2) of overview or 1 platform.
    @addSeriesMetric(data, options, data.chartData, paramsInfo.m1, 'area')
    @addSeriesMetric(data, options, data.chartData, paramsInfo.m2)
    options.series[1].yAxis = 1

    @addYAxis(data, options, paramsInfo.m1, paramsInfo.m2)

    options.xAxis.tickInterval = do ->
      # TODO: Highcharts ignores using this exact interval for some reason. Investigate further.

      # Interval = Number of datapoints / Number of xAxis labels
      numDatapoints = options.series[0].data.length
      interval = Math.floor( numDatapoints / 10 )
      interval = Math.min( numDatapoints, interval)
      return interval

    options

  getOptions: (data) ->
    that = @

    bd1 = moment(data.paramsInfo.bd1, 'YYYYMMDD')
    ed1 = moment(data.paramsInfo.ed1, 'YYYYMMDD')
    timespanDays = ed1.diff(bd1, 'days')

    options = {
      chart:
        height: 340
        alignTicks: true
      legend:
        align: 'left'
        verticalAlign: 'bottom'
        y: 13
        floating: true
        borderWidth: 0

      xAxis: {
        type:'datetime'
        labels:
          formatter: ->
            int = data.paramsInfo.interval
            date = moment(new Date(@value))
            timeStamp = ''
            timeFormat = ''
            switch int
              when "month"
                timeFormat = 'MMM'
              when "week"
                timeFormat = 'MMM DD'
              when "day"
                timeFormat = 'MMM DD'
              when "hour"
                # Show the time for up to 2 days. Hide the time for 3 days or more.
                if timespanDays <= 1
                  timeFormat = 'MMM DD ha'
                else
                  timeFormat = 'MMM DD'

            timeStamp = date.format(timeFormat)

            return timeStamp
      },
      yAxis: [],
      series: [],
      tooltip:
        shared: true
        formatter: ->
          interval = data.paramsInfo.interval

          dateParts = data.dateData.dateParts
          begin = new Date(dateParts[this.x].begin)
          end = new Date(dateParts[this.x].end)
          if interval is "hour"
            dateString = moment(begin).format('ddd MMM DD, hh:mm A')
          else if interval is "day"
            dateString = moment(begin).format('ddd MMM DD')
          else
            dateString = moment(begin).format('ddd MMM DD') + " - " + moment(end).format('ddd MMM DD')
          sb = '<b>' + dateString + '</b><br/>'
          _.each(@points, (point) ->
            # template for string
            t = '<span style="color:%COLOR%">\u25CF</span> %NAME%: <b>%VALUE%</b><br/>'
            s = ''

            # format value based on type of metric
            if point.series.options.dataFormat is 'Integer'
              value = Highcharts.numberFormat(point.y, 0, '.', ',')
            else if point.series.options.dataFormat is 'Milliseconds'
              value = DateHelper.convertMilliToTimeStamp(point.y)
            else if point.series.options.dataFormat is 'Percentage'
              value = Highcharts.numberFormat(point.y, 2, '.', ',') + "%"
            else if point.series.options.dataFormat is 'Decimal'
              value = Highcharts.numberFormat(point.y, 2, '.', ',')

            s = t.replace('%COLOR%', point.series.color).replace('%NAME%', point.series.name).replace('%VALUE%', value)
            sb += s
          )
          sb
    }

    options

  addYAxis: (data, options, m1, m2) ->
    lookups = data.lookups
    metrics = lookups.metrics

    # Adds an axis and associated options to yAxis array
    pushAxis = (options, index, opposite = false) ->
      metric = metrics[index]

      axis = {
        opposite: opposite,
        title:
            text: metric.name
        min: 0,
        max: metric.max
        minRange: metric.minRange
        labels:
            enabled: true
            x: 3
            y: 16
            align: if !opposite then 'left' else 'right'
        showFirstLabel: false
      }

      type = metric.type

      if type is "Integer"
          axis.labels.format = '{value:,.0f}'
      else if type is "Milliseconds"
        axis.type = 'datetime'
        axis.dateTimeLabelFormats = {
          second: '%M:%S'
          minute: '%M:%S'
          hour: '%M:%S'
          day: '%M:%S'
          week: '%M:%S'
          month: '%M:%S'
          year: '%M:%S'
        }
      else if type is "Percentage"

        axis.labels.formatter = -> return this.value + ' %'



      if (opposite)
        #axis.linkedTo = 0
        axis.gridLineWidth = 0

      options.yAxis.push(axis)

    pushAxis(options, m1, )

    if (m2)
      pushAxis(options, m2, '{value:,.0f}', true)


  addSeries: (options, seriesOptions) ->
    options.series.push
      name: seriesOptions.name
      data: seriesOptions.data
      dataFormat: seriesOptions.dataFormat
      type: seriesOptions.type

  addSeriesMetric: (data, options, chartData, metricId, type) ->
    lookups = data.lookups
    metric = lookups.metrics[metricId]

    metricValues = chartData.metric[metricId].values
    values = @adjustMetricData(metric, metricValues)

    @addSeries(options, {
      name: metric.name
      data: values
      dataFormat: metric.type
      type: type
    })

  addSeriesPlatform: (data, options, platformId, platformObj, metricId) ->
    lookups = data.lookups
    metric = lookups.metrics[metricId]
    platform = lookups.platforms[platformId]

    metricObj = platformObj[metricId]
    values = @adjustMetricData(metric, metricObj.values)

    @addSeries(options, {
      name: platform.name
      data: values
      dataFormat: metric.type
    })

  adjustMetricData: (metric, metricValues) ->
    # Caution: Be sure to clone(or use _.map) of dataArr, instead of making adjustments directly on dataArr(which is cached).
    #   This way, the adjustments won't persist and stack when the overview tab is visited again.
    if metric.type == "Percentage"
      return _.map(metricValues, (x) -> return x * 100)
    else
      # return the original series by default if no adjustments were made
      return metricValues
