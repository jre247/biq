var loginService = require('../utilities/login.js');
var config = require('../utilities/config.js');
var helper = require('../utilities/helper.js');

var destinationCreativeCreateUrl = "";
var creativeListingUrl = "";
var destinationCreativeListingItem = ".updated .col-name a";
var addFeatureButton = '#ddFeaturesAdd';
var creativesListing = "#campaign-creatives";
var createVideoModelInstanceUrl = "";
var createVideoDescriptionModelInstanceUrl = "";
var featureId = "";
var creativeId = "";
var creativeName1 = helper.guid();
var creativeName2 = helper.guid();
var creativeName3 = helper.guid();
var creativeName4 = helper.guid();
var creativeName5 = helper.guid();
var creativeName6 = helper.guid();
var creativeName7 = helper.guid();
var creativeName8 = helper.guid();
var creativeName9 = helper.guid();
var creativeName10 = helper.guid();
var creativeName11 = helper.guid();
var creativeName12 = helper.guid();
var creativeName13 = helper.guid();
var creativeName14 = helper.guid();
var loadTime = config.loadTime();
var launchUrl = "";
var campaignId = 242;

module.exports = {
	before : function(browser) {
		console.log('Setting up...');
		destinationCreativeCreateUrl = browser.launch_url + "campaigns/" + campaignId + "/creatives/destinations/create/step/1";
		console.log('destinationCreativeCreateUrl: ' + destinationCreativeCreateUrl);
		creativeListingUrl = browser.launch_url + "campaigns/" + campaignId + "/creatives";
		console.log('creativeListingUrl: ' + creativeListingUrl);

		//login
		launchUrl = browser.launch_url;
		loginService(browser, launchUrl)
    },
	'test deleting feature removes feature, before saving' : function (browser) {	
		browser		
		.url(destinationCreativeCreateUrl)	
		.waitForElementVisible(addFeatureButton, loadTime)
		
		//add calculator style feature and then delete it
		.click(addFeatureButton)
		.click('a[data-id="10019"]')
		.click('.feature-remove')
		
		.pause(1000)
		
		//assert feature has been deleted
		.assert.elementNotPresent('.feature-fields-container')
	},
   'test deleting feature among a list of features will remove the correct feature, before saving' : function (browser) {	
		browser		
		.url(destinationCreativeCreateUrl)	
		.waitForElementVisible(addFeatureButton, loadTime)
		
		//add combination feature
		.click(addFeatureButton)
		.click('a[data-id="10004"]') 
		
		//add email entry feature
		.click(addFeatureButton)
		.click('a[data-id="10008"]') 
		
		//add calculator style feature
		.click(addFeatureButton)
		.click('a[data-id="10019"]') 
		
		//delete calculator style feature
		.click('div[data-featuretype-id="10004"] .feature-remove')
		
		.pause(1000)
		
		//assert calculator style feature has been deleted
		.assert.elementNotPresent('div[data-featuretype-id="10004"]')
	},
   'test creating a destination creative with only name and description' : function (browser) {	
		browser		
		.url(destinationCreativeCreateUrl)	
		.waitForElementVisible(addFeatureButton, loadTime)
		
		//set name and description
		.setValue('input[name="Name"]', creativeName1)
		.setValue('textarea[name="Description"]', "grapefruit is a fruit")
		
		//save
		.click('.btn-success')
		
		.waitForElementVisible(creativesListing, loadTime)
		.assert.containsText(destinationCreativeListingItem, creativeName1)
	},
   'test existing creative has correct name and description' : function (browser) {	
		browser			
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath() // every selector now must be xpath
		.click("//a[text()='" + creativeName1 + "']")
		

		//set name and description
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)
		.assert.containsText(".creativeName", creativeName1)
		.assert.containsText('.creativeDescription', "grapefruit is a fruit")
	},
   'test new creative cannot use a name that exists for another creative' : function (browser) {	
		browser		
		.url(destinationCreativeCreateUrl)		
		.waitForElementVisible(addFeatureButton, loadTime)
		
		.setValue('input[name="Name"]', creativeName1)
		.pause(1000)
		.setValue('textarea[name="Description"]', "grapefruit is an awesome fruit")

		.waitForElementVisible('.error', loadTime)
		.assert.containsText(".error", "Please enter an unique destination name")
	},
   'test Calculator feature has correct properties (step 1: arrange)' : function (browser) {		
		browser		
		.url(destinationCreativeCreateUrl)		
		.waitForElementVisible(addFeatureButton, loadTime)
		
		//add Calculator feature
		.click(addFeatureButton)
		.click('a[data-id="10019"]') 

		//set feature properties
		.setValue('.featureName-custom', "Feature 1")
		.setValue("input[name='PageName1'", "Apple Page")
		.setValue("input[name='PageName2'", "Banana Page")
		.setValue("input[name='Name'", creativeName3)
		
		//save
		.click('.btn-success')
	},
   'test Calculator feature has correct properties (step 2: assert)' : function (browser) {		
		browser
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath()
		.click("//a[text()='" + creativeName3 + "']")
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)
		
		//assert feature properties
		.assert.containsText(".featureNameValue", "Feature 1")
		.assert.containsText('span[data-page-definition-id="1"]', "Apple Page")
		.assert.containsText('span[data-page-definition-id="2"]', "Banana Page")
	},
   'test Combination feature has correct properties (step 1: arrange)' : function (browser) {		
		browser		
		.url(destinationCreativeCreateUrl)		
		.waitForElementVisible(addFeatureButton, loadTime)
		
		//add Combination feature
		.click(addFeatureButton)
		.click('a[data-id="10004"]') 
	
		//select Information blueprint
		.click('div[data-blueprint-id="7"]')
		
		//set feature properties
		.setValue('.featureName-custom', "Feature 2")
		.setValue("input[name='PageName6'", "Apple Page")
		.setValue("input[name='Name'", creativeName4)
		
		//save
		.click('.btn-success')
	},
   'test Combination feature has correct properties (step 2: assert)' : function (browser) {		
		browser
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath()
		.click("//a[text()='" + creativeName4 + "']")
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)
		
		//assert feature properties
		.assert.containsText(".featureNameValue", "Feature 2")
		.assert.containsText('span[data-page-definition-id="6"]', "Apple Page")
	},
   'test Image feature has correct properties (step 1: arrange)' : function (browser) {		
		browser		
		.url(destinationCreativeCreateUrl)		
		.waitForElementVisible(addFeatureButton, loadTime)
		
		//add Image feature
		.click(addFeatureButton)
		.click('a[data-id="10002"]') 

		//set feature properties
		.setValue('.featureName-custom', "Feature 1")
		.setValue("input[name='PageName11'", "Apple Page")
		.setValue("input[name='PageName26'", "Apple Page 2")
		.setValue("input[name='Name'", creativeName5)
		
		//save
		.click('.btn-success')
	},
   'test Image feature has correct properties (step 2: assert)' : function (browser) {		
		browser
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath()
		.click("//a[text()='" + creativeName5 + "']")
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)
		
		//assert feature properties
		.assert.containsText(".featureNameValue", "Feature 1")
		.assert.containsText('span[data-page-definition-id="11"]', "Apple Page")
		.assert.containsText('span[data-page-definition-id="26"]', "Apple Page 2")
	},
   'test Questionaire feature has correct properties (step 1: arrange)' : function (browser) {		
		browser		
		.url(destinationCreativeCreateUrl)		
		.waitForElementVisible(addFeatureButton, loadTime)
		
		//add Questionaire feature
		.click(addFeatureButton)
		.click('a[data-id="10024"]') 

		//set feature properties
		.setValue('.featureName-custom', "Feature 1")
		.setValue("input[name='PageName7'", "Apple Page")
		.setValue("input[name='PageName8'", "Banana Page")
		.setValue("input[name='PageName9'", "Bananananaa Page")
		.setValue("input[name='Name'", creativeName6)
		
		//save
		.click('.btn-success')
	},
   'test Questionaire feature has correct properties (step 2: assert)' : function (browser) {		
		browser
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath()
		.click("//a[text()='" + creativeName6 + "']")
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)
		
		//assert feature properties
		.assert.containsText(".featureNameValue", "Feature 1")
		.assert.containsText('span[data-page-definition-id="7"]', "Apple Page")
		.assert.containsText('span[data-page-definition-id="8"]', "Banana Page")
		.assert.containsText('span[data-page-definition-id="9"]', "Bananananaa Page")
	},
	'test Video feature has correct properties (step 1: arrange)' : function (browser) {		
		browser		
		.url(destinationCreativeCreateUrl)		
		.waitForElementVisible(addFeatureButton, loadTime)
		
		//add Video feature
		.click(addFeatureButton)
		.click('a[data-id="10001"]') 

		//select Info Video Gallery blueprint
		.click('div[data-blueprint-id="18"]')
		
		//set feature properties
		.setValue('.featureName-custom', "Feature 1")
		.setValue("input[name='PageName10'", "Apple Page")
		.setValue("input[name='PageName25'", "Apple Page 2")
		.setValue("input[name='Name'", creativeName7)
		
		//save
		.click('.btn-success')
	},
   'test Video feature has correct properties (step 2: assert)' : function (browser) {		
		browser	
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath()
		.click("//a[text()='" + creativeName7 + "']")
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)
		
		//assert feature properties
		.assert.containsText(".featureNameValue", "Feature 1")
		.assert.containsText('span[data-page-definition-id="10"]', "Apple Page")
		.assert.containsText('span[data-page-definition-id="25"]', "Apple Page 2")
	},
	'test switching between new creative feature pages displays correct page fields' : function (browser) {
		browser
		.url(destinationCreativeCreateUrl)		
		.waitForElementVisible(addFeatureButton, loadTime)
		
		//add Video feature
		.click(addFeatureButton)
		.click('a[data-id="10001"]') 

		//select Info Video Gallery blueprint
		.click('div[data-blueprint-id="18"]')
		
		//set page name
		.setValue("input[name='PageName10'", "Apple Page")
		
		//now select other blueprint and make sure Info Video Gallery's page is not displayed
        .click('div[data-blueprint-id="21"]')
		
        //select Info Video Gallery blueprint and make sure its page is displayed
		.click('div[data-blueprint-id="18"]')
		.waitForElementVisible("input[name='PageName10']", loadTime)
	},
	'test switching between new creative feature pages clears page field value after switching back' : function (browser) {
		browser
		.url(destinationCreativeCreateUrl)		
		.waitForElementVisible(addFeatureButton, loadTime)
		
		//add Video feature
		.click(addFeatureButton)
		.click('a[data-id="10001"]') 

		//select Info Video Gallery blueprint
		.click('div[data-blueprint-id="18"]')
		
		//set page name
		.setValue("input[name='PageName10'", "Apple Page")
		
		//now select other blueprint and make sure Info Video Gallery's page is not displayed
        .click('div[data-blueprint-id="21"]')
 
        //select Info Video Gallery blueprint and make sure its page is displayed
		.click('div[data-blueprint-id="18"]')
		.waitForElementVisible("input[name='PageName10']", loadTime)
        .getValue("input[name='PageName10'", function(result){
			console.log(result.value);
			this.assert.equal(result.value, "");
		});
	},
	'test Video Description model instance create errors if Description Image is not supplied' : function (browser) {		
		browser	
		.url(creativeListingUrl)	
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath()
		.click("//a[text()='" + creativeName7 + "']")
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)

		 .waitForElementVisible(".model9", loadTime)
		 .click('.model9')
		 .waitForElementVisible(".model-instance-create", loadTime)
		 .click(".model-instance-create")
		
		.waitForElementVisible("input[name='name']", loadTime)
		.setValue("input[name='name']", "name test 1")

		//save
		.waitForElementVisible(".save", loadTime)
		.click(".save")
		
		//assert
		.waitForElementVisible('.validation-error', loadTime)
		.assert.containsText('.validation-error', "Description Image is required")
	},
	'test Video Description models display (step 1: arrange)' : function (browser) {		
		browser		
		.url(destinationCreativeCreateUrl)		
		.waitForElementVisible(addFeatureButton, loadTime)
		
		//add Video feature
		.click(addFeatureButton)
		.click('a[data-id="10001"]') 

		//select Info Video Gallery blueprint
		.click('div[data-blueprint-id="18"]')
		
		//set feature properties
		.setValue('.featureName-custom', "Feature 1")
		.setValue("input[name='PageName10'", "Apple Page")
		.setValue("input[name='PageName25'", "Apple Page 2")
		.setValue("input[name='Name'", creativeName8)
		
		//save
		.click('.btn-success')
	},
	'test Video Description models display (step 2: assert)' : function (browser) {		
		browser	
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath()
		.click("//a[text()='" + creativeName8 + "']")
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)
		
		//assert Video model shows
		.waitForElementVisible('.model9', loadTime)
		.waitForElementVisible('.model10', loadTime)
	},
	'test Image Description models display (step 1: arrange)' : function (browser) {		
		browser		
		.url(destinationCreativeCreateUrl)		
		.waitForElementVisible(addFeatureButton, loadTime)
		
		.click(addFeatureButton)
		.click('a[data-id="10002"]') 

		//set feature properties
		.setValue('.featureName-custom', "Feature 1")
		.setValue("input[name='PageName11'", "Apple Page")
		.setValue("input[name='PageName26'", "Apple Page 2")
		.setValue("input[name='Name'", creativeName9)
		
		//save
		.click('.btn-success')
	},
	'test Image models display (step 2: assert)' : function (browser) {		
		browser	
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath()
		.click("//a[text()='" + creativeName9 + "']")
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)
		
		//assert Video model shows
		.waitForElementVisible('.model11', loadTime)
	},
	'test Combination models display (step 1: arrange)' : function (browser) {		
		browser		
		.url(destinationCreativeCreateUrl)		
		.waitForElementVisible(addFeatureButton, loadTime)
		
		.click(addFeatureButton)
		.click('a[data-id="10004"]') 

		//set feature properties
		.setValue('.featureName-custom', "Feature 1")
		.setValue("input[name='PageName23'", "Apple Page")
		.setValue("input[name='PageName24'", "Apple Page 2")
		.setValue("input[name='Name'", creativeName10)
		
		//save
		.click('.btn-success')
	},
	'test Combination models display (step 2: assert)' : function (browser) {		
		browser	
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath()
		.click("//a[text()='" + creativeName10 + "']")
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)
		
		//assert Combination models show
		.waitForElementVisible('.model5', loadTime)
		.waitForElementVisible('.model6', loadTime)
		.waitForElementVisible('.model7', loadTime)
		.waitForElementVisible('.model8', loadTime)
	},
	'test Email Entry models display (step 1: arrange)' : function (browser) {		
		browser		
		.url(destinationCreativeCreateUrl)		
		.waitForElementVisible(addFeatureButton, loadTime)
		
		.click(addFeatureButton)
		.click('a[data-id="10008"]') 

		//set feature properties
		.setValue('.featureName-custom', "Feature 1")
		.setValue("input[name='Name'", creativeName11)
		.setValue("input[name='PageName13'", "Apple Page")
		.setValue("input[name='PageName14'", "Apple Page 2")
		
		//save
		.click('.btn-success')
	},
	'test Email Entry models display (step 2: assert)' : function (browser) {		
		browser	
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath()
		.click("//a[text()='" + creativeName11 + "']")
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)
		
		//assert Combination models show
		.waitForElementVisible('.model16', loadTime)
	},
	'test Framed model display (step 1: arrange)' : function (browser) {		
		browser		
		.url(destinationCreativeCreateUrl)		
		.waitForElementVisible(addFeatureButton, loadTime)
		
		.click(addFeatureButton)
		.click('a[data-id="10017"]') 

		//set feature properties
		.setValue('.featureName-custom', "Feature 1")
		.setValue("input[name='PageName29'", "Apple Page")
		.setValue("input[name='PageName30'", "Apple Page 2")
		.setValue("input[name='Name'", creativeName12)
		
		//save
		.click('.btn-success')
	},
	'test Framed model display (step 2: assert)' : function (browser) {		
		browser	
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath()
		.click("//a[text()='" + creativeName12 + "']")
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)
		
		//assert Combination models show
		.waitForElementVisible('.model18', loadTime)
	},
	'test Calculator Style model display (step 1: arrange)' : function (browser) {		
		browser		
		.url(destinationCreativeCreateUrl)		
		.waitForElementVisible(addFeatureButton, loadTime)
		
		.click(addFeatureButton)
		.click('a[data-id="10019"]') 

		//set feature properties
		.setValue('.featureName-custom', "Feature 1")
		.setValue("input[name='Name'", creativeName13)
		.setValue("input[name='PageName1'", "Apple Page")
		.setValue("input[name='PageName2'", "Apple Page 2")
		
		//save
		.click('.btn-success')
	},
	'test Calculator Style model display (step 2: assert)' : function (browser) {		
		browser	
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath()
		.click("//a[text()='" + creativeName13 + "']")
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)
		
		//assert Calculator Style models show
		.waitForElementVisible('.model1', loadTime)
		.waitForElementVisible('.model2', loadTime)
		.waitForElementVisible('.model3', loadTime)
	},
	'test Questionaire models display (step 1: arrange)' : function (browser) {		
		browser		
		.url(destinationCreativeCreateUrl)		
		.waitForElementVisible(addFeatureButton, loadTime)
		
		.click(addFeatureButton)
		.click('a[data-id="10024"]') 

		//set feature properties
		.setValue('.featureName-custom', "Feature 1")
		.setValue("input[name='Name'", creativeName14)
		.setValue("input[name='PageName7'", "Apple Page")
		.setValue("input[name='PageName8'", "Apple Page 2")
		.setValue("input[name='PageName9'", "Apple Page 3")
		
		//save
		.click('.btn-success')
	},
	'test Questionaire models display (step 2: assert)' : function (browser) {		
		browser	
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath()
		.click("//a[text()='" + creativeName14 + "']")
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)
		
		//assert Calculator Style models show
		.waitForElementVisible('.model14', loadTime)
		.waitForElementVisible('.model15', loadTime)
		
		.end();
	}
	
};
