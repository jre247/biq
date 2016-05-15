using BrightLine.Common.Framework;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Tests.Common
{
	public static class MockUtilityExtensions
	{
		public static EntityRepositoryInMemory<TEntity> ConvertToInMemoryRepository<TEntity, TRepo>(this TRepo repo) where TRepo : ICrudService<TEntity> where TEntity : EntityBase, new()
		{
			var genericRepo = new EntityRepositoryInMemory<TEntity>();
			var entities = repo.GetAll();
			foreach (var entity in entities)
			{
				genericRepo.Insert(entity);
			}

			return genericRepo;
		}
	}
}
