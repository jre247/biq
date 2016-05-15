# Set up logger
require 'lib/logger'

_bl.build = require './version'

# This is the entrypoint of the frontend scripts
routes = require './routes'

window.App = 
  sitemap: routes.sitemap
  mountNode: document.getElementById('app-wrapper')

Router = require './router'

App.mount = => 
  ReactDOM.render(<Router sitemap={App.sitemap}/>, App.mountNode)

setTimeout App.mount, 10

