using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using BrightLine.Common.Framework;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.AdFunction;
using BrightLine.Common.Utility.Platform;

namespace BrightLine.Service
{
	public class AdValidationService : IAdValidationService
	{
		public void ValidateAd(ValidationException vex, Ad ad)
		{
			// check for name is not null and name.length <=255
			ValidateAdName(vex, ad);

			ValidateAdPlatform(vex, ad);

			ValidateAdType(vex, ad);

			ValidateAdCreative(vex, ad);

			ValidateAdEndDate(ad);

			ValidateAdBeginDate(vex, ad);

			ValidateDeliveryGroup(ad, vex);

			ValidateCoordinates(vex, ad);
				
			//can only save companion_ad_id if ad type is commercial spot
			if (ad.CompanionAd != null)
				ValidateCompanionAd(ad, vex);
	
			// TODO: Think about if this check here is necessary
			//ValidateDestinationCreative(ad, vex);
		}

		

		#region Private Methods

		private void ValidateAdName(ValidationException vex, Ad ad)
		{
			if (string.IsNullOrEmpty(ad.Name))
				vex.Add("Ad name must be provided.");
			else if (ad.Name.Length > 255)
				vex.Add("Ad name must be less than 255 characters.");
			else if (ad.Campaign.Ads.Any(a => a.Id != ad.Id &&
				!string.IsNullOrEmpty(a.Name) && a.Name.ToLowerInvariant() == ad.Name.ToLowerInvariant()))
				vex.Add("Ad name must be unique in this campaign.");
		}


		private void ValidateAdPlatform(ValidationException vex, Ad ad)
		{
			// check platform
			if (ad.Platform == null)
				vex.Add("Platform must be selected.");
		}

		private void ValidateAdType(ValidationException vex, Ad ad)
		{
			// check ad type
			if (ad.AdType == null)
				vex.Add("Ad type must be selected.");
			else if (!ad.Campaign.Creatives.Any(c => c.AdType.Id == ad.AdType.Id))
				vex.Add("Ad type does not have a creative in this campaign.");
		}

		private void ValidateAdCreative(ValidationException vex, Ad ad)
		{
			// check if creative exists belongs to the ad type selected
			if (ad.Creative == null)
				vex.Add("Creative must be selected.");
			else
			{
				if (ad.Creative.Campaign.Id != ad.Campaign.Id)
					vex.Add("Creative does not belong to campaign.");
				else if (ad.Creative.AdType.Id != ad.AdType.Id)
					vex.Add("Creative cannot be used for ad type.");

				if (ad.Features != null)
					ad.Features.Clear();
				else
					ad.Features = new List<Feature>();

				ad.Creative.Features.ForEach(cf => ad.Features.Add(cf));
			}
		}

		private void ValidateAdEndDate(Ad ad)
		{
			// set the end date to the last measurable time before the end date and hour
			if (ad.EndDate.HasValue)
			{
				var ed = ad.EndDate.Value.AddDays(1).AddMilliseconds(-1.5);
				ad.EndDate = ed;
			}
		}

		private void ValidateAdBeginDate(ValidationException vex, Ad ad)
		{
			// check begin date < end date
			if (ad.BeginDate > ad.EndDate)
				vex.Add("Begin date must be after end date.");

			if (ad.Placement != null)
			{
				if (!ad.AdType.AdTypeGroups.Contains(ad.Placement.AdTypeGroup))
					vex.Add("Placement is invalid.");
				else
					ad.AdTypeGroup = ad.Placement.AdTypeGroup;
			}
		}


