var loginService = require('../utilities/login.js');
//var config = require('../utilities/config.js');

var createCampaignUrl = "";
var validationSummary = '.validation-summary-errors';
var editCampaignLink = 'a[title="Edit"]';

module.exports = {
	before : function(browser) {
		console.log('Setting up...');
		createCampaignUrl = browser.launch_url + '/campaigns/create';
		
		//login
		var launchUrl = browser.launch_url;
		loginService(browser, launchUrl)
    },
   'test campaign create with blank fields causing validation errors' : function (browser) {	
		browser
		
		.url(createCampaignUrl)
		
		//save
		.click('#submitBtn')
		
		.pause(1000)
		
		//assert
		.assert.containsText(validationSummary, 'Name is required.')
		.assert.containsText(validationSummary, 'Agency must be selected.')
		.assert.containsText(validationSummary, 'Product must be selected.')
		.assert.containsText(validationSummary, 'Campaign type must be selected.')
	},
   'test campaign create saves correctly' : function (browser) {	
		browser
		
		.url(createCampaignUrl)	
	
		//set form values
		.setValue('#Name', 'Campaign Test 1')		
		.click('#Agency option[value="1"]')
		.click('#Product option[value="2"]')
		.click('#CampaignType_1')	
		.click('#submitBtn')
	},
   'test campaign has correct field values (1)' : function (browser) {	
		browser
		
		//navigate to edit campaign page
		.waitForElementVisible(editCampaignLink, 5000)
		.click(editCampaignLink)
		.waitForElementVisible('#Name', 5000)
		
		//assert 
		.assert.value('#Name', 'Campaign Test 1')
		.assert.value('#Agency', '1')
		.assert.value('#Product', '2')
		.assert.attributeContains('#CampaignType_1', 'checked', 'true')
		
		//save
		.click('#submitBtn') //to redirect back to campaign summary page
	},
   'test campaign edit saves correctly' : function (browser) {	
		browser
		
		//navigate to edit campaign page
		.waitForElementVisible(editCampaignLink, 5000)
		.click(editCampaignLink)
		.waitForElementVisible('#Name', 5000)
		
		//set form values
		.clearValue('#Name')
		.setValue('#Name', 'Campaign Test 2')		
		.click('#Agency option[value="2"]')
		.click('#Product option[value="4"]')
		.click('#CampaignType_2')	
		
		//save
		.click('#submitBtn')
	},
   'test campaign has correct field values (2)' : function (browser) {	
		browser
		
		//navigate to edit campaign page
		.waitForElementVisible(editCampaignLink, 5000)
		.click(editCampaignLink)
		.waitForElementVisible('#Name', 5000)
		
		//assert 
		.assert.value('#Name', 'Campaign Test 2')
		.assert.value('#Agency', '2')
		.assert.value('#Product', '4')
		.assert.attributeContains('#CampaignType_2', 'checked', 'true')

		//tell nightwatch that i'm done with tests in this file
		.end();
	},
};
