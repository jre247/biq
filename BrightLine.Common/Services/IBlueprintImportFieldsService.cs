using BrightLine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface IBlueprintImportFieldsService
	{
		void CreateFields(BlueprintImportModel model, Dictionary<string, BlueprintImportModelDefinition> modelDefinitionDictionary, CmsModelDefinition modelDefinition = null, CmsSettingDefinition settingDefinition = null);
	}
}
