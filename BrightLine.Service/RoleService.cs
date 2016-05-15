using System.Collections.Generic;
using System.Linq;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Core;
using BrightLine.Utility;
using System;
using System.Collections.ObjectModel;

namespace BrightLine.Service
{
	public class RoleService : CrudService<Role>, IRoleService
	{
		private const string CacheKey = "UserRole_{0}";
		private static ICollection<string> EmptyRoles = new Collection<string> { "Guest" };
		private ISettingsService Settings { get;set;}

		public RoleService(IRepository<Role> repo)
			: base(repo)
		{ 
			Settings = IoC.Resolve<ISettingsService>();
		}

		public void ClearUserRoles(string email)
		{
			var key = GetCacheKey(email);
			if (Settings.CachingEnabled)
				IoC.Cache.Remove(key);
		}

		/// <summary>
		/// Gets roles for a given user by email
		/// </summary>
		public ICollection<string> GetRoles(string email)
		{
			var users = IoC.Resolve<IUserService>();

			var key = GetCacheKey(email);
			if (Settings.CachingEnabled)
			{
				var cached = IoC.Cache.Get(key);
				if (cached != null)
					return (ICollection<string>)cached;
			}

			var user = users.Where(u => u.Email.Equals(email)).FirstOrDefault();
			if (user == null)
				return EmptyRoles;

			var returnValue = user.Roles.Any() ? user.Roles.Select(o => o.Name).ToList() : EmptyRoles;
			if (Settings.CachingEnabled)
				IoC.Cache.Add(key, returnValue, TimeSpan.FromMinutes(Settings.CacheDuration));

			return returnValue;
		}

		private string GetCacheKey(string email)
		{
			return string.Format(CacheKey, email);
		}
	}
}
