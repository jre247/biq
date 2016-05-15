using System.Collections.Generic;
using System.Linq;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Core;

namespace BrightLine.Service
{
	public class FileTypeValidationService : CrudService<FileTypeValidation>, IFileTypeValidationService
	{
		public FileTypeValidationService(IRepository<FileTypeValidation> repo)
			: base(repo)
		{
		}

		public override FileTypeValidation Create(FileTypeValidation FileTypeValidation)
		{
			Upsert(FileTypeValidation);
			return FileTypeValidation;
		}

		public override List<FileTypeValidation> Create(IEnumerable<FileTypeValidation> FileTypeValidation)
		{
			Upsert(FileTypeValidation);
			return FileTypeValidation.ToList();
		}

		public override FileTypeValidation Update(FileTypeValidation FileTypeValidation)
		{
			Upsert(FileTypeValidation);
			return FileTypeValidation;
		}

		public override List<FileTypeValidation> Update(IEnumerable<FileTypeValidation> FileTypeValidations)
		{
			Upsert(FileTypeValidations);
			return FileTypeValidations.ToList();
		}

		public override FileTypeValidation Upsert(FileTypeValidation FileTypeValidation)
		{
			if (FileTypeValidation.Id == 0)
				base.Create(FileTypeValidation);
			else
				base.Update(FileTypeValidation);

			return FileTypeValidation;
		}

		public override List<FileTypeValidation> Upsert(IEnumerable<FileTypeValidation> FileTypeValidations)
		{
			foreach (var FileTypeValidation in FileTypeValidations)
			{
				Upsert(FileTypeValidation);
			}

			return FileTypeValidations.ToList();
		}

		public override FileTypeValidation Cud(FileTypeValidation FileTypeValidation, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (FileTypeValidation == null)
				return null;

			if (FileTypeValidation.IsDeleted)
				base.Delete(FileTypeValidation.Id, deleteType);
			else
				Upsert(FileTypeValidation);

			return FileTypeValidation;
		}

		public override List<FileTypeValidation> Cud(IEnumerable<FileTypeValidation> FileTypeValidations, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (FileTypeValidations == null)
				return new List<FileTypeValidation>();

			foreach (var FileTypeValidation in FileTypeValidations)
			{
				if (FileTypeValidation.IsDeleted)
					base.Delete(FileTypeValidation.Id, deleteType);
				else
					Upsert(FileTypeValidation);
			}

			return FileTypeValidations.Where(bp => !bp.IsDeleted).ToList();
		}

	
	}
}