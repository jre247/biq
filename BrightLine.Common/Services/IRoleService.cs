using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Core;
using BrightLine.Common.Models;

namespace BrightLine.Common.Services
{
	public interface IRoleService : ICrudService<Role>
	{
		void ClearUserRoles(string email);
		ICollection<string> GetRoles(string email);
	}
}
