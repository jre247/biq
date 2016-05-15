using System.Collections.Generic;
using System.Linq;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Core;

namespace BrightLine.Service
{
	public class ValidationService : CrudService<Validation>, IValidationService
	{
		public ValidationService(IRepository<Validation> repo)
			: base(repo)
		{
		}

		public override Validation Create(Validation validation)
		{
			Upsert(validation);
			return validation;
		}

		public override List<Validation> Create(IEnumerable<Validation> validation)
		{
			Upsert(validation);
			return validation.ToList();
		}

		public override Validation Update(Validation validation)
		{
			Upsert(validation);
			return validation;
		}

		public override List<Validation> Update(IEnumerable<Validation> validations)
		{
			Upsert(validations);
			return validations.ToList();
		}

		public override Validation Upsert(Validation validation)
		{
			if (validation.Id == 0)
				base.Create(validation);
			else
				base.Update(validation);

			return validation;
		}

		public override List<Validation> Upsert(IEnumerable<Validation> validations)
		{
			foreach (var validation in validations)
			{
				Upsert(validation);
			}

			return validations.ToList();
		}

		
	
	}
}