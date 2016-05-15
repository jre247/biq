AnalyticsHelper = require 'areas/campaigns/analytics/helpers/campaign-analytics-helper'

module.exports = 
  describe 'campaign-analytics-helper', ->
    describe 'redirectInfo()', ->

      describe "campaign doesn't have analytics", ->

        paramsInfo = 
          url: ''
        summary =
          id: 123
          hasAnalytics: false
        
        redirectInfo = AnalyticsHelper.redirectInfo(paramsInfo, summary)

        it 'should redirect', ->
          (redirectInfo.redirect).should.equal true

        it 'should redirect to "/campaigns/123/creatives"', ->
          (redirectInfo.url).should.equal '/campaigns/123/creatives'


      describe 'when no parameters are provided',  ->

        paramsInfo = 
          url: '/campaigns/20198/analytics'
          action: 'overview'
          interval: null
        summary =
          id: 20198
          hasAnalytics: true
          beginDate: "02/16/2015 12:00:00 AM"
          endDate: "03/02/2015 12:00:00 AM"
          timeInterval: "Day"

        redirectInfo = AnalyticsHelper.redirectInfo(paramsInfo, summary) 

        it 'should redirect', ->
          (redirectInfo.redirect).should.equal true

        expectedResult = "/campaigns/20198/analytics?bd1=20150216&ed1=20150302&int=day&m1=4&m2=5"
        it "should redirect to '#{expectedResult}'", ->
          (redirectInfo.url).should.equal expectedResult


      describe "when parameters are provided, and url dates fall outside campaign's range",  ->
        paramsInfo = 
          url: '/campaigns/20198/analytics?bd1=20150216&ed1=20150302&int=day&m1=4&m2=5'
          action: 'overview'
          interval: 'day'
          bd1: '20150216'
          ed1: '20150302'
          m1: 4
          m2: 5
        summary =
          id: 20198
          hasAnalytics: true
          beginDate: "02/17/2015 12:00:00 AM"
          endDate: "02/20/2015 12:00:00 AM"
          timeInterval: "Day"

        redirectInfo = AnalyticsHelper.redirectInfo(paramsInfo, summary)

        it 'should redirect', ->
          (redirectInfo.redirect).should.equal true

        expectedResult = "/campaigns/20198/analytics?bd1=20150217&ed1=20150220&int=day&m1=4&m2=5"
        it "should redirect to '#{expectedResult}'", ->
          (redirectInfo.url).should.equal expectedResult


      describe "should limit endDate to be yesterday, at most",  ->
        dateFormat = 'YYYYMMDD'
        yesterday = moment().add(-1, 'days')
        yesterdayString = yesterday.format(dateFormat)

        paramsInfo = 
          url: '/campaigns/20198/analytics?bd1=20150216&ed1=20150302&int=day&m1=4&m2=5'
          action: 'overview'
          interval: 'day'
          bd1: '20150216'
          ed1: yesterday.add(5, 'days').format(dateFormat)
          m1: 4
          m2: 5
        summary =
          id: 20198
          hasAnalytics: true
          beginDate: "02/17/2015 12:00:00 AM"
          endDate: "02/20/2016 12:00:00 AM"
          timeInterval: "Day"

        redirectInfo = AnalyticsHelper.redirectInfo(paramsInfo, summary)

        it 'should redirect', ->
          (redirectInfo.redirect).should.equal true

        expectedResult = "/campaigns/20198/analytics?bd1=20150217&ed1=#{yesterdayString}&int=day&m1=4&m2=5"
        it "should redirect to '#{expectedResult}'", ->
          (redirectInfo.url).should.equal expectedResult
