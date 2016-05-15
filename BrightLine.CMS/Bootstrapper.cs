using BrightLine.CMS;
using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.CMS.Services.Publish;
using BrightLine.Common.Services;
using BrightLine.Core;
using SimpleInjector;

namespace BrightLine.Cms
{
	public static partial class Bootstrapper
	{
		public static void InitializeContainer(Container container)
		{

			#region OLTP CrudService Registrations

			container.Register<ICmsService, CmsService>();
			container.Register<IModelService, ModelService>();
			container.Register<IModelInstanceService, ModelInstanceService>();
			container.Register<IModelInstanceSaveServerPropertiesService, ModelInstanceSaveServerPropertiesService>();
			container.Register<ISettingService, SettingService>();
			container.Register<IModelInstancePublishedJsonService, ModelInstancePublishedJsonService>();
			container.Register<ISettingInstancePublishedJsonService, SettingInstancePublishedJsonService>();
			container.Register<IModelInstanceSaveService, ModelInstanceSaveService>();
			container.Register<IModelInstanceRetrievalService, ModelInstanceRetrievalService>();
			container.Register<ISettingInstanceService, SettingInstanceService>();
			container.Register<ISettingInstanceSaveService, SettingInstanceSaveService>();
			container.Register<IModelInstanceValidationService, ModelInstanceValidationService>();
			container.Register<ISettingInstanceValidationService, SettingInstanceValidationService>();
			container.Register<ISettingInstanceRetrievalService, SettingInstanceRetrievalService>();
			container.Register<IModelInstanceLookupsService, ModelInstanceLookupsService>();
			container.Register<ISettingInstanceLookupsService, SettingInstanceLookupsService>();

			#endregion


		}
	}
}