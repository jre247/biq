using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.ViewModels.Resources;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BrightLine.Service
{
	public class StorageSourceService : CrudService<StorageSource>, IStorageSourceService
	{
		public StorageSourceService(IRepository<StorageSource> repo)
			: base(repo)
		{ }
	}
}
