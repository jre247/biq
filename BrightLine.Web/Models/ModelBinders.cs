using BrightLine.Common.Utility;
using BrightLine.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace BrightLine.Web.Helpers
{
	public class EntityModelBinder : DefaultModelBinder
	{
		protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext,
											 PropertyDescriptor propertyDescriptor)
		{
			//order here is important, lookups are all entities, but only need the lookup bindings 
			var type = propertyDescriptor.PropertyType;
			if (ReflectionHelper.HasInterface<ILookup>(type)) // a lookup, bind with LookupBinder
				LookupBinder.BindProperty(controllerContext, bindingContext, propertyDescriptor);
			else if (ReflectionHelper.HasInterface<IEntity>(type)) // an entity, bind with EntityBinder
				EntityBinder.BindProperty(controllerContext, bindingContext, propertyDescriptor);
			else
				base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
		}

		private static class LookupBinder
		{
			public static void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
			{
				var type = propertyDescriptor.PropertyType;
				// not a lookup, bind with base, bail
				if (!ReflectionHelper.HasInterface<ILookup>(type))
					return;

				var form = controllerContext.HttpContext.Request.Form;
				var property = propertyDescriptor.Name;
				var modelType = bindingContext.Model.GetType();
				var pi = ReflectionHelper.TryGetProperty(modelType, property);

				// if the value can't be set on the model (type mismatch probably), add model error and bail				
				var parsed = ReflectionHelper.TrySetValue(bindingContext.Model, property, form[property]);
				if (!parsed)
				{
					var display = ReflectionHelper.TryGetAttribute<DisplayAttribute>(pi);
					bindingContext.ModelState.AddModelError(property, "Invalid value for " + ((display == null) ? property : display.Name) + " selected.");
					return;
				}

				var lookup = ReflectionHelper.TryGetValue(bindingContext.Model, property) as ILookup;
				if (lookup != null && lookup.Id >= 0) // happy value, bail
					return;

				// check if the validators are all good.
				var validators = pi.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>();
				var messages = validators.Where(va => (va.ErrorMessage != null)).Select(va => va.ErrorMessage);
				var error = messages.Any() ? string.Join(", ", messages) : "Invalid value";
				foreach (var validator in validators)
				{
					if (validator.IsValid(lookup))
						continue;

					bindingContext.ModelState.AddModelError(property, error);
					break;
				}
			}
		}

		private static class EntityBinder
		{
			public static void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
			{
				var type = propertyDescriptor.PropertyType;
				// not a lookup, bind with base, bail
				if (!ReflectionHelper.HasInterface<IEntity>(type))
					return;

				var form = controllerContext.HttpContext.Request.Form;
				var property = propertyDescriptor.Name;
				var modelType = bindingContext.Model.GetType();
				var pi = ReflectionHelper.TryGetProperty(modelType, property);

				// if the value can't be set on the model (type mismatch probably), add model error and bail				
				var parsed = ReflectionHelper.TrySetValue(bindingContext.Model, property, form[property]);
				if (!parsed)
				{
					var display = ReflectionHelper.TryGetAttribute<DisplayAttribute>(pi);
					bindingContext.ModelState.AddModelError(property, "Invalid value for " + ((display == null) ? property : display.Name) + " selected.");
					return;
				}

				var entity = ReflectionHelper.TryGetValue(bindingContext.Model, property) as IEntity;
				if (entity != null && entity.Id >= 0) // happy value, bail
					return;

				// check if the validators are all good.
				var validators = pi.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>();
				var messages = validators.Where(va => (va.ErrorMessage != null)).Select(va => va.ErrorMessage);
				var error = messages.Any() ? string.Join(", ", messages) : "Invalid value";
				foreach (var validator in validators)
				{
					if (validator.IsValid(entity) || validator.IsValid(entity.Id))
						continue;

					bindingContext.ModelState.AddModelError(property, error);
					break;
				}
			}
		}
	}

}