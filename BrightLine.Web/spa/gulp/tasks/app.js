/* browserify task
   ---------------
   Bundle javascripty things with browserify!

   This task is set up to generate multiple separate bundles, from
   different sources, and to use Watchify when run from the default task.

   See browserify.bundleConfigs in gulp/config.js
*/
var config       = require('../config'),
  cfgAppCoffee = config.app_coffee,
  cfgAppStylus = config.app_stylus;

var gulp        = require('gulp');

var sourcemaps  = require('gulp-sourcemaps');
var concat      = require('gulp-concat')
var uglify      = require('gulp-uglify');
var minifyCSS   = require('gulp-minify-css');
var filter      = require('gulp-filter');
var notify      = require("gulp-notify");

var logger = require('../util/logger');
var handleErrors = require('../util/handleErrors');

var stylus       = require('gulp-stylus');

gulp.task('app_stylus', function (done) {
  var timeTaken = 'Task app_stylus'
  logger.start(timeTaken)

  var outputNamePrimary = cfgAppStylus[global.outputNamePrimary],
    outputNameSecondary = cfgAppStylus.outputNameMin;

  var gulped = gulp.src(cfgAppStylus.src)
    .pipe(sourcemaps.init())
    .pipe(stylus(cfgAppStylus.settings))
    .pipe(concat(outputNamePrimary))
    .pipe(sourcemaps.write('.'))

    // Write the CSS & Source maps
    .pipe(gulp.dest(cfgAppStylus.dest))

    // Filtering stream to only css files
    .pipe(filter('**/*.css')) 

  if (global.watch){
    gulped = gulped.pipe(require('browser-sync').reload({stream:true}))
  }

  return gulped
    .on('end', function(){
      if (global.minify) {
        logger.log('Minifying ' + outputNamePrimary + 
              ' to ' + outputNameSecondary)

        var filepath = cfgAppStylus.dest + '/' + outputNamePrimary
        
        gulp.src(filepath)
          .pipe(concat(outputNameSecondary))
          .pipe(minifyCSS())
          .on('error', handleErrors)
          .pipe(gulp.dest(cfgAppStylus.dest))
          .on('end', function(){
            logger.end(timeTaken)
            //done2()
            //done()       
          })
      }  else if (global.isDev) {
          var filepath = cfgAppStylus.dest + '/' + outputNamePrimary
          logger.log('Touching ' + filepath)
          // Create a file of format *.original.* for dev (even if it's unused by the client) to provide a file expected by teamcity build process.
          gulp.src(filepath)
          .pipe(concat(cfgAppStylus.outputName))
          .pipe(gulp.dest(cfgAppStylus.dest))
        } else {
        logger.end(timeTaken)
        
      }
    })
  //return gulped;
});




gulp.task('app_webpack', function(done) {
  var config        = require('../config');
  var cfgAppWebpack  = config.app_webpack;
  var logger        = require('../util/logger');
  var gulp          = require('gulp');
  var path          = require('path');
  var fs            = require('fs');

  var dest = path.join(__dirname, '../../', cfgAppWebpack.dest);

  logger.log('Touching webpack output files in "' + dest + '"')
  // Create files expected by teamcity build process.

  fs.writeFileSync(dest + '/vendor_spa.js', "");
  fs.writeFileSync(dest + '/vendor_spa.original.js', "");
  fs.writeFileSync(dest + '/app.js', "");
  fs.writeFileSync(dest + '/app.original.js', "");
});

gulp.task('app', ['app_stylus', 'app_webpack']);
