var dest = "./build";
var src = './src/app';
var docs = './docs'

module.exports = {

  docs: {
    src: './src/app/**/*.coffee',
    dest: './docs'
  },

  //Bower components
  vendor_css: {
    outputName: 'vendor.css',
    dest: "./../content/app/stylesheets"
  },

  //Bower components
  vendor_js_iq: {
    dest: './../content/app/javascripts',
    outputName: 'vendor_iq.original.js',
    outputNameMin: 'vendor_iq.js'
  },

  
  //App's styl files
  app_stylus: {
    src: "./src/**/*.styl",
    dest: "./../content/app/stylesheets",
    outputName: 'app.original.css',
    outputNameMin: 'app.css',
    settings: {
      errors: true,
      use: [ require('nib')() ]
    }
  },

  app_webpack: {
    dest: "./../content/app/javascripts"
  },

  test: {
    browserify: {
      // entry: './src/app/test/test',
      entry: './test/test',
      extensions: ['.coffee', '.hbs'],
      paths: ['./node_modules','./src/app']
    },
    dest: './../content/app/test',
    outputBundle: 'testBundle.js'

  },

  browserSync: {
    //When gulp(default) runs the first time, BS(browserSync) opens a localhost:3000 webpage,
    // which actually shows contents of localhost:81.
    //But, BS reloads both localhost:81 and localhost:3000 on updates.
    proxy: "localhost:8081"
  }
};
