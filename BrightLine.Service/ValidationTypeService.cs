using System.Collections.Generic;
using System.Linq;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Core;

namespace BrightLine.Service
{
	public class ValidationTypeService : CrudService<ValidationType>, IValidationTypeService
	{
		public ValidationTypeService(IRepository<ValidationType> repo)
			: base(repo)
		{
		}

		public override ValidationType Create(ValidationType validationType)
		{
			Upsert(validationType);
			return validationType;
		}

		public override List<ValidationType> Create(IEnumerable<ValidationType> validationType)
		{
			Upsert(validationType);
			return validationType.ToList();
		}

		public override ValidationType Update(ValidationType validationType)
		{
			Upsert(validationType);
			return validationType;
		}

		public override List<ValidationType> Update(IEnumerable<ValidationType> validationTypes)
		{
			Upsert(validationTypes);
			return validationTypes.ToList();
		}

		public override ValidationType Upsert(ValidationType validationType)
		{
			if (validationType.Id == 0)
				base.Create(validationType);
			else
				base.Update(validationType);

			return validationType;
		}

		public override List<ValidationType> Upsert(IEnumerable<ValidationType> validationTypes)
		{
			foreach (var validationType in validationTypes)
			{
				Upsert(validationType);
			}

			return validationTypes.ToList();
		}

		public override ValidationType Cud(ValidationType validationType, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (validationType == null)
				return null;

			if (validationType.IsDeleted)
				base.Delete(validationType.Id, deleteType);
			else
				Upsert(validationType);

			return validationType;
		}

		public override List<ValidationType> Cud(IEnumerable<ValidationType> validationTypes, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (validationTypes == null)
				return new List<ValidationType>();

			foreach (var validationType in validationTypes)
			{
				if (validationType.IsDeleted)
					base.Delete(validationType.Id, deleteType);
				else
					Upsert(validationType);
			}

			return validationTypes.Where(bp => !bp.IsDeleted).ToList();
		}

	
	}
}