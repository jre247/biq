using BrightLine.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BrightLine.Common.Services
{
	public interface ISftpService
	{
		void UploadResource(Stream fileStream, string filename, int campaignId, string resourceType);

		void UploadMetaResource(Stream fileStream, string p, int campaignId, string resourceType);
	}
}
