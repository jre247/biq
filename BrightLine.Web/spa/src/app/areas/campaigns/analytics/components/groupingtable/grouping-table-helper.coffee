MetricConfig     = require './metric-config'
CampaignAnalyticsHelper = require '../../helpers/campaign-analytics-helper'

module.exports = GroupingTableHelper =
  iterateGroupings: (groupings, groupingIterator, groupingsParent) ->
    _.each(groupings, (grouping) ->
      # Call the groupingIterator callback
      groupingIterator(grouping, groupingsParent)

      if grouping.values?.length
        GroupingTableHelper.iterateGroupings(grouping.values, groupingIterator, grouping)
    )

  abbreviateMetricName: (metricName) ->
    return metricName.replace('Total', '').replace('Impressions', 'Imps.').replace('Avg. ', '').replace('Interactive','Int.').trim()

  getDimDisplayName: (dimName) ->
    if(dimName == 'mediapartner')
      return 'Media Partner'
    else if(dimName == 'deliverygroup')
      return 'Delivery Group'
    else
      return dimName

  getTitle: (dims) ->
    self = @
    # Prepare Overview table title
    dimensionsSelectedNamesArr = _.chain(dims.split(';'))
      .map((dimName) ->
        dimNameFormatted = self.getDimDisplayName(dimName)
        dimNameFormatted = utility.string.capitalize(dimNameFormatted)
        return dimNameFormatted
      )
      .filter((dimFragName) -> dimFragName.length > 0)
      .value()

    # Set up Oxford commas
    dimLen = dimensionsSelectedNamesArr.length
    if dimLen > 1
      dimensionsSelectedNamesArr[dimLen-1] = "and " + dimensionsSelectedNamesArr[dimLen-1]
    dimensionsSelectedNames = dimensionsSelectedNamesArr.join(', ')

    return "Key Performance Indicators by #{dimensionsSelectedNames}"
