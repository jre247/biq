using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.ViewModels.Users;
using System.Linq;
using System.Web.Mvc;

namespace BrightLine.Web.Areas.Admin.Controllers
{
	public class UsersController : AdminController
	{
		public ActionResult Index(bool isInternal = false)
		{
			var usersRepo = IoC.Resolve<IUserService>();

			var users = usersRepo.GetAll(true, "Roles", "Advertiser", "MediaAgency");
			if(isInternal)
				users = users.Where(u => u.Internal);
			else
				users = users.Where(u => !u.Internal);

			var uvms = UserViewModel.FromUsers(users.ToArray());
			return View(uvms);
		}


	}
}
