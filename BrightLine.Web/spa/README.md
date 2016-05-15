## Getting started in local environment
1. Go to the SPA folder: 

  `cd /Brightline.Web/spa`
1. Download all the NPM packages:

 `npm install`

1. Download all the Bower packages: 

  `bower install`

1. Run the frontend build: 

  `gulp`

  In another terminal, run:

  `npm run local`

  This should open `localhost:3000`, which proxies to `localhost:8081`, and allows BrowserSync to reload and live-reload coffee and stylus files respectively.

  Sometimes, doing a git pull while gulp is running might cause errors where certain libraries are missing on the client. This could be due to an updated package.json, or bower.json. Stopping the currently running gulp process, and repeating all the above steps should resolve this.



## Running tests
* To run tests, open another shell, and run `gulp test`:
* To run tests in watch mode, run `gulp test_watch`. This is useful when working on test, or a piece of code that has corresponding tests.
* To view a more user-friendly output of tests, it's recommended to open `/Brightline.Web/Content/app/test/runner.html`. This is the file that Phantom JS runs each time to run the Mocha tests.


## Documentation
  Run `gulp docco` to build out a website with annotated source code. Output goes in `/Brightline.Web/spa/docs`

  
## Building in DEV/UAT/Production
The following is used in Teamcity for building a minified, un-sourcemapped output. 

1. `npm install` .

1. `bower install`

1. `gulp optimize`

1. In DEV, run `npm run dev`. In UAT, and Production, run `npm run pro`


  This runs the code through uglify, and get's rid of comments, sourcemaps, and applies dead-code removal. It also doesn't watch for updates, and doesn't use BrowserSync.

  In UAT and Production, it runs the build once that outputs unminified version, and a second time that runs it with minification via UglifyJS. Normal requests retrieve the minified version, while a developer could retrieve the unminified version (by adding the querystring '?buildmode=original') to troubleshoot an UAT/Production issue.
