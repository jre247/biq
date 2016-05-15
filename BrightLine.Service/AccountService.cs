using System.Security.Cryptography;
using System.Threading;
using System.Transactions;
using BrightLine.Common.Framework;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Common.ViewModels.Account;
using BrightLine.Core;

namespace BrightLine.Service
{
	public class AccountService : IAccountService
	{
		public AccountService()
		{
		}

		#region Authentication

		/// <summary>
		/// Attempts to Authenticate a User with provided password. Handles Failed password attempts and lock outs.
		/// </summary>
		public void Authenticate(User user, string password)
		{
			var settings = IoC.Resolve<ISettingsService>();
			var users = IoC.Resolve<IUserService>();

			if (user == null)
				throw new ArgumentNullException("User");

			var rand = (int)Math.Floor(new Random().NextDouble() * 500);
			Thread.Sleep(rand);

			if (!user.IsActive || user.IsDeleted)
			{
				throw new ViewValidationException("Invalid Email Address or Password");
			}

			if (user.LockOutWindowStart.HasValue)
			{
				var minSinceLockOut = DateTime.UtcNow.Subtract(user.LockOutWindowStart.Value).Minutes;
				if (minSinceLockOut < settings.AccountLockOutMinutes)
				{
					var minLeft = settings.AccountLockOutMinutes - minSinceLockOut;
					throw new ViewValidationException(string.Format("Account currently locked. Try again in {0} minute{1}", minLeft, minLeft == 1 ? string.Empty : "s"));
				}
			}

			var salt = user.Salt;
			var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, salt);
			if (passwordHash.Equals(user.Password))
			{
				user.FailedPasswordAttemptCount = 0;
				user.FailedPasswordAttemptWindowStart = null;
				user.LockOutWindowStart = null;
				user.LastLoginDate = DateTime.UtcNow;
				user.LastActivityDate = DateTime.UtcNow;
				users.Update(user);
			}
			else
			{
				PasswordFailure(user);
				throw new ViewValidationException("Invalid Email Address or Password");
			}
		}

		/// <summary>
		/// Increments failed password attempt if below threshold, otherwise, locks account.
		/// </summary>
		public void PasswordFailure(User user)
		{
			var users = IoC.Resolve<IUserService>();
			var settings = IoC.Resolve<ISettingsService>();

			// failed password attempt window is open
			if (user.FailedPasswordAttemptWindowStart.HasValue &&
				(user.FailedPasswordAttemptWindowStart.Value.AddMinutes(settings.AccountLockOutMinutes) > DateTime.UtcNow))
			{
				user.FailedPasswordAttemptCount++;
				if (user.FailedPasswordAttemptCount >= settings.MaxPasswordAttemptCount)
				{
					user.LockOutWindowStart = DateTime.UtcNow;
					users.Update(user);
					throw new ViewValidationException(string.Format("Account currently locked. Try again in {0} minutes", settings.AccountLockOutMinutes));
				}
				users.Update(user);
			}
			else 
			{
				// start or reset the Window
				user.FailedPasswordAttemptWindowStart = DateTime.UtcNow;
				user.FailedPasswordAttemptCount = 1;
				user.LockOutWindowStart = null;
				users.Update(user);
			}
		}

		#endregion

		#region Account Retrieval

		public void RetrieveAccount(User user)
		{
			var accountRetrievalRequests = IoC.Resolve<ICrudService<AccountRetrievalRequest>>();
			var emails = IoC.Resolve<IEmailService>();
			var settings = IoC.Resolve<ISettingsService>();

			var token = Guid.NewGuid();
			var secondaryToken = Guid.NewGuid();
			var salt = BCrypt.Net.BCrypt.GenerateSalt();
			var hash = BCrypt.Net.BCrypt.HashPassword(token.ToString(), salt);
			var request = new AccountRetrievalRequest
				{
					User = user,
					DateIssued = DateTime.UtcNow,
					DateExpired = DateTime.UtcNow.AddDays(settings.AccountRequestExpirationDayCount),
					Salt = salt,
					TokenHash = hash,
					SecondaryToken = secondaryToken
				};
			accountRetrievalRequests.Create(request);

			var settingsSvc = IoC.Resolve<SettingsService>();
			var message = new MailMessage
				{
					IsBodyHtml = false,
					Subject = "BrightLine iQ Account Retrieval Instructions",
					Body = string.Format("Click on the link below to reset your account: {0} http://{1}/account/reset?p={2}&s={3}", Environment.NewLine, settings.AppURL, token, secondaryToken),
					From = new MailAddress(settingsSvc.MailSupportAddress, settingsSvc.MailSupportName)
				};
			message.To.Add(new MailAddress(user.Email));
			emails.SendEmail(message);
		}

