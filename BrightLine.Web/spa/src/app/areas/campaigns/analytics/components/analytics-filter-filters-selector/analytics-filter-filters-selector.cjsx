FiltersHelper = require './filters-helper'
FieldSelect       = require 'components/field/components/field-select'
FieldHelper       = require 'components/field/field-helper'

formatterSelect       = FieldHelper.formatters.select.getItemsFromLookup

module.exports = React.createClass
  displayName: 'AnalyticsFilterFiltersSelector'

  propTypes:
    lookups: React.PropTypes.object.isRequired
    adsData: React.PropTypes.object.isRequired
    deliveryGroups: React.PropTypes.object.isRequired
    filters: React.PropTypes.string.isRequired

  mixins: [React.addons.PureRenderMixin]

  getInitialState: ->
    lookups = @props.lookups
    mediaPartnersLookups = []

    _.each(@props.adsData.mediaPartners, (mediaPartnerId) ->
      mediaPartner = lookups.mediaPartners[mediaPartnerId]
      if (mediaPartner)
        mediaPartnersLookups.push mediaPartner
    )

    mediaPartners =
      id:     'mediapartner'
      name:   'Media Partner'
      values: formatterSelect(mediaPartnersLookups)

    platforms =
      id:     'platform'
      name:   'Platform'
      values: formatterSelect(_.map(@props.adsData.platforms, (platformId) -> lookups.platforms[platformId]))

    deliveryGroups =
      id:     'deliverygroup'
      name:   'Delivery Group'
      values: formatterSelect(@props.deliveryGroups)

    filtersOriginal = [
      mediaPartners
      platforms
      deliveryGroups
    ]

    filters = FiltersHelper.getInitialFilters(filtersOriginal)
    fragmentFiltersStr = @props.filters

    return {mediaPartners, platforms, deliveryGroups, filters, fragmentFiltersStr}


  onFilterSelectorAddition: ->
    @setState
      filterSelectorAdditionRequest: true


  onFilterSelectorRemoval: (filter) ->
    # Set it as unselected
    filter.selected = false

    # Set all of its values as unselected as well
    for filterValueObj in filter.values
      filterValueObj.selected = false

    @forceUpdate()


  onFilterSelectChange: (filter, selections) ->
    # First, unselect everything.
    for value in filter.values
      value.selected = false

    # Next, if a selection was made, set its selected property to tru
    if selections.length
      selections[0].selected = true

    # Rerender
    @forceUpdate()


  onFiltersApply: ->
    # Get the current url
    url = location.pathname + location.search

    # Add/Update the filters querystring
    url = utility.updateQueryString('filters', @state.fragmentFiltersStr, url)

    # Redirect
    page(url)


  renderFiltersSelectors: ->
    return (
      _.chain(@state.filters)
        .filter((filter) -> filter.selected)
        .map((filter, index) =>
          filterSelectorProps =
            onChange: @onFilterSelectChange.bind(@, filter)
            placeholder: "All #{filter.name}s"
            items: filter.values
            values: _.filter(filter.values, (v) -> v.selected)
            searchable: true
            clearable: true

          <div className="filter-selector" key={index}>
            <div className="filter-selector-select-container">
              <FieldSelect  {...filterSelectorProps} />
            </div>
            <div className={cs({
                "filter-selector-remover": true
                hide: !filter.isRemovable
              })}
              onClick={@onFilterSelectorRemoval.bind(@, filter)}>
              <i className="fa fa-times-circle"></i>
            </div>
          </div>
        )
        .value()
    )


  renderFilterSelectorAdder: (shouldRenderAdder) ->
    return null if !shouldRenderAdder

    filterSelectorAdderProps =
      placeholder: 'Select...'
      items: formatterSelect(_.filter(@state.filters, (d) -> !d.selected))
      onChange: (selections) =>
        return if selections.length == 0

        # Get the selected filter object
        filter = _.find(@state.filters, (d) -> d.id == selections[0].id)

        # Set it as selected
        filter.selected = true

        # Maintain past selected order: remove the filter from its current location and append it to the very end
        filterIndex = @state.filters.indexOf(filter)
        @state.filters.splice(filterIndex, 1)   # Remove it from the array
        @state.filters.push filter              # Add it back to the array at the end

        # Reset filterSelectorAdditionRequest
        @state.filterSelectorAdditionRequest = false

        # Rerender
        @forceUpdate()

    return (
      <FieldSelect  {...filterSelectorAdderProps} />
    )


  renderFilterSelectorAddControls: ->
    # Render a FilterSelectorAdder for either of the following cases:
    # 1. There are no FilterSelectors active
    # 2. User clicked the AddFilterSelector button
    shouldRenderAdder = @state.allFiltersAreInactive || @state.filterSelectorAdditionRequest
    shouldRenderAdderBtn = !shouldRenderAdder && @state.notAllFiltersAreActive

    <div id="filter-selector-add-container">
      <div id="filter-selector-add-btn" className={cs({hide: !shouldRenderAdderBtn})} onClick={@onFilterSelectorAddition}>
        <i className="fa fa-plus-circle" />
      </div>
      <div id="filter-selector-add-container">
        {@renderFilterSelectorAdder(shouldRenderAdder)}
      </div>
    </div>


  render: ->
    @state.fragmentFiltersStrPrev = @state.fragmentFiltersStr
    @state.fragmentFiltersStr = FiltersHelper.getFiltersUrlFragment(@state.filters)

    @state.numFiltersActive = _.filter(@state.filters, (d) -> d.selected).length
    @state.allFiltersAreInactive = @state.numFiltersActive == 0
    @state.notAllFiltersAreActive = @state.numFiltersActive < @state.filters.length

    return (
      <div id="filters-selector-and-controls">
        <div id="filters-selector-precontainer">
          <div id="filters-selectors">
            {@renderFiltersSelectors()}
          </div>
        </div>

        {@renderFilterSelectorAddControls()}

        <div id="filters-apply" onClick={@onFiltersApply} className={cs({
            hide: @state.fragmentFiltersStrPrev == @state.fragmentFiltersStr || @state.allFiltersAreInactive
          })} >
          <i className="fa fa-chevron-circle-right" />
        </div>
      </div>
    )
