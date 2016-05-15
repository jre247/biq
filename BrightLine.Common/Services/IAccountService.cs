using BrightLine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Common.ViewModels.Account;

namespace BrightLine.Common.Services
{
	public interface IAccountService
	{
		void Authenticate(User user, string password);
		string CreateHash(string raw, ref string salt);
		void PasswordFailure(User user);
		void RetrieveAccount(User user);
		AccountRetrievalRequest GetRetrievalRequest(string token, string secondaryToken);
		void CreateInvitation(User user, User author);
		AccountInvitation GetInvitation(string token, string secondaryToken);
		void UpdatePassword(User user, string password);
		void UpdatePassword(User user, string password, AccountRetrievalRequest request);
		void SetInitialPassword(User user, string password, AccountInvitation invitation);
		void UpdateProfile(UserViewModel user);
	}
}
