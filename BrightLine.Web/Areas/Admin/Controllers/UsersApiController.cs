using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Authentication;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace BrightLine.Web.Areas.Campaigns.Controllers
{
	[RoutePrefix("api/users")]
	[PascalCase]
	public class UsersApiController : ApiController
	{
		private IUserService Users { get;set;}

		public UsersApiController()
		{
			Users = IoC.Resolve<IUserService>();
		}

		#region GET

		[GET("unlock/{id}")]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public bool Unlock(int id)
		{
			try
			{
				var user = Users.Get(id);
				user.LockOutWindowStart = null;
				user.FailedPasswordAttemptCount = 0;
				user.FailedPasswordAttemptWindowStart = null;
				Users.Update(user);
				return true;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("User could not be unlocked.", ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("resendinvitation/{id}")]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public bool ResendInvitation(int id)
		{
			try
			{
				var accounts = IoC.Resolve<IAccountService>();

				var user = Users.Get(id);
				if (user.IsActive)
					return false;

				accounts.CreateInvitation(user, Auth.UserModel);
				return true;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Invitation could not be resent.", ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		[GET("emailavailable/{email}")]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public bool EmailAvailable(string email)
		{
			try
			{
				var user = Users.Where(u => u.Email == email, true).FirstOrDefault();
				var available = (user == null);
				return available;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not check email availabiliy.", ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		#endregion

		#region Post

		[POST("save/{id}")]
		[System.Web.Http.AcceptVerbs("POST")]
		[System.Web.Http.HttpPost]
		public User Save([FromUri]int id, [FromBody] string json)
		{
			try
			{
				var user = Users.SaveUser(id, json);
				return user;
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Error saving user.", ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		#endregion
	}
}