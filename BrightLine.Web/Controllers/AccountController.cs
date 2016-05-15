using BrightLine.Common.Framework;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Resources;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.ViewModels.Account;
using BrightLine.Web.Helpers;
using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using BrightLine.Common.Utility;
using BrightLine.Common.Services;

namespace BrightLine.Web.Controllers
{
	public class AccountController : BaseController
	{
		private IAccountService Accounts { get;set;}
		private ICookieService Cookies { get;set;}
		private IUserService Users { get;set;}

		public AccountController()
		{
			Accounts = IoC.Resolve<IAccountService>();
			Cookies = IoC.Resolve<ICookieService>();
			Users = IoC.Resolve<IUserService>();
		}
		#region SignIn / SignOut

		public ActionResult Index()
		{
			if (Auth.IsAuthenticated())
				return Redirect();

			var model = new SignInViewModel();
			var email = Cookies.Get(AuthConstants.Cookies.Email);
			if (!string.IsNullOrEmpty(email))
				model.EmailAddress = email;

			return View(model);
		}

		[HttpPost]
		public ActionResult Index(SignInViewModel model, string redirect = null)
		{
			try
			{
				if (!ModelState.IsValid)
					return View();

				var email = !model.EmailAddress.Contains("@") ? string.Format("{0}@brightline.tv", model.EmailAddress) : model.EmailAddress;
				var user = Users.GetUserByEmail(email);
				if (user == null)
					throw new ViewValidationException(string.Format("Invalid Email Address or Password"));

				Accounts.Authenticate(user, model.Password);
				user.LastLoginDate = DateTime.UtcNow;
				Users.Update(user);

				Cookies.SetAuth(email, model.RememberMe);
				if (!string.IsNullOrEmpty(redirect))
					return Redirect(HttpUtility.UrlDecode(redirect));

				return RedirectToAction("Redirect", "Account");
			}
			catch (Exception ex)
			{
				IoC.Log.ErrorFormat("AccountController.Index()", ex);
				if (ex is ViewValidationException)
					ModelState.AddModelError(ex.Message, ex.Message);
				else
					AddGenericError();
			}

			return View();
		}

		public ActionResult SignOut()
		{
			Cookies.SignOut(HttpContext.Session);
			return RedirectToAction("Index", "Account", new { so = "1" });
		}

		public ActionResult Redirect()
		{
			var user = HttpContext.User;
			if (user == null || user.Identity == null)
				return RedirectToAction("Index");

			var email = user.Identity.Name;
			var currentUser = Users.GetUserByEmail(email);
			if (Users.IsPasswordExpired(currentUser))
				return RedirectToAction("UpdatePassword", "Account");

			return RedirectToAction("Index", "Campaigns");
		}

		#endregion

		#region Activate

		[HttpGet]
		public ActionResult Activate(string p, string s)
		{
			var model = new ActivateViewModel();
			try
			{
				var invitation = Accounts.GetInvitation(p, s);
				model.Token = p;
				model.SecondaryToken = s;
				return View(model);
			}
			catch (Exception ex)
			{
				IoC.Log.ErrorFormat("AccountController.Activate()", ex);
				if (ex is InvalidAccountRequestException)
					model.Invalid = true;

				if (ex is ViewValidationException)
					ModelState.AddModelError(ex.Message, ex.Message);
				else
					AddGenericError();

				return View(model);
			}
		}

		[HttpPost]
		public ActionResult Activate(ActivateViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			try
			{
				var invitation = Accounts.GetInvitation(model.Token, model.SecondaryToken);
				var user = invitation.InvitedUser;
				Accounts.SetInitialPassword(user, model.Password, invitation);
				Cookies.SetAuth(user.Email, false);
				return RedirectToAction("Index", "Account");
			}
			catch (Exception ex)
			{
				IoC.Log.ErrorFormat("[HttpPost] AccountController.Index()", ex);
				if (ex is InvalidAccountRequestException)
					model.Invalid = true;

				if (ex is ViewValidationException)
					ModelState.AddModelError(ex.Message, ex.Message);
				else
					AddGenericError();

				return View(model);
			}
		}

		#endregion

		#region Retrieve

		public ActionResult Retrieve()
		{
			var model = new RetrieveViewModel();
			ViewBag.MailAddress = IoC.Resolve<ISettingsService>().MailSupportAddress;
			return View(model);
		}

