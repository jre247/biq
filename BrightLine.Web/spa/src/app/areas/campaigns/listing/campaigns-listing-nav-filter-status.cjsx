module.exports = React.createClass
  displayName: 'StatusFilterView'

  render: ->
    statuses = _.map(['Upcoming', 'Delivering', 'Completed'], (status) -> 
      return {value: status, label: status}
    )

    return (
      <div className="status-filter" >
        <div  className="status-filter-select-container">
          <Select options={statuses} value={@props.value} searchable={false} placeholder='Status' onChange={@props.onChange}/>
        </div>
      </div>
    )
