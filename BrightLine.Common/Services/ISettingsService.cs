using BrightLine.Common.Models;
using BrightLine.Common.Utility.Enums;
using BrightLine.Core;
using System.Collections;
using System.Collections.Generic;

namespace BrightLine.Common.Services
{
	public interface ISettingsService
	{
		// convenience setting properties
		EnvironmentType CurrentEnvironment { get; }
		bool CachingEnabled { get; }
		ushort CacheDuration { get; }
		ushort MaxPasswordAttemptCount { get; }
		ushort AccountLockOutMinutes { get; }
		string AppURL { get; }
		ushort AccountRequestExpirationDayCount { get; }
		ushort InvitationExpirationDayCount { get; }
		string EmailFromName { get; }
		string EmailFromAddress { get; }
		ushort PasswordExpirationDayCount { get; }
		ushort PasswordChangeHourWindow { get; }
		ushort PasswordChangeLimit { get; }
		ushort PasswordHashHistoryLimit { get; }
		Dictionary<string, SettingValue> AllSettings { get; }
		void ClearCache();
		IRepository<Setting> Repo { get;}
		string MediaServerHost { get;  }
		string MediaServerUsername { get;  }
		int MediaServerPort { get;  }
		string PrivateSshKeyLocation { get; }
		string PrivateSshKeyPhrase { get;  }
		string MediaServerUploadRootLocation { get;  }
		string LocalFileUploadLocation { get; }
		string IqMaxVideoSize { get;}
		string IqMaxVideoDuration { get; }
		string MediaServerSshHost { get; }
		string NightwatchTestsReportLocation { get;}
		string GithubUserAgent { get; }
		string GithubUserAgentUsername { get; }
		string GithubUserAgentPassword { get; }
		string GithubRepositoryPagesPath { get; }
		string IntegrationServiceUsername { get;}
		string IntegrationServicePassword { get; }
		string BuildServerIp { get; }
		string IntegrationServicePort { get; }
		string MediaServerPassword { get;}
		string NightwatchServicePort { get;}
		string TemporaryResourcesDirectory { get;}
		int SftpTimeout { get; }
		string ChilKatUnlockCode { get;}
		int RabbitMQDefaultPort { get;}
		string RabbitMQUsername { get; }
		string RabbitMQPassword { get; }
		string RabbitMQHost { get;}
		string RabbitMQExchange { get;}
		string RabbitMQQueue { get; }
		string RabbitMQRoutingKey { get; }
		string SmtpHost { get; }
		int SmtpPort { get; }
		string SmtpUserName {get; }
		string SmtpPassword { get; }
		string MailSupportAddress { get; }
		string MailSupportName { get; }
		string CmsBucketName { get; }
		string CmsBaseUrl { get; }
		string CdnAccountId { get; }
		string CdnPurgeUri { get; }
		string CdnToken { get; }
		string AwsAccessId { get; }
		string AwsAccessKey { get; }
		string TrackingUrl { get; }
		string AdServerUrl { get;}
		string RedisClusterConnectionString { get; }
		string MainBrsFileUrl { get; }
		string RokuDirectIntegrationBrsFileUrl { get;}
		string MediaS3Bucket { get;}
		int RedisClusterDb { get;}
		string MediaCDNBaseUrl { get;}
		string MediaBaseUrl { get;}
		string MediaG1CDNBaseUrl { get;}
		string MediaG1BaseUrl { get;}
		int CMSVersion { get;}
		string MBList { get;}
	}
}
