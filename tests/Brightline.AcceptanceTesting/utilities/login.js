module.exports = function(browser, url){
    return browser
        .url(url)
        .waitForElementVisible('body', 1000)
        .setValue('#EmailAddress', 'uitester@mailinator.com')
        .setValue('#Password', 'Digital6!')
        .click('.btn-success');
};