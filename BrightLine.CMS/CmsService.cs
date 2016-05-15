using System.Data;
using BrightLine.CMS.AppImport;
using BrightLine.CMS.Commands;
using BrightLine.CMS.Models;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.DataAccess;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using BrightLine.Utility;
using BrightLine.Utility.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using BrightLine.Common.ViewModels.Models;
using BrightLine.CMS.Services;
using BrightLine.Data;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Common.Services;
using System.IO;
using BrightLine.Service.External.AmazonStorage;
using System.Configuration;
using System.Net;
using System.Text;
using BrightLine.Common.Utility;

namespace BrightLine.CMS
{
	/// <summary>
	/// Main entry point to cms import and publishing
	/// </summary>
	public class CmsService : ICmsService
	{
		private Dictionary<string, CampaignContentModel> _modelMap;
		private IRepository<CampaignContentSchema> CampaignContentSchemas { get; set; }
		private IModelInstanceService ModelInstanceService { get; set; }


		public CmsService()
		{
			CampaignContentSchemas = IoC.Resolve<IRepository<CampaignContentSchema>>();
			ModelInstanceService = IoC.Resolve<IModelInstanceService>();
		}

		/// <summary>
		/// Import content from spreadsheet.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="contents"></param>
		public BoolMessageItem<List<string>> ValidateSpreadSheet(byte[] contents)
		{
			var importer = new AppImporter();
			return ValidateSpreadSheet(importer, "temp", contents);
		}


		public Campaign GetCampaign(int schemaId)
		{
			var campaigns = IoC.Resolve<ICampaignService>();

			var contentSchema = CampaignContentSchemas.Get(schemaId);
			var campaign = campaigns.Where(c => c.Id == contentSchema.Campaign.Id).FirstOrDefault();
			return campaign;
		}


		public List<CampaignContentModel> GetCampaignModels(int schemaId)
		{
			var campaignContentModels = IoC.Resolve<IRepository<CampaignContentModel>>();

			var query = campaignContentModels.Where(item => item.Schema.Id == schemaId);
			var all = query.OrderBy(o => o.ModelName).ToList();
			return all;
		}


		public List<CampaignContentModelInstance> GetCampaignInstances(int schemaId, string modelType = null)
		{
			var campaignContentModelInstances = IoC.Resolve<IRepository<CampaignContentModelInstance>>();

			var query = campaignContentModelInstances.Where(item => item.Schema.Id == schemaId);
			if (!string.IsNullOrWhiteSpace(modelType))
				query = query.Where(o => o.ModelName.Equals(modelType, StringComparison.InvariantCultureIgnoreCase));

			var all = query.OrderBy(o => o.ModelName).ThenByDescending(o => o.Key).ToList();
			return all;
		}


		/// <summary>
		/// Import content from spreadsheet.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="contents"></param>
		public BoolMessageItem<List<string>> ValidateSpreadSheet(AppImporter importer, string name, byte[] contents)
		{
			name = AppImporterHelper.MassageName(name);
			var result = importer.Import(name, contents);
			return result;
		}


