# Recursively require all the unit test specs.
require('./unit/**/*.coffee', {glob: true}) 

# Recursively, require all files that end with '-test.coffee' in the main src/app area.
require('./../src/app/**/*-test.coffee', {glob: true}) 

# It's recommended to open C:\code\brightline\BrightLine.Web\Content\app\test\runner.html 
# in the browser for debugging tests, getting better UI, etc.
