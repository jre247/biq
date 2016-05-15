using BrightLine.Common.Models.Lookups;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface IFieldTypeService : ICrudService<FieldType>
	{
		int GetFieldTypeId(string fieldTypeName);
	}
}
