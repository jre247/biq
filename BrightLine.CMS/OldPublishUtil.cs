using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BrightLine.Utility;
using Newtonsoft.Json;

namespace BrightLine.CMS
{
	public class ModelSummary
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "data")]
		public string[] Data { get; set; }
	}

	public class CampaignPublishModel
	{
		[JsonProperty(PropertyName = "campaignName")]
		public string CampaignName { get; set; }
		[JsonProperty(PropertyName = "tag")]
		public string Tag { get; set; }
		[JsonProperty(PropertyName = "data")]
		public dynamic[] Data { get; set; }
	}

	public class RethinkDBSettings
	{
		public string Host { get; set; }
		public int Port { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string UpdateEndPoint { get; set; }

		public string Url
		{
			get
			{
				return string.Format("http://{0}", Host);
			}
		}

		public string UpdateUrl
		{
			get
			{
				return string.Format("http://{0}/{1}", Host, UpdateEndPoint);
			}
		}

		public string Auth
		{
			get
			{
				return Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", this.Username, this.Password)));
			}
		}
	}

	public class RethinkDBService
	{
		#region Fields

		private readonly RethinkDBSettings _settings;

		#endregion

		#region Initialization/Finalization

		public RethinkDBService(string env)
		{
			_settings = new RethinkDBSettings
			{
				Host = ConfigurationManager.AppSettings[string.Format("cms.rethinkdb.host.{0}", env.ToLower())],
				Username = ConfigurationManager.AppSettings[string.Format("cms.rethinkdb.writer.username.{0}", env.ToLower())],
				Password = ConfigurationManager.AppSettings[string.Format("cms.rethinkdb.writer.password.{0}", env.ToLower())],
				UpdateEndPoint = ConfigurationManager.AppSettings["cms.rethinkdb.updateEndpoint"]
			};
		}

		public RethinkDBService(RethinkDBSettings settings)
		{
			_settings = settings;
		}

		#endregion

		#region Public Methods

		public void InsertObjects(List<ModelSummary> objects)
		{
			// since the objects' data property is already a string, build the entire string to post to the server
			var data = GetPostObject(objects);
			try
			{
				var endpoint = new Uri(_settings.UpdateUrl);

				GetPOSTResponse(endpoint, data, null);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				throw;
			}
		}

		#endregion

		#region Private Methods

		private void GetPOSTResponse(Uri uri, string data, Action<string> callback)
		{
			var request = (HttpWebRequest)WebRequest.Create(uri);

			// timtout after 15s
			request.Timeout = 15000;

			request.Method = "POST";
			request.ContentType = "application/json";

			var encoding = new UTF8Encoding();
			var bytes = encoding.GetBytes(data);

			request.ContentLength = bytes.Length;

			using (var requestStream = request.GetRequestStream())
			{
				// Send the data
				requestStream.Write(bytes, 0, bytes.Length);
			}

			request.BeginGetResponse(x =>
			{
				using (var response = (HttpWebResponse)request.EndGetResponse(x))
				{
					if (callback != null)
					{
						callback(response.ToString());
					}
				}

			}, null);
		}


		private string GetPostObject(IEnumerable<ModelSummary> objects)
		{
			int count = 0;
			var sb = new StringBuilder();
			sb.Append("{ \"items\" : [ ");

			foreach (var item in objects)
			{
				sb.Append(count > 0 ? ", { " : " { ");

				sb.AppendFormat(" \"name\" : \"{0}\", ", item.Name);
				sb.AppendFormat(" \"data\" : [ {0} ] ", string.Join(",", item.Data));

				sb.Append("}");
				count++;
			}

			sb.Append("] }");

			return Regex.Replace(sb.ToString(), "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
		}

		#endregion
	}
}
