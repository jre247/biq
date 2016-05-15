using System.Collections.Generic;
using System.Linq;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Core;

namespace BrightLine.Service
{
	public class CmsModelDefinitionService : CrudService<CmsModelDefinition>, ICmsModelDefinitionService
	{
		public CmsModelDefinitionService(IRepository<CmsModelDefinition> repo)
			: base(repo)
		{
		}

		public override CmsModelDefinition Create(CmsModelDefinition cmsModelDefinition)
		{
			Upsert(cmsModelDefinition);
			return cmsModelDefinition;
		}

		public override List<CmsModelDefinition> Create(IEnumerable<CmsModelDefinition> cmsModelDefinition)
		{
			Upsert(cmsModelDefinition);
			return cmsModelDefinition.ToList();
		}

		public override CmsModelDefinition Update(CmsModelDefinition cmsModelDefinition)
		{
			Upsert(cmsModelDefinition);
			return cmsModelDefinition;
		}

		public override List<CmsModelDefinition> Update(IEnumerable<CmsModelDefinition> cmsModelDefinitions)
		{
			Upsert(cmsModelDefinitions);
			return cmsModelDefinitions.ToList();
		}

		public override CmsModelDefinition Upsert(CmsModelDefinition cmsModelDefinition)
		{
			if (cmsModelDefinition.Id == 0)
				base.Create(cmsModelDefinition);
			else
				base.Update(cmsModelDefinition);

			return cmsModelDefinition;
		}

		public override List<CmsModelDefinition> Upsert(IEnumerable<CmsModelDefinition> cmsModelDefinitions)
		{
			foreach (var cmsModelDefinition in cmsModelDefinitions)
			{
				Upsert(cmsModelDefinition);
			}

			return cmsModelDefinitions.ToList();
		}

		public override CmsModelDefinition Cud(CmsModelDefinition cmsModelDefinition, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (cmsModelDefinition == null)
				return null;

			if (cmsModelDefinition.IsDeleted)
				base.Delete(cmsModelDefinition.Id, deleteType);
			else
				Upsert(cmsModelDefinition);

			return cmsModelDefinition;
		}

		public override List<CmsModelDefinition> Cud(IEnumerable<CmsModelDefinition> cmsModelDefinitions, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (cmsModelDefinitions == null)
				return new List<CmsModelDefinition>();

			foreach (var cmsModelDefinition in cmsModelDefinitions)
			{
				if (cmsModelDefinition.IsDeleted)
					base.Delete(cmsModelDefinition.Id, deleteType);
				else
					Upsert(cmsModelDefinition);
			}

			return cmsModelDefinitions.Where(bp => !bp.IsDeleted).ToList();
		}

	
	}
}