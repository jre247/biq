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
var loadTime = config.loadTime();
var launchUrl = "";
var campaignId = 242;
var resource_i_594_516 = "";

module.exports = {
	before : function(browser) {
		console.log('Setting up...');
		destinationCreativeCreateUrl = browser.launch_url + "campaigns/" + campaignId + "/creatives/destinations/create/step/1";
		console.log('destinationCreativeCreateUrl: ' + destinationCreativeCreateUrl);
		creativeListingUrl = browser.launch_url + "campaigns/" + campaignId + "/creatives";
		console.log('creativeListingUrl: ' + creativeListingUrl);
		
		resource_i_594_516 = require('path').resolve(__dirname + '/resources/i-594-516.png');
		console.log('resource_i_594_516: ' + resource_i_594_516);
		//login
		launchUrl = browser.launch_url;
		loginService(browser, launchUrl)
		
		console.log('__dirname: ' + __dirname);
    },
	'create Video Feature' : function (browser) {		
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
		.setValue("input[name='PageName18'", "Apple Page 2")
		.setValue("input[name='Name'", creativeName1)
		
		//save
		.click('.btn-success')
	},
	'test Creating a Video Description Model Instance (step 1: arrange)' : function (browser) {		
		browser	
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath()
		.click("//a[text()='" + creativeName1 + "']")
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)

		 .waitForElementVisible(".model9", loadTime)
		 .click('.model9')
		 .waitForElementVisible(".model-instance-create", loadTime)
		 .click(".model-instance-create")
		
		.waitForElementVisible("input[name='name']", loadTime)
		.setValue("input[name='name']", "name test 1")
		//.setValue('.resource-file', __dirname + '\\resources\\i-594-516.png')
		.setValue('.resource-file', resource_i_594_516)

		//wait for image dimensions to display which means image is ready to be uploaded
		//.waitForElementVisible(".image-dimensions", loadTime)
		
		//save
		.waitForElementVisible(".save", loadTime)
		.click(".save")
	},
	'test Video Description Model Instance (step 2: arrange)' : function (browser) {		
		browser	
		.waitForElementVisible(creativesListing, loadTime)
		
		//click new destination creative item in listing
		.useXpath()
		.click("//a[text()='" + creativeName1 + "']")
		.useCss()
		.waitForElementVisible('#dest-name', loadTime)
		
		//click on Video Description model
		.waitForElementVisible(".model9", loadTime)
		.click('.model9')

		//edit Video Description model instance that was just created
		.waitForElementVisible('.instances-listing-item a', loadTime) 
		.click('.instances-listing-item a')
		
		//wait for model instance to be loaded
		.waitForElementVisible("input[name='name']", loadTime)
		
		//assert model instance values
		 .assert.containsText("input[name='name']", "name test 1")
		 
		.end();
	}
	
	
};
