
namespace BrightLine.Common.Utility.Authentication
{
	public static class AuthConstants
	{
		public static class Roles
		{
			public const string Developer = "Developer";
			public const string Admin = "Admin";
			public const string Employee = "Employee";
			public const string ImplementationAssistantAdmin = "ImplementationAssistantAdmin";
			public const string CMSAdmin = "CMSAdmin";
			public const string Client = "Client";
			public const string AgencyPartner = "AgencyPartner";
			public const string MediaPartner = "MediaPartner";
			public const string CMSEditor = "CMSEditor";
			public const string BlueprintAdmin = "BlueprintAdmin";
			public const string AppDeveloper = "AppDeveloper";
			public const string CampaignAdmin = "CampaignAdmin";
			public const string Leadership = "Leadership";
			public const string BusinessIntelligence = "BusinessIntelligence";
			public const string AdOps = "AdOps";
			public const string AccountManager = "AccountManager";
		}

		public static class Cookies
		{
			// Refer to code : global.asax Authentication_Request.
			// FEATURE : IQ-283: Allow Admins to view application as different role
			public const string AdminRoleOverride = "_bl.AFSiPTqDvK";
			public const string Email = "_bl.Sg8i3o8cHm";
		}

		public static class Errors
		{
			public const string INVALID_ROLE_SELECTED = "An invalid Role has been selected.";
			public const string EMPLOYEE_ROLE_NOT_SELECTED = "Employee Role has not been selected.";
			public const string EMPLOYEE_ROLE_MUST_NOT_BE_SELECTED = "Employee Role must not be selected.";
			public const string ADVERTISER_MUST_BE_SELECTED = "An advertiser must be selected for the Client role.";
			public const string MEDIA_AGENCY_MUST_BE_SELECTED = "A Media Agency must be selected for the Agency Partner role.";
			public const string MEDIA_PARTNER_MUST_BE_SELECTED = "A Media Partner must be selected for the Media Partner role.";
		}
	}
}