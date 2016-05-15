var loginService = require('../utilities/login.js');
var config = require('../utilities/config.js');
var helper = require('../utilities/helper.js');

var promotionalCreativeCreateUrl = "";
var creativeListingUrl = ""
var promotionalCreativeListingItem = ".updated .col-name a";
var promotionalEditForm = '#campaign-content-container';
var creativesListing = "#campaign-creatives";
var creativeName1 = helper.guid();
var creativeName2 = helper.guid();
var loadTime = config.loadTime();
var campaignId = 242;

module.exports = {
	before : function(browser) {
		console.log('Setting up...');
		promotionalCreativeCreateUrl = browser.launch_url + "campaigns/" + campaignId + "/creatives/promotionals/create"
		creativeListingUrl = browser.launch_url + "campaigns/" + campaignId + "/creatives";

		//login
		var launchUrl = browser.launch_url;
		loginService(browser, launchUrl)
    },
	'test creating a promotional creative with empty fields causes name validation error' : function (browser) {	
		browser		
		.url(promotionalCreativeCreateUrl)	
		.waitForElementVisible(promotionalEditForm, loadTime)
		.waitForElementVisible(".btn-success", loadTime)
		.click(".btn-success")
		
		.assert.containsText(".validation-summary-errors", "Name is required")
	},
	'test creating a promotional creative will save' : function (browser) {	
		browser		
		.url(promotionalCreativeCreateUrl)	
		.waitForElementVisible(promotionalEditForm, loadTime)
				
		//fill in form values
		.setValue("input[name='Name']", creativeName1)
		.click("select[name='AdType'] option[value='10008']")		
		.click("select[name='AdFunction'] option[value='4']")

		.click(".btn-success")
	},
	'test promotional creative field values persisted after create' : function (browser) {	
		browser		
		.waitForElementVisible(creativesListing, loadTime)
		
		//click on the promotional creative just created
		.useXpath()
		.click("//a[text()='" + creativeName1 + "']")
		.useCss()
		
		//assert form values
		.waitForElementVisible("input[name='Name']", loadTime)
		.assert.value("input[name='Name']", creativeName1)
		.assert.value("select[name='AdType']", '10008')	
		.assert.value("select[name='AdFunction']", '4')
	},
	'test editing a promotional creative' : function (browser) {	
		browser		
		.url(creativeListingUrl)	
		.waitForElementVisible(creativesListing, loadTime)
		
		//click on the promotional creative just created
		.useXpath()
		.click("//a[text()='" + creativeName1 + "']")
		.useCss()
		
		//fill in form values
		.waitForElementVisible("input[name='Name']", loadTime)
		.clearValue("input[name='Name']")
		.setValue("input[name='Name']", creativeName2)	
		.click("select[name='AdFunction'] option[value='5']")
		
		.click(".btn-success")
	},
	'test promotional creative field values persisted after edit' : function (browser) {	
		browser			
		.waitForElementVisible(creativesListing, loadTime)
		
		//click on the promotional creative just created
		.useXpath()
		.waitForElementVisible("//a[text()='" + creativeName2 + "']", loadTime)
		.click("//a[text()='" + creativeName2 + "']")
		.useCss()
		
		//assert form values
		.waitForElementVisible("input[name='Name']", loadTime)
		.assert.value("input[name='Name']", creativeName2)
		.assert.containsText(".adTypeName", 'Animated Roadblock')	
		.assert.value("select[name='AdFunction']", '5')
	},
	'test duplicate creative name' : function (browser) {	
		browser		
		.url(promotionalCreativeCreateUrl)	
		.waitForElementVisible(promotionalEditForm, loadTime)
				
		//fill in duplicate promotional creative name
		.setValue("input[name='Name']", creativeName2)		
		.pause(1000)
		
		//fill in description
		.setValue("textarea[name='Description']", "blahz")		
		.pause(1000)
		
		.click(".btn-success")
		.pause(1000)
		
		.waitForElementVisible(".error", loadTime)
		.assert.containsText(".error", "Please enter an unique creative name.")
		
		.end();
	}
};
