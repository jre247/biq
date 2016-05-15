###
 Allows a general way to resolve APIs on the client and cache them on the browser.
###

services = {}

# Set silent to true to prevent Service's log messages from polluting the console, when debugging other things.
silent = false

# The Cacher class is primarily responsible for fetching items from the server, and caching them. 
#  It has the actual code for invalidating an item, and any parent invalidation dependencies recursively.
#  Use the Cache class(further down) instead to route all requests through Cache.getFresh (which invalidates after a custom cacheDuration).

services.Cacher = class Cacher
  options: {}

  # A hash of all added services.
  #  In order to preserve all added Services when creating several instances of Cacher, the services and invalidatesList is on the prototype, and act like singletons.
  services: {}

  # A hash of an *inverted*, cache dependency tree. 
  # A more granular cache invalidates a less granular cache upon its own invalidation
  # Example:
  #   Campaign.Destination.get, upon its invalidation, should invalidate its parent, Campaign.Summary
  invalidatesList: {}

  constructor: ->
    # A store of all cacheditems' cachekeys. {serviceName: [array of cacheKeys]}
    # This will be used to clear out all cachekeys in a certain serviceName
    #  Example: {'serviceName': ['cacheKey1', 'cacheKey2', ..., 'cacheKeyN']}
    @keysCached = {}


    # This is where all the cached items are stored, against cacheKey
    @cachedItems = {}

  # Register a service in the list.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
  #  service: {name: '', method: ->, invalidates: ['',''] }
  addService: (service) ->
    service.invalidates = service.invalidates || []

    serviceName = service.name

    @services[serviceName] = service

    @invalidatesList[serviceName] = service.invalidates

  getCacheKey: (serviceName, args...) ->
      argsString = args.join(",")
      cacheKey = "#{serviceName}(#{argsString})"


  getFresh: (serviceName, args...) ->
    service = @services[serviceName]
    cacheKey = @getCacheKey(serviceName, args)

    deferred = RSVP.defer()

    # Exit gracefully if a service was not found
    if typeof service == 'undefined'

      do logger.error "No service was found with name: #{serviceName}. CacheKey: ", cacheKey if !silent
      deferred.resolve {}
      return deferred.promise


    service.method.apply(this, args)
    .then (fetchedJSON) =>
      cachedItem = @set(serviceName, cacheKey, fetchedJSON)
      
      # Output to do logger.
      do logger.info '[Not cached]', cachedItem if !silent

      deferred.resolve cachedItem
      
    return deferred.promise
    

  set: (serviceName, cacheKey, item) ->
    service = @services[serviceName]

    # Wrap the item, and include helpful references (Useful for debugging, invalidating, etc)
    cachedItem = 
      item: item
      key: cacheKey
      service: service


    # Store it in the cachedItems hash.
    @cachedItems[cacheKey] = cachedItem 

    # Add this cacheKey to the list of active cacheKeys for a service
    @keysCached[serviceName] = @keysCached[serviceName] || []
    @keysCached[serviceName].push(cacheKey)

    return cachedItem


  _invalidateParents: (service) ->

    # This recursively invalidates items in 'parent' services, found in this service's invalidates [].

    # loop through parent services found in this service's invalidates [].
    for serviceNameToInvalidate in service.invalidates
      # Invalidate cached items found in this service.
      if typeof @keysCached[serviceNameToInvalidate] != 'undefined'
        for cacheKey in @keysCached[serviceNameToInvalidate]
          @_invalidateItem(@cachedItems[cacheKey])

      # Next, recursively invalidate parents.
      @_invalidateParents(@services[serviceNameToInvalidate])

  _invalidateItem: (cachedItem) ->

    # Exit if cached item is already invalidated
    if typeof cachedItem == 'undefined'
      do logger.info '[Cached item already invalid]' if !silent
      return

    cacheKey = cachedItem.key

    # Log invalidation to console
    do logger.info '[  Clearing]', cacheKey if !silent

    # Remove reference to cachedItem from cachedItems
    delete @cachedItems[cacheKey]

    # Remove the key from keysCached
    keysCachedOfThisService = @keysCached[cachedItem.service.name]
    if keysCachedOfThisService
      keysCachedIndex = keysCachedOfThisService.indexOf(cacheKey)
      if keysCachedIndex
        @keysCached[cachedItem.service.name].splice(keysCachedIndex, 1) 


  invalidateItemAndParents: (cachedItem) ->
    #Invalidate the item
    @_invalidateItem(cachedItem)

    #Invalidate its parents
    @_invalidateParents(cachedItem.service)

  getCachedItem: (cacheKey) ->
    return @cachedItems[cacheKey]

services.cacher = cacher = new services.Cacher


