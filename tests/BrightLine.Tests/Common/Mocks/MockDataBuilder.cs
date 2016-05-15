using BrightLine.Core;
using System;
using System.Collections.Generic;

namespace BrightLine.Tests.Common.Mocks
{
	public static class MockBuilder<T> where T : class, IEntity, new()
	{
		public static IRepository<T> GetRepository()
		{
			var repo = new EntityRepositoryInMemory<T>();
			return repo;
		}

		public static IRepository<T> GetPopulatedRepository(Func<List<T>> fetcher)
		{
			var repository = GetRepository();
			if (fetcher == null)
				return repository;

			var fetched = fetcher();
			foreach (var entity in fetched)
				repository.Insert(entity);
			return repository;
		}

		public static IRepository<T> PopulateRepository(IRepository<T> repository, Func<List<T>> fetcher)
		{
			if (repository == null)
				repository = GetRepository();

			if (fetcher == null)
				return repository;

			var fetched = fetcher();
			foreach (var entity in fetched)
				repository.Insert(entity);
			return repository;
		}
	}
}