SearchBox = require 'components/searchbox/searchbox'

module.exports = GenericListingNav = React.createClass
  displayName: 'GenericListingNav'

  propTypes:
    title: React.PropTypes.string.isRequired #'Promotional Creatives'

    gridlistifierEnabled: React.PropTypes.bool.isRequired # true
    gridlistState: React.PropTypes.string # @state.gridlistState
    onToggleGridList: React.PropTypes.func # @onToggleGridList

    searchboxEnabled: React.PropTypes.bool.isRequired # true
    searchValue: React.PropTypes.string # @state.filterSearchTerm
    searchPlaceholder: React.PropTypes.string # 'Search Promotional Creatives...'
    onSearch: React.PropTypes.func # @onSearch

    createEnabled: React.PropTypes.bool.isRequired # true
    btnCreateClass: React.PropTypes.string # 'btn-create-promotional btn btn-success'
    btnCreateHref: React.PropTypes.string # "/campaigns/#{@props.campaignId}/creatives/promotionals/create"
    btnCreateTitle: React.PropTypes.string # 'New Promotional'

    collapserEnabled: React.PropTypes.bool.isRequired
    collapsableState: React.PropTypes.bool
    onToggleCollapser: React.PropTypes.func


  componentDidMount: ->
    $el = $(ReactDOM.findDOMNode(this))
    $el.find("[data-toggle='tooltip']").tooltip()


  onToggleGridList: (gridlistState) ->
    @props.onToggleGridList(gridlistState) if @props.onToggleGridList


  onToggleCollapser: ->
    nextCollapsableState = !@props.collapsableState
    @props.onToggleCollapser(nextCollapsableState) if @props.onToggleCollapser


  renderSearch: ->
    return null if !@props.searchboxEnabled

    searchboxProps =
      autoFocus: true
      searchValue: @props.searchValue
      searchPlaceholder: @props.searchPlaceholder
      classNameSearchClear: 'campaigns-filter-clear'
      classNameSearchField: 'campaigns-filter'
      onSearch: @props.onSearch

    return <SearchBox {...searchboxProps}/>


  renderCreate: ->
    return null if !@props.createEnabled

    return <a href={@props.btnCreateHref} className={@props.btnCreateClass}>{@props.btnCreateTitle}</a>


  renderGridlistifier: ->
    return null if !@props.gridlistifierEnabled

    gridlistState = @props.gridlistState

    return (
      <div className="gridlistifier">
        <a className={cs({
          'fa fa-th-large activate-grid': true
          'active': gridlistState == 'grid'
          'inactive': gridlistState == 'list'
          })}
          title="#{if gridlistState == 'grid' then '' else 'Switch to Grid view'}"
          onClick={@onToggleGridList.bind(this, 'grid')}
          data-toggle="tooltip" data-placement="top"
          ></a>

        <a className={cs({
          'fa fa-th-list activate-list': true
          'active': gridlistState == 'list'
          'inactive': gridlistState == 'grid'
          })}
          title="#{if gridlistState == 'list' then '' else 'Switch to List view'}"
          onClick={@onToggleGridList.bind(this, 'list')}
          data-toggle="tooltip" data-placement="top"
          ></a>
      </div>
    )


  renderCollapser: ->
    collapsableState = @props.collapsableState

    return (
      <a className={cs({
        "glyphicon collapser": true
        "glyphicon-chevron-up": !collapsableState
        "glyphicon-chevron-down": collapsableState
        })} 
        onClick={@onToggleCollapser}
        data-toggle="tooltip" data-placement="top"
        ></a>
    )

  render: ->
    return (
      <div className="row listing-nav nav">
        <div className="col-sm-3 listing-nav-title title">{@props.title}</div>
        <div className="col-sm-5 listing-nav-search-container">
          {@renderSearch()}
        </div>
        <div className="col-sm-4 listing-nav-actions-container">
          {@renderCreate()}
          {@props.customLinks}
          {@renderGridlistifier()}
          {@renderCollapser()}
        </div>
      </div>

    )
