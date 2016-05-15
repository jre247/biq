class CampaignAnalyticsLookupsHelper
  isMetricsLookupsLoaded = false
  metricsLookups = {}

  getMetricsLookups:() ->
    # only want to load metrics lookups once
    if isMetricsLookupsLoaded
      return @metricsLookups

    metricKeyToSpecialWidthMapping =
      '%ReturningUsers': 200
      'AvgVideoViewsPerSession': 200
      'AvgPageviewsPerSession': 200
      'AverageVideoDuration': 200
      'Video%Avg': 200
      'AvgPageviewDuration': 200
      'QualifiedVideoViews': 165
      'VideoCompletionRate': 165

    @metricsLookups = _.clone(window.lookups.metrics)

    self = @
    _.each(metricKeyToSpecialWidthMapping, (width, metricKey) ->
      self.metricsLookups[metricKey].width = width
    )

    isMetricsLookupsLoaded = true

    return @metricsLookups

  getMetricWidthForMetricName:(metricName) ->
    lookups = @getMetricsLookups()
    key = @getMetricLookupKeyForMetricName(metricName)
    lookupItem = lookups[key]

    return lookupItem.width

  getMetricLookupKeyForMetricName:(metricName) ->
    # remove the following characters from the Metric Name: periods, and spaces. Replace slashes with "Per"
    metricKey = metricName.replace('/', 'Per').replace(/\s+/g, '').replace('.', '')

    return metricKey

module.exports = new CampaignAnalyticsLookupsHelper
