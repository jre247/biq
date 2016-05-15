# This is found at https://enome.github.io/javascript/2014/05/09/lets-create-our-own-router-component-with-react-js.html
# A more natural, express-style router than ReactRouter
ErrorGeneral    = require 'components/error/error-general'
ErrorNotFound   = require 'components/error/error-not-found'

module.exports = Router = React.createClass
  displayName: 'Router'

  getInitialState: ->
    return {
      component: <div/>
    }


  componentDidMount: ->

    
    self = @

    # Expose onError to the App global variable
    App.onError = ->
      self.onError.apply(this, arguments)

    @props.sitemap.traverse((area) ->
      return if !area.isAllowedForUser()

      Component = area.handler
      return if !Component

      for path in area.paths
        # A reference to area of the current iteration is required inside the callback. Use an IEFE
        ((_area) ->
          page(path, (ctx, next) ->
            # Simply do a full page reload to reset the app if the app has been unmounted due to an error at the previous url
            if App.isUnmounted
              location.reload()
              return

            pathCurrent = ctx.path.toLowerCase()

            App.context = _.extend(ctx, 
              action: _area.action || ''
            )

            qsFrags = ctx.querystring.split('&')
            qsParsed = {}
            for qsFrag in qsFrags
              [key, value] = qsFrag.split('=')
              qsParsed[key] = value

            utility.promise.resolved()
            .then ->
              self.setState
                component: <Component params={ctx.params} qs={ctx.querystring} qsParsed={qsParsed} context={ctx} next={next}/> 
            .catch App.onError
          )
        )(area)
    )

    # Set up route roots for non-spa pages
    pathRootsForRazorViews = [
      '/campaigns/create'
      '/campaigns/edit'
      '/campaigns/history'
      '/cms'
      '/blueprints'
      '/admin'
      '/settings'
      '/developer'
      '/account'
    ]
    page('*', (ctx, next) ->
      pathCurrent = ctx.path.toLowerCase()
      pathIsForARazorView = _.filter(pathRootsForRazorViews, (pathRoot) ->
        return pathCurrent.indexOf(pathRoot) == 0
      ).length > 0

      if pathIsForARazorView
        do logger.log 'Redirecting to Razorview page'
        setTimeout ->
          window.location.href = ctx.path
        , 10
        return true
      else
        self.showNotFoundView()
    )
    
    setTimeout page.start, 10


  onError: ->
    # This callback is intended to be made available to all components for catching errors
    # Custom messages can be passed anywhere in the arguments
    # Usage: Call App.onError in the catch block of any promise.
    # Example: promise.then( -> ...).catch App.onError                            # arguments = [error]
    # Example: promise.then( -> ...).catch App.onError.bind("msg1", "msg2")       # arguments = ["msg1", "msg2", error]
    # Example: promise.then( -> ...).catch (e) -> App.onError(e, "msg1", "msg2")  # arguments = [error, "msg1", "msg2"]

    customMessages = []
    error = null
    _.each(arguments, (arg) ->
      if arg instanceof Error
        error = arg
      else
        customMessages.push arg
    )

    # Log the error
    do logger.error customMessages, error?.stack

    # Show the error view
    @showErrorView(error)


  showErrorView: (error) ->
    # When an error occurs within a React lifecycle method, ReactReconciler can't find the 
    #  proper instance from the mountNode to unmount the component from. 
    # Reset the mountNode, and mount the error view
    
    # Reset the mountNode
    mountNode = document.getElementById('app-wrapper')
    mountNode.innerHTML = '';

    # Set a flag so that the router does a full page reload on the hitting back button
    App.isUnmounted = true

    # Mount Error view
    ReactDOM.render(<ErrorGeneral />, mountNode)

  showNotFoundView: ->
    @setState
      component: <ErrorNotFound/>

  render: ->
    return @state.component
