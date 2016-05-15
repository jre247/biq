using BrightLine.Common.Models;
using BrightLine.Common.ViewModels.Blueprints;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Core;
using BrightLine.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace BrightLine.Common.Services
{
	public interface IBlueprintService : ICrudService<Blueprint>
	{
		/// <summary>
		/// Get the latest version of each group blueprint.
		/// </summary>
		/// <param name="includeDeleted">Specifies whether to return the deleted (archived) blueprints.</param>
		/// <returns>a list of the latest blueprint versions.</returns>
		List<Blueprint> GetLatestVersions(bool includeDeleted = false);

		BlueprintViewModel GetViewModel(Blueprint Blueprint);
		BlueprintViewModel GetViewModel();
		void GetLookupsForBlueprint(BlueprintViewModel vm);
		Task<Blueprint> Save(BlueprintViewModel model, HttpRequestBase Request);
	}
}