		public AccountRetrievalRequest GetRetrievalRequest(string token, string secondaryToken)
		{
			var accountRetrievalRequests = IoC.Resolve<ICrudService<AccountRetrievalRequest>>();

			Guid secToken;
			if (!Guid.TryParse(secondaryToken, out secToken))
				throw new InvalidAccountRequestException("Reset link is invalid. Please make another request.");

			var result = accountRetrievalRequests.GetAll().SingleOrDefault(o => o.SecondaryToken.Equals(secToken));
			if (result == null || result.IsDeleted || result.DateRetrieved.HasValue)
				throw new InvalidAccountRequestException("Reset link is invalid. Please make another request.");

			if (DateTime.Today > result.DateExpired)
				throw new InvalidAccountRequestException("Reset link has expired. Please make another request.");

			var hash = BCrypt.Net.BCrypt.HashPassword(token, result.Salt);
			if (!result.TokenHash.Equals(hash))
				throw new InvalidAccountRequestException("Reset link is invalid. Please make another request.");

			return result;
		}

		#endregion

		#region Invitation

		public void CreateInvitation(User user, User author)
		{
			var users = IoC.Resolve<IUserService>();
			var settings = IoC.Resolve<ISettingsService>();
			var emails = IoC.Resolve<IEmailService>();
			var accountInvitations = IoC.Resolve<IRepository<AccountInvitation>>();

			// Expire any previously-issued and unused invitations
			var invites = accountInvitations.Where(o => o.InvitedUser.Id == user.Id).ToList();
			foreach (var invite in invites)
			{
				invite.DateExpired = DateTime.UtcNow;
				accountInvitations.Update(invite);
			}

			var token = Guid.NewGuid();
			var secondaryToken = Guid.NewGuid();
			string salt = null;
			var hash = CreateHash(token.ToString(), ref salt);
			var invitation = new AccountInvitation
				{
					InvitedUser = user,
					CreatingUser = author,
					DateIssued = DateTime.UtcNow,
					DateExpired = DateTime.UtcNow.AddDays(settings.InvitationExpirationDayCount),
					Salt = salt,
					TokenHash = hash,
					SecondaryToken = secondaryToken
				};
			accountInvitations.Insert(invitation);
			accountInvitations.Save();

			if (user.AccountInvitations == null)
				user.AccountInvitations = new List<AccountInvitation>();
			user.AccountInvitations.Add(invitation);
			users.Save();

			var message = new MailMessage
			{
				IsBodyHtml = false,
				Subject = "BrightLine iQ Account Activation Instructions",
				Body = string.Format("You have been invited to BrightLine iQ. {0}Click on the link below to activate your account: {0}http://{1}/account/activate?p={2}&s={3}", Environment.NewLine, settings.AppURL, token, secondaryToken),
				From = new MailAddress(settings.EmailFromAddress, settings.EmailFromName)
			};

			message.To.Add(new MailAddress(user.Email));
			emails.SendEmail(message);
		}

		public AccountInvitation GetInvitation(string token, string secondaryToken)
		{
			var accountInvitations = IoC.Resolve<IRepository<AccountInvitation>>();

			Guid secToken;
			if (!Guid.TryParse(secondaryToken, out secToken))
				throw new InvalidAccountRequestException("Activation link is invalid. Please request another invitation.");

			var result = accountInvitations.GetAll().SingleOrDefault(o => o.SecondaryToken.Equals(secToken));
			if (result == null || result.IsDeleted || result.DateActivated.HasValue)
				throw new InvalidAccountRequestException("Activation link is invalid.");

			if (result.DateExpired <= DateTime.UtcNow)
				throw new InvalidAccountRequestException("Activation link is invalid. Please request another invitation.");

			var hash = BCrypt.Net.BCrypt.HashPassword(token, result.Salt);
			if (!result.TokenHash.Equals(hash))
				throw new InvalidAccountRequestException("Activation link is invalid. Please request another invitation.");

			return result;
		}

		#endregion

		#region Update Password

		public void UpdatePassword(User user, string password, AccountRetrievalRequest request)
		{
			var accountRetrievalRequests = IoC.Resolve<ICrudService<AccountRetrievalRequest>>();

			this.UpdatePassword(user, password);
			request.DateRetrieved = DateTime.UtcNow;
			accountRetrievalRequests.Update(request);
		}

