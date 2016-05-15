using System.Collections.Generic;
using System.Linq;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Core;

namespace BrightLine.Service
{
	public class PageDefinitionService : CrudService<PageDefinition>, IPageDefinitionService
	{
		public PageDefinitionService(IRepository<PageDefinition> repo)
			: base(repo)
		{
		}

		public override PageDefinition Create(PageDefinition PageDefinition)
		{
			Upsert(PageDefinition);
			return PageDefinition;
		}

		public override List<PageDefinition> Create(IEnumerable<PageDefinition> PageDefinition)
		{
			Upsert(PageDefinition);
			return PageDefinition.ToList();
		}

		public override PageDefinition Update(PageDefinition PageDefinition)
		{
			Upsert(PageDefinition);
			return PageDefinition;
		}

		public override List<PageDefinition> Update(IEnumerable<PageDefinition> PageDefinitions)
		{
			Upsert(PageDefinitions);
			return PageDefinitions.ToList();
		}

		public override PageDefinition Upsert(PageDefinition PageDefinition)
		{
			if (PageDefinition.Id == 0)
				base.Create(PageDefinition);
			else
				base.Update(PageDefinition);

			return PageDefinition;
		}

		public override List<PageDefinition> Upsert(IEnumerable<PageDefinition> PageDefinitions)
		{
			foreach (var PageDefinition in PageDefinitions)
			{
				Upsert(PageDefinition);
			}

			return PageDefinitions.ToList();
		}

		public override PageDefinition Cud(PageDefinition PageDefinition, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (PageDefinition == null)
				return null;

			if (PageDefinition.IsDeleted)
				base.Delete(PageDefinition.Id, deleteType);
			else
				Upsert(PageDefinition);

			return PageDefinition;
		}

		public override List<PageDefinition> Cud(IEnumerable<PageDefinition> PageDefinitions, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (PageDefinitions == null)
				return new List<PageDefinition>();

			foreach (var PageDefinition in PageDefinitions)
			{
				if (PageDefinition.IsDeleted)
					base.Delete(PageDefinition.Id, deleteType);
				else
					Upsert(PageDefinition);
			}

			return PageDefinitions.Where(bp => !bp.IsDeleted).ToList();
		}

	
	}
}