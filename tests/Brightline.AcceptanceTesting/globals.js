var HtmlReporter = require('nightwatch-html-reporter');
var reporter = new HtmlReporter({
    openBrowser: false,
    reportsDirectory: __dirname + '/reports_html'
});
module.exports = {
    reporter: reporter.fn
};