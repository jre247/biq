getFiltersOriginalMocked = ->
  filtersOriginal = [
      {
        id: 'mediapartner'
        name: 'Media Partner'
        values: []
      }
      {
        id: 'platform'
        name: 'Platform'
        values: []
      }
      {
        id: 'deliverygroup'
        name: 'Delivery Group'
        values: []
      }
    ]
  return filtersOriginal


module.exports =

  getInitialFilters: (filtersOriginal, user) ->
    filtersOriginal = filtersOriginal || getFiltersOriginalMocked()

    # Clone it to prevent polluting the cached api data that may be used somewhere else.
    filters = _.cloneDeep(filtersOriginal)

    # Set up an array, where selected filters will be pushed into in proper order
    filtersOrdered = _.clone(filters)

    # Set up the user's default filter setting.
    dimsUserDefault = []
    dimsUserAllowed = []
    user = user || (if _bl then _bl.user else {})

    if user.isEmployee
      dimsUserDefault = ['mediapartner']
      dimsUserAllowed = ['mediapartner', 'platform', 'deliverygroup']
    else if user.isAgencyPartner
      dimsUserDefault = ['mediapartner']
      dimsUserAllowed = ['mediapartner', 'deliverygroup']
    else if user.isMediaPartner
      dimsUserDefault = ['mediapartner']
      dimsUserAllowed = ['mediapartner', 'deliverygroup']

    # Filter out disallowed filters.
    filters = _.filter(filters, (d) ->
      return dimsUserAllowed.indexOf(d.id) >= 0
    )


    # Next, set up initial selections and isRemovable based on the above settings
    for dim in filters
      # Whether the filter is in the user's default set of filters
      dimIsUserDefault = dimsUserDefault.indexOf(dim.id) >= 0

      # Auto-Select the default filter.
      dim.selected = dimIsUserDefault

      # Set up removability. Only non-default filters are removable.
      dim.isRemovable = true


    # Next, set up further selections based on the current querystring
    selectionQS = utility.getQueryValue('filters')      # Example: "mediaPartner:Vevo;platform:roku,samsung"
    selectionQSDims = _.filter(selectionQS.split(';'))  # Example: ["mediaPartner:Vevo", "platform:roku,samsung"]

    if selectionQSDims.length > 0
      # Use the querystring to set up custom order as selected by the user
      filtersOrdered = []     # Start over

      for selectionQSDimFrag in selectionQSDims
        selectionQSDimFragSplit = selectionQSDimFrag.split(':')   # Example: ["platform", "roku,samsung"]
        selectionDimId = selectionQSDimFragSplit[0]               # Example: "platform"
        selectionDimVals = selectionQSDimFragSplit[1] || ''       # Example: "roku,samsung"

        if !selectionDimId
          continue

        # Get the selected filter object
        selectedFilter = _.find(filters, (d) -> return d.id == selectionDimId)

        # Set it to be selected
        selectedFilter.selected = true

        # Set all the values of the filter as selected accordingly
        for dimValueObj in selectedFilter.values
          dimValueObj.selected = selectionDimVals.indexOf(dimValueObj.id) >= 0

        filtersOrdered.push selectedFilter

      # Selected filters were pushed. Concatinate the unselected filters too
      filtersOrdered = filtersOrdered.concat(_.filter(filters, (f) -> !f.selected))

    # Now, filtersOrdered represents a state based on api, filtered by role, and selected by the url querystring
    return filtersOrdered


  getFiltersUrlFragment: (filters) ->
    fragmentFilters = []

    for filter in filters
      continue if !filter.selected

      fragmentFilter = []

      for filterVal in filter.values
        if filterVal.selected
          fragmentFilter.push filterVal.id

      fragmentFilters.push "#{filter.id}:#{fragmentFilter.join(',')}"

    fragmentFiltersStr = fragmentFilters.join(';')

    return fragmentFiltersStr


  extractDimsFromFiltersUrlFragment: (fragmentFilters) ->
    dimensions = []
                                                              # Example: "mediaPartner:Vevo;platform:roku,samsung"
    filters = fragmentFilters.split(';')                      # Example: ["mediaPartner:Vevo", "platform:roku,samsung"]
    for filterVal in filters
      [dimension, filterSubFilters] = filterVal.split(':')    # Example: ["mediaPartner", "Vevo"]

      if dimension
        dimensions.push(dimension)                            # Example: "mediaPartner"

    return dimensions.join(';')                               # Example: "mediaPartner;platform"
