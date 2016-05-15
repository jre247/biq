using BrightLine.Common.Models;
using BrightLine.Common.ViewModels.Blueprints;
using BrightLine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface IBlueprintImportService
	{
		Task<BoolMessageItem> ImportBlueprint(int? blueprintId, string blueprintManifestName);
	}
}
