using Brightline.Publishing.Areas.AdResponses.ViewModels.Html5;
using Brightline.Publishing.Areas.AdResponses.ViewModels.Html5.CompanionAd;
using Brightline.Publishing.Areas.AdResponses.ViewModels.Roku.CompanionAd;
using BrightLine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Brightline.Publishing.Areas.AdResponses.ViewModels.VAST
{
	// *Note: need to declare an XmlInclude for each concrete CompanionAd ViewModel that inherits from this Base CompanionAd ViewModel.
	//		-The reason for needing to declare these XmlIncludes here is so that Xml Serialization works properly for derived classes
	//		-It's also necessary to declare an XmlRoot here or else Xml Serialization doesn't work properly
	//	TODO: think of a way to not have to declare an XmlInclude for each concrete CompanionAd ViewModel here, since now this base class has a dependency on each of the concrete classes that inherit from it
	[XmlRoot(Namespace = "CompanionAd")]
	[XmlInclude(typeof(RokuRAFCompanionAdViewModel))]
	[XmlInclude(typeof(Html5CompanionAdViewModel))]
	[XmlInclude(typeof(RokuDICompanionAdViewModel))]
	public abstract class BaseCompanionAdViewModel
	{
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		[XmlAttribute(AttributeName = "width")]
		public int Width { get; set; }

		[XmlAttribute(AttributeName = "height")]
		public int Height { get; set; }

		[XmlAttribute(AttributeName = "apiFramework")]
		public string ApiFramework { get; set; }

		[XmlElement("IFrameResource")]
		public string IFrameResource { get; set; }

		[XmlElement("StaticResource")]
		public CompanionAdStaticResource StaticResource { get; set; }

		public abstract BaseCompanionAdViewModel Parse(Ad ad);
	}
}
