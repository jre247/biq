var config       = require('../config'),
  cfgTest = config.test,
  cfgVendorJsIq   = config.vendor_js_iq,
  cfgVendorJsSpa  = config.vendor_js_spa,
  cfgAppCoffee    = config.app_coffee;

var gulp          = require('gulp');

// var sourcemaps    = require('gulp-sourcemaps');
// var concat        = require('gulp-concat')

// var logger        = require('../util/logger');
// var handleErrors  = require('../util/handleErrors');

// var browserify    = require('browserify');
// var watchify     = require('watchify');
// var source        = require('vinyl-source-stream');
// var buffer        = require('vinyl-buffer');



gulp.task('test', [])

gulp.task('test_watch', ['setWatchTest', 'test_build'])

gulp.task('setWatchTest', function(){
  global.watchTest = true
})


gulp.task('test_run', function () {
  var mochaPhantomJS = require('gulp-mocha-phantomjs');
  console.log('running test_run')
  return gulp
  .src(cfgTest.dest + '/runner.html')
  .pipe(mochaPhantomJS())
  //.pipe(mochaPhantomJS({reporter: 'xunit'})); //, dump: cfgTest.dest + '/test.xml'}))
});

gulp.task('test_build', function(gulpCallback) {

  var configBsfy = cfgTest.browserify;

  var bundler = browserify({
    debug: false,
    paths: configBsfy.paths,
    extensions: configBsfy.extensions,

    entries: configBsfy.entry,

    bundleExternal: true,
    fullPaths: true,
    detectGlobals: false,
    insertGlobals: {},
    packageCache: {},
    cache: {}
  });

  var timeTakenAll = 'app_test - Browserifying bundle';

  var outputNamePrimary = cfgTest.outputBundle;

  var bundle = function(){
    logger.log('Browserifying', configBsfy.entry);
    logger.start(timeTakenAll)
    return bundler
      .bundle()
      // Report compile errors
      .on('error', handleErrors)
      // Specifiy the desired output filename here.
      .pipe(source(cfgTest.outputBundle))

      //Convert stream to a buffer
      .pipe(buffer())

      //Write to /content/app/test folder
      .pipe(gulp.dest(cfgTest.dest))

      .on('end', function(){ 
        logger.end(timeTakenAll)

        if (global.watchTest){
          // Trigger test_run when the test_watch command is used. Thus, changes to any part of the code or test reruns tests.
          gulp.start('test_run')
        }        
      })

  }   

  if(global.watchTest) {
    logger.log('Watchifying app_test bundle for updates...');
    // Wrap with watchify and rebundle on changes
    bundler = watchify(bundler);
    // Rebundle on update
    bundler.on('update', bundle);
    
  }else{
    
  }

  return bundle();
});
