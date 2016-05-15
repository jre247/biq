#http://stackoverflow.com/questions/3336316/how-to-access-line-numbers-when-wrapping-firebug-or-similar-console-api
#http://jsfiddle.net/epQ95/1/
#using method 1. 
#usage:
# javascript:   logger.log(1,2,[3,4])()
# coffeescript: logger.log(1,2,[3,4])()
# coffeescript: do logger.log(1,2,[3,4])

window.logger = do ->
  noop= ->  

  _logger =  ->
    
  methods = ['log', 'debug', 'info', 'warn', 'error', 'exception', 'time', 'timeEnd', 'timeStamp', 'profile', 'profileEnd', 'trace',    
      'assert', 'clear', 'count', 'dir', 'dirxml', 'group', 'groupCollapsed', 'groupEnd', 'markTimeline', 'profile', 'profileEnd', 'table', 
  ]
  methodsLength = methods.length

  _logger::settings = 
    elmahUrl: '/Error/LogJavaScriptError'

    #querystring to override settings found in @settings.environments["currentEnv"].transportMapByRoles
    qsLogToConsole: 'logtoconsole'
    qsLogToElmah: 'logtoelmah' 
    environments: 
      "local": 
        transportMapsByRoles:
          #each role's (true/false based) toggles for different transports (console, or elmah)
          # [0]: logToConsole? , [1]: logToElmah?

          isDeveloper: 
            methodsTransportViaConsole: methods #everything possible
            methodsTransportViaElmah: ['warn','error']          
          isEveryoneElse: 
            methodsTransportViaConsole: []
            methodsTransportViaElmah: [] 
          #...
      "dev": 
        transportMapsByRoles:
          #each role's (true/false based) toggles for different transports (console, or elmah)
          # [0]: logToConsole? , [1]: logToElmah?

          isDeveloper: 
            methodsTransportViaConsole: methods #everything possible
            methodsTransportViaElmah: ['warn','error']          
          isEveryoneElse: 
            methodsTransportViaConsole: []
            methodsTransportViaElmah: [] 
          #...
      "uat": 
        transportMapsByRoles:
          isDeveloper: 
            methodsTransportViaConsole: ['warn', 'error']
            methodsTransportViaElmah: ['warn','error']          
          isEveryoneElse: 
            methodsTransportViaConsole: []
            methodsTransportViaElmah: ['warn','error']   
      "pro":  
        transportMapsByRoles:
          isDeveloper: 
            methodsTransportViaConsole: ['warn', 'error']
            methodsTransportViaElmah: ['warn','error']          
          isEveryoneElse: 
            methodsTransportViaConsole: []
            methodsTransportViaElmah: ['warn','error']  

  #Determine current environment
  _logger::initEnv = () ->    
    for envKey, envSetting  of @settings.environments
      if _bl.env == envKey
        @currentEnvName = envKey
        @currentEnv = envSetting
        break
    if !@currentEnv?
      console.error "No matching environment was found for Logger!"


  _logger::initEnvSettings = () ->
    user = _bl.user

    @currentTransportMapKey = ''  #this eventually contains the key for @currentEnv.transportMapsByRoles. example: 'isDeveloper', or 'isEveryoneElse'
    for keyUser of user
      for keyRoleTransportMap, roleMapObj of @currentEnv.transportMapsByRoles
        if keyUser == keyRoleTransportMap
          @currentTransportMapKey = keyUser #example: 'isDeveloper', or 'isAdmin', or 'isEmployee' 

          #role found. break, since the first matched role in the transportMapsByRoles has the highest priority
          break

      if @currentTransportMapKey != ''
        #role found. break out of user obj key loop
        break

    #if transportMapsByRoles contains none of the roles found in the user object, set the role to 'isEveryoneElse'
    if @currentTransportMapKey == ''
      @currentTransportMapKey = 'isEveryoneElse'

    @currentTransportMap = @currentEnv.transportMapsByRoles[@currentTransportMapKey]

  _logger::initEnvSettingsOverrides = () ->
    
    qs = window.location.search.toLowerCase()
    @qsOverrideForConsole = if (qs.indexOf(@settings.qsLogToConsole) != -1) then true else false
    @qsOverrideForElmah   = if (qs.indexOf(@settings.qsLogToElmah) != -1)   then true else false


  _logger::trackedEvents = []
  _logger::trackEvent = (eventCustom) -> 
    eventDefault = 
      url: [location.pathname, location.search].join('')
    event = _.extend({}, eventDefault, eventCustom)
    @trackedEvents.push(event)


  _logger::logToElmah = (method, args) ->
    method = '[' + method + ']'

    message = 
      meta: 
        type: method
      args: args
      url: [location.pathname, location.search].join('')
      pastEvents: _.chain(@trackedEvents).clone().reverse().take(3).reverse().value()

    messageStr = JSON.stringify message
    $.ajax type: 'POST', url: @settings.elmahUrl, data: { message : messageStr }

    
  initConsoleWrapping = () ->
    #Wrap all the console.methods here
    for method in methods
      do (method) ->
        _logger.prototype[method] = ->

          args = Array.prototype.slice.call arguments

          #Elmah related logging
          if @qsOverrideForElmah || _.contains(@currentTransportMap.methodsTransportViaElmah, method)
            @logToElmah(method, args)
            # do my extra special stuff

          # do regular console logging, if possible
          if @qsOverrideForConsole || _.contains(@currentTransportMap.methodsTransportViaConsole, method)
            if window.console and window.console[method]
              return window.console[method].apply.bind(window.console[method], window.console, args)
            else
              noop     
          else
            noop 
  
  initConsoleWrapping()
  _loggerObj = new _logger
  _loggerObj.initEnv()
  _loggerObj.initEnvSettings()
  _loggerObj.initEnvSettingsOverrides()

  return _loggerObj

#tests
# logger.log('Log', 2,3)()
# logger.warn('warn', 2,3)()
# logger.error('error', 2,3)()
