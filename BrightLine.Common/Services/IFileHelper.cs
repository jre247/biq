using BrightLine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BrightLine.Common.Services
{
	public interface IFileHelper
	{
		 Resource CreateFile(HttpFileCollectionBase files);
		 Resource CreateFile(HttpPostedFileBase file);
		 string GetCloudFileDownloadUrl(Resource resource);
		 string[] ImageContentTypes {get;}
		 bool IsFilePresent(HttpPostedFileBase file);
		 bool IsFilePresent(HttpPostedFile file);
	}
}
