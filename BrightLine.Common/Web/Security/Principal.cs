using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using System.Security.Principal;

namespace BrightLine.Web.Models.Security
{
	public class CustomPrincipal
			   : IPrincipal
	{
		#region Initialization/Finalization

		public CustomPrincipal(CustomIdentity identy)
		{
			this.Identity = identy;
		}
		#endregion

		public IIdentity Identity { get; private set; }

		public bool IsInRole(string role)
		{
			var roles = IoC.Resolve<IRoleService>();

			return roles.GetRoles(Identity.Name).Contains(role);
		}
	}
}