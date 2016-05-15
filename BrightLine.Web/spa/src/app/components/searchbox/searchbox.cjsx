module.exports = class SearchBox extends React.Component

  @defaultProps =
    classNameSearchField: ''
    classNameSearchClear: ''
    searchPlaceholder: ''
    searchValue: ''
    delay: 300

  constructor: ->
    super

    @state =
      searchValue: @props.searchValue


  onSearch: (e) =>
    searchValue = e.target.value

    @setState
      searchValue: searchValue

    obj =
      searchText: searchValue
      searchBox: @
      event: e

    # Clear the previous @searchTimeout, if there's one in queue.
    clearTimeout(@searchTimeout)
    
    # Add the current(most recent) one to the queue.
    @searchTimeout = setTimeout( =>
      @props.onSearch(searchValue, obj)
    , @props.delay)


  onClear: (e) =>
    searchValue = ''

    @setState
      searchValue: searchValue

    obj =
      searchText: ''
      searchBox: @
      event: null

    @props.onSearch(searchValue, obj)


  componentDidMount: ->
    @refs.searchInput.focus()

  render: ->
    return (
      <div className={cs({
          'input-group input-group-simple component-searchbox': true
        })}>
        <div className={cs({
            "input-group-addon " : true
            "glyphicon glyphicon-search"        : @state.searchValue.length == 0
            "fa fa-times-circle" : @state.searchValue.length > 0
          })} onClick={@onClear} ref='searchInput'>
        </div>
        <input type="text"
          className="#{@props.classNameSearchField} form-control"
          placeholder={@props.searchPlaceholder}
          value={@state.searchValue}
          {if @props.autofocusDisabled then '' else 'autofocus'}
          onChange={@onSearch}
        />
      </div>
    )
