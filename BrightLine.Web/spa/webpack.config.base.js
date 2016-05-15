/*
The following are useful resources on setting up a build with Webpack:
  http://krasimirtsonev.com/blog/article/a-modern-react-starter-pack-based-on-webpack
  https://github.com/webpack/webpack-with-common-libs/blob/master/gulpfile.js
  http://www.jayway.com/2014/03/28/running-scripts-with-npm/
  http://webpack.github.io/docs/configuration.html
*/

var path = require('path')
var webpack = require('webpack');

// console.log("Running Webpack with WEBPACK_BUILDMODE as '" + buildMode + "'");

// var plugins = [
//   new webpack.NoErrorsPlugin()
// ];

// if (buildMode === 'minified') {
//   plugins.push(new UglifyJsPlugin({ minimize: true }));
//   plugins.push(new webpack.DefinePlugin({
//       "process.env": {
//         // This reduces the React library size via dead-code removal by UglifyJS.
//         "NODE_ENV": JSON.stringify("production")
//       }
//     }));
// }

// var buildModeConfigMap = {
//   original: {
//     devtool: 'eval',
//     outputFileName: '[name].original.js',
//   }
//   , minified: {
//     outputFileName: '[name].js',
//     production: true
//   }
// }

// var buildModeConfig = buildModeConfigMap[buildMode];

var webpackDevServerConfig = {
  port: 3002
};

var webpackConfigBase = {  
  webpackDevServerConfig: webpackDevServerConfig,
  cache: true,
  entry:{  
    app: './src/app/app',
    vendor_spa: './src/lib/vendor_spa'
  },
  noInfo: true,
  hot: false,

  //devtool: 'defined in extended config files'

  //production: 'defined in extended config files'

  output: {  
    path: path.join(__dirname, './../content/app/javascripts')
    //filename: 'defined in extended config files'
  },
  resolve: {  
    extensions: [  
      "",
      ".jsx",
      ".cjsx",
      ".coffee",
      ".js",
      '.styl',
      '.css',
      '.html'
    ],
    modulesDirectories: [  
      "app",
      'node_modules',
      'bower_components'
    ]
  },
  module: {  
    loaders: [
     {  
      test: /\.cjsx$/,
      loaders: [  
        "coffee",
        "cjsx",
        "envify-loader"
      ],
      include: path.join(__dirname, 'src')
     },
     {  
      test: /\.coffee$/,
      loader: 'coffee-loader',
      include: path.join(__dirname, 'src')
     },
    ]
  },
  plugins: [
    new webpack.NoErrorsPlugin()
    //Other plugins are defined in extended config files
  ]
};

module.exports = webpackConfigBase;
