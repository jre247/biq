using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.Models;
using BrightLine.CMS.Serialization;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;

namespace BrightLine.CMS
{
    public class CmsHelper
    {
        public static void ConfigureBaseType(DataModelSchema model, CampaignContentModel contentModel, IDictionary<string, CampaignContentModelBaseType> modelBaseTypes)
        {
            if (!string.IsNullOrEmpty(model.BaseType))
            {
                var baseType = model.BaseType.ToLower();
                if (modelBaseTypes.ContainsKey(baseType))
                    contentModel.BaseType = modelBaseTypes[baseType];
            }
        }


        public static string BuildInstanceFileName(int campaignId, CampaignContentModelInstance instance, bool isRawFormat)
        {
            var filename = string.Format("campaign_{0}_schema_{1}_{2}_{3}", campaignId, instance.Schema.Id, instance.ModelName, instance.Key);
            if(isRawFormat)
                return filename + "_raw.json";

            return filename + "_pub.json";
        }


        public static string BuildModelFileName(int campaignId, CampaignContentModel model, bool isRawFormat)
        {
            var filename = string.Format("campaign_{0}_schema_{1}_{2}", campaignId, model.Schema.Id,  model.ModelName);
            var publishedFileName = filename + "_pub.json";
            return publishedFileName;
        }


        public static CampaignContentSchema PopulateSchemaItem22(CampaignContentSchema schema, AppSchema appSchema)
        {
            schema.TotalLookups = appSchema.Lookups.Count;
            schema.TotalModels = appSchema.Models.Count;
            schema.IsPublished = false;
            schema.DataSchema = string.Empty;// jsonData.Schema;
            schema.DataModelsRaw = string.Empty;//jsonData.DataRaw;
            schema.DataModelsPublished = string.Empty;//jsonData.Data;
            return schema;
        }


        private const string DllName = "BrightLine.CMS";


        /// <summary>
        /// e.g. Csv\Csv1.csv.
        /// Where Csv is the folder under the Content folder.
        /// </summary>
        /// <param name="filePath">\Csv\Csv1.csv</param>
        /// <returns></returns>
        public static string GetEmbeddedResource(string filePath)
        {
            // 1. Create the path to the embedded resource based on the dll name.
            // NOTE: Folder names are separated by a ".".
            string path = string.Format(DllName + "._Resources.{0}", filePath);

            // 2. Get current assembly
            Assembly current = Assembly.GetExecutingAssembly();

            // 3. Get the path as a stream.
            Stream stream = current.GetManifestResourceStream(path);
            if (stream == null) throw new FileNotFoundException(path);

            // 4. Load the content.
            StreamReader reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
            return content;
        }
    }
}
