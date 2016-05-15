using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdFunction;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.CmsRefType;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Expose;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.Utility.FileType;
using BrightLine.Common.Utility.Platform;
using BrightLine.Common.Utility.ResourceType;
using BrightLine.Common.Utility.StorageSource;
using BrightLine.Common.Utility.ValidationType;
using Common.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Tests.Common
{
	public class MockHelper
	{
		public Mock<ILogHelper> GetMockLogerHelper()
		{
			var mockLogHelper = new Mock<ILogHelper>();
			var mockLogger = new Mock<ILog>();
			mockLogHelper.Setup(c => c.GetLogger()).Returns(mockLogger.Object);
			return mockLogHelper;
		}

		public static void BuildMockLookups()
		{
			var fieldTypeTable = "FieldType";
			var fileTypeTable = "FileType";
			var platformTable = "Platform";
			var resourceTypeTable = "ResourceType";
			var resourceTypeImageTable = "ResourceTypeImage";
			var resourceTypeVideoTable = "ResourceTypeVideo";
			var validationTypeTable = "ValidationType";
			var adTypeTable = "AdType";
			var exposeTable = "Expose";
			var cmsRefTypeTable = "CmsRefType";
			var storageSourceTable = "StorageSource";
			var roleTable = "Role";
			var adFunctionTable = "AdFunction";
			var agencyTable = "Agency";
			var productTable = "Product";
			var adTypeGroupTable = "AdTypeGroup";
			var mediaAgencyTable = "MediaAgency";
			var brandTable = "Brand";
			var advertiserTable = "Advertiser";
			var trackingEventTable = "TrackingEvent";
			var placementTable = "Placement";

			var lookups = new List<LookupItem>();

			lookups.Add(new LookupItem { Id = 1, Name = AdvertiserConstants.AdvertiserNames.GlaxoSmithKlein, TableName = advertiserTable });
			lookups.Add(new LookupItem { Id = 2, Name = AdvertiserConstants.AdvertiserNames.AmericanExpress, TableName = advertiserTable });
			lookups.Add(new LookupItem { Id = 3, Name = AdvertiserConstants.AdvertiserNames.Unilever, TableName = advertiserTable });

			lookups.Add(new LookupItem { Id = 1, Name = BrandConstants.BrandNames.Abreva, TableName = brandTable });
			lookups.Add(new LookupItem { Id = 2, Name = BrandConstants.BrandNames.AmericanExpressCards, TableName = brandTable });
			lookups.Add(new LookupItem { Id = 8, Name = BrandConstants.BrandNames.Bertolli, TableName = brandTable });

			lookups.Add(new LookupItem { Id = 1, Name = FieldTypeConstants.FieldTypeNames.Image, TableName = fieldTypeTable });
			lookups.Add(new LookupItem { Id = 2, Name = FieldTypeConstants.FieldTypeNames.RefToModel, TableName = fieldTypeTable });
			lookups.Add(new LookupItem { Id = 3, Name = FieldTypeConstants.FieldTypeNames.String, TableName = fieldTypeTable });
			lookups.Add(new LookupItem { Id = 4, Name = FieldTypeConstants.FieldTypeNames.Integer, TableName = fieldTypeTable });
			lookups.Add(new LookupItem { Id = 5, Name = FieldTypeConstants.FieldTypeNames.Float, TableName = fieldTypeTable });
			lookups.Add(new LookupItem { Id = 6, Name = FieldTypeConstants.FieldTypeNames.Bool, TableName = fieldTypeTable });
			lookups.Add(new LookupItem { Id = 7, Name = FieldTypeConstants.FieldTypeNames.Datetime, TableName = fieldTypeTable });
			lookups.Add(new LookupItem { Id = 8, Name = FieldTypeConstants.FieldTypeNames.Video, TableName = fieldTypeTable });
			lookups.Add(new LookupItem { Id = 9, Name = FieldTypeConstants.FieldTypeNames.RefToPage, TableName = fieldTypeTable });
			lookups.Add(new LookupItem { Id = 10, Name = FieldTypeConstants.FieldTypeNames.Html, TableName = fieldTypeTable });

			lookups.Add(new LookupItem { Id = 1, Name = FileTypeConstants.FileTypeNames.Jpeg, TableName = fileTypeTable });
			lookups.Add(new LookupItem { Id = 2, Name = FileTypeConstants.FileTypeNames.Png, TableName = fileTypeTable });
			lookups.Add(new LookupItem { Id = 3, Name = FileTypeConstants.FileTypeNames.Mp4, TableName = fileTypeTable });
			lookups.Add(new LookupItem { Id = 4, Name = FileTypeConstants.FileTypeNames.Jpg, TableName = fileTypeTable });
			lookups.Add(new LookupItem { Id = 5, Name = FileTypeConstants.FileTypeNames.Gif, TableName = fileTypeTable });

			lookups.Add(new LookupItem { Id = 1, Name = PlatformConstants.PlatformNames.DirecTV, TableName = platformTable });
			lookups.Add(new LookupItem { Id = 25, Name = PlatformConstants.PlatformNames.Roku, TableName = platformTable });
			lookups.Add(new LookupItem { Id = 30, Name = PlatformConstants.PlatformNames.TVOS, TableName = platformTable });
			lookups.Add(new LookupItem { Id = 16, Name = PlatformConstants.PlatformNames.Samsung, TableName = platformTable });
			lookups.Add(new LookupItem { Id = 28, Name = PlatformConstants.PlatformNames.FireTV, TableName = platformTable });

			lookups.Add(new LookupItem { Id = 1, Name = ResourceTypeConstants.ResourceTypeNames.SdImage, TableName = resourceTypeTable });
			lookups.Add(new LookupItem { Id = 2, Name = ResourceTypeConstants.ResourceTypeNames.HdImage, TableName = resourceTypeTable });
			lookups.Add(new LookupItem { Id = 3, Name = ResourceTypeConstants.ResourceTypeNames.SdVideo, TableName = resourceTypeTable });
			lookups.Add(new LookupItem { Id = 4, Name = ResourceTypeConstants.ResourceTypeNames.HdVideo, TableName = resourceTypeTable });

			lookups.Add(new LookupItem { Id = 1, Name = ResourceTypeConstants.ResourceTypeNames.SdImage, TableName = resourceTypeImageTable });
			lookups.Add(new LookupItem { Id = 2, Name = ResourceTypeConstants.ResourceTypeNames.HdImage, TableName = resourceTypeImageTable });

			lookups.Add(new LookupItem { Id = 3, Name = ResourceTypeConstants.ResourceTypeNames.SdVideo, TableName = resourceTypeVideoTable });
			lookups.Add(new LookupItem { Id = 4, Name = ResourceTypeConstants.ResourceTypeNames.HdVideo, TableName = resourceTypeVideoTable });

			lookups.Add(new LookupItem { Id = 1, Name = ValidationTypeConstants.ValidationTypeNames.Required, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 2, Name = ValidationTypeConstants.ValidationTypeNames.Unique, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 3, Name = ValidationTypeConstants.ValidationTypeNames.Width, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 4, Name = ValidationTypeConstants.ValidationTypeNames.MaxWidth, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 5, Name = ValidationTypeConstants.ValidationTypeNames.MinWidth, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 6, Name = ValidationTypeConstants.ValidationTypeNames.Height, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 7, Name = ValidationTypeConstants.ValidationTypeNames.MaxHeight, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 8, Name = ValidationTypeConstants.ValidationTypeNames.MinHeight, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 9, Name = ValidationTypeConstants.ValidationTypeNames.Length, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 10, Name = ValidationTypeConstants.ValidationTypeNames.MaxLength, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 11, Name = ValidationTypeConstants.ValidationTypeNames.MinLength, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 12, Name = ValidationTypeConstants.ValidationTypeNames.MinDatetime, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 13, Name = ValidationTypeConstants.ValidationTypeNames.MaxDatetime, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 14, Name = ValidationTypeConstants.ValidationTypeNames.MaxImageSize, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 15, Name = ValidationTypeConstants.ValidationTypeNames.MaxVideoSize, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 16, Name = ValidationTypeConstants.ValidationTypeNames.MaxVideoDuration, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 17, Name = ValidationTypeConstants.ValidationTypeNames.Extension, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 18, Name = ValidationTypeConstants.ValidationTypeNames.MinFloat, TableName = validationTypeTable });
			lookups.Add(new LookupItem { Id = 19, Name = ValidationTypeConstants.ValidationTypeNames.MaxFloat, TableName = validationTypeTable });

			lookups.Add(new LookupItem { Id = 10014, Name = AdTypeConstants.AdTypeNames.CommercialSpot, TableName = adTypeTable });
			lookups.Add(new LookupItem { Id = 10010, Name = AdTypeConstants.AdTypeNames.BrandDestination, TableName = adTypeTable });
			lookups.Add(new LookupItem { Id = 10001, Name = AdTypeConstants.AdTypeNames.ImageBanner, TableName = adTypeTable });
			lookups.Add(new LookupItem { Id = 10017, Name = AdTypeConstants.AdTypeNames.Overlay, TableName = adTypeTable });
			lookups.Add(new LookupItem { Id = 10003, Name = AdTypeConstants.AdTypeNames.VideoBanner, TableName = adTypeTable });
			lookups.Add(new LookupItem { Id = 10015, Name = AdTypeConstants.AdTypeNames.DedicatedBrandApp, TableName = adTypeTable });

			lookups.Add(new LookupItem { Id = 1, Name = ExposeConstants.ExposeNames.Server, TableName = exposeTable });
			lookups.Add(new LookupItem { Id = 2, Name = ExposeConstants.ExposeNames.Client, TableName = exposeTable });
			lookups.Add(new LookupItem { Id = 3, Name = ExposeConstants.ExposeNames.Both, TableName = exposeTable });


			lookups.Add(new LookupItem { Id = 1, Name = CmsRefTypeConstants.CmsRefTypeNames.Known, TableName = cmsRefTypeTable });
			lookups.Add(new LookupItem { Id = 2, Name = CmsRefTypeConstants.CmsRefTypeNames.Unknown, TableName = cmsRefTypeTable });

			lookups.Add(new LookupItem { Id = 1, Name = StorageSourceConstants.StorageSourceNames.Media, TableName = storageSourceTable });
			lookups.Add(new LookupItem { Id = 2, Name = StorageSourceConstants.StorageSourceNames.External, TableName = storageSourceTable });

			lookups.Add(new LookupItem { Id = 2, Name = AuthConstants.Roles.Admin, TableName = roleTable });
			lookups.Add(new LookupItem { Id = 3, Name = AuthConstants.Roles.Employee, TableName = roleTable });
			lookups.Add(new LookupItem { Id = 9, Name = AuthConstants.Roles.Client, TableName = roleTable });
			lookups.Add(new LookupItem { Id = 10, Name = AuthConstants.Roles.AgencyPartner, TableName = roleTable });
			lookups.Add(new LookupItem { Id = 1, Name = AuthConstants.Roles.Developer, TableName = roleTable });
			lookups.Add(new LookupItem { Id = 22, Name = AuthConstants.Roles.MediaPartner, TableName = roleTable });

			lookups.Add(new LookupItem { Id = 4, Name = AdFunctionConstants.AdFunctionNames.ClickToJump, TableName = adFunctionTable });
			lookups.Add(new LookupItem { Id = 6, Name = AdFunctionConstants.AdFunctionNames.OnDemand, TableName = adFunctionTable });

			lookups.Add(new LookupItem { Id = 1, Name = AgencyConstants.AgencyNames.BBDO, TableName = agencyTable });
			lookups.Add(new LookupItem { Id = 2, Name = AgencyConstants.AgencyNames.CaratNY, TableName = agencyTable });

			lookups.Add(new LookupItem { Id = 12, Name = ProductConstants.ProductNames.AmexDeo, TableName = productTable });
			lookups.Add(new LookupItem { Id = 3, Name = ProductConstants.ProductNames.AxeMasterbrand, TableName = productTable });
			lookups.Add(new LookupItem { Id = 22, Name = ProductConstants.ProductNames.Beano, TableName = productTable });

			lookups.Add(new LookupItem { Id = 10004, Name = AdTypeGroupConstants.AdTypeGroupNames.Destination, TableName = adTypeGroupTable });

			lookups.Add(new LookupItem { Id = 10004, Name = MediaAgencyConstants.MediaAgencyNames.Abc, TableName = mediaAgencyTable });

			lookups.Add(new LookupItem { Id = 1, Name = TrackingEventConstants.TrackingEventNames.CreativeView, TableName = trackingEventTable });
			lookups.Add(new LookupItem { Id = 2, Name = TrackingEventConstants.TrackingEventNames.Start, TableName = trackingEventTable });
			lookups.Add(new LookupItem { Id = 3, Name = TrackingEventConstants.TrackingEventNames.FirstQuartile, TableName = trackingEventTable });

			lookups.Add(new LookupItem { Id = 28, Name = PlacementConstants.Names.SamsungAdHubFirstTile, TableName = placementTable });
			lookups.Add(new LookupItem { Id = 1, Name = PlacementConstants.Names.CnbcHorizontalBanner, TableName = placementTable });

			Lookups.LookupsDictionary = new LookupsDictionary(lookups);
		}

	}
}