		[HttpPost]
		public ActionResult Retrieve(RetrieveViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			try
			{
				var user = Users.GetUserByEmail(model.EmailAddress);
				if (user != null)
					Accounts.RetrieveAccount(user);
				else
				{
					var rand = new Random().Next(1000);
					Thread.Sleep(rand);
				}

				model.Completed = true;
			}
			catch (Exception ex)
			{
				IoC.Log.ErrorFormat("[HttpPost]AccountController.Retrieve()", ex);
				if (ex is ViewValidationException)
					ModelState.AddModelError(ex.Message, ex.Message);
				else
					AddGenericError();
			}

			return View(model);
		}

		#endregion

		#region Reset

		[HttpGet]
		public ActionResult Reset(string p, string s)
		{
			var model = new ResetViewModel();

			try
			{
				var request = Accounts.GetRetrievalRequest(p, s);
				model.Token = p;
				model.SecondaryToken = s;
				return View(model);
			}
			catch (Exception ex)
			{
				IoC.Log.ErrorFormat("AccountController.Reset({0}, {1})", ex, p, s);
				if (ex is InvalidAccountRequestException)
					model.Invalid = true;

				if (ex is ViewValidationException)
					ModelState.AddModelError(ex.Message, ex.Message);
				else
					AddGenericError();

				return View(model);
			}
		}

		[HttpPost]
		public ActionResult Reset(ResetViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			try
			{
				var request = Accounts.GetRetrievalRequest(model.Token, model.SecondaryToken);
				var user = request.User;
				Accounts.UpdatePassword(user, model.Password, request);
				model.Completed = true;
				return View(model);
			}
			catch (Exception ex)
			{
				IoC.Log.ErrorFormat("AccountController.Reset()", ex);
				if (ex is InvalidAccountRequestException)
					model.Invalid = true;

				if (ex is ViewValidationException)
					ModelState.AddModelError(ex.Message, ex.Message);
				else
					AddGenericError();

				return View(model);
			}
		}

		#endregion

		#region Forced Password Change

		[HttpGet]
		public ActionResult UpdatePassword()
		{
			return View();
		}

		[HttpPost]
		public ActionResult UpdatePassword(UpdatePasswordViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			try
			{
				var user = HttpContext.User;
				if (user == null || user.Identity == null)
					return View(model);

				var email = user.Identity.Name;
				var currentUser = Users.GetUserByEmail(email);
				Accounts.UpdatePassword(currentUser, model.Password);
				return RedirectToAction("Index", "Campaigns");
			}
			catch (Exception ex)
			{
				IoC.Log.ErrorFormat("AccountController.UpdatePassword()", ex);
				if (ex is ViewValidationException)
					ModelState.AddModelError(ex.Message, ex.Message);
				else
					AddGenericError();
			}

			return View(model);
		}

		#endregion

		#region Settings

		public ActionResult Settings()
		{
			var user = Auth.UserModel;
			var um = new UserViewModel()
			{
				Id = user.Id,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				TimeZoneId = user.TimeZoneId
			};
			return View(um);
		}

		[HttpPost]
		public ActionResult Settings(UserViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Success = false; return View(model);
			}

			try
			{
				// If the user has specified a new password/confirmed password but did not enter their original password, return an error 
				if (!string.IsNullOrEmpty(model.NewPassword) && !string.IsNullOrEmpty(model.PasswordConfirm) && string.IsNullOrEmpty(model.Password))
				{
					ModelState.AddModelError("Password required.", "Password is required.");
					model.Success = false; return View(model);
				}

				Accounts.UpdateProfile(model);
				model.Success = true;

				return RedirectToAction("Settings").Success("Settings saved.");
			}
			catch (IllegalPasswordChangeException ipex)
			{
				IoC.Log.ErrorFormat("[HttpPost] AccountController.Settings(UserViewModel model)", ipex);
				ModelState.AddModelError("ipex", ipex.Message);
				model.Success = false;
			}
			catch (Exception ex)
			{
				IoC.Log.ErrorFormat("[HttpPost] AccountController.Settings(UserViewModel model)", ex);
				AddGenericError();
				model.Success = false;
			}

			return View(model);
		}

		#endregion

		#region Change role
		/// <summary>
		/// Allows admins to change role.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult ChangeRole(int id)
		{
			AuthWebAdminHelper.ChangeRole(id);

			// Go back to originating page.
			if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.AbsolutePath))
				return Redirect(Request.UrlReferrer.AbsolutePath);

			return RedirectToAction("Redirect", "Account");
		}


		/// <summary>
		/// Resets the role of admin who switched to another role.
		/// </summary>
		/// <returns></returns>
		public ActionResult ResetRole()
		{
			AuthWebAdminHelper.ResetRole();

			// Go back to originating page.
			if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.AbsolutePath))
				return Redirect(Request.UrlReferrer.AbsolutePath);

			return RedirectToAction("Redirect", "Account");
		}
		#endregion
	}
}
