module.exports = React.createClass
  displayName: 'Loader'
  
  propTypes:
    loaded: React.PropTypes.bool

  
  getDefaultProps: ->
    return {
      delay: 0 # In seconds
      loaded: false
    }


  getInitialState: ->
    return {waited: false}


  componentDidMount: ->
    @state.waitedTimeout = setTimeout( =>
      @setState({waited: true})
    , @props.delay * 1000)

  componentWillUnmount: ->
    clearTimeout @state.waitedTimeout

  render: ->
    ret = null
    if !@props.loaded && @state.waited
      ret = (
        <div className="spinner">
          <div className="bounce1"></div>
          <div className="bounce2"></div>
          <div className="bounce3"></div>
        </div>
      )

    return ret

