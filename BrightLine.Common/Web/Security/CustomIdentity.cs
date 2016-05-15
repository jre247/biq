using System.Security.Principal;
using System.Web.Security;

namespace BrightLine.Web.Models.Security
{
	public class CustomIdentity
				: IIdentity
	{

		#region Fields

		private const string AuthenticationTypeName = "BrightLineIQ";

		private readonly FormsAuthenticationTicket Ticket;

		#endregion

		#region Initialization/Finalization

		public CustomIdentity(FormsAuthenticationTicket ticket)
		{
			this.Ticket = ticket;
		}

		#endregion

		#region Properties

		public string AuthenticationType
		{
			get { return AuthenticationTypeName; }
		}

		public bool IsAuthenticated
		{
			get { return true; }
		}

		public string Name
		{
			get { return Ticket.Name; }
		}

		public string FriendlyName
		{
			get { return Ticket.UserData; }
		}

		#endregion
	}
}