# Each instance of the Cache class can have different cacheDuration set. But they all use the same cache store
# Cache class wraps Cacher class. All api requests should go through instances of Cache, not Cacher
# Set up an uniform way to:
#  Get a resource from cache (get)
#  Get a resource without cache (getFresh)
#  Clear a resource  (clear)
#  Clear all resources (clearAll)
services.Cache = class Cache
  optionsDefault:
    #This instance's absolute expiration duration (in seconds), after which a cached item is invalidated.
    cacheDuration: 180    
  
  constructor: (optionsCustom) ->
    @options = _.extend({}, @optionsDefault, optionsCustom, true)

  # Concatenates a service name and arguments for a unique identifier
  #
  # @example Get a cache key for retrieving a user
  #   getCacheKey('UserService.get', '1')
  #
  # @param [String] name of service being called
  # @param [String] comma separated list of arguments as string
  #
  getCacheKey: (serviceName, args...) ->
      cacher.getCacheKey(serviceName, args)

  # Cache.get tries to retrieve a cachedItem. If the item is not cached, it defers to Cache.getFresh 
  get: (serviceName, args...) =>

    cacheKey = @getCacheKey(serviceName, args)
    deferred = RSVP.defer()

    cachedItem = cacher.cachedItems[cacheKey]
    if cachedItem?
      do logger.info '[    Cached]', cachedItem if !silent

      deferred.resolve(cachedItem.item)
      return deferred.promise
    else
      return @getFresh.apply(this, arguments)

  ###
    Allows resolving services into a hash store, described by the schema's keys. 
    This adds on to the functionality provided by RSVP.hash: https://github.com/tildeio/rsvp.js/#hash-of-promises
    @param {object} schema - Key represents the schema, and the value is the to-be-resolved service's params as an array.
      Key: "<key where service will resolve>" : [service params 1, 2, ...]
      Key: "<key where service will resolve>:<dot namespaced path of resolved object>": [service params 1, 2, ...]
    @example
      cache.hash
        summary: ['Campaigns.Summary', campaignId]
        'platforms:platforms': ['Campaigns.Platforms', campaignId]
        'm1:metrics.1': ['Campaigns.getLookups']
      .then (store) ->
        do logger.log store.summary, store.platforms, store.m1
        ...
  ###
  hash: (schema) ->
    deferred = RSVP.defer()

    promises = {}
    for key, args of schema
      promises[key] = @get.apply(@, args)

    RSVP.hash(promises)
    .then (results) ->
      store = {}

      for schemaKey, cachedData of results

        # Start off from the root of the object returned by the api
        cachedDataPathCursor = cachedData

        # Parse out the schemaKey
        schemaKeyFragments = schemaKey.split(':')

        # Take the first part of the key. This is the key where the desired cachedData object will be stored
        storeKey = schemaKeyFragments[0]

        # Take the second part of the key. This is possibly namespaced via dots(.), and represents the path to object in this current cachedData object that should be stored in the hash
        cachedDataPathRaw = schemaKeyFragments[1]

        if typeof cachedDataPathRaw != 'undefined'
          # Make an array out of the raw path above. Used to implement what's described above
          cachedDataPath = cachedDataPathRaw.split('.')

          # Try to traverse the object as described by the namespaced path, and use what's found there
          while cachedDataPath.length > 0 && cachedDataPath[0] != ''
            cachedDataPathNext = cachedDataPath.shift()

            # Update the cursor to represent the current fragment of the path
            cachedDataPathCursor = cachedDataPathCursor[cachedDataPathNext]

            if typeof cachedDataPathCursor == 'undefined'
              do logger.log "Invalid path given to cache.hash(). No value was found at path fragment (#{cachedDataPathNext}) in given path (#{cachedDataPathRaw}) in object: ", cachedDataPathCursor
              break

        store[storeKey] = cachedDataPathCursor

      deferred.resolve store
    return deferred.promise

  # This wraps Cacher.getFresh in order to automatically invalidate item after some time(based on this instance's cacheDuration).
  getFresh: (serviceName, args...) =>     
          
    deferred = RSVP.defer()
    
    cacher.getFresh.apply(cacher, arguments)
    .then (cachedItem) =>
      
      # Invalidate the item after cacheDuration (s).
      setTimeout =>
        cacher.invalidateItemAndParents(cachedItem)
      , @options.cacheDuration * 1000

      # Resolve the actual json object found in the cachedItem wrapper.
      deferred.resolve(cachedItem.item)

    return deferred.promise

  # The last value in args will be considered to be the item. eg: Cache.set(serviceName, p1, p2, ..., pn, item)
  set: (serviceName, args...) ->
    item = args[args.length-1]
    args = args.slice(0, args.length-1)

    cacheKey = @getCacheKey(serviceName, args)

    cachedItem = cacher.set(serviceName, cacheKey, item)

    # Output to console
    do logger.info '[   Setting]', cachedItem if !silent

  clear: (serviceName, args...) ->
    service = cacher.services[serviceName]
    
    # Use the same parameters that is used for @get
    cacheKey = @getCacheKey(serviceName, args)

    # Exit gracefully if a service was not found
    if typeof service == 'undefined'
      do logger.error "No service was found with name: #{serviceName}. CacheKey: ", cacheKey if !silent
      return


    # Get the cachedItem from the cacheKey
    cachedItem = cacher.getCachedItem(cacheKey)

    # Skip, if item isn't in cache
    if typeof cachedItem == 'undefined'
      do logger.info '[Cached item already invalid]', cacheKey if !silent
      return

    # Invalidate the item and its parents
    cacher.invalidateItemAndParents(cachedItem)

  getAll: ->
    return cacher.cachedItems

  clearAll: ->
    services.cacher = cacher = new services.Cacher


# The following Cache instance will be useful for most usecases, having the default cacheDuration. 
#  Eg: app.services.cache.get(...);
# If a different cacheDuration is preferred, create a new instance.
#  Eg: var cache = new app.services.Cache({cacheDuration: 5});
services.cacheShort = new services.Cache({cacheDuration: 10})
services.cache = new services.Cache()
services.cacheLong = new services.Cache({cacheDuration: 10000})

module.exports = services

