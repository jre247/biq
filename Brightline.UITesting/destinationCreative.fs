module destinationCreative

open canopy
open runner
open System

let guid = Guid.NewGuid().ToString()

let creativesListingLoaded () = 
  (elements "#destinations-items").Length > 0
  //(elements "#destinations-items").Displayed)

let creativeLoaded () = 
  (elements "#ddFeaturesAdd").Length > 0

let all() =
    context "destination creative"

    "test deleting feature removes feature, before saving" &&& fun _ ->    
        url urlHelper.destinationCreativeCreateUrl
        waitFor creativeLoaded

        click "#ddFeaturesAdd" //add feature
        click ".ddFeaturesAddItem:first" //select Video feature
        click ".feature-remove" //delete feature

        //make sure the feature html is gone
        notDisplayed ".feature-fields-container"

    "test deleting feature among a list of features will remove the correct feature, before saving" &&& fun _ ->    
        url urlHelper.destinationCreativeCreateUrl
        waitFor creativeLoaded

        //add Calculator Style feature
        click "#ddFeaturesAdd" 
        click ".ddFeaturesAddItem:first" 

        //add Combination feature
        click "#ddFeaturesAdd" 
        click ".ddFeaturesAddItem:eq(1)" 

        //add Email Entry feature
        click "#ddFeaturesAdd" 
        click ".ddFeaturesAddItem:eq(2)"

        //remove Calculator Style feature
        click (first ".feature-remove")

        //make sure the Calculator Style feature was deleted
        count "#features-and-blueprints .row" 2 //make sure there are only 2 features now
        (element "#features-and-blueprints .row:eq(0) .featureName").Text == "Combination"
        (element "#features-and-blueprints .row:eq(1) .featureName").Text == "Email Entry"

    "test creating a destination creative with only name and description" &&& fun _ ->    
        url urlHelper.destinationCreativeCreateUrl
        waitFor creativeLoaded

        ".form-group:first input" << guid
        "textarea[name='Description']" << "grapefruit is a fruit" 

        click ".btn-success"

      //  waitFor creativesListingLoaded

        (element ".updated .col-name a").Text == guid

    "test existing creative has correct name and description" &&& fun _ ->    
        url urlHelper.creativeListingUrl

        click ".gridlist-item:last a"

        "input[name='Name']" == guid
        "textarea[name='Description']" == "grapefruit is a fruit" 

    "test new creative cannot use a name that exists for another creative" &&& fun _ ->    
        url urlHelper.destinationCreativeCreateUrl
        waitFor creativeLoaded
        ".form-group:first input" << guid
        sleep 2
        "textarea[name='Description']" << "grapefruit is a fruit" 

        contains "Please enter an unique destination name" (first ".error").Text 

    "test Calculator feature has correct properties" &&& fun _ ->    
        let featureName = "Feature 1"
        let quizPageName = "Apple Page"
        let resultPageName = "Banana Page"

        url urlHelper.destinationCreativeCreateUrl
        waitFor creativeLoaded
        click "#ddFeaturesAdd"
        click ".ddFeaturesAddItem:first"

        //fill in feature pages
        ".featureName-custom:first" << featureName
        "input[name='PageName1'" << quizPageName
        "input[name='PageName2'" << resultPageName

        //fill in name field
        let newGuid = Guid.NewGuid().ToString()
        "input[name='Name']" << newGuid

        //save
        click ".save"

        sleep 1
        waitFor creativesListingLoaded

        //make sure new creative appears in destination creative list
        (element ".updated .col-name a").Text == newGuid

        //go to edit destination creative screen
        click ".gridlist-item:last a"

        //validate Calculator feature properties
        (element ".fieldAsSpan:first").Text == featureName
        (element ".feature-page-form:first > span").Text == quizPageName
        (element ".feature-page-form:nth-child(2) > span").Text == resultPageName

    "test Combination feature has correct properties" &&& fun _ ->    
        let featureName = "Feature 2"
        let galleryPageName = "Apple Page"

        url urlHelper.destinationCreativeCreateUrl
        waitFor creativeLoaded
        click "#ddFeaturesAdd" //add feature
        click ".ddFeaturesAddItem:eq(1)" //select Combination feature
        click ".blueprint:eq(2)" //select Information blueprint

        //fill in feature pages
        ".featureName-custom:first" << featureName
        "input[name='PageName6'" << galleryPageName

        //fill in name field
        let newGuid = Guid.NewGuid().ToString()
        "input[name='Name']" << newGuid

        //save
        click ".save"

        waitFor creativesListingLoaded

        //make sure new creative appears in destination creative list
        (element ".updated .col-name a").Text == newGuid

        //go to edit destination creative screen
        click ".gridlist-item:last a"

        //validate Calculator feature properties
        (element ".fieldAsSpan:first").Text == featureName
        (element ".feature-page-form:first > span").Text == galleryPageName

    "test Image feature has correct properties" &&& fun _ ->    
        let featureName = "Feature 3"
        let galleryPageName = "Apple Page"

        url urlHelper.destinationCreativeCreateUrl
        waitFor creativeLoaded
        click "#ddFeaturesAdd" //add feature
        click ".ddFeaturesAddItem:eq(4)" //select Image feature

        //fill in feature  pages
        ".featureName-custom:first" << featureName
        "input[name='PageName11'" << galleryPageName

        //fill in name field
        let newGuid = Guid.NewGuid().ToString()
        "input[name='Name']" << newGuid

        //save
        click ".save"

        waitFor creativesListingLoaded

        //make sure new creative appears in destination creative list
        (element ".updated .col-name a").Text == newGuid

        //go to edit destination creative screen
        click ".gridlist-item:last a"

        //validate Calculator feature properties
        (element ".fieldAsSpan:first").Text == featureName
        (element ".feature-page-form:first > span").Text == galleryPageName

    "test Questionaire feature has correct properties" &&& fun _ ->    
        let featureName = "Feature 3"
        let introPageName = "Apple Page"
        let exitPageName = "Banana Page"
        let thankyouPageName = "Blueberries Page"

        url urlHelper.destinationCreativeCreateUrl
        waitFor creativeLoaded
        click "#ddFeaturesAdd" //add feature
        click ".ddFeaturesAddItem:eq(6)" //select Image feature

        //fill in feature  pages
        ".featureName-custom:first" << featureName
        "input[name='PageName7'" << introPageName
        "input[name='PageName8'" << exitPageName
        "input[name='PageName9'" << thankyouPageName

        //fill in name field
        let newGuid = Guid.NewGuid().ToString()
        "input[name='Name']" << newGuid

        //save
        click ".save"

        waitFor creativesListingLoaded

        //make sure new creative appears in destination creative list
        (element ".updated .col-name a").Text == newGuid

        //go to edit destination creative screen
        click ".gridlist-item:last a"

        //validate Calculator feature properties
        (element ".fieldAsSpan:first").Text == featureName
        (element ".feature-page-form:first > span").Text == introPageName
        (element ".feature-page-form:eq(1) > span").Text == exitPageName
        (element ".feature-page-form:eq(2) > span").Text == thankyouPageName

    "test Video feature has correct properties" &&& fun _ ->    
        let featureName = "Feature 3"
        let galleryPageName = "Apple Page"

        url urlHelper.destinationCreativeCreateUrl
        waitFor creativeLoaded
        click "#ddFeaturesAdd" //add feature
        click ".ddFeaturesAddItem:eq(7)" //select Image feature
        click ".blueprint:eq(1)" //select Info Video Gallery blueprint

        //fill in feature  pages
        ".featureName-custom:first" << featureName
        "input[name='PageName10']" << galleryPageName

        //fill in name field
        let newGuid = Guid.NewGuid().ToString()
        "input[name='Name']" << newGuid

        //save
        click ".save"

        waitFor creativesListingLoaded

        //make sure new creative appears in destination creative list
        (element ".updated .col-name a").Text == newGuid

        //go to edit destination creative screen
        click ".gridlist-item:last a"

        //validate Calculator feature properties
        (element ".fieldAsSpan:first").Text == featureName
        (element ".feature-page-form:first > span").Text == galleryPageName

    "test model instances are empty for Description model for Video feature" &&& fun _ ->    
        url urlHelper.creativeListingUrl

        //go to edit destination creative screen for video creative
        click ".gridlist-item:last a"

        //click on Description model
        click ".models-listing-item-name:first"

        (element ".models-or-instances-container .title").Text == "There are no Instances in this Model."

    "test model instances create errors if header image not supplied for Description model for Video feature" &&& fun _ ->    
        url urlHelper.creativeListingUrl

        //go to edit destination creative screen for video creative
        click ".gridlist-item:last a"

        //click on Description model
        click ".models-listing-item-name:first"

        //create instance
        click ".instance-create"

        //fill in name field
        "input[name='name']" << "name test 1"

        click ".save"

        (element ".validation-summary-errors > ul > li").Text == "Header Image is required"

    "test switching between new creative feature's pages displays correct page fields" &&& fun _ ->    
        url urlHelper.destinationCreativeCreateUrl
        waitFor creativeLoaded
        click "#ddFeaturesAdd" //add feature
        click ".ddFeaturesAddItem:last" //select Video feature
        click ".blueprint:eq(1)" //select Info Video Gallery blueprint
        displayed "input[name='PageName10']" 

        //now select other blueprint and make sure Info Video Gallery's page is not displayed
        click ".blueprint:first" 
        notDisplayed "input[name='PageName10']" 

        click ".blueprint:eq(1)" //select Info Video Gallery blueprint
        displayed "input[name='PageName10']" 

    "test switching between new creative feature's pages clears page field value after switching back" &&& fun _ ->    
        url urlHelper.destinationCreativeCreateUrl
        waitFor creativeLoaded
        click "#ddFeaturesAdd" //add feature
        click ".ddFeaturesAddItem:last" //select Video feature
        click ".blueprint:eq(1)" //select Info Video Gallery blueprint
        "input[name='PageName10']" << "Test 123" // add value to Gallery page field

        //now select other blueprint and then come back to Info Video Gallery blueprint to make sure the Gallery page field value is empty
        click ".blueprint:first" 
        click ".blueprint:eq(1)" 
        "input[name='PageName10']" == ""