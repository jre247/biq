var gulp = require('gulp');


/*
  Global properties:
    global.outputNamePrimary
      The razor views always point to the minified name by default
        in all envs. 
      
      In dev(set to "outputNameMin"):
        The primary(non-minified) output will use the minified name.
        There's no secondary(minified) output in dev.
      In pro(set to "outputName"):
        The primary(non-minified) output will use the non-minified name.
        The secondary(minified) output will use the minified name.

    global.production

    global.minify



  Current path

    default:
      vendor
        vendor_css
        vendor_js_iq
        vendor_js_spa

      watch
        setWatch - Sets global.isWatching to true. Task app_coffee determines whether to watchify the browserify bundle

        BrowserSync
          app_coffee
    pro:

*/
global.watchTest = false

gulp.task('setLocal', function(){
  global.production = false;
  global.minify = false;  
  global.outputNamePrimary = "outputNameMin";
  global.watch = true;

});

gulp.task('setDev', function(){
  global.production = false;
  global.minify = false;  
  global.outputNamePrimary = "outputNameMin";
  global.watch = false;
  global.isDev = true;
});

gulp.task('setPro', function(){
  global.production = true;
  global.minify = true;
  global.outputNamePrimary = "outputName";
  global.watch = false;
});

//run via 'gulp'

gulp.task('local', ['version', 'setLocal', 'vendor', 'watch'])

//run via 'gulp dev'. Does everything as default, without browserSync/
gulp.task('dev', ['version', 'setDev', 'vendor', 'app'])

//run via 'gulp pro'
gulp.task('pro', ['version', 'setPro', 'vendor', 'app']);

// Alias to local and pro
gulp.task('default', ['local'])
gulp.task('optimize', ['pro'])

//run docco via 'gulp docco' (Look at docco.js for further info)

//run tests once via 'gulp test'
//run tests continuously via 'gulp test_watch'. Watches for changes in specs, and app bundle.

gulp.task('version', function(done){
  var fs = require('fs');
  var exec = require('child_process').exec;

  var pad = function(d) {
    return (d < 10) ? '0' + d.toString() : d.toString();
  };
  
  var date = new Date(),
    versionString = [
      1,
      pad((date.getFullYear() % 100))
      , pad((date.getMonth() + 1))
      , pad(date.getDate())
      , pad(date.getHours())
      , pad(date.getMinutes())
    ].join('.');

  // Store some of the commit and build info into version.coffee (injected into _bl.build via initialize.coffee)
  exec("git rev-parse HEAD", function(error, stdout, stderr){
    commitHashFull = stdout;
    commitHashPartial = commitHashFull.substring(0, 5)
    fs.writeFileSync('./src/app/version.coffee', "module.exports = { version: '" + versionString + "', commitHash: '" + commitHashPartial + "'}");
    done();
  });  
});
