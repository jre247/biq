module campaign

open canopy
open runner



let all() =
    context "Campaign"

    "test campaign create with blank fields causing validation errors" &&& fun _ ->    
        url urlHelper.campaignCreateUrl
        
        click "#submitBtn"

        contains (element ".validation-summary-errors ul > li:first").Text "Name is required."
        contains (element ".validation-summary-errors ul > li:eq(1)").Text "Agency must be selected."
        contains (element ".validation-summary-errors ul > li:eq(2)").Text "Product must be selected."
        contains (element ".validation-summary-errors ul > li:eq(3)").Text "Campaign type must be selected."

    "test campaign create saves correctly" &&& fun _ ->    
        url urlHelper.campaignCreateUrl

        "#Name" << "Campaign Test 1"
        "#Agency" << "2"
        "#Product" << "2"
        check "#CampaignType_1"

        click "#submitBtn"

    "test campaign has correct field values" &&& fun _ ->    
        //edit campaign button
        click "#campaign-nav li:last a"

        "#Name" == "Campaign Test 1"
        "#Agency" == "Carat-NY"
        "#Product" == "American Express Cards"
        selected "#CampaignType_1"

        click "#submitBtn"

    "test campaign edit saves correctly" &&& fun _ ->    
        //edit campaign button
        click "#campaign-nav li:last a"

        "#Name" << "Campaign Test 1 b"
        "#Agency" << "3"
        "#Product" << "1"
        check "#CampaignType_2"

        click "#submitBtn"

    "test campaign has correct field values" &&& fun _ ->    
        //edit campaign button
        click "#campaign-nav li:last a"

        "#Name" == "Campaign Test 1 b"

        "#Agency" == "Carat-DET"
        "#Product" == "Abreva Medicated Cream"
        selected "#CampaignType_2"

        click "#submitBtn"
