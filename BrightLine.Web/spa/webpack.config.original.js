var path = require('path')
var webpack = require('webpack');
var _ = require('lodash')

var webpackConfigBase = require(path.join(__dirname, 'webpack.config.base.js'))

var webpackConfigOriginal = _.defaultsDeep({
    output: {
      filename: '[name].original.js'
    },
    devtool: 'eval'
  }, webpackConfigBase);

module.exports = webpackConfigOriginal;
