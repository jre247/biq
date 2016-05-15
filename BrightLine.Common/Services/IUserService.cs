using BrightLine.Common.Models;
using BrightLine.Core;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace BrightLine.Common.Services
{
	public interface IUserService : ICrudService<User>
	{
		/// <summary>
		/// Adds a campaign to the user's list of favorites.
		/// </summary>
		/// <param name="user">The user to edit.</param>
		/// <param name="campaign">The campaign to add/remove from the user's favorites.</param>
		/// <returns>true on success, false otherwise.</returns>
		bool ToggleFavorite(User user, Campaign campaign);
		
		/// <summary>
		/// Gets a lookup of user id to first/lastname.
		/// </summary>
		/// <returns></returns>
		Dictionary<int, string> GetUserFullNames();
		
		User GetUserByEmail(string email);

		User SaveUser(int id, string jsonData);

		JObject GetUserInfo(User user);

		void AuditLogin(User user);
		void AuditActivity(User user);

		bool IsPasswordExpired(User user);
	}
}
