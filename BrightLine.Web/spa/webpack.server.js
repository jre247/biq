// From http://www.christianalfoni.com/articles/2015_04_19_The-ultimate-webpack-setup

var Webpack = require('webpack');
var WebpackDevServer = require('webpack-dev-server');
var webpackConfig = require('./webpack.config.original.js');

var webpackDevServerConfig = webpackConfig.webpackDevServerConfig;

webpackConfig.entry = {
  app: [
    'webpack-dev-server/client?http://localhost:' + webpackDevServerConfig.port, // WebpackDevServer host and port
    'webpack/hot/only-dev-server', // "only" prevents reload on syntax errors,
    webpackConfig.entry.vendor_spa,
    webpackConfig.entry.app
  ]
};

// This is the port and path of webpack dev server that will be referenced by spa.cshtml
webpackConfig.output.publicPath = "http://localhost:" + webpackDevServerConfig.port + "/content/app/javascripts/";
webpackConfig.module.loaders[0].loaders.unshift("react-hot");
webpackConfig.plugins.unshift(new Webpack.HotModuleReplacementPlugin());

// First we fire up Webpack and pass in the configuration was created
var bundleStart = null;
var compiler = Webpack(webpackConfig);

// We give notice in the terminal when it starts bundling and
// set the time it started
compiler.plugin('compile', function() {
  console.log('Bundling...');
  bundleStart = Date.now();
});

// We also give notice when it is done compiling, including the
// time it took. Nice to have
compiler.plugin('done', function() {
  console.log('Bundled in ' + (Date.now() - bundleStart) + 'ms');
});

var webpackDevServer = new WebpackDevServer(compiler, {

  // publicPath is the path that the bundle will make requests to, for *.hot-update.json files.
  publicPath: webpackConfig.output.publicPath,

  // Configure hot replacement
  hot: true,

  // The rest is terminal configurations
  quiet: false,
  noInfo: true,
  stats: {
    colors: true
  }
});

// Start server and give notice in the terminal
// that we are starting the initial bundle
webpackDevServer.listen(webpackDevServerConfig.port, 'localhost', function () {
  console.log('Bundling project, please wait...');
});
