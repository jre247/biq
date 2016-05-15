using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Enums;
using BrightLine.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace BrightLine.Service
{
	public class SettingsService : CrudService<Setting>, ISettingsService
	{
		#region Private Members

		private static Dictionary<string, SettingValue> _values;
		private static EnvironmentType _currentEnvironment;
		private const string SETTING_BOOLEAN_TYPE = "Bool";
		private const string SETTING_INTEGER_TYPE = "Integer";
		private const string SETTING_STRING_TYPE = "String";
		private static bool IsSettingsCached = false;

		#endregion

		#region Public Members

		public string Environment { get { return GetSettingValue<string>("Environment"); } }	
		public EnvironmentType CurrentEnvironment { get { return GetSettingValue<EnvironmentType>("Environment"); } }
		public bool CachingEnabled { get { return GetSettingValue<bool>("CacheEnabled"); } }
		public ushort CacheDuration { get { return GetSettingValue<ushort>("CacheDuration"); } }
		public ushort MaxPasswordAttemptCount { get { return GetSettingValue<ushort>("MaxPasswordAttemptCount"); }}
		public ushort AccountLockOutMinutes { get { return GetSettingValue<ushort>("AccountLockOutMinutes"); } }
		public string AppURL { get { return GetSettingValue<string>("AppURL"); }}
		public ushort AccountRequestExpirationDayCount { get { return GetSettingValue<ushort>("AccountRequestExpirationDayCount"); } }
		public ushort InvitationExpirationDayCount { get { return GetSettingValue<ushort>("InvitationExpirationDayCount"); } }
		public string EmailFromName { get { return GetSettingValue<string>("EmailFromName"); } }
		public string EmailFromAddress { get { return GetSettingValue<string>("EmailFromAddress"); } }
		public ushort PasswordExpirationDayCount { get { return GetSettingValue<ushort>("PasswordExpirationDayCount"); } }
		public ushort PasswordChangeHourWindow { get { return GetSettingValue<ushort>("PasswordChangeHourWindow"); } }
		public ushort PasswordChangeLimit { get { return GetSettingValue<ushort>("PasswordChangeLimit"); } }
		public ushort PasswordHashHistoryLimit { get { return GetSettingValue<ushort>("PasswordHashHistoryLimit"); } }
		public string MediaServerHost { get { return GetSettingValue<string>("MediaServerHost"); } }
		public string MediaServerUsername { get { return GetSettingValue<string>("MediaServerUsername"); } }
		public int MediaServerPort { get { return GetSettingValue<int>("MediaServerPort"); } }
		public string PrivateSshKeyLocation { get { return GetSettingValue<string>("PrivateSshKeyLocation"); } }
		public string PrivateSshKeyPhrase { get { return GetSettingValue<string>("PrivateSshKeyPhrase"); } }
		public string MediaServerUploadRootLocation { get { return GetSettingValue<string>("MediaServerUploadRootLocation"); } }
		public string LocalFileUploadLocation { get { return GetSettingValue<string>("LocalFileUploadLocation"); } }
		public string IqMaxVideoSize { get { return GetSettingValue<string>("IqMaxVideoSize"); } }
		public string IqMaxVideoDuration { get { return GetSettingValue<string>("IqMaxVideoDuration"); } }
		public string GithubUserAgent { get { return GetSettingValue<string>("GithubUserAgent"); } }
		public string GithubUserAgentUsername { get { return GetSettingValue<string>("GithubUserAgentUsername"); } }
		public string GithubUserAgentPassword { get { return GetSettingValue<string>("GithubUserAgentPassword"); } }
		public string GithubRepositoryPagesPath { get { return GetSettingValue<string>("GithubRepositoryPagesPath"); } }
		public string IntegrationServiceUsername { get { return GetSettingValue<string>("IntegrationServiceUsername"); } }
		public string IntegrationServicePassword { get { return GetSettingValue<string>("IntegrationServicePassword"); } }
		public string IntegrationServicePort { get { return GetSettingValue<string>("IntegrationServicePort"); } }
		public string BuildServerIp { get { return GetSettingValue<string>("BuildServerIp"); } }
		public string MediaServerPassword { get { return GetSettingValue<string>("MediaServerPassword"); } }
		public string NightwatchServicePort { get { return GetSettingValue<string>("NightwatchServicePort"); } }
		public string TemporaryResourcesDirectory { get { return GetSettingValue<string>("TemporaryResourcesDirectory"); } }
		public int SftpTimeout { get { return GetSettingValue<int>("SftpTimeout"); } }
		public string ChilKatUnlockCode { get { return GetSettingValue<string>("ChilKatUnlockCode"); } }
		public int RabbitMQDefaultPort { get { return GetSettingValue<int>("RabbitMQDefaultPort"); } }
		public string RabbitMQUsername { get { return GetSettingValue<string>("RabbitMQUsername"); } }
		public string RabbitMQPassword { get { return GetSettingValue<string>("RabbitMQPassword"); } }
		public string RabbitMQHost { get { return GetSettingValue<string>("RabbitMQHost");  } }
		public string RabbitMQExchange { get { return GetSettingValue<string>("RabbitMQExchange"); } }
		public string RabbitMQQueue { get { return GetSettingValue<string>("RabbitMQQueue"); } }
		public string RabbitMQRoutingKey { get { return GetSettingValue<string>("RabbitMQRoutingKey"); } }
		public string MediaServerSshHost { get { return GetSettingValue<string>("MediaServerSshHost"); } }
		public string NightwatchTestsReportLocation { get { return GetSettingValue<string>("NightwatchTestsReportLocation"); } }
		public string TrackingUrl { get { return GetSettingValue<string>("TrackingUrl"); } }
		public string AdServerUrl { get { return GetSettingValue<string>("AdServerUrl"); } }
		public string RedisHost { get { return GetSettingValue<string>("RedisHost"); } }
		public string RedisClusterConnectionString { get { return GetSettingValue<string>("RedisClusterConnectionString");  } }
		public string CdnToken { get { return GetSettingValue<string>("app.cdn.token"); } }
		public string AwsAccessId { get { return GetSettingValue<string>("amazons3.AccessId"); } }
		public string AwsAccessKey { get { return GetSettingValue<string>("amazons3.AccessKey"); } }
		public string SmtpHost { get { return GetSettingValue<string>("app.smtp.host"); } }
		public int SmtpPort { get { return GetSettingValue<int>("app.smtp.port"); } }
		public string SmtpUserName { get { return GetSettingValue<string>("app.smtp.userName");  } }
		public string SmtpPassword { get { return GetSettingValue<string>("app.smtp.password");  } }
		public string MailSupportAddress { get { return GetSettingValue<string>("app.mail.supportAddress"); } }
		public string MailSupportName { get { return GetSettingValue<string>("app.mail.supportName"); } }
		public string CmsBucketName { get { return GetSettingValue<string>("app.cms.s3Bucket"); } }
		public string CmsBaseUrl { get { return GetSettingValue<string>("app.cms.baseUrl"); } }
		public string CdnAccountId { get { return GetSettingValue<string>("app.cdn.accountId"); } }
		public string CdnPurgeUri
		{
			get
			{
				var baseUrl = GetSettingValue<string>("app.cdn.purgeUri");
				var purgeUri = string.Format(baseUrl, this.CdnAccountId);
				return (purgeUri);
			}
		}
		public string MainBrsFileUrl { get { return GetSettingValue<string>("MainBrsFileUrl"); } }
		public string RokuDirectIntegrationBrsFileUrl { get { return GetSettingValue<string>("RokuDirectIntegrationBrsFileUrl"); } }
		public string MediaS3Bucket { get { return GetSettingValue<string>("app.media.s3Bucket"); } }
		public int RedisClusterDb { get { return GetSettingValue<int>("redis.cluster.db"); } }
		public string MediaCDNBaseUrl { get { return GetSettingValue<string>("app.media.cdn.baseUrl"); } }
		public string MediaBaseUrl { get { return GetSettingValue<string>("app.media.baseUrl"); } }
		public string MediaG1CDNBaseUrl { get { return GetSettingValue<string>("app.media.g1.cdn.baseUrl"); } }
		public string MediaG1BaseUrl { get { return GetSettingValue<string>("app.media.g1.baseUrl"); } }
		public int CMSVersion { get { return GetSettingValue<int>("CMSVersion"); } }
		public string MBList { get { return GetSettingValue<string>("MBList"); } }

		public Dictionary<string, SettingValue> AllSettings { get { return _values; } }
		public IRepository<Setting> Repo { get { return base.Repository; } }

		#endregion

		#region Init

		public SettingsService(IRepository<Setting> repo)
			: base(repo)
		{
			if (!IsSettingsCached)
				BuildAllSettings();		
		}

		#endregion

		#region Public Methods

		public void ClearCache()
		{
			IsSettingsCached = false;
		}

		#endregion

		#region Private Methods

		private static T GetSettingValue<T>(string settingName)
		{
			try
			{
				if (_values[settingName] == null)
					IoC.Log.Error("The following Setting does not exist: " + settingName);

				// If Setting is int then need to parse the int value
				if (typeof(T) == typeof(int))
				{
					var settingValueParsed = int.Parse(_values[settingName].Value.ToString());
					return (T)Convert.ChangeType(settingValueParsed, typeof(T));
				}

				// Cast Setting to generic T for all other types other than int
				return (T)_values[settingName].Value;
			}
			catch(Exception ex)
			{
				IoC.Log.Error("There was an unexpected error when retrieving the following Setting: " + settingName, ex);
			}

			return default(T);
			
		}

		private void SetCache()
		{
			IsSettingsCached = true;
		}


		private Dictionary<string, SettingValue> BuildAllSettings()
		{
			_values = new Dictionary<string, SettingValue>();

			var appSettings = ConfigurationManager.AppSettings;

			BuildSettingsFromDatabase();

			BuildSettingsFromConfig(appSettings);

			SetCache();

			return _values;
		}

		private void BuildSettingsFromConfig(NameValueCollection appSettings)
		{
			var environment = appSettings.GetString("Environment");
			Enum.TryParse(environment, true, out _currentEnvironment);
			_values.Add("Environment", GetConfigSettingValueInstance(_currentEnvironment));

			var mediaServerPortValue = appSettings.GetInt("MediaServerPort");
			_values.Add("MediaServerPort", GetConfigSettingValueInstance(mediaServerPortValue));

			//loop through all AppSetting keys and add them to an in memory dictionary
			//	*note: skipping Environment and MediaServerPort, since those were manually added to the dictionary already, since they needed to have their values parsed to non-string types
			var excludeSettings = new List<string> { "Environment", "MediaServerPort" };
			var appSettingKeys = appSettings.Keys;
			foreach (var key in appSettingKeys)
			{
				var keyAsString = key.ToString();
				var appSettingValue = appSettings.Get(keyAsString);

				if (!excludeSettings.Contains(keyAsString) && !_values.ContainsKey(keyAsString))
					_values.Add(keyAsString, GetConfigSettingValueInstance(appSettingValue));
			}
		}

		private SettingValue GetConfigSettingValueInstance<T>(T value)
		{
			return new SettingValue { SettingOrigin = SettingOrigin.Config, Value = value };
		}

		private void BuildSettingsFromDatabase()
		{
			var dbSettings = base.Repository.Where(s => !s.IsDeleted).ToList();
			foreach (var dbSetting in dbSettings)
			{
				SettingValue settingValue;

				if (dbSetting.Type == SETTING_BOOLEAN_TYPE)
					settingValue = new SettingValue { SettingOrigin = SettingOrigin.Database, Value = bool.Parse(dbSetting.Value) };
				else if (dbSetting.Type == SETTING_INTEGER_TYPE)
					settingValue = new SettingValue { SettingOrigin = SettingOrigin.Database, Value = ushort.Parse(dbSetting.Value) };
				else
					settingValue = new SettingValue { SettingOrigin = SettingOrigin.Database, Value = dbSetting.Value };

				_values.Add(dbSetting.Key, settingValue);
			}
		}

		#endregion
	}
}