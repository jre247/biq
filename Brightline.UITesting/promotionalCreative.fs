module promotionalCreative

open canopy
open runner
open System

let guid1 = Guid.NewGuid().ToString()
let guid2 = Guid.NewGuid().ToString()

let creativesListingLoaded () = 
  (elements "#promotionals-items").Length > 0
  //(elements "#destinations-items").Displayed)

let all() =
    context "promotional creative"

    "test creating a promotional creative with empty fields causes name validation error" &&& fun _ ->    
        url urlHelper.promotionalCreativeCreateUrl

        click ".btn-success"

        (element ".validation-summary-errors > ul > li").Text == "Name is required"

    "test creating a promotional creative will save" &&& fun _ ->    
        url urlHelper.promotionalCreativeCreateUrl

        "input[name='Name']" << guid1
        "select[name='AdType']" << "10003"
        "select[name='AdFunction']" << "4"

        click ".btn-success"

        waitFor creativesListingLoaded


    "test promotional creative field values persisted after save" &&& fun _ ->    
        url urlHelper.creativeListingUrl
        waitFor creativesListingLoaded

        //go to most recent promotional creative
        click urlHelper.recentPromotionalCreative

        //validate field values
        "input[name='Name']" == guid1
        "select[name='AdType']" == "Video Banner"
        "select[name='AdFunction']" == "Click to Jump"

    "test editing a promotional creative" &&& fun _ ->    
        url urlHelper.creativeListingUrl
        waitFor creativesListingLoaded

        //go to most recent promotional creative
        click urlHelper.recentPromotionalCreative 

        "input[name='Name']" << guid2
        "select[name='AdType']" << "10006"
        "select[name='AdFunction']" << "6"

        click ".btn-success"

        waitFor creativesListingLoaded

    "test promotional creative field values persisted after save" &&& fun _ ->    
        url urlHelper.creativeListingUrl
        waitFor creativesListingLoaded

        //go to most recent promotional creative
        click urlHelper.recentPromotionalCreative

        //validate field values
        "input[name='Name']" == guid2
        "select[name='AdType']" == "Video Skin"
        "select[name='AdFunction']" == "On Demand"



    "test duplicate creative name" &&& fun _ ->    
        url urlHelper.promotionalCreativeCreateUrl

        //fill in name field that is duplicate name
        "input[name='Name']" << guid2

        sleep 1

        //try saving
        click ".btn-success"

        sleep 1

        contains "Please enter an unique creative name." (element ".error div div").Text 