module.exports = FPSMixin = 
  fpsStart: ->
    if !@state.framesCounter
      @state.framesCounter = 1
      @state.framesCounterStart = new Date()

    setInterval =>
      @setState
        framesCounter: @state.framesCounter + 1
    , 0
      
  fpsStop: ->
    @setState
      framesCounter: 0
      framesCounterStart: undefined

  fpsRender: ->
    return null if !@state.framesCounterStart
    seconds = (new Date() - @state.framesCounterStart)/1000
    return (
      <div>{@state.framesCounter} frames -- {seconds} seconds -- {@state.framesCounter / seconds} FPS</div>
    )
