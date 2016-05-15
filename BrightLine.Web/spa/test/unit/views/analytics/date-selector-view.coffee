DateSelectorView = require 'areas/campaigns/analytics/views/date-selector-view'

dateFormat = 'YYYY-MM-DD'


getDates = (bd, ed, now) ->
  now = now || moment()
  dates =
    now: now
    nows: now.format(dateFormat)
    bd: bd
    ed: ed
    bds: bd.format(dateFormat)
    eds: ed.format(dateFormat)

  return dates

getSummary = (dates) ->
  # Mock summary api for a campaign ending in the future
  summary =
    beginDate: dates.bd.toDate().toString()
    endDate: dates.ed.toDate().toString()
    databaseDate: dates.now.toDate().toString()
    
  return summary

module.exports =

  describe 'date-selector-view', ->
    
    describe 'getSelections()', -> 

      dates = getDates(moment().add(-2, 'months'), moment().add(2, 'months'))
      describe "when campaign is running (bd = #{dates.bds}, ed = #{dates.eds}, now: #{dates.nows}):", ->
        summary = getSummary(dates)        
        selections = DateSelectorView::getSelections(summary)

        it "should return 'yesterday'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'yesterday').length > 0
          hasSelection.should.equal true

        it "should return 'last 7 days'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'last7days').length > 0
          hasSelection.should.equal true

        it "should return 'last 30 days'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'last30days').length > 0
          hasSelection.should.equal true

        it "should return 'last week'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'lastweek').length > 0
          hasSelection.should.equal true

        it "should return 'last month'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'lastmonth').length > 0
          hasSelection.should.equal true

        it "should return 'current week'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'currentweek').length > 0
          hasSelection.should.equal true

        it "should return 'current month'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'currentmonth').length > 0 
          hasSelection.should.equal true

        it "should return 'full campaign'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'fullcampaign').length > 0
          hasSelection.should.equal true

     
      dates = getDates(moment().add(-35, 'days'), moment().add(-1, 'days'))
      describe "when campaign has ended (bd = #{dates.bds}, ed = #{dates.eds}, now: #{dates.nows}):", ->        
        summary = getSummary(dates)
        selections = DateSelectorView::getSelections(summary)

        it "should NOT return'yesterday'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'yesterday').length > 0
          hasSelection.should.not.equal true

        it "should NOT return 'last 7 days'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'last7days').length > 0
          hasSelection.should.not.equal true

        it "should NOT return 'last 30 days'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'last30days').length > 0
          hasSelection.should.not.equal true
          
        it "should NOT return 'last week'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'lastweek').length > 0
          hasSelection.should.not.equal true
          
        it "should NOT return 'last month'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'lastmonth').length > 0
          hasSelection.should.not.equal true
          
        it "should NOT return 'current week'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'currentweek').length > 0            
          hasSelection.should.not.equal true

        it "should NOT return 'current month'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'currentmonth').length > 0 
          hasSelection.should.not.equal true
          
        it "should return 'full campaign'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'fullcampaign').length > 0
          hasSelection.should.equal true


      dates = getDates(moment('2015-03-31', dateFormat), moment('2015-05-31', dateFormat), moment('2015-04-28', dateFormat))
      describe "when considering Clinique (bd = #{dates.bds}, ed = #{dates.eds}, now: #{dates.nows} (tuesday)):", ->        
        summary = getSummary(dates)
        selections = DateSelectorView::getSelections(summary)

        it "should return 'yesterday'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'yesterday').length > 0
          hasSelection.should.equal true

        it "should return 'last 7 days'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'last7days').length > 0
          hasSelection.should.equal true

        it "should return 'last 30 days'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'last30days').length > 0
          hasSelection.should.equal true

        it "should return 'last week'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'lastweek').length > 0
          hasSelection.should.equal true

        it "should return 'last month'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'lastmonth').length > 0
          hasSelection.should.equal true

        it "should return 'current week'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'currentweek').length > 0            
          hasSelection.should.equal true

        it "should return 'current month'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'currentmonth').length > 0            
          hasSelection.should.equal true

        it "should return 'full campaign'", ->
          hasSelection = _.where(selections, (s) -> return s.id == 'fullcampaign').length > 0
          hasSelection.should.equal true