		/// <summary>
		/// Delivery Group is required for ads that have ad type set to Image Banner or Overlay
		/// </summary>
		/// <param name="ad"></param>
		/// <param name="vex"></param>
		private void ValidateDeliveryGroup(Ad ad, ValidationException vex)
		{
			var imageBannerAdType = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.ImageBanner];
			var overlayAdType = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];
			if (ad.AdType.Id == imageBannerAdType || ad.AdType.Id == overlayAdType)
			{
				if (ad.DeliveryGroup == null)
					vex.Add("Delivery Group is required.");
			}
		}

		private static void ValidateCoordinates(ValidationException vex, Ad ad)
		{
			if (ad.Platform.Id == Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku])
			{
				if (ad.XCoordinateHd > AdConstants.Coordinates.Platforms.Roku.Hd.xMax)
					vex.Add("Ad HD X Coordinate must be below 1280.");
				if (ad.XCoordinateHd < AdConstants.Coordinates.Platforms.Roku.Hd.xMin)
					vex.Add("Ad HD X Coordinate must be above 0.");
				if (ad.YCoordinateHd > AdConstants.Coordinates.Platforms.Roku.Hd.yMax)
					vex.Add("Ad HD Y Coordinate must be below 1280.");
				if (ad.YCoordinateHd < AdConstants.Coordinates.Platforms.Roku.Hd.yMin)
					vex.Add("Ad HD Y Coordinate must be above 0.");
				if (ad.XCoordinateSd > AdConstants.Coordinates.Platforms.Roku.Sd.xMax)
					vex.Add("Ad HD X Coordinate must be below 1280.");
				if (ad.XCoordinateSd < AdConstants.Coordinates.Platforms.Roku.Sd.xMin)
					vex.Add("Ad HD X Coordinate must be above 0.");
				if (ad.YCoordinateSd > AdConstants.Coordinates.Platforms.Roku.Sd.yMax)
					vex.Add("Ad HD Y Coordinate must be below 1280.");
				if (ad.YCoordinateSd < AdConstants.Coordinates.Platforms.Roku.Sd.yMin)
					vex.Add("Ad HD Y Coordinate must be above 0.");

			}

			if (ad.Platform.Id != Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku])
			{
				if (ad.XCoordinateSd.HasValue || ad.YCoordinateSd.HasValue)
					vex.Add("Ad SD X and Y Coordinates are not allowed for an Ad that has a platform that is not Roku.");
				if (ad.XCoordinateHd > AdConstants.Coordinates.Platforms.NonRoku.Hd.xMax)
					vex.Add("Ad HD X Coordinate must be below 1920.");
				if (ad.XCoordinateHd < AdConstants.Coordinates.Platforms.NonRoku.Hd.xMin)
					vex.Add("Ad HD X Coordinate must be above 0.");
				if (ad.YCoordinateHd > AdConstants.Coordinates.Platforms.NonRoku.Hd.yMax)
					vex.Add("Ad HD Y Coordinate must be below 1080.");
				if (ad.YCoordinateHd < AdConstants.Coordinates.Platforms.NonRoku.Hd.yMin)
					vex.Add("Ad HD Y Coordinate must be above 0.");
			}
		}


		private void ValidateCompanionAd(Ad ad, ValidationException vex)
		{
			var ads = IoC.Resolve<IAdService>();

			//validate ad's AdType is Commerical Spot
			if (ad.AdType.Id != Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot])
			{
				vex.Add("Ad is not proper Ad Type for Companion Ad selection.");
			}
			else
			{
				//validate companion ad id links to an existing ad
				var companionAd = ads.Get(ad.CompanionAd.Id);
				if (companionAd == null)
					vex.Add("There is no Ad that exists for this ad's Companion Ad Id.");
			}
		}

		private void ValidateDestinationCreative(Ad ad, ValidationException vex)
		{
			var creatives = IoC.Resolve<ICreativeService>();
			var adFunctionClickToJump = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.ClickToJump];
			
			//only validate for ad's Destination Creative if ad's Ad Function is "Click To Jump"
			if (ad.Creative.AdFunction.Id != adFunctionClickToJump)
				return;

			if (ad.DestinationCreative == null)
				vex.Add("Destination Creative must be selected.");
			else
			{
				//validate Brand Destination Creative exists
				var destinationCreative = creatives.Get(ad.DestinationCreative.Value);
				if (destinationCreative == null)
					vex.Add("Destination Creative does not exist.");


				//validate that both the Brand Destination Creative and Ad belong to the same Campaign
				if (destinationCreative.Campaign.Id != ad.Campaign.Id)
					vex.Add("Destination Creative does not belong to campaign.");
			}
		}


		#endregion

	}
}
