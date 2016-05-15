using Brightline.Publishing.Areas.AdResponses.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Areas.AdResponses.ViewModels
{
	public class AdResponseViewModel
	{
		[JsonIgnore] 
		public string Key { get;set;}

		public MetaDataViewModel Metadata { get; set; }
		public AdResponseBodyViewModel AdResponseBody { get; set; }

		public static AdResponseViewModel Clone(AdResponseViewModel viewModel)
		{
			return new AdResponseViewModel
			{
				Key = viewModel.Key,
				AdResponseBody = viewModel.AdResponseBody,
				Metadata = viewModel.Metadata
			};
		}
	}
}
