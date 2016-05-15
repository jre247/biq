using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;

namespace BrightLine.Web.Helpers
{
	public static class BrightlineHtmlHelper
	{
		public static MvcHtmlString JsonSerializeToString(this object data, object @default = null)
		{
			if (data == null)
				return @default != null ? MvcHtmlString.Create(@default.ToString()) : MvcHtmlString.Empty;

			var settings = new JsonSerializerSettings()
			{
				NullValueHandling = NullValueHandling.Include,
				PreserveReferencesHandling = PreserveReferencesHandling.None,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};

			var json = "";
			try
			{
				json = JsonConvert.SerializeObject(data, Formatting.None, settings);
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				if (@default != null)
					json = @default.ToString();
			}

			var mhts = MvcHtmlString.Create(json);
			return mhts;
		}

		public static MvcHtmlString Select(this HtmlHelper htmlHelper, string name,
			IEnumerable options, string dataValueField = "Id", string dataTextField = "Name", object @default = null, object htmlAttributes = null)
		{
			var model = htmlHelper.ViewData.Model;
			var requiredMessage = ToRequiredAttributes(model, name);
			var currentValue = GetCurrentValue(model, name, @default, dataValueField);

			var sb = new StringBuilder();
			sb.AppendFormat("<select id='{0}' name='{1}' {2}{3}>", name, name, ToHtmlAttributes(htmlAttributes), requiredMessage);
			if (options != null)
			{
				foreach (var item in options)
				{
					var value = ReflectionHelper.TryGetStringValue(item, dataValueField);
					if (value == int.MinValue.ToString(CultureInfo.InvariantCulture)) value = null;
					var text = ReflectionHelper.TryGetStringValue(item, dataTextField);
					var selected = (value == currentValue) ? " selected='selected'" : "";
					sb.AppendFormat("<option value='{0}'{1}>{2}</option>", value, selected, text);
				}
			}
			sb.Append("</select>");
			var html = MvcHtmlString.Create(sb.ToString());
			return html;
		}

		public static MvcHtmlString SelectFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
			IEnumerable options, string dataValueField = "Id", string dataTextField = "Name", object @default = null, object htmlAttributes = null)
		{
			var name = ExpressionHelper.GetExpressionText(expression);
			var selectHtml = Select(htmlHelper, name, options, dataValueField, dataTextField, @default, htmlAttributes);
			return selectHtml;
		}

		public static MvcHtmlString EnumRadioList(this HtmlHelper htmlHelper, string name, Type enumType,
			object @default = null, string spanClass = null)
		{
			var model = htmlHelper.ViewData.Model;
			var modelValue = ReflectionHelper.TryGetValue(model, name);
			var selected = (int)(modelValue ?? @default ?? 0);
			var requiredMessage = ToRequiredAttributes(model, name);

			var sb = new StringBuilder();
			foreach (var item in Enum.GetValues(enumType))
			{
				var value = (int)item;
				var isChecked = (selected == value) ? "checked='checked' " : "";
				var id = name + "_" + value;
				sb.AppendFormat("<span class='{0} radio'><input type='radio' name='{1}' id='{2}' value='{3}' {4}{5}/><label for='{2}'>{6}</label></span>",
					spanClass, name, id, value, isChecked, requiredMessage, item);
			}

			var html = new MvcHtmlString(sb.ToString());
			return html;
		}

