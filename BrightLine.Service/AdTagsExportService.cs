using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdTagExport;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.Platform;
using BrightLine.Common.Utility.Spreadsheets;
using BrightLine.Common.Utility.Spreadsheets.Writer;
using BrightLine.Common.ViewModels.AdTrackingUrl.IQ;
using BrightLine.Common.ViewModels.Cms;
using BrightLine.Service.AdTrackingUrl;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service
{
	public class AdTagsExportService : IAdTagsExportService
	{
		private ISpreadSheetWriter Writer;
		private List<AdTagExportViewModel> Ads = new List<AdTagExportViewModel>();

		public void ExportAdTags(Stream stream, Campaign campaign)
		{
			if(campaign == null)
				throw new ArgumentNullException("Campaign is null.");

			var campaigns = IoC.Resolve<ICampaignService>();

			var isAccessible = campaigns.IsAccessible(campaign);
			if(!isAccessible)
				return;

			Writer = SpreadsheetHelper.GetWriter();

			var campaignAds = GetCampaignAds(campaign);
			Ads = SetAdTagAndUrls(campaignAds, campaign);

			Writer.CreateWorkBook();

			PopulateCampaignSummary();

			Writer.Save(stream);
		}

		public List<AdTagExportViewModel> GetCampaignAds(Campaign campaign)
		{
			var ads = IoC.Resolve<IAdService>();

			var adTypesToExclude = new List<int> { AdTypeConstants.AdTypeIds.BrandDestination, AdTypeConstants.AdTypeIds.DedicatedBrandApp, AdTypeConstants.AdTypeIds.OnScreenSurvey };

			var campaignAds = ads.Where(a => !adTypesToExclude.Contains(a.AdType.Id) && a.Campaign.Id == campaign.Id)
				.Select(a => new AdTagExportViewModel
				{
					AdId = a.Id,
					AdTagId = a.AdTag != null ? (int?)a.AdTag.Id : null,
					PlatformName = a.Platform != null ? a.Platform.Name : null,
					PlatformId = a.Platform != null ? (int?)a.Platform.Id : null,
					MediaPartnerName = a.Placement.MediaPartner != null ? a.Placement.MediaPartner.Name : null,
					AdName = a.Name,
					CreativeName = a.Creative != null ? a.Creative.Name : null,
					AdTypeName = a.AdType != null ? a.AdType.Name : null,
					AdFormatName = a.AdFormat != null ? a.AdFormat.Name : null,
					AdFunctionName = a.AdFunction != null ? a.AdFunction.Name : null,
					BeginDate = a.BeginDate,
					EndDate = a.EndDate,
					AdTypeId = a.AdType != null ? (int?)a.AdType.Id : null,
					DeliveryGroupName = a.DeliveryGroup != null ? a.DeliveryGroup.Name : null,
					PlacementName = a.Placement.Name,
					PlacementId = a.Placement.Id
				})
				.OrderBy(o => o.AdTypeName)
				.ThenBy(o => o.PlacementName)
				.ThenBy(o => o.MediaPartnerName)
				.ToList();

			return campaignAds;
		}

		public List<AdTagExportViewModel> SetAdTagAndUrls(List<AdTagExportViewModel> ads, Campaign campaign)
		{
			var overlay = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay];
			var roku = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			var adTrackingUrlGenerator = new IQAdTrackingUrlGenerator();
			var adsWithUrls = new List<AdTagExportViewModel>();
			var commercialSpotAdType = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot];
			var settings = IoC.Resolve<ISettingsService>();

			foreach (var ad in ads)
			{
				var adTypeId = ad.AdTypeId;
				var adWithUrls = ad;
				var adTagId = ad.AdTagId.ToString();
				var platformId = ad.PlatformId.Value;

				// Generate Ad Tag Url
				var adTagUrlGenerator = new AdTagUrlGenerator();
				var adTagUrl = adTagUrlGenerator.Generate(adTagId, platformId);
				// Tack on mblist query param to Ad Tag for Roku Overlay
				if (adTypeId == overlay && platformId == roku)
					adTagUrl = string.Format("{0}&{1}={2}", adTagUrl, AdTagUrlConstants.QueryParams.MBList, settings.MBList);

				adWithUrls.AdTag = adTagUrl;

				// Generate Impression Url
				var impressionUrlViewModel = new IQAdTrackingUrlViewModel(AdTagUrlConstants.IQ.Types.Impression, true, ad.AdId, ad.PlacementId);
				adWithUrls.ImpressionUrl = adTrackingUrlGenerator.Generate(impressionUrlViewModel);

				if (ad.AdTypeId != commercialSpotAdType)
				{
					// Generate Click Url
					var adClickUrlViewModel = new IQAdTrackingUrlViewModel(AdTagUrlConstants.IQ.Types.AdClick, true, ad.AdId, ad.PlacementId);
					adWithUrls.ClickUrl = adTrackingUrlGenerator.Generate(adClickUrlViewModel);
				}

				adsWithUrls.Add(adWithUrls);
			}

			return adsWithUrls;
		}

		private void PopulateCampaignSummary()
		{
			const string sheetName = AdTagExportConstants.Spreadsheet.Name;
			Writer.CreateSheet(sheetName, true);

			// 1. add Platform Summary table
			var campaignSummaryTable = GetCampaignSummaryTable();
			Writer.WriteDataTable(sheetName, campaignSummaryTable, true, "A2"); //TODO: Remove hard-coded value
			Writer.FormatColumn(sheetName, 1);
			Writer.FormatColumn(sheetName, 2);
			Writer.FormatColumn(sheetName, 3);
			Writer.FormatColumn(sheetName, 4);
			Writer.FormatColumn(sheetName, 5);
			Writer.FormatColumn(sheetName, 6);
			Writer.FormatColumn(sheetName, 7);
			Writer.FormatColumn(sheetName, 8);
			Writer.FormatColumn(sheetName, 9);
			Writer.FormatColumn(sheetName, 10);
			Writer.FormatColumn(sheetName, 11);
			Writer.FormatColumn(sheetName, 12);
			Writer.FormatColumn(sheetName, 13);
			Writer.FormatColumn(sheetName, 14);

			

		}

		private DataTable GetCampaignSummaryTable()
		{
			var dataTable = new DataTable(AdTagExportConstants.Spreadsheet.DataTableName);

			//Adding columns to the DataTable object
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.AdId, typeof(int));
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.TagId, typeof(int));
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.Platform, typeof(string));
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.MediaPartner, typeof(string));
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.Name, typeof(string));
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.Creative, typeof(string));
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.AdType, typeof(string));
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.AdFormat, typeof(string));
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.AdFunction, typeof(string));
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.BeginDate, typeof(string));
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.EndDate, typeof(string));
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.DeliveryGroup, typeof(string));
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.Placement, typeof(string));
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.Tag, typeof(string));
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.Impression, typeof(string));
			dataTable.Columns.Add(AdTagExportConstants.Spreadsheet.Columns.Click, typeof(string));

			foreach (var ad in Ads)
			{
				var dr = dataTable.NewRow();
				dr[0] = ad.AdId;
				dr[1] = GetAdPropertyValue(ad, ad.AdTagId);
				dr[2] = GetAdPropertyValue(ad, ad.PlatformName);
				dr[3] = GetAdPropertyValue(ad, ad.MediaPartnerName);
				dr[4] = GetAdPropertyValue(ad, ad.AdName);
				dr[5] = GetAdPropertyValue(ad, ad.CreativeName);
				dr[6] = GetAdPropertyValue(ad, ad.AdTypeName);
				dr[7] = GetAdPropertyValue(ad, ad.AdFormatName);
				dr[8] = GetAdPropertyValue(ad, ad.AdFunctionName);
				dr[9] = GetAdPropertyValue(ad, ad.BeginDate);
				dr[10] = GetAdPropertyValue(ad, ad.EndDate);
				dr[11] = GetAdPropertyValue(ad, ad.DeliveryGroupName);
				dr[12] = GetAdPropertyValue(ad, ad.PlacementName);
				dr[13] = GetAdPropertyValue(ad, ad.AdTag);
				dr[14] = GetAdPropertyValue(ad, ad.ImpressionUrl);
				dr[15] = GetAdPropertyValue(ad, ad.ClickUrl);
				dataTable.Rows.Add(dr);
			}

			return dataTable;
		}

		private object GetAdPropertyValue<T>(AdTagExportViewModel ad, T adProperty)
		{
			if (adProperty == null)
				return DBNull.Value; //cannot write null to DataTable row value, but must use DBNull.Value instead

			return adProperty;
		}
	}
}
