var gulp = require('gulp');
var config = require('../config'),
  cfgDocs = config.docs;

gulp.task('docco', function(){
  var docco = require("gulp-docco");
  return gulp.src(cfgDocs.src)
    .pipe(docco())
    .pipe(gulp.dest(cfgDocs.dest));
});
