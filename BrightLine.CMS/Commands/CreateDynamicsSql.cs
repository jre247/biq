using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.AppImport;
using BrightLine.CMS.Models;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Utility.Commands;
using BrightLine.Core;

namespace BrightLine.CMS.Commands
{
    public class CreateDynamicSqlCommand: Command
    {
        private Campaign _campaign;
        private AppImporter _importer;
        private ElementLookup _elementLookup; 


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="importer"></param>
        public CreateDynamicSqlCommand(Campaign campaign, AppImporter importer, ElementLookup lookup) : base("CMS", false)
        {
            _campaign = campaign;
            _importer = importer;
            _elementLookup = lookup;
        }


        /// <summary>
        /// Import spreadsheet using an existing importer.
        /// </summary>
        /// <returns></returns>
        protected override object ExecuteInternal(object[] args)
        {
			var campaignContentModels = IoC.Resolve<ICrudService<CampaignContentModel>>();
			var campaignContentModelProperties = IoC.Resolve<ICrudService<CampaignContentModelProperty>>();

            var schema = _importer.GetSchema();
            
            // 1. Go through each model
            foreach (var model in schema.Models.Models)
            {
                // 2. Check if schema has any meta fields ? need to put the values into the CampaignContentModelPropertyValue
                var metaFields = model.Schema.GetFields((prop) => prop.IsMetaType);

                if (metaFields != null && metaFields.Count > 0)
                {
                    var contentModel = _elementLookup.GetContentModel(model.Name);
                    var contentModelProperties = campaignContentModelProperties.Where(p => p.Model.Id == contentModel.Id).ToList();

                    var dynamicFields = "[Campaign_Id] [int] ";

                    // 3. Generate the dynamic sql.
                    // declare @FileName		as varchar(200)
	                // declare @Theme			as varchar(200)
	                // declare @YoutubeURL		as varchar(200)
	                // declare @BitlyURL		as varchar(200)
	                // declare @TotalRunTime	as int
                    var fieldDeclarations = "";

                    // Generate the dynamic field selections.
                    // select @FileName	     = PropertyStringVal 		from #tmpModelValues where CmsModelInstance_id = @instanceId and CmsModelProperty_Id = 1; 
		            // select @Theme		 = PropertyStringVal	 	from #tmpModelValues where CmsModelInstance_id = @instanceId and CmsModelProperty_Id = 2;
		            // select @YoutubeURL	 = PropertyStringVal	 	from #tmpModelValues where CmsModelInstance_id = @instanceId and CmsModelProperty_Id = 5; 
		            // select @BitlyURL	     = PropertyStringVal		from #tmpModelValues where CmsModelInstance_id = @instanceId and CmsModelProperty_Id = 6; 
		            // select @TotalRunTime  = PropertyNumberVal        from #tmpModelValues where CmsModelInstance_id = @instanceId and CmsModelProperty_Id = 7; 
                    var fieldSelections = "";

                    var fieldNamesInBrackets = "[Campaign_Id]";
                    var fieldNamesAsVariables = "@campaignId";
                    
                    foreach (var property in contentModelProperties)
                    {
                        int propId = property.Id;
                        fieldNamesInBrackets += ", [" + property.Name + "]";
                        fieldNamesAsVariables += ", @" + property.Name;

                        dynamicFields += ", ";
                        if (property.PropertyType.Name == "string")
                        {
                            dynamicFields += "[" + property.Name + "] [nvarchar](max) NULL";
                            fieldDeclarations += "declare @" + property.Name + " as [nvarchar](max)" + Environment.NewLine;
                            fieldSelections += "select @" + property.Name + " = StringValue from #tmpModelValues where Instance_id = @instanceId and Property_Id = " + propId.ToString() + ";" + Environment.NewLine;
                        }
                        else if (property.PropertyType.Name == "number")
                        {
                            dynamicFields += "[" + property.Name + "] [float] NULL";
                            fieldDeclarations += "declare @" + property.Name + " as [float]" + Environment.NewLine;
                            fieldSelections += "select @" + property.Name + " = NumberValue from #tmpModelValues where Instance_id = @instanceId and Property_Id = " + propId.ToString() + ";" + Environment.NewLine;
                        }
                        else if (property.PropertyType.Name == "bool")
                        {
                            dynamicFields += "[" + property.Name + "] [bit] NULL";
                            fieldDeclarations += "declare @" + property.Name + " as [bit]" + Environment.NewLine;
                            fieldSelections += "select @" + property.Name + " = BoolValue from #tmpModelValues where Instance_id = @instanceId and Property_Id = " + propId.ToString() + ";" + Environment.NewLine;
                        }
                        else if (property.PropertyType.Name == "datetime")
                        {
                            dynamicFields += "[" + property.Name + "] [datetime] NULL";
                            fieldDeclarations += "declare @" + property.Name + " as [datetime]" + Environment.NewLine;
                            fieldSelections += "select @" + property.Name + " = DateValue from #tmpModelValues where Instance_id = @instanceId and Property_Id = " + propId.ToString() + ";" + Environment.NewLine;
                        }
                    }

                    // Lastly generate the insert statements
                    // INSERT INTO #tmpModel 
		            // ([CampaignId],[FileName],[Theme],[DateAdded],[DateRemoved],[YoutubeURL],[BitlyURL],[TotalRunTime],[Display],[ShortDisplay])
		            //  VALUES
		            // ( @CampaignId, @FileName, @Theme, @DateAdded, @DateRemoved, @YoutubeURL, @BitlyURL, @TotalRunTime, @Display, @ShortDisplay )
                    var sqlQueryTemplated = GetSqlQueryTemplate();

                    sqlQueryTemplated = sqlQueryTemplated.Replace("{{cmsFieldDeclarations}}", fieldDeclarations);
                    sqlQueryTemplated = sqlQueryTemplated.Replace("{{cmsFieldSelections}}"  , fieldSelections);
                    sqlQueryTemplated = sqlQueryTemplated.Replace("{{cmsFieldNames}}"       , fieldNamesInBrackets);
                    sqlQueryTemplated = sqlQueryTemplated.Replace("{{cmsFieldVariables}}"   , fieldNamesAsVariables);

                    contentModel.SqlTemplateQuery = sqlQueryTemplated;
                    contentModel.SqlTemplateFields = dynamicFields;
                    campaignContentModels.Update(contentModel);
                }
            }
            return null;
        }


        private string _sqlQueryTemplate;
        private string GetSqlQueryTemplate()
        {
            if (!string.IsNullOrEmpty(_sqlQueryTemplate))
                return _sqlQueryTemplate;

            _sqlQueryTemplate = CmsHelper.GetEmbeddedResource("SqlTemplateQuery.sql");
            return _sqlQueryTemplate;
        }
    }
}
