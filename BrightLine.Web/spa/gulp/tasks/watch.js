/* Notes:
   - gulp/tasks/browserify.js handles js recompiling with watchify
   - gulp/tasks/browserSync.js watches and reloads compiled files
*/

/*
Original path
default
  watch
    setwatch
      global.watch=true
    browsersync
      build
        browserify, stylus, img, markup
      browsersync(config)
    gulpwatch stylus, img, markup
*/


var gulp  = require('gulp');
var config= require('../config');

gulp.task('watch', ['app_stylus', 'browserSync'], function() {
  //gulp.watch(config.app_coffee.src, ['app_coffee']);
  gulp.watch(config.app_stylus.src, ['app_stylus']);
});

  gulp.task('browserSync', function() {
    var browserSync = require('browser-sync');
    browserSync(config.browserSync);
  });
  
