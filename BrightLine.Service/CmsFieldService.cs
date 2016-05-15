using System.Collections.Generic;
using System.Linq;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Core;

namespace BrightLine.Service
{
	public class CmsFieldService : CrudService<CmsField>, ICmsFieldService
	{
		public CmsFieldService(IRepository<CmsField> repo)
			: base(repo)
		{
		}

		public override CmsField Create(CmsField CmsField)
		{
			Upsert(CmsField);
			return CmsField;
		}

		public override List<CmsField> Create(IEnumerable<CmsField> CmsField)
		{
			Upsert(CmsField);
			return CmsField.ToList();
		}

		public override CmsField Update(CmsField CmsField)
		{
			Upsert(CmsField);
			return CmsField;
		}

		public override List<CmsField> Update(IEnumerable<CmsField> CmsFields)
		{
			Upsert(CmsFields);
			return CmsFields.ToList();
		}

		public override CmsField Upsert(CmsField CmsField)
		{
			if (CmsField.Id == 0)
				base.Create(CmsField);
			else
				base.Update(CmsField);

			return CmsField;
		}

		public override List<CmsField> Upsert(IEnumerable<CmsField> CmsFields)
		{
			foreach (var CmsField in CmsFields)
			{
				Upsert(CmsField);
			}

			return CmsFields.ToList();
		}

		public override CmsField Cud(CmsField CmsField, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (CmsField == null)
				return null;

			if (CmsField.IsDeleted)
				base.Delete(CmsField.Id, deleteType);
			else
				Upsert(CmsField);

			return CmsField;
		}

		public override List<CmsField> Cud(IEnumerable<CmsField> CmsFields, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (CmsFields == null)
				return new List<CmsField>();

			foreach (var CmsField in CmsFields)
			{
				if (CmsField.IsDeleted)
					base.Delete(CmsField.Id, deleteType);
				else
					Upsert(CmsField);
			}

			return CmsFields.Where(bp => !bp.IsDeleted).ToList();
		}

	
	}
}