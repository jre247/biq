module ad

open canopy
open runner
open System

let mutable endDate = ""
let mutable beginDate = ""
let guid1 = Guid.NewGuid().ToString()
let guid2 = Guid.NewGuid().ToString()
let recentAd = ".updated a:eq(1)"
let adsListingLoaded () = 
  (elements "#ads-items .placement-container").Length > 0
  //(elements "#destinations-items").Displayed)

let all() =
    context "ad"

    "test creating an ad with empty fields causes name validation error" &&& fun _ ->    
        url urlHelper.adCreateUrl

        click ".btn-success"

        (element ".validation-summary-errors > ul > li:first").Text == "Name is required"
        (element ".validation-summary-errors > ul > li:eq(1)").Text == "Creative must be selected"

    "test creating an ad with name field but empty creative selection causes creative selection validation error" &&& fun _ ->    
        url urlHelper.adCreateUrl

        "input[name='Name']" << guid1

        click ".btn-success"

        (element ".validation-summary-errors > ul > li:eq(0)").Text == "Creative must be selected"

    "test creating a valid ad saves" &&& fun _ ->    
        url urlHelper.adCreateUrl

        //fill in field values
        "input[name='Name']" << guid1
        "select[name='AdType']" << "10003"
        click (first ".creative")  //select first creative
        "select[name='AdFormat']" << "19"
        "select[name='Platform']" << "15"
        "select[name='Placement']" << "15"
        click ".date-selector" //open date selectors

        //set end date and begin date
        click ".datepickerFuture:not(.datepickerDisabled):last a"
        click ".datepickerFuture:not(.datepickerDisabled):first a"
        click ".date-picker-apply:last"
        endDate <- read ".date-selector:eq(1) span:eq(1)"
        beginDate <- read ".date-selector:first span:eq(1)"   

        //save
        click ".btn-success"

        waitFor adsListingLoaded

    "test ad field values are correct" &&& fun _ ->    
        click recentAd

        //fill in field values
        "input[name='Name']" == guid1
        "select[name='AdType']" == "Video Banner"
       // click (first ".creative")  //select first creative
        "select[name='AdFormat']" == ":60"
        "select[name='Platform']" == "Android"
        "select[name='Placement']" == "Samsung AdHub (2013)"
        contains beginDate (element ".date-selector:first span:eq(1)").Text  //validate begin date
        contains endDate (element ".date-selector:eq(1) span:eq(1)").Text  //validate end date

      //TODO: can't rely on "updated" class, need to find ad in listing by name (in this case a specific guid)
//    "test editing an ad will save" &&& fun _ ->    
//        url adListingUrl
//        click recentAd
//
//        //fill in field values
//        "input[name='Name']" << guid2
//        "select[name='AdType']" << "10002"
//        click (nth 1 ".creative")  //select first creative
//        "select[name='AdFormat']" << "17"
//        "select[name='Platform']" << "3"
//        "select[name='Placement']" << "2"
//
//        //save
//        click ".btn-success"
//
//        waitFor adsListingLoaded
//
//    "test ad field values are correct" &&& fun _ ->    
//        click recentAd
//
//        //fill in field values
//        "input[name='Name']" == guid2
//        "select[name='AdType']" == "Animated Banner"
//       // click (first ".creative")  //select first creative
//        "select[name='AdFormat']" == ":15"
//        "select[name='Platform']" == "AT&T U-Verse"
//        "select[name='Placement']" == "CNBC Horizontal Banner"

    