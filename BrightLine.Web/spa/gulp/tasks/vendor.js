
var gulp          = require('gulp');
var debug         = require('gulp-debug')
var concat        = require('gulp-concat');
var mainBowerFiles = require('main-bower-files');

var minifyCSS     = require('gulp-minify-css');
var gulpFilter    = require('gulp-filter');

var uglify        = require('gulp-uglify');

var logger = require('../util/logger');
var handleErrors = require('../util/handleErrors');

var config        = require('../config'),
  cfgVendorCss    = config.vendor_css,
  cfgVendorJsIq   = config.vendor_js_iq,
  cfgVendorJsSpa  = config.vendor_js_spa;


gulp.task('vendor', ['vendor_css', 'vendor_js_iq']);
gulp.task('vendorDev', ['setDev']);

gulp.task('vendor_css', function(callback) {
  var cssFilter = gulpFilter('**/*.css');

  return gulp.src(mainBowerFiles())
    .pipe(cssFilter)
    .pipe(concat(cfgVendorCss.outputName))
    .pipe(minifyCSS())
    .pipe(gulp.dest(cfgVendorCss.dest));
});


gulp.task('vendor_js_iq', function(done) {

  var timeTaken = 'vendor_js_iq'
  logger.start(timeTaken);

  var outputNamePrimary = cfgVendorJsIq[global.outputNamePrimary],
    outputNameSecondary = cfgVendorJsIq.outputNameMin;

  var jsFilter = gulpFilter(['**/*.js','!**/chaplin.js']);

  gulp.src(mainBowerFiles())
    .pipe(jsFilter)
    .pipe(concat(outputNamePrimary))
    .pipe(gulp.dest(cfgVendorJsIq.dest))
    .on('end', function(){
      if(global.minify){
        logger.log('Minifying ' + outputNamePrimary + 
          ' to ' + outputNameSecondary)

        var filepath = cfgVendorJsIq.dest + '/' + outputNamePrimary
        
        
        gulp.src(filepath)
          .pipe(concat(outputNameSecondary))
          .pipe(uglify())
          .pipe(gulp.dest(cfgVendorJsIq.dest))
          .on('end', function(){
            logger.end(timeTaken)
            done()       
          })
      } else if (global.isDev) {
          var filepath = cfgVendorJsIq.dest + '/' + outputNamePrimary
          logger.log('Touching ' + cfgVendorJsIq.outputName)
          // Create a file of format *.original.* for dev (even if it's unused by the client) to provide a file expected by teamcity build process.
          gulp.src(filepath)
          .pipe(concat(cfgVendorJsIq.outputName))
          .pipe(gulp.dest(cfgVendorJsIq.dest))
      } else {
        logger.end(timeTaken)
        done()
      }
    });
});
