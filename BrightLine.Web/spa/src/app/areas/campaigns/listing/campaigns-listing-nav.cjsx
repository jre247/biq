SearchBox = require 'components/searchbox/searchbox'
StatusFilter = require './campaigns-listing-nav-filter-status'

module.exports = React.createClass
  displayName: 'CampaignsListingNav'
  propTypes:
    lengthInfo: React.PropTypes.object.isRequired
    userSettings: React.PropTypes.object.isRequired
    onToggleSorts: React.PropTypes.func.isRequired
    onToggleFavorites: React.PropTypes.func.isRequired
    onToggleGridList: React.PropTypes.func.isRequired
    onToggleStatus: React.PropTypes.func.isRequired
    onSearch: React.PropTypes.func.isRequired


  componentDidMount: ->
    $el = $(ReactDOM.findDOMNode(this))
    $el.find("[data-toggle='tooltip']").tooltip()


  onToggleSorts: (sortOption, i) ->
    @props.onToggleSorts(sortOption, i)


  onToggleFavorites: ->
    @props.onToggleFavorites()


  onToggleGridList: (switchToState) ->
    @props.onToggleGridList(switchToState) if @props.onToggleGridList


  onToggleStatus: (status) ->
    @props.onToggleStatus(status) if @props.onToggleStatus


  renderCreateCampaignButton: ->
    return null if utility.user.isnt(['Admin', 'Developer', 'AdOps'])

    return (
      <div className="nav-campaigns-create inline ">
        <a className="nav-campaigns-create-btn btn btn-success" href="/campaigns/create">New Campaign</a>
      </div>
    )


  render:->
    lengthInfo = @props.lengthInfo
    userSettings = @props.userSettings
    sortOptions = userSettings.sortOptions

    searchboxProps = 
      autoFocus: true
      searchValue: userSettings.filterSearchTerm
      searchPlaceholder: 'Search Campaigns...'
      classNameSearchClear: 'campaigns-filter-clear'
      classNameSearchField: 'campaigns-filter'
      onSearch: @props.onSearch

    return (
      <div id="listing-nav">
        <div className="row">


          <div className="nav-campaigns-left pull-left col-sm-6">
            <div className="nav-campaigns-search inline">
              <SearchBox {...searchboxProps}/>
            </div>
            <div className="nav-campaigns-length inline">Showing {lengthInfo.displaying} of {lengthInfo.total}</div>
          </div>
          <div className="nav-campaigns-right pull-right col-sm-6">
            <div className="nav-campaigns-sorts inline">    
              <button className={cs({
                  'btn btn-filter-favorites': true
                  'starred': userSettings.filterFavoritesActive
                  'unstarred': !userSettings.filterFavoritesActive
                })} 
                onClick={@onToggleFavorites}>
                <i className="glyphicon glyphicon-star"></i>
                <span>Favorites</span>
              </button>
              &nbsp;&nbsp;&nbsp;
              
              {_.map(sortOptions, (sortOption, index) =>
                return (
                  <button key={index} className={cs({'btn btn-sort': true, 'btn-sort-active': sortOption.active})} 
                    onClick={@onToggleSorts.bind(this, sortOption, index)}>
                    <span>{sortOption.name}</span>

                    <i className={cs({
                      'glyphicon': true
                      'glyphicon-chevron-down': sortOption.reverse
                      'glyphicon-chevron-up': !sortOption.reverse
                      'hide': !sortOption.active
                    })} ></i>
                  </button> 
                )
              )}

            </div>

            <div id="status-filter-container">
              <StatusFilter onChange={@props.onToggleStatus} value={userSettings.filterStatus} />
            </div>

            <div className="nav-campaigns-actions inline">
              <div className="gridlistifier">
                <a className={cs({
                  'fa fa-th-large activate-grid': true
                  'active': userSettings.gridlist == 'grid'
                  'inactive': userSettings.gridlist == 'list'
                  })}  
                  title="#{if userSettings.gridlist == 'grid' then 'Showing in Grid view' else 'Switch to Grid view'}"
                  onClick={@onToggleGridList.bind(this, 'grid')}
                  data-toggle="tooltip" data-placement="top"
                  ></a>

                <a className={cs({
                  'fa fa-th-list activate-list': true
                  'active': userSettings.gridlist == 'list'
                  'inactive': userSettings.gridlist == 'grid'
                  })}  
                  title="#{if userSettings.gridlist == 'list' then 'Showing in List view' else 'Switch to List view'}"
                  onClick={@onToggleGridList.bind(this, 'list')}
                  data-toggle="tooltip" data-placement="top"
                  ></a>  
              </div>
            </div>

            {@renderCreateCampaignButton()}

          </div>

        </div>
      </div>
    )

