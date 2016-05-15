using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Blueprints;
using BrightLine.Common.Utility.SqlServer;
using BrightLine.Common.ViewModels.Blueprints;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility
{
	public class BlueprintHelper : IBlueprintHelper
	{
		/// <summary>
		/// Run a sql script that will cascade delete a blueprint
		/// </summary>
		public void CascadeDeleteBlueprint(string name)
		{
			var blueprints = IoC.Resolve<IBlueprintService>();

			if (string.IsNullOrEmpty(name))
				IoC.Log.Error("Cascade Delete for Blueprint cannot run since the blueprint Manifest Name parameter is null.");

			var blueprint = blueprints.Where(b => b.ManifestName == name).SingleOrDefault();
			if (blueprint != null)
			{
				//TODO: think about refactoring DataAccess to have a method that isn't generic and doesn't require a return type
				var vm = new List<BlueprintViewModel>();
				var da = new BrightLine.Common.Utility.SqlServer.DataAccess(DataAccessConstants.OLTP_CONNECTION_STRING);
				da.AddParameter(BlueprintConstants.SqlProcedures.Params.BlueprintId, blueprint.Id, DbType.Int32);
				da.ExecuteReader<BlueprintViewModel>(BlueprintConstants.SqlProcedures.CascadeDeleteBlueprint, (@as) => vm = @as);
			}
		}

	}
}
