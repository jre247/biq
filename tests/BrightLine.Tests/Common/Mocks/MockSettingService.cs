using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Tests.Common.Mocks
{
	public class MockSettingService : ISettingsService
	{
		public string LocalFileUploadLocation { get 
			{ 
				return "~/AppData";
			} 
		}

		public BrightLine.Common.Utility.Enums.EnvironmentType CurrentEnvironment
		{
			get { throw new NotImplementedException(); }
		}

		public bool CachingEnabled
		{
			get { return false;}
		}

		public ushort CacheDuration
		{
			get { throw new NotImplementedException(); }
		}

		public ushort MaxPasswordAttemptCount
		{
			get { throw new NotImplementedException(); }
		}

		public ushort AccountLockOutMinutes
		{
			get { throw new NotImplementedException(); }
		}

		public string AppURL
		{
			get { return "http://abc"; }
		}

		public ushort AccountRequestExpirationDayCount
		{
			get { throw new NotImplementedException(); }
		}

		public ushort InvitationExpirationDayCount
		{
			get { return 10; }
		}

		public string EmailFromName
		{
			get { return "abc"; }
		}

		public string EmailFromAddress
		{
			get { return "abc@gmail.com"; }
		}

		public ushort PasswordExpirationDayCount
		{
			get { return 10; }
		}

		public ushort PasswordChangeHourWindow
		{
			get { throw new NotImplementedException(); }
		}

		public ushort PasswordChangeLimit
		{
			get { throw new NotImplementedException(); }
		}

		public ushort PasswordHashHistoryLimit
		{
			get { throw new NotImplementedException(); }
		}

		public Dictionary<string, SettingValue> AllSettings
		{
			get { throw new NotImplementedException(); }
		}

		public void ClearCache()
		{
			throw new NotImplementedException();
		}

		public IRepository<Setting> Repo
		{
			get { throw new NotImplementedException(); }
		}

		public string MediaServerHost
		{
			get { return "Test"; }
		}

		public string MediaServerUsername
		{
			get { return "Test"; }
		}

		public int MediaServerPort
		{
			get { return 1; }
		}

		public string PrivateSshKeyLocation
		{
			get { return "Test"; }
		}

		public string PrivateSshKeyPhrase
		{
			get { return "Test"; }
		}

		public string MediaServerUploadRootLocation
		{
			get { return "Test"; }
		}


		public string IqMaxVideoSize
		{
			get { return "1000000"; }
		}

		public string IqMaxVideoDuration
		{
			get { return "60000"; }
		}

		public string MediaServerSshHost
		{
			get { throw new NotImplementedException(); }
		}


		public string NightwatchTestsReportLocation
		{
			get { throw new NotImplementedException(); }
		}


		public string GithubUserAgent
		{
			get 
			{ 
				return "BrightLine-iTV";
			}
		}

		public string GithubUserAgentUsername
		{
			get 
			{
				return "brightlinetesting";
			}
		}

		public string GithubUserAgentPassword
		{
			get
			{
				return "bcdrDKmJZD4frhM";
			}
		}

		public string GithubRepositoryPagesPath
		{
			get { return "_pages.json"; }
		}


		public string IntegrationServiceUsername
		{
			get { throw new NotImplementedException(); }
		}

		public string IntegrationServicePassword
		{
			get { throw new NotImplementedException(); }
		}

		public string BuildServerIp
		{
			get { throw new NotImplementedException(); }
		}

		public string IntegrationServicePort
		{
			get { throw new NotImplementedException(); }
		}


		public string MediaServerPassword
		{
			get { return "Test"; }
		}


		public string NightwatchServicePort
		{
			get { throw new NotImplementedException(); }
		}


		public string TemporaryResourcesDirectory
		{
			get { throw new NotImplementedException(); }
		}

		public int SftpTimeout
		{
			get { throw new NotImplementedException(); }
		}

		public string ChilKatUnlockCode
		{
			get { throw new NotImplementedException(); }
		}


		public string Environment
		{
			get { return "DEV"; }
		}


		public string AWSS3BaseUrl
		{
			get { return "https://s3.amazonaws.com"; }
		}


		public int RabbitMQDefaultPort
		{
			get { throw new NotImplementedException(); }
		}




		public string RabbitMQUsername
		{
			get { throw new NotImplementedException(); }
		}

		public string RabbitMQPassword
		{
			get { throw new NotImplementedException(); }
		}

		public string RabbitMQHost
		{
			get { throw new NotImplementedException(); }
		}

		public string RabbitMQExchange
		{
			get { throw new NotImplementedException(); }
		}

		public string RabbitMQQueue
		{
			get { throw new NotImplementedException(); }
		}

		public string RabbitMQRoutingKey
		{
			get { throw new NotImplementedException(); }
		}


		public string SmtpHost
		{
			get { throw new NotImplementedException(); }
		}

		public int SmtpPort
		{
			get { throw new NotImplementedException(); }
		}

		public string SmtpUserName
		{
			get { throw new NotImplementedException(); }
		}

		public string SmtpPassword
		{
			get { throw new NotImplementedException(); }
		}

		public string MailSupportAddress
		{
			get { throw new NotImplementedException(); }
		}

		public string MailSupportName
		{
			get { throw new NotImplementedException(); }
		}


		public string CmsBucketName
		{
			get { throw new NotImplementedException(); }
		}


		public string CmsBaseUrl
		{
			get { return "dev-cms.brightline.com"; }
		}

		public string CdnAccountId
		{
			get { throw new NotImplementedException(); }
		}

		public string CdnPurgeUri
		{
			get { throw new NotImplementedException(); }
		}

		public string CdnToken
		{
			get { throw new NotImplementedException(); }
		}


		public string AwsAccessId
		{
			get { throw new NotImplementedException(); }
		}

		public string AwsAccessKey
		{
			get { throw new NotImplementedException(); }
		}


		public string TrackingUrl
		{
			get { return "brightline.tracking.com/track"; }
		}

		public string RedisHost
		{
			get { throw new NotImplementedException(); }
		}


		public string RedisClusterConnectionString
		{
			get { throw new NotImplementedException(); }
		}


		public string MainBrsFileUrl
		{
			get { throw new NotImplementedException(); }
		}

		public string RokuDirectIntegrationBrsFileUrl
		{
			get { throw new NotImplementedException(); }
		}


		public string MediaS3Bucket
		{
			get { return "local-m.brightline.tv"; }
		}


		public string AdServerUrl
		{
			get { return "http://dev-as.brightline.tv"; }
		}

		public int RedisClusterDb
		{
			get { throw new NotImplementedException(); }
		}


		public string MediaCDNBaseUrl
		{
			get { return "//cdn-local-m.brightline.tv"; }
		}


		public string MediaBaseUrl
		{
			get { return "http://cdn-local-m.brightline.tv"; }
		}

		public string MediaG1CDNBaseUrl
		{
			get { return "http://cdn-media.brightline.tv"; }
		}

		public string MediaG1BaseUrl
		{
			get { throw new NotImplementedException(); }
		}


		public int CMSVersion
		{
			get { return 2; }
		}

		public string MBList
		{
			get { return "2450X,2500X"; }
		}
	}
}
