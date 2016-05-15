SelectOptionResolver  = require './components/field-select/select-option-resolver'
SelectValueResolver   = require './components/field-select/select-value-resolver'
SelectOptionDefault   = require './components/field-select/select-option-default'

module.exports = 
  formatters:
    number:
      general:
        formatterPre: (value) ->
          return value if value == '' || value == '.' || !value
          numeral().unformat(value)
        formatterPost: (value) ->
          return value if value == '' || value == '.' || !value
          numeral(value).format('0,0')
    select:
      getItemsFromLookup: (lookupEntities, optionsCustom) ->
        optionsDefault =
          clone: true
          sort: true
        options = _.merge(optionsDefault, optionsCustom)

        transformed = lookupEntities

        transformed = _.cloneDeep(transformed) if options.clone

        transformed = _.map(transformed, (entity) -> 
          entity.value = entity.id
          entity.label = entity.name
          return entity
        )

        transformed = _.sortBy(transformed, (entity) -> entity.name.toLowerCase()) if options.sort
        
        return transformed
          
      SelectOptionResolver: SelectOptionResolver
      SelectValueResolver:  SelectValueResolver
      
      SelectOptionDefault:  SelectOptionResolver(SelectOptionDefault)
      SelectValueDefault:   SelectValueResolver(SelectOptionDefault)

  misc:
    resource:
      image: 
        getDimensions: (file) ->
          # Returns a promise containing a dimensions object
          deferred = RSVP.defer()
          img = new Image()
          img.onload = ->
            fileWidth = this.width
            fileHeight = this.height

            deferred.resolve
              width: fileWidth
              height: fileHeight

          img.src = window.URL.createObjectURL(file)

          return deferred.promise
      video:
        getMetadata: (file) ->
          # Returns a promise containing a metadata object
          deferred = RSVP.defer()

          video = document.createElement('video')
          video.preload = 'metadata'
          video.onloadedmetadata = ->
            URL = window.URL || window.webkitURL
            URL.revokeObjectURL(this.src)

            metadata = 
              duration: parseInt(video.duration)
              width: parseInt(video.videoWidth)
              height: parseInt(video.videoHeight)

            deferred.resolve(metadata)

          video.src = URL.createObjectURL(file)

          return deferred.promise
