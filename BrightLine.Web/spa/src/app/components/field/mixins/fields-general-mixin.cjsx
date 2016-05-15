FieldsLifecycleMixin  = require './fields-lifecycle-mixin'
FieldsRenderMixin     = require './fields-render-mixin'
FieldsSaveMixin       = require './fields-save-mixin'

FieldsGeneralMixin = module.exports = _.extend({}, FieldsLifecycleMixin, FieldsRenderMixin, FieldsSaveMixin)
