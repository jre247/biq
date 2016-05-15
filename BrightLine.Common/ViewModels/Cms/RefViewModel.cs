using BrightLine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels
{
	public class RefViewModel
	{
		public string type { get; set; }
		public string model { get; set; }

		public RefViewModel(){
		
		}

		public RefViewModel(CmsRef cmsRef)
		{
			if (cmsRef.CmsModelDefinition != null)
				model = cmsRef.CmsModelDefinition.Name;

			if (cmsRef.CmsRefType != null)
				type = cmsRef.CmsRefType.Name;				
		}
	}

}