		/// <summary>
		/// Import the spreadsheet.
		/// </summary>
		/// <param name="campaign">The campaign associated with the import.</param>
		/// <param name="importer"></param>
		/// <returns></returns>
		public BoolMessageItem<List<string>> UpdateSpreadSheet(Campaign campaign, AppImporter importer, int schemaId, string tag, bool createNew)
		{
			BoolMessageItem<List<string>> importResult;
			//var schema = CampaignContentSchemas.Get(schemaId);


			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, 15, 0)))
			{
				// Creating new schema ? ( this is to support  multiple sets of data for an experience )
				// e.g. we can 1 demo 1 working/dev version, 1 for user a, 1 for user b etc ).
				if (createNew)
				{
					var schema = new CampaignContentSchema { Tag = tag };
					CampaignContentSchemas.Insert(schema);
					CampaignContentSchemas.Save();
				}

				// Do the import
				importResult = ImportSpreadSheet(campaign, importer, schemaId);

				// Audit the action.
				//IoC.AuditEvents.Audit("CMS", "Import", string.Format("{0} - {1} ({2})", schema.Campaign.Name, schema.Tag, schema.Id));

				// Now complete the transaction.
				transaction.Complete();
			}

			try
			{
				var da = new DataAccess("OLTP");
				da.AddParameter("@campaignId", DbType.Int32, campaign.Id);
				da.ExecuteNonQuery(@"[dbo].[CMS_InsertVideos]");
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}



			return importResult;
		}


		/// <summary>
		/// Import spreadsheet after validating first.
		/// </summary>
		/// <param name="campaign">The campaign associated with the import.</param>
		/// <param name="contents"></param>
		/// <returns></returns>
		public BoolMessageItem<List<string>> ImportSpreadSheet(Campaign campaign, int schemaId, byte[] contents)
		{
			var importer = new AppImporter();
			var result = ValidateSpreadSheet(importer, campaign.Name, contents);

			// The importer collects AS MANY errors as it can in 1 go.
			// Return this result with success/failure flag and list of errors to display in UI.
			if (!result.Success)
				return result;

			return ImportSpreadSheet(campaign, importer, schemaId);
		}


		/// <summary>
		/// Import spreadsheet using an existing importer.
		/// </summary>
		/// <param name="campaign">The campaign associated with the import.</param>
		/// <param name="importer"></param>
		/// <returns></returns>
		public BoolMessageItem<List<string>> ImportSpreadSheet(Campaign campaign, AppImporter importer, int schemaId)
		{
			var commands = new CommandList();
			Stopwatch sw;
			// 1. Load up the existing models from the database
			// This is to know :
			// a. what records have been deleted from the sheet
			// b. what records are new
			// c. what records are existing ( for updates )
			sw = Stopwatch.StartNew();
			var modelLookup = new CmsModelLookup(campaign, schemaId);
			modelLookup.LoadModels(true);
			modelLookup.LoadModelInstances();
			IoC.Log.TraceFormat("model lookup built: {0}ms", sw.ElapsedMilliseconds);

			// 2. Get the unique set of ids/keys from both excel, database ( to later on determine a delta of new / removed items )
			sw = Stopwatch.StartNew();
			var appSchema = importer.GetSchema();
			var modelSetFromDatabase = modelLookup.GetModelSet();
			var modelSetFromExcel = appSchema.GetModelSet();
			IoC.Log.TraceFormat("appSchema found out: {0}ms", sw.ElapsedMilliseconds);

			// 3. Creating the models / instances and json data is a 2 step process.
			// Create data command only creates the models/instances ( to create row ids ), but does NOT build up
			// and set the json data on the instance. This is because the ids of the rows of models/instances are 
			// needed to populate the json data so the ids can be used for reporting / anlaytics on the client side!!!
			sw = Stopwatch.StartNew();
			commands.Execute(new CreateModelDataCommand(campaign, importer, modelLookup, schemaId));
			_modelMap = commands.Last.LastResult.Result as Dictionary<string, CampaignContentModel>;
			IoC.Log.TraceFormat("CreateModelDataCommand: {0}ms", sw.ElapsedMilliseconds);

			// This can't really happen ( command.cs will log and rethrow error ).
			if (_modelMap == null)
				throw new CmsValidationException() { Errors = new List<string>() { "Error saving models" } };

			var schema = importer.GetSchema();
			var models = schema.Models;

			// 4. Create the elment lookup to get the primarykey/id of each model instance based on the key field from excel.
			// TODO: (CMS, LOOKUP, REFACTOR) - The elementlookup class can be removed and the modelLookup can be used exclusively.
			// in the functionality.... but modellookup
			sw = Stopwatch.StartNew();
			var elementLookup = new ElementLookup(schema, _modelMap, schemaId);
			elementLookup.InitLookups();
			IoC.Log.TraceFormat("elementLookup.InitLookups: {0}ms", sw.ElapsedMilliseconds);

			// 5. Each model could have it's own "implicit" models which are basically models in the short-hand style of a property.
			sw = Stopwatch.StartNew();
			commands.Execute(new CreateImplicitModelsCommand(importer.GetSchema(), modelLookup, elementLookup));
			IoC.Log.TraceFormat("CreateImplicitModelsCommand: {0}ms", sw.ElapsedMilliseconds);

			// 6. Now go through all the models and generate the json data now and update.
			sw = Stopwatch.StartNew();
			commands.Execute(new CreateModelDataJsonCommand(campaign, importer, elementLookup, schemaId));
			IoC.Log.TraceFormat("CreateModelDataJsonCommand: {0}ms", sw.ElapsedMilliseconds);

			// 7. Now populate the property values for metadata fields
			sw = Stopwatch.StartNew();
			commands.Execute(new CreateModelDataVerticalCommand(campaign, importer, elementLookup, schemaId, modelLookup));
			IoC.Log.TraceFormat("CreateModelDataVerticalCommand: {0}ms", sw.ElapsedMilliseconds);

			// 8. Generate the dynamic sql 
			sw = Stopwatch.StartNew();
			commands.Execute(new CreateDynamicSqlCommand(campaign, importer, elementLookup));
			IoC.Log.TraceFormat("CreateDynamicSqlCommand: {0}ms", sw.ElapsedMilliseconds);

			// 9. Now handle the delta between excel vs database ( e.g. removed deleted items )
			sw = Stopwatch.StartNew();
			commands.Execute(new DeleteDataCommand(schemaId, appSchema, modelSetFromDatabase, modelSetFromExcel));
			IoC.Log.TraceFormat("DeleteDataCommand: {0}ms", sw.ElapsedMilliseconds);

			// 10. Update the summary of the schema ( total models, lookups ).
			sw = Stopwatch.StartNew();
			sw = Stopwatch.StartNew();
			var contentSchema = CampaignContentSchemas.Get(schemaId);
			contentSchema.TotalModels = appSchema.Models == null ? 0 : appSchema.Models.Count;
			contentSchema.TotalLookups = appSchema.Lookups == null ? 0 : appSchema.Lookups.Count;
			CampaignContentSchemas.Save();
			IoC.Log.TraceFormat("CampaignContentSchemas.Update: {0}ms", sw.ElapsedMilliseconds);

			//// 11. Collect the benchmarks for all the commands.
			//var benchmarks = commands.GetBenchmarksAsString();
			//Log.Info("CMS import for schema id : " + schemaId + " with benchmark: " + Environment.NewLine + benchmarks);

			return new BoolMessageItem<List<string>>(true, "Save application", null);
		}

		/// <summary>
		/// Publishes data to a specified environment.
		/// </summary>
		/// <param name="campaign"></param>
		/// <param name="schema"></param>
		/// <param name="env"></param>
		public void Publish(Campaign campaign, CampaignContentSchema schema, string env)
		{
			if (string.IsNullOrWhiteSpace(env)) 
				throw new ArgumentNullException("env");

			var auditEvents = IoC.Resolve<IAuditEventService>();

			auditEvents.Audit("CMS", "Publish", string.Format("{0} - {1} ({2}) - {3} ", schema.Campaign.Name, schema.Tag, schema.Id, env));

			//TODO: BL404 Remove PublishOld and associated code when no longer pushing to RethinkDB
			PublishOld(campaign, schema, env);

			var svc = IoC.Resolve<ICrudService<CampaignContentModelInstance>>();
			var settingsSvc = IoC.Resolve<ISettingsService>();
			var bucketName = IoC.Resolve<ISettingsService>().CmsBucketName;

			var s3Settings = new AmazonS3Settings()
			{
				AccessId = settingsSvc.AwsAccessId,
				AccessKey = settingsSvc.AwsAccessKey,
				Bucket = bucketName
			};

			var s3Service = new AmazonS3Service(s3Settings);

			// All instances of all models.
			var items = svc.Where(instance => instance.Schema.Id == schema.Id && !instance.IsDeleted && instance.IsPublishable).ToList();

			// Group into models.
			var models = items.GroupBy(item => item.ModelName);

			// Store each model's key and a JArray of the instances to publish
			var modelsToPublish = new Dictionary<string, JArray>();

			foreach (var model in models)
			{
				// Create a key in the following format: environment/campaignId/modelName
				var keyName = env + "/" + schema.Campaign.CmsKey + "/" + model.Key.ToLower();
				var instances = model.Select(o => o.InstanceDataJson).ToArray();

				var toPublish = new JArray();
				
				foreach (var instance in instances)
				{
					toPublish.Add(JsonConvert.DeserializeObject(instance));
				}

				modelsToPublish.Add(keyName, toPublish);
			}

			// Publish each of the models its instances
			foreach (var model in modelsToPublish)
			{
				var stream = new MemoryStream();
				var writer = new StreamWriter(stream);

				var val = JsonConvert.SerializeObject(model.Value);

				writer.Write(val);
				writer.Flush();
				stream.Position = 0;

				s3Service.UploadSpecificContentType("application/json", stream, bucketName, model.Key);
			}

			// Update the schema to reflect the publish
			schema.IsPublished = true;
			schema.LastPublishEnvironment = env;
			schema.LastPublishedDate = DateTime.UtcNow;
			schema.ChangeComment = Auth.UserName;

			var schemaSvc = IoC.Resolve<ICrudService<CampaignContentSchema>>();
			schemaSvc.Update(schema);

			// Purge CDN
			var endpoint = IoC.Resolve<ISettingsService>().CmsBaseUrl + "/" + env + "/" + campaign.Id;
			PurgeCdnEndpoint(endpoint, true);
		}

		public BoolMessageItem SaveModelInstance(ModelInstanceSaveViewModel viewModel)
		{
			try
			{			
				var boolMessage = ModelInstanceService.Save(viewModel);

				if (boolMessage.Success)
				{
					if (OLTPContextSingleton.Instance != null)
						OLTPContextSingleton.Instance.SaveChanges();
				}
					

				return boolMessage;
				
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				return new BoolMessageItem(false, "uncaught exception occured in Cms Service");
			}
		}

		public BoolMessageItem SaveSettingInstance(ModelInstanceSaveViewModel viewModel)
		{
			try
			{
				var settingInstanceService = IoC.Resolve<ISettingInstanceService>();

				var boolMessage = settingInstanceService.Save(viewModel);

				if (boolMessage.Success)
				{
					if (OLTPContextSingleton.Instance != null)
						OLTPContextSingleton.Instance.SaveChanges();
				}


				return boolMessage;

			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				return new BoolMessageItem(false, "uncaught exception occured in Cms Service");
			}
		}	


		public JObject GetModelInstance(int modelInstanceId)
		{
			var modelInstance = ModelInstanceService.Get(modelInstanceId);

			return modelInstance;
		}

		public CreativeFeaturesViewModel GetModelsForCreative(int creativeId)
		{
			var modelService = IoC.Resolve<IModelService>();
			var models = modelService.GetModelsForCreative(creativeId);

			return models;
		}

		public CreativeFeaturesViewModel GetSettingsForCreative(int creativeId)
		{
			var settingService = IoC.Resolve<ISettingService>();
			var settings = settingService.GetSettingsForCreative(creativeId);

			return settings;
		}

		public Dictionary<int, ModelInstanceListViewModel> GetModelInstancesForModel(int modelId, bool verbose)
		{
			var modelInstanceService = IoC.Resolve<IModelInstanceService>();
			return modelInstanceService.GetModelInstancesForModel(modelId, verbose);
		}

		public Dictionary<int, ModelInstanceListViewModel> GetSettingInstancesForSetting(int settingId)
		{
			var settingInstanceService = IoC.Resolve<ISettingInstanceService>();
			return settingInstanceService.GetSettingInstancesForSetting(settingId);
		}

		public JObject GetSettingInstance(int settingInstanceId)
		{
			var settingInstanceService = IoC.Resolve<ISettingInstanceService>();
			var settingInstance = settingInstanceService.Get(settingInstanceId);

			return settingInstance;
		}

		#region Private Method

		/// <summary>
		/// Purges the CDN for a given endpoint
		/// </summary>
		/// <param name="endpoint">The CDN endpoint to clear</param>
		/// <param name="recursive">Whether the purge should apply recusively (i.e. clear child directories)</param>
		/// <returns></returns>
		private void PurgeCdnEndpoint(string endpoint, bool recursive = true)
		{
			var settingsSvc = IoC.Resolve<ISettingsService>();

			
			var token = settingsSvc.CdnToken;

			var purgeURI = settingsSvc.CdnPurgeUri;

			var query = new JObject();
			var list = new JArray();
			query["list"] = list;

			var item = new JObject();
			item["url"] = endpoint;
			item["recursive"] = recursive;
			list.Add(item);			

			var request = (HttpWebRequest)WebRequest.Create(purgeURI);
			request.Headers.Add("Authorization", "Bearer " + token);
			request.ContentType = "application/json";
			request.Method = "POST";


			// The Highwinds API does not comply with the default
			// WebRequest behavior of sending an initial request
			// and expecting a 100 (Continue).
			request.ServicePoint.Expect100Continue = false;

			var encoding = new ASCIIEncoding();
			var bytes = encoding.GetBytes(query.ToString(Formatting.None));

			request.ContentLength = bytes.Length;

			using (var requestStream = request.GetRequestStream())
			{
				requestStream.Write(bytes, 0, bytes.Length);
			}

			request.GetResponse();
		}


		private void PublishOld(Campaign campaign, CampaignContentSchema schema, string env = null)
		{
			var auditEvents = IoC.Resolve<IAuditEventService>();

			if (string.IsNullOrWhiteSpace(env)) env = Env.Name;

			auditEvents.Audit("CMS", "Publish", string.Format("{0} - {1} ({2}) - {3} ", schema.Campaign.Name, schema.Tag, schema.Id, env));

			var svc = IoC.Resolve<ICrudService<CampaignContentModelInstance>>();

			var rethinkdbService = new RethinkDBService(env);

			// All instances of all models.
			var items = svc.Where(instance => instance.Schema.Id == schema.Id && !instance.IsDeleted && instance.IsPublishable).ToList();

			// Group into models.
			var models = items.GroupBy(item => item.ModelName);
			var data = new List<ModelSummary>();

			foreach (var model in models)
			{
				var keyName = schema.Campaign.CmsKey + "_" + model.Key.ToLower();
				var instances = model.Select(o => o.InstanceDataJson).ToArray();
				var obj = new ModelSummary
				{
					Name = keyName,
					Data = instances
				};

				data.Add(obj);
			}

			rethinkdbService.InsertObjects(data);

			// Finally set the status to published.
			schema.IsPublished = true;
			schema.LastPublishEnvironment = env;
			schema.LastPublishedDate = DateTime.UtcNow;
			schema.ChangeComment = Auth.UserName;

			var schemaSvc = IoC.Resolve<ICrudService<CampaignContentSchema>>();
			schemaSvc.Update(schema);
		}


		#endregion

	}
}


