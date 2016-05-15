ListingNav = require './listing-nav'

module.exports = React.createClass
  mixins: [React.addons.PureRenderMixin]
  propTypes:
    campaignId: React.PropTypes.any

    filterSearchTerm: React.PropTypes.string.isRequired
    gridlistState: React.PropTypes.string #['list', 'grid'][0]
    collapsableState: React.PropTypes.bool.isRequired #false

    listingItemType: React.PropTypes.string.isRequired #'Promotional Creatives'
    idContainer: React.PropTypes.string.isRequired #'campaign-creatives-promotionals-listing-container'
    idListing: React.PropTypes.string.isRequired # 'campaign-creatives-promotionals-listing'
    idListingItems: React.PropTypes.string.isRequired # 'promotionals-items'
    searchableProperties: React.PropTypes.array.isRequired # ['name', 'adTypeName']
    ListingItemView: React.PropTypes.func.isRequired #require './creatives-listing-item-promotional'

    navOptions: React.PropTypes.object.isRequired
      ### navOptions should have the following
      title: 'Promotional Creatives'

      gridlistifierEnabled: true
      gridlistState: @state.gridlistState
      onToggleGridList: @onToggleGridList

      searchboxEnabled: true
      searchValue: @state.filterSearchTerm
      searchPlaceholder: 'Search Promotional Creatives...'
      onSearch: @onSearch

      collapsableState: @state.collapsableState
      onToggleCollapser: @onToggleCollapser

      createEnabled: true
      btnCreateClass: 'btn-create-promotional btn btn-success'
      btnCreateHref: "/campaigns/#{@props.campaignId}/creatives/promotionals/create"
      btnCreateTitle: 'New Promotional'
      ###







  getItemsFiltrate: ->
    searchableProperties = @props.searchableProperties
    filterSearchTerm = @props.filterSearchTerm

    return _.filter(@props.items, (item) ->
      for prop in searchableProperties
        propVal = item[prop]
        if propVal != null && propVal.toString().toLowerCase().indexOf(filterSearchTerm) != -1
          return true

      return false
    )


  renderListingNav: ->
    return <ListingNav {...@props.navOptions}/>


  render: ->
    campaignId = @props.campaignId
    itemsFiltrate = @getItemsFiltrate()
    collapsableState = @props.collapsableState
    gridlistState = @props.gridlistState
    ListingItemView = @props.ListingItemView
    self = @
    
    return (
      <div id={@props.idListingContainer}>
        <div id={@props.idListing} className={cs({
          "campaign-generic-listing container-fluid collapsable-container whitebox": true
          "collapsed": collapsableState
          })} >
          <div className='listing-nav-container container-fluid'>
            {@renderListingNav()}
          </div>
          <div className='listing-nav-sub-container container-fluid'>

          </div>
          <div className='listing-items-container'>
            <div className={cs({
                "collection_empty collection_empty_original collapsable-item": true
                "hide": self.props.items.length > 0
              })}>
              <p className="title">There are no {@props.listingItemType} in this Campaign</p>
            </div>
            <div className={cs({
                "collection_empty collection_empty_filtrate collapsable-item ": true
                "hide": !(self.props.items.length > 0 && itemsFiltrate.length == 0)
              })} >
              <p className="title">0 {@props.listingItemType} were found</p>
              <p className="desc">Try using a broader search term</p>
            </div>
            <div id={@props.idListingItems} className="overlay-items gridlist-items collapsable-item container-fluid">
              {_.map(itemsFiltrate, (item) ->
                return <ListingItemView campaignId={campaignId} item={item} gridlistState={gridlistState} key={item.id}/>
              )}
            </div>
          </div>
        </div>
      </div>
    )
