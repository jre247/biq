var path = require('path')
var webpack = require('webpack');
var _ = require('lodash')

var webpackConfigBase = require(path.join(__dirname, 'webpack.config.base.js'))

var webpackConfigMinified = _.defaultsDeep({
    production: true,
    output: {
      filename: '[name].js'
    }
  }, webpackConfigBase);

webpackConfigMinified.plugins = webpackConfigMinified.plugins.concat(
  new webpack.optimize.UglifyJsPlugin({ minimize: true }),
  new webpack.DefinePlugin({
    "process.env": {
      // This reduces the React library size via dead-code removal by UglifyJS.
      "NODE_ENV": JSON.stringify("production")
    }
  })
);

module.exports = webpackConfigMinified;