		public static MvcHtmlString EnumRadioListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
			Type enumType, object @default = null, string spanClass = null)
		{
			var name = ExpressionHelper.GetExpressionText(expression);
			var radio = EnumRadioList(htmlHelper, name, enumType, @default, spanClass);
			return radio;
		}

		public static MvcHtmlString SimpleMultipleSelect(this HtmlHelper htmlHelper, string name, IEnumerable options, string dataValueField, string dataTextField, IEnumerable selectedValues, object htmlAttributes = null)
		{
			var sb = new StringBuilder();
			sb.AppendFormat("<select id='{0}' name='{1}' {2} multiple='multiple'>", name, name, ToHtmlAttributes(htmlAttributes));

			var selected = new List<string>();
			if (selectedValues != null)
			{
				foreach (var sv in selectedValues)
				{
					var value = ReflectionHelper.TryGetStringValue(sv, dataValueField);
					selected.Add(value);
				}
			}
			if (options != null)
			{
				foreach (var item in options)
				{
					var isSelected = "";
					var value = ReflectionHelper.TryGetStringValue(item, dataValueField);
					var text = ReflectionHelper.TryGetStringValue(item, dataTextField);

					if (selected.Contains(value))
						isSelected = " selected='selected'";

					sb.AppendFormat("<option value='{0}'{1}>{2}</option>", value, isSelected, text);
				}
			}
			sb.Append("</select>");
			var html = new MvcHtmlString(sb.ToString());
			return html;
		}

		public static MvcHtmlString SimpleSelect(this HtmlHelper htmlHelper, string name, IEnumerable options, string dataValueField, string dataTextField, object selectedValue = null, object @default = null, object htmlAttributes = null)
		{
			var sb = new StringBuilder();
			sb.AppendFormat("<select id='{0}' name='{1}' {2}>", name, name, ToHtmlAttributes(htmlAttributes));

			var sv = (selectedValue == null) ? null : selectedValue.ToString();
			var dv = (@default == null) ? null : @default.ToString();
			if (options != null)
			{
				var set = false;
				const string selected = " selected='selected'";
				foreach (var item in options)
				{
					string s = null;
					var value = ReflectionHelper.TryGetStringValue(item, dataValueField);
					var text = ReflectionHelper.TryGetStringValue(item, dataTextField);
					if (sv != null && (sv == item.ToString() || sv == value))
					{
						if (set)
							sb.Replace(selected, ""); // clear out the default value if it is set.
						s = selected;
						set = true;
					}
					else if (!set && (dv == item.ToString() || dv == value))
					{
						s = selected;
						set = true;
					}

					sb.AppendFormat("<option value='{0}'{1}>{2}</option>", value, s, text);
				}
			}
			sb.Append("</select>");
			var html = new MvcHtmlString(sb.ToString());
			return html;
		}

		public static MvcHtmlString SelectEnum(this HtmlHelper htmlHelper, string name, Type enumType, object selectedValue, object htmlAttributes = null)
		{
			var sb = new StringBuilder();
			sb.AppendFormat("<select id='{0}' name='{1}' {2}>", name, name, ToHtmlAttributes(htmlAttributes));

			var enumValues = Enum.GetValues(enumType);
			var sv = (selectedValue == null) ? "" : selectedValue.ToString();
			foreach (var item in enumValues)
			{
				var selected = "";
				var value = (int)item;
				if (sv == item.ToString() || sv == value.ToString())
					selected = " selected='selected'";

				sb.AppendFormat("<option value='{0}'{1}>{2}</option>", value, selected, item);
			}

			sb.Append("</select>");
			var html = new MvcHtmlString(sb.ToString());
			return html;
		}

		public static string GetControllerName()
		{
			var segments = HttpContext.Current.Request.Url.Segments;
			var controller = (segments.Length > 1) ? segments[1] : "";
			return controller.ToLower();
		}

		#region Restricted Helpers

		public static MvcHtmlString RestrictedPartial(this HtmlHelper htmlHelper, bool isAllowed, string partialViewName, object model = null)
		{
			if (!isAllowed)
				return MvcHtmlString.Empty;

			return PartialExtensions.Partial(htmlHelper, partialViewName, model);
		}

		public static MvcHtmlString RestrictedPartial(this HtmlHelper htmlHelper, string role, string partialViewName, object model = null)
		{
			return RestrictedPartial(htmlHelper, new[] { role }, partialViewName, model);
		}

		public static MvcHtmlString RestrictedPartial(this HtmlHelper htmlHelper, string[] roles, string partialViewName, object model = null)
		{
			var isAllowed = Auth.IsUserInAnyRole(roles);
			return RestrictedPartial(htmlHelper, isAllowed, partialViewName, model);

		}

		public static MvcHtmlString RestrictedActionLink(this HtmlHelper htmlHelper, bool isAllowed, string linkText, string actionName = null, string controllerName = null, object routeValues = null, object htmlAttributes = null)
		{
			if (!isAllowed)
				return MvcHtmlString.Empty;

			return LinkExtensions.ActionLink(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString RestrictedActionLink(this HtmlHelper htmlHelper, string role, string linkText, string actionName = null, string controllerName = null, object routeValues = null, object htmlAttributes = null)
		{
			return RestrictedActionLink(htmlHelper, new[] { role }, linkText, actionName, controllerName, routeValues, htmlAttributes);
		}

		public static MvcHtmlString RestrictedActionLink(this HtmlHelper htmlHelper, string[] roles, string linkText, string actionName = null, string controllerName = null, object routeValues = null, object htmlAttributes = null)
		{
			var isAllowed = Auth.IsUserInAnyRole(roles);
			return RestrictedActionLink(htmlHelper, isAllowed, linkText, actionName, controllerName, routeValues, htmlAttributes);
		}

		public static HelperResult RestrictedAspect(this HtmlHelper htmlHelper, bool isAllowed, Func<Func<string, MvcHtmlString>, HelperResult> aspect)
		{
			if (!isAllowed)
				return new HelperResult((writer) => writer.Write(string.Empty));

			return aspect(s =>
			{
				var hs = MvcHtmlString.Create(s);
				return hs;
			});
		}

		public static HelperResult RestrictedAspect(this HtmlHelper htmlHelper, string role, Func<Func<string, MvcHtmlString>, HelperResult> aspect)
		{
			return RestrictedAspect(htmlHelper, new[] { role }, aspect);
		}

		public static HelperResult RestrictedAspect(this HtmlHelper htmlHelper, string[] roles, Func<Func<string, MvcHtmlString>, HelperResult> aspect)
		{
			var isAllowed = Auth.IsUserInAnyRole(roles);
			return RestrictedAspect(htmlHelper, isAllowed, aspect);
		}

		#endregion

		#region Private methods

		private static object ToHtmlAttributes(object htmlAttributes)
		{
			var sb = new StringBuilder();
			var oa = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
			if (oa == null)
				return sb.ToString();

			foreach (var o in oa)
			{
				sb.AppendFormat("{0}='{1}'", o.Key, o.Value);
			}

			return sb.ToString();
		}

		private static string GetCurrentValue(object model, string name, object @default, string valueField)
		{
			var modelValue = ReflectionHelper.TryGetValue(model, name);
			var value = ReflectionHelper.TryGetStringValue(modelValue, valueField) ?? ReflectionHelper.TryGetStringValue(@default, valueField);
			return value;
		}

		private static object ToRequiredAttributes(object model, string name)
		{
			var property = ReflectionHelper.TryGetProperty(model.GetType(), name);
			var requiredAttribute = (ReflectionHelper.TryGetAttribute(property, typeof(RequiredLookupAttribute)) ??
									   ReflectionHelper.TryGetAttribute(property, typeof(RequiredAttribute))) as ValidationAttribute;
			var display = ReflectionHelper.TryGetAttribute(property, typeof(DisplayAttribute)) as DisplayAttribute;

			if (requiredAttribute != null)
			{
				var errorMsg = requiredAttribute.ErrorMessage ?? (((display == null) ? property.Name : display.Name) + " required.");
				var requiredMsg = string.Format("data-val='true' data-val-required='{0}'", errorMsg);
				return requiredMsg;
			}

			var regexAttribute = ReflectionHelper.TryGetAttribute(property, typeof(RegularExpressionAttribute)) as RegularExpressionAttribute;
			if (regexAttribute != null)
			{
				var errorMsg = regexAttribute.ErrorMessage ?? (((display == null) ? property.Name : display.Name) + " required.");
				var regexPattern = regexAttribute.Pattern;
				var regexMsg = string.Format("data-val='true' data-val-regex='{0}' data-val-regex-pattern='{1}'", errorMsg, regexPattern);
				return regexMsg;
			}

			var naAttribute = ReflectionHelper.TryGetAttribute(property, typeof(NotApplicableAttribute)) as NotApplicableAttribute;
			if (naAttribute != null)
			{
				var errorMsg = naAttribute.ErrorMessage ?? (((display == null) ? property.Name : display.Name) + " required.");
				var naMsg = string.Format("data-val='true' data-val-required='{0}'", errorMsg);
				return naMsg;
			}

			return null;
		}

		#endregion
	}
}