using Brightline.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.Platform;
using BrightLine.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Publishing.Constants;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Areas.AdResponses.Helpers
{
	public class RokuDestinationAdResponseHelper
	{
		#region Public Methods

		/// <summary>
		/// Get a mapped environment for a Roku Destination Ad Response
		/// </summary>
		/// <param name="targetEnv"></param>
		/// <returns></returns>
		public static string GetMappedEnvironment(string targetEnv)
		{
			string mappedEnvironment = null;

			switch (targetEnv)
			{
				case PublishConstants.TargetEnvironments.Develop:
					mappedEnvironment = PublishConstants.DestinationAdResponse.Roku.MappedEnvironments.Develop;
					break;
				case PublishConstants.TargetEnvironments.Uat:
					mappedEnvironment = PublishConstants.DestinationAdResponse.Roku.MappedEnvironments.Uat;
					break;
				case PublishConstants.TargetEnvironments.Production:
					mappedEnvironment = PublishConstants.DestinationAdResponse.Roku.MappedEnvironments.Production;
					break;
				default:
					throw new ArgumentException("Target Environment is not valid in Destination Ad Response Helper: " + targetEnv);
			}

			return mappedEnvironment;
		}

		#endregion
	}
}