		public void SetInitialPassword(User user, string password, AccountInvitation invitation)
		{
			var accountInvitations = IoC.Resolve<IRepository<AccountInvitation>>();

			user.PasswordFormat = 0;
			user.IsActive = true;
			user.IsApproved = true;

			this.UpdatePassword(user, password);
			invitation.DateActivated = DateTime.UtcNow;
			accountInvitations.Update(invitation);
		}

		public void UpdatePassword(User user, string password)
		{
			var users = IoC.Resolve<IUserService>();

			HashPassword(user, password);
			using (var transaction = new TransactionScope())
			{
				UpdatePasswordHashHistory(user);

				// update the user
				user.LastPasswordChangedDate = user.LastActivityDate = DateTime.UtcNow;
				user.LockOutWindowStart = null;
				user.FailedPasswordAttemptCount = 0;
				users.Update(user);

				transaction.Complete();
			}
		}

		#endregion

		#region User profile

		public void UpdateProfile(UserViewModel model)
		{
			var users = IoC.Resolve<IUserService>();

			var user = users.Get(model.Id);
			if (user == null)
				throw new InvalidAccountRequestException("User information is invalid.");

			if (!string.IsNullOrEmpty(model.NewPassword))
			{
				var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password, user.Salt);
				if (!passwordHash.Equals(user.Password))
					throw new IllegalPasswordChangeException("Password is incorrect.");
			}

			using (var transaction = new TransactionScope())
			{
				user.FirstName = model.FirstName;
				user.LastName = model.LastName;
				user.TimeZoneId = model.TimeZoneId;

				users.Update(user);
				transaction.Complete();
			}

			if (!string.IsNullOrEmpty(model.NewPassword))
			{
				UpdatePassword(user, model.NewPassword);
			}
		}

		#endregion

		#region Helpers

		public string CreateHash(string raw, ref string salt)
		{
			if (string.IsNullOrEmpty(salt)) salt = BCrypt.Net.BCrypt.GenerateSalt();
			return BCrypt.Net.BCrypt.HashPassword(raw, salt);
		}

		#endregion

		#region Private methods

		private void HashPassword(User user, string password)
		{
			var users = IoC.Resolve<IUserService>();
			var settings = IoC.Resolve<ISettingsService>();

			// ensure User has not changed passwords more time in window than allowed
			var windowSpan = settings.PasswordChangeHourWindow;
			var windowBegin = DateTime.UtcNow.AddHours(-1 * windowSpan);
			var limit = settings.PasswordChangeLimit;

			var passwordChangesInWindow = user.PasswordHashes.Count(o => o.DateChanged < DateTime.UtcNow && o.DateChanged > windowBegin);
			if (passwordChangesInWindow > limit)
				throw new IllegalPasswordChangeException(string.Format("Password has been changed too many times in a {0} hour period. Please try again later.", windowSpan));

			var salt = user.Salt;

			var newPasswordWithOldSalt = CreateHash(password, ref salt);

			// ensure user has not used password before
			if (newPasswordWithOldSalt.Equals(user.Password) || user.PasswordHashes.Any(o => o.PasswordHash == newPasswordWithOldSalt))
				throw new IllegalPasswordChangeException("This password has already been used.");

			var newPasswordHash = CreateHash(password, ref salt);

			user.Password = newPasswordHash;
			user.Salt = salt;
			users.Update(user);
		}

		private void UpdatePasswordHashHistory(User user)
		{
			var settings = IoC.Resolve<ISettingsService>();
			var passwordHashHistories = IoC.Resolve<ICrudService<PasswordHashHistory>>();

			var limit = settings.PasswordHashHistoryLimit;
			var count = user.PasswordHashes.Count();

			if (count >= limit)
			{
				var numberToDelete = count - (count - limit + 2);

				var passwordHashHistoryToRemove = user.PasswordHashes.OrderBy(o => o.DateChanged).Take(numberToDelete);
				foreach (var hashHistory in passwordHashHistoryToRemove)
				{
					passwordHashHistories.Delete(hashHistory.Id);
				}

			}

			var passwordHashHistory = new PasswordHashHistory
			{
				DateChanged = DateTime.UtcNow,
				PasswordHash = user.Password,
				User = user
			};
			passwordHashHistories.Create(passwordHashHistory);
		}

		#endregion
	}
}
