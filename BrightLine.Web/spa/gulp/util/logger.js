/* bundleLogger
   ------------
   Provides gulp style logs to the bundle method in browserify.js
*/

var gutil        = require('gulp-util');
var prettyHrtime = require('pretty-hrtime');
var startTime = {};

module.exports = {
  start: function(msg) {
    startTime[msg] = process.hrtime();
    //gutil.log('Bundling', gutil.colors.green(filepath) + '...');
  },

  end: function(msg) {
    var taskTime = process.hrtime(startTime[msg]);
    var prettyTime = prettyHrtime(taskTime);
    gutil.log(gutil.colors.cyan(msg), 'took', gutil.colors.green(prettyTime));
  },
  log: function() {
    var args = Array.prototype.slice.call(arguments);

    gutil.log.apply(gutil.log, args)
  }

};
