  module.exports =

    # converts seconds to HH:MM:SS
    convertSecondsToTimeStamp: (seconds) ->
      milliseconds = seconds * 1000
      @convertMilliToTimeStamp(milliseconds)

    # converts milliseconds to HH:MM:SS
    convertMilliToTimeStamp: (duration) ->
      milliseconds = parseInt((duration % 1000) / 100)

      seconds = Math.ceil(parseFloat((duration / 1000) % 60))
      minutes = parseInt((duration / (1000 * 60)) % 60)

      # Want to avoid something like 02:60, which will instead be 02:59
      # This is basically the same thing as doing a Math.floor instead of Math.ceil above
      if seconds == 60
        seconds = 59

      hours = parseInt((duration / (1000 * 60 * 60)) % 24)
      hours = (if (hours < 10) then "0" + hours else hours)
      minutes = (if (minutes < 10) then "0" + minutes else minutes)
      seconds = (if (seconds < 10) then "0" + seconds else seconds)
      durationFormatted = ""
      if (hours > 0)
        durationFormatted = hours + ":"
      durationFormatted = durationFormatted + minutes + ":" + seconds
