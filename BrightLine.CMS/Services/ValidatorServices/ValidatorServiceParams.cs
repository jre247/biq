using BrightLine.Common.Models;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services
{
	public class ValidatorServiceParams
	{
		public Validation Validation {get;set;}
		public string InstanceFieldValue {get;set;}
		public FieldSaveViewModel InstanceField {get;set;}
		public ModelInstanceSaveViewModel InstanceViewModel {get;set;}
		public CmsField CmsField { get; set; }
		public int InstanceId { get; set; }
		public InstanceTypeEnum InstanceType { get;set;}
	}
}
