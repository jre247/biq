using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace BrightLine.Utility.Menus
{
	public class MenuItem
	{
		/// <summary>
		/// Id for the css link/anchor/li.
		/// </summary>
		public string CssId { get; set; }


		/// <summary>
		/// used for sorting the menu items
		/// </summary>
		public int Order { get; set; }


		/// <summary>
		/// Name of the menu item
		/// </summary>
		public string Text { get; set; }


		/// <summary>
		/// Controller for menu action
		/// </summary>
		public Type Controller { get; set; }


		/// <summary>
		/// Action on the controller
		/// </summary>
		public string Action { get; set; }


		/// <summary>
		/// Area of the application ( admin )
		/// </summary>
		public string Area { get; set; }


		/// <summary>
		/// Optional Url to use instead of an controller/action.
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Optional routevalues for the link
		/// </summary>
		public RouteValueDictionary RouteValues { get; set; }

		/// <summary>
		/// Optional html attributes for the menu item.
		/// </summary>
		public IDictionary<string, object> HtmlAttributes { get; set; }


		/// <summary>
		/// The roles that can see this
		/// </summary>
		public string[] Roles { get; set; }


		/// <summary>
		/// The controller name.
		/// </summary>
		public string ControllerName
		{
			get
			{
				var name = this.Controller.Name.Replace("Controller", "");
				return name;
			}
		}

		/// <summary>
		/// Child menu items.
		/// </summary>
		public List<MenuItem> Children { get; set; }


		/// <summary>
		/// Wehther or not this has submenues.
		/// </summary>
		/// <returns></returns>
		public bool HasSubmenu()
		{
			return Children != null && Children.Count > 0;
		}
	}
}
